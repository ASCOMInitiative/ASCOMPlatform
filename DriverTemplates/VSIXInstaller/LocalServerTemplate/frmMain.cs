using System.Windows.Forms;

namespace ASCOM.LocalServer
{
    public partial class FrmMain : Form
	{
		delegate void SetTextCallback(string text);

		public FrmMain()
		{
			InitializeComponent();
			this.ShowInTaskbar = false;
			this.Visible = false;
		}

	}
}