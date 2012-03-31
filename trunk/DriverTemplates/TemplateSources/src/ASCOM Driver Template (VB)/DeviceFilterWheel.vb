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

#Region "IFilerWheel Implementation"
    Public ReadOnly Property FocusOffsets() As Integer() Implements IFilterWheelV2.FocusOffsets
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Names As String() Implements IFilterWheelV2.Names
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property Position() As Short Implements IFilterWheelV2.Position
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Short)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

#End Region

    '//ENDOFINSERTEDFILE
End Class