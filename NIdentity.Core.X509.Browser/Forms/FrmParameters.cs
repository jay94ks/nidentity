using Newtonsoft.Json;
using NIdentity.Connector;
using NIdentity.Connector.X509;
using NIdentity.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIdentity.Core.X509.Browser.Forms
{
    public partial class FrmParameters : Form
    {
        public FrmParameters()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Parameter Model.
        /// </summary>
        public class Model
        {
            /// <summary>
            /// Server URI.
            /// </summary>
            [JsonProperty("server_uri")]
            public string ServerUri { get; set; } = "https://127.0.0.1:7000/";

            /// <summary>
            /// Super Mode.
            /// </summary>
            [JsonProperty("super_mode")]
            public bool SuperMode { get; set; }

            /// <summary>
            /// Authority Certificate.
            /// </summary>
            [JsonProperty("pfx_base64")]
            public string PfxBase64 { get; set; }

            /// <summary>
            /// Timeout.
            /// </summary>
            [JsonProperty("timeout")]
            public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(15);

            /// <summary>
            /// Executor Mode (auto detect).
            /// </summary>
            [JsonIgnore]
            public RemoteCommandExecutorMode Mode => string.IsNullOrWhiteSpace(ServerUri)
                ? RemoteCommandExecutorMode.Https
                : (ServerUri.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || ServerUri.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                ? RemoteCommandExecutorMode.Https : RemoteCommandExecutorMode.WebSockets);
        }

        // ------------------
        private static readonly string BasePath = Path.GetDirectoryName(typeof(FrmParameters).Assembly.Location);
        private static readonly string ModelPath = Path.Combine(BasePath, "settings.json");

        /// <summary>
        /// Load the <see cref="Model"/> from JSON.
        /// </summary>
        /// <returns></returns>
        public static Model LoadModel()
        {
            try
            {
                if (File.Exists(ModelPath))
                {
                    var Text = File.ReadAllText(ModelPath);
                    return JsonConvert.DeserializeObject<Model>(Text);
                }
            }

            catch { }
            return new Model();
        }

        /// <summary>
        /// Save the <see cref="Model"/> into JSON.
        /// </summary>
        /// <param name="Model"></param>
        private void SaveModel(Model Model)
        {
            try
            {
                File.WriteAllText(ModelPath,
                    Model.ToJson().ToString(Formatting.Indented));
            }
            catch
            {
            }
        }

        private Model m_Model;
        private Task m_Tester;
        private Action m_Canceler;

        /// <summary>
        /// Parameters.
        /// </summary>
        public static RemoteCommandExecutorParameters MakeParameters(Model ParameterModel) => new RemoteCommandExecutorParameters
        {
            ServerUri = !string.IsNullOrWhiteSpace(ParameterModel.ServerUri)
                    ? new Uri(ParameterModel.ServerUri)
                    : null,

            Certificate = !string.IsNullOrWhiteSpace(ParameterModel.PfxBase64)
                    ? Certificate.ImportPfx(Convert.FromBase64String(ParameterModel.PfxBase64))
                    : null,

            CacheRepository = null, Mode = ParameterModel.Mode, IsSuperMode = ParameterModel.SuperMode,
            Timeout = TimeSpan.FromSeconds((int)Math.Max(ParameterModel.Timeout.TotalSeconds, 1))
        };

        /// <summary>
        /// Parameter Model.
        /// </summary>
        public Model ParameterModel => GetterHelpers.Cached(ref m_Model, () => LoadModel());

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReloadModel();
        }

        /// <summary>
        /// Reset the parameters to saved parameters.
        /// </summary>
        private void ReloadModel()
        {
            var Parameters = MakeParameters(ParameterModel);
            var Certificate = Parameters.Certificate;
            if (Certificate is null || Certificate.HasPrivateKey == false)
            {
                m_LblAuthority.Text = "(Nothing Selected)";
                ParameterModel.PfxBase64 = null;
            }

            else
            {
                m_LblAuthority.Text = $"{Certificate.Subject} ({Certificate.SerialNumber})";
            }

            m_EditServerUri.Text = Parameters.ServerUri.ToString().TrimEnd('/');
            m_ChkSuperMode.Checked = Parameters.IsSuperMode;
            m_LstMode.SelectedIndex = (int)Parameters.Mode;
            m_Timeout.Value = (int)Math.Max(Parameters.Timeout.TotalSeconds, 1);
        }

        /// <summary>
        /// Called when Authority Certificate label clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectAuthorityCertificate(object sender, EventArgs e)
        {
            while (true)
            {
                using var Ofd = new OpenFileDialog
                {
                    Title = "Select Authority Ceritificate with Private Key",
                    Filter = "PKCS#12 Key Store (*.pfx, *.p12) | *.pfx; *.p12",
                    CheckPathExists = true
                };

                if (Ofd.ShowDialog() != DialogResult.OK)
                    return;

                if (string.IsNullOrWhiteSpace(Ofd.FileName) || File.Exists(Ofd.FileName) == false)
                {
                    if (string.IsNullOrWhiteSpace(Ofd.FileName))
                        continue;

                    MessageBox.Show(
                        $"Error: No file exists.\nPath: {Ofd.FileName}.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                try
                {
                    var PfxBytes = File.ReadAllBytes(Ofd.FileName);
                    if (PfxBytes is null || PfxBytes.Length <= 0)
                        throw new Exception("reselect");

                    var Cert = Certificate.ImportPfx(PfxBytes);
                    if (Cert is null)
                        throw new Exception("reselect");

                    if (Cert.HasPrivateKey == false)
                    {
                        MessageBox.Show(
                            "Error: the specified file has no private key, " +
                            "please select other file.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        continue;
                    }

                    ParameterModel.PfxBase64 = Convert.ToBase64String(PfxBytes);
                    m_LblAuthority.Text = $"{Cert.Subject} ({Cert.SerialNumber})";
                    break;
                }
                catch
                {
                    MessageBox.Show(
                        "Error: the specified file is corrupted or not supported, " +
                        "please select other file.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    continue;
                }
            }
        }

        /// <summary>
        /// Called when the connection mode selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectConnectionMode(object sender, EventArgs e)
        {
            var Value = m_EditServerUri.Text;
            if (string.IsNullOrWhiteSpace(Value) || !Value.Contains(':'))
            {
                MessageBox.Show(
                    "Error: no server-uri specified, before selecting mode, " +
                    "please fill it first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            var TargetMode  = (RemoteCommandExecutorMode)m_LstMode.SelectedIndex;
            var Scheme = Value.Split(':', 2).FirstOrDefault();
            var AfterPart = Value.Split(':', 2).LastOrDefault();

            if (TargetMode == RemoteCommandExecutorMode.WebSockets && Scheme == "http")
                m_EditServerUri.Text = ParameterModel.ServerUri = $"ws:{AfterPart}";

            else if (TargetMode == RemoteCommandExecutorMode.WebSockets && Scheme == "https")
                m_EditServerUri.Text = ParameterModel.ServerUri = $"wss:{AfterPart}";

            else if (TargetMode == RemoteCommandExecutorMode.Https && Scheme == "ws")
                m_EditServerUri.Text = ParameterModel.ServerUri = $"http:{AfterPart}";

            else if (TargetMode == RemoteCommandExecutorMode.Https && Scheme == "wss")
                m_EditServerUri.Text = ParameterModel.ServerUri = $"https:{AfterPart}";
        }

        /// <summary>
        /// Called when the Server URI changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeServerUri(object sender, EventArgs e)
        {
            var Value = m_EditServerUri.Text;
            if (string.IsNullOrWhiteSpace(Value) || !Value.Contains(':'))
                return;

            var Scheme = Value.Split(':',2).FirstOrDefault() ?? string.Empty;
            if (Scheme.Length <= 1)
                return;

            if (Scheme == "https" || Scheme == "http")
                m_LstMode.SelectedIndex = (int)RemoteCommandExecutorMode.Https;

            else
                m_LstMode.SelectedIndex = (int)RemoteCommandExecutorMode.WebSockets;

            ParameterModel.ServerUri = Value;
        }

        /// <summary>
        /// Called when the timeout set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSetTimeout(object sender, EventArgs e)
        {
            ParameterModel.Timeout = TimeSpan.FromSeconds(Math.Max((int)m_Timeout.Value, 1));
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveSettings(object sender, EventArgs e)
        {
            SaveModel(ParameterModel);

            // --> reload the model.
            m_Model = null;
            ReloadModel();

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelSettings(object sender, EventArgs e)
        {
            // --> reload the model.
            m_Model = null;
            ReloadModel();

            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Called when test connectivity button pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTestConnectivity(object sender, EventArgs e)
        {
            if (m_Tester != null && !m_Tester.IsCompleted)
            {
                CancelTester();
                return;
            }

            var Parameters = MakeParameters(ParameterModel);
            try { Parameters.ThrowExceptionIfInvalid(); }
            catch(Exception Error)
            {
                MessageBox.Show(
                    $"Error: {Error.Message}",
                     Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            var ButtonText = m_BtnTest.Text;
            var Cts = new CancellationTokenSource(ParameterModel.Timeout);
            void OnFormClosing(object _, EventArgs e) => CancelTester();

            // --> Run the tester asynchonously.
            async Task RunAsync(CancellationToken Token)
            {

                try
                {
                    using var Executor = new RemoteCommandExecutor(Parameters);
                    var Result = await Executor.X509.GetCertificateAsync(
                        Parameters.Certificate.Self, Token);

                    if (Result is null || Result.Type == CertificateType.Leaf)
                        throw new Exception("Error: no authority recognized.");

                    try
                    {
                        if (InvokeRequired)
                        {
                            Invoke(() =>
                            {
                                MessageBox.Show("Success: the server granted authority access.",
                                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            });

                            return;
                        }

                        MessageBox.Show("Success: the server granted authority access.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    catch { }
                }

                catch (Exception Error)
                {
                    ShowErrorFromAsync(Error);
                }

                finally
                {
                    try
                    {
                        if (InvokeRequired)
                            Invoke(() => m_BtnTest.Text = ButtonText);

                        else
                            m_BtnTest.Text = ButtonText;
                    }

                    catch { }
                    m_Canceler = null;
                    FormClosing -= OnFormClosing;
                }
            }

            m_BtnTest.Text = "Cancel Tester";
            m_Canceler = () =>
            {
                try { Cts.Cancel(); }
                catch
                {
                }
            };

            FormClosing += OnFormClosing;
            m_Tester = RunAsync(Cts.Token);
        }

        /// <summary>
        /// Show an error from asynchronous task.
        /// </summary>
        /// <param name="Error"></param>
        private void ShowErrorFromAsync(Exception Error)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(() => ShowErrorFromAsync(Error));
                    return;
                }

                MessageBox.Show(Error.Message, Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch { }
        }

        /// <summary>
        /// Cancel the tester task.
        /// </summary>
        private void CancelTester()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(() => CancelTester);
                    return;
                }

                m_BtnTest.Text = "Canceling...";
            }

            catch { }
            m_Canceler?.Invoke();
        }

        private void OnCheckSuperMode(object sender, EventArgs e)
        {
            ParameterModel.SuperMode = m_ChkSuperMode.Checked;
        }
    }
}
