' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceFilerWheel
    Implements IFilterWheelV2
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "IFilterWheel Implementation"
    Private fwOffsets As Integer() = New Integer(3) {0, 0, 0, 0} 'class level variable to hold focus offsets
    Private fwNames As String() = New String(3) {"Red", "Green", "Blue", "Clear"} 'class level variable to hold the filter names
    Private fwPosition As Short = 0 'class level variable to retain the current filterwheel position

    Public ReadOnly Property FocusOffsets() As Integer() Implements IFilterWheelV2.FocusOffsets
        Get
            For Each fwOffset As Integer In fwOffsets ' Write filter offsets to the log
                TL.LogMessage("FocusOffsets Get", fwOffset.ToString())
            Next

            Return fwOffsets
        End Get
    End Property

    Public ReadOnly Property Names As String() Implements IFilterWheelV2.Names
        Get
            For Each fwName As String In fwNames ' Write filter names to the log
                TL.LogMessage("Names Get", fwName)
            Next

            Return fwNames
        End Get
    End Property

    Public Property Position() As Short Implements IFilterWheelV2.Position
        Get
            TL.LogMessage("Position Get", fwPosition.ToString())
            Return fwPosition
        End Get
        Set(value As Short)
            TL.LogMessage("Position Set", value.ToString())
            If ((value < 0) Or (value > fwNames.Length - 1)) Then
                TL.LogMessage("Position Set", "Throwing InvalidValueException")
                Throw New InvalidValueException("Position", value.ToString(), "0 to " & (fwNames.Length - 1).ToString())
            End If
            fwPosition = value
        End Set
    End Property

#End Region

    '//ENDOFINSERTEDFILE
End Class