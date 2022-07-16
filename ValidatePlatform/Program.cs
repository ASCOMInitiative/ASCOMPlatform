using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ASCOM.Astrometry.NOVAS;
using static System.Collections.Specialized.BitVector32;

namespace ValidatePlatform
{
    /// <summary>
    /// Validate that the basic TraceLogger, Utilities, SOFA and NOVAS31 components are working OK
    /// </summary>
    internal class Program
    {
        static int returnCode = 0;
        static TraceLogger TL;
        static Util util;
        static string errorLog = "";

        static int Main(string[] args)
        {

            // Basic tests for NOVAS and SOFA to ensure that they work OK
            try
            {
                // Create a TraceLogger component
                try
                {
                    string osMode = Environment.Is64BitProcess ? "64" : "86";

                    TL = new TraceLogger("", $"ValidatePlatform{osMode}")
                    {
                        Enabled = true
                    };
                    LogMessage("Main", $"Successfully created TraceLogger.");
                    LogMessage("Main", $"Operating in X{osMode} mode.");
                    LogBlankLine();
                }
                catch (Exception ex)
                {
                    LogError("Main", $"Unable to create trace logger.",ex);
                }

                // Create a Utilities component
                try
                {
                    util = new Util();
                    LogMessage("Main", "Successfully created Utilities component");
                }
                catch (Exception ex)
                {
                    LogError("Main", $"Unable to create Utilities component.",ex);
                }
                LogBlankLine();

                // Test the SOFA COM object
                try
                {
                    Type sofaType = Type.GetTypeFromProgID("ASCOM.Astrometry.SOFA.SOFA");
                    dynamic sofa = Activator.CreateInstance(sofaType);
                    LogMessage("Main", "Successfully created SOFA COM object");

                    double tt1 = 2459773.0;
                    double tt2 = 0.99093;
                    double tai1 = 0.0;
                    double tai2 = 0.0;
                    int rc = sofa.TtTai(tt1, tt2, ref tai1, ref tai2);
                    double difference = (tt1 + tt2 - tai1 - tai2) * 24.0 * 60.0 * 60.0;
                    LogMessage("Main", $"TtTai called successfully. Input terrestrial time: {tt1 + tt2}, output atomic time: {tai1 + tai2}. Difference: {difference} seconds.");

                    if (Math.Abs(difference - 32.184) < 0.01)
                    {
                        LogMessage("Main", $"Received expected result from TtTai.");
                    }
                    else
                    {
                        LogError("Main", $"Received bad result from TtTai.",null);
                    }
                }
                catch (Exception ex)
                {
                    LogError("Main", $"Unable to create SOFA COM component.",ex);
                }
                LogBlankLine();

                // Test the SOFA .NET component.
                try
                {
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
                    LogError("Main", $"Unable to create SOFA .NET component.",ex);
                }
                LogBlankLine();

                // Test the NOVAS 3.1 component
                try
                {
                    const double EXPECTED_JULIAN_DATE = 2459774.0;

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
                }
                catch (Exception ex)
                {
                    LogError("Main", $"Unable to create NOVAS .NET component.",ex);
                }

                // DIsplay error log if necessary, otherwise continue silently.
                if (errorLog != "")
                {
                    errorLog = $"The issues below occurred while validating operation of the Platform. Please zip up all the files and sub-folders in your Documents\\ASCOM folder and post a message together with the zip on the ASCOM Talk Groups.Io forum.\r\n\r\n{errorLog}";
                    MessageBox.Show(errorLog, "Issues Validating Platform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetReturnCode(1);
                }

            }
            catch (Exception ex)
            {
                LogError("Main", $"Exception:",ex);
            }

            return returnCode;
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
                Console.WriteLine(logMessage);
            }
            catch { }
            try
            {
                TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
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
                errorLog += $"{message}\r\n{ex.Message}\r\n";
            }
        }

        public static void LogBlankLine()
        {
            LogMessage("", "");
        }

    }
}
