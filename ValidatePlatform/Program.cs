using ASCOM.Astrometry;
using ASCOM.Astrometry.NOVAS;
using ASCOM.Astrometry.Transform;
using ASCOM.Utilities;
using Microsoft.Win32;
using System;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Windows.Forms;

namespace ValidatePlatform
{
    /// <summary>
    /// Validate that the basic TraceLogger, Utilities, SOFA and NOVAS31 components are working OK
    /// </summary>
    internal class Program
    {
        const string SOFA_CLSID = @"{DF65E97B-ED0E-4F48-BBC9-4A8854C0EF6E}";
        const string ASTROMETRY_CLSID = @"{7F3582E3-9AA8-42CA-845C-2E6B13F362C1}";

        static int returnCode = 0;
        static TraceLogger TL;
        static Util util;
        static string errorLog = "";

        static int Main(string[] args)
        {
            // Set up assembly load and resolve event handlers
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                {
                    LogError("UnhandledException", "Unhandled exception occurred.", e.ExceptionObject as Exception);
                    SetReturnCode(2);
                };
            }
            catch (Exception ex)
            {
                LogError("Main", "Issue creating event handlers.", ex);
            }

            // Create a TraceLogger component
            try
            {
                string osMode = Environment.Is64BitProcess ? "64" : "86";

                TL = new TraceLogger("", $"ValidatePlatform{osMode}")
                {
                    Enabled = true
                };
                LogMessage("Main", $"Operating in X{osMode} mode on an {RuntimeInformation.OSArchitecture} OS using an {RuntimeInformation.ProcessArchitecture} processor.");
            }
            catch (Exception ex)
            {
                LogError("Main", $"Unable to create trace logger.", ex);
            }
            LogBlankLine();

            // Create a Utilities component
            try
            {
                util = new Util();
                LogMessage("Main", $"Successfully created Utilities component.");
            }
            catch (Exception ex)
            {
                LogError("Main", $"Unable to create Utilities component.", ex);
            }
            LogBlankLine();

            // Report on the SOFA ProgID COM registration
            try
            {
                ValidateSofaSubKey(@"ASCOM.Astrometry.SOFA.SOFA");
                ValidateSofaSubKey(@"ASCOM.Astrometry.SOFA.SOFA\CLSID");
                ValidateSofaValue(@"ASCOM.Astrometry.SOFA.SOFA\CLSID", "", SOFA_CLSID);
            }
            catch (Exception ex)
            {
                LogError("SOFA", $"SOFA COM registration exception", ex);
            }
            LogBlankLine();

            // Report on the SOFA CLSID COM registration
            try
            {
                ValidateSofaSubKey($@"CLSID\{SOFA_CLSID}");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}", "", "ASCOM.Astrometry.SOFA.SOFA");
                LogBlankLine();

                // Validate that expected sub-keys exist with the correct values
                ValidateSofaSubKey($@"CLSID\{SOFA_CLSID}\Implemented Categories");
                ValidateSofaSubKey($@"CLSID\{SOFA_CLSID}\Implemented Categories\{{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}}");
                LogBlankLine();

                ValidateSofaSubKey($@"CLSID\{SOFA_CLSID}\InprocServer32");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\InprocServer32", "", "mscoree.dll");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\InprocServer32", "Assembly", "ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\InprocServer32", "Class", "ASCOM.Astrometry.SOFA.SOFA");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\InprocServer32", "RuntimeVersion", "v2.0.50727");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\InprocServer32", "ThreadingModel", "Both");
                LogBlankLine();

                ValidateSofaSubKey($@"CLSID\{SOFA_CLSID}\InprocServer32\6.0.0.0");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\InprocServer32\6.0.0.0", "Assembly", "ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\InprocServer32\6.0.0.0", "Class", "ASCOM.Astrometry.SOFA.SOFA");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\InprocServer32\6.0.0.0", "RuntimeVersion", "v2.0.50727");
                LogBlankLine();

                ValidateSofaSubKey($@"CLSID\{SOFA_CLSID}\ProgId");
                ValidateSofaValue($@"CLSID\{SOFA_CLSID}\ProgId", "", "ASCOM.Astrometry.SOFA.SOFA");
                LogBlankLine();

                ValidateSofaSubKey($@"TypeLib\{ASTROMETRY_CLSID}");
                LogBlankLine();

                ValidateSofaSubKey($@"TypeLib\{ASTROMETRY_CLSID}\6.0");
                ValidateSofaValue($@"TypeLib\{ASTROMETRY_CLSID}\6.0", "", "ASCOM Astrometry");
                LogBlankLine();

                ValidateSofaSubKey($@"TypeLib\{ASTROMETRY_CLSID}\6.0\0");
                LogBlankLine();

                ValidateSofaSubKey($@"TypeLib\{ASTROMETRY_CLSID}\6.0\0\win32");
                ValidateSofaValue($@"TypeLib\{ASTROMETRY_CLSID}\6.0\0\win32", "", @"ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.tlb");
                LogBlankLine();

                ValidateSofaSubKey($@"TypeLib\{ASTROMETRY_CLSID}\6.0\FLAGS");
                ValidateSofaValue($@"TypeLib\{ASTROMETRY_CLSID}\6.0\FLAGS", "", @"0");
            }
            catch (Exception ex)
            {
                LogError("SOFA", $"CLSID COM registration exception", ex);
            }
            LogBlankLine();

            // Basic tests for NOVAS and SOFA to ensure that they work OK
            try
            {
                // Test the SOFA .NET component.
                try
                {
                    LogMessage("Main", "About to create SOFA .NET component...");
                    ASCOM.Astrometry.SOFA.SOFA sofaComponent = new ASCOM.Astrometry.SOFA.SOFA();
                    LogMessage("Main", "Successfully created SOFA .NET component.");

                    const double DEGREES_TO_RADIANS = Math.PI / 180.0; ;
                    const double HOURS_TO_RADIANS = Math.PI / 12.0; ;
                    const double RADIANS_TO_DEGREES = 180.0 / Math.PI;
                    const double RADIANS_TO_HOURS = 12.0 / Math.PI;

                    double TARGET_RA = util.HMSToHours("05:16:41.3591");  // Capella: 05h 16m 41.3591s
                    double TARGET_DEC = util.DMSToDegrees("+45:59:52.768"); // Capella: +45° 59' 52.768" 
                    const double JULIAN_UTC_DATE = 2459774.0; // 12:00 UTC 13th July 2022
                    const double SITE_LONGITUDE = 51.0; // 51 degrees north
                    const double SITE_LATITUDE = 0.0; // 0 degrees west
                    const double SITE_ELEVATION = 0.0; // 80m height

                    const double SITE_TEMPERATURE = 10.0; // Celsius
                    const double SITE_PRESSURE = 0.0; // Millibar
                    const double SITE_RH = 50; // Percent

                    const double OBSERVING_WAVELENGTH = 0.51; //Microns

                    double aob = 0.0, zob = 0.0, hob = 0.0, dob = 0.0, rob = 0.0, eo = 0.0;

                    // Call the Sofa component CelestialToObserved method.
                    int sofaRc = sofaComponent.CelestialToObserved(
                        TARGET_RA * HOURS_TO_RADIANS,
                        TARGET_DEC * DEGREES_TO_RADIANS,
                        0.0,
                        0.0,
                        0.0,
                        0.0,
                        JULIAN_UTC_DATE,
                        0.0,
                        0.0,
                        SITE_LONGITUDE * DEGREES_TO_RADIANS,
                        SITE_LATITUDE * DEGREES_TO_RADIANS,
                        SITE_ELEVATION,
                        0.0,
                        0.0,
                        SITE_PRESSURE,
                        SITE_TEMPERATURE,
                        SITE_RH / 100.0,
                        OBSERVING_WAVELENGTH,
                        ref aob,
                        ref zob,
                        ref hob,
                        ref dob,
                        ref rob,
                        ref eo
                        );

                    LogMessage("Main", $"RA: {util.HoursToHMS((rob - eo) * RADIANS_TO_HOURS, ":", ":", "", 3)}, Dec: {util.DegreesToDMS(dob * RADIANS_TO_DEGREES, ":", ":", "", 3)}");
                    double difference = Math.Abs(((rob - eo) * RADIANS_TO_HOURS) - 5.30514362725635);
                    if (difference < 0.01)
                    {
                        LogMessage("Main", $"Received expected result from CelestialToObserved - RA.");
                    }
                    else
                    {
                        LogError("Main", $"Received bad result from CelestialToObserved - RA.", null);
                    }

                    if (Math.Abs((dob * RADIANS_TO_DEGREES) - 46.0209960277937) < 0.01)
                    {
                        LogMessage("Main", $"Received expected result from CelestialToObserved - DEC.");
                    }
                    else
                    {
                        LogError("Main", $"Received bad result from CelestialToObserved - DEC.", null);
                    }
                }
                catch (Exception ex)
                {
                    LogError("Main", $"Unable to create SOFA .NET component.", ex);
                }
                LogBlankLine();

                // Test the NOVAS 3.1 component
                try
                {
                    const double EXPECTED_JULIAN_DATE = 2459774.0;

                    LogMessage("Main", "About to create NOVAS31 component...");
                    NOVAS31 novas31 = new NOVAS31();
                    LogMessage("Main", "Successfully created NOVAS31 component.");

                    double jd = novas31.JulianDate(2022, 7, 13, 12.0); // 2459774.0
                    double difference = Math.Abs(jd - EXPECTED_JULIAN_DATE);
                    LogMessage("Main", $"Returned NOVAS Julian date: {jd}, expected: {EXPECTED_JULIAN_DATE}. Difference: {difference}");
                    if (difference < 0.01)
                    {
                        LogMessage("Main", $"Received expected Julian date from NOVAS31.JulianDate.");
                    }
                    else
                    {
                        LogError("Main", $"Received bad Julian date {jd} from NOVAS31.JulianDate. Expected: {EXPECTED_JULIAN_DATE}", null);
                    }
                    LogBlankLine();

                    const double WALLACE_EXPECTED_DEC = 52.29549062657d;

                    // Site (ITRS)
                    double siteLonE_Deg = 9.712156d;      // East longitude, degrees
                    double siteLatN_Deg = 52.385639d;     // North latitude, degrees
                    double siteHeight_m = 200.0d;         // meters

                    // Star (ICRS, epoch 2000)
                    double ra2000_deg = 353.22987757d;
                    double dec2000_deg = 52.27730247d;
                    double pmRAstar_masPerYr = 22.9d;     // μα cosδ  (mas/yr)  <-- IMPORTANT: use as given
                    double pmDec_masPerYr = -2.1d;        // μδ       (mas/yr)
                    double plx_mas = 23.0d;               // parallax (mas)
                    double rv_kmps = 25.0d;               // radial velocity (km/s)

                    // UTC date/time
                    short y = 2003;
                    short m = 8;
                    short d = 26;
                    double hh = 0;
                    double mm = 37;
                    double ss = 38.97381d;

                    // IERS values (Wallace)
                    double dut1 = -0.349535d;            // UT1-UTC (seconds)
                    double dX_mas = 0.038d;              // celestial pole offsets (mas)
                    double dY_mas = -0.118d;             // celestial pole offsets (mas)

                    // For this epoch (2003-08-26): TAI-UTC = 32s, so TT-UTC = 32 + 32.184 = 64.184s
                    double ttMinusUtc_s = 64.184d;

                    double raTopo_h = 0.0d;
                    double decTopo_deg = 0.0d;

                    double jdUtc = novas31.JulianDate(y, m, d, hh + mm / 60.0 + ss / 3600.0);
                    double jdUt1 = jdUtc + dut1 / 86400.0d;
                    double jdTt = jdUtc + ttMinusUtc_s / 86400.0d;
                    double deltaT = ttMinusUtc_s - dut1;   // TT-UT1 seconds

                    // Observer on Earth (OnSurface)
                    var obs = new OnSurface();
                    obs.Latitude = siteLatN_Deg;
                    obs.Longitude = siteLonE_Deg;
                    obs.Height = siteHeight_m;
                    obs.Temperature = 10.0d;      // Celsius
                    obs.Pressure = 1010.0d;       // mbar (hPa)

                    // Star catalog entry (CatEntry3)
                    var star = new CatEntry3();
                    star.RA = ra2000_deg / 15.0d;              // HOURS (not degrees!)
                    star.Dec = dec2000_deg;                   // degrees
                    star.ProMoRA = pmRAstar_masPerYr;         // mas/year (this is μα cosδ as given)
                    star.ProMoDec = pmDec_masPerYr;           // mas/year
                    star.Parallax = plx_mas;                  // mas
                    star.RadialVelocity = rv_kmps;            // km/s

                    novas31.CelPole(jdTt, PoleOffsetCorrection.ReferredToGCRSAxes, dX_mas, dY_mas);

                    short rc = novas31.TopoStar(jdTt, deltaT, star, obs, Accuracy.Full, ref raTopo_h, ref decTopo_deg);

                    double decErrorArcSec = Math.Abs(decTopo_deg - 52.29549062657d) * 3600d;

                    Util util = new Util();
                    LogMessage("Main", $"Wallace: {util?.DegreesToDMS(WALLACE_EXPECTED_DEC, ":", ":", "", 2)}");
                    double differenceFromWallace_arcSec = Math.Abs(decTopo_deg - WALLACE_EXPECTED_DEC) * 3600d;
                    LogMessage("Main", $"NOVAS:   {util?.DegreesToDMS(decTopo_deg, ":", ":", "", 2)}, Difference from Wallace: {differenceFromWallace_arcSec:0.00} arcsec");

                    Transform transform = new Transform();
                    transform.SitePressure = 1010.0;
                    transform.SiteTemperature = 10.0;
                    transform.SiteElevation = siteHeight_m;
                    transform.SiteLatitude = siteLatN_Deg;
                    transform.SiteLongitude = siteLonE_Deg;
                    transform.JulianDateTT = jdTt;
                    transform.Refraction = false;
                    transform.SetJ2000(ra2000_deg * 24.0 / 360.0, dec2000_deg);
                    LogMessage("Main", $"SOFA:    {util?.DegreesToDMS(transform.DECTopocentric, ":", ":", "", 2)}, Difference from Wallace: {Math.Abs(transform.DECTopocentric - WALLACE_EXPECTED_DEC) * 3600d:0.00} arcsec");
                }
                catch (Exception ex)
                {
                    LogError("Main", $"Unable to create NOVAS .NET component.", ex);
                }

                LogBlankLine();

                // Display error log if necessary, otherwise continue silently.
                if (errorLog != "")
                {
                    errorLog = $"The issues below occurred while validating operation of the Platform. Please zip up all the files and sub-folders in your Documents\\ASCOM folder and post a message together with the zip on the ASCOM Talk Groups.Io forum.\r\n\r\n{errorLog}";
                    MessageBox.Show(errorLog, "Issues Validating Platform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetReturnCode(1);
                }

            }
            catch (Exception ex)
            {
                LogError("Main", $"Exception:", ex);
            }

            LogMessage("Main", $"Exit code: {returnCode}");

            return returnCode;
        }

        private static void ValidateSofaSubKey(string keyName)
        {
            try
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keyName, false))
                {
                    if (!(key is null)) // Key exists
                    {
                        LogMessage("ValidateSofaSubKey", $@"OK    The {keyName} sub-key exists.");
                    }
                    else
                    {
                        LogError("ValidateSofaSubKey", $@"Error The {keyName} sub-key does not exist.", null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ValidateSofaSubKey", $"Exception while validating sub-key: {keyName}", ex);
            }
        }

        /// <summary>
        /// Validates that the value of a specified registry key contains an expected value.
        /// </summary>
        /// <remarks>This method checks if the specified registry key and value exist, and if the actual
        /// value matches the expected value.  If the key or value does not exist, or if the actual value does not match
        /// the expected value, an error is logged. If the validation succeeds, a success message is logged.</remarks>
        /// <param name="keyName">The name of the registry key to validate. This must be a valid key under <see
        /// cref="Microsoft.Win32.Registry.ClassesRoot"/>.</param>
        /// <param name="valueName">The name of the value within the registry key to validate. This must match an existing value name in the
        /// key.</param>
        /// <param name="expectedValue">The expected value to compare against the actual value of the registry key. The comparison is
        /// case-insensitive and checks if the actual value contains the expected value.</param>
        private static void ValidateSofaValue(string keyName, string valueName, string expectedValue)
        {
            try
            {
                // Open the key
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keyName, false))
                {
                    // Check the key exists
                    if (!(key is null)) // Key exists
                    {
                        // Get all the value names in this key
                        string[] subKeyNames = key.GetValueNames();

                        // Check the specified value exists
                        if (Array.Exists(subKeyNames, element => element.Equals(valueName, StringComparison.OrdinalIgnoreCase)))
                        {
                            //LogMessage("ValidateSofaValue", $"The {keyName} key '{valueName}' value exists.");

                            // Get the value 

                            //Check the value exists
                            if (key?.GetValue(valueName) is string actualValue) // Value exists
                            {
                                //LogMessage("ValidateSofaValue", $"The {keyName} key '{valueName}' value is: ");
                                // Confirm that the value is correct
                                if (actualValue.ToUpper().Contains(expectedValue.ToUpper())) // Actual value contains the expected value
                                {
                                    LogMessage("ValidateSofaValue", $"OK    The {keyName} key '{valueName}' value ('{actualValue}') is correct! ({expectedValue})");
                                }
                                else // Actual value is different to expected value
                                {
                                    LogError("ValidateSofaValue", $"ERROR The {keyName} key '{valueName}' value ('{actualValue}') is INCORRECT.", null);
                                }
                            }
                            else
                            {
                                LogError("ValidateSofaValue", $"ERROR Unable to read {keyName} key '{valueName}' value.", null);
                            }
                        }
                        else // Specified name does not exist
                        {
                            LogError("ValidateSofaValue", $"ERROR The {keyName} key '{valueName}' value does not exist.", null);
                        }
                    }
                    else // Specified key does not exist
                    {
                        LogError("ValidateSofaValue", $"ERROR The registry key {keyName} does not exist, cannot validate {valueName} value.", null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ValidateSofaValue", $"Exception while validating key: {keyName}, value name: {valueName}", ex);
            }
        }

        //
        // Set the return code to the first error that occurs
        //
        static void SetReturnCode(int rc)
        {
            if (returnCode == 0) returnCode = rc;
        }

        //log messages and send to screen when appropriate
        public static void LogMessage(string section, string logMessage)
        {
            // Make sure none of these failing stops the overall migration process
            try
            {
                Console.WriteLine($"{section} - {logMessage}");
            }
            catch { }
            try
            {
                TL?.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            }
            catch { }
        }

        public static void LogError(string section, string message, Exception ex)
        {
            if (ex is null) // No exception so just print the message
            {
                LogMessage(section, $"{message}");
                errorLog += $"{message}\r\n";
            }
            else // Exception thrown so include the full text along with the message.
            {
                LogMessage(section, $"{message}\r\n{ex}");
                errorLog += $"{message} - {ex.Message}\r\n";
            }
        }

        public static void LogBlankLine()
        {
            LogMessage("", "");
        }

    }
}
