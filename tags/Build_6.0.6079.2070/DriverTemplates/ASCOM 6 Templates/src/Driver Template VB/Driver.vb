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
' Your driver's ID is ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS
'
' The Guid attribute sets the CLSID for ASCOM.DeviceName.TEMPLATEDEVICECLASS
' The ClassInterface/None addribute prevents an empty interface called
' _TEMPLATEDEVICECLASS from being created and used as the [default] interface
'

' This definition is used to select code that's only applicable for one device type
#Const Device = TEMPLATEDEVICECLASS

Imports ASCOM.Utilities
Imports ASCOM.DeviceInterface

<Guid("3A02C211-FA08-4747-B0BD-4B00EB159297")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class TEMPLATEDEVICECLASS
    '	==========
    ' Early-bind interface implemented by this driver
    ' TODO set the cursor to the end of the "Implements ITEMPLATEDEVICEINTERFACE" line
    ' and press Return.  This will implement the missing interface members for TEMPLATEDEVICECLASS
    '
    ' TODO Replace the not implemented exceptions with code to implement the function or
    ' throw the appropriate ASCOM exception.
    '
    Implements ITEMPLATEDEVICEINTERFACE

    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Private Shared driverID As String = "ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS"
    Private Shared driverDescription As String = "TEMPLATEDEVICENAME TEMPLATEDEVICECLASS"

#If Device = Telescope Then
    '
    ' Driver private data (rate collections), only used with the Telescope class
    '
    Private m_AxisRates(2) As AxisRates
#End If

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
#If Device = Telescope Then
        ' this is only required for the telescope class, it can be deleted from the sources for the other classes
        m_AxisRates(0) = New AxisRates(TelescopeAxes.axisPrimary)
        m_AxisRates(1) = New AxisRates(TelescopeAxes.axisSecondary)
        m_AxisRates(2) = New AxisRates(TelescopeAxes.axisTertiary)
#End If
        ' TODO Implement your additional construction here
    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Using P As New Profile() With {.DeviceType = "TEMPLATEDEVICECLASS"}
            If bRegister Then
                P.Register(driverID, driverDescription)
            Else
                P.Unregister(driverID)
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
    ' PUBLIC COM INTERFACE ITEMPLATEDEVICEINTERFACE IMPLEMENTATION
    '

    ''' <summary>
    ''' Displays the Setup Dialog form.
    ''' If the user clicks the OK button to dismiss the form, then
    ''' the new settings are saved, otherwise the old values are reloaded.
    ''' THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
    ''' </summary>
    Public Sub SetupDialog() Implements ITEMPLATEDEVICEINTERFACE.SetupDialog
        ' consider only showing the setup dialog if not connected
        ' or call a different dialog if connected
        If IsConnected Then
            System.Windows.Forms.MessageBox.Show("Already connected, just press OK")
        End If

        Using F As SetupDialogForm = New SetupDialogForm()
            Dim result As System.Windows.Forms.DialogResult = F.ShowDialog()
            If result = DialogResult.OK Then
                My.MySettings.Default.Save()
                Exit Sub
            End If
            My.MySettings.Default.Reload()
        End Using
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ITEMPLATEDEVICEINTERFACE.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ITEMPLATEDEVICEINTERFACE.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ITEMPLATEDEVICEINTERFACE.CommandBlind
        CheckConnected("CommandBlind")
        ' Call CommandString and return as soon as it finishes
        Me.CommandString(Command, Raw)
        ' or
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements ITEMPLATEDEVICEINTERFACE.CommandBool
        CheckConnected("CommandBool")
        Dim ret As String = CommandString(Command, Raw)
        ' TODO decode the return string and return true or false
        ' or
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements ITEMPLATEDEVICEINTERFACE.CommandString
        CheckConnected("CommandString")
        ' it's a good idea to put all the low level communication with the device here,
        ' then all communication calls this function
        ' you need something to ensure that only one command is in progress at a time
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ITEMPLATEDEVICEINTERFACE.Connected
        Get
            Return IsConnected
        End Get
        Set(ByVal value As Boolean)
            If (value = IsConnected) Then
                Return
            End If

            If (value) Then
                ' TODO connect to the device
                Dim comPort As String = My.Settings.CommPort
            Else
                ' TODO disconnect from the device
                Throw New System.NotImplementedException()
            End If
        End Set
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ITEMPLATEDEVICEINTERFACE.DriverVersion
        Get
            ' Get our own assembly and report its version number
            Return Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2)
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ITEMPLATEDEVICEINTERFACE.InterfaceVersion
        Get
            Return TEMPLATEINTERFACEVERSION
        End Get
    End Property

    Public ReadOnly Property Description As String Implements ITEMPLATEDEVICEINTERFACE.Description
        Get
            ' this pattern seems to be needed to allow a public property to return a private field
            Dim d As String = driverDescription
            Return d
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ITEMPLATEDEVICEINTERFACE.DriverInfo
        Get
            Throw New PropertyNotImplementedException("DriverInfo", False)
        End Get
    End Property

#Region "private properties and methods"
    ' here are some useful properties and methods that can be used as required
    ' to help with

    ''' <summary>
    ''' Returns true if there is a valid connection to the driver hardware
    ''' </summary>
    Private ReadOnly Property IsConnected As Boolean
        Get
            ' TODO check that the driver hardware connection exists and is connected to the hardware
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Use this function to throw an exception if we aren't connected to the hardware
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub CheckConnected(ByVal message As String)
        If Not IsConnected Then
            Throw New NotConnectedException(message)
        End If
    End Sub

#End Region

End Class

