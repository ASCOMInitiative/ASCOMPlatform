'NOVASCOM component implementation

Option Strict On
Imports ASCOM.Astrometry.NOVAS.NOVAS2
Imports System.Math
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities
Imports ASCOM.Astrometry.NOVAS
Imports ASCOM.Astrometry
Imports ASCOM.Astrometry.Kepler

Namespace NOVASCOM

#Region "NOVASCOM Implementation"
    ''' <summary>
    ''' NOVAS-COM: Represents the "state" of the Earth at a given Terrestrial Julian date
    ''' </summary>
    ''' <remarks>NOVAS-COM objects of class Earth represent the "state" of the Earth at a given Terrestrial Julian date. 
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
    <Guid("6BD93BA2-79C5-4077-9630-B7C6E30B2FDF"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class Earth
        Implements IEarth

        Private m_BaryPos, m_HeliPos As New PositionVector
        Private m_BaryVel, m_HeliVel As New VelocityVector
        Private m_BaryTime, m_MeanOb, m_EquOfEqu, m_NutLong, m_NutObl, m_TrueOb As Double
        Private m_EarthEph As IEphemeris
        Private m_Valid As Boolean 'Object has valid values
        'Private TL As TraceLogger
        ''' <summary>
        ''' Create a new instance of the Earth object
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            'TL = New Utilities.TraceLogger("", "Earth")
            'TL.Enabled = True
            'TL.LogMessage("New", "Start")
            m_EarthEph = New Ephemeris
            m_EarthEph.BodyType = BodyType.MajorPlanet
            m_EarthEph.Number = Body.Earth
            m_EarthEph.Name = "Earth"
            m_Valid = False 'Object is invalid
            'TL.LogMessage("New", "Initialised")
        End Sub

        ''' <summary>
        ''' Earth barycentric position
        ''' </summary>
        ''' <value>Barycentric position vector</value>
        ''' <returns>AU (Ref J2000)</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BarycentricPosition() As PositionVector Implements IEarth.BarycentricPosition
            Get
                Return m_BaryPos
            End Get
        End Property

        ''' <summary>
        ''' Earth barycentric time 
        ''' </summary>
        ''' <value>Barycentric dynamical time for given Terrestrial Julian Date</value>
        ''' <returns>Julian date</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BarycentricTime() As Double Implements IEarth.BarycentricTime
            Get
                Return m_BaryTime
            End Get
        End Property

        ''' <summary>
        ''' Earth barycentric velocity 
        ''' </summary>
        ''' <value>Barycentric velocity vector</value>
        ''' <returns>AU/day (ref J2000)</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BarycentricVelocity() As VelocityVector Implements IEarth.BarycentricVelocity
            Get
                Return m_BaryVel
            End Get
        End Property

        ''' <summary>
        ''' Ephemeris object used to provide the position of the Earth.
        ''' </summary>
        ''' <value>Earth ephemeris object </value>
        ''' <returns>Earth ephemeris object</returns>
        ''' <remarks>
        ''' Setting this is optional, if not set, the internal Kepler engine will be used.</remarks>
        Public Property EarthEphemeris() As IEphemeris Implements IEarth.EarthEphemeris
            Get
                Return m_EarthEph
            End Get
            Set(ByVal value As IEphemeris)
                m_EarthEph = value
            End Set
        End Property

        ''' <summary>
        ''' Earth equation of equinoxes 
        ''' </summary>
        ''' <value>Equation of the equinoxes</value>
        ''' <returns>Seconds</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EquationOfEquinoxes() As Double Implements IEarth.EquationOfEquinoxes
            Get
                Return m_EquOfEqu
            End Get
        End Property

        ''' <summary>
        ''' Earth heliocentric position
        ''' </summary>
        ''' <value>Heliocentric position vector</value>
        ''' <returns>AU (ref J2000)</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HeliocentricPosition() As PositionVector Implements IEarth.HeliocentricPosition
            Get
                Return m_HeliPos
            End Get
        End Property

        ''' <summary>
        ''' Earth heliocentric velocity 
        ''' </summary>
        ''' <value>Heliocentric velocity</value>
        ''' <returns>Velocity vector, AU/day (ref J2000)</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HeliocentricVelocity() As VelocityVector Implements IEarth.HeliocentricVelocity
            Get
                Return m_HeliVel
            End Get
        End Property

        ''' <summary>
        ''' Earth mean objiquity
        ''' </summary>
        ''' <value>Mean obliquity of the ecliptic</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MeanObliquity() As Double Implements IEarth.MeanObliquity
            Get
                Return m_MeanOb
            End Get
        End Property

        ''' <summary>
        ''' Earth nutation in longitude 
        ''' </summary>
        ''' <value>Nutation in longitude</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NutationInLongitude() As Double Implements IEarth.NutationInLongitude
            Get
                Return m_NutLong
            End Get
        End Property

        ''' <summary>
        ''' Earth nutation in obliquity 
        ''' </summary>
        ''' <value>Nutation in obliquity</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NutationInObliquity() As Double Implements IEarth.NutationInObliquity
            Get
                Return m_NutObl
            End Get
        End Property

        ''' <summary>
        ''' Initialize the Earth object for given terrestrial Julian date
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian date</param>
        ''' <returns>True if successful, else throws an exception</returns>
        ''' <remarks></remarks>
        Public Function SetForTime(ByVal tjd As Double) As Boolean Implements IEarth.SetForTime
            Dim m_peb(2), m_veb(2), m_pes(2), m_ves(2) As Double
            'TL.LogMessage("SetForTime", "Start")
            get_earth_nov(m_EarthEph, tjd, m_BaryTime, m_peb, m_veb, m_pes, m_ves)
            'TL.LogMessage("SetForTime", "After get_earth_nov")
            EarthTilt(tjd, m_MeanOb, m_TrueOb, m_EquOfEqu, m_NutLong, m_NutObl)
            'TL.LogMessage("SetForTime", "After earthtilt")
            m_BaryPos.x = m_peb(0)
            m_BaryPos.y = m_peb(1)
            m_BaryPos.z = m_peb(2)
            m_BaryVel.x = m_veb(0)
            m_BaryVel.y = m_veb(1)
            m_BaryVel.z = m_veb(2)

            m_HeliPos.x = m_pes(0)
            m_HeliPos.y = m_pes(1)
            m_HeliPos.z = m_pes(2)
            m_HeliVel.x = m_ves(0)
            m_HeliVel.y = m_ves(1)
            m_HeliVel.z = m_ves(2)

            m_Valid = True
            'Dim Earth As New bodystruct, POS(2), VEL(2) As Double
            'Earth.name = "Earth"
            'Earth.number = Body.Earth
            'Earth.type = NOVAS2Net.BodyType.MajorPlanet
            'ephemeris(tjd, Earth, Origin.SolarSystemBarycentre, POS, VEL)
            'm_BaryPos.x = POS(0)
            'm_BaryPos.y = POS(1)
            'm_BaryPos.z = POS(2)
            'm_BaryVel.x = VEL(0)
            'm_BaryVel.y = VEL(1)
            'm_BaryVel.z = VEL(2)
            'm_HeliPos.x = 0.0
            'm_HeliPos.y = 0.0
            'm_HeliPos.z = 0.0
            'm_HeliVel.x = 0.0
            'm_HeliVel.y = 0.0
            'm_HeliVel.z = 0.0
            'TL.LogMessage("SetForTime", "BaryPos x" & m_peb(0)) '& " " & POS(0))
            'TL.LogMessage("SetForTime", "BaryPos y" & m_peb(1)) '& " " & POS(1))
            'TL.LogMessage("SetForTime", "BaryPos z" & m_peb(2)) '& " " & POS(2))
            'TL.LogMessage("SetForTime", "BaryVel x" & m_veb(0)) '& " " & VEL(0))
            'TL.LogMessage("SetForTime", "BaryVel y" & m_veb(1)) '& " " & VEL(1))
            'TL.LogMessage("SetForTime", "BaryVel z" & m_veb(2)) '& " " & VEL(2))

            Return m_Valid
        End Function

        ''' <summary>
        ''' Earth true obliquity 
        ''' </summary>
        ''' <value>True obliquity of the ecliptic</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TrueObliquity() As Double Implements IEarth.TrueObliquity
            Get
                Return m_TrueOb
            End Get
        End Property
    End Class

    ''' <summary>
    ''' NOVAS-COM: Provide characteristics of a solar system body
    ''' </summary>
    ''' <remarks>NOVAS-COM objects of class Planet hold the characteristics of a solar system body. Properties are 
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
    <Guid("78F157E4-D03D-4efb-8248-745F9C63A850"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class Planet
        Implements IPlanet

        Private m_deltat As Double
        Private m_bDTValid As Boolean
        Private m_type As BodyType
        Private m_number As Integer
        Private m_name As String
        Private m_ephobj As IEphemeris
        Private m_ephdisps(4), m_earthephdisps(4) As Integer
        Private m_earthephobj As IEphemeris

        'Private TL As TraceLogger

        ''' <summary>
        ''' Create a new instance of the Plant class
        ''' </summary>
        ''' <remarks>This assigns default Kepler instances for the Earth and Planet objects so it is
        ''' not necessary to create and attach Kepler objects in order to get Kepler accuracy from this
        ''' component</remarks>
        Public Sub New()
            m_name = Nothing
            m_bDTValid = False
            m_ephobj = New Ephemeris
            m_earthephobj = New Ephemeris
            m_earthephobj.BodyType = BodyType.MajorPlanet
            m_earthephobj.Name = "Earth"
            m_earthephobj.Number = Body.Earth
            'TL = New TraceLogger("", "NOVASCOM")
            'TL.Enabled = True
            'TL.LogMessage("New", "Log started")
        End Sub
        ''' <summary>
        ''' Planet delta-T
        ''' </summary>
        ''' <value>The value of delta-T (TT - UT1) to use for reductions</value>
        ''' <returns>Seconds</returns>
        ''' <remarks>Setting this value is optional. If no value is set, an internal delta-T generator is used.</remarks>
        Public Property DeltaT() As Double Implements IPlanet.DeltaT
            Get
                If Not m_bDTValid Then Throw New Exceptions.ValueNotAvailableException("Planet:DeltaT DeltaT is not available")
                Return m_deltat
            End Get
            Set(ByVal value As Double)
                m_deltat = value
                m_bDTValid = True
            End Set
        End Property

        ''' <summary>
        ''' Ephemeris object used to provide the position of the Earth.
        ''' </summary>
        ''' <value>Earth ephemeris object</value>
        ''' <returns>Earth ephemeris object</returns>
        ''' <remarks>
        ''' Setting this is optional, if not set, the internal Kepler engine will be used.</remarks>
        Public Property EarthEphemeris() As IEphemeris Implements IPlanet.EarthEphemeris
            Get
                Return m_earthephobj
            End Get
            Set(ByVal value As IEphemeris)
                m_earthephobj = value
            End Set
        End Property

        ''' <summary>
        ''' The Ephemeris object used to provide positions of solar system bodies.
        ''' </summary>
        ''' <value>Body ephemeris object</value>
        ''' <returns>Body ephemeris object</returns>
        ''' <remarks>
        ''' Setting this is optional, if not set, the internal Kepler engine will be used.
        ''' </remarks>
        Public Property Ephemeris() As IEphemeris Implements IPlanet.Ephemeris
            Get
                Return m_ephobj
            End Get
            Set(ByVal value As IEphemeris)
                m_ephobj = value
            End Set
        End Property

        ''' <summary>
        ''' Get an apparent position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the apparent place.</returns>
        ''' <remarks></remarks>
        Public Function GetApparentPosition(ByVal tjd As Double) As PositionVector Implements IPlanet.GetApparentPosition
            Dim tdb, peb(3), veb(3), pes(3), ves(3), t2, t3, lighttime, _
                pos1(3), vel1(3), pos2(3), pos3(3), pos4(3), pos5(3), vec(8) As Double
            Dim iter As Integer, pv As New PositionVector

            '//
            '// This gets the barycentric terrestrial dynamical time (TDB).
            '//
            get_earth_nov(m_earthephobj, tjd, tdb, peb, veb, pes, ves)

            '//
            '// Get position and velocity of planet wrt barycenter of solar system.
            '//

            ephemeris_nov(m_ephobj, tdb, m_type, m_number, m_name, Origin.Barycentric, pos1, vel1)

            BaryToGeo(pos1, peb, pos2, lighttime)
            t3 = tdb - lighttime

            iter = 0
            Do
                t2 = t3
                ephemeris_nov(m_ephobj, t2, m_type, m_number, m_name, Origin.Barycentric, pos1, vel1)
                BaryToGeo(pos1, peb, pos2, lighttime)
                t3 = tdb - lighttime
                iter += 1
            Loop While ((Abs(t3 - t2) > 0.000001) And iter < 100)

            '//
            '// Finish apparent place computation.
            '//
            SunField(pos2, pes, pos3)
            Aberration(pos3, veb, lighttime, pos4)
            Precession(T0, pos4, tdb, pos5)
            Nutate(tdb, NutationDirection.MeanToTrue, pos5, vec)

            pv.x = vec(0)
            pv.y = vec(1)
            pv.z = vec(2)
            Return pv
        End Function

        ''' <summary>
        ''' Get an astrometric position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the astrometric place.</returns>
        ''' <remarks></remarks>
        Public Function GetAstrometricPosition(ByVal tjd As Double) As PositionVector Implements IPlanet.GetAstrometricPosition
            Dim t2, t3 As Double
            Dim lighttime, pos1(3), vel1(3), pos2(3), tdb, peb(3), veb(3), pes(3), ves(3) As Double
            'Dim tdbe As Double
            Dim iter As Integer
            'Dim earth As New bodystruct
            Dim RetVal As PositionVector
            'Dim pebe(3), pese(3), vebe(3), vese(3) As Double
            '//
            '// Get position of the Earth wrt center of Sun and barycenter of the
            '// solar system.
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//
            'earth.name = "Earth"
            'earth.number = Body.Earth
            'earth.type = NOVAS2Net.BodyType.MajorPlanet

            'hr = get_earth(tjd, earth, tdbe, pebe, vebe, pese, vese)

            get_earth_nov(m_earthephobj, tjd, tdb, peb, veb, pes, ves)

            'TL.LogMessage("GetAstrometricPosition", "tjd: " & tjd)
            'TL.LogMessage("GetAstrometricPosition", "tdb: " & tdb & " " & tdbe)
            'TL.LogMessage("GetAstrometricPosition", "get_earth peb(0): " & peb(0))
            'TL.LogMessage("GetAstrometricPosition", "get_earth peb(1): " & peb(1))
            'TL.LogMessage("GetAstrometricPosition", "get_earth peb(2): " & peb(2))
            'TL.LogMessage("GetAstrometricPosition", "get_earth veb(0): " & veb(0))
            'TL.LogMessage("GetAstrometricPosition", "get_earth veb(1): " & veb(1))
            'TL.LogMessage("GetAstrometricPosition", "get_earth veb(2): " & veb(2))
            'TL.LogMessage("GetAstrometricPosition", "get_earth pes(0): " & pes(0))
            'TL.LogMessage("GetAstrometricPosition", "get_earth pes(1): " & pes(1))
            'TL.LogMessage("GetAstrometricPosition", "get_earth pes(2): " & pes(2))
            'TL.LogMessage("GetAstrometricPosition", "get_earth ves(0): " & ves(0))
            'TL.LogMessage("GetAstrometricPosition", "get_earth ves(1): " & ves(1))
            'TL.LogMessage("GetAstrometricPosition", "get_earth ves(2): " & ves(2))

            '//
            '// Get position and velocity of planet wrt barycenter of solar system.
            '//

            ephemeris_nov(m_ephobj, tdb, m_type, m_number, m_name, _
                          Origin.Barycentric, pos1, vel1)
            'TL.LogMessage("GetAstrometricPosition", "tdb: " & tdb)

            BaryToGeo(pos1, peb, pos2, lighttime)
            t3 = tdb - lighttime

            iter = 0
            Do
                t2 = t3
                ephemeris_nov(m_ephobj, t2, m_type, m_number, m_name, Origin.Barycentric, pos1, vel1)
                BaryToGeo(pos1, peb, pos2, lighttime)
                t3 = tdb - lighttime
                iter += 1
            Loop While ((Abs(t3 - t2) > 0.000001) And (iter < 100))

            If (iter >= 100) Then Throw New Utilities.Exceptions.HelperException("Planet:GetAstrometricPoition ephemeris_nov did not converge in 100 iterations")

            '//
            '// pos2 is astrometric place.
            '//
            RetVal = New PositionVector()
            RetVal.x = pos2(0)
            RetVal.y = pos2(1)
            RetVal.z = pos2(2)

            Return RetVal

        End Function

        ''' <summary>
        ''' Get an local position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">The observing site</param>
        ''' <returns>PositionVector for the local place.</returns>
        ''' <remarks></remarks>
        Public Function GetLocalPosition(ByVal tjd As Double, ByVal site As Site) As PositionVector Implements IPlanet.GetLocalPosition
            Dim j, iter As Integer
            Dim st As SiteInfo
            Dim t2, t3 As Double
            Dim gast, lighttime, ujd, pog(3), vog(3), pb(3), vb(3), ps(3), _
             vs(3), pos1(3), vel1(3), pos2(3), vel2(3), pos3(3), vec(3), _
             tdb, peb(3), veb(3), pes(3), ves(3), oblm, oblt, eqeq, psi, eps As Double
            Dim pv As PositionVector

            '//
            '// Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
            '//
            If m_bDTValid Then
                ujd = tjd - m_deltat
            Else
                ujd = tjd - DeltaTCalc(tjd) / 86400.0
            End If

            '//
            '// Get the observer's site info
            '//
            Try
                st.Latitude = site.Latitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available")
            End Try
            Try
                st.Longitude = site.Longitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available")
            End Try
            Try
                st.Height = site.Height
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available")
            End Try
            '//
            '// Get position of Earth wrt the center of the Sun and the barycenter
            '// of solar system.
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//
            get_earth_nov(m_earthephobj, tjd, tdb, peb, veb, pes, ves)

            EarthTilt(tdb, oblm, oblt, eqeq, psi, eps)

            '//
            '// Get position and velocity of observer wrt center of the Earth.
            '//
            SiderealTime(ujd, 0.0, eqeq, gast)
            Terra(st, gast, pos1, vel1)
            Nutate(tdb, NutationDirection.TrueToMean, pos1, pos2)
            Precession(tdb, pos2, T0, pog)

            Nutate(tdb, NutationDirection.TrueToMean, vel1, vel2)
            Precession(tdb, vel2, T0, vog)

            '//
            '// Get position and velocity of observer wrt barycenter of solar 
            '// system and wrt center of the sun.
            '//
            For j = 0 To 2
                pb(j) = peb(j) + pog(j)
                vb(j) = veb(j) + vog(j)
                ps(j) = pes(j) + pog(j)
                vs(j) = ves(j) + vog(j)
            Next

            '//
            '// Get position of planet wrt barycenter of solar system.
            '//
            ephemeris_nov(m_ephobj, tdb, m_type, m_number, m_name, Origin.Barycentric, pos1, vel1)

            BaryToGeo(pos1, pb, pos2, lighttime)
            t3 = tdb - lighttime

            iter = 0
            Do
                t2 = t3
                ephemeris_nov(m_ephobj, t2, m_type, m_number, m_name, Origin.Barycentric, pos1, vel1)
                BaryToGeo(pos1, pb, pos2, lighttime)
                t3 = tdb - lighttime
                iter += 1
            Loop While ((Abs(t3 - t2) > 0.000001) And (iter < 100))

            If (iter >= 100) Then Throw New Utilities.Exceptions.HelperException("Planet:GetLocalPoition ephemeris_nov did not converge in 100 iterations")

            '//
            '// Finish local place calculation.
            '//
            SunField(pos2, ps, pos3)
            Aberration(pos3, vb, lighttime, vec)

            pv = New PositionVector
            pv.x = vec(0)
            pv.y = vec(1)
            pv.z = vec(2)

            Return pv
        End Function

        ''' <summary>
        ''' Get a topocentric position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">The observing site</param>
        ''' <param name="Refract">Apply refraction correction</param>
        ''' <returns>PositionVector for the topocentric place.</returns>
        ''' <remarks></remarks>
        Public Function GetTopocentricPosition(ByVal tjd As Double, ByVal site As Site, ByVal Refract As Boolean) As PositionVector Implements IPlanet.GetTopocentricPosition
            Dim j As Short, iter As Integer
            Dim ref As RefractionOption
            Dim st As SiteInfo
            Dim ujd, t2, t3, gast, pos1(3), pos2(3), pos4(3), pos5(3), _
             pos6(3), vel1(3), vel2(3), pog(3), vog(3), pob(3), vec(3), _
             vob(3), pos(3), lighttime, tdb, peb(3), veb(3), pes(3), ves(3), _
             oblm, oblt, eqeq, psi, eps As Double
            Dim ra, rra, dec, rdec, az, zd, dist As Double
            Dim wx As Boolean, pv As PositionVector

            '//
            '// Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
            '//
            If m_bDTValid Then
                ujd = tjd - m_deltat
            Else
                ujd = tjd - DeltaTCalc(tjd) / 86400.0
            End If

            '//
            '// Get the observer's site info
            '//
            Try
                st.Latitude = site.Latitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available")
            End Try
            Try
                st.Longitude = site.Longitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available")
            End Try
            Try
                st.Height = site.Height
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available")
            End Try

            '//
            '// Compute position and velocity of the observer, on mean equator
            '// and equinox of J2000.0, wrt the solar system barycenter and
            '// wrt to the center of the Sun. 
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//
            get_earth_nov(m_earthephobj, tjd, tdb, peb, veb, pes, ves)

            EarthTilt(tdb, oblm, oblt, eqeq, psi, eps)

            '//
            '// Get position and velocity of observer wrt center of the Earth.
            '//
            SiderealTime(ujd, 0.0, eqeq, gast)
            Terra(st, gast, pos1, vel1)
            Nutate(tdb, NutationDirection.TrueToMean, pos1, pos2)
            Precession(tdb, pos2, T0, pog)

            Nutate(tdb, NutationDirection.TrueToMean, vel1, vel2)
            Precession(tdb, vel2, T0, vog)

            '//
            '// Get position and velocity of observer wrt barycenter of solar system
            '// and wrt center of the sun.
            '//
            For j = 0 To 2

                pob(j) = peb(j) + pog(j)
                vob(j) = veb(j) + vog(j)
                pos(j) = pes(j) + pog(j)
            Next

            '// 
            '// Compute the apparent place of the planet using the position and
            '// velocity of the observer.
            '//
            '// First, get the position of the planet wrt barycenter of solar system.
            '//
            ephemeris_nov(m_ephobj, tdb, m_type, m_number, m_name, Origin.Barycentric, pos1, vel1)

            BaryToGeo(pos1, pob, pos2, lighttime)
            t3 = tdb - lighttime

            iter = 0
            Do
                t2 = t3
                ephemeris_nov(m_ephobj, t2, m_type, m_number, m_name, Origin.Barycentric, pos1, vel1)
                BaryToGeo(pos1, pob, pos2, lighttime)
                t3 = tdb - lighttime
                iter += 1
            Loop While ((Abs(t3 - t2) > 0.000001) And (iter < 100))

            If (iter >= 100) Then Throw New Utilities.Exceptions.HelperException("Planet:GetTopocentricPoition ephemeris_nov did not converge in 100 iterations")

            '//
            '// Finish topocentric place calculation.
            '//
            SunField(pos2, pos, pos4)
            Aberration(pos4, vob, lighttime, pos5)
            Precession(T0, pos5, tdb, pos6)
            Nutate(tdb, NutationDirection.MeanToTrue, pos6, vec)

            '//
            '// Calculate equatorial coordinates and distance
            '//
            Vector2RADec(vec, ra, dec) 'Get topo RA/Dec
            dist = Sqrt(Pow(vec(0), 2.0) + Pow(vec(1), 2.0) + Pow(vec(2), 2.0)) 'And dist

            '//
            '// Refract if requested
            '//
            ref = RefractionOption.NoRefraction 'Assume no refraction
            If Refract Then
                wx = True 'Assume site weather
                Try
                    st.Temperature = site.Temperature
                Catch ex As Exception 'Value unset so use standard refraction option
                    wx = False
                End Try
                Try
                    st.Pressure = site.Pressure
                Catch ex As Exception 'Value unset so use standard refraction option
                    wx = False
                End Try
                If wx Then 'Set refraction option
                    ref = RefractionOption.LocationRefraction
                Else
                    ref = RefractionOption.StandardRefraction
                End If
            End If
            '//
            '// This calculates Alt/Az coordinates. If ref > 0 then it refracts
            '// both the computed Alt/Az and the RA/Dec coordinates.
            '//
            If m_bDTValid Then
                Equ2Hor(tjd, m_deltat, 0.0, 0.0, st, ra, dec, ref, zd, az, rra, rdec)
            Else
                Equ2Hor(tjd, DeltaTCalc(tjd), 0.0, 0.0, st, ra, dec, ref, zd, az, rra, rdec)
            End If

            '//
            '// If we refracted, we now must compute new cartesian components
            '// Distance does not change...
            '//
            If (ref <> RefractionOption.NoRefraction) Then RADec2Vector(rra, rdec, dist, vec) 'If refracted, recompute New refracted vector

            'Create a new positionvector with calculated values
            pv = New PositionVector(vec(0), vec(1), vec(2), rra, rdec, dist, dist / C, az, (90.0 - zd))

            Return pv

        End Function
        ''' <summary>
        ''' Get a virtual position for given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the virtual place.</returns>
        ''' <remarks></remarks>
        Public Function GetVirtualPosition(ByVal tjd As Double) As PositionVector Implements IPlanet.GetVirtualPosition
            Dim t2, t3 As Double
            Dim lighttime, pos1(3), vel1(3), pos2(3), pos3(3), vec(3), _
                tdb, peb(3), veb(3), pes(3), ves(3), oblm, oblt, eqeq, psi, eps As Double
            Dim iter As Integer
            Dim pv As New PositionVector
            '//
            '// Get position nd velocity of Earth wrt barycenter of solar system.
            '//
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//
            get_earth_nov(m_earthephobj, tjd, tdb, peb, veb, pes, ves)

            EarthTilt(tdb, oblm, oblt, eqeq, psi, eps)

            '//
            '// Get position and velocity of planet wrt barycenter of solar system.
            '//

            Dim km_type As BodyType
            Select Case m_type
                Case BodyType.Comet
                    km_type = BodyType.Comet
                Case BodyType.MajorPlanet
                    km_type = BodyType.MajorPlanet
                Case BodyType.MinorPlanet
                    km_type = BodyType.MinorPlanet
            End Select

            ephemeris_nov(m_ephobj, tdb, km_type, m_number, m_name, Origin.Barycentric, pos1, vel1)
            BaryToGeo(pos1, peb, pos2, lighttime)

            t3 = tdb - lighttime

            iter = 0
            Do
                t2 = t3
                ephemeris_nov(m_ephobj, t2, km_type, m_number, m_name, Origin.Barycentric, pos1, vel1)
                BaryToGeo(pos1, peb, pos2, lighttime)
                t3 = tdb - lighttime
                iter += 1
            Loop While ((Abs(t3 - t2) > 0.000001) And (iter < 100))

            If (iter >= 100) Then Throw New Utilities.Exceptions.HelperException("Planet:GetVirtualPoition ephemeris_nov did not converge in 100 iterations")

            '//
            '// Finish virtual place computation.
            '//
            SunField(pos2, pes, pos3)

            Aberration(pos3, veb, lighttime, vec)

            pv.x = vec(0)
            pv.y = vec(1)
            pv.z = vec(2)
            Return pv

        End Function

        ''' <summary>
        ''' Planet name
        ''' </summary>
        ''' <value>For unnumbered minor planets, (Type=nvMinorPlanet and Number=0), the packed designation 
        ''' for the minor planet. For other types, this is not significant, but may be used to store 
        ''' a name.</value>
        ''' <returns>Name of planet</returns>
        ''' <remarks></remarks>
        Public Property Name() As String Implements IPlanet.Name
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        ''' <summary>
        ''' Planet number
        ''' </summary>
        ''' <value>For major planets (Type=nvMajorPlanet), a PlanetNumber value. For minor planets 
        ''' (Type=nvMinorPlanet), the number of the minor planet or 0 for unnumbered minor planet.</value>
        ''' <returns>Planet number</returns>
        ''' <remarks>The major planet number is its number out from the sun starting with Mercury = 1</remarks>
        Public Property Number() As Integer Implements IPlanet.Number
            Get
                Return m_number
            End Get
            Set(ByVal value As Integer)
                If ((m_type = BodyType.MajorPlanet) And ((value < 0) Or (value > 11))) Then Throw New Utilities.Exceptions.InvalidValueException("Planet.Number MajorPlanet number is < 0 or > 11 - " & value)
                m_number = value
            End Set
        End Property

        ''' <summary>
        ''' The type of solar system body
        ''' </summary>
        ''' <value>The type of solar system body</value>
        ''' <returns>Value from the BodyType enum</returns>
        ''' <remarks></remarks>
        Public Property Type() As BodyType Implements IPlanet.Type
            Get
                Return m_type
            End Get
            Set(ByVal value As BodyType)
                If (value < 0) Or (value > 2) Then Throw New Utilities.Exceptions.InvalidValueException("Planet.Type BodyType is < 0 or > 2: " & value)
                m_type = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' NOVAS-COM: PositionVector Class
    ''' </summary>
    ''' <remarks>NOVAS-COM objects of class PositionVector contain vectors used for positions (earth, sites, 
    ''' stars and planets) throughout NOVAS-COM. Of course, its properties include the x, y, and z 
    ''' components of the position. Additional properties are right ascension and declination, distance, 
    ''' and light time (applicable to star positions), and Alt/Az (available only in PositionVectors 
    ''' returned by Star or Planet methods GetTopocentricPosition()). You can initialize a PositionVector 
    ''' from a Star object (essentially an FK5 or HIP catalog entry) or a Site (lat/long/height). 
    ''' PositionVector has methods that can adjust the coordinates for precession, aberration and 
    ''' proper motion. Thus, a PositionVector object gives access to some of the lower-level NOVAS functions. 
    ''' <para><b>Note:</b> The equatorial coordinate properties of this object are dependent variables, and thus are read-only. Changing any cartesian coordinate will cause the equatorial coordinates to be recalculated. 
    ''' </para></remarks>
    <Guid("8D8B7043-49AA-40be-881F-0EC5D8E2213D"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class PositionVector
        Implements IPositionVector, IPositionVectorExtra
        Private xOk, yOk, zOk, RADecOk, AzElOk As Boolean
        Private PosVec(2), m_RA, m_DEC, m_Dist, m_Light, m_Alt, m_Az As Double

        ''' <summary>
        ''' Create a new, uninitialised position vector
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            xOk = False
            yOk = False
            zOk = False
            RADecOk = False
            AzElOk = False
        End Sub

        ''' <summary>
        '''  Create a new position vector with supplied initial values
        ''' </summary>
        ''' <param name="x">Position vector x co-ordinate</param>
        ''' <param name="y">Position vector y co-ordinate</param>
        ''' <param name="z">Position vector z co-ordinate</param>
        ''' <param name="RA">Right ascension (hours)</param>
        ''' <param name="DEC">Declination (degrees)</param>
        ''' <param name="Distance">Distance to object</param>
        ''' <param name="Light">Light-time to object</param>
        ''' <param name="Azimuth">Object azimuth</param>
        ''' <param name="Altitude">Object altitude</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal x As Double, _
                               ByVal y As Double, _
                               ByVal z As Double, _
                               ByVal RA As Double, _
                               ByVal DEC As Double, _
                               ByVal Distance As Double, _
                               ByVal Light As Double, _
                               ByVal Azimuth As Double, _
                               ByVal Altitude As Double)
            PosVec(0) = x
            xOk = True
            PosVec(1) = y
            yOk = True
            PosVec(2) = z
            zOk = True
            m_RA = RA
            m_DEC = DEC
            RADecOk = True
            m_Dist = Distance
            m_Light = Light
            m_Az = Azimuth
            m_Alt = Altitude
            AzElOk = True
        End Sub

        ''' <summary>
        ''' Adjust the position vector of an object for aberration of light
        ''' </summary>
        ''' <param name="vel">The velocity vector of the observer</param>
        ''' <remarks>The algorithm includes relativistic terms</remarks>
        Public Sub Aberration(ByVal vel As VelocityVector) Implements IPositionVector.Aberration
            Dim p(2), v(2) As Double
            If Not (xOk And yOk And zOk) Then Throw New Exceptions.ValueNotSetException("PositionVector:ProperMotion x, y or z has not been set")
            CheckEq()
            p(0) = PosVec(0)
            p(1) = PosVec(1)
            p(2) = PosVec(2)

            Try
                v(0) = vel.x
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:Aberration VelocityVector.x is not available")
            End Try
            Try
                v(1) = vel.y
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:Aberration VelocityVector.y is not available")

            End Try
            Try
                v(2) = vel.z
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:Aberration VelocityVector.z is not available")
            End Try
            NOVAS2.Aberration(p, v, m_Light, PosVec)
            RADecOk = False
            AzElOk = False
        End Sub

        ''' <summary>
        ''' The azimuth coordinate (degrees, + east)
        ''' </summary>
        ''' <value>The azimuth coordinate</value>
        ''' <returns>Degrees, + East</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Azimuth() As Double Implements IPositionVector.Azimuth
            Get
                If Not AzElOk Then Throw New Exceptions.ValueNotAvailableException("PositionVector:Azimuth Azimuth is not available")
                Return m_Az
            End Get
        End Property

        ''' <summary>
        ''' Declination coordinate
        ''' </summary>
        ''' <value>Declination coordinate</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Declination() As Double Implements IPositionVector.Declination
            Get
                If Not (xOk And yOk And zOk) Then Throw New Exceptions.ValueNotSetException("PositionVector:Declination x, y or z has not been set")
                CheckEq()
                Return m_DEC
            End Get
        End Property

        ''' <summary>
        ''' Distance/Radius coordinate
        ''' </summary>
        ''' <value>Distance/Radius coordinate</value>
        ''' <returns>AU</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Distance() As Double Implements IPositionVector.Distance
            Get
                If Not (xOk And yOk And zOk) Then Throw New Exceptions.ValueNotSetException("PositionVector:Distance x, y or z has not been set")
                CheckEq()
                Return m_Dist
            End Get
        End Property

        ''' <summary>
        ''' The elevation (altitude) coordinate (degrees, + up)
        ''' </summary>
        ''' <value>The elevation (altitude) coordinate (degrees, + up)</value>
        ''' <returns>(Degrees, + up</returns>
        ''' <remarks>Elevation is available only in PositionVectors returned from calls to 
        ''' Star.GetTopocentricPosition() and/or Planet.GetTopocentricPosition(). </remarks>
        ''' <exception cref="Exceptions.ValueNotAvailableException">When the position vector has not been 
        ''' initialised from Star.GetTopoCentricPosition and Planet.GetTopocentricPosition</exception>
        Public ReadOnly Property Elevation() As Double Implements IPositionVector.Elevation
            Get
                If Not AzElOk Then Throw New Exceptions.ValueNotAvailableException("PositionVector:Elevation Elevation is not available")
                Return m_Alt
            End Get
        End Property

        ''' <summary>
        ''' Light time from body to origin, days.
        ''' </summary>
        ''' <value>Light time from body to origin</value>
        ''' <returns>Days</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LightTime() As Double Implements IPositionVector.LightTime
            Get
                If Not (xOk And yOk And zOk) Then Throw New Exceptions.ValueNotSetException("PositionVector:LightTime x, y or z has not been set")
                CheckEq()
                Return m_Light
            End Get
        End Property

        ''' <summary>
        ''' Adjust the position vector for precession of equinoxes between two given epochs
        ''' </summary>
        ''' <param name="tjd">The first epoch (Terrestrial Julian Date)</param>
        ''' <param name="tjd2">The second epoch (Terrestrial Julian Date)</param>
        ''' <remarks>The coordinates are referred to the mean equator and equinox of the two respective epochs.</remarks>
        Public Sub Precess(ByVal tjd As Double, ByVal tjd2 As Double) Implements IPositionVector.Precess
            Dim p(2) As Double
            If Not (xOk And yOk And zOk) Then Throw New Exceptions.ValueNotSetException("PositionVector:Precess x, y or z has not been set")
            p(0) = PosVec(0)
            p(1) = PosVec(1)
            p(2) = PosVec(2)
            Precession(tjd, p, tjd2, PosVec)
            RADecOk = False
            AzElOk = False
        End Sub

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
        Public Function ProperMotion(ByVal vel As VelocityVector, ByVal tjd1 As Double, ByVal tjd2 As Double) As Boolean Implements IPositionVector.ProperMotion
            Dim p(2), v(2) As Double
            If Not (xOk And yOk And zOk) Then Throw New Exceptions.ValueNotSetException("PositionVector:ProperMotion x, y or z has not been set")
            p(0) = PosVec(0)
            p(1) = PosVec(1)
            p(2) = PosVec(2)
            Try
                v(0) = vel.x
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.x is not available")
            End Try
            Try
                v(1) = vel.y
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.y is not available")
            End Try
            Try
                v(2) = vel.z
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.z is not available")
            End Try
            NOVAS2.ProperMotion(tjd1, p, v, tjd2, PosVec)
            RADecOk = False
            AzElOk = False
        End Function

        ''' <summary>
        ''' RightAscension coordinate, hours
        ''' </summary>
        ''' <value>RightAscension coordinate</value>
        ''' <returns>Hours</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RightAscension() As Double Implements IPositionVector.RightAscension
            Get
                If Not (xOk And yOk And zOk) Then Throw New Exceptions.ValueNotSetException("PositionVector:RA x, y or z has not been set")
                CheckEq()
                Return m_RA
            End Get
        End Property

        ''' <summary>
        ''' Initialize the PositionVector from a Site object and Greenwich apparent sidereal time.
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="gast">Greenwich Apparent Sidereal Time</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks>The GAST parameter must be for Greenwich, not local. The time is rotated through the 
        ''' site longitude. See SetFromSiteJD() for an equivalent method that takes UTC Julian Date and 
        ''' Delta-T (eliminating the need for calculating hyper-accurate GAST yourself).</remarks>
        Public Function SetFromSite(ByVal site As Site, ByVal gast As Double) As Boolean Implements IPositionVector.SetFromSite
            Const f As Double = 0.00335281 '		// f = Earth ellipsoid flattening
            Dim df2, t, sinphi, cosphi, c, s, ach, ash, stlocl, sinst, cosst As Double

            '//
            '// Compute parameters relating to geodetic to geocentric conversion.
            '//
            df2 = Pow((1.0 - f), 2)
            Try
                t = site.Latitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:SetFromSite Site.Latitude is not available")
            End Try

            t = DEG2RAD * t
            sinphi = Sin(t)
            cosphi = Cos(t)
            c = 1.0 / Sqrt(Pow(cosphi, 2.0) + df2 * Pow(sinphi, 2.0))
            s = df2 * c

            Try
                t = site.Height
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:SetFromSite Site.Height is not available")
            End Try
            t /= 1000 '								// Elevation in KM
            ach = EARTHRAD * c + t
            ash = EARTHRAD * s + t

            '//
            '// Compute local sidereal time factors at the observer's longitude.
            '//
            Try
                t = site.Longitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:SetFromSite Site.Height is not available")
            End Try

            stlocl = (gast * 15.0 + t) * DEG2RAD
            sinst = Sin(stlocl)
            cosst = Cos(stlocl)

            '//
            '// Compute position vector components in AU
            '//

            PosVec(0) = (ach * cosphi * cosst) / KMAU
            PosVec(1) = (ach * cosphi * sinst) / KMAU
            PosVec(2) = (ash * sinphi) / KMAU

            RADecOk = False '		// These really aren't inteersting anyway for site vector
            AzElOk = False

            xOk = True '				// Object is valid
            yOk = True
            zOk = True
        End Function

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
        <ComVisible(False)> _
        Public Overloads Function SetFromSiteJD(ByVal site As Site, ByVal ujd As Double) As Boolean Implements IPositionVectorExtra.SetFromSiteJD
            SetFromSiteJD(site, ujd, 0.0)
        End Function

        ''' <summary>
        ''' Initialize the PositionVector from a Site object using UTC Julian date and Delta-T
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="ujd">UTC Julian Date</param>
        ''' <param name="delta_t">The value of Delta-T (TT - UT1) to use for reductions (seconds)</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks>The Julian date must be UTC Julian date, not terrestrial.</remarks>
        Public Overloads Function SetFromSiteJD(ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double) As Boolean Implements IPositionVector.SetFromSiteJD
            Dim dummy, secdiff, tdb, tjd, gast As Double
            Dim oblm, oblt, eqeq, psi, eps As Double

            '//
            '// Convert UTC Julian date to Terrestrial Julian Date then
            '// convert that to barycentric for earthtilt(), which we use
            '// to get the equation of equinoxes for sidereal_time(). Note
            '// that we're using UJD as input to the deltat(), but that is
            '// OK as the difference in time (~70 sec) is insignificant.
            '// For precise applications, the caller must specify delta_t.
            '//
            'tjd = ujd + ((delta_t != 0.0) ? delta_t : deltat(ujd))
            If (delta_t <> 0.0) Then
                tjd = ujd + delta_t
            Else
                tjd = ujd + DeltaTCalc(ujd)
            End If


            Tdb2Tdt(tjd, dummy, secdiff)
            tdb = tjd + secdiff / 86400.0
            EarthTilt(tdb, oblm, oblt, eqeq, psi, eps)

            '//
            '// Get the Greenwich Apparent Sidereal Time and call our
            '// SetFromSite() method.
            '//
            SiderealTime(ujd, 0.0, eqeq, gast)
            SetFromSite(site, gast)

        End Function

        ''' <summary>
        ''' Initialize the PositionVector from a Star object.
        ''' </summary>
        ''' <param name="star">The Star object from which to initialize</param>
        ''' <returns>True if successful or throws an exception</returns>
        ''' <remarks></remarks>
        ''' <exception cref="Exceptions.ValueNotAvailableException">If Parallax, RightAScension or Declination is not available in the supplied star object.</exception>
        Public Function SetFromStar(ByVal star As Star) As Boolean Implements IPositionVector.SetFromStar
            Dim paralx, r, d, cra, sra, cdc, sdc As Double

            '//
            '// If parallax is unknown, undetermined, or zero, set it to 1e-7 second
            '// of arc, corresponding to a distance of 10 megaparsecs.
            '//
            Try
                paralx = star.Parallax
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:SetFromStar Star.Parallax is not available")
            End Try

            If (paralx <= 0.0) Then paralx = 0.0000001

            '//
            '// Convert right ascension, declination, and parallax to position vector
            '// in equatorial system with units of AU.
            '//
            m_Dist = RAD2SEC / paralx
            Try
                m_RA = star.RightAscension
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:SetFromStar Star.RightAscension is not available")
            End Try

            r = m_RA * 15.0 * DEG2RAD '				// hrs -> deg -> rad
            Try
                m_DEC = star.Declination
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("PositionVector:SetFromStar Star.Declination is not available")
            End Try

            d = m_DEC * DEG2RAD '					/// deg -> rad

            cra = Cos(r)
            sra = Sin(r)
            cdc = Cos(d)
            sdc = Sin(d)

            PosVec(0) = m_Dist * cdc * cra
            PosVec(1) = m_Dist * cdc * sra
            PosVec(2) = m_Dist * sdc

            RADecOk = True '				// Object is valid
            xOk = True
            yOk = True
            zOk = True
        End Function

        ''' <summary>
        ''' Position cartesian x component
        ''' </summary>
        ''' <value>Cartesian x component</value>
        ''' <returns>Cartesian x component</returns>
        ''' <remarks></remarks>
        Public Property x() As Double Implements IPositionVector.x
            Get
                If Not xOk Then Throw New Exceptions.ValueNotSetException("PositionVector:x has not been set")
                Return PosVec(0)
            End Get
            Set(ByVal value As Double)
                PosVec(0) = value
                xOk = True
                RADecOk = False
                AzElOk = False
            End Set
        End Property

        ''' <summary>
        ''' Position cartesian y component
        ''' </summary>
        ''' <value>Cartesian y component</value>
        ''' <returns>Cartesian y component</returns>
        ''' <remarks></remarks>
        Public Property y() As Double Implements IPositionVector.y
            Get
                If Not yOk Then Throw New Exceptions.ValueNotSetException("PositionVector:y has not been set")
                Return PosVec(1)
            End Get
            Set(ByVal value As Double)
                PosVec(1) = value
                yOk = True
                RADecOk = False
                AzElOk = False
            End Set
        End Property

        ''' <summary>
        ''' Position cartesian z component
        ''' </summary>
        ''' <value>Cartesian z component</value>
        ''' <returns>Cartesian z component</returns>
        ''' <remarks></remarks>
        Public Property z() As Double Implements IPositionVector.z
            Get
                If Not zOk Then Throw New Exceptions.ValueNotSetException("PositionVector:z has not been set")
                Return PosVec(2)
            End Get
            Set(ByVal value As Double)
                PosVec(2) = value
                zOk = True
                RADecOk = False
                AzElOk = False
            End Set
        End Property

#Region "PositionVector Support Code"
        Private Sub CheckEq()
            If RADecOk Then Return ' Equatorial data already OK
            Vector2RADec(PosVec, m_RA, m_DEC) ' Calculate RA/Dec
            m_Dist = Sqrt(Pow(PosVec(0), 2) + Pow(PosVec(1), 2) + Pow(PosVec(2), 2))
            m_Light = m_Dist / C
            RADecOk = True
        End Sub
#End Region
    End Class

    ''' <summary>
    ''' NOVAS-COM: Site Class
    ''' </summary>
    ''' <remarks>NOVAS-COM objects of class Site contain the specifications for an observer's location on the Earth 
    ''' ellipsoid. Properties are latitude, longitude, height above mean sea level, the ambient temperature 
    ''' and the sea-level barmetric pressure. The latter two are used only for optional refraction corrections. 
    ''' Latitude and longitude are (common) geodetic, not geocentric. </remarks>
    <Guid("46ACFBCE-4EEE-496d-A4B6-7A5FDDD8F969"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class Site
        Implements ISite
        Private vHeight, vLatitude, vLongitude, vPressure, vTemperature As Double
        Private HeightValid, LatitudeValid, LongitudeValid, PressureValid, TemperatureValid As Boolean

        ''' <summary>
        ''' Initialises a new site object
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            HeightValid = False
            LatitudeValid = False
            LongitudeValid = False
            PressureValid = False
            TemperatureValid = False
        End Sub

        ''' <summary>
        ''' Height above mean sea level
        ''' </summary>
        ''' <value>Height above mean sea level</value>
        ''' <returns>Meters</returns>
        ''' <remarks></remarks>
        Public Property Height() As Double Implements ISite.Height
            Get
                If Not HeightValid Then Throw New Exceptions.ValueNotSetException("Height has not yet been set")
                Return vHeight
            End Get
            Set(ByVal value As Double)
                vHeight = value
                HeightValid = True
            End Set
        End Property

        ''' <summary>
        ''' Geodetic latitude (degrees, + north)
        ''' </summary>
        ''' <value>Geodetic latitude</value>
        ''' <returns>Degrees, + north</returns>
        ''' <remarks></remarks>
        Public Property Latitude() As Double Implements ISite.Latitude
            Get
                If Not LatitudeValid Then Throw New Exceptions.ValueNotSetException("Latitude has not yet been set")
                Return vLatitude
            End Get
            Set(ByVal value As Double)
                vLatitude = value
                LatitudeValid = True
            End Set
        End Property

        ''' <summary>
        ''' Geodetic longitude (degrees, + east)
        ''' </summary>
        ''' <value>Geodetic longitude</value>
        ''' <returns>Degrees, + east</returns>
        ''' <remarks></remarks>
        Public Property Longitude() As Double Implements ISite.Longitude
            Get
                If Not LongitudeValid Then Throw New Exceptions.ValueNotSetException("Longitude has not yet been set")
                Return vLongitude
            End Get
            Set(ByVal value As Double)
                vLongitude = value
                LongitudeValid = True
            End Set
        End Property

        ''' <summary>
        ''' Barometric pressure (millibars)
        ''' </summary>
        ''' <value>Barometric pressure</value>
        ''' <returns>Millibars</returns>
        ''' <remarks></remarks>
        Public Property Pressure() As Double Implements ISite.Pressure
            Get
                If Not PressureValid Then Throw New Exceptions.ValueNotSetException("Pressure has not yet been set")
                Return vPressure
            End Get
            Set(ByVal value As Double)
                vPressure = value
                PressureValid = True
            End Set
        End Property

        ''' <summary>
        ''' Set all site properties in one method call
        ''' </summary>
        ''' <param name="Latitude">The geodetic latitude (degrees, + north)</param>
        ''' <param name="Longitude">The geodetic longitude (degrees, +east)</param>
        ''' <param name="Height">Height above sea level (meters)</param>
        ''' <remarks></remarks>
        Public Sub [Set](ByVal Latitude As Double, ByVal Longitude As Double, ByVal Height As Double) Implements ISite.Set
            vLatitude = Latitude
            vLongitude = Longitude
            vHeight = Height
            LatitudeValid = True
            LongitudeValid = True
            HeightValid = True
        End Sub

        ''' <summary>
        ''' Ambient temperature (deg. Celsius)
        ''' </summary>
        ''' <value>Ambient temperature</value>
        ''' <returns>Degrees Celsius)</returns>
        ''' <remarks></remarks>
        Public Property Temperature() As Double Implements ISite.Temperature
            Get
                If Not TemperatureValid Then Throw New Exceptions.ValueNotSetException("Temperature has not yet been set")
                Return vTemperature
            End Get
            Set(ByVal value As Double)
                vTemperature = value
                TemperatureValid = True
            End Set
        End Property
    End Class

    ''' <summary>
    ''' NOVAS-COM: Star Class
    ''' </summary>
    ''' <remarks>NOVAS-COM objects of class Star contain the specifications for a star's catalog position in either FK5 or Hipparcos units (both must be J2000). Properties are right ascension and declination, proper motions, parallax, radial velocity, catalog type (FK5 or HIP), catalog number, optional ephemeris engine to use for barycenter calculations, and an optional value for delta-T. Unless you specifically set the DeltaT property, calculations performed by this class which require the value of delta-T (TT - UT1) rely on an internal function to estimate delta-T. 
    '''<para>The high-level NOVAS astrometric functions are implemented as methods of Star: 
    ''' GetTopocentricPosition(), GetLocalPosition(), GetApparentPosition(), GetVirtualPosition(), 
    ''' and GetAstrometricPosition(). These methods operate on the properties of the Star, and produce 
    ''' a PositionVector object. For example, to get the topocentric coordinates of a star, simply create 
    ''' and initialize a Star, then call Star.GetTopocentricPosition(). The resulting vaPositionVector's 
    ''' right ascension and declination properties are the topocentric equatorial coordinates, at the same 
    ''' time, the (optionally refracted) alt-az coordinates are calculated, and are also contained within 
    ''' the returned PositionVector. <b>Note that Alt/Az is available in PositionVectors returned from calling 
    ''' GetTopocentricPosition().</b></para></remarks>
    <Guid("8FD58EDE-DF7A-4fdc-9DEC-FD0B36424F5F"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class Star
        Implements IStar
        Private m_rv, m_plx, m_pmdec, m_pmra, m_ra, m_dec, m_deltat As Double
        Private m_rav, m_decv, m_bDTValid As Boolean
        Private m_earthephobj As Object
        Private m_cat, m_name As String
        Private m_num As Integer
        Private m_earth As BodyDescription
        Private hr As Short
        Dim m_earthephdisps(4) As Double

        ''' <summary>
        ''' Initialise a new instance of the star class
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            m_rv = 0.0 'Defaults to 0.0
            m_plx = 0.0 'Defaults to 0.0
            m_pmdec = 0.0 'Defaults to 0.0
            m_pmra = 0.0 'Defaults to 0.0
            m_rav = False 'RA not valid
            m_ra = 0.0
            m_decv = False 'Dec not valid
            m_dec = 0.0
            m_cat = "" '\0''No names
            m_name = "" '\0'
            m_num = 0
            m_earthephobj = Nothing '// No Earth ephemeris [sentinel]
            m_bDTValid = False 'Calculate delta-t
            m_earth = New BodyDescription
            m_earth.Number = Body.Earth
            m_earth.Name = "Earth"
            m_earth.Type = BodyType.MajorPlanet
        End Sub

        ''' <summary>
        ''' Three character catalog code for the star's data
        ''' </summary>
        ''' <value>Three character catalog code for the star's data</value>
        ''' <returns>Three character catalog code for the star's data</returns>
        ''' <remarks>Typically "FK5" but may be "HIP". For information only.</remarks>
        Public Property Catalog() As String Implements IStar.Catalog
            Get
                Return m_cat
            End Get
            Set(ByVal value As String)
                If Len(value) > 3 Then Throw New Utilities.Exceptions.InvalidValueException("Star.Catalog Catlog > 3 characters long: " & value)
                m_cat = value
            End Set
        End Property

        ''' <summary>
        ''' Mean catalog J2000 declination coordinate (degrees)
        ''' </summary>
        ''' <value>Mean catalog J2000 declination coordinate</value>
        ''' <returns>Degrees</returns>
        ''' <remarks></remarks>
        Public Property Declination() As Double Implements IStar.Declination
            Get
                If Not m_rav Then Throw New Exceptions.ValueNotSetException("Star.Declination Value not available")
                Return m_dec
            End Get
            Set(ByVal value As Double)
                m_dec = value
                m_decv = True
            End Set
        End Property

        ''' <summary>
        ''' The value of delta-T (TT - UT1) to use for reductions.
        ''' </summary>
        ''' <value>The value of delta-T (TT - UT1) to use for reductions.</value>
        ''' <returns>Seconds</returns>
        ''' <remarks>If this property is not set, calculations will use an internal function to estimate delta-T.</remarks>
        Public Property DeltaT() As Double Implements IStar.DeltaT
            Get
                If Not m_bDTValid Then Throw New Exceptions.ValueNotSetException("Star.DeltaT Value not available")
                Return m_deltat
            End Get
            Set(ByVal value As Double)
                m_deltat = value
                m_bDTValid = True
            End Set
        End Property

        ''' <summary>
        ''' Ephemeris object used to provide the position of the Earth.
        ''' </summary>
        ''' <value>Ephemeris object used to provide the position of the Earth.</value>
        ''' <returns>Ephemeris object</returns>
        ''' <remarks>If this value is not set, an internal Kepler object will be used to determine 
        ''' Earth ephemeris</remarks>
        Public Property EarthEphemeris() As Object Implements IStar.EarthEphemeris
            Get
                Return m_earthephobj
            End Get
            Set(ByVal value As Object)
                m_earthephobj = value
            End Set
        End Property

        ''' <summary>
        ''' Get an apparent position for a given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the apparent place.</returns>
        ''' <remarks></remarks>
        Public Function GetApparentPosition(ByVal tjd As Double) As PositionVector Implements IStar.GetApparentPosition
            Dim cat As New CatEntry
            Dim PV As New PositionVector

            Dim tdb, time2, peb(3), veb(3), pes(3), ves(3), pos1(3), pos2(3), _
                pos3(3), pos4(3), pos5(3), pos6(3), vel1(3), vec(3) As Double

            If Not (m_rav And m_decv) Then Throw New Exceptions.ValueNotSetException("Star.GetApparentPosition RA or DEC not available")

            '//
            '// Get the position and velocity of the Earth w/r/t the solar system
            '// barycenter and the center of mass of the Sun, on the mean equator
            '// and equinox of J2000.0
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//

            hr = GetEarth(tjd, m_earth, tdb, peb, veb, pes, ves)
            If hr > 0 Then
                vec(0) = 0.0
                vec(1) = 0.0
                vec(2) = 0.0
                Throw New Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr)
            End If
            cat.RA = m_ra
            cat.Dec = m_dec
            cat.ProMoRA = m_pmra
            cat.ProMoDec = m_pmdec
            cat.Parallax = m_plx
            cat.RadialVelocity = m_rv

            StarVectors(cat, pos1, vel1)
            ProperMotion(T0, pos1, vel1, tdb, pos2)

            BaryToGeo(pos2, peb, pos3, time2)
            SunField(pos3, pes, pos4)
            Aberration(pos4, veb, time2, pos5)
            Precession(T0, pos5, tdb, pos6)
            Nutate(tdb, NutationDirection.MeanToTrue, pos6, vec)
            PV.x = vec(0)
            PV.y = vec(1)
            PV.z = vec(2)
            Return PV
        End Function

        '//
        '// This is the NOVAS-COM implementation of astro_star(). See the
        '// original NOVAS-C sources for more info.
        '//
        ''' <summary>
        ''' Get an astrometric position for a given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the astrometric place.</returns>
        ''' <remarks></remarks>
        Public Function GetAstrometricPosition(ByVal tjd As Double) As PositionVector Implements IStar.GetAstrometricPosition
            Dim cat As New CatEntry
            Dim PV As New PositionVector
            Dim lighttime, pos1(3), vel1(3), pos2(3), tdb, peb(3), _
                veb(3), pes(3), ves(3), vec(3) As Double

            If Not (m_rav And m_decv) Then Throw New Exceptions.ValueNotSetException("Star.GetAstrometricPosition RA or DEC not available")

            '//
            '// Get the position and velocity of the Earth w/r/t the solar system
            '// barycenter and the center of mass of the Sun, on the mean equator
            '// and equinox of J2000.0
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//
            hr = GetEarth(tjd, m_earth, tdb, peb, veb, pes, ves)
            If hr > 0 Then
                vec(0) = 0.0
                vec(1) = 0.0
                vec(2) = 0.0
                Throw New Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr)
            End If

            cat.RA = m_ra
            cat.Dec = m_dec
            cat.ProMoRA = m_pmra
            cat.ProMoDec = m_pmdec
            cat.Parallax = m_plx
            cat.RadialVelocity = m_rv

            '//
            '// Compute astrometric place.
            '//

            StarVectors(cat, pos1, vel1)
            ProperMotion(T0, pos1, vel1, tdb, pos2)
            BaryToGeo(pos2, peb, vec, lighttime)

            PV.x = vec(0)
            PV.y = vec(1)
            PV.z = vec(2)
            Return PV

        End Function

        ''' <summary>
        ''' Get a local position for a given site and time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">A Site object representing the observing site</param>
        ''' <returns>PositionVector for the local place.</returns>
        ''' <remarks></remarks>
        Public Function GetLocalPosition(ByVal tjd As Double, ByVal site As Site) As PositionVector Implements IStar.GetLocalPosition
            Dim cat As New CatEntry
            Dim PV As New PositionVector
            Dim st As New SiteInfo
            Dim gast, lighttime, ujd, pog(3), vog(3), pb(3), vb(3), ps(3), _
             vs(3), pos1(3), vel1(3), pos2(3), vel2(3), pos3(3), pos4(3), _
             tdb, peb(3), veb(3), pes(3), ves(3), vec(3), oblm, oblt, eqeq, _
             psi, eps As Double
            Dim j As Integer

            If Not (m_rav And m_decv) Then Throw New Exceptions.ValueNotSetException("Star.GetLocalPosition RA or DEC not available")
            '//
            '// Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
            '//
            If (m_bDTValid) Then
                ujd = tjd - m_deltat
            Else
                ujd = tjd - DeltaTCalc(tjd) / 86400.0
            End If


            cat.RA = m_ra
            cat.Dec = m_dec
            cat.ProMoRA = m_pmra
            cat.ProMoDec = m_pmdec
            cat.Parallax = m_plx
            cat.RadialVelocity = m_rv

            Try
                st.Latitude = site.Latitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetLocalPosition Site.Latitude is not available")
            End Try

            Try
                st.Longitude = site.Longitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetLocalPosition Site.Longitude is not available")
            End Try
            Try
                st.Height = site.Height
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetLocalPosition Site.Height is not available")
            End Try

            '//
            '// Compute position and velocity of the observer, on mean equator
            '// and equinox of J2000.0, wrt the solar system barycenter and
            '// wrt to the center of the Sun.
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//
            hr = GetEarth(tjd, m_earth, tdb, peb, veb, pes, ves)
            If hr > 0 Then
                vec(0) = 0.0
                vec(1) = 0.0
                vec(2) = 0.0
                Throw New Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr)
            End If

            EarthTilt(tdb, oblm, oblt, eqeq, psi, eps)

            SiderealTime(ujd, 0.0, eqeq, gast)
            Terra(st, gast, pos1, vel1)
            Nutate(tdb, NutationDirection.TrueToMean, pos1, pos2)
            Precession(tdb, pos2, T0, pog)

            Nutate(tdb, NutationDirection.TrueToMean, vel1, vel2)
            Precession(tdb, vel2, T0, vog)

            For j = 0 To 2

                pb(j) = peb(j) + pog(j)
                vb(j) = veb(j) + vog(j)
                ps(j) = pes(j) + pog(j)
                vs(j) = ves(j) + vog(j)
            Next

            '//
            '// Compute local place.
            '//

            StarVectors(cat, pos1, vel1)
            ProperMotion(T0, pos1, vel1, tdb, pos2)
            BaryToGeo(pos2, pb, pos3, lighttime)
            SunField(pos3, ps, pos4)
            Aberration(pos4, vb, lighttime, vec)

            PV.x = vec(0)
            PV.y = vec(1)
            PV.z = vec(2)
            Return PV

        End Function

        '//
        '// This is the NOVAS-COM implementation of topo_star(). See the
        '// original NOVAS-C sources for more info.
        '//
        ''' <summary>
        ''' Get a topocentric position for a given site and time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <param name="site">A Site object representing the observing site</param>
        ''' <param name="Refract">True to apply atmospheric refraction corrections</param>
        ''' <returns>PositionVector for the topocentric place.</returns>
        ''' <remarks></remarks>
        Public Function GetTopocentricPosition(ByVal tjd As Double, ByVal site As Site, ByVal Refract As Boolean) As PositionVector Implements IStar.GetTopocentricPosition
            Dim ref As RefractionOption
            Dim j As Integer
            Dim cat As New CatEntry
            Dim st As New SiteInfo
            Dim lighttime, ujd, pob(3), pog(3), vob(3), vog(3), pos(3), gast, _
                pos1(3), pos2(3), pos3(3), pos4(3), pos5(3), pos6(3), _
                vel1(3), vel2(3), tdb, peb(3), veb(3), pes(3), ves(3), vec(3), _
                oblm, oblt, eqeq, psi, eps As Double
            Dim ra, rra, dec, rdec, az, zd, dist As Double
            Dim wx As Boolean

            If Not (m_rav And m_decv) Then Throw New Exceptions.ValueNotSetException("Star.GetTopocentricPosition RA or DEC not available")

            '//
            '// Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
            '//
            If (m_bDTValid) Then
                ujd = tjd - m_deltat
            Else
                ujd = tjd - DeltaTCalc(tjd) / 86400.0
            End If

            '//
            '// Get the observer's site info
            '//
            Try
                st.Latitude = site.Latitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available")
            End Try
            Try
                st.Longitude = site.Longitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available")
            End Try
            Try
                st.Height = site.Height
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available")
            End Try

            '//
            '// Compute position and velocity of the observer, on mean equator
            '// and equinox of J2000.0, wrt the solar system barycenter and
            '// wrt to the center of the Sun.
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//
            hr = GetEarth(tjd, m_earth, tdb, peb, veb, pes, ves)
            If hr > 0 Then
                vec(0) = 0.0
                vec(1) = 0.0
                vec(2) = 0.0
                Throw New Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr)
            End If

            EarthTilt(tdb, oblm, oblt, eqeq, psi, eps)

            SiderealTime(ujd, 0.0, eqeq, gast)
            Terra(st, gast, pos1, vel1)
            Nutate(tdb, NutationDirection.TrueToMean, pos1, pos2)
            Precession(tdb, pos2, T0, pog)

            Nutate(tdb, NutationDirection.TrueToMean, vel1, vel2)
            Precession(tdb, vel2, T0, vog)

            For j = 0 To 2
                pob(j) = peb(j) + pog(j)
                vob(j) = veb(j) + vog(j)
                pos(j) = pes(j) + pog(j)
            Next

            '//
            '// Convert FK5 info to vector form
            '//
            cat.RA = m_ra
            cat.Dec = m_dec
            cat.ProMoRA = m_pmra
            cat.ProMoDec = m_pmdec
            cat.Parallax = m_plx
            cat.RadialVelocity = m_rv
            StarVectors(cat, pos1, vel1)

            '//
            '// Finish topocentric place calculation.
            '//
            ProperMotion(T0, pos1, vel1, tdb, pos2)
            BaryToGeo(pos2, pob, pos3, lighttime)
            SunField(pos3, pos, pos4)
            Aberration(pos4, vob, lighttime, pos5)
            Precession(T0, pos5, tdb, pos6)
            Nutate(tdb, NutationDirection.MeanToTrue, pos6, vec)

            '//
            '// Calculate equatorial coordinates and distance
            '//
            Vector2RADec(vec, ra, dec) 'Get topo RA/Dec
            dist = Sqrt(Pow(vec(0), 2.0) + Pow(vec(1), 2.0) + Pow(vec(2), 2.0)) 'And dist

            '//
            '// Refract if requested
            '//
            ref = RefractionOption.NoRefraction 'Assume no refraction
            If Refract Then
                wx = True 'Assume site weather
                Try
                    st.Temperature = site.Temperature
                Catch ex As Exception 'Value unset so use standard refraction option
                    wx = False
                End Try
                Try
                    st.Pressure = site.Pressure
                Catch ex As Exception 'Value unset so use standard refraction option
                    wx = False
                End Try


                If wx Then 'Set refraction option
                    ref = RefractionOption.LocationRefraction
                Else
                    ref = RefractionOption.StandardRefraction
                End If
            End If
            '//
            '// This calculates Alt/Az coordinates. If ref > 0 then it refracts
            '// both the computed Alt/Az and the RA/Dec coordinates.
            '//
            If m_bDTValid Then
                Equ2Hor(tjd, m_deltat, 0.0, 0.0, st, ra, dec, ref, zd, az, rra, rdec)
            Else
                Equ2Hor(tjd, DeltaTCalc(tjd), 0.0, 0.0, st, ra, dec, ref, zd, az, rra, rdec)
            End If

            '//
            '// If we refracted, we now must compute new cartesian components
            '// Distance does not change...
            '//
            If (ref > 0) Then 'If refracted, recompute 
                RADec2Vector(rra, rdec, dist, vec) 'New refracted vector
            End If

            'Create a new positionvector with calculated values
            Dim PV As New PositionVector(vec(0), vec(1), vec(2), rra, rdec, dist, dist / C, az, (90.0 - zd))

            Return PV
        End Function

        ''' <summary>
        ''' Get a virtual position at a given time
        ''' </summary>
        ''' <param name="tjd">Terrestrial Julian Date for the position</param>
        ''' <returns>PositionVector for the virtual place.</returns>
        ''' <remarks></remarks>
        Public Function GetVirtualPosition(ByVal tjd As Double) As PositionVector Implements IStar.GetVirtualPosition
            '//
            '// This is the NOVAS-COM implementation of virtual_star(). See the
            '// original NOVAS-C sources for more info.
            '//
            Dim cat As New CatEntry
            Dim PV As New PositionVector

            Dim pos1(3), vel1(3), pos2(3), pos3(3), pos4(3), _
                tdb, peb(3), veb(3), pes(3), ves(3), vec(3), lighttime As Double

            If Not (m_rav And m_decv) Then Throw New Exceptions.ValueNotSetException("Star.GetVirtualPosition RA or DEC not available")

            cat.RA = m_ra
            cat.Dec = m_dec
            cat.ProMoRA = m_pmra
            cat.ProMoDec = m_pmdec
            cat.Parallax = m_plx
            cat.RadialVelocity = m_rv

            '//
            '// Compute position and velocity of the observer, on mean equator
            '// and equinox of J2000.0, wrt the solar system barycenter and
            '// wrt to the center of the Sun.
            '//
            '// This also gets the barycentric terrestrial dynamical time (TDB).
            '//

            hr = GetEarth(tjd, m_earth, tdb, peb, veb, pes, ves)
            If hr > 0 Then
                vec(0) = 0.0
                vec(1) = 0.0
                vec(2) = 0.0
                Throw New Exceptions.NOVASFunctionException("Star.GetApparentPosition", "get_earth", hr)
            End If

            '//
            '// Compute virtual place.
            '//
            StarVectors(cat, pos1, vel1)
            ProperMotion(T0, pos1, vel1, tdb, pos2)
            BaryToGeo(pos2, peb, pos3, lighttime)
            SunField(pos3, pes, pos4)
            Aberration(pos4, veb, lighttime, vec)

            PV.x = vec(0)
            PV.y = vec(1)
            PV.z = vec(2)
            Return PV

        End Function

        ''' <summary>
        ''' The catalog name of the star (50 char max)
        ''' </summary>
        ''' <value>The catalog name of the star</value>
        ''' <returns>Name (50 char max)</returns>
        ''' <remarks></remarks>
        Public Property Name() As String Implements IStar.Name
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                If Len(value) > 50 Then Throw New Utilities.Exceptions.InvalidValueException("Star.Name Name > 50 characters long: " & value)
                m_name = value
            End Set
        End Property

        ''' <summary>
        ''' The catalog number of the star
        ''' </summary>
        ''' <value>The catalog number of the star</value>
        ''' <returns>The catalog number of the star</returns>
        ''' <remarks></remarks>
        Public Property Number() As Integer Implements IStar.Number
            Get
                Return m_num
            End Get
            Set(ByVal value As Integer)
                m_num = value
            End Set
        End Property

        ''' <summary>
        ''' Catalog mean J2000 parallax (arcsec)
        ''' </summary>
        ''' <value>Catalog mean J2000 parallax</value>
        ''' <returns>Arc seconds</returns>
        ''' <remarks></remarks>
        Public Property Parallax() As Double Implements IStar.Parallax
            Get
                Return m_plx
            End Get
            Set(ByVal value As Double)
                m_plx = value
            End Set
        End Property

        ''' <summary>
        ''' Catalog mean J2000 proper motion in declination (arcsec/century)
        ''' </summary>
        ''' <value>Catalog mean J2000 proper motion in declination</value>
        ''' <returns>Arc seconds per century</returns>
        ''' <remarks></remarks>
        Public Property ProperMotionDec() As Double Implements IStar.ProperMotionDec
            Get
                Return m_pmdec
            End Get
            Set(ByVal value As Double)
                m_pmdec = value
            End Set
        End Property

        ''' <summary>
        ''' Catalog mean J2000 proper motion in right ascension (sec/century)
        ''' </summary>
        ''' <value>Catalog mean J2000 proper motion in right ascension</value>
        ''' <returns>Seconds per century</returns>
        ''' <remarks></remarks>
        Public Property ProperMotionRA() As Double Implements IStar.ProperMotionRA
            Get
                Return m_pmra
            End Get
            Set(ByVal value As Double)
                m_pmra = value
            End Set
        End Property

        ''' <summary>
        ''' Catalog mean J2000 radial velocity (km/sec)
        ''' </summary>
        ''' <value>Catalog mean J2000 radial velocity</value>
        ''' <returns>Kilometers per second</returns>
        ''' <remarks></remarks>
        Public Property RadialVelocity() As Double Implements IStar.RadialVelocity
            Get
                Return m_rv
            End Get
            Set(ByVal value As Double)
                m_rv = value
            End Set
        End Property

        ''' <summary>
        ''' Catalog mean J2000 right ascension coordinate (hours)
        ''' </summary>
        ''' <value>Catalog mean J2000 right ascension coordinate</value>
        ''' <returns>Hours</returns>
        ''' <remarks></remarks>
        Public Property RightAscension() As Double Implements IStar.RightAscension
            Get
                If Not m_rav Then Throw New Exceptions.ValueNotSetException("Star.RightAscension Value not available")
                Return m_ra
            End Get
            Set(ByVal value As Double)
                m_ra = value
                m_rav = True
            End Set
        End Property

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
        Public Sub [Set](ByVal RA As Double, ByVal Dec As Double, ByVal ProMoRA As Double, ByVal ProMoDec As Double, ByVal Parallax As Double, ByVal RadVel As Double) Implements IStar.Set
            m_ra = RA
            m_dec = Dec
            m_pmra = ProMoRA
            m_pmdec = ProMoDec
            m_plx = Parallax
            m_rv = RadVel
            m_rav = True
            m_decv = True
            m_num = 0
            m_name = "" '\0';
            m_cat = "" '\0';
        End Sub

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
        Public Sub SetHipparcos(ByVal RA As Double, ByVal Dec As Double, ByVal ProMoRA As Double, ByVal ProMoDec As Double, ByVal Parallax As Double, ByVal RadVel As Double) Implements IStar.SetHipparcos
            Dim hip, fk5 As New CatEntry

            hip.RA = RA
            hip.Dec = Dec
            hip.ProMoRA = ProMoRA
            hip.ProMoDec = ProMoDec
            hip.Parallax = Parallax
            hip.RadialVelocity = RadVel

            TransformHip(hip, fk5)

            m_ra = fk5.RA
            m_dec = fk5.Dec
            m_pmra = fk5.ProMoRA
            m_pmdec = fk5.ProMoDec
            m_plx = fk5.Parallax
            m_rv = fk5.RadialVelocity
            m_rav = True
            m_decv = True
            m_num = 0
            m_name = "" '\0';
            m_cat = "" '\0';

        End Sub
    End Class

    ''' <summary>
    ''' NOVAS-COM: VelocityVector Class
    ''' </summary>
    ''' <remarks>NOVAS-COM objects of class VelocityVector contain vectors used for velocities (earth, sites, 
    ''' planets, and stars) throughout NOVAS-COM. Of course, its properties include the x, y, and z 
    ''' components of the velocity. Additional properties are the velocity in equatorial coordinates of 
    ''' right ascension dot, declination dot and radial velocity. You can initialize a PositionVector from 
    ''' a Star object (essentially an FK5 or HIP catalog entry) or a Site (lat/long/height). For the star 
    ''' object the proper motions, distance and radial velocity are used, for a site, the velocity is that 
    ''' of the observer with respect to the Earth's center of mass. </remarks>
    <Guid("25F2ED0A-D0C1-403d-86B9-5F7CEBE97D87"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class VelocityVector
        Implements IVelocityVector, IVelocityVectorExtra

        Private m_xv, m_yv, m_zv, m_cv As Boolean
        Private m_v(2), m_VRA, m_RadVel, m_VDec As Double

        ''' <summary>
        ''' Creates a new velocity vector object
        ''' </summary>
        ''' <remarks> </remarks>
        Public Sub New()
            m_xv = False 'Vector is not valid
            m_yv = False
            m_zv = False
            m_cv = False 'Coordinate velocities not valid
        End Sub
        ''' <summary>
        '''  Linear velocity along the declination direction (AU/day)
        ''' </summary>
        ''' <value>Linear velocity along the declination direction</value>
        ''' <returns>AU/day</returns>
        ''' <remarks>This is not the proper motion (which is an angular rate and is dependent on the distance to the object).</remarks>
        Public ReadOnly Property DecVelocity() As Double Implements IVelocityVector.DecVelocity
            Get
                If Not (m_xv And m_yv And m_zv) Then Throw New Exceptions.ValueNotSetException("VelocityVector:DecVelocity x, y or z has not been set")
                CheckEq()
                Return m_VDec
            End Get
        End Property

        ''' <summary>
        ''' Linear velocity along the radial direction (AU/day)
        ''' </summary>
        ''' <value>Linear velocity along the radial direction</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RadialVelocity() As Double Implements IVelocityVector.RadialVelocity
            Get
                If Not (m_xv And m_yv And m_zv) Then Throw New Exceptions.ValueNotSetException("VelocityVector:RadialVelocity x, y or z has not been set")
                CheckEq()
                Return m_RadVel
            End Get
        End Property

        ''' <summary>
        ''' Linear velocity along the right ascension direction (AU/day)
        ''' </summary>
        ''' <value>Linear velocity along the right ascension direction</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RAVelocity() As Double Implements IVelocityVector.RAVelocity
            Get
                If Not (m_xv And m_yv And m_zv) Then Throw New Exceptions.ValueNotSetException("VelocityVector:RAVelocity x, y or z has not been set")
                CheckEq()
                Return m_VRA
            End Get
        End Property

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
        Public Function SetFromSite(ByVal site As Site, ByVal gast As Double) As Boolean Implements IVelocityVector.SetFromSite
            Const f As Double = 0.00335281 'f = Earth ellipsoid flattening
            Const omega As Double = 0.000072921151467 'omega = Earth angular velocity rad/sec
            Dim df2, t, sinphi, cosphi, c, s, ach, ash, stlocl, sinst, cosst As Double

            '//
            '// Compute parameters relating to geodetic to geocentric conversion.
            '//
            df2 = Pow((1.0 - f), 2)
            Try
                t = site.Latitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromSite Site.Latitude is not available")
            End Try

            t *= DEG2RAD
            sinphi = Sin(t)
            cosphi = Cos(t)
            c = 1.0 / Sqrt(Pow(cosphi, 2.0) + df2 * Pow(sinphi, 2.0))
            s = df2 * c
            Try
                t = site.Height
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromSite Site.Height is not available")
            End Try

            t /= 1000 'Elevation in KM
            ach = EARTHRAD * c + t
            ash = EARTHRAD * s + t

            '//
            '// Compute local sidereal time factors at the observer's longitude.
            '//
            Try
                t = site.Longitude
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromSite Site.Longitude is not available")
            End Try
            stlocl = (gast * 15.0 + t) * DEG2RAD
            sinst = Sin(stlocl)
            cosst = Cos(stlocl)

            '//
            '// Compute velocity vector components in AU/Day
            '//

            m_v(0) = (-omega * ach * cosphi * sinst) * 86400.0 / KMAU
            m_v(1) = (omega * ach * cosphi * cosst) * 86400.0 / KMAU
            m_v(2) = 0.0

            m_xv = True
            m_yv = True
            m_zv = True 'Vector is complete
            m_cv = False 'Not interesting for Site vector anyway

            Return True
        End Function


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
        <ComVisible(False)> _
        Public Function SetFromSiteJD(ByVal site As Site, ByVal ujd As Double) As Boolean Implements IVelocityVectorExtra.SetFromSiteJD
            SetFromSiteJD(site, ujd, 0.0)
        End Function


        ''' <summary>
        ''' Initialize the VelocityVector from a Site object using UTC Julian Date and Delta-T
        ''' </summary>
        ''' <param name="site">The Site object from which to initialize</param>
        ''' <param name="ujd">UTC Julian Date</param>
        ''' <param name="delta_t">The optional value of Delta-T (TT - UT1) to use for reductions (seconds)</param>
        ''' <returns>True if OK otherwise throws an exception</returns>
        ''' <remarks>The velocity vector is that of the observer with respect to the Earth's center 
        ''' of mass. The Julian date must be UTC Julian date, not terrestrial.</remarks>
        Public Function SetFromSiteJD(ByVal site As Site, ByVal ujd As Double, ByVal delta_t As Double) As Boolean Implements IVelocityVector.SetFromSiteJD
            Dim dummy, secdiff, tdb, tjd, gast As Double
            Dim oblm, oblt, eqeq, psi, eps As Double

            '//
            '// Convert UTC Julian date to Terrestrial Julian Date then
            '// convert that to barycentric for earthtilt(), which we use
            '// to get the equation of equinoxes for sidereal_time(). Note
            '// that we're using UJD as input to the deltat(), but that is
            '// OK as the difference in time (~70 sec) is insignificant.
            '// For precise applications, the caller must specify delta_t.
            '//

            If (delta_t <> 0.0) Then
                tjd = ujd + delta_t
            Else
                tjd = ujd + DeltaTCalc(ujd)
            End If

            Tdb2Tdt(tjd, dummy, secdiff)
            tdb = tjd + secdiff / 86400.0
            EarthTilt(tdb, oblm, oblt, eqeq, psi, eps)

            '//
            '// Get the Greenwich Apparent Sidereal Time and call our
            '// SetFromSite() method.
            '//
            SiderealTime(ujd, 0.0, eqeq, gast)
            SetFromSite(site, gast)
            Return True
        End Function

        ''' <summary>
        ''' Initialize the VelocityVector from a Star object.
        ''' </summary>
        ''' <param name="star">The Star object from which to initialize</param>
        ''' <returns>True if OK otherwise throws an exception</returns>
        ''' <remarks>The proper motions, distance and radial velocity are used in the velocity calculation. </remarks>
        ''' <exception cref="Exceptions.ValueNotAvailableException">If any of: Parallax, RightAscension, Declination, 
        ''' ProperMotionRA, ProperMotionDec or RadialVelocity are not available in the star object</exception>
        Public Function SetFromStar(ByVal star As Star) As Boolean Implements IVelocityVector.SetFromStar
            Dim t, paralx, r, d, cra, sra, cdc, sdc As Double

            '//
            '// If parallax is unknown, undetermined, or zero, set it to 1e-7 second
            '// of arc, corresponding to a distance of 10 megaparsecs.
            '//
            Try
                paralx = star.Parallax
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.Parallax is not available")
            End Try
            If (paralx <= 0.0) Then paralx = 0.0000001

            '//
            '// Convert right ascension, declination, and parallax to position vector
            '// in equatorial system with units of AU.
            '//
            Try
                r = star.RightAscension
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.RightAscension is not available")
            End Try
            Try
                d = star.Declination
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.Declination is not available")
            End Try

            d *= DEG2RAD

            cra = Cos(r)
            sra = Sin(r)
            cdc = Cos(d)
            sdc = Sin(d)

            '//
            '// Convert proper motion and radial velocity to orthogonal components of
            '// motion with units of AU/Day.
            '//
            Try
                t = star.ProperMotionRA
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.ProperMotionRA is not available")
            End Try

            m_VRA = t * 15.0 * cdc / (paralx * 36525.0)
            Try
                t = star.ProperMotionDec
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.ProperMotionDec is not available")
            End Try
            m_VDec = t / (paralx * 36525.0)
            Try
                t = star.RadialVelocity
            Catch ex As Exception
                Throw New Exceptions.ValueNotAvailableException("VelocityVector:SetFromStar Star.RadialVelocity is not available")
            End Try

            m_RadVel = t * 86400.0 / KMAU

            '//
            '// Transform motion vector to equatorial system.
            '//
            m_v(0) = -m_VRA * sra - m_VDec * sdc * cra + m_RadVel * cdc * cra
            m_v(1) = m_VRA * cra - m_VDec * sdc * sra + m_RadVel * cdc * sra
            m_v(2) = m_VDec * cdc + m_RadVel * sdc

            m_xv = True
            m_yv = True
            m_zv = True 'Vector is complete
            m_cv = True 'We have it all!

            Return True
        End Function

        ''' <summary>
        ''' Cartesian x component of velocity (AU/day)
        ''' </summary>
        ''' <value>Cartesian x component of velocity</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        Public Property x() As Double Implements IVelocityVector.x
            Get
                If Not m_xv Then Throw New Exceptions.ValueNotSetException("VelocityVector:x x value has not been set")
                Return m_v(0)
            End Get
            Set(ByVal value As Double)
                m_v(0) = value
                m_xv = True
            End Set
        End Property

        ''' <summary>
        ''' Cartesian y component of velocity (AU/day)
        ''' </summary>
        ''' <value>Cartesian y component of velocity</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        Public Property y() As Double Implements IVelocityVector.y
            Get
                If Not m_yv Then Throw New Exceptions.ValueNotSetException("VelocityVector:y y value has not been set")
                Return m_v(1)
            End Get
            Set(ByVal value As Double)
                m_v(1) = value
                m_yv = True
            End Set
        End Property

        ''' <summary>
        ''' Cartesian z component of velocity (AU/day)
        ''' </summary>
        ''' <value>Cartesian z component of velocity</value>
        ''' <returns>AU/day</returns>
        ''' <remarks></remarks>
        Public Property z() As Double Implements IVelocityVector.z
            Get
                If Not m_zv Then Throw New Exceptions.ValueNotSetException("VelocityVector:z z value has not been set")
                Return m_v(2)
            End Get
            Set(ByVal value As Double)
                m_v(2) = value
                m_zv = True
            End Set
        End Property

#Region "VelocityVector Support Code"
        Private Sub CheckEq()
            If (m_cv) Then Return 'Equatorial data already OK
            Vector2RADec(m_v, m_VRA, m_VDec) 'Calculate VRA/VDec
            m_RadVel = Sqrt(Pow(m_v(0), 2) + Pow(m_v(1), 2) + Pow(m_v(2), 2))
            m_cv = True
        End Sub
#End Region

    End Class
#End Region

End Namespace