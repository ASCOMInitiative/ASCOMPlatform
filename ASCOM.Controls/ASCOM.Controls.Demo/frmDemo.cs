using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace ASCOM.Controls.Demo
{
    public partial class frmDemo : Form
		{
		public frmDemo()
			{
			InitializeComponent();
			}

		private void frmDemo_Load(object sender, EventArgs e)
			{
			Trace.WriteLine("Loading frmDemo");
			}

		private void btnOK_Click(object sender, EventArgs e)
			{
			this.Close();
			}

		private void btnCancel_Click(object sender, EventArgs e)
			{
			this.Close();
			}
		}
	}
