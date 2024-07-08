using System;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry
{

    static class GlobalItems
    {

        // Physical constants
        internal const double MOON_RADIUS = 1737.0d; // km
        internal const double EARTH_RADIUS = 6378.0d; // km
        internal const double SUN_RADIUS = 696342.0d; // km
        internal const double MERCURY_RADIUS = 2439.7d; // km
        internal const double VENUS_RADIUS = 2439.7d; // km
        internal const double MARS_RADIUS = 3396.2d; // km
        internal const double JUPITER_RADIUS = 69911.0d; // km
        internal const double SATURN_RADIUS = 6051.8d; // km
        internal const double NEPTUNE_RADIUS = 24767.0d; // km
        internal const double URANUS_RADIUS = 24973.0d; // km
        internal const double PLUTO_RADIUS = 1153.0d; // km

        // Fixed event definitions
        internal const double SUN_RISE = -50.0d / 60.0d; // Degrees
        internal const double CIVIL_TWILIGHT = -6.0d; // Degrees
        internal const double NAUTICAL_TWILIGHT = -12.0d; // Degrees
        internal const double AMATEUR_ASRONOMICAL_TWILIGHT = -15.0d; // Degrees
        internal const double ASTRONOMICAL_TWILIGHT = -18.0d; // Degrees

        // Conversion factors
        internal const double HOURS2DEG = 15.0d;
        internal const double DEG2HOURS = 1.0d / 15.0d;
        internal const double DEG2HOURSSOLSID = 1.0d / 15.04107d;
        internal const double SECONDS2DAYS = 1.0d / (60.0d * 60.0d * 24.0d);
        internal const double AU2KILOMETRE = 149597870.691d;

        // NOVAS.COM Constants
        internal const short FN1 = 1;
        internal const short FN0 = 0;
        internal const double J2000BASE = 2451545.0d; // TDB Julian date of epoch J2000.0.
        internal const double KMAU = 149597870.0d; // Astronomical Unit in kilometres.
        internal const double MAU = 149597870000.0d; // Astronomical Unit in meters.
        internal const double C = 173.14463348d; // Speed of light in AU/Day.
        internal const double GS = 1.32712438E+20d; // Heliocentric gravitational constant.
        internal const double EARTHRAD = 6378.14d; // Radius of Earth in kilometres.
        internal const double F = 0.00335281d; // Earth ellipsoid flattening.
        internal const double OMEGA = 0.00007292115d; // Rotational angular velocity of Earth in radians/sec.
        internal const double TWOPI = 6.2831853071795862d; // Value of pi in radians.
        internal const double RAD2SEC = 206264.80624709636d; // Angle conversion constants.
        internal const double DEG2RAD = 0.017453292519943295d;
        internal const double RAD2DEG = 57.295779513082323d;

        // General constants
        internal const double TT_TAI_OFFSET = 32.184d; // 32.184 seconds
        internal const double MODIFIED_JULIAN_DAY_OFFSET = 2400000.5d; // This is the offset of Modified Julian dates from true Julian dates
        internal const double SECPERDAY = 86400.0d;
        internal const double DELTAUT1_BOUND = 0.9d; // Used to validate delta UT1 values input manually or automatically downloaded, which must line in the range -DELTAUT1_BOUND to +DELTAUT1_BOUND
        internal const double TROPICAL_YEAR_IN_DAYS = 365.24219d;

        internal const double OLE_AUTOMATION_JULIAN_DATE_OFFSET = 2415018.5d; // Offset of OLE automation dates from Julian dates
        internal const double JULIAN_DATE_MINIMUM_VALUE = -657435.0d + OLE_AUTOMATION_JULIAN_DATE_OFFSET; // Minimum valid Julian date value (1/1/0100 00:00:00) - because DateTime.FromOADate has this limit
        internal const double JULIAN_DATE_MAXIMUM_VALUE = 2958465.99999999d + OLE_AUTOMATION_JULIAN_DATE_OFFSET; // Maximum valid Julian date value (31/12/9999 23:59:59.999) - because DateTime.FromOADate has this limit

        internal const double RACIO_DEFAULT_VALUE = double.NaN; // NOVAS3: Default value that if still present will indicate that this value was not updated

        // Profile store Key names
        internal const string ASTROMETRY_SUBKEY = "Astrometry";
        internal const string AUTOMATIC_UPDATE_DELTAUT1_SUBKEY_NAME = ASTROMETRY_SUBKEY + @"\Latest Delta UT1 Data"; // Name of the Profile\Astrometry subkey in which automatically downloaded Delta UT1 predicted values will be stored
        internal const string AUTOMATIC_UPDATE_LEAP_SECOND_HISTORY_SUBKEY_NAME = ASTROMETRY_SUBKEY + @"\Latest Leap Second Data"; // Name of the Profile\Astrometry subkey in which automatically downloaded historic leap second values will be stored

        // Profile store value names
        internal const string UPDATE_TYPE_VALUE_NAME = "UTC and UT1 Data Update Method"; // Value name in Profile/Astrometry that determines how earth rotation data is updated: None, Automatic download, Manual entry, Built-in prediction.
        internal const string EARTH_ROTATION_DATA_LAST_UPDATED_VALUE_NAME = "Automatic Data Last Updated"; // Value name for the date and time that the scheduled task was last run
        internal const string MANUAL_LEAP_SECONDS_VALUENAME = "Manual Leap Seconds"; // Name of the manually updated leap second value
        internal const string MANUAL_DELTAUT1_VALUE_NAME = "Manual Delta UT1"; // Value name in Astrometry for manually entered Delta UT1 values
        internal const string AUTOMATIC_LEAP_SECONDS_VALUENAME = "Automatic Leap Seconds"; // Name of the automatically updated leap second value
        internal const string NEXT_LEAP_SECONDS_VALUENAME = "Automatic Next Leap Seconds"; // Name of the automatically updated next leap second value
        internal const string NEXT_LEAP_SECONDS_DATE_VALUENAME = "Automatic Next Leap Seconds Date"; // Name of the automatically updated next leap second commencement date value
        internal const string DELTAUT1_VALUE_NAME_FORMAT = "Delta UT1 Prediction for {0} - {1} - {2}"; // Format string for automatically downloaded delta UT1 value names. The 0, 1 and 2 placeholders are for year, month and day integers
        internal const string DELTAUT1_VALUE_NAME_YEAR_FORMAT = "0000"; // Format string for the year component of automatically downloaded delta UT1 value names.
        internal const string DELTAUT1_VALUE_NAME_MONTH_FORMAT = "00"; // Format string for the month component of automatically downloaded delta UT1 value names.
        internal const string DELTAUT1_VALUE_NAME_DAY_FORMAT = "00"; // Format string for the day component of automatically downloaded delta UT1 value names.
        internal const string DOWNLOAD_TASK_DATA_SOURCE_VALUE_NAME = "Download Task Data Source"; // Name of the automatic data source profile value
        internal const string DOWNLOAD_TASK_TIMEOUT_VALUE_NAME = "Download Task Timeout"; // Name of the automatic update timeout profile value
        internal const string DOWNLOAD_TASK_SCHEDULED_TIME_VALUE_NAME = "Download Task Scheduled Time"; // Value name for the scheduled job run time
        internal const string DOWNLOAD_TASK_REPEAT_FREQUENCY_VALUE_NAME = "Download Task Repeat Frequency"; // Value name for the scheduled job run time
        internal const string DOWNLOAD_TASK_TRACE_ENABLED_VALUE_NAME = "Download Task Trace Enabled"; // Value name for the scheduled job run time
        internal const string DOWNLOAD_TASK_TRACE_PATH_VALUE_NAME = "Download Task Trace Path"; // Value name for the path to the scheduled job trace file

        // Earth rotation data source names
        internal const string UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1 = "Built-in leap seconds and predicted delta UT1"; // Alternative value for earth rotation data source
        internal const string UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1 = "Specified leap seconds and specified delta UT1"; // Alternative value for earth rotation data source
        internal const string UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1 = "Specified leap seconds and predicted delta UT1"; // Alternative value for earth rotation data source
        internal const string UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1 = "Manual on demand Internet update"; // Alternative value for earth rotation data source
        internal const string UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1 = "Automatic scheduled Internet update"; // Alternative value for earth rotation data source

        // Delta UT1 filename and format
        internal const string DELTAUT1_FILE = "finals.daily"; // Name of the IERS file containing Delta UT1 predictions
        internal const int DELTAUT1_YEAR_START = 0;
        internal const int DELTAUT1_YEAR_LENGTH = 2; // Start position and length of the YEAR field in the finals.daily data line
        internal const int DELTAUT1_MONTH_START = 2;
        internal const int DELTAUT1_MONTH_LENGTH = 2; // Start position and length of the MONTH field in the finals.daily data line
        internal const int DELTAUT1_DAY_START = 4;
        internal const int DELTAUT1_DAY_LENGTH = 2; // Start position and length of the DAY field in the finals.daily data line
        internal const int DELTAUT1_JULIAN_DATE_START = 7;
        internal const int DELTAUT1_JULIAN_DATE_LENGTH = 8; // Start position and length of the JULKIAN DATE field in the finals.daily data line
        internal const int DELTAUT1_START = 58;
        internal const int DELTAUT1_LENGTH = 10; // Start position and length of the DELTAUT1 field in the finals.daily data line

        // Leap seconds filename and format
        // Friend Const LEAP_SECONDS_FILE As String = "leapsec.dat" ' Name of the IERS file containing leap second historic and future values
        internal const string LEAP_SECONDS_FILE = "tai-utc.dat"; // Name of the IERS file containing leap second historic and future values
        internal const int LEAP_SECONDS_YEAR_START = 0;
        internal const int LEAP_SECONDS_YEAR_LENGTH = 5; // Start position and length of the YEAR field in the tai-utc.dat data line
        internal const int LEAP_SECONDS_MONTH_START = 5;
        internal const int LEAP_SECONDS_MONTH_LENGTH = 4; // Start position and length of the MONTH field in the tai-utc.dat data line
        internal const int LEAP_SECONDS_DAY_START = 9;
        internal const int LEAP_SECONDS_DAY_LENGTH = 4; // Start position and length of the DAY field in the tai-utc.dat data line
        internal const int LEAP_SECONDS_JULIAN_DATE_START = 17;
        internal const int LEAP_SECONDS_JULIAN_DATE_LENGTH = 10; // Start position and length of the JULIAN DATE field in the tai-utc.dat data line
        internal const int LEAP_SECONDS_LEAPSECONDS_START = 36;
        internal const int LEAP_SECONDS_LEAPSECONDS_LENGTH = 12; // Start position and length of the NUMBER OF LEAP SECONDS field in the tai-utc.dat data line

        // Earth rotation data download configuration options
        internal const string EARTH_ROTATION_INTERNET_DATA_SOURCE_0 = "https://download.ascom-standards.org/earthrot/"; // Internet source options for earth rotation files
        internal const string EARTH_ROTATION_INTERNET_DATA_SOURCE_1 = "http://toshi.nofs.navy.mil/ser7/";
        internal const string EARTH_ROTATION_INTERNET_DATA_SOURCE_2 = "ftp://cddis.gsfc.nasa.gov/pub/products/iers/";
        internal const string EARTH_ROTATION_INTERNET_DATA_SOURCE_3 = "ftp://maia.usno.navy.mil/ser7/";
        internal const string EARTH_ROTATION_INTERNET_DATA_SOURCE_4 = "https://cddis.nasa.gov/archive/products/iers/";
        internal const string SCHEDULE_REPEAT_NONE = "None"; // Options for automatic update schedule repeat frequency
        internal const string SCHEDULE_REPEAT_DAILY = "Repeat daily";
        internal const string SCHEDULE_REPEAT_WEEKLY = "Repeat weekly";
        internal const string SCHEDULE_REPEAT_MONTHLY = "Repeat monthly";
        internal const string URI_PREFIX_HTTP = "http://";
        internal const string URI_PREFIX_HTTPS = "https://";
        internal const string URI_PREFIX_FTP = "ftp://";

        // Download task configuration
        internal const string DOWNLOAD_TASK_TRACE_LOG_FILETYPE = "EarthRotationUpdate";
        internal const string DOWNLOAD_TASK_NAME = "ASCOM - Update Earth Rotation Data"; // Name of the schedule job that runs the automatic download task
        internal const string DOWNLOAD_TASK_PATH = @"\" + DOWNLOAD_TASK_NAME; // Full schedule job path within the scheduler job tree. Has to be in the root for backward compatibility with XP!
        internal const string DOWNLOAD_TASK_EXECUTABLE_NAME = @"\ASCOM\Platform\Tools\EarthRotationUpdate.exe"; // File system location of the automatic download executable that is started by the scheduled task. The exe is placed here by the installer
        internal const string DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE = "Not published"; // Value to use for next leap seconds and its effective date before these are published
        internal const string DOWNLOAD_TASK_TIME_FORMAT = "dddd dd MMM yyyy - HH:mm:ss";
        internal const string DOWNLOAD_TASK_TRACE_DEFAULT_PATH_FORMAT = @"{0}\ASCOM\" + DOWNLOAD_TASK_TRACE_LOG_FILETYPE;
        internal const string DOWNLOAD_TASK_TRACE_FILE_NAME_FORMAT = @"{0}\Log {1}-{2}-{3} {4}{5}{6}";
        internal const string DOWNLOAD_TASK_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063";
        internal const int DOWNLOAD_TASK_TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH = 35;
        internal const int DOWNLOAD_TASK_NUMBER_OF_BACK_DAYS_OF_DELTAUT1_DATA_TO_LOAD = 20; // The download task will include this number of days of historic data as well as the current and all future data

        // Automatic update test configuration parameters - MUST BE SET TO 0 FOR PRODUCTION BUILDS!
        internal const int TEST_HISTORIC_DAYS_OFFSET = 0; // 1700 ' Offset in days to force the automatic update program to interpret historic leap second values as current and future values
        internal const int TEST_UTC_DAYS_OFFSET = 0; // 1011 ' Offset in days subtracted from the current time to force it to appear earlier than present in order to test correct leap second and DeltaUT1 values when leap seconds change
        internal const int TEST_UTC_HOURS_OFFSET = 0; // 10 ' Offset from current midnight in hours to be used to force the current time to appear as a specified value earlier than present in order to test correct leap second and DeltaUT1 values when leap seconds change
        internal const int TEST_UTC_MINUTES_OFFSET = 0; // 48 ' Offset in minutes to force the current time to appear earlier than present in order to test correct leap second and DeltaUT1 values when leap seconds change

        // Default values
        internal const string UPDATE_TYPE_DEFAULT = UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1; // Default value for earth rotation data source
        internal const string EARTH_ROTATION_DATA_LAST_UPDATED_DEFAULT = NEVER_UPDATED; // Default value for the scheduled job last run time
        internal const double MANUAL_DELTAUT1_DEFAULT = 0.0d; // Default value for the manual delta UT1 value
        internal const string AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT = NOT_DOWNLOADED; // Default value for the automatically downloaded number of leap seconds
        internal const string NEXT_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT = NOT_DOWNLOADED; // Default value for the next leap second
        internal const string NEXT_LEAP_SECONDS_DATE_NOT_AVAILABLE_DEFAULT = NOT_DOWNLOADED; // Default value for the next leap second effective date
        internal const string DOWNLOAD_TASK_INTERNET_DATA_SOURCE_DEFAULT = EARTH_ROTATION_INTERNET_DATA_SOURCE_0; // Default source for earth rotation files - JULY 2020 Changed to NASA because all USNO sites are unavailable until end of 2020
        internal const string DOWNLOAD_TASK_REPEAT_DEFAULT = SCHEDULE_REPEAT_WEEKLY; // Default repeat frequency for the automatic data download task
        internal const double DOWNLOAD_TASK_TIMEOUT_DEFAULT = 30.0d; // Default timeout in seconds for data transfers from earth rotation data sources
        internal const bool DOWNLOAD_TASK_TRACE_ENABLED_DEFAULT = true; // Initial state for download task trace output

        // Not available constants
        internal const double DOUBLE_VALUE_NOT_AVAILABLE = double.MinValue;
        internal readonly static DateTime DATE_VALUE_NOT_AVAILABLE = new(1, 1, 1);
        internal const string NOT_DOWNLOADED = "Not downloaded";
        internal const string NEVER_UPDATED = "Never";

        // Ultimate fallback-back value for number of leap seconds if all else fails
        internal const double LEAP_SECOND_ULTIMATE_FALLBACK_VALUE = 37.0d;

    }

    #region AstroUtil Enums and Structures
    /// <summary>
    /// Type of event for which an ephemeris is required
    /// </summary>
    /// <remarks></remarks>
    [Guid("946C9620-B292-4807-9B75-FC828AB1700B")]
    [ComVisible(true)]
    public enum EventType : int
    {
        /// <summary>
        /// SunRiseSunset
        /// </summary>
        SunRiseSunset = 0,
        /// <summary>
        /// MoonRiseMoonSet
        /// </summary>
        MoonRiseMoonSet = 1,
        /// <summary>
        /// CivilTwilight
        /// </summary>
        CivilTwilight = 2,
        /// <summary>
        /// NauticalTwilight
        /// </summary>
        NauticalTwilight = 3,
        /// <summary>
        /// AmateurAstronomicalTwilight
        /// </summary>
        AmateurAstronomicalTwilight = 4,
        /// <summary>
        /// AstronomicalTwilight
        /// </summary>
        AstronomicalTwilight = 5,
        /// <summary>
        /// MercuryRiseSet
        /// </summary>
        MercuryRiseSet = 6,
        /// <summary>
        /// VenusRiseSet
        /// </summary>
        VenusRiseSet = 7,
        /// <summary>
        /// MarsRiseSet
        /// </summary>
        MarsRiseSet = 8,
        /// <summary>
        /// JupiterRiseSet
        /// </summary>
        JupiterRiseSet = 9,
        /// <summary>
        /// SaturnRiseSet
        /// </summary>
        SaturnRiseSet = 10,
        /// <summary>
        /// UranusRiseSet
        /// </summary>
        UranusRiseSet = 11,
        /// <summary>
        /// NeptuneRiseSet
        /// </summary>
        NeptuneRiseSet = 12,
        /// <summary>
        /// PlutoRiseSet
        /// </summary>
        PlutoRiseSet = 13
    }
    #endregion

    #region NOVAS2 Enums
    /// <summary>
    /// Type of body, Major Planet, Moon, Sun or Minor Planet
    /// </summary>
    /// <remarks></remarks>
    [Guid("A1D2C046-F7BC-474f-8D95-6E7B761DEECB")]
    [ComVisible(true)]
    public enum BodyType : int
    {
        /// <summary>
        /// Luna
        /// </summary>
        /// <remarks></remarks>
        Moon = 0,

        /// <summary>
        /// The Sun
        /// </summary>
        /// <remarks></remarks>
        Sun = 0,

        /// <summary>
        /// Major planet
        /// </summary>
        /// <remarks></remarks>
        MajorPlanet = 0,

        /// <summary>
        /// Minor planet
        /// </summary>
        /// <remarks></remarks>
        MinorPlanet = 1,

        /// <summary>
        /// Comet
        /// </summary>
        /// <remarks></remarks>
        Comet = 2
    }

    /// <summary>
    /// Co-ordinate origin: centre of Sun or solar system barycentre
    /// </summary>
    /// <remarks></remarks>
    [Guid("9591FC6A-3EF1-41ae-9FE1-FE0C76686A85")]
    [ComVisible(true)]
    public enum Origin : int
    {
        /// <summary>
        /// Centre of mass of the solar system
        /// </summary>
        /// <remarks></remarks>
        Barycentric = 0,
        /// <summary>
        /// Centre of mass of the Sun
        /// </summary>
        /// <remarks></remarks>
        Heliocentric = 1
    }

    /// <summary>
    /// Body number starting with Mercury = 1
    /// </summary>
    /// <remarks></remarks>
    [Guid("C839867F-E152-44e1-8356-2DB450329EDC")]
    [ComVisible(true)]
    public enum Body : int
    {
        /// <summary>
        /// Mercury
        /// </summary>
        /// <remarks></remarks>
        Mercury = 1,
        /// <summary>
        /// Venus
        /// </summary>
        /// <remarks></remarks>
        Venus = 2,
        /// <summary>
        /// Earth
        /// </summary>
        /// <remarks></remarks>
        Earth = 3,
        /// <summary>
        /// Mars
        /// </summary>
        /// <remarks></remarks>
        Mars = 4,
        /// <summary>
        /// Jupiter
        /// </summary>
        /// <remarks></remarks>
        Jupiter = 5,
        /// <summary>
        /// Saturn
        /// </summary>
        /// <remarks></remarks>
        Saturn = 6,
        /// <summary>
        /// Uranus
        /// </summary>
        /// <remarks></remarks>
        Uranus = 7,
        /// <summary>
        /// Neptune
        /// </summary>
        /// <remarks></remarks>
        Neptune = 8,
        /// <summary>
        /// Pluto
        /// </summary>
        /// <remarks></remarks>
        Pluto = 9,
        /// <summary>
        /// Sun
        /// </summary>
        /// <remarks></remarks>
        Sun = 10,
        /// <summary>
        /// Moon
        /// </summary>
        /// <remarks></remarks>
        Moon = 11
    }

    /// <summary>
    /// Type of refraction correction
    /// </summary>
    /// <remarks></remarks>
    [Guid("32914B41-41C3-4f68-A974-E9ABCE0BA03A")]
    [ComVisible(true)]
    public enum RefractionOption : int
    {
        /// <summary>
        /// No refraction correction will be applied
        /// </summary>
        /// <remarks></remarks>
        NoRefraction = 0,
        /// <summary>
        /// Refraction will be applied based on "standard" weather values of temperature = 10.0C and sea level pressure = 1010 millibar
        /// </summary>
        /// <remarks></remarks>
        StandardRefraction = 1,
        /// <summary>
        /// Refraction will be applied based on the temperature and pressure supplied in the site location structure
        /// </summary>
        /// <remarks></remarks>
        LocationRefraction = 2
    }

    /// <summary>
    /// Type of transformation: Epoch, Equator and Equinox or all three
    /// </summary>
    /// <remarks></remarks>
    [Guid("6ADE707E-1D7E-471a-94C9-F9FCC56755B1")]
    [ComVisible(true)]
    public enum TransformationOption : int
    {
        /// <summary>
        /// Change epoch only
        /// </summary>
        /// <remarks></remarks>
        ChangeEpoch = 1,
        /// <summary>
        /// Change equator and equinox
        /// </summary>
        /// <remarks></remarks>
        ChangeEquatorAndEquinox = 2,
        /// <summary>
        /// Change equator, equinox and epoch
        /// </summary>
        /// <remarks></remarks>
        ChangeEquatorAndEquinoxAndEpoch = 3
    }

    /// <summary>
    /// Direction of nutation correction
    /// </summary>
    /// <remarks></remarks>
    [Guid("394E0981-3344-4cff-8ABA-E19A775AAD29")]
    [ComVisible(true)]
    public enum NutationDirection : int
    {
        /// <summary>
        /// Convert mean equator and equinox to true equator and equinox
        /// </summary>
        /// <remarks></remarks>
        MeanToTrue = 0,
        /// <summary>
        /// Convert true equator and equinox to mean equator and equinox
        /// </summary>
        /// <remarks></remarks>
        TrueToMean = 1
    }
    #endregion

    #region NOVAS3 Enums
    /// <summary>
    /// Direction of transformation: ITRS to Terrestrial Intermediate or vice versa
    /// </summary>
    /// <remarks></remarks>
    [Guid("45EAC3DA-08FB-49E2-B852-114312933742")]
    [ComVisible(true)]
    public enum TransformationDirection : short
    {
        /// <summary>
        /// ITRSToTerrestrialIntermediate
        /// </summary>
        ITRSToTerrestrialIntermediate = 0,
        /// <summary>
        /// TerrestrialIntermediateToITRS
        /// </summary>
        TerrestrialIntermediateToITRS = 1
    }
    /// <summary>
    /// Location of observer
    /// </summary>
    /// <remarks></remarks>
    [Guid("8FFEAC07-F976-4fd8-8547-3DCFF25F5FA3")]
    [ComVisible(true)]
    public enum ObserverLocation : short
    {
        /// <summary>
        /// Observer at centre of the earth
        /// </summary>
        /// <remarks></remarks>
        EarthGeoCenter = 0,
        /// <summary>
        /// Observer on earth's surface
        /// </summary>
        /// <remarks></remarks>
        EarthSurface = 1,
        /// <summary>
        /// Observer in near-earth spacecraft
        /// </summary>
        /// <remarks></remarks>
        SpaceNearEarth = 2
    }

    /// <summary>
    /// Calculation accuracy
    /// </summary>
    /// <remarks>
    /// In full-accuracy mode,
    /// <list type="bullet">
    /// <item>nutation calculations use the IAU 2000A model [iau2000a, nutation_angles];</item>
    /// <item>gravitational deflection is calculated using three bodies: Sun, Jupiter, and Saturn [grav_def];</item>
    /// <item>the equation of the equinoxes includes the entire series when computing the “complementary terms" [ee_ct];</item>
    /// <item>geocentric positions of solar system bodies are adjusted for light travel time using split, or two-part, 
    /// Julian dates in calls to ephemeris and iterate with a convergence tolerance of 10-12 days [light_time, ephemeris];</item>
    /// <item>ephemeris calls the appropriate solar system ephemeris using split, or two-part, Julian dates primarily to support 
    /// light-time calculations [ephemeris, solarsystem_hp, light_time].</item>
    /// </list>
    /// <para>In reduced-accuracy mode,</para>
    /// <list type="bullet">
    /// <item>nutation calculations use the 2000K model, which is the default for this mode;</item>
    /// <item>gravitational deflection is calculated using only one body, the Sun [grav_def];</item>
    /// <item>the equation of the equinoxes excludes terms smaller than 2 micro arc seconds when computing the "complementary terms" [ee_ct];</item>
    /// <item>geocentric positions of solar system bodies are adjusted for light travel time using single-value Julian dates 
    /// in calls to ephemeris and iterate with a convergence tolerance of 10-9 days [light-time, ephemeris, solar system];</item>
    /// <item>ephemeris calls the appropriate solar system ephemeris using single-value Julian dates [ephemeris, solar system].</item>
    /// </list>
    /// <para>In full-accuracy mode, the IAU 2000A nutation series (1,365 terms) is used [iau2000a]. Evaluating the series for nutation is 
    /// usually the main computational burden in NOVAS, so using reduced-accuracy mode improves execution time, often noticeably. 
    /// In reduced-accuracy mode, the NOVAS 2000K nutation series (488 terms) is used by default [nu2000k]. This mode can be used 
    /// when the accuracy requirements are not better than 0.1 milliarcsecond for stars or 3.5 milli arc-seconds for solar system bodies. 
    /// Selecting this approach can reduce the time required for Earth-rotation computations by about two-thirds.</para>
    /// </remarks>
    [Guid("F10B748F-4F90-4acf-9EB0-76D50293E9A9")]
    [ComVisible(true)]
    public enum Accuracy : short
    {
        /// <summary>
        /// Full accuracy
        /// </summary>
        /// <remarks>Suitable when precision of better than 0.1 milli arc-second for stars or 3.5 milli arc-seconds for solar system bodies is required.</remarks>
        Full = 0, // ... full accuracy
        /// <summary>
        /// Reduced accuracy
        /// </summary>
        /// <remarks>Suitable when precision of less than 0.1 milli arc-second for stars or 3.5 milli arc-seconds for solar system bodies is required.</remarks>
        Reduced = 1 // ... reduced accuracy
    }

    /// <summary>
    /// Coordinate system of the output position
    /// </summary>
    /// <remarks>Used by function Place</remarks>
    [Guid("0EF9BC38-B790-4416-8FEF-E03758B6B630")]
    [ComVisible(true)]
    public enum CoordSys : short
    {
        /// <summary>
        /// GCRS or "local GCRS"
        /// </summary>
        /// <remarks></remarks>
        GCRS = 0,
        /// <summary>
        /// True equator and equinox of date
        /// </summary>
        /// <remarks></remarks>
        EquinoxOfDate = 1,
        /// <summary>
        /// True equator and CIO of date
        /// </summary>
        /// <remarks></remarks>
        CIOOfDate = 2,
        /// <summary>
        /// Astrometric coordinates, i.e., without light deflection or aberration.
        /// </summary>
        /// <remarks></remarks>
        Astrometric = 3
    }

    /// <summary>
    /// Type of sidereal time
    /// </summary>
    /// <remarks></remarks>
    [Guid("7722AE51-F475-4c69-8B35-B2EDBD297C66")]
    [ComVisible(true)]
    public enum GstType : short
    {
        /// <summary>
        /// Greenwich mean sidereal time
        /// </summary>
        /// <remarks></remarks>
        GreenwichMeanSiderealTime = 0,
        /// <summary>
        /// Greenwich apparent sidereal time
        /// </summary>
        /// <remarks></remarks>
        GreenwichApparentSiderealTime = 1
    }

    /// <summary>
    /// Computation method
    /// </summary>
    /// <remarks></remarks>
    [Guid("8D9E6EF5-CE9C-4ba9-8B24-C0FA5067D8FA")]
    [ComVisible(true)]
    public enum Method : short
    {
        /// <summary>
        /// Based on CIO
        /// </summary>
        /// <remarks></remarks>
        CIOBased = 0,
        /// <summary>
        /// Based on equinox
        /// </summary>
        /// <remarks></remarks>
        EquinoxBased = 1
    }

    /// <summary>
    /// Output vector reference system
    /// </summary>
    /// <remarks></remarks>
    [Guid("CD7AEAC0-1BFA-447e-A43E-62C231B0FC55")]
    [ComVisible(true)]
    public enum OutputVectorOption : short
    {
        /// <summary>
        /// Referred to GCRS axes
        /// </summary>
        /// <remarks></remarks>
        ReferredToGCRSAxes = 0,
        /// <summary>
        /// Referred to the equator and equinox of date
        /// </summary>
        /// <remarks></remarks>
        ReferredToEquatorAndEquinoxOfDate = 1
    }

    /// <summary>
    /// Type of pole offset
    /// </summary>
    /// <remarks>Used by CelPole.</remarks>
    [Guid("AF69D7CC-A59C-4fcc-BE17-C2F568957BFD")]
    [ComVisible(true)]
    public enum PoleOffsetCorrection : short
    {
        /// <summary>
        /// For corrections to angular coordinates of modelled pole referred to mean ecliptic of date, that is, delta-delta-psi 
        /// and delta-delta-epsilon. 
        /// </summary>
        /// <remarks></remarks>
        ReferredToMeanEclipticOfDate = 1,
        /// <summary>
        /// For corrections to components of modelled pole unit vector referred to GCRS axes, that is, dx and dy.
        /// </summary>
        /// <remarks></remarks>
        ReferredToGCRSAxes = 2
    }

    /// <summary>
    /// Direction of frame conversion
    /// </summary>
    /// <remarks>Used by FrameTie method.</remarks>
    [Guid("3AC3E32A-EDCE-4234-AA50-CDB346851C5D")]
    [ComVisible(true)]
    public enum FrameConversionDirection : short
    {
        /// <summary>
        /// Dynamical to ICRS transformation.
        /// </summary>
        /// <remarks></remarks>
        DynamicalToICRS = -1,
        /// <summary>
        /// ICRS to dynamical transformation.
        /// </summary>
        /// <remarks></remarks>
        ICRSToDynamical = 1
    }

    /// <summary>
    /// Location of observer, determining whether the gravitational deflection due to the earth itself is applied.
    /// </summary>
    /// <remarks>Used by GravDef method.</remarks>
    [Guid("C39A798C-53F7-460b-853F-DA5389B4324D")]
    [ComVisible(true)]
    public enum EarthDeflection : short
    {
        /// <summary>
        /// No earth deflection (normally means observer is at geocenter)
        /// </summary>
        /// <remarks></remarks>
        NoEarthDeflection = 0,
        /// <summary>
        /// Add in earth deflection (normally means observer is on or above surface of earth, including earth orbit)
        /// </summary>
        /// <remarks></remarks>
        AddEarthDeflection = 1
    }

    /// <summary>
    /// Reference system in which right ascension is given
    /// </summary>
    /// <remarks></remarks>
    [Guid("DF215CCF-2C25-48e5-A357-A8300C1EA027")]
    [ComVisible(true)]
    public enum ReferenceSystem : short
    {
        /// <summary>
        /// GCRS
        /// </summary>
        /// <remarks></remarks>
        GCRS = 1,
        /// <summary>
        /// True equator and equinox of date
        /// </summary>
        /// <remarks></remarks>
        TrueEquatorAndEquinoxOfDate = 2
    }

    /// <summary>
    /// Type of equinox
    /// </summary>
    /// <remarks></remarks>
    [Guid("5EDEE8B3-E223-4fb7-924F-4C56E8373380")]
    [ComVisible(true)]
    public enum EquinoxType : short
    {
        /// <summary>
        /// Mean equinox
        /// </summary>
        /// <remarks></remarks>
        MeanEquinox = 0,
        /// <summary>
        /// True equinox
        /// </summary>
        /// <remarks></remarks>
        TrueEquinox = 1
    }

    /// <summary>
    /// Type of transformation
    /// </summary>
    /// <remarks></remarks>
    [Guid("8BBA934E-D874-48a2-A3E2-C842A7FFFB35")]
    [ComVisible(true)]
    public enum TransformationOption3 : short
    {
        /// <summary>
        /// Change epoch only
        /// </summary>
        /// <remarks></remarks>
        ChangeEpoch = 1,
        /// <summary>
        /// Change equator and equinox; sane epoch
        /// </summary>
        /// <remarks></remarks>
        ChangeEquatorAndEquinox = 2,
        /// <summary>
        /// Change equator, equinox and epoch
        /// </summary>
        /// <remarks></remarks>
        ChangeEquatorAndEquinoxAndEpoch = 3,
        /// <summary>
        /// change equator and equinox J2000.0 to ICRS
        /// </summary>
        /// <remarks></remarks>
        ChangeEquatorAndEquinoxJ2000ToICRS = 4,
        /// <summary>
        /// change ICRS to equator and equinox of J2000.0
        /// </summary>
        /// <remarks></remarks>
        ChangeICRSToEquatorAndEquinoxOfJ2000 = 5
    }

    /// <summary>
    /// Type of object
    /// </summary>
    /// <remarks></remarks>
    [Guid("5BBB931B-358C-40ac-921C-B48373F01348")]
    [ComVisible(true)]
    public enum ObjectType : short
    {
        /// <summary>
        /// Major planet, sun or moon
        /// </summary>
        /// <remarks></remarks>
        MajorPlanetSunOrMoon = 0,
        /// <summary>
        /// Minor planet
        /// </summary>
        /// <remarks></remarks>
        MinorPlanet = 1,
        /// <summary>
        /// Object located outside the solar system
        /// </summary>
        /// <remarks></remarks>
        ObjectLocatedOutsideSolarSystem = 2
    }

    /// <summary>
    /// Body or location
    /// </summary>
    /// <remarks>This numbering convention is used by ephemeris routines; do not confuse with the Body enum, which is used in most 
    /// other places within NOVAS3.
    /// <para>
    /// The numbering convention for 'target' and 'centre' is:
    /// <pre>
    ///             0  =  Mercury           7 = Neptune
    ///             1  =  Venus             8 = Pluto
    ///             2  =  Earth             9 = Moon
    ///             3  =  Mars             10 = Sun
    ///             4  =  Jupiter          11 = Solar system barycentre.
    ///             5  =  Saturn           12 = Earth-Moon barycentre.
    ///             6  =  Uranus           13 = Nutations (long. and oblique.)</pre>
    /// </para>
    /// <para>
    /// If nutations are desired, set 'target' = 14; 'centre' will be ignored on that call.
    /// </para>
    /// </remarks>
    [Guid("60E342F9-3CC3-4b98-8045-D61B5A7D974B")]
    [ComVisible(true)]
    public enum Target : short
    {
        /// <summary>
        /// Mercury
        /// </summary>
        /// <remarks></remarks>
        Mercury = 0,
        /// <summary>
        /// Venus
        /// </summary>
        /// <remarks></remarks>
        Venus = 1,
        /// <summary>
        /// Earth
        /// </summary>
        /// <remarks></remarks>
        Earth = 2,
        /// <summary>
        /// Mars
        /// </summary>
        /// <remarks></remarks>
        Mars = 3,
        /// <summary>
        /// Jupiter
        /// </summary>
        /// <remarks></remarks>
        Jupiter = 4,
        /// <summary>
        /// Saturn
        /// </summary>
        /// <remarks></remarks>
        Saturn = 5,
        /// <summary>
        /// Uranus
        /// </summary>
        /// <remarks></remarks>
        Uranus = 6,
        /// <summary>
        /// Neptune
        /// </summary>
        /// <remarks></remarks>
        Neptune = 7,
        /// <summary>
        /// Pluto
        /// </summary>
        /// <remarks></remarks>
        Pluto = 8,
        /// <summary>
        /// Moon
        /// </summary>
        /// <remarks></remarks>
        Moon = 9,
        /// <summary>
        /// Sun
        /// </summary>
        /// <remarks></remarks>
        Sun = 10,
        /// <summary>
        /// Solar system barycentre
        /// </summary>
        /// <remarks></remarks>
        SolarSystemBarycentre = 11,
        /// <summary>
        /// Earth moon barycentre
        /// </summary>
        /// <remarks></remarks>
        EarthMoonBarycentre = 12,
        /// <summary>
        /// Nutations
        /// </summary>
        /// <remarks></remarks>
        Nutations = 13
    }
    #endregion

    #region Public NOVAS 2 Structures
    /// <summary>
    /// Structure to hold body type, number and name
    /// </summary>
    /// <remarks>Designates a celestial object.
    /// </remarks>
    [Guid("558F644F-E112-4e88-9D79-20063BB25C3E")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct BodyDescription
    {
        /// <summary>
        /// Type of body
        /// </summary>
        /// <remarks>
        /// 0 = Major planet, Sun, or Moon
        /// 1 = Minor planet
        /// </remarks>
        public BodyType Type;
        /// <summary>
        /// body number
        /// </summary>
        /// <remarks><pre>
        /// For 'type' = 0: Mercury = 1, ..., Pluto = 9, Sun = 10, Moon = 11
        /// For 'type' = 1: minor planet number
        /// </pre></remarks>
        public Body Number;
        /// <summary>
        /// Name of the body (limited to 99 characters)
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.BStr, SizeConst = 100)]
        public string Name; // char[100]
    }

    /// <summary>
    /// Structure to hold astrometric catalogue data
    /// </summary>
    /// <remarks>
    /// The astrometric catalogue data for a star; equator and equinox and units will depend on the catalogue. 
    /// While this structure can be used as a generic container for catalogue data, all high-level 
    /// NOVAS-C functions require J2000.0 catalogue data with FK5-type units (shown in square brackets below).
    /// </remarks>
    [Guid("6320FEDA-8582-4048-988A-7D4DE7978C71")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CatEntry
    {
        /// <summary>
        /// 3-character catalogue designator. 
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.BStr, SizeConst = 4)]
        public string Catalog; // char[4] was <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=4)> Was this before changing for COM compatibility

        /// <summary>
        /// Name of star.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.BStr, SizeConst = 51)]
        public string StarName; // char[51] was <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=51)> _

        /// <summary>
        /// Integer identifier assigned to star.
        /// </summary>
        /// <remarks></remarks>
        public int StarNumber;

        /// <summary>
        /// Mean right ascension [hours].
        /// </summary>
        /// <remarks></remarks>
        public double RA;

        /// <summary>
        /// Mean declination [degrees].
        /// </summary>
        /// <remarks></remarks>
        public double Dec;

        /// <summary>
        /// Proper motion in RA [seconds of time per century].
        /// </summary>
        /// <remarks></remarks>
        public double ProMoRA;

        /// <summary>
        /// Proper motion in declination [arc seconds per century].
        /// </summary>
        /// <remarks></remarks>
        public double ProMoDec;

        /// <summary>
        /// Parallax [arc seconds].
        /// </summary>
        /// <remarks></remarks>
        public double Parallax;

        /// <summary>
        /// Radial velocity [kilometres per second]
        /// </summary>
        /// <remarks></remarks>
        public double RadialVelocity;
    }

    /// <summary>
    /// Structure to hold site information
    /// </summary>
    /// <remarks>
    /// Data for the observer's location.  The atmospheric parameters are used only by the refraction 
    /// function called from function 'equ_to_hor'. Additional parameters can be added to this 
    /// structure if a more sophisticated refraction model is employed.
    /// </remarks>
    [Guid("ED02B64A-320F-47cd-90D9-3DF2DF07602D")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SiteInfo
    {
        /// <summary>
        /// Geodetic latitude in degrees; north positive.
        /// </summary>
        /// <remarks></remarks>
        public double Latitude; // geodetic latitude in degrees; north positive.
        /// <summary>
        /// Geodetic longitude in degrees; east positive.
        /// </summary>
        /// <remarks></remarks>
        public double Longitude; // geodetic longitude in degrees; east positive.
        /// <summary>
        /// Height of the observer in meters.
        /// </summary>
        /// <remarks></remarks>
        public double Height; // height of the observer in meters.
        /// <summary>
        /// Temperature (degrees Celsius).
        /// </summary>
        /// <remarks></remarks>
        public double Temperature; // temperature (degrees Celsius).
        /// <summary>
        /// Atmospheric pressure (millibars)
        /// </summary>
        /// <remarks></remarks>
        public double Pressure; // atmospheric pressure (millibars)
    }

    /// <summary>
    /// Structure to hold a position vector
    /// </summary>
    /// <remarks>Object position vector
    /// </remarks>
    [Guid("69651C90-75F5-4f46-8D0F-22D186151D45")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PosVector
    {
        /// <summary>
        /// x co-ordinate
        /// </summary>
        /// <remarks></remarks>
        public double x;
        /// <summary>
        /// y co-ordinate
        /// </summary>
        /// <remarks></remarks>
        public double y;
        /// <summary>
        /// z co-ordinate
        /// </summary>
        /// <remarks></remarks>
        public double z;
    }

    /// <summary>
    /// Structure to hold a velocity vector
    /// </summary>
    /// <remarks>Object velocity vector
    /// </remarks>
    [Guid("F18240B0-00CC-4ff7-9A94-AC835387F959")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct VelVector
    {
        /// <summary>
        /// x velocity component 
        /// </summary>
        /// <remarks></remarks>
        public double x;
        /// <summary>
        /// y velocity component
        /// </summary>
        /// <remarks></remarks>
        public double y;
        /// <summary>
        /// z velocity component
        /// </summary>
        /// <remarks></remarks>
        public double z;
    }

    /// <summary>
    /// Structure to hold Sun and Moon fundamental arguments
    /// </summary>
    /// <remarks>Fundamental arguments, in radians
    /// </remarks>
    [Guid("5EE28FFB-39CD-4d23-BF62-11EE4C581681")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FundamentalArgs
    {
        /// <summary>
        /// l (mean anomaly of the Moon)
        /// </summary>
        /// <remarks></remarks>
        public double l;
        /// <summary>
        /// l' (mean anomaly of the Sun)
        /// </summary>
        /// <remarks></remarks>
        public double ldash;
        /// <summary>
        /// F (L - omega; L = mean longitude of the Moon)
        /// </summary>
        /// <remarks></remarks>
        public double F;
        /// <summary>
        /// D (mean elongation of the Moon from the Sun)
        /// </summary>
        /// <remarks></remarks>
        public double D;
        /// <summary>
        /// Omega (mean longitude of the Moon's ascending node)
        /// </summary>
        /// <remarks></remarks>
        public double Omega;
    }
    #endregion

    #region Public NOVAS 3 Structures

    /// <summary>
    /// Catalogue entry structure
    /// </summary>
    /// <remarks>Basic astrometric data for any celestial object located outside the solar system; the catalogue data for a star.
    /// <para>This structure is identical to the NOVAS2 CatEntry structure expect that, for some reason, the StarName and Catalogue fields
    /// have been swapped in the NOVAS3 structure.</para>
    /// <para>
    /// Please note that some units have changed from those used in NOVAS2 as follows:
    /// <list type="bullet">
    /// <item>proper motion in right ascension: from seconds per century to milliarcseconds per year</item>
    /// <item>proper motion in declination: from arc seconds per century to milliarcseconds per year</item>
    /// <item>parallax: from arc seconds to milliarcseconds</item>
    /// </list>
    /// </para>
    /// </remarks>
    [Guid("5325E96C-BD24-4470-A0F6-E917B05805E1")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CatEntry3
    {
        /// <summary>
        /// Name of celestial object. (maximum 50 characters)
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string StarName;

        /// <summary>
        /// 3-character catalogue designator. 
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Catalog;

        /// <summary>
        /// Integer identifier assigned to object.
        /// </summary>
        /// <remarks></remarks>
        public int StarNumber;

        /// <summary>
        /// ICRS right ascension (hours)
        /// </summary>
        /// <remarks></remarks>
        public double RA;

        /// <summary>
        /// ICRS declination (degrees)
        /// </summary>
        /// <remarks></remarks>
        public double Dec;

        /// <summary>
        /// ICRS proper motion in right ascension (milliarcseconds/year)
        /// </summary>
        /// <remarks></remarks>
        public double ProMoRA;

        /// <summary>
        /// ICRS proper motion in declination (milliarcseconds/year)
        /// </summary>
        /// <remarks></remarks>
        public double ProMoDec;

        /// <summary>
        /// Parallax (milliarcseconds)
        /// </summary>
        /// <remarks></remarks>
        public double Parallax;

        /// <summary>
        /// Radial velocity (km/s)
        /// </summary>
        /// <remarks></remarks>
        public double RadialVelocity;
    }

    /// <summary>
    /// Celestial object structure
    /// </summary>
    /// <remarks>Designates a celestial object</remarks>
    [Guid("AEFE0EA0-D013-46a9-B77D-6D0FDD661005")]
    [ComVisible(true)]
    public struct Object3
    {
        /// <summary>
        /// Type of object
        /// </summary>
        /// <remarks></remarks>
        public ObjectType Type;
        /// <summary>
        /// Object identification number
        /// </summary>
        /// <remarks></remarks>
        public Body Number;
        /// <summary>
        /// Name of object(maximum 50 characters)
        /// </summary>
        /// <remarks></remarks>
        public string Name;
        /// <summary>
        /// Catalogue entry for the object
        /// </summary>
        /// <remarks></remarks>
        public CatEntry3 Star;
    }

    /// <summary>
    /// Celestial object's place in the sky
    /// </summary>
    /// <remarks></remarks>
    [Guid("9AD852C3-A895-4f69-AEC0-C9CA44283FA0")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SkyPos
    {
        /// <summary>
        /// Unit vector toward object (dimensionless)
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R8)]
        public double[] RHat;
        /// <summary>
        /// Apparent, topocentric, or astrometric right ascension (hours)
        /// </summary>
        /// <remarks></remarks>
        public double RA;
        /// <summary>
        /// Apparent, topocentric, or astrometric declination (degrees)
        /// </summary>
        /// <remarks></remarks>
        public double Dec;
        /// <summary>
        /// True (geometric, Euclidean) distance to solar system body or 0.0 for star (AU)
        /// </summary>
        /// <remarks></remarks>
        public double Dis;
        /// <summary>
        /// Radial velocity (km/s)
        /// </summary>
        /// <remarks></remarks>
        public double RV;
    }

    /// <summary>
    /// Observer’s position and velocity in a near-Earth spacecraft.
    /// </summary>
    /// <remarks></remarks>
    [Guid("15737EA5-E4FA-40da-8BDA-B8CF96D89E43")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct InSpace
    {
        /// <summary>
        /// Geocentric position vector (x, y, z), components in km with respect to true equator and equinox of date
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R8)]
        public double[] ScPos;
        /// <summary>
        /// Geocentric velocity vector (x_dot, y_dot, z_dot), components in km/s with respect to true equator and equinox of date
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R8)]
        public double[] ScVel;
    }

    /// <summary>
    /// Right ascension of the Celestial Intermediate Origin (CIO) with respect to the GCRS.
    /// </summary>
    /// <remarks></remarks>
    [Guid("4959930F-0CDB-4324-A0E0-F60A351454B7")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct RAOfCio
    {
        /// <summary>
        /// TDB Julian date
        /// </summary>
        /// <remarks></remarks>
        public double JdTdb;
        /// <summary>
        /// Right ascension of the CIO with respect to the GCRS (arc seconds)
        /// </summary>
        /// <remarks></remarks>
        public double RACio;
    }

    /// <summary>
    /// Parameters of observer's location
    /// </summary>
    /// <remarks>This structure is identical to the NOVAS2 SiteInfo structure but is included so that NOVAS3 naming
    /// conventions are maintained, making it easier to relate this code to the NOVAS3 documentation and C code.</remarks>
    [Guid("277380A5-6599-448f-9232-1C280073D3CD")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct OnSurface
    {
        /// <summary>
        /// Geodetic (ITRS) latitude; north positive (degrees)
        /// </summary>
        /// <remarks></remarks>
        public double Latitude;
        /// <summary>
        /// Geodetic (ITRS) longitude; east positive (degrees)
        /// </summary>
        /// <remarks></remarks>
        public double Longitude;
        /// <summary>
        /// Observer's height above sea level
        /// </summary>
        /// <remarks></remarks>
        public double Height;
        /// <summary>
        /// Observer's location's ambient temperature (degrees Celsius)
        /// </summary>
        /// <remarks></remarks>
        public double Temperature;
        /// <summary>
        /// Observer's location's atmospheric pressure (millibars)
        /// </summary>
        /// <remarks></remarks>
        public double Pressure;
    }

    /// <summary>
    /// General specification for the observer's location
    /// </summary>
    /// <remarks></remarks>
    [Guid("64A25FDD-3687-45e0-BEAF-18C361E5E340")]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct Observer
    {
        /// <summary>
        /// Code specifying the location of the observer: 0=at geocenter; 1=surface of earth; 2=near-earth spacecraft
        /// </summary>
        /// <remarks></remarks>
        public ObserverLocation Where;
        /// <summary>
        /// Data for an observer's location on the surface of the Earth (where = 1)
        /// </summary>
        /// <remarks></remarks>
        public OnSurface OnSurf;
        /// <summary>
        /// Data for an observer's location on a near-Earth spacecraft (where = 2)
        /// </summary>
        /// <remarks></remarks>
        public InSpace NearEarth;
    }
    #endregion

    #region Internal NOVAS3 Structures
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct JDHighPrecision
    {
        public double JDPart1;
        public double JDPart2;
    }

    // Internal version of Object3 with correct marshalling hints and type for Number field
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct Object3Internal
    {
        public ObjectType Type;
        public short Number;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string Name;
        public CatEntry3 Star;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAOfCioArray
    {

        internal RAOfCio Value1;
        internal RAOfCio Value2;
        internal RAOfCio Value3;
        internal RAOfCio Value4;
        internal RAOfCio Value5;
        internal RAOfCio Value6;
        internal RAOfCio Value7;
        internal RAOfCio Value8;
        internal RAOfCio Value9;
        internal RAOfCio Value10;
        internal RAOfCio Value11;
        internal RAOfCio Value12;
        internal RAOfCio Value13;
        internal RAOfCio Value14;
        internal RAOfCio Value15;
        internal RAOfCio Value16;
        internal RAOfCio Value17;
        internal RAOfCio Value18;
        internal RAOfCio Value19;
        internal RAOfCio Value20;

        internal void Initialise()
        {
            Value1.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value2.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value3.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value4.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value5.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value6.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value7.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value8.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value9.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value10.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value11.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value12.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value13.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value14.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value15.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value16.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value17.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value18.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value19.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
            Value20.RACio = GlobalItems.RACIO_DEFAULT_VALUE;
        }
    }
}
#endregion
