Namespace KEPLER
    ''' <summary>
    ''' Kepler Ephemeris Object
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
    Public Class Ephemeris
        Implements KEPLER.IEphemeris

        Private Const DTVEL As Double = 0.01

        'Ephemeris variables
        Private m_Name As String 'Name of body
        Private m_Number As PlanetNumber 'Number of body
        Private m_bNumberValid As Boolean
        Private m_Type As BodyType 'Type of body
        Private m_bTypeValid As Boolean
        Private m_e As orbit 'Elements, etc for minor planets/comets, etc.

        'gplan variables
        Private ss(NARGS, 31), cc(NARGS, 31), Args(NARGS), LP_equinox, NF_arcsec, Ea_arcsec, pA_precession As Double

        ''' <summary>
        ''' Create a new Ephemeris component and initialise it
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            m_bTypeValid = False
            m_Name = "" 'Sentinel
            m_Type = Nothing
        End Sub
        ''' <summary>
        ''' Semi-major axis (AU)
        ''' </summary>
        ''' <value>Semi-major axis in AU</value>
        ''' <returns>Semi-major axis in AU</returns>
        ''' <remarks></remarks>
        Public Property a() As Double Implements KEPLER.IEphemeris.a
            Get
                Return m_e.a
            End Get
            Set(ByVal value As Double)
                m_e.a = value
            End Set
        End Property

        ''' <summary>
        ''' The type of solar system body represented by this instance of the ephemeris engine (enum)
        ''' </summary>
        ''' <value>The type of solar system body represented by this instance of the ephemeris engine (enum)</value>
        ''' <returns>0 for major planet, 1 for minot planet and 2 for comet</returns>
        ''' <remarks></remarks>
        Public Property BodyType() As KEPLER.BodyType Implements KEPLER.IEphemeris.BodyType
            Get
                If Not m_bTypeValid Then Throw New Exceptions.ValueNotSetException("KEPLER:BodyType BodyType has not been set")
                Return m_Type
            End Get
            Set(ByVal value As KEPLER.BodyType)
                m_Type = value
                m_bTypeValid = True
            End Set
        End Property

        ''' <summary>
        ''' Orbital eccentricity
        ''' </summary>
        ''' <value>Orbital eccentricity </value>
        ''' <returns>Orbital eccentricity </returns>
        ''' <remarks></remarks>
        Public Property e() As Double Implements KEPLER.IEphemeris.e
            Get
                Return m_e.ecc
            End Get
            Set(ByVal value As Double)
                m_e.ecc = value
            End Set
        End Property

        ''' <summary>
        ''' Epoch of osculation of the orbital elements (terrestrial Julian date)
        ''' </summary>
        ''' <value>Epoch of osculation of the orbital elements</value>
        ''' <returns>Terrestrial Julian date</returns>
        ''' <remarks></remarks>
        Public Property Epoch() As Double Implements KEPLER.IEphemeris.Epoch
            Get
                Return m_e.epoch
            End Get
            Set(ByVal value As Double)
                m_e.epoch = value
            End Set
        End Property

        ''' <summary>
        ''' Slope parameter for magnitude
        ''' </summary>
        ''' <value>Slope parameter for magnitude</value>
        ''' <returns>Slope parameter for magnitude</returns>
        ''' <remarks></remarks>
        Public Property G() As Double Implements KEPLER.IEphemeris.G
            Get
                Throw New Exceptions.ValueNotAvailableException("Kepler:G Read - Magnitude slope parameter calculation not implemented")
            End Get
            Set(ByVal value As Double)
                Throw New Exceptions.ValueNotAvailableException("Kepler:G Write - Magnitude slope parameter calculation not implemented")
            End Set
        End Property

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
        Public Function GetPositionAndVelocity(ByVal tjd As Double) As Double() Implements KEPLER.IEphemeris.GetPositionAndVelocity
            Dim posvec(5) As Double
            Dim ai(1) As Integer
            Dim pos(3, 3) As Double
            Dim op As orbit = New orbit
            Dim i As Integer
            Dim qjd, p(2) As Double

            If Not m_bTypeValid Then Throw New Exceptions.ValueNotSetException("Kepler:GetPositionAndVelocity Body type has not been set")

            Select Case m_Type
                Case KEPLER.BodyType.kepMajorPlanet 'MAJOR PLANETS [unimpl. SUN, MOON]
                    Select Case m_Number
                        Case PlanetNumber.kepMercury
                            op = mercury
                        Case PlanetNumber.kepVenus
                            op = venus
                        Case PlanetNumber.kepEarth
                            op = earth
                        Case PlanetNumber.kepMars
                            op = mars
                        Case PlanetNumber.kepjupiter
                            op = jupiter
                        Case PlanetNumber.kepSaturn
                            op = saturn
                        Case PlanetNumber.kepUranus
                            op = uranus
                        Case PlanetNumber.kepNeptune
                            op = neptune
                        Case PlanetNumber.kepPluto
                            op = pluto
                        Case Else
                            Throw New Exceptions.InvalidValueException("Kepler:GetPositionAndVelocity Invalid value for planet number: " & m_Number)
                    End Select

                Case KEPLER.BodyType.kepMinorPlanet 'MINOR PLANET
                    '//TODO: Check elements
                    op = m_e

                Case KEPLER.BodyType.kepComet 'COMET
                    '//TODO: Check elements
                    op = m_e
            End Select
            For i = 0 To 2
                qjd = tjd + CDbl(i - 1) * DTVEL
                KeplerCalc(qjd, op, p)
                pos(i, 0) = p(0)
                pos(i, 1) = p(1)
                pos(i, 2) = p(2)
            Next

            'pos(1,x) contains the pos vector
            'pos(0,x) and pos(2,x) are used to determine the velocity based on position change with time!
            For i = 0 To 2
                posvec(i) = pos(1, i)
                posvec(3 + i) = (pos(2, i) - pos(0, i)) / (2.0 * DTVEL)
            Next

            Return posvec
        End Function

        ''' <summary>
        ''' Absolute visual magnitude
        ''' </summary>
        ''' <value>Absolute visual magnitude</value>
        ''' <returns>Absolute visual magnitude</returns>
        ''' <remarks></remarks>
        Public Property H() As Double Implements KEPLER.IEphemeris.H
            Get
                Throw New Exceptions.ValueNotAvailableException("Kepler:H Read - Visual magnitude calculation not implemented")
            End Get
            Set(ByVal value As Double)
                Throw New Exceptions.ValueNotAvailableException("Kepler:H Write - Visual magnitude calculation not implemented")
            End Set
        End Property

        ''' <summary>
        ''' The J2000.0 inclination (deg.)
        ''' </summary>
        ''' <value>The J2000.0 inclination</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public Property Incl() As Double Implements KEPLER.IEphemeris.Incl
            Get
                Return m_e.i
            End Get
            Set(ByVal value As Double)
                m_e.i = value
            End Set
        End Property

        ''' <summary>
        ''' Mean anomaly at the epoch
        ''' </summary>
        ''' <value>Mean anomaly at the epoch</value>
        ''' <returns>Mean anomaly at the epoch</returns>
        ''' <remarks></remarks>
        Public Property M() As Double Implements KEPLER.IEphemeris.M
            Get
                Return m_e.M
            End Get
            Set(ByVal value As Double)
                m_e.M = value
            End Set
        End Property

        ''' <summary>
        ''' Mean daily motion (deg/day)
        ''' </summary>
        ''' <value>Mean daily motion</value>
        ''' <returns>Degrees per day</returns>
        ''' <remarks></remarks>
        Public Property n() As Double Implements KEPLER.IEphemeris.n
            Get
                Return m_e.dm
            End Get
            Set(ByVal value As Double)
                m_e.dm = value
            End Set
        End Property

        ''' <summary>
        ''' The name of the body.
        ''' </summary>
        ''' <value>The name of the body or packed MPC designation</value>
        ''' <returns>The name of the body or packed MPC designation</returns>
        ''' <remarks>If this instance represents an unnumbered minor planet, Ephemeris.Name must be the 
        ''' packed MPC designation. For other types, this is for display only.</remarks>
        Public Property Name() As String Implements KEPLER.IEphemeris.Name
            Get
                If m_Name = "" Then Throw New Exceptions.ValueNotSetException("KEPLER:Name Name has not been set")
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property

        ''' <summary>
        ''' The J2000.0 longitude of the ascending node (deg.)
        ''' </summary>
        ''' <value>The J2000.0 longitude of the ascending node</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public Property Node() As Double Implements KEPLER.IEphemeris.Node
            Get
                Return m_e.W
            End Get
            Set(ByVal value As Double)
                m_e.W = value
            End Set
        End Property

        ''' <summary>
        ''' The major or minor planet number
        ''' </summary>
        ''' <value>The major or minor planet number</value>
        ''' <returns>Number or zero if not numbered</returns>
        ''' <remarks></remarks>
        Public Property Number() As KEPLER.PlanetNumber Implements KEPLER.IEphemeris.Number
            Get
                If Not m_bNumberValid Then Throw New Exceptions.ValueNotSetException("KEPLER:Number Planet number has not been set")
                Return m_Number
            End Get
            Set(ByVal value As KEPLER.PlanetNumber)
                m_Number = value
            End Set
        End Property

        ''' <summary>
        ''' Orbital period (years)
        ''' </summary>
        ''' <value>Orbital period</value>
        ''' <returns>Years</returns>
        ''' <remarks></remarks>
        Public Property P() As Double Implements KEPLER.IEphemeris.P
            Get
                Throw New Exceptions.ValueNotAvailableException("Kepler:P Read - Orbital period calculation not implemented")
            End Get
            Set(ByVal value As Double)
                Throw New Exceptions.ValueNotAvailableException("Kepler:P Write - Orbital period calculation not implemented")
            End Set
        End Property

        ''' <summary>
        ''' The J2000.0 argument of perihelion (deg.)
        ''' </summary>
        ''' <value>The J2000.0 argument of perihelion</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public Property Peri() As Double Implements KEPLER.IEphemeris.Peri
            Get
                Return m_e.wp
            End Get
            Set(ByVal value As Double)
                m_e.wp = value
            End Set
        End Property

        ''' <summary>
        ''' Perihelion distance (AU)
        ''' </summary>
        ''' <value>Perihelion distance</value>
        ''' <returns>AU</returns>
        ''' <remarks></remarks>
        Public Property q() As Double Implements KEPLER.IEphemeris.q
            Get
                Return m_e.a
            End Get
            Set(ByVal value As Double)
                m_e.a = value
            End Set
        End Property

        ''' <summary>
        ''' Reciprocal semi-major axis (1/AU)
        ''' </summary>
        ''' <value>Reciprocal semi-major axis</value>
        ''' <returns>1/AU</returns>
        ''' <remarks></remarks>
        Public Property z() As Double Implements KEPLER.IEphemeris.z
            Get
                Return 1.0 / m_e.a
            End Get
            Set(ByVal value As Double)
                m_e.a = 1.0 / value
            End Set
        End Property
    End Class
End Namespace