'Transform component implementation

Imports System.Math
Imports ASCOM.Utilities
Imports ASCOM.Utilities.Interfaces
Imports System.Runtime.InteropServices
Imports ASCOM.Astrometry.Kepler
Imports ASCOM.Astrometry.AstroUtils
Imports System.Reflection

Namespace Transform
    ''' <summary>
    ''' Coordinate transform component; J2000 - apparent - local topocentric
    ''' </summary>
    ''' <remarks>Use this component to transform between J2000, apparent and local topocentric (JNow) coordinates or 
    ''' vice versa. To use the component, instantiate it, then use one of SetJ2000 or SetJNow or SetApparent to 
    ''' initialise with known values. Now use the RAJ2000, DECJ200, RAJNow, DECJNow, RAApparent and DECApparent etc. 
    ''' properties to read off the required transformed values.
    '''<para>The component can be reused simply by setting new co-ordinates with a Set command, there
    ''' is no need to create a new component each time a transform is required.</para>
    ''' <para>Transforms are effected through the ASCOM NOVAS.Net engine that encapsulates the USNO NOVAS 3.1 library. 
    ''' The USNO NOVAS reference web page is: 
    ''' <href>http://www.usno.navy.mil/USNO/astronomical-applications/software-products/novas</href>
    ''' and the NOVAS 3.1 user guide is included in the ASCOM Developer Components install.
    ''' </para>
    ''' </remarks>
    <Guid("779CD957-5502-4939-A661-EBEE9E1F485E"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class Transform
        Implements ITransform, IDisposable
        Private disposedValue As Boolean = False        ' To detect redundant calls
        Private Utl As Util, AstroUtl As AstroUtils.AstroUtils, Nov31 As NOVAS.NOVAS31
        Private RAJ2000Value, RATopoValue, DECJ2000Value, DECTopoValue, SiteElevValue, SiteLatValue, SiteLongValue, SiteTempValue As Double
        Private RAApparentValue, DECApparentValue, AzimuthTopoValue, ElevationTopoValue, JDTT As Double
        Private RefracValue, RequiresRecalculate As Boolean
        Private LastSetBy As SetBy

        Private Earth As BodyDescription
        Private Location As New OnSurface, Cat3 As New CatEntry3

        Private TL As TraceLogger
        Private Sw, SwRecalculate As Stopwatch

        Private Enum SetBy
            Never
            J2000
            Apparent
            Topocentric
            AzimuthElevation
            Refresh
        End Enum

#Region "New and IDisposable"
        Sub New()
            TL = New TraceLogger("", "Transform")
            TL.Enabled = GetBool(TRACE_TRANSFORM, TRACE_TRANSFORM_DEFAULT) 'Get enabled / disabled state from the user registry
            TL.LogMessage("New", "Trace logger created OK")

            Utl = New Util 'Get a Util component for Julian functions
            Sw = New Stopwatch
            SwRecalculate = New Stopwatch
            AstroUtl = New AstroUtils.AstroUtils
            Nov31 = New NOVAS.NOVAS31

            Earth.Name = "Earth"
            Earth.Number = Body.Earth
            Earth.Type = BodyType.MajorPlanet

            RAJ2000Value = Double.NaN 'Initialise to invalid values in case these are read before they are set
            DECJ2000Value = Double.NaN
            RATopoValue = Double.NaN
            DECTopoValue = Double.NaN
            SiteElevValue = Double.NaN
            SiteLatValue = Double.NaN
            SiteLongValue = Double.NaN
            RefracValue = False
            LastSetBy = SetBy.Never
            RequiresRecalculate = True
            JDTT = 0 ' Initialise to a value that forces the current PC date time to be used in determining the TT Julian date of interest
            Call CheckGAC()
            TL.LogMessage("New", "NOVAS initialised OK")
        End Sub

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If Not Utl Is Nothing Then 'Clean up Util object
                    Utl.Dispose()
                    Utl = Nothing
                End If
                If Not AstroUtl Is Nothing Then
                    AstroUtl.Dispose()
                    AstroUtl = Nothing
                End If
                If Not Nov31 Is Nothing Then
                    Nov31.Dispose()
                    Nov31 = Nothing
                End If
                'Clean up the NOVAS.Net objects
                Earth = Nothing
                Cat3 = Nothing
                Location = Nothing
                If Not Sw Is Nothing Then
                    Sw.Stop()
                    Sw = Nothing
                End If
                If Not SwRecalculate Is Nothing Then
                    SwRecalculate.Stop()
                    SwRecalculate = Nothing
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        ''' <summary>
        ''' Cleans up resources used by the Transform component
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

#Region "ITransform Implementation"
        ''' <summary>
        ''' Gets or sets the site latitude
        ''' </summary>
        ''' <value>Site latitude</value>
        ''' <returns>Latitude in degrees</returns>
        ''' <remarks>Positive numbers north of the equator, negative numbers south.</remarks>
        Property SiteLatitude() As Double Implements ITransform.SiteLatitude
            Get
                CheckSet("SiteLatitude", SiteLatValue, "Site latitude has not been set")
                TL.LogMessage("SiteLatitude Get", Utl.DegreesToDMS(SiteLatValue, ":", ":", "", 3))
                Return SiteLatValue
            End Get
            Set(ByVal value As Double)
                If SiteLatValue <> value Then RequiresRecalculate = True
                SiteLatValue = value
                TL.LogMessage("SiteLatitude Set", Utl.DegreesToDMS(value, ":", ":", "", 3))
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the site longitude
        ''' </summary>
        ''' <value>Site longitude</value>
        ''' <returns>Longitude in degrees</returns>
        ''' <remarks>Positive numbers east of the Greenwich meridian, negative numbes west of the Greenwich meridian.</remarks>
        Property SiteLongitude() As Double Implements ITransform.SiteLongitude
            Get
                CheckSet("SiteLongitude", SiteLongValue, "Site longitude has not been set")
                TL.LogMessage("SiteLongitude Get", Utl.DegreesToDMS(SiteLongValue, ":", ":", "", 3))
                Return SiteLongValue
            End Get
            Set(ByVal value As Double)
                If SiteLongValue <> value Then RequiresRecalculate = True
                SiteLongValue = value
                TL.LogMessage("SiteLongitude Set", Utl.DegreesToDMS(value, ":", ":", "", 3))
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the site elevation above sea level
        ''' </summary>
        ''' <value>Site elevation</value>
        ''' <returns>Elevation in metres</returns>
        ''' <remarks></remarks>
        Property SiteElevation() As Double Implements ITransform.SiteElevation
            Get
                CheckSet("SiteElevation", SiteElevValue, "Site elevation has not been set")
                TL.LogMessage("SiteElevation Get", SiteElevValue.ToString)
                Return SiteElevValue
            End Get
            Set(ByVal value As Double)
                If SiteElevValue <> value Then RequiresRecalculate = True
                SiteElevValue = value
                TL.LogMessage("SiteElevation Set", value.ToString)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the site ambient temperature
        ''' </summary>
        ''' <value>Site ambient temperature</value>
        ''' <returns>Temperature in degrees Celsius</returns>
        ''' <remarks></remarks>
        Property SiteTemperature() As Double Implements ITransform.SiteTemperature
            Get
                CheckSet("SiteTemperature", SiteTempValue, "Site temperature has not been set")
                TL.LogMessage("SiteTemperature Get", SiteTempValue.ToString)
                Return SiteTempValue
            End Get
            Set(ByVal value As Double)
                If SiteTempValue <> value Then RequiresRecalculate = True
                SiteTempValue = value
                TL.LogMessage("SiteTemperature Set", value.ToString)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a flag indicating whether refraction is calculated for topocentric co-ordinates
        ''' </summary>
        ''' <value>True / false flag indicating refaction is included / omitted from topocentric co-ordinates</value>
        ''' <returns>Boolean flag</returns>
        ''' <remarks></remarks>
        Property Refraction() As Boolean Implements ITransform.Refraction
            Get
                TL.LogMessage("Refraction Get", RefracValue.ToString)
                Return RefracValue
            End Get
            Set(ByVal value As Boolean)
                If RefracValue <> value Then RequiresRecalculate = True
                RefracValue = value
                TL.LogMessage("Refraction Set", value.ToString)
            End Set
        End Property

        ''' <summary>
        ''' Causes the transform component to recalculate values derrived from the last Set command
        ''' </summary>
        ''' <remarks>Use this when you have set J2000 co-ordinates and wish to ensure that the mount points to the same 
        ''' co-ordinates allowing for local effects that change with time such as refraction.
        ''' <para><b style="color:red">Note:</b> As of Platform 6 SP2 use of this method is not required, refresh is always performed automatically when required.</para></remarks>
        Sub Refresh() Implements ITransform.Refresh
            TL.LogMessage("Refresh", "")
            Recalculate()
        End Sub

        ''' <summary>
        ''' Sets the known J2000 Right Ascension and Declination coordinates that are to be transformed
        ''' </summary>
        ''' <param name="RA">RA in J2000 co-ordinates</param>
        ''' <param name="DEC">DEC in J2000 co-ordinates</param>
        ''' <remarks></remarks>
        Sub SetJ2000(ByVal RA As Double, ByVal DEC As Double) Implements ITransform.SetJ2000
            LastSetBy = SetBy.J2000
            If (RA <> RAJ2000Value) Or (DEC <> DECJ2000Value) Then RequiresRecalculate = True
            RAJ2000Value = RA
            DECJ2000Value = DEC
            TL.LogMessage("SetJ2000", "RA: " & Utl.HoursToHMS(RA, ":", ":", "", 3) & ", DEC: " & Utl.DegreesToDMS(DEC, ":", ":", "", 3))
        End Sub

        ''' <summary>
        ''' Sets the known apparent Right Ascension and Declination coordinates that are to be transformed
        ''' </summary>
        ''' <param name="RA">RA in apparent co-ordinates</param>
        ''' <param name="DEC">DEC in apparent co-ordinates</param>
        ''' <remarks></remarks>
        Sub SetApparent(ByVal RA As Double, ByVal DEC As Double) Implements ITransform.SetApparent
            LastSetBy = SetBy.Apparent
            If (RA <> RAApparentValue) Or (DEC <> DECApparentValue) Then RequiresRecalculate = True
            RAApparentValue = ValidateRA("SetApparent", RA)
            DECApparentValue = ValidateDec("SetApparent", DEC)
            TL.LogMessage("SetApparent", "RA: " & Utl.HoursToHMS(RA, ":", ":", "", 3) & ", DEC: " & Utl.DegreesToDMS(DEC, ":", ":", "", 3))
        End Sub

        '''<summary>
        ''' Sets the known local topocentric Right Ascension and Declination coordinates that are to be transformed
        ''' </summary>
        ''' <param name="RA">RA in local topocentric co-ordinates</param>
        ''' <param name="DEC">DEC in local topocentric co-ordinates</param>
        ''' <remarks></remarks>
        Sub SetTopocentric(ByVal RA As Double, ByVal DEC As Double) Implements ITransform.SetTopocentric
            LastSetBy = SetBy.Topocentric
            If (RA <> RATopoValue) Or (DEC <> DECTopoValue) Then RequiresRecalculate = True
            RATopoValue = RA
            DECTopoValue = DEC
            TL.LogMessage("SetTopocentric", "RA: " & Utl.HoursToHMS(RA, ":", ":", "", 3) & ", DEC: " & Utl.DegreesToDMS(DEC, ":", ":", "", 3))
        End Sub

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
        ''' 
        ReadOnly Property RAJ2000() As Double Implements ITransform.RAJ2000
            Get
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read RAJ2000 before SetJ2000 or SetTopocentric has been called")
                Recalculate()
                CheckSet("RAJ2000", RAJ2000Value, "RA J2000 can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("RAJ2000 Get", Utl.HoursToHMS(RAJ2000Value, ":", ":", "", 3))
                Return RAJ2000Value
            End Get
        End Property

        ''' <summary>
        ''' Returns the Declination in J2000 co-ordinates
        ''' </summary>
        ''' <value>J2000 Declination</value>
        ''' <returns>Declination in degrees</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        ReadOnly Property DecJ2000() As Double Implements ITransform.DECJ2000
            Get
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read DECJ2000 before SetJ2000 or SetTopocentric has been called")
                Recalculate()
                CheckSet("DecJ2000", DECJ2000Value, "DEC J2000 can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("DecJ2000 Get", Utl.DegreesToDMS(DECJ2000Value, ":", ":", "", 3))
                Return DECJ2000Value
            End Get
        End Property

        ''' <summary>
        ''' Returns the Right Ascension in local topocentric co-ordinates
        ''' </summary>
        ''' <value>Local topocentric Right Ascension</value>
        ''' <returns>Right Ascension in hours</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        ReadOnly Property RATopocentric() As Double Implements ITransform.RATopocentric
            Get
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read RATopocentric before SetJ2000 or SetTopocentric has been called")
                Recalculate()
                CheckSet("RATopocentric", RATopoValue, "RA topocentric can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("RATopocentric Get", Utl.HoursToHMS(RATopoValue, ":", ":", "", 3))
                Return RATopoValue
            End Get
        End Property

        ''' <summary>
        ''' Returns the Declination in local topocentric co-ordinates
        ''' </summary>
        ''' <value>Local topocentric Declination</value>
        ''' <returns>Declination in degrees</returns>
        ''' <exception cref="Exceptions.TransformUninitialisedException">Exception thrown if an attempt is made
        ''' to read a value before any of the Set methods has been used or if the value can not be derived from the
        ''' information in the last Set method used. E.g. topocentric values will be unavailable if the last Set was
        ''' a SetApparent and one of the Site properties has not been set.</exception>
        ''' <remarks></remarks>
        ReadOnly Property DECTopocentric() As Double Implements ITransform.DECTopocentric
            Get
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read DECTopocentric before SetJ2000 or SetTopocentric has been called")
                Recalculate()
                CheckSet("DECTopocentric", DECTopoValue, "DEC topocentric can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("DECTopocentric Get", Utl.DegreesToDMS(DECTopoValue, ":", ":", "", 3))
                Return DECTopoValue
            End Get
        End Property

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
        ReadOnly Property RAApparent() As Double Implements ITransform.RAApparent
            Get
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read DECApparent before SetJ2000 or SetApparent has been called")
                Recalculate()
                TL.LogMessage("RAApparent Get", Utl.HoursToHMS(RAApparentValue, ":", ":", "", 3))
                Return RAApparentValue
            End Get
        End Property

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
        ReadOnly Property DECApparent() As Double Implements ITransform.DECApparent
            Get
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read DECApparent before SetJ2000 or SetApparent has been called")
                Recalculate()
                TL.LogMessage("DECApparent Get", Utl.DegreesToDMS(DECApparentValue, ":", ":", "", 3))
                Return DECApparentValue
            End Get
        End Property

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
        ReadOnly Property AzimuthTopocentric() As Double Implements ITransform.AzimuthTopocentric
            Get
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read AzimuthTopocentric before SetJ2000 or SetApparent has been called")
                RequiresRecalculate = True 'Force a recalculation of Azimuth
                Recalculate()
                CheckSet("AzimuthTopocentric", AzimuthTopoValue, "Azimuth topocentric can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("AzimuthTopocentric Get", Utl.DegreesToDMS(AzimuthTopoValue, ":", ":", "", 3))
                Return AzimuthTopoValue
            End Get
        End Property

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
        ReadOnly Property ElevationTopocentric() As Double Implements ITransform.ElevationTopocentric
            Get
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read ElevationTopocentric before SetJ2000 or SetApparent has been called")
                RequiresRecalculate = True 'Force a recalculation of Elevation
                Recalculate()
                CheckSet("ElevationTopocentric", ElevationTopoValue, "Elevation topocentric can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("ElevationTopocentric Get", Utl.DegreesToDMS(ElevationTopoValue, ":", ":", "", 3))
                Return ElevationTopoValue
            End Get
        End Property

        ''' <summary>
        ''' Sets the topocentric azimuth and elevation
        ''' </summary>
        ''' <param name="Azimuth">Topocentric Azimuth in degrees</param>
        ''' <param name="Elevation">Topocentric elevation in degrees</param>
        ''' <remarks></remarks>
        Sub SetAzimuthElevation(Azimuth As Double, Elevation As Double) Implements ITransform.SetAzimuthElevation
            LastSetBy = SetBy.AzimuthElevation
            RequiresRecalculate = True
            AzimuthTopoValue = Azimuth
            ElevationTopoValue = Elevation
            TL.LogMessage("SetAzimuthElevation", "Azimuth: " & Utl.DegreesToDMS(Azimuth, ":", ":", "", 3) & ", Elevation: " & Utl.DegreesToDMS(Elevation, ":", ":", "", 3))
        End Sub

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
        Property JulianDateTT As Double Implements ITransform.JulianDateTT
            Get
                Return JDTT
            End Get
            Set(value As Double)
                JDTT = value
            End Set
        End Property
#End Region

#Region "Support Code"

        Private Sub CheckSet(ByVal Caller As String, ByVal Value As Double, ByVal ErrMsg As String)
            If Double.IsNaN(Value) Then
                TL.LogMessage(Caller, "Throwing TransformUninitialisedException: " & ErrMsg)
                Throw New Exceptions.TransformUninitialisedException(ErrMsg)
            End If
        End Sub

        Private Sub J2000ToTopo()
            Dim rc As Short, RefracOption As RefractionOption, JulianDateUT1, JulianDateTT, DeltaT As Double

            If Double.IsNaN(SiteElevValue) Then Throw New Exceptions.TransformUninitialisedException("Site elevation has not been set")
            If Double.IsNaN(SiteLatValue) Then Throw New Exceptions.TransformUninitialisedException("Site latitude has not been set")
            If Double.IsNaN(SiteLongValue) Then Throw New Exceptions.TransformUninitialisedException("Site longitude has not been set")
            If Double.IsNaN(SiteTempValue) Then Throw New Exceptions.TransformUninitialisedException("Site temperature has not been set")

            Sw.Reset() : Sw.Start()

            Location.Height = SiteElevValue
            Location.Latitude = SiteLatValue
            Location.Longitude = SiteLongValue
            Location.Pressure = 1000.0
            Location.Temperature = SiteTempValue

            Cat3.Dec = DECJ2000Value
            Cat3.RA = RAJ2000Value
            Cat3.ProMoRA = 0.0
            Cat3.ProMoDec = 0.0
            Cat3.Parallax = 0.0
            Cat3.RadialVelocity = 0.0

            JulianDateUT1 = GetJDUT1()
            JulianDateTT = GetJDTT()
            DeltaT = DeltaTCalc(GetJDUT1)
            TL.LogMessage("  J2000 To Topo", "JD UT1, JD TT, DeltaT: " & JulianDateUT1 & " " & JulianDateTT & " " & DeltaT)
            'Get unrefracted topo RA and DEC
            rc = Nov31.TopoStar(JulianDateTT, DeltaT, Cat3, Location, Accuracy.Full, RATopoValue, DECTopoValue)

            'Set refraction option variable
            If RefracValue Then
                RefracOption = RefractionOption.LocationRefraction
            Else
                RefracOption = RefractionOption.NoRefraction
            End If

            'Calculate Az/El and refracted RA and DEC if required
            TL.LogMessage("  J2000 To Topo", "  Unrefracted topocentric RA/DEC:                               " & Utl.HoursToHMS(RATopoValue, ":", ":", "", 3) & " " & _
                          Utl.DegreesToDMS(DECTopoValue, ":", ":", "", 3) & _
                          " Refraction: " & RefracValue.ToString & ", " & _
                          FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

            Sw.Reset() : Sw.Start()
            Nov31.Equ2Hor(JulianDateUT1, DeltaT, Accuracy.Full, 0.0, 0.0, Location, RATopoValue, DECTopoValue, RefracOption, ElevationTopoValue, AzimuthTopoValue, RATopoValue, DECTopoValue)
            ElevationTopoValue = 90.0 - ElevationTopoValue 'Convert zenith distance to elevation
            Sw.Stop()
            TL.LogMessage("  J2000 To Topo", "  Final topocentric RA/DEC (including refraction if specified): " & Utl.HoursToHMS(RATopoValue, ":", ":", "", 3) & " " & _
                          Utl.DegreesToDMS(DECTopoValue, ":", ":", "", 3) & _
                          " Refraction: " & RefracValue.ToString & ", " & _
                          FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")
            TL.LogMessage("  J2000 To Topo", "  Azimuth/Elevation: " & Utl.DegreesToDMS(AzimuthTopoValue, ":", ":", "", 3) & " " & _
                          Utl.DegreesToDMS(ElevationTopoValue, ":", ":", "", 3) & ", " & _
                          FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

        End Sub

        Private Sub J2000ToApparent()
            Dim rc As Short
            Sw.Reset() : Sw.Start()

            Cat3.Dec = DECJ2000Value
            Cat3.RA = RAJ2000Value
            Cat3.ProMoRA = 0.0
            Cat3.ProMoDec = 0.0
            Cat3.Parallax = 0.0
            Cat3.RadialVelocity = 0.0
            rc = Nov31.AppStar(GetJDTT, Cat3, Accuracy.Full, RAApparentValue, DECApparentValue)

            Sw.Stop()
            TL.LogMessage("  J2000 To Apparent", "  RA/Dec Apparent: " & Utl.HoursToHMS(RAApparentValue, ":", ":", "", 3) & " " & Utl.DegreesToDMS(DECApparentValue, ":", ":", "", 3) & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")
        End Sub

        Private Sub TopoToJ2000()
            TopoToJ2000(True) 'We do want to calculate azimuth and elevation
        End Sub

        Private Sub TopoToJ2000(CalculateAzEl As Boolean)
            ' April 2012 - Revised to fully take account of refraction when calaculating J2000 RA/Dec. It need to be inside the iteration loop

            Dim RAOld, DECOld, DeltaRA, DeltaDEC, RACalc, DECCalc, DeltaT, JulianDateTT As Double
            Dim ct As Integer, rc As Short, RefracOption As RefractionOption
            Dim JulianDate, JulianDateUT1, RARef, DecRef, AzRef, ElRef As Double

            If Double.IsNaN(SiteElevValue) Then Throw New Exceptions.TransformUninitialisedException("Site elevation has not been set")
            If Double.IsNaN(SiteLatValue) Then Throw New Exceptions.TransformUninitialisedException("Site latitude has not been set")
            If Double.IsNaN(SiteLongValue) Then Throw New Exceptions.TransformUninitialisedException("Site longitude has not been set")
            If Double.IsNaN(SiteTempValue) Then Throw New Exceptions.TransformUninitialisedException("Site temperature has not been set")

            Sw.Reset() : Sw.Start()
            Location.Height = SiteElevValue
            Location.Latitude = SiteLatValue
            Location.Longitude = SiteLongValue
            Location.Temperature = SiteTempValue

            JulianDate = Utl.JulianDate
            JulianDateUT1 = GetJDUT1()
            JulianDateTT = GetJDTT()
            DeltaT = DeltaTCalc(GetJDUT1)

            TL.LogMessage("  Topo To J2000", "  Julian Date: " & JulianDate & ", JulianDateTT: " & JulianDateTT & ", DeltaT: " & DeltaT & ", CalculateAzEl: " & CalculateAzEl.ToString)

            RAJ2000Value = RATopoValue
            DECJ2000Value = DECTopoValue
            ct = 0
            Do
                ct += 1
                RAOld = RAJ2000Value
                DECOld = DECJ2000Value

                ' Appy refraction to the RA/Dec coordinates if required
                If RefracValue Then
                    RefracOption = RefractionOption.LocationRefraction
                Else
                    RefracOption = RefractionOption.NoRefraction
                End If

                Cat3.RA = RAOld
                Cat3.Dec = DECOld
                Cat3.ProMoRA = 0.0
                Cat3.ProMoDec = 0.0
                Cat3.Parallax = 0.0
                Cat3.RadialVelocity = 0.0
                rc = Nov31.TopoStar(JulianDateTT, DeltaT, Cat3, Location, Accuracy.Full, RACalc, DECCalc)
                Nov31.Equ2Hor(JulianDateUT1, DeltaT, Accuracy.Full, 0.0, 0.0, Location, RACalc, DECCalc, RefracOption, ElRef, AzRef, RARef, DecRef)
                If CalculateAzEl Then ' Update the az/el values
                    AzimuthTopoValue = AzRef
                    ElevationTopoValue = 90.0 - ElRef 'Convert zenith distance to elevation
                    'TL.LogMessage("  Ref", "  " & ct.ToString & " " & Utl.HoursToHMS(RACalc, ":", ":", "", 3) & " " & Utl.HoursToHMS(RARef, ":", ":", "", 3) & " " & Utl.DegreesToDMS(DECCalc, ":", ":", "", 3) & " " & Utl.DegreesToDMS(DecRef, ":", ":", "", 3))
                End If
                DeltaRA = RARef - RAOld
                DeltaRA = AstroUtl.Range(DeltaRA, -12.0, True, 12.0, True) ' Constrain DeltaRA to the range -12.0 to +12.0
                DeltaDEC = DecRef - DECOld
                'TL.LogMessage("  Topo", "  " & ct.ToString & " " & Utl.HoursToHMS(RACalc, ":", ":", "", 3) & " " & Utl.HoursToHMS(DeltaRA, ":", ":", "", 3) & " " & Utl.DegreesToDMS(DECCalc, ":", ":", "", 3) & " " & Utl.DegreesToDMS(DeltaDEC, ":", ":", "", 3))

                RAJ2000Value = RATopoValue - DeltaRA
                DECJ2000Value = DECTopoValue - DeltaDEC

                'TL.LogMessage("  Calc", "  " & ct.ToString & " " & Utl.HoursToHMS(RAJ2000Value, ":", ":", "", 3) & " " & Utl.HoursToHMS(Abs(RAOld - RAJ2000Value), ":", ":", "", 3) & " " & Utl.DegreesToDMS(DECJ2000Value, ":", ":", "", 3) & " " & Utl.DegreesToDMS(Abs(DECOld - DECJ2000Value), ":", ":", "", 3))


                TL.LogMessage("  Iteration", "  " & ct.ToString & " " & Utl.HoursToHMS(RAJ2000Value, ":", ":", "", 3) & " " & Utl.DegreesToDMS(DECJ2000Value, ":", ":", "", 3))
                'TL.BlankLine()
            Loop Until (ct > 5) And ((ct = 20) Or (Abs(RAOld - RAJ2000Value) < 1 / (24 * 60 * 60) And Abs(DECOld - DECJ2000Value) < 1 / (24 * 60 * 60)))

            Sw.Stop()
            TL.LogMessage("  Topo To J2000", "  " & Utl.HoursToHMS(RAJ2000Value, ":", ":", "", 3) & " " & Utl.DegreesToDMS(DECJ2000Value, ":", ":", "", 3) & ", " & ct & " iterations " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

            If ct = 20 Then
                TL.LogMessage("", "Iteration loop counter is 20 so routine failed to converge, throw an error to indicate this")
                Throw New ASCOM.Astrometry.Exceptions.ConvergenceFailureException("Transform.TopoToJ2000 failed to converge after 20 iterations")
            End If

        End Sub

        Private Sub ApparentToJ2000()
            Dim rc As Short, ErrorMessage As String

            Sw.Reset() : Sw.Start()
            rc = Nov31.MeanStar(GetJDTT, RAApparentValue, DECApparentValue, Accuracy.Full, RAJ2000Value, DECJ2000Value)
            If rc <> 0 Then
                TL.LogMessage("ApparentToJ2000", "MeanStar bad return value: " & rc & ", RAApparentValue: " & RAApparentValue & ", DECApparentValue: " & DECApparentValue & ", RAJ2000Value: " & RAJ2000Value & ", DecJ2000Value: " & DECJ2000Value)
                Select Case rc
                    Case 1
                        ErrorMessage = "NOVAS MeanStar function failed to converge after 30 iterations"
                    Case 10 To 19
                        ErrorMessage = "Error from NOVAS function Vector2RaDec"
                    Case Else
                        ErrorMessage = "Error from NOVAS function AppStar"
                End Select
                Throw New Astrometry.Exceptions.NOVASFunctionException(ErrorMessage, "MeanStar", rc)
            End If
            Sw.Stop()
            TL.LogMessage("  Apparent To J2000", "  " & Utl.HoursToHMS(RAJ2000Value, ":", ":", "", 3) & " " & Utl.DegreesToDMS(DECJ2000Value, ":", ":", "", 3) & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

        End Sub

        Private Sub Recalculate() 'Calculate values for derrived co-ordinates
            SwRecalculate.Reset() : SwRecalculate.Start()
            If RequiresRecalculate Or (RefracValue = True) Then
                TL.LogMessage("Recalculate", "Requires Recalculate: " & RequiresRecalculate.ToString & ", Refraction: " & RefracValue.ToString)
                Select Case LastSetBy
                    Case SetBy.J2000 'J2000 coordinates have bee set so calculate apparent and topocentric coords
                        TL.LogMessage("  Recalculate", "  Values last set by SetJ2000")
                        'Check whether required topo values have been set
                        If (Not Double.IsNaN(SiteLatValue)) And _
                           (Not Double.IsNaN(SiteLongValue)) And _
                           (Not Double.IsNaN(SiteElevValue)) And _
                           (Not Double.IsNaN(SiteTempValue)) Then
                            J2000ToTopo() 'All required site values present so calc Topo values
                        Else 'Set to NaN
                            RATopoValue = Double.NaN
                            DECTopoValue = Double.NaN
                            AzimuthTopoValue = Double.NaN
                            ElevationTopoValue = Double.NaN
                        End If
                        Call J2000ToApparent()
                    Case SetBy.Topocentric 'Topocentric co-ordinates have been set so calculate J2000 and apparent coords
                        TL.LogMessage("  Recalculate", "  Values last set by SetTopocentric")
                        'Check whether required topo values have been set
                        If (Not Double.IsNaN(SiteLatValue)) And _
                           (Not Double.IsNaN(SiteLongValue)) And _
                           (Not Double.IsNaN(SiteElevValue)) And _
                           (Not Double.IsNaN(SiteTempValue)) Then 'They have so calculate remaining values
                            Call TopoToJ2000()
                            Call J2000ToApparent()
                        Else 'Set the topo and apaprent values to NaN
                            RAJ2000Value = Double.NaN
                            DECJ2000Value = Double.NaN
                            RAApparentValue = Double.NaN
                            DECApparentValue = Double.NaN
                            AzimuthTopoValue = Double.NaN
                            ElevationTopoValue = Double.NaN
                        End If
                    Case SetBy.Apparent 'Apparent values have been set so calculate J2000 values and topo values if appropriate
                        TL.LogMessage("  Recalculate", "  Values last set by SetApparent")
                        Call ApparentToJ2000() 'Calculate J2000 value
                        'Check whether required topo values have been set
                        If (Not Double.IsNaN(SiteLatValue)) And _
                           (Not Double.IsNaN(SiteLongValue)) And _
                           (Not Double.IsNaN(SiteElevValue)) And _
                           (Not Double.IsNaN(SiteTempValue)) Then
                            J2000ToTopo() 'All required site values present so calc Topo values
                        Else
                            RATopoValue = Double.NaN
                            DECTopoValue = Double.NaN
                            AzimuthTopoValue = Double.NaN
                            ElevationTopoValue = Double.NaN
                        End If
                    Case SetBy.AzimuthElevation
                        TL.LogMessage("  Recalculate", "  Values last set by AzimuthElevation")
                        If (Not Double.IsNaN(SiteLatValue)) And _
                           (Not Double.IsNaN(SiteLongValue)) And _
                           (Not Double.IsNaN(SiteElevValue)) And _
                           (Not Double.IsNaN(SiteTempValue)) Then
                            Call AzElToTopo()
                            Call TopoToJ2000(False) 'Calculate J2000 value but don't update the azimuth and elevation that were set by the user
                            Call J2000ToApparent()
                        Else
                            RAJ2000Value = Double.NaN
                            DECJ2000Value = Double.NaN
                            RAApparentValue = Double.NaN
                            DECApparentValue = Double.NaN
                            RATopoValue = Double.NaN
                            DECTopoValue = Double.NaN
                        End If
                    Case Else 'Neither SetJ2000 nor SetTopocentric nor SetApparent have been called, so throw an exception
                        TL.LogMessage("Recalculate", "Neither SetJ2000 nor SetTopocentric nor SetApparent have been called. Throwing TransforUninitialisedException")
                        Throw New Exceptions.TransformUninitialisedException("Can't recalculate Transform object values because neither SetJ2000 nor SetTopocentric nor SetApparent have been called")
                End Select
                TL.LogMessage("  Recalculate", "  Completed in " & FormatNumber(SwRecalculate.Elapsed.TotalMilliseconds, 2) & "ms")
                RequiresRecalculate = False 'Reset the recalculate flag
            Else
                TL.LogMessage("  Recalculate", "No parameters have changed, refraction is " & RefracValue & ", recalculation not required")
            End If
            SwRecalculate.Stop()
        End Sub

        Private Sub AzElToTopo()
            Dim GreenwichApparentSiderealTime, LocalApparentSiderealTime, RefractionCorrection, ElevationDegRefracted, ElevationRadians, AzimuthRadians As Double
            Dim DeclinationSin, DeclinationRadians, LatitudeRadians, HourAngleRadians, HourAngle As Double
            Dim RefracOption As RefractionOption
            Dim DeltaT, JulianDate, JulianDateTT As Double
            Dim EstimatedElevation, EstimatedAzimuth, LastAltitude, LastAzimuth As Double, LoopCount As Integer
            Dim RACalculated, DECCalculated As Double
            Dim Az, El As Double
            Dim ZdN, AzN, RaRN, DecRN As Double

            Const RadToDeg As Double = 180.0 / PI
            Const DegToRad As Double = PI / 180.0
            Const RadToHours As Double = 180.0 / (PI * 15.0)
            Const Deg2Hours As Double = 1.0 / 15.0

            Sw.Reset() : Sw.Start()

            If Double.IsNaN(SiteElevValue) Then Throw New Exceptions.TransformUninitialisedException("Site elevation has not been set")
            If Double.IsNaN(SiteLatValue) Then Throw New Exceptions.TransformUninitialisedException("Site latitude has not been set")
            If Double.IsNaN(SiteLongValue) Then Throw New Exceptions.TransformUninitialisedException("Site longitude has not been set")
            If Double.IsNaN(SiteTempValue) Then Throw New Exceptions.TransformUninitialisedException("Site temperature has not been set")

            Location.Height = SiteElevValue
            Location.Latitude = SiteLatValue
            Location.Longitude = SiteLongValue
            Location.Temperature = SiteTempValue

            Nov31.SiderealTime(AstroUtl.JulianDateUT1(0.0), 0.0, AstroUtl.DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, GreenwichApparentSiderealTime)
            LocalApparentSiderealTime = AstroUtl.ConditionHA(GreenwichApparentSiderealTime + SiteLongValue * Deg2Hours)
            TL.LogMessage("  AzEl To Topo", "  Sidereal time: " & Utl.HoursToHMS(GreenwichApparentSiderealTime, ":", ":", "", 3) & " " & Utl.HoursToHMS(LocalApparentSiderealTime, ":", ":", "", 3))

            TL.LogMessage("  AzEl To Topo", "  Azimuth: " & Utl.DegreesToDMS(AzimuthTopoValue, ":", ":", "", 3))

            LatitudeRadians = SiteLatValue * DegToRad
            TL.LogMessage("  AzEl To Topo", "  Site Latitude: " & Utl.DegreesToDMS(SiteLatValue, ":", ":", "", 3))

            JulianDate = Utl.JulianDate
            DeltaT = DeltaTCalc(JulianDate)
            JulianDateTT = AstroUtl.JulianDateTT(0.0)
            TL.LogMessage("  AzEl To Topo", "  Julian Date: " & JulianDate & ", JulianDateTT: " & JulianDateTT & ", DeltaT: " & DeltaT)

            'Set refraction option variable
            If RefracValue Then
                RefracOption = RefractionOption.LocationRefraction
                TL.LogMessage("  AzEl To Topo", "  Refraction enabled")
                RefractionCorrection = Nov31.Refract(Location, RefracOption, 90.0 - ElevationTopoValue)
            Else
                RefracOption = RefractionOption.NoRefraction
                TL.LogMessage("  AzEl To Topo", "  Refraction disabled")
                RefractionCorrection = 0.0
            End If
            ElevationDegRefracted = ElevationTopoValue - RefractionCorrection
            TL.LogMessage("  AzEl To Topo", "  Elevation: " & Utl.DegreesToDMS(ElevationTopoValue, ":", ":", "", 3) & ", Refracted: " & Utl.DegreesToDMS(ElevationDegRefracted, ":", ":", "", 3) & ", Refraction: " & Utl.DegreesToDMS(RefractionCorrection, ":", ":", "", 3))
            EstimatedAzimuth = AzimuthTopoValue
            EstimatedElevation = ElevationDegRefracted
            LoopCount = 0

            Do
                LoopCount += 1
                LastAzimuth = EstimatedAzimuth
                LastAltitude = EstimatedElevation
                TL.LogMessage("  Iteration", "  " & LoopCount.ToString & _
                              " Estimated Azimuth: " & Utl.DegreesToDMS(Az, ":", ":", "", 3) & _
                              ", Estimated Elevation: " & Utl.DegreesToDMS(El, ":", ":", "", 3) & _
                              ", Estimated HA: " & Utl.HoursToHMS(HourAngle, ":", ":", "", 3) & _
                              ", Estimated RA: " & Utl.HoursToHMS(RATopoValue, ":", ":", "", 3) & _
                              ", Estimated Declination: " & Utl.DegreesToDMS(DECTopoValue, ":", ":", "", 3))

                AzimuthRadians = EstimatedAzimuth * DegToRad
                ElevationRadians = EstimatedElevation * DegToRad

                DeclinationSin = (Sin(ElevationRadians) * Sin(LatitudeRadians)) + (Cos(ElevationRadians) * Cos(LatitudeRadians) * Cos(AzimuthRadians))
                DeclinationRadians = Atan2(DeclinationSin, Sqrt(-DeclinationSin * DeclinationSin + 1.0))
                DECCalculated = DeclinationRadians * RadToDeg

                HourAngleRadians = Atan2(-Sin(AzimuthRadians) * Cos(ElevationRadians), -Cos(AzimuthRadians) * Sin(LatitudeRadians) * Cos(ElevationRadians) + Sin(ElevationRadians) * Cos(LatitudeRadians))
                HourAngle = HourAngleRadians * RadToHours
                RACalculated = AstroUtl.ConditionRA(LocalApparentSiderealTime - HourAngle)

                Nov31.Equ2Hor(GetJDUT1, DeltaT, Accuracy.Full, 0.0, 0.0, Location, RACalculated, DECCalculated, RefracOption, El, Az, RATopoValue, DECTopoValue)
                El = 90.0 - El 'Convert zenith distance to elevation
                EstimatedAzimuth = EstimatedAzimuth + (AzimuthTopoValue - Az)
                EstimatedElevation = EstimatedElevation + (ElevationTopoValue - El)
            Loop Until (LoopCount = 20) Or (((ElevationTopoValue - El) < 0.00001) And ((AzimuthTopoValue - Az) < 0.00001))

            Nov31.Equ2Hor(GetJDUT1, DeltaT, Accuracy.Full, 0.0, 0.0, Location, RACalculated, DECCalculated, RefracOption, ZdN, AzN, RaRN, DecRN)
            TL.LogMessage("  AzEl To Topo", "  NOVAS3 RA: " & Utl.HoursToHMS(RaRN, ":", ":", "", 3) & ", Declination: " & Utl.DegreesToDMS(DecRN, ":", ":", "", 3))
            TL.LogMessage("  AzEl To Topo", "  NOVAS3 Azimuth: " & Utl.DegreesToDMS(AzN, ":", ":", "", 3) & ", Elevation: " & Utl.DegreesToDMS(90.0 - ZdN, ":", ":", "", 3) & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

            Sw.Stop()
            TL.BlankLine()
        End Sub

        Private Function GetJDTT() As Double
            Dim Retval As Double

            If JDTT = 0.0 Then
                Retval = AstroUtl.JulianDateTT(0.0)
            Else
                Retval = JDTT
            End If

            TL.LogMessage("GetJDTT", Retval.ToString)
            Return Retval
        End Function

        Private Function GetJDUT1() As Double
            Dim Retval As Double
            If JDTT = 0.0 Then
                Retval = AstroUtl.JulianDateUT1(0.0)
            Else
                Retval = JDTT - (AstroUtl.DeltaT() / SECPERDAY) ' UT1 = TT - DeltaT
            End If
            TL.LogMessage("GetJDUT1", Retval.ToString)
            Return Retval
        End Function

        Private Sub CheckGAC()
            Dim strPath As String
            TL.LogMessage("CheckGAC", "Started")
            strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
            TL.LogMessage("CheckGAC", "Assembly path: " & strPath)
        End Sub

        Private Sub RunningVersions(ByVal TL As TraceLogger)
            Dim AssemblyNames() As AssemblyName
            TL.LogMessage("Versions", "Utilities version: " & Assembly.GetExecutingAssembly.GetName.Version.ToString)
            TL.LogMessage("Versions", "CLR version: " & System.Environment.Version.ToString)
            AssemblyNames = Assembly.GetExecutingAssembly.GetReferencedAssemblies

            'Get Operating system information
            Dim OS As System.OperatingSystem = System.Environment.OSVersion
            TL.LogMessage("Versions", "OS Version " & OS.Platform & " Service Pack: " & OS.ServicePack & " Full: " & OS.VersionString)
            'Get file system information
            Dim MachineName As String = System.Environment.MachineName
            Dim ProcCount As Integer = System.Environment.ProcessorCount
            Dim SysDir As String = System.Environment.SystemDirectory
            Dim WorkSet As Long = System.Environment.WorkingSet
            TL.LogMessage("Versions", "Machine name: " & MachineName & " Number of processors: " & ProcCount & " System directory: " & SysDir & " Working set size: " & WorkSet & " bytes")

            'Get fully qualified paths to particular directories in a non OS specific way
            'There are many more options in the SpecialFolders Enum than are shown here!
            TL.LogMessage("Versions", "Application Data: " & System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
            TL.LogMessage("Versions", "Common Files: " & System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles))
            TL.LogMessage("Versions", "My Documents: " & System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
            TL.LogMessage("Versions", "Program Files: " & System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles))
            TL.LogMessage("Versions", "System: " & System.Environment.GetFolderPath(Environment.SpecialFolder.System))
            TL.LogMessage("Versions", "Current: " & System.Environment.CurrentDirectory)
        End Sub

        Private Function ValidateRA(Caller As String, RA As Double) As Double
            If (RA < 0.0) Or (RA >= 24.0) Then Throw New InvalidValueException(Caller, RA.ToString(), "0 to 23.9999")
            Return RA
        End Function

        Private Function ValidateDec(Caller As String, Dec As Double) As Double
            If (Dec < 0.0) Or (Dec > 90.0) Then Throw New InvalidValueException(Caller, Dec.ToString(), "0 to 90.0")
            Return Dec
        End Function

#End Region

    End Class
End Namespace