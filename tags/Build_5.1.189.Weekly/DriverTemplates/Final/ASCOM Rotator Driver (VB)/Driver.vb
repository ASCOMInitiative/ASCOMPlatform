'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Rotator driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM Rotator interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Rotator template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.$safeprojectname$.Rotator
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Rotator
' The ClassInterface/None addribute prevents an empty interface called
' _Rotator from being created and used as the [default] interface
'
<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Rotator
'	==========
	Implements IRotator	' Early-bind interface implemented by this driver
'	==========
	'
	' Driver ID and descriptive string that shows in the Chooser
	'
	Private Shared s_csDriverID As String = "ASCOM.$safeprojectname$.Rotator"
	' TODO Change the descriptive string for your driver then remove this line
	Private Shared s_csDriverDescription As String = "$safeprojectname$ Rotator"

	'
	' Constructor - Must be public for COM registration!
	'
	Public Sub New()
		' TODO Implement your additional construction here
	End Sub

#Region "ASCOM Registration"

	Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

		Dim P As New Helper.Profile()
		P.DeviceTypeV = "Rotator"			'  Requires Helper 5.0.3 or later
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
	' PUBLIC COM INTERFACE IRotator IMPLEMENTATION
	'

#Region "IRotator members"

	Public ReadOnly Property CanReverse() As Boolean Implements IRotator.CanReverse
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanReverse", False)
		End Get
	End Property

	Public Property Connected() As Boolean Implements IRotator.Connected
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Connected", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("Connected", True)
		End Set
	End Property

	Public Sub Halt() Implements IRotator.Halt
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("Halt")
	End Sub

	Public ReadOnly Property IsMoving() As Boolean Implements IRotator.IsMoving
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("IsMoving", False)
		End Get
	End Property

	Public Sub Move(ByVal Position As Single) Implements IRotator.Move
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("Move")
	End Sub

	Public Sub MoveAbsolute(ByVal Position As Single) Implements IRotator.MoveAbsolute
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("MoveAbsolute")
	End Sub

	Public ReadOnly Property Position() As Single Implements IRotator.Position
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Position", False)
		End Get
	End Property

	Public Property Reverse() As Boolean Implements IRotator.Reverse
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Reverse", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("Reverse", True)
		End Set
	End Property

	Public Sub SetupDialog() Implements IRotator.SetupDialog
		' TODO Replace this with your implementation
		Dim F As SetupDialogForm = New SetupDialogForm()
		F.ShowDialog()
	End Sub

	Public ReadOnly Property StepSize() As Single Implements IRotator.StepSize
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("StepSize", False)
		End Get
	End Property

	Public ReadOnly Property TargetPosition() As Single Implements IRotator.TargetPosition
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("TargetPosition", False)
		End Get
	End Property

#End Region

End Class
