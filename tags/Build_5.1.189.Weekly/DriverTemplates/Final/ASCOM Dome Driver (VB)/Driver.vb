'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Dome driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM Dome interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Dome template
' --------------------------------------------------------------------------------
'

' Your driver's ID is ASCOM.$safeprojectname$.Dome
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Dome
' The ClassInterface/None addribute prevents an empty interface called
' _Dome from being created and used as the [default] interface
'
<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Dome
'	==========
	Implements IDome	' Early-bind interface implemented by this driver
'	==========
	'
	' Driver ID and descriptive string that shows in the Chooser
	'
	Private Shared s_csDriverID As String = "ASCOM.$safeprojectname$.Dome"
	' TODO Change the descriptive string for your driver then remove this line
	Private Shared s_csDriverDescription As String = "$safeprojectname$ Dome"

	'
	' Constructor - Must be public for COM registration!
	'
	Public Sub New()
		' TODO Implement your additional construction here
	End Sub

#Region "ASCOM Registration"

	Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

		Dim P As New Helper.Profile()
		P.DeviceTypeV = "Dome"				'  Requires Helper 5.0.3 or later
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
	' PUBLIC COM INTERFACE IDome IMPLEMENTATION
	'

#Region "IDome Members"

	Public Sub AbortSlew() Implements IDome.AbortSlew
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("AbortSlew")
	End Sub

	Public ReadOnly Property Altitude() As Double Implements IDome.Altitude
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Altitude", False)
		End Get
	End Property

	Public ReadOnly Property AtHome() As Boolean Implements IDome.AtHome
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("AtHome", False)
		End Get
	End Property

	Public ReadOnly Property AtPark() As Boolean Implements IDome.AtPark
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("AtPark", False)
		End Get
	End Property

	Public ReadOnly Property Azimuth() As Double Implements IDome.Azimuth
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Azimuth", False)
		End Get
	End Property

	Public ReadOnly Property CanFindHome() As Boolean Implements IDome.CanFindHome
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanFindHome", False)
		End Get
	End Property

	Public ReadOnly Property CanPark() As Boolean Implements IDome.CanPark
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanPark", False)
		End Get
	End Property

	Public ReadOnly Property CanSetAltitude() As Boolean Implements IDome.CanSetAltitude
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetAltitude", False)
		End Get
	End Property

	Public ReadOnly Property CanSetAzimuth() As Boolean Implements IDome.CanSetAzimuth
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetAzimuth", False)
		End Get
	End Property

	Public ReadOnly Property CanSetPark() As Boolean Implements IDome.CanSetPark
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetPark", False)
		End Get
	End Property

	Public ReadOnly Property CanSetShutter() As Boolean Implements IDome.CanSetShutter
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetShutter", False)
		End Get
	End Property

	Public ReadOnly Property CanSlave() As Boolean Implements IDome.CanSlave
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSlave", False)
		End Get
	End Property

	Public ReadOnly Property CanSyncAzimuth() As Boolean Implements IDome.CanSyncAzimuth
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSyncAzimuth", False)
		End Get
	End Property

	Public Sub CloseShutter() Implements IDome.CloseShutter
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("CloseShutter")
	End Sub

	Public Sub CommandBlind(ByVal Command As String) Implements IDome.CommandBlind
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("CommandBlind")
	End Sub

	Public Function CommandBool(ByVal Command As String) As Boolean Implements IDome.CommandBool
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("CommandBool")
	End Function

	Public Function CommandString(ByVal Command As String) As String Implements IDome.CommandString
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("CommandString")
	End Function

	Public Property Connected() As Boolean Implements IDome.Connected
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Connected", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("Connected", True)
		End Set
	End Property

	Public ReadOnly Property Description() As String Implements IDome.Description
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Description", False)
		End Get
	End Property

	Public ReadOnly Property DriverInfo() As String Implements IDome.DriverInfo
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("DriverInfo", False)
		End Get
	End Property

	Public Sub FindHome() Implements IDome.FindHome
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("FindHome")
	End Sub

	Public ReadOnly Property InterfaceVersion() As Short Implements IDome.InterfaceVersion
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("InterfaceVersion", False)
		End Get
	End Property

	Public ReadOnly Property Name() As String Implements IDome.Name
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Name", False)
		End Get
	End Property

	Public Sub OpenShutter() Implements IDome.OpenShutter
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("OpenShutter")
	End Sub

	Public Sub Park() Implements IDome.Park
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("Park")
	End Sub

	Public Sub SetPark() Implements IDome.SetPark
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SetPark")
	End Sub

	Public Sub SetupDialog() Implements IDome.SetupDialog
		Dim F As SetupDialogForm = New SetupDialogForm()
		F.ShowDialog()
	End Sub

	Public ReadOnly Property ShutterStatus() As ShutterState Implements IDome.ShutterStatus
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("ShutterStatus", False)
		End Get
	End Property

	Public Property Slaved() As Boolean Implements IDome.Slaved
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Slaved", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("Slaved", True)
		End Set
	End Property

	Public Sub SlewToAltitude(ByVal Altitude As Double) Implements IDome.SlewToAltitude
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SlewToAltitude")
	End Sub

	Public Sub SlewToAzimuth(ByVal Azimuth As Double) Implements IDome.SlewToAzimuth
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SlewToAzimuth")
	End Sub

	Public ReadOnly Property Slewing() As Boolean Implements IDome.Slewing
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Slewing", False)
		End Get
	End Property

	Public Sub SyncToAzimuth(ByVal Azimuth As Double) Implements IDome.SyncToAzimuth
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SyncToAzimuth")
	End Sub

#End Region

End Class

