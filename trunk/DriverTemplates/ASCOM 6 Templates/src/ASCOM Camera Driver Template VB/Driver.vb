'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Camera driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM Camera interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Camera template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.$safeprojectname$.Camera
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Camera
' The ClassInterface/None addribute prevents an empty interface called
' _Camera from being created and used as the [default] interface
'
Imports ASCOM.Utilities
Imports ASCOM.DeviceInterface

<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
<ComVisible(True)> _
Public Class Camera
    '	==========
    Implements ICameraV2  ' Early-bind interface implemented by this driver
    '	==========
    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Private Shared s_csDriverID As String = "ASCOM.$safeprojectname$.Camera"
    ' TODO Change the descriptive string for your driver then remove this line
    Private Shared s_csDriverDescription As String = "$safeprojectname$ Camera"

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        ' TODO Implement your additional construction here
    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)
        Using Dim P As New Profile()
            P.DeviceType = "Camera"
            If bRegister Then
                P.Register(s_csDriverID, s_csDriverDescription)
            Else
                P.Unregister(s_csDriverID)
            End If
        End Using
    End Sub

    <ComRegisterFunction()> _
    Public Shared Sub RegisterASCOM(ByVal T As Type)
        RegUnregASCOM(True)
    End Sub

    <ComUnregisterFunction()> _
    Public Shared Sub UnregisterASCOM(ByVal T As Type)
        RegUnregASCOM(False)
    End Sub

#End Region

    '
    ' PUBLIC COM INTERFACE ICamera IMPLEMENTATION
    '
#Region "ICamera Members"

    Public Sub SetupDialog() Implements ICameraV2.SetupDialog
        Using f As New SetupDialogForm
            f.ShowDialog()
        End Using
    End Sub

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ICameraV2.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ICameraV2.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements ICameraV2.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements ICameraV2.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Sub Dispose() Implements ICameraV2.Dispose
        Throw New System.NotImplementedException()
    End Sub

    Public Sub AbortExposure() Implements ICameraV2.AbortExposure
        Throw New System.NotImplementedException()
    End Sub

    Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer) Implements ICameraV2.PulseGuide
        Throw New System.NotImplementedException()
    End Sub

    Public Sub StartExposure(ByVal Duration As Double, ByVal Light As Boolean) Implements ICameraV2.StartExposure
        Throw New System.NotImplementedException()
    End Sub

    Public Sub StopExposure() Implements ICameraV2.StopExposure
        Throw New System.NotImplementedException()
    End Sub

    Public Property Connected() As Boolean Implements ICameraV2.Connected
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ICameraV2.Description
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements ICameraV2.DriverInfo
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ICameraV2.DriverVersion
        Get
            Dim Ass As Reflection.Assembly

            Ass = Reflection.Assembly.GetExecutingAssembly 'Get our own assembly and report its version number
            Return Ass.GetName.Version.Major.ToString & "." & Ass.GetName.Version.Minor.ToString
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ICameraV2.InterfaceVersion
        Get
            Return 2
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements ICameraV2.Name
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SupportedActions() As ArrayList Implements ICameraV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Property BinX() As Short Implements ICameraV2.BinX
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Short)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property BinY() As Short Implements ICameraV2.BinY
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Short)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property CameraState() As CameraStates Implements ICameraV2.CameraState
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CameraXSize() As Integer Implements ICameraV2.CameraXSize
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CameraYSize() As Integer Implements ICameraV2.CameraYSize
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanAbortExposure() As Boolean Implements ICameraV2.CanAbortExposure
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanAsymmetricBin() As Boolean Implements ICameraV2.CanAsymmetricBin
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanGetCoolerPower() As Boolean Implements ICameraV2.CanGetCoolerPower
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanPulseGuide() As Boolean Implements ICameraV2.CanPulseGuide
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetCCDTemperature() As Boolean Implements ICameraV2.CanSetCCDTemperature
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanStopExposure() As Boolean Implements ICameraV2.CanStopExposure
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CCDTemperature() As Double Implements ICameraV2.CCDTemperature
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property CoolerOn() As Boolean Implements ICameraV2.CoolerOn
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property CoolerPower() As Double Implements ICameraV2.CoolerPower
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ElectronsPerADU() As Double Implements ICameraV2.ElectronsPerADU
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property FullWellCapacity() As Double Implements ICameraV2.FullWellCapacity
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property HasShutter() As Boolean Implements ICameraV2.HasShutter
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property HeatSinkTemperature() As Double Implements ICameraV2.HeatSinkTemperature
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ImageArray() As Object Implements ICameraV2.ImageArray
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ImageArrayVariant() As Object Implements ICameraV2.ImageArrayVariant
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ImageReady() As Boolean Implements ICameraV2.ImageReady
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property IsPulseGuiding() As Boolean Implements ICameraV2.IsPulseGuiding
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property LastExposureDuration() As Double Implements ICameraV2.LastExposureDuration
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property LastExposureStartTime() As String Implements ICameraV2.LastExposureStartTime
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property MaxADU() As Integer Implements ICameraV2.MaxADU
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property MaxBinX() As Short Implements ICameraV2.MaxBinX
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property MaxBinY() As Short Implements ICameraV2.MaxBinY
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property NumX() As Integer Implements ICameraV2.NumX
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Integer)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property NumY() As Integer Implements ICameraV2.NumY
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Integer)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property PixelSizeX() As Double Implements ICameraV2.PixelSizeX
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property PixelSizeY() As Double Implements ICameraV2.PixelSizeY
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property SetCCDTemperature() As Double Implements ICameraV2.SetCCDTemperature
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property StartX() As Integer Implements ICameraV2.StartX
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Integer)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property StartY() As Integer Implements ICameraV2.StartY
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Integer)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property BayerOffsetX() As Short Implements ICameraV2.BayerOffsetX
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property BayerOffsetY() As Short Implements ICameraV2.BayerOffsetY
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanFastReadout() As Boolean Implements ICameraV2.CanFastReadout
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ExposureMax() As Double Implements ICameraV2.ExposureMax
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ExposureMin() As Double Implements ICameraV2.ExposureMin
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ExposureResolution() As Double Implements ICameraV2.ExposureResolution
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property FastReadout() As Boolean Implements ICameraV2.FastReadout
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property Gain() As Short Implements ICameraV2.Gain
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Short)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property GainMax() As Short Implements ICameraV2.GainMax
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property GainMin() As Short Implements ICameraV2.GainMin
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Gains() As ArrayList Implements ICameraV2.Gains
        Get
            Return New ArrayList()
        End Get
    End Property

    Public ReadOnly Property PercentCompleted() As Short Implements ICameraV2.PercentCompleted
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ReadoutMode() As Short Implements ICameraV2.ReadoutMode
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ReadoutModes() As ArrayList Implements ICameraV2.ReadoutModes
        Get
            Return New ArrayList()
        End Get
    End Property

    Public ReadOnly Property SensorName() As String Implements ICameraV2.SensorName
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SensorType() As SensorType Implements ICameraV2.SensorType
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

#End Region

End Class
