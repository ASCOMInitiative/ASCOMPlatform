' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

Class DeviceFocuser
    Implements IFocuserV3
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "IFocuser Implementation"

    Private focuserPosition As Integer = 0 ' Class level variable to hold the current focuser position
    Private Const focuserSteps As Integer = 10000

	''' <summary>
	''' True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
	''' </summary>
	Public ReadOnly Property Absolute() As Boolean Implements IFocuserV3.Absolute
		Get
			TL.LogMessage("Absolute Get", True.ToString())
			Return True ' This is an absolute focuser
		End Get
	End Property

	''' <summary>
	''' Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
	''' </summary>
	Public Sub Halt() Implements IFocuserV3.Halt
		TL.LogMessage("Halt", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("Halt")
	End Sub

	''' <summary>
	''' True if the focuser is currently moving to a new position. False if the focuser is stationary.
	''' </summary>
	Public ReadOnly Property IsMoving() As Boolean Implements IFocuserV3.IsMoving
		Get
			TL.LogMessage("IsMoving Get", False.ToString())
			Return False ' This focuser always moves instantaneously so no need for IsMoving ever to be True
		End Get
	End Property

	''' <summary>
	''' State of the connection to the focuser.
	''' </summary>
	Public Property Link() As Boolean Implements IFocuserV3.Link
		Get
			TL.LogMessage("Link Get", Me.Connected.ToString())
			Return Me.Connected ' Direct function to the connected method, the Link method is just here for backwards compatibility
		End Get
		Set(value As Boolean)
			TL.LogMessage("Link Set", value.ToString())
			Me.Connected = value ' Direct function to the connected method, the Link method is just here for backwards compatibility
		End Set
	End Property

	''' <summary>
	''' Maximum increment size allowed by the focuser;
	''' i.e. the maximum number of steps allowed in one move operation.
	''' </summary>
	Public ReadOnly Property MaxIncrement() As Integer Implements IFocuserV3.MaxIncrement
		Get
			TL.LogMessage("MaxIncrement Get", focuserSteps.ToString())
			Return focuserSteps ' Maximum change in one move
		End Get
	End Property

	''' <summary>
	''' Maximum step position permitted.
	''' </summary>
	Public ReadOnly Property MaxStep() As Integer Implements IFocuserV3.MaxStep
		Get
			TL.LogMessage("MaxStep Get", focuserSteps.ToString())
			Return focuserSteps ' Maximum extent of the focuser, so position range is 0 to 10,000
		End Get
	End Property

	''' <summary>
	''' Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
	''' </summary>
	''' <param name="Position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
	Public Sub Move(Position As Integer) Implements IFocuserV3.Move
		TL.LogMessage("Move", Position.ToString())
		focuserPosition = Position ' Set the focuser position
	End Sub

	''' <summary>
	''' Current focuser position, in steps.
	''' </summary>
	Public ReadOnly Property Position() As Integer Implements IFocuserV3.Position
		Get
			Return focuserPosition ' Return the focuser position
		End Get
	End Property

	''' <summary>
	''' Step size (microns) for the focuser.
	''' </summary>
	Public ReadOnly Property StepSize() As Double Implements IFocuserV3.StepSize
		Get
			TL.LogMessage("StepSize Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("StepSize", False)
		End Get
	End Property

	''' <summary>
	''' The state of temperature compensation mode (if available), else always False.
	''' </summary>
	Public Property TempComp() As Boolean Implements IFocuserV3.TempComp
		Get
			TL.LogMessage("TempComp Get", False.ToString())
			Return False
		End Get
		Set(value As Boolean)
			TL.LogMessage("TempComp Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("TempComp", True)
		End Set
	End Property

	''' <summary>
	''' True if focuser has temperature compensation available.
	''' </summary>
	Public ReadOnly Property TempCompAvailable() As Boolean Implements IFocuserV3.TempCompAvailable
		Get
			TL.LogMessage("TempCompAvailable Get", False.ToString())
			Return False ' Temperature compensation is not available in this driver
		End Get
	End Property

	''' <summary>
	''' Current ambient temperature in degrees Celsius as measured by the focuser.
	''' </summary>
	Public ReadOnly Property Temperature() As Double Implements IFocuserV3.Temperature
		Get
			TL.LogMessage("Temperature Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Temperature", False)
		End Get
	End Property

#End Region

	'//ENDOFINSERTEDFILE
End Class