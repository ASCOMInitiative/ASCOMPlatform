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
    End Interface
End Namespace
#End Region

#Region "Kepler Ephemeris Interface"

Namespace Kepler
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
    ''' Represents the "state" of the Earth at a given Terrestrial Julian date
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
    ''' Provide characteristics of a solar system body
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
        <DispId(3)> Function GetLocalPosition(ByVal tjd As Double, ByVal site As Site) As PositionVector
        ''' <summary>
        ''' Get a topocentric position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">The observing site</param>
        ''' <param name="Refract">Apply refraction correction</param>
        ''' <returns>PositionVector for the topocentric place.</returns>
        ''' <remarks></remarks>
        <DispId(4)> Function GetTopocentricPosition(ByVal tjd As Double, ByVal site As Site, ByVal Refract As Boolean) As PositionVector
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
    ''' NOVAS-COM PositionVector Class
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
    ComVisible(True)> _
    Public Interface IPositionVector
        ''' <summary>
        ''' Adjust the position vector of an object for aberration of light
        ''' </summary>
        ''' <param name="vel">The velocity vector of the observer</param>
        ''' <remarks>The algorithm includes relativistic terms</remarks>
        <DispId(1)> Sub Aberration(ByVal vel As VelocityVector)
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
        <DispId(3)> Function ProperMotion(ByVal vel As VelocityVector, ByVal tjd1 As Double, ByVal tjd2 As Double) As Boolean
        ''' <summary>
        ''' Initialize the PositionVector from a Site object and Greenwich apparent sidereal time.
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="gast">Greenwich Apparent Sidereal Time</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks>The GAST parameter must be for Greenwich, not local. The time is rotated through the 
        ''' site longitude. See SetFromSiteJD() for an equivalent method that takes UTC Julian Date and 
        ''' Delta-T (eliminating the need for calculating hyper-accurate GAST yourself).</remarks>
        <DispId(4)> Function SetFromSite(ByVal site As Site, ByVal gast As Double) As Boolean
        ''' <summary>
        ''' Initialize the PositionVector from a Site object using UTC Julian date and Delta-T
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="ujd">UTC Julian Date</param>
        ''' <param name="delta_t">The value of Delta-T (TT - UT1) to use for reductions (seconds)</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks>The Julian date must be UTC Julian date, not terrestrial.
        ''' </remarks>
        <DispId(5)> Overloads Function SetFromSiteJD(ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double) As Boolean
        ''' <summary>
        ''' Initialize the PositionVector from a Star object.
        ''' </summary>
        ''' <param name="star">The Star object from which to initialize</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks></remarks>
        ''' <exception cref="Exceptions.ValueNotAvailableException">If Parallax, RightAScension or Declination is not available in the supplied star object.</exception>
        <DispId(6)> Function SetFromStar(ByVal star As Star) As Boolean
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
        Overloads Function SetFromSiteJD(ByVal site As Site, ByVal ujd As Double) As Boolean
    End Interface

    ''' <summary>
    ''' NOVAS-COM Site Class
    ''' </summary>
    ''' <remarks>Objects of class Site contain the specifications for an observer's location on the Earth 
    ''' ellipsoid. Properties are latitude, longitude, height above mean sea level, the ambient temperature 
    ''' and the sea-level barmetric pressure. The latter two are used only for optional refraction corrections. 
    ''' Latitude and longitude are (common) geodetic, not geocentric. </remarks>
    <Guid("2414C071-8A5B-4d53-89BC-CAF30BA7123B"), _
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
    ''' NOVAS-COM Star Class
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
        <DispId(5)> Function GetLocalPosition(ByVal tjd As Double, ByVal site As Site) As PositionVector
        ''' <summary>
        ''' Get a topocentric position for a given site and time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">A Site object representing the observing site</param>
        ''' <param name="Refract">True to apply atmospheric refraction corrections</param>
        ''' <returns>PositionVector for the topocentric place.</returns>
        ''' <remarks></remarks>
        <DispId(6)> Function GetTopocentricPosition(ByVal tjd As Double, ByVal site As Site, ByVal Refract As Boolean) As PositionVector
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
    ''' NOVAS_COM VelocityVector Class
    ''' </summary>
    ''' <remarks>Objects of class VelocityVector contain vectors used for velocities (earth, sites, 
    ''' planets, and stars) throughout NOVAS-COM. Of course, its properties include the x, y, and z 
    ''' components of the velocity. Additional properties are the velocity in equatorial coordinates of 
    ''' right ascension dot, declination dot and radial velocity. You can initialize a PositionVector from 
    ''' a Star object (essentially an FK5 or HIP catalog entry) or a Site (lat/long/height). For the star 
    ''' object the proper motions, distance and radial velocity are used, for a site, the velocity is that 
    ''' of the observer with respect to the Earth's center of mass. </remarks>
    <Guid("8DD80835-29C6-49d6-8E4D-8887B20E707E"), _
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
        <DispId(1)> Function SetFromSite(ByVal site As Site, ByVal gast As Double) As Boolean
        ''' <summary>
        ''' Initialize the VelocityVector from a Site object using UTC Julian Date and Delta-T
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="ujd">UTC Julian Date</param>
        ''' <param name="delta_t">The optional value of Delta-T (TT - UT1) to use for reductions (seconds)</param>
        ''' <returns>True if OK otherwise throws an exception</returns>
        ''' <remarks>The velocity vector is that of the observer with respect to the Earth's center 
        ''' of mass. The Julian date must be UTC Julian date, not terrestrial.</remarks>
        <DispId(2)> Function SetFromSiteJD(ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double) As Boolean
        ''' <summary>
        ''' Initialize the VelocityVector from a Star object.
        ''' </summary>
        ''' <param name="star">The Star object from which to initialize</param>
        ''' <returns>True if OK otherwise throws an exception</returns>
        ''' <remarks>The proper motions, distance and radial velocity are used in the velocity calculation. </remarks>
        ''' <exception cref="Exceptions.ValueNotAvailableException">If any of: Parallax, RightAscension, Declination, 
        ''' ProperMotionRA, ProperMotionDec or RadialVelocity are not available in the star object</exception>
        <DispId(3)> Function SetFromStar(ByVal star As Star) As Boolean
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
        Function SetFromSiteJD(ByVal site As Site, ByVal ujd As Double) As Boolean
    End Interface

End Namespace
#End Region

#Region "NOVAS2 Interface"
Namespace NOVAS

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
        ''' <remarks></remarks>
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
        ''' <remarks></remarks>
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
        ''' <remarks></remarks>
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
        ''' <remarks></remarks>
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
        ''' <remarks></remarks>
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

        'Public Shared Function solarsystem(ByVal tjd As Double, _
        '                                      ByVal body As Body, _
        '                                      ByRef origin As Origin, _
        '                                      ByRef pos As Double(), _
        '                                      ByRef vel As Double()) As Short
        'Dim vpos As New posvector, vvel As New velvector
        'Dim rc As Short
        '    If Is64Bit() Then
        '        rc = solarsystem64(tjd, body, origin, vpos, vvel)
        '    Else
        '        rc = solarsystem32(tjd, body, origin, vpos, vvel)
        '    End If
        '    PosVecToArr(vpos, pos)
        '    VelVecToArr(vvel, vel)
        '    Return rc
        'End Function
    End Interface
End Namespace
#End Region