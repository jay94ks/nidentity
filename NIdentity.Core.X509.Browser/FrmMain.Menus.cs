﻿using NIdentity.Connector;
using NIdentity.Connector.X509;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Controls.Internals;

namespace NIdentity.Core.X509.Browser
{
    public partial class FrmMain
    {
        private InstrusiveWorker m_Worker = new();

        /// <summary>
        /// Initialize menus.
        /// </summary>
        private void InitializeMenus()
        {
            m_MenuGenerate.Enabled = false;
            m_MenuRevoke.Enabled = false;
            m_MenuUnrevoke.Enabled = false;
            m_MenuDelete.Enabled = false;
        }

        /// <summary>
        /// Called when "Settings" menu clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSettings(object sender, EventArgs e)
        {
            using var Frm = new FrmParameters();
            if (Frm.ShowDialog() != DialogResult.OK)
                return;

            var Model = FrmParameters.LoadModel();
            m_Parameters = FrmParameters.MakeParameters(Model);
        }

        /// <summary>
        /// Called when "Connect" menu clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnect(object sender, EventArgs e)
        {
            m_Settings.Enabled = false;
            m_Connect.Enabled = false;
            m_Disconnect.Enabled = true;

            if (m_Executor != null)
                m_Executor.Dispose();

            var Model = FrmParameters.LoadModel();
            m_Worker = new InstrusiveWorker();
            m_Parameters = FrmParameters.MakeParameters(Model);
            m_Executor = new RemoteCommandExecutor(m_Parameters);
            m_X509 = new X509CommandExecutor(m_Executor);

            m_CertList.SetExecutor(m_Executor);
            m_CertTree.SetExecutor(m_Executor);

            m_CertList.Authority = m_Parameters.Certificate;
            m_CertTree.Authority = m_Parameters.Certificate;

            m_CertTree.Reload();
            m_CertList.Reload();
        }

        /// <summary>
        /// Called when "Disconnect" menu clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDisconnect(object sender, EventArgs e)
        {
            m_Settings.Enabled = true;
            m_Connect.Enabled = true;
            m_Disconnect.Enabled = false;

            ResetLists();

            m_Executor?.Dispose();
            m_Worker?.Dispose();
            
            m_X509 = null;
            m_Executor = null;
            m_Worker = null;
        }

        /// <summary>
        /// Called when "Exit" menu clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, EventArgs e) => Application.Exit();

        /// <summary>
        /// Called when `Generate` menu clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMenuGenerate(object sender, EventArgs e)
        {
            if (m_CertTree.SelectedNode is null)
                return;

            if (m_CertTree.SelectedNode.Tag is not Certificate Certificate)
                return;

            using var Generator = new FrmGenerator();

            
            Generator.Issuer = Certificate.Subject;
            Generator.IssuerKeyIdentifier = Certificate.KeyIdentifier;

            if (Generator.ShowDialog() != DialogResult.OK)
                return;

            var Command = Generator.Command;
            var SavePath = Generator.PfxSavePath;


            m_Worker.Enqueue(async (Token) =>
            {
                var Data = await m_X509.GenerateCertificateAsync(Command, Token);
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

                if (Certificate is null)
                    return;

                Invoke(() => m_CertTree.Reload());
            });
        }
    }

}
