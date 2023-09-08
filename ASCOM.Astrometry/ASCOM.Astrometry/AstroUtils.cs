using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static ASCOM.Astrometry.GlobalItems;
using ASCOM.Utilities;
using Microsoft.VisualBasic;

namespace ASCOM.Astrometry.AstroUtils
{
    /// <summary>
    /// Class providing a suite of tested astronomy support functions to save develpment effort and provide consistant behaviour.
    /// </summary>
    /// <remarks>
    /// A number of these routines are provided to support migration from the Astro32.dll. Unlike Astro32, these routines will work in 
    /// both 32bit and 64bit applications.
    /// </remarks>
    [Guid("5679F94A-D4D1-40D3-A0F8-7CE61100A691")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class AstroUtils : IAstroUtils, IDisposable
    {

        private TraceLogger TL;
        private Util Utl;
        private NOVAS.NOVAS31 Nov31;
        private RegistryAccess RegAccess;
        private SOFA.SOFA Sofa;
        private EarthRotationParameters Parameters;
        private bool DisposeTraceLogger;
        private bool disposedValue; // To detect redundant Dispose calls

        internal struct BodyInfo
        {
            public double Altitude;
            public double Distance;
            public double Radius;
        }

        #region New and IDisposable Support
        public AstroUtils() // Create own trace logger
        {
            TL = new TraceLogger("", "AstroUtils");
            TL.Enabled = Utilities.Global.GetBool(Utilities.Global.ASTROUTILS_TRACE, Utilities.Global.ASTROUTILS_TRACE_DEFAULT); // Get enabled / disabled state from the user registry
            DisposeTraceLogger = true;
            InitialiseAstroUtils();
        }

        internal AstroUtils(TraceLogger SuppliedTL) // Use the supplied tracelogger
        {
            TL = SuppliedTL;
            DisposeTraceLogger = false;
            InitialiseAstroUtils();
        }

        private void InitialiseAstroUtils()
        {
            Utl = new Util();
            Nov31 = new NOVAS.NOVAS31();
            Sofa = new SOFA.SOFA();
            RegAccess = new RegistryAccess();
            TL.LogMessage("New", "AstroUtils created Utilities component OK");
            Parameters = new EarthRotationParameters(TL);
            TL.LogMessage("New", "AstroUtils created Earth Rotation Parameters object OK");
            TL.LogMessage("New", "Finished initialisation OK");
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (TL is not null & DisposeTraceLogger) // Only dispose of the tracelogger if we created it
                    {
                        TL.Enabled = false;
                        TL.Dispose();
                        TL = null;
                    }
                    if (Utl is not null)
                    {
                        Utl.Dispose();
                        Utl = null;
                    }
                }
                if (Nov31 is not null)
                {
                    Nov31.Dispose();
                    Nov31 = null;
                }
                if (RegAccess is not null)
                {
                    RegAccess.Dispose();
                    RegAccess = (RegistryAccess)null;
                }
                if (Sofa is not null)
                {
                    Sofa.Dispose();
                    Sofa = null;
                }
            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// Releases all resources owned by the AstroUtils component and readies it for disposal
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AstroUtils()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }
        #endregion

        /// <summary>
        /// Flexible routine to range a number into a given range between a lower and an higher bound.
        /// </summary>
        /// <param name="Value">Value to be ranged</param>
        /// <param name="LowerBound">Lowest value of the range</param>
        /// <param name="LowerEqual">Boolean flag indicating whether the ranged value can have the lower bound value</param>
        /// <param name="UpperBound">Highest value of the range</param>
        /// <param name="UpperEqual">Boolean flag indicating whether the ranged value can have the upper bound value</param>
        /// <returns>The ranged nunmber as a double</returns>
        /// <exception cref="ASCOM.InvalidValueException">Thrown if the lower bound is greater than the upper bound.</exception>
        /// <exception cref="ASCOM.InvalidValueException">Thrown if LowerEqual and UpperEqual are both false and the ranged value equals
        /// one of these values. This is impossible to handle as the algorithm will always violate one of the rules!</exception>
        /// <remarks>
        /// UpperEqual and LowerEqual switches control whether the ranged value can be equal to either the upper and lower bounds. So, 
        /// to range an hour angle into the range 0 to 23.999999.. hours, use this call: 
        /// <code>RangedValue = Range(InputValue, 0.0, True, 24.0, False)</code>
        /// <para>The input value will be returned in the range where 0.0 is an allowable value and 24.0 is not i.e. in the range 0..23.999999..</para>
        /// <para>It is not permissible for both LowerEqual and UpperEqual to be false because it will not be possible to return a value that is exactly equal 
        /// to either lower or upper bounds. An exception is thrown if this scenario is requested.</para>
        /// </remarks>
        public double Range(double Value, double LowerBound, bool LowerEqual, double UpperBound, bool UpperEqual)
        {
            double ModuloValue;
            if (LowerBound >= UpperBound)
                throw new InvalidValueException("Range", "LowerBound is >= UpperBound", "LowerBound must be less than UpperBound");

            ModuloValue = UpperBound - LowerBound;

            if (LowerEqual)
            {
                if (UpperEqual) // Lowest >= Highest <=
                {
                    do
                    {
                        if (Value < LowerBound)
                            Value += ModuloValue;
                        if (Value > UpperBound)
                            Value -= ModuloValue;
                    }
                    while (!(Value >= LowerBound & Value <= UpperBound));
                }
                else // Lowest >= Highest <
                {
                    do
                    {
                        if (Value < LowerBound)
                            Value += ModuloValue;
                        if (Value >= UpperBound)
                            Value -= ModuloValue;
                    }
                    while (!(Value >= LowerBound & Value < UpperBound));
                }
            }
            else if (UpperEqual) // Lowest > Highest<=
            {
                do
                {
                    if (Value <= LowerBound)
                        Value += ModuloValue;
                    if (Value > UpperBound)
                        Value -= ModuloValue;
                }
                while (!(Value > LowerBound & Value <= UpperBound));
            }
            else // Lowest > Highest <
            {
                if (Value == LowerBound)
                    throw new InvalidValueException("Range", "The supplied value equals the LowerBound. This can not be ranged when LowerEqual and UpperEqual are both false ", "LowerBound > Value < UpperBound");
                if (Value == UpperBound)
                    throw new InvalidValueException("Range", "The supplied value equals the UpperBound. This can not be ranged when LowerEqual and UpperEqual are both false ", "LowerBound > Value < UpperBound");
                do
                {
                    if (Value <= LowerBound)
                        Value += ModuloValue;
                    if (Value >= UpperBound)
                        Value -= ModuloValue;
                }
                while (!(Value > LowerBound & Value < UpperBound));
            }
            return Value;
        }

        /// <summary>
        /// Conditions an hour angle to be in the range -12.0 to +12.0 by adding or subtracting 24.0 hours
        /// </summary>
        /// <param name="HA">Hour angle to condition</param>
        /// <returns>Hour angle in the range -12.0 to +12.0</returns>
        /// <remarks></remarks>
        public double ConditionHA(double HA)
        {
            double ReturnValue;

            ReturnValue = Range(HA, -12.0d, true, +12.0d, true);
            TL.LogMessage("ConditionHA", "Conditioned HA: " + Utl.HoursToHMS(HA, ":", ":", "", 3) + " to: " + Utl.HoursToHMS(ReturnValue, ":", ":", "", 3));

            return ReturnValue;
        }

        /// <summary>
        /// Conditions a Right Ascension value to be in the range 0 to 23.999999.. hours 
        /// </summary>
        /// <param name="RA">Right ascension to be conditioned</param>
        /// <returns>Right ascension in the range 0 to 23.999999...</returns>
        /// <remarks></remarks>
        public double ConditionRA(double RA)
        {
            double ReturnValue;

            ReturnValue = Range(RA, 0.0d, true, 24.0d, false);
            TL.LogMessage("ConditionRA", "Conditioned RA: " + Utl.HoursToHMS(RA, ":", ":", "", 3) + " to: " + Utl.HoursToHMS(ReturnValue, ":", ":", "", 3));

            return ReturnValue;
        }

        /// <summary>
        /// Returns the current DeltaT value in seconds
        /// </summary>
        /// <returns>DeltaT in seconds</returns>
        /// <remarks>DeltaT is the difference between terrestrial time and the UT1 variant of universal time. ie.e TT = UT1 + DeltaT</remarks>
        public double DeltaT()
        {
            DateTime CurrentUTCDate;
            double JDUtc, DeltaTValue;

            CurrentUTCDate = DateTime.UtcNow;
            JDUtc = UTCJulianDate(CurrentUTCDate);
            DeltaTValue = Parameters.DeltaT();
            TL.LogMessage("DeltaT", string.Format("Returning DeltaT: {0} at Julian date: {1} ({2})", DeltaTValue, JDUtc, CurrentUTCDate.ToString("dddd dd MMMM yyyy HH:mm:ss.fff")));

            return DeltaTValue;
        }

        /// <summary>
        /// Current Julian date based on the UTC time scale
        /// </summary>
        /// <value>Julian day</value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double JulianDateUtc
        {
            get
            {
                DateTime CurrentUTCDate;
                double JDUtc;

                CurrentUTCDate = DateTime.UtcNow;
                JDUtc = UTCJulianDate(CurrentUTCDate);
                TL.LogMessage("JulianDateUtc", string.Format("Returning Julian date (UTC): {0} at UTC: {1}", JDUtc, CurrentUTCDate.ToString("dddd dd MMMM yyyy HH:mm:ss.fff")));

                return JDUtc;
            }
        }

        /// <summary>
        /// Current Julian date based on the terrestrial time (TT) time scale
        /// </summary>
        /// <param name="DeltaUT1">Current value for Delta-UT1, the difference between UTC and UT1; always in the range -0.9 to +0.9 seconds.
        /// Use 0.0 to calculate TT through TAI. Delta-UT1 varies irregularly throughout the year.</param>
        /// <returns>Double - Julian date on the UT1 timescale.</returns>
        /// <remarks>When Delta-UT1 is provided, Terrestrial time is calculated as TT = UTC + DeltaUT1 + DeltaT. Otherwise, when Delta-UT1 is 0.0, 
        /// TT is calculated as TT = UTC + ΔAT + 32.184s, where ΔAT is the current number of leap seconds applied to UTC (34 at April 2012, with 
        /// the 35th being added at the end of June 2012). The resulting TT value is then converted to a Julian date and returned.
        /// <para>Forecast values of Delta-UT1 are published by IERS Bulletin A at http://maia.usno.navy.mil/ser7/ser7.dat
        /// </para></remarks>
        public double JulianDateTT(double DeltaUT1)
        {
            DateTime UTCDate, UT1Date, TTDate;
            double JD;
            TimeSpan DeltaTTimespan, DeltaUT1Timespan;

            if (DeltaUT1 < -0.9d | DeltaUT1 > 0.9d)
                throw new InvalidValueException("JulianDateUT1", DeltaUT1.ToString(), "-0.9 to +0.9");

            UTCDate = DateTime.UtcNow;

            if (DeltaUT1 != 0.0d) // A specific value has been provided so use it and get to TT via DeltaT
            {
                // Compute as TT = UTC + DeltaUT1 + DeltaT
                DeltaTTimespan = TimeSpan.FromSeconds(DeltaT()); // Get DeltaT as a timesapn
                DeltaUT1Timespan = TimeSpan.FromSeconds(DeltaUT1); // Convert DeltaUT to a timesapn
                UT1Date = UTCDate.Add(DeltaUT1Timespan); // Add delta-ut to UTC to yield UT1
                TTDate = UT1Date.Add(DeltaTTimespan); // Add delta-t to UT1 to yield TT
            }
            else // No value provided so get to TT through TAI
            {
                // Computation method TT = UTC + ΔAT + 32.184s. ΔAT = 35.0 leap seconds in June 2012
                TTDate = UTCDate.Add(TimeSpan.FromSeconds(Parameters.LeapSeconds() + TT_TAI_OFFSET));
            }

            JD = Nov31.JulianDate(Convert.ToInt16(TTDate.Year), Convert.ToInt16(TTDate.Month), Convert.ToInt16(TTDate.Day), TTDate.TimeOfDay.TotalHours);
            TL.LogMessage("JulianDateTT", "Returning: " + JD + "at TT: " + TTDate.ToString("dddd dd MMMM yyyy HH:mm:ss.fff") + ", at UTC: " + UTCDate.ToString( "dddd dd MMMM yyyy HH:mm:ss.fff"));

            return JD;
        }

        /// <summary>
        /// Current Julian date based on the UT1 time scale
        /// </summary>
        /// <param name="DeltaUT1">Current value for Delta-UT1, the difference between UTC and UT1; always in the range -0.9 to +0.9 seconds.
        /// Use 0.0 if you do not know this value; it varies irregularly throughout the year.</param>
        /// <returns>Double - Julian date on the UT1 timescale.</returns>
        /// <remarks>UT1 time is calculated as UT1 = UTC + DeltaUT1 when DeltaUT1 is non zero. otherwise it is calaulcated through TAI and DeltaT.
        /// This value is then converted to a Julian date and returned.
        /// <para>When Delta-UT1 is provided, UT1 is calculated as UT1 = UTC + DeltaUT1. Otherwise, when Delta-UT1 is 0.0, 
        /// DeltaUT1 is calculated as DeltaUT1 = TT - DeltaT = UTC + ΔAT + 32.184s - DeltaT, where ΔAT is the current number of leap seconds applied 
        /// to UTC (34 at April 2012, with the 35th being added at the end of June 2012).</para>
        /// <para>Forecast values of DUT1 are published by IERS Bulletin A at http://maia.usno.navy.mil/ser7/ser7.dat
        /// </para></remarks>
        public double JulianDateUT1(double DeltaUT1)
        {
            DateTime CurrentUTCDate, UT1Date, TTDate;
            double JDUtc, JD, jd1 = default, jd2 = default, DeltaT;
            TimeSpan DeltaUT1Timespan;
            int rc;

            if (DeltaUT1 < -0.9d | DeltaUT1 > 0.9d)
                throw new InvalidValueException("JulianDateUT1", DeltaUT1.ToString(), "-0.9 to +0.9");

            CurrentUTCDate = DateTime.UtcNow;

            if (DeltaUT1 != 0.0d) // Calculate as UT1 = UTC - DeltaUT1
            {
                DeltaUT1Timespan = TimeSpan.FromSeconds(DeltaUT1); // Convert DeltaUT1 to a timesapn
                UT1Date = CurrentUTCDate.Add(DeltaUT1Timespan); // Add delta-ut1 to UTC to yield UT1
            }
            else
            {
                // Calculation UT1 = TT - DeltaT = UTC + ΔAT + 32.184s - DeltaT
                JDUtc = UTCJulianDate(CurrentUTCDate);
                // DeltaT = DeltaTCalc(JDUtc)
                DeltaT = Parameters.DeltaT();
                TTDate = CurrentUTCDate.Add(TimeSpan.FromSeconds(Parameters.LeapSeconds() + TT_TAI_OFFSET));
                UT1Date = TTDate.Subtract(TimeSpan.FromSeconds(DeltaT));
            }

            // Revised to use SOFA to calculate the Julian date
            rc = Sofa.Dtf2d("UTC", UT1Date.Year, UT1Date.Month, UT1Date.Day, UT1Date.Hour, UT1Date.Minute, UT1Date.Second + UT1Date.Millisecond / 1000.0d, ref jd1, ref jd2);
            if (rc != 0)
                TL.LogMessage("JulianDateUT1", string.Format("Bad return code from Sofa.Dtf2d: {0} for UT1 date: {1}", rc, UT1Date.ToString("dddd dd MMMM yyyy HH:mm:ss.fff")));

            JD = jd1 + jd2;
            TL.LogMessage("JulianDateUT1", string.Format("Returning Julian date: {0} at UT1: {1} and UTC: {2}", JD, UT1Date.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"), CurrentUTCDate.ToString("dddd dd MMMM yyyy HH:mm:ss.fff")));

            return JD;
        }

        /// <summary>
        /// Computes atmospheric refraction in zenith distance. 
        /// </summary>
        /// <param name="Location">Structure containing observer's location.</param>
        /// <param name="RefOption">1 ... Use 'standard' atmospheric conditions; 2 ... Use atmospheric 
        /// parameters input in the 'Location' structure.</param>
        /// <param name="ZdObs">Observed zenith distance, in degrees.</param>
        /// <returns>Unrefracted zenith distance in degrees.</returns>
        /// <remarks>This version computes approximate refraction for optical wavelengths. This function 
        /// can be used for planning observations or telescope pointing, but should not be used for the 
        /// reduction of precise observations.
        /// <para>Note: Unlike the NOVAS Refract method, Unrefract returns the unrefracted zenith distance itself rather than 
        /// the difference between the refracted and unrefracted zenith distances.</para></remarks>
        public double UnRefract(OnSurface Location, RefractionOption RefOption, double ZdObs)
        {
            int LoopCount;
            double RefractedPosition, UnrefractedPosition;

            if (ZdObs < 0.0d | ZdObs > 90.0d)
                throw new InvalidValueException("Unrefract", "Zenith distance", "0.0 to 90.0 degrees");

            LoopCount = 0;
            UnrefractedPosition = ZdObs;
            do
            {
                LoopCount += 1;
                RefractedPosition = UnrefractedPosition - Nov31.Refract(Location, RefOption, UnrefractedPosition);
                UnrefractedPosition = UnrefractedPosition + (ZdObs - RefractedPosition);
                TL.LogMessage("Unrefract", LoopCount + ": " + RefractedPosition + " " + UnrefractedPosition);
            }
            while (!(LoopCount == 20 | RefractedPosition == ZdObs));

            TL.LogMessage("Unrefract", "Final: " + LoopCount + ", Unrefracted zenith distance: " + UnrefractedPosition);
            return UnrefractedPosition;

        }

        /// <summary>
        /// Converts a calendar day, month, year to a modified Julian date
        /// </summary>
        /// <param name="Day">Integer day of ther month</param>
        /// <param name="Month">Integer month of the year</param>
        /// <param name="Year">Integer year</param>
        /// <returns>Double modified julian date</returns>
        /// <remarks></remarks>
        public double CalendarToMJD(int Day, int Month, int Year)
        {
            double JD, MJD;
            JD = Nov31.JulianDate(Convert.ToInt16(Year), Convert.ToInt16(Month), Convert.ToInt16(Day), 0.0d);
            MJD = JD - MODIFIED_JULIAN_DAY_OFFSET;
            return MJD;
        }

        /// <summary>
        /// Translates a modified Julian date to a VB ole automation date, presented as a double
        /// </summary>
        /// <param name="MJD">Modified Julian date</param>
        /// <returns>Date as a VB ole automation date</returns>
        /// <remarks></remarks>
        public double MJDToOADate(double MJD)
        {
            DateTime JulianDate;
            double JulianOADate;

            JulianDate = Utl.DateJulianToLocal(MJD + MODIFIED_JULIAN_DAY_OFFSET);
            JulianOADate = JulianDate.ToOADate();

            return JulianOADate;

        }

        /// <summary>
        /// Translates a modified Julian date to a date
        /// </summary>
        /// <param name="MJD">Modified Julian date</param>
        /// <returns>Date representing the modified Julian date</returns>
        /// <remarks></remarks>
        public DateTime MJDToDate(double MJD)
        {
            DateTime JulianDate;

            JulianDate = Utl.DateJulianToLocal(MJD + MODIFIED_JULIAN_DAY_OFFSET);

            return JulianDate;

        }

        /// <summary>
        /// Returns a modified Julian date as a string formatted acording to the supplied presentation format
        /// </summary>
        /// <param name="MJD">Mofified julian date</param>
        /// <param name="PresentationFormat">Format representation</param>
        /// <returns>Date string</returns>
        /// <exception cref="FormatException">Thrown if the provided PresentationFormat is not valid.</exception>
        /// <remarks>This expects the standard Microsoft date and time formatting characters as described 
        /// in http://msdn.microsoft.com/en-us/library/362btx8f(v=VS.90).aspx
        /// </remarks>
        public string FormatMJD(double MJD, string PresentationFormat)
        {
            DateTime MJDDate;
            string MJDDateString;
            MJDDate = MJDToDate(MJD);
            MJDDateString = MJDDate.ToString(PresentationFormat);
            return MJDDateString;
        }

        /// <summary>
        /// Proivides an estimates of DeltaUT1, the difference between UTC and UT1. DeltaUT1 = UT1 - UTC
        /// </summary>
        /// <param name="JulianDate">Julian date when DeltaUT is required</param>
        /// <returns>Double DeltaUT in seconds</returns>
        /// <remarks>DeltaUT varies only slowly, so the Julian date can be based on UTC, UT1 or Terrestrial Time.</remarks>
        public double DeltaUT(double JulianDate)
        {
            double deltaUT1;

            deltaUT1 = Parameters.DeltaUT1(JulianDate);
            if (!DisposeTraceLogger)
                TL.LogMessage("AstroUtils.DeltaUT", string.Format("DeltaUT1 = {0} on Julian date {1}", deltaUT1.ToString("+0.000;-0.000;0.000"), JulianDate)); // Only log this message for internal applications

            return deltaUT1;
        }

        double IAstroUtils.DeltaUT1(double JulianDate) => DeltaUT(JulianDate);

        /// <summary>
        /// Returns a Julian date as a string formatted according to the supplied presentation format
        /// </summary>
        /// <param name="JD">Julian date</param>
        /// <param name="PresentationFormat">Format representation</param>
        /// <returns>Date as a string</returns>
        /// <remarks>This expects the standard Microsoft date and time formatting characters as described 
        /// in http://msdn.microsoft.com/en-us/library/362btx8f(v=VS.90).aspx
        /// </remarks>
        public string FormatJD(double JD, string PresentationFormat)
        {
            double DaysSinceJ2000;
            DateTime J2000Date, ActualDate;
            string Retval;

            TL.LogMessage("FormatJD", "JD, PresentationFormat: " + JD + " " + PresentationFormat);

            DaysSinceJ2000 = JD - J2000BASE;

            // J2000 corresponds to 2000 Jan 1st 12.00 midday
            J2000Date = new DateTime(2000, 1, 1, 12, 0, 0);
            ActualDate = J2000Date.AddDays(DaysSinceJ2000);
            TL.LogMessage("FormatJD", "  DaysSinceJ2000, J2000Date, ActualDate: " + DaysSinceJ2000 + " " + J2000Date.ToString() + " " + ActualDate.ToString());

            Retval = ActualDate.ToString(PresentationFormat);
            TL.LogMessage("FormatJD", "  Result: " + Retval);

            return Retval;
        }

        /// <summary>
        /// Sets or returns the number of leap seconds used in ASCOM Astrometry functions
        /// </summary>
        /// <value>Integer number of seconds</value>
        /// <returns>Current number of leap seconds</returns>
        /// <remarks>The property value is stored in the ASCOM Profile under the name \Astrometry\Leap Seconds. Any change made to this property 
        /// will be persisted to the ASCOM Profile store and will be immediately availble to this and all future instances of AstroUtils.
        /// <para>The current value and any announced but not yet actioned change are listed 
        /// here: ftp://hpiers.obspm.fr/iers/bul/bulc/bulletinc.dat</para> </remarks>
        public int LeapSeconds
        {
            get
            {
                double leapSecondsValue;
                leapSecondsValue = Parameters.LeapSeconds();
                if (!DisposeTraceLogger)
                    TL.LogMessage("AstroUtils.LeapSeconds", string.Format("Current leap seconds = {0}", leapSecondsValue)); // Only log this message for internal applications
                return (int)Math.Round(leapSecondsValue);
            }
            set
            {
                Parameters.ManualLeapSeconds = value;
            }
        }

        /// <summary>
        /// Function that returns a list of rise and set events of a particular type that occur on a particular day at a given latitude, longitude and time zone
        /// </summary>
        /// <param name="TypeofEvent">Type of event e.g. Sunrise or Astronomical twilight</param>
        /// <param name="Day">Integer Day number</param>
        /// <param name="Month">Integer Month number</param>
        /// <param name="Year">Integer Year number</param>
        /// <param name="SiteLatitude">Site latitude</param>
        /// <param name="SiteLongitude">Site longitude (West of Greenwich is negative)</param>
        /// <param name="SiteTimeZone">Site time zone offset (West of Greenwich is negative)</param>
        /// <returns>An arraylist of event information (see Remarks for arraylist structure).
        /// </returns>
        /// <exception cref="ASCOM.InvalidValueException">If the combination of day, month and year is invalid e.g. 31st September.</exception>
        /// <remarks>
        /// <para>The definitions of sunrise, sunset and the various twilights that are used in this method are taken from the 
        /// <a href="http://aa.usno.navy.mil/faq/docs/RST_defs.php">US Naval Observatory Definitions</a>.
        /// </para>
        /// <para>The dynamics of the sun, Earth and Moon can result at some latitudes in days where there may be no, 1 or 2 rise or set events during 
        /// a 24 hour period; in consequence, results are returned in the flexible form of arraylist.</para>
        /// <para>The returned zero based arraylist has the following values:
        /// <list type="Bullet">
        /// <item>Arraylist(0)                              - Boolean - True if the body is above the event limit at midnight (the beginning of the 24 hour day), false if it is below the event limit</item>
        /// <item>Arraylist(1)                              - Integer - Number of rise events in this 24 hour period</item>
        /// <item>Arraylist(2)                              - Integer - Number of set events in this 24 hour period</item>
        /// <item>Arraylist(3) onwards                      - Double  - Values of rise events in hours </item>
        /// <item>Arraylist(3 + NumberOfRiseEvents) onwards - Double  - Values of set events in hours </item>
        /// </list></para>
        /// <para>If the number of rise events is zero the first double value will be the first set event. If the numbers of both rise and set events
        /// are zero, there will be no double values and the arraylist will just contain elements 0, 1 and 2, the above/below horizon flag and the integer count values.</para>
        /// <para>The algorithm employed in this method is taken from Astronomy on the Personal Computer (Montenbruck and Pfleger) pp 46..56, 
        /// Springer Fourth Edition 2000, Fourth Printing 2009. The day is divided into twelve two hour intervals and a quadratic equation is fitted
        /// to the altitudes at the beginning, middle and end of each interval. The resulting equation coefficients are then processed to determine 
        /// the number of roots within the interval (each of which corresponds to a rise or set event) and their sense (rise or set). 
        /// These results are are then aggregated over the day and the resultant list of values returned as the function result.
        /// </para>
        /// <para>High precision ephemeredes for the Sun, Moon and Earth and other planets from the JPL DE421 series are employed as delivered by the 
        /// ASCOM NOVAS 3.1 component rather than using the lower precision ephemeredes employed by Montenbruck and Pfleger.
        /// </para>
        /// <para><b>Accuracy</b> Whole year almanacs for Sunrise/Sunset, Moonrise/Moonset and the various twilights every 5 degrees from the 
        /// North pole to the South Pole at a variety of longitudes, timezones and dates have been compared to data from
        /// the <a href="http://aa.usno.navy.mil/data/docs/RS_OneYear.php">US Naval Observatory Astronomical Data</a> web site. The RMS error has been found to be 
        /// better than 0.5 minute over the latitude range 80 degrees North to 80 degrees South and better than 5 minutes from 80 degrees to the relevant pole.
        /// Most returned values are within 1 minute of the USNO values although some very infrequent grazing event times at lattiudes from 67 to 90 degrees North and South can be up to 
        /// 10 minutes different.
        /// </para>
        /// <para>An Almanac program that creates a year's worth of information for a given event, lattitude, longitude and timezone is included in the 
        /// developer code examples elsewhere in this help file. This creates an output file with an almost identical format to that used by the USNO web site 
        /// and allows comprehensive checking of acccuracy for a given set of parameters.</para>
        /// </remarks>
        public ArrayList EventTimes(EventType TypeofEvent, int Day, int Month, int Year, double SiteLatitude, double SiteLongitude, double SiteTimeZone)
        {
            bool DoesRise, DoesSet, AboveHorizon = default;
            double CentreTime, AltitiudeMinus1, Altitiude0, AltitiudePlus1, a, b, c, XSymmetry, YExtreme, Discriminant, RefractionCorrection;
            double DeltaX, Zero1, Zero2, JD;
            int NZeros;
            var Observer = new OnSurface();
            var Retval = new ArrayList();
            List<double> BodyRises = new List<double>(), BodySets = new List<double>();
            BodyInfo BodyInfoMinus1, BodyInfo0, BodyInfoPlus1;
            DateTime TestDate;

            DoesRise = false;
            DoesSet = false;

            try
            {
                TestDate = DateTime.Parse(Month + "/" + Day + "/" + Year, System.Globalization.CultureInfo.InvariantCulture); // Test whether this is a valid date e.g is not the 31st of February
            }
            catch (FormatException ex) // Catch case where day exceeds the maximum number of days in the month
            {
                throw new InvalidValueException("Day or Month", Day.ToString() + " " + Month.ToString() + " " + Year.ToString(), "Day must not exceed the number of days in the month");
            }
            catch (Exception ex) // Throw all other exceptions as they are are received
            {
                TL.LogMessageCrLf("EventTimes", ex.ToString());
                throw;
            }

            // Calculate Julian date in the local timezone
            JD = Nov31.JulianDate((short)Year, (short)Month, (short)Day, 0.0d) - SiteTimeZone / 24.0d;

            // Initialise observer structure and calculate the refraction at the hozrizon
            Observer.Latitude = SiteLatitude;
            Observer.Longitude = SiteLongitude;
            RefractionCorrection = Nov31.Refract(Observer, RefractionOption.StandardRefraction, 90.0d);

            // Iterate over the day in two hour periods

            // Start at 01:00 as the centre time i.e. then time range will be 00:00 to 02:00
            CentreTime = 1.0d;

            do
            {
                // Calculate body positional information
                BodyInfoMinus1 = BodyAltitude(TypeofEvent, JD, CentreTime - 1d, SiteLatitude, SiteLongitude);
                BodyInfo0 = BodyAltitude(TypeofEvent, JD, CentreTime, SiteLatitude, SiteLongitude);
                BodyInfoPlus1 = BodyAltitude(TypeofEvent, JD, CentreTime + 1d, SiteLatitude, SiteLongitude);

                // Correct alititude for body's apparent size, parallax, required distance below horizon and refraction
                switch (TypeofEvent)
                {
                    case EventType.MoonRiseMoonSet:
                        {
                            // Parallax and apparent size are dynamically calculated for the Moon because it is so close and does not transcribe a circular orbit
                            AltitiudeMinus1 = BodyInfoMinus1.Altitude - EARTH_RADIUS * RAD2DEG / BodyInfoMinus1.Distance + BodyInfoMinus1.Radius * RAD2DEG / BodyInfoMinus1.Distance + RefractionCorrection;
                            Altitiude0 = BodyInfo0.Altitude - EARTH_RADIUS * RAD2DEG / BodyInfo0.Distance + BodyInfo0.Radius * RAD2DEG / BodyInfo0.Distance + RefractionCorrection;
                            AltitiudePlus1 = BodyInfoPlus1.Altitude - EARTH_RADIUS * RAD2DEG / BodyInfoPlus1.Distance + BodyInfoPlus1.Radius * RAD2DEG / BodyInfoPlus1.Distance + RefractionCorrection;
                            break;
                        }
                    case EventType.SunRiseSunset:
                        {
                            AltitiudeMinus1 = BodyInfoMinus1.Altitude - SUN_RISE;
                            Altitiude0 = BodyInfo0.Altitude - SUN_RISE;
                            AltitiudePlus1 = BodyInfoPlus1.Altitude - SUN_RISE;
                            break;
                        }
                    case EventType.CivilTwilight:
                        {
                            AltitiudeMinus1 = BodyInfoMinus1.Altitude - CIVIL_TWILIGHT;
                            Altitiude0 = BodyInfo0.Altitude - CIVIL_TWILIGHT;
                            AltitiudePlus1 = BodyInfoPlus1.Altitude - CIVIL_TWILIGHT;
                            break;
                        }
                    case EventType.NauticalTwilight:
                        {
                            AltitiudeMinus1 = BodyInfoMinus1.Altitude - NAUTICAL_TWILIGHT;
                            Altitiude0 = BodyInfo0.Altitude - NAUTICAL_TWILIGHT;
                            AltitiudePlus1 = BodyInfoPlus1.Altitude - NAUTICAL_TWILIGHT;
                            break;
                        }
                    case EventType.AmateurAstronomicalTwilight:
                        {
                            AltitiudeMinus1 = BodyInfoMinus1.Altitude - AMATEUR_ASRONOMICAL_TWILIGHT;
                            Altitiude0 = BodyInfo0.Altitude - AMATEUR_ASRONOMICAL_TWILIGHT;
                            AltitiudePlus1 = BodyInfoPlus1.Altitude - AMATEUR_ASRONOMICAL_TWILIGHT;
                            break;
                        }
                    case EventType.AstronomicalTwilight:
                        {
                            AltitiudeMinus1 = BodyInfoMinus1.Altitude - ASTRONOMICAL_TWILIGHT;
                            Altitiude0 = BodyInfo0.Altitude - ASTRONOMICAL_TWILIGHT;
                            AltitiudePlus1 = BodyInfoPlus1.Altitude - ASTRONOMICAL_TWILIGHT; // Planets so correct for radius of plant and refraction
                            break;
                        }

                    default:
                        {
                            AltitiudeMinus1 = BodyInfoMinus1.Altitude + RefractionCorrection + RAD2DEG * BodyInfo0.Radius / BodyInfo0.Distance;
                            Altitiude0 = BodyInfo0.Altitude + RefractionCorrection + RAD2DEG * BodyInfo0.Radius / BodyInfo0.Distance;
                            AltitiudePlus1 = BodyInfoPlus1.Altitude + RefractionCorrection + RAD2DEG * BodyInfo0.Radius / BodyInfo0.Distance;
                            break;
                        }
                }

                if (CentreTime == 1.0d)
                {
                    if (AltitiudeMinus1 < 0d)
                    {
                        AboveHorizon = false;
                    }
                    else
                    {
                        AboveHorizon = true;
                    }
                }

                // Assess quadratic equation
                c = Altitiude0;
                b = 0.5d * (AltitiudePlus1 - AltitiudeMinus1);
                a = 0.5d * (AltitiudePlus1 + AltitiudeMinus1) - Altitiude0;

                XSymmetry = -b / (2.0d * a);
                YExtreme = (a * XSymmetry + b) * XSymmetry + c;
                Discriminant = b * b - 4.0d * a * c;

                DeltaX = double.NaN;
                Zero1 = double.NaN;
                Zero2 = double.NaN;
                NZeros = 0;

                if (Discriminant > 0.0d)                 // there are zeros
                {
                    DeltaX = 0.5d * Math.Sqrt(Discriminant) / Math.Abs(a);
                    Zero1 = XSymmetry - DeltaX;
                    Zero2 = XSymmetry + DeltaX;
                    if (Math.Abs(Zero1) <= 1.0d)
                        NZeros = NZeros + 1; // This zero is in interval
                    if (Math.Abs(Zero2) <= 1.0d)
                        NZeros = NZeros + 1; // This zero is in interval

                    if (Zero1 < -1.0d)
                        Zero1 = Zero2;
                }

                switch (NZeros)
                {
                    // cases depend on values of discriminant - inner part of STEP 4
                    case 0: // nothing  - go to next time slot
                        {
                            break;
                        }
                    case 1:                      // simple rise / set event
                        {
                            if (AltitiudeMinus1 < 0.0d)       // The body is set at start of event so this must be a rising event
                            {
                                DoesRise = true;
                                BodyRises.Add(CentreTime + Zero1);
                            }
                            else                    // must be setting
                            {
                                DoesSet = true;
                                BodySets.Add(CentreTime + Zero1);
                            }

                            break;
                        }
                    case 2:                      // rises and sets within interval
                        {
                            if (AltitiudeMinus1 < 0.0d) // The body is set at start of event so it must rise first then set
                            {
                                BodyRises.Add(CentreTime + Zero1);
                                BodySets.Add(CentreTime + Zero2);
                            }
                            else                    // The body is risen at the start of the event so it must set first then rise
                            {
                                BodyRises.Add(CentreTime + Zero2);
                                BodySets.Add(CentreTime + Zero1);
                            }
                            DoesRise = true;
                            DoesSet = true;
                            break;
                        }
                        // Zero2 = 1
                }
                CentreTime += 2.0d; // Increment by 2 hours to get the next 2 hour slot in the day
            }

            while (!(DoesRise & DoesSet & Math.Abs(SiteLatitude) < 60.0d | CentreTime == 25.0d));

            Retval.Add(AboveHorizon); // Add above horizon at midnight flag
            Retval.Add(BodyRises.Count); // Add the number of bodyrises
            Retval.Add(BodySets.Count); // Add the number of bodysets

            foreach (double BodyRise in BodyRises) // Add the list of moonrises
                Retval.Add(BodyRise);

            foreach (double BodySet in BodySets) // Add the list of moonsets
                Retval.Add(BodySet);

            return Retval;
        }

        /// <summary>
        /// Returns the altitude of the body given the input parameters
        /// </summary>
        /// <param name="TypeOfEvent">Type of event to be calaculated</param>
        /// <param name="JD">UTC Julian date</param>
        /// <param name="Hour">Hour of Julian day</param>
        /// <param name="Latitude">Site Latitude</param>
        /// <param name="Longitude">Site Longitude</param>
        /// <returns>The altitude of the body (degrees)</returns>
        /// <remarks></remarks>
        private BodyInfo BodyAltitude(EventType TypeOfEvent, double JD, double Hour, double Latitude, double Longitude)
        {
            double Instant, Tau, Gmst = default, DeltaT;
            short rc;
            var Obj3 = new Object3();
            var Location = new OnSurface();
            var Cat = new CatEntry3();
            var SkyPosition = new SkyPos();
            var Obs = new Observer();
            var Retval = new BodyInfo();

            Instant = JD + Hour / 24.0d; // Add the hour to the whole Julian day number
            // DeltaT = DeltaTCalc(JD)
            DeltaT = Parameters.DeltaT(JD);

            switch (TypeOfEvent)
            {
                case EventType.MercuryRiseSet:
                    {
                        Obj3.Name = "Mercury";
                        Obj3.Number = Body.Mercury;
                        break;
                    }
                case EventType.VenusRiseSet:
                    {
                        Obj3.Name = "Venus";
                        Obj3.Number = Body.Venus;
                        break;
                    }
                case EventType.MarsRiseSet:
                    {
                        Obj3.Name = "Mars";
                        Obj3.Number = Body.Mars;
                        break;
                    }
                case EventType.JupiterRiseSet:
                    {
                        Obj3.Name = "Jupiter";
                        Obj3.Number = Body.Jupiter;
                        break;
                    }
                case EventType.SaturnRiseSet:
                    {
                        Obj3.Name = "Saturn";
                        Obj3.Number = Body.Saturn;
                        break;
                    }
                case EventType.UranusRiseSet:
                    {
                        Obj3.Name = "Uranus";
                        Obj3.Number = Body.Uranus;
                        break;
                    }
                case EventType.NeptuneRiseSet:
                    {
                        Obj3.Name = "Neptune";
                        Obj3.Number = Body.Neptune;
                        break;
                    }
                case EventType.PlutoRiseSet:
                    {
                        Obj3.Name = "Pluto";
                        Obj3.Number = Body.Pluto;
                        break;
                    }
                case EventType.MoonRiseMoonSet:
                    {
                        Obj3.Name = "Moon";
                        Obj3.Number = Body.Moon;
                        break;
                    }
                case EventType.SunRiseSunset:
                case EventType.AmateurAstronomicalTwilight:
                case EventType.AstronomicalTwilight:
                case EventType.CivilTwilight:
                case EventType.NauticalTwilight:
                    {
                        Obj3.Name = "Sun";
                        Obj3.Number = Body.Sun;
                        break;
                    }

                default:
                    {
                        throw new InvalidValueException("TypeOfEvent", TypeOfEvent.ToString(), "Unknown type of event");
                    }
            }

            Obj3.Star = Cat;
            Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

            Obs.OnSurf = Location;
            Obs.Where = ObserverLocation.EarthGeoCenter;

            Nov31.Place(Instant + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref SkyPosition);
            Retval.Distance = SkyPosition.Dis * AU2KILOMETRE; // Distance is in AU so save it in km

            rc = Nov31.SiderealTime(Instant, 0.0d, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, ref Gmst);

            Tau = HOURS2DEG * (Range(Gmst + Longitude * DEG2HOURS, 0d, true, 24.0d, false) - SkyPosition.RA); // East longitude is  positive
            Retval.Altitude = Math.Asin(Math.Sin(Latitude * DEG2RAD) * Math.Sin(SkyPosition.Dec * DEG2RAD) + Math.Cos(Latitude * DEG2RAD) * Math.Cos(SkyPosition.Dec * DEG2RAD) * Math.Cos(Tau * DEG2RAD)) * RAD2DEG;

            switch (TypeOfEvent)
            {
                case EventType.MercuryRiseSet:
                    {
                        Retval.Radius = MERCURY_RADIUS; // km
                        break;
                    }
                case EventType.VenusRiseSet:
                    {
                        Retval.Radius = VENUS_RADIUS; // km
                        break;
                    }
                case EventType.MarsRiseSet:
                    {
                        Retval.Radius = MARS_RADIUS; // km
                        break;
                    }
                case EventType.JupiterRiseSet:
                    {
                        Retval.Radius = JUPITER_RADIUS; // km
                        break;
                    }
                case EventType.SaturnRiseSet:
                    {
                        Retval.Radius = SATURN_RADIUS; // km
                        break;
                    }
                case EventType.UranusRiseSet:
                    {
                        Retval.Radius = URANUS_RADIUS; // km
                        break;
                    }
                case EventType.NeptuneRiseSet:
                    {
                        Retval.Radius = NEPTUNE_RADIUS; // km
                        break;
                    }
                case EventType.PlutoRiseSet:
                    {
                        Retval.Radius = PLUTO_RADIUS; // km
                        break;
                    }
                case EventType.MoonRiseMoonSet:
                    {
                        Retval.Radius = MOON_RADIUS; // km
                        break;
                    }

                default:
                    {
                        Retval.Radius = SUN_RADIUS; // km
                        break;
                    }
            }

            return Retval;
        }

        /// <summary>
        /// Returns the fraction of the Moon's surface that is illuminated 
        /// </summary>
        /// <param name="JD">Julian day (UTC) for which the Moon illumination is required</param>
        /// <returns>Percentage illumination of the Moon</returns>
        /// <remarks> The algorithm used is that given in Astronomical Algorithms (Second Edition, Corrected to August 2009) 
        /// Chapter 48 p345 by Jean Meeus (Willmann-Bell 1991). The Sun and Moon positions are calculated by high precision NOVAS 3.1 library using JPL DE 421 ephemeredes.
        /// </remarks>
        public double MoonIllumination(double JD)
        {
            var Obj3 = new Object3();
            var Location = new OnSurface();
            var Cat = new CatEntry3();
            SkyPos SunPosition = new SkyPos(), MoonPosition = new SkyPos();
            var Obs = new Observer();
            double Phi, Inc, k, DeltaT;

            // DeltaT = DeltaTCalc(JD)
            DeltaT = Parameters.DeltaT(JD);

            // Calculate Moon RA, Dec and distance
            Obj3.Name = "Moon";
            Obj3.Number = Body.Moon;
            Obj3.Star = Cat;
            Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

            Obs.OnSurf = Location;
            Obs.Where = ObserverLocation.EarthGeoCenter;

            Nov31.Place(JD + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref MoonPosition);

            // Calculate Sun RA, Dec and distance
            Obj3.Name = "Sun";
            Obj3.Number = Body.Sun;
            Obj3.Star = Cat;
            Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

            Nov31.Place(JD + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref SunPosition);

            // Calculate geocentriic elongation of the Moon
            Phi = Math.Acos(Math.Sin(SunPosition.Dec * DEG2RAD) * Math.Sin(MoonPosition.Dec * DEG2RAD) + Math.Cos(SunPosition.Dec * DEG2RAD) * Math.Cos(MoonPosition.Dec * DEG2RAD) * Math.Cos((SunPosition.RA - MoonPosition.RA) * HOURS2DEG * DEG2RAD));

            // Calculate the phase angle of the Moon
            Inc = Math.Atan2(SunPosition.Dis * Math.Sin(Phi), MoonPosition.Dis - SunPosition.Dis * Math.Cos(Phi));

            // Calculate the illuminated fraction of the Moon's disc
            k = (1.0d + Math.Cos(Inc)) / 2.0d;

            return k;
        }

        /// <summary>
        /// Returns the Moon phase as an angle
        /// </summary>
        /// <param name="JD">Julian day (UTC) for which the Moon phase is required</param>
        /// <returns>Moon phase as an angle between -180.0 amd +180.0 (see Remarks for further description)</returns>
        /// <remarks>To allow maximum freedom in displaying the Moon phase, this function returns the excess of the apparent geocentric longitude
        /// of the Moon over the apparent geocentric longitude of the Sun, expressed as an angle in the range -180.0 to +180.0 degrees.
        /// This definition is taken from Astronomical Algorithms (Second Edition, Corrected to August 2009) Chapter 49 p349
        /// by Jean Meeus (Willmann-Bell 1991).
        /// <para>The frequently used eight phase description for phases of the Moon can be easily constructed from the results of this function
        /// using logic similar to the following:
        /// <code>
        /// Select Case MoonPhase
        ///     Case -180.0 To -135.0
        ///         Phase = "Full Moon"
        ///     Case -135.0 To -90.0
        ///         Phase = "Waning Gibbous"
        ///     Case -90.0 To -45.0
        ///         Phase = "Last Quarter"
        ///     Case -45.0 To 0.0
        ///         Phase = "Waning Crescent"
        ///     Case 0.0 To 45.0
        ///         Phase = "New Moon"
        ///     Case 45.0 To 90.0
        ///         Phase = "Waxing Crescent"
        ///     Case 90.0 To 135.0
        ///         Phase = "First Quarter"
        ///     Case 135.0 To 180.0
        ///         Phase = "Waxing Gibbous"
        /// End Select
        /// </code></para>
        /// <para>Other representations can be easily constructed by changing the angle ranges and text descriptors as desired. The result range -180 to +180
        /// was chosen so that negative values represent the Moon waning and positive values represent the Moon waxing.</para>
        /// </remarks>
        public double MoonPhase(double JD)
        {
            var Obj3 = new Object3();
            var Location = new OnSurface();
            var Cat = new CatEntry3();
            SkyPos SunPosition = new SkyPos(), MoonPosition = new SkyPos();
            var Obs = new Observer();
            double PositionAngle, DeltaT;

            // DeltaT = DeltaTCalc(JD)
            DeltaT = Parameters.DeltaT(JD);

            // Calculate Moon RA, Dec and distance
            Obj3.Name = "Moon";
            Obj3.Number = Body.Moon;
            Obj3.Star = Cat;
            Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

            Obs.OnSurf = Location;
            Obs.Where = ObserverLocation.EarthGeoCenter;

            Nov31.Place(JD + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref MoonPosition);

            // Calculate Sun RA, Dec and distance
            Obj3.Name = "Sun";
            Obj3.Number = Body.Sun;
            Obj3.Star = Cat;
            Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

            Nov31.Place(JD + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref SunPosition);

            // Return the difference between the sun and moon RA's expressed as degrees from -180 to +180
            PositionAngle = Range((MoonPosition.RA - SunPosition.RA) * HOURS2DEG, -180.0d, false, 180.0d, true);

            return PositionAngle;

        }

        private double UTCJulianDate(DateTime UTCJDate)
        {
            double jd1 = default, jd2 = default;
            int rc;

            // Revised to use SOFA to calculate the Julian date
            rc = Sofa.Dtf2d("UTC", UTCJDate.Year, UTCJDate.Month, UTCJDate.Day, UTCJDate.Hour, UTCJDate.Minute, UTCJDate.Second + UTCJDate.Millisecond / 1000.0d, ref jd1, ref jd2);
            if (rc != 0)
                TL.LogMessage("UTCJulianDate", string.Format("Bad return code from Sofa.Dtf2d: {0} for date: {1}", rc, UTCJDate.ToString("dddd dd MMMM yyyy HH:mm:ss.fff")));
            // TL.LogMessage("UTCJulianDate", String.Format("Returning Julian date of {0} for date: {1}", jd1 + jd2, UTCJDate.ToString("dddd dd MMMM yyyy HH:mm:ss.fff")))

            return jd1 + jd2;

        }

        /// <summary>
        /// Refresh to parameter values and invalidate caches in the parameters object so that any new values wil be used
        /// </summary>
        internal void Refresh()
        {
            Parameters.RefreshState();
        }

    }
}