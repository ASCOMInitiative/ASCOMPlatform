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
        Private Utl As Util, AstroUtl As AstroUtils.AstroUtils
        Dim SOFA As SOFA.SOFA
        Private RAJ2000Value, RATopoValue, DECJ2000Value, DECTopoValue, SiteElevValue, SiteLatValue, SiteLongValue, SiteTempValue As Double
        Private RAApparentValue, DECApparentValue, AzimuthTopoValue, ElevationTopoValue, JulianDateTTValue, JulianDateUTCValue As Double
        Private RefracValue, RequiresRecalculate As Boolean
        Private LastSetBy As SetBy

        Private TL As TraceLogger
        Private Sw, SwRecalculate As Stopwatch

        Private Const HOURS2RADIANS As Double = Math.PI / 12.0
        Private Const DEGREES2RADIANS As Double = Math.PI / 180.0
        Private Const RADIANS2HOURS As Double = 12.0 / Math.PI
        Private Const RADIANS2DEGREES As Double = 180.0 / Math.PI
        Private Const TWOPI As Double = 2.0 * Math.PI

        Private Const DATE_FORMAT As String = "dd/MM/yyyy HH:mm:ss.fff"

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
            SOFA = New SOFA.SOFA

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
            JulianDateTTValue = 0 ' Initialise to a value that forces the current PC date time to be used in determining the TT Julian date of interest
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
                TL.LogMessage("SiteLatitude Get", FormatDec(SiteLatValue))
                Return SiteLatValue
            End Get
            Set(ByVal value As Double)
                If SiteLatValue <> value Then RequiresRecalculate = True
                SiteLatValue = value
                TL.LogMessage("SiteLatitude Set", FormatDec(value))
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
                TL.LogMessage("SiteLongitude Get", FormatDec(SiteLongValue))
                Return SiteLongValue
            End Get
            Set(ByVal value As Double)
                If SiteLongValue <> value Then RequiresRecalculate = True
                SiteLongValue = value
                TL.LogMessage("SiteLongitude Set", FormatDec(value))
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
            RAJ2000Value = ValidateRA("SetJ2000", RA)
            DECJ2000Value = ValidateDec("SetJ2000", DEC)
            TL.LogMessage("SetJ2000", "RA: " & Format(RA) & ", DEC: " & FormatDec(DEC))
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
            TL.LogMessage("SetApparent", "RA: " & Format(RA) & ", DEC: " & FormatDec(DEC))
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
            RATopoValue = ValidateRA("SetTopocentric", RA)
            DECTopoValue = ValidateDec("SetTopocentric", DEC)
            TL.LogMessage("SetTopocentric", "RA: " & Format(RA) & ", DEC: " & FormatDec(DEC))
        End Sub

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
            TL.LogMessage("SetAzimuthElevation", "Azimuth: " & FormatDec(Azimuth) & ", Elevation: " & FormatDec(Elevation))
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
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read RAJ2000 before a SetXX method has been called")
                Recalculate()
                CheckSet("RAJ2000", RAJ2000Value, "RA J2000 can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("RAJ2000 Get", FormatRA(RAJ2000Value))
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
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read DECJ2000 before a SetXX method has been called")
                Recalculate()
                CheckSet("DecJ2000", DECJ2000Value, "DEC J2000 can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("DecJ2000 Get", FormatDec(DECJ2000Value))
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
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read RATopocentric before a SetXX method  has been called")
                Recalculate()
                CheckSet("RATopocentric", RATopoValue, "RA topocentric can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("RATopocentric Get", FormatRA(RATopoValue))
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
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read DECTopocentric before a SetXX method has been called")
                Recalculate()
                CheckSet("DECTopocentric", DECTopoValue, "DEC topocentric can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("DECTopocentric Get", FormatDec(DECTopoValue))
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
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read DECApparent before a SetXX method has been called")
                Recalculate()
                TL.LogMessage("RAApparent Get", FormatRA(RAApparentValue))
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
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read DECApparent before a SetXX method has been called")
                Recalculate()
                TL.LogMessage("DECApparent Get", FormatDec(DECApparentValue))
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
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read AzimuthTopocentric before a SetXX method has been called")
                RequiresRecalculate = True 'Force a recalculation of Azimuth
                Recalculate()
                CheckSet("AzimuthTopocentric", AzimuthTopoValue, "Azimuth topocentric can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("AzimuthTopocentric Get", FormatDec(AzimuthTopoValue))
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
                If LastSetBy = SetBy.Never Then Throw New Exceptions.TransformUninitialisedException("Attempt to read ElevationTopocentric before a SetXX method has been called")
                RequiresRecalculate = True 'Force a recalculation of Elevation
                Recalculate()
                CheckSet("ElevationTopocentric", ElevationTopoValue, "Elevation topocentric can not be derived from the information provided. Are site parameters set?")
                TL.LogMessage("ElevationTopocentric Get", FormatDec(ElevationTopoValue))
                Return ElevationTopoValue
            End Get
        End Property

        ''' <summary>
        ''' Sets or returns the Julian date on the Terrestrial Time timescale for which the transform will be made
        ''' </summary>
        ''' <value>Julian date (Terrestrial Time) of the transform</value>
        ''' <returns>Terrestrial Time Julian date that will be used by Transform or zero if the PC's current clock value will be used to calculate the Julian date.</returns>
        ''' <remarks>This method was introduced in May 2012. Previously, Transform used the current date-time of the PC when calculating transforms; 
        ''' this remains the default behaviour for backward compatibility.
        ''' The inital value of this parameter is 0.0, which is a special value that forces Transform to replicate original behaviour by determining the  
        ''' Julian date from the PC's current date and time. If this property is non zero, that particular terrestrial time Julian date is used in preference 
        ''' to the value derrived from the PC's clock.
        ''' <para>Only one of JulianDateTT or JulianDateUTC needs to be set. Use whichever is more readily available, there is no
        ''' need to set both values. Transform will use the last set value of either JulianDateTT or JulianDateUTC as the basis for its calculations.</para></remarks>
        Property JulianDateTT As Double Implements ITransform.JulianDateTT
            Get
                Return JulianDateTTValue
            End Get
            Set(value As Double)
                Dim tai1, tai2, utc1, utc2 As Double
                JulianDateTTValue = value
                RequiresRecalculate = True ' Force a recalculation because the Julian date has changed

                If JulianDateTTValue <> 0.0 Then
                    'Calculate UTC
                    If (SOFA.TtTai(JulianDateTTValue, 0.0, tai1, tai2) <> 0) Then TL.LogMessage("JulianDateUTC Set", "Utctai - Bad return code")
                    If (SOFA.TaiUtc(tai1, tai2, utc1, utc2) <> 0) Then TL.LogMessage("JulianDateUTC Set", "Taitt - Bad return code")
                    JulianDateUTCValue = utc1 + utc2

                    TL.LogMessage("JulianDateTT Set", JulianDateTTValue.ToString & " " & Julian2DateTime(JulianDateTTValue).ToString(DATE_FORMAT) & ", JDUTC: " & Julian2DateTime(JulianDateUTCValue).ToString(DATE_FORMAT))
                Else ' Handle special case of 0.0
                    JulianDateUTCValue = 0.0
                    TL.LogMessage("JulianDateTT Set", "Calculations will now be based on PC the DateTime")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets or returns the Julian date on the UTC timescale for which the transform will be made
        ''' </summary>
        ''' <value>Julian date (UTC) of the transform</value>
        ''' <returns>UTC Julian date that will be used by Transform or zero if the PC's current clock value will be used to calculate the Julian date.</returns>
        ''' <remarks>Introduced in April 2014 as an alternative to JulianDateTT. Only one of JulianDateTT or JulianDateUTC needs to be set. Use whichever is more readily available, there is no
        ''' need to set both values. Transform will use the last set value of either JulianDateTT or JulianDateUTC as the basis for its calculations.</remarks>
        Property JulianDateUTC As Double Implements ITransform.JulianDateUTC
            Get
                Return JulianDateUTCValue
            End Get
            Set(value As Double)
                Dim tai1, tai2, tt1, tt2 As Double
                JulianDateUTCValue = value
                RequiresRecalculate = True ' Force a recalculation because the Julian date has changed

                If JulianDateUTCValue <> 0.0 Then
                    ' Calculate Terrestrial Time equivalent
                    If (SOFA.UtcTai(JulianDateUTCValue, 0.0, tai1, tai2) <> 0) Then TL.LogMessage("JulianDateUTC Set", "Utctai - Bad return code")
                    If (SOFA.TaiTt(tai1, tai2, tt1, tt2) <> 0) Then TL.LogMessage("JulianDateUTC Set", "Taitt - Bad return code")
                    JulianDateTTValue = tt1 + tt2

                    TL.LogMessage("JulianDateUTC Set", JulianDateTTValue.ToString & " " & Julian2DateTime(JulianDateUTCValue).ToString(DATE_FORMAT) & ", JDTT: " & Julian2DateTime(JulianDateTTValue).ToString(DATE_FORMAT))
                Else ' Handle special case of 0.0
                    JulianDateTTValue = 0.0
                    TL.LogMessage("JulianDateUTC Set", "Calculations will now be based on PC the DateTime")
                End If
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
            Dim DeltaT, DUT1, JDUTCSofa As Double
            Dim aob, zob, hob, dob, rob, eo As Double
            Dim JDUTCSofaDateTime As DateTime

            If Double.IsNaN(SiteElevValue) Then Throw New Exceptions.TransformUninitialisedException("Site elevation has not been set")
            If Double.IsNaN(SiteLatValue) Then Throw New Exceptions.TransformUninitialisedException("Site latitude has not been set")
            If Double.IsNaN(SiteLongValue) Then Throw New Exceptions.TransformUninitialisedException("Site longitude has not been set")
            If Double.IsNaN(SiteTempValue) Then Throw New Exceptions.TransformUninitialisedException("Site temperature has not been set")

            Sw.Reset() : Sw.Start()

            JDUTCSofa = GetJDUTCSofa()
            DeltaT = DeltaTCalc(JDUTCSofa)
            DUT1 = AstroUtl.DeltaUT(JDUTCSofa)

            JDUTCSofaDateTime = Julian2DateTime(JDUTCSofa)

            Sw.Reset() : Sw.Start()

            If RefracValue Then ' Include refraction
                SOFA.CelestialToObserved(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0, 0.0, 0.0, 0.0, JDUTCSofa, 0.0, DUT1, _
                                         SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0, 0.0, 1000.0, SiteTempValue, 0.8, 0.57, aob, zob, hob, dob, rob, eo)
            Else ' No refraction
                SOFA.CelestialToObserved(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0, 0.0, 0.0, 0.0, JDUTCSofa, 0.0, DUT1, _
                                         SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, aob, zob, hob, dob, rob, eo)
            End If

            RATopoValue = SOFA.Anp(rob - eo) * RADIANS2HOURS ' // Convert CIO RA to equinox of date RA by subtracting the equation of the origins and convert from radians to hours
            DECTopoValue = dob * RADIANS2DEGREES ' Convert Dec from radians to degrees
            AzimuthTopoValue = aob * RADIANS2DEGREES
            ElevationTopoValue = 90.0 - zob * RADIANS2DEGREES

            TL.LogMessage("  J2000 To Topo", "  Topocentric RA/DEC (including refraction if specified):  " & FormatRA(RATopoValue) & " " & FormatDec(DECTopoValue) & " Refraction: " & RefracValue.ToString & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")
            TL.LogMessage("  J2000 To Topo", "  Azimuth/Elevation: " & FormatDec(AzimuthTopoValue) & " " & FormatDec(ElevationTopoValue) & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")
            TL.LogMessage("  J2000 To Topo", "  Completed")
            TL.BlankLine()
        End Sub

        Private Sub J2000ToApparent()
            Dim ri, di, eo As Double
            Dim JDTTSofa As Double

            Sw.Reset() : Sw.Start()
            JDTTSofa = GetJDTTSofa()

            SOFA.CelestialToIntermediate(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0, 0.0, 0.0, 0.0, JDTTSofa, 0.0, ri, di, eo)
            RAApparentValue = SOFA.Anp(ri - eo) * RADIANS2HOURS ' // Convert CIO RA to equinox of date RA by subtracting the equation of the origins and convert from radians to hours
            DECApparentValue = di * RADIANS2DEGREES ' Convert Dec from radians to degrees

            TL.LogMessage("  J2000 To Apparent", "  Apparent RA/Dec:   " & FormatRA(RAApparentValue) & " " & FormatDec(DECApparentValue) & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

        End Sub

        Private Sub TopoToJ2000()
            Dim RACelestrial, DecCelestial, JDTTSofa, JDUTCSofa, DUT1 As Double, RetCode As Integer
            Dim aob, zob, hob, dob, rob, eo As Double

            If Double.IsNaN(SiteElevValue) Then Throw New Exceptions.TransformUninitialisedException("Site elevation has not been set")
            If Double.IsNaN(SiteLatValue) Then Throw New Exceptions.TransformUninitialisedException("Site latitude has not been set")
            If Double.IsNaN(SiteLongValue) Then Throw New Exceptions.TransformUninitialisedException("Site longitude has not been set")
            If Double.IsNaN(SiteTempValue) Then Throw New Exceptions.TransformUninitialisedException("Site temperature has not been set")

            Sw.Reset() : Sw.Start()

            JDUTCSofa = GetJDUTCSofa()
            JDTTSofa = GetJDTTSofa()
            DUT1 = AstroUtl.DeltaUT(JDUTCSofa)

            Sw.Reset() : Sw.Start()
            If RefracValue Then ' Refraction is requuired
                RetCode = SOFA.ObservedToCelestial("R", SOFA.Anp(RATopoValue * HOURS2RADIANS + SOFA.Eo06a(JDTTSofa, 0.0)), DECTopoValue * DEGREES2RADIANS, JDUTCSofa, 0.0, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0, 0.0, 1000, SiteTempValue, 0.85, 0.57, RACelestrial, DecCelestial)
            Else
                RetCode = SOFA.ObservedToCelestial("R", SOFA.Anp(RATopoValue * HOURS2RADIANS + SOFA.Eo06a(JDTTSofa, 0.0)), DECTopoValue * DEGREES2RADIANS, JDUTCSofa, 0.0, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, RACelestrial, DecCelestial)
            End If

            RAJ2000Value = RACelestrial * RADIANS2HOURS
            DECJ2000Value = DecCelestial * RADIANS2DEGREES
            TL.LogMessage("  Topo To J2000", "  J2000 RA/Dec:" & FormatRA(RAJ2000Value) & " " & FormatDec(DECJ2000Value) & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

            ' Now calculate the corresponding AzEl values from the J2000 values
            Sw.Reset() : Sw.Start()
            If RefracValue Then ' Include refraction
                SOFA.CelestialToObserved(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0, 0.0, 0.0, 0.0, JDUTCSofa, 0.0, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0, 0.0, 1000.0, SiteTempValue, 0.8, 0.57, aob, zob, hob, dob, rob, eo)
            Else ' No refraction
                SOFA.CelestialToObserved(RAJ2000Value * HOURS2RADIANS, DECJ2000Value * DEGREES2RADIANS, 0.0, 0.0, 0.0, 0.0, JDUTCSofa, 0.0, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, aob, zob, hob, dob, rob, eo)
            End If

            AzimuthTopoValue = aob * RADIANS2DEGREES
            ElevationTopoValue = 90.0 - zob * RADIANS2DEGREES

            TL.LogMessage("  Topo To J2000", "  Azimuth/Elevation: " & FormatDec(AzimuthTopoValue) & " " & FormatDec(ElevationTopoValue) & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

        End Sub

        Private Sub ApparentToJ2000()
            Dim JulianDateTTSofa, RACelestial, DecCelestial, JulianDateUTCSofa, eo As Double

            Sw.Reset() : Sw.Start()

            JulianDateTTSofa = GetJDTTSofa()
            JulianDateUTCSofa = GetJDUTCSofa()

            SOFA.IntermediateToCelestial(SOFA.Anp(RAApparentValue * HOURS2RADIANS + SOFA.Eo06a(JulianDateUTCSofa, 0.0)), DECApparentValue * DEGREES2RADIANS, JulianDateTTSofa, 0.0, RACelestial, DecCelestial, eo)
            RAJ2000Value = RACelestial * RADIANS2HOURS
            DECJ2000Value = DecCelestial * RADIANS2DEGREES
            TL.LogMessage("  Apparent To J2000", "  J2000 RA/Dec" & FormatRA(RAJ2000Value) & " " & FormatDec(DECJ2000Value) & ", " & FormatNumber(Sw.Elapsed.TotalMilliseconds, 2) & "ms")

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
                            Call AzElToJ2000()
                            Call J2000ToTopo()
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

        Private Sub AzElToJ2000()
            Dim RetCode As Integer, JulianDateUTCSofa, JulianDateTTSofa, RACelestial, DecCelestial, DUT1 As Double

            Sw.Reset() : Sw.Start()

            If Double.IsNaN(SiteElevValue) Then Throw New Exceptions.TransformUninitialisedException("Site elevation has not been set")
            If Double.IsNaN(SiteLatValue) Then Throw New Exceptions.TransformUninitialisedException("Site latitude has not been set")
            If Double.IsNaN(SiteLongValue) Then Throw New Exceptions.TransformUninitialisedException("Site longitude has not been set")
            If Double.IsNaN(SiteTempValue) Then Throw New Exceptions.TransformUninitialisedException("Site temperature has not been set")

            JulianDateUTCSofa = GetJDUTCSofa()
            JulianDateTTSofa = GetJDTTSofa()
            DUT1 = AstroUtl.DeltaUT(JulianDateUTCSofa)

            If RefracValue Then ' Refraction is requuired
                RetCode = SOFA.ObservedToCelestial("A", AzimuthTopoValue * DEGREES2RADIANS, (90.0 - ElevationTopoValue) * DEGREES2RADIANS, JulianDateUTCSofa, 0.0, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, SiteElevValue, 0.0, 0.0, 1000, SiteTempValue, 0.85, 0.57, RACelestial, DecCelestial)
            Else
                RetCode = SOFA.ObservedToCelestial("A", AzimuthTopoValue * DEGREES2RADIANS, (90.0 - ElevationTopoValue) * DEGREES2RADIANS, JulianDateUTCSofa, 0.0, DUT1, SiteLongValue * DEGREES2RADIANS, SiteLatValue * DEGREES2RADIANS, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, RACelestial, DecCelestial)
            End If

            RAJ2000Value = RACelestial * RADIANS2HOURS
            DECJ2000Value = DecCelestial * RADIANS2DEGREES

            TL.LogMessage("  AzEl To J2000", "  SOFA RA: " & FormatRA(RAJ2000Value) & ", Declination: " & FormatDec(DECJ2000Value))

            Sw.Stop()
            TL.BlankLine()
        End Sub

        Private Function GetJDUTCSofa() As Double
            Dim Retval, utc1, utc2 As Double, Now As DateTime

            If JulianDateUTCValue = 0.0 Then
                Now = Date.UtcNow
                If (SOFA.Dtf2d("", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, CDbl(Now.Second) + CDbl(Now.Millisecond) / 1000.0, utc1, utc2) <> 0) Then TL.LogMessage("Dtf2d", "Bad return code")
                Retval = utc1 + utc2
            Else
                Retval = JulianDateUTCValue
            End If
            TL.LogMessage("  GetJDUTCSofa", "  " & Retval.ToString & " " & Julian2DateTime(Retval).ToString(DATE_FORMAT))
            Return Retval
        End Function

        Private Function GetJDTTSofa() As Double
            Dim Retval, utc1, utc2, tai1, tai2, tt1, tt2 As Double, Now As DateTime

            If JulianDateTTValue = 0.0 Then
                Now = Date.UtcNow

                If (SOFA.Dtf2d("", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, CDbl(Now.Second) + CDbl(Now.Millisecond) / 1000.0, utc1, utc2) <> 0) Then TL.LogMessage("Dtf2d", "Bad return code")

                If (SOFA.UtcTai(utc1, utc2, tai1, tai2) <> 0) Then TL.LogMessage("GetJDTTSofa", "Utctai - Bad return code")
                If (SOFA.TaiTt(tai1, tai2, tt1, tt2) <> 0) Then TL.LogMessage("GetJDTTSofa", "Taitt - Bad return code")

                Retval = tt1 + tt2
            Else
                Retval = JulianDateTTValue
            End If
            TL.LogMessage("  GetJDTTSofa", "  " & Retval.ToString & " " & Julian2DateTime(Retval).ToString(DATE_FORMAT))
            Return Retval
        End Function

        Private Sub CheckGAC()
            Dim strPath As String
            TL.LogMessage("CheckGAC", "Started")
            strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
            TL.LogMessage("CheckGAC", "Assembly path: " & strPath)
        End Sub

        Private Function ValidateRA(Caller As String, RA As Double) As Double
            If (RA < 0.0) Or (RA >= 24.0) Then Throw New InvalidValueException(Caller, RA.ToString(), "0 to 23.9999")
            Return RA
        End Function

        Private Function ValidateDec(Caller As String, Dec As Double) As Double
            If (Dec < -90.0) Or (Dec > 90.0) Then Throw New InvalidValueException(Caller, Dec.ToString(), "-90.0 to 90.0")
            Return Dec
        End Function

        Private Function Julian2DateTime(m_JulianDate As Double) As DateTime
            Dim L, N, I, J, JDLong As Long
            Dim JDFraction, Remainder As Double
            Dim Day, Month, Year, Hours, Minutes, Seconds, MilliSeconds As Integer
            Dim Retval As DateTime

            Dim debug As Boolean = False ' Local flag to create some additional debug output

            Try
                If m_JulianDate > 2378507.5 Then ' 1/1/1800
                    JDLong = CLng(Math.Floor(m_JulianDate))
                    JDFraction = m_JulianDate - Math.Floor(m_JulianDate)
                    If debug Then TL.LogMessage("ConvertFromJulian", "Initial: " & JDLong & " " & JDFraction)

                    L = JDLong + 68569
                    N = CLng((4 * L) \ 146097)
                    L = L - CLng((146097 * N + 3) \ 4)
                    I = CLng((4000 * (L + 1) \ 1461001))
                    L = L - CLng((1461 * I) \ 4) + 31
                    J = CLng((80 * L) \ 2447)
                    Day = CInt(L - CLng((2447 * J) \ 80))
                    L = CLng(J \ 11)
                    Month = CInt(J + 2 - 12 * L)
                    Year = CInt(100 * (N - 49) + I + L)

                    If debug Then TL.LogMessage("ConvertFromJulian", "DMY: " & Day & " " & Month & " " & Year)

                    JDFraction += (5.0 / (24.0 * 60.0 * 60.0 * 10000.0))

                    ' Allow for Julian days to start at 12:00 rather than 00:00  
                    If JDFraction >= 0.5 Then ' After midnight so add 1 to the julian day and remove half a day from the day fraction
                        If debug Then TL.LogMessage("ConvertFromJulian", "JDFraction >= 0.5: " & JDFraction)
                        Day += 1
                        JDFraction -= 0.5
                        If debug Then TL.LogMessage("ConvertFromJulian", "DMY: " & Day & " " & JDFraction)
                    Else ' Before midnight so just add half a day 
                        JDFraction += 0.5
                    End If

                    Hours = CInt(Int(JDFraction * 24.0)) : Remainder = (JDFraction * 24.0) - CDbl(Hours) : If debug Then TL.LogMessage("ConvertFromJulian", "Hours: " & Hours & " " & Remainder) 'Remainder as a fraction of an hour
                    Minutes = CInt(Int(Remainder * 60.0)) : Remainder = (Remainder * 60.0) - CDbl(Minutes) : If debug Then TL.LogMessage("ConvertFromJulian", "Minutes: " & Minutes & " " & Remainder) 'Remainder as a fraction of a minute
                    Seconds = CInt(Int(Remainder * 60.0)) : Remainder = (Remainder * 60.0) - CDbl(Seconds) : If debug Then TL.LogMessage("ConvertFromJulian", "Seconds: " & Seconds & " " & Remainder) 'Remainder as a fraction of a second
                    MilliSeconds = CInt(Int(Remainder * 1000.0))

                    If debug Then TL.LogMessage("ConvertFromJulian", JDLong & " " & JDFraction & " " & Day & " " & Hours & " " & Minutes & " " & Seconds & " " & MilliSeconds)
                    Retval = New DateTime(Year, Month, Day, Hours, Minutes, Seconds, MilliSeconds)
                Else ' Early or invalid julian date so return a default value
                    Retval = New Date(1800, 1, 10) ' Return this as a default bad value
                End If
            Catch ex As Exception
                TL.LogMessageCrLf("", "Exception: " & ex.ToString)
                Retval = New Date(1900, 1, 10) ' Return this as a default bad value
            End Try

            Return (Retval)
        End Function

        Private Function FormatRA(RA As Double) As String
            Return Utl.HoursToHMS(RA, ":", ":", "", 3)
        End Function

        Private Function FormatDec(Dec As Double) As String
            Return Utl.DegreesToDMS(Dec, ":", ":", "", 3)
        End Function

#End Region

    End Class
End Namespace