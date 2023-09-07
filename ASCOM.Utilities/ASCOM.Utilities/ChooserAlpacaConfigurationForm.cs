using System;
using System.Windows.Forms;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{
    public partial class ChooserAlpacaConfigurationForm
    {
        private ChooserForm chooserForm;

        /// <summary>
    /// Initialiser enabling the Chooser to pass in a reference to itself so that it's variables can be accessed
    /// </summary>
    /// <param name="chooser"></param>
        internal ChooserAlpacaConfigurationForm(ChooserForm chooser)
        {

            // This call is required by the designer.
            InitializeComponent();

            // Save the supplied reference to the Chooser for use in the form load and OK button events
            chooserForm = chooser;

        }

        /// <summary>
    /// Form load event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        private void ChooserAlpacaConfigurationForm_Load(object sender, EventArgs e)
        {

            // Initialise controls to the values held in the Chooser
            NumDiscoveryBroadcasts.Value = Convert.ToDecimal(chooserForm.AlpacaNumberOfBroadcasts);
            NumDiscoveryDuration.Value = Convert.ToDecimal(chooserForm.AlpacaTimeout);
            NumDiscoveryIpPort.Value = Convert.ToDecimal(chooserForm.AlpacaDiscoveryPort);
            ChkDNSResolution.Checked = chooserForm.AlpacaDnsResolution;
            ChkListAllDiscoveredDevices.Checked = chooserForm.AlpacaShowDiscoveredDevices;
            ChkShowDeviceDetails.Checked = chooserForm.AlpacaShowDeviceDetails;
            NumExtraChooserWidth.Value = Convert.ToDecimal(chooserForm.AlpacaChooserIncrementalWidth);
            ChkShowCreateNewAlpacaDriverMessage.Checked = !GetBool(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT);
            ChkMultiThreadedChooser.Checked = chooserForm.AlpacaMultiThreadedChooser;
            // Set the IP v4 / v6 radio boxes
            if (chooserForm.AlpacaUseIpV4 & chooserForm.AlpacaUseIpV6) // // Both Then IPv4 And v6 are enabled so Set the "both" button
            {
                RadIpV4AndV6.Checked = true;
            }
            else // Only one of v4 Or v6 Is enabled so set accordingly 
            {
                RadIpV4.Checked = chooserForm.AlpacaUseIpV4;
                RadIpV6.Checked = chooserForm.AlpacaUseIpV6;
            }

        }

        /// <summary>
    /// OK button event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        private void BtnOK_Click(object sender, EventArgs e)
        {

            // User clicked OK so save the new values to the Chooser's variables 
            chooserForm.AlpacaNumberOfBroadcasts = Convert.ToInt32(NumDiscoveryBroadcasts.Value);
            chooserForm.AlpacaTimeout = Convert.ToInt32(NumDiscoveryDuration.Value);
            chooserForm.AlpacaDiscoveryPort = Convert.ToInt32(NumDiscoveryIpPort.Value);
            chooserForm.AlpacaDnsResolution = ChkDNSResolution.Checked;
            chooserForm.AlpacaShowDiscoveredDevices = ChkListAllDiscoveredDevices.Checked;
            chooserForm.AlpacaShowDeviceDetails = ChkShowDeviceDetails.Checked;
            chooserForm.AlpacaChooserIncrementalWidth = Convert.ToInt32(NumExtraChooserWidth.Value);
            SetName(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, (!ChkShowCreateNewAlpacaDriverMessage.Checked).ToString());
            chooserForm.AlpacaMultiThreadedChooser = ChkMultiThreadedChooser.Checked;

            // Set the IP v4 And v6 variables as necessary
            if (RadIpV4.Checked)  // The Then IPv4 radio button Is checked so Set the IP v4 And IP v6 variables accordingly
            {
                chooserForm.AlpacaUseIpV4 = true;
                chooserForm.AlpacaUseIpV6 = false;
            }

            if (RadIpV6.Checked)  // The Then IPv6 radio button Is checked so Set the IP v4 And IP v6 variables accordingly
            {
                chooserForm.AlpacaUseIpV4 = false;
                chooserForm.AlpacaUseIpV6 = true;
            }

            if (RadIpV4AndV6.Checked) // The Then IPv4 And IPV6 radio button Is checked so Set the IP v4 And IP v6 variables accordingly
            {
                chooserForm.AlpacaUseIpV4 = true;
                chooserForm.AlpacaUseIpV6 = true;
            }

            // Indicate success so that the Chooser can persist the values
            DialogResult = DialogResult.OK;
            Close();

        }

        /// <summary>
    /// Cancel button event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {

            // User clicked Cancel so changes will not be made to the Chooser variables nor will they be persisted
            DialogResult = DialogResult.Cancel;
            Close();

        }
    }
}