' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceRotator
    Implements IRotatorV2
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "IRotator Implementation"
    Public ReadOnly Property CanReverse() As Boolean Implements IRotatorV2.CanReverse
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Sub Halt() Implements IRotatorV2.Halt
        Throw New ASCOM.PropertyNotImplementedException()
    End Sub

    Public ReadOnly Property IsMoving() As Boolean Implements IRotatorV2.IsMoving
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Sub Move(Position As Single) Implements IRotatorV2.Move
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public Sub MoveAbsolute(Position As Single) Implements IRotatorV2.MoveAbsolute
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public ReadOnly Property Position() As Single Implements IRotatorV2.Position
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property Reverse() As Boolean Implements IRotatorV2.Reverse
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public ReadOnly Property StepSize() As Single Implements IRotatorV2.StepSize
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property TargetPosition() As Single Implements IRotatorV2.TargetPosition
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

#End Region

    '//ENDOFINSERTEDFILE
End Class