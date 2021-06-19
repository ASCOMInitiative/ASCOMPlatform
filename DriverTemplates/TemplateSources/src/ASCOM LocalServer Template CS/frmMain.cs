using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.TEMPLATEDEVICENAME.Server
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