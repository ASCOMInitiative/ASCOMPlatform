Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM SafetyMonitor driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM SafetyMonitor interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from SafetyMonitor template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.$safeprojectname$.SafetyMonitor
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.SafetyMonitor
' The ClassInterface/None addribute prevents an empty interface called
' _SafetyMonitor from being created and used as the [default] interface
'

<Guid("$guid2$")> _
    <ClassInterface(ClassInterfaceType.None)> _
    <ComVisible(True)> _
Public Class SafetyMonitor
    '	==========
    Implements ISafetyMonitor
    ' Early-bind interface implemented by this driver
    '	==========
    ' 
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Private Shared _driverID As String = "ASCOM.$safeprojectname$.SafetyMonitor"
    ' TODO Change the descriptive string for your driver then remove this line
    Private Shared _driverDescription As String = "$safeprojectname$ SafetyMonitor"

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        ' TODO Implement your additional construction here
    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Using P As New Profile()
            P.DeviceType = "SafetyMonitor"
            If bRegister Then
                P.Register(_driverID, _driverDescription)
            Else
                P.Unregister(_driverID)
            End If
        End Using
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

#Region "Interface implementation"

    Public Sub SetupDialog() Implements ISafetyMonitor.SetupDialog
        Using f As New SetupDialogForm
            f.ShowDialog()
        End Using
    End Sub

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ISafetyMonitor.Action
        Throw New ASCOM.MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ISafetyMonitor.CommandBlind
        Throw New ASCOM.MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements ISafetyMonitor.CommandBool
        Throw New ASCOM.MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements ISafetyMonitor.CommandString
        Throw New ASCOM.MethodNotImplementedException("CommandString")
    End Function

    Public Sub Dispose() Implements ISafetyMonitor.Dispose
        Throw New System.NotImplementedException()
    End Sub

    Public Property Connected() As Boolean Implements ISafetyMonitor.Connected
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ISafetyMonitor.Description
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements ISafetyMonitor.DriverInfo
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ISafetyMonitor.DriverVersion
        Get
            Dim Ass As Reflection.Assembly

            Ass = Reflection.Assembly.GetExecutingAssembly 'Get our own assembly and report its version number
            Return Ass.GetName.Version.Major.ToString & "." & Ass.GetName.Version.Minor.ToString
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ISafetyMonitor.InterfaceVersion
        Get
            Return 2
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements ISafetyMonitor.Name
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SupportedActions() As ArrayList Implements ISafetyMonitor.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public ReadOnly Property IsSafe() As Boolean Implements ISafetyMonitor.IsSafe
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property
#End Region
End Class
