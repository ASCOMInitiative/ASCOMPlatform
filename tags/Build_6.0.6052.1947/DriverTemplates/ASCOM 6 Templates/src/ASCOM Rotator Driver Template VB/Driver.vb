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
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
<ComVisible(True)> _
Public Class Rotator
    '	==========
    Implements IRotatorV2 ' Early-bind interface implemented by this driver
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
        Using P As New Profile()
            P.DeviceType = "Rotator"
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
    ' PUBLIC COM INTERFACE IRotator IMPLEMENTATION
    '

#Region "IRotator members"

    Public Sub SetupDialog() Implements IRotatorV2.SetupDialog
        Using f As New SetupDialogForm
            f.ShowDialog()
        End Using
    End Sub

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IRotatorV2.Action
        Throw New ASCOM.MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IRotatorV2.CommandBlind
        Throw New ASCOM.MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements IRotatorV2.CommandBool
        Throw New ASCOM.MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements IRotatorV2.CommandString
        Throw New ASCOM.MethodNotImplementedException("CommandString")
    End Function

    Public Sub Dispose() Implements IRotatorV2.Dispose
        Throw New System.NotImplementedException()
    End Sub

    Public Sub Halt() Implements IRotatorV2.Halt
        Throw New System.NotImplementedException()
    End Sub

    Public Sub Move(ByVal Position As Single) Implements IRotatorV2.Move
        Throw New System.NotImplementedException()
    End Sub

    Public Sub MoveAbsolute(ByVal Position As Single) Implements IRotatorV2.MoveAbsolute
        Throw New System.NotImplementedException()
    End Sub

    Public Property Connected() As Boolean Implements IRotatorV2.Connected
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IRotatorV2.Description
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements IRotatorV2.DriverInfo
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IRotatorV2.DriverVersion
        Get
            Dim Ass As Reflection.Assembly

            Ass = Reflection.Assembly.GetExecutingAssembly 'Get our own assembly and report its version number
            Return Ass.GetName.Version.Major.ToString & "." & Ass.GetName.Version.Minor.ToString
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IRotatorV2.InterfaceVersion
        Get
            Return 2
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements IRotatorV2.Name
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SupportedActions() As ArrayList Implements IRotatorV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public ReadOnly Property CanReverse() As Boolean Implements IRotatorV2.CanReverse
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property IsMoving() As Boolean Implements IRotatorV2.IsMoving
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Position() As Single Implements IRotatorV2.Position
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property Reverse() As Boolean Implements IRotatorV2.Reverse
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property StepSize() As Single Implements IRotatorV2.StepSize
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property TargetPosition() As Single Implements IRotatorV2.TargetPosition
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property
#End Region

End Class
