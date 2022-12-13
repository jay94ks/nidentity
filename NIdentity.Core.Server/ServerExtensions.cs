using NIdentity.Core.Commands;
using NIdentity.Core.Server.Commands;
using NIdentity.Core.Server.Endpoints;

namespace NIdentity.Core.Server
{
    public static class ServerExtensions
    {
        /// <summary>
        /// Add a command executor.
        /// </summary>
        /// <param name="Services"></param>
        /// <returns></returns>
        public static CommandExecutor.Builder AddCommandExecutor(this IServiceCollection Services, bool EnableBulk = true)
        {
            var Builder = new CommandExecutor.Builder();

            if (EnableBulk)
                Builder.EnableBulk();

            Services.AddScoped<ICommandExecutor>(Services => Builder.Build(Services));
            return Builder;
        }

        /// <summary>
        /// Use a command executor's terraforming middleware.
        /// </summary>
        /// <param name="Builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCommandTerraforms(this IApplicationBuilder Builder, Action<CommandTerraformBuilder> Terraformers)
        {
            var Ctb = new CommandTerraformBuilder();

            Terraformers?.Invoke(Ctb);
            Builder.Use((Http, Next) =>
            {
                var Context = CommandTerraformContext.FromHttpContext(Http);

                foreach (var Each in Ctb.GetTerraformers())
                    Context.Terraformers.Enqueue(Each);

                return Next.Invoke();
            });

            return Builder;
        }

        /// <summary>
        /// Add middleware that enables command executor endpoints.
        /// This will enable "websocket" and "http" json command protocol.
        /// </summary>
        /// <param name="Router"></param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapCommandEndpoint(this IEndpointRouteBuilder Router, string Path = "/api/infra/live")
        {
            Router
                .Map(Path, Http =>
                {
                    var Method = Http.Request.Method.ToLower();
                    if (Method == "get")
                    {
                        return CommandTerraformContext.TerraformAsync(Http,
                            () => HttpExecutorEndpoint.HandleGetAsync(Http));

                    }

                    if (Method == "post")
                    {
                        return CommandTerraformContext.TerraformAsync(Http,
                            () => HttpExecutorEndpoint.HandlePostAsync(Http));
                    }

                    // --> method not allowed.
                    Http.Response.StatusCode = 405;
                    return Task.CompletedTask;
                });

            return Router;
        }
    }
}