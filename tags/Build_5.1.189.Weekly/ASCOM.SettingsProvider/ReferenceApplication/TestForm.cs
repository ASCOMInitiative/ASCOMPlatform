using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ASCOM.HelperNET;

namespace ASCOM.SettingsProvider.ReferenceApplication
	{
	public partial class TestForm : Form
		{
		private string DeviceClass
			{
			get;
			set;
			}
		private string DeviceID
			{
			get;
			set;
			}
		private List<string> DriverTypes = new List<string>()
		{
			"Camera",
			"Dome",
			"Focuser",
			"Rotator",
			"Switch",
			"Telescope"
		};
		public TestForm()
			{
			InitializeComponent();
			cbDriverType.Items.AddRange(DriverTypes.ToArray());
			}

		private void btnSave_Click(object sender, EventArgs e)
			{
			Properties.Settings.Default.Save();
			}

		private void btnChoose_Click(object sender, EventArgs e)
			{
			string selectedDeviceType = cbDriverType.Text;
			Chooser chooser = new Chooser();
			chooser.DeviceType = selectedDeviceType;
			this.DeviceClass = cbDriverType.Text;
			this.DeviceID = chooser.Choose(lblSelectedDeviceID.Text);
			lblSelectedDeviceID.Text = this.DeviceID;
			Properties.Settings.Default.Save();
			}

		private void btnLoad_Click(object sender, EventArgs e)
			{

			}
		}
	}
