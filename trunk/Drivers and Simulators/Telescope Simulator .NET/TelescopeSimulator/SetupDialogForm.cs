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
        
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
         
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
        public bool CanSetPierSide
        {
            get { return checkBoxCanSetPierSide.Checked; }
            set { checkBoxCanSetPierSide.Checked = value; }

        }
        public bool CanSetTracking
        {
            get { return checkBoxCanTrackingRates.Checked; }
            set { checkBoxCanTrackingRates.Checked = value; }

        }
        public bool CanSlew
        {
            get { return checkBoxCanSlew.Checked; }
            set { checkBoxCanSlew.Checked = value; }

        }
        public bool CanOptics
        {
            get { return checkBoxCanOptics.Checked; }
            set { checkBoxCanOptics.Checked = value; }

        }
        public bool CanAlignmentMode
        {
            get { return checkBoxCanAlignmentMode.Checked; }
            set { checkBoxCanAlignmentMode.Checked = value; }

        }
        public double ApertureArea
        {
            get
            {
                double area;
                double.TryParse(textBoxApertureArea.Text, out area);
                return area; 


            }
            set { textBoxApertureArea.Text = value.ToString(); }
        }
        public double ApertureDiameter
        {
            get
            {
                double area;
                double.TryParse(textBoxAperture.Text, out area);
                return area;


            }
            set { textBoxAperture.Text = value.ToString(); }
        }
        public int AlignmentMode
        {
            get
            {
                if (radioButtonAltAzimuth.Checked)
                {return 0;}
                if (radioButtonEquatorial.Checked)
                {return 2;}
                if (radioButtonGermanEquatorial.Checked)
                { return 1; }
                return 1;
            }
            set
            {
                switch (value)
                {
                    case 0:
                        radioButtonAltAzimuth.Checked = true;
                        break;
                    case 1:
                        radioButtonGermanEquatorial.Checked = true;
                        break;
                    case 2:
                        radioButtonEquatorial.Checked = true;
                        break;
                }
            }
        }
        #endregion

    }
}