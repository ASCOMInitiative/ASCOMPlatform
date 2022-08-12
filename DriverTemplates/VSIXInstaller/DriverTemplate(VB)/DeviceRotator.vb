' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities
Imports ASCOM.Astrometry.AstroUtils

Class DeviceRotator
    Implements IRotatorV3
    Private m_util As New Util()
    Private TL As New TraceLogger()
    Private astroUtilities As New AstroUtils()

#Region "IRotator Implementation"

    Private rotatorPosition As Single = 0 ' Synced or mechanical position angle of the rotator
    Private rotatorMechanicalPosition As Single = 0 ' Mechanical position angle Of the rotator 

	''' <summary>
	''' Indicates whether the Rotator supports the <see cref="Reverse" /> method.
	''' </summary>
	''' <returns>
	''' True if the Rotator supports the <see cref="Reverse" /> method.
	''' </returns>
	Public ReadOnly Property CanReverse() As Boolean Implements IRotatorV3.CanReverse
		Get
			TL.LogMessage("CanReverse Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Immediately stop any Rotator motion due to a previous <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see> method call.
	''' </summary>
	Public Sub Halt() Implements IRotatorV3.Halt
		TL.LogMessage("Halt", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("Halt")
	End Sub

	''' <summary>
	''' Indicates whether the rotator is currently moving
	''' </summary>
	''' <returns>True if the Rotator is moving to a new position. False if the Rotator is stationary.</returns>
	Public ReadOnly Property IsMoving() As Boolean Implements IRotatorV3.IsMoving
		Get
			TL.LogMessage("IsMoving Get", False.ToString()) ' This rotator has instantaneous movement
			Return False
		End Get
	End Property

	''' <summary>
	''' Causes the rotator to move Position degrees relative to the current <see cref="Position" /> value.
	''' </summary>
	''' <param name="Position">Relative position to move in degrees from current <see cref="Position" />.</param>
	Public Sub Move(Position As Single) Implements IRotatorV3.Move
		TL.LogMessage("Move", Position.ToString()) ' Move by this amount
		rotatorPosition += Position
		rotatorPosition = astroUtilities.Range(rotatorPosition, 0.0, True, 360.0, False) ' Ensure value is in the range 0.0..359.9999...
	End Sub

	''' <summary>
	''' Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
	''' </summary>
	''' <param name="Position">Absolute position in degrees.</param>
	Public Sub MoveAbsolute(Position As Single) Implements IRotatorV3.MoveAbsolute
		TL.LogMessage("MoveAbsolute", Position.ToString()) ' Move to this position
		rotatorPosition = Position
		rotatorPosition = astroUtilities.Range(rotatorPosition, 0.0, True, 360.0, False) ' Ensure value is in the range 0.0..359.9999...
	End Sub

	''' <summary>
	''' Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
	''' </summary>
	''' <param name="Position">Absolute position in degrees.</param>
	Public ReadOnly Property Position() As Single Implements IRotatorV3.Position
		Get
			TL.LogMessage("Position Get", rotatorPosition.ToString()) ' This rotator has instantaneous movement
			Return rotatorPosition
		End Get
	End Property

	''' <summary>
	''' Sets or Returns the rotator’s Reverse state.
	''' </summary>
	Public Property Reverse() As Boolean Implements IRotatorV3.Reverse
		Get
			TL.LogMessage("Reverse Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Reverse", False)
		End Get
		Set(value As Boolean)
			TL.LogMessage("Reverse Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Reverse", True)
		End Set
	End Property

	''' <summary>
	''' The minimum StepSize, in degrees.
	''' </summary>
	Public ReadOnly Property StepSize() As Single Implements IRotatorV3.StepSize
		Get
			TL.LogMessage("StepSize Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("StepSize", False)
		End Get
	End Property

	''' <summary>
	''' The destination position angle for Move() and MoveAbsolute().
	''' </summary>
	Public ReadOnly Property TargetPosition() As Single Implements IRotatorV3.TargetPosition
		Get
			TL.LogMessage("TargetPosition Get", rotatorPosition.ToString()) ' This rotator has instantaneous movement
			Return rotatorPosition
		End Get
	End Property

	' IRotatorV3 methods

	''' <summary>
	''' This returns the raw mechanical position of the rotator in degrees.
	''' </summary>
	Public ReadOnly Property MechanicalPosition As Single Implements IRotatorV3.MechanicalPosition
		Get
			TL.LogMessage("MechanicalPosition Get", rotatorMechanicalPosition.ToString())
			Return rotatorMechanicalPosition
		End Get
	End Property

	''' <summary>
	''' Moves the rotator to the specified mechanical angle. 
	''' </summary>
	''' <param name="Position">Mechanical rotator position angle.</param>
	Public Sub MoveMechanical(Position As Single) Implements IRotatorV3.MoveMechanical
		TL.LogMessage("MoveMechanical", Position.ToString()) ' Move To this position

		' TODO Implement correct sync behaviour. i.e. If the rotator has been synced the mechanical And rotator positions won't be the same
		rotatorMechanicalPosition = astroUtilities.Range(Position, 0.0, True, 360.0, False) ' Ensure value Is In the range 0.0..359.9999...
		rotatorPosition = astroUtilities.Range(Position, 0.0, True, 360.0, False) ' Ensure value is In the range 0.0..359.9999...
	End Sub

	''' <summary>
	''' Syncs the rotator to the specified position angle without moving it. 
	''' </summary>
	''' <param name="Position">Synchronised rotator position angle.</param>
	Public Sub Sync(Position As Single) Implements IRotatorV3.Sync

		TL.LogMessage("Sync", Position.ToString()) ' Sync To this position

		' TODO Implement correct sync behaviour. i.e. the rotator mechanical And rotator positions may Not be the same
		rotatorPosition = astroUtilities.Range(Position, 0.0, True, 360.0, False) ' Ensure value is In the range 0.0..359.9999...
	End Sub

#End Region

	'//ENDOFINSERTEDFILE
End Class