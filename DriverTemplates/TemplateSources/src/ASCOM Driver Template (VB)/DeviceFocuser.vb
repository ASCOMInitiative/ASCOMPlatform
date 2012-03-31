' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceFocuser
    Implements IFocuserV2
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "IFocuser Implementation"
    Public ReadOnly Property Absolute() As Boolean Implements IFocuserV2.Absolute
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Sub Halt() Implements IFocuserV2.Halt
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public ReadOnly Property IsMoving() As Boolean Implements IFocuserV2.IsMoving
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property Link() As Boolean Implements IFocuserV2.Link
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property MaxIncrement() As Integer Implements IFocuserV2.MaxIncrement
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property MaxStep() As Integer Implements IFocuserV2.MaxStep
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Sub Move(Position As Integer) Implements IFocuserV2.Move
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public ReadOnly Property Position() As Integer Implements IFocuserV2.Position
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property StepSize() As Double Implements IFocuserV2.StepSize
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property TempComp() As Boolean Implements IFocuserV2.TempComp
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property TempCompAvailable() As Boolean Implements IFocuserV2.TempCompAvailable
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Temperature() As Double Implements IFocuserV2.Temperature
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

#End Region

    '//ENDOFINSERTEDFILE
End Class