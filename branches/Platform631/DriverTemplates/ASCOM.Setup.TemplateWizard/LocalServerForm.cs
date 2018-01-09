using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ASCOM.Setup
{
	public partial class LocalServerForm : Form
	{
		/// <summary>
		/// Format string for ASCOM DeviceId.
		/// </summary>
		private const string csDeviceIdFormat = "ASCOM.{0}.{1}";
		private const string csIdentifierRegex = @"^[a-zA-Z]([a-zA-Z0-9]*)$";
		/// <summary>
		/// The device class for a LocalServer project.
		/// </summary>
		private const string csLocalServerClass = "Server";
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
		public LocalServerForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Handles the Validating event of the txtDeviceName control.
		/// Checks that the typed text is a valid ASCOM DeviceName.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
		/// <remarks>
		/// The device name must start with an alphabetic character and may be followed by any number of alphanumeric
		/// characters. There may be no whitespace or punctuation.
		/// </remarks>
		private void txtDeviceName_Validating(object sender, CancelEventArgs e)
		{
			Regex rxValidateDeviceName = new Regex(csIdentifierRegex);
			if (!rxValidateDeviceName.IsMatch(txtDeviceName.Text))
			{
				e.Cancel = true;
				errorProvider.SetError(txtDeviceName, "Invalid DeviceId. Must begin with a letter and contain only alphanumeric characters");
			}

		}

		/// <summary>
		/// Handles the Validated event of the <see cref="txtDeviceName"/> control
		/// and clears the control's error state.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void txtDeviceName_Validated(object sender, EventArgs e)
		{
			errorProvider.SetError(this.txtDeviceName, String.Empty);
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
		internal static string DeviceClass
		{
			get
			{
				return csLocalServerClass;
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
				if (String.IsNullOrEmpty(txtDeviceName.Text))
				{
					return csDeviceNamePlaceholder;
				}
				return (txtOrganizationName.Text ?? String.Empty) + txtDeviceName.Text;
			}
		}

		public string Namespace
		{
			get
			{
				return String.Format(csNamespaceFormat, DeviceName);
			}
		}
        private void cbDeviceClass_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.lblDeviceId.Text = this.DeviceId;
		}

		/// <summary>
		/// Handles the TextChanged event of the txtDeviceName and txtDeviceName controls.
		/// Sets the value of lblDeviceId based on user input.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void txtDeviceName_TextChanged(object sender, EventArgs e)
		{
			this.lblDeviceId.Text = this.DeviceId;
		}

		/// <summary>
		/// Handles the Click event of the btnCreate control.
		/// Closes the form.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void btnCreate_Click(object sender, EventArgs e)
		{
			// Before dismissing the dialog, run one final validation pass.
			if (!this.ValidateChildren(ValidationConstraints.Enabled))
				return;	// without closing the form.
			this.Close();
		}

		/// <summary>
		/// Handles the Validating event of the txtOrganizationName control.
		/// The value of this control must either be empty, or must contain only alphabetics.
		/// </summary>
		/// <param name="sender">The source of the event, not used.</param>
		/// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
		/// <remarks>
		/// The organization name must either be blank, or contain a sequence of alphabetic characters.
		/// No white space or punctuation is allowed.
		/// </remarks>
		private void txtOrganizationName_Validating(object sender, CancelEventArgs e)
		{
			if (String.IsNullOrEmpty(txtOrganizationName.Text))
				return;
			Regex rxValidateOrgName = new Regex(@"^[a-zA-Z]+$");
			if (!rxValidateOrgName.IsMatch(txtOrganizationName.Text))
			{
				e.Cancel = true;
				errorProvider.SetError(this.txtOrganizationName, "Invalid organization name. Must either be empty, or contain only alphabetic characters.\nBy convention, the first character should be upper case.");
			}
		}

		/// <summary>
		/// Handles the Validated event of the <see cref="txtOrganizationName"/> control
		/// and clears the control's error state.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void txtOrganizationName_Validated(object sender, EventArgs e)
		{
			errorProvider.SetError(txtOrganizationName, String.Empty);
		}
	}
}
