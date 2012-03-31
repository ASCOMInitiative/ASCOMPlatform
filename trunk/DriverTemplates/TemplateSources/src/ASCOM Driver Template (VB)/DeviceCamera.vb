' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports System.Collections
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceCamera
    Implements ICameraV2
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "ICamera Implementation"
    Public Sub AbortExposure() Implements ICameraV2.AbortExposure
        Throw New MethodNotImplementedException()
    End Sub

    Public ReadOnly Property BayerOffsetX() As Short Implements ICameraV2.BayerOffsetX
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property BayerOffsetY() As Short Implements ICameraV2.BayerOffsetY
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property BinX() As Short Implements ICameraV2.BinX
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Short)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public Property BinY() As Short Implements ICameraV2.BinY
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Short)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property CCDTemperature() As Double Implements ICameraV2.CCDTemperature
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CameraState() As CameraStates Implements ICameraV2.CameraState
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CameraXSize() As Integer Implements ICameraV2.CameraXSize
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CameraYSize() As Integer Implements ICameraV2.CameraYSize
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanAbortExposure() As Boolean Implements ICameraV2.CanAbortExposure
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanAsymmetricBin() As Boolean Implements ICameraV2.CanAsymmetricBin
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanFastReadout() As Boolean Implements ICameraV2.CanFastReadout
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanGetCoolerPower() As Boolean Implements ICameraV2.CanGetCoolerPower
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanPulseGuide() As Boolean Implements ICameraV2.CanPulseGuide
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetCCDTemperature() As Boolean Implements ICameraV2.CanSetCCDTemperature
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanStopExposure() As Boolean Implements ICameraV2.CanStopExposure
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property CoolerOn() As Boolean Implements ICameraV2.CoolerOn
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property CoolerPower() As Double Implements ICameraV2.CoolerPower
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ElectronsPerADU() As Double Implements ICameraV2.ElectronsPerADU
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ExposureMax() As Double Implements ICameraV2.ExposureMax
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ExposureMin() As Double Implements ICameraV2.ExposureMin
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ExposureResolution() As Double Implements ICameraV2.ExposureResolution
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property FastReadout() As Boolean Implements ICameraV2.FastReadout
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property FullWellCapacity() As Double Implements ICameraV2.FullWellCapacity
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property Gain() As Short Implements ICameraV2.Gain
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Short)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property GainMax() As Short Implements ICameraV2.GainMax
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property GainMin() As Short Implements ICameraV2.GainMin
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Gains() As ArrayList Implements ICameraV2.Gains
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property HasShutter() As Boolean Implements ICameraV2.HasShutter
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property HeatSinkTemperature() As Double Implements ICameraV2.HeatSinkTemperature
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ImageArray() As Object Implements ICameraV2.ImageArray
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ImageArrayVariant() As Object Implements ICameraV2.ImageArrayVariant
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ImageReady() As Boolean Implements ICameraV2.ImageReady
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property IsPulseGuiding() As Boolean Implements ICameraV2.IsPulseGuiding
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property LastExposureDuration() As Double Implements ICameraV2.LastExposureDuration
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property LastExposureStartTime() As String Implements ICameraV2.LastExposureStartTime
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property MaxADU() As Integer Implements ICameraV2.MaxADU
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property MaxBinX() As Short Implements ICameraV2.MaxBinX
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property MaxBinY() As Short Implements ICameraV2.MaxBinY
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property NumX() As Integer Implements ICameraV2.NumX
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Integer)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public Property NumY() As Integer Implements ICameraV2.NumY
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Integer)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property PercentCompleted() As Short Implements ICameraV2.PercentCompleted
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property PixelSizeX() As Double Implements ICameraV2.PixelSizeX
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property PixelSizeY() As Double Implements ICameraV2.PixelSizeY
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Sub PulseGuide(Direction As GuideDirections, Duration As Integer) Implements ICameraV2.PulseGuide
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public Property ReadoutMode() As Short Implements ICameraV2.ReadoutMode
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Short)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property ReadoutModes() As ArrayList Implements ICameraV2.ReadoutModes
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SensorName() As String Implements ICameraV2.SensorName
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SensorType() As SensorType Implements ICameraV2.SensorType
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property SetCCDTemperature() As Double Implements ICameraV2.SetCCDTemperature
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Double)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public Sub StartExposure(Duration As Double, Light As Boolean) Implements ICameraV2.StartExposure
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public Property StartX() As Integer Implements ICameraV2.StartX
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Integer)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public Property StartY() As Integer Implements ICameraV2.StartY
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Integer)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public Sub StopExposure() Implements ICameraV2.StopExposure
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

#End Region

    '//ENDOFINSERTEDFILE
End Class