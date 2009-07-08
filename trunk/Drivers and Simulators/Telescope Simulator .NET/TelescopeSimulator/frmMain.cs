using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.TelescopeSimulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);


        public frmMain()
        {
            InitializeComponent();

        }
        public void DoSetupDialog()
        {
            SetupDialogForm setupForm = new SetupDialogForm();

            setupForm.VersionOneOnly = TelescopeHardware.VersionOneOnly;
            setupForm.CanFindHome = TelescopeHardware.CanFindHome;
            setupForm.CanPark = TelescopeHardware.CanPark;
            setupForm.NumberMoveAxis = TelescopeHardware.NumberMoveAxis;
            setupForm.OnTop = TelescopeHardware.OnTop;
            setupForm.CanPulseGuide = TelescopeHardware.CanPulseGuide;
            setupForm.CanSetEquatorialRates = TelescopeHardware.CanSetEquatorialRates;
            setupForm.CanSetGuideRates = TelescopeHardware.CanSetGuideRates;
            setupForm.CanSetPark = TelescopeHardware.CanSetPark;
            setupForm.CanSetPierSide = TelescopeHardware.CanSetPierSide;
            setupForm.CanSetTracking = TelescopeHardware.CanSetTracking;
            setupForm.CanSlew = TelescopeHardware.CanSlew;
            setupForm.CanAlignmentMode = TelescopeHardware.CanAlignmentMode;
            setupForm.AlignmentMode = TelescopeHardware.AlignmentMode;
            setupForm.CanOptics = TelescopeHardware.CanOptics;
            setupForm.ApertureArea = TelescopeHardware.ApertureArea;
            setupForm.ApertureDiameter = TelescopeHardware.ApertureDiameter;
            setupForm.FocalLength = TelescopeHardware.FocalLength;
            setupForm.CanSlewAltAz = TelescopeHardware.CanSlewAltAz;
            setupForm.CanSlewAltAzAsync = TelescopeHardware.CanSlewAltAzAsync;
            setupForm.CanSlewAsync = TelescopeHardware.CanSlewAsync;
            setupForm.CanSync = TelescopeHardware.CanSync;
            setupForm.CanSyncAltAz = TelescopeHardware.CanSyncAltAz;
            setupForm.CanUnpark = TelescopeHardware.CanUnpark;
            setupForm.CanAltAz = TelescopeHardware.CanAltAz;
            setupForm.CanDateTime = TelescopeHardware.CanDateTime;
            setupForm.CanDoesRefraction = TelescopeHardware.CanDoesRefraction;
            setupForm.CanEquatorial = TelescopeHardware.CanEquatorial;
            setupForm.CanLatLongElev = TelescopeHardware.CanLatLongElev;
            setupForm.CanPierSide = TelescopeHardware.CanPierSide;
            setupForm.CanDualAxisPulseGuide = TelescopeHardware.CanDualAxisPulseGuide;
            setupForm.AutoTrack = TelescopeHardware.AutoTrack;
            setupForm.DisconnectOnPark = TelescopeHardware.DisconnectOnPark;
            setupForm.Refraction = TelescopeHardware.Refraction;
            setupForm.CanTrackingRates = TelescopeHardware.CanTrackingRates;
            setupForm.CanSiderealTime = TelescopeHardware.CanSiderealTime;
            setupForm.NoCoordinatesAtPark = TelescopeHardware.NoCoordinatesAtPark;
            setupForm.EquatorialSystem = TelescopeHardware.EquatorialSystem;
            setupForm.Elevation = TelescopeHardware.Elevation;
            setupForm.Latitude = TelescopeHardware.Latitude;
            setupForm.Longitude = TelescopeHardware.Longitude;
            setupForm.MaximumSlewRate = TelescopeHardware.MaximumSlewRate;

            DialogResult ans = setupForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {
                TelescopeHardware.VersionOneOnly = setupForm.VersionOneOnly;
                TelescopeHardware.CanFindHome = setupForm.CanFindHome;
                TelescopeHardware.CanPark = setupForm.CanPark;
                TelescopeHardware.NumberMoveAxis = setupForm.NumberMoveAxis;
                TelescopeHardware.OnTop = setupForm.OnTop;
                TelescopeHardware.CanPulseGuide = setupForm.CanPulseGuide;
                TelescopeHardware.CanSetEquatorialRates = setupForm.CanSetEquatorialRates;
                TelescopeHardware.CanSetGuideRates = setupForm.CanSetGuideRates;
                TelescopeHardware.CanSetPark = setupForm.CanSetPark;
                TelescopeHardware.CanSetPierSide = setupForm.CanSetPierSide;
                TelescopeHardware.CanSetTracking = setupForm.CanSetTracking;
                TelescopeHardware.CanSlew = setupForm.CanSlew;
                TelescopeHardware.CanAlignmentMode = setupForm.CanAlignmentMode;
                TelescopeHardware.AlignmentMode = setupForm.AlignmentMode;
                TelescopeHardware.ApertureArea = setupForm.ApertureArea;
                TelescopeHardware.ApertureDiameter = setupForm.ApertureDiameter;
                TelescopeHardware.FocalLength = setupForm.FocalLength;
                TelescopeHardware.CanSlewAltAzAsync = setupForm.CanSlewAltAzAsync;
                TelescopeHardware.CanSlewAltAz = setupForm.CanSlewAltAz;
                TelescopeHardware.CanSync = setupForm.CanSync;
                TelescopeHardware.CanSyncAltAz = setupForm.CanSyncAltAz;
                TelescopeHardware.CanUnpark = setupForm.CanUnpark;
                TelescopeHardware.CanAltAz = setupForm.CanAltAz;
                TelescopeHardware.CanDateTime = setupForm.CanDateTime;
                TelescopeHardware.CanDoesRefraction = setupForm.CanDoesRefraction;
                TelescopeHardware.CanEquatorial = setupForm.CanEquatorial;
                TelescopeHardware.CanLatLongElev = setupForm.CanLatLongElev;
                TelescopeHardware.CanPierSide = setupForm.CanPierSide;
                TelescopeHardware.CanDualAxisPulseGuide = setupForm.CanDualAxisPulseGuide;
                TelescopeHardware.AutoTrack = setupForm.AutoTrack;
                TelescopeHardware.DisconnectOnPark = setupForm.DisconnectOnPark;
                TelescopeHardware.Refraction = setupForm.Refraction;
                TelescopeHardware.CanTrackingRates = setupForm.CanTrackingRates;
                TelescopeHardware.CanSiderealTime = setupForm.CanSiderealTime;
                TelescopeHardware.NoCoordinatesAtPark = setupForm.NoCoordinatesAtPark;
                TelescopeHardware.EquatorialSystem = setupForm.EquatorialSystem;
                TelescopeHardware.Elevation = setupForm.Elevation;
                TelescopeHardware.Latitude = setupForm.Latitude;
                TelescopeHardware.Longitude = setupForm.Longitude;
                TelescopeHardware.MaximumSlewRate = setupForm.MaximumSlewRate;

                this.TopMost = setupForm.OnTop;
            }

            setupForm.Dispose();
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            DoSetupDialog();
        }

        private void buttonTraffic_Click(object sender, EventArgs e)
        {
            SharedResources.TrafficForm.Show();
        }
        public double SiderealTime
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetLstText);
                string text = AstronomyFunctions.ConvertDoubleToHMS(value);
                try{this.Invoke(setText, text);}
                catch { }
                
           
            }
        }
        private void SetLstText(string text)
        {
            labelLst.Text = text;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            TelescopeHardware.Start();
        }
    }
}