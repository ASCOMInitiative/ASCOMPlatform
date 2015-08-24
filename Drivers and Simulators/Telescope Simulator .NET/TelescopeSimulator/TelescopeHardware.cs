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
// 18-SEP-2102  Rick Burke  Improved support for simulating pulse guiding
// May/June 2014 cdr 6.1    Change the telescope hardware to use an axis based method
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    public static class TelescopeHardware
    {
        // change to using a Windows timer to avoid threading problems
        private static System.Windows.Forms.Timer s_wTimer;

        private static long idCount; // Counter to generate ever increasing sequential ID numbers
 
        // this emulates a hardware connection
        // the dictionary maintains a list of connected drivers, a dictionary is used so only one of each
        // driver is maintained.
        // Connected will be reported as true if any driver is connected
        // each driver instance has a unique id generated using ObjectIDGenerator
        private static ConcurrentDictionary<long, bool> connectStates;// = new ConcurrentDictionary<long, bool>();

        private static Utilities.Profile s_Profile;
        private static bool onTop;
        public static ASCOM.Utilities.TraceLogger TL;

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

        //
        // Vectors are used for pairs of angles that represent the various positions and rates
        //
        // X is the primary axis, Hour angle, Right Ascension or azimuth and Y is the secondary axis,
        // declination or altitude.
        //
        // Ra and hour angle are in hours and the mount positions, Declination, azimuth and altitude are in degrees.
        //

        /// <summary>
        /// current altitude (Y, deg) and azimuth (X, Deg), derived from the mountAxes
        /// </summary>
        private static Vector altAzm;

        /// <summary>
        /// Park axis positions, X primary, Y secondary
        /// </summary>
        private static Vector parkAxes;

        /// <summary>
        /// current Ra (X, hrs) and Dec (Y, deg), derived from the mount axes
        /// </summary>
        private static Vector currentRaDec;

        /// <summary>
        /// Target right ascension (X, hrs) and declination (Y, deg)
        /// </summary>
        private static Vector targetRaDec;

        /// <summary>
        /// Right Ascension (X) and declination (Y) rates, deg/sec
        /// </summary>
        public static Vector rateRaDec = new Vector();

        /// <summary>
        /// Guide rates, deg/sec. X Ra/Azm, Y Alt/Dec
        /// </summary>
        public static Vector guideRate = new Vector();

        //private static SlewType slewState;

        // speeds are in deg/sec.
        private static double slewSpeedFast;
        private static double slewSpeedMedium;
        private static double slewSpeedSlow;

        // durations are in secs.
        internal static double GuideDurationShort { get; private set; }
        internal static double GuideDurationMedium { get; private set; }
        internal static double GuideDurationLong { get; private set; }

        private static bool southernHemisphere;

        private static int dateDelta;

        public static bool isPulseGuidingRa;
        public static bool isPulseGuidingDec;

        /// <summary>
        /// duration in seconds for guiding
        /// </summary>
        public static Vector guideDuration = new Vector();

        /// <summary>
        /// Axis Rates, deg/sec.
        /// </summary>
        public static Vector rateAxes = new Vector();

        private static DateTime settleTime;

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
                //Connected = false;
                rateAxes = new Vector();

                TL = new ASCOM.Utilities.TraceLogger("", "TelescopeSimHardware");
                TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.SIMULATOR_TRACE, GlobalConstants.SIMULATOR_TRACE_DEFAULT);

                connectStates = new ConcurrentDictionary<long, bool>();
                idCount = 0; // Initialise count to zero

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

                altAzm.Y = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAltitude"), CultureInfo.InvariantCulture);
                altAzm.X = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAzimuth"), CultureInfo.InvariantCulture);
                parkAxes.Y = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAltitude"), CultureInfo.InvariantCulture);
                parkAxes.X = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAzimuth"), CultureInfo.InvariantCulture);

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

                if (TelescopeSimulator.m_MainForm == null)
                {
                    TelescopeSimulator.m_MainForm = new FrmMain();
                }

                //Set the form setting for the Always On Top Value
                TelescopeSimulator.m_MainForm.TopMost = onTop;

                slewSpeedFast = maximumSlewRate * SharedResources.TIMER_INTERVAL;
                slewSpeedMedium = slewSpeedFast * 0.1;
                slewSpeedSlow = slewSpeedFast * 0.02;
                SlewDirection = SlewDirection.SlewNone;

                GuideDurationShort = 0.8 * SharedResources.TIMER_INTERVAL;
                GuideDurationMedium = 2.0 * GuideDurationShort;
                GuideDurationLong = 2.0 * GuideDurationMedium;

                guideRate.X = 15.0 * (1.0 / 3600.0) / SharedResources.SIDRATE;
                guideRate.Y = guideRate.X;
                rateRaDec.Y = 0;
                rateRaDec.X = 0;

                TrackingRate = DriveRates.driveSidereal;
                SlewSettleTime = 0;
                ChangePark(AtPark);

                // invalid target position
                targetRaDec = new Vector(double.NaN, double.NaN);
                SlewState = SlewType.SlewNone;

                // set the home position
                homeAxes = new Vector();    // AltAz is North and Level, hope Meade don't mind!
                switch (alignmentMode)
                {
                    case AlignmentModes.algGermanPolar:
                        // looking at the pole, counterweight down
                        homeAxes.X = 90;
                        homeAxes.Y = 90;
                        break;
                    case AlignmentModes.algPolar:
                        // looking East, tube level
                        homeAxes.X = -90;
                        homeAxes.Y = 0;
                        break;
                }
                TL.LogMessage("TelescopeHardware New", "Successfully initialised hardware");

            }
            catch (Exception ex)
            {
                EventLogCode.LogEvent("ASCOM.Simulator.Telescope", "TelescopeHardware Initialiser Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.TelescopeSimulatorNew, ex.ToString());
                System.Windows.Forms.MessageBox.Show("Telescope HardwareInitialise: " + ex.ToString());
            }
        }

        public static void Start()
        {
            //Connected = false;
            Tracking = AutoTrack;
            AtPark = false;

            rateAxes.X = 0;
            rateAxes.Y = 0;

            lastUpdateTime = DateTime.Now;
            s_wTimer.Start();
        }

        //Update the Telescope Based on Timed Events
        private static void m_wTimer_Tick(object sender, EventArgs e)
        {
            MoveAxes();
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
            {
                return trackingMode != TrackingMode.Off;
            }
            set
            {
                if (value)
                {
                    switch (AlignmentMode)
                    {
                        case AlignmentModes.algAltAz:
                            trackingMode = TrackingMode.AltAz;
                            break;
                        case AlignmentModes.algGermanPolar:
                        case AlignmentModes.algPolar:
                            trackingMode = Latitude >= 0 ? TrackingMode.EqN : TrackingMode.EqS;
                            break;
                    }
                }
                else
                {
                    trackingMode = TrackingMode.Off;
                }
                TelescopeSimulator.m_MainForm.Tracking();
            }
        }

        public static double Altitude
        {
            get { return altAzm.Y; }
            set { altAzm.Y = value; }
        }

        public static double Azimuth
        {
            get { return altAzm.X; }
            set { altAzm.X = value; }
        }
        public static double ParkAltitude
        {
            get { return parkAxes.Y; }
            set
            {
                parkAxes.Y = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", value.ToString(CultureInfo.InvariantCulture));
            }
        }
        public static double ParkAzimuth
        {
            get { return parkAxes.X; }
            set
            {
                parkAxes.X = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        //public static bool Connected {get; set;}

        public static long GetId()
        {
            idCount += 1;
            TL.LogMessage("GetId", "Generated new ID: " + idCount.ToString());
            return idCount;
        }

        public static bool Connected
        {
            get
            {
                TL.LogMessage("Hardware.Connected Get", "Number of connected devices: " + connectStates.Count + ", Returning: " + (connectStates.Count > 0).ToString());
                return connectStates.Count > 0;
            }
        }

        public static void SetConnected(long id, bool value)
        {
            // add or remove the instance, this is done once regardless of the number of calls
            if (value)
            {
                bool notAlreadyPresent = connectStates.TryAdd(id, true);
                TL.LogMessage("Hardware.Connected Set", "Set Connected to: True, AlreadyConnected: " + (!notAlreadyPresent).ToString());
            }
            else
            {
                bool successfullyRemoved = connectStates.TryRemove(id, out value);
                TL.LogMessage("Hardware.Connected Set", "Set Connected to: False, Successfully removed: " + successfullyRemoved.ToString());
            }
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
            get { return currentRaDec.X; }
            set { currentRaDec.X = value; }
        }

        public static double Declination
        {
            get { return currentRaDec.Y; }
            set { currentRaDec.Y = value; }
        }

        public static bool AtPark { get; private set; }

        public static SlewType SlewState { get; private set; }

        public static SlewSpeed SlewSpeed { get; set; }

        public static SlewDirection SlewDirection { get; set; }

        /// <summary>
        /// report if the mount is at the home position by comparing it's position with the home position.
        /// </summary>
        public static bool AtHome
        {
            get
            {
                return (mountAxes - homeAxes).LengthSquared < 0.01;
            }
        }

        public static double SiderealTime { get; private set; }

        public static double TargetRightAscension
        {
            get { return targetRaDec.X; }
            set { targetRaDec.X = value; }
        }
        public static double TargetDeclination
        {
            get { return targetRaDec.Y; }
            set { targetRaDec.Y = value; }
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
            get { return rateRaDec.Y; }
            set { rateRaDec.Y = value; }
        }

        // converts the rate between seconds per sidereal second and seconds per second
        public static double RightAscensionRate
        {
            get { return rateRaDec.X / SIDEREAL_RATE_SEC_SEC; }
            set { rateRaDec.X = value * SIDEREAL_RATE_SEC_SEC; }
        }

        public static double GuideRateDeclination
        {
            get { return guideRate.Y; }
            set { guideRate.Y = value; }
        }
        public static double GuideRateRightAscension
        {
            get { return guideRate.X; }
            set { guideRate.X = value; }
        }

        public static DriveRates TrackingRate { get; set; }

        public static double SlewSettleTime { get; set; }

        public static bool IsPulseGuiding
        {
            get
            {
                return (isPulseGuidingDec || isPulseGuidingRa);
            }
        }

        public static bool IsParked
        {
            get { return AtPark; }
        }
        #endregion

        #region Helper Functions

        /// <summary>
        /// Gets the side of pier using the right ascension, assuming it depends on the
        /// hour aangle only.  Used for Destinaation side of Pier, NOT to determine the mount
        /// pointing state
        /// </summary>
        /// <param name="rightAscension">The right ascension.</param>
        /// <param name="declination">The declination.</param>
        /// <returns></returns>
        public static PierSide SideOfPierRaDec(double rightAscension, double declination)
        {
            PierSide sideOfPier;
            if (alignmentMode != ASCOM.DeviceInterface.AlignmentModes.algGermanPolar)
            {
                return ASCOM.DeviceInterface.PierSide.pierUnknown;
            }
            else
            {
                double ha = AstronomyFunctions.HourAngle(rightAscension, longitude);
                if (ha < 0.0 && ha >= -12.0) sideOfPier = PierSide.pierWest;
                else if (ha >= 0.0 && ha <= 12.0) sideOfPier = PierSide.pierEast;
                else sideOfPier = PierSide.pierUnknown;
                LogMessage("SideOfPierRaDec", "Ra {0}, Dec {1}, Ha {2}, sop {3}", rightAscension, declination, ha, sideOfPier);

                return sideOfPier;
            }
        }

        public static void Park()
        {
            Tracking = false;
            StartSlewAxes(parkAxes, SlewType.SlewPark);
        }

        public static void FindHome()
        {
            if (AtPark)
            {
                throw new ParkedException("Cannot find Home when Parked");
            }

            Tracking = false;
            StartSlewAxes(homeAxes, SlewType.SlewHome);
        }

        public static void ChangePark(bool newValue)
        {
            AtPark = newValue;
            if (AtPark) TelescopeSimulator.m_MainForm.ParkButton("Unpark");
            else TelescopeSimulator.m_MainForm.ParkButton("Park");
        }

        private static void LogMessage(string identifier, string format, params object[] args)
        {
            TL.LogMessage(identifier, string.Format(CultureInfo.InvariantCulture, format, args));
        }

        #endregion

        #region Telescope using Axis control

        // The telescope is implemented using two axes that represent the primary and secondary telescope axes.
        // The role of the axes varies depending on the mount type.
        // The primary axis is the azimuth axis for an AltAz mount
        // and the hour angle axis for polar mounts.
        // The secondary axis is the altitude axis for AltAz mounts and the declination axis for polar mounts.
        //
        // all motion is done and all positions are set and obtained using these axes.
        //

        private enum PointingState
        {
            Normal,
            ThroughThePole
        }

        /// <summary>
        /// the current mount axis values in degrees. X is primary, Y is secondary
        /// </summary>
        private static Vector mountAxes;

        private static Vector targetAxes;
        private static Vector homeAxes;

        private static double hourAngleLimit = 20;     // the number of degrees a GEM can go past the meridian

        private static PointingState pointingState;
        private static TrackingMode trackingMode;
        private static bool slewing;

        private const double SIDEREAL_RATE_DEG_SEC = 15.041 / 3600;
        private const double SOLAR_RATE_DEG_SEC = 15.0 / 3600;
        private const double LUNAR_RATE_DEG_SEC = 14.515 / 3600;
        private const double KING_RATE_DEG_SEC = 15.037 / 3600;
        private const double SIDEREAL_RATE_SEC_SEC = 15.041 / 15.0;

        private static DateTime lastUpdateTime;

        public static PierSide SideOfPier
        {
            get
            {
                return (mountAxes.Y <= 90 && mountAxes.Y >= -90) ?
                    PierSide.pierEast : PierSide.pierWest;
            }
            set
            {
                // check the new side can be reached
                var pa = AstronomyFunctions.RangeAzimuth(mountAxes.X - 180);
                if (pa >= hourAngleLimit + 180 && pa < -hourAngleLimit)
                {
                    throw new InvalidOperationException("set SideOfPier " + value.ToString() + " cannot be reached at the current position");
                }

                // change the pier side
                StartSlewAxes(pa, 180 - mountAxes.Y, SlewType.SlewRaDec);
            }
        }

        /// <summary>
        /// This is called every TIMER_INTERVAL period and applies the current movement rates to the axes,
        /// copes with the range and updates the displayed values
        /// </summary>
        private static void MoveAxes()
        {
            // get the time since the last update. This avoids problems with the
            // timer interval varying and greatly improves tracking.
            var now = DateTime.Now;
            var updateInterval = (now - lastUpdateTime).TotalSeconds;
            lastUpdateTime = now;

            // these are the changes to be applied to the axes during this update interval
            Vector change = new Vector();

            // determine the change in ha required as a result of tracking
            change.X = GetTrackingChange(updateInterval);

            // convert the change in Ha to altitude and azimuth changes
            if (alignmentMode == AlignmentModes.algAltAz)
            {
                change = ConvertRateToAltAz(change.X);
            }

            // Move towards the target position
            change += DoSlew();

            // handle HC button moves
            change += HcMoves();

            // Axis movement
            change += rateAxes;

            // Ra and Dec rate, this assumes a polar mount
            change += rateRaDec;

            change += PulseGuide(updateInterval);

            // update the axis values
            mountAxes += change;

            // check the axis values, stop movement past limits
            CheckAxisLimits(change.X);

            // update the displayed values
            UpdatePositions();

            // check and update slew state 
            switch (SlewState)
            {
                case SlewType.SlewSettle:
                    if (DateTime.Now >= settleTime)
                    {
                        SharedResources.TrafficLine(SharedResources.MessageType.Slew, "(Slew Complete)");
                        SlewState = SlewType.SlewNone;
                    }
                    break;
                //case SlewType.SlewNone:
                //    break;
                //case SlewType.SlewMoveAxis:
                //    break;
                //case SlewType.SlewRaDec:
                //    break;
                //case SlewType.SlewAltAz:
                //    break;
                //case SlewType.SlewPark:
                //    break;
                //case SlewType.SlewHome:
                //    break;
                //case SlewType.SlewHandpad:
                //    break;
                //default:
                //    break;
            }

        }

        /// <summary>
        /// returns the mount traacking movement in hour angle during the update intervaaal
        /// </summary>
        /// <param name="updateInterval">The update interval.</param>
        /// <returns></returns>
        private static double GetTrackingChange(double updateInterval)
        {
            if (!Tracking)
            {
                return 0;
            }
            double haChange = 0;
            // determine the change required as a result of tracking
            // generate the change in hour angle as a result of tracking
            switch (TrackingRate)
            {
                case DriveRates.driveSidereal:
                    haChange = SIDEREAL_RATE_DEG_SEC * updateInterval;     // change in degrees
                    break;
                case DriveRates.driveSolar:
                    haChange = SOLAR_RATE_DEG_SEC * updateInterval;     // change in degrees
                    break;
                case DriveRates.driveLunar:
                    haChange = LUNAR_RATE_DEG_SEC * updateInterval;     // change in degrees
                    break;
                case DriveRates.driveKing:
                    haChange = KING_RATE_DEG_SEC * updateInterval;     // change in degrees
                    break;
            }
            // adjust the target to allow for tracking the primary axis while slewing
            targetAxes.X += haChange;
            return haChange;
        }

        /// <summary>
        /// Return the axis movement as a result of any slew that's taking place
        /// </summary>
        /// <returns></returns>
        private static Vector DoSlew()
        {
            Vector change = new Vector();
            if (!slewing)
            {
                return change;
            }
            // Move towards the target position
            double delta;
            bool finished = true;
            delta = targetAxes.X - mountAxes.X;
            while (delta < -180 || delta > 180)
            {
                if (delta < -180) delta += 360;
                if (delta > 180) delta -= 360;
            }
            int signDelta = delta < 0 ? -1 : +1;
            delta = Math.Abs(delta);

            if (delta < slewSpeedSlow)
            {
                change.X = delta * signDelta;
            }
            else if (delta < slewSpeedMedium * 2)
            {
                change.X = slewSpeedSlow * signDelta;
                finished = false;
            }
            else if (delta < slewSpeedFast * 2)
            {
                change.X = slewSpeedMedium * signDelta;
                finished = false;
            }
            else
            {
                change.X = slewSpeedFast * signDelta;
                finished = false;
            }
            delta = targetAxes.Y - mountAxes.Y;
            while (delta < -180 || delta > 180)
            {
                if (delta < -180) delta += 360;
                if (delta > 180) delta -= 360;
            }
            signDelta = delta < 0 ? -1 : +1;
            delta = Math.Abs(delta);
            if (delta < slewSpeedSlow)
            {
                change.Y = delta * signDelta;
            }
            else if (delta < slewSpeedMedium * 2)
            {
                change.Y = slewSpeedSlow * signDelta;
                finished = false;
            }
            else if (delta < slewSpeedFast * 2)
            {
                change.Y = slewSpeedMedium * signDelta;
                finished = false;
            }
            else
            {
                change.Y = slewSpeedFast * signDelta;
                finished = false;
            }
            if (finished)
            {
                slewing = false;
                switch (SlewState)
                {
                    case SlewType.SlewRaDec:
                    case SlewType.SlewAltAz:
                        SlewState = SlewType.SlewSettle;
                        settleTime = DateTime.Now + TimeSpan.FromSeconds(SlewSettleTime);
                        LogMessage("Settle", "Moved from slew to settle");
                        break;
                    case SlewType.SlewPark:
                        SlewState = SlewType.SlewNone;
                        ChangePark(true);
                        break;
                    case SlewType.SlewHome:
                        SlewState = SlewType.SlewNone;
                        break;
                    case SlewType.SlewNone:
                        break;
                    //case SlewType.SlewSettle:
                    //    break;
                    //case SlewType.SlewMoveAxis:
                    //    break;
                    //case SlewType.SlewHandpad:
                    //    break;
                    default:
                        SlewState = SlewType.SlewNone;
                        break;
                }
            }
            return change;
        }

        /// <summary>
        /// return the change in axis values as a result of any HC button presses
        /// </summary>
        /// <returns></returns>
        private static Vector HcMoves()
        {
            Vector change = new Vector();
            if (SlewDirection == Simulator.SlewDirection.SlewNone)
            {
                return change;
            }
            // handle HC button moves
            double delta = 0;
            switch (SlewSpeed)
            {
                case SlewSpeed.SlewSlow:
                    delta = slewSpeedSlow;
                    break;
                case SlewSpeed.SlewMedium:
                    delta = slewSpeedMedium;
                    break;
                case SlewSpeed.SlewFast:
                    delta = slewSpeedFast;
                    break;
            }
            // check the button states
            switch (SlewDirection)
            {
                case SlewDirection.SlewNorth:
                case SlewDirection.SlewUp:
                    change.Y = delta;
                    break;
                case SlewDirection.SlewSouth:
                case SlewDirection.SlewDown:
                    change.Y = -delta;
                    break;
                case SlewDirection.SlewEast:
                case SlewDirection.SlewLeft:
                    change.X = delta;
                    break;
                case SlewDirection.SlewWest:
                case SlewDirection.SlewRight:
                    change.X = -delta;
                    break;
                case Simulator.SlewDirection.SlewNone:
                    break;
            }
            return change;
        }

        /// <summary>
        /// Return the axis change as a result of any pulse guide operation during the update interval
        /// </summary>
        /// <param name="updateInterval">The update interval.</param>
        /// <returns></returns>
        private static Vector PulseGuide(double updateInterval)
        {
            Vector change = new Vector();
            // PulseGuide implementation
            if (isPulseGuidingRa)
            {
                if (guideDuration.X <= 0)
                {
                    isPulseGuidingRa = false;
                }
                else
                {
                    // assume polar mount only
                    var gd = guideDuration.X > updateInterval ? updateInterval : guideDuration.X;
                    guideDuration.X -= gd;
                    // assumes guide rate is in deg/sec
                    change.X = guideRate.X * gd;
                }
            }
            if (isPulseGuidingDec)
            {
                if (guideDuration.Y <= 0)
                {
                    isPulseGuidingDec = false;
                }
                else
                {
                    var gd = guideDuration.Y > updateInterval ? updateInterval : guideDuration.Y;
                    guideDuration.Y -= gd;
                    change.Y = guideRate.Y * gd;
                }
            }
            return change;
        }

        /// <summary>
        /// Checks the axis limits. AltAz and Polar mounts allow continuous movement,
        /// it is set to a sensible range.
        /// GEM mounts check the hour angle limit and stop movement past it.
        /// </summary>
        /// <param name="primaryChange">The primary change.</param>
        private static void CheckAxisLimits(double primaryChange)
        {
            // check the ranges of the axes
            // primary axis must be in the range 0 to 360 for AltAz or Polar
            // and -hourAngleLimit to 180 + hourAngleLimit for german polar
            switch (alignmentMode)
            {
                case AlignmentModes.algAltAz:
                    // the primary axis must be in the range 0 to 360
                    mountAxes.X = AstronomyFunctions.RangeAzimuth(mountAxes.X);
                    break;
                case AlignmentModes.algGermanPolar:
                    // the primary axis needs to be in the range -180 to +180 to correspond with hour angles
                    // of -12 to 12.
                    // check if we have hit the hour angle limit
                    if ((mountAxes.X >= hourAngleLimit + 180 && primaryChange > 0) ||
                        (mountAxes.X <= -hourAngleLimit && primaryChange < 0))
                    {
                        // undo the movement when the limit is hit
                        mountAxes.X -= primaryChange;
                    }
                    break;
                case AlignmentModes.algPolar:
                    // the axis needs to be in the range -180 to +180 to correspond with hour angles
                    // of -12 to 12.
                    while (mountAxes.X <= -180.0 || mountAxes.X > 180.0)
                    {
                        if (mountAxes.X <= -180.0) mountAxes.X += 360;
                        if (mountAxes.X > 180) mountAxes.X -= 360;
                    }
                    break;
            }
            // secondary must be in the range -90 to 0 to +90 for normal 
            // and +90 to 180 to 270 for through the pole.
            // rotation is continuous
            while (mountAxes.Y >= 270.0 || mountAxes.Y < -90.0)
            {
                if (mountAxes.Y >= 270) mountAxes.Y -= 360.0;
                if (mountAxes.Y < -90) mountAxes.Y += 360.0;
            }
        }

        /// <summary>
        /// Convert the move rate in hour angle to a change in altitude and azimuth
        /// </summary>
        /// <param name="haChange">The ha change.</param>
        /// <returns></returns>
        private static Vector ConvertRateToAltAz(double haChange)
        {
            Vector change = new Vector();

            double latRad = latitude * SharedResources.DEG_RAD;
            double azmRad = altAzm.X * SharedResources.DEG_RAD;
            double zenithAngle = (90 - altAzm.Y) * SharedResources.DEG_RAD;     // in radians

            // get the azimuth and elevation rates, as a ratio of the tracking rate
            double elevationRate = Math.Sin(azmRad) * Math.Cos(latRad);
            // fails at zenith so set a very large value, the limit check will trap this
            double azimuthRate =
                altAzm.Y != 90.0 ?
                (Math.Sin(latRad) * Math.Sin(zenithAngle) - Math.Cos(latRad) * Math.Cos(zenithAngle) * Math.Cos(azmRad)) / Math.Sin(zenithAngle) :
                altAzm.X >= 90 && altAzm.X <= 270 ? 10000 : -10000;

            // get the changes in altitude and azimuth using the hour angle change and rates.
            change.Y = elevationRate * haChange;
            change.X = azimuthRate * haChange;
            // stop the secondary going past the vertical
            if (change.Y > 90 - altAzm.Y)
                change.Y = 0;
            // limit the primary to the maximum slew rate
            if (change.X < -slewSpeedFast)
                change.X = -slewSpeedFast;
            if (change.X > slewSpeedFast)
                change.X = slewSpeedFast;
            return change;
        }

        /// <summary>
        /// Update the mount positions and state from the axis positions
        /// </summary>
        private static void UpdatePositions()
        {
            SiderealTime = AstronomyFunctions.LocalSiderealTime(Longitude);

            pointingState = mountAxes.Y <= 90 ? PointingState.Normal : PointingState.ThroughThePole;

            altAzm = MountFunctions.ConvertAxesToAltAzm(mountAxes);
            currentRaDec = MountFunctions.ConvertAxesToRaDec(mountAxes);

            UpdateDisplay();
        }

        private static void UpdateDisplay()
        {
            // display the values, must be done on the UI thread
            TelescopeSimulator.m_MainForm.SiderealTime(SiderealTime);
            TelescopeSimulator.m_MainForm.Altitude(Altitude);
            TelescopeSimulator.m_MainForm.Azimuth(Azimuth);
            TelescopeSimulator.m_MainForm.RightAscension(RightAscension);
            TelescopeSimulator.m_MainForm.Declination(Declination);
            TelescopeSimulator.m_MainForm.Tracking();
            TelescopeSimulator.m_MainForm.LedPier(SideOfPier);

            if (AtPark) TelescopeSimulator.m_MainForm.lblPARK.ForeColor = Color.Red;
            else TelescopeSimulator.m_MainForm.lblPARK.ForeColor = Color.SaddleBrown;
            if (AtHome) TelescopeSimulator.m_MainForm.lblHOME.ForeColor = Color.Red;
            else TelescopeSimulator.m_MainForm.lblHOME.ForeColor = Color.SaddleBrown;
            if (!IsSlewing) TelescopeSimulator.m_MainForm.labelSlew.ForeColor = Color.SaddleBrown;
            else TelescopeSimulator.m_MainForm.labelSlew.ForeColor = Color.Red;
        }

        public static void StartSlewRaDec(double rightAscension, double declination, bool doSideOfPier)
        {
            Vector raDec = new Vector(rightAscension, declination);
            targetAxes = MountFunctions.ConvertRaDecToAxes(raDec);

            StartSlewAxes(targetAxes, SlewType.SlewRaDec);
            LogMessage("StartSlewRaDec", "Ra {0}, dec {1}, doSOP {2}", rightAscension, declination, doSideOfPier);
        }

        public static void StartSlewAltAz(double altitude, double azimuth)
        {
            LogMessage("StartSlewAltAz", "{0}, {1}", altitude, azimuth);
            StartSlewAltAz(new Vector(azimuth, altitude));
            return;
            //switch (AlignmentMode)
            //{
            //    case AlignmentModes.algAltAz:
            //        targetAxes = new Vector(azimuth, altitude);
            //        break;
            //    case AlignmentModes.algGermanPolar:
            //        var ra = AstronomyFunctions.CalculateRA(altitude, azimuth, latitude, longitude);
            //        targetAxes.X = AstronomyFunctions.HourAngle(ra, longitude) * 15.0;
            //        targetAxes.Y = AstronomyFunctions.CalculateDec(altitude, azimuth, latitude);
            //        if (targetAxes.X > 180.0 || targetAxes.Y < 0)
            //        {
            //            // adjust the targets to be through the pole
            //            targetAxes.X += 180;
            //            targetAxes.Y = 180 - targetAxes.Y;
            //        }
            //        if (longitude < 0)
            //        {
            //            targetAxes.Y = -targetAxes.Y;
            //        }
            //        break;
            //    case AlignmentModes.algPolar:
            //        ra = AstronomyFunctions.CalculateRA(altitude, azimuth, latitude, longitude);
            //        targetAxes.X = AstronomyFunctions.HourAngle(ra, longitude) * 15.0;
            //        targetAxes.Y = AstronomyFunctions.CalculateDec(altitude, azimuth, latitude);
            //        break;
            //}
            //slewing = true;
            //SlewState = SlewType.SlewAltAz;

            //ChangePark(false);
        }

        public static void StartSlewAltAz(Vector targetAltAzm)
        {
            LogMessage("StartSlewAltAz", "Azm {0}, Alt {1}", targetAltAzm.X, targetAltAzm.Y);

            Vector target = MountFunctions.ConvertAltAzmToAxes(targetAltAzm);
            if (target.LengthSquared > 0)
            {
                StartSlewAxes(target, SlewType.SlewAltAz);
            }
        }

        public static void StartSlewAxes(double primaryAxis, double secondaryAxis, SlewType slewState)
        {
            StartSlewAxes(new Vector(primaryAxis, secondaryAxis), slewState);
        }

        /// <summary>
        /// Starts a slew to the target position in degrees.
        /// </summary>
        /// <param name="targetPosition">The position.</param>
        public static void StartSlewAxes(Vector targetPosition, SlewType slewState)
        {
            targetAxes = targetPosition;
            SlewState = slewState;
            slewing = true;
            ChangePark(false);
        }

        public static bool IsSlewing
        {
            get
            {
                if (SlewState != SlewType.SlewNone)
                    return true;
                if (slewing)
                    return true;
                if (rateAxes.LengthSquared != 0)
                    return true;
                if (rateRaDec.LengthSquared != 0)
                    return true;
                return slewing && rateAxes.Y != 0 && rateAxes.X != 0;
            }
        }

        public static void AbortSlew()
        {
            slewing = false;
            rateAxes = new Vector();
            rateRaDec = new Vector();
            SlewState = SlewType.SlewNone;
        }

        public static void SyncToTargetRaDec()
        {
            mountAxes = MountFunctions.ConvertRaDecToAxes(targetRaDec);
            UpdatePositions();
        }

        public static void SyncToAltAzm(double targetAzimuth, double targetAltitude)
        {
            mountAxes = MountFunctions.ConvertAltAzmToAxes(new Vector(targetAzimuth, targetAltitude));
            UpdatePositions();
        }

        #endregion

        public static double AvailableTimeInThisPointingState
        {
            get
            {
                if (AlignmentMode != AlignmentModes.algGermanPolar)
                {
                    return double.MaxValue;
                }
                double degToLimit = mountAxes.X + hourAngleLimit + 360;
                while (degToLimit > 180) degToLimit -= 360;
                return degToLimit * 240;
            }
        }

        public static double TimeUntilPointingStateCanChange
        {
            get
            {
                if (AlignmentMode != AlignmentModes.algGermanPolar)
                {
                    return double.MaxValue;
                }
                var degToLimit = mountAxes.X - hourAngleLimit + 360;
                while (degToLimit > 180) degToLimit -= 360;
                return degToLimit * 240;
            }
        }
    }

    internal enum TrackingMode
    {
        Off,
        AltAz,
        EqN,
        EqS
    }
}
