using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;

namespace ASCOM.Simulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            labelVersion.Text = string.Format(CultureInfo.CurrentCulture, "ASCOM Telescope Simulator .NET Version {0}.{1}.{2}", version.Major, version.Minor, version.Build);
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

        private void CmdOK_Click(object sender, EventArgs e)
        {

        }

        private void CmdCancel_Click(object sender, EventArgs e)
        {

        }

        private void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://ascom-standards.org/");
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
            get { return int.Parse(comboBoxEquatorialSystem.SelectedValue.ToString(), CultureInfo.CurrentCulture); }
            set { comboBoxEquatorialSystem.SelectedValue = value.ToString(CultureInfo.CurrentCulture); }
        }
        public double Elevation
        {
            get
            {
                if (!double.TryParse(textBoxElevation.Text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double elevation))
                    elevation = double.NaN;
                return elevation;

            }
            set { textBoxElevation.Text = value.ToString(CultureInfo.CurrentCulture); }
        }
        public double Latitude
        {
            get
            {
                double lat = 0;
                try
                {
                    lat = double.Parse(textBoxLatitudeDegrees.Text, CultureInfo.CurrentCulture) + double.Parse(textBoxLatitudeMinutes.Text, CultureInfo.CurrentCulture) / 60;
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

                textBoxLatitudeDegrees.Text = ((int)value).ToString("00", CultureInfo.CurrentCulture);
                textBoxLatitudeMinutes.Text = ((value - (int)value) * 60).ToString("00.00", CultureInfo.CurrentCulture);
            }
        }
        public double Longitude
        {
            get
            {
                double log = 0;
                try
                {
                    log = double.Parse(textBoxLongitudeDegrees.Text, CultureInfo.CurrentCulture) + double.Parse(textBoxLongitudeMinutes.Text, CultureInfo.CurrentCulture) / 60;
                    if (comboBoxLongitude.SelectedItem.ToString().ToUpperInvariant() == "W") { log = -log; }
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
                }
                else
                {
                    comboBoxLongitude.SelectedIndex = 0;
                }

                textBoxLongitudeDegrees.Text = ((int)value).ToString("000", CultureInfo.CurrentCulture);
                textBoxLongitudeMinutes.Text = ((value - (int)value) * 60).ToString("00.00", CultureInfo.CurrentCulture);
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
            set { checkBoxVersionOne.Checked = value; }
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
            get { return int.Parse(comboBoxNumberMoveAxis.SelectedItem.ToString(), CultureInfo.CurrentCulture); }
            set { comboBoxNumberMoveAxis.SelectedItem = value.ToString(CultureInfo.CurrentCulture); }
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
        public bool CanDestinationSideOfPier
        {
            get { return chkCanDestinationSideofPier.Checked; }
            set { chkCanDestinationSideofPier.Checked = value; }

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
                if (!double.TryParse(textBoxApertureArea.Text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double area))
                    area = -1;  // indicates an error - for now
                return area;
            }
            set { textBoxApertureArea.Text = value.ToString(CultureInfo.CurrentCulture); }
        }

        public double ApertureDiameter
        {
            get
            {
                if (!double.TryParse(textBoxAperture.Text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double aperture))
                    aperture = -1;
                return aperture;
            }
            set { textBoxAperture.Text = value.ToString(CultureInfo.CurrentCulture); }
        }

        public double FocalLength
        {
            get
            {
                if (!double.TryParse(textBoxFocalLength.Text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double focal))
                    focal = -1;
                return focal;
            }
            set { textBoxFocalLength.Text = value.ToString(CultureInfo.CurrentCulture); }
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

        public bool NoSyncPastMeridian
        {
            get { return checkBoxNoSyncPastMeridian.Checked; }
            set { checkBoxNoSyncPastMeridian.Checked = value; }
        }
        #endregion

        private void ButtonSetParkPosition_Click(object sender, EventArgs e)
        {
            TelescopeHardware.ParkAltitude = TelescopeHardware.Altitude;
            TelescopeHardware.ParkAzimuth = TelescopeHardware.Azimuth;
        }


        private void ButtonParkHomeAndStartupOptions_Click(object sender, EventArgs e)
        {
            Form formPark = new ParkHomeAndStartupForm();
            formPark.ShowDialog();
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            // Bring the setup dialogue to the front of the screen
            TopMost = true;
            Focus();
            BringToFront();
            TopMost = false;

            // this bizarre sequence seems to be required to bring the setup dialog to the front.
            //this.TopMost = true;
            //this.Activate();
            //this.BringToFront();
            //this.TopMost = false;
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
            get { return m_Display; }
            set { m_Display = value; }
        }
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
    }
}