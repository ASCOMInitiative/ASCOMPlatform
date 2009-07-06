Namespace KEPLER
#Region "Enums"
    ''' <summary>
    ''' TYpe of body for which ephemeris is required
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BodyType
        ''' <summary>
        ''' Comet
        ''' </summary>
        ''' <remarks></remarks>
        kepComet = 2
        ''' <summary>
        ''' Major planet
        ''' </summary>
        ''' <remarks></remarks>
        kepMajorPlanet = 0
        ''' <summary>
        ''' Minor planet
        ''' </summary>
        ''' <remarks></remarks>
        kepMinorPlanet = 1
    End Enum
    ''' <summary>
    ''' Major planet number
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum PlanetNumber
        ''' <summary>
        ''' Mercury
        ''' </summary>
        ''' <remarks></remarks>
        kepMercury = 1
        ''' <summary>
        ''' Venus
        ''' </summary>
        ''' <remarks></remarks>
        kepVenus = 2
        ''' <summary>
        ''' Earth
        ''' </summary>
        ''' <remarks></remarks>
        kepEarth = 3
        ''' <summary>
        ''' Mars
        ''' </summary>
        ''' <remarks></remarks>
        kepMars = 4
        ''' <summary>
        ''' Jupiter
        ''' </summary>
        ''' <remarks></remarks>
        kepjupiter = 5
        ''' <summary>
        ''' Saturn
        ''' </summary>
        ''' <remarks></remarks>
        kepSaturn = 6
        ''' <summary>
        ''' Uranus
        ''' </summary>
        ''' <remarks></remarks>
        kepUranus = 7
        ''' <summary>
        ''' Neptune
        ''' </summary>
        ''' <remarks></remarks>
        kepNeptune = 8
        ''' <summary>
        ''' Pluto
        ''' </summary>
        ''' <remarks></remarks>
        kepPluto = 9
    End Enum
#End Region

#Region "IEphemeris Interface"

    ''' <summary>
    ''' Kepler Ephemeris Interface
    ''' </summary>
    ''' <remarks>
    ''' The Ephemeris object contains an orbit engine which takes the orbital parameters of a solar system 
    ''' body, plus a a terrestrial date/time, and produces the heliocentric equatorial position and 
    ''' velocity vectors of the body in Cartesian coordinates. Orbital parameters are not required for 
    ''' the major planets, Kepler contains an ephemeris generator for these bodies that is within 0.05 
    ''' arc seconds of the JPL DE404 over a wide range of times, Perturbations from major planets are applied 
    ''' to ephemerides for minor planets. 
    ''' <para>The results are passed back as an array containing the two vectors. 
    ''' Note that this is the format expected for the ephemeris generator used by the NOVAS-COM vector 
    ''' astrometry engine. For more information see the description of Ephemeris.GetPositionAndVelocity().</para>
    ''' <para>
    ''' <b>Ephemeris Calculations</b><br />
    ''' The ephemeris calculations in Kepler draw heavily from the work of 
    ''' Stephen Moshier moshier@world.std.com. kepler is released as a free software package, further 
    ''' extending the work of Mr. Moshier.</para>
    ''' <para>Kepler does not integrate orbits to the current epoch. If you want the accuracy resulting from 
    ''' an integrated orbit, you must integrate separately and supply Kepler with elements of the current 
    ''' epoch. Orbit integration is on the list of things for the next major version.</para>
    ''' <para>Kepler uses polynomial approximations for the major planet ephemerides. The tables 
    ''' of coefficients were derived by a least squares fit of periodic terms to JPL's DE404 ephemerides. 
    ''' The periodic frequencies used were determined by spectral analysis and comparison with VSOP87 and 
    ''' other analytical planetary theories. The least squares fit to DE404 covers the interval from -3000 
    ''' to +3000 for the outer planets, and -1350 to +3000 for the inner planets. For details on the 
    ''' accuracy of the major planet ephemerides, see the Accuracy Tables page. </para>
    ''' <para>
    ''' <b>Date and Time Systems</b><br /><br />
    ''' For a detailed explanation of astronomical timekeeping systems, see A Time Tutorial on the NASA 
    ''' Goddard Spaceflight Center site, and the USNO Systems of Time site. 
    ''' <br /><br /><i>ActiveX Date values </i><br />
    ''' These are the Windows standard "date serial" numbers, and are expressed in local time or 
    ''' UTC (see below). The fractional part of these numbers represents time within a day. 
    ''' They are used throughout applications such as Excel, Visual Basic, VBScript, and other 
    ''' ActiveX capable environments. 
    ''' <br /><br /><i>Julian dates </i><br />
    ''' These are standard Julian "date serial" numbers, and are expressed in UTC time or Terrestrial 
    ''' time. The fractional part of these numbers represents time within a day. The standard ActiveX 
    ''' "Double" precision of 15 digits gives a resolution of about one millisecond in a full Julian date. 
    ''' This is sufficient for the purposes of this program. 
    ''' <br /><br /><i>Hourly Time Values </i><br />
    ''' These are typically used to represent sidereal time and right ascension. They are simple real 
    ''' numbers in units of hours. 
    ''' <br /><br /><i>UTC Time Scale </i><br />
    ''' Most of the ASCOM methods and properties that accept date/time values (either Date or Julian) 
    ''' assume that the date/time is in Coordinated Universal Time (UTC). Where necessary, this time 
    ''' is converted internally to other scales. Note that UTC seconds are based on the Cesium atom, 
    ''' not planetary motions. In order to keep UTC in sync with planetary motion, leap seconds are 
    ''' inserted periodically. The error is at most 900 milliseconds.
    ''' <br /><br /><i>UT1 Time Scale </i><br />
    ''' The UT1 time scale is the planetary equivalent of UTC. It it runs smoothly and varies a bit 
    ''' with time, but it is never more than 900 milliseconds different from UTC. 
    ''' <br /><br /><i>TT Time Scale </i><br />
    ''' The Terrestrial Dynamical Time (TT) scale is used in solar system orbital calculations. 
    ''' It is based completely on planetary motions; you can think of the solar system as a giant 
    ''' TT clock. It differs from UT1 by an amount called "delta-t", which slowly increases with time, 
    ''' and is about 60 seconds right now (2001). </para>
    ''' </remarks>
    Public Interface IEphemeris
        ''' <summary>
        ''' Compute rectangular (x/y/z) heliocentric J2000 equatorial coordinates of position (AU) and 
        ''' velocity (KM/sec.).
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian date/time for which position and velocity is to be computed</param>
        ''' <returns>Array of 6 values containing rectangular (x/y/z) heliocentric J2000 equatorial 
        ''' coordinates of position (AU) and velocity (KM/sec.) for the body.</returns>
        ''' <remarks>The TJD parameter is the date/time as a Terrestrial Time Julian date. See below for 
        ''' more info. If you are using ACP, there are functions available to convert between UTC and 
        ''' Terrestrial time, and for estimating the current value of delta-T. See the Overview page for 
        ''' the Kepler.Ephemeris class for more information on time keeping systems.</remarks>
        Function GetPositionAndVelocity(ByVal tjd As Double) As Double()
        ''' <summary>
        ''' Semi-major axis (AU)
        ''' </summary>
        ''' <value>Semi-major axis in AU</value>
        ''' <returns>Semi-major axis in AU</returns>
        ''' <remarks></remarks>
        Property a() As Double
        ''' <summary>
        ''' The type of solar system body represented by this instance of the ephemeris engine (enum)
        ''' </summary>
        ''' <value>The type of solar system body represented by this instance of the ephemeris engine (enum)</value>
        ''' <returns>0 for major planet, 1 for minot planet and 2 for comet</returns>
        ''' <remarks></remarks>
        Property BodyType() As BodyType
        ''' <summary>
        ''' Orbital eccentricity
        ''' </summary>
        ''' <value>Orbital eccentricity </value>
        ''' <returns>Orbital eccentricity </returns>
        ''' <remarks></remarks>
        Property e() As Double
        ''' <summary>
        ''' Epoch of osculation of the orbital elements (terrestrial Julian date)
        ''' </summary>
        ''' <value>Epoch of osculation of the orbital elements</value>
        ''' <returns>Terrestrial Julian date</returns>
        ''' <remarks></remarks>
        Property Epoch() As Double
        ''' <summary>
        ''' Slope parameter for magnitude
        ''' </summary>
        ''' <value>Slope parameter for magnitude</value>
        ''' <returns>Slope parameter for magnitude</returns>
        ''' <remarks></remarks>
        Property G() As Double
        ''' <summary>
        ''' Absolute visual magnitude
        ''' </summary>
        ''' <value>Absolute visual magnitude</value>
        ''' <returns>Absolute visual magnitude</returns>
        ''' <remarks></remarks>
        Property H() As Double
        ''' <summary>
        ''' The J2000.0 inclination (deg.)
        ''' </summary>
        ''' <value>The J2000.0 inclination</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Property Incl() As Double
        ''' <summary>
        ''' Mean anomaly at the epoch
        ''' </summary>
        ''' <value>Mean anomaly at the epoch</value>
        ''' <returns>Mean anomaly at the epoch</returns>
        ''' <remarks></remarks>
        Property M() As Double
        ''' <summary>
        ''' Mean daily motion (deg/day)
        ''' </summary>
        ''' <value>Mean daily motion</value>
        ''' <returns>Degrees per day</returns>
        ''' <remarks></remarks>
        Property n() As Double
        ''' <summary>
        ''' The name of the body.
        ''' </summary>
        ''' <value>The name of the body or packed MPC designation</value>
        ''' <returns>The name of the body or packed MPC designation</returns>
        ''' <remarks>If this instance represents an unnumbered minor planet, Ephemeris.Name must be the 
        ''' packed MPC designation. For other types, this is for display only.</remarks>
        Property Name() As String
        ''' <summary>
        ''' The J2000.0 longitude of the ascending node (deg.)
        ''' </summary>
        ''' <value>The J2000.0 longitude of the ascending node</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Property Node() As Double
        ''' <summary>
        ''' The major or minor planet number
        ''' </summary>
        ''' <value>The major or minor planet number</value>
        ''' <returns>Number or zero if not numbered</returns>
        ''' <remarks></remarks>
        Property Number() As PlanetNumber
        ''' <summary>
        ''' Orbital period (years)
        ''' </summary>
        ''' <value>Orbital period</value>
        ''' <returns>Years</returns>
        ''' <remarks></remarks>
        Property P() As Double
        ''' <summary>
        ''' The J2000.0 argument of perihelion (deg.)
        ''' </summary>
        ''' <value>The J2000.0 argument of perihelion</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Property Peri() As Double
        ''' <summary>
        ''' Perihelion distance (AU)
        ''' </summary>
        ''' <value>Perihelion distance</value>
        ''' <returns>AU</returns>
        ''' <remarks></remarks>
        Property q() As Double
        ''' <summary>
        ''' Reciprocal semi-major axis (1/AU)
        ''' </summary>
        ''' <value>Reciprocal semi-major axis</value>
        ''' <returns>1/AU</returns>
        ''' <remarks></remarks>
        Property z() As Double
    End Interface
#End Region

#Region "Private Structures"
    Friend Structure plantbl
        Friend maxargs As Integer
        Friend max_harmonic() As Integer
        Friend max_power_of_t As Integer
        Friend arg_tbl() As Integer
        Friend lon_tbl() As Double
        Friend lat_tbl() As Double
        Friend rad_tbl() As Double
        Friend distance As Double
        Friend timescale As Double
        Friend trunclvl As Double

        Friend Sub New(ByVal ma As Integer, ByVal mh() As Integer, ByVal mpt As Integer, ByVal at() As Integer, ByVal lot() As Double, ByVal lat() As Double, ByVal rat() As Double, ByVal dis As Double, ByVal ts As Double, ByVal tl As Double)
            maxargs = ma
            max_harmonic = mh
            max_power_of_t = mpt
            arg_tbl = at
            lon_tbl = lot
            lat_tbl = lat
            rad_tbl = rat
            distance = dis
            timescale = ts
            trunclvl = tl
        End Sub
    End Structure

    Friend Structure orbit
        Friend obname As String '/* name of the object */
        Friend epoch As Double '/* epoch of orbital elements */
        Friend i As Double '/* inclination	*/
        Friend W As Double '/* longitude of the ascending node */
        Friend wp As Double '/* argument of the perihelion */
        Friend a As Double '/* mean distance (semimajor axis) */
        Friend dm As Double    '/* daily motion */
        Friend ecc As Double   '/* eccentricity */
        Friend M As Double '/* mean anomaly */
        Friend equinox As Double   '/* epoch of equinox and ecliptic */
        Friend mag As Double   '/* visual magnitude at 1AU from earth and sun */
        Friend sdiam As Double '/* equatorial semidiameter at 1au, arc seconds */
        '/* The following used by perterbation formulas: */
        Friend ptable As plantbl
        Friend L As Double  '/* computed mean longitude */
        Friend r As Double  '/* computed radius vector */
        Friend plat As Double   '/* perturbation in ecliptic latitude */

        Friend Sub New(ByVal obn As String, ByVal ep As Double, ByVal i_p As Double, ByVal W_p As Double, _
                       ByVal wp_p As Double, ByVal a_p As Double, ByVal dm_p As Double, ByVal ecc_p As Double, _
                       ByVal M_p As Double, ByVal eq As Double, ByVal mg As Double, ByVal sd As Double, _
                       ByVal pt As plantbl, ByVal L_p As Double, ByVal r_p As Double, ByVal pl As Double)
            obname = obn
            epoch = ep
            i = i_p
            W = W_p
            wp = wp_p
            a = a_p
            dm = dm_p
            ecc = ecc_p
            M = M_p
            equinox = eq
            mag = mg
            sdiam = sd
            ptable = pt
            L = L_p
            r = r_p
            plat = pl
        End Sub
    End Structure
#End Region

End Namespace