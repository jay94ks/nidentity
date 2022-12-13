using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Newtonsoft.Json;
using NIdentity.Core;
using NIdentity.Core.Server;
using NIdentity.Core.X509;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Server;
using NIdentity.Core.X509.Server.Commands;
using NIdentity.Core.X509.Server.Commands.Certificates;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NIdentity
{
    public class App
    {

        public static void Main(string[] Args) => BuildWebApp(CreateAppBuilder(Args)).Run();

        /// <summary>
        /// Create an <see cref="WebApplicationBuilder"/> instance.
        /// </summary>
        /// <param name="Args"></param>
        /// <returns></returns>
        internal static WebApplicationBuilder CreateAppBuilder(string[] Args)
        {
            var Host = WebApplication.CreateBuilder(Args);
            var Urls = new List<string>();

            Urls.Add("http://0.0.0.0:7000");
            if (Debugger.IsAttached || File.Exists(Path.Combine(AppEnv.APP_ROOT, AppEnv.SSL_CERT_FILE)))
                Urls.Add("https://0.0.0.0:7001/");

            Certificate Cert = null;
            if (File.Exists(Path.Combine(AppEnv.APP_ROOT, AppEnv.SSL_CERT_FILE)))
            {
                var Data = File.ReadAllBytes(
                    Path.Combine(AppEnv.APP_ROOT, AppEnv.SSL_CERT_FILE));

                Cert = CertificateStore.Import(Data)
                    .Certificates.FirstOrDefault(X => X.HasPrivateKey);
            }

            Host.WebHost.UseUrls(Urls.ToArray());
            Host.WebHost.ConfigureKestrel(Kestrel =>
            {
                Kestrel.ConfigureHttpsDefaults(Https =>
                {
                    if (Cert != null)
                    {
                        Https.ServerCertificate = Cert.ToDotNetCert();
                    }

                    Https.CheckCertificateRevocation = false;
                    Https.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                    Https.ClientCertificateValidation = (_, _2, _3) => true;

                    Https.OnAuthenticate = (A, B) =>
                    {
                        B.AllowRenegotiation = true;
                        B.CertificateRevocationCheckMode = X509RevocationMode.NoCheck;
                    };

                });
            });

            Host.Services.AddCors();
            Host.Services.AddRazorPages()
                .AddApplicationPart(typeof(App).Assembly)
                .AddNewtonsoftJson()
                ;

            Host.Services
                .AddScoped(X => X.GetRequiredService<IDbContextFactory<AppContext>>().CreateDbContext())
                .AddPooledDbContextFactory<AppContext>(X =>
                {
                    var Conn = new MySqlConnectionStringBuilder();

                    Conn.Server = AppEnv.MYSQL_HOST;
                    Conn.Port = (uint)AppEnv.MYSQL_PORT;
                    Conn.UserID = AppEnv.MYSQL_USER;
                    Conn.Password = AppEnv.MYSQL_PASS;
                    Conn.Database = AppEnv.MYSQL_DB;

                    X.UseMySql(WaitForMySQL(Conn.ToString()), ServerVersion.AutoDetect(Conn.ToString()),
                        X => X.MigrationsAssembly(typeof(App).Assembly.GetName().Name));
                });

            Host.Services
                .AddCommandExecutor()
                .MapX509ServerCommands()
                ;


            var X509Settings = Host.Services.AddX509<AppContext>();
            ConfigureX509Environment(X509Settings);

            return Host;
        }

        /// <summary>
        /// Configures X509 Environment.
        /// </summary>
        /// <param name="X509Settings"></param>
        private static void ConfigureX509Environment(X509ServerSettings X509Settings)
        {
            X509Settings.CachePath = Path.Combine(AppEnv.APP_ROOT, "data/caches");
            if (Directory.Exists(X509Settings.CachePath) == false)
                Directory.CreateDirectory(X509Settings.CachePath);

            X509Settings.CrlTerm = TimeSpan.FromDays(Math.Max(AppEnv.CRL_TERM, 3));
            X509Settings.CerTerm = TimeSpan.FromDays(Math.Max(AppEnv.CER_TERM, 3));
            X509Settings.MaxCachedKeys = Math.Max(AppEnv.MAX_CACHE_KEYS, 64);
            X509Settings.IsSuperMode = AppEnv.RUN_AS_SUPER == 1;

            X509Settings.ExecutorSettings.HttpBaseUri = new Uri(AppEnv.HTTP_BASE_URL);
            X509Settings.ExecutorSettings.HttpOcsp = X509Settings.HttpOcsp;
            X509Settings.ExecutorSettings.HttpCRL = X509Settings.HttpCRL;
            X509Settings.ExecutorSettings.HttpCAIssuers = X509Settings.HttpCAIssuers;
            X509Settings.ExecutorSettings.MaxCountPerListRequest
                = Math.Max(X509Settings.MaxCachedKeys / 4, 32);

            if (File.Exists(Path.Combine(AppEnv.APP_ROOT, AppEnv.GENESIS_CMDS)))
            {
                var Json = File.ReadAllText(Path.Combine(AppEnv.APP_ROOT, AppEnv.GENESIS_CMDS), Encoding.UTF8);
                var Genesis = JsonConvert.DeserializeObject<X509ExecutorPrerequisites>(Json);

                if (Genesis.RequiredKeys is null)
                    Genesis.RequiredKeys = new List<X509GenerateCommand>();

                X509Settings.ExecutorSettings.Prerequisites = Genesis;
            }

            else if (Debugger.IsAttached)
            {
                X509Settings.ExecutorSettings.Prerequisites.RequiredKeys.Add(new X509GenerateCommand
                {
                    KeyType = CertificateType.Root,
                    Algorithm = "rsa-2048", // "secp384r1",
                    ExpirationHours = 10 * 365 * 24,
                    Subject = "CN=NIdentity Root CA",
                    Issuer = "CN=NIdentity Root CA",
                    WithOcsp = true,
                    WithCrlDists = true,
                    WithCAIssuers = true
                });
            }
        }

        /// <summary>
        /// Build the web application.
        /// </summary>
        /// <param name="Host"></param>
        /// <returns></returns>
        private static WebApplication BuildWebApp(WebApplicationBuilder Host)
        {
            var Wapp = Host.Build();

            Wapp.UseCors()
                .UseWebSockets();

            using (var Scope = Wapp.Services.CreateScope())
            {
                Scope.ServiceProvider
                    .GetRequiredService<AppContext>()
                    .Database.Migrate();
            }

            Wapp.UseCommandTerraforms(Terraforms =>
                {
                    Terraforms
                        .WithX509();

                })
                .UseRouting()
                .UseEndpoints(X =>
                {
                    X.MapRazorPages();
                    X.MapControllers();
                    X.MapCommandEndpoint("/api/infra/live");
                    X.MapX509Endpoint();
                });

            Wapp.MigrateX509();
            return Wapp;
        }

        /// <summary>
        /// Wait for MySQL to be ready.
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        private static string WaitForMySQL(string ConnStr)
        {
            while (true)
            {
                using var Temp = new MySqlConnection(ConnStr);

                try
                {
                    Temp.Open();

                }
                catch
                {
                    Thread.Sleep(1000 * 5);
                    continue;
                }

                Temp.Close();
                break;
            }

            return ConnStr.ToString();
        }
    }
}