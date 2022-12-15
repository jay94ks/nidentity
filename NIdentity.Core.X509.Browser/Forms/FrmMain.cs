using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIdentity.Connector;
using NIdentity.Connector.X509;
using NIdentity.Core.Helpers;
using NIdentity.Core.X509.Browser.Forms.Docs;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Documents;
using System.Diagnostics;

namespace NIdentity.Core.X509.Browser.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var Lrr = m_LstRevokeReason.Items;
            for(int i = 0; i < Lrr.Count; ++i)
            {
                var Each = Lrr[i];
                var Cmn = m_CmnRevoke.DropDownItems.Add(Each.ToString());
                Cmn.Tag = i;
                Cmn.Click += OnRevokeWithReason;
            }

            SetDocumentOrigin(null);
        }

        private void OnExitApp(object sender, EventArgs e) => Application.Exit();
        private void OnSettings(object sender, EventArgs e)
        {
            using var Settings = new FrmParameters();
            Settings.ShowDialog();
        }

        private void OnActivate(object sender, EventArgs e)
        {
            m_MenuStatusSettings.Enabled = false;
            m_MenuStatusActivate.Enabled = false;
            m_MenuStatusDeactivate.Enabled = true;
            m_PnlControls.Visible = false;

            lock (this)
            {
                m_TokenSource = new CancellationTokenSource();
                m_Token = m_TokenSource.Token;
            }

            try
            {
                var Model = FrmParameters.LoadModel();
                var Parameters = FrmParameters.MakeParameters(Model);

                m_Executor = new RemoteCommandExecutor(Parameters);
                m_Authority = Parameters.Certificate;

                m_Executor.Executing += Json =>
                {
                    try
                    {
                        Invoke(() =>
                        {
                            var Data = Json.ToString(Formatting.None);
                            var Text = DateTimeOffset.Now.ToString() + " EXEC: " + Data;
                            toolStripStatusLabel1.Text = "Executing: " + Data;

                            if (listBox1.Items.Count >= 1024)
                                listBox1.Items.RemoveAt(1);

                            listBox1.Items.Add(Text);
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                    }
                    catch
                    {
                    }
                };

                m_Executor.Executed += Result =>
                {
                    try {
                        Invoke(() =>
                        {
                            var Data = Result.ToJson().ToString(Formatting.None);
                            var Text = DateTimeOffset.Now.ToString() + " RETN: " + Data;
                            toolStripStatusLabel1.Text = "Result: " + Data;

                            if (listBox1.Items.Count >= 1024)
                                listBox1.Items.RemoveAt(1);

                            listBox1.Items.Add(Text);
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                    }
                    catch
                    {
                    }
                };
            }

            catch(Exception Error)
            {
                MessageBox.Show(Error.Message, Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                OnDeactivate(sender, e);
                return;
            }

            Start();
        }

        private void OnDeactivate(object sender, EventArgs e)
        {
            m_MenuStatusSettings.Enabled = true;
            m_MenuStatusActivate.Enabled = true;
            m_MenuStatusDeactivate.Enabled = false;
            m_PnlControls.Visible = false;

            m_BtnExecute.Enabled = false;
            label3.Text = "NOT CONNECTED";
            label4.Visible = false;

            Cleanup();

            try { m_Executor?.Dispose(); }
            catch
            {
            }

            m_ViewCertificate.SelectedObject = null;
            m_TreeCerts.Nodes.Clear();
            SetDocumentOrigin(null);

            m_Authority = null;
            m_Executor = null;
        }

        private void Cleanup()
        {
            CancellationTokenSource Cts;
            lock (this)
            {
                if ((Cts = m_TokenSource) is null)
                    return;

                m_Token = new CancellationToken(true);
                m_TokenSource = null;
            }

            Cts?.Cancel();
            Cts?.Dispose();
        }

        private CancellationTokenSource m_TokenSource;
        private CancellationToken m_Token;
        private Certificate m_Authority;

        // ----
        private CancellationToken Token { get { lock (this) return m_Token; } }

        // ---- 
        private RemoteCommandExecutor m_Executor;
        private Queue<Func<RemoteCommandExecutor, Task>> m_Actions = new();
        private Task m_Worker = Task.CompletedTask;


        private async Task RunWorker(CancellationToken Token)
        {
            await Task.Yield();

            try { await Task.Delay(0, Token); }
            catch
            {
                return;
            }

            while(true)
            {
                Func<RemoteCommandExecutor, Task> Action;
                RemoteCommandExecutor Executor;

                try { Executor = Invoke(() => m_Executor); }
                catch
                {
                    Executor = null;
                }

                lock(this)
                {
                    if (!m_Actions.TryDequeue(out Action) ||
                        Token.IsCancellationRequested || Executor is null)
                    {
                        m_Actions.Clear();
                        m_Worker = Task.CompletedTask;
                        break;
                    }
                }

                try { await Action.Invoke(Executor); }
                catch(Exception Error)
                {
                    try
                    {
                        Invoke(() =>
                        {
                            if (Debugger.IsAttached)
                            {
                                MessageBox.Show(Error.ToString(), Text,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            else
                            {
                                MessageBox.Show(Error.Message, Text,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        });
                    }
                    catch { }
                }
            }
        }


        /// <summary>
        /// Execute an asynchronous task.
        /// </summary>
        /// <param name="Action"></param>
        /// <returns></returns>
        private FrmMain Exec(Func<CancellationToken, RemoteCommandExecutor, Task> Action)
        {
            var Token = this.Token;
            lock(this)
            {
                m_Actions.Enqueue(X => Action.Invoke(Token, X));
                if (m_Worker.IsCompleted)
                    m_Worker = RunWorker(Token);
            }

            return this;
        }

        private void Start()
        {
            m_TreeCerts.Nodes.Clear();
            if (m_Authority is null)
                return;

            var Model = FrmParameters.LoadModel();
            if (Model.ServerUri.StartsWith("wss://"))
                label3.Text = $"WEBSOCKET {Model.ServerUri}";

            else
                label3.Text = $"POST {Model.ServerUri} HTTP/1.1";

            if (m_Authority is null)
                label4.Visible = false;

            else
            {
                label4.Text = $"SSL certificate: {m_Authority.Subject}, SN: {m_Authority.SerialNumber} ({m_Authority.KeyIdentifier})";
                label4.Visible = true;
            }

            var Node = m_TreeCerts.Nodes
                .Add(m_Authority.Subject);

            Node.Tag = m_Authority;
            m_Authority.UserTag = Node;
            m_BtnExecute.Enabled = true;

            LoadMore(m_Authority);
        }

        private void LoadMore(Certificate Authority, Action<IEnumerable<Certificate>> Callback = null)
        {
            Exec(async (Token, Executor) =>
            {
                var Offset = 0;
                var Certificates = new List<Certificate>();
                
                while (Authority != null)
                {
                    var List = await Executor.X509.ListCertificatesAsync(Authority, Offset, 20, Token);
                    if (List is null)
                        break;

                    Offset += List.Count;
                    Certificates.AddRange(List);

                    if (List.Count < 20)
                        break;
                }

                Invoke(() =>
                {
                    Display(Authority, Certificates);
                    Callback?.Invoke(Certificates.ToArray());
                });
            });
        }

        private void Display(Certificate Origin, IEnumerable<Certificate> Certificates)
        {
            var Node = Origin.UserTag as TreeNode;

            foreach (var Each in Certificates)
            {
                TreeNode Child = null;
                for(var i = 0; i < Node.Nodes.Count; ++i)
                {
                    var Cert = Node.Nodes[i].Tag as Certificate;
                    if (Each.Self.IsExact(Cert))
                    {
                        Child = Node.Nodes[i];
                        break;
                    }
                }

                if (Child is null)
                    Child = Node.Nodes.Add(Each.Subject);

                Child.Tag = Each;
                Each.UserTag = Child;
            }

            var Queue = new Queue<TreeNode>();
            for (var i = 0; i < Node.Nodes.Count; ++i)
            {
                var Cert = Node.Nodes[i].Tag as Certificate;
                if (Certificates.FirstOrDefault(X => X.Self.IsExact(Cert)) is null)
                    Queue.Enqueue(Node.Nodes[i]);
            }

            while (Queue.TryDequeue(out var Each))
                Node.Nodes.Remove(Each);
        }

        private void OnLoadRequest(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (m_Authority is null || e.Node.Tag is not Certificate Origin)
            {
                m_CmnNew.Enabled = false;
                m_CmnRevoke.Enabled = false;
                m_CmnUnresolve.Enabled = false;
                m_CmnStatus.Enabled = false;
                return;
            }

            var NoDelete = Origin.Self.IsExact(m_Authority);

            m_BtnDelete.Enabled = false;
            m_ViewCertificate.SelectedObject = Origin;
            m_PnlControls.Visible = m_BtnStatus.Enabled = true;

            m_LstRevokeReason.Enabled = !Origin.IsRevokeIdentified;
            m_BtnRevoke.Enabled = !Origin.IsRevokeIdentified;
            m_BtnUnrevoke.Enabled = Origin.IsRevokeIdentified;

            // -- Context Menu.
            m_CmnNew.Enabled = m_CmnStatus.Enabled = true;
            m_CmnRevoke.Enabled = !Origin.IsRevokeIdentified;
            m_CmnUnresolve.Enabled = Origin.IsRevokeIdentified;
            m_TabCertInfo.Text = $"{Origin.Subject} ({Origin.KeyIdentifier})";

            if (Origin.IsRevokeIdentified)
                m_LstRevokeReason.SelectedIndex = (int)Origin.RevokeReason.Value;

            else
                m_LstRevokeReason.SelectedIndex = 0;

            SetDocumentOrigin(Origin);
            if (Origin.Type == CertificateType.Leaf)
            {
                m_BtnDelete.Enabled = !NoDelete;
                return;
            }

            LoadMore(Origin, X => m_BtnDelete.Enabled = !NoDelete && X.Count() <= 0);
        }

        private void OnRevokeOrigin(object sender, EventArgs e)
        {
            var Node = m_TreeCerts.SelectedNode;
            if (m_Authority is null || Node is null || Node.Tag is not Certificate Origin)
                return;

            var Reason = (CertificateRevokeReason)m_LstRevokeReason.SelectedIndex;
            if (Reason == CertificateRevokeReason.None)
            {
                MessageBox.Show(
                    "Error: select the reason in left combo box.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            if (Origin.IsRevokeIdentified)
            {
                MessageBox.Show(
                    "This certificate is already revoked. to alter its reason, please unrevoke first.",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return;
            }

            Exec(async (Token, Executor) =>
            {
                if (await Executor.X509.RevokeCertificateAsync(Origin, Reason, Token))
                {
                    var Reload = await Executor.X509.GetCertificateAsync(Origin, Token);
                    try
                    {
                        Invoke(() =>
                        {
                            if (m_Authority is null || Node is null || Node.Tag is not Certificate CurrentOrigin)
                                return;

                            if (!Origin.Self.IsExact(CurrentOrigin))
                                return;

                            Node.Tag = Reload;
                            Reload.UserTag = Node;

                            m_BtnRevoke.Enabled = false;
                            m_BtnUnrevoke.Enabled = true;
                            m_LstRevokeReason.Enabled = false;
                            m_ViewCertificate.SelectedObject = Reload;
                        });
                    }

                    catch
                    {
                    }
                }
            });
        }

        private void OnUnrevokeOrigin(object sender, EventArgs e)
        {
            var Node = m_TreeCerts.SelectedNode;
            if (m_Authority is null || Node is null || Node.Tag is not Certificate Origin)
                return;

            if (!Origin.IsRevokeIdentified)
                return;

            Exec(async (Token, Executor) =>
            {
                if (await Executor.X509.UnrevokeCertificateAsync(Origin, Token))
                {
                    var Reload = await Executor.X509.GetCertificateAsync(Origin, Token);
                    try
                    {
                        Invoke(() =>
                        {
                            if (m_Authority is null || Node is null || Node.Tag is not Certificate CurrentOrigin)
                                return;

                            if (!Origin.Self.IsExact(CurrentOrigin))
                                return;

                            Node.Tag = Reload;
                            Reload.UserTag = Node;

                            m_BtnRevoke.Enabled = true;
                            m_BtnUnrevoke.Enabled = false;
                            m_LstRevokeReason.Enabled = true;
                            m_ViewCertificate.SelectedObject = Reload;
                        });
                    }

                    catch
                    {
                    }
                }
            });
        }

        private void OnDeleteOrigin(object sender, EventArgs e)
        {
            var Node = m_TreeCerts.SelectedNode;
            if (m_Authority is null || Node is null || Node.Tag is not Certificate Origin)
                return;

            if (MessageBox.Show(
                "This behavior is irreversible. Would you like to proceed?",
                Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                != DialogResult.Yes)
                return;

            Exec(async (Token, Executor) =>
            {
                if (await Executor.X509.DeleteCertificateAsync(Origin, Token))
                {
                    try
                    {
                        Invoke(() =>
                        {
                            if (m_Authority is null || Node is null || Node.Tag is not Certificate CurrentOrigin)
                                return;

                            if (!Origin.Self.IsExact(CurrentOrigin))
                                return;

                            Node.Remove();

                            m_PnlControls.Visible = false;
                            m_BtnRevoke.Enabled = false;
                            m_BtnUnrevoke.Enabled = false;
                            m_LstRevokeReason.Enabled = false;
                            m_ViewCertificate.SelectedObject = null;
                        });
                    }

                    catch
                    {
                    }
                }
            });
        }

        private void OnGenerateNew(object sender, EventArgs e)
        {
            var Node = m_TreeCerts.SelectedNode;
            if (m_Authority is null || Node is null || Node.Tag is not Certificate Origin)
                return;

            using var New = new FrmGenerator();

            New.Issuer = Origin.Subject;
            New.IssuerKeyIdentifier = Origin.KeyIdentifier;

            if (New.ShowDialog() != DialogResult.OK)
                return;

            var Command = New.Command;
            var SavePath = New.PfxSavePath;


            Exec(async (Token, Executor) =>
            {
                var Data = await Executor.X509.GenerateCertificateAsync(Command, Token);
                if (Data is not X509GenerateCommand.Result Result || Result.Success == false)
                {
                    try
                    {
                        Invoke(() =>
                        {
                            MessageBox.Show(
                                "Error: failed to generate a new certificate.\n" +
                                $"{Data.ReasonKind}: {Data.Reason}",
                                Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        });
                    }

                    catch { }
                    return;
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(SavePath)) 
                    { 
                        var PfxBytes = Convert.FromBase64String(Result.PfxBase64);
                        var IsPfx = SavePath.EndsWith(".pfx");

                        if (SavePath.EndsWith(".pem"))
                            PfxBytes = Certificate.ImportPfx(PfxBytes).ExportPem();


                        File.WriteAllBytes(SavePath, PfxBytes);
                        try
                        {
                            Invoke(() =>
                            {
                                MessageBox.Show(
                                    "New certificate generated successfully.\n" +
                                    $"Pfx path: {SavePath}.",
                                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            });
                        }

                        catch { }
                    }
                }
                catch { }

                if (m_Authority is null || Node is null || Node.Tag is not Certificate)
                    return;

                LoadMore(Origin);
            });
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }


        private void OnRevokeWithReason(object sender, EventArgs e)
        {
            if (sender is not ToolStripItem Item)
                return;

            m_LstRevokeReason.SelectedIndex = (int)Item.Tag;
            OnRevokeOrigin(sender, e);
        }

        private void OnStatusView(object sender, EventArgs e)
        {
            var Node = m_TreeCerts.SelectedNode;
            if (m_Authority is null || Node is null || Node.Tag is not Certificate Origin)
                return;

            Exec(async (Token, Executor) =>
            {
                var Cmd = new X509GetCertificateMetaCommand();

                Cmd.Subject = Origin.Subject;
                Cmd.KeyIdentifier = Origin.KeyIdentifier;

                var Result = await Executor.X509.GetCertificateMetaAsync(Cmd, Token);

                try
                {
                    Invoke(() =>
                    {
                        var Frm = new FrmStatus();

                        Cmd.Type = "x509.cert_get_meta";
                        Frm.Certificate = m_Authority;
                        Frm.Text = $"NIdentity X509: {Cmd.Subject} ({Cmd.KeyIdentifier})";
                        Frm.Command = Cmd.ToJson().ToString(Formatting.Indented);
                        Frm.Result = Result.ToJson().ToString(Formatting.Indented);

                        Frm.Show();
                    });
                }
                catch { }
            });
        }


        private void OnExportCer(object sender, EventArgs e)
        {
            var Node = m_TreeCerts.SelectedNode;
            if (m_Authority is null || Node is null || Node.Tag is not Certificate Origin)
                return;

            var CerBytes = null as byte[];

            try { CerBytes = Origin.Export(); }
            catch
            {
                MessageBox.Show(
                    "Error: this certificate does not support to export as .cer file.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            using var Sfd = new SaveFileDialog()
            {
                Title = "Select CER file Save Location",
                Filter = "DER encoded binary X.509 (*.cer) | *.cer"
            };

            if (Sfd.ShowDialog() != DialogResult.OK)
                return;

            var FileName = Sfd.FileName;
            if (!FileName.EndsWith(".cer"))
                FileName += ".cer";

            try { File.WriteAllBytes(FileName, CerBytes); }
            catch
            {
                MessageBox.Show(
                    $"Error: failed to write file: {FileName}.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        // ------------
        private Certificate m_DocOrigin;
        private Document m_Document;

        private Dictionary<string, TreeNode> m_DocNodes = new();

        private void SetDocumentOrigin(Certificate Origin)
        {
            m_DocOrigin = Origin;

            m_DocNodes.Clear();
            m_TreeDocs.Nodes.Clear();
            m_ViewDoc.SelectedObject = null;

            m_EditDocument.Text = "";
            m_EditDocument.ReadOnly = true;

            m_BtnNewDocument.Enabled = false;
            m_BtnDeleteDocument.Enabled = false;

            m_BtnSaveDocument.Enabled = false;
            m_Document = null;

            if (Origin is null)
                return;

            m_BtnNewDocument.Enabled = true;
            LoadDocument(Origin, "/");
        }

        private void LoadDocument(Certificate Origin, string Path)
        {
            Exec(async (Token, Executor) =>
            {
                DocumentList List = null;

                try { List = await Executor.X509.ListDocumentsAsync(Origin.Self, Path, Token); }
                catch
                {
                }

                if (List is null || List.Documents is null || List.Documents.Length <= 0)
                    return;

                try
                {
                    Invoke(() =>
                    {
                        UpdateTreeNode(List.Documents);
                    });
                }
                catch { }
            });

            Exec(async (Token, Executor) =>
            {
                Document Document;

                try { Document = await Executor.X509.ReadDocumentAsync(Origin.Self, Path, null, Token); }
                catch
                {
                    return;
                }
                try
                {
                    Invoke(() =>
                    {
                        m_Document = Document;
                        m_ViewDoc.SelectedObject = Document;
                        m_BtnSaveDocument.Enabled = false;
                        m_EditDocument.Text = Document.Data;
                        m_EditDocument.ReadOnly = false;
                    });
                }
                catch { }
            });
        }

        private void UpdateTreeNode(IEnumerable<string> Documents)
        {
            foreach (var Each in Documents)
            {
                var Full = Each.Split('/')
                    .Where(X => !string.IsNullOrWhiteSpace(X))
                    .Reverse().Append("").Reverse().ToArray();

                var Paths = new Queue<string>(Full);
                var Progress = new List<string>();
                var Parent = null as TreeNode;

                while (Paths.TryDequeue(out var Name))
                {
                    var Cur = string.Join("/", Progress.Append(Name)).Trim('/');

                    if (m_DocNodes.TryGetValue(Cur, out var Node))
                        Parent = Node;

                    else
                    {
                        if (Parent is null)
                            Parent = m_TreeDocs.Nodes.Add("/" + Name);

                        else
                            Parent = Parent.Nodes.Add("/" + Name);

                        Parent.Tag = Cur;
                        m_DocNodes.Add(Cur, Parent);
                    }

                    Progress.Add(Name);
                }
            }
        }

        private void OnNewDocument(object sender, EventArgs e)
        {
            if (m_DocOrigin is null)
                return;

            var Origin = m_DocOrigin;
            using var Frm = new FrmNewDocument();

            Frm.Document.Identity = new DocumentIdentity(Origin.Self, "");
            if (Frm.ShowDialog() != DialogResult.OK)
                return;

            OnCompleteNewDocument(Origin, Frm.Document);
        }

        private void OnCompleteNewDocument(Certificate Origin, Document Document)
        {
            Exec(async (Token, Executor) =>
            {
                Document Result = null;
                Exception Error = null;
                try { Result = await Executor.X509.WriteDocumentAsync(Document, Token); }
                catch (Exception _Error)
                {
                    Error = _Error;
                }

                if (Result is null)
                {
                    try
                    {
                        Invoke(() =>
                        {
                            if (Error != null && MessageBox.Show(Error.Message, Text,
                                MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                                == DialogResult.Cancel)
                            {
                                return;
                            }

                            using var Frm = new FrmNewDocument();

                            Frm.Document.Identity = new DocumentIdentity(Origin.Self, "");
                            if (Frm.ShowDialog() != DialogResult.OK)
                                return;

                            OnCompleteNewDocument(Origin, Frm.Document);
                        });
                    }

                    catch { }
                    return;
                }

                UpdateTreeNode(new string[] { Result.Identity.PathName });
            });
        }

        private void OnSelectDocument(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is null || e.Node.Tag == null || m_DocOrigin is null)
                return;

            LoadDocument(m_DocOrigin, e.Node.Tag.ToString());
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            m_BtnSaveDocument.Enabled = true;
        }

        private void m_BtnSaveDocument_Click(object sender, EventArgs e)
        {
            if (m_DocOrigin is null || m_Document is null)
            {
                m_BtnSaveDocument.Enabled = false;
                return;
            }

            m_Document.Data = m_EditDocument.Text;

            Exec(async (Token, Executor) =>
            {
                var New = await Executor.X509.WriteDocumentAsync(m_Document, Token);
                try
                {
                    Invoke(() =>
                    {
                        if (New is null)
                            return;

                        m_Document = New;
                        m_ViewDoc.SelectedObject = New;
                        m_BtnSaveDocument.Enabled = false;
                        m_EditDocument.Text = New.Data;
                        m_EditDocument.ReadOnly = false;
                    });
                }
                catch { }
            });
        }

        private void OnDocumentPropertyChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (m_DocOrigin is null || m_Document is null)
            {
                m_BtnSaveDocument.Enabled = false;
                return;
            }

            m_BtnSaveDocument.Enabled = true;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void m_BtnExecute_Click(object sender, EventArgs e)
        {
            if (m_Authority is null)
                return;

            var Text = m_EditManualCommand.Text;


            Exec(async (Token, Exec) =>
            {
                var Json = JsonConvert.DeserializeObject<JObject>(Text);
                if (Json is null)
                {
                    try
                    {
                        Invoke(() =>
                        {
                            m_EditManualResult.Text = "null";
                        });
                    }
                    catch { }
                    return;
                }

                var Result = await Exec.Execute(Json, Token);
                try
                {
                    Invoke(() =>
                    {
                        if (Result is not RemoteCommandResult Remote)
                            m_EditManualResult.Text = Result.ToJson().ToString(Formatting.Indented);

                        else
                            m_EditManualResult.Text = Remote.Response.ToString(Formatting.Indented);
                    });
                }
                catch { }
            });
        }

        private void pFXToPEMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string PathName;
            {

                using var Ofd = new OpenFileDialog
                {
                    Title = "Select PFX Location",
                    Filter = "PKCS#12 Key Store (*.pfx)|*.pfx"
                };

                if (Ofd.ShowDialog() != DialogResult.OK)
                    return;

                PathName = Ofd.FileName;
                if (!PathName.EndsWith(".pfx"))
                    PathName += ".pfx";
            }

            Certificate Cert;
            try
            {
                Cert = CertificateStore.Import(File.ReadAllBytes(PathName))
                    .Certificates.OrderByDescending(X => X.HasPrivateKey ? 1 : 0)
                    .FirstOrDefault();
            }
            catch
            {
                MessageBox.Show(
                    "Error: failed to laod PFX file, please choose other file.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            var PemBytes = Cert.ExportPem();
            {
                using var Sfd = new SaveFileDialog
                {
                    Title = "Select PEM Save Location",
                    Filter = "PKCS8 Key Store (*.pem)|*.pem"
                };

                if (Sfd.ShowDialog() != DialogResult.OK)
                    return;

                PathName = Sfd.FileName;
                if (!PathName.EndsWith(".pem"))
                    PathName += ".pem";
            }

            try { File.WriteAllBytes(PathName, PemBytes); }
            catch
            {
                MessageBox.Show(
                    "Error: failed to save PEM file, IO error.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void pFXPasswordChangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var Pfx = new FrmChangePfxPassword();
            Pfx.ShowDialog();
        }
    }
}