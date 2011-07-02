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
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Dome
    '	==========
    Implements IDomeV2   ' Early-bind interface implemented by this driver
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
        Using P As New Profile()
            P.DeviceType = "Dome"
            If bRegister Then
                P.Register(s_csDriverID, s_csDriverDescription)
            Else
                P.Unregister(s_csDriverID)
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

    '
    ' PUBLIC COM INTERFACE IDome IMPLEMENTATION
    '

#Region "IDome Members"

    Public Sub SetupDialog() Implements IDomeV2.SetupDialog
        Using f As New SetupDialogForm
            f.ShowDialog()
        End Using
    End Sub

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IDomeV2.Action
        Throw New ASCOM.MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IDomeV2.CommandBlind
        Throw New ASCOM.MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements IDomeV2.CommandBool
        Throw New ASCOM.MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements IDomeV2.CommandString
        Throw New ASCOM.MethodNotImplementedException("CommandString")
    End Function

    Public Sub Dispose() Implements IDomeV2.Dispose
        Throw New System.NotImplementedException()
    End Sub

    Public Sub AbortSlew() Implements IDomeV2.AbortSlew
        Throw New System.NotImplementedException()
    End Sub

    Public Sub CloseShutter() Implements IDomeV2.CloseShutter
        Throw New System.NotImplementedException()
    End Sub

    Public Sub FindHome() Implements IDomeV2.FindHome
        Throw New System.NotImplementedException()
    End Sub

    Public Sub OpenShutter() Implements IDomeV2.OpenShutter
        Throw New System.NotImplementedException()
    End Sub

    Public Sub Park() Implements IDomeV2.Park
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SetPark() Implements IDomeV2.SetPark
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SlewToAltitude(ByVal Altitude As Double) Implements IDomeV2.SlewToAltitude
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SlewToAzimuth(ByVal Azimuth As Double) Implements IDomeV2.SlewToAzimuth
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SyncToAzimuth(ByVal Azimuth As Double) Implements IDomeV2.SyncToAzimuth
        Throw New System.NotImplementedException()
    End Sub

    Public Property Connected() As Boolean Implements IDomeV2.Connected
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IDomeV2.Description
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements IDomeV2.DriverInfo
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IDomeV2.DriverVersion
        Get
            Dim Ass As Reflection.Assembly

            Ass = Reflection.Assembly.GetExecutingAssembly 'Get our own assembly and report its version number
            Return Ass.GetName.Version.Major.ToString & "." & Ass.GetName.Version.Minor.ToString
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IDomeV2.InterfaceVersion
        Get
            Return 2
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements IDomeV2.Name
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SupportedActions() As ArrayList Implements IDomeV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public ReadOnly Property Altitude() As Double Implements IDomeV2.Altitude
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements IDomeV2.AtHome
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements IDomeV2.AtPark
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Azimuth() As Double Implements IDomeV2.Azimuth
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanFindHome() As Boolean Implements IDomeV2.CanFindHome
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanPark() As Boolean Implements IDomeV2.CanPark
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetAltitude() As Boolean Implements IDomeV2.CanSetAltitude
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetAzimuth() As Boolean Implements IDomeV2.CanSetAzimuth
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements IDomeV2.CanSetPark
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetShutter() As Boolean Implements IDomeV2.CanSetShutter
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSlave() As Boolean Implements IDomeV2.CanSlave
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSyncAzimuth() As Boolean Implements IDomeV2.CanSyncAzimuth
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ShutterStatus() As ShutterState Implements IDomeV2.ShutterStatus
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property Slaved() As Boolean Implements IDomeV2.Slaved
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Slewing() As Boolean Implements IDomeV2.Slewing
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property
#End Region

End Class

