using System;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.Kepler
{

    // NOTE on changed behaviour for elliptical orbit comet calculations with - Peter Simpson January 2023

    // The original Kepler component assigned both the Kepler.a (semi-major axis) and Kepler.q (perihelion distance) properties to the same
    // Orbit structure variable 'a' which is defined here as the mean distance (semi-major axis).

    // However, the semi-major axis value is not available in MPC one-line orbit ephemeris data although it can be calculated from the perihelion distance for
    // elliptical orbits (eccentricity <1.0) by the formula: SemiMajorAxis = PerihelionDistance / (1 - OrbitalEccentricity)

    // Users tracking comets are being caught out because they are setting the perihelion distance from the MPC data but the Kepler component actually requires the semi-major axis in 
    // order to calculate the correct orbit for elliptical comets and currently is using the supplied perihelion distance and thus returns the wrong answer.

    // It is now necessary to disambiguate the Kepler.a and Kepler.q properties, but in a way that is backward compatible with previous behaviour. There are four scenarios to consider:
    // Kepler.q   Kepler.a
    // 1) Un-set     Un-set
    // 2) Set        Un-set
    // 3) Un-set     Set
    // 4) Set        Set

    // Original internal use of Orbit.a by elliptical comet scenario when Kepler.GetPositionAndVelocity is called:
    // 1) A value of 0.0 is used in calculation, which will result in a wrong answer
    // 2) The perihelion distance is used, which results in the wrong answer
    // 3) The semi-major axis value is used, which results in the correct answer
    // 4) Whichever property was set last is used, which will give the correct answer if semi-major axis was set last and the wrong answer if perihelion distance was set last

    // In addition there were some undesirable outcomes when reading the Kepler object a and q properties for elliptical orbit comets:
    // a) If a was set then q would have an incorrect value 
    // b) If q was set then a would have an incorrect value 

    // The code has been modified to behave like this in each scenario when Kepler.GetPositionAndVelocity is called:
    // 1) A value of 0.0 is used in the calculation, which will result in a wrong answer
    // 2) The semi-major axis value is calculated from the perihelion distance and used, which results in the correct answer
    // 3) The supplied semi-major axis value is used, which results in the correct answer
    // 4) The supplied semi-major axis value is used, which results in the correct answer

    // Improved property behaviours by scenario:
    // 1) Semi-major axis and perihelion distance return 0.0;
    // 2) Perihelion distance has the set value and semi-major axis returns a calculated value
    // 3) Semi-major axis has the set value and perihelion distance returns a calculated value
    // 4) Both perihelion distance and semi-major axis return their set values

    /// <summary>
    /// KEPLER: Ephemeris Object
    /// </summary>
    /// <remarks>
    /// The Kepler Ephemeris object contains an orbit engine which takes the orbital parameters of a solar system 
    /// body, plus a a terrestrial date/time, and produces the heliocentric equatorial position and 
    /// velocity vectors of the body in Cartesian coordinates. Orbital parameters are not required for 
    /// the major planets, Kepler contains an ephemeris generator for these bodies that is within 0.05 
    /// arc seconds of the JPL DE404 over a wide range of times, Perturbations from major planets are applied 
    /// to ephemerides for minor planets. 
    /// <para>The results are passed back as an array containing the two vectors. 
    /// Note that this is the format expected for the ephemeris generator used by the NOVAS-COM vector 
    /// astrometry engine. For more information see the description of Ephemeris.GetPositionAndVelocity().</para>
    /// <para>
    /// <b>Ephemeris Calculations</b><br />
    /// The ephemeris calculations in Kepler draw heavily from the work of 
    /// Stephen Moshier moshier@world.std.com. kepler is released as a free software package, further 
    /// extending the work of Mr. Moshier.</para>
    /// <para>Kepler does not integrate orbits to the current epoch. If you want the accuracy resulting from 
    /// an integrated orbit, you must integrate separately and supply Kepler with elements of the current 
    /// epoch. Orbit integration is on the list of things for the next major version.</para>
    /// <para>Kepler uses polynomial approximations for the major planet ephemerides. The tables 
    /// of coefficients were derived by a least squares fit of periodic terms to JPL's DE404 ephemerides. 
    /// The periodic frequencies used were determined by spectral analysis and comparison with VSOP87 and 
    /// other analytical planetary theories. The least squares fit to DE404 covers the interval from -3000 
    /// to +3000 for the outer planets, and -1350 to +3000 for the inner planets. For details on the 
    /// accuracy of the major planet ephemerides, see the Accuracy Tables page. </para>
    /// <para>
    /// <b>Date and Time Systems</b><br /><br />
    /// For a detailed explanation of astronomical timekeeping systems, see A Time Tutorial on the NASA 
    /// Goddard Spaceflight Center site, and the USNO Systems of Time site. 
    /// <br /><br /><i>ActiveX Date values </i><br />
    /// These are the Windows standard "date serial" numbers, and are expressed in local time or 
    /// UTC (see below). The fractional part of these numbers represents time within a day. 
    /// They are used throughout applications such as Excel, Visual Basic, VBScript, and other 
    /// ActiveX capable environments. 
    /// <br /><br /><i>Julian dates </i><br />
    /// These are standard Julian "date serial" numbers, and are expressed in UTC time or Terrestrial 
    /// time. The fractional part of these numbers represents time within a day. The standard ActiveX 
    /// "Double" precision of 15 digits gives a resolution of about one millisecond in a full Julian date. 
    /// This is sufficient for the purposes of this program. 
    /// <br /><br /><i>Hourly Time Values </i><br />
    /// These are typically used to represent sidereal time and right ascension. They are simple real 
    /// numbers in units of hours. 
    /// <br /><br /><i>UTC Time Scale </i><br />
    /// Most of the ASCOM methods and properties that accept date/time values (either Date or Julian) 
    /// assume that the date/time is in Coordinated Universal Time (UTC). Where necessary, this time 
    /// is converted internally to other scales. Note that UTC seconds are based on the Cesium atom, 
    /// not planetary motions. In order to keep UTC in sync with planetary motion, leap seconds are 
    /// inserted periodically. The error is at most 900 milliseconds.
    /// <br /><br /><i>UT1 Time Scale </i><br />
    /// The UT1 time scale is the planetary equivalent of UTC. It it runs smoothly and varies a bit 
    /// with time, but it is never more than 900 milliseconds different from UTC. 
    /// <br /><br /><i>TT Time Scale </i><br />
    /// The Terrestrial Dynamical Time (TT) scale is used in solar system orbital calculations. 
    /// It is based completely on planetary motions; you can think of the solar system as a giant 
    /// TT clock. It differs from UT1 by an amount called "delta-t", which slowly increases with time, 
    /// and is about 60 seconds right now (2001). </para>
    /// </remarks>
    [Guid("2F2B0413-1F83-4777-B3B4-38DE3C32DC6B")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Ephemeris : IEphemeris
    {

        private const double DTVEL = 0.01d;

        // Ephemeris variables
        private string m_Name; // Name of body
        private Body m_Number; // Number of body
        private bool m_bNumberValid;
        private BodyType m_Type; // Type of body
        private bool m_bTypeValid;
        private KeplerGlobalCode.Orbit m_e = new KeplerGlobalCode.Orbit(0.0d); // Elements, etc for minor planets/comets, etc.
        // Public Shared TL As TraceLogger
        // gplan variables
        private double[,] ss = new double[19, 32], cc = new double[19, 32];
        private double[] Args = new double[19];
        private double LP_equinox, NF_arcsec, Ea_arcsec, pA_precession;

        /// <summary>
        /// Create a new Ephemeris component and initialise it
        /// </summary>
        /// <remarks></remarks>
        public Ephemeris()
        {
            // TL = New TraceLogger("", "KeplerEphemeris")
            // TL.Enabled = GetBool(ASTROUTILS_TRACE, ASTROUTILS_TRACE_DEFAULT) 'Get enabled / disabled state from the user registry
            // TL.LogMessage("New", "Kepler Created")
            m_bTypeValid = false;
            m_Name = ""; // Sentinel
            m_Type = default;
            m_e.ptable.lon_tbl = new double[] { 0.0d }; // Initialise orbit arrays
            m_e.ptable.lat_tbl = new double[] { 0.0d };
        }
        /// <summary>
        /// Semi-major axis (AU)
        /// </summary>
        /// <value>Semi-major axis in AU</value>
        /// <returns>Semi-major axis in AU</returns>
        /// <remarks></remarks>
        public double a
        {
            get
            {
                if (double.IsNaN(m_e.semiMajorAxis))
                {
                    // TL.LogMessage("Get a", $"NOT SET - Returning semi-major axis: {0.0}")
                    return 0.0d;
                }
                else
                {
                    // TL.LogMessage("Get a", $"SET - Returning semi-major axis: {m_e.semiMajorAxis}")
                    return m_e.semiMajorAxis;
                }
            }
            set
            {
                // TL.LogMessage("Set a", $"Semi-major axis: {value}")
                m_e.semiMajorAxis = value;
                m_e.a = value;
            }
        }

        /// <summary>
        /// Perihelion distance (AU)
        /// </summary>
        /// <value>Perihelion distance</value>
        /// <returns>AU</returns>
        /// <remarks></remarks>
        public double q
        {
            get
            {
                if (double.IsNaN(m_e.perihelionDistance))
                {
                    // TL.LogMessage("Get q", $"NOT SET - Returning perihelion distance: {0.0}")
                    return 0.0d;
                }
                else
                {
                    // TL.LogMessage("Get q", $"SET - Returning perihelion distance: {m_e.perihelionDistance}")
                    return m_e.perihelionDistance;
                }
            }
            set
            {
                // TL.LogMessage("Set q", $"Perihelion distance: {value}")
                m_e.perihelionDistance = value;
                m_e.a = value;
            }
        }

        /// <summary>
        /// The type of solar system body represented by this instance of the ephemeris engine (enum)
        /// </summary>
        /// <value>The type of solar system body represented by this instance of the ephemeris engine (enum)</value>
        /// <returns>0 for major planet, 1 for minot planet and 2 for comet</returns>
        /// <remarks></remarks>
        public BodyType BodyType
        {
            get
            {
                if (!m_bTypeValid)
                    throw new Exceptions.ValueNotSetException("KEPLER:BodyType BodyType has not been set");
                return m_Type;
            }
            set
            {
                m_Type = value;
                m_bTypeValid = true;
            }
        }

        /// <summary>
        /// Orbital eccentricity
        /// </summary>
        /// <value>Orbital eccentricity </value>
        /// <returns>Orbital eccentricity </returns>
        /// <remarks></remarks>
        public double e
        {
            get
            {
                return m_e.ecc;
            }
            set
            {
                m_e.ecc = value;
                m_e.eccentricityHasBeenSet = true; // Record that an eccentricity value has been set (used for parameter validation in GetPositionAndVelocity())
            }
        }

        /// <summary>
        /// Epoch of osculation of the orbital elements (terrestrial Julian date)
        /// </summary>
        /// <value>Epoch of osculation of the orbital elements</value>
        /// <returns>Terrestrial Julian date</returns>
        /// <remarks></remarks>
        public double Epoch
        {
            get
            {
                return m_e.epoch;
            }
            set
            {
                m_e.epoch = value;
            }
        }

        /// <summary>
        /// Slope parameter for magnitude
        /// </summary>
        /// <value>Slope parameter for magnitude</value>
        /// <returns>Slope parameter for magnitude</returns>
        /// <remarks></remarks>
        public double G
        {
            get
            {
                throw new Exceptions.ValueNotAvailableException("Kepler:G Read - Magnitude slope parameter calculation not implemented");
            }
            set
            {
                throw new Exceptions.ValueNotAvailableException("Kepler:G Write - Magnitude slope parameter calculation not implemented");
            }
        }

        /// <summary>
        /// Compute rectangular (x/y/z) heliocentric J2000 equatorial coordinates of position (AU) and 
        /// velocity (KM/sec.).
        /// </summary>
        /// <param name="tjd">Terrestrial Julian date/time for which position and velocity is to be computed</param>
        /// <returns>Array of 6 values containing rectangular (x/y/z) heliocentric J2000 equatorial 
        /// coordinates of position (AU) and velocity (KM/sec.) for the body.</returns>
        /// <remarks>The TJD parameter is the date/time as a Terrestrial Time Julian date. See below for 
        /// more info. If you are using ACP, there are functions available to convert between UTC and 
        /// Terrestrial time, and for estimating the current value of delta-T. See the Overview page for 
        /// the Kepler.Ephemeris class for more information on time keeping systems.</remarks>
        public double[] GetPositionAndVelocity(double tjd)
        {
            var posvec = new double[6];
            var ai = new int[2];
            var pos = new double[4, 4];
            var op = new KeplerGlobalCode.Orbit();
            int i;

            if (!m_bTypeValid)
                throw new Exceptions.ValueNotSetException("Kepler:GetPositionAndVelocity Body type has not been set");
            // TL.LogMessage("GetPosAndVel", m_Number.ToString)

            // TL?.LogMessage("GetPositionAndVelocity", $"Body type: {m_Type}, Eccentricity: {m_e.ecc}")
            // TL?.LogMessage("GetPositionAndVelocity0", $"Perihelion distance: {m_e.perihelionDistance}, Semi-major axis: {m_e.semiMajorAxis}, m_e.a: {m_e.a}")

            switch (m_Type)
            {
                case BodyType.MajorPlanet: // MAJOR PLANETS [unimpl. SUN, MOON]
                    {
                        switch (m_Number)
                        {
                            case Body.Mercury:
                                {
                                    op = KeplerGlobalCode.mercury;
                                    break;
                                }
                            case Body.Venus:
                                {
                                    op = KeplerGlobalCode.venus;
                                    break;
                                }
                            case Body.Earth:
                                {
                                    op = KeplerGlobalCode.earthplanet;
                                    break;
                                }
                            case Body.Mars:
                                {
                                    op = KeplerGlobalCode.mars;
                                    break;
                                }
                            case Body.Jupiter:
                                {
                                    op = KeplerGlobalCode.jupiter;
                                    break;
                                }
                            case Body.Saturn:
                                {
                                    op = KeplerGlobalCode.saturn;
                                    break;
                                }
                            case Body.Uranus:
                                {
                                    op = KeplerGlobalCode.uranus;
                                    break;
                                }
                            case Body.Neptune:
                                {
                                    op = KeplerGlobalCode.neptune;
                                    break;
                                }
                            case Body.Pluto:
                                {
                                    op = KeplerGlobalCode.pluto;
                                    break;
                                }

                            default:
                                {
                                    throw new Utilities.Exceptions.InvalidValueException("Kepler:GetPositionAndVelocity Invalid value for planet number: " + ((int)m_Number).ToString());
                                }
                        }

                        break;
                    }

                case BodyType.MinorPlanet: // MINOR PLANET
                    {
                        // //TODO: Check elements
                        op = m_e;
                        break;
                    }

                case BodyType.Comet: // COMET
                    {
                        // //TODO: Check elements

                        // Test whether this comet is in an elliptical orbit as opposed to parabolic or hyperbolic
                        if (m_e.ecc < 1.0d)
                        {
                            // TL?.LogMessage("GetPositionAndVelocity1", $"Perihelion distance: {m_e.perihelionDistance}, Semi-major axis: {m_e.semiMajorAxis}, m_e.a: {m_e.a}, Eccentricity has been set: {m_e.eccentricityHasBeenSet}, Eccentricity: {m_e.ecc}")
                            // For comets in elliptical orbits (ecc < 1.0) ensure that we use the semi-major axis instead of the perihelion distance.
                            // Handle the four possible scenarios for semi-major axis and perihelion distance
                            // 1) Un-set     Un-set
                            // 2) Set        Un-set
                            // 3) Un-set     Set
                            // 4) Set        Set
                            if (double.IsNaN(m_e.semiMajorAxis)) // Semi-major axis is not set
                            {
                                if (double.IsNaN(m_e.perihelionDistance)) // No semi-major axis or perihelion distance
                                {
                                    // TL?.LogMessage("GetPositionAndVelocity2", $"Perihelion distance: {m_e.perihelionDistance}, Semi-major axis: {m_e.semiMajorAxis}, m_e.a: {m_e.a}")

                                    // Throw an exception because we can't calculate the orbit without either the semi-major axis value or the perihelion distance value.
                                    throw new InvalidOperationException($"Kepler.GetPositionAndVelocity - Cannot calculate comet position because neither the semi-major axis nor the perihelion distance have been provided.");
                                }

                                else
                                {
                                    // No semi-major axis but we do have perihelion distance so calculate semi-major axis from the formula: SemiMajorAxis = PerihelionDistance / (1 - OrbitalEccentricity) and use this

                                    // Validate that the calculation can be completed
                                    if (!m_e.eccentricityHasBeenSet)
                                        throw new InvalidOperationException($"Kepler.GetPositionAndVelocity - Cannot calculate comet position because the orbit eccentricity has not been provided.");

                                    m_e.a = m_e.perihelionDistance / (1.0d - m_e.ecc);
                                    m_e.semiMajorAxis = m_e.a;
                                    // TL?.LogMessage("GetPositionAndVelocity3", $"Perihelion distance: {m_e.perihelionDistance}, Semi-major axis: {m_e.semiMajorAxis}, m_e.a: {m_e.a}")
                                }
                            }
                            else // Semi-major axis has been set so use this
                            {
                                m_e.a = m_e.semiMajorAxis;

                                if (double.IsNaN(m_e.perihelionDistance))
                                {
                                    // Update perihelion distance from the formula: PerihelionDistance  = SemiMajorAxis * (1 - OrbitalEccentricity) and use this

                                    // Validate that the calculation can be completed, otherwise ignore because the orbit can still be calculated
                                    if (m_e.eccentricityHasBeenSet)
                                    {
                                        m_e.perihelionDistance = m_e.semiMajorAxis * (1.0d - m_e.ecc);
                                    }
                                }

                                // TL?.LogMessage("GetPositionAndVelocity4", $"Perihelion distance: {m_e.perihelionDistance}, Semi-major axis: {m_e.semiMajorAxis}, m_e.a: {m_e.a}")
                                else
                                {
                                    // The perihelion distance has been set so just leave it as is
                                    // TL?.LogMessage("GetPositionAndVelocity5", $"Perihelion distance: {m_e.perihelionDistance}, Semi-major axis: {m_e.semiMajorAxis}, m_e.a: {m_e.a}")
                                }
                            }
                        }
                        else if (!double.IsNaN(m_e.semiMajorAxis)) // Eccentricity is >=1.0 and this is a parabolic or hyperbolic orbit so there is no major axis
                        {
                            throw new InvalidOperationException($"Kepler.GetPositionAndVelocity - Eccentricity is >=1.0 {m_e.ecc} (parabolic or hyperbolic trajectory, not an elliptical orbit) but a semi-major axis value has been set implying an orbit.");
                        }

                        op = m_e;
                        break;
                    }
            }

            // TL?.LogMessage("GetPositionAndVelocity6", $"Perihelion distance: {m_e.perihelionDistance}, Semi-major axis: {m_e.semiMajorAxis}, m_e.a: {m_e.a}")

            for (i = 0; i <= 2; i++)
            {
                var p = new double[3];
                double qjd;
                qjd = tjd + (i - 1) * DTVEL;
                // TL?.LogMessage("GetPositionAndVelocity", $"tjd: {tjd}, qjd: {qjd}, DTVEL: {DTVEL}")
                KeplerGlobalCode.KeplerCalc(qjd, ref op, ref p);
                // TL.LogMessage("GetPosVel", $"Loop {i} - Array p: {p(0)}, {p(1)}, {p(2)}")
                pos[i, 0] = p[0];
                pos[i, 1] = p[1];
                pos[i, 2] = p[2];
            }

            // pos(1,x) contains the pos vector
            // pos(0,x) and pos(2,x) are used to determine the velocity based on position change with time!
            for (i = 0; i <= 2; i++)
            {
                posvec[i] = pos[1, i];
                posvec[3 + i] = (pos[2, i] - pos[0, i]) / (2.0d * DTVEL);
            }

            // TL.LogMessage("GetPosVel", $"Loop {i} - Array posvec: {posvec(0)}, {posvec(1)}, {posvec(2)}, {posvec(3)}, {posvec(4)}, {posvec(5)}")

            return posvec;
        }

        /// <summary>
        /// Absolute visual magnitude
        /// </summary>
        /// <value>Absolute visual magnitude</value>
        /// <returns>Absolute visual magnitude</returns>
        /// <remarks></remarks>
        public double H
        {
            get
            {
                throw new Exceptions.ValueNotAvailableException("Kepler:H Read - Visual magnitude calculation not implemented");
            }
            set
            {
                throw new Exceptions.ValueNotAvailableException("Kepler:H Write - Visual magnitude calculation not implemented");
            }
        }

        /// <summary>
        /// The J2000.0 inclination (deg.)
        /// </summary>
        /// <value>The J2000.0 inclination</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double Incl
        {
            get
            {
                return m_e.i;
            }
            set
            {
                m_e.i = value;
            }
        }

        /// <summary>
        /// Mean anomaly at the epoch
        /// </summary>
        /// <value>Mean anomaly at the epoch</value>
        /// <returns>Mean anomaly at the epoch</returns>
        /// <remarks></remarks>
        public double M
        {
            get
            {
                return m_e.M;
            }
            set
            {
                m_e.M = value;
            }
        }

        /// <summary>
        /// Mean daily motion (deg/day)
        /// </summary>
        /// <value>Mean daily motion</value>
        /// <returns>Degrees per day</returns>
        /// <remarks></remarks>
        public double n
        {
            get
            {
                return m_e.dm;
            }
            set
            {
                m_e.dm = value;
            }
        }

        /// <summary>
        /// The name of the body.
        /// </summary>
        /// <value>The name of the body or packed MPC designation</value>
        /// <returns>The name of the body or packed MPC designation</returns>
        /// <remarks>If this instance represents an unnumbered minor planet, Ephemeris.Name must be the 
        /// packed MPC designation. For other types, this is for display only.</remarks>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(m_Name))
                    throw new Exceptions.ValueNotSetException("KEPLER:Name Name has not been set");
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// The J2000.0 longitude of the ascending node (deg.)
        /// </summary>
        /// <value>The J2000.0 longitude of the ascending node</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double Node
        {
            get
            {
                return m_e.W;
            }
            set
            {
                m_e.W = value;
            }
        }

        /// <summary>
        /// The major or minor planet number
        /// </summary>
        /// <value>The major or minor planet number</value>
        /// <returns>Number or zero if not numbered</returns>
        /// <remarks></remarks>
        public Body Number
        {
            get
            {
                if (!m_bNumberValid)
                    throw new Exceptions.ValueNotSetException("KEPLER:Number Planet number has not been set");
                return m_Number;
            }
            set
            {
                m_Number = value;
                m_bNumberValid = true;
            }
        }

        /// <summary>
        /// Orbital period (years)
        /// </summary>
        /// <value>Orbital period</value>
        /// <returns>Years</returns>
        /// <remarks></remarks>
        public double P
        {
            get
            {
                throw new Exceptions.ValueNotAvailableException("Kepler:P Read - Orbital period calculation not implemented");
            }
            set
            {
                throw new Exceptions.ValueNotAvailableException("Kepler:P Write - Orbital period calculation not implemented");
            }
        }

        /// <summary>
        /// The J2000.0 argument of perihelion (deg.)
        /// </summary>
        /// <value>The J2000.0 argument of perihelion</value>
        /// <returns>Degrees</returns>
        /// <remarks></remarks>
        public double Peri
        {
            get
            {
                return m_e.wp;
            }
            set
            {
                m_e.wp = value;
            }
        }

        /// <summary>
        /// Reciprocal semi-major axis (1/AU)
        /// </summary>
        /// <value>Reciprocal semi-major axis</value>
        /// <returns>1/AU</returns>
        /// <remarks></remarks>
        public double z
        {
            get
            {
                return 1.0d / m_e.a;
            }
            set
            {
                m_e.a = 1.0d / value;
            }
        }
    }
}