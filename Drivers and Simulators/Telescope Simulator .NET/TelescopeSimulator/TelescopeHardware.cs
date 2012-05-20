//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Simulated Telescope Hardware
//
// Description:	This implements a simulated Telescope Hardware
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Telescope Driver template
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Timers;
using System.Runtime.InteropServices;
using System.Drawing;
using ASCOM.DeviceInterface;
using System.Globalization;
using ASCOM.Utilities;
using System.Diagnostics;

namespace ASCOM.Simulator
{
    public static class TelescopeHardware
    {
        // change to using a Windows timer to avoid threading problems
        private static System.Windows.Forms.Timer s_wTimer;

        private static Utilities.Profile s_Profile;
        private static bool onTop;
        private static ASCOM.Utilities.TraceLogger TL;

        //Capabilities
        private static bool canFindHome;
        private static bool canPark;
        private static bool versionOne;
        private static int numberMoveAxis;
        private static bool canPulseGuide;
        private static bool canDualAxisPulseGuide;
        private static bool canSetEquatorialRates;
        private static bool canSetGuideRates;
        private static bool canSetPark;
        private static bool canSetPierSide;
        private static bool canSetTracking;
        private static bool canSlew;
        private static bool canSlewAltAz;
        private static bool canAlignmentMode;
        private static bool canOptics;
        private static bool canSlewAltAzAsync;
        private static bool canSlewAsync;
        private static bool canSync;
        private static bool canSyncAltAz;
        private static bool canUnpark;
        private static bool canAltAz;
        private static bool canDateTime;
        private static bool canDoesRefraction;
        private static bool canEquatorial;
        private static bool canLatLongElev;
        private static bool canSiderealTime;
        private static bool canPierSide;
        private static bool canTrackingRates;

        //Telescope Implementation
        private static ASCOM.DeviceInterface.AlignmentModes alignmentMode;
        private static double apertureArea;
        private static double apertureDiameter;
        private static double focalLength;
        private static bool autoTrack;
        private static bool disconnectOnPark;
        private static bool refraction;
        private static int equatorialSystem;
        private static bool noCoordinatesAtPark;
        private static double latitude;
        private static double longitude;
        private static double elevation;
        private static int maximumSlewRate;

        private static double altitude;
        private static double azimuth;
        private static double parkAltitude;
        private static double parkAzimuth;

        private static double rightAscension;
        private static double declination;
        private static double siderealTime;

        private static double targetRightAscension;
        private static double targetDeclination;

        private static bool tracking;
        private static bool atPark;

        private static double declinationRate;
        private static double rightAscensionRate;
        public static double guideRateDeclination;
        public static double guideRateRightAscension;


        private static int trackingRate;

        private static SlewType slewState;
        private static SlewDirection slewDirection;
        private static SlewSpeed slewSpeed;

        private static double slewSpeedFast;
        private static double slewSpeedMedium;
        private static double slewSpeedSlow;

        private static double slewSettleTime;

        private static bool southernHemisphere;

        private static int dateDelta;

        public static bool isPulseGuidingRa;
        public static bool isPulseGuidingDec;
        public static DateTime pulseGuideRaEndTime;
        public static DateTime pulseGuideDecEndTime;


        public static double deltaAz;
        public static double deltaAlt;
        public static double deltaRa;
        public static double deltaDec;

        private static DateTime settleTime;

        public static PierSide sideOfPier;

        private static bool connected;      //Keep track of the connection status of the hardware


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static TelescopeHardware()
        {
            try
            {
                s_Profile = new Utilities.Profile();
                s_wTimer = new System.Windows.Forms.Timer();
                s_wTimer.Interval = (int)(SharedResources.TIMER_INTERVAL * 1000);
                s_wTimer.Tick += new EventHandler(m_wTimer_Tick);

                southernHemisphere = false;
                connected = false;
                deltaAz = 0;
                deltaAlt = 0;
                deltaRa = 0;
                deltaDec = 0;

                TL = new ASCOM.Utilities.TraceLogger("", "TelescopeSimHardware");
                TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.SIMULATOR_TRACE, GlobalConstants.SIMULATOR_TRACE_DEFAULT);

                // check if the profile settings are correct 
                if (s_Profile.GetValue(SharedResources.PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
                {
                    // load the default settings
                    //Main Driver Settings
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlwaysOnTop", "false");

                    //Telescope Implementions
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ApertureArea", SharedResources.INSTRUMENT_APERTURE_AREA.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Aperture", SharedResources.INSTRUMENT_APERTURE.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "FocalLength", SharedResources.INSTRUMENT_FOCAL_LENGTH.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AutoTrack", "false");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DiscPark", "false");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NoCoordAtPark", "false");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Refraction", "true");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "EquatorialSystem", "1");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "MaxSlewRate", "20");

                    //' Geography
                    //'
                    //' Based on the UTC offset, create a longitude somewhere in
                    //' the time zone, a latitude between 0 and 60 and a site
                    //' elevation between 0 and 1000 metres. This gives the
                    //' client some geo position without having to open the
                    //' Setup dialog.
                    Random r = new Random();
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    double lat = 51.07861;// (r.NextDouble() * 60); lock for testing
                    double lng = (((-(double)(localZone.GetUtcOffset(DateTime.Now).Seconds) / 3600) + r.NextDouble() - 0.5) * 15);
                    if (localZone.GetUtcOffset(DateTime.Now).Seconds == 0) lng = -0.29444; //lock for testing
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Elevation", Math.Round((r.NextDouble() * 1000), 0).ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Longitude", lng.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Latitude", lat.ToString(CultureInfo.InvariantCulture));

                    //Start the scope in parked position
                    if (lat >= 0)
                    {
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuth", "180");
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", "180");
                    }
                    else
                    {
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuth", "90");
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", "90");
                    }
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAltitude", (90 - Math.Abs(lat)).ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", (90 - Math.Abs(lat)).ToString(CultureInfo.InvariantCulture));

                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DateDelta", "0");

                    //Capabilities Settings
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "V1", "false", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanFindHome", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPark", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NumMoveAxis", "2", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPulseGuide", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetEquRates", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPark", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPierSide", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetTracking", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlew", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAlignMode", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanOptics", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAsync", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSync", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanUnpark", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAltAz", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDateTime", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanEquatorial", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanLatLongElev", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSiderealTime", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPierSide", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanTrackingRates", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", "true", "Capabilities");
                }

                //Load up the values from saved
                onTop = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "AlwaysOnTop"));

                switch (int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "AlignMode"), CultureInfo.InvariantCulture))
                {
                    case 0:
                        alignmentMode = ASCOM.DeviceInterface.AlignmentModes.algAltAz;
                        break;
                    case 1:
                        alignmentMode = ASCOM.DeviceInterface.AlignmentModes.algGermanPolar;
                        break;
                    case 2:
                        alignmentMode = ASCOM.DeviceInterface.AlignmentModes.algPolar;
                        break;
                    default:
                        alignmentMode = ASCOM.DeviceInterface.AlignmentModes.algGermanPolar;
                        break;
                }

                apertureArea = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ApertureArea"), CultureInfo.InvariantCulture);
                apertureDiameter = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Aperture"), CultureInfo.InvariantCulture);
                focalLength = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "FocalLength"), CultureInfo.InvariantCulture);
                autoTrack = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "AutoTrack"));
                disconnectOnPark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "DiscPark"));
                refraction = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Refraction"));
                equatorialSystem = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "EquatorialSystem"), CultureInfo.InvariantCulture);
                noCoordinatesAtPark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "NoCoordAtPark"));
                elevation = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Elevation"), CultureInfo.InvariantCulture);
                latitude = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Latitude"), CultureInfo.InvariantCulture);
                longitude = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Longitude"), CultureInfo.InvariantCulture);
                maximumSlewRate = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "MaxSlewRate"), CultureInfo.InvariantCulture);

                altitude = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAltitude"), CultureInfo.InvariantCulture);
                azimuth = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAzimuth"), CultureInfo.InvariantCulture);
                parkAltitude = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAltitude"), CultureInfo.InvariantCulture);
                parkAzimuth = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAzimuth"), CultureInfo.InvariantCulture);

                //TODO allow for version 1, 2 or 3
                versionOne = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "V1", "Capabilities"));
                canFindHome = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanFindHome", "Capabilities"));
                canPark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPark", "Capabilities"));
                numberMoveAxis = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "NumMoveAxis", "Capabilities"), CultureInfo.InvariantCulture);
                canPulseGuide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPulseGuide", "Capabilities"));
                canSetEquatorialRates = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetEquRates", "Capabilities"));
                canSetGuideRates = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", "Capabilities"));
                canSetPark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetPark", "Capabilities"));
                canSetPierSide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetPierSide", "Capabilities"));
                canSetTracking = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetTracking", "Capabilities"));
                canSlew = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlew", "Capabilities"));
                canSlewAltAz = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", "Capabilities"));
                canAlignmentMode = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanAlignMode", "Capabilities"));
                canOptics = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanOptics", "Capabilities"));
                canSlewAltAzAsync = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", "Capabilities"));
                canSlewAsync = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAsync", "Capabilities"));
                canSync = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSync", "Capabilities"));
                canSyncAltAz = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", "Capabilities"));
                canUnpark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanUnpark", "Capabilities"));
                canAltAz = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanAltAz", "Capabilities"));
                canDateTime = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDateTime", "Capabilities"));
                canDoesRefraction = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", "Capabilities"));
                canEquatorial = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanEquatorial", "Capabilities"));
                canLatLongElev = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanLatLongElev", "Capabilities"));
                canSiderealTime = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSiderealTime", "Capabilities"));
                canPierSide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPierSide", "Capabilities"));
                canTrackingRates = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanTrackingRates", "Capabilities"));
                canDualAxisPulseGuide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", "Capabilities"));

                dateDelta = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "DateDelta"), CultureInfo.InvariantCulture);

                if (latitude < 0) { southernHemisphere = true; }

                //Set the form setting for the Always On Top Value
                TelescopeSimulator.m_MainForm.TopMost = onTop;

                slewSpeedFast = maximumSlewRate * SharedResources.TIMER_INTERVAL;
                slewSpeedMedium = slewSpeedFast * 0.1;
                slewSpeedSlow = slewSpeedFast * 0.02;

                guideRateRightAscension = 15.0 * (1.0 / 3600.0) / SharedResources.SIDRATE;
                guideRateDeclination = guideRateRightAscension;
                declinationRate = 0;
                rightAscensionRate = 0;

                trackingRate = (int)DriveRates.driveSidereal;
                slewSettleTime = 0;
                ChangePark(atPark);

                targetRightAscension = SharedResources.INVALID_COORDINATE;
                targetDeclination = SharedResources.INVALID_COORDINATE;
                slewState = SlewType.SlewNone;
            }
            catch (Exception ex)
            {
                EventLogCode.LogEvent("ASCOM.Simulator.Telescope", "TelescopeHardware Initialiser Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.TelescopeSimulatorNew, ex.ToString());
                System.Windows.Forms.MessageBox.Show("Telescope HardwareInitialise: " + ex.ToString());
            }
        }

        public static void Start()
        {
            connected = false;
            tracking = AutoTrack;
            atPark = false;

            if (tracking)
            {
                CalculateAltAz();
                if (altitude < 0.0)
                {
                    declination = 0.0;
                    rightAscension = AstronomyFunctions.LocalSiderealTime(longitude) + 3.0;
                    if (rightAscension > 24.0) rightAscension = rightAscension - 24.0;
                    CalculateAltAz();
                }
            }
            else
            {
                CalculateRaDec();
            }
            siderealTime = AstronomyFunctions.LocalSiderealTime(longitude);

            s_wTimer.Start();
        }

        //Update the Telescope Based on Timed Events
        private static void m_wTimer_Tick(object sender, EventArgs e)
        {
            HardwareEvent();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static void HardwareEvent()
        {
            double step;
            switch (slewState)
            {
                case SlewType.SlewNone:
                    if (tracking)
                    {
                        // apply the ra and dec offset rates
                        // rates are in arc seconds per second
                        rightAscension += (rightAscensionRate / 3600) * SharedResources.TIMER_INTERVAL;
                        declination += (declinationRate / 3600) * SharedResources.TIMER_INTERVAL;
                        CalculateAltAz();
                    }
                    else
                    {
                        CalculateRaDec();
                    }
                    break;
                case SlewType.SlewSettle:
                    if (DateTime.Now >= settleTime)
                    {
                        SharedResources.TrafficLine(SharedResources.MessageType.Slew, "(Slew Complete)");
                        slewState = SlewType.SlewNone;
                    }
                    break;
                case SlewType.SlewMoveAxis:
                    if (alignmentMode == ASCOM.DeviceInterface.AlignmentModes.algAltAz)
                    {
                        azimuth += deltaAz * SharedResources.TIMER_INTERVAL;
                        altitude += deltaAlt * SharedResources.TIMER_INTERVAL;
                        azimuth = AstronomyFunctions.RangeAzimuth(azimuth);
                        altitude = AstronomyFunctions.RangeAlt(altitude);
                        CalculateRaDec();
                    }
                    else
                    {
                        rightAscension += (deltaAz * SharedResources.TIMER_INTERVAL) / 15;
                        declination += deltaAlt * SharedResources.TIMER_INTERVAL;
                        declination = AstronomyFunctions.RangeDec(declination);
                        rightAscension = AstronomyFunctions.RangeRA(rightAscension);
                        CalculateAltAz();
                    }
                    break;
                case SlewType.SlewRaDec:
                    TL.LogStart("SlewRaDec", string.Format("Fast {0}, Med {1}, Slow {2}, Target RA {3}, Dec {4}", slewSpeedFast, slewSpeedMedium, slewSpeedSlow, targetRightAscension, targetDeclination));

                    // determine the RA Step in degrees
                    step = GetStepSize(Math.Abs(deltaRa * 360.0 / 24.0));

                    step *= Math.Sign(deltaRa);
                    TL.LogContinue(", RaStep " + step);

                    // step is in degrees but the Ra values are in hours
                    rightAscension += step / 15.0;
                    deltaRa -= step / 15.0;

                    //Dec Step
                    step = GetStepSize(Math.Abs(deltaDec));
                    step *= Math.Sign(deltaDec);
                    TL.LogFinish(", DecStep " + step);

                    declination += step;
                    deltaDec -= step;

                    declination = AstronomyFunctions.RangeDec(declination);
                    rightAscension = AstronomyFunctions.RangeRA(rightAscension);
                    TL.LogMessage("RA, Dec", rightAscension + " " + deltaRa + " " + declination + " " + deltaDec);
                    CalculateAltAz();

                    if (Math.Abs(deltaRa) < 0.0003 && Math.Abs(deltaDec) < 0.0003)
                    {
                        TL.LogMessage("Settle", "Moved from slew to settle");
                        slewState = SlewType.SlewSettle;
                        settleTime = DateTime.Now + TimeSpan.FromSeconds(slewSettleTime);
                    }
                    TL.BlankLine();
                    break;
                case SlewType.SlewAltAz:
                case SlewType.SlewPark:
                case SlewType.SlewHome:
                    settleTime = DateTime.Now + TimeSpan.FromSeconds(slewSettleTime);

                    //Altitude Step
                    step = GetStepSize(Math.Abs(deltaAlt));
                    step *= Math.Sign(deltaAlt);

                    altitude += step;
                    deltaAlt -= step;
                    TL.LogFinish(" " + step + " " + altitude + " " + deltaAlt);

                    //Azimuth Step
                    step = GetStepSize(Math.Abs(deltaAz));
                    step *= Math.Sign(deltaAz);

                    azimuth += step;
                    deltaAz -= step;
                    TL.LogFinish(" " + step + " " + azimuth + " " + deltaAz);

                    azimuth = AstronomyFunctions.RangeAzimuth(azimuth);
                    altitude = AstronomyFunctions.RangeAlt(altitude);
                    CalculateRaDec();

                    if (Math.Abs(deltaAz) < 0.0000001 && Math.Abs(deltaAlt) < 0.0000001)
                    {
                        if (slewState == SlewType.SlewPark)
                        {
                            slewState = SlewType.SlewNone;
                            ChangePark(true);
                        }
                        else if (slewState == SlewType.SlewHome)
                        {
                            slewState = SlewType.SlewNone;
                        }
                        else
                            slewState = SlewType.SlewSettle;
                    }
                    break;
                case SlewType.SlewHandpad:
                    double z = Math.Cos(declination * SharedResources.DEG_RAD);
                    if (z < 0.001) { z = 0.001; }
                    // adjust step according to the slew speed, it's in deg/sec.
                    if (slewSpeed == SlewSpeed.SlewFast)
                    {
                        step = slewSpeedFast;
                    }
                    else if (slewSpeed == SlewSpeed.SlewMedium)
                    {
                        step = slewSpeedMedium;
                    }
                    else
                    {
                        step = slewSpeedSlow;
                    }
                    switch (slewDirection)
                    {
                        // altaz scope HC buttons adjust Altitude and azimuth
                        case SlewDirection.SlewUp:
                            altitude += step;
                            declination = AstronomyFunctions.CalculateDec(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD);
                            rightAscension = AstronomyFunctions.CalculateRA(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewDown:
                            altitude -= step;
                            declination = AstronomyFunctions.CalculateDec(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD);
                            rightAscension = AstronomyFunctions.CalculateRA(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewRight:
                            azimuth += step;
                            declination = AstronomyFunctions.CalculateDec(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD);
                            rightAscension = AstronomyFunctions.CalculateRA(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewLeft:
                            azimuth -= step;
                            declination = AstronomyFunctions.CalculateDec(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD);
                            rightAscension = AstronomyFunctions.CalculateRA(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            break;
                        // equatorial mount scopes adjust the Ra and Dec
                        case SlewDirection.SlewNorth:
                            declination += step;
                            declination = AstronomyFunctions.RangeDec(declination);
                            altitude = AstronomyFunctions.CalculateAltitude(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            azimuth = AstronomyFunctions.CalculateAzimuth(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewSouth:
                            declination -= step;
                            declination = AstronomyFunctions.RangeDec(declination);
                            altitude = AstronomyFunctions.CalculateAltitude(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            azimuth = AstronomyFunctions.CalculateAzimuth(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewEast:
                            rightAscension += step * (z / 15);    // Ra is in hours
                            rightAscension = AstronomyFunctions.RangeRA(rightAscension);
                            altitude = AstronomyFunctions.CalculateAltitude(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            azimuth = AstronomyFunctions.CalculateAzimuth(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewWest:
                            rightAscension -= step * (z / 15);
                            rightAscension = AstronomyFunctions.RangeRA(rightAscension);
                            altitude = AstronomyFunctions.CalculateAltitude(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            azimuth = AstronomyFunctions.CalculateAzimuth(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
                            break;
                    }
                    break;
            }


            if (isPulseGuidingRa || isPulseGuidingDec)
            {
                // do pulse guiding
                ChangePark(false);
                if (pulseGuideRaEndTime >= DateTime.Now)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Slew, "(PulseGuide in RA complete)");
                    isPulseGuidingRa = false;
                }
                if (pulseGuideDecEndTime > DateTime.Now)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Slew, "(PulseGuide in Dec complete)");
                    isPulseGuidingDec = false;
                }

                double raRate;
                if (tracking) raRate = rightAscensionRate;
                else raRate = 15;
                raRate = (raRate / SharedResources.SIDRATE) / 3600;
                if (isPulseGuidingRa) raRate = raRate + (guideRateRightAscension / 15);

                double decRate = declinationRate / 3600;
                if (isPulseGuidingDec) decRate += guideRateDeclination;

                rightAscension += raRate * SharedResources.TIMER_INTERVAL;
                declination += decRate * SharedResources.TIMER_INTERVAL;

                declination = AstronomyFunctions.RangeDec(declination);
                rightAscension = AstronomyFunctions.RangeRA(rightAscension);

                CalculateAltAz();
            }

            //Calculate Current SideOfPier
            siderealTime = AstronomyFunctions.LocalSiderealTime(longitude);
            sideOfPier = SideOfPierRaDec(rightAscension, declination);

            // display the values
            TelescopeSimulator.m_MainForm.SiderealTime(siderealTime);
            TelescopeSimulator.m_MainForm.Altitude(altitude);
            TelescopeSimulator.m_MainForm.Azimuth(azimuth);
            TelescopeSimulator.m_MainForm.RightAscension(rightAscension);
            TelescopeSimulator.m_MainForm.Declination(declination);
            TelescopeSimulator.m_MainForm.Tracking();
            TelescopeSimulator.m_MainForm.LedPier(sideOfPier);

            if (atPark) TelescopeSimulator.m_MainForm.lblPARK.ForeColor = Color.Red;
            else TelescopeSimulator.m_MainForm.lblPARK.ForeColor = Color.SaddleBrown;
            if (AtHome) TelescopeSimulator.m_MainForm.lblHOME.ForeColor = Color.Red;
            else TelescopeSimulator.m_MainForm.lblHOME.ForeColor = Color.SaddleBrown;
            if (slewState == SlewType.SlewNone) TelescopeSimulator.m_MainForm.labelSlew.ForeColor = Color.SaddleBrown;
            else TelescopeSimulator.m_MainForm.labelSlew.ForeColor = Color.Red;
        }

        /// <summary>
        /// Returns the step size, adjusted for the current slew speed
        /// </summary>
        /// <param name="fullStepSize"></param>
        /// <returns></returns>
        private static double GetStepSize(double fullStepSize)
        {
            double step = 0;
            if (slewSpeedFast / SharedResources.TIMER_INTERVAL >= 50)
            {
                step = fullStepSize;
            }
            else if (fullStepSize > 2 * slewSpeedFast)
            {
                step = slewSpeedFast;
            }
            else if (fullStepSize > 2 * slewSpeedMedium)
            {
                step = slewSpeedMedium;
            }
            else if (fullStepSize > 2 * slewSpeedSlow)
            {
                step = slewSpeedSlow;
            }
            else
            {
                step = fullStepSize;
            }
            return step;
        }

        #region Properties For Settings

        //I used some of these as dual purpose if the driver uses the same exact property
        public static ASCOM.DeviceInterface.AlignmentModes AlignmentMode
        {
            get { return alignmentMode; }
            set
            {
                alignmentMode = value;
                switch (value)
                {
                    case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "0");
                        break;
                    case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1");
                        break;
                    case ASCOM.DeviceInterface.AlignmentModes.algPolar:
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "2");
                        break;
                }
            }
        }

        public static bool OnTop
        {
            get { return onTop; }
            set
            {
                onTop = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "OnTop", value.ToString(), "");
            }
        }
        public static bool AutoTrack
        {
            get { return autoTrack; }
            set
            {
                autoTrack = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AutoTrack", value.ToString(), "");
            }
        }

        public static bool NoCoordinatesAtPark
        {
            get { return noCoordinatesAtPark; }
            set
            {
                noCoordinatesAtPark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NoCoordAtPark", value.ToString());
            }
        }

        public static bool VersionOneOnly
        {
            get { return versionOne; }
            set
            {
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "V1", value.ToString(), "Capabilities");
                versionOne = value;
            }
        }

        public static bool DisconnectOnPark
        {
            get { return disconnectOnPark; }
            set
            {
                disconnectOnPark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DiscPark", value.ToString(), "");
            }
        }

        public static bool Refraction
        {
            get { return refraction; }
            set
            {
                refraction = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Refraction", value.ToString(), "");
            }
        }

        public static int EquatorialSystem
        {
            get { return equatorialSystem; }
            set
            {
                equatorialSystem = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "EquatorialSystem", value.ToString(CultureInfo.InvariantCulture), "");
            }
        }

        public static double Elevation
        {
            get { return elevation; }
            set
            {
                elevation = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Elevation", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Latitude", value.ToString(CultureInfo.InvariantCulture));
                if (latitude < 0) { southernHemisphere = true; }
            }
        }

        public static double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Longitude", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static int MaximumSlewRate
        {
            get { return maximumSlewRate; }
            set
            {
                maximumSlewRate = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "MaxSlewRate", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool CanFindHome
        {
            get { return canFindHome; }
            set
            {
                canFindHome = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanFindHome", value.ToString(), "Capabilities");
            }
        }

        public static bool CanOptics
        {
            get { return canOptics; }
            set
            {
                canOptics = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanOptics", value.ToString(), "Capabilities");
            }
        }

        public static bool CanPark
        {
            get { return canPark; }
            set
            {
                canPark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPark", value.ToString(), "Capabilities");
            }
        }

        public static int NumberMoveAxis
        {
            get { return numberMoveAxis; }
            set
            {
                numberMoveAxis = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NumMoveAxis", value.ToString(CultureInfo.InvariantCulture), "Capabilities");
            }
        }

        public static bool CanPulseGuide
        {
            get { return canPulseGuide; }
            set
            {
                canPulseGuide = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPulseGuide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanDualAxisPulseGuide
        {
            get { return canDualAxisPulseGuide; }
            set
            {
                canDualAxisPulseGuide = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetEquatorialRates
        {
            get { return canSetEquatorialRates; }
            set
            {
                canSetEquatorialRates = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetEquRates", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetGuideRates
        {
            get { return canSetGuideRates; }
            set
            {
                canSetGuideRates = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetPark
        {
            get { return canSetPark; }
            set
            {
                canSetPark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPark", value.ToString(), "Capabilities");
            }
        }

        public static bool CanPierSide
        {
            get { return canPierSide; }
            set
            {
                canPierSide = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPierSide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetPierSide
        {
            get { return canSetPierSide; }
            set
            {
                canSetPierSide = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPierSide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetTracking
        {
            get { return canSetTracking; }
            set
            {
                canSetTracking = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetTracking", value.ToString(), "Capabilities");
            }
        }

        public static bool CanTrackingRates
        {
            get { return canTrackingRates; }
            set
            {
                canTrackingRates = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanTrackingRates", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlew
        {
            get { return canSlew; }
            set
            {
                canSlew = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlew", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSync
        {
            get { return canSync; }
            set
            {
                canSync = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSync", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlewAsync
        {
            get { return canSlewAsync; }
            set
            {
                canSlewAsync = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAsync", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlewAltAz
        {
            get { return canSlewAltAz; }
            set
            {
                canSlewAltAz = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSyncAltAz
        {
            get { return canSyncAltAz; }
            set
            {
                canSyncAltAz = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", value.ToString(), "Capabilities");
            }
        }

        public static bool CanAltAz
        {
            get { return canAltAz; }
            set
            {
                canAltAz = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAltAz", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlewAltAzAsync
        {
            get { return canSlewAltAzAsync; }
            set
            {
                canSlewAltAzAsync = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", value.ToString(), "Capabilities");
            }
        }

        public static bool CanAlignmentMode
        {
            get { return canAlignmentMode; }
            set
            {
                canAlignmentMode = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAlignMode", value.ToString(), "Capabilities");
            }
        }

        public static bool CanUnpark
        {
            get { return canUnpark; }
            set
            {
                canUnpark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanUnpark", value.ToString(), "Capabilities");
            }
        }

        public static bool CanDateTime
        {
            get { return canDateTime; }
            set
            {
                canDateTime = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDateTime", value.ToString(), "Capabilities");
            }
        }

        public static bool CanDoesRefraction
        {
            get { return canDoesRefraction; }
            set
            {
                canDoesRefraction = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", value.ToString(), "Capabilities");
            }
        }

        public static bool CanEquatorial
        {
            get { return canEquatorial; }
            set
            {
                canEquatorial = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanEquatorial", value.ToString(), "Capabilities");
            }
        }

        public static bool CanLatLongElev
        {
            get { return canLatLongElev; }
            set
            {
                canLatLongElev = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanLatLongElev", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSiderealTime
        {
            get { return canSiderealTime; }
            set
            {
                canSiderealTime = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSiderealTime", value.ToString(), "Capabilities");
            }
        }

        #endregion

        #region Telescope Implementation
        public static bool Tracking
        {
            get
            { return tracking; }
            set
            { tracking = value; }
        }
        public static double Altitude
        {
            get { return altitude; }
            set { altitude = value; }
        }
        public static double Azimuth
        {
            get { return azimuth; }
            set { azimuth = value; }
        }
        public static double ParkAltitude
        {
            get { return parkAltitude; }
            set
            {
                parkAltitude = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", value.ToString(CultureInfo.InvariantCulture));
            }
        }
        public static double ParkAzimuth
        {
            get { return parkAzimuth; }
            set
            {
                parkAzimuth = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool Connected
        {
            get
            { return connected; }
            set
            { connected = value; }
        }

        public static bool CanMoveAxis(ASCOM.DeviceInterface.TelescopeAxes axis)
        {
            int ax = 0;
            switch (axis)
            {
                case ASCOM.DeviceInterface.TelescopeAxes.axisPrimary:
                    ax = 1;
                    break;
                case ASCOM.DeviceInterface.TelescopeAxes.axisSecondary:
                    ax = 2;
                    break;
                case ASCOM.DeviceInterface.TelescopeAxes.axisTertiary:
                    ax = 3;
                    break;
            }

            if (ax == 0 || ax > numberMoveAxis)
            { return false; }
            else
            { return true; }
        }

        public static bool CanSetDeclinationRate
        { get { return canSetEquatorialRates; } }

        public static bool CanSetRightAscensionRate
        { get { return canSetEquatorialRates; } }

        public static double ApertureArea
        {
            get { return apertureArea; }
            set
            {
                apertureArea = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ApertureArea", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double ApertureDiameter
        {
            get { return apertureDiameter; }
            set
            {
                apertureDiameter = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Aperture", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double FocalLength
        {
            get { return focalLength; }
            set
            {
                focalLength = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "FocalLength", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool SouthernHemisphere
        { get { return southernHemisphere; } }

        public static double RightAscension
        {
            get { return rightAscension; }
            set { rightAscension = value; }
        }

        public static double Declination
        {
            get { return declination; }
            set { declination = value; }
        }

        public static bool AtPark
        { get { return atPark; } }

        public static SlewType SlewState
        {
            get { return slewState; }
            set { slewState = value; }
        }

        public static SlewSpeed SlewSpeed
        {
            get { return slewSpeed; }
            set { slewSpeed = value; }
        }

        public static SlewDirection SlewDirection
        {
            get { return slewDirection; }
            set { slewDirection = value; }
        }

        /// <summary>
        /// report if the mount is at the home position by comparing it's position with the home position.
        /// </summary>
        public static bool AtHome
        {
            get
            {
                return (Math.Abs(azimuth - 180.0) < 0.01 && Math.Abs(altitude - (90 - latitude)) < 0.01);
            }
        }

        public static double SiderealTime
        { get { return siderealTime; } }


        public static double TargetRightAscension
        {
            get { return targetRightAscension; }
            set { targetRightAscension = value; }
        }
        public static double TargetDeclination
        {
            get { return targetDeclination; }
            set { targetDeclination = value; }
        }
        public static int DateDelta
        {
            get { return dateDelta; }
            set
            {
                dateDelta = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DateDelta", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double DeclinationRate
        {
            get { return declinationRate; }
            set { declinationRate = value; }
        }
        public static double RightAscensionRate
        {
            get { return rightAscensionRate; }
            set { rightAscensionRate = value; }
        }

        public static double GuideRateDeclination
        {
            get { return guideRateDeclination; }
            set { guideRateDeclination = value; }
        }
        public static double GuideRateRightAscension
        {
            get { return guideRateRightAscension; }
            set { guideRateRightAscension = value; }
        }
        public static int TrackingRate
        {
            get { return trackingRate; }
            set { trackingRate = value; }
        }
        public static double SlewSettleTime
        {
            get { return slewSettleTime; }
            set { slewSettleTime = value; }
        }

        public static bool IsPulseGuiding
        {
            get
            {
                return (isPulseGuidingDec || isPulseGuidingRa);
            }

        }
        public static bool IsParked
        {
            get { return atPark; }
        }
        #endregion

        #region Helper Functions
        public static PierSide SideOfPierRaDec(double rightAscension, double declination)
        {
            PierSide SideOfPier;
            //double hourAngle;
            if (alignmentMode != ASCOM.DeviceInterface.AlignmentModes.algGermanPolar)
            {
                return ASCOM.DeviceInterface.PierSide.pierUnknown;
            }
            else
            {
                double Ha = AstronomyFunctions.HourAngle(rightAscension, longitude);
                if (Ha < 0.0 && Ha >= -12.0) SideOfPier = PierSide.pierWest;
                //else if (Ha < -6.0 && Ha >= -12.0) return  PierSide.pierWest;
                else if (Ha >= 0.0 && Ha <= 12.0) SideOfPier = PierSide.pierEast;
                //else if (Ha > 6.0 && Ha <= 12.0) return PierSide.pierEast;
                else SideOfPier = PierSide.pierUnknown;
                TL.LogMessage("SideOfPierRaDec", rightAscension + " " + declination + " " + Ha + " " + SideOfPier.ToString());

                return SideOfPier;

                //hourAngle = AstronomyFunctions.RangeHa(AstronomyFunctions.LocalSiderealTime(m_Longitude) - RightAscension);
                //hourAngle = AstronomyFunctions.LocalSiderealTime(m_Longitude) - RightAscension;
                //TL.LogMessage("SideOfPierRaDec", "Longitude: " + m_Longitude + "LST: " + AstronomyFunctions.LocalSiderealTime(m_Longitude) + "HA: " + hourAngle + " RA: " + RightAscension);
                //if (hourAngle >=0) return ASCOM.DeviceInterface.PierSide.pierEast;
                //else return ASCOM.DeviceInterface.PierSide.pierWest;

            }
        }

        public static ASCOM.DeviceInterface.PierSide SideOfPier(double azimuth)
        {
            if (alignmentMode != ASCOM.DeviceInterface.AlignmentModes.algGermanPolar)
            {
                return ASCOM.DeviceInterface.PierSide.pierUnknown;
            }
            if (azimuth >= 180) return ASCOM.DeviceInterface.PierSide.pierEast;
            else return ASCOM.DeviceInterface.PierSide.pierWest;
        }

        public static void StartSlewRaDec(double rightAscension, double declination, bool doSideOfPier)
        {
            //ASCOM.DeviceInterface.PierSide targetSideOfPier;
            slewState = SlewType.SlewNone;

            /*if (DoSideOfPier) targetSideOfPier = SideOfPierRaDec(RightAscension, Declination);
            else targetSideOfPier = m_SideOfPier;

            if (targetSideOfPier != m_SideOfPier)
            {
                if (RightAscension >= 12) m_RightAscension = RightAscension - 12;
                else m_RightAscension = RightAscension + 12;

                CalculateAltAz();
                m_SideOfPier = targetSideOfPier;
            } */
            deltaRa = rightAscension - TelescopeHardware.rightAscension;
            deltaDec = declination - TelescopeHardware.declination;
            deltaAlt = 0;
            deltaAz = 0;
            TL.LogMessage("StartSlewRaDec", rightAscension + " " + declination + " " + doSideOfPier + " " + deltaRa + " " + deltaDec);

            if (deltaRa < -12) deltaRa = deltaRa + 24;
            else if (deltaRa > 12) deltaRa = deltaRa - 24;
            TL.LogMessage("StartSlewRaDec", rightAscension + " " + declination + " " + doSideOfPier + " " + deltaRa + " " + deltaDec);

            ChangePark(false);

            slewState = SlewType.SlewRaDec;
        }

        public static void StartSlewAltAz(double altitude, double azimuth, bool doSideOfPier, SlewType slew)
        {
            TL.LogMessage("StartSlewAltAz", altitude + " " + azimuth + " " + doSideOfPier + " " + Enum.GetName(typeof(SlewType), slew));
            //ASCOM.DeviceInterface.PierSide targetSideOfPier;
            slewState = SlewType.SlewNone;

            // this seems to do a pier flip by changing the azimuth by 180 degrees if it's neccessary
            /*if (DoSideOfPier) targetSideOfPier = SideOfPier(Azimuth);
            else targetSideOfPier = m_SideOfPier;

            if (targetSideOfPier != m_SideOfPier)
            {
                if (Azimuth >= 180) m_Azimuth = Azimuth -180;
                else m_Azimuth = Azimuth + 180;

                CalculateRaDec();
                m_SideOfPier = targetSideOfPier;
            }*/
            deltaRa = 0;
            deltaDec = 0;
            deltaAlt = altitude - TelescopeHardware.altitude;
            deltaAz = azimuth - TelescopeHardware.azimuth;

            if (deltaAz < -180.0) deltaAz += 360.0;
            if (deltaAz >= 180.0) deltaAz -= 360.0;

            ChangePark(false);

            slewState = slew;
        }

        public static void Park()
        {
            tracking = false;
            TelescopeSimulator.m_MainForm.Tracking();

            StartSlewAltAz(parkAltitude, parkAzimuth, true, SlewType.SlewPark);

        }

        public static void FindHome()
        {
            double altitude;
            double azimuth;
            if (atPark) throw new ParkedException();
            if (latitude >= 0) azimuth = 180;
            else azimuth = 0;

            altitude = 90 - latitude;

            tracking = false;
            TelescopeSimulator.m_MainForm.Tracking();

            StartSlewAltAz(altitude, azimuth, true, SlewType.SlewHome);
        }

        //public static void ChangeHome(bool NewValue)
        //{
        //    m_AtHome = NewValue;
        //}

        public static void ChangePark(bool newValue)
        {
            atPark = newValue;
            if (atPark) TelescopeSimulator.m_MainForm.ParkButton("Unpark");
            else TelescopeSimulator.m_MainForm.ParkButton("Park");
        }

        public static void CalculateAltAz()
        {
            altitude = AstronomyFunctions.CalculateAltitude(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
            azimuth = AstronomyFunctions.CalculateAzimuth(rightAscension * SharedResources.HRS_RAD, declination * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
            TL.LogMessage("TimerEvent:CalcAltAz", altitude + " " + deltaAlt + " " + azimuth + " " + deltaAz);
        }
        public static void CalculateRaDec()
        {
            declination = AstronomyFunctions.CalculateDec(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD);
            rightAscension = AstronomyFunctions.CalculateRA(altitude * SharedResources.DEG_RAD, azimuth * SharedResources.DEG_RAD, latitude * SharedResources.DEG_RAD, longitude * SharedResources.DEG_RAD);
            TL.LogMessage("TimerEvent:CalcRADec", altitude + " " + azimuth + " " + latitude + " " + longitude);
        }
        #endregion

    }

}
