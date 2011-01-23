using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.TrivialSimulator
	{
	public partial class SwitchStatus : Form
		{
		public SwitchStatus(SwitchCollection switches)
			{
			InitializeComponent();
			this.switchBindingSource.DataSource = switches;
			}
		}
	}
