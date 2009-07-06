using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.TelescopeSimulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BrowseToAscom(object sender, EventArgs e)
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

        #region Properties for Settings
        public bool OnTop
        {
            get { return checkBoxOnTop.Checked; }
            set { checkBoxOnTop.Checked = value; }
        }
        public bool VersionOneOnly
        {
            get { return checkBoxVersionOne.Checked; }
            set { checkBoxVersionOne.Checked = value;}
        }
        public bool CanFindHome
        {
            get { return checkBoxCanFindHome.Checked; }
            set { checkBoxCanFindHome.Checked = value; }

        }
        public bool CanPark
        {
            get { return checkBoxCanPark.Checked; }
            set { checkBoxCanPark.Checked = value; }

        }
        public int NumberMoveAxis
        {
            get { return int.Parse(comboBoxNumberMoveAxis.SelectedItem.ToString()); }
            set { comboBoxNumberMoveAxis.SelectedItem = value.ToString(); }
        }

        public bool CanPulseGuide
        {
            get { return checkBoxCanPulseGuide.Checked; }
            set { checkBoxCanPulseGuide.Checked = value; }

        }
        public bool CanSetEquatorialRates
        {
            get { return checkBoxCanTrackingRates.Checked; }
            set { checkBoxCanTrackingRates.Checked = value; }
        }
        public bool CanSetGuideRates
        {
            get { return checkBoxCanGuideRates.Checked; }
            set { checkBoxCanGuideRates.Checked = value; }
        }
        public bool CanSetPark
        {
            get { return checkBoxCanSetParkPosition.Checked; }
            set { checkBoxCanSetParkPosition.Checked = value; }

        }
        public bool CanSetSideOfPier
        {
            get { return checkBoxCanSetSideOfPier.Checked; }
            set { checkBoxCanSetSideOfPier.Checked = value; }

        }
        #endregion

    }
}