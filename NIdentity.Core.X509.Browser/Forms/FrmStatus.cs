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
    public partial class FrmStatus : Form
    {
        public FrmStatus()
        {
            InitializeComponent();
        }

        public Certificate Certificate { get; set; }
        public string Command { get; set; }
        public string Result { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            textBox1.Text = Command ?? string.Empty;
            textBox2.Text = Result ?? string.Empty;

            var Model = FrmParameters.LoadModel();
            if (Model.ServerUri.StartsWith("wss://"))
                label3.Text = $"WEBSOCKET {Model.ServerUri}";

            else
                label3.Text = $"POST {Model.ServerUri} HTTP/1.1";

            if (Certificate is null)
                label4.Visible = false;

            else
            {
                label4.Text = $"SSL certificate: {Certificate.Subject}, SN: {Certificate.SerialNumber} ({Certificate.KeyIdentifier})";
            }
        }
    }
}
