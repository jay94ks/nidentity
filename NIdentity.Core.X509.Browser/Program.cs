using NIdentity.Core.X509.Browser.Forms;
using System.Net;

namespace NIdentity.Core.X509.Browser
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServicePointManager.ServerCertificateValidationCallback = (S, C, C2, S2) =>
            {
                return true;
            };
            ServicePointManager.CheckCertificateRevocationList = false;
            

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmMain());
        }
    }
}