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
// 18-SEP-2012  Rick Burke  Improved support for simulating pulse guiding
// May/June 2014 cdr 6.1    Change the telescope hardware to use an axis based method
// May 2022     Rick Burke	Corrected E/W button movement directions
//                          Hid MoveAxis and PulseGuide buttons since PulseGuide movement initiated by the buttons had been disabled.
// --------------------------------------------------------------------------------
//

// Ignore Spelling: Unsubscriber Lat Elev Utils astro util

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Collections.Generic;
using ASCOM.Astrometry.AstroUtils;

namespace ASCOM.Simulator
{
    public static class TelescopeHardware
    {
        #region How the simulator works and Operations

        // OPERATIONS
        
        // Operations were part of a PoC to introduce independent completion variables for all async methods that was implemented in June / July 2023 as part of Platform 7 feature testing.
        // These completion variables would return true /false or, when something went wrong, would throw exceptions continually until the next operation was started.

        // The TelescopeHardware.OperationComplete property is not currently exposed through the driver's ITelescopeV4 interface but
        // the code is left in place in case it is useful in the future.

        // HOW THE SIMULATOR WORKS

        // The telescope is implemented using two axes that represent the primary and secondary telescope axes.
        // The role of the axes varies depending on the mount type.
        // The primary axis is the azimuth axis for an AltAz mount and the hour angle axis for polar mounts.
        // The secondary axis is the altitude axis for AltAz mounts and the declination axis for polar mounts.

        // All motion is done and all positions are set by manipulating the axis angle values.

        // Vectors are used for pairs of angles that represent the various positions and rates
        // Vector.X is the primary axis = Axis angle, Hour angle, Right Ascension or Azimuth and Vector.Y is the secondary axis = Axis angle, Declination or Altitude.

        // Ra and hour angle vectors are in hours
        // Mount position, Declination, azimuth and altitude vectors are in degrees.

        // NORTHERN HEMISPHERE AXIS CONVENTIONS
        //
        // ALT/AZ ALIGNMENT - Slews always use the Normal (pierEast) pointing state even when the mount has been manually set to the TTP pointing state
        //   Primary axis - 0 degrees = North with value increasing clockwise giving 90 degrees = East, 180 degrees = South and 270 = West.
        //   Secondary axis, 0 degrees at the horizon increasing to 90 degrees at the zenith

        // POLAR ALIGNMENT - Slews always use the Normal (pierEast) pointing state even when the mount has been manually set to the TTP pointing state
        //   Primary axis - 0 degrees = hour angle 0, -180 degrees = Hour angle -12 and +180 degrees = hour angle +12
        //   Secondary axis, 0 degrees at declination 0, +90 at the northern equatorial pole. Negative values at negative declinations towards the south.

        // GERMAN POLAR ALIGNMENT - Normal pointing state (pierEast) i.e. when observing hour angles 0 to +12.
        //   Primary axis - 0 degrees = hour angle 0, +180 degrees = hour angle +12
        //   Secondary axis - 0 degrees at declination 0, +90 at the northern equatorial pole. Negative values at negative declinations towards the south.

        // GERMAN POLAR ALIGNMENT - Through The Pole pointing state (pierWest) i.e. When observing hour angles -12 to 0.
        //   Primary axis - 0 degrees = hour angle -12, +180 degrees = hour angle 0
        //   Secondary axis - +90 at the equatorial pole. 180-declination at smaller declinations. Note that at negative declinations the axis angle will be >180

        // SOUTHERN HEMISPHERE AXIS CONVENTIONS
        //
        // ALT/AZ ALIGNMENT - Slews always use the Normal (pierEast) pointing state even when the mount has been manually set to the TTP pointing state
        //   Primary axis - 0 degrees = North with value increasing clockwise giving 90 degrees = East, 180 degrees = South and 270 = West.
        //   Secondary axis, 0 degrees at the horizon increasing to 90 degrees at the zenith

        // POLAR ALIGNMENT - Slews always use the Normal (pierEast) pointing state even when the mount has been manually set to the TTP pointing state
        //   Primary axis - 0 degrees = hour angle 0, -180 degrees = Hour angle -12 and +180 degrees = hour angle +12
        //   Secondary axis, 0 degrees at declination 0, +90 at the southern equatorial pole. Negative values at positive declinations towards the north.

        // GERMAN POLAR ALIGNMENT - Normal pointing state (pierEast) i.e. when observing hour angles 0 to +12.
        //   Primary axis - 0 degrees = hour angle 0, +180 degrees = hour angle +12
        //   Secondary axis - 0 degrees at declination 0, +90 at the southern equatorial pole. Negative values at positive declinations towards the north.

        // GERMAN POLAR ALIGNMENT - Through The Pole pointing state (pierWest) i.e. When observing hour angles -12 to 0.
        //   Primary axis - 0 degrees = hour angle -12, +180 degrees = hour angle 0
        //   Secondary axis - +90 at the equatorial pole. 180-declination at smaller declinations. Note that at positive declinations the axis angle will be >180

        #endregion

        #region Constants

        // Start-up options values       
        private const string STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION = "Start up at simulator Default Position";
        private const string STARTUP_OPTION_START_POSITION = "Start up at configured Start Position";
        private const string STARTUP_OPTION_PARKED_POSITION = "Start up at configured Park Position";
        private const string STARTUP_OPTION_LASTUSED_POSITION = "Start up at last Shutdown Position";
        private const string STARTUP_OPTION_HOME_POSITION = "Start up at configured Home Position";
        private const double SIDEREAL_SECONDS_TO_SI_SECONDS = 0.99726956631945; // Based on earth sidereal rotation period of 23 hours 56 minutes 4.09053 seconds

        // Useful mathematical constants
        private const double SIDEREAL_RATE_DEG_PER_SIDEREAL_SECOND = 360.0 / (24.0 * 60.0 * 60.0); // Degrees per sidereal second, given the earth's rotation of 360 degrees in 1 sidereal day
        private const double SIDEREAL_RATE_DEG_PER_SI_SECOND = SIDEREAL_RATE_DEG_PER_SIDEREAL_SECOND / SIDEREAL_SECONDS_TO_SI_SECONDS; // Degrees per SI second
        private const double SOLAR_RATE_DEG_SEC = 15.0 / 3600.0;
        private const double LUNAR_RATE_DEG_SEC = 14.515 / 3600.0;
        private const double KING_RATE_DEG_SEC = 15.037 / 3600.0;
        private const double DEGREES_TO_ARCSECONDS = 3600.0;
        private const double ARCSECONDS_TO_DEGREES = 1.0 / 3600.0;
        private const double ARCSECONDS_PER_RA_SECOND = 15.0; // To convert "seconds of RA" (24 hours = a whole circle) to arc seconds (360 degrees = a whole circle)
        private const double DEG_RAD = 0.01745329251994329;
        private const double RAD_DEG = 57.29577951308232;
        private const double HRS_RAD = 0.2617993877991494;

        #endregion

        #region Private variables

        private static short interfaceVersion = 4;
        private static Operation currentOperation = Operation.None; // Initialise to no operation

        // change to using a Windows timer to avoid threading problems
        private static System.Windows.Forms.Timer updateStateTimer;
        private static System.Windows.Forms.Timer updateHandboxTimer;

        private static readonly object updateLockObject = new object();

        private static long idCount; // Counter to generate ever increasing sequential ID numbers

        // this emulates a hardware connection
        // the dictionary maintains a list of connected drivers, a dictionary is used so only one of each
        // driver is maintained.
        // Connected will be reported as true if any driver is connected
        // each driver instance has a unique id generated using ObjectIDGenerator
        private static ConcurrentDictionary<long, bool> connectStates;// = new ConcurrentDictionary<long, bool>();
        private static readonly object getIdLockObj = new object();

        private static Profile s_Profile;
        private static bool onTop;
        public static TraceLogger TL;
        public static Util util;
        public static AstroUtils astroUtils;

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
        private static bool canDestinationSideOfPier;
        private static bool canTrackingRates;

        //Telescope Implementation
        private static AlignmentModes alignmentMode;
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
        private static bool noSyncPastMeridian;

        private static bool atPark;

        //
        // Vectors are used for pairs of angles that represent the various positions and rates
        //
        // X is the primary axis, Hour angle, Right Ascension or azimuth and Y is the secondary axis,
        // declination or altitude.
        //
        // Ra and hour angle are in hours and the mount positions, Declination, azimuth and altitude are in degrees.
        //

        /// <summary>
        /// Current azimuth (X) and altitude (Y )in degrees, derived from the mountAxes Vector
        /// </summary>
        private static Vector currentAltAzm;

        /// <summary>
        /// Park axis positions, X primary, Y secondary in Alt/Az degrees
        /// </summary>
        private static Vector parkPosition;

        /// <summary>
        /// current Ra (X, hrs) and Dec (Y, deg), derived from the mount axes
        /// </summary>
        private static Vector currentRaDec;

        /// <summary>
        /// Target right ascension (X, hrs) and declination (Y, deg)
        /// </summary>
        private static Vector targetRaDec;

        /// <summary>
        /// Flag to say which Telescope position will be used when the simulator is started
        /// </summary>
        private static string startupMode;

        private static DateTime settleTime;

        //private static SlewType slewState;

        // speeds are in deg/sec.
        private static double slewSpeedFast;
        private static double slewSpeedMedium;
        private static double slewSpeedSlow;

        /// <summary>
        /// Shutdown position in Alt/Az degrees
        /// </summary>
        private static Vector shutdownPosition = new Vector();

        /// <summary>
        /// Internal versions of the Right Ascension and declination rates in mount simulator units
        /// This "Internal" vector holds values in the units used internally by the simulator (degrees per SI second)
        /// </summary>
        private static Vector rateRaDecOffsetInternal = new Vector();

        /// <summary>
        /// Right Ascension and declination rates as set through the RightAscensionRate and DeclinationRate properties
        /// This "External" vector holds values in the units specified in the telescope interface standard (arc-seconds per sidereal second for RightAscensionRate and arc-seconds per SI second for DeclinationRate)
        /// </summary>
        private static Vector rateRaDecOffsetExternal = new Vector();

        private static int dateDelta;

        // Telescope mount simulation variables
        // The telescope is implemented using two axes that represent the primary and secondary telescope axes.
        // The role of the axes varies depending on the mount type.
        // The primary axis is the azimuth axis for an AltAz mount
        // and the hour angle axis for polar mounts.
        // The secondary axis is the altitude axis for AltAz mounts and the declination axis for polar mounts.
        //
        // all motion is done and all positions are set and obtained using these axes.
        //

        /// <summary>
        /// Axis position in mount axis degrees. X is primary (RA or Azimuth axis), Y is secondary (Dec or Altitude axis)
        /// </summary>
        private static Vector mountAxesDegrees;

        /// <summary>
        /// Slew target in mount axis degrees
        /// </summary>
        private static Vector targetAxesDegrees;

        private static double hourAngleLimit = 20;     // the number of degrees a GEM can go past the meridian

        //private static PointingState pointingState;
        private static TrackingMode trackingMode;
        private static bool slewing;

        private static Stopwatch timeSinceLastUpdate;

        #endregion

        #region Internal variables

        // durations are in secs.
        internal static double GuideDurationShort { get; private set; }

        internal static double GuideDurationMedium { get; private set; }

        internal static double GuideDurationLong { get; private set; }

        // Internal variables used to communicate with the Start-up / Park / Home configuration form
        /// <summary>
        /// Start position in Alt/Az degrees
        /// </summary>
        internal static Vector StartCoordinates = new Vector();

        /// <summary>
        /// Home position - X = Azimuth, Y= Altitude (degrees)
        /// </summary>
        internal static Vector HomePosition;

        internal static List<string> StartupOptions;

        #endregion

        #region Public variables

        /// <summary>
        /// Name of the current operation.
        /// </summary>
        public static Operation CurrentOperation
        {
            get
            {
                // Return the current operation
                return currentOperation;
            }

            set
            {
                // Assign the new operation as the current operation
                currentOperation = value;
            }
        }

        public static bool OperationComplete
        {
            get
            {
                //TL?.LogMessage("OperationComplete - GET", $"{operationComplete}");
                return operationComplete;
            }

            set
            {
                operationComplete = value;
                //TL?.LogMessage("OperationComplete - SET", $"{operationComplete}");
            }
        }
        public static Exception OperationException { get; set; }

        /// <summary>
        /// Guide rates, deg/sec. X Ra/Azm, Y Alt/Dec
        /// </summary>
        public static Vector guideRate = new Vector();

        public static bool isPulseGuidingRa;

        public static bool isPulseGuidingDec;

        /// <summary>
        /// duration in seconds for guiding
        /// </summary>
        public static Vector guideDuration = new Vector();

        /// <summary>
        /// Axis Rates (deg/sec) set by the MoveAxis method
        /// </summary>
        public static Vector rateMoveAxes = new Vector();
        private static bool operationComplete;

        #endregion

        #region  Enums
        internal enum TrackingMode
        {
            Off,
            AltAz,
            EqN,
            EqS
        }

        private enum PointingState
        {
            Normal,
            ThroughThePole
        }

        #endregion

        #region Initialiser, Simulator start and timer functions

        /// <summary>
        /// Static initialiser for the TelescopeHardware class
        /// </summary>
        static TelescopeHardware()
        {
            try
            {
                s_Profile = new Utilities.Profile();
                updateStateTimer = new System.Windows.Forms.Timer();
                updateStateTimer.Interval = Convert.ToInt32(SharedResources.STATE_UPDATE_TIMER_INTERVAL * 1000.0);
                updateStateTimer.Tick += UpdateStateTimer_Tick;

                updateHandboxTimer = new System.Windows.Forms.Timer();
                updateHandboxTimer.Interval = Convert.ToInt32(SharedResources.HANDBOX_UPDATE_TIMER_INTERVAL * 1000.0);
                updateHandboxTimer.Tick += UpdateHandboxTimer_Tick;

                // Initialise Operation variables
                OperationComplete = true;
                OperationException = null;

                // Initialise the time since last update stopwatch timer
                timeSinceLastUpdate = new Stopwatch();

                SouthernHemisphere = false;
                //Connected = false;
                rateMoveAxes = new Vector();

                TL = new TraceLogger("", "TelescopeSimHardware");
                TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.SIMULATOR_TRACE, GlobalConstants.SIMULATOR_TRACE_DEFAULT);

                // Create utilities objects
                util = new Util();
                astroUtils = new AstroUtils();

                TL.LogMessage("TelescopeHardware", string.Format("Alignment mode 1: {0}", alignmentMode));
                connectStates = new ConcurrentDictionary<long, bool>();
                idCount = 0; // Initialise count to zero

                // Populate the start-up options collection
                StartupOptions = new List<string>() { STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION, STARTUP_OPTION_LASTUSED_POSITION, STARTUP_OPTION_START_POSITION, STARTUP_OPTION_PARKED_POSITION, STARTUP_OPTION_HOME_POSITION };

                // check if the profile settings are correct 
                if (s_Profile.GetValue(SharedResources.PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
                {
                    // load the default settings
                    //Main Driver Settings
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlwaysOnTop", "false");

                    // Telescope Implementation
                    // Initialise mount type to German Polar
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1"); // 1 = Start as German Polar m9ount type
                    alignmentMode = AlignmentModes.algGermanPolar; // Added by Peter because the Profile setting was set to German Polar but the alignment mode value at this point was still zer0 = Alt/Az!

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

                    // set default home and configured start positions
                    HomePosition = new Vector();
                    TL.LogMessage("TelescopeHardware", string.Format("Alignment mode 2: {0}", alignmentMode));
                    switch (alignmentMode)
                    {
                        case AlignmentModes.algGermanPolar:
                            // looking at the pole, counterweight down
                            HomePosition.X = 0;
                            HomePosition.Y = lat;
                            TL.LogMessage("TelescopeHardware", string.Format("German Polar - Setting HomeAxes to {0} {1}", HomePosition.X.ToString(CultureInfo.InvariantCulture), HomePosition.Y.ToString(CultureInfo.InvariantCulture)));
                            s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuthConfigured", HomePosition.X.ToString(CultureInfo.InvariantCulture));
                            s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAltitudeConfigured", HomePosition.Y.ToString(CultureInfo.InvariantCulture));
                            break;
                        case AlignmentModes.algPolar:
                            // looking East, tube level
                            HomePosition.X = 90;
                            HomePosition.Y = 0;
                            TL.LogMessage("TelescopeHardware", string.Format("Polar - Setting HomeAxes to {0} {1}", HomePosition.X.ToString(CultureInfo.InvariantCulture), HomePosition.Y.ToString(CultureInfo.InvariantCulture)));
                            s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAltitudeConfigured", HomePosition.X.ToString(CultureInfo.InvariantCulture));
                            s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuthConfigured", HomePosition.Y.ToString(CultureInfo.InvariantCulture));
                            break;
                        case AlignmentModes.algAltAz:
                            HomePosition.X = 0;    // AltAz is North and Level, hope Meade don't mind!
                            HomePosition.Y = 0;
                            TL.LogMessage("TelescopeHardware", string.Format("Alt/Az - Setting HomeAxes to {0} {1}", HomePosition.X.ToString(CultureInfo.InvariantCulture), HomePosition.Y.ToString(CultureInfo.InvariantCulture)));
                            s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAltitudeConfigured", HomePosition.X.ToString(CultureInfo.InvariantCulture));
                            s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuthConfigured", HomePosition.Y.ToString(CultureInfo.InvariantCulture));
                            break;
                    }
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "HomeAzimuth", HomePosition.X.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "HomeAltitude", HomePosition.Y.ToString(CultureInfo.InvariantCulture));

                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ShutdownAzimuth", HomePosition.X.ToString(CultureInfo.InvariantCulture)); // Set some default last shutdown values
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ShutdownAltitude", HomePosition.Y.ToString(CultureInfo.InvariantCulture));

                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartupMode", STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION); // Set the original simulator behaviour as the default start-up mode

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
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDestinationSideOfPier", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanTrackingRates", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", "true", "Capabilities");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "InterfaceVersion", "4", "Capabilities");
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

                currentAltAzm.Y = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAltitude", "", "0"), CultureInfo.InvariantCulture); // Get the default start position
                currentAltAzm.X = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAzimuth", "", "0"), CultureInfo.InvariantCulture);
                StartCoordinates.Y = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAltitudeConfigured", "", "0"), CultureInfo.InvariantCulture); // Get the configured start position
                StartCoordinates.X = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAzimuthConfigured", "", "0"), CultureInfo.InvariantCulture);

                parkPosition.Y = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAltitude", "", "0"), CultureInfo.InvariantCulture);
                parkPosition.X = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAzimuth", "", "0"), CultureInfo.InvariantCulture);

                // Retrieve the Home position
                HomePosition.X = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "HomeAzimuth", "", "0"), CultureInfo.InvariantCulture);
                HomePosition.Y = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "HomeAltitude", "", "0"), CultureInfo.InvariantCulture);

                // Retrieve the previous shutdown position
                shutdownPosition.X = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ShutdownAzimuth", "", "0"), CultureInfo.InvariantCulture);
                shutdownPosition.Y = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ShutdownAltitude", "", "0"), CultureInfo.InvariantCulture);

                // Retrieve the start-up mode
                startupMode = s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartupMode", "", STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION);

                // Select the configured start-up position
                switch (startupMode)
                {
                    case STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION: // No action just go with the built-in values already in altAzm
                        break;
                    case STARTUP_OPTION_PARKED_POSITION:
                        currentAltAzm = parkPosition;
                        break;
                    case STARTUP_OPTION_START_POSITION:
                        currentAltAzm = StartCoordinates;
                        break;
                    case STARTUP_OPTION_LASTUSED_POSITION:
                        currentAltAzm = shutdownPosition;
                        break;
                    case STARTUP_OPTION_HOME_POSITION:
                        currentAltAzm = HomePosition;
                        break;
                    default: // No action just go with the built-in simulator start-up position already in altAzm
                        break;
                }

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
                canDestinationSideOfPier = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDestinationSideOfPier", "Capabilities", "True"));
                canTrackingRates = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanTrackingRates", "Capabilities"));
                canDualAxisPulseGuide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", "Capabilities"));
                noSyncPastMeridian = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "NoSyncPastMeridian", "Capabilities", "false"));

                dateDelta = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "DateDelta"), CultureInfo.InvariantCulture);

                interfaceVersion = short.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "InterfaceVersion", "", "4"), CultureInfo.InvariantCulture);

                if (latitude < 0) { SouthernHemisphere = true; }

                if (TelescopeSimulator.m_MainForm == null)
                {
                    TelescopeSimulator.m_MainForm = new FrmMain();
                }

                //Set the form setting for the Always On Top Value
                TelescopeSimulator.m_MainForm.TopMost = onTop;

                slewSpeedFast = maximumSlewRate * SharedResources.STATE_UPDATE_TIMER_INTERVAL;
                slewSpeedMedium = slewSpeedFast * 0.1;
                slewSpeedSlow = slewSpeedFast * 0.02;
                SlewDirection = SlewDirection.SlewNone;

                GuideDurationShort = 0.8 * SharedResources.STATE_UPDATE_TIMER_INTERVAL;
                GuideDurationMedium = 2.0 * GuideDurationShort;
                GuideDurationLong = 2.0 * GuideDurationMedium;

                // Set default pulse guide rates
                //   Guide rate RA:          15 arc seconds per SI second
                //   Guide rate Declination: 15 arc seconds per SI second
                guideRate.X = 15.0 * (1.0 / 3600.0); // Degrees per SI second
                guideRate.Y = 15.0 * (1.0 / 3600.0); // Degrees per SI second
                rateRaDecOffsetInternal.Y = 0;
                rateRaDecOffsetInternal.X = 0;

                TrackingRate = DriveRates.driveSidereal;
                SlewSettleTime = 0;
                ChangePark(AtPark);

                // invalid target position
                targetRaDec = new Vector(double.NaN, double.NaN);
                SlewState = SlewType.SlewNone;

                mountAxesDegrees = MountFunctions.ConvertAltAzmToAxes(currentAltAzm); // Convert the start position AltAz coordinates into the current axes representation and set this as the simulator start position
                TL.LogMessage("TelescopeHardware New", string.Format("Start-up mode: {0}, Azimuth: {1}, Altitude: {2}", startupMode, currentAltAzm.X.ToString(CultureInfo.InvariantCulture), currentAltAzm.Y.ToString(CultureInfo.InvariantCulture)));
                TL.LogMessage("TelescopeHardware New", $"Tracking rate: {SIDEREAL_RATE_DEG_PER_SI_SECOND} degrees per SI second, {SIDEREAL_RATE_DEG_PER_SI_SECOND * 3600.0} degrees per SI hour. Sidereal day:{util.HoursToHMS(SIDEREAL_SECONDS_TO_SI_SECONDS * 24.0, ":", ":", "", 3)} HH:MM:SS.xxx");
                TL.LogMessage("TelescopeHardware New", "Successfully initialised hardware");

            }
            catch (Exception ex)
            {
                EventLogCode.LogEvent("ASCOM.Simulator.Telescope", "TelescopeHardware Initialiser Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.TelescopeSimulatorNew, ex.ToString());
                System.Windows.Forms.MessageBox.Show("Telescope HardwareInitialise: " + ex.ToString());
            }
        }

        private static void UpdateHandboxTimer_Tick(object sender, EventArgs e)
        {
            lock (updateLockObject)
            {
                UpdateDisplay();
            }
        }

        public static void Start()
        {
            // Start with the configured Tracking state and the mount unparked
            Tracking = AutoTrack;
            AtPark = false;

            rateMoveAxes.X = 0;
            rateMoveAxes.Y = 0;

            // Initialise last time since last update timer 
            timeSinceLastUpdate.Start();

            // Initialise the sidereal time
            SiderealTime = AstronomyFunctions.LocalSiderealTime(Longitude);

            // Initialise movement pointers and create initial display
            MoveAxes();
            UpdateDisplay();

            // Start the timer that updates telescope state
            updateStateTimer.Start();
            updateHandboxTimer.Start();
            TL.LogMessage("Start", "State and hand box timers started.");
        }

        //Update the Telescope Based on Timed Events
        private static void UpdateStateTimer_Tick(object sender, EventArgs e)
        {
            //TL.LogMessage("UpdateStateTimer_Tick", "Updating state...");
            lock (updateLockObject)
            {
                MoveAxes();
            }
        }

        /// <summary>
        /// This is called every TIMER_INTERVAL period and applies the current movement rates to the axes,
        /// copes with the range and updates the displayed values
        /// </summary>
        /// <remarks>
        /// Axis assignment in telescope alignment modes 
        /// Polar / German Polar: Primary axis is RA (X coordinate in vector maths)      - Secondary axis is Declination (Y coordinate in vector maths)
        /// Alt/Az mount mode:    Primary axis is Azimuth (X coordinate in vector maths) - Secondary axis is Altitude (Y coordinate in vector maths)
        /// 
        /// In Alt/Az mode: north = azimuth 0, east = azimuth 90, south=azimuth 180 and west = azimuth 270
        /// </remarks>
        private static void MoveAxes()
        {
            // TL.LogMessage("MoveAxes", "Starting state update...");

            // Get the time since the last update. This avoids problems with the timer interval varying and greatly improves tracking.
            double timeInSecondsSinceLastUpdate = timeSinceLastUpdate.Elapsed.TotalSeconds;
            timeSinceLastUpdate.Restart();

            // Find the hour angle change (in degrees) due to tracking that occurred during this interval
            double haChangeDegrees = GetTrackingChangeInDegrees(timeInSecondsSinceLastUpdate);

            // This vector accumulates all changes to the current primary and secondary axis positions as a result of movement during this update interval
            Vector changeDegrees = new Vector();

            // Apply tracking changes
            if ((rateMoveAxes.X == 0.0) & (rateMoveAxes.Y == 0.0)) // No MoveAxis rates have been set so handle normally
            {
                // Determine the changes in current axis position and target axis position required as a result of tracking
                if (Tracking) // Tracking is enabled
                {
                    switch (alignmentMode)
                    {
                        case AlignmentModes.algGermanPolar: // In polar aligned mounts an HA change moves only the RA (primary) axis so update this, no change is required to the Dec (secondary) axis
                        case AlignmentModes.algPolar:
                            // Set the change in the primary (RA) axis position due to tracking 
                            changeDegrees.X = haChangeDegrees; // Set the change in the RA (primary) current axis position due to tracking 

                            // Update the slew target's RA (primary) axis position that will also have changed due to tracking
                            targetAxesDegrees.X += haChangeDegrees;

                            // Apply the RightAscensionRate offset
                            // The RA rate offset (rateRaDecOffsetInternal.X) is subtracted because the primary RA axis increases its angle value in a clockwise direction
                            // but RA decreases when moving in this direction
                            changeDegrees.X -= rateRaDecOffsetInternal.X * timeInSecondsSinceLastUpdate;

                            // Apply the DeclinationRate offset
                            // The relationship between the declination axis rotation direction and the associated declination value switches from
                            // correlated (declination increases as mechanical angle increases) to inverted (declination decreases as mechanical angle increases) depending on the mount pointing state.
                            // In addition, the sense of required rate corrections is inverted when in the southern hemisphere compared to the northern hemisphere
                            if (SouthernHemisphere) // Southern hemisphere
                            {
                                changeDegrees.Y += (SideOfPier == PierSide.pierEast ? -rateRaDecOffsetInternal.Y : +rateRaDecOffsetInternal.Y) * timeInSecondsSinceLastUpdate; // Add or subtract declination rate depending on pointing state
                            }
                            else // Northern hemisphere
                            {
                                changeDegrees.Y += (SideOfPier == PierSide.pierEast ? +rateRaDecOffsetInternal.Y : -rateRaDecOffsetInternal.Y) * timeInSecondsSinceLastUpdate; // Add or subtract declination rate depending on pointing state
                            }

                            //TL.LogMessage("MoveAxes", $"RA internal offset rate: {rateRaDecOffsetInternal.X}, Dec internal offset rate: {rateRaDecOffsetInternal.Y}. " +
                            //    $"Total change this interval: {changeDegrees.X}, {changeDegrees.Y}. Time since last update: {timeInSecondsSinceLastUpdate}"
                            //    );
                            break;

                        case AlignmentModes.algAltAz: // In Alt/Az aligned mounts the HA change moves both RA (primary) and Dec (secondary) axes so both need to be updated

                            // Set the change in the Azimuth (primary) and Altitude (secondary) axis positions due to tracking plus any RA / dec rate offsets
                            // The RA rate offset (rateRaDecOffsetInternal.X) is subtracted because the primary RA axis increases its angle value in a clockwise direction
                            // but RA decreases when moving in this direction
                            changeDegrees = ConvertRateToAltAz(haChangeDegrees / timeInSecondsSinceLastUpdate - rateRaDecOffsetInternal.X, rateRaDecOffsetInternal.Y, timeInSecondsSinceLastUpdate);

                            // Update the slew target's Azimuth (primary) and Altitude (secondary) axis positions that will also have changed due to tracking
                            targetAxesDegrees = MountFunctions.ConvertRaDecToAxes(targetRaDec, false);
                            break;
                    }

                    // TL.LogMessage("MoveAxes", $"Time since last update: {timeInSecondsSinceLastUpdate} seconds. HA change {haChangeDegrees} degrees. Alignment mode: {alignmentMode}.");
                    // TL.LogMessage("MoveAxes", $"Movement - Primary axis: {changeDegrees.X} degrees, Secondary axis: {changeDegrees.Y} degrees.");
                    // TL.LogMessage("MoveAxes", $"Movement rate - Primary axis: {changeDegrees.X * DEGREES_TO_ARCSECONDS / timeInSecondsSinceLastUpdate} arc-seconds per SI second, " +
                    //     $"Secondary axis: {changeDegrees.Y * DEGREES_TO_ARCSECONDS / timeInSecondsSinceLastUpdate} arc-seconds per SI second");
                    //TL.LogMessage("MoveAxes", $"Movement includes RightAscensionRate/DeclinationRate movement of: {rateRaDecOffsetInternal.X * timeInSecondsSinceLastUpdate} degrees, {rateRaDecOffsetInternal.Y * timeInSecondsSinceLastUpdate} degrees.");
                } // Mount is tracking
                else // Mount is not tracking
                {
                    // No axis change
                    //TL.LogMessage("MoveAxes", $"Tracking disabled - no changes.Time since last update: {timeInSecondsSinceLastUpdate} seconds.");
                } // Mount is not tracking

                // Move towards the target position if slewing
                changeDegrees += DoSlew();

            } // MoveAxis is not active
            else // A moveAxis rate has been set so treat as a Move 
            {
                // Calculate movement allowing for the time since the last movement correction was applied 
                switch (alignmentMode)
                {
                    case AlignmentModes.algAltAz:
                        // Direction sense is the same in both hemispheres
                        changeDegrees = Vector.Multiply(rateMoveAxes, timeInSecondsSinceLastUpdate);
                        break;

                    case AlignmentModes.algPolar:
                        // NORTHERN HEMISPHERE: Positive axis rates increase the secondary axis angle resulting in increases in declination, which is what we want
                        // SOUTHERN HEMISPHERE: Positive axis rates increase the secondary axis angle resulting in decreases in declination, so we reverse the sense here to ensure that positive axis rates result in increases in declination
                        if (SouthernHemisphere) // In the southern hemisphere
                        {
                            changeDegrees.X = rateMoveAxes.X * timeInSecondsSinceLastUpdate; // Retain the primary axis direction sense
                            changeDegrees.Y = -rateMoveAxes.Y * timeInSecondsSinceLastUpdate; // Swap the secondary axis direction sense
                        }
                        else // In the northern hemisphere
                        {
                            changeDegrees = Vector.Multiply(rateMoveAxes, timeInSecondsSinceLastUpdate); // Retain both primary and secondary axis senses
                        }
                        break;

                    case AlignmentModes.algGermanPolar:
                        // NORTHERN HEMISPHERE - NORMAL POINTING STATE (pierEast): Positive axis rates increase the secondary axis angle resulting in increases in declination, which is what we want
                        // NORTHERN HEMISPHERE - THROUGH THE POLE POINTING STATE (pierWest): Positive axis rates increase the secondary axis angle resulting in decreases in declination, which is what we want
                        // SOUTHERN HEMISPHERE - NORMAL POINTING STATE (pierEast): Positive axis rates increase the secondary axis angle resulting in decreases in declination, so we reverse the sense here to ensure that positive axis rates result in increases in declination
                        // SOUTHERN HEMISPHERE - THROUGH THE POLE POINTING STATE (pierWest): Positive axis rates increase the secondary axis angle resulting in decreases in declination, so we reverse the sense here to ensure that positive axis rates result in increases in declination
                        if (SouthernHemisphere) // In the southern hemisphere
                        {
                            changeDegrees.X = rateMoveAxes.X * timeInSecondsSinceLastUpdate; // Retain the primary axis direction sense
                            changeDegrees.Y = -rateMoveAxes.Y * timeInSecondsSinceLastUpdate; // Swap the secondary axis direction sense
                        }
                        else // In the northern hemisphere
                        {
                            changeDegrees = Vector.Multiply(rateMoveAxes, timeInSecondsSinceLastUpdate); // Retain both primary and secondary axis senses
                        }
                        break;

                    default:
                        break;
                }
                //TL.LogMessage("MoveAxes MoveAxis", $"Primary axis move rate: {rateMoveAxes.X}, Secondary axis move rate: {rateMoveAxes.Y}. Applied changes - Primary axis: {changeDegrees.X}, Secondary axis: {changeDegrees.Y}. Time since last update: {timeInSecondsSinceLastUpdate} seconds.");

            } // MoveAxis is active

            // handle HC button moves
            changeDegrees += HcMoves();

            // Pulse guiding changes
            changeDegrees += PulseGuide(timeInSecondsSinceLastUpdate);

            // Update the axis positions with the total change in this interval
            mountAxesDegrees += changeDegrees;

            // check the axis values, stop movement past limits
            CheckAxisLimits(changeDegrees.X);

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

                        // End the current slew operation
                        if ((SlewState != SlewType.SlewHome) & (SlewState == SlewType.SlewPark))
                            EndOperation("SlewSettle");
                    }
                    break;
            }

            // List changes this cycle
            //TL.LogMessage($"MoveAxes (Final)", $"RA: {currentRaDec.X.ToHMS()}, Dec: {currentRaDec.Y.ToDMS()}, Hour angle: {astroUtils.ConditionHA(SiderealTime - currentRaDec.X).ToHMS()}, Sidereal Time: {SiderealTime.ToHMS()}");
            //TL.LogMessage($"MoveAxes (Final)", $"Azimuth: {currentAltAzm.X.ToDMS()}, Altitude: {currentAltAzm.Y.ToDMS()}, Pointing state (pierEast = Normal): {SideOfPier}");
            // TL.LogMessage($"MoveAxes (Final)",
            //  $"Primary axis angle:  {mountAxesDegrees.X.ToDMS()}, Secondary axis angle:  {mountAxesDegrees.Y.ToDMS()}, " +
            // $"Primary axis change: {changeDegrees.X.ToDMS()}, Secondary axis change: {changeDegrees.Y.ToDMS()}."
            // );
            //TL.BlankLine();
        }

        #endregion

        #region Properties For Settings

        public static short InterfaceVersion
        {
            get
            {
                return interfaceVersion;
            }

            set
            {
                interfaceVersion = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "InterfaceVersion", value.ToString(), "");
            }
        }


        public static AlignmentModes AlignmentMode
        {
            get { return alignmentMode; }
            set
            {
                alignmentMode = value;
                switch (value)
                {
                    case AlignmentModes.algAltAz:
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "0");
                        break;
                    case AlignmentModes.algGermanPolar:
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1");
                        break;
                    case AlignmentModes.algPolar:
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
                // Assign the Southern hemisphere property to true or false depending on the site latitude
                SouthernHemisphere = latitude <= 0.0;
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

        public static bool CanDestinationSideOfPier
        {
            get { return canDestinationSideOfPier; }
            set
            {
                canDestinationSideOfPier = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDestinationSideOfPier", value.ToString(), "Capabilities");
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

        public static bool NoSyncPastMeridian
        {
            get { return noSyncPastMeridian; }
            set
            {
                noSyncPastMeridian = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NoSyncPastMeridian", value.ToString(), "Capabilities");
            }
        }

        #endregion

        #region Telescope Implementation

        public static double Altitude
        {
            get { return currentAltAzm.Y; }
            set { currentAltAzm.Y = value; }
        }

        public static bool AtPark
        {
            get
            {
                return atPark;
            }
            private set
            {
                atPark = value;
            }
        }

        public static double Azimuth
        {
            get { return currentAltAzm.X; }
            set { currentAltAzm.X = value; }
        }

        public static double ParkAltitude
        {
            get { return parkPosition.Y; }
            set
            {
                parkPosition.Y = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double ParkAzimuth
        {
            get { return parkPosition.X; }
            set
            {
                parkPosition.X = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static long GetId()
        {
            lock (getIdLockObj)
            {
                Interlocked.Increment(ref idCount); // Increment the counter in a thread safe fashion
                TL.LogMessage("GetId", "Generated new ID: " + idCount.ToString());
                return idCount;
            }
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
                TL.LogMessage("Hardware.Connected Set", $"Set Connected to: True, AlreadyConnected: {!notAlreadyPresent}, Connection count: {connectStates.Count}");
            }
            else
            {
                bool successfullyRemoved = connectStates.TryRemove(id, out _);
                TL.LogMessage("Hardware.Connected Set", $"Set Connected to: False, Successfully removed: {successfullyRemoved}, Connection count: {connectStates.Count}");
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

        public static bool SouthernHemisphere { get; private set; }

        public static double DeclinationRate
        {
            get { return rateRaDecOffsetExternal.Y; }
            set
            {
                rateRaDecOffsetExternal.Y = value; // Save the provided rate to be returned through the Get property
                rateRaDecOffsetInternal.Y = value * ARCSECONDS_TO_DEGREES; // Save the rate in the internal units that the simulator uses
                TL.LogMessage("DeclinationRate Set", $"Value to be set (as received): {rateRaDecOffsetExternal.Y} arc seconds per SI second. Converted to internal rate of: {rateRaDecOffsetInternal.Y} degrees per SI second.");
            }
        }

        public static double Declination
        {
            get { return currentRaDec.Y; }
            set { currentRaDec.Y = value; }
        }

        public static double RightAscension
        {
            get { return currentRaDec.X; }
            set { currentRaDec.X = value; }
        }

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
                //LogMessage("AtHome", "Distance from Home: {0}, AtHome: {1}", (mountAxes - MountFunctions.ConvertAltAzmToAxes(HomePosition)).LengthSquared, (mountAxes - MountFunctions.ConvertAltAzmToAxes(HomePosition)).LengthSquared < 0.01);

                // If a FindHome operation has been started, return its sate
                if (currentOperation == Operation.FindHome)
                    return false;

                // Otherwise return true if the mount is close to the  home position.
                bool atHome = (mountAxesDegrees - MountFunctions.ConvertAltAzmToAxes(HomePosition)).LengthSquared < 0.01;

                return atHome;
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

        public static int DateDelta
        {
            get { return dateDelta; }
            set
            {
                dateDelta = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DateDelta", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Manages RightAscensionRate. Units are "seconds of RA per sidereal second".
        /// </summary>
        /// <remarks>
        /// 1) This property retains the value supplied by set RightAscensionRate in the rateRaDecOffsetExternal.X vector element so that it can be returned by get RightAscensionRate.
        /// 2) The set RightAscensionRate value is also converted to the internal units used by the simulator (arcsec per SI second) and stored in the rateRaDecOffsetInternal.X vector element
        /// </remarks>
        public static double RightAscensionRate
        {
            get { return rateRaDecOffsetExternal.X; }
            set
            {
                // Save the provided rate (seconds of RA per sidereal second) to be returned through the Get property
                rateRaDecOffsetExternal.X = value;

                // Save the provided rate for internal use in the units (degrees per SI second) that the simulator uses.
                // SIDEREAL_SECONDS_TO_SI_SECONDS converts from sidereal seconds to SI seconds
                // Have to multiply by the SIDEREAL_SECONDS_TO_SI_SECONDS conversion factor (0.99726956631945) because SI seconds are longer than sidereal seconds and hence the simulator movement will be less in one SI second than in one sidereal second
                // ARCSECONDS_PER_RA_SECOND converts from seconds of RA (1 circle = 24 hours) to arc-seconds (1 circle = 360 degrees)
                // ARCSECONDS_TO_DEGREES converts from arc-seconds to degrees
                rateRaDecOffsetInternal.X = value * SIDEREAL_SECONDS_TO_SI_SECONDS * ARCSECONDS_PER_RA_SECOND * ARCSECONDS_TO_DEGREES;
                TL.LogMessage("RightAscensionRate Set", $"Value to be set (as received): {value} seconds per sidereal second. Converted to internal rate of: {value * SIDEREAL_SECONDS_TO_SI_SECONDS} seconds per SI second = {rateRaDecOffsetInternal.X} degrees per SI second.");
            }
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

        public static PierSide SideOfPier
        {
            get
            {
                return (mountAxesDegrees.Y <= 90 && mountAxesDegrees.Y >= -90) ?
                    PierSide.pierEast : PierSide.pierWest;
            }
            set
            {
                // check the new side can be reached
                var pa = AstronomyFunctions.RangeAzimuth(mountAxesDegrees.X - 180);
                if (pa >= hourAngleLimit + 180 && pa < -hourAngleLimit)
                {
                    throw new InvalidOperationException("set SideOfPier " + value.ToString() + " cannot be reached at the current position");
                }

                StartOperation(Operation.SideOfPier);

                // change the pier side
                StartSlewAxes(pa, 180 - mountAxesDegrees.Y, SlewType.SlewRaDec, Operation.SideOfPier);
            }
        }

        public static bool IsSlewing
        {
            get
            {

                if (SlewState != SlewType.SlewNone)
                {
                    //TL.LogMessage("IsSlewing", $"TRUE - {SlewState} - SlewState != SlewType.None");
                    return true;
                }

                if (slewing)
                {
                    //TL.LogMessage("IsSlewing", $"TRUE - {SlewState} - slewing == true");
                    return true;
                }

                if (rateMoveAxes.LengthSquared != 0)
                {
                    //TL.LogMessage("IsSlewing", $"TRUE - {SlewState} - rateMoveAxes.LengthSquared != 0");
                    return true;
                }

                //TL.LogMessage("IsSlewing", $"SlewState: {SlewState}, Slewing: {slewing}, rateMoveAxes.X:{rateMoveAxes.X}, rateMoveAxes.Y: {rateMoveAxes.Y} - Returning: {slewing && rateMoveAxes.Y != 0 && rateMoveAxes.X != 0}");
                return slewing && rateMoveAxes.Y != 0 && rateMoveAxes.X != 0;
            }
        }

        public static void AbortSlew()
        {
            slewing = false;
            rateMoveAxes = new Vector();
            rateRaDecOffsetInternal = new Vector();
            SlewState = SlewType.SlewNone;

            // End running operations as defined in the interface specification
            switch (currentOperation)
            {
                case Operation.MoveAxis:
                case Operation.SlewToAltAzAsync:
                case Operation.SlewToCoordinatesAsync:
                case Operation.SlewToTargetAsync:
                    // End these operations if they are running
                    EndOperation("AbortSlew", new OperationCancelledException("Slew / Move interrupted because the AbortSlew method was called."));
                    break;

                default:
                    // No action for other operations
                    break;
            }
        }

        public static void SyncToTarget()
        {
            mountAxesDegrees = MountFunctions.ConvertRaDecToAxes(targetRaDec, true);
            UpdatePositions();
        }

        public static void SyncToAltAz(double targetAzimuth, double targetAltitude)
        {
            mountAxesDegrees = MountFunctions.ConvertAltAzmToAxes(new Vector(targetAzimuth, targetAltitude));
            UpdatePositions();
        }

        public static void StartSlewRaDec(double rightAscension, double declination, bool doSideOfPier, Operation operation)
        {
            Vector raDec = new Vector(rightAscension, declination);
            targetAxesDegrees = MountFunctions.ConvertRaDecToAxes(raDec);

            LogMessage("StartSlewRaDec", $"RA: {rightAscension.ToHMS()}, Declination: {declination.ToDMS()}, DoSOP {doSideOfPier}");
            StartSlewAxes(targetAxesDegrees, SlewType.SlewRaDec, operation);
        }

        public static void StartSlewAltAz(double altitude, double azimuth, Operation operation)
        {
            StartSlewAltAz(new Vector(azimuth, altitude), operation);
            return;
        }

        public static void StartSlewAltAz(Vector targetAltAz, Operation operation)
        {
            LogMessage("StartSlewAltAz", $"Azimuth: {targetAltAz.X.ToDMS()}, Altitude: {targetAltAz.Y.ToDMS()}");

            Vector target = MountFunctions.ConvertAltAzmToAxes(targetAltAz);
            if (target.LengthSquared > 0)
            {
                StartSlewAxes(target, SlewType.SlewAltAz, operation);
            }
        }

        public static void StartSlewAxes(double primaryAxis, double secondaryAxis, SlewType slewState, Operation operation)
        {
            StartSlewAxes(new Vector(primaryAxis, secondaryAxis), slewState, operation);
        }

        /// <summary>
        /// Starts a slew to the target position in mount axis degrees.
        /// </summary>
        /// <param name="targetPosition">The position.</param>
        public static void StartSlewAxes(Vector targetPosition, SlewType slewState, Operation operation)
        {
            StartOperation(operation);

            targetAxesDegrees = targetPosition;
            SlewState = slewState;
            slewing = true;
            ChangePark(false);
        }

        public static void Park()
        {
            Vector parkCoordinates;

            parkCoordinates = MountFunctions.ConvertAltAzmToAxes(parkPosition); // Convert the park position AltAz coordinates into the current axes representation
            Tracking = false;

            StartSlewAxes(parkCoordinates, SlewType.SlewPark, Operation.Park);
        }

        public static void FindHome()
        {
            if (AtPark)
            {
                throw new ParkedException("Cannot find Home when Parked");
            }

            Tracking = false;
            TL.LogMessage("FindHome", $"HomePosition.X: {HomePosition.X.ToDMS()}, HomePosition.Y: {HomePosition.Y.ToDMS()}");
            StartSlewAxes(MountFunctions.ConvertAltAzmToAxes(HomePosition), SlewType.SlewHome, Operation.FindHome);
        }

        /// <summary>
        /// Restore the configured Tracking state when configuration prevent s clients from doing this themselves
        /// </summary>
        /// <remarks>
        /// When the simulator is configured to prevent clients from changing the Tracking state it is necessary to restore the configured state after
        /// performing functions such as Park() and FindHome. This method is called in several places to ensure that the configured Tracking state is active 
        /// even after commands where tracking is intentionally disabled by the simulator itself.
        /// </remarks>
        public static void RestoreTrackingStateIfNecessary()
        {
            if (!CanSetTracking) Tracking = AutoTrack;
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Gets the side of pier using the right ascension, assuming it depends on the
        /// hour angle only.  Used for Destination side of Pier, NOT to determine the mount
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
                LogMessage("SideOfPierRaDec", $"RA: {rightAscension.ToHMS()}, Declination: {declination.ToDMS()}, HA: {ha.ToHMS()}, Side of pier: {sideOfPier}");

                return sideOfPier;
            }
        }

        public static void ChangePark(bool newValue)
        {
            AtPark = newValue;
            if (AtPark) TelescopeSimulator.m_MainForm.ParkButton("Unpark");
            else TelescopeSimulator.m_MainForm.ParkButton("Park");
        }

        public static double AvailableTimeInThisPointingState
        {
            get
            {
                if (AlignmentMode != AlignmentModes.algGermanPolar)
                {
                    return double.MaxValue;
                }
                double degToLimit = mountAxesDegrees.X + hourAngleLimit + 360;
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
                var degToLimit = mountAxesDegrees.X - hourAngleLimit + 360;
                while (degToLimit > 180) degToLimit -= 360;
                return degToLimit * 240;
            }
        }

        private static void LogMessage(string identifier, string format, params object[] args)
        {
            TL.LogMessage(identifier, string.Format(CultureInfo.InvariantCulture, format, args));
        }

        /// <summary>
        /// Returns the hour angle change due to tracking in degrees during the update interval
        /// </summary>
        /// <param name="updateInterval">The update interval.</param>
        /// <returns></returns>
        private static double GetTrackingChangeInDegrees(double updateInterval)
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
                    haChange = SIDEREAL_RATE_DEG_PER_SI_SECOND * updateInterval;     // change in degrees
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
            bool finished;

            // Check primary axis
            delta = targetAxesDegrees.X - mountAxesDegrees.X;
            while (delta < -180 || delta > 180)
            {
                if (delta < -180) delta += 360;
                if (delta > 180) delta -= 360;
            }
            int signDelta = delta < 0 ? -1 : +1;
            delta = Math.Abs(delta);

            // Slewing is complete because we are less than one slow-speed unit from the target.
            if (delta < slewSpeedSlow)
            {
                change.X = delta * signDelta;
                finished = true;
            }
            else if (delta < slewSpeedMedium * 2)
            {
                // Move by one slow speed unit
                change.X = slewSpeedSlow * signDelta;
                finished = false;
            }
            else if (delta < slewSpeedFast * 2)
            {
                // Move by one medium speed unit
                change.X = slewSpeedMedium * signDelta;
                finished = false;
            }
            else
            {
                // Move by one fast speed unit
                change.X = slewSpeedFast * signDelta;
                finished = false;
            }

            // Check secondary axis
            delta = targetAxesDegrees.Y - mountAxesDegrees.Y;
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

            // If finished then complete processing
            if (finished)
            {
                slewing = false;
                switch (SlewState)
                {
                    case SlewType.SlewRaDec:
                    case SlewType.SlewAltAz:
                        if (SlewSettleTime == 0.0) // Slew finishes here!
                        {
                            SlewState = SlewType.SlewNone;
                            LogMessage("DoSlew", "No settle time - Slew done");
                            EndOperation("DoSlew - Slew Done");
                        }
                        else
                        {
                            SlewState = SlewType.SlewSettle;
                            settleTime = DateTime.Now + TimeSpan.FromSeconds(SlewSettleTime);
                            LogMessage("DoSlew", $"Moved from slew to settle for {SlewSettleTime} seconds");
                        }
                        break;

                    case SlewType.SlewPark:
                        LogMessage("DoSlew", "Parked done");
                        SlewState = SlewType.SlewNone;
                        ChangePark(true);

                        // End the current operation
                        EndOperation("DoSlew - Park Done");
                        break;

                    case SlewType.SlewHome:
                        LogMessage("DoSlew", "Home done");
                        SlewState = SlewType.SlewNone;

                        // End the current operation
                        EndOperation("DoSlew - Home Done");
                        break;

                    case SlewType.SlewNone:
                        break;

                    default:
                        //Covers SlewType.SlewMoveAxis and SlewType.SlewHandpad:
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
                case SlewDirection.SlewWest:
                case SlewDirection.SlewLeft:
                    change.X = delta;
                    break;
                case SlewDirection.SlewEast:
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
            double guideTime;

            // Handle AltAz alignment differently to Polar and German polar.
            switch (alignmentMode)
            {
                case AlignmentModes.algAltAz:

                    // Only run the process if we are currently pulse guiding
                    if (IsPulseGuiding)
                    {
                        // Set a flag when RA pulse guiding is complete
                        if (guideDuration.X <= 0)
                        {
                            isPulseGuidingRa = false;
                            guideDuration.X = 0.0;
                        }

                        // Set a flag when declination pulse guiding is complete
                        if (guideDuration.Y <= 0)
                        {
                            isPulseGuidingDec = false;
                            guideDuration.Y = 0.0;
                        }

                        // If pulse guiding is active on either axis undertake the calculation
                        if ((guideDuration.X > 0.0) | (guideDuration.Y > 0.0))
                        {
                            // Calculate the time during which pulse guiding was actually active in this time interval, either the whole interval or part of it
                            double guideTimeRA = guideDuration.X > updateInterval ? updateInterval : guideDuration.X;
                            double guideTimeDeclination = guideDuration.Y > updateInterval ? updateInterval : guideDuration.Y;

                            // Update the remaining time of the pulse guide interval
                            if (guideDuration.X > 0.0)
                                guideDuration.X -= updateInterval;
                            if (guideDuration.Y > 0.0)
                                guideDuration.Y -= updateInterval;

                            // Calculate the change due to any RA and declination pulse guiding in this interval
                            change = ConvertRateToAltAz(guideRate.X * guideTimeRA / updateInterval, guideRate.Y * guideTimeDeclination / updateInterval, updateInterval);
                        }
                    }
                    break;

                case AlignmentModes.algPolar:
                    if (isPulseGuidingRa)
                    {
                        if (guideDuration.X <= 0)
                        {
                            isPulseGuidingRa = false;
                        }
                        else
                        {
                            // assume polar mount only
                            guideTime = guideDuration.X > updateInterval ? updateInterval : guideDuration.X;
                            guideDuration.X -= guideTime;

                            // assumes guide rate is in deg/sec
                            change.X = guideRate.X * guideTime;
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
                            guideTime = guideDuration.Y > updateInterval ? updateInterval : guideDuration.Y;
                            guideDuration.Y -= guideTime;

                            // Calculate the change in this interval allowing for inversion of declination direction when in the southern hemisphere.
                            if (SouthernHemisphere) // Invert the change to match the simulator mechanical axis scale
                            {
                                change.Y = -guideRate.Y * guideTime;
                            }
                            else // Northern hemisphere
                            {
                                change.Y = guideRate.Y * guideTime;
                            }
                        }
                    }
                    break;

                case AlignmentModes.algGermanPolar:
                    if (isPulseGuidingRa)
                    {
                        if (guideDuration.X <= 0)
                        {
                            isPulseGuidingRa = false;
                        }
                        else
                        {
                            // assume polar mount only
                            guideTime = guideDuration.X > updateInterval ? updateInterval : guideDuration.X;
                            guideDuration.X -= guideTime;

                            // assumes guide rate is in deg/sec
                            change.X = guideRate.X * guideTime;
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
                            guideTime = guideDuration.Y > updateInterval ? updateInterval : guideDuration.Y;
                            guideDuration.Y -= guideTime;

                            // Calculate the change in this interval allowing for inversion of declination direction when the pointing state is through the pole.
                            if (SideOfPier == PierSide.pierEast) // Normal state
                            {
                                change.Y = guideRate.Y * guideTime;
                            }
                            else // Through the pole state
                            {
                                change.Y = -guideRate.Y * guideTime;
                            }

                            // Invert the direction of the declination change when in the southern hemisphere to match the simulator mechanical axis specification
                            if (SouthernHemisphere)
                            {
                                change.Y = -change.Y;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            // End the pulse guide operation if one is active and guiding has completed
            if (currentOperation == Operation.PulseGuide & !isPulseGuidingRa & !isPulseGuidingDec)
            {
                EndOperation("PulseGuide");
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
            // and -hourAngleLimit to 180 + hourAngleLimit for German polar
            switch (alignmentMode)
            {
                case AlignmentModes.algAltAz:
                    // the primary axis must be in the range 0 to 360
                    mountAxesDegrees.X = AstronomyFunctions.RangeAzimuth(mountAxesDegrees.X);
                    break;
                case AlignmentModes.algGermanPolar:
                    // the primary axis needs to be in the range -180 to +180 to correspond with hour angles
                    // of -12 to 12.
                    // check if we have hit the hour angle limit
                    if ((mountAxesDegrees.X >= hourAngleLimit + 180 && primaryChange > 0) ||
                        (mountAxesDegrees.X <= -hourAngleLimit && primaryChange < 0))
                    {
                        // undo the movement when the limit is hit
                        mountAxesDegrees.X -= primaryChange;
                    }
                    break;
                case AlignmentModes.algPolar:
                    // the axis needs to be in the range -180 to +180 to correspond with hour angles
                    // of -12 to 12.
                    while (mountAxesDegrees.X <= -180.0 || mountAxesDegrees.X > 180.0)
                    {
                        if (mountAxesDegrees.X <= -180.0) mountAxesDegrees.X += 360;
                        if (mountAxesDegrees.X > 180) mountAxesDegrees.X -= 360;
                    }
                    break;
            }
            // secondary must be in the range -90 to 0 to +90 for normal 
            // and +90 to 180 to 270 for through the pole.
            // rotation is continuous
            while (mountAxesDegrees.Y >= 270.0 || mountAxesDegrees.Y < -90.0)
            {
                if (mountAxesDegrees.Y >= 270) mountAxesDegrees.Y -= 360.0;
                if (mountAxesDegrees.Y < -90) mountAxesDegrees.Y += 360.0;
            }
        }

        /// <summary>
        /// Convert [HA,Dec] position and rate into [Az,El] position and rate.
        /// </summary>
        /// <param name="phi">site latitude (radians)</param>
        /// <param name="ha"> hour angle (radians)</param>
        /// <param name="dec">declination (radians)</param>
        /// <param name="hadot">rate of change of ha (radians per unit time)</param>
        /// <param name="decdot">rate of change of declination (radians per unit time)</param>
        /// <param name="a">azimuth (N through E, radians)</param>
        /// <param name="e">elevation (radians)</param>
        /// <param name="ad">rate of change of a (radians per unit time)</param>
        /// <param name="ed">rate of change of e (radians per unit time)</param>
        /// <remarks>
        /// 1) For sidereal tracking ha should include the sidereal rate as well as any differential rate.
        /// 
        /// 2) The units of the velocity arguments are up to the caller.
        ///
        /// This revision:  2023 January 9
        ///
        /// Author P.T.Wallace.
        /// 
        /// 
        /// </remarks>
        static void Tran(double phi, double ha, double dec, double hadot, double decdot, out double a, out double e, out double ad, out double ed)
        {
            double sh, ch, sd, cd, x, y, z, w, xd, yd, zd, sp, cp, rxy2, rxy, xyp;

            /* Functions of HA and Dec. */
            sh = Math.Sin(ha);
            ch = Math.Cos(ha);
            sd = Math.Sin(dec);
            cd = Math.Cos(dec);

            /* Unit vector (right-handed) P & V. */
            x = ch * cd;
            y = -sh * cd;
            z = sd;

            w = decdot * sd;
            xd = hadot * y - w * ch;
            yd = -hadot * x + w * sh;
            zd = decdot * cd;

            /* Rotate P & V into Cartesian [2pi-Az,El]. */
            sp = Math.Sin(phi);
            cp = Math.Cos(phi);
            w = x * sp - z * cp;
            z = x * cp + z * sp;
            x = w;

            w = xd * sp - zd * cp;
            zd = xd * cp + zd * sp;
            xd = w;

            /* Vector's component in XY plane. */
            rxy2 = x * x + y * y;
            rxy = Math.Sqrt(rxy2);

            /* Position and velocity in [Az,El]. */
            xyp = x * xd + y * yd;
            if (rxy != 0.0)
            {
                a = Math.Atan2(y, -x);
                e = Math.Atan2(z, rxy);
                ad = -(x * yd - y * xd) / rxy2;
                ed = (zd * rxy2 - z * xyp) / rxy;
            }
            else
            {
                a = 0.0;
                e = (z != 0.0) ? Math.Atan2(z, rxy) : 0.0;
                ad = 0.0;
                ed = 0.0;
            }
        }

        /// <summary>
        /// Convert the move rate in hour angle to a change in altitude and azimuth
        /// </summary>
        /// <param name="haChange">The ha change.</param>
        /// <returns></returns>
        private static Vector ConvertRateToAltAz(double haChange, double decChange, double timeInSecondsThisInterval)
        {
            Vector change = new Vector();

            double phi = Latitude * DEG_RAD; // Site latitude in radians
            double ha = (SiderealTime - currentRaDec.X) * HRS_RAD; // Current hour angle in radians
            double dec = currentRaDec.Y * DEG_RAD; // Current declination in radians
            double hadot = haChange * DEG_RAD; // Rate of change in HA due to tracking plus any RA offset rate in radians per second
            double decdot = decChange * DEG_RAD; // Rate of change in declination due to any declination rate offset in radians per second

#pragma warning disable IDE0018 // Suppress In-line variable declaration

            double a; // Azimuth (N through E, radians)
            double e; // Elevation (radians)
            double ad; // Rate of change of azimuth (radians per unit time)
            double ed; // Rate of change of elevation (radians per unit time)

#pragma warning restore IDE0018 // Enable In-line variable declaration

            // Calculate azimuth and elevation rates (radians per second)
            Tran(phi, ha, dec, hadot, decdot, out a, out e, out ad, out ed);

            double azimuthChange = ad * timeInSecondsThisInterval; // Change in azimuth this interval in radians
            double elevationChange = ed * timeInSecondsThisInterval; // Change in elevation this interval in radians

            change.X = azimuthChange * RAD_DEG; // Convert azimuth change in radians to degrees
            change.Y = elevationChange * RAD_DEG; // Convert elevation change in radians to degrees

            return change;
        }

        /// <summary>
        /// Update the mount positions and state from the axis positions
        /// </summary>
        private static void UpdatePositions()
        {
            // Update sidereal time for next loop
            SiderealTime = AstronomyFunctions.LocalSiderealTime(Longitude);

            //pointingState = mountAxesDegrees.Y <= 90 ? PointingState.Normal : PointingState.ThroughThePole;

            currentAltAzm = MountFunctions.ConvertAxesToAltAzm(mountAxesDegrees);
            currentRaDec = MountFunctions.ConvertAxesToRaDec(mountAxesDegrees);
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

            TelescopeSimulator.m_MainForm.LabelState(TelescopeSimulator.m_MainForm.lblPARK, AtPark);
            TelescopeSimulator.m_MainForm.LabelState(TelescopeSimulator.m_MainForm.lblHOME, AtHome);
            TelescopeSimulator.m_MainForm.LabelState(TelescopeSimulator.m_MainForm.labelSlew, IsSlewing);
        }

        public static void StartOperation(Operation operation)
        {
            TL.LogMessage("OperationState-Start", $"Starting new operation: {operation} - Current operation is: {CurrentOperation}");
            CurrentOperation = operation;

            // Clear any previous exception that was being thrown
            OperationException = null;

            // Mark this operation as underway.
            OperationComplete = false;
        }

        public static void EndOperation(string sourceName)
        {
            EndOperation(sourceName, null);
        }

        public static void EndOperation(string sourceName, Exception exception)
        {
            if (exception is null)
                TL.LogMessage("OperationState-End", $"{sourceName} - Ending operation {currentOperation}");
            else
                TL.LogMessage("OperationState-End", $"{sourceName} - Ending operation {currentOperation} with error number: 0x{exception.HResult:X8} and error message: '{exception.Message}'");
            // Mark this operation as complete
            OperationComplete = true;

            // Set the exception to be returned by the OperationComplete property, if any
            OperationException = exception;
            CurrentOperation = Operation.None;
        }

        #endregion
    }
}
