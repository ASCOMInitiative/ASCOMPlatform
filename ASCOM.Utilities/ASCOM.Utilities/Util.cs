using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{

    /// <summary>
    /// Provides a set of utility functions for ASCOM clients and drivers
    /// </summary>
    /// <remarks></remarks>
    [Guid("E861C6D8-B55B-494a-BC59-0F27F981CA98")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Util : IUtil, IUtilExtra, IDisposable
    {
        // ========
        // UTIL.CLS
        // ========
        // 
        // Implementation of the ASCOM DriverHelper Util class.
        // 
        // Written:  21-Aug-00   Robert B. Denny <rdenny@dc3.com>
        // 
        // Edits:
        // 
        // When      Who     What
        // --------- ---     --------------------------------------------------
        // 23-Feb-09 pwgs    5.1.0 - Refactored for Utilities
        // ---------------------------------------------------------------------

        private Stopwatch m_StopWatch = new(); // Create a high resolution timing device
        private string m_SerTraceFile = SERIAL_DEFAULT_FILENAME; // Set the default trace file name
        private TraceLogger TL;

        private RegistryAccess myProfile; // Hold the access object for the ASCOM profile store

        #region New and IDisposable Support
        private bool disposedValue = false;        // To detect redundant calls

        /// <summary>
        /// Create a Utility object
        /// </summary>
        public Util() : base()
        {
            myProfile = new RegistryAccess();
            WaitForMilliseconds(1); // Fire off the first instance which always takes longer than the others!
            TL = new TraceLogger("", "Util");
            TL.Enabled = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT); // Get enabled / disabled state from the user registry
            TL.LogMessage("New", "Trace logger created OK");
        }

        // IDisposable
        /// <summary>
        /// Disposes of resources used by the profile object - called by IDisposable interface
        /// </summary>
        /// <param name="disposing"></param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                if (myProfile is not null)
                {
                    myProfile.Dispose();
                    myProfile = null;
                }
            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// Disposes of resources used by the profile object
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finaliser
        /// </summary>
        ~Util()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region Util Implementation
        /// <summary>
        /// Pauses for a given interval in milliseconds.
        /// </summary>
        /// <param name="Milliseconds">The number of milliseconds to wait</param>
        /// <remarks>Repeatedly puts the calling Win32 process to sleep, totally freezing it, for 10 milliseconds, 
        /// then pumps events so the script or program calling it will receive its normal flow of events, until the 
        /// pause interval elapses. If the pause interval is 20 milliseconds or less, the sleep interval is reduced 
        /// to 0, causing the calling Win32 process to give up control to the kernel scheduler and then immediately 
        /// become eligible for scheduling. </remarks>
        public void WaitForMilliseconds(int Milliseconds)
        {
            double EndPoint;
            m_StopWatch.Reset(); // Initialise from last times use
            m_StopWatch.Start(); // Start timer straight away

            if (Milliseconds > 20) // Wait most of the time (to within the last 20 ms) using sleep(10) to reduce CPU usage
            {
                EndPoint = (Milliseconds - 20) * (double)Stopwatch.Frequency / 1000.0d;
                do
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                while (m_StopWatch.ElapsedTicks < EndPoint);
            }
            // Calculate the final tick end point and wait using sleep(0) for maximum accuracy
            EndPoint = Milliseconds * (double)Stopwatch.Frequency / 1000.0d;
            while (m_StopWatch.ElapsedTicks < EndPoint)
                Thread.Sleep(0);
        }
        // 
        // This will convert virtually anything resembling a sexagesimal
        // format number into a real number. The input may even be missing
        // the seconds or even the minutes part.
        // 
        /// <summary>
        /// Convert sexagesimal degrees to binary double-precision degrees
        /// </summary>
        /// <param name="DMS">The sexagesimal input string (degrees)</param>
        /// <returns>The double-precision binary value (degrees) represented by the sexagesimal input</returns>
        /// <remarks><para>The sexagesimal to real conversion methods such as this one are flexible enough to convert just 
        /// about anything that resembles sexagesimal. Thee way they operate is to first separate the input string 
        /// into numeric "tokens", strings consisting only of the numerals 0-9, plus and minus, and period. All other 
        /// characters are considered delimiters. Once the input string is parsed into tokens they are converted to 
        /// numerics. </para>
        /// <para>If there are more than three numeric tokens, only the first three are considered, the remainder 
        /// are ignored. Left to right positionally, the tokens are assumed to represent units (degrees or hours), 
        /// minutes, and seconds. If only two tokens are present, they are assumed to be units and minutes, and if 
        /// only one token is present, it is assumed to be units. Any token can have a fractional part. Of course it 
        /// would not be normal (for example) for both the minutes and seconds parts to have fractional parts, but it 
        /// would be legal. So 00:30.5:30 would convert to 1.0 unit. </para>
        /// <para>Note that plain units, for example 23.128734523 are acceptable to the method. </para>
        /// </remarks>
        public double DMSToDegrees(string DMS)
        {
            double DMSToDegreesRet = default;
            // Refactored to use .NET regular expressions
            short sg;
            Regex rx;
            MatchCollection ms;
            string Pattern;
            DMS = Strings.Trim(DMS); // Just in case...
            if (Strings.Left(DMS, 1) == "-")
            {
                sg = -1;
                DMS = Strings.Right(DMS, Strings.Len(DMS) - 1); // Remove '-'
            }
            else
            {
                sg = 1;
            }
            if (Strings.InStr(1.1d.ToString(), ",") > 0)
            {
                Pattern = @"[0-9\,]+";
            }
            else
            {
                Pattern = @"[0-9\.]+";
            }

            rx = new Regex(Pattern);
            // rx.Pattern = "[0-9\.]+"                             ' RX for number groups
            // 
            // Thanks to Chris Rowland, this allows conversions for systems
            // which use ',' -or '.' as the decimal point! Quite clever!!
            // 
            // rx.IgnoreCas = True
            // rx.Global = True
            ms = rx.Matches(DMS); // Find all number groups
            DMSToDegreesRet = 0.0d; // Assume no numbers at all
            if (ms.Count > 0) // At least one numeric part
            {
                DMSToDegreesRet = Conversions.ToDouble(ms[0].Value); // Degrees
                if (ms.Count > 1) // At least 2 numeric parts
                {
                    DMSToDegreesRet = DMSToDegreesRet + Conversions.ToDouble(ms[1].Value) / 60.0d; // Minutes
                    if (ms.Count > 2) // All three parts present
                    {
                        DMSToDegreesRet = DMSToDegreesRet + Conversions.ToDouble(ms[2].Value) / 3600.0d; // Seconds
                    }
                }
            }
            DMSToDegreesRet = sg * DMSToDegreesRet; // Apply sign
            return DMSToDegreesRet;

        }

        /// <summary>
        /// Convert sexagesimal hours to binary double-precision hours
        /// </summary>
        /// <param name="HMS">The sexagesimal input string (hours)</param>
        /// <returns>The double-precision binary value (hours) represented by the sexagesimal input </returns>
        /// <remarks>
        /// <para>The sexagesimal to real conversion methods such as this one are flexible enough to convert just about 
        /// anything that resembles sexagesimal. Thee way they operate is to first separate the input string into 
        /// numeric "tokens", strings consisting only of the numerals 0-9, plus and minus, and period. All other 
        /// characters are considered delimiters. Once the input string is parsed into tokens they are converted to 
        /// numerics. </para>
        /// 
        /// <para>If there are more than three numeric tokens, only the first three are considered, the remainder 
        /// are ignored. Left to right positionally, the tokens are assumed to represent units (degrees or hours), 
        /// minutes, and seconds. If only two tokens are present, they are assumed to be units and minutes, and if 
        /// only one token is present, it is assumed to be units. Any token can have a fractional part. </para>
        /// 
        /// <para>Of course it would not be normal (for example) for both the minutes and seconds parts to have 
        /// fractional parts, but it would be legal. So 00:30.5:30 would convert to 1.0 unit. Note that plain units, 
        /// for example 23.128734523 are acceptable to the method. </para>
        /// </remarks>
        public double HMSToHours(string HMS)
        {
            double HMSToHoursRet = default;
            HMSToHoursRet = DMSToDegrees(HMS);
            return HMSToHoursRet;
        }

        /// <summary>
        /// Convert sexagesimal hours to binary double-precision hours
        /// </summary>
        /// <param name="HMS">The sexagesimal input string (hours)</param>
        /// <returns>The double-precision binary value (hours) represented by the sexagesimal input</returns>
        /// <remarks>
        /// <para>The sexagesimal to real conversion methods such as this one are flexible enough to convert just about 
        /// anything that resembles sexagesimal. Thee way they operate is to first separate the input string into 
        /// numeric "tokens", strings consisting only of the numerals 0-9, plus and minus, and period. All other 
        /// characters are considered delimiters. Once the input string is parsed into tokens they are converted to 
        /// numerics. </para>
        /// 
        /// <para>If there are more than three numeric tokens, only the first three are considered, the remainder 
        /// are ignored. Left to right positionally, the tokens are assumed to represent units (degrees or hours), 
        /// minutes, and seconds. If only two tokens are present, they are assumed to be units and minutes, and if 
        /// only one token is present, it is assumed to be units. Any token can have a fractional part. </para>
        /// 
        /// <para>Of course it would not be normal (for example) for both the minutes and seconds parts to have 
        /// fractional parts, but it would be legal. So 00:30.5:30 would convert to 1.0 unit. Note that plain units, 
        /// for example 23.128734523 are acceptable to the method. </para>
        /// </remarks>
        public double HMSToDegrees(string HMS)
        {
            double HMSToDegreesRet = default;
            HMSToDegreesRet = DMSToDegrees(HMS) * 15.0d;
            return HMSToDegreesRet;
        }

        #region DegreesToDMS
        // 
        // Convert a real number to sexagesimal whole, minutes, seconds. Allow
        // specifying the number of decimal digits on seconds. Called by
        // HoursToHMS below, which just has different default delimiters.
        // 
        /// <summary>
        /// Convert degrees to sexagesimal degrees, minutes and seconds with default delimiters DD° MM' SS" 
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
        /// characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToDMS(double Degrees)
        {
            return DegreesToDMS(Degrees, "° ", "' ", "\"", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal degrees, minutes and seconds with with default minute and second delimiters MM' SS" 
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="DegDelim">The delimiter string separating degrees and minutes </param>
        /// <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
        /// characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToDMS(double Degrees, string DegDelim)
        {
            return DegreesToDMS(Degrees, DegDelim, "' ", "\"", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal degrees, minutes and seconds with default second delimiter SS" 
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="DegDelim">The delimiter string separating degrees and minutes </param>
        /// <param name="MinDelim">The delimiter string to append to the minutes part </param>
        /// <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
        /// characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToDMS(double Degrees, string DegDelim, string MinDelim)
        {
            return DegreesToDMS(Degrees, DegDelim, MinDelim, "\"", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal degrees, minutes and seconds
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="DegDelim">The delimiter string separating degrees and minutes </param>
        /// <param name="MinDelim">The delimiter string to append to the minutes part </param>
        /// <param name="SecDelim">The delimiter string to append to the seconds part</param>
        /// <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
        /// characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToDMS(double Degrees, string DegDelim, string MinDelim, string SecDelim)
        {
            return DegreesToDMS(Degrees, DegDelim, MinDelim, SecDelim, 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal degrees, minutes and seconds with specified second decimal places
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="DegDelim">The delimiter string separating degrees and minutes </param>
        /// <param name="MinDelim">The delimiter string to append to the minutes part </param>
        /// <param name="SecDelim">The delimiter string to append to the seconds part</param>
        /// <param name="SecDecimalDigits">The number of digits after the decimal point on the seconds part </param>
        /// <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
        /// characters.</para>
        /// </remarks>
        public string DegreesToDMS(double Degrees, string DegDelim, string MinDelim, string SecDelim, int SecDecimalDigits)
        {
            string DegreesToDMSRet = default;
            string w, m, s;
            bool n;
            string f;
            int i;

            if (Degrees < 0.0d)
            {
                Degrees = -Degrees;
                n = true;
            }
            else
            {
                n = false;
            }

            w = Strings.Format(Conversion.Fix(Degrees), "00"); // Whole part
            Degrees = (Degrees - Conversions.ToDouble(w)) * 60.0d; // Minutes
            m = Strings.Format(Conversion.Fix(Degrees), "00"); // Integral minutes
            Degrees = (Degrees - Conversions.ToDouble(m)) * 60.0d; // Seconds

            if (SecDecimalDigits == 0) // If no decimal digits wanted
            {
                f = "00"; // No decimal point or decimal digits
            }
            else // Decimal digits on seconds
            {
                f = "00."; // Format$ string
                var loopTo = SecDecimalDigits;
                for (i = 1; i <= loopTo; i++)
                    f = f + "0";
            }

            s = Strings.Format(Degrees, f); // Format seconds with requested decimal digits
            if (Strings.Left(s, 2) == "60") // If seconds got rounded to 60
            {
                s = Strings.Format(0, f); // Seconds are 0
                m = Strings.Format(Conversions.ToShort(m) + 1, "00"); // Carry to minutes
                if (m == "60") // If minutes got rounded to 60
                {
                    m = "00"; // Minutes are 0
                    w = Strings.Format(Conversions.ToShort(w) + 1, "00"); // Carry to whole part
                }
            }

            DegreesToDMSRet = w + DegDelim + m + MinDelim + s + SecDelim;
            if (n)
                DegreesToDMSRet = "-" + DegreesToDMSRet;
            return DegreesToDMSRet;

        }
        #endregion

        #region HoursToHMS
        /// <summary>
        /// Convert hours to sexagesimal hours, minutes, and seconds with default delimiters HH:MM:SS
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string HoursToHMS(double Hours)
        {
            return DegreesToDMS(Hours, ":", ":", "", 0);
        }

        /// <summary>
        /// Convert hours to sexagesimal hours, minutes, and seconds with default minutes and seconds delimiters MM:SS
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        /// <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string HoursToHMS(double Hours, string HrsDelim)
        {
            return DegreesToDMS(Hours, HrsDelim, ":", "", 0);
        }

        /// <summary>
        /// Convert hours to sexagesimal hours, minutes, and seconds with default second delimiter of null string
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        /// <param name="MinDelim">The delimiter string separating minutes and seconds </param>
        /// <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string HoursToHMS(double Hours, string HrsDelim, string MinDelim)
        {
            return DegreesToDMS(Hours, HrsDelim, MinDelim, "", 0);
        }

        /// <summary>
        /// Convert hours to sexagesimal hours, minutes, and seconds
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        /// <param name="MinDelim">The delimiter string separating minutes and seconds </param>
        /// <param name="SecDelim">The delimiter string to append to the seconds part </param>
        /// <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string HoursToHMS(double Hours, string HrsDelim, string MinDelim, string SecDelim)
        {
            return DegreesToDMS(Hours, HrsDelim, MinDelim, SecDelim, 0);
        }

        /// <summary>
        /// Convert hours to sexagesimal hours, minutes, and seconds with specified number of second decimal places
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        /// <param name="MinDelim">The delimiter string separating minutes and seconds </param>
        /// <param name="SecDelim">The delimiter string to append to the seconds part </param>
        /// <param name="SecDecimalDigits">The number of digits after the decimal point on the seconds part </param>
        /// <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// </remarks>
        public string HoursToHMS(double Hours, string HrsDelim, string MinDelim, string SecDelim, int SecDecimalDigits)
        {
            return DegreesToDMS(Hours, HrsDelim, MinDelim, SecDelim, SecDecimalDigits);
        }
        #endregion

        #region DegreesToHMS
        // Public Overloads Function DegreesToHMS(ByVal Degrees As Double, Optional ByVal HrsDelim As String = ":", Optional ByVal MinDelim As String = ":", Optional ByVal SecDelim As String = "", Optional ByVal SecDecimalDigits As Short = 0) As String Implements IUtil.DegreesToHMS
        /// <summary>
        /// Convert degrees to sexagesimal hours, minutes, and seconds with default delimiters of HH:MM:SS
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself.</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToHMS(double Degrees)
        {
            return DegreesToHMS(Degrees, ":", ":", "", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal hours, minutes, and seconds with the default second and minute delimiters of MM:SS
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToHMS(double Degrees, string HrsDelim)
        {
            return DegreesToHMS(Degrees, HrsDelim, ":", "", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal hours, minutes, and seconds with the default second delimiter SS (null string)
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <param name="MinDelim">The delimiter string separating minutes and seconds</param>
        /// <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim)
        {
            return DegreesToHMS(Degrees, HrsDelim, MinDelim, "", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal hours, minutes, and seconds
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <param name="MinDelim">The delimiter string separating minutes and seconds</param>
        /// <param name="SecDelim">The delimiter string to append to the seconds part </param>
        /// <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim, string SecDelim)
        {
            return DegreesToHMS(Degrees, HrsDelim, MinDelim, SecDelim, 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal hours, minutes, and seconds with the specified number of second decimal places
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <param name="MinDelim">The delimiter string separating minutes and seconds</param>
        /// <param name="SecDelim">The delimiter string to append to the seconds part </param>
        /// <param name="SecDecimalDigits">The number of digits after the decimal point on the seconds part </param>
        /// <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
        /// </remarks>
        public string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim, string SecDelim, int SecDecimalDigits)
        {
            return DegreesToDMS(Degrees / 15.0d, HrsDelim, MinDelim, SecDelim, SecDecimalDigits);
        }

        #endregion

        #region DegreesToDM
        // Convert a real number to sexagesimal whole, minutes. Allow
        // specifying the number of decimal digits on minutes. Called by
        // HoursToHM below, which just has different default delimiters.

        /// <summary>
        /// Convert degrees to sexagesimal degrees and minutes with default delimiters DD° MM'
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToDM(double Degrees)
        {
            return DegreesToDM(Degrees, "° ", "'", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal degrees and minutes with the default minutes delimiter MM'
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="DegDelim">The delimiter string separating degrees </param>
        /// <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToDM(double Degrees, string DegDelim)
        {
            return DegreesToDM(Degrees, DegDelim, "'", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal degrees and minutes
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="DegDelim">The delimiter string separating degrees </param>
        /// <param name="MinDelim">The delimiter string to append to the minutes </param>
        /// <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToDM(double Degrees, string DegDelim, string MinDelim)
        {
            return DegreesToDM(Degrees, DegDelim, MinDelim, 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal degrees and minutes with the specified number of minute decimal places
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="DegDelim">The delimiter string separating degrees </param>
        /// <param name="MinDelim">The delimiter string to append to the minutes </param>
        /// <param name="MinDecimalDigits">The number of digits after the decimal point on the minutes part </param>
        /// <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// </remarks>
        public string DegreesToDM(double Degrees, string DegDelim, string MinDelim, int MinDecimalDigits)
        {
            string DegreesToDMRet = default;
            string w, m, f;
            bool n;
            int i;

            if (Degrees < 0.0d)
            {
                Degrees = -Degrees;
                n = true;
            }
            else
            {
                n = false;
            }

            w = Strings.Format(Conversion.Fix(Degrees), "00"); // Whole part
            Degrees = (Degrees - Conversions.ToDouble(w)) * 60.0d; // Minutes

            if (MinDecimalDigits == 0) // If no decimal digits wanted
            {
                f = "00"; // No decimal point or decimal digits
            }
            else // Decimal digits on minutes
            {
                f = "00."; // Format$ string
                var loopTo = MinDecimalDigits;
                for (i = 1; i <= loopTo; i++)
                    f = f + "0";
            }

            m = Strings.Format(Degrees, f); // Format minutes with requested decimal digits
            if (Strings.Left(m, 2) == "60") // If minutes got rounded to 60
            {
                m = Strings.Format(0, f); // minutes are 0
                w = Strings.Format(Conversions.ToShort(w) + 1, "00"); // Carry to whole part
            }

            DegreesToDMRet = w + DegDelim + m + MinDelim;
            if (n)
                DegreesToDMRet = "-" + DegreesToDMRet;
            return DegreesToDMRet;

        }
        #endregion

        #region HoursToHM
        /// <summary>
        /// Convert hours to sexagesimal hours and minutes with default delimiters HH:MM
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with an suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string HoursToHM(double Hours)
        {
            return DegreesToDM(Hours, ":", "", 0);
        }

        /// <summary>
        /// Convert hours to sexagesimal hours and minutes with default minutes delimiter MM (null string)
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with an suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string HoursToHM(double Hours, string HrsDelim)
        {
            return DegreesToDM(Hours, HrsDelim, "", 0);
        }

        /// <summary>
        /// Convert hours to sexagesimal hours and minutes
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <param name="MinDelim">The delimiter string to append to the minutes part </param>
        /// <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// <para>This overload is not available through COM, please use 
        /// "HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with an suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string HoursToHM(double Hours, string HrsDelim, string MinDelim)
        {
            return DegreesToDM(Hours, HrsDelim, MinDelim, 0);
        }

        /// <summary>
        /// Convert hours to sexagesimal hours and minutes with supplied number of minute decimal places
        /// </summary>
        /// <param name="Hours">The hours value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours </param>
        /// <param name="MinDelim">The delimiter string to append to the minutes part </param>
        /// <param name="MinDecimalDigits">The number of digits after the decimal point on the minutes part </param>
        /// <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        /// </remarks>
        public string HoursToHM(double Hours, string HrsDelim, string MinDelim, int MinDecimalDigits)
        {
            return DegreesToDM(Hours, HrsDelim, MinDelim, MinDecimalDigits);
        }

        #endregion

        #region DegreesToHM
        // Public Function DegreesToHM(ByVal Degrees As Double, Optional ByVal HrsDelim As String = ":", Optional ByVal MinDelim As String = "", Optional ByVal MinDecimalDigits As Short = 0) As String Implements IUtil.DegreesToHM
        /// <summary>
        /// Convert degrees to sexagesimal hours and minutes with default delimiters HH:MM
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToHM(double Degrees)
        {
            return DegreesToHM(Degrees, ":", "", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal hours and minutes with default minute delimiter MM (null string)
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToHM(double Degrees, string HrsDelim)
        {
            return DegreesToHM(Degrees, HrsDelim, "", 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal hours and minutes
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <param name="MinDelim">The delimiter string to append to the minutes part</param>
        /// <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        /// <para>This overload is not available through COM, please use 
        /// "DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        /// with suitable parameters to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public string DegreesToHM(double Degrees, string HrsDelim, string MinDelim)
        {
            return DegreesToHM(Degrees, HrsDelim, MinDelim, 0);
        }

        /// <summary>
        /// Convert degrees to sexagesimal hours and minutes with supplied number of minute decimal places
        /// </summary>
        /// <param name="Degrees">The degrees value to convert</param>
        /// <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        /// <param name="MinDelim">The delimiter string to append to the minutes part</param>
        /// <param name="MinDecimalDigits">Number of minutes decimal places</param>
        /// <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        /// <remarks>
        /// <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        /// </remarks>
        public string DegreesToHM(double Degrees, string HrsDelim, string MinDelim, int MinDecimalDigits)
        {
            return DegreesToDM(Degrees / 15.0d, HrsDelim, MinDelim, MinDecimalDigits);
        }

        #endregion

        #endregion

        #region Util2 Implementation

        /// <summary>
        /// Current Platform version in Major.Minor form
        /// </summary>
        /// <returns>Current Platform version in Major.Minor form</returns>
        /// <remarks>Please be careful if you wish to convert this string into a number within your application 
        /// because the ASCOM Platform is used internationally and some countries use characters other 
        /// than point as the decimal separator. 
        /// <para>If your application tries to convert 5.5 into a Double value when running on a PC localised to 
        /// France, you will get an exception because the French decimal separator is comma and 5.5 is not 
        /// a valid representation of a decimal number in that locale.</para>
        /// <para>If you still wish to turn the Platform Version into a Double value, you can use an 
        /// approach such as this:</para>
        /// <code>If Double.Parse(Util.PlatformVersion, CultureInfo.InvariantCulture) &lt; 5.5 Then...</code>
        /// <para>If you just wish to test whether the platform is greater than a particular level,
        /// you can use the <see cref="IsMinimumRequiredVersion">IsMinimumRequiredVersion</see> method.</para>
        /// </remarks>
        public string PlatformVersion
        {
            get
            {
                string PlatformVersionRet = default;
                PlatformVersionRet = myProfile.GetProfile("", "PlatformVersion");
                PlatformVersionRet = ConditionPlatformVersion(PlatformVersionRet, myProfile, TL); // Check for Forced Platform versions
                TL.LogMessage("PlatformVersion Get", PlatformVersionRet);
                return PlatformVersionRet;
            }
        }

        /// <summary>
        /// Tests whether the current platform version is at least equal to the supplied major and minor 
        /// version numbers, returns false if this is not the case
        /// </summary>
        /// <param name="RequiredMajorVersion">The required major version number</param>
        /// <param name="RequiredMinorVersion">The required minor version number</param>
        /// <returns>True if the current platform version equals or exceeds the major and minor values provided</returns>
        /// <remarks>This function provides a simple way to test for a minimum platform level.
        /// If for example, your application requires at least platform version 5.5 then you can use 
        /// code such as this to make a test and display information as appropriate.
        /// <code > Const requiredMajorVersion as Integer = 5
        /// Const requiredMinorVersion as Integer = 5 ' Requires Platform version 5.5
        /// Dim Utils as New ASCOM.Utilities.Util
        /// isOK = Utils.IsMinimumRequiredVersion(requiredMajorVersion, requiredMinorVersion)
        /// If Not isOK Then 
        ///    ' Abort, throw exception, print an error or whatever.
        ///    End
        /// EndIf
        /// 
        /// </code></remarks>
        public bool IsMinimumRequiredVersion(int RequiredMajorVersion, int RequiredMinorVersion)
        {
            Version PlatformVersion, RequiredVersion;
            // Create a version object from the platform version string
            PlatformVersion = new Version(myProfile.GetProfile("", "PlatformVersion"));
            // Create a version object from the supplied major and minor required version numbers
            RequiredVersion = new Version(RequiredMajorVersion, RequiredMinorVersion);

            if (PlatformVersion.CompareTo(RequiredVersion) >= 0)
            {
                return true; // Platform version is equal to or greater than the required version
            }
            else
            {
                return false;
            } // Platform version is less than the required version
        }

        /// <summary>
        /// Change the serial trace file (default C:\SerialTrace.txt)
        /// </summary>
        /// <value>Serial trace file name including fully qualified path e.g. C:\SerialTrace.txt</value>
        /// <returns>Serial trace file name </returns>
        /// <remarks>Change this before setting the SerialTrace property to True. </remarks>
        public string SerialTraceFile
        {
            get
            {
                return m_SerTraceFile;
            }
            set
            {
                m_SerTraceFile = value;
            }
        }

        /// <summary>
        /// Enable/disable serial I/O tracing
        /// </summary>
        /// <value>Boolean - Enable/disable serial I/O tracing</value>
        /// <returns>Enabled - disabled state of serial tracing</returns>
        /// <remarks>If you want to change the serial trace file path, change the SerialTraceFile property before setting this to True. 
        /// After setting this to True, serial trace info will be written to the last-set serial trace file. </remarks>
        public bool SerialTrace
        {
            get
            {
                if (!string.IsNullOrEmpty(myProfile.GetProfile("", SERIAL_FILE_NAME_VARNAME))) // There is a filename so tracing is enabled
                {
                    return true;
                }
                else // No filename so tracing is disabled
                {
                    return false;
                }
            }
            set
            {
                if (value) // We are enabling tracing so write the filename to profile
                {
                    myProfile.WriteProfile("", SERIAL_FILE_NAME_VARNAME, m_SerTraceFile);
                }
                else // Disabling so write a null string instead
                {
                    myProfile.WriteProfile("", SERIAL_FILE_NAME_VARNAME, "");
                }
            }
        }

        /// <summary>
        /// The name of the computer's time zone
        /// </summary>
        /// <returns>The name of the computer's time zone</returns>
        /// <remarks>This will be in the local language of the operating system, and will reflect any daylight/summer time that may be in 
        /// effect. </remarks>
        public string TimeZoneName
        {
            get
            {
                return GetTimeZoneName();
            }
        }

        /// <summary>
        /// UTC offset (hours) for the computer's clock
        /// </summary>
        /// <returns>UTC offset (hours) for the computer's clock</returns>
        /// <remarks>The offset is in hours, such that UTC = local + offset. The offset includes any daylight/summer time that may be 
        /// in effect.</remarks>
        public double TimeZoneOffset
        {
            get
            {
                return GetTimeZoneOffset();
            }
        }

        /// <summary>
        /// The current UTC Date
        /// </summary>
        /// <returns>The current UTC Date</returns>
        /// <remarks></remarks>
        public DateTime UTCDate
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Current Julian date
        /// </summary>
        /// <returns>Current Julian date</returns>
        /// <remarks>This is quantised to the second in the COM component but to a small decimal fraction in the .NET component</remarks>
        public double JulianDate
        {
            get
            {
                return DateUTCToJulian(DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Convert local-time Date to Julian date
        /// </summary>
        /// <param name="LocalDate">Date in local-time</param>
        /// <returns>Julian date</returns>
        /// <remarks>Julian dates are always in UTC </remarks>
        public double DateLocalToJulian(DateTime LocalDate)
        {
            return DateUTCToJulian(CvtUTC(ref LocalDate));
        }

        /// <summary>
        /// Convert Julian date to local-time Date
        /// </summary>
        /// <param name="JD">Julian date to convert</param>
        /// <returns>Date in local-time for the given Julian date</returns>
        /// <remarks>Julian dates are always in UTC</remarks>
        public DateTime DateJulianToLocal(double JD)
        {
            var argd = DateJulianToUTC(JD);
            return CvtLocal(ref argd);
        }

        /// <summary>
        /// Convert UTC Date to Julian date
        /// </summary>
        /// <param name="UTCDate">UTC date to convert</param>
        /// <returns>Julian date</returns>
        /// <remarks>Julian dates are always in UTC </remarks>
        public double DateUTCToJulian(DateTime UTCDate)
        {
            return UTCDate.ToOADate() + 2415018.5d;
        }

        /// <summary>
        /// Convert Julian date to UTC Date
        /// </summary>
        /// <param name="JD">Julian date</param>
        /// <returns>Date in UTC for the given Julian date</returns>
        /// <remarks>Julian dates are always in UTC </remarks>
        public DateTime DateJulianToUTC(double JD)
        {
            return DateTime.FromOADate(JD - 2415018.5d);
        }

        /// <summary>
        /// Convert UTC Date to local-time Date
        /// </summary>
        /// <param name="UTCDate">Date in UTC</param>
        /// <returns>Date in local-time</returns>
        /// <remarks></remarks>
        public DateTime DateUTCToLocal(DateTime UTCDate)
        {
            return CvtLocal(ref UTCDate);
        }

        /// <summary>
        /// Convert local-time Date to UTC Date
        /// </summary>
        /// <param name="LocalDate">Date in local-time</param>
        /// <returns> Date in UTC</returns>
        /// <remarks></remarks>
        public DateTime DateLocalToUTC(DateTime LocalDate)
        {
            return CvtUTC(ref LocalDate);
        }

        /// <summary>
        /// Convert a string safearray to an ArrayList that can be used in scripting languages
        /// </summary>
        /// <param name="stringArray">Array of strings</param>
        /// <returns>Collection of integers</returns>
        /// <remarks></remarks>
        public ArrayList ToStringCollection(string[] stringArray)
        {
            ArrayList ToStringCollectionRet = default;
            ToStringCollectionRet = new ArrayList();
            foreach (string item in stringArray)
                ToStringCollectionRet.Add(item);
            return ToStringCollectionRet;
        }

        /// <summary>
        /// Convert an integer safearray to an ArrayList collection that can be used in scripting languages
        /// </summary>
        /// <param name="integerArray">Safearray of integers</param>
        /// <returns>Colection of integers</returns>
        /// <remarks></remarks>
        public ArrayList ToIntegerCollection(int[] integerArray)
        {
            ArrayList ToIntegerCollectionRet = default;
            ToIntegerCollectionRet = new ArrayList();
            foreach (int item in integerArray)
                ToIntegerCollectionRet.Add(item);
            return ToIntegerCollectionRet;
        }

        /// <summary>
        /// Convert from one set of speed / temperature / pressure rain rate units to another
        /// </summary>
        /// <param name="InputValue">Value to convert</param>
        /// <param name="FromUnits">Integer value from the Units enum indicating the value's current units</param>
        /// <param name="ToUnits">Integer value from the Units enum indicating the units to which the input value should be converted</param>
        /// <returns>Input value expressed in the new units</returns>
        /// <exception cref="InvalidValueException">When the specified from and to units can not refer to the same value. e.g. attempting to convert miles per hour to degrees Celsius</exception>
        /// <remarks>
        /// <para>Conversions available:</para>
        /// <list type="bullet">
        /// <item>metres per second &lt;==&gt; miles per hour &lt;==&gt; knots</item>
        /// <item>Celsius &lt;==&gt; Fahrenheit &lt;==&gt; Kelvin</item>
        /// <item>hecto Pascals (hPa) &lt;==&gt; milli bar(mbar) &lt;==&gt; mm of mercury &lt;==&gt; inches of mercury</item>
        /// <item>mm per hour &lt;==&gt; inches per hour</item>
        /// </list>
        /// <para>Knots conversions use the international nautical mile definition (1 nautical mile = 1852m) rather than the original UK or US Admiralty definitions.</para>
        /// <para>For convenience, milli bar and hecto Pascals are shown as separate units even though they have numerically identical values and there is a 1:1 conversion between them.</para>
        /// </remarks>
        public double ConvertUnits(double InputValue, Units FromUnits, Units ToUnits)
        {
            double intermediateValue, finalValue;

            if (FromUnits >= Units.metresPerSecond & FromUnits <= Units.knots & ToUnits >= Units.metresPerSecond & ToUnits <= Units.knots)        // Speed conversion
            {
                // First convert the input to metres per second
                switch (FromUnits)
                {
                    case Units.metresPerSecond:
                        {
                            intermediateValue = InputValue;
                            break;
                        }
                    case Units.milesPerHour:
                        {
                            intermediateValue = InputValue * 0.44704d;
                            break;
                        }
                    case Units.knots:
                        {
                            intermediateValue = InputValue * 0.514444444d;
                            break;
                        }

                    default:
                        {
                            throw new InvalidValueException("Unknown \"From\" speed units: " + FromUnits.ToString());
                        }
                }

                // Now convert metres per second to the output value
                switch (ToUnits)
                {
                    case Units.metresPerSecond:
                        {
                            finalValue = intermediateValue;
                            break;
                        }
                    case Units.milesPerHour:
                        {
                            finalValue = intermediateValue / 0.44704d;
                            break;
                        }
                    case Units.knots:
                        {
                            finalValue = intermediateValue / 0.514444444d;
                            break;
                        }

                    default:
                        {
                            throw new InvalidValueException("Unknown \"To\" speed units: " + ToUnits.ToString());
                        }
                }

                return finalValue;
            }

            else if (FromUnits >= Units.degreesCelsius & FromUnits <= Units.degreesKelvin & ToUnits >= Units.degreesCelsius & ToUnits <= Units.degreesKelvin) // Temperature conversion
            {

                // First convert the input to degrees K
                switch (FromUnits)
                {
                    case Units.degreesCelsius:
                        {
                            intermediateValue = InputValue - ABSOLUTE_ZERO_CELSIUS;
                            break;
                        }
                    case Units.degreesFahrenheit:
                        {
                            intermediateValue = (InputValue + 459.67d) * 5.0d / 9.0d;
                            break;
                        }
                    case Units.degreesKelvin:
                        {
                            intermediateValue = InputValue;
                            break;
                        }

                    default:
                        {
                            throw new InvalidValueException("Unknown \"From\" temperature units: " + FromUnits.ToString());
                        }
                }

                // Now convert degrees K to the output value
                switch (ToUnits)
                {
                    case Units.degreesCelsius:
                        {
                            finalValue = intermediateValue + ABSOLUTE_ZERO_CELSIUS;
                            break;
                        }
                    case Units.degreesFahrenheit:
                        {
                            finalValue = intermediateValue * 9.0d / 5.0d - 459.67d;
                            break;
                        }
                    case Units.degreesKelvin:
                        {
                            finalValue = intermediateValue;
                            break;
                        }

                    default:
                        {
                            throw new InvalidValueException("Unknown \"To\" temperature units: " + ToUnits.ToString());
                        }
                }

                return finalValue;
            }
            else if (FromUnits >= Units.hPa & FromUnits <= Units.inHg & ToUnits >= Units.hPa & ToUnits <= Units.inHg) // Pressure conversion
            {
                // First convert the input to hPa
                switch (FromUnits)
                {
                    case Units.hPa:
                        {
                            intermediateValue = InputValue;
                            break;
                        }
                    case Units.mBar:
                        {
                            intermediateValue = InputValue;
                            break;
                        }
                    case Units.mmHg:
                        {
                            intermediateValue = InputValue * 1.33322368d;
                            break;
                        }
                    case Units.inHg:
                        {
                            intermediateValue = InputValue * 33.8638816d;
                            break;
                        }

                    default:
                        {
                            throw new InvalidValueException("Unknown \"From\" pressure units: " + FromUnits.ToString());
                        }
                }

                // Now convert hPa to the output value
                switch (ToUnits)
                {
                    case Units.hPa:
                        {
                            finalValue = intermediateValue;
                            break;
                        }
                    case Units.mBar:
                        {
                            finalValue = intermediateValue;
                            break;
                        }
                    case Units.mmHg:
                        {
                            finalValue = intermediateValue / 1.33322368d;
                            break;
                        }
                    case Units.inHg:
                        {
                            finalValue = intermediateValue / 33.8638816d;
                            break;
                        }

                    default:
                        {
                            throw new InvalidValueException("Unknown \"To\" pressure units: " + ToUnits.ToString());
                        }
                }

                return finalValue;
            }

            else if (FromUnits >= Units.mmPerHour & FromUnits <= Units.inPerHour & ToUnits >= Units.mmPerHour & ToUnits <= Units.inPerHour) // Rain rate conversion
            {
                // First convert the input to mm
                switch (FromUnits)
                {
                    case Units.mmPerHour:
                        {
                            intermediateValue = InputValue;
                            break;
                        }
                    case Units.inPerHour:
                        {
                            intermediateValue = InputValue * 25.4d;
                            break;
                        }

                    default:
                        {
                            throw new InvalidValueException("Unknown \"From\" rain rate units: " + FromUnits.ToString());
                        }
                }

                // Now convert mm to the output value
                switch (ToUnits)
                {
                    case Units.mmPerHour:
                        {
                            finalValue = intermediateValue;
                            break;
                        }
                    case Units.inPerHour:
                        {
                            finalValue = intermediateValue / 25.4d;
                            break;
                        }

                    default:
                        {
                            throw new InvalidValueException("Unknown \"To\" rain rate units: " + ToUnits.ToString());
                        }
                }

                return finalValue;
            }

            else
            {
                throw new InvalidOperationException("From and to units are not of the same type. From: " + FromUnits.ToString() + ", To: " + ToUnits.ToString());
            }
        }

        /// <summary>
        /// Calculate the dew point (°Celsius) given the ambient temperature (°Celsius) and relative humidity (%)
        /// </summary>
        /// <param name="RelativeHumidity">Relative humidity expressed in percent (0.0 .. 100.0)</param>
        /// <param name="AmbientTemperature">Ambient temperature (°Celsius)</param>
        /// <returns>Dew point (°Celsius)</returns>
        /// <exception cref="InvalidValueException">When relative humidity &lt; 0.0% or &gt; 100.0%></exception>
        /// <exception cref="InvalidValueException">When ambient temperature &lt; absolute zero or &gt; 100.0C></exception>
        /// <remarks>'Calculation uses Vaisala formula for water vapour saturation pressure and is accurate to 0.083% over -20C - +50°C
        /// <para>http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf </para>
        /// </remarks>
        public double Humidity2DewPoint(double RelativeHumidity, double AmbientTemperature)
        {
            // Formulae taken from Vaisala: http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf 
            double Pws, Pw, Td;

            // Constants from Vaisala document
            const double A = 6.116441d;
            const double m = 7.591386d;
            const double Tn = 240.7263d;

            // Validate input values
            if (RelativeHumidity < 0.0d | RelativeHumidity > 100.0d)
                throw new InvalidValueException("Humidity2DewPoint - Relative humidity is < 0.0% or > 100.0%: " + RelativeHumidity.ToString());
            if (AmbientTemperature < ABSOLUTE_ZERO_CELSIUS | AmbientTemperature > 100.0d)
                throw new InvalidValueException("Humidity2DewPoint - Ambient temperature is < " + ABSOLUTE_ZERO_CELSIUS + "C or > 100.0C: " + AmbientTemperature.ToString());

            Pws = A * Math.Pow(10.0d, m * AmbientTemperature / (AmbientTemperature + Tn)); // Calculate water vapor saturation pressure, Pws, from Vaisala formula (6) - In hPa
            Pw = Pws * RelativeHumidity / 100.0d; // Calculate measured vapor pressure, Pw
            Td = Tn / (m / Math.Log10(Pw / A) - 1.0d); // Finally, calculate dew-point in °C

            TL.LogMessage("Humidity2DewPoint", "DewPoint: " + Td + ", Given Relative Humidity: " + RelativeHumidity + ", Given Ambient temperature: " + AmbientTemperature + ", Pws: " + Pws + ", Pw: " + Pw);

            return Td;
        }

        /// <summary>
        /// Calculate the relative humidity (%) given the ambient temperature (°Celsius) and dew point (°Celsius)
        /// </summary>
        /// <param name="DewPoint">Dewpoint in (°Celsius)</param>
        /// <param name="AmbientTemperature">Ambient temperature (°Celsius)</param>
        /// <returns>Humidity expressed in percent (0.0 .. 100.0)</returns>
        /// <exception cref="InvalidValueException">When dew point &lt; absolute zero or &gt; 100.0C></exception>
        /// <exception cref="InvalidValueException">When ambient temperature &lt; absolute zero or &gt; 100.0C></exception>
        /// <remarks>'Calculation uses the Vaisala formula for water vapour saturation pressure and is accurate to 0.083% over -20C - +50°C
        /// <para>http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf </para>
        /// </remarks>
        public double DewPoint2Humidity(double DewPoint, double AmbientTemperature)
        {
            // Formulae taken from Vaisala: http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf 
            double RH;

            // Constants from Vaisala document
            const double m = 7.591386d;
            const double Tn = 240.7263d;

            // Validate input values
            if (DewPoint < ABSOLUTE_ZERO_CELSIUS | DewPoint > 100.0d)
                throw new InvalidValueException("DewPoint2Humidity - Dew point is < " + ABSOLUTE_ZERO_CELSIUS + "C or > 100.0C: " + DewPoint.ToString());
            if (AmbientTemperature < ABSOLUTE_ZERO_CELSIUS | AmbientTemperature > 100.0d)
                throw new InvalidValueException("DewPoint2Humidity - Ambient temperature is < " + ABSOLUTE_ZERO_CELSIUS + "C or > 100.0C: " + AmbientTemperature.ToString());

            RH = 100.0d * Math.Pow(10.0d, m * (DewPoint / (DewPoint + Tn) - AmbientTemperature / (AmbientTemperature + Tn)));
            TL.LogMessage("DewPoint2Humidity", "RH: " + RH + ", Given Dew point: " + DewPoint + ", Given Ambient temperature: " + AmbientTemperature);

            return RH;
        }

        /// <summary>
        /// Convert atmospheric pressure from one altitude above mean sea level to another
        /// </summary>
        /// <param name="Pressure">Measured pressure in hPa (mBar) at the "From" altitude</param>
        /// <param name="FromAltitudeAboveMeanSeaLevel">"Altitude at which the input pressure was measured (metres)</param>
        /// <param name="ToAltitudeAboveMeanSeaLevel">Altitude to which the pressure is to be converted (metres)</param>
        /// <returns>Pressure in hPa at the "To" altitude</returns>
        /// <remarks>Uses the equation: p = p0 * (1.0 - 2.25577E-05 h)^5.25588</remarks>
        public double ConvertPressure(double Pressure, double FromAltitudeAboveMeanSeaLevel, double ToAltitudeAboveMeanSeaLevel)
        {
            // Convert supplied pressure to sea level then convert again to the required altitude using this equation:
            // p = p0 (1 - 2.25577 10-5 h)5.25588
            double SeaLevelPressure, ActualPressure;
            SeaLevelPressure = Pressure / Math.Pow(1.0d - 0.0000225577d * FromAltitudeAboveMeanSeaLevel, 5.25588d);
            ActualPressure = SeaLevelPressure * Math.Pow(1.0d - 0.0000225577d * ToAltitudeAboveMeanSeaLevel, 5.25588d);

            TL.LogMessage("ConvertPressure", "SeaLevelPressure: " + SeaLevelPressure + ", ActualPressure: " + ActualPressure + ", Given Pressure: " + Pressure + ", Given FromAltitudeAboveMeanSeaLevel: " + FromAltitudeAboveMeanSeaLevel + ", Given ToAltitudeAboveMeanSeaLevel: " + ToAltitudeAboveMeanSeaLevel);

            return ActualPressure;
        }

        #endregion

        #region Array To ArrAyVariant Code

        /// <summary>
        /// Convert an array of .NET built-in types to an equivalent Variant arrray (array of .NET Objects)
        /// </summary>
        /// <param name="SuppliedObject">The array to convert to variant types</param>
        /// <returns>A Variant array</returns>
        /// <exception cref="InvalidValueException">If the supplied array contains elements of an unsuported Type.</exception>
        /// <exception cref="InvalidValueException">If the array rank is outside the range 1 to 5.</exception>
        /// <exception cref="InvalidValueException">If the supplied object is not an array.</exception>
        /// <remarks>This function will primarily be of use to Scripting Language programmers who need to convert Camera and Video ImageArrays from their native types to Variant types. If this is not done, 
        /// the scripting language will throw a type mismatch exception when it receives, for example, Int32 element types instead of the expected Variant types.
        /// <para>A VBScript Camera usage example is: Image = UTIL.ArrayToVariantArray(CAMERA.ImageArray) This example assumes that the camera and utilities objects have already been created with CreateObject statements.</para>
        /// <para>The supported .NET types are:
        /// <list type="bullet">
        /// <item><description>Int16</description></item>
        /// <item><description>Int32</description></item>
        /// <item><description>UInt16</description></item>
        /// <item><description>UInt32</description></item>
        /// <item><description>UInt64</description></item>
        /// <item><description>Byte</description></item>
        /// <item><description>SByte</description></item>
        /// <item><description>Single</description></item>
        /// <item><description>Double</description></item>
        /// <item><description>Boolean</description></item>
        /// <item><description>DateTime</description></item>
        /// <item><description>String</description></item>
        /// </list>
        /// </para>
        /// <para>The function supports arrays with 1 to 5 dimensions (Rank = 1 to 5). If the supplied array already contains elements of Variant type, it is returned as-is without any processing.</para></remarks>
        public object ArrayToVariantArray(object SuppliedObject)
        {
            object ReturnObject; // An object tp represent the Variant array
            Type TypeOfSuppliedObject, ArrayType; // Variables to hold the Type of the Array and the Type of its elements
            Array SuppliedArray; // Variable to hold the supplied array as an Array type (as opposed to Object)
            string ElementTypeName; // Variable to hold the name of the type of elements in the array
            var Sw = new Stopwatch();

            Sw.Start();

            try
            {
                TypeOfSuppliedObject = SuppliedObject.GetType(); // Get the Type of the supplied object

                if (TypeOfSuppliedObject.IsArray) // If the object is an array then process
                {
                    SuppliedArray = (Array)SuppliedObject; // Convert the Object to an Array type
                    ArrayType = SuppliedArray.GetType(); // Get the type of the array elements
                    ElementTypeName = ArrayType.GetElementType().Name;
                    TL.LogMessage("ArrayToVariantArray", "Array Type: " + ArrayType.Name + ", Element Type: " + ElementTypeName + ", Array Rank: " + SuppliedArray.Rank);

                    switch (ElementTypeName ?? "") // Compare the supplied element type with the list of support types
                    {
                        case var @case when @case == (typeof(object).Name ?? ""):
                            {
                                ReturnObject = SuppliedObject; // Already a variant array so just return the original array
                                break;
                            }
                        case var case1 when case1 == (typeof(short).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<short>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case2 when case2 == (typeof(int).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<int>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case3 when case3 == (typeof(long).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<long>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case4 when case4 == (typeof(ushort).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<ushort>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case5 when case5 == (typeof(uint).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<uint>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case6 when case6 == (typeof(ulong).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<ulong>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case7 when case7 == (typeof(byte).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<byte>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case8 when case8 == (typeof(sbyte).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<sbyte>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case9 when case9 == (typeof(float).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<float>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case10 when case10 == (typeof(double).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<double>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case11 when case11 == (typeof(bool).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<bool>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case12 when case12 == (typeof(DateTime).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<DateTime>(SuppliedObject, SuppliedArray);
                                break;
                            }
                        case var case13 when case13 == (typeof(string).Name ?? ""):
                            {
                                ReturnObject = ProcessArray<string>(SuppliedObject, SuppliedArray); // We have a non-supported element type so throw an exception
                                break;
                            }

                        default:
                            {
                                TL.LogMessage("ArrayToVariantArray", "Unsupported array type: " + ElementTypeName + ", throwing exception");
                                throw new InvalidValueException("Unsupported array type: " + ElementTypeName);
                            }
                    }
                }
                else // Not an array so throw an exception
                {
                    TL.LogMessage("ArrayToVariantArray", "Supplied object is not an array, throwing exception");
                    throw new InvalidValueException("Supplied object is not an array");
                }

                Sw.Stop();
                TL.LogMessage("ArrayToVariantArray", "Completed processing in " + Sw.Elapsed.TotalMilliseconds.ToString("0.0") + " milliseconds");

                return ReturnObject; // Return the variant array
            }

            catch (Exception ex) // Catch any exceptions, log them and return to the calling application
            {
                TL.LogMessageCrLf("ArrayToVariantArray", "Exception: " + ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Turns an array of type T into a variant array of Object
        /// </summary>
        /// <typeparam name="T">The type to convert to Variant</typeparam>
        /// <param name="SuppliedObject">The supplied array of Type T as an Object</param>
        /// <param name="SuppliedArray">The supplied array of Type T as an Array</param>
        /// <returns>The array with all elements represented as Variant objects</returns>
        /// <remarks>Works for 1 to 5 dimensional arrays of any Type</remarks>
        private object ProcessArray<T>(object SuppliedObject, Array SuppliedArray)
        {
            object ReturnArray;
            object[] ObjectArray1;
            object[,] ObjectArray2;
            object[,,] ObjectArray3;
            object[,,,] ObjectArray4;
            object[,,,,] ObjectArray5;

            switch (SuppliedArray.Rank)
            {
                case 1:
                    {
                        T[] OneDimArray = (T[])SuppliedObject;
                        ObjectArray1 = new object[(SuppliedArray.GetLength(0))];
                        TL.LogMessage("ProcessArray", "Array Rank 1: " + OneDimArray.GetLength(0));
                        Array.Copy(OneDimArray, ObjectArray1, OneDimArray.LongLength);
                        ReturnArray = ObjectArray1;
                        break;
                    }
                case 2:
                    {
                        T[,] TwoDimArray = (T[,])SuppliedObject;
                        ObjectArray2 = new object[(TwoDimArray.GetLength(0)), (TwoDimArray.GetLength(1))];
                        TL.LogMessage("ProcessArray", "Array Rank 2: " + TwoDimArray.GetLength(0) + " x " + TwoDimArray.GetLength(1));
                        Array.Copy(TwoDimArray, ObjectArray2, TwoDimArray.LongLength);
                        ReturnArray = ObjectArray2;
                        break;
                    }
                case 3:
                    {
                        T[,,] ThreeDimArray = (T[,,])SuppliedObject;
                        ObjectArray3 = new object[(ThreeDimArray.GetLength(0)), (ThreeDimArray.GetLength(1)), (ThreeDimArray.GetLength(2))];
                        TL.LogMessage("ProcessArray", "Array Rank 3: " + ThreeDimArray.GetLength(0) + " x " + ThreeDimArray.GetLength(1) + " x " + ThreeDimArray.GetLength(2));
                        Array.Copy(ThreeDimArray, ObjectArray3, ThreeDimArray.LongLength);
                        ReturnArray = ObjectArray3;
                        break;
                    }
                case 4:
                    {
                        T[,,,] FourDimArray = (T[,,,])SuppliedObject;
                        ObjectArray4 = new object[(FourDimArray.GetLength(0)), (FourDimArray.GetLength(1)), (FourDimArray.GetLength(2)), (FourDimArray.GetLength(3))];
                        TL.LogMessage("ProcessArray", "Array Rank 4: " + FourDimArray.GetLength(0) + " x " + FourDimArray.GetLength(1) + " x " + FourDimArray.GetLength(2) + " x " + FourDimArray.GetLength(3));
                        Array.Copy(FourDimArray, ObjectArray4, FourDimArray.LongLength);
                        ReturnArray = ObjectArray4;
                        break;
                    }
                case 5:
                    {
                        T[,,,,] FiveDimArray = (T[,,,,])SuppliedObject;
                        ObjectArray5 = new object[(FiveDimArray.GetLength(0)), (FiveDimArray.GetLength(1)), (FiveDimArray.GetLength(2)), (FiveDimArray.GetLength(3)), (FiveDimArray.GetLength(4))];
                        TL.LogMessage("ProcessArray", "Array Rank 5: " + FiveDimArray.GetLength(0) + " x " + FiveDimArray.GetLength(1) + " x " + FiveDimArray.GetLength(2) + " x " + FiveDimArray.GetLength(3) + " x " + FiveDimArray.GetLength(4));
                        Array.Copy(FiveDimArray, ObjectArray5, FiveDimArray.LongLength);
                        ReturnArray = ObjectArray5;
                        break;
                    }

                default:
                    {
                        TL.LogMessage("ProcessArrayOfType", "Array rank is outside the range 1..5: " + SuppliedArray.Rank + ", throwing exception");
                        throw new InvalidValueException("Array rank is outside the range 1..5: " + SuppliedArray.Rank);
                    }
            }

            return ReturnArray;
        }

        #endregion

        #region Platform version properties

        /// <summary>
        /// Platform major version number
        /// </summary>
        /// <value>Platform major version number</value>
        /// <returns>Integer version number</returns>
        /// <remarks></remarks>
        public int MajorVersion
        {
            get
            {
                Version AscomVersion;
                AscomVersion = new Version(myProfile.GetProfile(PLATFORM_INFORMATION_SUBKEY, PLATFORM_VERSION, PLATFORM_VERSION_DEFAULT_BAD_VALUE));
                TL.LogMessage("MajorVersion Get", AscomVersion.Major.ToString());
                return AscomVersion.Major;
            }
        }

        /// <summary>
        /// Platform minor version number
        /// </summary>
        /// <value>Platform minor version number</value>
        /// <returns>Integer version number</returns>
        /// <remarks></remarks>
        public int MinorVersion
        {
            get
            {
                Version AscomVersion;
                AscomVersion = new Version(myProfile.GetProfile(PLATFORM_INFORMATION_SUBKEY, PLATFORM_VERSION, PLATFORM_VERSION_DEFAULT_BAD_VALUE));
                TL.LogMessage("MinorVersion Get", AscomVersion.Minor.ToString());
                return AscomVersion.Minor;
            }
        }

        /// <summary>
        /// Platform service pack number
        /// </summary>
        /// <value>Platform service pack number</value>
        /// <returns>Integer service pack number</returns>
        /// <remarks></remarks>
        public int ServicePack
        {
            get
            {
                Version AscomVersion;
                AscomVersion = new Version(myProfile.GetProfile(PLATFORM_INFORMATION_SUBKEY, PLATFORM_VERSION, PLATFORM_VERSION_DEFAULT_BAD_VALUE));
                TL.LogMessage("ServicePack Get", AscomVersion.Build.ToString());
                return AscomVersion.Build;
            }
        }

        /// <summary>
        /// Platform build number
        /// </summary>
        /// <value>Platform build number</value>
        /// <returns>Integer build number</returns>
        /// <remarks></remarks>
        public int BuildNumber
        {
            get
            {
                Version AscomVersion;
                AscomVersion = new Version(myProfile.GetProfile(PLATFORM_INFORMATION_SUBKEY, PLATFORM_VERSION, PLATFORM_VERSION_DEFAULT_BAD_VALUE));
                TL.LogMessage("BuildNumber Get", AscomVersion.Revision.ToString());
                return AscomVersion.Revision;
            }
        }

        #endregion

        #region Time Support Functions
        // ------------------------------------------------------------------------
        // FUNCTION    : GetTimeZoneOffset()
        // 
        // PURPOSE     : Return the time zone offset in hours, such that
        // UTC - local + offset
        // ------------------------------------------------------------------------
        private double GetTimeZoneOffset()
        {
            // 6.1 SP2 - Revised to return .TotalHours value instead of .Hours value so that fractional time zone values are returned accurately.
            return -TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
        }
        // ------------------------------------------------------------------------
        // FUNCTION    : GetTimeZoneName()
        // 
        // PURPOSE     : Use GetTimeZoneInfo to determine the time zone for this
        // system, including daylight effects, if any.
        // ------------------------------------------------------------------------
        private string GetTimeZoneName()
        {
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
            {
                return TimeZone.CurrentTimeZone.DaylightName;
            }
            else
            {
                return TimeZone.CurrentTimeZone.StandardName;
            }
        }
        // ------------------------------------------------------------------------
        // FUNCTION    : CvtUTC()
        // 
        // PURPOSE     : Returns a UTC Date for the given local Date
        // ------------------------------------------------------------------------
        private DateTime CvtUTC(ref DateTime d)
        {
            return Conversions.ToDate(DateTime.FromOADate(d.ToOADate() + GetTimeZoneOffset() / 24.0d));
        }

        // ------------------------------------------------------------------------
        // FUNCTION    : CvtLocal()
        // 
        // PURPOSE     : Returns a Local Date for the given UTC Date
        // ------------------------------------------------------------------------
        private DateTime CvtLocal(ref DateTime d)
        {
            return Conversions.ToDate(DateTime.FromOADate(d.ToOADate() - GetTimeZoneOffset() / 24.0d));
        }
        #endregion

        #region COM Registration
        /// <summary>
        /// Function that is called by RegAsm when the assembly is registered for COM
        /// </summary>
        /// <remarks>This is necessary to ensure that the mscoree.dll can be found when the SetSearchDirectories function has been called in an application e.g. by Inno installer post v5.5.9</remarks>
        [ComRegisterFunction]
        private static void COMRegisterActions(Type typeToRegister)
        {
            COMRegister(typeToRegister);
        }

        /// <summary>
        /// Function that is called by RegAsm when the assembly is registered for COM
        /// </summary>
        [ComUnregisterFunction]
        private static void COMUnRegisterActions(Type typeToRegister)
        {
            // No action on unregister, this method has been included to remove a compiler warning
        }

        #endregion

    }
}