using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            Version version = new Version(Application.ProductVersion);
            labelVersion.Text = "ASCOM Telescope Simulator .NET " + string.Format("Version {0}.{1}.{2}", version.Major, version.Minor, version.Build);
            TimeZone localZone = TimeZone.CurrentTimeZone;
            labelTime.Text = "Time zone is " + localZone.StandardName;
            if (localZone.IsDaylightSavingTime(DateTime.Now))
            {
                labelTime.Text += " (currently DST)"; 
            }

            //This is a little silly, but you cant build a combobox with values without a datasource
            //I would get confused trying to decifer the original logic if I didnt set the values the same
            Collection<ComboBoxItem> items = new Collection<ComboBoxItem>();
            ComboBoxItem item1 = new ComboBoxItem();
            item1.Display = "Local";
            item1.Value = "1";

            ComboBoxItem item2 = new ComboBoxItem();
            item2.Display = "B1950";
            item2.Value = "4";

            ComboBoxItem item3 = new ComboBoxItem();
            item3.Display = "J2000";
            item3.Value = "2";

            ComboBoxItem item4 = new ComboBoxItem();
            item4.Display = "J2050";
            item4.Value = "3";

            ComboBoxItem item5 = new ComboBoxItem();
            item5.Display = "Other";
            item5.Value = "0";


            items.Add(item1);
            items.Add(item2);
            items.Add(item3);
            items.Add(item4);
            items.Add(item5);

            comboBoxEquatorialSystem.DataSource = items;
            comboBoxEquatorialSystem.DisplayMember = "Display";
            comboBoxEquatorialSystem.ValueMember = "Value";
               // Local,1
               // B1950,4
               // J2000,2
               // J2050,3
               // Other,0
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
        public int EquatorialSystem
        {
            get { return int.Parse(comboBoxEquatorialSystem.SelectedValue.ToString()); }
            set { comboBoxEquatorialSystem.SelectedValue = value.ToString(); }
        }
        public double Elevation
        {
            get 
            {
                double elevation;
                double.TryParse(textBoxElevation.Text, out elevation);
                return elevation;
                
            }
            set { textBoxElevation.Text = value.ToString(); }
        }
        public double Latitude
        {
            get
            {
                double lat=0;
                try
                {
                    lat = double.Parse(textBoxLatitudeDegrees.Text) + double.Parse(textBoxLatitudeMinutes.Text) / 60;
                    if (comboBoxLatitude.SelectedItem.ToString() == "S") { lat = -lat; }
                }
                catch { }
                return lat;
            }
            set
            {
                if (value < 0)
                {
                    comboBoxLatitude.SelectedIndex = 1;
                    value = -value;
                }
                else
                {
                    comboBoxLatitude.SelectedIndex = 0;
                }

                textBoxLatitudeDegrees.Text = ((int)value).ToString("00");
                textBoxLatitudeMinutes.Text = ((value - (int)value) * 60).ToString("00.00");
            }
        }
        public double Longitude
        {
            get
            {
                double log = 0;
                try
                {
                    log = double.Parse(textBoxLongitudeDegrees.Text) + double.Parse(textBoxLongitudeMinutes.Text) / 60;
                    if (comboBoxLongitude.SelectedItem.ToString() == "W") { log = -log; }
                }
                catch { }
                return log;
            }
            set
            {
                if (value < 0)
                {
                    value = -value;
                    comboBoxLongitude.SelectedIndex = 1;
                }else{
                    comboBoxLongitude.SelectedIndex = 0;
                }

                textBoxLongitudeDegrees.Text = ((int)value).ToString("000");
                textBoxLongitudeMinutes.Text = ((value - (int)value) * 60).ToString("00.00");
            }
        }

        public int MaximumSlewRate
        {
            get { return (int)numericUpDownSlewRate.Value; }
            set { numericUpDownSlewRate.Value = value; }
        }
        public bool AutoTrack
        {
            get { return checkBoxAutoTrack.Checked; }
            set { checkBoxAutoTrack.Checked = value; }
        }
        public bool NoCoordinatesAtPark
        {
            get { return checkBoxNoCoordinatesAtPark.Checked; }
            set { checkBoxNoCoordinatesAtPark.Checked = value; }
        }
        public bool DisconnectOnPark
        {
            get { return checkBoxDisconnectOnPark.Checked; }
            set { checkBoxDisconnectOnPark.Checked = value; }
        }
        public bool OnTop
        {
            get { return checkBoxOnTop.Checked; }
            set { checkBoxOnTop.Checked = value; }
        }
        public bool Refraction
        {
            get { return checkBoxRefraction.Checked; }
            set { checkBoxRefraction.Checked = value; }
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
        public bool CanDualAxisPulseGuide
        {
            get { return checkBoxCanDualAxisPulseGuide.Checked; }
            set { checkBoxCanDualAxisPulseGuide.Checked = value; }

        }
        public bool CanSetEquatorialRates
        {
            get { return checkBoxCanSetEquatorialRates.Checked; }
            set { checkBoxCanSetEquatorialRates.Checked = value; }
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
        public bool CanPierSide
        {
            get { return checkBoxCanPierSide.Checked; }
            set { checkBoxCanPierSide.Checked = value; }

        }
        public bool CanTrackingRates
        {
            get { return checkBoxCanTrackingRates.Checked; }
            set { checkBoxCanTrackingRates.Checked = value; }

        }
        public bool CanSetTracking
        {
            get { return checkBoxCanSetTracking.Checked; }
            set { checkBoxCanSetTracking.Checked = value; }

        }
        public bool CanSlew
        {
            get { return checkBoxCanSlew.Checked; }
            set { checkBoxCanSlew.Checked = value; }

        }
        public bool CanSync
        {
            get { return checkBoxCanSync.Checked; }
            set { checkBoxCanSync.Checked = value; }

        }
        public bool CanSlewAsync
        {
            get { return checkBoxCanSlewAsync.Checked; }
            set { checkBoxCanSlewAsync.Checked = value; }

        }
        public bool CanSlewAltAz
        {
            get { return checkBoxCanSlewAltAz.Checked; }
            set { checkBoxCanSlewAltAz.Checked = value; }

        }
        public bool CanAltAz
        {
            get { return checkBoxCanAltAz.Checked; }
            set { checkBoxCanAltAz.Checked = value; }

        }
        public bool CanSyncAltAz
        {
            get { return checkBoxCanSyncAltAz.Checked; }
            set { checkBoxCanSyncAltAz.Checked = value; }

        }
        public bool CanSlewAltAzAsync
        {
            get { return checkBoxCanSlewAltAzAsync.Checked; }
            set { checkBoxCanSlewAltAzAsync.Checked = value; }

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
        public bool CanUnpark
        {
            get { return checkBoxCanUnpark.Checked; }
            set { checkBoxCanUnpark.Checked = value; }

        }
        public bool CanDateTime
        {
            get { return checkBoxCanDateTime.Checked; }
            set { checkBoxCanDateTime.Checked = value; }

        }
        public bool CanDoesRefraction
        {
            get { return checkBoxCanDoesRefraction.Checked; }
            set { checkBoxCanDoesRefraction.Checked = value; }

        }
        public bool CanLatLongElev
        {
            get { return checkBoxCanLatLongElev.Checked; }
            set { checkBoxCanLatLongElev.Checked = value; }

        }
        public bool CanEquatorial
        {
            get { return checkBoxCanEquatorial.Checked; }
            set { checkBoxCanEquatorial.Checked = value; }

        }
        public bool CanSiderealTime
        {
            get { return checkBoxCanSiderealTime.Checked; }
            set { checkBoxCanSiderealTime.Checked = value; }

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
                double aperture;
                double.TryParse(textBoxAperture.Text, out aperture);
                return aperture;


            }
            set { textBoxAperture.Text = value.ToString(); }
        }
        public double FocalLength
        {
            get
            {
                double focal;
                double.TryParse(textBoxFocalLength.Text, out focal);
                return focal;


            }
            set { textBoxFocalLength.Text = value.ToString(); }
        }
        public ASCOM.DeviceInterface.AlignmentModes AlignmentMode
        {
            get
            {
                if (radioButtonAltAzimuth.Checked)
                { return ASCOM.DeviceInterface.AlignmentModes.algAltAz; }
                if (radioButtonEquatorial.Checked)
                { return ASCOM.DeviceInterface.AlignmentModes.algPolar; }
                if (radioButtonGermanEquatorial.Checked)
                { return ASCOM.DeviceInterface.AlignmentModes.algGermanPolar; }
                return ASCOM.DeviceInterface.AlignmentModes.algGermanPolar;
            }
            set
            {
                switch (value)
                {
                    case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
                        radioButtonAltAzimuth.Checked = true;
                        break;
                    case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
                        radioButtonGermanEquatorial.Checked = true;
                        break;
                    case ASCOM.DeviceInterface.AlignmentModes.algPolar:
                        radioButtonEquatorial.Checked = true;
                        break;
                }
            }
        }
        #endregion

        private void buttonSetParkPosition_Click(object sender, EventArgs e)
        {
            TelescopeHardware.ParkAltitude = TelescopeHardware.Altitude;
            TelescopeHardware.ParkAzimuth = TelescopeHardware.Azimuth;
        }

    }

    [ComVisible(false)]
    class ComboBoxItem
    {
        public ComboBoxItem()
        { }
        private string m_Display;
        private string m_Value;

        public string Display
        {
            get {return m_Display;}
            set { m_Display = value; }
        }
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
    }
}