'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM TEMPLATEDEVICECLASS driver for TEMPLATEDEVICENAME
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM TEMPLATEDEVICECLASS interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from TEMPLATEDEVICECLASS template
' ---------------------------------------------------------------------------------
'
'
' Your driver's ID is ASCOM.DeviceName.TEMPLATEDEVICECLASS
'
' The Guid attribute sets the CLSID for ASCOM.DeviceName.TEMPLATEDEVICECLASS
' The ClassInterface/None addribute prevents an empty interface called
' _TEMPLATEDEVICECLASS from being created and used as the [default] interface
'
Imports ASCOM.Utilities

<Guid("3A02C211-FA08-4747-B0BD-4B00EB159297")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class TEMPLATEDEVICECLASS
    '	==========
    Implements ITEMPLATEDEVICECLASS ' Early-bind interface implemented by this driver
    '	==========
    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Private Shared s_csDriverID As String = "ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS"
    Private Shared s_csDriverDescription As String = "TEMPLATEDEVICENAME TEMPLATEDEVICECLASS"

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        ' TODO Implement your additional construction here
    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Dim P As New Profile() With {.DeviceType = "TEMPLATEDEVICECLASS"}
        If bRegister Then
            P.Register(s_csDriverID, s_csDriverDescription)
        Else
            P.Unregister(s_csDriverID)
        End If
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
    ' PUBLIC COM INTERFACE ITEMPLATEDEVICECLASS IMPLEMENTATION
    '
    Public Sub SetupDialog() Implements ITEMPLATEDEVICECLASS.SetupDialog
        Dim F As SetupDialogForm = New SetupDialogForm()
        F.ShowDialog()
    End Sub
End Class

