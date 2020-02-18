' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities
Imports ASCOM.Astrometry.AstroUtils

Class DeviceRotator
    Implements IRotatorV2
    Private m_util As New Util()
    Private TL As New TraceLogger()
    Private astroUtilities As New AstroUtils()

#Region "IRotator Implementation"

    Private rotatorPosition As Single = 0 ' Synced or mechanical position angle of the rotator depending on the value of CanSync
    Private rotatorMechanicalPosition As Single = 0 ' Mechanical position angle Of the rotator 

    Public ReadOnly Property CanReverse() As Boolean Implements IRotatorV2.CanReverse
        Get
            TL.LogMessage("CanReverse Get", False.ToString())
            Return False
        End Get
    End Property

    Public Sub Halt() Implements IRotatorV2.Halt
        TL.LogMessage("Halt", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("Halt")
    End Sub

    Public ReadOnly Property IsMoving() As Boolean Implements IRotatorV2.IsMoving
        Get
            TL.LogMessage("IsMoving Get", False.ToString()) ' This rotator has instantaneous movement
            Return False
        End Get
    End Property

    Public Sub Move(Position As Single) Implements IRotatorV2.Move
        TL.LogMessage("Move", Position.ToString()) ' Move by this amount
        rotatorPosition += Position
        rotatorPosition = astroUtilities.Range(rotatorPosition, 0.0, True, 360.0, False) ' Ensure value is in the range 0.0..359.9999...
    End Sub

    Public Sub MoveAbsolute(Position As Single) Implements IRotatorV2.MoveAbsolute
        TL.LogMessage("MoveAbsolute", Position.ToString()) ' Move to this position
        rotatorPosition = Position
        rotatorPosition = astroUtilities.Range(rotatorPosition, 0.0, True, 360.0, False) ' Ensure value is in the range 0.0..359.9999...
    End Sub

    Public ReadOnly Property Position() As Single Implements IRotatorV2.Position
        Get
            TL.LogMessage("Position Get", rotatorPosition.ToString()) ' This rotator has instantaneous movement
            Return rotatorPosition
        End Get
    End Property

    Public Property Reverse() As Boolean Implements IRotatorV2.Reverse
        Get
            TL.LogMessage("Reverse Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Reverse", False)
        End Get
        Set(value As Boolean)
            TL.LogMessage("Reverse Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Reverse", True)
        End Set
    End Property

    Public ReadOnly Property StepSize() As Single Implements IRotatorV2.StepSize
        Get
            TL.LogMessage("StepSize Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("StepSize", False)
        End Get
    End Property

    Public ReadOnly Property TargetPosition() As Single Implements IRotatorV2.TargetPosition
        Get
            TL.LogMessage("TargetPosition Get", rotatorPosition.ToString()) ' This rotator has instantaneous movement
            Return rotatorPosition
        End Get
    End Property

    ' IRotatorV3 methods

    Public ReadOnly Property CanSync As Boolean
        Get
            TL.LogMessage("CanSync Get", False.ToString());
            Return False;
        End Get
    End Property

    Public ReadOnly Property MechanicalPosition As Single
        Get
            TL.LogMessage("MechanicalPosition Get", rotatorMechanicalPosition.ToString())
            Return rotatorMechanicalPosition
        End Get
    End Property

    Public Sub MoveMechanical(Position As Single)
        TL.LogMessage("MoveMechanical", Position.ToString()) ' Move To this position

        ' TODO Implement correct sync behaviour. i.e. If the rotator has been synced the mechanical And rotator positions won't be the same
        rotatorMechanicalPosition = astroUtilities.Range(Position, 0.0, True, 360.0, False) ' Ensure value Is In the range 0.0..359.9999...
        rotatorPosition = astroUtilities.Range(Position, 0.0, True, 360.0, False) ' Ensure value Is In the range 0.0..359.9999...
    End Sub

    Public Sub Sync(Position As Single)

        TL.LogMessage("Sync", Position.ToString()) ' Sync To this position

        ' TODO Implement correct sync behaviour. i.e. the rotator mechanical And rotator positions may Not be the same
        rotatorPosition = astroUtilities.Range(Position, 0.0, True, 360.0, False); // Ensure value Is In the range 0.0..359.9999...
    End Sub

#End Region

    '//ENDOFINSERTEDFILE
End Class