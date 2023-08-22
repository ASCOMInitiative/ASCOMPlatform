using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);

        public frmMain()
        {
            InitializeComponent();
        }

    }
}