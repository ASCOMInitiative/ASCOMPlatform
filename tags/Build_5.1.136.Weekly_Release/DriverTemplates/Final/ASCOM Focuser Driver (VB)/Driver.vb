'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Focuser driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM Focuser interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Focuser template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.$safeprojectname$.Focuser
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Focuser
' The ClassInterface/None addribute prevents an empty interface called
' _Focuser from being created and used as the [default] interface
'
<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Focuser
'	==========
	Implements IFocuser	' Early-bind interface implemented by this driver
'	==========
	'
	' Driver ID and descriptive string that shows in the Chooser
	'
	Private Shared s_csDriverID As String = "ASCOM.$safeprojectname$.Focuser"
	' TODO Change the descriptive string for your driver then remove this line
	Private Shared s_csDriverDescription As String = "$safeprojectname$ Focuser"

	'
	' Constructor - Must be public for COM registration!
	'
	Public Sub New()
		' TODO Implement your additional construction here
	End Sub

#Region "ASCOM Registration"

	Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

		Dim P As New Helper.Profile()
		P.DeviceTypeV = "Focuser"			'  Requires Helper 5.0.3 or later
		If bRegister Then
			P.Register(s_csDriverID, s_csDriverDescription)
		Else
			P.Unregister(s_csDriverID)
		End If
		Try									' In case Helper becomes native .NET
			Marshal.ReleaseComObject(P)
		Catch ex As Exception
			' Ignore exception
		End Try
		P = Nothing

	End Sub

	<ComRegisterFunction()> _
	Public Shared Sub RegisterASCOM(ByVal T As Type)

		RegUnregASCOM(True)

	End Sub

	<ComUnregisterFunction()> _
	Public Shared Sub UnregisterASCOM(ByVal T As Type)

		RegUnregASCOM(False)

	End Sub

#End Region

	'
	' PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
	'

#Region "IFocuser Members"

	Public ReadOnly Property Absolute() As Boolean Implements IFocuser.Absolute
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Absolute", False)
		End Get
	End Property

	Public Sub Halt() Implements IFocuser.Halt
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("Halt")
	End Sub

	Public ReadOnly Property IsMoving() As Boolean Implements IFocuser.IsMoving
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("IsMoving", False)
		End Get
	End Property

	Public Property Link() As Boolean Implements IFocuser.Link
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Link", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("Link", True)
		End Set
	End Property

	Public ReadOnly Property MaxIncrement() As Integer Implements IFocuser.MaxIncrement
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("MaxIncrement", False)
		End Get
	End Property

	Public ReadOnly Property MaxStep() As Integer Implements IFocuser.MaxStep
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("MaxStep", False)
		End Get
	End Property

	Public Sub Move(ByVal val As Integer) Implements IFocuser.Move
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("Move")
	End Sub

	Public ReadOnly Property Position() As Integer Implements IFocuser.Position
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Position", False)
		End Get
	End Property

	Public Sub SetupDialog() Implements IFocuser.SetupDialog
		Dim F As SetupDialogForm = New SetupDialogForm()
		F.ShowDialog()
	End Sub

	Public ReadOnly Property StepSize() As Double Implements IFocuser.StepSize
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("StepSize", False)
		End Get
	End Property

	Public Property TempComp() As Boolean Implements IFocuser.TempComp
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("TempComp", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("TempComp", True)
		End Set
	End Property

	Public ReadOnly Property TempCompAvailable() As Boolean Implements IFocuser.TempCompAvailable
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("TempCompAvailable", False)
		End Get
	End Property

	Public ReadOnly Property Temperature() As Double Implements IFocuser.Temperature
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Temperature", False)
		End Get
	End Property

#End Region

End Class
