// NOVASCOM component implementation

using System;
using static System.Math;
using System.Runtime.InteropServices;
using ASCOM.Astrometry.Kepler;
using ASCOM.Astrometry.NOVAS;
using static ASCOM.Astrometry.NOVAS.NOVAS2;

namespace ASCOM.Astrometry.NOVASCOM
{

    #region NOVASCOM Implementation
    /// <summary>
    /// NOVAS-COM: Represents the "state" of the Earth at a given Terrestrial Julian date
    /// </summary>
    /// <remarks>NOVAS-COM objects of class Earth represent the "state" of the Earth at a given Terrestrial Julian date. 
    /// The state includes barycentric and heliocentric position vectors for the earth, plus obliquity, 
    /// nutation and the equation of the equinoxes. Unless set by the client, the Earth ephemeris used is 
    /// computed using an internal approximation. The client may optionally attach an ephemeris object for 
    /// increased accuracy. 
    /// <para><b>Ephemeris Generator</b><br />
    /// The ephemeris generator object used with NOVAS-COM must support a single 
    /// method GetPositionAndVelocity(tjd). This method must take a terrestrial Julian date (like the 
    /// NOVAS-COM methods) as its single parameter, and return an array of Double 
    /// containing the rectangular (x/y/z) heliocentric J2000 equatorial coordinates of position (AU) and velocity 
    /// (KM/sec.). In addition, it must support three read/write properties BodyType, Name, and Number, 
    /// which correspond to the Type, Name, and Number properties of Novas.Planet. 
    /// </para></remarks>
    [Guid("6BD93BA2-79C5-4077-9630-B7C6E30B2FDF")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Earth : IEarth
    {

        private PositionVector m_BaryPos = new PositionVector(), m_HeliPos = new PositionVector();
        private VelocityVector m_BaryVel = new VelocityVector(), m_HeliVel = new VelocityVector();
        private double m_BaryTime, m_MeanOb, m_EquOfEqu, m_NutLong, m_NutObl, m_TrueOb;
        private IEphemeris m_EarthEph;
        private bool m_Valid; // Object has valid values
        // Private TL As TraceLogger
        /// <summary>
        /// Create a new instance of the Earth object
        /// </summary>
        /// <remarks></remarks>
        public Earth()
        {
            // TL = New Utilities.TraceLogger("", "Earth")
            // TL.Enabled = True
            // TL.LogMessage("New", "Start")
            m_EarthEph = new Ephemeris();
            m_EarthEph.BodyType = BodyType.MajorPlanet;
            m_EarthEph.Number = Body.Earth;
            m_EarthEph.Name = "Earth";
            m_Valid = false; // Object is invalid
            // TL.LogMessage("New", "Initialised")
        }

        /// <summary>
        /// Earth barycentric position
        /// </summary>
        /// <value>Barycentric position vector</value>
        /// <returns>AU (Ref J2000)</returns>
        /// <remarks></remarks>
        public PositionVector BarycentricPosition
        {
            get
            {
                return m_BaryPos;
            }
        }

        /// <summary>
        /// Earth barycentric time 
        /// </summary>
        /// <value>Barycentric dynamical time for given Terrestrial Julian Date</value>
        /// <returns>Julian date</returns>
        /// <remarks></remarks>
        public double BarycentricTime
        {
            get
            {
                return m_BaryTime;
            }
        }

        /// <summary>
        /// Earth barycentric velocity 
        /// </summary>
        /// <value>Barycentric velocity vector</value>
        /// <returns>AU/day (ref J2000)</returns>
        /// <remarks></remarks>
        public VelocityVector BarycentricVelocity
        {
            get
            {
                return m_BaryVel;
            }
        }

        /// <summary>
        /// Ephemeris object used to provide the position of the Earth.
        /// </summary>
        /// <value>Earth ephemeris object </value>
        /// <returns>Earth ephemeris object</returns>
        /// <remarks>
        /// Setting this is optional, if not set, the internal Kepler engine will be used.</remarks>
        public IEphemeris EarthEphemeris
        {
            get
            {
                return m_EarthEph;
            }
            set
            {
                m_EarthEph = value;
            }
        }

        /// <summary>
        /// Earth equation of equinoxes 
        /// </summary>
        /// <value>Equation of the equinoxes</value>
        /// <returns>Seconds</returns>
        /// <remarks></remarks>
        public double EquationOfEquinoxes
        {
            get
            {
                return m_EquOfEqu;
            }
        }

        /// <summary>
        /// Earth heliocentric position
        /// </summary>
        /// <value>Heliocentric position vector</value>
        /// <returns>AU (ref J2000)</returns>
        /// <remarks></remarks>
        public PositionVector HeliocentricPosition
        {
            get
            {
                return m_HeliPos;
            }
        }

        /// <summary>
        /// Earth heliocentric velocity 
        /// </summary>
        /// <value>Heliocentric velocity</value>
        /// <returns>Velocity vector, AU/day (ref J2000)</returns>
        /// <remarks></remarks>
        public VelocityVector HeliocentricVelocity
        {
            get
            {
                return m_HeliVel;
            }
        }

        /// <summary>
        /// Earth mean objiquity
        /// </summary>
        /// <value>Mean obliquity of the ecliptic</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double MeanObliquity
        {
            get
            {
                return m_MeanOb;
            }
        }

        /// <summary>
        /// Earth nutation in longitude 
        /// </summary>
        /// <value>Nutation in longitude</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double NutationInLongitude
        {
            get
            {
                return m_NutLong;
            }
        }

        /// <summary>
        /// Earth nutation in obliquity 
        /// </summary>
        /// <value>Nutation in obliquity</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double NutationInObliquity
        {
            get
            {
                return m_NutObl;
            }
        }

        /// <summary>
        /// Initialize the Earth object for given terrestrial Julian date
        /// </summary>
        /// <param name="tjd">Terrestrial Julian date</param>
        /// <returns>True if successful, else throws an exception</returns>
        /// <remarks></remarks>
        public bool SetForTime(double tjd)
        {
            double[] m_peb = new double[3], m_veb = new double[3], m_pes = new double[3], m_ves = new double[3];
            // TL.LogMessage("SetForTime", "Start")
            EphemerisCode.get_earth_nov(ref m_EarthEph, tjd, ref m_BaryTime, ref m_peb, ref m_veb, ref m_pes, ref m_ves);
            // TL.LogMessage("SetForTime", "After get_earth_nov")
            EarthTilt(tjd, ref m_MeanOb, ref m_TrueOb, ref m_EquOfEqu, ref m_NutLong, ref m_NutObl);
            // TL.LogMessage("SetForTime", "After earthtilt")
            m_BaryPos.x = m_peb[0];
            m_BaryPos.y = m_peb[1];
            m_BaryPos.z = m_peb[2];
            m_BaryVel.x = m_veb[0];
            m_BaryVel.y = m_veb[1];
            m_BaryVel.z = m_veb[2];

            m_HeliPos.x = m_pes[0];
            m_HeliPos.y = m_pes[1];
            m_HeliPos.z = m_pes[2];
            m_HeliVel.x = m_ves[0];
            m_HeliVel.y = m_ves[1];
            m_HeliVel.z = m_ves[2];

            m_Valid = true;
            // Dim Earth As New bodystruct, POS(2), VEL(2) As Double
            // Earth.name = "Earth"
            // Earth.number = Body.Earth
            // Earth.type = NOVAS2Net.BodyType.MajorPlanet
            // ephemeris(tjd, Earth, Origin.SolarSystemBarycentre, POS, VEL)
            // m_BaryPos.x = POS(0)
            // m_BaryPos.y = POS(1)
            // m_BaryPos.z = POS(2)
            // m_BaryVel.x = VEL(0)
            // m_BaryVel.y = VEL(1)
            // m_BaryVel.z = VEL(2)
            // m_HeliPos.x = 0.0
            // m_HeliPos.y = 0.0
            // m_HeliPos.z = 0.0
            // m_HeliVel.x = 0.0
            // m_HeliVel.y = 0.0
            // m_HeliVel.z = 0.0
            // TL.LogMessage("SetForTime", "BaryPos x" & m_peb(0)) '& " " & POS(0))
            // TL.LogMessage("SetForTime", "BaryPos y" & m_peb(1)) '& " " & POS(1))
            // TL.LogMessage("SetForTime", "BaryPos z" & m_peb(2)) '& " " & POS(2))
            // TL.LogMessage("SetForTime", "BaryVel x" & m_veb(0)) '& " " & VEL(0))
            // TL.LogMessage("SetForTime", "BaryVel y" & m_veb(1)) '& " " & VEL(1))
            // TL.LogMessage("SetForTime", "BaryVel z" & m_veb(2)) '& " " & VEL(2))

            return m_Valid;
        }

        /// <summary>
        /// Earth true obliquity 
        /// </summary>
        /// <value>True obliquity of the ecliptic</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double TrueObliquity
        {
            get
            {
                return m_TrueOb;
            }
        }
    }

    /// <summary>
    /// NOVAS-COM: Provide characteristics of a solar system body
    /// </summary>
    /// <remarks>NOVAS-COM objects of class Planet hold the characteristics of a solar system body. Properties are 
    /// type (major or minor planet), number (for major and numbered minor planets), name (for unnumbered 
    /// minor planets and comets), the ephemeris object to be used for orbital calculations, an optional 
    /// ephemeris object to use for barycenter calculations, and an optional value for delta-T. 
    /// <para>The number values for major planets are 1 to 9 for Mercury to Pluto, 10 for Sun and 11 for Moon. The last two obviously 
    /// aren't planets, but this numbering is a NOVAS convention that enables us to retrieve useful information about these bodies.
    /// </para>
    /// <para>The high-level NOVAS astrometric functions are implemented as methods of Planet: 
    /// GetTopocentricPosition(), GetLocalPosition(), GetApparentPosition(), GetVirtualPosition(), 
    /// and GetAstrometricPosition(). These methods operate on the properties of the Planet, and produce 
    /// a PositionVector object. For example, to get the topocentric coordinates of a planet, create and 
    /// initialize a planet then call 
    /// Planet.GetTopocentricPosition(). The resulting PositionVector's right ascension and declination 
    /// properties are the topocentric equatorial coordinates, at the same time, the (optionally 
    /// refracted) alt-az coordinates are calculated, and are also contained within the returned 
    /// PositionVector. <b>Note that Alt/Az is available in PositionVectors returned from calling 
    /// GetTopocentricPosition().</b> The accuracy of these calculations is typically dominated by the accuracy 
    /// of the attached ephemeris generator. </para>
    /// <para><b>Ephemeris Generator</b><br />
    /// By default, Kepler instances are attached for both Earth and Planet objects so it is
    /// not necessary to create and attach these in order to get Kepler accuracy from this
    /// component</para>
    /// <para>The ephemeris generator object used with NOVAS-COM must support a single 
    /// method GetPositionAndVelocity(tjd). This method must take a terrestrial Julian date (like the 
    /// NOVAS-COM methods) as its single parameter, and return an array of Double 
    /// containing the rectangular (x/y/z) heliocentric J2000 equatorial coordinates of position (AU) and velocity 
    /// (KM/sec.). In addition, it must support three read/write properties BodyType, Name, and Number, 
    /// which correspond to the Type, Name, and Number properties of Novas.Planet. 
    /// </para>
    /// </remarks>
    [Guid("78F157E4-D03D-4efb-8248-745F9C63A850")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Planet : IPlanet
    {

        private double m_deltat;
        private bool m_bDTValid;
        private BodyType m_type;
        private int m_number;
        private string m_name;
        private IEphemeris m_ephobj;
        private int[] m_ephdisps = new int[5], m_earthephdisps = new int[5];
        private IEphemeris m_earthephobj;

        private NOVAS31 Nov31;

        // Private TL As TraceLogger, Utl As Util

        /// <summary>
        /// Create a new instance of the Plant class
        /// </summary>
        /// <remarks>This assigns default Kepler instances for the Earth and Planet objects so it is
        /// not necessary to create and attach Kepler objects in order to get Kepler accuracy from this
        /// component</remarks>
        public Planet()
        {
            m_name = null;
            m_bDTValid = false;
            m_ephobj = new Ephemeris();
            m_earthephobj = new Ephemeris();
            m_earthephobj.BodyType = BodyType.MajorPlanet;
            m_earthephobj.Name = "Earth";
            m_earthephobj.Number = Body.Earth;

            // TL = New TraceLogger("", "NOVASCOMPlanet")
            // TL.Enabled = True
            // TL.LogMessage("New", "Log started")
            // Utl = New Util

            Nov31 = new NOVAS31(); // Create a NOVAS31 object for hanbdling sun and moon calculations

        }
        /// <summary>
        /// Planet delta-T
        /// </summary>
        /// <value>The value of delta-T (TT - UT1) to use for reductions</value>
        /// <returns>Seconds</returns>
        /// <remarks>Setting this value is optional. If no value is set, an internal delta-T generator is used.</remarks>
        public double DeltaT
        {
            get
            {
                if (!m_bDTValid)
                    throw new Exceptions.ValueNotAvailableException("Planet:DeltaT DeltaT is not available");
                return m_deltat;
            }
            set
            {
                m_deltat = value;
                m_bDTValid = true;
            }
        }

        /// <summary>
        /// Ephemeris object used to provide the position of the Earth.
        /// </summary>
        /// <value>Earth ephemeris object</value>
        /// <returns>Earth ephemeris object</returns>
        /// <remarks>
        /// Setting this is optional, if not set, the internal Kepler engine will be used.</remarks>
        public IEphemeris EarthEphemeris
        {
            get
            {
                return m_earthephobj;
            }
            set
            {
                m_earthephobj = value;
            }
        }

        /// <summary>
        /// The Ephemeris object used to provide positions of solar system bodies.
        /// </summary>
        /// <value>Body ephemeris object</value>
        /// <returns>Body ephemeris object</returns>
        /// <remarks>
        /// Setting this is optional, if not set, the internal Kepler engine will be used.
        /// </remarks>
        public IEphemeris Ephemeris
        {
            get
            {
                return m_ephobj;
            }
            set
            {
                m_ephobj = value;
            }
        }

        /// <summary>
        /// Get an apparent position for given time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <returns>PositionVector for the apparent place.</returns>
        /// <remarks></remarks>
        public PositionVector GetApparentPosition(double tjd)
        {
            double tdb = default, t2, t3, lighttime = default;
            double[] peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4], pos1 = new double[4], vel1 = new double[4], pos2 = new double[4], pos3 = new double[4], pos4 = new double[4], pos5 = new double[4], vec = new double[9];
            int iter;
            PositionVector pv;

            Object3 Obj3;
            double RA = default, Dec = default, Dis = default;
            int rc;

            Obj3 = new Object3();

            if (m_type == BodyType.MajorPlanet & (m_number == 10 | m_number == 11)) // Handle Sun and Moon through NOVAS31,all the rest by the original Bob Denny method
            {
                Obj3.Number = CommonCode.NumberToBody(m_number);
                Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

                rc = Nov31.AppPlanet(tjd, Obj3, Accuracy.Full, ref RA, ref Dec, ref Dis); // Get the apparent RA/Dec
                RADec2Vector(RA, Dec, Dis, ref pos1); // Convert to a vector

                pv = new PositionVector(pos1[0], pos1[1], pos1[2], RA, Dec, Dis, Dis / GlobalItems.C); // Create the position vector to return
            }
            else
            {
                //
                // This gets the barycentric terrestrial dynamical time (TDB).
                //
                EphemerisCode.get_earth_nov(ref m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);

                //
                // Get position and velocity of planet wrt barycenter of solar system.
                //

                EphemerisCode.ephemeris_nov(ref m_ephobj, tdb, m_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);

                BaryToGeo(pos1, peb, ref pos2, ref lighttime);
                t3 = tdb - lighttime;

                iter = 0;
                do
                {
                    t2 = t3;
                    EphemerisCode.ephemeris_nov(ref m_ephobj, t2, m_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);
                    BaryToGeo(pos1, peb, ref pos2, ref lighttime);
                    t3 = tdb - lighttime;
                    iter += 1;
                }
                while (Abs(t3 - t2) > 0.000001d & iter < 100);

                //
                // Finish apparent place computation.
                //
                SunField(pos2, pes, ref pos3);
                Aberration(pos3, veb, lighttime, ref pos4);
                Precession(GlobalItems.J2000BASE, pos4, tdb, ref pos5);
                Nutate(tdb, NutationDirection.MeanToTrue, pos5, ref vec);

                pv = new PositionVector();
                pv.x = vec[0];
                pv.y = vec[1];
                pv.z = vec[2];
            }

            return pv;
        }

        /// <summary>
        /// Get an astrometric position for given time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <returns>PositionVector for the astrometric place.</returns>
        /// <remarks></remarks>
        public PositionVector GetAstrometricPosition(double tjd)
        {
            double t2, t3;
            double lighttime = default, tdb = default;
            double[] pos1 = new double[4], vel1 = new double[4], pos2 = new double[4], peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4];
            // Dim tdbe As Double
            int iter;
            // Dim earth As New bodystruct
            PositionVector RetVal;
            Object3 Obj3;
            double RA = default, Dec = default, Dis = default;
            int rc;
            // TL.LogMessage("GetAstrometricPosition", "tjd: " & tjd & ", BodyType: " & m_type.ToString & ", Number: " & Number)
            Obj3 = new Object3();

            if (m_type == BodyType.MajorPlanet & (m_number == 10 | m_number == 11)) // Handle Sun and Moon through NOVAS31,all the rest by the original Bob Denny method
            {
                Obj3.Number = CommonCode.NumberToBody(m_number);
                Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

                rc = Nov31.AstroPlanet(tjd, Obj3, Accuracy.Full, ref RA, ref Dec, ref Dis); // Get the astro RA/Dec
                RADec2Vector(RA, Dec, Dis, ref pos1); // Convert to a vector

                RetVal = new PositionVector(pos1[0], pos1[1], pos1[2], RA, Dec, Dis, Dis / GlobalItems.C); // Create the position vector to return
            }

            else
            {


                // Dim pebe(3), pese(3), vebe(3), vese(3) As Double
                //
                // Get position of the Earth wrt center of Sun and barycenter of the
                // solar system.
                //
                // This also gets the barycentric terrestrial dynamical time (TDB).
                //
                // earth.name = "Earth"
                // earth.number = Body.Earth
                // earth.type = NOVAS2Net.BodyType.MajorPlanet

                // hr = get_earth(tjd, earth, tdbe, pebe, vebe, pese, vese)

                EphemerisCode.get_earth_nov(ref m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);

                // TL.LogMessage("GetAstrometricPosition", "tjd: " & tjd)
                // TL.LogMessage("GetAstrometricPosition", "tdb: " & tdb & " " & tdbe)
                // TL.LogMessage("GetAstrometricPosition", "get_earth peb(0): " & peb(0))
                // TL.LogMessage("GetAstrometricPosition", "get_earth peb(1): " & peb(1))
                // TL.LogMessage("GetAstrometricPosition", "get_earth peb(2): " & peb(2))
                // TL.LogMessage("GetAstrometricPosition", "get_earth veb(0): " & veb(0))
                // TL.LogMessage("GetAstrometricPosition", "get_earth veb(1): " & veb(1))
                // TL.LogMessage("GetAstrometricPosition", "get_earth veb(2): " & veb(2))
                // TL.LogMessage("GetAstrometricPosition", "get_earth pes(0): " & pes(0))
                // TL.LogMessage("GetAstrometricPosition", "get_earth pes(1): " & pes(1))
                // TL.LogMessage("GetAstrometricPosition", "get_earth pes(2): " & pes(2))
                // TL.LogMessage("GetAstrometricPosition", "get_earth ves(0): " & ves(0))
                // TL.LogMessage("GetAstrometricPosition", "get_earth ves(1): " & ves(1))
                // TL.LogMessage("GetAstrometricPosition", "get_earth ves(2): " & ves(2))

                //
                // Get position and velocity of planet wrt barycenter of solar system.
                //

                EphemerisCode.ephemeris_nov(ref m_ephobj, tdb, m_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);
                // TL.LogMessage("GetAstrometricPosition", "tdb: " & tdb)

                BaryToGeo(pos1, peb, ref pos2, ref lighttime);
                t3 = tdb - lighttime;

                iter = 0;
                do
                {
                    t2 = t3;
                    EphemerisCode.ephemeris_nov(ref m_ephobj, t2, m_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);
                    BaryToGeo(pos1, peb, ref pos2, ref lighttime);
                    t3 = tdb - lighttime;
                    iter += 1;
                }
                while (Abs(t3 - t2) > 0.000001d & iter < 100);

                if (iter >= 100)
                    throw new Utilities.Exceptions.HelperException("Planet:GetAstrometricPoition ephemeris_nov did not converge in 100 iterations");

                //
                // pos2 is astrometric place.
                //
                RetVal = new PositionVector();
                RetVal.x = pos2[0];
                RetVal.y = pos2[1];
                RetVal.z = pos2[2];
            }

            return RetVal;

        }

        /// <summary>
        /// Get an local position for given time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <param name="site">The observing site</param>
        /// <returns>PositionVector for the local place.</returns>
        /// <remarks></remarks>
        public PositionVector GetLocalPosition(double tjd, Site site)
        {
            int j, iter;
            var st = default(SiteInfo);
            double t2, t3;
            double gast = default, lighttime = default, ujd, tdb = default, oblm = default, oblt = default, eqeq = default, psi = default, eps = default;
            double[] pog = new double[4], vog = new double[4], pb = new double[4], vb = new double[4], ps = new double[4], vs = new double[4], pos1 = new double[4], vel1 = new double[4], pos2 = new double[4], vel2 = new double[4], pos3 = new double[4], vec = new double[4], peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4];
            PositionVector pv;
            var Obj3 = new Object3();
            var OnSurf = new OnSurface();
            RefractionOption Ref3;
            short rc;
            double ra = default, rra = default, dec = default, rdec = default, az = default, zd = default, dist = default;

            //
            // Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
            //
            if (!m_bDTValid) // April 2012 - correced bug, deltat was not treated as seconds and also adapted to work with Novas31
            {
                m_deltat = DeltatCode.DeltaTCalc(tjd);
            }
            ujd = tjd - m_deltat / 86400.0d;

            //
            // Get the observer's site info
            //
            try
            {
                st.Latitude = site.Latitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available");
            }
            try
            {
                st.Longitude = site.Longitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available");
            }
            try
            {
                st.Height = site.Height;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available");
            }

            if (m_type == BodyType.MajorPlanet & (m_number == 10 | m_number == 11)) // Handle Sun and Moon through NOVAS31,all the rest by the original Bob Denny method
            {

                OnSurf.Height = site.Height;
                OnSurf.Latitude = site.Latitude;
                OnSurf.Longitude = site.Longitude;
                OnSurf.Pressure = site.Pressure;
                OnSurf.Temperature = site.Temperature;

                Obj3.Number = CommonCode.NumberToBody(m_number);
                Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

                Ref3 = RefractionOption.NoRefraction;

                rc = Nov31.LocalPlanet(tjd, Obj3, m_deltat, OnSurf, Accuracy.Full, ref ra, ref dec, ref dist);
                Nov31.Equ2Hor(ujd, m_deltat, Accuracy.Full, 0.0d, 0.0d, OnSurf, ra, dec, Ref3, ref zd, ref az, ref rra, ref rdec);

                RADec2Vector(rra, rdec, dist, ref vec);
                pv = new PositionVector(vec[0], vec[1], vec[2], rra, rdec, dist, dist / GlobalItems.C, az, 90.0d - zd);
            }

            else // Some other planet
            {

                //
                // Get position of Earth wrt the center of the Sun and the barycenter
                // of solar system.
                //
                // This also gets the barycentric terrestrial dynamical time (TDB).
                //
                EphemerisCode.get_earth_nov(ref m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);

                EarthTilt(tdb, ref oblm, ref oblt, ref eqeq, ref psi, ref eps);

                //
                // Get position and velocity of observer wrt center of the Earth.
                //
                SiderealTime(ujd, 0.0d, eqeq, ref gast);
                Terra(ref st, gast, ref pos1, ref vel1);
                Nutate(tdb, NutationDirection.TrueToMean, pos1, ref pos2);
                Nov31.Precession(tdb, pos2, GlobalItems.J2000BASE, ref pog);

                Nutate(tdb, NutationDirection.TrueToMean, vel1, ref vel2);
                Nov31.Precession(tdb, vel2, GlobalItems.J2000BASE, ref vog);

                //
                // Get position and velocity of observer wrt barycenter of solar 
                // system and wrt center of the sun.
                //
                for (j = 0; j <= 2; j++)
                {
                    pb[j] = peb[j] + pog[j];
                    vb[j] = veb[j] + vog[j];
                    ps[j] = pes[j] + pog[j];
                    vs[j] = ves[j] + vog[j];
                }

                //
                // Get position of planet wrt barycenter of solar system.
                //
                EphemerisCode.ephemeris_nov(ref m_ephobj, tdb, m_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);

                BaryToGeo(pos1, pb, ref pos2, ref lighttime);
                t3 = tdb - lighttime;

                iter = 0;
                do
                {
                    t2 = t3;
                    EphemerisCode.ephemeris_nov(ref m_ephobj, t2, m_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);
                    BaryToGeo(pos1, pb, ref pos2, ref lighttime);
                    t3 = tdb - lighttime;
                    iter += 1;
                }
                while (Abs(t3 - t2) > 0.000001d & iter < 100);

                if (iter >= 100)
                    throw new Utilities.Exceptions.HelperException("Planet:GetLocalPoition ephemeris_nov did not converge in 100 iterations");

                //
                // Finish local place calculation.
                //
                SunField(pos2, ps, ref pos3);
                Aberration(pos3, vb, lighttime, ref vec);

                pv = new PositionVector();
                pv.x = vec[0];
                pv.y = vec[1];
                pv.z = vec[2];
            }

            return pv;
        }

        /// <summary>
        /// Get a topocentric position for given time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <param name="site">The observing site</param>
        /// <param name="Refract">Apply refraction correction</param>
        /// <returns>PositionVector for the topocentric place.</returns>
        /// <remarks></remarks>
        public PositionVector GetTopocentricPosition(double tjd, Site site, bool Refract)
        {
            short j;
            int iter;
            RefractionOption @ref;
            var st = default(SiteInfo);
            double ujd, t2, t3, gast = default, lighttime = default, tdb = default, oblm = default, oblt = default, eqeq = default, psi = default, eps = default;
            double[] pos1 = new double[4], pos2 = new double[4], pos4 = new double[4], pos5 = new double[4], pos6 = new double[4], vel1 = new double[4], vel2 = new double[4], pog = new double[4], vog = new double[4], pob = new double[4], vec = new double[4], vob = new double[4], pos = new double[4], peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4];
            double ra = default, rra = default, dec = default, rdec = default, az = default, zd = default, dist = default;
            bool wx;
            PositionVector pv;

            var Obj3 = new Object3();
            var OnSurf = new OnSurface();
            RefractionOption Ref3;
            short rc;

            //
            // Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
            //
            if (!m_bDTValid) // April 2012 - correced bug, deltat was not treated as seconds and also adapted to work with Novas31
            {
                m_deltat = DeltatCode.DeltaTCalc(tjd);
            }
            ujd = tjd - m_deltat / 86400.0d;

            //
            // Get the observer's site info
            //
            try
            {
                st.Latitude = site.Latitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available");
            }
            try
            {
                st.Longitude = site.Longitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available");
            }
            try
            {
                st.Height = site.Height;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available");
            }

            if (m_type == BodyType.MajorPlanet & (m_number == 10 | m_number == 11)) // Handle Sun and Moon through NOVAS31,all the rest by the original Bob Denny method
            {

                OnSurf.Height = site.Height;
                OnSurf.Latitude = site.Latitude;
                OnSurf.Longitude = site.Longitude;
                OnSurf.Pressure = site.Pressure;
                OnSurf.Temperature = site.Temperature;

                Obj3.Number = CommonCode.NumberToBody(m_number);
                Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

                if (Refract)
                {
                    Ref3 = RefractionOption.LocationRefraction;
                }
                else
                {
                    Ref3 = RefractionOption.NoRefraction;
                }

                rc = Nov31.TopoPlanet(tjd, Obj3, m_deltat, OnSurf, Accuracy.Full, ref ra, ref dec, ref dist);
                Nov31.Equ2Hor(ujd, m_deltat, Accuracy.Full, 0.0d, 0.0d, OnSurf, ra, dec, Ref3, ref zd, ref az, ref rra, ref rdec);

                RADec2Vector(rra, rdec, dist, ref vec);
                pv = new PositionVector(vec[0], vec[1], vec[2], rra, rdec, dist, dist / GlobalItems.C, az, 90.0d - zd);
            }

            else // Some other planet
            {


                //
                // Compute position and velocity of the observer, on mean equator
                // and equinox of J2000.0, wrt the solar system barycenter and
                // wrt to the center of the Sun. 
                //
                // This also gets the barycentric terrestrial dynamical time (TDB).
                //
                EphemerisCode.get_earth_nov(ref m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);

                EarthTilt(tdb, ref oblm, ref oblt, ref eqeq, ref psi, ref eps);

                //
                // Get position and velocity of observer wrt center of the Earth.
                //
                SiderealTime(ujd, 0.0d, eqeq, ref gast);
                Terra(ref st, gast, ref pos1, ref vel1);
                Nutate(tdb, NutationDirection.TrueToMean, pos1, ref pos2);
                Nov31.Precession(tdb, pos2, GlobalItems.J2000BASE, ref pog);

                Nutate(tdb, NutationDirection.TrueToMean, vel1, ref vel2);
                Nov31.Precession(tdb, vel2, GlobalItems.J2000BASE, ref vog);

                //
                // Get position and velocity of observer wrt barycenter of solar system
                // and wrt center of the sun.
                //
                for (j = 0; j <= 2; j++)
                {

                    pob[j] = peb[j] + pog[j];
                    vob[j] = veb[j] + vog[j];
                    pos[j] = pes[j] + pog[j];
                }

                // 
                // Compute the apparent place of the planet using the position and
                // velocity of the observer.
                //
                // First, get the position of the planet wrt barycenter of solar system.
                //
                EphemerisCode.ephemeris_nov(ref m_ephobj, tdb, m_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);

                BaryToGeo(pos1, pob, ref pos2, ref lighttime);
                t3 = tdb - lighttime;

                iter = 0;
                do
                {
                    t2 = t3;
                    EphemerisCode.ephemeris_nov(ref m_ephobj, t2, m_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);
                    BaryToGeo(pos1, pob, ref pos2, ref lighttime);
                    t3 = tdb - lighttime;
                    iter += 1;
                }
                while (Abs(t3 - t2) > 0.000001d & iter < 100);

                if (iter >= 100)
                    throw new Utilities.Exceptions.HelperException("Planet:GetTopocentricPoition ephemeris_nov did not converge in 100 iterations");

                //
                // Finish topocentric place calculation.
                //
                SunField(pos2, pos, ref pos4);
                Aberration(pos4, vob, lighttime, ref pos5);
                Precession(GlobalItems.J2000BASE, pos5, tdb, ref pos6);
                Nutate(tdb, NutationDirection.MeanToTrue, pos6, ref vec);

                //
                // Calculate equatorial coordinates and distance
                //
                Vector2RADec(vec, ref ra, ref dec); // Get topo RA/Dec
                dist = Sqrt(Pow(vec[0], 2.0d) + Pow(vec[1], 2.0d) + Pow(vec[2], 2.0d)); // And dist

                //
                // Refract if requested
                //
                @ref = RefractionOption.NoRefraction; // Assume no refraction
                if (Refract)
                {
                    wx = true; // Assume site weather
                    try
                    {
                        st.Temperature = site.Temperature;
                    }
                    catch (Exception ex) // Value unset so use standard refraction option
                    {
                        wx = false;
                    }
                    try
                    {
                        st.Pressure = site.Pressure;
                    }
                    catch (Exception ex) // Value unset so use standard refraction option
                    {
                        wx = false;
                    }
                    if (wx) // Set refraction option
                    {
                        @ref = RefractionOption.LocationRefraction;
                    }
                    else
                    {
                        @ref = RefractionOption.StandardRefraction;
                    }
                }
                //
                // This calculates Alt/Az coordinates. If ref > 0 then it refracts
                // both the computed Alt/Az and the RA/Dec coordinates.
                //
                if (m_bDTValid)
                {
                    Equ2Hor(tjd, m_deltat, 0.0d, 0.0d, ref st, ra, dec, @ref, ref zd, ref az, ref rra, ref rdec);
                }
                else
                {
                    Equ2Hor(tjd, DeltatCode.DeltaTCalc(tjd), 0.0d, 0.0d, ref st, ra, dec, @ref, ref zd, ref az, ref rra, ref rdec);
                }

                //
                // If we refracted, we now must compute new cartesian components
                // Distance does not change...
                //
                if (@ref != RefractionOption.NoRefraction)
                    RADec2Vector(rra, rdec, dist, ref vec); // If refracted, recompute New refracted vector

                // Create a new positionvector with calculated values
                pv = new PositionVector(vec[0], vec[1], vec[2], rra, rdec, dist, dist / GlobalItems.C, az, 90.0d - zd);

            }


            return pv;

        }
        /// <summary>
        /// Get a virtual position for given time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <returns>PositionVector for the virtual place.</returns>
        /// <remarks></remarks>
        public PositionVector GetVirtualPosition(double tjd)
        {
            double t2, t3;
            double lighttime = default, tdb = default, oblm = default, oblt = default, eqeq = default, psi = default, eps = default;
            double[] pos1 = new double[4], vel1 = new double[4], pos2 = new double[4], pos3 = new double[4], vec = new double[4], peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4];
            int iter;
            var pv = new PositionVector();
            Object3 Obj3;
            double RA = default, Dec = default, Dis = default;
            int rc;

            Obj3 = new Object3();

            if (m_type == BodyType.MajorPlanet & (m_number == 10 | m_number == 11)) // Handle Sun and Moon through NOVAS31,all the rest by the original Bob Denny method
            {
                Obj3.Number = CommonCode.NumberToBody(m_number);
                Obj3.Type = ObjectType.MajorPlanetSunOrMoon;

                rc = Nov31.VirtualPlanet(tjd, Obj3, Accuracy.Full, ref RA, ref Dec, ref Dis); // Get the astro RA/Dec
                RADec2Vector(RA, Dec, Dis, ref pos1); // Convert to a vector

                pv = new PositionVector(pos1[0], pos1[1], pos1[2], RA, Dec, Dis, Dis / GlobalItems.C); // Create the position vector to return
            }

            else
            {

                //
                // Get position nd velocity of Earth wrt barycenter of solar system.
                //
                //
                // This also gets the barycentric terrestrial dynamical time (TDB).
                //
                EphemerisCode.get_earth_nov(ref m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);

                EarthTilt(tdb, ref oblm, ref oblt, ref eqeq, ref psi, ref eps);

                //
                // Get position and velocity of planet wrt barycenter of solar system.
                //

                var km_type = default(BodyType);
                switch (m_type)
                {
                    case BodyType.Comet:
                        {
                            km_type = BodyType.Comet;
                            break;
                        }
                    case BodyType.MajorPlanet:
                        {
                            km_type = BodyType.MajorPlanet;
                            break;
                        }
                    case BodyType.MinorPlanet:
                        {
                            km_type = BodyType.MinorPlanet;
                            break;
                        }
                }

                EphemerisCode.ephemeris_nov(ref m_ephobj, tdb, km_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);
                BaryToGeo(pos1, peb, ref pos2, ref lighttime);

                t3 = tdb - lighttime;

                iter = 0;
                do
                {
                    t2 = t3;
                    EphemerisCode.ephemeris_nov(ref m_ephobj, t2, km_type, m_number, m_name, Origin.Barycentric, ref pos1, ref vel1);
                    BaryToGeo(pos1, peb, ref pos2, ref lighttime);
                    t3 = tdb - lighttime;
                    iter += 1;
                }
                while (Abs(t3 - t2) > 0.000001d & iter < 100);

                if (iter >= 100)
                    throw new Utilities.Exceptions.HelperException("Planet:GetVirtualPoition ephemeris_nov did not converge in 100 iterations");

                //
                // Finish virtual place computation.
                //
                SunField(pos2, pes, ref pos3);

                Aberration(pos3, veb, lighttime, ref vec);

                pv.x = vec[0];
                pv.y = vec[1];
                pv.z = vec[2];
            }

            return pv;

        }

        /// <summary>
        /// Planet name
        /// </summary>
        /// <value>For unnumbered minor planets, (Type=nvMinorPlanet and Number=0), the packed designation 
        /// for the minor planet. For other types, this is not significant, but may be used to store 
        /// a name.</value>
        /// <returns>Name of planet</returns>
        /// <remarks></remarks>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// Planet number
        /// </summary>
        /// <value>For major planets (Type = <see cref="BodyType.MajorPlanet" />, a PlanetNumber value from 1 to 11. For minor planets 
        /// (Type = <see cref="BodyType.MinorPlanet" />, the number of the minor planet or 0 for unnumbered minor planet.</value>
        /// <returns>Planet number</returns>
        /// <remarks>The major planet number is its number out from the sun starting with Mercury = 1, ending at Pluto = 9. Planet 10 gives 
        /// values for the Sun and planet 11 gives values for the Moon</remarks>
        public int Number
        {
            get
            {
                return m_number;
            }
            set
            {
                // April 2012 - corrected to disallow planet number 0
                if (m_type == BodyType.MajorPlanet & (value < 1 | value > 11))
                    throw new Utilities.Exceptions.InvalidValueException("Planet.Number MajorPlanet number is < 1 or > 11 - " + value);
                m_number = value;
            }
        }

        /// <summary>
        /// The type of solar system body
        /// </summary>
        /// <value>The type of solar system body</value>
        /// <returns>Value from the BodyType enum</returns>
        /// <remarks></remarks>
        public BodyType Type
        {
            get
            {
                return m_type;
            }
            set
            {
                if ((int)value < 0 | (int)value > 2)
                    throw new Utilities.Exceptions.InvalidValueException("Planet.Type BodyType is < 0 or > 2: " + ((int)value).ToString());
                m_type = value;
            }
        }
    }

    /// <summary>
    /// NOVAS-COM: PositionVector Class
    /// </summary>
    /// <remarks>NOVAS-COM objects of class PositionVector contain vectors used for positions (earth, sites, 
    /// stars and planets) throughout NOVAS-COM. Of course, its properties include the x, y, and z 
    /// components of the position. Additional properties are right ascension and declination, distance, 
    /// and light time (applicable to star positions), and Alt/Az (available only in PositionVectors 
    /// returned by Star or Planet methods GetTopocentricPosition()). You can initialize a PositionVector 
    /// from a Star object (essentially an FK5 or HIP catalog entry) or a Site (lat/long/height). 
    /// PositionVector has methods that can adjust the coordinates for precession, aberration and 
    /// proper motion. Thus, a PositionVector object gives access to some of the lower-level NOVAS functions. 
    /// <para><b>Note:</b> The equatorial coordinate properties of this object are dependent variables, and thus are read-only. Changing any cartesian coordinate will cause the equatorial coordinates to be recalculated. 
    /// </para></remarks>
    [Guid("8D8B7043-49AA-40be-881F-0EC5D8E2213D")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class PositionVector : IPositionVector, IPositionVectorExtra
    {
        private bool xOk, yOk, zOk, RADecOk, AzElOk;
        private double[] PosVec = new double[3];
        private double m_RA, m_DEC, m_Dist, m_Light, m_Alt, m_Az;
        private NOVAS31 Nov31 = new NOVAS31();

        /// <summary>
        /// Create a new, uninitialised position vector
        /// </summary>
        /// <remarks></remarks>
        public PositionVector()
        {
            xOk = false;
            yOk = false;
            zOk = false;
            RADecOk = false;
            AzElOk = false;
        }

        /// <summary>
        /// Create a new position vector with supplied initial values
        /// </summary>
        /// <param name="x">Position vector x co-ordinate</param>
        /// <param name="y">Position vector y co-ordinate</param>
        /// <param name="z">Position vector z co-ordinate</param>
        /// <param name="RA">Right ascension (hours)</param>
        /// <param name="DEC">Declination (degrees)</param>
        /// <param name="Distance">Distance to object</param>
        /// <param name="Light">Light-time to object</param>
        /// <param name="Azimuth">Object azimuth</param>
        /// <param name="Altitude">Object altitude</param>
        /// <remarks></remarks>
        public PositionVector(double x, double y, double z, double RA, double DEC, double Distance, double Light, double Azimuth, double Altitude)
        {
            PosVec[0] = x;
            xOk = true;
            PosVec[1] = y;
            yOk = true;
            PosVec[2] = z;
            zOk = true;
            m_RA = RA;
            m_DEC = DEC;
            RADecOk = true;
            m_Dist = Distance;
            m_Light = Light;
            m_Az = Azimuth;
            m_Alt = Altitude;
            AzElOk = true;
        }

        /// <summary>
        /// Create a new position vector with supplied initial values
        /// </summary>
        /// <param name="x">Position vector x co-ordinate</param>
        /// <param name="y">Position vector y co-ordinate</param>
        /// <param name="z">Position vector z co-ordinate</param>
        /// <param name="RA">Right ascension (hours)</param>
        /// <param name="DEC">Declination (degrees)</param>
        /// <param name="Distance">Distance to object</param>
        /// <param name="Light">Light-time to object</param>
        /// <remarks></remarks>
        public PositionVector(double x, double y, double z, double RA, double DEC, double Distance, double Light)
        {
            PosVec[0] = x;
            xOk = true;
            PosVec[1] = y;
            yOk = true;
            PosVec[2] = z;
            zOk = true;
            m_RA = RA;
            m_DEC = DEC;
            RADecOk = true;
            m_Dist = Distance;
            m_Light = Light;
            AzElOk = false;
        }

        /// <summary>
        /// Adjust the position vector of an object for aberration of light
        /// </summary>
        /// <param name="vel">The velocity vector of the observer</param>
        /// <remarks>The algorithm includes relativistic terms</remarks>
        public void Aberration(VelocityVector vel)
        {
            double[] p = new double[3], v = new double[3];
            if (!(xOk & yOk & zOk))
                throw new Exceptions.ValueNotSetException("PositionVector:ProperMotion x, y or z has not been set");
            CheckEq();
            p[0] = PosVec[0];
            p[1] = PosVec[1];
            p[2] = PosVec[2];

            try
            {
                v[0] = vel.x;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:Aberration VelocityVector.x is not available");
            }
            try
            {
                v[1] = vel.y;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:Aberration VelocityVector.y is not available");

            }
            try
            {
                v[2] = vel.z;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:Aberration VelocityVector.z is not available");
            }
            NOVAS2.Aberration(p, v, m_Light, ref PosVec);
            RADecOk = false;
            AzElOk = false;
        }

        /// <summary>
        /// The azimuth coordinate (degrees, + east)
        /// </summary>
        /// <value>The azimuth coordinate</value>
        /// <returns>Degrees, + East</returns>
        /// <remarks></remarks>
        public double Azimuth
        {
            get
            {
                if (!AzElOk)
                    throw new Exceptions.ValueNotAvailableException("PositionVector:Azimuth Azimuth is not available");
                return m_Az;
            }
        }

        /// <summary>
        /// Declination coordinate
        /// </summary>
        /// <value>Declination coordinate</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double Declination
        {
            get
            {
                if (!(xOk & yOk & zOk))
                    throw new Exceptions.ValueNotSetException("PositionVector:Declination x, y or z has not been set");
                CheckEq();
                return m_DEC;
            }
        }

        /// <summary>
        /// Distance/Radius coordinate
        /// </summary>
        /// <value>Distance/Radius coordinate</value>
        /// <returns>AU</returns>
        /// <remarks></remarks>
        public double Distance
        {
            get
            {
                if (!(xOk & yOk & zOk))
                    throw new Exceptions.ValueNotSetException("PositionVector:Distance x, y or z has not been set");
                CheckEq();
                return m_Dist;
            }
        }

        /// <summary>
        /// The elevation (altitude) coordinate (degrees, + up)
        /// </summary>
        /// <value>The elevation (altitude) coordinate (degrees, + up)</value>
        /// <returns>(Degrees, + up</returns>
        /// <remarks>Elevation is available only in PositionVectors returned from calls to 
        /// Star.GetTopocentricPosition() and/or Planet.GetTopocentricPosition(). </remarks>
        /// <exception cref="Exceptions.ValueNotAvailableException">When the position vector has not been 
        /// initialised from Star.GetTopoCentricPosition and Planet.GetTopocentricPosition</exception>
        public double Elevation
        {
            get
            {
                if (!AzElOk)
                    throw new Exceptions.ValueNotAvailableException("PositionVector:Elevation Elevation is not available");
                return m_Alt;
            }
        }

        /// <summary>
        /// Light time from body to origin, days.
        /// </summary>
        /// <value>Light time from body to origin</value>
        /// <returns>Days</returns>
        /// <remarks></remarks>
        public double LightTime
        {
            get
            {
                if (!(xOk & yOk & zOk))
                    throw new Exceptions.ValueNotSetException("PositionVector:LightTime x, y or z has not been set");
                CheckEq();
                return m_Light;
            }
        }

        /// <summary>
        /// Adjust the position vector for precession of equinoxes between two given epochs
        /// </summary>
        /// <param name="tjd">The first epoch (Terrestrial Julian Date)</param>
        /// <param name="tjd2">The second epoch (Terrestrial Julian Date)</param>
        /// <remarks>The coordinates are referred to the mean equator and equinox of the two respective epochs.</remarks>
        public void Precess(double tjd, double tjd2)
        {
            var p = new double[3];
            if (!(xOk & yOk & zOk))
                throw new Exceptions.ValueNotSetException("PositionVector:Precess x, y or z has not been set");
            p[0] = PosVec[0];
            p[1] = PosVec[1];
            p[2] = PosVec[2];
            Nov31.Precession(tjd, p, tjd2, ref PosVec);
            RADecOk = false;
            AzElOk = false;
        }

        /// <summary>
        /// Adjust the position vector for proper motion (including foreshortening effects)
        /// </summary>
        /// <param name="vel">The velocity vector of the object</param>
        /// <param name="tjd1">The first epoch (Terrestrial Julian Date)</param>
        /// <param name="tjd2">The second epoch (Terrestrial Julian Date)</param>
        /// <returns>True if successful or throws an exception.</returns>
        /// <remarks></remarks>
        /// <exception cref="Exceptions.ValueNotSetException">If the position vector x, y or z values has not been set</exception>
        /// <exception cref="Exceptions.ValueNotAvailableException">If the supplied velocity vector does not have valid x, y and z components</exception>
        public bool ProperMotion(VelocityVector vel, double tjd1, double tjd2)
        {
            double[] p = new double[3], v = new double[3];
            if (!(xOk & yOk & zOk))
                throw new Exceptions.ValueNotSetException("PositionVector:ProperMotion x, y or z has not been set");
            p[0] = PosVec[0];
            p[1] = PosVec[1];
            p[2] = PosVec[2];
            try
            {
                v[0] = vel.x;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.x is not available");
            }
            try
            {
                v[1] = vel.y;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.y is not available");
            }
            try
            {
                v[2] = vel.z;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.z is not available");
            }
            NOVAS2.ProperMotion(tjd1, p, v, tjd2, ref PosVec);
            RADecOk = false;
            AzElOk = false;
            return default;
        }

        /// <summary>
        /// RightAscension coordinate, hours
        /// </summary>
        /// <value>RightAscension coordinate</value>
        /// <returns>Hours</returns>
        /// <remarks></remarks>
        public double RightAscension
        {
            get
            {
                if (!(xOk & yOk & zOk))
                    throw new Exceptions.ValueNotSetException("PositionVector:RA x, y or z has not been set");
                CheckEq();
                return m_RA;
            }
        }

        /// <summary>
        /// Initialize the PositionVector from a Site object and Greenwich apparent sidereal time.
        /// </summary>
        /// <param name="site">The Site object from which to initialize</param>
        /// <param name="gast">Greenwich Apparent Sidereal Time</param>
        /// <returns>True if successful or throws an exception</returns>
        /// <remarks>The GAST parameter must be for Greenwich, not local. The time is rotated through the 
        /// site longitude. See SetFromSiteJD() for an equivalent method that takes UTC Julian Date and 
        /// Delta-T (eliminating the need for calculating hyper-accurate GAST yourself).</remarks>
        public bool SetFromSite(Site site, double gast)
        {
            const double f = 0.00335281d; // f = Earth ellipsoid flattening
            double df2, t, sinphi, cosphi, c, s, ach, ash, stlocl, sinst, cosst;

            //
            // Compute parameters relating to geodetic to geocentric conversion.
            //
            df2 = Pow(1.0d - f, 2d);
            try
            {
                t = site.Latitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:SetFromSite Site.Latitude is not available");
            }

            t = GlobalItems.DEG2RAD * t;
            sinphi = Sin(t);
            cosphi = Cos(t);
            c = 1.0d / Sqrt(Pow(cosphi, 2.0d) + df2 * Pow(sinphi, 2.0d));
            s = df2 * c;

            try
            {
                t = site.Height;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:SetFromSite Site.Height is not available");
            }
            t /= 1000d; // Elevation in KM
            ach = GlobalItems.EARTHRAD * c + t;
            ash = GlobalItems.EARTHRAD * s + t;

            //
            // Compute local sidereal time factors at the observer's longitude.
            //
            try
            {
                t = site.Longitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:SetFromSite Site.Height is not available");
            }

            stlocl = (gast * 15.0d + t) * GlobalItems.DEG2RAD;
            sinst = Sin(stlocl);
            cosst = Cos(stlocl);

            //
            // Compute position vector components in AU
            //

            PosVec[0] = ach * cosphi * cosst / GlobalItems.KMAU;
            PosVec[1] = ach * cosphi * sinst / GlobalItems.KMAU;
            PosVec[2] = ash * sinphi / GlobalItems.KMAU;

            RADecOk = false; // These really aren't inteersting anyway for site vector
            AzElOk = false;

            xOk = true; // Object is valid
            yOk = true;
            zOk = true;
            return default;
        }

        /// <summary>
        /// Initialize the PositionVector from a Site object using UTC Julian date
        /// </summary>
        /// <param name="site">The Site object from which to initialize</param>
        /// <param name="ujd">UTC Julian Date</param>
        /// <returns>True if successful or throws an exception</returns>
        /// <remarks>The Julian date must be UTC Julian date, not terrestrial. Calculations will use the internal delta-T tables and estimator to get 
        /// delta-T. 
        /// This overload is not available through COM, please use 
        /// "SetFromSiteJD(ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double)"
        /// with delta_t set to 0.0 to achieve this effect.
        /// </remarks>
        [ComVisible(false)]
        public bool SetFromSiteJD(Site site, double ujd)
        {
            SetFromSiteJD(site, ujd, 0.0d);
            return default;
        }

        /// <summary>
        /// Initialize the PositionVector from a Site object using UTC Julian date and Delta-T
        /// </summary>
        /// <param name="site">The Site object from which to initialize</param>
        /// <param name="ujd">UTC Julian Date</param>
        /// <param name="delta_t">The value of Delta-T (TT - UT1) to use for reductions (seconds)</param>
        /// <returns>True if successful or throws an exception</returns>
        /// <remarks>The Julian date must be UTC Julian date, not terrestrial.</remarks>
        public bool SetFromSiteJD(Site site, double ujd, double delta_t)
        {
            double dummy = default, secdiff = default, tdb, tjd, gast = default;
            double oblm = default, oblt = default, eqeq = default, psi = default, eps = default;

            //
            // Convert UTC Julian date to Terrestrial Julian Date then
            // convert that to barycentric for earthtilt(), which we use
            // to get the equation of equinoxes for sidereal_time(). Note
            // that we're using UJD as input to the deltat(), but that is
            // OK as the difference in time (~70 sec) is insignificant.
            // For precise applications, the caller must specify delta_t.
            //
            // tjd = ujd + ((delta_t != 0.0) ? delta_t : deltat(ujd))
            if (delta_t != 0.0d)
            {
                tjd = ujd + delta_t;
            }
            else
            {
                tjd = ujd + DeltatCode.DeltaTCalc(ujd);
            }


            Tdb2Tdt(tjd, ref dummy, ref secdiff);
            tdb = tjd + secdiff / 86400.0d;
            EarthTilt(tdb, ref oblm, ref oblt, ref eqeq, ref psi, ref eps);

            //
            // Get the Greenwich Apparent Sidereal Time and call our
            // SetFromSite() method.
            //
            SiderealTime(ujd, 0.0d, eqeq, ref gast);
            SetFromSite(site, gast);
            return default;

        }

        /// <summary>
        /// Initialize the PositionVector from a Star object.
        /// </summary>
        /// <param name="star">The Star object from which to initialize</param>
        /// <returns>True if successful or throws an exception</returns>
        /// <remarks></remarks>
        /// <exception cref="Exceptions.ValueNotAvailableException">If Parallax, RightAScension or Declination is not available in the supplied star object.</exception>
        public bool SetFromStar(Star star)
        {
            double paralx, r, d, cra, sra, cdc, sdc;

            //
            // If parallax is unknown, undetermined, or zero, set it to 1e-7 second
            // of arc, corresponding to a distance of 10 megaparsecs.
            //
            try
            {
                paralx = star.Parallax;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:SetFromStar Star.Parallax is not available");
            }

            if (paralx <= 0.0d)
                paralx = 0.0000001d;

            //
            // Convert right ascension, declination, and parallax to position vector
            // in equatorial system with units of AU.
            //
            m_Dist = GlobalItems.RAD2SEC / paralx;
            try
            {
                m_RA = star.RightAscension;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:SetFromStar Star.RightAscension is not available");
            }

            r = m_RA * 15.0d * GlobalItems.DEG2RAD; // hrs -> deg -> rad
            try
            {
                m_DEC = star.Declination;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("PositionVector:SetFromStar Star.Declination is not available");
            }

            d = m_DEC * GlobalItems.DEG2RAD; /// deg -> rad

            cra = Cos(r);
            sra = Sin(r);
            cdc = Cos(d);
            sdc = Sin(d);

            PosVec[0] = m_Dist * cdc * cra;
            PosVec[1] = m_Dist * cdc * sra;
            PosVec[2] = m_Dist * sdc;

            RADecOk = true; // Object is valid
            xOk = true;
            yOk = true;
            zOk = true;
            return default;
        }

        /// <summary>
        /// Position cartesian x component
        /// </summary>
        /// <value>Cartesian x component</value>
        /// <returns>Cartesian x component</returns>
        /// <remarks></remarks>
        public double x
        {
            get
            {
                if (!xOk)
                    throw new Exceptions.ValueNotSetException("PositionVector:x has not been set");
                return PosVec[0];
            }
            set
            {
                PosVec[0] = value;
                xOk = true;
                RADecOk = false;
                AzElOk = false;
            }
        }

        /// <summary>
        /// Position cartesian y component
        /// </summary>
        /// <value>Cartesian y component</value>
        /// <returns>Cartesian y component</returns>
        /// <remarks></remarks>
        public double y
        {
            get
            {
                if (!yOk)
                    throw new Exceptions.ValueNotSetException("PositionVector:y has not been set");
                return PosVec[1];
            }
            set
            {
                PosVec[1] = value;
                yOk = true;
                RADecOk = false;
                AzElOk = false;
            }
        }

        /// <summary>
        /// Position cartesian z component
        /// </summary>
        /// <value>Cartesian z component</value>
        /// <returns>Cartesian z component</returns>
        /// <remarks></remarks>
        public double z
        {
            get
            {
                if (!zOk)
                    throw new Exceptions.ValueNotSetException("PositionVector:z has not been set");
                return PosVec[2];
            }
            set
            {
                PosVec[2] = value;
                zOk = true;
                RADecOk = false;
                AzElOk = false;
            }
        }

        #region PositionVector Support Code
        private void CheckEq()
        {
            if (RADecOk)
                return; // Equatorial data already OK
            Vector2RADec(PosVec, ref m_RA, ref m_DEC); // Calculate RA/Dec
            m_Dist = Sqrt(Pow(PosVec[0], 2d) + Pow(PosVec[1], 2d) + Pow(PosVec[2], 2d));
            m_Light = m_Dist / GlobalItems.C;
            RADecOk = true;
        }
        #endregion
    }

    /// <summary>
    /// NOVAS-COM: Site Class
    /// </summary>
    /// <remarks>NOVAS-COM objects of class Site contain the specifications for an observer's location on the Earth 
    /// ellipsoid. Properties are latitude, longitude, height above mean sea level, the ambient temperature 
    /// and the sea-level barmetric pressure. The latter two are used only for optional refraction corrections. 
    /// Latitude and longitude are (common) geodetic, not geocentric. </remarks>
    [Guid("46ACFBCE-4EEE-496d-A4B6-7A5FDDD8F969")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Site : ISite
    {
        private double vHeight, vLatitude, vLongitude, vPressure, vTemperature;
        private bool HeightValid, LatitudeValid, LongitudeValid, PressureValid, TemperatureValid;

        /// <summary>
        /// Initialises a new site object
        /// </summary>
        /// <remarks></remarks>
        public Site()
        {
            HeightValid = false;
            LatitudeValid = false;
            LongitudeValid = false;
            PressureValid = false;
            TemperatureValid = false;
        }

        /// <summary>
        /// Height above mean sea level
        /// </summary>
        /// <value>Height above mean sea level</value>
        /// <returns>Meters</returns>
        /// <remarks></remarks>
        public double Height
        {
            get
            {
                if (!HeightValid)
                    throw new Exceptions.ValueNotSetException("Height has not yet been set");
                return vHeight;
            }
            set
            {
                vHeight = value;
                HeightValid = true;
            }
        }

        /// <summary>
        /// Geodetic latitude (degrees, + north)
        /// </summary>
        /// <value>Geodetic latitude</value>
        /// <returns>Degrees, + north</returns>
        /// <remarks></remarks>
        public double Latitude
        {
            get
            {
                if (!LatitudeValid)
                    throw new Exceptions.ValueNotSetException("Latitude has not yet been set");
                return vLatitude;
            }
            set
            {
                vLatitude = value;
                LatitudeValid = true;
            }
        }

        /// <summary>
        /// Geodetic longitude (degrees, + east)
        /// </summary>
        /// <value>Geodetic longitude</value>
        /// <returns>Degrees, + east</returns>
        /// <remarks></remarks>
        public double Longitude
        {
            get
            {
                if (!LongitudeValid)
                    throw new Exceptions.ValueNotSetException("Longitude has not yet been set");
                return vLongitude;
            }
            set
            {
                vLongitude = value;
                LongitudeValid = true;
            }
        }

        /// <summary>
        /// Barometric pressure (millibars)
        /// </summary>
        /// <value>Barometric pressure</value>
        /// <returns>Millibars</returns>
        /// <remarks></remarks>
        public double Pressure
        {
            get
            {
                if (!PressureValid)
                    throw new Exceptions.ValueNotSetException("Pressure has not yet been set");
                return vPressure;
            }
            set
            {
                vPressure = value;
                PressureValid = true;
            }
        }

        /// <summary>
        /// Set all site properties in one method call
        /// </summary>
        /// <param name="Latitude">The geodetic latitude (degrees, + north)</param>
        /// <param name="Longitude">The geodetic longitude (degrees, +east)</param>
        /// <param name="Height">Height above sea level (meters)</param>
        /// <remarks></remarks>
        public void Set(double Latitude, double Longitude, double Height)
        {
            vLatitude = Latitude;
            vLongitude = Longitude;
            vHeight = Height;
            LatitudeValid = true;
            LongitudeValid = true;
            HeightValid = true;
        }

        /// <summary>
        /// Ambient temperature (deg. Celsius)
        /// </summary>
        /// <value>Ambient temperature</value>
        /// <returns>Degrees Celsius)</returns>
        /// <remarks></remarks>
        public double Temperature
        {
            get
            {
                if (!TemperatureValid)
                    throw new Exceptions.ValueNotSetException("Temperature has not yet been set");
                return vTemperature;
            }
            set
            {
                vTemperature = value;
                TemperatureValid = true;
            }
        }
    }

    /// <summary>
    /// NOVAS-COM: Star Class
    /// </summary>
    /// <remarks>NOVAS-COM objects of class Star contain the specifications for a star's catalog position in either FK5 or Hipparcos units (both must be J2000). Properties are right ascension and declination, proper motions, parallax, radial velocity, catalog type (FK5 or HIP), catalog number, optional ephemeris engine to use for barycenter calculations, and an optional value for delta-T. Unless you specifically set the DeltaT property, calculations performed by this class which require the value of delta-T (TT - UT1) rely on an internal function to estimate delta-T. 
    /// <para>The high-level NOVAS astrometric functions are implemented as methods of Star: 
    /// GetTopocentricPosition(), GetLocalPosition(), GetApparentPosition(), GetVirtualPosition(), 
    /// and GetAstrometricPosition(). These methods operate on the properties of the Star, and produce 
    /// a PositionVector object. For example, to get the topocentric coordinates of a star, simply create 
    /// and initialize a Star, then call Star.GetTopocentricPosition(). The resulting vaPositionVector's 
    /// right ascension and declination properties are the topocentric equatorial coordinates, at the same 
    /// time, the (optionally refracted) alt-az coordinates are calculated, and are also contained within 
    /// the returned PositionVector. <b>Note that Alt/Az is available in PositionVectors returned from calling 
    /// GetTopocentricPosition().</b></para></remarks>
    [Guid("8FD58EDE-DF7A-4fdc-9DEC-FD0B36424F5F")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Star : IStar
    {
        private double m_rv, m_plx, m_pmdec, m_pmra, m_ra, m_dec, m_deltat;
        private bool m_rav, m_decv, m_bDTValid;
        private object m_earthephobj;
        private string m_cat, m_name;
        private int m_num;
        private BodyDescription m_earth;
        private short hr;
        private double[] m_earthephdisps = new double[5];
        private NOVAS31 Nov31 = new NOVAS31();

        /// <summary>
        /// Initialise a new instance of the star class
        /// </summary>
        /// <remarks></remarks>
        public Star()
        {
            m_rv = 0.0d; // Defaults to 0.0
            m_plx = 0.0d; // Defaults to 0.0
            m_pmdec = 0.0d; // Defaults to 0.0
            m_pmra = 0.0d; // Defaults to 0.0
            m_rav = false; // RA not valid
            m_ra = 0.0d;
            m_decv = false; // Dec not valid
            m_dec = 0.0d;
            m_cat = ""; // \0''No names
            m_name = ""; // \0'
            m_num = 0;
            m_earthephobj = null; // No Earth ephemeris [sentinel]
            m_bDTValid = false; // Calculate delta-t
            m_earth = new BodyDescription();
            m_earth.Number = Body.Earth;
            m_earth.Name = "Earth";
            m_earth.Type = BodyType.MajorPlanet;
        }

        /// <summary>
        /// Three character catalog code for the star's data
        /// </summary>
        /// <value>Three character catalog code for the star's data</value>
        /// <returns>Three character catalog code for the star's data</returns>
        /// <remarks>Typically "FK5" but may be "HIP". For information only.</remarks>
        public string Catalog
        {
            get
            {
                return m_cat;
            }
            set
            {
                if (value.Length > 3)
                    throw new Utilities.Exceptions.InvalidValueException("Star.Catalog Catlog > 3 characters long: " + value);
                m_cat = value;
            }
        }

        /// <summary>
        /// Mean catalog J2000 declination coordinate (degrees)
        /// </summary>
        /// <value>Mean catalog J2000 declination coordinate</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double Declination
        {
            get
            {
                if (!m_rav)
                    throw new Exceptions.ValueNotSetException("Star.Declination Value not available");
                return m_dec;
            }
            set
            {
                m_dec = value;
                m_decv = true;
            }
        }

        /// <summary>
        /// The value of delta-T (TT - UT1) to use for reductions.
        /// </summary>
        /// <value>The value of delta-T (TT - UT1) to use for reductions.</value>
        /// <returns>Seconds</returns>
        /// <remarks>If this property is not set, calculations will use an internal function to estimate delta-T.</remarks>
        public double DeltaT
        {
            get
            {
                if (!m_bDTValid)
                    throw new Exceptions.ValueNotSetException("Star.DeltaT Value not available");
                return m_deltat;
            }
            set
            {
                m_deltat = value;
                m_bDTValid = true;
            }
        }

        /// <summary>
        /// Ephemeris object used to provide the position of the Earth.
        /// </summary>
        /// <value>Ephemeris object used to provide the position of the Earth.</value>
        /// <returns>Ephemeris object</returns>
        /// <remarks>If this value is not set, an internal Kepler object will be used to determine 
        /// Earth ephemeris</remarks>
        public object EarthEphemeris
        {
            get
            {
                return m_earthephobj;
            }
            set
            {
                m_earthephobj = value;
            }
        }

        /// <summary>
        /// Get an apparent position for a given time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <returns>PositionVector for the apparent place.</returns>
        /// <remarks></remarks>
        public PositionVector GetApparentPosition(double tjd)
        {
            var cat = new CatEntry();
            var PV = new PositionVector();

            double tdb = default, time2 = default;
            double[] peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4], pos1 = new double[4], pos2 = new double[4], pos3 = new double[4], pos4 = new double[4], pos5 = new double[4], pos6 = new double[4], vel1 = new double[4], vec = new double[4];

            if (!(m_rav & m_decv))
                throw new Exceptions.ValueNotSetException("Star.GetApparentPosition RA or DEC not available");

            //
            // Get the position and velocity of the Earth w/r/t the solar system
            // barycenter and the center of mass of the Sun, on the mean equator
            // and equinox of J2000.0
            //
            // This also gets the barycentric terrestrial dynamical time (TDB).
            //

            hr = GetEarth(tjd, ref m_earth, ref tdb, ref peb, ref veb, ref pes, ref ves);
            if (hr > 0)
            {
                vec[0] = 0.0d;
                vec[1] = 0.0d;
                vec[2] = 0.0d;
                throw new Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr);
            }
            cat.RA = m_ra;
            cat.Dec = m_dec;
            cat.ProMoRA = m_pmra;
            cat.ProMoDec = m_pmdec;
            cat.Parallax = m_plx;
            cat.RadialVelocity = m_rv;

            StarVectors(cat, ref pos1, ref vel1);
            ProperMotion(GlobalItems.J2000BASE, pos1, vel1, tdb, ref pos2);

            BaryToGeo(pos2, peb, ref pos3, ref time2);
            SunField(pos3, pes, ref pos4);
            Aberration(pos4, veb, time2, ref pos5);
            Nov31.Precession(GlobalItems.J2000BASE, pos5, tdb, ref pos6);
            Nutate(tdb, NutationDirection.MeanToTrue, pos6, ref vec);
            PV.x = vec[0];
            PV.y = vec[1];
            PV.z = vec[2];
            return PV;
        }

        //
        // This is the NOVAS-COM implementation of astro_star(). See the
        // original NOVAS-C sources for more info.
        //
        /// <summary>
        /// Get an astrometric position for a given time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <returns>PositionVector for the astrometric place.</returns>
        /// <remarks></remarks>
        public PositionVector GetAstrometricPosition(double tjd)
        {
            var cat = new CatEntry();
            var PV = new PositionVector();
            double lighttime = default, tdb = default;
            double[] pos1 = new double[4], vel1 = new double[4], pos2 = new double[4], peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4], vec = new double[4];

            if (!(m_rav & m_decv))
                throw new Exceptions.ValueNotSetException("Star.GetAstrometricPosition RA or DEC not available");

            //
            // Get the position and velocity of the Earth w/r/t the solar system
            // barycenter and the center of mass of the Sun, on the mean equator
            // and equinox of J2000.0
            //
            // This also gets the barycentric terrestrial dynamical time (TDB).
            //
            hr = GetEarth(tjd, ref m_earth, ref tdb, ref peb, ref veb, ref pes, ref ves);
            if (hr > 0)
            {
                vec[0] = 0.0d;
                vec[1] = 0.0d;
                vec[2] = 0.0d;
                throw new Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr);
            }

            cat.RA = m_ra;
            cat.Dec = m_dec;
            cat.ProMoRA = m_pmra;
            cat.ProMoDec = m_pmdec;
            cat.Parallax = m_plx;
            cat.RadialVelocity = m_rv;

            //
            // Compute astrometric place.
            //

            StarVectors(cat, ref pos1, ref vel1);
            ProperMotion(GlobalItems.J2000BASE, pos1, vel1, tdb, ref pos2);
            BaryToGeo(pos2, peb, ref vec, ref lighttime);

            PV.x = vec[0];
            PV.y = vec[1];
            PV.z = vec[2];
            return PV;

        }

        /// <summary>
        /// Get a local position for a given site and time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <param name="site">A Site object representing the observing site</param>
        /// <returns>PositionVector for the local place.</returns>
        /// <remarks></remarks>
        public PositionVector GetLocalPosition(double tjd, Site site)
        {
            var cat = new CatEntry();
            var PV = new PositionVector();
            var st = new SiteInfo();
            double gast = default, lighttime = default, ujd, tdb = default, oblm = default, oblt = default, eqeq = default, psi = default, eps = default;
            double[] pog = new double[4], vog = new double[4], pb = new double[4], vb = new double[4], ps = new double[4], vs = new double[4], pos1 = new double[4], vel1 = new double[4], pos2 = new double[4], vel2 = new double[4], pos3 = new double[4], pos4 = new double[4], peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4], vec = new double[4];
            int j;

            if (!(m_rav & m_decv))
                throw new Exceptions.ValueNotSetException("Star.GetLocalPosition RA or DEC not available");
            //
            // Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
            //
            if (m_bDTValid)
            {
                ujd = tjd - m_deltat;
            }
            else
            {
                ujd = tjd - DeltatCode.DeltaTCalc(tjd) / 86400.0d;
            }


            cat.RA = m_ra;
            cat.Dec = m_dec;
            cat.ProMoRA = m_pmra;
            cat.ProMoDec = m_pmdec;
            cat.Parallax = m_plx;
            cat.RadialVelocity = m_rv;

            try
            {
                st.Latitude = site.Latitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetLocalPosition Site.Latitude is not available");
            }

            try
            {
                st.Longitude = site.Longitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetLocalPosition Site.Longitude is not available");
            }
            try
            {
                st.Height = site.Height;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetLocalPosition Site.Height is not available");
            }

            //
            // Compute position and velocity of the observer, on mean equator
            // and equinox of J2000.0, wrt the solar system barycenter and
            // wrt to the center of the Sun.
            //
            // This also gets the barycentric terrestrial dynamical time (TDB).
            //
            hr = GetEarth(tjd, ref m_earth, ref tdb, ref peb, ref veb, ref pes, ref ves);
            if (hr > 0)
            {
                vec[0] = 0.0d;
                vec[1] = 0.0d;
                vec[2] = 0.0d;
                throw new Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr);
            }

            EarthTilt(tdb, ref oblm, ref oblt, ref eqeq, ref psi, ref eps);

            SiderealTime(ujd, 0.0d, eqeq, ref gast);
            Terra(ref st, gast, ref pos1, ref vel1);
            Nutate(tdb, NutationDirection.TrueToMean, pos1, ref pos2);
            Nov31.Precession(tdb, pos2, GlobalItems.J2000BASE, ref pog);

            Nutate(tdb, NutationDirection.TrueToMean, vel1, ref vel2);
            Nov31.Precession(tdb, vel2, GlobalItems.J2000BASE, ref vog);

            for (j = 0; j <= 2; j++)
            {

                pb[j] = peb[j] + pog[j];
                vb[j] = veb[j] + vog[j];
                ps[j] = pes[j] + pog[j];
                vs[j] = ves[j] + vog[j];
            }

            //
            // Compute local place.
            //

            StarVectors(cat, ref pos1, ref vel1);
            ProperMotion(GlobalItems.J2000BASE, pos1, vel1, tdb, ref pos2);
            BaryToGeo(pos2, pb, ref pos3, ref lighttime);
            SunField(pos3, ps, ref pos4);
            Aberration(pos4, vb, lighttime, ref vec);

            PV.x = vec[0];
            PV.y = vec[1];
            PV.z = vec[2];
            return PV;

        }

        //
        // This is the NOVAS-COM implementation of topo_star(). See the
        // original NOVAS-C sources for more info.
        //
        /// <summary>
        /// Get a topocentric position for a given site and time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <param name="site">A Site object representing the observing site</param>
        /// <param name="Refract">True to apply atmospheric refraction corrections</param>
        /// <returns>PositionVector for the topocentric place.</returns>
        /// <remarks></remarks>
        public PositionVector GetTopocentricPosition(double tjd, Site site, bool Refract)
        {
            RefractionOption @ref;
            int j;
            var cat = new CatEntry();
            var st = new SiteInfo();
            double lighttime = default, ujd, gast = default, tdb = default, oblm = default, oblt = default, eqeq = default, psi = default, eps = default;
            double[] pob = new double[4], pog = new double[4], vob = new double[4], vog = new double[4], pos = new double[4], pos1 = new double[4], pos2 = new double[4], pos3 = new double[4], pos4 = new double[4], pos5 = new double[4], pos6 = new double[4], vel1 = new double[4], vel2 = new double[4], peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4], vec = new double[4];
            double ra = default, rra = default, dec = default, rdec = default, az = default, zd = default, dist;
            bool wx;

            if (!(m_rav & m_decv))
                throw new Exceptions.ValueNotSetException("Star.GetTopocentricPosition RA or DEC not available");

            //
            // Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
            //
            if (m_bDTValid)
            {
                ujd = tjd - m_deltat;
            }
            else
            {
                ujd = tjd - DeltatCode.DeltaTCalc(tjd) / 86400.0d;
            }

            //
            // Get the observer's site info
            //
            try
            {
                st.Latitude = site.Latitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available");
            }
            try
            {
                st.Longitude = site.Longitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available");
            }
            try
            {
                st.Height = site.Height;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available");
            }

            //
            // Compute position and velocity of the observer, on mean equator
            // and equinox of J2000.0, wrt the solar system barycenter and
            // wrt to the center of the Sun.
            //
            // This also gets the barycentric terrestrial dynamical time (TDB).
            //
            hr = GetEarth(tjd, ref m_earth, ref tdb, ref peb, ref veb, ref pes, ref ves);
            if (hr > 0)
            {
                vec[0] = 0.0d;
                vec[1] = 0.0d;
                vec[2] = 0.0d;
                throw new Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr);
            }

            EarthTilt(tdb, ref oblm, ref oblt, ref eqeq, ref psi, ref eps);

            SiderealTime(ujd, 0.0d, eqeq, ref gast);
            Terra(ref st, gast, ref pos1, ref vel1);
            Nutate(tdb, NutationDirection.TrueToMean, pos1, ref pos2);
            Nov31.Precession(tdb, pos2, GlobalItems.J2000BASE, ref pog);

            Nutate(tdb, NutationDirection.TrueToMean, vel1, ref vel2);
            Nov31.Precession(tdb, vel2, GlobalItems.J2000BASE, ref vog);

            for (j = 0; j <= 2; j++)
            {
                pob[j] = peb[j] + pog[j];
                vob[j] = veb[j] + vog[j];
                pos[j] = pes[j] + pog[j];
            }

            //
            // Convert FK5 info to vector form
            //
            cat.RA = m_ra;
            cat.Dec = m_dec;
            cat.ProMoRA = m_pmra;
            cat.ProMoDec = m_pmdec;
            cat.Parallax = m_plx;
            cat.RadialVelocity = m_rv;
            StarVectors(cat, ref pos1, ref vel1);

            //
            // Finish topocentric place calculation.
            //
            ProperMotion(GlobalItems.J2000BASE, pos1, vel1, tdb, ref pos2);
            BaryToGeo(pos2, pob, ref pos3, ref lighttime);
            SunField(pos3, pos, ref pos4);
            Aberration(pos4, vob, lighttime, ref pos5);
            Precession(GlobalItems.J2000BASE, pos5, tdb, ref pos6);
            Nutate(tdb, NutationDirection.MeanToTrue, pos6, ref vec);

            //
            // Calculate equatorial coordinates and distance
            //
            Vector2RADec(vec, ref ra, ref dec); // Get topo RA/Dec
            dist = Sqrt(Pow(vec[0], 2.0d) + Pow(vec[1], 2.0d) + Pow(vec[2], 2.0d)); // And dist

            //
            // Refract if requested
            //
            @ref = RefractionOption.NoRefraction; // Assume no refraction
            if (Refract)
            {
                wx = true; // Assume site weather
                try
                {
                    st.Temperature = site.Temperature;
                }
                catch (Exception ex) // Value unset so use standard refraction option
                {
                    wx = false;
                }
                try
                {
                    st.Pressure = site.Pressure;
                }
                catch (Exception ex) // Value unset so use standard refraction option
                {
                    wx = false;
                }


                if (wx) // Set refraction option
                {
                    @ref = RefractionOption.LocationRefraction;
                }
                else
                {
                    @ref = RefractionOption.StandardRefraction;
                }
            }
            //
            // This calculates Alt/Az coordinates. If ref > 0 then it refracts
            // both the computed Alt/Az and the RA/Dec coordinates.
            //
            if (m_bDTValid)
            {
                Equ2Hor(tjd, m_deltat, 0.0d, 0.0d, ref st, ra, dec, @ref, ref zd, ref az, ref rra, ref rdec);
            }
            else
            {
                Equ2Hor(tjd, DeltatCode.DeltaTCalc(tjd), 0.0d, 0.0d, ref st, ra, dec, @ref, ref zd, ref az, ref rra, ref rdec);
            }

            //
            // If we refracted, we now must compute new cartesian components
            // Distance does not change...
            //
            if ((int)@ref > 0) // If refracted, recompute 
            {
                RADec2Vector(rra, rdec, dist, ref vec); // New refracted vector
            }

            // Create a new positionvector with calculated values
            var PV = new PositionVector(vec[0], vec[1], vec[2], rra, rdec, dist, dist / GlobalItems.C, az, 90.0d - zd);

            return PV;
        }

        /// <summary>
        /// Get a virtual position at a given time
        /// </summary>
        /// <param name="tjd">Terrestrial Julian Date for the position</param>
        /// <returns>PositionVector for the virtual place.</returns>
        /// <remarks></remarks>
        public PositionVector GetVirtualPosition(double tjd)
        {
            //
            // This is the NOVAS-COM implementation of virtual_star(). See the
            // original NOVAS-C sources for more info.
            //
            var cat = new CatEntry();
            var PV = new PositionVector();

            double[] pos1 = new double[4], vel1 = new double[4], pos2 = new double[4], pos3 = new double[4], pos4 = new double[4], peb = new double[4], veb = new double[4], pes = new double[4], ves = new double[4], vec = new double[4];
            double tdb = default, lighttime = default;

            if (!(m_rav & m_decv))
                throw new Exceptions.ValueNotSetException("Star.GetVirtualPosition RA or DEC not available");

            cat.RA = m_ra;
            cat.Dec = m_dec;
            cat.ProMoRA = m_pmra;
            cat.ProMoDec = m_pmdec;
            cat.Parallax = m_plx;
            cat.RadialVelocity = m_rv;

            //
            // Compute position and velocity of the observer, on mean equator
            // and equinox of J2000.0, wrt the solar system barycenter and
            // wrt to the center of the Sun.
            //
            // This also gets the barycentric terrestrial dynamical time (TDB).
            //

            hr = GetEarth(tjd, ref m_earth, ref tdb, ref peb, ref veb, ref pes, ref ves);
            if (hr > 0)
            {
                vec[0] = 0.0d;
                vec[1] = 0.0d;
                vec[2] = 0.0d;
                throw new Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr);
            }

            //
            // Compute virtual place.
            //
            StarVectors(cat, ref pos1, ref vel1);
            ProperMotion(GlobalItems.J2000BASE, pos1, vel1, tdb, ref pos2);
            BaryToGeo(pos2, peb, ref pos3, ref lighttime);
            SunField(pos3, pes, ref pos4);
            Aberration(pos4, veb, lighttime, ref vec);

            PV.x = vec[0];
            PV.y = vec[1];
            PV.z = vec[2];
            return PV;

        }

        /// <summary>
        /// The catalog name of the star (50 char max)
        /// </summary>
        /// <value>The catalog name of the star</value>
        /// <returns>Name (50 char max)</returns>
        /// <remarks></remarks>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if (value.Length > 50)
                    throw new Utilities.Exceptions.InvalidValueException("Star.Name Name > 50 characters long: " + value);
                m_name = value;
            }
        }

        /// <summary>
        /// The catalog number of the star
        /// </summary>
        /// <value>The catalog number of the star</value>
        /// <returns>The catalog number of the star</returns>
        /// <remarks></remarks>
        public int Number
        {
            get
            {
                return m_num;
            }
            set
            {
                m_num = value;
            }
        }

        /// <summary>
        /// Catalog mean J2000 parallax (arcsec)
        /// </summary>
        /// <value>Catalog mean J2000 parallax</value>
        /// <returns>Arc seconds</returns>
        /// <remarks></remarks>
        public double Parallax
        {
            get
            {
                return m_plx;
            }
            set
            {
                m_plx = value;
            }
        }

        /// <summary>
        /// Catalog mean J2000 proper motion in declination (arcsec/century)
        /// </summary>
        /// <value>Catalog mean J2000 proper motion in declination</value>
        /// <returns>Arc seconds per century</returns>
        /// <remarks></remarks>
        public double ProperMotionDec
        {
            get
            {
                return m_pmdec;
            }
            set
            {
                m_pmdec = value;
            }
        }

        /// <summary>
        /// Catalog mean J2000 proper motion in right ascension (sec/century)
        /// </summary>
        /// <value>Catalog mean J2000 proper motion in right ascension</value>
        /// <returns>Seconds per century</returns>
        /// <remarks></remarks>
        public double ProperMotionRA
        {
            get
            {
                return m_pmra;
            }
            set
            {
                m_pmra = value;
            }
        }

        /// <summary>
        /// Catalog mean J2000 radial velocity (km/sec)
        /// </summary>
        /// <value>Catalog mean J2000 radial velocity</value>
        /// <returns>Kilometers per second</returns>
        /// <remarks></remarks>
        public double RadialVelocity
        {
            get
            {
                return m_rv;
            }
            set
            {
                m_rv = value;
            }
        }

        /// <summary>
        /// Catalog mean J2000 right ascension coordinate (hours)
        /// </summary>
        /// <value>Catalog mean J2000 right ascension coordinate</value>
        /// <returns>Hours</returns>
        /// <remarks></remarks>
        public double RightAscension
        {
            get
            {
                if (!m_rav)
                    throw new Exceptions.ValueNotSetException("Star.RightAscension Value not available");
                return m_ra;
            }
            set
            {
                m_ra = value;
                m_rav = true;
            }
        }

        /// <summary>
        /// Initialize all star properties with one call
        /// </summary>
        /// <param name="RA">Catalog mean right ascension (hours)</param>
        /// <param name="Dec">Catalog mean declination (degrees)</param>
        /// <param name="ProMoRA">Catalog mean J2000 proper motion in right ascension (sec/century)</param>
        /// <param name="ProMoDec">Catalog mean J2000 proper motion in declination (arcsec/century)</param>
        /// <param name="Parallax">Catalog mean J2000 parallax (arcsec)</param>
        /// <param name="RadVel">Catalog mean J2000 radial velocity (km/sec)</param>
        /// <remarks>Assumes positions are FK5. If Parallax is set to zero, NOVAS-COM assumes the object 
        /// is on the "celestial sphere", which has a distance of 10 megaparsecs. </remarks>
        public void Set(double RA, double Dec, double ProMoRA, double ProMoDec, double Parallax, double RadVel)
        {
            m_ra = RA;
            m_dec = Dec;
            m_pmra = ProMoRA;
            m_pmdec = ProMoDec;
            m_plx = Parallax;
            m_rv = RadVel;
            m_rav = true;
            m_decv = true;
            m_num = 0;
            m_name = ""; // \0';
            m_cat = ""; // \0';
        }

        /// <summary>
        /// Initialise all star properties in one call using Hipparcos data. Transforms to FK5 standard used by NOVAS.
        /// </summary>
        /// <param name="RA">Catalog mean right ascension (hours)</param>
        /// <param name="Dec">Catalog mean declination (degrees)</param>
        /// <param name="ProMoRA">Catalog mean J2000 proper motion in right ascension (sec/century)</param>
        /// <param name="ProMoDec">Catalog mean J2000 proper motion in declination (arcsec/century)</param>
        /// <param name="Parallax">Catalog mean J2000 parallax (arcsec)</param>
        /// <param name="RadVel">Catalog mean J2000 radial velocity (km/sec)</param>
        /// <remarks>Assumes positions are Hipparcos standard and transforms to FK5 standard used by NOVAS. 
        /// <para>If Parallax is set to zero, NOVAS-COM assumes the object is on the "celestial sphere", 
        /// which has a distance of 10 megaparsecs.</para>
        /// </remarks>
        public void SetHipparcos(double RA, double Dec, double ProMoRA, double ProMoDec, double Parallax, double RadVel)
        {
            CatEntry hip = new CatEntry(), fk5 = new CatEntry();

            hip.RA = RA;
            hip.Dec = Dec;
            hip.ProMoRA = ProMoRA;
            hip.ProMoDec = ProMoDec;
            hip.Parallax = Parallax;
            hip.RadialVelocity = RadVel;

            TransformHip(ref hip, ref fk5);

            m_ra = fk5.RA;
            m_dec = fk5.Dec;
            m_pmra = fk5.ProMoRA;
            m_pmdec = fk5.ProMoDec;
            m_plx = fk5.Parallax;
            m_rv = fk5.RadialVelocity;
            m_rav = true;
            m_decv = true;
            m_num = 0;
            m_name = ""; // \0';
            m_cat = ""; // \0';

        }
    }

    /// <summary>
    /// NOVAS-COM: VelocityVector Class
    /// </summary>
    /// <remarks>NOVAS-COM objects of class VelocityVector contain vectors used for velocities (earth, sites, 
    /// planets, and stars) throughout NOVAS-COM. Of course, its properties include the x, y, and z 
    /// components of the velocity. Additional properties are the velocity in equatorial coordinates of 
    /// right ascension dot, declination dot and radial velocity. You can initialize a PositionVector from 
    /// a Star object (essentially an FK5 or HIP catalog entry) or a Site (lat/long/height). For the star 
    /// object the proper motions, distance and radial velocity are used, for a site, the velocity is that 
    /// of the observer with respect to the Earth's center of mass. </remarks>
    [Guid("25F2ED0A-D0C1-403d-86B9-5F7CEBE97D87")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class VelocityVector : IVelocityVector, IVelocityVectorExtra
    {

        private bool m_xv, m_yv, m_zv, m_cv;
        private double[] m_v = new double[3];
        private double m_VRA, m_RadVel, m_VDec;
        private NOVAS31 Nov31 = new NOVAS31();

        /// <summary>
        /// Creates a new velocity vector object
        /// </summary>
        /// <remarks> </remarks>
        public VelocityVector()
        {
            m_xv = false; // Vector is not valid
            m_yv = false;
            m_zv = false;
            m_cv = false; // Coordinate velocities not valid
        }
        /// <summary>
        /// Linear velocity along the declination direction (AU/day)
        /// </summary>
        /// <value>Linear velocity along the declination direction</value>
        /// <returns>AU/day</returns>
        /// <remarks>This is not the proper motion (which is an angular rate and is dependent on the distance to the object).</remarks>
        public double DecVelocity
        {
            get
            {
                if (!(m_xv & m_yv & m_zv))
                    throw new Exceptions.ValueNotSetException("VelocityVector:DecVelocity x, y or z has not been set");
                CheckEq();
                return m_VDec;
            }
        }

        /// <summary>
        /// Linear velocity along the radial direction (AU/day)
        /// </summary>
        /// <value>Linear velocity along the radial direction</value>
        /// <returns>AU/day</returns>
        /// <remarks></remarks>
        public double RadialVelocity
        {
            get
            {
                if (!(m_xv & m_yv & m_zv))
                    throw new Exceptions.ValueNotSetException("VelocityVector:RadialVelocity x, y or z has not been set");
                CheckEq();
                return m_RadVel;
            }
        }

        /// <summary>
        /// Linear velocity along the right ascension direction (AU/day)
        /// </summary>
        /// <value>Linear velocity along the right ascension direction</value>
        /// <returns>AU/day</returns>
        /// <remarks></remarks>
        public double RAVelocity
        {
            get
            {
                if (!(m_xv & m_yv & m_zv))
                    throw new Exceptions.ValueNotSetException("VelocityVector:RAVelocity x, y or z has not been set");
                CheckEq();
                return m_VRA;
            }
        }

        /// <summary>
        /// Initialize the VelocityVector from a Site object and Greenwich Apparent Sdereal Time.
        /// </summary>
        /// <param name="site">The Site object from which to initialize</param>
        /// <param name="gast">Greenwich Apparent Sidereal Time</param>
        /// <returns>True if OK or throws an exception</returns>
        /// <remarks>The velocity vector is that of the observer with respect to the Earth's center 
        /// of mass. The GAST parameter must be for Greenwich, not local. The time is rotated through 
        /// the site longitude. See SetFromSiteJD() for an equivalent method that takes UTC Julian 
        /// Date and optionally Delta-T (eliminating the need for calculating hyper-accurate GAST yourself). </remarks>
        public bool SetFromSite(Site site, double gast)
        {
            const double f = 0.00335281d; // f = Earth ellipsoid flattening
            const double omega = 0.000072921151467d; // omega = Earth angular velocity rad/sec
            double df2, t, sinphi, cosphi, c, s, ach, ash, stlocl, sinst, cosst;

            //
            // Compute parameters relating to geodetic to geocentric conversion.
            //
            df2 = Pow(1.0d - f, 2d);
            try
            {
                t = site.Latitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromSite Site.Latitude is not available");
            }

            t *= GlobalItems.DEG2RAD;
            sinphi = Sin(t);
            cosphi = Cos(t);
            c = 1.0d / Sqrt(Pow(cosphi, 2.0d) + df2 * Pow(sinphi, 2.0d));
            s = df2 * c;
            try
            {
                t = site.Height;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromSite Site.Height is not available");
            }

            t /= 1000d; // Elevation in KM
            ach = GlobalItems.EARTHRAD * c + t;
            ash = GlobalItems.EARTHRAD * s + t;

            //
            // Compute local sidereal time factors at the observer's longitude.
            //
            try
            {
                t = site.Longitude;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromSite Site.Longitude is not available");
            }
            stlocl = (gast * 15.0d + t) * GlobalItems.DEG2RAD;
            sinst = Sin(stlocl);
            cosst = Cos(stlocl);

            //
            // Compute velocity vector components in AU/Day
            //

            m_v[0] = -omega * ach * cosphi * sinst * 86400.0d / GlobalItems.KMAU;
            m_v[1] = omega * ach * cosphi * cosst * 86400.0d / GlobalItems.KMAU;
            m_v[2] = 0.0d;

            m_xv = true;
            m_yv = true;
            m_zv = true; // Vector is complete
            m_cv = false; // Not interesting for Site vector anyway

            return true;
        }


        /// <summary>
        /// Initialize the VelocityVector from a Site object using UTC Julian Date
        /// </summary>
        /// <param name="site">The Site object from which to initialize</param>
        /// <param name="ujd">UTC Julian Date</param>
        /// <returns>True if OK otherwise throws an exception</returns>
        /// <remarks>The velocity vector is that of the observer with respect to the Earth's center 
        /// of mass. The Julian date must be UTC Julian date, not terrestrial. This call will use 
        /// the internal tables and estimator to get delta-T.
        /// This overload is not available through COM, please use 
        /// "SetFromSiteJD(ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double)"
        /// with delta_t set to 0.0 to achieve this effect.
        /// </remarks>
        [ComVisible(false)]
        public bool SetFromSiteJD(Site site, double ujd)
        {
            SetFromSiteJD(site, ujd, 0.0d);
            return default;
        }


        /// <summary>
        /// Initialize the VelocityVector from a Site object using UTC Julian Date and Delta-T
        /// </summary>
        /// <param name="site">The Site object from which to initialize</param>
        /// <param name="ujd">UTC Julian Date</param>
        /// <param name="delta_t">The optional value of Delta-T (TT - UT1) to use for reductions (seconds)</param>
        /// <returns>True if OK otherwise throws an exception</returns>
        /// <remarks>The velocity vector is that of the observer with respect to the Earth's center 
        /// of mass. The Julian date must be UTC Julian date, not terrestrial.</remarks>
        public bool SetFromSiteJD(Site site, double ujd, double delta_t)
        {
            double dummy = default, secdiff = default, tdb, tjd, gast = default;
            double oblm = default, oblt = default, eqeq = default, psi = default, eps = default;

            //
            // Convert UTC Julian date to Terrestrial Julian Date then
            // convert that to barycentric for earthtilt(), which we use
            // to get the equation of equinoxes for sidereal_time(). Note
            // that we're using UJD as input to the deltat(), but that is
            // OK as the difference in time (~70 sec) is insignificant.
            // For precise applications, the caller must specify delta_t.
            //

            if (delta_t != 0.0d)
            {
                tjd = ujd + delta_t;
            }
            else
            {
                tjd = ujd + DeltatCode.DeltaTCalc(ujd);
            }

            Tdb2Tdt(tjd, ref dummy, ref secdiff);
            tdb = tjd + secdiff / 86400.0d;
            EarthTilt(tdb, ref oblm, ref oblt, ref eqeq, ref psi, ref eps);

            //
            // Get the Greenwich Apparent Sidereal Time and call our
            // SetFromSite() method.
            //
            SiderealTime(ujd, 0.0d, eqeq, ref gast);
            SetFromSite(site, gast);
            return true;
        }

        /// <summary>
        /// Initialize the VelocityVector from a Star object.
        /// </summary>
        /// <param name="star">The Star object from which to initialize</param>
        /// <returns>True if OK otherwise throws an exception</returns>
        /// <remarks>The proper motions, distance and radial velocity are used in the velocity calculation. </remarks>
        /// <exception cref="Exceptions.ValueNotAvailableException">If any of: Parallax, RightAscension, Declination, 
        /// ProperMotionRA, ProperMotionDec or RadialVelocity are not available in the star object</exception>
        public bool SetFromStar(Star star)
        {
            double t, paralx, r, d, cra, sra, cdc, sdc;

            //
            // If parallax is unknown, undetermined, or zero, set it to 1e-7 second
            // of arc, corresponding to a distance of 10 megaparsecs.
            //
            try
            {
                paralx = star.Parallax;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.Parallax is not available");
            }
            if (paralx <= 0.0d)
                paralx = 0.0000001d;

            //
            // Convert right ascension, declination, and parallax to position vector
            // in equatorial system with units of AU.
            //
            try
            {
                r = star.RightAscension;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.RightAscension is not available");
            }
            try
            {
                d = star.Declination;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.Declination is not available");
            }

            d *= GlobalItems.DEG2RAD;

            cra = Cos(r);
            sra = Sin(r);
            cdc = Cos(d);
            sdc = Sin(d);

            //
            // Convert proper motion and radial velocity to orthogonal components of
            // motion with units of AU/Day.
            //
            try
            {
                t = star.ProperMotionRA;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.ProperMotionRA is not available");
            }

            m_VRA = t * 15.0d * cdc / (paralx * 36525.0d);
            try
            {
                t = star.ProperMotionDec;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.ProperMotionDec is not available");
            }
            m_VDec = t / (paralx * 36525.0d);
            try
            {
                t = star.RadialVelocity;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.RadialVelocity is not available");
            }

            m_RadVel = t * 86400.0d / GlobalItems.KMAU;

            //
            // Transform motion vector to equatorial system.
            //
            m_v[0] = -m_VRA * sra - m_VDec * sdc * cra + m_RadVel * cdc * cra;
            m_v[1] = m_VRA * cra - m_VDec * sdc * sra + m_RadVel * cdc * sra;
            m_v[2] = m_VDec * cdc + m_RadVel * sdc;

            m_xv = true;
            m_yv = true;
            m_zv = true; // Vector is complete
            m_cv = true; // We have it all!

            return true;
        }

        /// <summary>
        /// Cartesian x component of velocity (AU/day)
        /// </summary>
        /// <value>Cartesian x component of velocity</value>
        /// <returns>AU/day</returns>
        /// <remarks></remarks>
        public double x
        {
            get
            {
                if (!m_xv)
                    throw new Exceptions.ValueNotSetException("VelocityVector:x x value has not been set");
                return m_v[0];
            }
            set
            {
                m_v[0] = value;
                m_xv = true;
            }
        }

        /// <summary>
        /// Cartesian y component of velocity (AU/day)
        /// </summary>
        /// <value>Cartesian y component of velocity</value>
        /// <returns>AU/day</returns>
        /// <remarks></remarks>
        public double y
        {
            get
            {
                if (!m_yv)
                    throw new Exceptions.ValueNotSetException("VelocityVector:y y value has not been set");
                return m_v[1];
            }
            set
            {
                m_v[1] = value;
                m_yv = true;
            }
        }

        /// <summary>
        /// Cartesian z component of velocity (AU/day)
        /// </summary>
        /// <value>Cartesian z component of velocity</value>
        /// <returns>AU/day</returns>
        /// <remarks></remarks>
        public double z
        {
            get
            {
                if (!m_zv)
                    throw new Exceptions.ValueNotSetException("VelocityVector:z z value has not been set");
                return m_v[2];
            }
            set
            {
                m_v[2] = value;
                m_zv = true;
            }
        }

        #region VelocityVector Support Code
        private void CheckEq()
        {
            if (m_cv)
                return; // Equatorial data already OK
            Vector2RADec(m_v, ref m_VRA, ref m_VDec); // Calculate VRA/VDec
            m_RadVel = Sqrt(Pow(m_v[0], 2d) + Pow(m_v[1], 2d) + Pow(m_v[2], 2d));
            m_cv = true;
        }
        #endregion

    }
    #endregion

    #region Private Utiity code
    static class CommonCode
    {
        internal static Body NumberToBody(int Number)
        {
            switch (Number)
            {
                case 1:
                    {
                        return Body.Mercury;
                    }
                case 2:
                    {
                        return Body.Venus;
                    }
                case 3:
                    {
                        return Body.Earth;
                    }
                case 4:
                    {
                        return Body.Mars;
                    }
                case 5:
                    {
                        return Body.Jupiter;
                    }
                case 6:
                    {
                        return Body.Saturn;
                    }
                case 7:
                    {
                        return Body.Uranus;
                    }
                case 8:
                    {
                        return Body.Neptune;
                    }
                case 9:
                    {
                        return Body.Pluto;
                    }
                case 10:
                    {
                        return Body.Sun;
                    }
                case 11:
                    {
                        return Body.Moon;
                    }

                default:
                    {
                        throw new InvalidValueException("PlanetNumberToBody", Number.ToString(), "1 to 11");
                    }
            }
        }
    }
    #endregion

}