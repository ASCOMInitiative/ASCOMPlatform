using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ASCOM.Controls;
using System.Diagnostics;

namespace ASCOM.Simulator
{
    public partial class FrmMain : Form
    {
        private const double guideRate = 15.0 / 3600.0;       // sidereal - more or less

        delegate void SetTextCallback(string text);

        private Utilities.Util util = new ASCOM.Utilities.Util();

        public FrmMain()
        {
            InitializeComponent(); 
            this.BringToFront();
            //this.BackColor = Color.Brown;
        }

        public void DoSetupDialog()
        {
            using (SetupDialogForm setupForm = new SetupDialogForm())
            {
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
                this.BringToFront();
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
            }
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            DoSetupDialog();
            SetSlewButtons();
        }

        private void buttonTraffic_Click(object sender, EventArgs e)
        {
            SharedResources.TrafficForm.Show();
        }

        private void SetSlewButtons()
        {
            if (TelescopeHardware.AlignmentMode == DeviceInterface.AlignmentModes.algAltAz)
            {
                buttonSlewUp.Text = "U";
                buttonSlewDown.Text = "D";
                buttonSlewRight.Text = "R";
                buttonSlewLeft.Text = "L";
            }
            else if (TelescopeHardware.SouthernHemisphere)
            {
                buttonSlewUp.Text = "S";
                buttonSlewDown.Text = "N";
                buttonSlewRight.Text = "E";
                buttonSlewLeft.Text = "W";
            }
            else
            {
                buttonSlewUp.Text = "N";
                buttonSlewDown.Text = "S";
                buttonSlewRight.Text = "E";
                buttonSlewLeft.Text = "W";
            }
        }


        public void SiderealTime(double value)
        {
                SetTextCallback setText = new SetTextCallback(SetLstText);
                string text = util.HoursToHMS(value);
                try{this.Invoke(setText, text);}
                catch { }
        }

        public void RightAscension(double value)
        {
                SetTextCallback setText = new SetTextCallback(SetRaText);
                string text = util.HoursToHMS(value);
                try { this.Invoke(setText, text); }
                catch { }
        }

        public void Declination(double value)
        {
                SetTextCallback setText = new SetTextCallback(SetDecText);
                string text = util.DegreesToDMS(value);
                try { this.Invoke(setText, text); }
                catch { }
        }

        public void Altitude(double value)
        {
                SetTextCallback setText = new SetTextCallback(SetAltitudeText);
                string text = util.DegreesToDMS(value);
                try { this.Invoke(setText, text); }
                catch { }
        }

        public void Azimuth(double value)
        {
                SetTextCallback setText = new SetTextCallback(SetAzimuthText);
                string text = util.DegreesToDMS(value);
                try { this.Invoke(setText, text); }
                catch { }
        }

        public void ParkButton(string value)
        {
                SetTextCallback setText = new SetTextCallback(SetParkButtonText);
                try { this.Invoke(setText, value); }
                catch { }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SetSlewButtons();
            TelescopeHardware.Start();
        }

        #region Thread Safe Callback Functions
        private void SetLstText(string text)
        {
            labelLst.Text = text;
        }
        private void SetRaText(string text)
        {
            labelRa.Text = text;
        }
        private void SetDecText(string text)
        {
            labelDec.Text = text;
        }
        private void SetAltitudeText(string text)
        {
            labelAlt.Text = text;
        }
        private void SetAzimuthText(string text)
        {
            labelAz.Text = text;
        }

        private void SetParkButtonText(string text)
        {
            buttonUnpark.Text = text;
        }

        #endregion

        #region slew/guide control using buttons

        private void SetPulseGuideParms(double guideRateDec, double guideRateRa)
		{
            // stop an axis slew if that's what is enabled
            if (this.radioButtonMoveAxis.Checked)
            {
                TelescopeHardware.SlewDirection = SlewDirection.SlewNone;
                return;
            }
            Debug.Assert(guideRateDec != 0.0 || guideRateRa != 0.0);
			Debug.Assert( !( guideRateDec != 0.0 && guideRateRa != 0.0 ) );

			if ( guideRateDec != 0.0 )
			{
				TelescopeHardware.GuideRateDeclination = guideRateDec;
				TelescopeHardware.isPulseGuidingDec = true;
                TelescopeHardware.guideDuration.Y = GuideDuration();
			}
			else
			{
                if (TelescopeHardware.SouthernHemisphere)
                {
                    guideRateRa *= -1;
                }
				TelescopeHardware.GuideRateRightAscension = guideRateRa;
				TelescopeHardware.isPulseGuidingRa = true;
                TelescopeHardware.guideDuration.X = GuideDuration();
			}
		}

        private static double GuideDuration()
        {
            double duration = TelescopeHardware.GuideDurationShort;

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                duration = TelescopeHardware.GuideDurationMedium;
            }
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                duration = TelescopeHardware.GuideDurationLong;
            }
            return duration;
        }

        private static void SetSlewSpeed()
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                TelescopeHardware.SlewSpeed = SlewSpeed.SlewMedium;
            }
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                TelescopeHardware.SlewSpeed = SlewSpeed.SlewSlow;
            }
            else
            {
                TelescopeHardware.SlewSpeed = SlewSpeed.SlewFast;
            }
        }

        private void StartSlew(SlewDirection direction)
        {
			if ( this.radioButtonPulseGuide.Checked )
			{
				// Nothing to do for pulse guiding here.
				return;
			}
            if (TelescopeHardware.AlignmentMode == DeviceInterface.AlignmentModes.algAltAz)
            {
                TelescopeHardware.SlewDirection = direction;
            }
            else
            {
                switch (direction)
                {
                    case SlewDirection.SlewEast:
                    case SlewDirection.SlewRight:
                        TelescopeHardware.SlewDirection = SlewDirection.SlewEast;
                        break;
                    case SlewDirection.SlewWest:
                    case SlewDirection.SlewLeft:
                        TelescopeHardware.SlewDirection = SlewDirection.SlewWest;
                        break;
                    case SlewDirection.SlewNorth:
                    case SlewDirection.SlewUp:
                        TelescopeHardware.SlewDirection = TelescopeHardware.SouthernHemisphere  ? SlewDirection.SlewSouth : SlewDirection.SlewNorth;
                        break;
                    case SlewDirection.SlewSouth:
                    case SlewDirection.SlewDown:
                        TelescopeHardware.SlewDirection = TelescopeHardware.SouthernHemisphere  ? SlewDirection.SlewNorth : SlewDirection.SlewSouth;
                        break;
                    case SlewDirection.SlewNone:
                    default:
                        TelescopeHardware.SlewDirection = SlewDirection.SlewNone;
                        break;
                }
            }
            SetSlewSpeed();
        }

        private void buttonSlewUp_MouseDown(object sender, MouseEventArgs e)
        {
            StartSlew(SlewDirection.SlewUp);
        }

        private void buttonSlewUp_MouseUp(object sender, MouseEventArgs e)
        {
            SetPulseGuideParms(guideRate, 0.0);
        }

        private void buttonSlewDown_MouseDown(object sender, MouseEventArgs e)
        {
            StartSlew(SlewDirection.SlewDown);
        }

        private void buttonSlewDown_MouseUp(object sender, MouseEventArgs e)
        {
			SetPulseGuideParms( -guideRate, 0.0);
		}

        private void buttonSlewRight_MouseDown(object sender, MouseEventArgs e)
        {
            StartSlew(SlewDirection.SlewRight);
        }

        private void buttonSlewRight_MouseUp(object sender, MouseEventArgs e)
        {
			SetPulseGuideParms( 0.0, guideRate);
		}

        private void buttonSlewLeft_MouseDown(object sender, MouseEventArgs e)
        {
            StartSlew(SlewDirection.SlewLeft);
        }

        private void buttonSlewLeft_MouseUp(object sender, MouseEventArgs e)
        {
			SetPulseGuideParms( 0.0, -guideRate);
		}

        private void buttonSlewStop_Click(object sender, EventArgs e)
        {
            TelescopeHardware.AbortSlew();
        }
        #endregion

        private void checkBoxTrack_CheckedChanged(object sender, EventArgs e)
        {
            if (TelescopeHardware.Tracking == checkBoxTrack.Checked)
                return;
            TelescopeHardware.Tracking = checkBoxTrack.Checked;
        }

        public void Tracking()
        {
            if (TelescopeHardware.Tracking == checkBoxTrack.Checked)
                return;
            // this avoids triggering the checked changed event
            checkBoxTrack.CheckState = TelescopeHardware.Tracking ? CheckState.Checked : CheckState.Unchecked;
        }

        public void LedPier(ASCOM.DeviceInterface.PierSide sideOfPier)
        {
            if (sideOfPier == ASCOM.DeviceInterface.PierSide.pierEast)
            {
                ledPierEast.Status=TrafficLight.Green;
                ledPierEast.Visible = true;
                ledPierWest.Visible = false;
            }
            else
            {
                ledPierWest.Status=TrafficLight.Red;
                ledPierWest.Visible = true;
                ledPierEast.Visible = false;
            }
        }

        private void buttonUnpark_Click(object sender, EventArgs e)
        {
            if (TelescopeHardware.IsParked)
            {
                TelescopeHardware.ChangePark(false);
                TelescopeHardware.Tracking = true;
            }
            else
            {
                TelescopeHardware.ChangePark(true);
                TelescopeHardware.Tracking = false;
                TelescopeHardware.Park();
            }
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            TelescopeHardware.FindHome();
        }
    }
}