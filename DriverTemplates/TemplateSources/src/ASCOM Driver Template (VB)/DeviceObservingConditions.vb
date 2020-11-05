' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceObservingConditions
    Implements IObservingConditions

    Private TL As New TraceLogger()

#Region "IObservingConditions Implementation"

    Public Property AveragePeriod() As Double Implements IObservingConditions.AveragePeriod
        Get
            TL.LogMessage("AveragePeriod", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("AveragePeriod", False)
        End Get
        Set(value As Double)
            TL.LogMessage("AveragePeriod", "Set Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("AveragePeriod", True)
        End Set
    End Property

    Public ReadOnly Property CloudCover() As Double Implements IObservingConditions.CloudCover
        Get
            TL.LogMessage("CloudCover", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("CloudCover", False)
        End Get
    End Property

    Public ReadOnly Property DewPoint() As Double Implements IObservingConditions.DewPoint
        Get
            TL.LogMessage("DewPoint", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("DewPoint", False)
        End Get
    End Property

    Public ReadOnly Property Humidity() As Double Implements IObservingConditions.Humidity
        Get
            TL.LogMessage("Humidity", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Humidity", False)
        End Get
    End Property

    Public ReadOnly Property Pressure() As Double Implements IObservingConditions.Pressure
        Get
            TL.LogMessage("Pressure", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Pressure", False)
        End Get
    End Property

    Public ReadOnly Property RainRate() As Double Implements IObservingConditions.RainRate
        Get
            TL.LogMessage("RainRate", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("RainRate", False)
        End Get
    End Property

    Public ReadOnly Property SkyBrightness() As Double Implements IObservingConditions.SkyBrightness
        Get
            TL.LogMessage("SkyBrightness", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SkyBrightness", False)
        End Get
    End Property

    Public ReadOnly Property SkyQuality() As Double Implements IObservingConditions.SkyQuality
        Get
            TL.LogMessage("SkyQuality", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SkyQuality", False)
        End Get
    End Property

    Public ReadOnly Property StarFWHM() As Double Implements IObservingConditions.StarFWHM
        Get
            TL.LogMessage("StarFWHM", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("StarFWHM", False)
        End Get
    End Property

    Public ReadOnly Property SkyTemperature() As Double Implements IObservingConditions.SkyTemperature
        Get
            TL.LogMessage("SkyTemperature", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SkyTemperature", False)
        End Get
    End Property

    Public ReadOnly Property Temperature() As Double Implements IObservingConditions.Temperature
        Get
            TL.LogMessage("Temperature", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Temperature", False)
        End Get
    End Property

    Public ReadOnly Property WindDirection() As Double Implements IObservingConditions.WindDirection
        Get
            TL.LogMessage("WindDirection", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("WindDirection", False)
        End Get
    End Property

    Public ReadOnly Property WindGust() As Double Implements IObservingConditions.WindGust
        Get
            TL.LogMessage("WindGust", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("WindGust", False)
        End Get
    End Property

    Public ReadOnly Property WindSpeed() As Double Implements IObservingConditions.WindSpeed
        Get
            TL.LogMessage("WindSpeed", "Get Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("WindSpeed", False)
        End Get
    End Property

    Public Function TimeSinceLastUpdate(PropertyName As String) As Double Implements IObservingConditions.TimeSinceLastUpdate
        ' Test for an empty property name, if found, return the time since the most recent update to any sensor
        If Not String.IsNullOrEmpty(PropertyName) Then
            Select Case PropertyName.Trim.ToLowerInvariant
                ' Return the time for properties that are implemented, otherwise fall through to the MethodNotImplementedException
                Case "averageperiod"
                Case "cloudcover"
                Case "dewpoint"
                Case "humidity"
                Case "pressure"
                Case "rainrate"
                Case "skybrightness"
                Case "skyquality"
                Case "skytemperature"
                Case "starfwhm"
                Case "temperature"
                Case "winddirection"
                Case "windgust"
                Case "windspeed"
                    ' Throw an exception on the properties that are Not implemented
                    TL.LogMessage("TimeSinceLastUpdate", PropertyName & " - not implemented")
                    Throw New MethodNotImplementedException("TimeSinceLastUpdate(" + PropertyName + ")")
                Case Else
                    TL.LogMessage("TimeSinceLastUpdate", PropertyName & " - unrecognised")
                    Throw New ASCOM.InvalidValueException("TimeSinceLastUpdate(" + PropertyName + ")")
            End Select
        Else
            ' Return the time since the most recent update to any sensor
            TL.LogMessage("TimeSinceLastUpdate", $"The time since the most recent sensor update is not implemented")
            Throw New MethodNotImplementedException("TimeSinceLastUpdate(" + PropertyName + ")")
        End If
    End Function

    Public Function SensorDescription(PropertyName As String) As String Implements IObservingConditions.SensorDescription
        Select Case PropertyName.Trim.ToLowerInvariant
            Case "averageperiod"
                Return "Average period in hours, immediate values are only available"
            Case "cloudcover"
            Case "dewpoint"
            Case "humidity"
            Case "pressure"
            Case "rainrate"
            Case "skybrightness"
            Case "skyquality"
            Case "skytemperature"
            Case "starfwhm"
            Case "temperature"
            Case "winddirection"
            Case "windgust"
            Case "windspeed"
                ' Throw an exception on the properties that are Not implemented
                TL.LogMessage("SensorDescription", $"Property {PropertyName} is not implemented")
                Throw New MethodNotImplementedException($"SensorDescription - Property {PropertyName} is not implemented")
        End Select

        TL.LogMessage("SensorDescription", $"Invalid sensor name: {PropertyName}")
        Throw New InvalidValueException($"SensorDescription - Invalid property name: {PropertyName}")
    End Function

    Public Sub Refresh() Implements IObservingConditions.Refresh
        TL.LogMessage("Refresh", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("Refresh")
    End Sub

#End Region

    '//ENDOFINSERTEDFILE
End Class