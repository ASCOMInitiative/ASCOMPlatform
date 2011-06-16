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
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
<ComVisible(True)> _
Public Class Focuser
    '	==========
    Implements IFocuserV2 ' Early-bind interface implemented by this driver
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
        Using P As New Profile()
            P.DeviceType = "Focuser"
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
    ' PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
    '

#Region "IFocuser Members"

    Public Sub SetupDialog() Implements IFocuserV2.SetupDialog
        Using f As New SetupDialogForm
            f.ShowDialog()
        End Using
    End Sub

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IFocuserV2.Action
        Throw New ASCOM.MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IFocuserV2.CommandBlind
        Throw New ASCOM.MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements IFocuserV2.CommandBool
        Throw New ASCOM.MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements IFocuserV2.CommandString
        Throw New ASCOM.MethodNotImplementedException("CommandString")
    End Function

    Public Sub Dispose() Implements IFocuserV2.Dispose
        Throw New System.NotImplementedException()
    End Sub

    Public Sub Halt() Implements IFocuserV2.Halt
        Throw New System.NotImplementedException()
    End Sub

    Public Sub Move(ByVal Value As Integer) Implements IFocuserV2.Move
        Throw New System.NotImplementedException()
    End Sub

    Public Property Connected() As Boolean Implements IFocuserV2.Connected
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IFocuserV2.Description
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements IFocuserV2.DriverInfo
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IFocuserV2.DriverVersion
        Get
            Dim Ass As Reflection.Assembly

            Ass = Reflection.Assembly.GetExecutingAssembly 'Get our own assembly and report its version number
            Return Ass.GetName.Version.Major.ToString & "." & Ass.GetName.Version.Minor.ToString
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IFocuserV2.InterfaceVersion
        Get
            Return 2
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements IFocuserV2.Name
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SupportedActions() As ArrayList Implements IFocuserV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public ReadOnly Property Absolute() As Boolean Implements IFocuserV2.Absolute
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property IsMoving() As Boolean Implements IFocuserV2.IsMoving
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property Link() As Boolean Implements IFocuserV2.Link
        Get
            Return Me.Connected         ' use the V2 Connected property
        End Get
        Set(ByVal value As Boolean)
            Me.Connected = value        ' use the V2 Connected property
        End Set
    End Property

    Public ReadOnly Property MaxIncrement() As Integer Implements IFocuserV2.MaxIncrement
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property MaxStep() As Integer Implements IFocuserV2.MaxStep
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Position() As Integer Implements IFocuserV2.Position
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property StepSize() As Double Implements IFocuserV2.StepSize
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property TempComp() As Boolean Implements IFocuserV2.TempComp
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property TempCompAvailable() As Boolean Implements IFocuserV2.TempCompAvailable
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Temperature() As Double Implements IFocuserV2.Temperature
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

#End Region

End Class
