'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM FilterWheel driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM FilterWheel interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from FilterWheel template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.$safeprojectname$.FilterWheel
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.FilterWheel
' The ClassInterface/None addribute prevents an empty interface called
' _FilterWheel from being created and used as the [default] interface
'
<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class FilterWheel
    '	==========
    Implements IFilterWheel ' Early-bind interface implemented by this driver
    '	==========
    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Private Shared s_csDriverID As String = "ASCOM.$safeprojectname$.FilterWheel"
    ' TODO Change the descriptive string for your driver then remove this line
    Private Shared s_csDriverDescription As String = "$safeprojectname$ FilterWheel"

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        ' TODO Implement your additional construction here
    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Dim P As New Helper.Profile()
        P.DeviceTypeV = "FilterWheel"           '  Requires Helper 5.0.3 or later
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
    ' PUBLIC COM INTERFACE IFilterWheel IMPLEMENTATION
    '

#Region "IFilterWheel Members"
Public Property Connected() As Boolean Implements IFilterWheel.Connected
        Get
            ' TODO Replace this with your implementation
            Throw New PropertyNotImplementedException("Connected", False)
        End Get
        Set(ByVal value As Boolean)
            ' TODO Replace this with your implementation
            Throw New PropertyNotImplementedException("Connected", True)
        End Set
    End Property

    Public Property Position() As Short Implements IFilterWheel.Position
        Get
            ' TODO Replace this with your implementation
            Throw New PropertyNotImplementedException("Position", False)
        End Get
        Set(ByVal value As Short)
            ' TODO Replace this with your implementation
            Throw New PropertyNotImplementedException("Position", True)
        End Set
    End Property

    Public ReadOnly Property FocusOffsets() As Integer() Implements IFilterWheel.FocusOffsets
        Get
            ' TODO Replace this with your implementation
            Throw New PropertyNotImplementedException("FocusOffsets", False)
        End Get
    End Property

    Public ReadOnly Property Names() As String() Implements IFilterWheel.Names
        Get
            ' TODO Replace this with your implementation
            Throw New PropertyNotImplementedException("Names", False)
        End Get
    End Property

    Public Sub SetupDialog() Implements IFilterWheel.SetupDialog
        Dim F As SetupDialogForm = New SetupDialogForm()
        F.ShowDialog()
    End Sub
#End Region

End Class
