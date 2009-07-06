'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Switch driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM Switch interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Switch template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.$safeprojectname$.Switch
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Switch
' The ClassInterface/None addribute prevents an empty interface called
' _Switch from being created and used as the [default] interface
'
<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Switch
    '	==========
    Implements ISwitch ' Early-bind interface implemented by this driver
    '	==========
    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Private Shared s_csDriverID As String = "ASCOM.$safeprojectname$.Switch"
    ' TODO Change the descriptive string for your driver then remove this line
    Private Shared s_csDriverDescription As String = "$safeprojectname$ Switch"

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        ' TODO Implement your additional construction here
    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Dim P As New Helper.Profile()
        P.DeviceTypeV = "Switch"           '  Requires Helper 5.0.3 or later
        If bRegister Then
            P.Register(s_csDriverID, s_csDriverDescription)
        Else
            P.Unregister(s_csDriverID)
        End If
        Try                                 ' In case Helper becomes native .NET
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
    ' PUBLIC COM INTERFACE ISwitch IMPLEMENTATION
    '

#Region "ISwitch Members"
    Public Property Connected() As Boolean Implements ISwitch.Connected
        Get
            Throw New PropertyNotImplementedException("Connected", False)
        End Get
        Set(ByVal value As Boolean)
            Throw New PropertyNotImplementedException("Connected", True)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ISwitch.Description
        Get
            Throw New PropertyNotImplementedException("Description", False)
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements ISwitch.DriverInfo
        Get
            Throw New PropertyNotImplementedException("DriverInfo", False)
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ISwitch.DriverVersion
        Get
            Throw New PropertyNotImplementedException("DriverVersion", False)
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ISwitch.InterfaceVersion
        Get
            Throw New PropertyNotImplementedException("InterfaceVersion", False)
        End Get
    End Property

    Public ReadOnly Property MaxSwitch() As Short Implements ISwitch.MaxSwitch
        Get
            Throw New PropertyNotImplementedException("MaxSwitch", False)
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements ISwitch.Name
        Get
            Throw New PropertyNotImplementedException("Name", False)
        End Get
    End Property

    Public Function GetSwitch(ByVal ID As Short) As Boolean Implements ISwitch.GetSwitch
        Throw New MethodNotImplementedException("GetSwitch")
    End Function

    Public Function GetSwitchName(ByVal ID As Short) As String Implements ISwitch.GetSwitchName
        Throw New MethodNotImplementedException("GetSwitchName")
    End Function

    Public Sub SetSwitch(ByVal ID As Short, ByVal State As Boolean) Implements ISwitch.SetSwitch
        Throw New MethodNotImplementedException("SetSwitch")
    End Sub

    Public Sub SetSwitchName(ByVal ID As Short, ByVal State As String) Implements ISwitch.SetSwitchName
        Throw New MethodNotImplementedException("SetSwitchName")
    End Sub

    Public Sub SetupDialog() Implements ISwitch.SetupDialog
        Dim F As SetupDialogForm = New SetupDialogForm()
        F.ShowDialog()
    End Sub

#End Region

End Class
