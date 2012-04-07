using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.TEMPLATEDEVICENAME;

namespace ASCOM.TEMPLATEDEVICENAME
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        Profile driverProfile;
        public SetupDialogForm()
        {
            InitializeComponent();
            // Retrieve current values of user settings from the ASCOM Profile 
            driverProfile = new Profile();
            driverProfile.DeviceType = "TEMPLATEDEVICECLASS";
            textBox1.Text = driverProfile.GetValue(TEMPLATEDEVICECLASS.driverID, TEMPLATEDEVICECLASS.comPortProfileName, "", TEMPLATEDEVICECLASS.comPortDefault);
            chkTrace.Checked = Convert.ToBoolean(driverProfile.GetValue(TEMPLATEDEVICECLASS.driverID, TEMPLATEDEVICECLASS.traceLevelProfileName, "", TEMPLATEDEVICECLASS.traceLevelDefault));
        }

        private void SetupDialogForm_FormClosed(Object sender, FormClosedEventArgs e) // Form closed event handler
        {
            driverProfile.Dispose();
            driverProfile = null;
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Persist new values of user settings to the ASCOM profile
            driverProfile.WriteValue(TEMPLATEDEVICECLASS.driverID, TEMPLATEDEVICECLASS.comPortProfileName, textBox1.Text);
            driverProfile.WriteValue(TEMPLATEDEVICECLASS.driverID, TEMPLATEDEVICECLASS.traceLevelProfileName, chkTrace.Checked.ToString());
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
    }
}