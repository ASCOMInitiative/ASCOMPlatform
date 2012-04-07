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

    Private Const ccdWidth As Integer = 1394 ' Constants to define the ccd pixel dimenstions
    Private Const ccdHeight As Integer = 1040
    Private Const pixelSize As Double = 6.45 ' Constant for the pixel physical dimension

    Private cameraNumX As Integer = ccdWidth ' Initialise variables to hold values required for functionality tested by Conform
    Private cameraNumY As Integer = ccdHeight
    Private cameraStartX As Integer = 0
    Private cameraStartY As Integer = 0
    Private exposureStart As DateTime = DateTime.MinValue
    Private cameraLastExposureDuration As Double = 0.0
    Private cameraImageReady As Boolean = False
    Private cameraImageArray As Integer(,)
    Private cameraImageArrayVariant As Object(,)

    Public Sub AbortExposure() Implements ICameraV2.AbortExposure
        TL.LogMessage("AbortExposure", "Not implemented")
        Throw New MethodNotImplementedException("AbortExposure")
    End Sub

    Public ReadOnly Property BayerOffsetX() As Short Implements ICameraV2.BayerOffsetX
        Get
            TL.LogMessage("BayerOffsetX Get", "Not implemented")
            Throw New PropertyNotImplementedException("BayerOffsetX", False)
        End Get
    End Property

    Public ReadOnly Property BayerOffsetY() As Short Implements ICameraV2.BayerOffsetY
        Get
            TL.LogMessage("BayerOffsetY Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("BayerOffsetY", False)
        End Get
    End Property

    Public Property BinX() As Short Implements ICameraV2.BinX
        Get
            TL.LogMessage("BinX Get", "1")
            Return 1
        End Get
        Set(value As Short)
            TL.LogMessage("BinX Set", value.ToString())
            If (Not (value = 1)) Then
                TL.LogMessage("BinX Set", "Value out of range, throwing InvalidValueException")
                Throw New ASCOM.InvalidValueException("BinX", value.ToString(), "1") ' Only 1 is valid in this simple template
            End If
        End Set
    End Property

    Public Property BinY() As Short Implements ICameraV2.BinY
        Get
            TL.LogMessage("BinY Get", "1")
            Return 1
        End Get
        Set(value As Short)
            TL.LogMessage("BinY Set", value.ToString())
            If (Not (value = 1)) Then
                TL.LogMessage("BinX Set", "Value out of range, throwing InvalidValueException")
                Throw New ASCOM.InvalidValueException("BinY", value.ToString(), "1") ' Only 1 is valid in this simple template
            End If
        End Set
    End Property

    Public ReadOnly Property CCDTemperature() As Double Implements ICameraV2.CCDTemperature
        Get
            TL.LogMessage("CCDTemperature Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("CCDTemperature", False)
        End Get
    End Property

    Public ReadOnly Property CameraState() As CameraStates Implements ICameraV2.CameraState
        Get
            TL.LogMessage("CameraState Get", CameraStates.cameraIdle.ToString())
            Return CameraStates.cameraIdle
        End Get
    End Property

    Public ReadOnly Property CameraXSize() As Integer Implements ICameraV2.CameraXSize
        Get
            TL.LogMessage("CameraXSize Get", ccdWidth.ToString())
            Return ccdWidth
        End Get
    End Property

    Public ReadOnly Property CameraYSize() As Integer Implements ICameraV2.CameraYSize
        Get
            TL.LogMessage("CameraYSize Get", ccdHeight.ToString())
            Return ccdHeight
        End Get
    End Property

    Public ReadOnly Property CanAbortExposure() As Boolean Implements ICameraV2.CanAbortExposure
        Get
            TL.LogMessage("CanAbortExposure Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanAsymmetricBin() As Boolean Implements ICameraV2.CanAsymmetricBin
        Get
            TL.LogMessage("CanAsymmetricBin Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanFastReadout() As Boolean Implements ICameraV2.CanFastReadout
        Get
            TL.LogMessage("CanFastReadout Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanGetCoolerPower() As Boolean Implements ICameraV2.CanGetCoolerPower
        Get
            TL.LogMessage("CanGetCoolerPower Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanPulseGuide() As Boolean Implements ICameraV2.CanPulseGuide
        Get
            TL.LogMessage("CanPulseGuide Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetCCDTemperature() As Boolean Implements ICameraV2.CanSetCCDTemperature
        Get
            TL.LogMessage("CanSetCCDTemperature Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanStopExposure() As Boolean Implements ICameraV2.CanStopExposure
        Get
            TL.LogMessage("CanStopExposure Get", False.ToString())
            Return False
        End Get
    End Property

    Public Property CoolerOn() As Boolean Implements ICameraV2.CoolerOn
        Get
            TL.LogMessage("CoolerOn Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("CoolerOn", False)
        End Get
        Set(value As Boolean)
            TL.LogMessage("CoolerOn Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("CoolerOn", True)
        End Set
    End Property

    Public ReadOnly Property CoolerPower() As Double Implements ICameraV2.CoolerPower
        Get
            TL.LogMessage("AbortExposure Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("CoolerPower", False)
        End Get
    End Property

    Public ReadOnly Property ElectronsPerADU() As Double Implements ICameraV2.ElectronsPerADU
        Get
            TL.LogMessage("ElectronsPerADU Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ElectronsPerADU", False)
        End Get
    End Property

    Public ReadOnly Property ExposureMax() As Double Implements ICameraV2.ExposureMax
        Get
            TL.LogMessage("ExposureMax Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ExposureMax", False)
        End Get
    End Property

    Public ReadOnly Property ExposureMin() As Double Implements ICameraV2.ExposureMin
        Get
            TL.LogMessage("ExposureMin Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ExposureMin", False)
        End Get
    End Property

    Public ReadOnly Property ExposureResolution() As Double Implements ICameraV2.ExposureResolution
        Get
            TL.LogMessage("ExposureResolution Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ExposureResolution", False)
        End Get
    End Property

    Public Property FastReadout() As Boolean Implements ICameraV2.FastReadout
        Get
            TL.LogMessage("FastReadout Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("FastReadout", False)
        End Get
        Set(value As Boolean)
            TL.LogMessage("FastReadout Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("FastReadout", True)
        End Set
    End Property

    Public ReadOnly Property FullWellCapacity() As Double Implements ICameraV2.FullWellCapacity
        Get
            TL.LogMessage("FullWellCapacity Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("FullWellCapacity", False)
        End Get
    End Property

    Public Property Gain() As Short Implements ICameraV2.Gain
        Get
            TL.LogMessage("Gain Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Gain", False)
        End Get
        Set(value As Short)
            TL.LogMessage("Gain Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Gain", True)
        End Set
    End Property

    Public ReadOnly Property GainMax() As Short Implements ICameraV2.GainMax
        Get
            TL.LogMessage("GainMax Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("GainMax", False)
        End Get
    End Property

    Public ReadOnly Property GainMin() As Short Implements ICameraV2.GainMin
        Get
            TL.LogMessage("GainMin Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("GainMin", False)
        End Get
    End Property

    Public ReadOnly Property Gains() As ArrayList Implements ICameraV2.Gains
        Get
            TL.LogMessage("Gains Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Gains", False)
        End Get
    End Property

    Public ReadOnly Property HasShutter() As Boolean Implements ICameraV2.HasShutter
        Get
            TL.LogMessage("HasShutter Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property HeatSinkTemperature() As Double Implements ICameraV2.HeatSinkTemperature
        Get
            TL.LogMessage("HeatSinkTemperature Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("HeatSinkTemperature", False)
        End Get
    End Property

    Public ReadOnly Property ImageArray() As Object Implements ICameraV2.ImageArray
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!")
            End If

            ReDim cameraImageArray(cameraNumX - 1, cameraNumY - 1)
            Return cameraImageArray
        End Get
    End Property

    Public ReadOnly Property ImageArrayVariant() As Object Implements ICameraV2.ImageArrayVariant
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!")
            End If

            ReDim cameraImageArrayVariant(cameraNumX - 1, cameraNumY - 1)
            For i As Integer = 0 To cameraImageArray.GetLength(1) - 1
                For j As Integer = 0 To cameraImageArray.GetLength(0) - 1
                    cameraImageArrayVariant(j, i) = cameraImageArray(j, i)
                Next
            Next

            Return cameraImageArrayVariant
        End Get
    End Property

    Public ReadOnly Property ImageReady() As Boolean Implements ICameraV2.ImageReady
        Get
            TL.LogMessage("ImageReady Get", cameraImageReady.ToString())
            Return cameraImageReady
        End Get
    End Property

    Public ReadOnly Property IsPulseGuiding() As Boolean Implements ICameraV2.IsPulseGuiding
        Get
            TL.LogMessage("IsPulseGuiding Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("IsPulseGuiding", False)
        End Get
    End Property

    Public ReadOnly Property LastExposureDuration() As Double Implements ICameraV2.LastExposureDuration
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("LastExposureDuration Get", "Throwing InvalidOperationException because of a call to LastExposureDuration before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to LastExposureDuration before the first image has been taken!")
            End If
            TL.LogMessage("LastExposureDuration Get", cameraLastExposureDuration.ToString())
            Return cameraLastExposureDuration
        End Get
    End Property

    Public ReadOnly Property LastExposureStartTime() As String Implements ICameraV2.LastExposureStartTime
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("LastExposureStartTime Get", "Throwing InvalidOperationException because of a call to LastExposureStartTime before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to LastExposureStartTime before the first image has been taken!")
            End If
            Dim exposureStartString As String = exposureStart.ToString("yyyy-MM-ddTHH:mm:ss")
            TL.LogMessage("LastExposureStartTime Get", exposureStartString.ToString())
            Return exposureStartString
        End Get
    End Property

    Public ReadOnly Property MaxADU() As Integer Implements ICameraV2.MaxADU
        Get
            TL.LogMessage("MaxADU Get", "20000")
            Return 20000
        End Get
    End Property

    Public ReadOnly Property MaxBinX() As Short Implements ICameraV2.MaxBinX
        Get
            TL.LogMessage("MaxBinX Get", "1")
            Return 1
        End Get
    End Property

    Public ReadOnly Property MaxBinY() As Short Implements ICameraV2.MaxBinY
        Get
            TL.LogMessage("MaxBinY Get", "1")
            Return 1
        End Get
    End Property

    Public Property NumX() As Integer Implements ICameraV2.NumX
        Get
            TL.LogMessage("NumX Get", cameraNumX.ToString())
            Return cameraNumX
        End Get
        Set(value As Integer)
            cameraNumX = value
            TL.LogMessage("NumX set", value.ToString())
        End Set
    End Property

    Public Property NumY() As Integer Implements ICameraV2.NumY
        Get
            TL.LogMessage("NumY Get", cameraNumY.ToString())
            Return cameraNumY
        End Get
        Set(value As Integer)
            cameraNumY = value
            TL.LogMessage("NumY set", value.ToString())
        End Set
    End Property

    Public ReadOnly Property PercentCompleted() As Short Implements ICameraV2.PercentCompleted
        Get
            TL.LogMessage("PercentCompleted Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("PercentCompleted", False)
        End Get
    End Property

    Public ReadOnly Property PixelSizeX() As Double Implements ICameraV2.PixelSizeX
        Get
            TL.LogMessage("PixelSizeX Get", pixelSize.ToString())
            Return pixelSize
        End Get
    End Property

    Public ReadOnly Property PixelSizeY() As Double Implements ICameraV2.PixelSizeY
        Get
            TL.LogMessage("PixelSizeY Get", pixelSize.ToString())
            Return pixelSize
        End Get
    End Property

    Public Sub PulseGuide(Direction As GuideDirections, Duration As Integer) Implements ICameraV2.PulseGuide
        TL.LogMessage("PulseGuide", "Not implemented - " & Direction.ToString)
        Throw New ASCOM.MethodNotImplementedException("Direction")
    End Sub

    Public Property ReadoutMode() As Short Implements ICameraV2.ReadoutMode
        Get
            TL.LogMessage("ReadoutMode Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ReadoutMode", False)
        End Get
        Set(value As Short)
            TL.LogMessage("ReadoutMode Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ReadoutMode", True)
        End Set
    End Property

    Public ReadOnly Property ReadoutModes() As ArrayList Implements ICameraV2.ReadoutModes
        Get
            TL.LogMessage("ReadoutModes Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ReadoutModes", False)
        End Get
    End Property

    Public ReadOnly Property SensorName() As String Implements ICameraV2.SensorName
        Get
            TL.LogMessage("SensorName Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SensorName", False)
        End Get
    End Property

    Public ReadOnly Property SensorType() As SensorType Implements ICameraV2.SensorType
        Get
            TL.LogMessage("SensorType Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SensorType", False)
        End Get
    End Property

    Public Property SetCCDTemperature() As Double Implements ICameraV2.SetCCDTemperature
        Get
            TL.LogMessage("SetCCDTemperature Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SetCCDTemperature", False)
        End Get
        Set(value As Double)
            TL.LogMessage("SetCCDTemperature Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SetCCDTemperature", True)
        End Set
    End Property

    Public Sub StartExposure(Duration As Double, Light As Boolean) Implements ICameraV2.StartExposure
        If (Duration < 0.0) Then Throw New InvalidValueException("StartExposure", Duration.ToString(), "0.0 upwards")
        If (cameraNumX > ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraNumX.ToString(), ccdWidth.ToString())
        If (cameraNumY > ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraNumY.ToString(), ccdHeight.ToString())
        If (cameraStartX > ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraStartX.ToString(), ccdWidth.ToString())
        If (cameraStartY > ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraStartY.ToString(), ccdHeight.ToString())

        cameraLastExposureDuration = Duration
        exposureStart = DateTime.Now
        System.Threading.Thread.Sleep(Duration * 1000) ' Sleep for the duration to simulate exposure 
        TL.LogMessage("StartExposure", Duration.ToString() + " " + Light.ToString())
        cameraImageReady = True
    End Sub

    Public Property StartX() As Integer Implements ICameraV2.StartX
        Get
            TL.LogMessage("StartX Get", cameraStartX.ToString())
            Return cameraStartX
        End Get
        Set(value As Integer)
            cameraStartX = value
            TL.LogMessage("StartX set", value.ToString())
        End Set
    End Property

    Public Property StartY() As Integer Implements ICameraV2.StartY
        Get
            TL.LogMessage("StartY Get", cameraStartY.ToString())
            Return cameraStartY
        End Get
        Set(value As Integer)
            cameraStartY = value
            TL.LogMessage("StartY set", value.ToString())
        End Set
    End Property

    Public Sub StopExposure() Implements ICameraV2.StopExposure
        TL.LogMessage("StopExposure", "Not implemented")
        Throw New MethodNotImplementedException("StopExposure")
    End Sub

#End Region

    '//ENDOFINSERTEDFILE
End Class