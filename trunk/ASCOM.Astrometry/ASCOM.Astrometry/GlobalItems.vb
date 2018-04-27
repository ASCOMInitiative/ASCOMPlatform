Imports System.Runtime.InteropServices
Imports System.Globalization
Imports ASCOM.Utilities
Imports System.IO

Module GlobalItems

    ' Physical contants
    Friend Const MOON_RADIUS As Double = 1737.0 ' km
    Friend Const EARTH_RADIUS As Double = 6378.0 ' km
    Friend Const SUN_RADIUS As Double = 696342.0 ' km
    Friend Const MERCURY_RADIUS As Double = 2439.7 ' km
    Friend Const VENUS_RADIUS As Double = 2439.7 ' km
    Friend Const MARS_RADIUS As Double = 3396.2 ' km
    Friend Const JUPITER_RADIUS As Double = 69911.0 ' km
    Friend Const SATURN_RADIUS As Double = 6051.8 ' km
    Friend Const NEPTUNE_RADIUS As Double = 24767.0 ' km
    Friend Const URANUS_RADIUS As Double = 24973.0 ' km
    Friend Const PLUTO_RADIUS As Double = 1153.0 ' km

    ' Fixed event definitions
    Friend Const SUN_RISE As Double = -50.0 / 60.0 ' Degrees
    Friend Const CIVIL_TWILIGHT As Double = -6.0 ' Degrees
    Friend Const NAUTICAL_TWILIGHT As Double = -12.0 ' Degrees
    Friend Const AMATEUR_ASRONOMICAL_TWILIGHT As Double = -15.0 ' Degrees
    Friend Const ASTRONOMICAL_TWILIGHT As Double = -18.0 ' Degrees

    ' Conversion factors
    Friend Const HOURS2DEG As Double = 15.0
    Friend Const DEG2HOURS As Double = 1.0 / 15.0
    Friend Const DEG2HOURSSOLSID As Double = 1.0 / 15.04107
    Friend Const SECONDS2DAYS As Double = 1.0 / (60.0 * 60.0 * 24.0)
    Friend Const AU2KILOMETRE As Double = 149597870.691

    'NOVAS.COM Constants
    Friend Const FN1 As Short = 1
    Friend Const FN0 As Short = 0
    Friend Const J2000BASE As Double = 2451545.0 'TDB Julian date of epoch J2000.0.
    Friend Const KMAU As Double = 149597870.0 'Astronomical Unit in kilometers.
    Friend Const MAU As Double = 149597870000.0 'Astronomical Unit in meters.
    Friend Const C As Double = 173.14463348 ' Speed of light in AU/Day.
    Friend Const GS As Double = 1.32712438E+20 ' Heliocentric gravitational constant.
    Friend Const EARTHRAD As Double = 6378.14 'Radius of Earth in kilometers.
    Friend Const F As Double = 0.00335281 'Earth ellipsoid flattening.
    Friend Const OMEGA As Double = 0.00007292115 'Rotational angular velocity of Earth in radians/sec.
    Friend Const TWOPI As Double = 6.2831853071795862 'Value of pi in radians.
    Friend Const RAD2SEC As Double = 206264.80624709636 'Angle conversion constants.
    Friend Const DEG2RAD As Double = 0.017453292519943295
    Friend Const RAD2DEG As Double = 57.295779513082323

    'General constants
    Friend Const TT_TAI_OFFSET As Double = 32.184 '32.184 seconds
    Friend Const MODIFIED_JULIAN_DAY_OFFSET As Double = 2400000.5 'This is the offset of Modified Julian dates from true Julian dates
    Friend Const SECPERDAY As Double = 86400.0
    Friend Const DELTAUT1_BOUND As Double = 0.9 ' Used to validate delta UT1 values input manually or automatically downloaded, which must line in the range -DELTAUT1_BOUND to +DELTAUT1_BOUND
    Friend Const TROPICAL_YEAR_IN_DAYS As Double = 365.24219

    Friend Const OLE_AUTOMATION_JULIAN_DATE_OFFSET As Double = 2415018.5 ' Offset of OLE automation dates from Julian dates
    Friend Const RACIO_DEFAULT_VALUE As Double = Double.NaN 'NOVAS3: Default value that if still present will indicate that this value was not updated

    'Profile store Key names
    Friend Const ASTROMETRY_SUBKEY = "Astrometry"
    Friend Const AUTOMATIC_UPDATE_DELTAUT1_SUBKEY_NAME = ASTROMETRY_SUBKEY & "\Latest Delta UT1 Data" ' Name of the Profile\Astrometry subkey in which automatically downloaded Delta UT1 predicted values will be stored
    Friend Const AUTOMATIC_UPDATE_LEAP_SECOND_HISTORY_SUBKEY_NAME = ASTROMETRY_SUBKEY & "\Latest Leap Second Data" ' Name of the Profile\Astrometry subkey in which automatically downloaded historic leap second values will be stored

    'Profile store value names
    Friend Const UPDATE_TYPE_VALUE_NAME As String = "UTC and UT1 Data Update Method" ' Value name in Profile/Astrometry that determines how earth rotation data is updated: None, Automatic download, Manual entry, Built-in prediction.
    Friend Const EARTH_ROTATION_DATA_LAST_UPDATED_VALUE_NAME As String = "Automatic Data Last Updated" ' Value name for the date and time that the scheduled task was last run
    Friend Const MANUAL_LEAP_SECONDS_VALUENAME As String = "Manual Leap Seconds" ' Name of the manually updated leap second value
    Friend Const MANUAL_DELTAUT1_VALUE_NAME As String = "Manual Delta UT1" ' Value name in Astrometry for manually entered Delta UT1 values
    Friend Const AUTOMATIC_LEAP_SECONDS_VALUENAME As String = "Automatic Leap Seconds" ' Name of the automatically updated leap second value
    Friend Const NEXT_LEAP_SECONDS_VALUENAME As String = "Automatic Next Leap Seconds" ' Name of the automatically updated next leap second value
    Friend Const NEXT_LEAP_SECONDS_DATE_VALUENAME As String = "Automatic Next Leap Seconds Date" ' Name of the automatically updated next leap second commencement date value
    Friend Const DELTAUT1_VALUE_NAME_FORMAT As String = "Delta UT1 Prediction for {0} - {1} - {2}" ' Format string for automatically downloaded delta UT1 value names. The 0, 1 and 2 placeholders are for year, month and day integers
    Friend Const DELTAUT1_VALUE_NAME_YEAR_FORMAT As String = "0000" ' Format string for the year component of automatically downloaded delta UT1 value names.
    Friend Const DELTAUT1_VALUE_NAME_MONTH_FORMAT As String = "00" ' Format string for the month component of automatically downloaded delta UT1 value names.
    Friend Const DELTAUT1_VALUE_NAME_DAY_FORMAT As String = "00" ' Format string for the day component of automatically downloaded delta UT1 value names.
    Friend Const DOWNLOAD_TASK_DATA_SOURCE_VALUE_NAME As String = "Download Task Data Source" ' Name of the automatic data data source profile value
    Friend Const DOWNLOAD_TASK_TIMEOUT_VALUE_NAME As String = "Download Task Timeout" ' Name of the automatic update timeout profile value
    Friend Const DOWNLOAD_TASK_SCHEDULED_TIME_VALUE_NAME As String = "Download Task Scheduled Time" ' Value name for the scheduled job run time
    Friend Const DOWNLOAD_TASK_REPEAT_FREQUENCY_VALUE_NAME As String = "Download Task Repeat Frequency" ' Value name for the scheduled job run time
    Friend Const DOWNLOAD_TASK_TRACE_ENABLED_VALUE_NAME As String = "Download Task Trace Enabled" ' Value name for the scheduled job run time
    Friend Const DOWNLOAD_TASK_TRACE_PATH_VALUE_NAME As String = "Download Task Trace Path" ' Value name for the path to the scheduled job trace file

    ' Earth rotation data source names
    Friend Const UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1 As String = "Built-in leap seconds and predicted delta UT1" ' Alternative value for earth rotation data source
    Friend Const UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1 As String = "Manual leap seconds and manual delta UT1" ' Alternative value for earth rotation data source
    Friend Const UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1 As String = "Manual leap seconds and predicted delta UT1" ' Alternative value for earth rotation data source
    Friend Const UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1 As String = "On demand leap seconds and delta UT1" ' Alternative value for earth rotation data source
    Friend Const UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1 As String = "Automatic leap seconds and delta UT1" ' Alternative value for earth rotation data source

    ' Delta UT1 filename and format
    Friend Const DELTAUT1_FILE As String = "finals.daily" ' Name of the IERS file containing Delta UT1 predictions
    Friend Const DELTAUT1_YEAR_START As Integer = 0 : Friend Const DELTAUT1_YEAR_LENGTH As Integer = 2 ' Start position and length of the YEAR field in the finals.daily data line
    Friend Const DELTAUT1_MONTH_START As Integer = 2 : Friend Const DELTAUT1_MONTH_LENGTH As Integer = 2 ' Start position and length of the MONTH field in the finals.daily data line
    Friend Const DELTAUT1_DAY_START As Integer = 4 : Friend Const DELTAUT1_DAY_LENGTH As Integer = 2 ' Start position and length of the DAY field in the finals.daily data line
    Friend Const DELTAUT1_JULIAN_DATE_START As Integer = 7 : Friend Const DELTAUT1_JULIAN_DATE_LENGTH As Integer = 8 ' Start position and length of the JULKIAN DATE field in the finals.daily data line
    Friend Const DELTAUT1_START As Integer = 58 : Friend Const DELTAUT1_LENGTH As Integer = 10 ' Start position and length of the DELTAUT1 field in the finals.daily data line

    ' Leap seconds filename and format
    'Friend Const LEAP_SECONDS_FILE As String = "leapsec.dat" ' Name of the IERS file containing leap second historic and future values
    Friend Const LEAP_SECONDS_FILE As String = "tai-utc.dat" ' Name of the IERS file containing leap second historic and future values
    Friend Const LEAP_SECONDS_YEAR_START As Integer = 0 : Friend Const LEAP_SECONDS_YEAR_LENGTH As Integer = 5 ' Start position and length of the YEAR field in the tai-utc.dat data line
    Friend Const LEAP_SECONDS_MONTH_START As Integer = 5 : Friend Const LEAP_SECONDS_MONTH_LENGTH As Integer = 4 ' Start position and length of the MONTH field in the tai-utc.dat data line
    Friend Const LEAP_SECONDS_DAY_START As Integer = 9 : Friend Const LEAP_SECONDS_DAY_LENGTH As Integer = 4 ' Start position and length of the DAY field in the tai-utc.dat data line
    Friend Const LEAP_SECONDS_JULIAN_DATE_START As Integer = 17 : Friend Const LEAP_SECONDS_JULIAN_DATE_LENGTH As Integer = 10 ' Start position and length of the JULIAN DATE field in the tai-utc.dat data line
    Friend Const LEAP_SECONDS_LEAPSECONDS_START As Integer = 36 : Friend Const LEAP_SECONDS_LEAPSECONDS_LENGTH As Integer = 12 ' Start position and length of the NUMBER OF LEAP SECONDS field in the tai-utc.dat data line

    ' Earth rotation data download configuration options
    Friend Const EARTH_ROTATION_DATA_SOURCE_0 As String = "http://maia.usno.navy.mil/ser7/" ' SOurce options for for earth rotation files
    Friend Const EARTH_ROTATION_DATA_SOURCE_1 As String = "http://toshi.nofs.navy.mil/ser7/"
    Friend Const EARTH_ROTATION_DATA_SOURCE_2 As String = "ftp://cddis.gsfc.nasa.gov/pub/products/iers/"
    Friend Const EARTH_ROTATION_DATA_SOURCE_3 As String = "ftp://maia.usno.navy.mil/ser7/"
    Friend Const EARTH_ROTATION_DATA_SOURCE_4 As String = "ftp://toshi.nofs.navy.mil/ser7/"
    Friend Const SCHEDULE_REPEAT_NONE As String = "None" ' Options for autoamtic update schedule repeat frequency
    Friend Const SCHEDULE_REPEAT_DAILY As String = "Repeat daily"
    Friend Const SCHEDULE_REPEAT_WEEKLY As String = "Repeat weekly"
    Friend Const SCHEDULE_REPEAT_MONTHLY As String = "Repeat monthly"
    Friend Const URI_PREFIX_HTTP As String = "http://"
    Friend Const URI_PREFIX_HTTPS As String = "https://"
    Friend Const URI_PREFIX_FTP As String = "ftp://"

    ' Download task configuration
    Friend Const DOWNLOAD_TASK_TRACE_LOG_FILETYPE As String = "EarthRotationUpdate"
    Friend Const DOWNLOAD_TASK_NAME As String = "ASCOM - Update Earth Rotation Data" ' Name of the schedule job that runs the automatic download task
    Friend Const DOWNLOAD_TASK_PATH As String = "\" + DOWNLOAD_TASK_NAME ' Full schedule job path within the scheduler job tree. Has to be in the root for backward compatibilty with XP!
    Friend Const DOWNLOAD_TASK_EXECUTABLE_NAME As String = "\ASCOM\Platform 6\Tools\EarthRotationUpdate.exe" ' File system location of the automatic download executable that is started by the scheduled task. The exe is placed here by the installer
    Friend Const DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE As String = "Not published" ' Value to use for next leap seconds and its effective date before these are published
    Friend Const DOWNLOAD_TASK_TIME_FORMAT = "dddd dd MMM yyyy - HH:mm:ss"
    Friend Const DOWNLOAD_TASK_TRACE_DEFAULT_PATH_FORMAT = "{0}\ASCOM\" + DOWNLOAD_TASK_TRACE_LOG_FILETYPE
    Friend Const DOWNLOAD_TASK_TRACE_FILE_NAME_FORMAT = "{0}\Log {1}-{2}-{3} {4}{5}{6}"
    Friend Const DOWNLOAD_TASK_USER_AGENT As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063"
    Friend Const DOWNLOAD_TASK_TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH As Integer = 35
    Friend Const DOWNLOAD_TASK_NUMBER_OF_BACK_DAYS_OF_DELTAUT1_DATA_TO_LOAD As Integer = 20 ' The download task will include this number of days of historic data as well as the current and all future data

    ' Automatic update test configuration parameters - MUST BE SET TO 0 FOR PRODUCTION BUILDS!
    Friend Const TEST_HISTORIC_DAYS_OFFSET As Integer = 0 '1700 ' Offset in days to force the automatic update program to interpret historic leap second values as current and future values
    Friend Const TEST_UTC_DAYS_OFFSET As Integer = 0 '1011 ' Offset in days subtracted from the current time to force it to appear earlier than present in order to test correct leap second and DeltaUT1 values when leap seconds change
    Friend Const TEST_UTC_HOURS_OFFSET As Integer = 0 '10 ' Offset from current midnight in hours to be used to force the current time to appear as a specified valueearlier than present in order to test correct leap second and DeltaUT1 values when leap seconds change
    Friend Const TEST_UTC_MINUTES_OFFSET As Integer = 0 '48 ' Offset in minutes to force the current time to appear earlier than present in order to test correct leap second and DeltaUT1 values when leap seconds change

    ' Default values
    Friend Const UPDATE_TYPE_DEFAULT As String = UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1 ' Default value for earth rotation data source
    Friend Const EARTH_ROTATION_DATA_LAST_UPDATED_DEFAULT As String = NEVER_UPDATED ' Default value for the scheduled job last run time
    Friend Const MANUAL_DELTAUT1_DEFAULT As Double = 0.0 ' Default value for the manual delta UT1 value
    Friend Const AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT As String = NOT_DOWNLOADED ' Default value for the automatically downloaded number of leap seconds
    Friend Const NEXT_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT As String = NOT_DOWNLOADED ' Default value for the next leap second
    Friend Const NEXT_LEAP_SECONDS_DATE_NOT_AVAILABLE_DEFAULT As String = NOT_DOWNLOADED ' Default value for the next leap second effective date
    Friend Const DOWNLOAD_TASK_DATA_UPDATE_SOURCE_DEFAULT As String = EARTH_ROTATION_DATA_SOURCE_0 ' Default source for earth rotation files
    Friend Const DOWNLOAD_TASK_REPEAT_DEFAULT As String = SCHEDULE_REPEAT_WEEKLY ' Default repeat frequency for the automatic data download task
    Friend Const DOWNLOAD_TASK_TIMEOUT_DEFAULT As Double = 60.0 ' Default timeout in seconds for data transfers from earth rotation data sources
    Friend Const DOWNLOAD_TASK_TRACE_ENABLED_DEFAULT As Boolean = True ' Initial state for download task trace output

    ' Not available constants
    Friend Const DOUBLE_VALUE_NOT_AVAILABLE As Double = Double.MinValue
    Friend ReadOnly DATE_VALUE_NOT_AVAILABLE As DateTime = New DateTime(1, 1, 1)
    Friend Const NOT_DOWNLOADED As String = "Not downloaded"
    Friend Const NEVER_UPDATED As String = "Never"

End Module

#Region "AstroUtil Enums and Structures"
''' <summary>
''' Type of event for which an ephemeris is required
''' </summary>
''' <remarks></remarks>
<Guid("946C9620-B292-4807-9B75-FC828AB1700B"),
ComVisible(True)>
Public Enum EventType As Integer
    SunRiseSunset = 0
    MoonRiseMoonSet = 1
    CivilTwilight = 2
    NauticalTwilight = 3
    AmateurAstronomicalTwilight = 4
    AstronomicalTwilight = 5
    MercuryRiseSet = 6
    VenusRiseSet = 7
    MarsRiseSet = 8
    JupiterRiseSet = 9
    SaturnRiseSet = 10
    UranusRiseSet = 11
    NeptuneRiseSet = 12
    PlutoRiseSet = 13
End Enum
#End Region

#Region "NOVAS2 Enums"
''' <summary>
''' Type of body, Major Planet, Moon, Sun or Minor Planet
''' </summary>
''' <remarks></remarks>
<Guid("A1D2C046-F7BC-474f-8D95-6E7B761DEECB"),
ComVisible(True)>
Public Enum BodyType As Integer
    ''' <summary>
    ''' Luna
    ''' </summary>
    ''' <remarks></remarks>
    Moon = 0

    ''' <summary>
    ''' The Sun
    ''' </summary>
    ''' <remarks></remarks>
    Sun = 0

    ''' <summary>
    ''' Major planet
    ''' </summary>
    ''' <remarks></remarks>
    MajorPlanet = 0

    ''' <summary>
    ''' Minor planet
    ''' </summary>
    ''' <remarks></remarks>
    MinorPlanet = 1

    ''' <summary>
    ''' Comet
    ''' </summary>
    ''' <remarks></remarks>
    Comet = 2
End Enum

''' <summary>
''' Co-ordinate origin: centre of Sun or solar system barycentre
''' </summary>
''' <remarks></remarks>
<Guid("9591FC6A-3EF1-41ae-9FE1-FE0C76686A85"),
ComVisible(True)>
Public Enum Origin As Integer
    ''' <summary>
    ''' Centre of mass of the solar system
    ''' </summary>
    ''' <remarks></remarks>
    Barycentric = 0
    ''' <summary>
    ''' Centre of mass of the Sun
    ''' </summary>
    ''' <remarks></remarks>
    Heliocentric = 1
End Enum

''' <summary>
''' Body number starting with Mercury = 1
''' </summary>
''' <remarks></remarks>
<Guid("C839867F-E152-44e1-8356-2DB450329EDC"),
ComVisible(True)>
Public Enum Body As Integer
    ''' <summary>
    ''' Mercury
    ''' </summary>
    ''' <remarks></remarks>
    Mercury = 1
    ''' <summary>
    ''' Venus
    ''' </summary>
    ''' <remarks></remarks>
    Venus = 2
    ''' <summary>
    ''' Earth
    ''' </summary>
    ''' <remarks></remarks>
    Earth = 3
    ''' <summary>
    ''' Mars
    ''' </summary>
    ''' <remarks></remarks>
    Mars = 4
    ''' <summary>
    ''' Jupiter
    ''' </summary>
    ''' <remarks></remarks>
    Jupiter = 5
    ''' <summary>
    ''' Saturn
    ''' </summary>
    ''' <remarks></remarks>
    Saturn = 6
    ''' <summary>
    ''' Uranus
    ''' </summary>
    ''' <remarks></remarks>
    Uranus = 7
    ''' <summary>
    ''' Neptune
    ''' </summary>
    ''' <remarks></remarks>
    Neptune = 8
    ''' <summary>
    ''' Pluto
    ''' </summary>
    ''' <remarks></remarks>
    Pluto = 9
    ''' <summary>
    ''' Sun
    ''' </summary>
    ''' <remarks></remarks>
    Sun = 10
    ''' <summary>
    ''' Moon
    ''' </summary>
    ''' <remarks></remarks>
    Moon = 11
End Enum

''' <summary>
''' Type of refraction correction
''' </summary>
''' <remarks></remarks>
<Guid("32914B41-41C3-4f68-A974-E9ABCE0BA03A"),
ComVisible(True)>
Public Enum RefractionOption As Integer
    ''' <summary>
    ''' No refraction correction will be applied
    ''' </summary>
    ''' <remarks></remarks>
    NoRefraction = 0
    ''' <summary>
    ''' Refraction will be applied based on "standard" weather values of temperature = 10.0C and sea level pressure = 1010 millibar
    ''' </summary>
    ''' <remarks></remarks>
    StandardRefraction = 1
    ''' <summary>
    ''' Refraction will be applied based on the temperature and pressure supplied in the site location structure
    ''' </summary>
    ''' <remarks></remarks>
    LocationRefraction = 2
End Enum

''' <summary>
''' Type of transformation: Epoch, Equator and Equinox or all three
''' </summary>
''' <remarks></remarks>
<Guid("6ADE707E-1D7E-471a-94C9-F9FCC56755B1"),
ComVisible(True)>
Public Enum TransformationOption As Integer
    ''' <summary>
    ''' Change epoch only
    ''' </summary>
    ''' <remarks></remarks>
    ChangeEpoch = 1
    ''' <summary>
    ''' Change equator and equinox
    ''' </summary>
    ''' <remarks></remarks>
    ChangeEquatorAndEquinox = 2
    ''' <summary>
    ''' Change equator, equinox and epoch
    ''' </summary>
    ''' <remarks></remarks>
    ChangeEquatorAndEquinoxAndEpoch = 3
End Enum

''' <summary>
''' Direction of nutation correction
''' </summary>
''' <remarks></remarks>
<Guid("394E0981-3344-4cff-8ABA-E19A775AAD29"),
ComVisible(True)>
Public Enum NutationDirection As Integer
    ''' <summary>
    ''' Convert mean equator and equinox to true equator and equinox
    ''' </summary>
    ''' <remarks></remarks>
    MeanToTrue = 0
    ''' <summary>
    ''' Convert true equator and equinox to mean equator and equinox
    ''' </summary>
    ''' <remarks></remarks>
    TrueToMean = 1
End Enum
#End Region

#Region "NOVAS3 Enums"
''' <summary>
''' Direction of transformation: ITRS to Terrestrial Intermediate or vice versa
''' </summary>
''' <remarks></remarks>
<Guid("45EAC3DA-08FB-49E2-B852-114312933742"),
ComVisible(True)>
Public Enum TransformationDirection As Short
    ITRSToTerrestrialIntermediate = 0
    TerrestrialIntermediateToITRS = 1
End Enum
''' <summary>
''' Location of observer
''' </summary>
''' <remarks></remarks>
<Guid("8FFEAC07-F976-4fd8-8547-3DCFF25F5FA3"),
ComVisible(True)>
Public Enum ObserverLocation As Short
    ''' <summary>
    ''' Observer at centre of the earth
    ''' </summary>
    ''' <remarks></remarks>
    EarthGeoCenter = 0
    ''' <summary>
    ''' Observer on earth's surface
    ''' </summary>
    ''' <remarks></remarks>
    EarthSurface = 1
    ''' <summary>
    ''' Observer in near-earth spacecraft
    ''' </summary>
    ''' <remarks></remarks>
    SpaceNearEarth = 2
End Enum

''' <summary>
''' Calculation accuracy
''' </summary>
''' <remarks>
''' In full-accuracy mode,
''' <list type="bullet">
'''<item>nutation calculations use the IAU 2000A model [iau2000a, nutation_angles];</item>
'''<item>gravitational deflection is calculated using three bodies: Sun, Jupiter, and Saturn [grav_def];</item>
'''<item>the equation of the equinoxes includes the entire series when computing the “complementary terms" [ee_ct];</item>
'''<item>geocentric positions of solar system bodies are adjusted for light travel time using split, or two-part, 
''' Julian dates in calls to ephemeris and iterate with a convergence tolerance of 10-12 days [light_time, ephemeris];</item>
'''<item>ephemeris calls the appropriate solar system ephemeris using split, or two-part, Julian dates primarily to support 
''' light-time calculations [ephemeris, solarsystem_hp, light_time].</item>
''' </list>
'''<para>In reduced-accuracy mode,</para>
''' <list type="bullet">
''' <item>nutation calculations use the 2000K model, which is the default for this mode;</item>
''' <item>gravitational deflection is calculated using only one body, the Sun [grav_def];</item>
''' <item>the equation of the equinoxes excludes terms smaller than 2 microarcseconds when computing the "complementary terms" [ee_ct];</item>
''' <item>geocentric positions of solar system bodies are adjusted for light travel time using single-value Julian dates 
''' in calls to ephemeris and iterate with a convergence tolerance of 10-9 days [light-time, ephemeris, solarsystem];</item>
''' <item>ephemeris calls the appropriate solar system ephemeris using single-value Julian dates [ephemeris, solarsystem].</item>
''' </list>
''' <para>In full-accuracy mode, the IAU 2000A nutation series (1,365 terms) is used [iau2000a]. Evaluating the series for nutation is 
''' usually the main computational burden in NOVAS, so using reduced-accuracy mode improves execution time, often noticeably. 
''' In reduced-accuracy mode, the NOVAS 2000K nutation series (488 terms) is used by default [nu2000k]. This mode can be used 
''' when the accuracy requirements are not better than 0.1 milliarcsecond for stars or 3.5 milliarcseconds for solar system bodies. 
''' Selecting this approach can reduce the time required for Earth-rotation computations by about two-thirds.</para>
''' </remarks>
<Guid("F10B748F-4F90-4acf-9EB0-76D50293E9A9"),
ComVisible(True)>
Public Enum Accuracy As Short
    ''' <summary>
    ''' Full accuracy
    ''' </summary>
    ''' <remarks>Suitable when precision of better than 0.1 milliarcsecond for stars or 3.5 milliarcseconds for solar system bodies is required.</remarks>
    Full = 0 '... full accuracy
    ''' <summary>
    ''' Reduced accuracy
    ''' </summary>
    ''' <remarks>Suitable when precision of less than 0.1 milliarcsecond for stars or 3.5 milliarcseconds for solar system bodies is required.</remarks>
    Reduced = 1 '... reduced accuracy
End Enum

''' <summary>
''' Coordinate system of the output position
''' </summary>
''' <remarks>Used by function Place</remarks>
<Guid("0EF9BC38-B790-4416-8FEF-E03758B6B630"),
ComVisible(True)>
Public Enum CoordSys As Short
    ''' <summary>
    ''' GCRS or "local GCRS"
    ''' </summary>
    ''' <remarks></remarks>
    GCRS = 0
    ''' <summary>
    ''' True equator and equinox of date
    ''' </summary>
    ''' <remarks></remarks>
    EquinoxOfDate = 1
    ''' <summary>
    ''' True equator and CIO of date
    ''' </summary>
    ''' <remarks></remarks>
    CIOOfDate = 2
    ''' <summary>
    ''' Astrometric coordinates, i.e., without light deflection or aberration.
    ''' </summary>
    ''' <remarks></remarks>
    Astrometric = 3
End Enum

''' <summary>
''' Type of sidereal time
''' </summary>
''' <remarks></remarks>
<Guid("7722AE51-F475-4c69-8B35-B2EDBD297C66"),
ComVisible(True)>
Public Enum GstType As Short
    ''' <summary>
    ''' Greenwich mean sidereal time
    ''' </summary>
    ''' <remarks></remarks>
    GreenwichMeanSiderealTime = 0
    ''' <summary>
    ''' Greenwich apparent sidereal time
    ''' </summary>
    ''' <remarks></remarks>
    GreenwichApparentSiderealTime = 1
End Enum

''' <summary>
''' Computation method
''' </summary>
''' <remarks></remarks>
<Guid("8D9E6EF5-CE9C-4ba9-8B24-C0FA5067D8FA"),
ComVisible(True)>
Public Enum Method As Short
    ''' <summary>
    ''' Based on CIO
    ''' </summary>
    ''' <remarks></remarks>
    CIOBased = 0
    ''' <summary>
    ''' Based on equinox
    ''' </summary>
    ''' <remarks></remarks>
    EquinoxBased = 1
End Enum

''' <summary>
''' Output vector reference system
''' </summary>
''' <remarks></remarks>
<Guid("CD7AEAC0-1BFA-447e-A43E-62C231B0FC55"),
ComVisible(True)>
Public Enum OutputVectorOption As Short
    ''' <summary>
    ''' Referred to GCRS axes
    ''' </summary>
    ''' <remarks></remarks>
    ReferredToGCRSAxes = 0
    ''' <summary>
    ''' Referred to the equator and equinox of date
    ''' </summary>
    ''' <remarks></remarks>
    ReferredToEquatorAndEquinoxOfDate = 1
End Enum

''' <summary>
''' Type of pole ofset
''' </summary>
''' <remarks>Used by CelPole.</remarks>
<Guid("AF69D7CC-A59C-4fcc-BE17-C2F568957BFD"),
ComVisible(True)>
Public Enum PoleOffsetCorrection As Short
    ''' <summary>
    ''' For corrections to angular coordinates of modeled pole referred to mean ecliptic of date, that is, delta-delta-psi 
    ''' and delta-delta-epsilon. 
    ''' </summary>
    ''' <remarks></remarks>
    ReferredToMeanEclipticOfDate = 1
    ''' <summary>
    ''' For corrections to components of modeled pole unit vector referred to GCRS axes, that is, dx and dy.
    ''' </summary>
    ''' <remarks></remarks>
    ReferredToGCRSAxes = 2
End Enum

''' <summary>
''' Direction of frame conversion
''' </summary>
''' <remarks>Used by FrameTie method.</remarks>
<Guid("3AC3E32A-EDCE-4234-AA50-CDB346851C5D"),
ComVisible(True)>
Public Enum FrameConversionDirection As Short
    ''' <summary>
    ''' Dynamical to ICRS transformation.
    ''' </summary>
    ''' <remarks></remarks>
    DynamicalToICRS = -1
    ''' <summary>
    ''' ICRS to dynamical transformation.
    ''' </summary>
    ''' <remarks></remarks>
    ICRSToDynamical = 1
End Enum

''' <summary>
''' Location of observer, determining whether the gravitational deflection due to the earth itself is applied.
''' </summary>
''' <remarks>Used by GravDef method.</remarks>
<Guid("C39A798C-53F7-460b-853F-DA5389B4324D"),
ComVisible(True)>
Public Enum EarthDeflection As Short
    ''' <summary>
    ''' No earth deflection (normally means observer is at geocenter)
    ''' </summary>
    ''' <remarks></remarks>
    NoEarthDeflection = 0
    ''' <summary>
    ''' Add in earth deflection (normally means observer is on or above surface of earth, including earth orbit)
    ''' </summary>
    ''' <remarks></remarks>
    AddEarthDeflection = 1
End Enum

''' <summary>
''' Reference system in which right ascension is given
''' </summary>
''' <remarks></remarks>
<Guid("DF215CCF-2C25-48e5-A357-A8300C1EA027"),
ComVisible(True)>
Public Enum ReferenceSystem As Short
    ''' <summary>
    ''' GCRS
    ''' </summary>
    ''' <remarks></remarks>
    GCRS = 1
    ''' <summary>
    ''' True equator and equinox of date
    ''' </summary>
    ''' <remarks></remarks>
    TrueEquatorAndEquinoxOfDate = 2
End Enum

''' <summary>
''' Type of equinox
''' </summary>
''' <remarks></remarks>
<Guid("5EDEE8B3-E223-4fb7-924F-4C56E8373380"),
ComVisible(True)>
Public Enum EquinoxType As Short
    ''' <summary>
    ''' Mean equinox
    ''' </summary>
    ''' <remarks></remarks>
    MeanEquinox = 0
    ''' <summary>
    ''' True equinox
    ''' </summary>
    ''' <remarks></remarks>
    TrueEquinox = 1
End Enum

''' <summary>
''' Type of transformation
''' </summary>
''' <remarks></remarks>
<Guid("8BBA934E-D874-48a2-A3E2-C842A7FFFB35"),
ComVisible(True)>
Public Enum TransformationOption3 As Short
    ''' <summary>
    ''' Change epoch only
    ''' </summary>
    ''' <remarks></remarks>
    ChangeEpoch = 1
    ''' <summary>
    ''' Change equator and equinox; sane epoch
    ''' </summary>
    ''' <remarks></remarks>
    ChangeEquatorAndEquinox = 2
    ''' <summary>
    ''' Change equator, equinox and epoch
    ''' </summary>
    ''' <remarks></remarks>
    ChangeEquatorAndEquinoxAndEpoch = 3
    ''' <summary>
    ''' change equator and equinox J2000.0 to ICRS
    ''' </summary>
    ''' <remarks></remarks>
    ChangeEquatorAndEquinoxJ2000ToICRS = 4
    ''' <summary>
    ''' change ICRS to equator and equinox of J2000.0
    ''' </summary>
    ''' <remarks></remarks>
    ChangeICRSToEquatorAndEquinoxOfJ2000 = 5
End Enum

''' <summary>
''' Type of object
''' </summary>
''' <remarks></remarks>
<Guid("5BBB931B-358C-40ac-921C-B48373F01348"),
ComVisible(True)>
Public Enum ObjectType As Short
    ''' <summary>
    ''' Major planet, sun or moon
    ''' </summary>
    ''' <remarks></remarks>
    MajorPlanetSunOrMoon = 0
    ''' <summary>
    ''' Minor planet
    ''' </summary>
    ''' <remarks></remarks>
    MinorPlanet = 1
    ''' <summary>
    ''' Object located outside the solar system
    ''' </summary>
    ''' <remarks></remarks>
    ObjectLocatedOutsideSolarSystem = 2
End Enum

''' <summary>
''' Body or location
''' </summary>
''' <remarks>This numbering convention is used by ephemeris routines; do not confuse with the Body enum, which is used in most 
''' other places within NOVAS3.
''' <para>
''' The numbering convention for 'target' and'center' is:
''' <pre>
'''             0  =  Mercury           7 = Neptune
'''             1  =  Venus             8 = Pluto
'''             2  =  Earth             9 = Moon
'''             3  =  Mars             10 = Sun
'''             4  =  Jupiter          11 = Solar system bary.
'''             5  =  Saturn           12 = Earth-Moon bary.
'''             6  =  Uranus           13 = Nutations (long. and obliq.)</pre>
''' </para>
''' <para>
''' If nutations are desired, set 'target' = 14; 'center' will be ignored on that call.
''' </para>
'''</remarks>
<Guid("60E342F9-3CC3-4b98-8045-D61B5A7D974B"),
ComVisible(True)>
Public Enum Target As Short
    ''' <summary>
    ''' Mercury
    ''' </summary>
    ''' <remarks></remarks>
    Mercury = 0
    ''' <summary>
    ''' Venus
    ''' </summary>
    ''' <remarks></remarks>
    Venus = 1
    ''' <summary>
    ''' Earth
    ''' </summary>
    ''' <remarks></remarks>
    Earth = 2
    ''' <summary>
    ''' Mars
    ''' </summary>
    ''' <remarks></remarks>
    Mars = 3
    ''' <summary>
    ''' Jupiter
    ''' </summary>
    ''' <remarks></remarks>
    Jupiter = 4
    ''' <summary>
    ''' Saturn
    ''' </summary>
    ''' <remarks></remarks>
    Saturn = 5
    ''' <summary>
    ''' Uranus
    ''' </summary>
    ''' <remarks></remarks>
    Uranus = 6
    ''' <summary>
    ''' Neptune
    ''' </summary>
    ''' <remarks></remarks>
    Neptune = 7
    ''' <summary>
    ''' Pluto
    ''' </summary>
    ''' <remarks></remarks>
    Pluto = 8
    ''' <summary>
    ''' Moon
    ''' </summary>
    ''' <remarks></remarks>
    Moon = 9
    ''' <summary>
    ''' Sun
    ''' </summary>
    ''' <remarks></remarks>
    Sun = 10
    ''' <summary>
    ''' Solar system barycentre
    ''' </summary>
    ''' <remarks></remarks>
    SolarSystemBarycentre = 11
    ''' <summary>
    ''' Earth moon barycentre
    ''' </summary>
    ''' <remarks></remarks>
    EarthMoonBarycentre = 12
    ''' <summary>
    ''' Nutations
    ''' </summary>
    ''' <remarks></remarks>
    Nutations = 13
End Enum
#End Region

#Region "Public NOVAS 2 Structures"
''' <summary>
''' Structure to hold body type, number and name
''' </summary>
''' <remarks>Designates a celestial object.
''' </remarks>
<Guid("558F644F-E112-4e88-9D79-20063BB25C3E"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential)>
Public Structure BodyDescription
    ''' <summary>
    ''' Type of body
    ''' </summary>
    ''' <remarks>
    ''' 0 = Major planet, Sun, or Moon
    ''' 1 = Minor planet
    ''' </remarks>
    Public Type As BodyType
    ''' <summary>
    ''' body number
    ''' </summary>
    ''' <remarks><pre>
    ''' For 'type' = 0: Mercury = 1, ..., Pluto = 9, Sun = 10, Moon = 11
    ''' For 'type' = 1: minor planet number
    ''' </pre></remarks>
    Public Number As Body
    ''' <summary>
    ''' Name of the body (limited to 99 characters)
    ''' </summary>
    ''' <remarks></remarks>
    <MarshalAsAttribute(UnmanagedType.BStr, SizeConst:=100)>
    Public Name As String 'char[100]
End Structure

''' <summary>
''' Structure to hold astrometric catalogue data
''' </summary>
''' <remarks>
''' The astrometric catalog data for a star; equator and equinox and units will depend on the catalog. 
''' While this structure can be used as a generic container for catalog data, all high-level 
''' NOVAS-C functions require J2000.0 catalog data with FK5-type units (shown in square brackets below).
''' </remarks>
<Guid("6320FEDA-8582-4048-988A-7D4DE7978C71"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.[Ansi])>
Public Structure CatEntry
    ''' <summary>
    ''' 3-character catalog designator. 
    ''' </summary>
    ''' <remarks></remarks>
    <MarshalAsAttribute(UnmanagedType.BStr, SizeConst:=4)>
    Public Catalog As String 'char[4] was <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=4)> Was this before changing for COM compatibility

    ''' <summary>
    ''' Name of star.
    ''' </summary>
    ''' <remarks></remarks>
    <MarshalAsAttribute(UnmanagedType.BStr, SizeConst:=51)>
    Public StarName As String 'char[51] was <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=51)> _

    ''' <summary>
    ''' Integer identifier assigned to star.
    ''' </summary>
    ''' <remarks></remarks>
    Public StarNumber As Integer

    ''' <summary>
    ''' Mean right ascension [hours].
    ''' </summary>
    ''' <remarks></remarks>
    Public RA As Double

    ''' <summary>
    ''' Mean declination [degrees].
    ''' </summary>
    ''' <remarks></remarks>
    Public Dec As Double

    ''' <summary>
    ''' Proper motion in RA [seconds of time per century].
    ''' </summary>
    ''' <remarks></remarks>
    Public ProMoRA As Double

    ''' <summary>
    ''' Proper motion in declination [arcseconds per century].
    ''' </summary>
    ''' <remarks></remarks>
    Public ProMoDec As Double

    ''' <summary>
    ''' Parallax [arcseconds].
    ''' </summary>
    ''' <remarks></remarks>
    Public Parallax As Double

    ''' <summary>
    ''' Radial velocity [kilometers per second]
    ''' </summary>
    ''' <remarks></remarks>
    Public RadialVelocity As Double
End Structure

''' <summary>
''' Structure to hold site information
''' </summary>
''' <remarks>
''' Data for the observer's location.  The atmospheric parameters are used only by the refraction 
''' function called from function 'equ_to_hor'. Additional parameters can be added to this 
''' structure if a more sophisticated refraction model is employed.
''' </remarks>
<Guid("ED02B64A-320F-47cd-90D9-3DF2DF07602D"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.[Ansi])>
Public Structure SiteInfo
    ''' <summary>
    ''' Geodetic latitude in degrees; north positive.
    ''' </summary>
    ''' <remarks></remarks>
    Public Latitude As Double 'geodetic latitude in degrees; north positive.
    ''' <summary>
    ''' Geodetic longitude in degrees; east positive.
    ''' </summary>
    ''' <remarks></remarks>
    Public Longitude As Double 'geodetic longitude in degrees; east positive.
    ''' <summary>
    ''' Height of the observer in meters.
    ''' </summary>
    ''' <remarks></remarks>
    Public Height As Double 'height of the observer in meters.
    ''' <summary>
    ''' Temperature (degrees Celsius).
    ''' </summary>
    ''' <remarks></remarks>
    Public Temperature As Double 'temperature (degrees Celsius).
    ''' <summary>
    ''' Atmospheric pressure (millibars)
    ''' </summary>
    ''' <remarks></remarks>
    Public Pressure As Double 'atmospheric pressure (millibars)
End Structure

''' <summary>
''' Structure to hold a position vector
''' </summary>
''' <remarks>Object position vector
''' </remarks>
<Guid("69651C90-75F5-4f46-8D0F-22D186151D45"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.[Ansi])>
Public Structure PosVector
    ''' <summary>
    ''' x co-ordinate
    ''' </summary>
    ''' <remarks></remarks>
    Public x As Double
    ''' <summary>
    ''' y co-ordinate
    ''' </summary>
    ''' <remarks></remarks>
    Public y As Double
    ''' <summary>
    ''' z co-ordinate
    ''' </summary>
    ''' <remarks></remarks>
    Public z As Double
End Structure

''' <summary>
''' Structure to hold a velocity vector
''' </summary>
''' <remarks>Object velocity vector
''' </remarks>
<Guid("F18240B0-00CC-4ff7-9A94-AC835387F959"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.[Ansi])>
Public Structure VelVector
    ''' <summary>
    ''' x velocity component 
    ''' </summary>
    ''' <remarks></remarks>
    Public x As Double
    ''' <summary>
    ''' y velocity component
    ''' </summary>
    ''' <remarks></remarks>
    Public y As Double
    ''' <summary>
    ''' z velocity component
    ''' </summary>
    ''' <remarks></remarks>
    Public z As Double
End Structure

''' <summary>
''' Structure to hold Sun and Moon fundamental arguments
''' </summary>
''' <remarks>Fundamental arguments, in radians
'''</remarks>
<Guid("5EE28FFB-39CD-4d23-BF62-11EE4C581681"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.[Ansi])>
Public Structure FundamentalArgs
    ''' <summary>
    ''' l (mean anomaly of the Moon)
    ''' </summary>
    ''' <remarks></remarks>
    Public l As Double
    ''' <summary>
    ''' l' (mean anomaly of the Sun)
    ''' </summary>
    ''' <remarks></remarks>
    Public ldash As Double
    ''' <summary>
    ''' F (L - omega; L = mean longitude of the Moon)
    ''' </summary>
    ''' <remarks></remarks>
    Public F As Double
    ''' <summary>
    ''' D (mean elongation of the Moon from the Sun)
    ''' </summary>
    ''' <remarks></remarks>
    Public D As Double
    ''' <summary>
    ''' Omega (mean longitude of the Moon's ascending node)
    ''' </summary>
    ''' <remarks></remarks>
    Public Omega As Double
End Structure
#End Region

#Region "Public NOVAS 3 Structures"

''' <summary>
''' Catalogue entry structure
''' </summary>
''' <remarks>Basic astrometric data for any celestial object located outside the solar system; the catalog data for a star.
''' <para>This structure is identical to the NOVAS2 CatEntry structure expect that, for some reason, the StarName and Catalog fields
''' have been swapped in the NOVAS3 structure.</para>
''' <para>
''' Please note that some units have changed from those used in NOVAS2 as follows:
''' <list type="bullet">
''' <item>proper motion in right ascension: from seconds per century to milliarcseconds per year</item>
''' <item>proper motion in declination: from arcseconds per century to milliarcseconds per year</item>
''' <item>parallax: from arcseconds to milliarcseconds</item>
''' </list>
''' </para>
''' </remarks>
<Guid("5325E96C-BD24-4470-A0F6-E917B05805E1"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.[Ansi])>
Public Structure CatEntry3
    ''' <summary>
    ''' Name of celestial object. (maximum 50 characters)
    ''' </summary>
    ''' <remarks></remarks>
    <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=51)>
    Public StarName As String

    ''' <summary>
    ''' 3-character catalog designator. 
    ''' </summary>
    ''' <remarks></remarks>
    <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=4)>
    Public Catalog As String

    ''' <summary>
    ''' Integer identifier assigned to object.
    ''' </summary>
    ''' <remarks></remarks>
    Public StarNumber As Integer

    ''' <summary>
    ''' ICRS right ascension (hours)
    ''' </summary>
    ''' <remarks></remarks>
    Public RA As Double

    ''' <summary>
    ''' ICRS declination (degrees)
    ''' </summary>
    ''' <remarks></remarks>
    Public Dec As Double

    ''' <summary>
    ''' ICRS proper motion in right ascension (milliarcseconds/year)
    ''' </summary>
    ''' <remarks></remarks>
    Public ProMoRA As Double

    ''' <summary>
    ''' ICRS proper motion in declination (milliarcseconds/year)
    ''' </summary>
    ''' <remarks></remarks>
    Public ProMoDec As Double

    ''' <summary>
    ''' Parallax (milli-arcseconds)
    ''' </summary>
    ''' <remarks></remarks>
    Public Parallax As Double

    ''' <summary>
    ''' Radial velocity (km/s)
    ''' </summary>
    ''' <remarks></remarks>
    Public RadialVelocity As Double
End Structure

''' <summary>
''' Celestial object structure
''' </summary>
''' <remarks>Designates a celestial object</remarks>
<Guid("AEFE0EA0-D013-46a9-B77D-6D0FDD661005"),
ComVisible(True)>
Public Structure Object3
    ''' <summary>
    ''' Type of object
    ''' </summary>
    ''' <remarks></remarks>
    Public Type As ObjectType
    ''' <summary>
    ''' Object identification number
    ''' </summary>
    ''' <remarks></remarks>
    Public Number As Body
    ''' <summary>
    ''' Name of object(maximum 50 characters)
    ''' </summary>
    ''' <remarks></remarks>
    Public Name As String
    ''' <summary>
    ''' Catalogue entry for the object
    ''' </summary>
    ''' <remarks></remarks>
    Public Star As CatEntry3
End Structure

''' <summary>
''' Celestial object's place in the sky
''' </summary>
''' <remarks></remarks>
<Guid("9AD852C3-A895-4f69-AEC0-C9CA44283FA0"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
Public Structure SkyPos
    ''' <summary>
    ''' Unit vector toward object (dimensionless)
    ''' </summary>
    ''' <remarks></remarks>
    <MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst:=3, ArraySubType:=UnmanagedType.R8)>
    Public RHat() As Double
    ''' <summary>
    ''' Apparent, topocentric, or astrometric right ascension (hours)
    ''' </summary>
    ''' <remarks></remarks>
    Public RA As Double
    ''' <summary>
    ''' Apparent, topocentric, or astrometric declination (degrees)
    ''' </summary>
    ''' <remarks></remarks>
    Public Dec As Double
    ''' <summary>
    ''' True (geometric, Euclidian) distance to solar system body or 0.0 for star (AU)
    ''' </summary>
    ''' <remarks></remarks>
    Public Dis As Double
    ''' <summary>
    ''' Radial velocity (km/s)
    ''' </summary>
    ''' <remarks></remarks>
    Public RV As Double
End Structure

''' <summary>
''' Observer’s position and velocity in a near-Earth spacecraft.
''' </summary>
''' <remarks></remarks>
<Guid("15737EA5-E4FA-40da-8BDA-B8CF96D89E43"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential)>
Public Structure InSpace
    ''' <summary>
    ''' Geocentric position vector (x, y, z), components in km with respect to true equator and equinox of date
    ''' </summary>
    ''' <remarks></remarks>
    <MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst:=3, ArraySubType:=UnmanagedType.R8)>
    Public ScPos() As Double
    ''' <summary>
    ''' Geocentric velocity vector (x_dot, y_dot, z_dot), components in km/s with respect to true equator and equinox of date
    ''' </summary>
    ''' <remarks></remarks>
    <MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst:=3, ArraySubType:=UnmanagedType.R8)>
    Public ScVel() As Double
End Structure

''' <summary>
''' Right ascension of the Celestial Intermediate Origin (CIO) with respect to the GCRS.
''' </summary>
''' <remarks></remarks>
<Guid("4959930F-0CDB-4324-A0E0-F60A351454B7"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential)>
Public Structure RAOfCio
    ''' <summary>
    ''' TDB Julian date
    ''' </summary>
    ''' <remarks></remarks>
    Public JdTdb As Double
    ''' <summary>
    ''' Right ascension of the CIO with respect to the GCRS (arcseconds)
    ''' </summary>
    ''' <remarks></remarks>
    Public RACio As Double
End Structure

''' <summary>
''' Parameters of observer's location
''' </summary>
''' <remarks>This structure is identical to the NOVAS2 SiteInfo structure but is included so that NOVAS3 naming
''' conventions are maintained, making it easier to relate this code to the NOVAS3 documentation and C code.</remarks>
<Guid("277380A5-6599-448f-9232-1C280073D3CD"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential)>
Public Structure OnSurface
    ''' <summary>
    ''' Geodetic (ITRS) latitude; north positive (degrees)
    ''' </summary>
    ''' <remarks></remarks>
    Public Latitude As Double
    ''' <summary>
    ''' Geodetic (ITRS) longitude; east positive (degrees)
    ''' </summary>
    ''' <remarks></remarks>
    Public Longitude As Double
    ''' <summary>
    ''' Observer's height above sea level
    ''' </summary>
    ''' <remarks></remarks>
    Public Height As Double
    ''' <summary>
    ''' Observer's location's ambient temperature (degrees Celsius)
    ''' </summary>
    ''' <remarks></remarks>
    Public Temperature As Double
    ''' <summary>
    ''' Observer's location's atmospheric pressure (millibars)
    ''' </summary>
    ''' <remarks></remarks>
    Public Pressure As Double
End Structure

''' <summary>
''' General specification for the observer's location
''' </summary>
''' <remarks></remarks>
<Guid("64A25FDD-3687-45e0-BEAF-18C361E5E340"),
ComVisible(True),
StructLayoutAttribute(LayoutKind.Sequential)>
Public Structure Observer
    ''' <summary>
    ''' Code specifying the location of the observer: 0=at geocenter; 1=surface of earth; 2=near-earth spacecraft
    ''' </summary>
    ''' <remarks></remarks>
    Public Where As ObserverLocation
    ''' <summary>
    ''' Data for an observer's location on the surface of the Earth (where = 1)
    ''' </summary>
    ''' <remarks></remarks>
    Public OnSurf As OnSurface
    ''' <summary>
    ''' Data for an observer's location on a near-Earth spacecraft (where = 2)
    ''' </summary>
    ''' <remarks></remarks>
    Public NearEarth As InSpace
End Structure
#End Region

#Region "Internal NOVAS3 Structures"
<StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
Friend Structure JDHighPrecision
    Public JDPart1 As Double
    Public JDPart2 As Double
End Structure

'Internal version of Object3 with correct marshalling hints and type for Number field
<StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
Friend Structure Object3Internal
    Public Type As ObjectType
    Public Number As Short
    <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=51)>
    Public Name As String
    Public Star As CatEntry3
End Structure

<StructLayoutAttribute(LayoutKind.Sequential)>
Friend Structure RAOfCioArray

    Friend Value1 As RAOfCio
    Friend Value2 As RAOfCio
    Friend Value3 As RAOfCio
    Friend Value4 As RAOfCio
    Friend Value5 As RAOfCio
    Friend Value6 As RAOfCio
    Friend Value7 As RAOfCio
    Friend Value8 As RAOfCio
    Friend Value9 As RAOfCio
    Friend Value10 As RAOfCio
    Friend Value11 As RAOfCio
    Friend Value12 As RAOfCio
    Friend Value13 As RAOfCio
    Friend Value14 As RAOfCio
    Friend Value15 As RAOfCio
    Friend Value16 As RAOfCio
    Friend Value17 As RAOfCio
    Friend Value18 As RAOfCio
    Friend Value19 As RAOfCio
    Friend Value20 As RAOfCio

    Friend Sub Initialise()
        Value1.RACio = RACIO_DEFAULT_VALUE
        Value2.RACio = RACIO_DEFAULT_VALUE
        Value3.RACio = RACIO_DEFAULT_VALUE
        Value4.RACio = RACIO_DEFAULT_VALUE
        Value5.RACio = RACIO_DEFAULT_VALUE
        Value6.RACio = RACIO_DEFAULT_VALUE
        Value7.RACio = RACIO_DEFAULT_VALUE
        Value8.RACio = RACIO_DEFAULT_VALUE
        Value9.RACio = RACIO_DEFAULT_VALUE
        Value10.RACio = RACIO_DEFAULT_VALUE
        Value11.RACio = RACIO_DEFAULT_VALUE
        Value12.RACio = RACIO_DEFAULT_VALUE
        Value13.RACio = RACIO_DEFAULT_VALUE
        Value14.RACio = RACIO_DEFAULT_VALUE
        Value15.RACio = RACIO_DEFAULT_VALUE
        Value16.RACio = RACIO_DEFAULT_VALUE
        Value17.RACio = RACIO_DEFAULT_VALUE
        Value18.RACio = RACIO_DEFAULT_VALUE
        Value19.RACio = RACIO_DEFAULT_VALUE
        Value20.RACio = RACIO_DEFAULT_VALUE
    End Sub
End Structure
#End Region
