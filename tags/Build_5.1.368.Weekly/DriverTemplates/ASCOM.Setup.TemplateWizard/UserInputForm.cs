using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ASCOM.Setup
{
	public partial class UserInputForm : Form
	{
		/// <summary>
		/// Format string for ASCOM DeviceID.
		/// </summary>
		private const string csDeviceIdFormat = "ASCOM.{0}.{1}";
		/// <summary>
		/// Format string for driver namespace.
		/// </summary>
		private const string csNamespaceFormat = "ASCOM.{0}";
		/// <summary>
		/// Placeholder text used for null or invalid DeviceName.
		/// </summary>
		private const string csDeviceNamePlaceholder = "<DeviceName>";
		/// <summary>
		/// Placeholder text used for null or invalid DeviceClass.
		/// </summary>
		private const string csDeviceClassPlaceholder = "<DeviceClass>";
		public UserInputForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Handles the Validating event of the txtDeviceName control.
		/// Checks that the typed text is a valid ASCOM DeviceName.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
		private void txtDeviceName_Validating(object sender, CancelEventArgs e)
		{
			Regex rxValidateDeviceName = new Regex(@"^[a-zA-Z]\w*$");
			if (!rxValidateDeviceName.IsMatch(this.txtDeviceName.Text))
			{
				errorProvider.SetError(this.txtDeviceName, "Invalid DeviceID. Must begin with a letter and contain only alphanumeric characters");
				e.Cancel = true;
			}

		}

		/// <summary>
		/// Handles the Validated event of the txtDeviceName control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void txtDeviceName_Validated(object sender, EventArgs e)
		{
			errorProvider.Clear();
		}

		/// <summary>
		/// Gets the full ASCOM device id (COM ProgID), in the format
		/// ASCOM.{DeviceName}.{DeviceClass}
		/// </summary>
		/// <value>The full ASCOM device id (COM ProgID).</value>
		internal string DeviceId
		{
			get
			{
				return String.Format(csDeviceIdFormat, DeviceName, DeviceClass);
			}
		}
		/// <summary>
		/// Gets the ASCOM DeviceClass of the device.
		/// </summary>
		/// <value>The ASCOM device class.</value>
		internal string DeviceClass
		{
			get
			{
				return (string)this.cbDeviceClass.SelectedItem ?? csDeviceClassPlaceholder;
			}
		}
		/// <summary>
		/// Gets the ASCOM DeviceName of the device.
		/// </summary>
		/// <value>The name of the device.</value>
		internal string DeviceName
		{
			get
			{
				if (String.IsNullOrEmpty(this.txtDeviceName.Text))
				{
					return csDeviceNamePlaceholder;
				}
				return this.txtDeviceName.Text;
			}
		}

		public string Namespace
		{
			get
			{
				return String.Format(csNamespaceFormat, this.DeviceName);
			}
		}
        private void cbDeviceClass_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.lblDeviceId.Text = this.DeviceId;
		}

		private void txtDeviceName_TextChanged(object sender, EventArgs e)
		{
			this.lblDeviceId.Text = this.DeviceId;
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
