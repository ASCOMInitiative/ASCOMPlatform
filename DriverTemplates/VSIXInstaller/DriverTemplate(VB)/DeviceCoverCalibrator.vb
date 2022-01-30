' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

Class DeviceCoverCalibrator
    Implements ICoverCalibratorV1
    Private util = New Util()
    Private tl = New TraceLogger()

#Region "ICoverCalibrator Implementation"

    ''' <summary>
    ''' Returns the state of the device cover, if present, otherwise returns "NotPresent"
    ''' </summary>
    Public ReadOnly Property CoverState() As CoverStatus Implements ICoverCalibratorV1.CoverState
        Get
            tl.LogMessage("CoverState Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("CoverState", False)
        End Get
    End Property

    ''' <summary>
    ''' Initiates cover opening if a cover is present
    ''' </summary>
    Public Sub OpenCover() Implements ICoverCalibratorV1.OpenCover
        tl.LogMessage("OpenCover", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("OpenCover")
    End Sub

    ''' <summary>
    ''' Initiates cover closing if a cover is present
    ''' </summary>
    Public Sub CloseCover() Implements ICoverCalibratorV1.CloseCover
        tl.LogMessage("CloseCover", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("CloseCover")
    End Sub

    ''' <summary>
    ''' Stops any cover movement that may be in progress if a cover is present and cover movement can be interrupted.
    ''' </summary>
    Public Sub HaltCover() Implements ICoverCalibratorV1.HaltCover
        tl.LogMessage("HaltCover", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("HaltCover")
    End Sub

    ''' <summary>
    ''' Returns the state of the calibration device, if present, otherwise returns "NotPresent"
    ''' </summary>
    Public ReadOnly Property CalibratorState() As CalibratorStatus Implements ICoverCalibratorV1.CalibratorState
        Get
            tl.LogMessage("CalibratorState Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("CalibratorState", False)
        End Get
    End Property

    ''' <summary>
    ''' Returns the current calibrator brightness in the range 0 (completely off) to <see cref="MaxBrightness"/> (fully on)
    ''' </summary>
    Public ReadOnly Property Brightness As Integer Implements ICoverCalibratorV1.Brightness
        Get
            tl.LogMessage("Brightness Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Brightness", False)
        End Get
    End Property

    ''' <summary>
    ''' The Brightness value that makes the calibrator deliver its maximum illumination.
    ''' </summary>
    Public ReadOnly Property MaxBrightness As Integer Implements ICoverCalibratorV1.MaxBrightness
        Get
            tl.LogMessage("MaxBrightness Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("MaxBrightness", False)
        End Get
    End Property

    ''' <summary>
    ''' Turns the calibrator on at the specified brightness if the device has calibration capability
    ''' </summary>
    ''' <param name="Brightness"></param>
    Public Sub CalibratorOn(Brightness As Integer) Implements ICoverCalibratorV1.CalibratorOn
        tl.LogMessage("CalibratorOn", $"Not implemented. Value set: {Brightness}")
        Throw New ASCOM.MethodNotImplementedException("CalibratorOn")
    End Sub

    ''' <summary>
    ''' Turns the calibrator off if the device has calibration capability
    ''' </summary>
    Public Sub CalibratorOff() Implements ICoverCalibratorV1.CalibratorOff
        tl.LogMessage("CalibratorOff", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("CalibratorOff")
    End Sub

#End Region

    '//ENDOFINSERTEDFILE
End Class