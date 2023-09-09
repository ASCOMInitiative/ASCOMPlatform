using System;
using System.Diagnostics;
// Transform component implementation

using static System.Math;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Utilities;

namespace ASCOM.Astrometry.Transform
{
    /// <summary>
    /// Coordinate transform component; J2000 - apparent - topocentric
    /// </summary>
    /// <remarks>Use this component to transform between J2000, apparent and topocentric (JNow) coordinates or 
    /// vice versa. To use the component, instantiate it, then use one of SetJ2000 or SetJNow or SetApparent to 
    /// initialise with known values. Now use the RAJ2000, DECJ200, RAJNow, DECJNow, RAApparent and DECApparent etc. 
    /// properties to read off the required transformed values.
    /// <para>The component can be reused simply by setting new co-ordinates with a Set command, there
    /// is no need to create a new component each time a transform is required.</para>
    /// <para>Transforms are effected through the ASCOM NOVAS.Net engine that encapsulates the USNO NOVAS 3.1 library. 
    /// The USNO NOVAS reference web page is: 
    /// <href>http://www.usno.navy.mil/USNO/astronomical-applications/software-products/novas</href>
    /// and the NOVAS 3.1 user guide is included in the ASCOM Developer Components install.
    /// </para>
    /// </remarks>
    [Guid("779CD957-5502-4939-A661-EBEE9E1F485E")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Transform : ITransform, IDisposable
    {
        private bool disposedValue = false;        // To detect redundant calls
        private Util Utl;
        private AstroUtils.AstroUtils AstroUtl;
        private SOFA.SOFA SOFA;
        private double RAJ2000Value, RATopoValue, DECJ2000Value, DECTopoValue, SiteElevValue, SiteLatValue, SiteLongValue, SiteTempValue, SitePressureValue;
        private double RAApparentValue, DECApparentValue, AzimuthTopoValue, ElevationTopoValue, JulianDateTTValue, JulianDateUTCValue;
        private bool RefracValue, RequiresRecalculate;
        private SetBy LastSetBy;

        private TraceLogger TL;
        private Stopwatch Sw, SwRecalculate;

        private const double HOURS2RADIANS = PI / 12.0d;
        private const double DEGREES2RADIANS = PI / 180.0d;
        private const double RADIANS2HOURS = 12.0d / PI;
        private const double RADIANS2DEGREES = 180.0d / PI;

        private const string DATE_FORMAT = "dd/MM/yyyy HH:mm:ss.fff";

        private const double STANDARD_PRESSURE = 1013.25d; // Standard atmospheric pressure (hPa)
        private const double ABSOLUTE_ZERO_CELSIUS = -273.15d; // Absolute zero expressed in Celsius
        private enum SetBy
        {
            Never,
            J2000,
            Apparent,
            Topocentric,
            AzimuthElevation,
            Refresh
        }

        #region New and IDisposable
        public Transform()
        {
            TL = new TraceLogger("", "Transform");
            TL.Enabled = Utilities.Global.GetBool(Utilities.Global.TRACE_TRANSFORM, Utilities.Global.TRACE_TRANSFORM_DEFAULT); // Get enabled / disabled state from the user registry
            TL.LogMessage("New", "Trace logger created OK");

            Utl = new Util(); // Get a Util component for Julian functions
            Sw = new Stopwatch();
            SwRecalculate = new Stopwatch();
            AstroUtl = new AstroUtils.AstroUtils();
            SOFA = new SOFA.SOFA();

            // Initialise to invalid values in case these are read before they are set
            RAJ2000Value = double.NaN;
            DECJ2000Value = double.NaN;
            RATopoValue = double.NaN;
            DECTopoValue = double.NaN;
            SiteElevValue = double.NaN;
            SiteLatValue = double.NaN;
            SiteLongValue = double.NaN;
            SitePressureValue = double.NaN;

            RefracValue = false;
            LastSetBy = SetBy.Never;
            RequiresRecalculate = true;
            JulianDateTTValue = 0d; // Initialise to a value that forces the current PC date time to be used in determining the TT Julian date of interest
            CheckGAC();
            TL.LogMessage("New", "NOVAS initialised OK");
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (Utl is not null) // Clean up Util object
                {
                    Utl.Dispose();
                    Utl = null;
                }
                if (AstroUtl is not null)
                {
                    AstroUtl.Dispose();
                    AstroUtl = null;
                }
                if (Sw is not null)
                {
                    Sw.Stop();
                    Sw = null;
                }
                if (SwRecalculate is not null)
                {
                    SwRecalculate.Stop();
                    SwRecalculate = null;
                }
            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// Cleans up resources used by the Transform component
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region ITransform Implementation
        /// <summary>
        /// Gets or sets the site latitude
        /// </summary>
        /// <value>Site latitude (-90.0 to +90.0)</value>
        /// <returns>Latitude in degrees</returns>
        /// <remarks>Positive numbers north of the equator, negative numbers south.</remarks>
        public double SiteLatitude
        {
            get
            {
                CheckSet("SiteLatitude", SiteLatValue, "Site latitude has not been set");
                TL.LogMessage("SiteLatitude Get", FormatDec(SiteLatValue));
                return SiteLatValue;
            }
            set
            {
                if (value < -90.0d | value > 90.0d)
                    throw new InvalidValueException("SiteLatitude", value.ToString(), "-90.0 degrees", "+90.0 degrees");
                if (SiteLatValue != value)
                    RequiresRecalculate = true;
                SiteLatValue = value;
                TL.LogMessage("SiteLatitude Set", FormatDec(value));
            }
        }

        /// <summary>
        /// Gets or sets the site longitude
        /// </summary>
        /// <value>Site longitude (-180.0 to +180.0)</value>
        /// <returns>Longitude in degrees</returns>
        /// <remarks>Positive numbers east of the Greenwich meridian, negative numbers west of the Greenwich meridian.</remarks>
        public double SiteLongitude
        {
            get
            {
                CheckSet("SiteLongitude", SiteLongValue, "Site longitude has not been set");
                TL.LogMessage("SiteLongitude Get", FormatDec(SiteLongValue));
                return SiteLongValue;
            }
            set
            {
                if (value < -180.0d | value > 180.0d)
                    throw new InvalidValueException("SiteLongitude", value.ToString(), "-180.0 degrees", "+180.0 degrees");
                if (SiteLongValue != value)
                    RequiresRecalculate = true;
                SiteLongValue = value;
                TL.LogMessage("SiteLongitude Set", FormatDec(value));
            }
        }

        /// <summary>
        /// Gets or sets the site elevation above sea level
        /// </summary>
        /// <value>Site elevation (-300.0 to +10,000.0 metres)</value>
        /// <returns>Elevation in metres</returns>
        /// <remarks></remarks>
        public double SiteElevation
        {
            get
            {
                CheckSet("SiteElevation", SiteElevValue, "Site elevation has not been set");
                TL.LogMessage("SiteElevation Get", SiteElevValue.ToString());
                return SiteElevValue;
            }
            set
            {
                if (value < -300.0d | value > 10000.0d)
                    throw new InvalidValueException("SiteElevation", value.ToString(), "-300.0 metres", "+10000.0 metres");
                if (SiteElevValue != value)
                    RequiresRecalculate = true;
                SiteElevValue = value;
                TL.LogMessage("SiteElevation Set", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the site ambient temperature (not reduced to sea level)
        /// </summary>
        /// <value>Site ambient temperature (-273.15 to 100.0 Celsius)</value>
        /// <returns>Temperature in degrees Celsius</returns>
        /// <remarks>This property represents the air temperature as measured by a thermometer at the observing site. It must not be a "reduced to sea level" value.</remarks>
        public double SiteTemperature
        {
            get
            {
                CheckSet("SiteTemperature", SiteTempValue, "Site temperature has not been set");
                TL.LogMessage("SiteTemperature Get", SiteTempValue.ToString());
                return SiteTempValue;
            }
            set
            {
                if (value < -273.15d | value > 100.0d)
                    throw new InvalidValueException("SiteTemperature", value.ToString(), "-273.15 Celsius", "+100.0 Celsius");
                if (SiteTempValue != value)
                    RequiresRecalculate = true;
                SiteTempValue = value;
                TL.LogMessage("SiteTemperature Set", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the site atmospheric pressure (not reduced to sea level)
        /// </summary>
        /// <value>Site atmospheric pressure (0.0 to 1200.0 hPa (mbar))</value>
        /// <returns>Atmospheric pressure (hPa)</returns>
        /// <remarks>This property represents the atmospheric pressure as measured by a barometer at the observing site. It must not be a "reduced to sea level" value.</remarks>
        public double SitePressure
        {
            get
            {
                CheckSet("SitePressure", SitePressureValue, "Site atmospheric pressure has not been set");
                TL.LogMessage("SitePressure Get", SitePressureValue.ToString());
                return SitePressureValue;
            }
            set
            {
                if (value < 0.0d | value > 1200.0d)
                    throw new InvalidValueException("SitePressure", value.ToString(), "0.0hPa (mbar)", "+1200.0hPa (mbar)");
                if (SitePressureValue != value)
                    RequiresRecalculate = true;
                SitePressureValue = value;
                TL.LogMessage("SitePressure Set", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating whether refraction is calculated for topocentric co-ordinates
        /// </summary>
        /// <value>True / false flag indicating refraction is included / omitted from topocentric co-ordinates</value>
        /// <returns>Boolean flag</returns>
        /// <remarks></remarks>
        public bool Refraction
        {
            get
            {
                TL.LogMessage("Refraction Get", RefracValue.ToString());
                return RefracValue;
            }
            set
            {
                if (RefracValue != value)
                    RequiresRecalculate = true;
                RefracValue = value;
                TL.LogMessage("Refraction Set", value.ToString());
            }
        }

        /// <summary>
        /// Causes the transform component to recalculate values derived from the last Set command
        /// </summary>
        /// <remarks>Use this when you have set J2000 co-ordinates and wish to ensure that the mount points to the same 
        /// co-ordinates allowing for local effects that change with time such as refraction.
        /// <para><b style="color:red">Note:</b> As of Platform 6 SP2 use of this method is not required, refresh is always performed automatically when required.</para></remarks>
        public void Refresh()
        {
            TL.LogMessage("Refresh", "");
            Recalculate();
        }

        /// <summary>
        /// Sets the known J2000 Right Ascension and Declination coordinates that are to be transformed
        /// </summary>
        /// <param name="RA">RA in J2000 co-ordinates (0.0 to 23.999 hours)</param>
        /// <param name="DEC">DEC in J2000 co-ordinates (-90.0 to +90.0)</param>
        /// <remarks></remarks>
        public void SetJ2000(double RA, double DEC)
        {

            if (RA != RAJ2000Value | DEC != DECJ2000Value)
            {
                RAJ2000Value = ValidateRA("SetJ2000", RA);
                DECJ2000Value = ValidateDec("SetJ2000", DEC);
                RequiresRecalculate = true;
            }

            LastSetBy = SetBy.J2000;
            TL.LogMessage("SetJ2000", "RA: " + FormatRA(RA) + ", DEC: " + FormatDec(DEC));
        }

        /// <summary>
        /// Sets the known apparent Right Ascension and Declination coordinates that are to be transformed
        /// </summary>
        /// <param name="RA">RA in apparent co-ordinates (0.0 to 23.999 hours)</param>
        /// <param name="DEC">DEC in apparent co-ordinates (-90.0 to +90.0)</param>
        /// <remarks></remarks>
        public void SetApparent(double RA, double DEC)
        {

            if (RA != RAApparentValue | DEC != DECApparentValue)
            {
                RAApparentValue = ValidateRA("SetApparent", RA);
                DECApparentValue = ValidateDec("SetApparent", DEC);
                RequiresRecalculate = true;
            }

            LastSetBy = SetBy.Apparent;
            TL.LogMessage("SetApparent", "RA: " + FormatRA(RA) + ", DEC: " + FormatDec(DEC));
        }

        /// <summary>
        /// Sets the known topocentric Right Ascension and Declination coordinates that are to be transformed
        /// </summary>
        /// <param name="RA">RA in topocentric co-ordinates (0.0 to 23.999 hours)</param>
        /// <param name="DEC">DEC in topocentric co-ordinates (-90.0 to +90.0)</param>
        /// <remarks></remarks>
        public void SetTopocentric(double RA, double DEC)
        {

            if (RA != RATopoValue | DEC != DECTopoValue)
            {
                RATopoValue = ValidateRA("SetTopocentric", RA);
                DECTopoValue = ValidateDec("SetTopocentric", DEC);
                RequiresRecalculate = true;
            }

            LastSetBy = SetBy.Topocentric;
            TL.LogMessage("SetTopocentric", "RA: " + FormatRA(RA) + ", DEC: " + FormatDec(DEC));
        }

        /// <summary>
        /// Sets the topocentric azimuth and elevation
        /// </summary>
        /// <param name="Azimuth">Topocentric Azimuth in degrees (0.0 to 359.999999 - north zero, east 90 deg etc.)</param>
        /// <param name="Elevation">Topocentric elevation in degrees (-90.0 to +90.0)</param>
        /// <remarks></remarks>
        public void SetAzimuthElevation(double Azimuth, double Elevation)
        {

            if (Azimuth < 0.0d | Azimuth >= 360.0d)
                throw new InvalidValueException("SetAzimuthElevation Azimuth", Azimuth.ToString(), "0.0 hours", "23.9999999... hours");
            if (Elevation < -90.0d | Elevation > 90.0d)
                throw new InvalidValueException("SetAzimuthElevation Elevation", Elevation.ToString(), "-90.0 degrees", "+90.0 degrees");

            AzimuthTopoValue = Azimuth;
            ElevationTopoValue = Elevation;
            RequiresRecalculate = true;

            LastSetBy = SetBy.AzimuthElevation;
            TL.LogMessage("SetAzimuthElevation", "Azimuth: " + FormatDec(Azimuth) + ", Elevation: " + FormatDec(Elevation));
        }

        /// <summary>
        /// Returns the Right Ascension in J2000 co-ordinates
        /// </summary>
        /// <value>J2000 Right Ascension</value>
        /// <returns>Right Ascension in hours</returns>
        /// <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        /// to read a value before any of the Set methods has been used or if the value can not be derived from the
        /// information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        /// a SetApparent and one of the Site properties has not been set.</exception>
        /// <remarks></remarks>
        public double RAJ2000
        {
            get
            {
                if (LastSetBy == SetBy.Never)
                    throw new Exceptions.TransformUninitialisedException("Attempt to read RAJ2000 before a SetXX method has been called");
                Recalculate();
                CheckSet("RAJ2000", RAJ2000Value, "RA J2000 can not be derived from the information provided. Are site parameters set?");
                TL.LogMessage("RAJ2000 Get", FormatRA(RAJ2000Value));
                return RAJ2000Value;
            }
        }

        /// <summary>
        /// Returns the Declination in J2000 co-ordinates
        /// </summary>
        /// <value>J2000 Declination</value>
        /// <returns>Declination in degrees</returns>
        /// <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        /// to read a value before any of the Set methods has been used or if the value can not be derived from the
        /// information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        /// a SetApparent and one of the Site properties has not been set.</exception>
        /// <remarks></remarks>
        public double DECJ2000
        {
            get
            {
                if (LastSetBy == SetBy.Never)
                    throw new Exceptions.TransformUninitialisedException("Attempt to read DECJ2000 before a SetXX method has been called");
                Recalculate();
                CheckSet("DecJ2000", DECJ2000Value, "DEC J2000 can not be derived from the information provided. Are site parameters set?");
                TL.LogMessage("DecJ2000 Get", FormatDec(DECJ2000Value));
                return DECJ2000Value;
            }
        }

        /// <summary>
        /// Returns the Right Ascension in topocentric co-ordinates
        /// </summary>
        /// <value>Topocentric Right Ascension</value>
        /// <returns>Topocentric Right Ascension in hours</returns>
        /// <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        /// to read a value before any of the Set methods has been used or if the value can not be derived from the
        /// information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        /// a SetApparent and one of the Site properties has not been set.</exception>
        /// <remarks></remarks>
        public double RATopocentric
        {
            get
            {
                if (LastSetBy == SetBy.Never)
                    throw new Exceptions.TransformUninitialisedException("Attempt to read RATopocentric before a SetXX method  has been called");
                Recalculate();
                CheckSet("RATopocentric", RATopoValue, "RA topocentric can not be derived from the information provided. Are site parameters set?");
                TL.LogMessage("RATopocentric Get", FormatRA(RATopoValue));
                return RATopoValue;
            }
        }

        /// <summary>
        /// Returns the Declination in topocentric co-ordinates
        /// </summary>
        /// <value>Topocentric Declination</value>
        /// <returns>Declination in degrees</returns>
        /// <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        /// to read a value before any of the Set methods has been used or if the value can not be derived from the
        /// information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        /// a SetApparent and one of the Site properties has not been set.</exception>
        /// <remarks></remarks>
        public double DECTopocentric
        {
            get
            {
                if (LastSetBy == SetBy.Never)
                    throw new Exceptions.TransformUninitialisedException("Attempt to read DECTopocentric before a SetXX method has been called");
                Recalculate();
                CheckSet("DECTopocentric", DECTopoValue, "DEC topocentric can not be derived from the information provided. Are site parameters set?");
                TL.LogMessage("DECTopocentric Get", FormatDec(DECTopoValue));
                return DECTopoValue;
            }
        }

        /// <summary>
        /// Returns the Right Ascension in apparent co-ordinates
        /// </summary>
        /// <value>Apparent Right Ascension</value>
        /// <returns>Right Ascension in hours</returns>
        /// <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        /// to read a value before any of the Set methods has been used or if the value can not be derived from the
        /// information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        /// a SetApparent and one of the Site properties has not been set.</exception>
        /// <remarks></remarks>
        public double RAApparent
        {
            get
            {
                if (LastSetBy == SetBy.Never)
                    throw new Exceptions.TransformUninitialisedException("Attempt to read DECApparent before a SetXX method has been called");
                Recalculate();
                TL.LogMessage("RAApparent Get", FormatRA(RAApparentValue));
                return RAApparentValue;
            }
        }

        /// <summary>
        /// Returns the Declination in apparent co-ordinates
        /// </summary>
        /// <value>Apparent Declination</value>
        /// <returns>Declination in degrees</returns>
        /// <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        /// to read a value before any of the Set methods has been used or if the value can not be derived from the
        /// information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        /// a SetApparent and one of the Site properties has not been set.</exception>
        /// <remarks></remarks>
        public double DECApparent
        {
            get
            {
                if (LastSetBy == SetBy.Never)
                    throw new Exceptions.TransformUninitialisedException("Attempt to read DECApparent before a SetXX method has been called");
                Recalculate();
                TL.LogMessage("DECApparent Get", FormatDec(DECApparentValue));
                return DECApparentValue;
            }
        }

        /// <summary>
        /// Returns the topocentric azimuth angle of the target
        /// </summary>
        /// <value>Topocentric azimuth angle</value>
        /// <returns>Azimuth angle in degrees</returns>
        /// <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        /// to read a value before any of the Set methods has been used or if the value can not be derived from the
        /// information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        /// a SetApparent and one of the Site properties has not been set.</exception>
        /// <remarks></remarks>
        public double AzimuthTopocentric
        {
            get
            {
                if (LastSetBy == SetBy.Never)
                    throw new Exceptions.TransformUninitialisedException("Attempt to read AzimuthTopocentric before a SetXX method has been called");
                RequiresRecalculate = true; // Force a recalculation of Azimuth
                Recalculate();
                CheckSet("AzimuthTopocentric", AzimuthTopoValue, "Azimuth topocentric can not be derived from the information provided. Are site parameters set?");
                TL.LogMessage("AzimuthTopocentric Get", FormatDec(AzimuthTopoValue));
                return AzimuthTopoValue;
            }
        }

        /// <summary>
        /// Returns the topocentric elevation of the target
        /// </summary>
        /// <value>Topocentric elevation angle</value>
        /// <returns>Elevation angle in degrees</returns>
        /// <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        /// to read a value before any of the Set methods has been used or if the value can not be derived from the
        /// information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        /// a SetApparent and one of the Site properties has not been set.</exception>
        /// <remarks></remarks>
        public double ElevationTopocentric
        {
            get
            {
                if (LastSetBy == SetBy.Never)
                    throw new Exceptions.TransformUninitialisedException("Attempt to read ElevationTopocentric before a SetXX method has been called");
                RequiresRecalculate = true; // Force a recalculation of Elevation
                Recalculate();
                CheckSet("ElevationTopocentric", ElevationTopoValue, "Elevation topocentric can not be derived from the information provided. Are site parameters set?");
                TL.LogMessage("ElevationTopocentric Get", FormatDec(ElevationTopoValue));
                return ElevationTopoValue;
            }
        }

        /// <summary>
        /// Sets or returns the Julian date on the Terrestrial Time timescale for which the transform will be made
        /// </summary>
        /// <value>Julian date (Terrestrial Time) of the transform (1757583.5 to 5373484.499999 = 00:00:00 1/1/0100 to 23:59:59.999 31/12/9999)</value>
        /// <returns>Terrestrial Time Julian date that will be used by Transform or zero if the PC's current clock value will be used to calculate the Julian date.</returns>
        /// <remarks>This method was introduced in May 2012. Previously, Transform used the current date-time of the PC when calculating transforms; 
        /// this remains the default behaviour for backward compatibility.
        /// The initial value of this parameter is 0.0, which is a special value that forces Transform to replicate original behaviour by determining the  
        /// Julian date from the PC's current date and time. If this property is non zero, that particular terrestrial time Julian date is used in preference 
        /// to the value derived from the PC's clock.
        /// <para>Only one of JulianDateTT or JulianDateUTC needs to be set. Use whichever is more readily available, there is no
        /// need to set both values. Transform will use the last set value of either JulianDateTT or JulianDateUTC as the basis for its calculations.</para></remarks>
        public double JulianDateTT
        {
            get
            {
                return JulianDateTTValue;
            }
            set
            {
                double tai1 = default, tai2 = default, utc1 = default, utc2 = default;

                // Validate the supplied value, it must be 0.0 or within the permitted range
                if (value != 0.0d & (value < GlobalItems.JULIAN_DATE_MINIMUM_VALUE | value > GlobalItems.JULIAN_DATE_MAXIMUM_VALUE))
                {
                    throw new InvalidValueException("JulianDateTT", value.ToString(), GlobalItems.JULIAN_DATE_MINIMUM_VALUE.ToString(), GlobalItems.JULIAN_DATE_MAXIMUM_VALUE.ToString());
                }

                JulianDateTTValue = value;
                RequiresRecalculate = true; // Force a recalculation because the Julian date has changed

                if (JulianDateTTValue != 0.0d)
                {
                    // Calculate UTC
                    if (SOFA.TtTai(JulianDateTTValue, 0.0d, ref tai1, ref tai2) != 0)
                        TL.LogMessage("JulianDateTT Set", "TtTai - Bad return code");
                    if (SOFA.TaiUtc(tai1, tai2, ref utc1, ref utc2) != 0)
                        TL.LogMessage("JulianDateTT Set", "TaiUtc - Bad return code");
                    JulianDateUTCValue = utc1 + utc2;

                    TL.LogMessage("JulianDateTT Set", JulianDateTTValue.ToString() + " " + Utl.DateJulianToUTC(JulianDateTTValue).ToString(DATE_FORMAT) + ", JDUTC: " + Utl.DateJulianToUTC(JulianDateUTCValue).ToString(DATE_FORMAT));
                }
                else // Handle special case of 0.0
                {
                    JulianDateUTCValue = 0.0d;
                    TL.LogMessage("JulianDateTT Set", "Calculations will now be based on PC the DateTime");
                }
            }
        }

        /// <summary>
        /// Sets or returns the Julian date on the UTC timescale for which the transform will be made
        /// </summary>
        /// <value>Julian date (UTC) of the transform (1757583.5 to 5373484.499999 = 00:00:00 1/1/0100 to 23:59:59.999 31/12/9999)</value>
        /// <returns>UTC Julian date that will be used by Transform or zero if the PC's current clock value will be used to calculate the Julian date.</returns>
        /// <remarks>Introduced in April 2014 as an alternative to JulianDateTT. Only one of JulianDateTT or JulianDateUTC needs to be set. Use whichever is more readily available, there is no
        /// need to set both values. Transform will use the last set value of either JulianDateTT or JulianDateUTC as the basis for its calculations.</remarks>
        public double JulianDateUTC
        {
            get
            {
                return JulianDateUTCValue;
            }
            set
            {
                double tai1 = default, tai2 = default, tt1 = default, tt2 = default;

                // Validate the supplied value, it must be 0.0 or within the permitted range
                if (value != 0.0d & (value < GlobalItems.JULIAN_DATE_MINIMUM_VALUE | value > GlobalItems.JULIAN_DATE_MAXIMUM_VALUE))
                {
                    throw new InvalidValueException("JulianDateUTC", value.ToString(), GlobalItems.JULIAN_DATE_MINIMUM_VALUE.ToString(), GlobalItems.JULIAN_DATE_MAXIMUM_VALUE.ToString());
                }

                JulianDateUTCValue = value;
                RequiresRecalculate = true; // Force a recalculation because the Julian date has changed

                if (JulianDateUTCValue != 0.0d)
                {
                    // Calculate Terrestrial Time equivalent
                    if (SOFA.UtcTai(JulianDateUTCValue, 0.0d, ref tai1, ref tai2) != 0)
                        TL.LogMessage("JulianDateUTC Set", "UtcTai - Bad return code");
                    if (SOFA.TaiTt(tai1, tai2, ref tt1, ref tt2) != 0)
                        TL.LogMessage("JulianDateUTC Set", "TaiTt - Bad return code");
                    JulianDateTTValue = tt1 + tt2;

                    TL.LogMessage("JulianDateUTC Set", JulianDateTTValue.ToString() + " " + Utl.DateJulianToUTC(JulianDateUTCValue).ToString(DATE_FORMAT) + ", JDTT: " + Utl.DateJulianToUTC(JulianDateTTValue).ToString(DATE_FORMAT));
                }
                else // Handle special case of 0.0
                {
                    JulianDateTTValue = 0.0d;
                    TL.LogMessage("JulianDateUTC Set", "Calculations will now be based on PC the DateTime");
                }
            }
        }
        #endregion

        #region Support Code

        private void CheckSet(string Caller, double Value, string ErrMsg)
        {
            if (double.IsNaN(Value))
            {
                TL.LogMessage(Caller, "Throwing TransformUninitialisedException: " + ErrMsg);
                throw new Exceptions.TransformUninitialisedException(ErrMsg);
            }
        }

        private void J2000ToTopo()
        {
            double DUT1, JDUTCSofa;
            double aob = default, zob = default, hob = default, dob = default, rob = default, eo = default;

            if (double.IsNaN(SiteElevValue))
                throw new Exceptions.TransformUninitialisedException("Site elevation has not been set");
            if (double.IsNaN(SiteLatValue))
                throw new Exceptions.TransformUninitialisedException("Site latitude has not been set");
            if (double.IsNaN(SiteLongValue))
                throw new Exceptions.TransformUninitialisedException("Site longitude has not been set");
            if (double.IsNaN(SiteTempValue))
                throw new Exceptions.TransformUninitialisedException("Site temperature has not been set");

            // Calculate site pressure at site elevation if this has not been provided
            CalculateSitePressureIfRequired();

            Sw.Reset();
            Sw.Start();

            JDUTCSofa = GetJDUTCSofa();
            DUT1 = AstroUtl.DeltaUT(JDUTCSofa);

            Sw.Reset();
            Sw.Start();

            if (RefracValue) // Include refraction
            {
                SOFA.CelestialToObserved(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0d, 0.0d, 0.0d, 0.0d, JDUTCSofa, 0.0d, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0d, 0.0d, SitePressureValue, SiteTempValue, 0.8d, 0.57d, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
            }
            else // No refraction
            {
                SOFA.CelestialToObserved(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0d, 0.0d, 0.0d, 0.0d, JDUTCSofa, 0.0d, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
            }

            RATopoValue = SOFA.Anp(rob - eo) * RADIANS2HOURS; // // Convert CIO RA to equinox of date RA by subtracting the equation of the origins and convert from radians to hours
            DECTopoValue = dob * RADIANS2DEGREES; // Convert Dec from radians to degrees
            AzimuthTopoValue = aob * RADIANS2DEGREES;
            ElevationTopoValue = 90.0d - zob * RADIANS2DEGREES;

            TL.LogMessage("  J2000 To Topo", "  Topocentric RA/DEC (including refraction if specified):  " + FormatRA(RATopoValue) + " " + FormatDec(DECTopoValue) + " Refraction: " + RefracValue.ToString() + ", " + Sw.Elapsed.TotalMilliseconds.ToString("0.00") + "ms");
            TL.LogMessage("  J2000 To Topo", "  Azimuth/Elevation: " + FormatDec(AzimuthTopoValue) + " " + FormatDec(ElevationTopoValue) + ", " + Sw.Elapsed.TotalMilliseconds.ToString("0.00") + "ms");
            TL.LogMessage("  J2000 To Topo", "  Completed");
            TL.BlankLine();
        }

        private void J2000ToApparent()
        {
            double ri = default, di = default, eo = default;
            double JDTTSofa;

            Sw.Reset();
            Sw.Start();
            JDTTSofa = GetJDTTSofa();

            SOFA.CelestialToIntermediate(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0d, 0.0d, 0.0d, 0.0d, JDTTSofa, 0.0d, ref ri, ref di, ref eo);
            RAApparentValue = SOFA.Anp(ri - eo) * RADIANS2HOURS; // // Convert CIO RA to equinox of date RA by subtracting the equation of the origins and convert from radians to hours
            DECApparentValue = di * RADIANS2DEGREES; // Convert Dec from radians to degrees

            TL.LogMessage("  J2000 To Apparent", "  Apparent RA/Dec:   " + FormatRA(RAApparentValue) + " " + FormatDec(DECApparentValue) + ", " + Sw.Elapsed.TotalMilliseconds.ToString("0.00") + "ms");

        }

        private void TopoToJ2000()
        {
            double RACelestrial = default, DecCelestial = default, JDTTSofa, JDUTCSofa, DUT1;
            int RetCode;
            double aob = default, zob = default, hob = default, dob = default, rob = default, eo = default;

            if (double.IsNaN(SiteElevValue))
                throw new Exceptions.TransformUninitialisedException("Site elevation has not been set");
            if (double.IsNaN(SiteLatValue))
                throw new Exceptions.TransformUninitialisedException("Site latitude has not been set");
            if (double.IsNaN(SiteLongValue))
                throw new Exceptions.TransformUninitialisedException("Site longitude has not been set");
            if (double.IsNaN(SiteTempValue))
                throw new Exceptions.TransformUninitialisedException("Site temperature has not been set");

            // Calculate site pressure at site elevation if this has not been provided
            CalculateSitePressureIfRequired();

            Sw.Reset();
            Sw.Start();

            JDUTCSofa = GetJDUTCSofa();
            JDTTSofa = GetJDTTSofa();
            DUT1 = AstroUtl.DeltaUT(JDUTCSofa);

            Sw.Reset();
            Sw.Start();
            if (RefracValue) // Refraction is required
            {
                RetCode = SOFA.ObservedToCelestial("R", SOFA.Anp(RATopoValue * HOURS2RADIANS + SOFA.Eo06a(JDTTSofa, 0.0d)), DECTopoValue * DEGREES2RADIANS, JDUTCSofa, 0.0d, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0d, 0.0d, SitePressureValue, SiteTempValue, 0.85d, 0.57d, ref RACelestrial, ref DecCelestial);
            }
            else
            {
                RetCode = SOFA.ObservedToCelestial("R", SOFA.Anp(RATopoValue * HOURS2RADIANS + SOFA.Eo06a(JDTTSofa, 0.0d)), DECTopoValue * DEGREES2RADIANS, JDUTCSofa, 0.0d, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, ref RACelestrial, ref DecCelestial);
            }

            RAJ2000Value = RACelestrial * RADIANS2HOURS;
            DECJ2000Value = DecCelestial * RADIANS2DEGREES;
            TL.LogMessage("  Topo To J2000", "  J2000 RA/Dec:" + FormatRA(RAJ2000Value) + " " + FormatDec(DECJ2000Value) + ", " + Sw.Elapsed.TotalMilliseconds.ToString("0.00") + "ms");

            // Now calculate the corresponding AzEl values from the J2000 values
            Sw.Reset();
            Sw.Start();
            if (RefracValue) // Include refraction
            {
                SOFA.CelestialToObserved(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0d, 0.0d, 0.0d, 0.0d, JDUTCSofa, 0.0d, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0d, 0.0d, SitePressureValue, SiteTempValue, 0.8d, 0.57d, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
            }
            else // No refraction
            {
                SOFA.CelestialToObserved(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0d, 0.0d, 0.0d, 0.0d, JDUTCSofa, 0.0d, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
            }

            AzimuthTopoValue = aob * RADIANS2DEGREES;
            ElevationTopoValue = 90.0d - zob * RADIANS2DEGREES;

            TL.LogMessage("  Topo To J2000", "  Azimuth/Elevation: " + FormatDec(AzimuthTopoValue) + " " + FormatDec(ElevationTopoValue) + ", " + Sw.Elapsed.TotalMilliseconds.ToString("0.00") + "ms");

        }

        private void ApparentToJ2000()
        {
            double JulianDateTTSofa, RACelestial = default, DecCelestial = default, JulianDateUTCSofa, eo = default;

            Sw.Reset();
            Sw.Start();

            JulianDateTTSofa = GetJDTTSofa();
            JulianDateUTCSofa = GetJDUTCSofa();

            SOFA.IntermediateToCelestial(SOFA.Anp(RAApparentValue * HOURS2RADIANS + SOFA.Eo06a(JulianDateUTCSofa, 0.0d)), DECApparentValue * DEGREES2RADIANS, JulianDateTTSofa, 0.0d, ref RACelestial, ref DecCelestial, ref eo);
            RAJ2000Value = RACelestial * RADIANS2HOURS;
            DECJ2000Value = DecCelestial * RADIANS2DEGREES;
            TL.LogMessage("  Apparent To J2000", "  J2000 RA/Dec" + FormatRA(RAJ2000Value) + " " + FormatDec(DECJ2000Value) + ", " + Sw.Elapsed.TotalMilliseconds.ToString("0.00") + "ms");

        }

        private void Recalculate() // Calculate values for derived co-ordinates
        {
            SwRecalculate.Reset();
            SwRecalculate.Start();
            if (RequiresRecalculate | RefracValue == true)
            {
                TL.LogMessage("Recalculate", $"Requires Recalculate: {RequiresRecalculate}, Refraction: {RefracValue}, Latitude: {SiteLatValue}, Longitude: {SiteLongValue}, Elevation: {SiteElevValue}, Temperature: {SiteTempValue}");
                switch (LastSetBy)
                {
                    case SetBy.J2000: // J2000 coordinates have bee set so calculate apparent and topocentric coordinates
                        {
                            TL.LogMessage("  Recalculate", "  Values last set by SetJ2000");
                            // Check whether required topo values have been set
                            if (!double.IsNaN(SiteLatValue) & !double.IsNaN(SiteLongValue) & !double.IsNaN(SiteElevValue) & !double.IsNaN(SiteTempValue))
                            {
                                J2000ToTopo(); // All required site values present so calculate Topo values
                            }
                            else // Set to NaN
                            {
                                RATopoValue = double.NaN;
                                DECTopoValue = double.NaN;
                                AzimuthTopoValue = double.NaN;
                                ElevationTopoValue = double.NaN;
                            }
                            J2000ToApparent();
                            break;
                        }
                    case SetBy.Topocentric: // Topocentric co-ordinates have been set so calculate J2000 and apparent coordinates
                        {
                            TL.LogMessage("  Recalculate", "  Values last set by SetTopocentric");
                            // Check whether required topo values have been set
                            if (!double.IsNaN(SiteLatValue) & !double.IsNaN(SiteLongValue) & !double.IsNaN(SiteElevValue) & !double.IsNaN(SiteTempValue)) // They have so calculate remaining values
                            {
                                TopoToJ2000();
                                J2000ToApparent();
                            }
                            else // Set the topo and apparent values to NaN
                            {
                                RAJ2000Value = double.NaN;
                                DECJ2000Value = double.NaN;
                                RAApparentValue = double.NaN;
                                DECApparentValue = double.NaN;
                                AzimuthTopoValue = double.NaN;
                                ElevationTopoValue = double.NaN;
                            }

                            break;
                        }
                    case SetBy.Apparent: // Apparent values have been set so calculate J2000 values and topo values if appropriate
                        {
                            TL.LogMessage("  Recalculate", "  Values last set by SetApparent");
                            ApparentToJ2000(); // Calculate J2000 value
                                               // Check whether required topo values have been set
                            if (!double.IsNaN(SiteLatValue) & !double.IsNaN(SiteLongValue) & !double.IsNaN(SiteElevValue) & !double.IsNaN(SiteTempValue))
                            {
                                J2000ToTopo(); // All required site values present so calculate Topo values
                            }
                            else
                            {
                                RATopoValue = double.NaN;
                                DECTopoValue = double.NaN;
                                AzimuthTopoValue = double.NaN;
                                ElevationTopoValue = double.NaN;
                            }

                            break;
                        }
                    case SetBy.AzimuthElevation:
                        {
                            TL.LogMessage("  Recalculate", "  Values last set by AzimuthElevation");
                            if (!double.IsNaN(SiteLatValue) & !double.IsNaN(SiteLongValue) & !double.IsNaN(SiteElevValue) & !double.IsNaN(SiteTempValue))
                            {
                                AzElToJ2000();
                                J2000ToTopo();
                                J2000ToApparent();
                            }
                            else
                            {
                                RAJ2000Value = double.NaN;
                                DECJ2000Value = double.NaN;
                                RAApparentValue = double.NaN;
                                DECApparentValue = double.NaN;
                                RATopoValue = double.NaN;
                                DECTopoValue = double.NaN;
                            } // Neither SetJ2000 nor SetTopocentric nor SetApparent have been called, so throw an exception

                            break;
                        }

                    default:
                        {
                            TL.LogMessage("Recalculate", "Neither SetJ2000 nor SetTopocentric nor SetApparent have been called. Throwing TransforUninitialisedException");
                            throw new Exceptions.TransformUninitialisedException("Can't recalculate Transform object values because neither SetJ2000 nor SetTopocentric nor SetApparent have been called");
                        }
                }
                TL.LogMessage("  Recalculate", "  Completed in " + SwRecalculate.Elapsed.TotalMilliseconds.ToString("0.00") + "ms");
                RequiresRecalculate = false; // Reset the recalculate flag
            }
            else
            {
                TL.LogMessage("  Recalculate", "No parameters have changed, refraction is " + RefracValue + ", recalculation not required");
            }
            SwRecalculate.Stop();
        }

        private void AzElToJ2000()
        {
            int RetCode;
            double JulianDateUTCSofa, RACelestial = default, DecCelestial = default, DUT1;

            Sw.Reset();
            Sw.Start();

            if (double.IsNaN(SiteElevValue))
                throw new Exceptions.TransformUninitialisedException("Site elevation has not been set");
            if (double.IsNaN(SiteLatValue))
                throw new Exceptions.TransformUninitialisedException("Site latitude has not been set");
            if (double.IsNaN(SiteLongValue))
                throw new Exceptions.TransformUninitialisedException("Site longitude has not been set");
            if (double.IsNaN(SiteTempValue))
                throw new Exceptions.TransformUninitialisedException("Site temperature has not been set");

            // Calculate site pressure at site elevation if this has not been provided
            CalculateSitePressureIfRequired();

            JulianDateUTCSofa = GetJDUTCSofa();
            DUT1 = AstroUtl.DeltaUT(JulianDateUTCSofa);

            if (RefracValue) // Refraction is required
            {
                RetCode = SOFA.ObservedToCelestial("A", AzimuthTopoValue * DEGREES2RADIANS, (90.0d - ElevationTopoValue) * DEGREES2RADIANS, JulianDateUTCSofa, 0.0d, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0d, 0.0d, SitePressureValue, SiteTempValue, 0.85d, 0.57d, ref RACelestial, ref DecCelestial);
            }
            else
            {
                RetCode = SOFA.ObservedToCelestial("A", AzimuthTopoValue * DEGREES2RADIANS, (90.0d - ElevationTopoValue) * DEGREES2RADIANS, JulianDateUTCSofa, 0.0d, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, ref RACelestial, ref DecCelestial);
            }

            RAJ2000Value = RACelestial * RADIANS2HOURS;
            DECJ2000Value = DecCelestial * RADIANS2DEGREES;

            TL.LogMessage("  AzEl To J2000", "  SOFA RA: " + FormatRA(RAJ2000Value) + ", Declination: " + FormatDec(DECJ2000Value));

            Sw.Stop();
            TL.BlankLine();
        }

        private double GetJDUTCSofa()
        {
            double Retval, utc1 = default, utc2 = default;
            DateTime Now;

            if (JulianDateUTCValue == 0.0d) // No specific UTC date / time has been set so use the current date / time
            {
                Now = DateTime.UtcNow;
                if (SOFA.Dtf2d("UTC", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second + Now.Millisecond / 1000.0d, ref utc1, ref utc2) != 0)
                    TL.LogMessage("Dtf2d", "Bad return code");
                Retval = utc1 + utc2;
            }
            else // A specific UTC date / time has been set so use it
            {
                Retval = JulianDateUTCValue;
            }
            TL.LogMessage("  GetJDUTCSofa", "  " + Retval.ToString() + " " + Utl.DateJulianToUTC(Retval).ToString(DATE_FORMAT));
            return Retval;
        }

        private double GetJDTTSofa()
        {
            double Retval, utc1 = default, utc2 = default, tai1 = default, tai2 = default, tt1 = default, tt2 = default;
            DateTime Now;

            if (JulianDateTTValue == 0.0d) // No specific TT date / time has been set so use the current date / time
            {
                Now = DateTime.UtcNow;

                // First calculate the UTC Julian date, then convert this to the equivalent TAI Julian date then convert this to the equivalent TT Julian date
                if (SOFA.Dtf2d("UTC", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second + Now.Millisecond / 1000.0d, ref utc1, ref utc2) != 0)
                    TL.LogMessage("Dtf2d", "Bad return code");
                if (SOFA.UtcTai(utc1, utc2, ref tai1, ref tai2) != 0)
                    TL.LogMessage("GetJDTTSofa", "UtcTai - Bad return code");
                if (SOFA.TaiTt(tai1, tai2, ref tt1, ref tt2) != 0)
                    TL.LogMessage("GetJDTTSofa", "TaiTt - Bad return code");

                Retval = tt1 + tt2;
            }
            else // A specific TT date / time has been set so use it
            {
                Retval = JulianDateTTValue;
            }
            TL.LogMessage("  GetJDTTSofa", "  " + Retval.ToString() + " " + Utl.DateJulianToUTC(Retval).ToString(DATE_FORMAT));
            return Retval;
        }

        private void CheckGAC()
        {
            string strPath;
            TL.LogMessage("CheckGAC", "Started");
            strPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            TL.LogMessage("CheckGAC", "Assembly path: " + strPath);
        }

        private double ValidateRA(string Caller, double RA)
        {
            if (RA < 0.0d | RA >= 24.0d)
                throw new InvalidValueException(Caller, RA.ToString(), "0 to 23.9999");
            return RA;
        }

        private double ValidateDec(string Caller, double Dec)
        {
            if (Dec < -90.0d | Dec > 90.0d)
                throw new InvalidValueException(Caller, Dec.ToString(), "-90.0 to 90.0");
            return Dec;
        }

        private string FormatRA(double RA)
        {
            return Utl.HoursToHMS(RA, ":", ":", "", 3);
        }

        private string FormatDec(double Dec)
        {
            return Utl.DegreesToDMS(Dec, ":", ":", "", 3);
        }

        private void CalculateSitePressureIfRequired()
        {
            // Derive the site pressure from the site elevation if the pressure has not been set explicitly
            if (!double.IsNaN(SitePressureValue)) // Site pressure has already been set so don't override the set value
            {
                TL.LogMessage("  CalculateSitePressure", $"  Site pressure has been set to {SitePressureValue:0.0}hPa.");
            }
            else // Site pressure has not been set so derive a value based on the supplied observatory height and temperature
            {
                // phpa = 1013.25 * exp ( −hm / ( 29.3 * tsl ) ); NOTE this equation calculates the site pressure and uses the site temperature REDUCED TO SEA LEVEL MESURED IN DEGREES KELVIN
                // tsl = tSite − 0.0065(0 − hsite);  NOTE this equation reduces the site temperature to sea level
                SitePressureValue = STANDARD_PRESSURE * Exp(-SiteElevValue / (29.3d * (SiteTempValue + 0.0065d * SiteElevValue - ABSOLUTE_ZERO_CELSIUS)));
                TL.LogMessage("  CalculateSitePressure", $"  Site pressure has not been set by user, calculated value: {SitePressureValue:0.0}hPa.");
            }
        }

        #endregion

    }
}