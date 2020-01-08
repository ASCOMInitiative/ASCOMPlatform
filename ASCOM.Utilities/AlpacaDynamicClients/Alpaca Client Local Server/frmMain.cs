using System.Windows.Forms;

namespace ASCOM.DynamicRemoteClients
{
    public partial class FrmMain : Form
    {
        delegate void SetTextCallback(string text);

        public FrmMain()
        {
            InitializeComponent();
        }

    }
}