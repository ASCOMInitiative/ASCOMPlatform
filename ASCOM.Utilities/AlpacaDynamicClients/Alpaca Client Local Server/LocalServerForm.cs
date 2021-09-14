using System.Windows.Forms;

namespace ASCOM.DynamicRemoteClients
{
    public partial class LocalServerForm : Form
    {

        public LocalServerForm()
        {
            InitializeComponent();
            this.Visible = false;
            this.VisibleChanged += LocalServerForm_VisibleChanged;
            this.Load += LocalServerForm_Load;
        }

        private void LocalServerForm_Load(object sender, System.EventArgs e)
        {
            Visible = false;
        }

        private void LocalServerForm_VisibleChanged(object sender, System.EventArgs e)
        {
            if (Visible) Visible = false;
        }
    }
}