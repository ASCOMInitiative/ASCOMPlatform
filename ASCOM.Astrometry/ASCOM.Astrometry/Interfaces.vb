'ASCOM.Astrometry public interfaces

Imports ASCOM.Astrometry.NOVASCOM
Imports ASCOM.Utilities
Imports System.Runtime.InteropServices
Imports ASCOM.Astrometry.Kepler

#Region "Transform Interface"
Namespace Transform
    ''' <summary>
    ''' Interface to the coordinate transform component; J2000 - apparent - local topocentric
    ''' </summary>
    ''' <remarks>Use this component to transform between J2000, apparent and local topocentric (JNow) coordinates or 
    ''' vice versa. To use the component, instantiate it, then use one of SetJ2000 or SetJNow or SetApparent to 
    ''' initialise with known values. Now use the RAJ2000, DECJ200, RAJNow, DECJNow, RAApparent and DECApparent 
    ''' properties to read off the required transformed values.
    '''<para>The component can be reused simply by setting new co-ordinates with a Set command, there
    ''' is no need to create a new component each time a transform is required.</para>
    ''' <para>Transforms are effected through the ASCOM NOVAS-COM engine that encapsulates the USNO NOVAS2 library. 
    ''' The USNO NOVAS reference web page is: 
    ''' http://www.usno.navy.mil/USNO/astronomical-applications/software-products/novas/novas-fortran/novas-fortran 
    ''' </para>
    ''' </remarks>
    <Guid("6F38768E-C52D-41c7-9E0F-B8E4AFE341A7"), _
    ComVisible(True)> _
    Public Interface ITransform
        ''' <summary>
        ''' Gets or sets the site latitude
        ''' </summary>
        ''' <value>Site latitude</value>
        ''' <returns>Latitude in degrees</returns>
        ''' <remarks>Positive numbers north of the equator, negative numbers south.</remarks>
        <DispId(1)> Property SiteLatitude() As Double
        ''' <summary>
        ''' Gets or sets the site longitude
        ''' </summary>
        ''' <value>Site longitude</value>
        ''' <returns>Longitude in degrees</returns>
        ''' <remarks>Positive numbers east of the Greenwich meridian, negative numbes west of the Greenwich meridian.</remarks>
        <DispId(2)> Property SiteLongitude() As Double
        ''' <summary>
        ''' Gets or sets the site elevation above sea level
        ''' </summary>
        ''' <value>Site elevation</value>
        ''' <returns>Elevation in metres</returns>
        ''' <remarks></remarks>
        <DispId(3)> Property SiteElevation() As Double
        ''' <summary>
        ''' Gets or sets the site ambient temperature
        ''' </summary>
        ''' <value>Site ambient temperature</value>
        ''' <returns>Temperature in degrees Celsius</returns>
        ''' <remarks></remarks>
        <DispId(4)> Property SiteTemperature() As Double
        ''' <summary>
        ''' Gets or sets a flag indicating whether refraction is calculated for topocentric co-ordinates
        ''' </summary>
        ''' <value>True / false flag indicating refaction is included / omitted from topocentric co-ordinates</value>
        ''' <returns>Boolean flag</returns>
        ''' <remarks></remarks>
        <DispId(5)> Property Refraction() As Boolean
        ''' <summary>
        ''' Causes the transform component to recalculate values derrived from the last Set command
        ''' </summary>
        ''' <remarks>Use this when you have set J2000 co-ordinates and wish to ensure that the mount points to the same 
        ''' co-ordinates allowing for local effects that change with time such as refraction.</remarks>
        <DispId(6)> Sub Refresh()
        ''' <summary>
        ''' Sets the known J2000 Right Ascension and Declination coordinates that are to be transformed
        ''' </summary>
        ''' <param name="RA">RA in J2000 co-ordinates</param>
        ''' <param name="DEC">DEC in J2000 co-ordinates</param>
        ''' <remarks></remarks>
        <DispId(7)> Sub SetJ2000(ByVal RA As Double, ByVal DEC As Double)
        ''' <summary>
        ''' Sets the known apparent Right Ascension and Declination coordinates that are to be transformed
        ''' </summary>
        ''' <param name="RA">RA in apparent co-ordinates</param>
        ''' <param name="DEC">DEC in apparent co-ordinates</param>
        ''' <remarks></remarks>
        <DispId(8)> Sub SetApparent(ByVal RA As Double, ByVal DEC As Double)
        '''<summary>
        ''' Sets the known local topocentric Right Ascension and Declination coordinates that are to be transformed
        ''' </summary>
        ''' <param name="RA">RA in local topocentric co-ordinates</param>
        ''' <param name="DEC">DEC in local topocentric co-ordinates</param>
        ''' <remarks></remarks>
        <DispId(9)> Sub SetTopocentric(ByVal RA As Double, ByVal DEC As Double)
        ''' <summary>
        ''' Returns the Right Ascension in J2000 co-ordinates
        ''' </summary>
        ''' <value>J2000 Right Ascension</value>
        ''' <returns>Right Ascension in hours</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        <DispId(10)> ReadOnly Property RAJ2000() As Double
        ''' <summary>
        ''' Returns the Declination in J2000 co-ordinates
        ''' </summary>
        ''' <value>J2000 Declination</value>
        ''' <returns>J2000 Declination</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        <DispId(11)> ReadOnly Property DECJ2000() As Double
        ''' <summary>
        ''' Returns the Right Ascension in local topocentric co-ordinates
        ''' </summary>
        ''' <value>Local topocentric Right Ascension</value>
        ''' <returns>Local topocentric Right Ascension</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        <DispId(12)> ReadOnly Property RATopocentric() As Double
        ''' <summary>
        ''' Returns the Declination in local topocentric co-ordinates
        ''' </summary>
        ''' <value>Local topocentric Declination</value>
        ''' <returns>Local topocentric Declination</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        <DispId(13)> ReadOnly Property DECTopocentric() As Double
        ''' <summary>
        ''' Returns the Right Ascension in apparent co-ordinates
        ''' </summary>
        ''' <value>Apparent Right Ascension</value>
        ''' <returns>Right Ascension in hours</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        <DispId(14)> ReadOnly Property RAApparent() As Double
        ''' <summary>
        ''' Returns the Declination in apparent co-ordinates
        ''' </summary>
        ''' <value>Apparent Declination</value>
        ''' <returns>Declination in degrees</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        <DispId(15)> ReadOnly Property DECApparent() As Double
        ''' <summary>
        ''' Returns the topocentric azimth angle of the target
        ''' </summary>
        ''' <value>Topocentric azimuth angle</value>
        ''' <returns>Azimuth angle in degrees</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        <DispId(16)> ReadOnly Property AzimuthTopocentric() As Double
        ''' <summary>
        ''' Returns the topocentric elevation of the target
        ''' </summary>
        ''' <value>Topocentric elevation angle</value>
        ''' <returns>Elevation angle in degrees</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        <DispId(17)> ReadOnly Property ElevationTopocentric() As Double
        ''' <summary>
        ''' Sets known Altitude and Azimuth values which are to be transformed
        ''' </summary>
        ''' <param name="Azimuth">Object's azimuth in degrees</param>
        ''' <param name="Elevation">Object's Elevation in degrees</param>
        ''' <remarks></remarks>
        <DispId(18)> Sub SetAzimuthElevation(Azimuth As Double, Elevation As Double)
        ''' <summary>
        ''' Sets or return the Julian date (terrestrial time) for which the transform will be made
        ''' </summary>
        ''' <value>Julian date (terrestrial time) of the transform</value>
        ''' <returns>Terrestrial time Julian date that will be used by Transform or zero if the PC's current clock value will be used to calculate
        ''' the Julian date.</returns>
        ''' <remarks>This method was introduced in May 2012. Previously, Transform used the current date-time of the PC when calculating transforms; 
        ''' this remains the default behaviour for backward compatibility.
        ''' The inital value of this parameter is 0 which is a special value that forces Transform to replicate original behaviour by determining the  
        ''' Julian date from the PC's current date and time. If this property is non zero, that terrestrial time Julian date is used in preference 
        ''' to the value derrived from the PC's clock.</remarks>
        <DispId(19)> Property JulianDateTT As Double
    End Interface
End Namespace
#End Region

#Region "Kepler Ephemeris Interface"

Namespace Kepler
    ''' <summary>
    ''' Interface to the Kepler Ephemeris component
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
    <Guid("54A8F586-C7B7-4899-8AA1-6044BDE4ABFA"), _
    ComVisible(True)> _
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
        <DispId(1)> Function GetPositionAndVelocity(ByVal tjd As Double) As Double()
        ''' <summary>
        ''' Semi-major axis (AU)
        ''' </summary>
        ''' <value>Semi-major axis in AU</value>
        ''' <returns>Semi-major axis in AU</returns>
        ''' <remarks></remarks>
        <DispId(2)> Property a() As Double
        ''' <summary>
        ''' The type of solar system body represented by this instance of the ephemeris engine (enum)
        ''' </summary>
        ''' <value>The type of solar system body represented by this instance of the ephemeris engine (enum)</value>
        ''' <returns>0 for major planet, 1 for minot planet and 2 for comet</returns>
        ''' <remarks></remarks>
        <DispId(3)> Property BodyType() As BodyType
        ''' <summary>
        ''' Orbital eccentricity
        ''' </summary>
        ''' <value>Orbital eccentricity </value>
        ''' <returns>Orbital eccentricity </returns>
        ''' <remarks></remarks>
        <DispId(4)> Property e() As Double
        ''' <summary>
        ''' Epoch of osculation of the orbital elements (terrestrial Julian date)
        ''' </summary>
        ''' <value>Epoch of osculation of the orbital elements</value>
        ''' <returns>Terrestrial Julian date</returns>
        ''' <remarks></remarks>
        <DispId(5)> Property Epoch() As Double
        ''' <summary>
        ''' Slope parameter for magnitude
        ''' </summary>
        ''' <value>Slope parameter for magnitude</value>
        ''' <returns>Slope parameter for magnitude</returns>
        ''' <remarks></remarks>
        <DispId(6)> Property G() As Double
        ''' <summary>
        ''' Absolute visual magnitude
        ''' </summary>
        ''' <value>Absolute visual magnitude</value>
        ''' <returns>Absolute visual magnitude</returns>
        ''' <remarks></remarks>
        <DispId(7)> Property H() As Double
        ''' <summary>
        ''' The J2000.0 inclination (deg.)
        ''' </summary>
        ''' <value>The J2000.0 inclination</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(8)> Property Incl() As Double
        ''' <summary>
        ''' Mean anomaly at the epoch
        ''' </summary>
        ''' <value>Mean anomaly at the epoch</value>
        ''' <returns>Mean anomaly at the epoch</returns>
        ''' <remarks></remarks>
        <DispId(9)> Property M() As Double
        ''' <summary>
        ''' Mean daily motion (deg/day)
        ''' </summary>
        ''' <value>Mean daily motion</value>
        ''' <returns>Degrees per day</returns>
        ''' <remarks></remarks>
        <DispId(10)> Property n() As Double
        ''' <summary>
        ''' The name of the body.
        ''' </summary>
        ''' <value>The name of the body or packed MPC designation</value>
        ''' <returns>The name of the body or packed MPC designation</returns>
        ''' <remarks>If this instance represents an unnumbered minor planet, Ephemeris.Name must be the 
        ''' packed MPC designation. For other types, this is for display only.</remarks>
        <DispId(11)> Property Name() As String
        ''' <summary>
        ''' The J2000.0 longitude of the ascending node (deg.)
        ''' </summary>
        ''' <value>The J2000.0 longitude of the ascending node</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(12)> Property Node() As Double
        ''' <summary>
        ''' The major or minor planet number
        ''' </summary>
        ''' <value>The major or minor planet number</value>
        ''' <returns>Number or zero if not numbered</returns>
        ''' <remarks></remarks>
        <DispId(13)> Property Number() As Body
        ''' <summary>
        ''' Orbital period (years)
        ''' </summary>
        ''' <value>Orbital period</value>
        ''' <returns>Years</returns>
        ''' <remarks></remarks>
        <DispId(14)> Property P() As Double
        ''' <summary>
        ''' The J2000.0 argument of perihelion (deg.)
        ''' </summary>
        ''' <value>The J2000.0 argument of perihelion</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(15)> Property Peri() As Double
        ''' <summary>
        ''' Perihelion distance (AU)
        ''' </summary>
        ''' <value>Perihelion distance</value>
        ''' <returns>AU</returns>
        ''' <remarks></remarks>
        <DispId(16)> Property q() As Double
        ''' <summary>
        ''' Reciprocal semi-major axis (1/AU)
        ''' </summary>
        ''' <value>Reciprocal semi-major axis</value>
        ''' <returns>1/AU</returns>
        ''' <remarks></remarks>
        <DispId(17)> Property z() As Double
    End Interface
End Namespace
#End Region

#Region "NOVASCOM Interfaces"
Namespace NOVASCOM
    ''' <summary>
    ''' Interface to an Earth object that represents the "state" of the Earth at a given Terrestrial Julian date
    ''' </summary>
    ''' <remarks>Objects of class Earth represent the "state" of the Earth at a given Terrestrial Julian date. 
    ''' The state includes barycentric and heliocentric position vectors for the earth, plus obliquity, 
    ''' nutation and the equation of the equinoxes. Unless set by the client, the Earth ephemeris used is 
    ''' computed using an internal approximation. The client may optionally attach an ephemeris object for 
    ''' increased accuracy. 
    ''' <para><b>Ephemeris Generator</b><br />
    ''' The ephemeris generator object used with NOVAS-COM must support a single 
    ''' method GetPositionAndVelocity(tjd). This method must take a terrestrial Julian date (like the 
    ''' NOVAS-COM methods) as its single parameter, and return an array of Double 
    ''' containing the rectangular (x/y/z) heliocentric J2000 equatorial coordinates of position (AU) and velocity 
    ''' (KM/sec.). In addition, it must support three read/write properties BodyType, Name, and Number, 
    ''' which correspond to the Type, Name, and Number properties of Novas.Planet. 
    ''' </para></remarks>
    <Guid("FF6DA248-BA2A-4a62-BA0A-AAD433EAAC85"), _
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch), _
    ComVisible(True)> _
    Public Interface IEarth
        ''' <summary>
        ''' Initialize the Earth object for given terrestrial Julian date
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian date</param>
        ''' <returns>True if successful, else throws an exception</returns>
        ''' <remarks></remarks>
        <DispId(1)> Function SetForTime(ByVal tjd As Double) As Boolean
        ''' <summary>
        ''' Earth barycentric position
        ''' </summary>
        ''' <value>Barycentric position vector</value>
        ''' <returns>AU (Ref J2000)</returns>
        ''' <remarks></remarks>
        <DispId(2)> ReadOnly Property BarycentricPosition() As PositionVector
        ''' <summary>
        ''' Earth barycentric time 
        ''' </summary>
        ''' <value>Barycentric dynamical time for given Terrestrial Julian Date</value>
        ''' <returns>Julian date</returns>
        ''' <remarks></remarks>
        <DispId(3)> ReadOnly Property BarycentricTime() As Double
        ''' <summary>
        ''' Earth barycentric velocity 
        ''' </summary>
        ''' <value>Barycentric velocity vector</value>
        ''' <returns>AU/day (ref J2000)</returns>
        ''' <remarks></remarks>
        <DispId(4)> ReadOnly Property BarycentricVelocity() As VelocityVector
        ''' <summary>
        ''' Ephemeris object used to provide the position of the Earth.
        ''' </summary>
        ''' <value>Earth ephemeris object </value>
        ''' <returns>Earth ephemeris object</returns>
        ''' <remarks>
        ''' Setting this is optional, if not set, the internal Kepler engine will be used.</remarks>
        <DispId(5)> Property EarthEphemeris() As IEphemeris
        ''' <summary>
        ''' Earth equation of equinoxes 
        ''' </summary>
        ''' <value>Equation of the equinoxes</value>
        ''' <returns>Seconds</returns>
        ''' <remarks></remarks>
        <DispId(6)> ReadOnly Property EquationOfEquinoxes() As Double
        ''' <summary>
        ''' Earth heliocentric position
        ''' </summary>
        ''' <value>Heliocentric position vector</value>
        ''' <returns>AU (ref J2000)</returns>
        ''' <remarks></remarks>
        <DispId(7)> ReadOnly Property HeliocentricPosition() As PositionVector
        ''' <summary>
        ''' Earth heliocentric velocity 
        ''' </summary>
        ''' <value>Heliocentric velocity</value>
        ''' <returns>Velocity vector, AU/day (ref J2000)</returns>
        ''' <remarks></remarks>
        <DispId(8)> ReadOnly Property HeliocentricVelocity() As VelocityVector
        ''' <summary>
        ''' Earth mean objiquity
        ''' </summary>
        ''' <value>Mean obliquity of the ecliptic</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(9)> ReadOnly Property MeanObliquity() As Double
        ''' <summary>
        ''' Earth nutation in longitude 
        ''' </summary>
        ''' <value>Nutation in longitude</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(10)> ReadOnly Property NutationInLongitude() As Double
        ''' <summary>
        ''' Earth nutation in obliquity 
        ''' </summary>
        ''' <value>Nutation in obliquity</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(11)> ReadOnly Property NutationInObliquity() As Double
        ''' <summary>
        ''' Earth true obliquity 
        ''' </summary>
        ''' <value>True obliquity of the ecliptic</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(12)> ReadOnly Property TrueObliquity() As Double
    End Interface

    ''' <summary>
    ''' Interface to a Planet component that provides characteristics of a solar system body
    ''' </summary>
    ''' <remarks>Objects of class Planet hold the characteristics of a solar system body. Properties are 
    ''' type (major or minor planet), number (for major and numbered minor planets), name (for unnumbered 
    ''' minor planets and comets), the ephemeris object to be used for orbital calculations, an optional 
    ''' ephemeris object to use for barycenter calculations, and an optional value for delta-T. 
    ''' <para>The high-level NOVAS astrometric functions are implemented as methods of Planet: 
    ''' GetTopocentricPosition(), GetLocalPosition(), GetApparentPosition(), GetVirtualPosition(), 
    ''' and GetAstrometricPosition(). These methods operate on the properties of the Planet, and produce 
    ''' a PositionVector object. For example, to get the topocentric coordinates of a planet, create and 
    ''' initialize a planet, create initialize and attach an ephemeris object, then call 
    ''' Planet.GetTopocentricPosition(). The resulting PositionVector's right ascension and declination 
    ''' properties are the topocentric equatorial coordinates, at the same time, the (optionally 
    ''' refracted) alt-az coordinates are calculated, and are also contained within the returned 
    ''' PositionVector. <b>Note that Alt/Az is available in PositionVectors returned from calling 
    ''' GetTopocentricPosition().</b> The accuracy of these calculations is typically dominated by the accuracy 
    ''' of the attached ephemeris generator. </para>
    ''' <para><b>Ephemeris Generator</b><br />
    ''' By default, Kepler instances are attached for both Earth and Planet objects so it is
    ''' not necessary to create and attach these in order to get Kepler accuracy from this
    ''' component</para>
    ''' <para>The ephemeris generator object used with NOVAS-COM must support a single 
    ''' method GetPositionAndVelocity(tjd). This method must take a terrestrial Julian date (like the 
    ''' NOVAS-COM methods) as its single parameter, and return an array of Double 
    ''' containing the rectangular (x/y/z) heliocentric J2000 equatorial coordinates of position (AU) and velocity 
    ''' (KM/sec.). In addition, it must support three read/write properties BodyType, Name, and Number, 
    ''' which correspond to the Type, Name, and Number properties of Novas.Planet. 
    ''' </para>
    '''</remarks>
    <Guid("CAE65556-EA7A-4252-BF28-D0E967AEF04D"), _
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch), _
    ComVisible(True)> _
    Public Interface IPlanet
        ''' <summary>
        ''' Get an apparent position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the apparent place.</returns>
        ''' <remarks></remarks>
        <DispId(1)> Function GetApparentPosition(ByVal tjd As Double) As PositionVector
        ''' <summary>
        ''' Get an astrometric position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the astrometric place.</returns>
        ''' <remarks></remarks>
        <DispId(2)> Function GetAstrometricPosition(ByVal tjd As Double) As PositionVector
        ''' <summary>
        ''' Get an local position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">The observing site</param>
        ''' <returns>PositionVector for the local place.</returns>
        ''' <remarks></remarks>
        <DispId(3)> Function GetLocalPosition(ByVal tjd As Double, <MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site) As PositionVector
        ''' <summary>
        ''' Get a topocentric position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">The observing site</param>
        ''' <param name="Refract">Apply refraction correction</param>
        ''' <returns>PositionVector for the topocentric place.</returns>
        ''' <remarks></remarks>
        <DispId(4)> Function GetTopocentricPosition(ByVal tjd As Double, <MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site, ByVal Refract As Boolean) As PositionVector
        ''' <summary>
        ''' Get a virtual position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the virtual place.</returns>
        ''' <remarks></remarks>
        <DispId(5)> Function GetVirtualPosition(ByVal tjd As Double) As PositionVector
        ''' <summary>
        ''' Planet delta-T
        ''' </summary>
        ''' <value>The value of delta-T (TT - UT1) to use for reductions</value>
        ''' <returns>Seconds</returns>
        ''' <remarks>Setting this value is optional. If no value is set, an internal delta-T generator is used.</remarks>
        <DispId(6)> Property DeltaT() As Double
        ''' <summary>
        ''' Ephemeris object used to provide the position of the Earth.
        ''' </summary>
        ''' <value>Earth ephemeris object</value>
        ''' <returns>Earth ephemeris object</returns>
        ''' <remarks>
        ''' Setting this is optional, if not set, the internal Kepler engine will be used.</remarks>
        <DispId(7)> Property EarthEphemeris() As IEphemeris
        ''' <summary>
        ''' The Ephemeris object used to provide positions of solar system bodies.
        ''' </summary>
        ''' <value>Body ephemeris object</value>
        ''' <returns>Body ephemeris object</returns>
        ''' <remarks>
        ''' Setting this is optional, if not set, the internal Kepler engine will be used.
        ''' </remarks>
        <DispId(8)> Property Ephemeris() As IEphemeris
        ''' <summary>
        ''' Planet name
        ''' </summary>
        ''' <value>For unnumbered minor planets, (Type=nvMinorPlanet and Number=0), the packed designation 
        ''' for the minor planet. For other types, this is not significant, but may be used to store 
        ''' a name.</value>
        ''' <returns>Name of planet</returns>
        ''' <remarks></remarks>
        <DispId(9)> Property Name() As String
        ''' <summary>
        ''' Planet number
        ''' </summary>
        ''' <value>For major planets (Type=nvMajorPlanet), a PlanetNumber value. For minor planets 
        ''' (Type=nvMinorPlanet), the number of the minor planet or 0 for unnumbered minor planet.</value>
        ''' <returns>Planet number</returns>
        ''' <remarks>The major planet number is its number out from the sun starting with Mercury = 1</remarks>
        <DispId(10)> Property Number() As Integer
        ''' <summary>
        ''' The type of solar system body
        ''' </summary>
        ''' <value>The type of solar system body</value>
        ''' <returns>Value from the BodyType enum</returns>
        ''' <remarks></remarks>
        <DispId(11)> Property Type() As BodyType
    End Interface

    ''' <summary>
    ''' Interface to the NOVAS-COM PositionVector Class
    ''' </summary>
    ''' <remarks>Objects of class PositionVector contain vectors used for positions (earth, sites, 
    ''' stars and planets) throughout NOVAS-COM. Of course, its properties include the x, y, and z 
    ''' components of the position. Additional properties are right ascension and declination, distance, 
    ''' and light time (applicable to star positions), and Alt/Az (available only in PositionVectors 
    ''' returned by Star or Planet methods GetTopocentricPosition()). You can initialize a PositionVector 
    ''' from a Star object (essentially an FK5 or HIP catalog entry) or a Site (lat/long/height). 
    ''' PositionVector has methods that can adjust the coordinates for precession, aberration and 
    ''' proper motion. Thus, a PositionVector object gives access to some of the lower-level NOVAS functions. 
    ''' <para><b>Note:</b> The equatorial coordinate properties of this object are dependent variables, and thus are read-only. Changing any cartesian coordinate will cause the equatorial coordinates to be recalculated. 
    ''' </para></remarks>
    <Guid("A3B6F9AA-B331-47c7-B8F0-4FBECF0638AA"), _
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch), _
    ComVisible(True)> _
    Public Interface IPositionVector
        ''' <summary>
        ''' Adjust the position vector of an object for aberration of light
        ''' </summary>
        ''' <param name="vel">The velocity vector of the observer</param>
        ''' <remarks>The algorithm includes relativistic terms</remarks>
        <DispId(1)> Sub Aberration(<MarshalAs(UnmanagedType.IDispatch)> ByVal vel As VelocityVector)
        ''' <summary>
        ''' Adjust the position vector for precession of equinoxes between two given epochs
        ''' </summary>
        ''' <param name="tjd">The first epoch (Terrestrial Julian Date)</param>
        ''' <param name="tjd2">The second epoch (Terrestrial Julian Date)</param>
        ''' <remarks>The coordinates are referred to the mean equator and equinox of the two respective epochs.</remarks>
        <DispId(2)> Sub Precess(ByVal tjd As Double, ByVal tjd2 As Double)
        ''' <summary>
        ''' Adjust the position vector for proper motion (including foreshortening effects)
        ''' </summary>
        ''' <param name="vel">The velocity vector of the object</param>
        ''' <param name="tjd1">The first epoch (Terrestrial Julian Date)</param>
        ''' <param name="tjd2">The second epoch (Terrestrial Julian Date)</param>
        ''' <returns>True if successful or throws an exception.</returns>
        ''' <remarks></remarks>
        ''' <exception cref="Exceptions.ValueNotSetException">If the position vector x, y or z values has not been set</exception>
        ''' <exception cref="Exceptions.ValueNotAvailableException">If the supplied velocity vector does not have valid x, y and z components</exception>
        <DispId(3)> Function ProperMotion(<MarshalAs(UnmanagedType.IDispatch)> ByVal vel As VelocityVector, ByVal tjd1 As Double, ByVal tjd2 As Double) As Boolean
        ''' <summary>
        ''' Initialize the PositionVector from a Site object and Greenwich apparent sidereal time.
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="gast">Greenwich Apparent Sidereal Time</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks>The GAST parameter must be for Greenwich, not local. The time is rotated through the 
        ''' site longitude. See SetFromSiteJD() for an equivalent method that takes UTC Julian Date and 
        ''' Delta-T (eliminating the need for calculating hyper-accurate GAST yourself).</remarks>
        <DispId(4)> Function SetFromSite(<MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site, ByVal gast As Double) As Boolean
        ''' <summary>
        ''' Initialize the PositionVector from a Site object using UTC Julian date and Delta-T
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="ujd">UTC Julian Date</param>
        ''' <param name="delta_t">The value of Delta-T (TT - UT1) to use for reductions (seconds)</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks>The Julian date must be UTC Julian date, not terrestrial.
        ''' </remarks>
        <DispId(5)> Overloads Function SetFromSiteJD(<MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double) As Boolean
        ''' <summary>
        ''' Initialize the PositionVector from a Star object.
        ''' </summary>
        ''' <param name="star">The Star object from which to initialize</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks></remarks>
        ''' <exception cref="Exceptions.ValueNotAvailableException">If Parallax, RightAScension or Declination is not available in the supplied star object.</exception>
        <DispId(6)> Function SetFromStar(<MarshalAs(UnmanagedType.IDispatch)> ByVal star As Star) As Boolean
        ''' <summary>
        ''' The azimuth coordinate (degrees, + east)
        ''' </summary>
        ''' <value>The azimuth coordinate</value>
        ''' <returns>Degrees, + East</returns>
        ''' <remarks></remarks>
        <DispId(7)> ReadOnly Property Azimuth() As Double
        ''' <summary>
        ''' Declination coordinate
        ''' </summary>
        ''' <value>Declination coordinate</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(8)> ReadOnly Property Declination() As Double
        ''' <summary>
        ''' Distance/Radius coordinate
        ''' </summary>
        ''' <value>Distance/Radius coordinate</value>
        ''' <returns>AU</returns>
        ''' <remarks></remarks>
        <DispId(9)> ReadOnly Property Distance() As Double
        ''' <summary>
        ''' The elevation (altitude) coordinate (degrees, + up)
        ''' </summary>
        ''' <value>The elevation (altitude) coordinate (degrees, + up)</value>
        ''' <returns>(Degrees, + up</returns>
        ''' <remarks>Elevation is available only in PositionVectors returned from calls to 
        ''' Star.GetTopocentricPosition() and/or Planet.GetTopocentricPosition(). </remarks>
        ''' <exception cref="Exceptions.ValueNotAvailableException">When the position vector has not been 
        ''' initialised from Star.GetTopoCentricPosition and Planet.GetTopocentricPosition</exception>
        <DispId(10)> ReadOnly Property Elevation() As Double
        ''' <summary>
        ''' Light time from body to origin, days.
        ''' </summary>
        ''' <value>Light time from body to origin</value>
        ''' <returns>Days</returns>
        ''' <remarks></remarks>
        <DispId(11)> ReadOnly Property LightTime() As Double
        ''' <summary>
        ''' RightAscension coordinate, hours
        ''' </summary>
        ''' <value>RightAscension coordinate</value>
        ''' <returns>Hours</returns>
        ''' <remarks></remarks>
        <DispId(12)> ReadOnly Property RightAscension() As Double
        ''' <summary>
        ''' Position cartesian x component
        ''' </summary>
        ''' <value>Cartesian x component</value>
        ''' <returns>Cartesian x component</returns>
        ''' <remarks></remarks>
        <DispId(13)> Property x() As Double
        ''' <summary>
        ''' Position cartesian y component
        ''' </summary>
        ''' <value>Cartesian y component</value>
        ''' <returns>Cartesian y component</returns>
        ''' <remarks></remarks>
        <DispId(14)> Property y() As Double
        ''' <summary>
        ''' Position cartesian z component
        ''' </summary>
        ''' <value>Cartesian z component</value>
        ''' <returns>Cartesian z component</returns>
        ''' <remarks></remarks>
        <DispId(15)> Property z() As Double
    End Interface

    ''' <summary>
    ''' Interface for PositionVector methods that are only accessible through .NET and not through COM
    ''' </summary>
    ''' <remarks></remarks>
    <ComVisible(False)> _
    Public Interface IPositionVectorExtra
        ''' <summary>
        ''' Initialize the PositionVector from a Site object using UTC Julian date
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="ujd">UTC Julian Date</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks>The Julian date must be UTC Julian date, not terrestrial. Calculations will use the internal delta-T tables and estimator to get 
        ''' delta-T. 
        ''' This overload is not available through COM, please use 
        ''' "SetFromSiteJD(ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double)"
        ''' with delta_t set to 0.0 to achieve this effect.
        ''' </remarks>
        Overloads Function SetFromSiteJD(<MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site, ByVal ujd As Double) As Boolean
    End Interface

    ''' <summary>
    ''' Interface to the NOVAS-COM Site Class
    ''' </summary>
    ''' <remarks>Objects of class Site contain the specifications for an observer's location on the Earth 
    ''' ellipsoid. Properties are latitude, longitude, height above mean sea level, the ambient temperature 
    ''' and the sea-level barmetric pressure. The latter two are used only for optional refraction corrections. 
    ''' Latitude and longitude are (common) geodetic, not geocentric. </remarks>
    <Guid("2414C071-8A5B-4d53-89BC-CAF30BA7123B"), _
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch), _
    ComVisible(True)> _
    Public Interface ISite
        ''' <summary>
        ''' Set all site properties in one method call
        ''' </summary>
        ''' <param name="Latitude">The geodetic latitude (degrees, + north)</param>
        ''' <param name="Longitude">The geodetic longitude (degrees, +east)</param>
        ''' <param name="Height">Height above sea level (meters)</param>
        ''' <remarks></remarks>
        <DispId(1)> Sub [Set](ByVal Latitude As Double, ByVal Longitude As Double, ByVal Height As Double)
        ''' <summary>
        ''' Height above mean sea level
        ''' </summary>
        ''' <value>Height above mean sea level</value>
        ''' <returns>Meters</returns>
        ''' <remarks></remarks>
        <DispId(2)> Property Height() As Double
        ''' <summary>
        ''' Geodetic latitude (degrees, + north)
        ''' </summary>
        ''' <value>Geodetic latitude</value>
        ''' <returns>Degrees, + north</returns>
        ''' <remarks></remarks>
        <DispId(3)> Property Latitude() As Double
        ''' <summary>
        ''' Geodetic longitude (degrees, + east)
        ''' </summary>
        ''' <value>Geodetic longitude</value>
        ''' <returns>Degrees, + east</returns>
        ''' <remarks></remarks>
        <DispId(4)> Property Longitude() As Double
        ''' <summary>
        ''' Barometric pressure (millibars)
        ''' </summary>
        ''' <value>Barometric pressure</value>
        ''' <returns>Millibars</returns>
        ''' <remarks></remarks>
        <DispId(5)> Property Pressure() As Double
        ''' <summary>
        ''' Ambient temperature (deg. Celsius)
        ''' </summary>
        ''' <value>Ambient temperature</value>
        ''' <returns>Degrees Celsius)</returns>
        ''' <remarks></remarks>
        <DispId(6)> Property Temperature() As Double
    End Interface

    ''' <summary>
    ''' Interface to the NOVAS-COM Star Class
    ''' </summary>
    ''' <remarks>Objects of class Site contain the specifications for a star's catalog position in either FK5 or Hipparcos units (both must be J2000). Properties are right ascension and declination, proper motions, parallax, radial velocity, catalog type (FK5 or HIP), catalog number, optional ephemeris engine to use for barycenter calculations, and an optional value for delta-T. Unless you specifically set the DeltaT property, calculations performed by this class which require the value of delta-T (TT - UT1) rely on an internal function to estimate delta-T. 
    '''<para>The high-level NOVAS astrometric functions are implemented as methods of Star: 
    ''' GetTopocentricPosition(), GetLocalPosition(), GetApparentPosition(), GetVirtualPosition(), 
    ''' and GetAstrometricPosition(). These methods operate on the properties of the Star, and produce 
    ''' a PositionVector object. For example, to get the topocentric coordinates of a star, simply create 
    ''' and initialize a Star, then call Star.GetTopocentricPosition(). The resulting vaPositionVector's 
    ''' right ascension and declination properties are the topocentric equatorial coordinates, at the same 
    ''' time, the (optionally refracted) alt-az coordinates are calculated, and are also contained within 
    ''' the returned PositionVector. <b>Note that Alt/Az is available in PositionVectors returned from calling 
    ''' GetTopocentricPosition().</b></para></remarks>
    <Guid("89145C95-9B78-494e-99FE-BD2EF4386096"), _
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch), _
    ComVisible(True)> _
    Public Interface IStar
        ''' <summary>
        ''' Initialize all star properties with one call
        ''' </summary>
        ''' <param name="RA">Catalog mean right ascension (hours)</param>
        ''' <param name="Dec">Catalog mean declination (degrees)</param>
        ''' <param name="ProMoRA">Catalog mean J2000 proper motion in right ascension (sec/century)</param>
        ''' <param name="ProMoDec">Catalog mean J2000 proper motion in declination (arcsec/century)</param>
        ''' <param name="Parallax">Catalog mean J2000 parallax (arcsec)</param>
        ''' <param name="RadVel">Catalog mean J2000 radial velocity (km/sec)</param>
        ''' <remarks>Assumes positions are FK5. If Parallax is set to zero, NOVAS-COM assumes the object 
        ''' is on the "celestial sphere", which has a distance of 10 megaparsecs. </remarks>
        <DispId(1)> Sub [Set](ByVal RA As Double, ByVal Dec As Double, ByVal ProMoRA As Double, ByVal ProMoDec As Double, ByVal Parallax As Double, ByVal RadVel As Double)
        ''' <summary>
        ''' Initialise all star properties in one call using Hipparcos data. Transforms to FK5 standard used by NOVAS.
        ''' </summary>
        ''' <param name="RA">Catalog mean right ascension (hours)</param>
        ''' <param name="Dec">Catalog mean declination (degrees)</param>
        ''' <param name="ProMoRA">Catalog mean J2000 proper motion in right ascension (sec/century)</param>
        ''' <param name="ProMoDec">Catalog mean J2000 proper motion in declination (arcsec/century)</param>
        ''' <param name="Parallax">Catalog mean J2000 parallax (arcsec)</param>
        ''' <param name="RadVel">Catalog mean J2000 radial velocity (km/sec)</param>
        ''' <remarks>Assumes positions are Hipparcos standard and transforms to FK5 standard used by NOVAS. 
        ''' <para>If Parallax is set to zero, NOVAS-COM assumes the object is on the "celestial sphere", 
        ''' which has a distance of 10 megaparsecs.</para>
        ''' </remarks>
        <DispId(2)> Sub SetHipparcos(ByVal RA As Double, ByVal Dec As Double, ByVal ProMoRA As Double, ByVal ProMoDec As Double, ByVal Parallax As Double, ByVal RadVel As Double)
        ''' <summary>
        ''' Get an apparent position for a given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the apparent place.</returns>
        ''' <remarks></remarks>
        <DispId(3)> Function GetApparentPosition(ByVal tjd As Double) As PositionVector
        ''' <summary>
        ''' Get an astrometric position for a given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the astrometric place.</returns>
        ''' <remarks></remarks>
        <DispId(4)> Function GetAstrometricPosition(ByVal tjd As Double) As PositionVector
        ''' <summary>
        ''' Get a local position for a given site and time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">A Site object representing the observing site</param>
        ''' <returns>PositionVector for the local place.</returns>
        ''' <remarks></remarks>
        <DispId(5)> Function GetLocalPosition(ByVal tjd As Double, <MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site) As PositionVector
        ''' <summary>
        ''' Get a topocentric position for a given site and time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">A Site object representing the observing site</param>
        ''' <param name="Refract">True to apply atmospheric refraction corrections</param>
        ''' <returns>PositionVector for the topocentric place.</returns>
        ''' <remarks></remarks>
        <DispId(6)> Function GetTopocentricPosition(ByVal tjd As Double, <MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site, ByVal Refract As Boolean) As PositionVector
        ''' <summary>
        ''' Get a virtual position at a given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the virtual place.</returns>
        ''' <remarks></remarks>
        <DispId(7)> Function GetVirtualPosition(ByVal tjd As Double) As PositionVector
        ''' <summary>
        ''' Three character catalog code for the star's data
        ''' </summary>
        ''' <value>Three character catalog code for the star's data</value>
        ''' <returns>Three character catalog code for the star's data</returns>
        ''' <remarks>Typically "FK5" but may be "HIP". For information only.</remarks>
        <DispId(8)> Property Catalog() As String
        ''' <summary>
        ''' Mean catalog J2000 declination coordinate (degrees)
        ''' </summary>
        ''' <value>Mean catalog J2000 declination coordinate</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        <DispId(9)> Property Declination() As Double
        ''' <summary>
        ''' The value of delta-T (TT - UT1) to use for reductions.
        ''' </summary>
        ''' <value>The value of delta-T (TT - UT1) to use for reductions.</value>
        ''' <returns>Seconds</returns>
        ''' <remarks>If this property is not set, calculations will use an internal function to estimate delta-T.</remarks>
        <DispId(10)> Property DeltaT() As Double
        ''' <summary>
        ''' Ephemeris object used to provide the position of the Earth.
        ''' </summary>
        ''' <value>Ephemeris object used to provide the position of the Earth.</value>
        ''' <returns>Ephemeris object</returns>
        ''' <remarks>If this value is not set, an internal Kepler object will be used to determine 
        ''' Earth ephemeris</remarks>
        <DispId(11)> Property EarthEphemeris() As Object
        ''' <summary>
        ''' The catalog name of the star (50 char max)
        ''' </summary>
        ''' <value>The catalog name of the star</value>
        ''' <returns>Name (50 char max)</returns>
        ''' <remarks></remarks>
        <DispId(12)> Property Name() As String
        ''' <summary>
        ''' The catalog number of the star
        ''' </summary>
        ''' <value>The catalog number of the star</value>
        ''' <returns>The catalog number of the star</returns>
        ''' <remarks></remarks>
        <DispId(13)> Property Number() As Integer
        ''' <summary>
        ''' Catalog mean J2000 parallax (arcsec)
        ''' </summary>
        ''' <value>Catalog mean J2000 parallax</value>
        ''' <returns>Arc seconds</returns>
        ''' <remarks></remarks>
        <DispId(14)> Property Parallax() As Double
        ''' <summary>
        ''' Catalog mean J2000 proper motion in declination (arcsec/century)
        ''' </summary>
        ''' <value>Catalog mean J2000 proper motion in declination</value>
        ''' <returns>Arc seconds per century</returns>
        ''' <remarks></remarks>
        <DispId(15)> Property ProperMotionDec() As Double
        ''' <summary>
        ''' Catalog mean J2000 proper motion in right ascension (sec/century)
        ''' </summary>
        ''' <value>Catalog mean J2000 proper motion in right ascension</value>
        ''' <returns>Seconds per century</returns>
        ''' <remarks></remarks>
        <DispId(16)> Property ProperMotionRA() As Double
        ''' <summary>
        ''' Catalog mean J2000 radial velocity (km/sec)
        ''' </summary>
        ''' <value>Catalog mean J2000 radial velocity</value>
        ''' <returns>Kilometers per second</returns>
        ''' <remarks></remarks>
        <DispId(17)> Property RadialVelocity() As Double
        ''' <summary>
        ''' Catalog mean J2000 right ascension coordinate (hours)
        ''' </summary>
        ''' <value>Catalog mean J2000 right ascension coordinate</value>
        ''' <returns>Hours</returns>
        ''' <remarks></remarks>
        <DispId(18)> Property RightAscension() As Double
    End Interface

    ''' <summary>
    ''' interface to the NOVAS_COM VelocityVector Class
    ''' </summary>
    ''' <remarks>Objects of class VelocityVector contain vectors used for velocities (earth, sites, 
    ''' planets, and stars) throughout NOVAS-COM. Of course, its properties include the x, y, and z 
    ''' components of the velocity. Additional properties are the velocity in equatorial coordinates of 
    ''' right ascension dot, declination dot and radial velocity. You can initialize a PositionVector from 
    ''' a Star object (essentially an FK5 or HIP catalog entry) or a Site (lat/long/height). For the star 
    ''' object the proper motions, distance and radial velocity are used, for a site, the velocity is that 
    ''' of the observer with respect to the Earth's center of mass. </remarks>
    <Guid("8DD80835-29C6-49d6-8E4D-8887B20E707E"), _
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch), _
    ComVisible(True)> _
    Public Interface IVelocityVector
        ''' <summary>
        ''' Initialize the VelocityVector from a Site object and Greenwich Apparent Sdereal Time.
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="gast">Greenwich Apparent Sidereal Time</param>
        ''' <returns>True if OK or throws an exception</returns>
        ''' <remarks>The velocity vector is that of the observer with respect to the Earth's center 
        ''' of mass. The GAST parameter must be for Greenwich, not local. The time is rotated through 
        ''' the site longitude. See SetFromSiteJD() for an equivalent method that takes UTC Julian 
        ''' Date and optionally Delta-T (eliminating the need for calculating hyper-accurate GAST yourself). </remarks>
        <DispId(1)> Function SetFromSite(<MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site, ByVal gast As Double) As Boolean
        ''' <summary>
        ''' Initialize the VelocityVector from a Site object using UTC Julian Date and Delta-T
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="ujd">UTC Julian Date</param>
        ''' <param name="delta_t">The optional value of Delta-T (TT - UT1) to use for reductions (seconds)</param>
        ''' <returns>True if OK otherwise throws an exception</returns>
        ''' <remarks>The velocity vector is that of the observer with respect to the Earth's center 
        ''' of mass. The Julian date must be UTC Julian date, not terrestrial.</remarks>
        <DispId(2)> Function SetFromSiteJD(<MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double) As Boolean
        ''' <summary>
        ''' Initialize the VelocityVector from a Star object.
        ''' </summary>
        ''' <param name="star">The Star object from which to initialize</param>
        ''' <returns>True if OK otherwise throws an exception</returns>
        ''' <remarks>The proper motions, distance and radial velocity are used in the velocity calculation. </remarks>
        ''' <exception cref="Exceptions.ValueNotAvailableException">If any of: Parallax, RightAscension, Declination, 
        ''' ProperMotionRA, ProperMotionDec or RadialVelocity are not available in the star object</exception>
        <DispId(3)> Function SetFromStar(<MarshalAs(UnmanagedType.IDispatch)> ByVal star As Star) As Boolean
        ''' <summary>
        '''  Linear velocity along the declination direction (AU/day)
        ''' </summary>
        ''' <value>Linear velocity along the declination direction</value>
        ''' <returns>AU/day</returns>
        ''' <remarks>This is not the proper motion (which is an angular rate and is dependent on the distance to the object).</remarks>
        <DispId(4)> ReadOnly Property DecVelocity() As Double
        ''' <summary>
        ''' Linear velocity along the radial direction (AU/day)
        ''' </summary>
        ''' <value>Linear velocity along the radial direction</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        <DispId(5)> ReadOnly Property RadialVelocity() As Double
        ''' <summary>
        ''' Linear velocity along the right ascension direction (AU/day)
        ''' </summary>
        ''' <value>Linear velocity along the right ascension direction</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        <DispId(6)> ReadOnly Property RAVelocity() As Double
        ''' <summary>
        ''' Cartesian x component of velocity (AU/day)
        ''' </summary>
        ''' <value>Cartesian x component of velocity</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        <DispId(7)> Property x() As Double
        ''' <summary>
        ''' Cartesian y component of velocity (AU/day)
        ''' </summary>
        ''' <value>Cartesian y component of velocity</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        <DispId(8)> Property y() As Double
        ''' <summary>
        ''' Cartesian z component of velocity (AU/day)
        ''' </summary>
        ''' <value>Cartesian z component of velocity</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        <DispId(9)> Property z() As Double
    End Interface

    ''' <summary>
    ''' Interface for VelocityVector methods that are only accessible through .NET and not through COM
    ''' </summary>
    ''' <remarks></remarks>
    <ComVisible(False)> _
    Public Interface IVelocityVectorExtra
        ''' <summary>
        ''' Initialize the VelocityVector from a Site object using UTC Julian Date
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="ujd">UTC Julian Date</param>
        ''' <returns>True if OK otherwise throws an exception</returns>
        ''' <remarks>The velocity vector is that of the observer with respect to the Earth's center 
        ''' of mass. The Julian date must be UTC Julian date, not terrestrial. This call will use 
        ''' the internal tables and estimator to get delta-T.
        ''' This overload is not available through COM, please use 
        ''' "SetFromSiteJD(ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double)"
        ''' with delta_t set to 0.0 to achieve this effect.
        ''' </remarks>
        Function SetFromSiteJD(<MarshalAs(UnmanagedType.IDispatch)> ByVal site As Site, ByVal ujd As Double) As Boolean
    End Interface

End Namespace
#End Region

#Region "NOVAS2 Interface"
Namespace NOVAS

    ''' <summary>
    ''' Interface to the NOVAS2 component
    ''' </summary>
    ''' <remarks>Implemented by the NOVAS2COM component</remarks>
    <Guid("3D201554-007C-47e6-805D-F66D1CA35543"), _
        ComVisible(True)> _
    Public Interface INOVAS2
        ''' <summary>
        ''' Computes the apparent place of a star 
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for apparent place.</param>
        ''' <param name="earth">Structure containing the body designation for the earth</param>
        ''' <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units (defined in novas.h).</param>
        ''' <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <returns><pre>
        '''  0...Everything OK
        ''' >0...Error code from function 'solarsystem'.</pre></returns>
        ''' <remarks></remarks>
        <DispId(1)> Function AppStar(ByVal tjd As Double, _
                                            ByRef earth As BodyDescription, _
                                            ByRef star As CatEntry, _
                                            ByRef ra As Double, _
                                            ByRef dec As Double) As Short
        ''' <summary>
        ''' Computes the topocentric place of a star
        ''' </summary>
        ''' <param name="tjd"> TT (or TDT) Julian date for topocentric place.</param>
        ''' <param name="earth">Structure containing the body designation for the Earth.</param>
        ''' <param name="deltat">Difference TT (or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units.</param>
        ''' <param name="location">Structure containing observer's location</param>
        ''' <param name="ra">OUT: Topocentric right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec">OUT: Topocentric declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'.</pre></returns>
        ''' <remarks></remarks>
        <DispId(2)> Function TopoStar(ByVal tjd As Double, _
                                         ByRef earth As BodyDescription, _
                                         ByVal deltat As Double, _
                                         ByRef star As CatEntry, _
                                         ByRef location As SiteInfo, _
                                         ByRef ra As Double, _
                                         ByRef dec As Double) As Short

        ''' <summary>
        ''' Compute the apparent place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for apparent place.</param>
        ''' <param name="ss_object">Structure containing the body designation for the solar system body</param>
        ''' <param name="earth">Structure containing the body designation for the Earth</param>
        ''' <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet at 'tjd' in AU.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.
        ''' </pre></returns>
        ''' <remarks>
        ''' <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        ''' used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        ''' can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        ''' </remarks>
        <DispId(3)> Function AppPlanet(ByVal tjd As Double, _
                                          ByRef ss_object As BodyDescription, _
                                          ByRef earth As BodyDescription, _
                                          ByRef ra As Double, _
                                          ByRef dec As Double, _
                                          ByRef dis As Double) As Short

        ''' <summary>
        ''' Computes the topocentric place of a planet, given the location of the observer.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for topocentric place.</param>
        ''' <param name="ss_object">structure containing the body designation for the solar system body</param>
        ''' <param name="earth">structure containing the body designation for the Earth</param>
        ''' <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="location">structure containing observer's location</param>
        ''' <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet at 'tjd' in AU.</param>
        ''' <returns> <pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.
        ''' </pre></returns>
        ''' <remarks>
        ''' <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        ''' used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        ''' can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        ''' </remarks>
        <DispId(4)> Function TopoPlanet(ByVal tjd As Double, _
                                           ByRef ss_object As BodyDescription, _
                                           ByRef earth As BodyDescription, _
                                           ByVal deltat As Double, _
                                           ByRef location As SiteInfo, _
                                           ByRef ra As Double, _
                                           ByRef dec As Double, _
                                           ByRef dis As Double) As Short

        ''' <summary>
        '''  Computes the virtual place of a star
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for virtual place.</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth.</param>
        ''' <param name="star">Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        ''' <param name="ra">OUT: Virtual right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT: Virtual declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'
        ''' </pre></returns>
        ''' <remarks>
        ''' Computes the virtual place of a star at date 'tjd', given its 
        ''' mean place, proper motion, parallax, and radial velocity for J2000.0.</remarks>
        <DispId(5)> Function VirtualStar(ByVal tjd As Double, _
                                                ByRef earth As BodyDescription, _
                                                ByRef star As CatEntry, _
                                                ByRef ra As Double, _
                                                ByRef dec As Double) As Short

        ''' <summary>
        '''  Computes the local place of a star
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for local place.</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="star">Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        ''' <param name="location">Pointer to structure containing observer's location</param>
        ''' <param name="ra">OUT: Local right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT: Local declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'.
        '''</pre></returns>
        ''' <remarks></remarks>
        <DispId(6)> Function LocalStar(ByVal tjd As Double, _
                                              ByRef earth As BodyDescription, _
                                              ByVal deltat As Double, _
                                              ByRef star As CatEntry, _
                                              ByRef location As SiteInfo, _
                                              ByRef ra As Double, _
                                              ByRef dec As Double) As Short

        ''' <summary>
        ''' Computes the virtual place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for virtual place.</param>
        ''' <param name="ss_object">Pointer to structure containing the body designation for the solar system body</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="ra">OUT: Virtual right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT: Virtual declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.
        ''' </pre></returns>
        ''' <remarks>
        ''' <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        ''' used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        ''' can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        ''' </remarks>
        <DispId(7)> Function VirtualPlanet(ByVal tjd As Double, _
                                                  ByRef ss_object As BodyDescription, _
                                                  ByRef earth As BodyDescription, _
                                                  ByRef ra As Double, _
                                                  ByRef dec As Double, _
                                                  ByRef dis As Double) As Short

        ''' <summary>
        '''  Computes the local place of a planet or other solar system body, given the location of the observer.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for local place.</param>
        ''' <param name="ss_object"> Pointer to structure containing the body designation for the solar system body</param>
        ''' <param name="earth"> Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="location"> Pointer to structure containing observer's location</param>
        ''' <param name="ra">OUT: Local right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT: Local declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.
        ''' </pre></returns>
        ''' <remarks>
        ''' <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        ''' used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        ''' can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        ''' </remarks>
        <DispId(8)> Function LocalPlanet(ByVal tjd As Double, _
                                                ByRef ss_object As BodyDescription, _
                                                ByRef earth As BodyDescription, _
                                                ByVal deltat As Double, _
                                                ByRef location As SiteInfo, _
                                                ByRef ra As Double, _
                                                ByRef dec As Double, _
                                                ByRef dis As Double) As Short

        ''' <summary>
        ''' Computes the astrometric place of a star
        ''' </summary>
        ''' <param name="tjd">  TT (or TDT) Julian date for astrometric place.</param>
        ''' <param name="earth"> Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="star"> Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        ''' <param name="ra">OUT:  Astrometric right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT:  Astrometric declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'.
        ''' </pre></returns>
        ''' <remarks>     Computes the astrometric place of a star, given its mean place, proper motion, parallax, and radial velocity for J2000.0.</remarks>
        <DispId(9)> Function AstroStar(ByVal tjd As Double, _
                                              ByRef earth As BodyDescription, _
                                              ByRef star As CatEntry, _
                                              ByRef ra As Double, _
                                              ByRef dec As Double) As Short

        ''' <summary>
        ''' Computes the astrometric place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for calculation.</param>
        ''' <param name="ss_object">Pointer to structure containing the body designation for the solar system body</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="ra">OUT: Astrometric right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT:  Astrometric declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.</pre></returns>
        ''' <remarks>
        ''' <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        ''' used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        ''' can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        ''' </remarks>
        <DispId(10)> Function AstroPlanet(ByVal tjd As Double, _
                                                ByRef ss_object As BodyDescription, _
                                                ByRef earth As BodyDescription, _
                                                ByRef ra As Double, _
                                                ByRef dec As Double, _
                                                ByRef dis As Double) As Short

        ''' <summary>
        ''' Transform apparent equatorial coordinates to horizon coordinates
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date.</param>
        ''' <param name="deltat">Difference TT (or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="x">Conventionally-defined x coordinate of celestial ephemeris  pole with respect to IERS reference pole, in arcseconds. </param>
        ''' <param name="y">Conventionally-defined y coordinate of celestial ephemeris  pole with respect to IERS reference pole, in arcseconds.</param>
        ''' <param name="location">structure containing observer's location</param>
        ''' <param name="ra"> Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date.</param>
        ''' <param name="dec">Topocentric declination of object of interest, in degrees,  referred to true equator and equinox of date.</param>
        ''' <param name="ref_option">Refraction option</param>
        ''' <param name="zd">OUT: Topocentric zenith distance in degrees, affected by  refraction if 'ref_option' is non-zero.</param>
        ''' <param name="az">OUT: Topocentric azimuth (measured east from north) in degrees.</param>
        ''' <param name="rar">OUT: Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        ''' <param name="decr">OUT: Topocentric declination of object of interest, in degrees, referred to true equator and equinox of date, affected by  refraction if 'ref_option' is non-zero.</param>
        ''' <remarks>This function transforms apparent equatorial coordinates (right 
        ''' ascension and declination) to horizon coordinates (zenith 
        ''' distance and azimuth).  It uses a method that properly accounts 
        ''' for polar motion, which is significant at the sub-arcsecond 
        ''' level.  This function can also adjust coordinates for atmospheric 
        ''' refraction.</remarks>
        <DispId(11)> Sub Equ2Hor(ByVal tjd As Double, _
                                      ByVal deltat As Double, _
                                      ByVal x As Double, _
                                      ByVal y As Double, _
                                      ByRef location As SiteInfo, _
                                      ByVal ra As Double, _
                                      ByVal dec As Double, _
                                      ByVal ref_option As RefractionOption, _
                                      ByRef zd As Double, _
                                      ByRef az As Double, _
                                      ByRef rar As Double, _
                                      ByRef decr As Double)

        ''' <summary>
        '''  To convert Hipparcos data at epoch J1991.25 to epoch J2000.0 and FK5-style units.
        ''' </summary>
        ''' <param name="hipparcos">An entry from the Hipparcos catalog, at epoch J1991.25, with all members having Hipparcos catalog units.  See Note 1 below</param>
        ''' <param name="fk5">The transformed input entry, at epoch J2000.0, with all  members having FK5 catalog units.  See Note 2 below</param>
        ''' <remarks>To be used only for Hipparcos or Tycho stars  with linear space motion.
        ''' <para><pre>
        ''' 1. Hipparcos epoch and units:
        '''    Epoch: J1991.25
        '''    Right ascension (RA): degrees
        '''    Declination (Dec): degrees
        '''    Proper motion in RA * cos (Dec): milliarcseconds per year
        '''    Proper motion in Dec: milliarcseconds per year
        '''    Parallax: milliarcseconds
        '''    Radial velocity: kilometers per second (not in catalog)
        ''' 
        ''' 2. FK5 epoch and units:
        '''    Epoch: J2000.0
        '''    Right ascension: hours
        '''    Declination: degrees
        '''    Proper motion in RA: seconds of time per Julian century
        '''    Proper motion in Dec: arcseconds per Julian century
        '''    Parallax: arcseconds
        '''    Radial velocity: kilometers per second
        '''</pre></para></remarks>
        <DispId(12)> Sub TransformHip(ByRef hipparcos As CatEntry, _
                                            ByRef fk5 As CatEntry)

        ''' <summary>
        ''' To transform a star's catalog quantities for a change of epoch and/or equator and equinox.
        ''' </summary>
        ''' <param name="option">Transformation option<pre>
        '''    = 1 ... change epoch; same equator and equinox
        '''    = 2 ... change equator and equinox; same epoch
        '''    = 3 ... change equator and equinox and epoch
        '''</pre></param>
        ''' <param name="date_incat">TT Julian date, or year, of input catalog data.</param>
        ''' <param name="incat">An entry from the input catalog</param>
        ''' <param name="date_newcat">TT Julian date, or year, of transformed catalog data.</param>
        ''' <param name="newcat_id">Three-character abbreviated name of the transformed catalog.</param>
        ''' <param name="newcat">OUT: The transformed catalog entry</param>
        ''' <remarks><pre>
        ''' 1. 'date_incat' and 'date_newcat' may be specified either as a 
        '''    Julian date (e.g., 2433282.5) or a Julian year and fraction 
        '''    (e.g., 1950.0).  Values less than 10000 are assumed to be years.
        ''' 
        ''' 2. option = 1 updates the star's data to account for the star's space motion between 
        '''               the first and second dates, within a fixed reference frame.
        '''    option = 2 applies a rotation of the reference frame corresponding to precession 
        '''               between the first and second dates, but leaves the star fixed in space.
        '''    option = 3 provides both transformations.
        ''' 
        ''' 3. This subroutine cannot be properly used to bring data from 
        '''    old (pre-FK5) star catalogs into the modern system, because old 
        '''    catalogs were compiled using a set of constants that are 
        '''    incompatible with the IAU (1976) system.
        ''' 
        ''' 4. This function uses TDB Julian dates internally, but no 
        '''    distinction between TDB and TT is necessary.
        '''</pre></remarks>
        <DispId(13)> Sub TransformCat(ByVal [option] As TransformationOption, _
                                            ByVal date_incat As Double, _
                                            ByRef incat As CatEntry, _
                                            ByVal date_newcat As Double, _
                                            ByRef newcat_id As Byte(), _
                                            ByRef newcat As CatEntry)

        ''' <summary>
        '''  Computes the Greenwich apparent sidereal time, at Julian date 'jd_high' + 'jd_low'.
        ''' </summary>
        ''' <param name="jd_high">Julian date, integral part.</param>
        ''' <param name="jd_low">Julian date, fractional part.</param>
        ''' <param name="ee"> Equation of the equinoxes (seconds of time). [Note: this  quantity is computed by function 'earthtilt'.]</param>
        ''' <param name="gst">Greenwich apparent sidereal time, in hours.</param>
        ''' <remarks></remarks>
        <DispId(14)> Sub SiderealTime(ByVal jd_high As Double, _
                                            ByVal jd_low As Double, _
                                            ByVal ee As Double, _
                                            ByRef gst As Double)

        ''' <summary>
        ''' Precesses equatorial rectangular coordinates from one epoch to another.
        ''' </summary>
        ''' <param name="tjd1">TDB Julian date of first epoch.</param>
        ''' <param name="pos">Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of first epoch.</param>
        ''' <param name="tjd2">TDB Julian date of second epoch.</param>
        ''' <param name="pos2">OUT: Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of second epoch.</param>
        ''' <remarks>The coordinates are referred to the mean equator and equinox of the two respective epochs.</remarks>
        <DispId(15)> Sub Precession(ByVal tjd1 As Double, ByVal pos As Double(), ByVal tjd2 As Double, ByRef pos2 As Double())

        ''' <summary>
        '''  Computes quantities related to the orientation of the Earth's rotation axis at Julian date 'tjd'.
        ''' </summary>
        ''' <param name="tjd">TDB Julian date of the desired time</param>
        ''' <param name="mobl">OUT:  Mean obliquity of the ecliptic in degrees at 'tjd'.</param>
        ''' <param name="tobl">OUT: True obliquity of the ecliptic in degrees at 'tjd'.</param>
        ''' <param name="eq">OUT: Equation of the equinoxes in seconds of time at 'tjd'.</param>
        ''' <param name="dpsi">OUT: Nutation in longitude in arcseconds at 'tjd'.</param>
        ''' <param name="deps">OUT: Nutation in obliquity in arcseconds at 'tjd'.</param>
        ''' <remarks></remarks>
        <DispId(16)> Sub EarthTilt(ByVal tjd As Double, _
                                        ByRef mobl As Double, _
                                        ByRef tobl As Double, _
                                        ByRef eq As Double, _
                                        ByRef dpsi As Double, _
                                        ByRef deps As Double)

        ''' <summary>
        ''' This function allows for the specification of celestial pole offsets for high-precision applications.  
        ''' </summary>
        ''' <param name="del_dpsi">Value of offset in delta psi (dpsi) in arcseconds.</param>
        ''' <param name="del_deps">Value of offset in delta epsilon (deps) in arcseconds.</param>
        ''' <remarks>These are added to the nutation parameters delta psi and delta epsilon.
        ''' <para>1. This function sets the values of global variables 'PSI_COR'and 'EPS_COR' declared at the top of file 'novas.c'.  These global variables are used only in NOVAS function 'earthtilt'.</para>
        ''' <para>2. This function, if used, should be called before any other NOVAS functions for a given date.  Values of the pole offsets specified via a call to this function will be used until explicitly changed.</para>
        ''' <para>3. Daily values of the offsets are published, for example, in IERS Bulletins A and B.</para>
        ''' <para>4. This function is the "C" version of Fortran NOVAS routine "celpol".</para>
        ''' </remarks>
        <DispId(17)> Sub CelPole(ByVal del_dpsi As Double, _
                                      ByVal del_deps As Double)

        ''' <summary>
        ''' Retrieves the position and velocity of a body from a fundamental ephemeris.
        ''' </summary>
        ''' <param name="tjd">TDB Julian date.</param>
        ''' <param name="cel_obj">Structure containing the designation of the body of interest</param>
        ''' <param name="origin">Origin point (solar system barycentre or centre of mass of the Sun</param>
        ''' <param name="pos">OUT: Position vector of 'body' at tjd; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        ''' <param name="vel">OUT: Velocity vector of 'body' at tjd; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        ''' <returns><pre>
        ''' 0    ... Everything OK.
        ''' 1    ... Invalid value of 'origin'.
        ''' 2    ... Invalid value of 'type' in 'cel_obj'.
        ''' 3    ... Unable to allocate memory.
        ''' 10+n ... where n is the error code from 'solarsystem'.
        ''' 20+n ... where n is the error code from 'readeph'.</pre></returns>
        ''' <remarks></remarks>
        <DispId(18)> Function Ephemeris(ByVal tjd As Double, _
                                        ByRef cel_obj As BodyDescription, _
                                        ByVal origin As Origin, _
                                        ByRef pos As Double(), _
                                        ByRef vel As Double()) As Short

        ''' <summary>
        ''' Provides the position and velocity of the Earth
        ''' </summary>
        ''' <param name="tjd">TDB Julian date.</param>
        ''' <param name="body">Body identification number.
        ''' <pre>
        ''' Set 'body' = 0 or 'body' = 1 or 'body' = 10 for the Sun.
        ''' Set 'body' = 2 or 'body' = 3 for the Earth.
        '''</pre></param>
        ''' <param name="origin">Required origin: solar system barycenter or center of mass of the Sun</param>
        ''' <param name="pos">OUT: Position vector of 'body' at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        ''' <param name="vel">OUT: Velocity vector of 'body' at 'tjd'; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        ''' <returns><pre>
        ''' 0...Everything OK.
        ''' 1...Input Julian date ('tjd') out of range.
        ''' 2...Invalid value of 'body'.
        '''</pre></returns>
        ''' <remarks> Provides the position and velocity of the Earth at epoch 'tjd' by evaluating a closed-form theory without reference to an  external file.  This function can also provide the position and velocity of the Sun.</remarks>
        <DispId(19)> Function SolarSystem(ByVal tjd As Double, _
                                               ByVal body As Body, _
                                               ByVal origin As Origin, _
                                               ByRef pos As Double(), _
                                               ByRef vel As Double()) As Short

        ''' <summary>
        ''' Converts an vector in equatorial rectangular coordinates to equatorial spherical coordinates.
        ''' </summary>
        ''' <param name="pos">Position vector, equatorial rectangular coordinates.</param>
        ''' <param name="ra">OUT: Right ascension in hours.</param>
        ''' <param name="dec">OUT: Declination in degrees.</param>
        ''' <returns><pre>
        ''' 0...Everything OK.
        ''' 1...All vector components are zero; 'ra' and 'dec' are indeterminate.
        ''' 2...Both vec[0] and vec[1] are zero, but vec[2] is nonzero; 'ra' is indeterminate.</pre>
        ''' </returns>
        ''' <remarks></remarks>
        <DispId(20)> Function Vector2RADec(ByVal pos As Double(), _
                                                ByRef ra As Double, _
                                                ByRef dec As Double) As Short

        ''' <summary>
        ''' Converts angular quanities for stars to vectors.
        ''' </summary>
        ''' <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units </param>
        ''' <param name="pos">Position vector, equatorial rectangular coordinates, components in AU.</param>
        ''' <param name="vel">Velocity vector, equatorial rectangular coordinates, components in AU/Day.</param>
        ''' <remarks></remarks>
        <DispId(21)> Sub StarVectors(ByVal star As CatEntry, ByRef pos As Double(), ByRef vel As Double())

        ''' <summary>
        ''' Converts equatorial spherical coordinates to a vector (equatorial rectangular coordinates).
        ''' </summary>
        ''' <param name="ra">Right ascension (hours).</param>
        ''' <param name="dec">Declination (degrees).</param>
        ''' <param name="dist">Distance</param>
        ''' <param name="pos">Position vector, equatorial rectangular coordinates (AU).</param>
        ''' <remarks></remarks>
        <DispId(22)> Sub RADec2Vector(ByVal ra As Double, _
                                              ByVal dec As Double, _
                                              ByVal dist As Double, _
                                              ByRef pos As Double())


        ''' <summary>
        ''' Obtains the barycentric and heliocentric positions and velocities of the Earth from the solar system ephemeris.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date.</param>
        ''' <param name="earth">Structure containing the body designation for the Earth.</param>
        ''' <param name="tdb">OUT: TDB Julian date corresponding to 'tjd'.</param>
        ''' <param name="bary_earthp">OUT: Barycentric position vector of Earth at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        ''' <param name="bary_earthv">OUT: Barycentric velocity vector of Earth at 'tjd'; equatorial rectangular system referred to the mean equator and equinox  of J2000.0, in AU/Day.</param>
        ''' <param name="helio_earthp">OUT: Heliocentric position vector of Earth at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        ''' <param name="helio_earthv">OUT: Heliocentric velocity vector of Earth at 'tjd'; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'.</pre>
        ''' </returns>
        ''' <remarks></remarks>
        <DispId(23)> Function GetEarth(ByVal tjd As Double, _
                                            ByRef earth As BodyDescription, _
                                            ByRef tdb As Double, _
                                            ByRef bary_earthp As Double(), _
                                            ByRef bary_earthv As Double(), _
                                            ByRef helio_earthp As Double(), _
                                            ByRef helio_earthv As Double()) As Short

        '
        'START OF NEW
        '
        ''' <summary>
        ''' Computes the mean place of a star for J2000.0
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date of apparent place.</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="ra">Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec"> Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="mra">OUT: Mean right ascension J2000.0 in hours.</param>
        ''' <param name="mdec">OUT: Mean declination J2000.0 in degrees.</param>
        ''' <returns><pre>
        '''   0...Everything OK.
        '''   1...Iterative process did not converge after 20 iterations.
        ''' >10...Error from function 'app_star'.</pre></returns>
        ''' <remarks>Computes the mean place of a star for J2000.0, given its apparent 
        ''' place at date 'tjd'.  Proper motion, parallax and radial velocity 
        ''' are assumed to be zero.
        '''</remarks>
        <DispId(24)> Function MeanStar(ByVal tjd As Double, _
                                                ByRef earth As BodyDescription, _
                                                ByVal ra As Double, _
                                                ByVal dec As Double, _
                                                ByRef mra As Double, _
                                                ByRef mdec As Double) As Short

        ''' <summary>
        ''' Transforms a vector from an Earth-fixed geographic system to a space-fixed system
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date</param>
        ''' <param name="gast">Greenwich apparent sidereal time, in hours.</param>
        ''' <param name="x"> Conventionally-defined X coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="vece"> Vector in geocentric rectangular Earth-fixed system, referred to geographic equator and Greenwich meridian.</param>
        ''' <param name="vecs">OUT: Vector in geocentric rectangular space-fixed system, referred to mean equator and equinox of J2000.0.</param>
        ''' <remarks>Transforms a vector from an Earth-fixed geographic system to a space-fixed system based on mean equator and equinox of J2000.0; applies rotations for wobble, spin, nutation, and precession.</remarks>
        <DispId(25)> Sub Pnsw(ByVal tjd As Double, _
                                   ByVal gast As Double, _
                                   ByVal x As Double, _
                                   ByVal y As Double, _
                                   ByVal vece As Double(), _
                                   ByRef vecs As Double())

        ''' <summary>
        ''' Transforms geocentric rectangular coordinates from rotating system to non-rotating system
        ''' </summary>
        ''' <param name="st">Local apparent sidereal time at reference meridian, in hours.</param>
        ''' <param name="pos1">Vector in geocentric rectangular rotating system, referred to rotational equator and orthogonal reference meridian.</param>
        ''' <param name="pos2">OUT: Vector in geocentric rectangular non-rotating system, referred to true equator and equinox of date.</param>
        ''' <remarks>Transforms geocentric rectangular coordinates from rotating system based on rotational equator and orthogonal reference meridian to  non-rotating system based on true equator and equinox of date.</remarks>
        <DispId(26)> Sub Spin(ByVal st As Double, _
                                   ByVal pos1 As Double(), _
                                   ByRef pos2 As Double())


        ''' <summary>
        ''' Corrects Earth-fixed geocentric rectangular coordinates for polar motion.
        ''' </summary>
        ''' <param name="x"> Conventionally-defined X coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="pos1">Vector in geocentric rectangular Earth-fixed system, referred to geographic equator and Greenwich meridian.</param>
        ''' <param name="pos2">OUT: Vector in geocentric rectangular rotating system, referred to rotational equator and orthogonal Greenwich meridian</param>
        ''' <remarks>Corrects Earth-fixed geocentric rectangular coordinates for polar motion.  Transforms a vector from Earth-fixed geographic system to rotating system based on rotational equator and orthogonal Greenwich meridian through axis of rotation.</remarks>
        <DispId(27)> Sub Wobble(ByVal x As Double, _
                                        ByVal y As Double, _
                                        ByVal pos1 As Double(), _
                                        ByRef pos2 As Double())

        ''' <summary>
        ''' Computes the position and velocity vectors of a terrestrial observer with respect to the center of the Earth.
        ''' </summary>
        ''' <param name="locale">Longitude, latitude and height of the observer (in a SiteInfoStruct)</param>
        ''' <param name="st">Local apparent sidereal time at reference meridian in hours.</param>
        ''' <param name="pos"> Position vector of observer with respect to center of Earth, equatorial rectangular coordinates, referred to true equator and equinox of date, components in AU.</param>
        ''' <param name="vel"> Velocity vector of observer with respect to center of Earth, equatorial rectangular coordinates, referred to true equator and equinox of date, components in AU/Day.</param>
        ''' <remarks></remarks>
        <DispId(28)> Sub Terra(ByRef locale As SiteInfo, _
                                       ByVal st As Double, _
                                       ByRef pos As Double(), _
                                       ByRef vel As Double())


        ''' <summary>
        ''' Applies proper motion, including foreshortening effects, to a star's position.
        ''' </summary>
        ''' <param name="tjd1">TDB Julian date of first epoch.</param>
        ''' <param name="pos">Position vector at first epoch.</param>
        ''' <param name="vel">Velocity vector at first epoch.</param>
        ''' <param name="tjd2">TDB Julian date of second epoch.</param>
        ''' <param name="pos2">OUT: Position vector at second epoch.</param>
        ''' <remarks></remarks>
        <DispId(29)> Sub ProperMotion(ByVal tjd1 As Double, _
                                            ByVal pos As Double(), _
                                            ByVal vel As Double(), _
                                            ByVal tjd2 As Double, _
                                            ByRef pos2 As Double())

        ''' <summary>
        ''' Moves the origin of coordinates from the barycenter of the solar system to the center of mass of the Earth
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at solar system barycenter, components in AU.</param>
        ''' <param name="earthvector">Position vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU.</param>
        ''' <param name="pos2">OUT: Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="lighttime">OUT: Light time from body to Earth in days.</param>
        ''' <remarks>This corrects for parallax.</remarks>
        <DispId(30)> Sub BaryToGeo(ByVal pos As Double(), _
                                          ByVal earthvector As Double(), _
                                          ByRef pos2 As Double(), _
                                          ByRef lighttime As Double)

        ''' <summary>
        ''' Corrects position vector for the deflection of light in the gravitational field of the Sun. 
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="earthvector">Position vector of center of mass of the Earth, referred to origin at center of mass of the Sun, components in AU.</param>
        ''' <param name="pos2">Position vector, referred to origin at center of mass of the Earth, corrected for gravitational deflection, components in AU.</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks>This function is valid for bodies within the solar system as well as for stars.</remarks>
        <DispId(31)> Function SunField(ByVal pos As Double(), _
                                             ByVal earthvector As Double(), _
                                             ByRef pos2 As Double()) As Short

        ''' <summary>
        ''' Corrects position vector for aberration of light.
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="vel">Velocity vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU/day.</param>
        ''' <param name="lighttime">Light time from body to Earth in days.</param>
        ''' <param name="pos2">OUT: Position vector, referred to origin at center of mass of the Earth, corrected for aberration, components in AU</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks>Algorithm includes relativistic terms.</remarks>
        <DispId(32)> Function Aberration(ByVal pos As Double(), _
                                              ByVal vel As Double(), _
                                              ByVal lighttime As Double, _
                                              ByRef pos2 As Double()) As Short

        ''' <summary>
        ''' Nutates equatorial rectangular coordinates from mean equator and equinox of epoch to true equator and equinox of epoch.
        ''' </summary>
        ''' <param name="tjd">TDB julian date of epoch.</param>
        ''' <param name="fn">Flag determining 'direction' of transformation;<pre>
        '''    fn  = 0 transformation applied, mean to true.
        '''    fn != 0 inverse transformation applied, true to mean.</pre></param>
        ''' <param name="pos">Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of epoch.</param>
        ''' <param name="pos2">OUT: Position vector, geocentric equatorial rectangular coordinates, referred to true equator and equinox of epoch.</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks>Inverse transformation may be applied by setting flag 'fn'.</remarks>
        <DispId(33)> Function Nutate(ByVal tjd As Double, _
                                          ByVal fn As NutationDirection, _
                                          ByVal pos As Double(), _
                                          ByRef pos2 As Double()) As Short

        ''' <summary>
        ''' Provides fast evaluation of the nutation components according to the 1980 IAU Theory of Nutation.
        ''' </summary>
        ''' <param name="tdbtime">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="longnutation">OUT: Nutation in longitude in arcseconds.</param>
        ''' <param name="obliqnutation">OUT: Nutation in obliquity in arcseconds.</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks></remarks>
        <DispId(34)> Function NutationAngles(ByVal tdbtime As Double, _
                                                   ByRef longnutation As Double, _
                                                   ByRef obliqnutation As Double) As Short

        ''' <summary>
        ''' To compute the fundamental arguments.
        ''' </summary>
        ''' <param name="t">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="a">OUT: FundamentalArgsStruct containing: <pre>
        '''   a[0] = l (mean anomaly of the Moon)
        '''   a[1] = l' (mean anomaly of the Sun)
        '''   a[2] = F (L - omega; L = mean longitude of the Moon)
        '''   a[3] = D (mean elongation of the Moon from the Sun)
        '''   a[4] = omega (mean longitude of the Moon's ascending node)</pre></param>
        ''' <remarks></remarks>
        <DispId(35)> Sub FundArgs(ByVal t As Double, _
                                        ByRef a As Double())

        ''' <summary>
        ''' Converts TDB to TT or TDT
        ''' </summary>
        ''' <param name="tdb">TDB Julian date.</param>
        ''' <param name="tdtjd">OUT: TT (or TDT) Julian date.</param>
        ''' <param name="secdiff">OUT: Difference tdbjd-tdtjd, in seconds.</param>
        ''' <remarks>Computes the terrestrial time (TT) or terrestrial dynamical time (TDT) Julian date corresponding to a barycentric dynamical time (TDB) Julian date.</remarks>
        <DispId(36)> Sub Tdb2Tdt(ByVal tdb As Double, _
                                      ByRef tdtjd As Double, _
                                      ByRef secdiff As Double)

        ''' <summary>
        ''' Sets up a structure of type 'body' - defining a celestial object- based on the input parameters.
        ''' </summary>
        ''' <param name="type">Type of body</param>
        ''' <param name="number">Body number</param>
        ''' <param name="name">Name of the body.</param>
        ''' <param name="cel_obj">OUT: Structure containg the body definition </param>
        ''' <returns><pre>
        ''' = 0 ... everything OK
        ''' = 1 ... invalid value of 'type'
        ''' = 2 ... 'number' out of range
        '''</pre></returns>
        ''' <remarks></remarks>
        <DispId(37)> Function SetBody(ByVal type As BodyType, _
                                            ByVal number As Body, _
                                            ByVal name As String, _
                                            ByRef cel_obj As BodyDescription) As Short

        'Public Shared Function readeph(ByVal mp As Integer, _
        '                               ByVal name As System.IntPtr, _
        '                               ByVal jd As Double, _
        '                               ByRef err As Integer) As posvector
        'End Function

        ''' <summary>
        ''' To create a structure of type 'cat_entry' containing catalog data for a star or "star-like" object.
        ''' </summary>
        ''' <param name="catalog">Three-character catalog identifier (e.g. HIP = Hipparcos, FK5 = FK5).  This identifier also specifies the reference system and units of the data; i.e. they are the same as the specified catalog.</param>
        ''' <param name="star_name">Object name (50 characters maximum).</param>
        ''' <param name="star_num">Object number in the catalog.</param>
        ''' <param name="ra">Right ascension of the object.</param>
        ''' <param name="dec">Declination of the object.</param>
        ''' <param name="pm_ra">Proper motion in right ascension.</param>
        ''' <param name="pm_dec">Proper motion in declination.</param>
        ''' <param name="parallax">Parallax.</param>
        ''' <param name="rad_vel">Radial velocity.</param>
        ''' <param name="star">OUT: Structure containing the input data</param>
        ''' <remarks></remarks>
        <DispId(38)> Sub MakeCatEntry(ByVal catalog As String, _
                                             ByVal star_name As String, _
                                             ByVal star_num As Integer, _
                                             ByVal ra As Double, _
                                             ByVal dec As Double, _
                                             ByVal pm_ra As Double, _
                                             ByVal pm_dec As Double, _
                                             ByVal parallax As Double, _
                                             ByVal rad_vel As Double, _
                                             ByRef star As CatEntry)

        ''' <summary>
        ''' Computes atmospheric refraction in zenith distance.
        ''' </summary>
        ''' <param name="location">structure containing observer's location</param>
        ''' <param name="ref_option">refraction option</param>
        ''' <param name="zd_obs">bserved zenith distance, in degrees.</param>
        ''' <returns>Atmospheric refraction, in degrees.</returns>
        ''' <remarks>This version computes approximate refraction for optical wavelengths.</remarks>
        <DispId(39)> Function Refract(ByRef location As SiteInfo, _
                                           ByVal ref_option As Short, _
                                           ByVal zd_obs As Double) As Double

        ''' <summary>
        ''' This function will compute the Julian date for a given calendar date (year, month, day, hour).
        ''' </summary>
        ''' <param name="year">Year number</param>
        ''' <param name="month">Month number.</param>
        ''' <param name="day">Day number</param>
        ''' <param name="hour">Time in hours</param>
        ''' <returns>OUT: Julian date.</returns>
        ''' <remarks></remarks>
        <DispId(40)> Function JulianDate(ByVal year As Short, _
                                               ByVal month As Short, _
                                               ByVal day As Short, _
                                               ByVal hour As Double) As Double

        ''' <summary>
        ''' Compute a date on the Gregorian calendar given the Julian date.
        ''' </summary>
        ''' <param name="tjd">Julian date.</param>
        ''' <param name="year">OUT: Year number</param>
        ''' <param name="month">OUT: Month number.</param>
        ''' <param name="day">OUT: Day number</param>
        ''' <param name="hour">OUT: Time in hours</param>
        ''' <remarks></remarks>
        <DispId(41)> Sub CalDate(ByVal tjd As Double, _
                                       ByRef year As Short, _
                                       ByRef month As Short, _
                                       ByRef day As Short, _
                                       ByRef hour As Double)

        ''' <summary>
        ''' Compute equatorial spherical coordinates of Sun referred to the mean equator and equinox of date.
        ''' </summary>
        ''' <param name="jd">Julian date on TDT or ET time scale.</param>
        ''' <param name="ra">OUT: Right ascension referred to mean equator and equinox of date (hours).</param>
        ''' <param name="dec">OUT: Declination referred to mean equator and equinox of date  (degrees).</param>
        ''' <param name="dis">OUT: Geocentric distance (AU).</param>
        ''' <remarks></remarks>
        <DispId(42)> Sub SunEph(ByVal jd As Double, _
                                           ByRef ra As Double, _
                                           ByRef dec As Double, _
                                           ByRef dis As Double)

        ''' <summary>
        ''' Return the value of DeltaT for the given Julian date
        ''' </summary>
        ''' <param name="Tjd">Julian date for which the delta T value is required</param>
        ''' <returns>Double value of DeltaT (seconds)</returns>
        ''' <remarks>Valid between the years 1650 and 2050</remarks>
        <DispId(43)> Function DeltaT(ByVal Tjd As Double) As Double

    End Interface
End Namespace
#End Region

#Region "NOVAS3 Interface"
Namespace NOVAS

    ''' <summary>
    ''' Interface to the NOVAS3 component
    ''' </summary>
    ''' <remarks>Implemented by the NOVAS3 component
    ''' <para><b>Note: </b>This interface is now deprecated, please use INOVAS31 instead.</para>
    '''</remarks>
    <Guid("5EF15982-D79E-42f7-B20B-E83232E2B86B"), ComVisible(True)> _
    Public Interface INOVAS3

        ' PlanetEphemeris, ReadEph, SolarSystem and State relate to reading ephemeris values

        ''' <summary>
        ''' Get position and velocity of target with respect to the centre object. 
        ''' </summary>
        ''' <param name="Tjd"> Two-element array containing the Julian date, which may be split any way (although the first 
        ''' element is usually the "integer" part, and the second element is the "fractional" part).  Julian date is in the 
        ''' TDB or "T_eph" time scale.</param>
        ''' <param name="Target">Target object</param>
        ''' <param name="Center">Centre object</param>
        ''' <param name="Position">Position vector array of target relative to center, measured in AU.</param>
        ''' <param name="Velocity">Velocity vector array of target relative to center, measured in AU/day.</param>
        ''' <returns><pre>
        ''' 0   ...everything OK.
        ''' 1,2 ...error returned from State.</pre>
        ''' </returns>
        ''' <remarks>This function accesses the JPL planetary ephemeris to give the position and velocity of the target 
        ''' object with respect to the center object.</remarks>
        <DispId(1)> Function PlanetEphemeris(ByRef Tjd() As Double, _
                                                     ByVal Target As Target, _
                                                     ByVal Center As Target, _
                                                     ByRef Position() As Double, _
                                                     ByRef Velocity() As Double) As Short

        ''' <summary>
        ''' Read object ephemeris
        ''' </summary>
        ''' <param name="Mp">The number of the asteroid for which the position in desired.</param>
        ''' <param name="Name">The name of the asteroid.</param>
        ''' <param name="Jd"> The Julian date on which to find the position and velocity.</param>
        ''' <param name="Err">Error code; always set equal to 9 (see note below).</param>
        ''' <returns> 6-element array of double cotaining position and velocity vector values, with all elements set to zero.</returns>
        ''' <remarks> This is a dummy version of function 'ReadEph'.  It serves as a stub for the "real" 'ReadEph' 
        ''' (part of the USNO/AE98 minor planet ephemerides) when NOVAS-C is used without the minor planet ephemerides.
        ''' <para>
        '''  This dummy function is not intended to be called.  It merely serves as a stub for the "real" 'ReadEph' 
        ''' when NOVAS-C is used without the minor planet ephemerides.  If this function is called, an error of 9 will be returned.
        ''' </para>
        ''' </remarks>
        <DispId(2)> Function ReadEph(ByVal Mp As Integer, _
                                             ByVal Name As String, _
                                             ByVal Jd As Double, _
                                             ByRef Err As Integer) As Double()

        ''' <summary>
        ''' Interface between the JPL direct-access solar system ephemerides and NOVAS-C.
        ''' </summary>
        ''' <param name="Tjd">Julian date of the desired time, on the TDB time scale.</param>
        ''' <param name="Body">Body identification number for the solar system object of interest; 
        ''' Mercury = 1, ..., Pluto= 9, Sun= 10, Moon = 11.</param>
        ''' <param name="Origin">Origin code; solar system barycenter= 0, center of mass of the Sun = 1, center of Earth = 2.</param>
        ''' <param name="Pos">Position vector of 'body' at tjd; equatorial rectangular coordinates in AU referred to the ICRS.</param>
        ''' <param name="Vel">Velocity vector of 'body' at tjd; equatorial rectangular system referred to the ICRS.</param>
        ''' <returns>Always returns 0</returns>
        ''' <remarks></remarks>
        <DispId(3)> Function SolarSystem(ByVal Tjd As Double, _
                                                 ByVal Body As Body, _
                                                 ByVal Origin As Origin, _
                                                 ByRef Pos() As Double, _
                                                 ByRef Vel() As Double) As Short

        ''' <summary>
        '''  Read and interpolate the JPL planetary ephemeris file.
        ''' </summary>
        ''' <param name="Jed">2-element Julian date (TDB) at which interpolation is wanted. Any combination of jed[0]+jed[1] which falls within the time span on the file is a permissible epoch.  See Note 1 below.</param>
        ''' <param name="Target">The requested body to get data for from the ephemeris file.</param>
        ''' <param name="TargetPos">The barycentric position vector array of the requested object, in AU. (If target object is the Moon, then the vector is geocentric.)</param>
        ''' <param name="TargetVel">The barycentric velocity vector array of the requested object, in AU/Day.</param>
        ''' <returns>
        ''' <pre>
        ''' 0 ...everything OK
        ''' 1 ...error reading ephemeris file
        ''' 2 ...epoch out of range.
        ''' </pre></returns>
        ''' <remarks>
        ''' The target number designation of the astronomical bodies is:
        ''' <pre>
        '''         = 0: Mercury,               1: Venus, 
        '''         = 2: Earth-Moon barycenter, 3: Mars, 
        '''         = 4: Jupiter,               5: Saturn, 
        '''         = 6: Uranus,                7: Neptune, 
        '''         = 8: Pluto,                 9: geocentric Moon, 
        '''         =10: Sun.
        ''' </pre>
        ''' <para>
        '''  NOTE 1. For ease in programming, the user may put the entire epoch in jed[0] and set jed[1] = 0. 
        ''' For maximum interpolation accuracy,  set jed[0] = the most recent midnight at or before interpolation epoch, 
        ''' and set jed[1] = fractional part of a day elapsed between jed[0] and epoch. As an alternative, it may prove 
        ''' convenient to set jed[0] = some fixed epoch, such as start of the integration and jed[1] = elapsed interval 
        ''' between then and epoch.
        ''' </para>
        ''' </remarks>
        <DispId(4)> Function State(ByRef Jed() As Double, _
                                           ByVal Target As Target, _
                                           ByRef TargetPos() As Double, _
                                           ByRef TargetVel() As Double) As Short

        ' The following methods come from NOVAS3

        ''' <summary>
        '''  Corrects position vector for aberration of light.  Algorithm includes relativistic terms.
        ''' </summary>
        ''' <param name="Pos"> Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="Vel"> Velocity vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU/day.</param>
        ''' <param name="LightTime"> Light time from object to Earth in days.</param>
        ''' <param name="Pos2"> Position vector, referred to origin at center of mass of the Earth, corrected for aberration, components in AU</param>
        ''' <remarks>If 'lighttime' = 0 on input, this function will compute it.</remarks>
        <DispId(5)> Sub Aberration(ByVal Pos() As Double, _
                                            ByVal Vel() As Double, _
                                            ByVal LightTime As Double, _
                                            ByRef Pos2() As Double)

        ''' <summary>
        ''' Compute the apparent place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="JdTt"> TT Julian date for apparent place.</param>
        ''' <param name="SsBody"> Pointer to structure containing the body designation for the solar system body </param>
        ''' <param name="Accuracy"> Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        ''' <param name="Dec"> Apparent declination in degrees, referred to true equator and equinox of date.</param>
        ''' <param name="Dis"> True distance from Earth to planet at 'JdTt' in AU.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Type' in structure 'SsBody'
        ''' > 10 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(6)> Function AppPlanet(ByVal JdTt As Double, _
                                               ByVal SsBody As Object3, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double, _
                                               ByRef Dis As Double) As Short

        ''' <summary>
        '''  Computes the apparent place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for apparent place.</param>
        ''' <param name="Star">Catalog entry structure containing catalog data forthe object in the ICRS </param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date 'JdTt'.</param>
        ''' <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date 'JdTt'.</param>
        ''' <returns>
        ''' <pre>
        '''    0 ... Everything OK
        ''' > 10 ... Error code from function 'MakeObject'
        ''' > 20 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(7)> Function AppStar(ByVal JdTt As Double, _
                                             ByVal Star As CatEntry3, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Ra As Double, _
                                             ByRef Dec As Double) As Short

        ''' <summary>
        ''' Compute the astrometric place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for astrometric place.</param>
        ''' <param name="SsBody">structure containing the body designation for the solar system body </param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Astrometric right ascension in hours (referred to the ICRS, without light deflection or aberration).</param>
        ''' <param name="Dec">Astrometric declination in degrees (referred to the ICRS, without light deflection or aberration).</param>
        ''' <param name="Dis">True distance from Earth to planet in AU.</param>
        ''' <returns>
        ''' <pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Type' in structure 'SsBody'
        ''' > 10 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(8)> Function AstroPlanet(ByVal JdTt As Double, _
                                                 ByVal SsBody As Object3, _
                                                 ByVal Accuracy As Accuracy, _
                                                 ByRef Ra As Double, _
                                                 ByRef Dec As Double, _
                                                 ByRef Dis As Double) As Short

        ''' <summary>
        '''  Computes the astrometric place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for astrometric place.</param>
        ''' <param name="Star">Catalog entry structure containing catalog data for the object in the ICRS</param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Astrometric right ascension in hours (referred to the ICRS, without light deflection or aberration).</param>
        ''' <param name="Dec">Astrometric declination in degrees (referred to the ICRS, without light deflection or aberration).</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        ''' > 10 ... Error code from function 'MakeObject'
        ''' > 20 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(9)> Function AstroStar(ByVal JdTt As Double, _
                                               ByVal Star As CatEntry3, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double) As Short

        ''' <summary>
        ''' Move the origin of coordinates from the barycenter of the solar system to the observer (or the geocenter); i.e., this function accounts for parallax (annual+geocentric or justannual).
        ''' </summary>
        ''' <param name="Pos">Position vector, referred to origin at solar system barycenter, components in AU.</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar system barycenter, components in AU.</param>
        ''' <param name="Pos2"> Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="Lighttime">Light time from object to Earth in days.</param>
        ''' <remarks></remarks>
        <DispId(10)> Sub Bary2Obs(ByVal Pos() As Double, _
                                          ByVal PosObs() As Double, _
                                          ByRef Pos2() As Double, _
                                          ByRef Lighttime As Double)

        ''' <summary>
        ''' This function will compute a date on the Gregorian calendar given the Julian date.
        ''' </summary>
        ''' <param name="Tjd">Julian date.</param>
        ''' <param name="Year">Year</param>
        ''' <param name="Month">Month number</param>
        ''' <param name="Day">day number</param>
        ''' <param name="Hour">Fractional hour of the day</param>
        ''' <remarks></remarks>
        <DispId(11)> Sub CalDate(ByVal Tjd As Double, _
                                         ByRef Year As Short, _
                                         ByRef Month As Short, _
                                         ByRef Day As Short, _
                                         ByRef Hour As Double)

        ''' <summary>
        '''  This function allows for the specification of celestial pole offsets for high-precision applications.  Each set of offsets is a correction to the modeled position of the pole for a specific date, derived from observations and published by the IERS.
        ''' </summary>
        ''' <param name="Tjd">TDB or TT Julian date for pole offsets.</param>
        ''' <param name="Type"> Type of pole offset. 1 for corrections to angular coordinates of modeled pole referred to mean ecliptic of date, that is, delta-delta-psi and delta-delta-epsilon.  2 for corrections to components of modeled pole unit vector referred to GCRS axes, that is, dx and dy.</param>
        ''' <param name="Dpole1">Value of celestial pole offset in first coordinate, (delta-delta-psi or dx) in milliarcseconds.</param>
        ''' <param name="Dpole2">Value of celestial pole offset in second coordinate, (delta-delta-epsilon or dy) in milliarcseconds.</param>
        ''' <returns><pre>
        ''' 0 ... Everything OK
        ''' 1 ... Invalid value of 'Type'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(12)> Function CelPole(ByVal Tjd As Double, _
                                              ByVal Type As PoleOffsetCorrection, _
                                              ByVal Dpole1 As Double, _
                                              ByVal Dpole2 As Double) As Short

        ''' <summary>
        ''' Calaculate an array of CIO RA values around a given date
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date.</param>
        ''' <param name="NPts"> Number of Julian dates and right ascension values requested (not less than 2 or more than 20).</param>
        ''' <param name="Cio"> An arraylist of RaOfCIO structures containing a time series of the right ascension of the 
        ''' Celestial Intermediate Origin (CIO) with respect to the GCRS.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... error opening the 'cio_ra.bin' file
        ''' 2 ... 'JdTdb' not in the range of the CIO file; 
        ''' 3 ... 'NPts' out of range
        ''' 4 ... unable to allocate memory for the internal 't' array; 
        ''' 5 ... unable to allocate memory for the internal 'ra' array; 
        ''' 6 ... 'JdTdb' is too close to either end of the CIO file; unable to put 'NPts' data points into the output object.
        ''' </pre></returns>
        ''' <remarks>
        ''' <para>
        ''' Given an input TDB Julian date and the number of data points desired, this function returns a set of 
        ''' Julian dates and corresponding values of the GCRS right ascension of the celestial intermediate origin (CIO).  
        ''' The range of dates is centered (at least approximately) on the requested date.  The function obtains 
        ''' the data from an external data file.</para>
        ''' <example>How to create and retrieve values from the arraylist
        ''' <code>
        ''' Dim CioList As New ArrayList, Nov3 As New ASCOM.Astrometry.NOVAS3
        ''' 
        ''' rc = Nov3.CioArray(2455251.5, 20, CioList) ' Get 20 values around date 00:00:00 February 24th 2010
        ''' MsgBox("Nov3 RC= " <![CDATA[&]]>  rc)
        ''' 
        ''' For Each CioA As ASCOM.Astrometry.RAOfCio In CioList
        '''     MsgBox("CIO Array " <![CDATA[&]]> CioA.JdTdb <![CDATA[&]]> " " <![CDATA[&]]> CioA.RACio)
        ''' Next
        ''' </code>
        ''' </example>
        ''' </remarks>
        <DispId(13)> Function CioArray(ByVal JdTdb As Double, _
                                                ByVal NPts As Integer, _
                                                ByRef Cio As ArrayList) As Short

        ''' <summary>
        ''' Compute the orthonormal basis vectors of the celestial intermediate system.
        ''' </summary>
        ''' <param name="JdTdbEquionx">TDB Julian date of epoch.</param>
        ''' <param name="RaCioEquionx">Right ascension of the CIO at epoch (hours).</param>
        ''' <param name="RefSys">Reference system in which right ascension is given. 1 ... GCRS; 2 ... True equator and equinox of date.</param>
        ''' <param name="Accuracy">Accuracy</param>
        ''' <param name="x">Unit vector toward the CIO, equatorial rectangular coordinates, referred to the GCRS.</param>
        ''' <param name="y">Unit vector toward the y-direction, equatorial rectangular coordinates, referred to the GCRS.</param>
        ''' <param name="z">Unit vector toward north celestial pole (CIP), equatorial rectangular coordinates, referred to the GCRS.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of input variable 'RefSys'.
        ''' </pre></returns>
        ''' <remarks>
        ''' To compute the orthonormal basis vectors, with respect to the GCRS (geocentric ICRS), of the celestial 
        ''' intermediate system defined by the celestial intermediate pole (CIP) (in the z direction) and 
        ''' the celestial intermediate origin (CIO) (in the x direction).  A TDB Julian date and the 
        ''' right ascension of the CIO at that date is required as input.  The right ascension of the CIO 
        ''' can be with respect to either the GCRS origin or the true equinox of date -- different algorithms 
        ''' are used in the two cases.</remarks>
        <DispId(14)> Function CioBasis(ByVal JdTdbEquionx As Double, _
                                               ByVal RaCioEquionx As Double, _
                                               ByVal RefSys As ReferenceSystem, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef x As Double, _
                                               ByRef y As Double, _
                                               ByRef z As Double) As Short

        ''' <summary>
        ''' Returns the location of the celestial intermediate origin (CIO) for a given Julian date, as a right ascension 
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="RaCio">Right ascension of the CIO, in hours.</param>
        ''' <param name="RefSys">Reference system in which right ascension is given</param>
        ''' <returns><pre>
        '''    0 ... everything OK
        '''    1 ... unable to allocate memory for the 'cio' array
        ''' > 10 ... 10 + the error code from function 'CioArray'.
        ''' </pre></returns>
        ''' <remarks>  This function returns the location of the celestial intermediate origin (CIO) for a given Julian date, as a right ascension with respect to either the GCRS (geocentric ICRS) origin or the true equinox of date.  The CIO is always located on the true equator (= intermediate equator) of date.</remarks>
        <DispId(15)> Function CioLocation(ByVal JdTdb As Double, _
                                                  ByVal Accuracy As Accuracy, _
                                                  ByRef RaCio As Double, _
                                                  ByRef RefSys As ReferenceSystem) As Short

        ''' <summary>
        ''' Computes the true right ascension of the celestial intermediate origin (CIO) at a given TT Julian date.  This is -(equation of the origins).
        ''' </summary>
        ''' <param name="JdTt">TT Julian date</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="RaCio"> Right ascension of the CIO, with respect to the true equinox of date, in hours (+ or -).</param>
        ''' <returns>
        ''' <pre>
        '''   0  ... everything OK
        '''   1  ... invalid value of 'Accuracy'
        ''' > 10 ... 10 + the error code from function 'CioLocation'
        ''' > 20 ... 20 + the error code from function 'CioBasis'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(16)> Function CioRa(ByVal JdTt As Double, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef RaCio As Double) As Short

        ''' <summary>
        ''' Returns the difference in light-time, for a star, between the barycenter of the solar system and the observer (or the geocenter).
        ''' </summary>
        ''' <param name="Pos1">Position vector of star, with respect to origin at solar system barycenter.</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar system barycenter, components in AU.</param>
        ''' <returns>Difference in light time, in the sense star to barycenter minus star to earth, in days.</returns>
        ''' <remarks>
        ''' Alternatively, this function returns the light-time from the observer (or the geocenter) to a point on a 
        ''' light ray that is closest to a specific solar system body.  For this purpose, 'Pos1' is the position 
        ''' vector toward observed object, with respect to origin at observer (or the geocenter); 'PosObs' is 
        ''' the position vector of solar system body, with respect to origin at observer (or the geocenter), 
        ''' components in AU; and the returned value is the light time to point on line defined by 'Pos1' 
        ''' that is closest to solar system body (positive if light passes body before hitting observer, i.e., if 
        ''' 'Pos1' is within 90 degrees of 'PosObs').
        ''' </remarks>
        <DispId(17)> Function DLight(ByVal Pos1() As Double, _
                                             ByVal PosObs() As Double) As Double

        ''' <summary>
        ''' Converts an ecliptic position vector to an equatorial position vector.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for coordinates.</param>
        ''' <param name="CoordSys">Coordinate system selection. 0 ... mean equator and equinox of date; 1 ... true equator and equinox of date; 2 ... ICRS</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos1"> Position vector, referred to specified ecliptic and equinox of date.  If 'CoordSys' = 2, 'pos1' must be on mean ecliptic and equinox of J2000.0; see Note 1 below.</param>
        ''' <param name="Pos2">Position vector, referred to specified equator and equinox of date.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'CoordSys'
        ''' </pre></returns>
        ''' <remarks>
        ''' To convert an ecliptic vector (mean ecliptic and equinox of J2000.0 only) to an ICRS vector, 
        ''' set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. 
        ''' Except for the output from this case, all vectors are assumed to be with respect to a dynamical system.
        ''' </remarks>
        <DispId(18)> Function Ecl2EquVec(ByVal JdTt As Double, _
                                                 ByVal CoordSys As CoordSys, _
                                                 ByVal Accuracy As Accuracy, _
                                                 ByVal Pos1() As Double, _
                                                 ByRef Pos2() As Double) As Short

        ''' <summary>
        '''  Compute the "complementary terms" of the equation of the equinoxes consistent with IAU 2000 resolutions.
        ''' </summary>
        ''' <param name="JdHigh">High-order part of TT Julian date.</param>
        ''' <param name="JdLow">Low-order part of TT Julian date.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <returns>Complementary terms, in radians.</returns>
        ''' <remarks>
        ''' Series from IERS Conventions (2003), Chapter 5, Table 5.2C, with some adjustments to coefficient values 
        ''' copied from IERS function 'eect2000', which has a more complete series.
        ''' </remarks>
        <DispId(19)> Function EeCt(ByVal JdHigh As Double, _
                                           ByVal JdLow As Double, _
                                           ByVal Accuracy As Accuracy) As Double

        ''' <summary>
        '''  Retrieves the position and velocity of a solar system body from a fundamental ephemeris.
        ''' </summary>
        ''' <param name="Jd"> TDB Julian date split into two parts, where the sum jd[0] + jd[1] is the TDB Julian date.</param>
        ''' <param name="CelObj">Structure containing the designation of the body of interest </param>
        ''' <param name="Origin"> Origin code; solar system barycenter = 0, center of mass of the Sun = 1.</param>
        ''' <param name="Accuracy">Slection for accuracy</param>
        ''' <param name="Pos">Position vector of the body at 'Jd'; equatorial rectangular coordinates in AU referred to the ICRS.</param>
        ''' <param name="Vel">Velocity vector of the body at 'Jd'; equatorial rectangular system referred to the mean equator and equinox of the ICRS, in AU/Day.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Origin'
        '''    2 ... Invalid value of 'Type' in 'CelObj'; 
        '''    3 ... Unable to allocate memory
        ''' 10+n ... where n is the error code from 'SolarSystem'; 
        ''' 20+n ... where n is the error code from 'ReadEph'.
        ''' </pre></returns>
        ''' <remarks>It is recommended that the input structure 'cel_obj' be created using function 'MakeObject' in file novas.c.</remarks>
        <DispId(20)> Function Ephemeris(ByVal Jd() As Double, _
                                                ByVal CelObj As Object3, _
                                                ByVal Origin As Origin, _
                                                ByVal Accuracy As Accuracy, _
                                                ByRef Pos() As Double, _
                                                ByRef Vel() As Double) As Short

        ''' <summary>
        ''' To convert right ascension and declination to ecliptic longitude and latitude.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for coordinates.</param>
        ''' <param name="CoordSys"> Coordinate system: 0 ... mean equator and equinox of date 'JdTt'; 1 ... true equator and equinox of date 'JdTt'; 2 ... ICRS</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Ra">Right ascension in hours, referred to specified equator and equinox of date.</param>
        ''' <param name="Dec">Declination in degrees, referred to specified equator and equinox of date.</param>
        ''' <param name="ELon">Ecliptic longitude in degrees, referred to specified ecliptic and equinox of date.</param>
        ''' <param name="ELat">Ecliptic latitude in degrees, referred to specified ecliptic and equinox of date.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'CoordSys'
        ''' </pre></returns>
        ''' <remarks>
        ''' To convert ICRS RA and dec to ecliptic coordinates (mean ecliptic and equinox of J2000.0), 
        ''' set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. 
        ''' Except for the input to this case, all input coordinates are dynamical.
        ''' </remarks>
        <DispId(21)> Function Equ2Ecl(ByVal JdTt As Double, _
                                              ByVal CoordSys As CoordSys, _
                                              ByVal Accuracy As Accuracy, _
                                              ByVal Ra As Double, _
                                              ByVal Dec As Double, _
                                              ByRef ELon As Double, _
                                              ByRef ELat As Double) As Short

        ''' <summary>
        ''' Converts an equatorial position vector to an ecliptic position vector.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for</param>
        ''' <param name="CoordSys"> Coordinate system selection. 0 ... mean equator and equinox of date 'JdTt'; 1 ... true equator and equinox of date 'JdTt'; 2 ... ICRS</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos1">Position vector, referred to specified equator and equinox of date.</param>
        ''' <param name="Pos2">Position vector, referred to specified ecliptic and equinox of date.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'CoordSys'
        ''' </pre></returns>
        ''' <remarks>To convert an ICRS vector to an ecliptic vector (mean ecliptic and equinox of J2000.0 only), 
        ''' set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. Except for 
        ''' the input to this case, all vectors are assumed to be with respect to a dynamical system.</remarks>
        <DispId(22)> Function Equ2EclVec(ByVal JdTt As Double, _
                                                 ByVal CoordSys As CoordSys, _
                                                 ByVal Accuracy As Accuracy, _
                                                 ByVal Pos1() As Double, _
                                                 ByRef Pos2() As Double) As Short

        ''' <summary>
        ''' Converts ICRS right ascension and declination to galactic longitude and latitude.
        ''' </summary>
        ''' <param name="RaI">ICRS right ascension in hours.</param>
        ''' <param name="DecI">ICRS declination in degrees.</param>
        ''' <param name="GLon">Galactic longitude in degrees.</param>
        ''' <param name="GLat">Galactic latitude in degrees.</param>
        ''' <remarks></remarks>
        <DispId(23)> Sub Equ2Gal(ByVal RaI As Double, _
                                         ByVal DecI As Double, _
                                         ByRef GLon As Double, _
                                         ByRef GLat As Double)

        ''' <summary>
        ''' Transforms topocentric right ascension and declination to zenith distance and azimuth.  
        ''' </summary>
        ''' <param name="Jd_Ut1">UT1 Julian date.</param>
        ''' <param name="DeltT">Difference TT-UT1 at 'jd_ut1', in seconds.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="x">onventionally-defined x coordinate of celestial intermediate pole with respect to ITRS reference pole, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined y coordinate of celestial intermediate pole with respect to ITRS reference pole, in arcseconds.</param>
        ''' <param name="Location">Structure containing observer's location </param>
        ''' <param name="Ra">Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date.</param>
        ''' <param name="Dec">Topocentric declination of object of interest, in degrees, referred to true equator and equinox of date.</param>
        ''' <param name="RefOption">Refraction option. 0 ... no refraction; 1 ... include refraction, using 'standard' atmospheric conditions;
        ''' 2 ... include refraction, using atmospheric parametersinput in the 'Location' structure.</param>
        ''' <param name="Zd">Topocentric zenith distance in degrees, affected by refraction if 'ref_option' is non-zero.</param>
        ''' <param name="Az">Topocentric azimuth (measured east from north) in degrees.</param>
        ''' <param name="RaR"> Topocentric right ascension of object of interest, in hours, referred to true equator and 
        ''' equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        ''' <param name="DecR">Topocentric declination of object of interest, in degrees, referred to true equator and 
        ''' equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        ''' <remarks>This function transforms topocentric right ascension and declination to zenith distance and azimuth.  
        ''' It uses a method that properly accounts for polar motion, which is significant at the sub-arcsecond level.  
        ''' This function can also adjust coordinates for atmospheric refraction.</remarks>
        <DispId(24)> Sub Equ2Hor(ByVal Jd_Ut1 As Double, _
                                         ByVal DeltT As Double, _
                                         ByVal Accuracy As Accuracy, _
                                         ByVal x As Double, _
                                         ByVal y As Double, _
                                         ByVal Location As OnSurface, _
                                         ByVal Ra As Double, _
                                         ByVal Dec As Double, _
                                         ByVal RefOption As RefractionOption, _
                                         ByRef Zd As Double, _
                                         ByRef Az As Double, _
                                         ByRef RaR As Double, _
                                         ByRef DecR As Double)

        ''' <summary>
        ''' Returns the value of the Earth Rotation Angle (theta) for a given UT1 Julian date. 
        ''' </summary>
        ''' <param name="JdHigh">High-order part of UT1 Julian date.</param>
        ''' <param name="JdLow">Low-order part of UT1 Julian date.</param>
        ''' <returns>The Earth Rotation Angle (theta) in degrees.</returns>
        ''' <remarks> The expression used is taken from the note to IAU Resolution B1.8 of 2000.  1. The algorithm used 
        ''' here is equivalent to the canonical theta = 0.7790572732640 + 1.00273781191135448 * t, where t is the time 
        ''' in days from J2000 (t = JdHigh + JdLow - T0), but it avoids many two-PI 'wraps' that 
        ''' decrease precision (adopted from SOFA Fortran routine iau_era00; see also expression at top 
        ''' of page 35 of IERS Conventions (1996)).</remarks>
        <DispId(25)> Function Era(ByVal JdHigh As Double, _
                                          ByVal JdLow As Double) As Double

        ''' <summary>
        ''' Computes quantities related to the orientation of the Earth's rotation axis at Julian date 'JdTdb'.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian Date.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Mobl">Mean obliquity of the ecliptic in degrees at 'JdTdb'.</param>
        ''' <param name="Tobl">True obliquity of the ecliptic in degrees at 'JdTdb'.</param>
        ''' <param name="Ee">Equation of the equinoxes in seconds of time at 'JdTdb'.</param>
        ''' <param name="Dpsi">Nutation in longitude in arcseconds at 'JdTdb'.</param>
        ''' <param name="Deps">Nutation in obliquity in arcseconds at 'JdTdb'.</param>
        ''' <remarks>Values of the celestial pole offsets 'PSI_COR' and 'EPS_COR' are set using function 'cel_pole', 
        ''' if desired.  See the prolog of 'cel_pole' for details.</remarks>
        <DispId(26)> Sub ETilt(ByVal JdTdb As Double, _
                                       ByVal Accuracy As Accuracy, _
                                       ByRef Mobl As Double, _
                                       ByRef Tobl As Double, _
                                       ByRef Ee As Double, _
                                       ByRef Dpsi As Double, _
                                       ByRef Deps As Double)

        ''' <summary>
        '''  To transform a vector from the dynamical reference system to the International Celestial Reference System (ICRS), or vice versa.
        ''' </summary>
        ''' <param name="Pos1">Position vector, equatorial rectangular coordinates.</param>
        ''' <param name="Direction">Set 'direction' <![CDATA[<]]> 0 for dynamical to ICRS transformation. Set 'direction' <![CDATA[>=]]> 0 for 
        ''' ICRS to dynamical transformation.</param>
        ''' <param name="Pos2">Position vector, equatorial rectangular coordinates.</param>
        ''' <remarks></remarks>
        <DispId(27)> Sub FrameTie(ByVal Pos1() As Double, _
                                          ByVal Direction As FrameConversionDirection, _
                                          ByRef Pos2() As Double)

        ''' <summary>
        ''' To compute the fundamental arguments (mean elements) of the Sun and Moon.
        ''' </summary>
        ''' <param name="t">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="a">Double array of fundamental arguments</param>
        ''' <remarks>
        ''' Fundamental arguments, in radians:
        ''' <pre>
        '''   a[0] = l (mean anomaly of the Moon)
        '''   a[1] = l' (mean anomaly of the Sun)
        '''   a[2] = F (mean argument of the latitude of the Moon)
        '''   a[3] = D (mean elongation of the Moon from the Sun)
        '''   a[4] = a[4] (mean longitude of the Moon's ascending node);
        '''                from Simon section 3.4(b.3),
        '''                precession = 5028.8200 arcsec/cy)
        ''' </pre>
        ''' </remarks>
        <DispId(28)> Sub FundArgs(ByVal t As Double, _
                                          ByRef a() As Double)

        ''' <summary>
        ''' Converts GCRS right ascension and declination to coordinates with respect to the equator of date (mean or true).
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of equator to be used for output coordinates.</param>
        ''' <param name="CoordSys"> Coordinate system selection for output coordinates.; 0 ... mean equator and 
        ''' equinox of date; 1 ... true equator and equinox of date; 2 ... true equator and CIO of date</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="RaG">GCRS right ascension in hours.</param>
        ''' <param name="DecG">GCRS declination in degrees.</param>
        ''' <param name="Ra"> Right ascension in hours, referred to specified equator and right ascension origin of date.</param>
        ''' <param name="Dec">Declination in degrees, referred to specified equator of date.</param>
        ''' <returns>
        ''' <pre>
        '''    0 ... everything OK
        ''' >  0 ... error from function 'Vector2RaDec'' 
        ''' > 10 ... 10 + error from function 'CioLocation'
        ''' > 20 ... 20 + error from function 'CioBasis'
        ''' </pre>></returns>
        ''' <remarks>For coordinates with respect to the true equator of date, the origin of right ascension can be either the true equinox or the celestial intermediate origin (CIO).
        ''' <para> This function only supports the CIO-based method.</para></remarks>
        <DispId(29)> Function Gcrs2Equ(ByVal JdTt As Double, _
                                               ByVal CoordSys As CoordSys, _
                                               ByVal Accuracy As Accuracy, _
                                               ByVal RaG As Double, _
                                               ByVal DecG As Double, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double) As Short

        ''' <summary>
        ''' This function computes the geocentric position and velocity of an observer on 
        ''' the surface of the earth or on a near-earth spacecraft.</summary>
        ''' <param name="JdTt">TT Julian date.</param>
        ''' <param name="DeltaT">Value of Delta T (= TT - UT1) at 'JdTt'.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Obs">Data specifying the location of the observer</param>
        ''' <param name="Pos">Position vector of observer, with respect to origin at geocenter, 
        ''' referred to GCRS axes, components in AU.</param>
        ''' <param name="Vel">Velocity vector of observer, with respect to origin at geocenter, 
        ''' referred to GCRS axes, components in AU/day.</param>
        ''' <returns>
        ''' <pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'Accuracy'.
        ''' </pre></returns>
        ''' <remarks>The final vectors are expressed in the GCRS.</remarks>
        <DispId(30)> Function GeoPosVel(ByVal JdTt As Double, _
                                        ByVal DeltaT As Double, _
                                        ByVal Accuracy As Accuracy, _
                                        ByVal Obs As Observer, _
                                        ByRef Pos() As Double, _
                                        ByRef Vel() As Double) As Short

        ''' <summary>
        ''' Computes the total gravitational deflection of light for the observed object due to the major gravitating bodies in the solar system.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date of observation.</param>
        ''' <param name="LocCode">Code for location of observer, determining whether the gravitational deflection due to the earth itself is applied.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos1"> Position vector of observed object, with respect to origin at observer (or the geocenter), 
        ''' referred to ICRS axes, components in AU.</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar 
        ''' system barycenter, referred to ICRS axes, components in AU.</param>
        ''' <param name="Pos2">Position vector of observed object, with respect to origin at observer (or the geocenter), 
        ''' referred to ICRS axes, corrected for gravitational deflection, components in AU.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        ''' <![CDATA[<]]> 30 ... Error from function 'Ephemeris'; 
        ''' > 30 ... Error from function 'MakeObject'.
        ''' </pre></returns>
        ''' <remarks>This function valid for an observed body within the solar system as well as for a star.
        ''' <para>
        ''' If 'Accuracy' is set to zero (full accuracy), three bodies (Sun, Jupiter, and Saturn) are 
        ''' used in the calculation.  If the reduced-accuracy option is set, only the Sun is used in the 
        ''' calculation.  In both cases, if the observer is not at the geocenter, the deflection due to the Earth is included.
        ''' </para>
        ''' </remarks>
        <DispId(31)> Function GravDef(ByVal JdTdb As Double, _
                                              ByVal LocCode As EarthDeflection, _
                                              ByVal Accuracy As Accuracy, _
                                              ByVal Pos1() As Double, _
                                              ByVal PosObs() As Double, _
                                              ByRef Pos2() As Double) As Short

        ''' <summary>
        ''' Corrects position vector for the deflection of light in the gravitational field of an arbitrary body.
        ''' </summary>
        ''' <param name="Pos1">Position vector of observed object, with respect to origin at observer 
        ''' (or the geocenter), components in AU.</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at 
        ''' solar system barycenter, components in AU.</param>
        ''' <param name="PosBody">Position vector of gravitating body, with respect to origin at solar system 
        ''' barycenter, components in AU.</param>
        ''' <param name="RMass">Reciprocal mass of gravitating body in solar mass units, that is, 
        ''' Sun mass / body mass.</param>
        ''' <param name="Pos2">Position vector of observed object, with respect to origin at observer 
        ''' (or the geocenter), corrected for gravitational deflection, components in AU.</param>
        ''' <remarks>This function valid for an observed body within the solar system as well as for a star.</remarks>
        <DispId(32)> Sub GravVec(ByVal Pos1() As Double, _
                                         ByVal PosObs() As Double, _
                                         ByVal PosBody() As Double, _
                                         ByVal RMass As Double, _
                                         ByRef Pos2() As Double)

        ''' <summary>
        ''' Compute the intermediate right ascension of the equinox at the input Julian date
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date.</param>
        ''' <param name="Equinox">Equinox selection flag: mean pr true</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <returns>Intermediate right ascension of the equinox, in hours (+ or -). If 'equinox' = 1 
        ''' (i.e true equinox), then the returned value is the equation of the origins.</returns>
        ''' <remarks></remarks>
        <DispId(33)> Function IraEquinox(ByVal JdTdb As Double, _
                                                ByVal Equinox As EquinoxType, _
                                                ByVal Accuracy As Accuracy) As Double

        ''' <summary>
        ''' Compute the Julian date for a given calendar date (year, month, day, hour).
        ''' </summary>
        ''' <param name="Year">Year number</param>
        ''' <param name="Month">Month number</param>
        ''' <param name="Day">Day number</param>
        ''' <param name="Hour">Fractional hour of the day</param>
        ''' <returns>Computed Julian date.</returns>
        ''' <remarks>This function makes no checks for a valid input calendar date. The input calendar date 
        ''' must be Gregorian. The input time value can be based on any UT-like time scale (UTC, UT1, TT, etc.) 
        ''' - output Julian date will have the same basis.</remarks>
        <DispId(34)> Function JulianDate(ByVal Year As Short, _
                                                 ByVal Month As Short, _
                                                 ByVal Day As Short, _
                                                 ByVal Hour As Double) As Double

        ''' <summary>
        ''' Computes the geocentric position of a solar system body, as antedated for light-time.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date of observation.</param>
        ''' <param name="SsObject">Structure containing the designation for thesolar system body</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin 
        ''' at solar system barycenter, referred to ICRS axes, components in AU.</param>
        ''' <param name="TLight0">First approximation to light-time, in days (can be set to 0.0 if unknown)</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos">Position vector of body, with respect to origin at observer (or the geocenter), 
        ''' referred to ICRS axes, components in AU.</param>
        ''' <param name="TLight">Final light-time, in days.</param>
        ''' <returns><pre>
        '''    0 ... everything OK
        '''    1 ... algorithm failed to converge after 10 iterations
        ''' <![CDATA[>]]> 10 ... error is 10 + error from function 'SolarSystem'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(35)> Function LightTime(ByVal JdTdb As Double, _
                                                ByVal SsObject As Object3, _
                                                ByVal PosObs() As Double, _
                                                ByVal TLight0 As Double, _
                                                ByVal Accuracy As Accuracy, _
                                                ByRef Pos() As Double, _
                                                ByRef TLight As Double) As Short

        ''' <summary>
        ''' Determines the angle of an object above or below the Earth's limb (horizon).
        ''' </summary>
        ''' <param name="PosObj">Position vector of observed object, with respect to origin at 
        ''' geocenter, components in AU.</param>
        ''' <param name="PosObs">Position vector of observer, with respect to origin at geocenter, 
        ''' components in AU.</param>
        ''' <param name="LimbAng">Angle of observed object above (+) or below (-) limb in degrees.</param>
        ''' <param name="NadirAng">Nadir angle of observed object as a fraction of apparent radius of limb: <![CDATA[<]]> 1.0 ... 
        ''' below the limb; = 1.0 ... on the limb;  <![CDATA[>]]> 1.0 ... above the limb</param>
        ''' <remarks>The geometric limb is computed, assuming the Earth to be an airless sphere (no 
        ''' refraction or oblateness is included).  The observer can be on or above the Earth.  
        ''' For an observer on the surface of the Earth, this function returns the approximate unrefracted 
        ''' altitude.</remarks>
        <DispId(36)> Sub LimbAngle(ByVal PosObj() As Double, _
                                           ByVal PosObs() As Double, _
                                           ByRef LimbAng As Double, _
                                           ByRef NadirAng As Double)

        ''' <summary>
        ''' Computes the local place of a solar system body.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for local place.</param>
        ''' <param name="SsBody">structure containing the body designation for the solar system body</param>
        ''' <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="Position">Specifies the position of the observer</param>
        ''' <param name="Accuracy">Specifies accuracy level</param>
        ''' <param name="Ra">Local right ascension in hours, referred to the 'local GCRS'.</param>
        ''' <param name="Dec">Local declination in degrees, referred to the 'local GCRS'.</param>
        ''' <param name="Dis">True distance from Earth to planet in AU.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Where' in structure 'Location'; 
        ''' <![CDATA[>]]> 10 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(37)> Function LocalPlanet(ByVal JdTt As Double, _
                                                  ByVal SsBody As Object3, _
                                                  ByVal DeltaT As Double, _
                                                  ByVal Position As OnSurface, _
                                                  ByVal Accuracy As Accuracy, _
                                                  ByRef Ra As Double, _
                                                  ByRef Dec As Double, _
                                                  ByRef Dis As Double) As Short

        ''' <summary>
        '''  Computes the local place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for local place. delta_t (double)</param>
        ''' <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="Star">catalog entry structure containing catalog data for the object in the ICRS</param>
        ''' <param name="Position">Structure specifying the position of the observer </param>
        ''' <param name="Accuracy">Specifies accuracy level.</param>
        ''' <param name="Ra">Local right ascension in hours, referred to the 'local GCRS'.</param>
        ''' <param name="Dec">Local declination in degrees, referred to the 'local GCRS'.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Where' in structure 'Location'
        ''' > 10 ... Error code from function 'MakeObject'
        ''' > 20 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(38)> Function LocalStar(ByVal JdTt As Double, _
                                            ByVal DeltaT As Double, _
                                            ByVal Star As CatEntry3, _
                                            ByVal Position As OnSurface, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double) As Short

        ''' <summary>
        ''' Create a structure of type 'cat_entry' containing catalog data for a star or "star-like" object.
        ''' </summary>
        ''' <param name="StarName">Object name (50 characters maximum).</param>
        ''' <param name="Catalog">Three-character catalog identifier (e.g. HIP = Hipparcos, TY2 = Tycho-2)</param>
        ''' <param name="StarNum">Object number in the catalog.</param>
        ''' <param name="Ra">Right ascension of the object (hours).</param>
        ''' <param name="Dec">Declination of the object (degrees).</param>
        ''' <param name="PmRa">Proper motion in right ascension (milliarcseconds/year).</param>
        ''' <param name="PmDec">Proper motion in declination (milliarcseconds/year).</param>
        ''' <param name="Parallax">Parallax (milliarcseconds).</param>
        ''' <param name="RadVel">Radial velocity (kilometers/second).</param>
        ''' <param name="Star">CatEntry3 structure containing the input data</param>
        ''' <remarks></remarks>
        <DispId(39)> Sub MakeCatEntry(ByVal StarName As String, _
                                        ByVal Catalog As String, _
                                        ByVal StarNum As Integer, _
                                        ByVal Ra As Double, _
                                        ByVal Dec As Double, _
                                        ByVal PmRa As Double, _
                                        ByVal PmDec As Double, _
                                        ByVal Parallax As Double, _
                                        ByVal RadVel As Double, _
                                        ByRef Star As CatEntry3)

        ''' <summary>
        ''' Makes a structure of type 'InSpace' - specifying the position and velocity of an observer situated 
        ''' on a near-Earth spacecraft.
        ''' </summary>
        ''' <param name="ScPos">Geocentric position vector (x, y, z) in km.</param>
        ''' <param name="ScVel">Geocentric velocity vector (x_dot, y_dot, z_dot) in km/s.</param>
        ''' <param name="ObsSpace">InSpace structure containing the position and velocity of an observer situated 
        ''' on a near-Earth spacecraft</param>
        ''' <remarks></remarks>
        <DispId(40)> Sub MakeInSpace(ByVal ScPos() As Double, _
                                         ByVal ScVel() As Double, _
                                         ByRef ObsSpace As InSpace)

        ''' <summary>
        ''' Makes a structure of type 'object' - specifying a celestial object - based on the input parameters.
        ''' </summary>
        ''' <param name="Type">Type of object: 0 ... major planet, Sun, or Moon;  1 ... minor planet; 
        ''' 2 ... object located outside the solar system (e.g. star, galaxy, nebula, etc.)</param>
        ''' <param name="Number">Body number: For 'Type' = 0: Mercury = 1,...,Pluto = 9, Sun = 10, Moon = 11; 
        ''' For 'Type' = 1: minor planet numberFor 'Type' = 2: set to 0 (zero)</param>
        ''' <param name="Name">Name of the object (50 characters maximum).</param>
        ''' <param name="StarData">Structure containing basic astrometric data for any celestial object 
        ''' located outside the solar system; the catalog data for a star</param>
        ''' <param name="CelObj">Structure containing the object definition</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'Type'
        ''' 2 ... 'Number' out of range
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(41)> Function MakeObject(ByVal Type As ObjectType, _
                                               ByVal Number As Short, _
                                               ByVal Name As String, _
                                               ByVal StarData As CatEntry3, _
                                               ByRef CelObj As Object3) As Short

        ''' <summary>
        '''  Makes a structure of type 'observer' - specifying the location of the observer.
        ''' </summary>
        ''' <param name="Where">Integer code specifying location of observer: 0: observer at geocenter; 
        ''' 1: observer on surface of earth; 2: observer on near-earth spacecraft</param>
        ''' <param name="ObsSurface">Structure containing data for an observer's location on the surface 
        ''' of the Earth; used when 'Where' = 1</param>
        ''' <param name="ObsSpace"> Structure containing an observer's location on a near-Earth spacecraft; 
        ''' used when 'Where' = 2 </param>
        ''' <param name="Obs">Structure specifying the location of the observer </param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... input value of 'Where' is out-of-range.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(42)> Function MakeObserver(ByVal Where As ObserverLocation, _
                                              ByVal ObsSurface As OnSurface, _
                                              ByVal ObsSpace As InSpace, _
                                              ByRef Obs As Observer) As Short

        ''' <summary>
        '''  Makes a structure of type 'observer' specifying an observer at the geocenter.
        ''' </summary>
        ''' <param name="ObsAtGeocenter">Structure specifying the location of the observer at the geocenter</param>
        ''' <remarks></remarks>
        <DispId(43)> Sub MakeObserverAtGeocenter(ByRef ObsAtGeocenter As Observer)

        ''' <summary>
        '''  Makes a structure of type 'observer' specifying the position and velocity of an observer 
        ''' situated on a near-Earth spacecraft.
        ''' </summary>
        ''' <param name="ScPos">Geocentric position vector (x, y, z) in km.</param>
        ''' <param name="ScVel">Geocentric position vector (x, y, z) in km.</param>
        ''' <param name="ObsInSpace">Structure containing the position and velocity of an observer 
        ''' situated on a near-Earth spacecraft</param>
        ''' <remarks>Both input vectors are with respect to true equator and equinox of date.</remarks>
        <DispId(44)> Sub MakeObserverInSpace(ByVal ScPos() As Double, _
                                                ByVal ScVel() As Double, _
                                                ByRef ObsInSpace As Observer)

        ''' <summary>
        ''' Makes a structure of type 'observer' specifying the location of and weather for an observer 
        ''' on the surface of the Earth.
        ''' </summary>
        ''' <param name="Latitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        ''' <param name="Longitude">Geodetic (ITRS) longitude in degrees; east positive.</param>
        ''' <param name="Height">Height of the observer (meters).</param>
        ''' <param name="Temperature">Temperature (degrees Celsius).</param>
        ''' <param name="Pressure">Atmospheric pressure (millibars).</param>
        ''' <param name="ObsOnSurface">Structure containing the location of and weather for an observer on 
        ''' the surface of the Earth</param>
        ''' <remarks></remarks>
        <DispId(45)> Sub MakeObserverOnSurface(ByVal Latitude As Double, _
                                                ByVal Longitude As Double, _
                                                ByVal Height As Double, _
                                                ByVal Temperature As Double, _
                                                ByVal Pressure As Double, _
                                                ByRef ObsOnSurface As Observer)

        ''' <summary>
        ''' Makes a structure of type 'on_surface' - specifying the location of and weather for an 
        ''' observer on the surface of the Earth.
        ''' </summary>
        ''' <param name="Latitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        ''' <param name="Longitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        ''' <param name="Height">Height of the observer (meters).</param>
        ''' <param name="Temperature">Temperature (degrees Celsius).</param>
        ''' <param name="Pressure">Atmospheric pressure (millibars).</param>
        ''' <param name="ObsSurface">Structure containing the location of and weather for an 
        ''' observer on the surface of the Earth.</param>
        ''' <remarks></remarks>
        <DispId(46)> Sub MakeOnSurface(ByVal Latitude As Double, _
                                        ByVal Longitude As Double, _
                                        ByVal Height As Double, _
                                        ByVal Temperature As Double, _
                                        ByVal Pressure As Double, _
                                        ByRef ObsSurface As OnSurface)

        ''' <summary>
        ''' Compute the mean obliquity of the ecliptic.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian Date.</param>
        ''' <returns>Mean obliquity of the ecliptic in arcseconds.</returns>
        ''' <remarks></remarks>
        <DispId(47)> Function MeanObliq(ByVal JdTdb As Double) As Double

        ''' <summary>
        ''' Computes the ICRS position of a star, given its apparent place at date 'JdTt'.  
        ''' Proper motion, parallax and radial velocity are assumed to be zero.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of apparent place.</param>
        ''' <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        ''' <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date.</param>
        ''' <param name="Accuracy">Specifies accuracy level</param>
        ''' <param name="IRa">ICRS right ascension in hours.</param>
        ''' <param name="IDec">ICRS declination in degrees.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Iterative process did not converge after 30 iterations; 
        ''' > 10 ... Error from function 'Vector2RaDec'
        ''' > 20 ... Error from function 'AppStar'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(48)> Function MeanStar(ByVal JdTt As Double, _
                                          ByVal Ra As Double, _
                                          ByVal Dec As Double, _
                                          ByVal Accuracy As Accuracy, _
                                          ByRef IRa As Double, _
                                          ByRef IDec As Double) As Short

        ''' <summary>
        ''' Normalize angle into the range 0 <![CDATA[<=]]> angle <![CDATA[<]]> (2 * pi).
        ''' </summary>
        ''' <param name="Angle">Input angle (radians).</param>
        ''' <returns>The input angle, normalized as described above (radians).</returns>
        ''' <remarks></remarks>
        <DispId(49)> Function NormAng(ByVal Angle As Double) As Double

        ''' <summary>
        ''' Nutates equatorial rectangular coordinates from mean equator and equinox of epoch to true equator and equinox of epoch.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date of epoch.</param>
        ''' <param name="Direction">Flag determining 'direction' of transformation; direction  = 0 
        ''' transformation applied, mean to true; direction != 0 inverse transformation applied, true to mean.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos">Position vector, geocentric equatorial rectangular coordinates, referred to 
        ''' mean equator and equinox of epoch.</param>
        ''' <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, referred to 
        ''' true equator and equinox of epoch.</param>
        ''' <remarks> Inverse transformation may be applied by setting flag 'direction'</remarks>
        <DispId(50)> Sub Nutation(ByVal JdTdb As Double, _
                                      ByVal Direction As NutationDirection, _
                                      ByVal Accuracy As Accuracy, _
                                      ByVal Pos() As Double, _
                                      ByRef Pos2() As Double)

        ''' <summary>
        ''' Returns the values for nutation in longitude and nutation in obliquity for a given TDB Julian date.
        ''' </summary>
        ''' <param name="t">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="DPsi">Nutation in longitude in arcseconds.</param>
        ''' <param name="DEps">Nutation in obliquity in arcseconds.</param>
        ''' <remarks>The nutation model selected depends upon the input value of 'Accuracy'.  See notes below for important details.
        ''' <para>
        ''' This function selects the nutation model depending first upon the input value of 'Accuracy'.  
        ''' If 'Accuracy' = 0 (full accuracy), the IAU 2000A nutation model is used.  If 'Accuracy' = 1 
        ''' a specially truncated (and therefore faster) version of IAU 2000A, called 'NU2000K' is used.
        ''' </para>
        ''' </remarks>
        <DispId(51)> Sub NutationAngles(ByVal t As Double, _
                                          ByVal Accuracy As Accuracy, _
                                          ByRef DPsi As Double, _
                                          ByRef DEps As Double)

        ''' <summary>
        ''' Computes the apparent direction of a star or solar system body at a specified time 
        ''' and in a specified coordinate system.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for place.</param>
        ''' <param name="CelObject"> Specifies the celestial object of interest</param>
        ''' <param name="Location">Specifies the location of the observer</param>
        ''' <param name="DeltaT"> Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="CoordSys">Code specifying coordinate system of the output position. 0 ... GCRS or 
        ''' "local GCRS"; 1 ... true equator and equinox of date; 2 ... true equator and CIO of date; 
        ''' 3 ... astrometric coordinates, i.e., without light deflection or aberration.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Output">Structure specifying object's place on the sky at time 'JdTt', 
        ''' with respect to the specified output coordinate system</param>
        ''' <returns>
        ''' <pre>
        ''' = 0         ... No problems.
        ''' = 1         ... invalid value of 'CoordSys'
        ''' = 2         ... invalid value of 'Accuracy'
        ''' = 3         ... Earth is the observed object, and the observer is either at the geocenter or on the Earth's surface (not permitted)
        ''' > 10, <![CDATA[<]]> 40  ... 10 + error from function 'Ephemeris'
        ''' > 40, <![CDATA[<]]> 50  ... 40 + error from function 'GeoPosVel'
        ''' > 50, <![CDATA[<]]> 70  ... 50 + error from function 'LightTime'
        ''' > 70, <![CDATA[<]]> 80  ... 70 + error from function 'GravDef'
        ''' > 80, <![CDATA[<]]> 90  ... 80 + error from function 'CioLocation'
        ''' > 90, <![CDATA[<]]> 100 ... 90 + error from function 'CioBasis'
        ''' </pre>
        ''' </returns>
        ''' Values of 'location->where' and 'CoordSys' dictate the various standard kinds of place:
        ''' <pre>
        '''     Location->Where = 0 and CoordSys = 1: apparent place
        '''     Location->Where = 1 and CoordSys = 1: topocentric place
        '''     Location->Where = 0 and CoordSys = 0: virtual place
        '''     Location->Where = 1 and CoordSys = 0: local place
        '''     Location->Where = 0 and CoordSys = 3: astrometric place
        '''     Location->Where = 1 and CoordSys = 3: topocentric astrometric place
        ''' </pre>
        ''' <para>Input value of 'DeltaT' is used only when 'Location->Where' equals 1 or 2 (observer is 
        ''' on surface of Earth or in a near-Earth satellite). </para>
        ''' <remarks>
        ''' </remarks>
        <DispId(52)> Function Place(ByVal JdTt As Double, _
                                       ByVal CelObject As Object3, _
                                       ByVal Location As Observer, _
                                       ByVal DeltaT As Double, _
                                       ByVal CoordSys As CoordSys, _
                                       ByVal Accuracy As Accuracy, _
                                       ByRef Output As SkyPos) As Short

        ''' <summary>
        '''  Precesses equatorial rectangular coordinates from one epoch to another.
        ''' </summary>
        ''' <param name="JdTdb1">TDB Julian date of first epoch.  See remarks below.</param>
        ''' <param name="Pos1">Position vector, geocentric equatorial rectangular coordinates, referred to mean dynamical equator and equinox of first epoch.</param>
        ''' <param name="JdTdb2">TDB Julian date of second epoch.  See remarks below.</param>
        ''' <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, referred to mean dynamical equator and equinox of second epoch.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... Precession not to or from J2000.0; 'JdTdb1' or 'JdTdb2' not 2451545.0.
        ''' </pre></returns>
        ''' <remarks> One of the two epochs must be J2000.0.  The coordinates are referred to the mean dynamical equator and equinox of the two respective epochs.</remarks>
        <DispId(53)> Function Precession(ByVal JdTdb1 As Double, _
                                           ByVal Pos1() As Double, _
                                           ByVal JdTdb2 As Double, _
                                           ByRef Pos2() As Double) As Short

        ''' <summary>
        ''' Applies proper motion, including foreshortening effects, to a star's position.
        ''' </summary>
        ''' <param name="JdTdb1">TDB Julian date of first epoch.</param>
        ''' <param name="Pos">Position vector at first epoch.</param>
        ''' <param name="Vel">Velocity vector at first epoch.</param>
        ''' <param name="JdTdb2">TDB Julian date of second epoch.</param>
        ''' <param name="Pos2">Position vector at second epoch.</param>
        ''' <remarks></remarks>
        <DispId(54)> Sub ProperMotion(ByVal JdTdb1 As Double, _
                                       ByVal Pos() As Double, _
                                       ByVal Vel() As Double, _
                                       ByVal JdTdb2 As Double, _
                                       ByRef Pos2() As Double)

        ''' <summary>
        ''' Converts equatorial spherical coordinates to a vector (equatorial rectangular coordinates).
        ''' </summary>
        ''' <param name="Ra">Right ascension (hours).</param>
        ''' <param name="Dec">Declination (degrees).</param>
        ''' <param name="Dist">Distance in AU</param>
        ''' <param name="Vector">Position vector, equatorial rectangular coordinates (AU).</param>
        ''' <remarks></remarks>
        <DispId(55)> Sub RaDec2Vector(ByVal Ra As Double, _
                                      ByVal Dec As Double, _
                                      ByVal Dist As Double, _
                                      ByRef Vector() As Double)

        ''' <summary>
        ''' Predicts the radial velocity of the observed object as it would be measured by spectroscopic means.
        ''' </summary>
        ''' <param name="CelObject">Specifies the celestial object of interest</param>
        ''' <param name="Pos"> Geometric position vector of object with respect to observer, corrected for light-time, in AU.</param>
        ''' <param name="Vel">Velocity vector of object with respect to solar system barycenter, in AU/day.</param>
        ''' <param name="VelObs">Velocity vector of observer with respect to solar system barycenter, in AU/day.</param>
        ''' <param name="DObsGeo">Distance from observer to geocenter, in AU.</param>
        ''' <param name="DObsSun">Distance from observer to Sun, in AU.</param>
        ''' <param name="DObjSun">Distance from object to Sun, in AU.</param>
        ''' <param name="Rv">The observed radial velocity measure times the speed of light, in kilometers/second.</param>
        ''' <remarks> Radial velocity is here defined as the radial velocity measure (z) times the speed of light.  
        ''' For a solar system body, it applies to a fictitious emitter at the center of the observed object, 
        ''' assumed massless (no gravitational red shift), and does not in general apply to reflected light.  
        ''' For stars, it includes all effects, such as gravitational red shift, contained in the catalog 
        ''' barycentric radial velocity measure, a scalar derived from spectroscopy.  Nearby stars with a known 
        ''' kinematic velocity vector (obtained independently of spectroscopy) can be treated like 
        ''' solar system objects.</remarks>
        <DispId(56)> Sub RadVel(ByVal CelObject As Object3, _
                                   ByVal Pos() As Double, _
                                   ByVal Vel() As Double, _
                                   ByVal VelObs() As Double, _
                                   ByVal DObsGeo As Double, _
                                   ByVal DObsSun As Double, _
                                   ByVal DObjSun As Double, _
                                   ByRef Rv As Double)

        ''' <summary>
        ''' Computes atmospheric refraction in zenith distance. 
        ''' </summary>
        ''' <param name="Location">Structure containing observer's location.</param>
        ''' <param name="RefOption">1 ... Use 'standard' atmospheric conditions; 2 ... Use atmospheric 
        ''' parameters input in the 'Location' structure.</param>
        ''' <param name="ZdObs">Observed zenith distance, in degrees.</param>
        ''' <returns>Atmospheric refraction, in degrees.</returns>
        ''' <remarks>This version computes approximate refraction for optical wavelengths. This function 
        ''' can be used for planning observations or telescope pointing, but should not be used for the 
        ''' reduction of precise observations.</remarks>
        <DispId(57)> Function Refract(ByVal Location As OnSurface, _
                                        ByVal RefOption As RefractionOption, _
                                        ByVal ZdObs As Double) As Double

        ''' <summary>
        ''' Computes the Greenwich apparent sidereal time, at Julian date 'JdHigh' + 'JdLow'.
        ''' </summary>
        ''' <param name="JdHigh">High-order part of UT1 Julian date.</param>
        ''' <param name="JdLow">Low-order part of UT1 Julian date.</param>
        ''' <param name="DeltaT"> Difference TT-UT1 at 'JdHigh'+'JdLow', in seconds of time.</param>
        ''' <param name="GstType">0 ... compute Greenwich mean sidereal time; 1 ... compute Greenwich apparent sidereal time</param>
        ''' <param name="Method">Selection for method: 0 ... CIO-based method; 1 ... equinox-based method</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Gst">Greenwich apparent sidereal time, in hours.</param>
        ''' <returns><pre>
        '''          0 ... everything OK
        '''          1 ... invalid value of 'Accuracy'
        '''          2 ... invalid value of 'Method'
        ''' > 10, <![CDATA[<]]> 30 ... 10 + error from function 'CioRai'
        ''' </pre></returns>
        ''' <remarks> The Julian date may be split at any point, but for highest precision, set 'JdHigh' 
        ''' to be the integral part of the Julian date, and set 'JdLow' to be the fractional part.</remarks>
        <DispId(58)> Function SiderealTime(ByVal JdHigh As Double, _
                                           ByVal JdLow As Double, _
                                           ByVal DeltaT As Double, _
                                           ByVal GstType As GstType, _
                                           ByVal Method As Method, _
                                           ByVal Accuracy As Accuracy, _
                                           ByRef Gst As Double) As Short

        ''' <summary>
        ''' Transforms a vector from one coordinate system to another with same origin and axes rotated about the z-axis.
        ''' </summary>
        ''' <param name="Angle"> Angle of coordinate system rotation, positive counterclockwise when viewed from +z, in degrees.</param>
        ''' <param name="Pos1">Position vector.</param>
        ''' <param name="Pos2">Position vector expressed in new coordinate system rotated about z by 'angle'.</param>
        ''' <remarks></remarks>
        <DispId(59)> Sub Spin(ByVal Angle As Double, _
                              ByVal Pos1() As Double, _
                              ByRef Pos2() As Double)

        ''' <summary>
        ''' Converts angular quantities for stars to vectors.
        ''' </summary>
        ''' <param name="Star">Catalog entry structure containing ICRS catalog data </param>
        ''' <param name="Pos">Position vector, equatorial rectangular coordinates, components in AU.</param>
        ''' <param name="Vel">Velocity vector, equatorial rectangular coordinates, components in AU/Day.</param>
        ''' <remarks></remarks>
        <DispId(60)> Sub StarVectors(ByVal Star As CatEntry3, _
                                      ByRef Pos() As Double, _
                                      ByRef Vel() As Double)

        ''' <summary>
        ''' Computes the Terrestrial Time (TT) or Terrestrial Dynamical Time (TDT) Julian date corresponding 
        ''' to a Barycentric Dynamical Time (TDB) Julian date.
        ''' </summary>
        ''' <param name="TdbJd">TDB Julian date.</param>
        ''' <param name="TtJd">TT Julian date.</param>
        ''' <param name="SecDiff">Difference 'tdb_jd'-'tt_jd', in seconds.</param>
        ''' <remarks>Expression used in this function is a truncated form of a longer and more precise 
        ''' series given in: Explanatory Supplement to the Astronomical Almanac, pp. 42-44 and p. 316. 
        ''' The result is good to about 10 microseconds.</remarks>
        <DispId(61)> Sub Tdb2Tt(ByVal TdbJd As Double, _
                                    ByRef TtJd As Double, _
                                    ByRef SecDiff As Double)

        ''' <summary>
        ''' This function rotates a vector from the terrestrial to the celestial system. 
        ''' </summary>
        ''' <param name="JdHigh">High-order part of UT1 Julian date.</param>
        ''' <param name="JdLow">Low-order part of UT1 Julian date.</param>
        ''' <param name="DeltaT">Value of Delta T (= TT - UT1) at the input UT1 Julian date.</param>
        ''' <param name="Method"> Selection for method: 0 ... CIO-based method; 1 ... equinox-based method</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="OutputOption">0 ... The output vector is referred to GCRS axes; 1 ... The output 
        ''' vector is produced with respect to the equator and equinox of date.</param>
        ''' <param name="x">Conventionally-defined X coordinate of celestial intermediate pole with respect to 
        ''' ITRF pole, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of celestial intermediate pole with respect to 
        ''' ITRF pole, in arcseconds.</param>
        ''' <param name="VecT">Position vector, geocentric equatorial rectangular coordinates, referred to ITRF 
        ''' axes (terrestrial system) in the normal case where 'option' = 0.</param>
        ''' <param name="VecC"> Position vector, geocentric equatorial rectangular coordinates, referred to GCRS 
        ''' axes (celestial system) or with respect to the equator and equinox of date, depending on 'Option'.</param>
        ''' <returns><pre>
        '''    0 ... everything is ok
        '''    1 ... invalid value of 'Accuracy'
        '''    2 ... invalid value of 'Method'
        ''' > 10 ... 10 + error from function 'CioLocation'
        ''' > 20 ... 20 + error from function 'CioBasis'
        ''' </pre></returns>
        ''' <remarks>'x' = 'y' = 0 means no polar motion transformation.
        ''' <para>
        ''' The 'option' flag only works for the equinox-based method.
        '''</para></remarks>
        <DispId(62)> Function Ter2Cel(ByVal JdHigh As Double, _
                                        ByVal JdLow As Double, _
                                        ByVal DeltaT As Double, _
                                        ByVal Method As Method, _
                                        ByVal Accuracy As Accuracy, _
                                        ByVal OutputOption As OutputVectorOption, _
                                        ByVal x As Double, _
                                        ByVal y As Double, _
                                        ByVal VecT() As Double, _
                                        ByRef VecC() As Double) As Short

        ''' <summary>
        ''' Computes the position and velocity vectors of a terrestrial observer with respect to the center of the Earth.
        ''' </summary>
        ''' <param name="Location">Structure containing observer's location </param>
        ''' <param name="St">Local apparent sidereal time at reference meridian in hours.</param>
        ''' <param name="Pos">Position vector of observer with respect to center of Earth, equatorial 
        ''' rectangular coordinates, referred to true equator and equinox of date, components in AU.</param>
        ''' <param name="Vel">Velocity vector of observer with respect to center of Earth, equatorial rectangular 
        ''' coordinates, referred to true equator and equinox of date, components in AU/day.</param>
        ''' <remarks>
        ''' If reference meridian is Greenwich and st=0, 'pos' is effectively referred to equator and Greenwich.
        ''' <para> This function ignores polar motion, unless the observer's longitude and latitude have been 
        ''' corrected for it, and variation in the length of day (angular velocity of earth).</para>
        ''' <para>The true equator and equinox of date do not form an inertial system.  Therefore, with respect 
        ''' to an inertial system, the very small velocity component (several meters/day) due to the precession 
        ''' and nutation of the Earth's axis is not accounted for here.</para>
        ''' </remarks>
        <DispId(63)> Sub Terra(ByVal Location As OnSurface, _
                               ByVal St As Double, _
                               ByRef Pos() As Double, _
                               ByRef Vel() As Double)

        ''' <summary>
        ''' Computes the topocentric place of a solar system body.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for topocentric place.</param>
        ''' <param name="SsBody">structure containing the body designation for the solar system body</param>
        ''' <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="Position">Specifies the position of the observer</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        ''' <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date.</param>
        ''' <param name="Dis">True distance from Earth to planet at 'JdTt' in AU.</param>
        ''' <returns><pre>
        ''' =  0 ... Everything OK.
        ''' =  1 ... Invalid value of 'Where' in structure 'Location'.
        ''' > 10 ... Error code from function 'Place'.
        '''</pre></returns>
        ''' <remarks></remarks>
        <DispId(64)> Function TopoPlanet(ByVal JdTt As Double, _
                                            ByVal SsBody As Object3, _
                                            ByVal DeltaT As Double, _
                                            ByVal Position As OnSurface, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double, _
                                            ByRef Dis As Double) As Short

        ''' <summary>
        ''' Computes the topocentric place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for topocentric place.</param>
        ''' <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="Star">Catalog entry structure containing catalog data for the object in the ICRS</param>
        ''' <param name="Position">Specifies the position of the observer</param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra"> Topocentric right ascension in hours, referred to true equator and equinox of date 'JdTt'.</param>
        ''' <param name="Dec">Topocentric declination in degrees, referred to true equator and equinox of date 'JdTt'.</param>
        ''' <returns><pre>
        ''' =  0 ... Everything OK.
        ''' =  1 ... Invalid value of 'Where' in structure 'Location'.
        ''' > 10 ... Error code from function 'MakeObject'.
        ''' > 20 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(65)> Function TopoStar(ByVal JdTt As Double, _
                                          ByVal DeltaT As Double, _
                                          ByVal Star As CatEntry3, _
                                          ByVal Position As OnSurface, _
                                          ByVal Accuracy As Accuracy, _
                                          ByRef Ra As Double, _
                                          ByRef Dec As Double) As Short

        ''' <summary>
        '''  To transform a star's catalog quantities for a change of epoch and/or equator and equinox.
        ''' </summary>
        ''' <param name="TransformOption">Transformation option</param>
        ''' <param name="DateInCat">TT Julian date, or year, of input catalog data.</param>
        ''' <param name="InCat">An entry from the input catalog, with units as given in the struct definition </param>
        ''' <param name="DateNewCat">TT Julian date, or year, of transformed catalog data.</param>
        ''' <param name="NewCatId">Three-character abbreviated name of the transformed catalog. </param>
        ''' <param name="NewCat"> The transformed catalog entry, with units as given in the struct definition </param>
        ''' <returns>
        ''' <pre>
        ''' = 0 ... Everything OK.
        ''' = 1 ... Invalid value of an input date for option 2 or 3 (see Note 1 below).
        ''' </pre></returns>
        ''' <remarks>Also used to rotate catalog quantities on the dynamical equator and equinox of J2000.0 to the ICRS or vice versa.
        ''' <para>1. 'DateInCat' and 'DateNewCat' may be specified either as a Julian date (e.g., 2433282.5) or 
        ''' a Julian year and fraction (e.g., 1950.0).  Values less than 10000 are assumed to be years. 
        ''' For 'TransformOption' = 2 or 'TransformOption' = 3, either 'DateInCat' or 'DateNewCat' must be 2451545.0 or 
        ''' 2000.0 (J2000.0).  For 'TransformOption' = 4 and 'TransformOption' = 5, 'DateInCat' and 'DateNewCat' are ignored.</para>
        ''' <para>2. 'TransformOption' = 1 updates the star's data to account for the star's space motion between the first 
        ''' and second dates, within a fixed reference frame. 'TransformOption' = 2 applies a rotation of the reference 
        ''' frame corresponding to precession between the first and second dates, but leaves the star fixed in 
        ''' space. 'TransformOption' = 3 provides both transformations. 'TransformOption' = 4 and 'TransformOption' = 5 provide a a 
        ''' fixed rotation about very small angles (<![CDATA[<]]>0.1 arcsecond) to take data from the dynamical system 
        ''' of J2000.0 to the ICRS ('TransformOption' = 4) or vice versa ('TransformOption' = 5).</para>
        '''<para>3. For 'TransformOption' = 1, input data can be in any fixed reference system. for 'TransformOption' = 2 or 
        ''' 'TransformOption' = 3, this function assumes the input data is in the dynamical system and produces output 
        ''' in the dynamical system.  for 'TransformOption' = 4, the input data must be on the dynamical equator and 
        ''' equinox of J2000.0.  for 'TransformOption' = 5, the input data must be in the ICRS.</para>
        '''<para>4. This function cannot be properly used to bring data from old star catalogs into the 
        ''' modern system, because old catalogs were compiled using a set of constants that are incompatible 
        ''' with modern values.  In particular, it should not be used for catalogs whose positions and 
        ''' proper motions were derived by assuming a precession constant significantly different from 
        ''' the value implicit in function 'precession'.</para></remarks>
        <DispId(66)> Function TransformCat(ByVal TransformOption As TransformationOption3, _
                                            ByVal DateInCat As Double, _
                                            ByVal InCat As CatEntry3, _
                                            ByVal DateNewCat As Double, _
                                            ByVal NewCatId As String, _
                                            ByRef NewCat As CatEntry3) As Short

        ''' <summary>
        ''' Convert Hipparcos catalog data at epoch J1991.25 to epoch J2000.0, for use within NOVAS.
        ''' </summary>
        ''' <param name="Hipparcos">An entry from the Hipparcos catalog, at epoch J1991.25, with all members 
        ''' having Hipparcos catalog units.  See Note 1 below </param>
        ''' <param name="Hip2000">The transformed input entry, at epoch J2000.0.  See Note 2 below</param>
        ''' <remarks>To be used only for Hipparcos or Tycho stars with linear space motion.  Both input and 
        ''' output data is in the ICRS.
        ''' <para>
        ''' 1. Input (Hipparcos catalog) epoch and units:
        ''' <list type="bullet">
        ''' <item>Epoch: J1991.25</item>
        ''' <item>Right ascension (RA): degrees</item>
        ''' <item>Declination (Dec): degrees</item>
        ''' <item>Proper motion in RA: milliarcseconds per year</item>
        ''' <item>Proper motion in Dec: milliarcseconds per year</item>
        ''' <item>Parallax: milliarcseconds</item>
        ''' <item>Radial velocity: kilometers per second (not in catalog)</item>
        ''' </list>
        ''' </para>
        ''' <para>
        ''' 2. Output (modified Hipparcos) epoch and units:
        ''' <list type="bullet">
        ''' <item>Epoch: J2000.0</item>
        ''' <item>Right ascension: hours</item>
        ''' <item>Declination: degrees</item>
        ''' <item>Proper motion in RA: milliarcseconds per year</item>
        ''' <item>Proper motion in Dec: milliarcseconds per year</item>
        ''' <item>Parallax: milliarcseconds</item>
        ''' <item>Radial velocity: kilometers per second</item>
        ''' </list>>
        ''' </para>
        ''' </remarks>
        <DispId(67)> Sub TransformHip(ByVal Hipparcos As CatEntry3, _
                                          ByRef Hip2000 As CatEntry3)

        ''' <summary>
        ''' Converts a vector in equatorial rectangular coordinates to equatorial spherical coordinates.
        ''' </summary>
        ''' <param name="Pos">Position vector, equatorial rectangular coordinates.</param>
        ''' <param name="Ra">Right ascension in hours.</param>
        ''' <param name="Dec">Declination in degrees.</param>
        ''' <returns>
        ''' <pre>
        ''' = 0 ... Everything OK.
        ''' = 1 ... All vector components are zero; 'Ra' and 'Dec' are indeterminate.
        ''' = 2 ... Both Pos[0] and Pos[1] are zero, but Pos[2] is nonzero; 'Ra' is indeterminate.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(68)> Function Vector2RaDec(ByVal Pos() As Double, _
                                              ByRef Ra As Double, _
                                              ByRef Dec As Double) As Short

        ''' <summary>
        ''' Compute the virtual place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for virtual place.</param>
        ''' <param name="SsBody">structure containing the body designation for the solar system body(</param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Virtual right ascension in hours, referred to the GCRS.</param>
        ''' <param name="Dec">Virtual declination in degrees, referred to the GCRS.</param>
        ''' <param name="Dis">True distance from Earth to planet in AU.</param>
        ''' <returns>
        ''' <pre>
        ''' =  0 ... Everything OK.
        ''' =  1 ... Invalid value of 'Type' in structure 'SsBody'.
        ''' > 10 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(69)> Function VirtualPlanet(ByVal JdTt As Double, _
                                              ByVal SsBody As Object3, _
                                              ByVal Accuracy As Accuracy, _
                                              ByRef Ra As Double, _
                                              ByRef Dec As Double, _
                                              ByRef Dis As Double) As Short

        ''' <summary>
        ''' Computes the virtual place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for virtual place.</param>
        ''' <param name="Star">catalog entry structure containing catalog data for the object in the ICRS</param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Virtual right ascension in hours, referred to the GCRS.</param>
        ''' <param name="Dec">Virtual declination in degrees, referred to the GCRS.</param>
        ''' <returns>
        ''' <pre>
        ''' =  0 ... Everything OK.
        ''' > 10 ... Error code from function 'MakeObject'.
        ''' > 20 ... Error code from function 'Place'
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(70)> Function VirtualStar(ByVal JdTt As Double, _
                                             ByVal Star As CatEntry3, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Ra As Double, _
                                             ByRef Dec As Double) As Short

        ''' <summary>
        ''' Corrects a vector in the ITRF (rotating Earth-fixed system) for polar motion, and also corrects 
        ''' the longitude origin (by a tiny amount) to the Terrestrial Intermediate Origin (TIO).
        ''' </summary>
        ''' <param name="Tjd">TT or UT1 Julian date.</param>
        ''' <param name="x">Conventionally-defined X coordinate of Celestial Intermediate Pole with 
        ''' respect to ITRF pole, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of Celestial Intermediate Pole with 
        ''' respect to ITRF pole, in arcseconds.</param>
        ''' <param name="Pos1">Position vector, geocentric equatorial rectangular coordinates, 
        ''' referred to ITRF axes.</param>
        ''' <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, 
        ''' referred to true equator and TIO.</param>
        ''' <remarks></remarks>
        <DispId(71)> Sub Wobble(ByVal Tjd As Double, _
                                 ByVal x As Double, _
                                 ByVal y As Double, _
                                 ByVal Pos1() As Double, _
                                 ByRef Pos2() As Double)

        ''' <summary>
        ''' Return the value of DeltaT for the given Julian date
        ''' </summary>
        ''' <param name="Tjd">Julian date for which the delta T value is required</param>
        ''' <returns>Double value of DeltaT (seconds)</returns>
        ''' <remarks>Valid between the years 1650 and 2050</remarks>
        <DispId(72)> Function DeltaT(ByVal Tjd As Double) As Double
    End Interface
End Namespace
#End Region

#Region "NOVAS31 Interface"
Namespace NOVAS

    ''' <summary>
    ''' Interface to the NOVAS31 component
    ''' </summary>
    ''' <remarks>Implemented by the NOVAS31 component</remarks>
    <Guid("A9C1E5CF-2AA4-404D-B16A-79F5C8B1062F"), ComVisible(True)> _
    Public Interface INOVAS31

        ' PlanetEphemeris, ReadEph, SolarSystem and State relate to reading ephemeris values

        ''' <summary>
        ''' Get position and velocity of target with respect to the centre object. 
        ''' </summary>
        ''' <param name="Tjd"> Two-element array containing the Julian date, which may be split any way (although the first 
        ''' element is usually the "integer" part, and the second element is the "fractional" part).  Julian date is in the 
        ''' TDB or "T_eph" time scale.</param>
        ''' <param name="Target">Target object</param>
        ''' <param name="Center">Centre object</param>
        ''' <param name="Position">Position vector array of target relative to center, measured in AU.</param>
        ''' <param name="Velocity">Velocity vector array of target relative to center, measured in AU/day.</param>
        ''' <returns><pre>
        ''' 0   ...everything OK.
        ''' 1,2 ...error returned from State.</pre>
        ''' </returns>
        ''' <remarks>This function accesses the JPL planetary ephemeris to give the position and velocity of the target 
        ''' object with respect to the center object.</remarks>
        <DispId(1)> Function PlanetEphemeris(ByRef Tjd() As Double, _
                                                     ByVal Target As Target, _
                                                     ByVal Center As Target, _
                                                     ByRef Position() As Double, _
                                                     ByRef Velocity() As Double) As Short

        ''' <summary>
        ''' Read object ephemeris
        ''' </summary>
        ''' <param name="Mp">The number of the asteroid for which the position in desired.</param>
        ''' <param name="Name">The name of the asteroid.</param>
        ''' <param name="Jd"> The Julian date on which to find the position and velocity.</param>
        ''' <param name="Err">Error code; always set equal to 9 (see note below).</param>
        ''' <returns> 6-element array of double cotaining position and velocity vector values, with all elements set to zero.</returns>
        ''' <remarks> This is a dummy version of function 'ReadEph'.  It serves as a stub for the "real" 'ReadEph' 
        ''' (part of the USNO/AE98 minor planet ephemerides) when NOVAS-C is used without the minor planet ephemerides.
        ''' <para>
        '''  This dummy function is not intended to be called.  It merely serves as a stub for the "real" 'ReadEph' 
        ''' when NOVAS-C is used without the minor planet ephemerides.  If this function is called, an error of 9 will be returned.
        ''' </para>
        ''' </remarks>
        <DispId(2)> Function ReadEph(ByVal Mp As Integer, _
                                             ByVal Name As String, _
                                             ByVal Jd As Double, _
                                             ByRef Err As Integer) As Double()

        ''' <summary>
        ''' Interface between the JPL direct-access solar system ephemerides and NOVAS-C.
        ''' </summary>
        ''' <param name="Tjd">Julian date of the desired time, on the TDB time scale.</param>
        ''' <param name="Body">Body identification number for the solar system object of interest; 
        ''' Mercury = 1, ..., Pluto= 9, Sun= 10, Moon = 11.</param>
        ''' <param name="Origin">Origin code; solar system barycenter= 0, center of mass of the Sun = 1, center of Earth = 2.</param>
        ''' <param name="Pos">Position vector of 'body' at tjd; equatorial rectangular coordinates in AU referred to the ICRS.</param>
        ''' <param name="Vel">Velocity vector of 'body' at tjd; equatorial rectangular system referred to the ICRS.</param>
        ''' <returns>Always returns 0</returns>
        ''' <remarks></remarks>
        <DispId(3)> Function SolarSystem(ByVal Tjd As Double, _
                                                 ByVal Body As Body, _
                                                 ByVal Origin As Origin, _
                                                 ByRef Pos() As Double, _
                                                 ByRef Vel() As Double) As Short

        ''' <summary>
        '''  Read and interpolate the JPL planetary ephemeris file.
        ''' </summary>
        ''' <param name="Jed">2-element Julian date (TDB) at which interpolation is wanted. Any combination of jed[0]+jed[1] which falls within the time span on the file is a permissible epoch.  See Note 1 below.</param>
        ''' <param name="Target">The requested body to get data for from the ephemeris file.</param>
        ''' <param name="TargetPos">The barycentric position vector array of the requested object, in AU. (If target object is the Moon, then the vector is geocentric.)</param>
        ''' <param name="TargetVel">The barycentric velocity vector array of the requested object, in AU/Day.</param>
        ''' <returns>
        ''' <pre>
        ''' 0 ...everything OK
        ''' 1 ...error reading ephemeris file
        ''' 2 ...epoch out of range.
        ''' </pre></returns>
        ''' <remarks>
        ''' The target number designation of the astronomical bodies is:
        ''' <pre>
        '''         = 0: Mercury,               1: Venus, 
        '''         = 2: Earth-Moon barycenter, 3: Mars, 
        '''         = 4: Jupiter,               5: Saturn, 
        '''         = 6: Uranus,                7: Neptune, 
        '''         = 8: Pluto,                 9: geocentric Moon, 
        '''         =10: Sun.
        ''' </pre>
        ''' <para>
        '''  NOTE 1. For ease in programming, the user may put the entire epoch in jed[0] and set jed[1] = 0. 
        ''' For maximum interpolation accuracy,  set jed[0] = the most recent midnight at or before interpolation epoch, 
        ''' and set jed[1] = fractional part of a day elapsed between jed[0] and epoch. As an alternative, it may prove 
        ''' convenient to set jed[0] = some fixed epoch, such as start of the integration and jed[1] = elapsed interval 
        ''' between then and epoch.
        ''' </para>
        ''' </remarks>
        <DispId(4)> Function State(ByRef Jed() As Double, _
                                           ByVal Target As Target, _
                                           ByRef TargetPos() As Double, _
                                           ByRef TargetVel() As Double) As Short

        ' The following methods come from NOVAS3

        ''' <summary>
        '''  Corrects position vector for aberration of light.  Algorithm includes relativistic terms.
        ''' </summary>
        ''' <param name="Pos"> Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="Vel"> Velocity vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU/day.</param>
        ''' <param name="LightTime"> Light time from object to Earth in days.</param>
        ''' <param name="Pos2"> Position vector, referred to origin at center of mass of the Earth, corrected for aberration, components in AU</param>
        ''' <remarks>If 'lighttime' = 0 on input, this function will compute it.</remarks>
        <DispId(5)> Sub Aberration(ByVal Pos() As Double, _
                                            ByVal Vel() As Double, _
                                            ByVal LightTime As Double, _
                                            ByRef Pos2() As Double)

        ''' <summary>
        ''' Compute the apparent place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="JdTt"> TT Julian date for apparent place.</param>
        ''' <param name="SsBody"> Pointer to structure containing the body designation for the solar system body </param>
        ''' <param name="Accuracy"> Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        ''' <param name="Dec"> Apparent declination in degrees, referred to true equator and equinox of date.</param>
        ''' <param name="Dis"> True distance from Earth to planet at 'JdTt' in AU.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Type' in structure 'SsBody'
        ''' > 10 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(6)> Function AppPlanet(ByVal JdTt As Double, _
                                               ByVal SsBody As Object3, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double, _
                                               ByRef Dis As Double) As Short

        ''' <summary>
        '''  Computes the apparent place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for apparent place.</param>
        ''' <param name="Star">Catalog entry structure containing catalog data forthe object in the ICRS </param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date 'JdTt'.</param>
        ''' <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date 'JdTt'.</param>
        ''' <returns>
        ''' <pre>
        '''    0 ... Everything OK
        ''' > 10 ... Error code from function 'MakeObject'
        ''' > 20 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(7)> Function AppStar(ByVal JdTt As Double, _
                                             ByVal Star As CatEntry3, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Ra As Double, _
                                             ByRef Dec As Double) As Short

        ''' <summary>
        ''' Compute the astrometric place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for astrometric place.</param>
        ''' <param name="SsBody">structure containing the body designation for the solar system body </param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Astrometric right ascension in hours (referred to the ICRS, without light deflection or aberration).</param>
        ''' <param name="Dec">Astrometric declination in degrees (referred to the ICRS, without light deflection or aberration).</param>
        ''' <param name="Dis">True distance from Earth to planet in AU.</param>
        ''' <returns>
        ''' <pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Type' in structure 'SsBody'
        ''' > 10 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(8)> Function AstroPlanet(ByVal JdTt As Double, _
                                                 ByVal SsBody As Object3, _
                                                 ByVal Accuracy As Accuracy, _
                                                 ByRef Ra As Double, _
                                                 ByRef Dec As Double, _
                                                 ByRef Dis As Double) As Short

        ''' <summary>
        '''  Computes the astrometric place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for astrometric place.</param>
        ''' <param name="Star">Catalog entry structure containing catalog data for the object in the ICRS</param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Astrometric right ascension in hours (referred to the ICRS, without light deflection or aberration).</param>
        ''' <param name="Dec">Astrometric declination in degrees (referred to the ICRS, without light deflection or aberration).</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        ''' > 10 ... Error code from function 'MakeObject'
        ''' > 20 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(9)> Function AstroStar(ByVal JdTt As Double, _
                                               ByVal Star As CatEntry3, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double) As Short

        ''' <summary>
        ''' Move the origin of coordinates from the barycenter of the solar system to the observer (or the geocenter); i.e., this function accounts for parallax (annual+geocentric or justannual).
        ''' </summary>
        ''' <param name="Pos">Position vector, referred to origin at solar system barycenter, components in AU.</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar system barycenter, components in AU.</param>
        ''' <param name="Pos2"> Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="Lighttime">Light time from object to Earth in days.</param>
        ''' <remarks></remarks>
        <DispId(10)> Sub Bary2Obs(ByVal Pos() As Double, _
                                          ByVal PosObs() As Double, _
                                          ByRef Pos2() As Double, _
                                          ByRef Lighttime As Double)

        ''' <summary>
        ''' This function will compute a date on the Gregorian calendar given the Julian date.
        ''' </summary>
        ''' <param name="Tjd">Julian date.</param>
        ''' <param name="Year">Year</param>
        ''' <param name="Month">Month number</param>
        ''' <param name="Day">day number</param>
        ''' <param name="Hour">Fractional hour of the day</param>
        ''' <remarks></remarks>
        <DispId(11)> Sub CalDate(ByVal Tjd As Double, _
                                         ByRef Year As Short, _
                                         ByRef Month As Short, _
                                         ByRef Day As Short, _
                                         ByRef Hour As Double)

        ''' <summary>
        ''' This function rotates a vector from the celestial to the terrestrial system.  Specifically, it transforms a vector in the
        ''' GCRS (a local space-fixed system) to the ITRS (a rotating earth-fixed system) by applying rotations for the GCRS-to-dynamical
        ''' frame tie, precession, nutation, Earth rotation, and polar motion.
        ''' </summary>
        ''' <param name="JdHigh">High-order part of UT1 Julian date.</param>
        ''' <param name="JdLow">Low-order part of UT1 Julian date.</param>
        ''' <param name="DeltaT">Value of Delta T (= TT - UT1) at the input UT1 Julian date.</param>
        ''' <param name="Method"> Selection for method: 0 ... CIO-based method; 1 ... equinox-based method</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="OutputOption">0 ... The output vector is referred to GCRS axes; 1 ... The output 
        ''' vector is produced with respect to the equator and equinox of date. (See note 2 below)</param>
        ''' <param name="x">Conventionally-defined X coordinate of celestial intermediate pole with respect to 
        ''' ITRS pole, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of celestial intermediate pole with respect to 
        ''' ITRS pole, in arcseconds.</param>
        ''' <param name="VecT">Position vector, geocentric equatorial rectangular coordinates,
        ''' referred to GCRS axes (celestial system) or with respect to
        ''' the equator and equinox of date, depending on 'option'.</param>
        ''' <param name="VecC">Position vector, geocentric equatorial rectangular coordinates,
        ''' referred to ITRS axes (terrestrial system).</param>
        ''' <returns><pre>
        '''    0 ... everything is ok
        '''    1 ... invalid value of 'Accuracy'
        '''    2 ... invalid value of 'Method'
        ''' > 10 ... 10 + error from function 'CioLocation'
        ''' > 20 ... 20 + error from function 'CioBasis'
        ''' </pre></returns>
        ''' <remarks>Note 1: 'x' = 'y' = 0 means no polar motion transformation.
        ''' <para>
        ''' Note2: 'option' = 1 only works for the equinox-based method.
        '''</para></remarks>
        <DispId(12)> Function Cel2Ter(ByVal JdHigh As Double, _
                                        ByVal JdLow As Double, _
                                        ByVal DeltaT As Double, _
                                        ByVal Method As Method, _
                                        ByVal Accuracy As Accuracy, _
                                        ByVal OutputOption As OutputVectorOption, _
                                        ByVal x As Double, _
                                        ByVal y As Double, _
                                        ByVal VecT() As Double, _
                                        ByRef VecC() As Double) As Short


        ''' <summary>
        '''  This function allows for the specification of celestial pole offsets for high-precision applications.  Each set of offsets is a correction to the modeled position of the pole for a specific date, derived from observations and published by the IERS.
        ''' </summary>
        ''' <param name="Tjd">TDB or TT Julian date for pole offsets.</param>
        ''' <param name="Type"> Type of pole offset. 1 for corrections to angular coordinates of modeled pole referred to mean ecliptic of date, that is, delta-delta-psi and delta-delta-epsilon.  2 for corrections to components of modeled pole unit vector referred to GCRS axes, that is, dx and dy.</param>
        ''' <param name="Dpole1">Value of celestial pole offset in first coordinate, (delta-delta-psi or dx) in milliarcseconds.</param>
        ''' <param name="Dpole2">Value of celestial pole offset in second coordinate, (delta-delta-epsilon or dy) in milliarcseconds.</param>
        ''' <returns><pre>
        ''' 0 ... Everything OK
        ''' 1 ... Invalid value of 'Type'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(13)> Function CelPole(ByVal Tjd As Double, _
                                              ByVal Type As PoleOffsetCorrection, _
                                              ByVal Dpole1 As Double, _
                                              ByVal Dpole2 As Double) As Short

        ''' <summary>
        ''' Calaculate an array of CIO RA values around a given date
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date.</param>
        ''' <param name="NPts"> Number of Julian dates and right ascension values requested (not less than 2 or more than 20).</param>
        ''' <param name="Cio"> An arraylist of RaOfCIO structures containing a time series of the right ascension of the 
        ''' Celestial Intermediate Origin (CIO) with respect to the GCRS.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... error opening the 'cio_ra.bin' file
        ''' 2 ... 'JdTdb' not in the range of the CIO file; 
        ''' 3 ... 'NPts' out of range
        ''' 4 ... unable to allocate memory for the internal 't' array; 
        ''' 5 ... unable to allocate memory for the internal 'ra' array; 
        ''' 6 ... 'JdTdb' is too close to either end of the CIO file; unable to put 'NPts' data points into the output object.
        ''' </pre></returns>
        ''' <remarks>
        ''' <para>
        ''' Given an input TDB Julian date and the number of data points desired, this function returns a set of 
        ''' Julian dates and corresponding values of the GCRS right ascension of the celestial intermediate origin (CIO).  
        ''' The range of dates is centered (at least approximately) on the requested date.  The function obtains 
        ''' the data from an external data file.</para>
        ''' <example>How to create and retrieve values from the arraylist
        ''' <code>
        ''' Dim CioList As New ArrayList, Nov3 As New ASCOM.Astrometry.NOVAS3
        ''' 
        ''' rc = Nov3.CioArray(2455251.5, 20, CioList) ' Get 20 values around date 00:00:00 February 24th 2010
        ''' MsgBox("Nov3 RC= " <![CDATA[&]]>  rc)
        ''' 
        ''' For Each CioA As ASCOM.Astrometry.RAOfCio In CioList
        '''     MsgBox("CIO Array " <![CDATA[&]]> CioA.JdTdb <![CDATA[&]]> " " <![CDATA[&]]> CioA.RACio)
        ''' Next
        ''' </code>
        ''' </example>
        ''' </remarks>
        <DispId(14)> Function CioArray(ByVal JdTdb As Double, _
                                                ByVal NPts As Integer, _
                                                ByRef Cio As ArrayList) As Short

        ''' <summary>
        ''' Compute the orthonormal basis vectors of the celestial intermediate system.
        ''' </summary>
        ''' <param name="JdTdbEquionx">TDB Julian date of epoch.</param>
        ''' <param name="RaCioEquionx">Right ascension of the CIO at epoch (hours).</param>
        ''' <param name="RefSys">Reference system in which right ascension is given. 1 ... GCRS; 2 ... True equator and equinox of date.</param>
        ''' <param name="Accuracy">Accuracy</param>
        ''' <param name="x">Unit vector toward the CIO, equatorial rectangular coordinates, referred to the GCRS.</param>
        ''' <param name="y">Unit vector toward the y-direction, equatorial rectangular coordinates, referred to the GCRS.</param>
        ''' <param name="z">Unit vector toward north celestial pole (CIP), equatorial rectangular coordinates, referred to the GCRS.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of input variable 'RefSys'.
        ''' </pre></returns>
        ''' <remarks>
        ''' To compute the orthonormal basis vectors, with respect to the GCRS (geocentric ICRS), of the celestial 
        ''' intermediate system defined by the celestial intermediate pole (CIP) (in the z direction) and 
        ''' the celestial intermediate origin (CIO) (in the x direction).  A TDB Julian date and the 
        ''' right ascension of the CIO at that date is required as input.  The right ascension of the CIO 
        ''' can be with respect to either the GCRS origin or the true equinox of date -- different algorithms 
        ''' are used in the two cases.</remarks>
        <DispId(15)> Function CioBasis(ByVal JdTdbEquionx As Double, _
                                               ByVal RaCioEquionx As Double, _
                                               ByVal RefSys As ReferenceSystem, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef x As Double, _
                                               ByRef y As Double, _
                                               ByRef z As Double) As Short

        ''' <summary>
        ''' Returns the location of the celestial intermediate origin (CIO) for a given Julian date, as a right ascension 
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="RaCio">Right ascension of the CIO, in hours.</param>
        ''' <param name="RefSys">Reference system in which right ascension is given</param>
        ''' <returns><pre>
        '''    0 ... everything OK
        '''    1 ... unable to allocate memory for the 'cio' array
        ''' > 10 ... 10 + the error code from function 'CioArray'.
        ''' </pre></returns>
        ''' <remarks>  This function returns the location of the celestial intermediate origin (CIO) for a given Julian date, as a right ascension with respect to either the GCRS (geocentric ICRS) origin or the true equinox of date.  The CIO is always located on the true equator (= intermediate equator) of date.</remarks>
        <DispId(16)> Function CioLocation(ByVal JdTdb As Double, _
                                                  ByVal Accuracy As Accuracy, _
                                                  ByRef RaCio As Double, _
                                                  ByRef RefSys As ReferenceSystem) As Short

        ''' <summary>
        ''' Computes the true right ascension of the celestial intermediate origin (CIO) at a given TT Julian date.  This is -(equation of the origins).
        ''' </summary>
        ''' <param name="JdTt">TT Julian date</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="RaCio"> Right ascension of the CIO, with respect to the true equinox of date, in hours (+ or -).</param>
        ''' <returns>
        ''' <pre>
        '''   0  ... everything OK
        '''   1  ... invalid value of 'Accuracy'
        ''' > 10 ... 10 + the error code from function 'CioLocation'
        ''' > 20 ... 20 + the error code from function 'CioBasis'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(17)> Function CioRa(ByVal JdTt As Double, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef RaCio As Double) As Short

        ''' <summary>
        ''' Returns the difference in light-time, for a star, between the barycenter of the solar system and the observer (or the geocenter).
        ''' </summary>
        ''' <param name="Pos1">Position vector of star, with respect to origin at solar system barycenter.</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar system barycenter, components in AU.</param>
        ''' <returns>Difference in light time, in the sense star to barycenter minus star to earth, in days.</returns>
        ''' <remarks>
        ''' Alternatively, this function returns the light-time from the observer (or the geocenter) to a point on a 
        ''' light ray that is closest to a specific solar system body.  For this purpose, 'Pos1' is the position 
        ''' vector toward observed object, with respect to origin at observer (or the geocenter); 'PosObs' is 
        ''' the position vector of solar system body, with respect to origin at observer (or the geocenter), 
        ''' components in AU; and the returned value is the light time to point on line defined by 'Pos1' 
        ''' that is closest to solar system body (positive if light passes body before hitting observer, i.e., if 
        ''' 'Pos1' is within 90 degrees of 'PosObs').
        ''' </remarks>
        <DispId(18)> Function DLight(ByVal Pos1() As Double, _
                                             ByVal PosObs() As Double) As Double

        ''' <summary>
        ''' Converts an ecliptic position vector to an equatorial position vector.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for coordinates.</param>
        ''' <param name="CoordSys">Coordinate system selection. 0 ... mean equator and equinox of date; 1 ... true equator and equinox of date; 2 ... ICRS</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos1"> Position vector, referred to specified ecliptic and equinox of date.  If 'CoordSys' = 2, 'pos1' must be on mean ecliptic and equinox of J2000.0; see Note 1 below.</param>
        ''' <param name="Pos2">Position vector, referred to specified equator and equinox of date.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'CoordSys'
        ''' </pre></returns>
        ''' <remarks>
        ''' To convert an ecliptic vector (mean ecliptic and equinox of J2000.0 only) to an ICRS vector, 
        ''' set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. 
        ''' Except for the output from this case, all vectors are assumed to be with respect to a dynamical system.
        ''' </remarks>
        <DispId(19)> Function Ecl2EquVec(ByVal JdTt As Double, _
                                                 ByVal CoordSys As CoordSys, _
                                                 ByVal Accuracy As Accuracy, _
                                                 ByVal Pos1() As Double, _
                                                 ByRef Pos2() As Double) As Short

        ''' <summary>
        '''  Compute the "complementary terms" of the equation of the equinoxes consistent with IAU 2000 resolutions.
        ''' </summary>
        ''' <param name="JdHigh">High-order part of TT Julian date.</param>
        ''' <param name="JdLow">Low-order part of TT Julian date.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <returns>Complementary terms, in radians.</returns>
        ''' <remarks>
        ''' 1. The series used in this function was derived from Series from IERS Conventions (2003), Chapter 5, Table 5.2C.
        ''' This same series was also adopted for use in the IAU's Standards of Fundamental Astronomy (SOFA) software (i.e., subroutine 
        ''' eect00.for and function eect00.c).
        ''' <para>2. The low-accuracy series used in this function is a simple implementation derived from the first reference, in which terms
        ''' smaller than 2 microarcseconds have been omitted.</para>
        ''' <para>3. This function is based on NOVAS Fortran routine 'eect2000', with the low-accuracy formula taken from NOVAS Fortran routine 'etilt'.</para>
        ''' </remarks>
        <DispId(20)> Function EeCt(ByVal JdHigh As Double, _
                                           ByVal JdLow As Double, _
                                           ByVal Accuracy As Accuracy) As Double

        ''' <summary>
        '''  Retrieves the position and velocity of a solar system body from a fundamental ephemeris.
        ''' </summary>
        ''' <param name="Jd"> TDB Julian date split into two parts, where the sum jd[0] + jd[1] is the TDB Julian date.</param>
        ''' <param name="CelObj">Structure containing the designation of the body of interest </param>
        ''' <param name="Origin"> Origin code; solar system barycenter = 0, center of mass of the Sun = 1.</param>
        ''' <param name="Accuracy">Slection for accuracy</param>
        ''' <param name="Pos">Position vector of the body at 'Jd'; equatorial rectangular coordinates in AU referred to the ICRS.</param>
        ''' <param name="Vel">Velocity vector of the body at 'Jd'; equatorial rectangular system referred to the mean equator and equinox of the ICRS, in AU/Day.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Origin'
        '''    2 ... Invalid value of 'Type' in 'CelObj'; 
        '''    3 ... Unable to allocate memory
        ''' 10+n ... where n is the error code from 'SolarSystem'; 
        ''' 20+n ... where n is the error code from 'ReadEph'.
        ''' </pre></returns>
        ''' <remarks>It is recommended that the input structure 'cel_obj' be created using function 'MakeObject' in file novas.c.</remarks>
        <DispId(21)> Function Ephemeris(ByVal Jd() As Double, _
                                                ByVal CelObj As Object3, _
                                                ByVal Origin As Origin, _
                                                ByVal Accuracy As Accuracy, _
                                                ByRef Pos() As Double, _
                                                ByRef Vel() As Double) As Short

        ''' <summary>
        ''' To convert right ascension and declination to ecliptic longitude and latitude.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for coordinates.</param>
        ''' <param name="CoordSys"> Coordinate system: 0 ... mean equator and equinox of date 'JdTt'; 1 ... true equator and equinox of date 'JdTt'; 2 ... ICRS</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Ra">Right ascension in hours, referred to specified equator and equinox of date.</param>
        ''' <param name="Dec">Declination in degrees, referred to specified equator and equinox of date.</param>
        ''' <param name="ELon">Ecliptic longitude in degrees, referred to specified ecliptic and equinox of date.</param>
        ''' <param name="ELat">Ecliptic latitude in degrees, referred to specified ecliptic and equinox of date.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'CoordSys'
        ''' </pre></returns>
        ''' <remarks>
        ''' To convert ICRS RA and dec to ecliptic coordinates (mean ecliptic and equinox of J2000.0), 
        ''' set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. 
        ''' Except for the input to this case, all input coordinates are dynamical.
        ''' </remarks>
        <DispId(22)> Function Equ2Ecl(ByVal JdTt As Double, _
                                              ByVal CoordSys As CoordSys, _
                                              ByVal Accuracy As Accuracy, _
                                              ByVal Ra As Double, _
                                              ByVal Dec As Double, _
                                              ByRef ELon As Double, _
                                              ByRef ELat As Double) As Short

        ''' <summary>
        ''' Converts an equatorial position vector to an ecliptic position vector.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for</param>
        ''' <param name="CoordSys"> Coordinate system selection. 0 ... mean equator and equinox of date 'JdTt'; 1 ... true equator and equinox of date 'JdTt'; 2 ... ICRS</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos1">Position vector, referred to specified equator and equinox of date.</param>
        ''' <param name="Pos2">Position vector, referred to specified ecliptic and equinox of date.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'CoordSys'
        ''' </pre></returns>
        ''' <remarks>To convert an ICRS vector to an ecliptic vector (mean ecliptic and equinox of J2000.0 only), 
        ''' set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. Except for 
        ''' the input to this case, all vectors are assumed to be with respect to a dynamical system.</remarks>
        <DispId(23)> Function Equ2EclVec(ByVal JdTt As Double, _
                                                 ByVal CoordSys As CoordSys, _
                                                 ByVal Accuracy As Accuracy, _
                                                 ByVal Pos1() As Double, _
                                                 ByRef Pos2() As Double) As Short

        ''' <summary>
        ''' Converts ICRS right ascension and declination to galactic longitude and latitude.
        ''' </summary>
        ''' <param name="RaI">ICRS right ascension in hours.</param>
        ''' <param name="DecI">ICRS declination in degrees.</param>
        ''' <param name="GLon">Galactic longitude in degrees.</param>
        ''' <param name="GLat">Galactic latitude in degrees.</param>
        ''' <remarks></remarks>
        <DispId(24)> Sub Equ2Gal(ByVal RaI As Double, _
                                         ByVal DecI As Double, _
                                         ByRef GLon As Double, _
                                         ByRef GLat As Double)

        ''' <summary>
        ''' Transforms topocentric right ascension and declination to zenith distance and azimuth.  
        ''' </summary>
        ''' <param name="Jd_Ut1">UT1 Julian date.</param>
        ''' <param name="DeltT">Difference TT-UT1 at 'jd_ut1', in seconds.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="x">onventionally-defined x coordinate of celestial intermediate pole with respect to ITRS reference pole, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined y coordinate of celestial intermediate pole with respect to ITRS reference pole, in arcseconds.</param>
        ''' <param name="Location">Structure containing observer's location </param>
        ''' <param name="Ra">Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date.</param>
        ''' <param name="Dec">Topocentric declination of object of interest, in degrees, referred to true equator and equinox of date.</param>
        ''' <param name="RefOption">Refraction option. 0 ... no refraction; 1 ... include refraction, using 'standard' atmospheric conditions;
        ''' 2 ... include refraction, using atmospheric parametersinput in the 'Location' structure.</param>
        ''' <param name="Zd">Topocentric zenith distance in degrees, affected by refraction if 'ref_option' is non-zero.</param>
        ''' <param name="Az">Topocentric azimuth (measured east from north) in degrees.</param>
        ''' <param name="RaR"> Topocentric right ascension of object of interest, in hours, referred to true equator and 
        ''' equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        ''' <param name="DecR">Topocentric declination of object of interest, in degrees, referred to true equator and 
        ''' equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        ''' <remarks>This function transforms topocentric right ascension and declination to zenith distance and azimuth.  
        ''' It uses a method that properly accounts for polar motion, which is significant at the sub-arcsecond level.  
        ''' This function can also adjust coordinates for atmospheric refraction.</remarks>
        <DispId(25)> Sub Equ2Hor(ByVal Jd_Ut1 As Double, _
                                         ByVal DeltT As Double, _
                                         ByVal Accuracy As Accuracy, _
                                         ByVal x As Double, _
                                         ByVal y As Double, _
                                         ByVal Location As OnSurface, _
                                         ByVal Ra As Double, _
                                         ByVal Dec As Double, _
                                         ByVal RefOption As RefractionOption, _
                                         ByRef Zd As Double, _
                                         ByRef Az As Double, _
                                         ByRef RaR As Double, _
                                         ByRef DecR As Double)

        ''' <summary>
        ''' Returns the value of the Earth Rotation Angle (theta) for a given UT1 Julian date. 
        ''' </summary>
        ''' <param name="JdHigh">High-order part of UT1 Julian date.</param>
        ''' <param name="JdLow">Low-order part of UT1 Julian date.</param>
        ''' <returns>The Earth Rotation Angle (theta) in degrees.</returns>
        ''' <remarks> The expression used is taken from the note to IAU Resolution B1.8 of 2000.  1. The algorithm used 
        ''' here is equivalent to the canonical theta = 0.7790572732640 + 1.00273781191135448 * t, where t is the time 
        ''' in days from J2000 (t = JdHigh + JdLow - T0), but it avoids many two-PI 'wraps' that 
        ''' decrease precision (adopted from SOFA Fortran routine iau_era00; see also expression at top 
        ''' of page 35 of IERS Conventions (1996)).</remarks>
        <DispId(26)> Function Era(ByVal JdHigh As Double, _
                                          ByVal JdLow As Double) As Double

        ''' <summary>
        ''' Computes quantities related to the orientation of the Earth's rotation axis at Julian date 'JdTdb'.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian Date.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Mobl">Mean obliquity of the ecliptic in degrees at 'JdTdb'.</param>
        ''' <param name="Tobl">True obliquity of the ecliptic in degrees at 'JdTdb'.</param>
        ''' <param name="Ee">Equation of the equinoxes in seconds of time at 'JdTdb'.</param>
        ''' <param name="Dpsi">Nutation in longitude in arcseconds at 'JdTdb'.</param>
        ''' <param name="Deps">Nutation in obliquity in arcseconds at 'JdTdb'.</param>
        ''' <remarks>Values of the celestial pole offsets 'PSI_COR' and 'EPS_COR' are set using function 'cel_pole', 
        ''' if desired.  See the prolog of 'cel_pole' for details.</remarks>
        <DispId(27)> Sub ETilt(ByVal JdTdb As Double, _
                                       ByVal Accuracy As Accuracy, _
                                       ByRef Mobl As Double, _
                                       ByRef Tobl As Double, _
                                       ByRef Ee As Double, _
                                       ByRef Dpsi As Double, _
                                       ByRef Deps As Double)

        ''' <summary>
        '''  To transform a vector from the dynamical reference system to the International Celestial Reference System (ICRS), or vice versa.
        ''' </summary>
        ''' <param name="Pos1">Position vector, equatorial rectangular coordinates.</param>
        ''' <param name="Direction">Set 'direction' <![CDATA[<]]> 0 for dynamical to ICRS transformation. Set 'direction' <![CDATA[>=]]> 0 for 
        ''' ICRS to dynamical transformation.</param>
        ''' <param name="Pos2">Position vector, equatorial rectangular coordinates.</param>
        ''' <remarks></remarks>
        <DispId(28)> Sub FrameTie(ByVal Pos1() As Double, _
                                          ByVal Direction As FrameConversionDirection, _
                                          ByRef Pos2() As Double)

        ''' <summary>
        ''' To compute the fundamental arguments (mean elements) of the Sun and Moon.
        ''' </summary>
        ''' <param name="t">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="a">Double array of fundamental arguments</param>
        ''' <remarks>
        ''' Fundamental arguments, in radians:
        ''' <pre>
        '''   a[0] = l (mean anomaly of the Moon)
        '''   a[1] = l' (mean anomaly of the Sun)
        '''   a[2] = F (mean argument of the latitude of the Moon)
        '''   a[3] = D (mean elongation of the Moon from the Sun)
        '''   a[4] = a[4] (mean longitude of the Moon's ascending node);
        '''                from Simon section 3.4(b.3),
        '''                precession = 5028.8200 arcsec/cy)
        ''' </pre>
        ''' </remarks>
        <DispId(29)> Sub FundArgs(ByVal t As Double, _
                                          ByRef a() As Double)

        ''' <summary>
        ''' Converts GCRS right ascension and declination to coordinates with respect to the equator of date (mean or true).
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of equator to be used for output coordinates.</param>
        ''' <param name="CoordSys"> Coordinate system selection for output coordinates.; 0 ... mean equator and 
        ''' equinox of date; 1 ... true equator and equinox of date; 2 ... true equator and CIO of date</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="RaG">GCRS right ascension in hours.</param>
        ''' <param name="DecG">GCRS declination in degrees.</param>
        ''' <param name="Ra"> Right ascension in hours, referred to specified equator and right ascension origin of date.</param>
        ''' <param name="Dec">Declination in degrees, referred to specified equator of date.</param>
        ''' <returns>
        ''' <pre>
        '''    0 ... everything OK
        ''' >  0 ... error from function 'Vector2RaDec'' 
        ''' > 10 ... 10 + error from function 'CioLocation'
        ''' > 20 ... 20 + error from function 'CioBasis'
        ''' </pre>></returns>
        ''' <remarks>For coordinates with respect to the true equator of date, the origin of right ascension can be either the true equinox or the celestial intermediate origin (CIO).
        ''' <para> This function only supports the CIO-based method.</para></remarks>
        <DispId(30)> Function Gcrs2Equ(ByVal JdTt As Double, _
                                               ByVal CoordSys As CoordSys, _
                                               ByVal Accuracy As Accuracy, _
                                               ByVal RaG As Double, _
                                               ByVal DecG As Double, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double) As Short

        ''' <summary>
        ''' This function computes the geocentric position and velocity of an observer on 
        ''' the surface of the earth or on a near-earth spacecraft.</summary>
        ''' <param name="JdTt">TT Julian date.</param>
        ''' <param name="DeltaT">Value of Delta T (= TT - UT1) at 'JdTt'.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Obs">Data specifying the location of the observer</param>
        ''' <param name="Pos">Position vector of observer, with respect to origin at geocenter, 
        ''' referred to GCRS axes, components in AU.</param>
        ''' <param name="Vel">Velocity vector of observer, with respect to origin at geocenter, 
        ''' referred to GCRS axes, components in AU/day.</param>
        ''' <returns>
        ''' <pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'Accuracy'.
        ''' </pre></returns>
        ''' <remarks>The final vectors are expressed in the GCRS.</remarks>
        <DispId(31)> Function GeoPosVel(ByVal JdTt As Double, _
                                        ByVal DeltaT As Double, _
                                        ByVal Accuracy As Accuracy, _
                                        ByVal Obs As Observer, _
                                        ByRef Pos() As Double, _
                                        ByRef Vel() As Double) As Short

        ''' <summary>
        ''' Computes the total gravitational deflection of light for the observed object due to the major gravitating bodies in the solar system.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date of observation.</param>
        ''' <param name="LocCode">Code for location of observer, determining whether the gravitational deflection due to the earth itself is applied.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos1"> Position vector of observed object, with respect to origin at observer (or the geocenter), 
        ''' referred to ICRS axes, components in AU.</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar 
        ''' system barycenter, referred to ICRS axes, components in AU.</param>
        ''' <param name="Pos2">Position vector of observed object, with respect to origin at observer (or the geocenter), 
        ''' referred to ICRS axes, corrected for gravitational deflection, components in AU.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        ''' <![CDATA[<]]> 30 ... Error from function 'Ephemeris'; 
        ''' > 30 ... Error from function 'MakeObject'.
        ''' </pre></returns>
        ''' <remarks>This function valid for an observed body within the solar system as well as for a star.
        ''' <para>
        ''' If 'Accuracy' is set to zero (full accuracy), three bodies (Sun, Jupiter, and Saturn) are 
        ''' used in the calculation.  If the reduced-accuracy option is set, only the Sun is used in the 
        ''' calculation.  In both cases, if the observer is not at the geocenter, the deflection due to the Earth is included.
        ''' </para>
        ''' </remarks>
        <DispId(32)> Function GravDef(ByVal JdTdb As Double, _
                                              ByVal LocCode As EarthDeflection, _
                                              ByVal Accuracy As Accuracy, _
                                              ByVal Pos1() As Double, _
                                              ByVal PosObs() As Double, _
                                              ByRef Pos2() As Double) As Short

        ''' <summary>
        ''' Corrects position vector for the deflection of light in the gravitational field of an arbitrary body.
        ''' </summary>
        ''' <param name="Pos1">Position vector of observed object, with respect to origin at observer 
        ''' (or the geocenter), components in AU.</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at 
        ''' solar system barycenter, components in AU.</param>
        ''' <param name="PosBody">Position vector of gravitating body, with respect to origin at solar system 
        ''' barycenter, components in AU.</param>
        ''' <param name="RMass">Reciprocal mass of gravitating body in solar mass units, that is, 
        ''' Sun mass / body mass.</param>
        ''' <param name="Pos2">Position vector of observed object, with respect to origin at observer 
        ''' (or the geocenter), corrected for gravitational deflection, components in AU.</param>
        ''' <remarks>This function valid for an observed body within the solar system as well as for a star.</remarks>
        <DispId(33)> Sub GravVec(ByVal Pos1() As Double, _
                                         ByVal PosObs() As Double, _
                                         ByVal PosBody() As Double, _
                                         ByVal RMass As Double, _
                                         ByRef Pos2() As Double)

        ''' <summary>
        ''' Compute the intermediate right ascension of the equinox at the input Julian date
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date.</param>
        ''' <param name="Equinox">Equinox selection flag: mean pr true</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <returns>Intermediate right ascension of the equinox, in hours (+ or -). If 'equinox' = 1 
        ''' (i.e true equinox), then the returned value is the equation of the origins.</returns>
        ''' <remarks></remarks>
        <DispId(34)> Function IraEquinox(ByVal JdTdb As Double, _
                                                ByVal Equinox As EquinoxType, _
                                                ByVal Accuracy As Accuracy) As Double

        ''' <summary>
        ''' Compute the Julian date for a given calendar date (year, month, day, hour).
        ''' </summary>
        ''' <param name="Year">Year number</param>
        ''' <param name="Month">Month number</param>
        ''' <param name="Day">Day number</param>
        ''' <param name="Hour">Fractional hour of the day</param>
        ''' <returns>Computed Julian date.</returns>
        ''' <remarks>This function makes no checks for a valid input calendar date. The input calendar date 
        ''' must be Gregorian. The input time value can be based on any UT-like time scale (UTC, UT1, TT, etc.) 
        ''' - output Julian date will have the same basis.</remarks>
        <DispId(35)> Function JulianDate(ByVal Year As Short, _
                                                 ByVal Month As Short, _
                                                 ByVal Day As Short, _
                                                 ByVal Hour As Double) As Double

        ''' <summary>
        ''' Computes the geocentric position of a solar system body, as antedated for light-time.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date of observation.</param>
        ''' <param name="SsObject">Structure containing the designation for thesolar system body</param>
        ''' <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin 
        ''' at solar system barycenter, referred to ICRS axes, components in AU.</param>
        ''' <param name="TLight0">First approximation to light-time, in days (can be set to 0.0 if unknown)</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos">Position vector of body, with respect to origin at observer (or the geocenter), 
        ''' referred to ICRS axes, components in AU.</param>
        ''' <param name="TLight">Final light-time, in days.</param>
        ''' <returns><pre>
        '''    0 ... everything OK
        '''    1 ... algorithm failed to converge after 10 iterations
        ''' <![CDATA[>]]> 10 ... error is 10 + error from function 'SolarSystem'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(36)> Function LightTime(ByVal JdTdb As Double, _
                                                ByVal SsObject As Object3, _
                                                ByVal PosObs() As Double, _
                                                ByVal TLight0 As Double, _
                                                ByVal Accuracy As Accuracy, _
                                                ByRef Pos() As Double, _
                                                ByRef TLight As Double) As Short

        ''' <summary>
        ''' Determines the angle of an object above or below the Earth's limb (horizon).
        ''' </summary>
        ''' <param name="PosObj">Position vector of observed object, with respect to origin at 
        ''' geocenter, components in AU.</param>
        ''' <param name="PosObs">Position vector of observer, with respect to origin at geocenter, 
        ''' components in AU.</param>
        ''' <param name="LimbAng">Angle of observed object above (+) or below (-) limb in degrees.</param>
        ''' <param name="NadirAng">Nadir angle of observed object as a fraction of apparent radius of limb: <![CDATA[<]]> 1.0 ... 
        ''' below the limb; = 1.0 ... on the limb;  <![CDATA[>]]> 1.0 ... above the limb</param>
        ''' <remarks>The geometric limb is computed, assuming the Earth to be an airless sphere (no 
        ''' refraction or oblateness is included).  The observer can be on or above the Earth.  
        ''' For an observer on the surface of the Earth, this function returns the approximate unrefracted 
        ''' altitude.</remarks>
        <DispId(37)> Sub LimbAngle(ByVal PosObj() As Double, _
                                           ByVal PosObs() As Double, _
                                           ByRef LimbAng As Double, _
                                           ByRef NadirAng As Double)

        ''' <summary>
        ''' Computes the local place of a solar system body.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for local place.</param>
        ''' <param name="SsBody">structure containing the body designation for the solar system body</param>
        ''' <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="Position">Specifies the position of the observer</param>
        ''' <param name="Accuracy">Specifies accuracy level</param>
        ''' <param name="Ra">Local right ascension in hours, referred to the 'local GCRS'.</param>
        ''' <param name="Dec">Local declination in degrees, referred to the 'local GCRS'.</param>
        ''' <param name="Dis">True distance from Earth to planet in AU.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Where' in structure 'Location'; 
        ''' <![CDATA[>]]> 10 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(38)> Function LocalPlanet(ByVal JdTt As Double, _
                                                  ByVal SsBody As Object3, _
                                                  ByVal DeltaT As Double, _
                                                  ByVal Position As OnSurface, _
                                                  ByVal Accuracy As Accuracy, _
                                                  ByRef Ra As Double, _
                                                  ByRef Dec As Double, _
                                                  ByRef Dis As Double) As Short

        ''' <summary>
        '''  Computes the local place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for local place. delta_t (double)</param>
        ''' <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="Star">catalog entry structure containing catalog data for the object in the ICRS</param>
        ''' <param name="Position">Structure specifying the position of the observer </param>
        ''' <param name="Accuracy">Specifies accuracy level.</param>
        ''' <param name="Ra">Local right ascension in hours, referred to the 'local GCRS'.</param>
        ''' <param name="Dec">Local declination in degrees, referred to the 'local GCRS'.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Invalid value of 'Where' in structure 'Location'
        ''' > 10 ... Error code from function 'MakeObject'
        ''' > 20 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(39)> Function LocalStar(ByVal JdTt As Double, _
                                            ByVal DeltaT As Double, _
                                            ByVal Star As CatEntry3, _
                                            ByVal Position As OnSurface, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double) As Short

        ''' <summary>
        ''' Create a structure of type 'cat_entry' containing catalog data for a star or "star-like" object.
        ''' </summary>
        ''' <param name="StarName">Object name (50 characters maximum).</param>
        ''' <param name="Catalog">Three-character catalog identifier (e.g. HIP = Hipparcos, TY2 = Tycho-2)</param>
        ''' <param name="StarNum">Object number in the catalog.</param>
        ''' <param name="Ra">Right ascension of the object (hours).</param>
        ''' <param name="Dec">Declination of the object (degrees).</param>
        ''' <param name="PmRa">Proper motion in right ascension (milliarcseconds/year).</param>
        ''' <param name="PmDec">Proper motion in declination (milliarcseconds/year).</param>
        ''' <param name="Parallax">Parallax (milliarcseconds).</param>
        ''' <param name="RadVel">Radial velocity (kilometers/second).</param>
        ''' <param name="Star">CatEntry3 structure containing the input data</param>
        ''' <remarks></remarks>
        <DispId(40)> Sub MakeCatEntry(ByVal StarName As String, _
                                        ByVal Catalog As String, _
                                        ByVal StarNum As Integer, _
                                        ByVal Ra As Double, _
                                        ByVal Dec As Double, _
                                        ByVal PmRa As Double, _
                                        ByVal PmDec As Double, _
                                        ByVal Parallax As Double, _
                                        ByVal RadVel As Double, _
                                        ByRef Star As CatEntry3)

        ''' <summary>
        ''' Makes a structure of type 'InSpace' - specifying the position and velocity of an observer situated 
        ''' on a near-Earth spacecraft.
        ''' </summary>
        ''' <param name="ScPos">Geocentric position vector (x, y, z) in km.</param>
        ''' <param name="ScVel">Geocentric velocity vector (x_dot, y_dot, z_dot) in km/s.</param>
        ''' <param name="ObsSpace">InSpace structure containing the position and velocity of an observer situated 
        ''' on a near-Earth spacecraft</param>
        ''' <remarks></remarks>
        <DispId(41)> Sub MakeInSpace(ByVal ScPos() As Double, _
                                         ByVal ScVel() As Double, _
                                         ByRef ObsSpace As InSpace)

        ''' <summary>
        ''' Makes a structure of type 'object' - specifying a celestial object - based on the input parameters.
        ''' </summary>
        ''' <param name="Type">Type of object: 0 ... major planet, Sun, or Moon;  1 ... minor planet; 
        ''' 2 ... object located outside the solar system (e.g. star, galaxy, nebula, etc.)</param>
        ''' <param name="Number">Body number: For 'Type' = 0: Mercury = 1,...,Pluto = 9, Sun = 10, Moon = 11; 
        ''' For 'Type' = 1: minor planet numberFor 'Type' = 2: set to 0 (zero)</param>
        ''' <param name="Name">Name of the object (50 characters maximum).</param>
        ''' <param name="StarData">Structure containing basic astrometric data for any celestial object 
        ''' located outside the solar system; the catalog data for a star</param>
        ''' <param name="CelObj">Structure containing the object definition</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... invalid value of 'Type'
        ''' 2 ... 'Number' out of range
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(42)> Function MakeObject(ByVal Type As ObjectType, _
                                               ByVal Number As Short, _
                                               ByVal Name As String, _
                                               ByVal StarData As CatEntry3, _
                                               ByRef CelObj As Object3) As Short

        ''' <summary>
        '''  Makes a structure of type 'observer' - specifying the location of the observer.
        ''' </summary>
        ''' <param name="Where">Integer code specifying location of observer: 0: observer at geocenter; 
        ''' 1: observer on surface of earth; 2: observer on near-earth spacecraft</param>
        ''' <param name="ObsSurface">Structure containing data for an observer's location on the surface 
        ''' of the Earth; used when 'Where' = 1</param>
        ''' <param name="ObsSpace"> Structure containing an observer's location on a near-Earth spacecraft; 
        ''' used when 'Where' = 2 </param>
        ''' <param name="Obs">Structure specifying the location of the observer </param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... input value of 'Where' is out-of-range.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(43)> Function MakeObserver(ByVal Where As ObserverLocation, _
                                              ByVal ObsSurface As OnSurface, _
                                              ByVal ObsSpace As InSpace, _
                                              ByRef Obs As Observer) As Short

        ''' <summary>
        '''  Makes a structure of type 'observer' specifying an observer at the geocenter.
        ''' </summary>
        ''' <param name="ObsAtGeocenter">Structure specifying the location of the observer at the geocenter</param>
        ''' <remarks></remarks>
        <DispId(44)> Sub MakeObserverAtGeocenter(ByRef ObsAtGeocenter As Observer)

        ''' <summary>
        '''  Makes a structure of type 'observer' specifying the position and velocity of an observer 
        ''' situated on a near-Earth spacecraft.
        ''' </summary>
        ''' <param name="ScPos">Geocentric position vector (x, y, z) in km.</param>
        ''' <param name="ScVel">Geocentric position vector (x, y, z) in km.</param>
        ''' <param name="ObsInSpace">Structure containing the position and velocity of an observer 
        ''' situated on a near-Earth spacecraft</param>
        ''' <remarks>Both input vectors are with respect to true equator and equinox of date.</remarks>
        <DispId(45)> Sub MakeObserverInSpace(ByVal ScPos() As Double, _
                                                ByVal ScVel() As Double, _
                                                ByRef ObsInSpace As Observer)

        ''' <summary>
        ''' Makes a structure of type 'observer' specifying the location of and weather for an observer 
        ''' on the surface of the Earth.
        ''' </summary>
        ''' <param name="Latitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        ''' <param name="Longitude">Geodetic (ITRS) longitude in degrees; east positive.</param>
        ''' <param name="Height">Height of the observer (meters).</param>
        ''' <param name="Temperature">Temperature (degrees Celsius).</param>
        ''' <param name="Pressure">Atmospheric pressure (millibars).</param>
        ''' <param name="ObsOnSurface">Structure containing the location of and weather for an observer on 
        ''' the surface of the Earth</param>
        ''' <remarks></remarks>
        <DispId(46)> Sub MakeObserverOnSurface(ByVal Latitude As Double, _
                                                ByVal Longitude As Double, _
                                                ByVal Height As Double, _
                                                ByVal Temperature As Double, _
                                                ByVal Pressure As Double, _
                                                ByRef ObsOnSurface As Observer)

        ''' <summary>
        ''' Makes a structure of type 'on_surface' - specifying the location of and weather for an 
        ''' observer on the surface of the Earth.
        ''' </summary>
        ''' <param name="Latitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        ''' <param name="Longitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        ''' <param name="Height">Height of the observer (meters).</param>
        ''' <param name="Temperature">Temperature (degrees Celsius).</param>
        ''' <param name="Pressure">Atmospheric pressure (millibars).</param>
        ''' <param name="ObsSurface">Structure containing the location of and weather for an 
        ''' observer on the surface of the Earth.</param>
        ''' <remarks></remarks>
        <DispId(47)> Sub MakeOnSurface(ByVal Latitude As Double, _
                                        ByVal Longitude As Double, _
                                        ByVal Height As Double, _
                                        ByVal Temperature As Double, _
                                        ByVal Pressure As Double, _
                                        ByRef ObsSurface As OnSurface)

        ''' <summary>
        ''' Compute the mean obliquity of the ecliptic.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian Date.</param>
        ''' <returns>Mean obliquity of the ecliptic in arcseconds.</returns>
        ''' <remarks></remarks>
        <DispId(48)> Function MeanObliq(ByVal JdTdb As Double) As Double

        ''' <summary>
        ''' Computes the ICRS position of a star, given its apparent place at date 'JdTt'.  
        ''' Proper motion, parallax and radial velocity are assumed to be zero.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date of apparent place.</param>
        ''' <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        ''' <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date.</param>
        ''' <param name="Accuracy">Specifies accuracy level</param>
        ''' <param name="IRa">ICRS right ascension in hours.</param>
        ''' <param name="IDec">ICRS declination in degrees.</param>
        ''' <returns><pre>
        '''    0 ... Everything OK
        '''    1 ... Iterative process did not converge after 30 iterations; 
        ''' > 10 ... Error from function 'Vector2RaDec'
        ''' > 20 ... Error from function 'AppStar'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(49)> Function MeanStar(ByVal JdTt As Double, _
                                          ByVal Ra As Double, _
                                          ByVal Dec As Double, _
                                          ByVal Accuracy As Accuracy, _
                                          ByRef IRa As Double, _
                                          ByRef IDec As Double) As Short

        ''' <summary>
        ''' Normalize angle into the range 0 <![CDATA[<=]]> angle <![CDATA[<]]> (2 * pi).
        ''' </summary>
        ''' <param name="Angle">Input angle (radians).</param>
        ''' <returns>The input angle, normalized as described above (radians).</returns>
        ''' <remarks></remarks>
        <DispId(50)> Function NormAng(ByVal Angle As Double) As Double

        ''' <summary>
        ''' Nutates equatorial rectangular coordinates from mean equator and equinox of epoch to true equator and equinox of epoch.
        ''' </summary>
        ''' <param name="JdTdb">TDB Julian date of epoch.</param>
        ''' <param name="Direction">Flag determining 'direction' of transformation; direction  = 0 
        ''' transformation applied, mean to true; direction != 0 inverse transformation applied, true to mean.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Pos">Position vector, geocentric equatorial rectangular coordinates, referred to 
        ''' mean equator and equinox of epoch.</param>
        ''' <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, referred to 
        ''' true equator and equinox of epoch.</param>
        ''' <remarks> Inverse transformation may be applied by setting flag 'direction'</remarks>
        <DispId(51)> Sub Nutation(ByVal JdTdb As Double, _
                                      ByVal Direction As NutationDirection, _
                                      ByVal Accuracy As Accuracy, _
                                      ByVal Pos() As Double, _
                                      ByRef Pos2() As Double)

        ''' <summary>
        ''' Returns the values for nutation in longitude and nutation in obliquity for a given TDB Julian date.
        ''' </summary>
        ''' <param name="t">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="DPsi">Nutation in longitude in arcseconds.</param>
        ''' <param name="DEps">Nutation in obliquity in arcseconds.</param>
        ''' <remarks>The nutation model selected depends upon the input value of 'Accuracy'.  See notes below for important details.
        ''' <para>
        ''' This function selects the nutation model depending first upon the input value of 'Accuracy'.  
        ''' If 'Accuracy' = 0 (full accuracy), the IAU 2000A nutation model is used.  If 'Accuracy' = 1 
        ''' a specially truncated (and therefore faster) version of IAU 2000A, called 'NU2000K' is used.
        ''' </para>
        ''' </remarks>
        <DispId(52)> Sub NutationAngles(ByVal t As Double, _
                                          ByVal Accuracy As Accuracy, _
                                          ByRef DPsi As Double, _
                                          ByRef DEps As Double)

        ''' <summary>
        ''' Computes the apparent direction of a star or solar system body at a specified time 
        ''' and in a specified coordinate system.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for place.</param>
        ''' <param name="CelObject"> Specifies the celestial object of interest</param>
        ''' <param name="Location">Specifies the location of the observer</param>
        ''' <param name="DeltaT"> Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="CoordSys">Code specifying coordinate system of the output position. 0 ... GCRS or 
        ''' "local GCRS"; 1 ... true equator and equinox of date; 2 ... true equator and CIO of date; 
        ''' 3 ... astrometric coordinates, i.e., without light deflection or aberration.</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Output">Structure specifying object's place on the sky at time 'JdTt', 
        ''' with respect to the specified output coordinate system</param>
        ''' <returns>
        ''' <pre>
        ''' = 0         ... No problems.
        ''' = 1         ... invalid value of 'CoordSys'
        ''' = 2         ... invalid value of 'Accuracy'
        ''' = 3         ... Earth is the observed object, and the observer is either at the geocenter or on the Earth's surface (not permitted)
        ''' > 10, <![CDATA[<]]> 40  ... 10 + error from function 'Ephemeris'
        ''' > 40, <![CDATA[<]]> 50  ... 40 + error from function 'GeoPosVel'
        ''' > 50, <![CDATA[<]]> 70  ... 50 + error from function 'LightTime'
        ''' > 70, <![CDATA[<]]> 80  ... 70 + error from function 'GravDef'
        ''' > 80, <![CDATA[<]]> 90  ... 80 + error from function 'CioLocation'
        ''' > 90, <![CDATA[<]]> 100 ... 90 + error from function 'CioBasis'
        ''' </pre>
        ''' </returns>
        ''' Values of 'location->where' and 'CoordSys' dictate the various standard kinds of place:
        ''' <pre>
        '''     Location->Where = 0 and CoordSys = 1: apparent place
        '''     Location->Where = 1 and CoordSys = 1: topocentric place
        '''     Location->Where = 0 and CoordSys = 0: virtual place
        '''     Location->Where = 1 and CoordSys = 0: local place
        '''     Location->Where = 0 and CoordSys = 3: astrometric place
        '''     Location->Where = 1 and CoordSys = 3: topocentric astrometric place
        ''' </pre>
        ''' <para>Input value of 'DeltaT' is used only when 'Location->Where' equals 1 or 2 (observer is 
        ''' on surface of Earth or in a near-Earth satellite). </para>
        ''' <remarks>
        ''' </remarks>
        <DispId(53)> Function Place(ByVal JdTt As Double, _
                                       ByVal CelObject As Object3, _
                                       ByVal Location As Observer, _
                                       ByVal DeltaT As Double, _
                                       ByVal CoordSys As CoordSys, _
                                       ByVal Accuracy As Accuracy, _
                                       ByRef Output As SkyPos) As Short

        ''' <summary>
        '''  Precesses equatorial rectangular coordinates from one epoch to another.
        ''' </summary>
        ''' <param name="JdTdb1">TDB Julian date of first epoch.  See remarks below.</param>
        ''' <param name="Pos1">Position vector, geocentric equatorial rectangular coordinates, referred to mean dynamical equator and equinox of first epoch.</param>
        ''' <param name="JdTdb2">TDB Julian date of second epoch.  See remarks below.</param>
        ''' <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, referred to mean dynamical equator and equinox of second epoch.</param>
        ''' <returns><pre>
        ''' 0 ... everything OK
        ''' 1 ... Precession not to or from J2000.0; 'JdTdb1' or 'JdTdb2' not 2451545.0.
        ''' </pre></returns>
        ''' <remarks> One of the two epochs must be J2000.0.  The coordinates are referred to the mean dynamical equator and equinox of the two respective epochs.</remarks>
        <DispId(54)> Function Precession(ByVal JdTdb1 As Double, _
                                           ByVal Pos1() As Double, _
                                           ByVal JdTdb2 As Double, _
                                           ByRef Pos2() As Double) As Short

        ''' <summary>
        ''' Applies proper motion, including foreshortening effects, to a star's position.
        ''' </summary>
        ''' <param name="JdTdb1">TDB Julian date of first epoch.</param>
        ''' <param name="Pos">Position vector at first epoch.</param>
        ''' <param name="Vel">Velocity vector at first epoch.</param>
        ''' <param name="JdTdb2">TDB Julian date of second epoch.</param>
        ''' <param name="Pos2">Position vector at second epoch.</param>
        ''' <remarks></remarks>
        <DispId(55)> Sub ProperMotion(ByVal JdTdb1 As Double, _
                                       ByVal Pos() As Double, _
                                       ByVal Vel() As Double, _
                                       ByVal JdTdb2 As Double, _
                                       ByRef Pos2() As Double)

        ''' <summary>
        ''' Converts equatorial spherical coordinates to a vector (equatorial rectangular coordinates).
        ''' </summary>
        ''' <param name="Ra">Right ascension (hours).</param>
        ''' <param name="Dec">Declination (degrees).</param>
        ''' <param name="Dist">Distance in AU</param>
        ''' <param name="Vector">Position vector, equatorial rectangular coordinates (AU).</param>
        ''' <remarks></remarks>
        <DispId(56)> Sub RaDec2Vector(ByVal Ra As Double, _
                                      ByVal Dec As Double, _
                                      ByVal Dist As Double, _
                                      ByRef Vector() As Double)

        ''' <summary>
        ''' Predicts the radial velocity of the observed object as it would be measured by spectroscopic means.
        ''' </summary>
        ''' <param name="CelObject">Specifies the celestial object of interest</param>
        ''' <param name="Pos"> Geometric position vector of object with respect to observer, corrected for light-time, in AU.</param>
        ''' <param name="Vel">Velocity vector of object with respect to solar system barycenter, in AU/day.</param>
        ''' <param name="VelObs">Velocity vector of observer with respect to solar system barycenter, in AU/day.</param>
        ''' <param name="DObsGeo">Distance from observer to geocenter, in AU.</param>
        ''' <param name="DObsSun">Distance from observer to Sun, in AU.</param>
        ''' <param name="DObjSun">Distance from object to Sun, in AU.</param>
        ''' <param name="Rv">The observed radial velocity measure times the speed of light, in kilometers/second.</param>
        ''' <remarks> Radial velocity is here defined as the radial velocity measure (z) times the speed of light.  
        ''' For a solar system body, it applies to a fictitious emitter at the center of the observed object, 
        ''' assumed massless (no gravitational red shift), and does not in general apply to reflected light.  
        ''' For stars, it includes all effects, such as gravitational red shift, contained in the catalog 
        ''' barycentric radial velocity measure, a scalar derived from spectroscopy.  Nearby stars with a known 
        ''' kinematic velocity vector (obtained independently of spectroscopy) can be treated like 
        ''' solar system objects.</remarks>
        <DispId(57)> Sub RadVel(ByVal CelObject As Object3, _
                                   ByVal Pos() As Double, _
                                   ByVal Vel() As Double, _
                                   ByVal VelObs() As Double, _
                                   ByVal DObsGeo As Double, _
                                   ByVal DObsSun As Double, _
                                   ByVal DObjSun As Double, _
                                   ByRef Rv As Double)

        ''' <summary>
        ''' Computes atmospheric refraction in zenith distance. 
        ''' </summary>
        ''' <param name="Location">Structure containing observer's location.</param>
        ''' <param name="RefOption">1 ... Use 'standard' atmospheric conditions; 2 ... Use atmospheric 
        ''' parameters input in the 'Location' structure.</param>
        ''' <param name="ZdObs">Observed zenith distance, in degrees.</param>
        ''' <returns>Atmospheric refraction, in degrees.</returns>
        ''' <remarks>This version computes approximate refraction for optical wavelengths. This function 
        ''' can be used for planning observations or telescope pointing, but should not be used for the 
        ''' reduction of precise observations.</remarks>
        <DispId(58)> Function Refract(ByVal Location As OnSurface, _
                                        ByVal RefOption As RefractionOption, _
                                        ByVal ZdObs As Double) As Double

        ''' <summary>
        ''' Computes the Greenwich sidereal time, either mean or apparent, at Julian date 'JdHigh' + 'JdLow'.
        ''' </summary>
        ''' <param name="JdHigh">High-order part of UT1 Julian date.</param>
        ''' <param name="JdLow">Low-order part of UT1 Julian date.</param>
        ''' <param name="DeltaT"> Difference TT-UT1 at 'JdHigh'+'JdLow', in seconds of time.</param>
        ''' <param name="GstType">0 ... compute Greenwich mean sidereal time; 1 ... compute Greenwich apparent sidereal time</param>
        ''' <param name="Method">Selection for method: 0 ... CIO-based method; 1 ... equinox-based method</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Gst">Greenwich apparent sidereal time, in hours.</param>
        ''' <returns><pre>
        '''          0 ... everything OK
        '''          1 ... invalid value of 'Accuracy'
        '''          2 ... invalid value of 'Method'
        ''' > 10, <![CDATA[<]]> 30 ... 10 + error from function 'CioRai'
        ''' </pre></returns>
        ''' <remarks> The Julian date may be split at any point, but for highest precision, set 'JdHigh' 
        ''' to be the integral part of the Julian date, and set 'JdLow' to be the fractional part.</remarks>
        <DispId(59)> Function SiderealTime(ByVal JdHigh As Double, _
                                           ByVal JdLow As Double, _
                                           ByVal DeltaT As Double, _
                                           ByVal GstType As GstType, _
                                           ByVal Method As Method, _
                                           ByVal Accuracy As Accuracy, _
                                           ByRef Gst As Double) As Short

        ''' <summary>
        ''' Transforms a vector from one coordinate system to another with same origin and axes rotated about the z-axis.
        ''' </summary>
        ''' <param name="Angle"> Angle of coordinate system rotation, positive counterclockwise when viewed from +z, in degrees.</param>
        ''' <param name="Pos1">Position vector.</param>
        ''' <param name="Pos2">Position vector expressed in new coordinate system rotated about z by 'angle'.</param>
        ''' <remarks></remarks>
        <DispId(60)> Sub Spin(ByVal Angle As Double, _
                              ByVal Pos1() As Double, _
                              ByRef Pos2() As Double)

        ''' <summary>
        ''' Converts angular quantities for stars to vectors.
        ''' </summary>
        ''' <param name="Star">Catalog entry structure containing ICRS catalog data </param>
        ''' <param name="Pos">Position vector, equatorial rectangular coordinates, components in AU.</param>
        ''' <param name="Vel">Velocity vector, equatorial rectangular coordinates, components in AU/Day.</param>
        ''' <remarks></remarks>
        <DispId(61)> Sub StarVectors(ByVal Star As CatEntry3, _
                                      ByRef Pos() As Double, _
                                      ByRef Vel() As Double)

        ''' <summary>
        ''' Computes the Terrestrial Time (TT) or Terrestrial Dynamical Time (TDT) Julian date corresponding 
        ''' to a Barycentric Dynamical Time (TDB) Julian date.
        ''' </summary>
        ''' <param name="TdbJd">TDB Julian date.</param>
        ''' <param name="TtJd">TT Julian date.</param>
        ''' <param name="SecDiff">Difference 'tdb_jd'-'tt_jd', in seconds.</param>
        ''' <remarks>Expression used in this function is a truncated form of a longer and more precise 
        ''' series given in: Explanatory Supplement to the Astronomical Almanac, pp. 42-44 and p. 316. 
        ''' The result is good to about 10 microseconds.</remarks>
        <DispId(62)> Sub Tdb2Tt(ByVal TdbJd As Double, _
                                    ByRef TtJd As Double, _
                                    ByRef SecDiff As Double)

        ''' <summary>
        ''' This function rotates a vector from the terrestrial to the celestial system. 
        ''' </summary>
        ''' <param name="JdHigh">High-order part of UT1 Julian date.</param>
        ''' <param name="JdLow">Low-order part of UT1 Julian date.</param>
        ''' <param name="DeltaT">Value of Delta T (= TT - UT1) at the input UT1 Julian date.</param>
        ''' <param name="Method"> Selection for method: 0 ... CIO-based method; 1 ... equinox-based method</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="OutputOption">0 ... The output vector is referred to GCRS axes; 1 ... The output 
        ''' vector is produced with respect to the equator and equinox of date.</param>
        ''' <param name="x">Conventionally-defined X coordinate of celestial intermediate pole with respect to 
        ''' ITRF pole, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of celestial intermediate pole with respect to 
        ''' ITRF pole, in arcseconds.</param>
        ''' <param name="VecT">Position vector, geocentric equatorial rectangular coordinates, referred to ITRF 
        ''' axes (terrestrial system) in the normal case where 'option' = 0.</param>
        ''' <param name="VecC"> Position vector, geocentric equatorial rectangular coordinates, referred to GCRS 
        ''' axes (celestial system) or with respect to the equator and equinox of date, depending on 'Option'.</param>
        ''' <returns><pre>
        '''    0 ... everything is ok
        '''    1 ... invalid value of 'Accuracy'
        '''    2 ... invalid value of 'Method'
        ''' > 10 ... 10 + error from function 'CioLocation'
        ''' > 20 ... 20 + error from function 'CioBasis'
        ''' </pre></returns>
        ''' <remarks>'x' = 'y' = 0 means no polar motion transformation.
        ''' <para>
        ''' The 'option' flag only works for the equinox-based method.
        '''</para></remarks>
        <DispId(63)> Function Ter2Cel(ByVal JdHigh As Double, _
                                        ByVal JdLow As Double, _
                                        ByVal DeltaT As Double, _
                                        ByVal Method As Method, _
                                        ByVal Accuracy As Accuracy, _
                                        ByVal OutputOption As OutputVectorOption, _
                                        ByVal x As Double, _
                                        ByVal y As Double, _
                                        ByVal VecT() As Double, _
                                        ByRef VecC() As Double) As Short

        ''' <summary>
        ''' Computes the position and velocity vectors of a terrestrial observer with respect to the center of the Earth.
        ''' </summary>
        ''' <param name="Location">Structure containing observer's location </param>
        ''' <param name="St">Local apparent sidereal time at reference meridian in hours.</param>
        ''' <param name="Pos">Position vector of observer with respect to center of Earth, equatorial 
        ''' rectangular coordinates, referred to true equator and equinox of date, components in AU.</param>
        ''' <param name="Vel">Velocity vector of observer with respect to center of Earth, equatorial rectangular 
        ''' coordinates, referred to true equator and equinox of date, components in AU/day.</param>
        ''' <remarks>
        ''' If reference meridian is Greenwich and st=0, 'pos' is effectively referred to equator and Greenwich.
        ''' <para> This function ignores polar motion, unless the observer's longitude and latitude have been 
        ''' corrected for it, and variation in the length of day (angular velocity of earth).</para>
        ''' <para>The true equator and equinox of date do not form an inertial system.  Therefore, with respect 
        ''' to an inertial system, the very small velocity component (several meters/day) due to the precession 
        ''' and nutation of the Earth's axis is not accounted for here.</para>
        ''' </remarks>
        <DispId(64)> Sub Terra(ByVal Location As OnSurface, _
                               ByVal St As Double, _
                               ByRef Pos() As Double, _
                               ByRef Vel() As Double)

        ''' <summary>
        ''' Computes the topocentric place of a solar system body.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for topocentric place.</param>
        ''' <param name="SsBody">structure containing the body designation for the solar system body</param>
        ''' <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="Position">Specifies the position of the observer</param>
        ''' <param name="Accuracy">Selection for accuracy</param>
        ''' <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        ''' <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date.</param>
        ''' <param name="Dis">True distance from Earth to planet at 'JdTt' in AU.</param>
        ''' <returns><pre>
        ''' =  0 ... Everything OK.
        ''' =  1 ... Invalid value of 'Where' in structure 'Location'.
        ''' > 10 ... Error code from function 'Place'.
        '''</pre></returns>
        ''' <remarks></remarks>
        <DispId(65)> Function TopoPlanet(ByVal JdTt As Double, _
                                            ByVal SsBody As Object3, _
                                            ByVal DeltaT As Double, _
                                            ByVal Position As OnSurface, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double, _
                                            ByRef Dis As Double) As Short

        ''' <summary>
        ''' Computes the topocentric place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for topocentric place.</param>
        ''' <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        ''' <param name="Star">Catalog entry structure containing catalog data for the object in the ICRS</param>
        ''' <param name="Position">Specifies the position of the observer</param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra"> Topocentric right ascension in hours, referred to true equator and equinox of date 'JdTt'.</param>
        ''' <param name="Dec">Topocentric declination in degrees, referred to true equator and equinox of date 'JdTt'.</param>
        ''' <returns><pre>
        ''' =  0 ... Everything OK.
        ''' =  1 ... Invalid value of 'Where' in structure 'Location'.
        ''' > 10 ... Error code from function 'MakeObject'.
        ''' > 20 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(66)> Function TopoStar(ByVal JdTt As Double, _
                                          ByVal DeltaT As Double, _
                                          ByVal Star As CatEntry3, _
                                          ByVal Position As OnSurface, _
                                          ByVal Accuracy As Accuracy, _
                                          ByRef Ra As Double, _
                                          ByRef Dec As Double) As Short

        ''' <summary>
        '''  To transform a star's catalog quantities for a change of epoch and/or equator and equinox.
        ''' </summary>
        ''' <param name="TransformOption">Transformation option</param>
        ''' <param name="DateInCat">TT Julian date, or year, of input catalog data.</param>
        ''' <param name="InCat">An entry from the input catalog, with units as given in the struct definition </param>
        ''' <param name="DateNewCat">TT Julian date, or year, of transformed catalog data.</param>
        ''' <param name="NewCatId">Three-character abbreviated name of the transformed catalog. </param>
        ''' <param name="NewCat"> The transformed catalog entry, with units as given in the struct definition </param>
        ''' <returns>
        ''' <pre>
        ''' = 0 ... Everything OK.
        ''' = 1 ... Invalid value of an input date for option 2 or 3 (see Note 1 below).
        ''' </pre></returns>
        ''' <remarks>Also used to rotate catalog quantities on the dynamical equator and equinox of J2000.0 to the ICRS or vice versa.
        ''' <para>1. 'DateInCat' and 'DateNewCat' may be specified either as a Julian date (e.g., 2433282.5) or 
        ''' a Julian year and fraction (e.g., 1950.0).  Values less than 10000 are assumed to be years. 
        ''' For 'TransformOption' = 2 or 'TransformOption' = 3, either 'DateInCat' or 'DateNewCat' must be 2451545.0 or 
        ''' 2000.0 (J2000.0).  For 'TransformOption' = 4 and 'TransformOption' = 5, 'DateInCat' and 'DateNewCat' are ignored.</para>
        ''' <para>2. 'TransformOption' = 1 updates the star's data to account for the star's space motion between the first 
        ''' and second dates, within a fixed reference frame. 'TransformOption' = 2 applies a rotation of the reference 
        ''' frame corresponding to precession between the first and second dates, but leaves the star fixed in 
        ''' space. 'TransformOption' = 3 provides both transformations. 'TransformOption' = 4 and 'TransformOption' = 5 provide a a 
        ''' fixed rotation about very small angles (<![CDATA[<]]>0.1 arcsecond) to take data from the dynamical system 
        ''' of J2000.0 to the ICRS ('TransformOption' = 4) or vice versa ('TransformOption' = 5).</para>
        '''<para>3. For 'TransformOption' = 1, input data can be in any fixed reference system. for 'TransformOption' = 2 or 
        ''' 'TransformOption' = 3, this function assumes the input data is in the dynamical system and produces output 
        ''' in the dynamical system.  for 'TransformOption' = 4, the input data must be on the dynamical equator and 
        ''' equinox of J2000.0.  for 'TransformOption' = 5, the input data must be in the ICRS.</para>
        '''<para>4. This function cannot be properly used to bring data from old star catalogs into the 
        ''' modern system, because old catalogs were compiled using a set of constants that are incompatible 
        ''' with modern values.  In particular, it should not be used for catalogs whose positions and 
        ''' proper motions were derived by assuming a precession constant significantly different from 
        ''' the value implicit in function 'precession'.</para></remarks>
        <DispId(67)> Function TransformCat(ByVal TransformOption As TransformationOption3, _
                                            ByVal DateInCat As Double, _
                                            ByVal InCat As CatEntry3, _
                                            ByVal DateNewCat As Double, _
                                            ByVal NewCatId As String, _
                                            ByRef NewCat As CatEntry3) As Short

        ''' <summary>
        ''' Convert Hipparcos catalog data at epoch J1991.25 to epoch J2000.0, for use within NOVAS.
        ''' </summary>
        ''' <param name="Hipparcos">An entry from the Hipparcos catalog, at epoch J1991.25, with all members 
        ''' having Hipparcos catalog units.  See Note 1 below </param>
        ''' <param name="Hip2000">The transformed input entry, at epoch J2000.0.  See Note 2 below</param>
        ''' <remarks>To be used only for Hipparcos or Tycho stars with linear space motion.  Both input and 
        ''' output data is in the ICRS.
        ''' <para>
        ''' 1. Input (Hipparcos catalog) epoch and units:
        ''' <list type="bullet">
        ''' <item>Epoch: J1991.25</item>
        ''' <item>Right ascension (RA): degrees</item>
        ''' <item>Declination (Dec): degrees</item>
        ''' <item>Proper motion in RA: milliarcseconds per year</item>
        ''' <item>Proper motion in Dec: milliarcseconds per year</item>
        ''' <item>Parallax: milliarcseconds</item>
        ''' <item>Radial velocity: kilometers per second (not in catalog)</item>
        ''' </list>
        ''' </para>
        ''' <para>
        ''' 2. Output (modified Hipparcos) epoch and units:
        ''' <list type="bullet">
        ''' <item>Epoch: J2000.0</item>
        ''' <item>Right ascension: hours</item>
        ''' <item>Declination: degrees</item>
        ''' <item>Proper motion in RA: milliarcseconds per year</item>
        ''' <item>Proper motion in Dec: milliarcseconds per year</item>
        ''' <item>Parallax: milliarcseconds</item>
        ''' <item>Radial velocity: kilometers per second</item>
        ''' </list>>
        ''' </para>
        ''' </remarks>
        <DispId(68)> Sub TransformHip(ByVal Hipparcos As CatEntry3, _
                                          ByRef Hip2000 As CatEntry3)

        ''' <summary>
        ''' Converts a vector in equatorial rectangular coordinates to equatorial spherical coordinates.
        ''' </summary>
        ''' <param name="Pos">Position vector, equatorial rectangular coordinates.</param>
        ''' <param name="Ra">Right ascension in hours.</param>
        ''' <param name="Dec">Declination in degrees.</param>
        ''' <returns>
        ''' <pre>
        ''' = 0 ... Everything OK.
        ''' = 1 ... All vector components are zero; 'Ra' and 'Dec' are indeterminate.
        ''' = 2 ... Both Pos[0] and Pos[1] are zero, but Pos[2] is nonzero; 'Ra' is indeterminate.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(69)> Function Vector2RaDec(ByVal Pos() As Double, _
                                              ByRef Ra As Double, _
                                              ByRef Dec As Double) As Short

        ''' <summary>
        ''' Compute the virtual place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for virtual place.</param>
        ''' <param name="SsBody">structure containing the body designation for the solar system body(</param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Virtual right ascension in hours, referred to the GCRS.</param>
        ''' <param name="Dec">Virtual declination in degrees, referred to the GCRS.</param>
        ''' <param name="Dis">True distance from Earth to planet in AU.</param>
        ''' <returns>
        ''' <pre>
        ''' =  0 ... Everything OK.
        ''' =  1 ... Invalid value of 'Type' in structure 'SsBody'.
        ''' > 10 ... Error code from function 'Place'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(70)> Function VirtualPlanet(ByVal JdTt As Double, _
                                              ByVal SsBody As Object3, _
                                              ByVal Accuracy As Accuracy, _
                                              ByRef Ra As Double, _
                                              ByRef Dec As Double, _
                                              ByRef Dis As Double) As Short

        ''' <summary>
        ''' Computes the virtual place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        ''' </summary>
        ''' <param name="JdTt">TT Julian date for virtual place.</param>
        ''' <param name="Star">catalog entry structure containing catalog data for the object in the ICRS</param>
        ''' <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        ''' <param name="Ra">Virtual right ascension in hours, referred to the GCRS.</param>
        ''' <param name="Dec">Virtual declination in degrees, referred to the GCRS.</param>
        ''' <returns>
        ''' <pre>
        ''' =  0 ... Everything OK.
        ''' > 10 ... Error code from function 'MakeObject'.
        ''' > 20 ... Error code from function 'Place'
        ''' </pre></returns>
        ''' <remarks></remarks>
        <DispId(71)> Function VirtualStar(ByVal JdTt As Double, _
                                             ByVal Star As CatEntry3, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Ra As Double, _
                                             ByRef Dec As Double) As Short

        ''' <summary>
        ''' Corrects a vector in the ITRF (rotating Earth-fixed system) for polar motion, and also corrects 
        ''' the longitude origin (by a tiny amount) to the Terrestrial Intermediate Origin (TIO).
        ''' </summary>
        ''' <param name="Tjd">TT or UT1 Julian date.</param>
        '''       direction (short int)
        ''' <param name="Direction">Flag determining 'direction' of transformation;
        ''' direction  = 0 transformation applied, ITRS to terrestrial intermediate system
        ''' direction != 0 inverse transformation applied, terrestrial intermediate system to ITRS</param>
        ''' <param name="x">Conventionally-defined X coordinate of Celestial Intermediate Pole with 
        ''' respect to ITRF pole, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of Celestial Intermediate Pole with 
        ''' respect to ITRF pole, in arcseconds.</param>
        ''' <param name="Pos1">Position vector, geocentric equatorial rectangular coordinates, 
        ''' referred to ITRF axes.</param>
        ''' <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, 
        ''' referred to true equator and TIO.</param>
        ''' <remarks></remarks>
        <DispId(72)> Sub Wobble(ByVal Tjd As Double, _
                                 ByVal Direction As TransformationDirection, _
                                 ByVal x As Double, _
                                 ByVal y As Double, _
                                 ByVal Pos1() As Double, _
                                 ByRef Pos2() As Double)

        ''' <summary>
        ''' Return the value of DeltaT for the given Julian date
        ''' </summary>
        ''' <param name="Tjd">Julian date for which the delta T value is required</param>
        ''' <returns>Double value of DeltaT (seconds)</returns>
        ''' <remarks>Valid between the years 1650 and 2050</remarks>
        <DispId(73)> Function DeltaT(ByVal Tjd As Double) As Double
    End Interface
End Namespace
#End Region

#Region "AstroUtils Interface"
Namespace AstroUtils
    <ComVisible(True), Guid("143068F6-ADC9-4751-AC39-924111396F0F"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
    Public Interface IAstroUtils
        <DispId(1)> Function ConditionRA(RA As Double) As Double
        <DispId(2)> Function ConditionHA(HA As Double) As Double
        <DispId(3)> Function DeltaT() As Double
        <DispId(4)> Function UnRefract(ByVal Location As OnSurface, ByVal RefOption As RefractionOption, ByVal ZdObs As Double) As Double
        <DispId(5)> ReadOnly Property JulianDateUtc As Double
        <DispId(6)> Function JulianDateTT(DeltaUT1 As Double) As Double
        <DispId(7)> Function JulianDateUT1(DeltaUT1 As Double) As Double
        ''' <summary>
        ''' Flexible routine to range a number between a lower and an higher bound. Switches control whether the ranged value can be equal to either the
        ''' lower or upper bounds.
        ''' </summary>
        ''' <param name="Value">Value to be ranged</param>
        ''' <param name="LowerBound">Lowest value of the range</param>
        ''' <param name="LowerEqual">Boolean flag indicating whether the ranged value can have the lower bound value</param>
        ''' <param name="UpperBound">Highest value of the range</param>
        ''' <param name="UpperEqual">Boolean flag indicating whether the ranged value can have the upper bound value</param>
        ''' <returns>The ranged nunmber as a double</returns>
        ''' <exception cref="ASCOM.InvalidValueException">Thrown if the lower bound is greater than the upper bound.</exception>
        ''' <exception cref="ASCOM.InvalidValueException">Thrown if LowerEqual and UpperEqual are both false and the ranged value equals
        ''' one of these values. This is impossible to handle as the algorithm will always violate one of the rules!</exception>
        ''' <remarks></remarks>
        <DispId(8)> Function Range(Value As Double, LowerBound As Double, LowerEqual As Boolean, UpperBound As Double, UpperEqual As Boolean) As Double
        ''' <summary>
        ''' Converts a calendar day, month, year to a modified Julian date
        ''' </summary>
        ''' <param name="Day">Integer day of ther month</param>
        ''' <param name="Month">Integer month of the year</param>
        ''' <param name="Year">Integer year</param>
        ''' <returns>Double modified julian date</returns>
        ''' <remarks></remarks>
        <DispId(9)> Function CalendarToMJD(Day As Integer, Month As Integer, Year As Integer) As Double
        ''' <summary>
        ''' Translates a modified Julian date to a VB ole automation date, presented as a double
        ''' </summary>
        ''' <param name="MJD">Modified Julian date</param>
        ''' <returns>Date as a VB ole automation date</returns>
        ''' <remarks></remarks>
        <DispId(10)> Function MJDToOADate(MJD As Double) As Double
        ''' <summary>
        ''' Translates a modified Julian date to a date
        ''' </summary>
        ''' <param name="MJD">Modified Julian date</param>
        ''' <returns>Date representing the modified Julian date</returns>
        ''' <remarks></remarks>
        <DispId(11)> Function MJDToDate(MJD As Double) As Date
        ''' <summary>
        ''' Returns a modified Julian date as a string formatted according to the supplied presentation format
        ''' </summary>
        ''' <param name="MJD">Mofified julian date</param>
        ''' <param name="PresentationFormat">Format representation</param>
        ''' <returns>Date string</returns>
        ''' <exception cref="FormatException">Thrown if the provided PresentationFormat is not valid.</exception>
        ''' <remarks>This expects the standard Microsoft date and time formatting characters as described 
        ''' in http://msdn.microsoft.com/en-us/library/362btx8f(v=VS.90).aspx
        ''' </remarks>
        <DispId(12)> Function FormatMJD(MJD As Double, PresentationFormat As String) As String
        ''' <summary>
        ''' Proivides an estimates of DeltaUT1, the difference between UTC and UT1. DeltaUT1 = UT1 - UTC
        ''' </summary>
        ''' <param name="JulianDate">Julian date when DeltaUT is required</param>
        ''' <returns>Double DeltaUT in seconds</returns>
        ''' <remarks>DeltaUT varies only slowly, so the Julian date can be based on UTC, UT1 or Terrestrial Time.</remarks>
        <DispId(13)> Function DeltaUT1(JulianDate As Double) As Double
        ''' <summary>
        ''' Returns a Julian date as a string formatted according to the supplied presentation format
        ''' </summary>
        ''' <param name="JD">Julian date</param>
        ''' <param name="PresentationFormat">Format representation</param>
        ''' <returns>Date as a string</returns>
        ''' <remarks>This expects the standard Microsoft date and time formatting characters as described 
        ''' in http://msdn.microsoft.com/en-us/library/362btx8f(v=VS.90).aspx
        ''' </remarks>
        <DispId(14)> Function FormatJD(JD As Double, PresentationFormat As String) As String
        ''' <summary>
        ''' Sets or returns the number of leap seconds used in ASCOM Astrometry functions
        ''' </summary>
        ''' <value>Integer number of seconds</value>
        ''' <returns>Current number of leap seconds</returns>
        ''' <remarks>The property value is stored in the ASCOM Profile under the name \Astrometry\Leap Seconds. Any change made to this property 
        ''' will be persisted to the ASCOM Profile store and will be immediately availble to this and all future instances of AstroUtils.
        ''' <para>The current value and any announced but not yet actioned change are listed 
        ''' here: ftp://hpiers.obspm.fr/iers/bul/bulc/bulletinc.dat</para> </remarks>
        <DispId(15)> Property LeapSeconds As Integer
        ''' <summary>
        ''' Function that returns a list of rise and set events of a particular type that occur on a particular day at a given latitude, longitude and time zone
        ''' </summary>
        ''' <param name="TypeofEvent">Type of event e.g. Sunrise or Astronomical twighlight</param>
        ''' <param name="Day">Integer Day number</param>
        ''' <param name="Month">Integer Month number</param>
        ''' <param name="Year">Integer Year number</param>
        ''' <param name="SiteLatitude">Site latitude</param>
        ''' <param name="SiteLongitude">Site longitude (West of Greenwich is negative)</param>
        ''' <param name="SiteTimeZone">Site time zone offset (West of Greenwich is negative)</param>
        ''' <returns>An arraylist of event information (see Remarks for arraylist structure).
        '''</returns>
        ''' <exception cref="ASCOM.InvalidValueException">If the combination of day, month and year is invalid e.g. 31st September.</exception>
        ''' <remarks>
        ''' <para>The definitions of sunrise, sunset and the various twighlights that are used in this method are taken from the 
        ''' <a href="http://aa.usno.navy.mil/faq/docs/RST_defs.php">US Naval Observatory Definitions</a>.
        ''' </para>
        ''' <para>The dynamics of the sun, Earth and Moon can result at some latitudes in days where there may be no, 1 or 2 rise or set events during 
        ''' a 24 hour period; in consequence, results are returned in the flexible form of arraylist.</para>
        ''' <para>The returned zero based arraylist has the following values:
        ''' <list type="Bullet">
        ''' <item>Arraylist(0)                              - Boolean - True if the body is above the event limit at midnight (the beginning of the 24 hour day), false if it is below the event limit</item>
        ''' <item>Arraylist(1)                              - Integer - Number of rise events in this 24 hour period</item>
        ''' <item>Arraylist(2)                              - Integer - Number of set events in this 24 hour period</item>
        ''' <item>Arraylist(3) onwards                      - Double  - Values of rise events in hours </item>
        ''' <item>Arraylist(3 + NumberOfRiseEvents) onwards - Double  - Values of set events in hours </item>
        ''' </list></para>
        ''' <para>If the number of rise events is zero the first double value will be the first set event. If the numbers of both rise and set events
        ''' are zero, there will be no double values and the arraylist will just contain elements 0, 1 and 2, the above/below horizon flag and the integer count values.</para>
        ''' <para>The algorithm employed in this method is taken from Astronomy on the Personal Computer (Montenbruck and Pfleger) pp 46..56, 
        ''' Springer Fourth Edition 2000, Fourth Printing 2009. The day is divided into twelve two hour intervals and a quadratic equation is fitted
        ''' to the altitudes at the beginning, middle and end of each interval. The resulting equation coefficients are then processed to determine 
        ''' the number of roots within the interval (each of which corresponds to a rise or set event) and their sense (rise or set). 
        ''' These results are are then aggregated over the day and the resultant list of values returned as the function result.
        ''' </para>
        ''' <para>High precision ephemeredes for the Sun, Moon and Earth and other planets from the JPL DE421 series are employed as delivered by the 
        ''' ASCOM NOVAS 3.1 component rather than using the lower precision ephemeredes employed by Montenbruck and Pfleger.
        ''' </para>
        ''' <para><b>Accuracy</b> Whole year almanacs for Sunrise/Sunset, Moonrise/Moonset and the various twighlights every 5 degrees from the 
        ''' North pole to the South Pole at a variety of longitudes, timezones and dates have been compared to data from
        ''' the <a href="http://aa.usno.navy.mil/data/docs/RS_OneYear.php">US Naval Observatory Astronomical Data</a> web site. The RMS error has been found to be 
        ''' better than 0.5 minute over the latitude range 80 degrees North to 80 degrees South and better than 5 minutes from 80 degrees to the relevant pole.
        ''' Most returned values are within 1 minute of the USNO values although some very infrequent grazing event times at lattiudes from 67 to 90 degrees North and South can be up to 
        ''' 10 minutes different.
        ''' </para>
        ''' <para>An Almanac program that creates a year's worth of information for a given event, lattitude, longitude and timezone is included in the 
        ''' developer code examples elsewhere in this help file. This creates an output file with an almost identical format to that used by the USNO web site 
        ''' and allows comprehensive checking of acccuracy for a given set of parameters.</para>
        ''' </remarks>
        <DispId(16)> Function EventTimes(TypeofEvent As EventType, Day As Integer, Month As Integer, Year As Integer, SiteLatitude As Double, SiteLongitude As Double, SiteTimeZone As Double) As ArrayList
        ''' <summary>
        ''' Returns the fraction of the Moon's surface that is illuminated 
        ''' </summary>
        ''' <param name="JD">Julian day (UTC) for which the Moon illumination is required</param>
        ''' <returns>Percentage illumination of the Moon</returns>
        ''' <remarks> The algorithm used is that given in Astronomical Algorithms (Second Edition, Corrected to August 2009) 
        ''' Chapter 48 p345 by Jean Meeus (Willmann-Bell 1991). The Sun and Moon positions are calculated by high precision NOVAS 3.1 library using JPL DE 421 ephemeredes.
        ''' </remarks>
        <DispId(17)> Function MoonIllumination(JD As Double) As Double
        ''' <summary>
        ''' Returns the Moon phase as an angle
        ''' </summary>
        ''' <param name="JD">Julian day (UTC) for which the Moon phase is required</param>
        ''' <returns>Moon phase as an angle between -180.0 amd +180.0 (see Remarks for further description)</returns>
        ''' <remarks>To allow maximum freedom in displaying the Moon phase, this function returns the excess of the apparent geocentric longitude
        ''' of the Moon over the apparent geocentric longitude of the Sun, expressed as an angle in the range -180.0 to +180.0 degrees.
        ''' This definition is taken from Astronomical Algorithms (Second Edition, Corrected to August 2009) Chapter 49 p349
        ''' by Jean Meeus (Willmann-Bell 1991).
        ''' <para>The frequently used eight phase description for phases of the Moon can be easily constructed from the results of this function
        ''' using logic similar to the following:
        ''' <code>
        '''Select Case MoonPhase
        '''     Case -180.0 To -135.0
        '''         Phase = "Full Moon"
        '''     Case -135.0 To -90.0
        '''         Phase = "Waning Gibbous"
        '''     Case -90.0 To -45.0
        '''         Phase = "Last Quarter"
        '''     Case -45.0 To 0.0
        '''         Phase = "Waning Crescent"
        '''     Case 0.0 To 45.0
        '''         Phase = "New Moon"
        '''     Case 45.0 To 90.0
        '''         Phase = "Waxing Crescent"
        '''     Case 90.0 To 135.0
        '''         Phase = "First Quarter"
        '''     Case 135.0 To 180.0
        '''         Phase = "Waxing Gibbous"
        '''End Select
        ''' </code></para>
        ''' <para>Other representations can be easily constructed by changing the angle ranges and text descriptors as desired. The result range -180 to +180
        ''' was chosen so that negative values represent the Moon waning and positive values represent the Moon waxing.</para>
        '''</remarks>
        <DispId(18)> Function MoonPhase(JD As Double) As Double

    End Interface

End Namespace
#End Region

#Region "SOFA Interface"
Namespace SOFA

    ''' <summary>
    ''' Interface to the SOFA component
    ''' </summary>
    ''' <remarks>Implemented by the SOFA component</remarks>
    <Guid("8E322A40-8E75-49FC-B75B-984A45D35C0A"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
    Public Interface ISOFA
        ''' <summary>
        ''' Major number of the SOFA issue currently used by this component.
        ''' </summary>
        ''' <returns>Integer issue number</returns>
        ''' <remarks></remarks>
        Function SofaReleaseNumber() As Integer

        ''' <summary>
        ''' Release date of the SOFA issue currently used by this component.
        ''' </summary>
        ''' <returns>String date in format yyyy-mm-dd</returns>
        ''' <remarks></remarks>
        Function SofaIssueDate() As String

        ''' <summary>
        ''' Release date of the revision to the SOFA Issue that is actually being used by this component.
        ''' </summary>
        ''' <returns>String date in format yyyy-mm-dd</returns>
        ''' <remarks>When a new issue is employed that doesn't yet have a revision, this mehtod will return the SofaIssueDate</remarks>
        Function SofaRevisionDate() As String

        ''' <summary>
        ''' ICRS RA,Dec to observed place.
        ''' </summary>
        ''' <param name="rc">ICRS RA (radians, note 1)</param>
        ''' <param name="dc">ICRS Dec (radians, note 2)</param>
        ''' <param name="pr">RA Proper motion (radians/year)</param>
        ''' <param name="pd">Dec Proper motion (radians/year</param>
        ''' <param name="px">Parallax (arcsec)</param>
        ''' <param name="rv">Radial veolcity (Km/s, +ve if receding</param>
        ''' <param name="utc1">UTC Julian date (part 1, notes 3,4)</param>
        ''' <param name="utc2">UTC Julian date (part 2, notes 3,4)</param>
        ''' <param name="dut1">UT1 - UTC (seonds, note 5)</param>
        ''' <param name="elong">Site longitude (radians, note 6)</param>
        ''' <param name="phi">Site Latitude (radians, note 6)</param>
        ''' <param name="hm">Site Height (meters, notes 6,8)</param>
        ''' <param name="xp">Polar motion co-ordinate (radians, note 7)</param>
        ''' <param name="yp">Polar motion co-ordinate (radians,note 7)</param>
        ''' <param name="phpa">Site Presure (hPa = mB, note 8)</param>
        ''' <param name="tc">Site Temperature (C)</param>
        ''' <param name="rh">Site relative humidity (fraction in the range: 0.0 to 1.0)</param>
        ''' <param name="wl">Observation wavem=length (micrometres, note 9)</param>
        ''' <param name="aob">Observed Azimuth (radians)</param>
        ''' <param name="zob">Observed Zenith distance (radians)</param>
        ''' <param name="hob">Observed Hour Angle (radians)</param>
        ''' <param name="dob">Observed Declination (radians)</param>
        ''' <param name="rob">Observed RA (radians)</param>
        ''' <param name="eo">Equation of the origins (ERA-GST)</param>
        ''' <returns>+1 = dubious year (Note 4), 0 = OK, -1 = unacceptable date</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>Star data for an epoch other than J2000.0 (for example from the Hipparcos catalog, which has an epoch of J1991.25) will require a preliminary call to iauPmsafe before use.</description></item>
        ''' <item><description>The proper motion in RA is dRA/dt rather than cos(Dec)*dRA/dt.</description></item>
        ''' <item><description>utc1+utc2 is quasi Julian Date (see Note 2), apportioned in any convenient way between the two arguments, for example where utc1 is the Julian Day Number and utc2 is the fraction of a day.
        ''' <para>However, JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The convention in the present function is that the JD day represents UTC days whether the length is 86399, 86400 or 86401 SI seconds.</para>
        ''' <para>Applications should use the function iauDtf2d to convert from calendar date and time of day into 2-part quasi Julian Date, as it implements the leap-second-ambiguity convention just described.</para></description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>UT1-UTC is tabulated in IERS bulletins.  It increases by exactly one second at the end of each positive UTC leap second, introduced in order to keep UT1-UTC within +/- 0.9s.  n.b. This practice is under review, and in the future UT1-UTC may grow essentially without limit.</description></item>
        ''' <item><description>The geographical coordinates are with respect to the WGS84 reference ellipsoid.  TAKE CARE WITH THE LONGITUDE SIGN:  the longitude required by the present function is east-positive (i.e. right-handed), in accordance with geographical convention.</description></item>
        ''' <item><description>The polar motion xp,yp can be obtained from IERS bulletins.  The values are the coordinates (in radians) of the Celestial Intermediate Pole with respect to the International Terrestrial Reference System (see IERS Conventions 2003), measured along the meridians 0 and 90 deg west respectively.  For many applications, xp and yp can be set to zero.</description></item>
        ''' <item><description>If hm, the height above the ellipsoid of the observing station in meters, is not known but phpa, the pressure in hPa (=mB), is available, an adequate estimate of hm can be obtained from the expression:
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>hm = -29.3 * tsl * log ( phpa / 1013.25 );</b></p>
        ''' <para>where tsl is the approximate sea-level air temperature in K (See Astrophysical Quantities, C.W.Allen, 3rd edition, section 52).  Similarly, if the pressure phpa is not known, it can be estimated from the height of the observing station, hm, as follows:</para>
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>phpa = 1013.25 * exp ( -hm / ( 29.3 * tsl ) );</b></p>
        ''' <para>Note, however, that the refraction is nearly proportional to the pressure and that an accurate phpa value is important for precise work.</para></description></item>
        ''' <item><description>The argument wl specifies the observing wavelength in micrometers.  The transition from optical to radio is assumed to occur at 100 micrometers (about 3000 GHz).</description></item>
        ''' <item><description>The accuracy of the result is limited by the corrections for refraction, which use a simple A*tan(z) + B*tan^3(z) model. Providing the meteorological parameters are known accurately and there are no gross local effects, the predicted observed coordinates should be within 0.05 arcsec (optical) or 1 arcsec (radio) for a zenith distance of less than 70 degrees, better than 30 arcsec (optical or radio) at 85 degrees and better than 20 arcmin (optical) or 30 arcmin (radio) at the horizon.
        ''' <para>Without refraction, the complementary functions iauAtco13 and iauAtoc13 are self-consistent to better than 1 microarcsecond all over the celestial sphere.  With refraction included, consistency falls off at high zenith distances, but is still better than 0.05 arcsec at 85 degrees.</para></description></item>
        ''' <item><description>"Observed" Az,ZD means the position that would be seen by a perfect geodetically aligned theodolite.  (Zenith distance is used rather than altitude in order to reflect the fact that no allowance is made for depression of the horizon.)  This is related to the observed HA,Dec via the standard rotation, using the geodetic latitude (corrected for polar motion), while the observed HA and RA are related simply through the Earth rotation angle and the site longitude.  "Observed" RA,Dec or HA,Dec thus means the position that would be seen by a perfect equatorial with its polar axis aligned to the Earth's axis of rotation.</description></item>
        ''' <item><description>It is advisable to take great care with units, as even unlikely values of the input parameters are accepted and processed in accordance with the models used.</description></item>
        ''' </list>
        ''' </remarks>
        Function Atco13(rc As Double,
                                 dc As Double,
                                 pr As Double,
                                 pd As Double,
                                 px As Double,
                                 rv As Double,
                                 utc1 As Double,
                                 utc2 As Double,
                                 dut1 As Double,
                                 elong As Double,
                                 phi As Double,
                                 hm As Double,
                                 xp As Double,
                                 yp As Double,
                                 phpa As Double,
                                 tc As Double,
                                 rh As Double,
                                 wl As Double,
                                 ByRef aob As Double,
                                 ByRef zob As Double,
                                 ByRef hob As Double,
                                 ByRef dob As Double,
                                 ByRef rob As Double,
                                 ByRef eo As Double) As Integer

        ''' <summary>
        ''' Transform ICRS star data, epoch J2000.0, to CIRS.
        ''' </summary>
        ''' <param name="rc">ICRS right ascension at J2000.0 (radians, Note 1)</param>
        ''' <param name="dc">ICRS declination at J2000.0 (radians, Note 1)</param>
        ''' <param name="pr">RA proper motion (radians/year; Note 2)</param>
        ''' <param name="pd">Dec proper motion (radians/year)</param>
        ''' <param name="px">parallax (arcsec)</param>
        ''' <param name="rv">radial velocity (km/s, +ve if receding)</param>
        ''' <param name="date1">TDB as a 2-part Julian Date (Note 3)</param>
        ''' <param name="date2">TDB as a 2-part Julian Date (Note 3)</param>
        ''' <param name="ri">CIRS geocentric RA (radians)</param>
        ''' <param name="di">CIRS geocentric Dec (radians)</param>
        ''' <param name="eo">equation of the origins (ERA-GST, Note 5)</param>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>Star data for an epoch other than J2000.0 (for example from the Hipparcos catalog, which has an epoch of J1991.25) will require a preliminary call to iauPmsafe before use.</description></item>
        ''' <item><description>The proper motion in RA is dRA/dt rather than cos(Dec)*dRA/dt.</description></item>
        ''' <item><description> The TDB date date1+date2 is a Julian Date, apportioned in any convenient way between the two arguments.  For example, JD(TDB)=2450123.8g could be expressed in any of these ways, among others:
        ''' <table style="width:340px;" cellspacing="0">
        ''' <col style="width:80px;"></col>
        ''' <col style="width:80px;"></col>
        ''' <col style="width:180px;"></col>
        ''' <tr>
        ''' <td colspan="1" align="center" rowspan="1" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid;
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 1</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 2</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 180px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="220px">
        ''' <b>Method</b></td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        '''  2450123.8</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.0</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' JD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2451545.0</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' -1421.3</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' J2000 method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2400000.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 50123.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' MJD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2450123.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' Date and time method</td>
        ''' </tr>
        ''' </table>
        ''' <para>The JD method is the most natural and convenient to use in cases where the loss of several decimal digits of resolution is acceptable.  The J2000 method is best matched to the way the argument is handled internally 
        ''' and will deliver the optimum resolution.  The MJD method and the date and time methods are both good compromises between resolution and convenience.  For most applications of this function the choice will not be at all critical.</para>
        ''' <para>TT can be used instead of TDB without any significant impact on accuracy.</para>
        ''' </description></item>
        ''' <item><description>The available accuracy is better than 1 milliarcsecond, limited mainly by the precession-nutation model that is used, namely IAU 2000A/2006.  Very close to solar system bodies, additional 
        ''' errors of up to several milliarcseconds can occur because of unmodeled light deflection;  however, the Sun's contribution is taken into account, to first order.  The accuracy limitations of 
        ''' the SOFA function iauEpv00 (used to compute Earth position and velocity) can contribute aberration errors of up to 5 microarcseconds.  Light deflection at the Sun's limb is uncertain at the 0.4 mas level.</description></item>
        ''' <item><description>Should the transformation to (equinox based) apparent place be required rather than (CIO based) intermediate place, subtract the equation of the origins from the returned right ascension:
        ''' RA = RI - EO. (The Anp function can then be applied, as required, to keep the result in the conventional 0-2pi range.)</description></item>
        ''' </list>
        ''' </remarks>
        Sub Atci13(rc As Double,
                        dc As Double,
                        pr As Double,
                        pd As Double,
                        px As Double,
                        rv As Double,
                        date1 As Double,
                        date2 As Double,
                        ByRef ri As Double,
                        ByRef di As Double,
                        ByRef eo As Double)

        ''' <summary>
        ''' Transform star RA,Dec from geocentric CIRS to ICRS astrometric.
        ''' </summary>
        ''' <param name="ri">CIRS geocentric RA (radians)</param>
        ''' <param name="di">CIRS geocentric Dec (radians)</param>
        ''' <param name="date1">TDB as a 2-part Julian Date (Note 1)</param>
        ''' <param name="date2">TDB as a 2-part Julian Date (Note 1)</param>
        ''' <param name="rc">ICRS astrometric RA (radians)</param>
        ''' <param name="dc">ICRS astrometric Dec (radians)</param>
        ''' <param name="eo">equation of the origins (ERA-GST, Note 4)</param>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description> The TDB date date1+date2 is a Julian Date, apportioned in any convenient way between the two arguments.  For example, JD(TDB)=2450123.8g could be expressed in any of these ways, among others:
        ''' <table style="width:340px;" cellspacing="0">
        ''' <col style="width:80px;"></col>
        ''' <col style="width:80px;"></col>
        ''' <col style="width:180px;"></col>
        ''' <tr>
        ''' <td colspan="1" align="center" rowspan="1" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid;
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 1</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 2</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 180px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="220px">
        ''' <b>Method</b></td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        '''  2450123.8</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.0</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' JD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2451545.0</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' -1421.3</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' J2000 method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2400000.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 50123.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' MJD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2450123.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' Date and time method</td>
        ''' </tr>
        ''' </table>
        ''' <para>The JD method is the most natural and convenient to use in cases where the loss of several decimal digits of resolution is acceptable.  The J2000 method is best matched to the way the argument is handled internally 
        ''' and will deliver the optimum resolution.  The MJD method and the date and time methods are both good compromises between resolution and convenience.  For most applications of this function the choice will not be at all critical.</para>
        ''' <para>TT can be used instead of TDB without any significant impact on accuracy.</para>
        ''' </description></item>
        ''' <item><description>Iterative techniques are used for the aberration and light deflection corrections so that the functions Atic13 and Atci13 are accurate inverses; 
        ''' even at the edge of the Sun's disk the discrepancy is only about 1 nanoarcsecond.</description></item>
        ''' <item><description>The available accuracy is better than 1 milliarcsecond, limited mainly by the precession-nutation model that is used, namely IAU 2000A/2006.  Very close to solar system bodies, additional 
        ''' errors of up to several milliarcseconds can occur because of unmodeled light deflection;  however, the Sun's contribution is taken into account, to first order.  The accuracy limitations of 
        ''' the SOFA function iauEpv00 (used to compute Earth position and velocity) can contribute aberration errors of up to 5 microarcseconds.  Light deflection at the Sun's limb is uncertain at the 0.4 mas level.</description></item>
        ''' <item><description>Should the transformation to (equinox based) J2000.0 mean place be required rather than (CIO based) ICRS coordinates, subtract the equation of the origins from the returned right ascension:
        ''' RA = RI - EO.  (The Anp function can then be applied, as required, to keep the result in the conventional 0-2pi range.)</description></item>
        ''' </list>
        ''' </remarks>
        Sub Atic13(ri As Double,
                         di As Double,
                         date1 As Double,
                         date2 As Double,
                         ByRef rc As Double,
                         ByRef dc As Double,
                         ByRef eo As Double)

        Function Atoc13(type As String,
                                 ob1 As Double,
                                 ob2 As Double,
                                 utc1 As Double,
                                 utc2 As Double,
                                 dut1 As Double,
                                 elong As Double,
                                 phi As Double,
                                 hm As Double,
                                 xp As Double,
                                 yp As Double,
                                 phpa As Double,
                                 tc As Double,
                                 rh As Double,
                                 wl As Double,
                                 ByRef rc As Double,
                                 ByRef dc As Double) As Integer

        Function Atio13(ri As Double,
                                         di As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef aob As Double,
                                         ByRef zob As Double,
                                         ByRef hob As Double,
                                         ByRef dob As Double,
                                         ByRef rob As Double) As Integer

        Function Atoi13(type As String,
                                         ob1 As Double,
                                         ob2 As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef ri As Double,
                                         ByRef di As Double) As Integer



        ''' <summary>
        ''' Equation of the origins, IAU 2006 precession and IAU 2000A nutation.
        ''' </summary>
        ''' <param name="date1">TT as a 2-part Julian Date (Note 1)</param>
        ''' <param name="date2">TT as a 2-part Julian Date (Note 1)</param>
        ''' <returns>Equation of the origins in radians (Note 2)</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description> The TT date date1+date2 is a Julian Date, apportioned in any convenient way between the two arguments.  For example, JD(TT)=2450123.7 could be expressed in any of these ways, among others:
        ''' <table style="width:340px;" cellspacing="0">
        ''' <col style="width:80px;"></col>
        ''' <col style="width:80px;"></col>
        ''' <col style="width:180px;"></col>
        ''' <tr>
        ''' <td colspan="1" align="center" rowspan="1" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid;
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 1</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 2</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 180px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="220px">
        ''' <b>Method</b></td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        '''  2450123.7</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.0</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' JD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2451545.0</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' -1421.3</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' J2000 method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2400000.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 50123.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' MJD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2450123.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' Date and time method</td>
        ''' </tr>
        ''' </table>
        ''' <para>The JD method is the most natural and convenient to use in cases where the loss of several decimal digits of resolution is acceptable.  The J2000 method is best matched to the way the argument is handled internally 
        ''' and will deliver the optimum resolution.  The MJD method and the date and time methods are both good compromises between resolution and convenience.  For most applications of this function the choice will not be at all critical.</para>
        ''' </description></item>
        ''' <item><description> The equation of the origins is the distance between the true equinox and the celestial intermediate origin and, equivalently, the difference between Earth rotation angle and Greenwich
        ''' apparent sidereal time (ERA-GST).  It comprises the precession (since J2000.0) in right ascension plus the equation of the equinoxes (including the small correction terms).</description></item>
        ''' </list>
        ''' </remarks>
        Function Eo06a(date1 As Double,
                            date2 As Double) As Double

        ''' <summary>
        ''' Normalize angle into the range 0 &lt;= a &lt; 2pi.
        ''' </summary>
        ''' <param name="a">Angle (radians)</param>
        ''' <returns>Angle in range 0-2pi</returns>
        ''' <remarks></remarks>
        Function Anp(a As Double) As Double

        ''' <summary>
        ''' Convert hours, minutes, seconds to radians.
        ''' </summary>
        ''' <param name="s">sign:  '-' = negative, otherwise positive</param>
        ''' <param name="ihour">Hours</param>
        ''' <param name="imin">Minutes</param>
        ''' <param name="sec">Seconds</param>
        ''' <param name="rad">Angle in radians</param>
        ''' <returns>Status:  0 = OK, 1 = ihour outside range 0-23, 2 = imin outside range 0-59, 3 = sec outside range 0-59.999...</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>The result is computed even if any of the range checks fail.</description></item>
        ''' <item><description>Negative ihour, imin and/or sec produce a warning status, but the absolute value is used in the conversion.</description></item>
        ''' <item><description>If there are multiple errors, the status value reflects only the first, the smallest taking precedence.</description></item>
        ''' </list>
        ''' </remarks>
        Function Tf2a(s As String,
                               ihour As Integer,
                               imin As Integer,
                               sec As Double,
                               ByRef rad As Double) As Integer


        ''' <summary>
        ''' Convert degrees, arcminutes, arcseconds to radians.
        ''' </summary>
        ''' <param name="s">Sign:  '-' = negative, otherwise positive</param>
        ''' <param name="ideg">Degrees</param>
        ''' <param name="iamin">Arcminutes</param>
        ''' <param name="asec">Arcseconds</param>
        ''' <param name="rad">Angle in radian</param>
        ''' <returns>Status:  0 = OK, 1 = ideg outside range 0-359, 2 = iamin outside range 0-59, 3 = asec outside range 0-59.999...</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>The result is computed even if any of the range checks fail.</description></item>
        ''' <item><description>Negative ideg, iamin and/or asec produce a warning status, but the absolute value is used in the conversion.</description></item>
        ''' <item><description>If there are multiple errors, the status value reflects only the first, the smallest taking precedence.</description></item>
        ''' </list>
        ''' </remarks>
        Function Af2a(s As String,
                                ideg As Integer,
                                iamin As Integer,
                                asec As Double,
                                ByRef rad As Double) As Integer


        ''' <summary>
        ''' Encode date and time fields into 2-part Julian Date (or in the case of UTC a quasi-JD form that includes special provision for leap seconds).
        ''' </summary>
        ''' <param name="scale">Time scale ID (Note 1)</param>
        ''' <param name="iy">Year in Gregorian calendar (Note 2)</param>
        ''' <param name="im">Month in Gregorian calendar (Note 2)</param>
        ''' <param name="id">Day in Gregorian calendar (Note 2)</param>
        ''' <param name="ihr">Hour</param>
        ''' <param name="imn">Minute</param>
        ''' <param name="sec">Seconds</param>
        ''' <param name="d1">2-part Julian Date (Notes 3, 4)</param>
        ''' <param name="d2">2-part Julian Date (Notes 3, 4)</param>
        ''' <returns>Status: +3 = both of next two, +2 = time is after end of day (Note 5), +1 = dubious year (Note 6), 0 = OK, -1 = bad year, -2 = bad month, -3 = bad day, -4 = bad hour, -5 = bad minute, -6 = bad second (&lt;0)</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>Scale identifies the time scale.  Only the value "UTC" (in upper case) is significant, and enables handling of leap seconds (see Note 4).</description></item>
        ''' <item><description>For calendar conventions and limitations, see iauCal2jd.</description></item>
        ''' <item><description>The sum of the results, d1+d2, is Julian Date, where normally d1 is the Julian Day Number and d2 is the fraction of a day.  In the case of UTC, where the use of JD is problematical, special conventions apply:  see the next note.</description></item>
        ''' <item><description>JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The SOFA internal convention is that the quasi-JD day represents UTC days whether the length is 86399,
        ''' 86400 or 86401 SI seconds.  In the 1960-1972 era there were smaller jumps (in either direction) each time the linear UTC(TAI) expression was changed, and these "mini-leaps" are also included in the SOFA convention.</description></item>
        ''' <item><description>The warning status "time is after end of day" usually means that the sec argument is greater than 60.0.  However, in a day ending in a leap second the limit changes to 61.0 (or 59.0 in the case of a negative leap second).</description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>Only in the case of continuous and regular time scales (TAI, TT, TCG, TCB and TDB) is the result d1+d2 a Julian Date, strictly speaking.  In the other cases (UT1 and UTC) the result must be
        ''' used with circumspection;  in particular the difference between two such results cannot be interpreted as a precise time interval.</description></item>
        ''' </list>
        ''' </remarks>
        Function Dtf2d(scale As String,
                                 iy As Integer,
                                 im As Integer,
                                 id As Integer,
                                 ihr As Integer,
                                 imn As Integer,
                                 sec As Double,
                                 ByRef d1 As Double,
                                 ByRef d2 As Double) As Integer


        ''' <summary>
        ''' Time scale transformation:  Coordinated Universal Time, UTC, to International Atomic Time, TAI.
        ''' </summary>
        ''' <param name="utc1">UTC as a 2-part quasi Julian Date (Notes 1-4)</param>
        ''' <param name="utc2">UTC as a 2-part quasi Julian Date (Notes 1-4)</param>
        ''' <param name="tai1">TAI as a 2-part Julian Date (Note 5)</param>
        ''' <param name="tai2">TAI as a 2-part Julian Date (Note 5)</param>
        ''' <returns>Status: +1 = dubious year (Note 3) 0 = OK -1 = unacceptable date</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>utc1+utc2 is quasi Julian Date (see Note 2), apportioned in any convenient way between the two arguments, for example where utc1 is the Julian Day Number and utc2 is the fraction of a day.</description></item>
        ''' <item><description>JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The convention in the present function is that the JD day represents UTC days whether the
        ''' length is 86399, 86400 or 86401 SI seconds.  In the 1960-1972 era there were smaller jumps (in either direction) each time the linear UTC(TAI) expression was changed, and these "mini-leaps" are also included in the SOFA convention.</description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>The function iauDtf2d converts from calendar date and time of day into 2-part Julian Date, and in the case of UTC implements the leap-second-ambiguity convention described above.</description></item>
        ''' <item><description>The returned TAI1,TAI2 are such that their sum is the TAI Julian Date.</description></item>
        ''' </list>
        ''' </remarks>
        Function UtcTai(utc1 As Double,
                                utc2 As Double,
                                ByRef tai1 As Double,
                                ByRef tai2 As Double) As Integer


        ''' <summary>
        ''' Time scale transformation:  International Atomic Time, TAI, to Terrestrial Time, TT.
        ''' </summary>
        ''' <param name="tai1">TAI as a 2-part Julian Date</param>
        ''' <param name="tai2">TAI as a 2-part Julian Date</param>
        ''' <param name="tt1">TT as a 2-part Julian Date</param>
        ''' <param name="tt2">TT as a 2-part Julian Date</param>
        ''' <returns>Status:  0 = OK</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description> tai1+tai2 is Julian Date, apportioned in any convenient way between the two arguments, for example where tai1 is the Julian Day Number and tai2 is the fraction of a day.  The returned
        ''' tt1,tt2 follow suit.</description></item>
        ''' </list>
        ''' </remarks>
        Function TaiTt(tai1 As Double,
                                  tai2 As Double,
                                  ByRef tt1 As Double,
                                  ByRef tt2 As Double) As Integer

    End Interface
End Namespace
#End Region