'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM $safeprojectname$ driver for DeviceName
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM $safeprojectname$ interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from $safeprojectname$ template
' ---------------------------------------------------------------------------------
'
'
' Your driver's ID is ASCOM.DeviceName.$safeprojectname$
'
' The Guid attribute sets the CLSID for ASCOM.DeviceName.$safeprojectname$
' The ClassInterface/None addribute prevents an empty interface called
' _$safeprojectname$ from being created and used as the [default] interface
'
<Guid("GUIDSUBST")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class $safeprojectname$
'	==========
	Implements I$safeprojectname$	' Early-bind interface implemented by this driver
'	==========
	'
	' Driver ID and descriptive string that shows in the Chooser
	'
	Private Shared s_csDriverID As String = "ASCOM.DeviceName.$safeprojectname$"
	' TODO Change the descriptive string for your driver then remove this line
	Private Shared s_csDriverDescription As String = "DeviceName $safeprojectname$"

	'
	' Constructor - Must be public for COM registration!
	'
	Public Sub New()
		' TODO Implement your additional construction here
	End Sub

#Region "ASCOM Registration"

	Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

		Dim P As New Helper.Profile()
		P.DeviceTypeV = "$safeprojectname$"			'  Requires Helper 5.0.3 or later
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
	' PUBLIC COM INTERFACE I$safeprojectname$ IMPLEMENTATION
	'

#Region "I$safeprojectname$ Members"

	Public Sub SetupDialog() Implements I$safeprojectname$.SetupDialog
		Dim F As SetupDialogForm = New SetupDialogForm()
		F.ShowDialog()
	End Sub

#End Region

End Class

