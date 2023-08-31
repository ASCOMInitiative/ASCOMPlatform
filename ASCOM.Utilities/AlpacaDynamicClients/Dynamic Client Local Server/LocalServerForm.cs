using System.Windows.Forms;

namespace ASCOM.LocalServer
{
    public partial class LocalServerForm : Form
    {
        private delegate void SetTextCallback(string text);

        public LocalServerForm()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Visible = false;
        }

    }
}