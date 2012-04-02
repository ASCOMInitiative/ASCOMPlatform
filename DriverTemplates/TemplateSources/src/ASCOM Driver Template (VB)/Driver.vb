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
#Const Device = "TEMPLATEDEVICECLASS"

Imports ASCOM
Imports ASCOM.Astrometry
Imports ASCOM.Astrometry.AstroUtils
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text

<Guid("3A02C211-FA08-4747-B0BD-4B00EB159297")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class TEMPLATEDEVICECLASS

    ' The Guid attribute sets the CLSID for ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS
    ' The ClassInterface/None addribute prevents an empty interface called
    ' _TEMPLATEDEVICENAME from being created and used as the [default] interface

    ' TODO Replace the not implemented exceptions with code to implement the function or
    ' throw the appropriate ASCOM exception.
    '
    Implements ITEMPLATEDEVICEINTERFACE

    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Private Shared driverID As String = "ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS"
    Private Shared driverDescription As String = "TEMPLATEDEVICENAME TEMPLATEDEVICECLASS"

    Private connectedState As Boolean ' Private variable to hold the connected state
    Private utilities As Util ' Private variable to hold an ASCOM Utilities object
    Private astroUtilities As AstroUtils
    Private TL As TraceLogger ' Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()

        TL = New TraceLogger("", "TEMPLATEDEVICENAME")
        TL.Enabled = My.MySettings.Default.Trace
        TL.LogMessage("TEMPLATEDEVICECLASS", "Starting initialisation")

        connectedState = False ' Initialise connected to false
        utilities = New Util() ' Initialise util object
        astroUtilities = New AstroUtils 'Initialise new astro utiliites object

        'TODO: Implement your additional construction here

        TL.LogMessage("TEMPLATEDEVICECLASS", "Completed initialisation")
    End Sub

    '
    ' PUBLIC COM INTERFACE ITEMPLATEDEVICEINTERFACE IMPLEMENTATION
    '

#Region "Common properties and methods"
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
            TL.LogMessage("SupportedActions Get", "Returning empty arraylist")
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
            TL.LogMessage("Connected Get", IsConnected.ToString())
            Return IsConnected
        End Get
        Set(value As Boolean)
            TL.LogMessage("Connected Set", value.ToString())
            If value = IsConnected Then
                Return
            End If

            If value Then
                connectedState = True
                ' TODO connect to the device
                Dim comPort As String = My.MySettings.Default.CommPort
            Else
                ' TODO disconnect from the device
                connectedState = False
            End If
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ITEMPLATEDEVICEINTERFACE.Description
        Get
            ' this pattern seems to be needed to allow a public property to return a private field
            Dim d As String = driverDescription
            TL.LogMessage("Description Get", d)
            Return d
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ITEMPLATEDEVICEINTERFACE.DriverInfo
        Get
            Dim m_version As Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
            ' TODO customise this driver description
            Dim s_driverInfo As String = "Information about the driver itself. Version: " + m_version.Major.ToString() + "." + m_version.Minor.ToString()
            TL.LogMessage("DriverInfo Get", s_driverInfo)
            Return s_driverInfo
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ITEMPLATEDEVICEINTERFACE.DriverVersion
        Get
            ' Get our own assembly and report its version number
            TL.LogMessage("DriverVersion Get", Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2))
            Return Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2)
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ITEMPLATEDEVICEINTERFACE.InterfaceVersion
        Get
            TL.LogMessage("InterfaceVersion Get", "TEMPLATEINTERFACEVERSION")
            Return TEMPLATEINTERFACEVERSION
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ITEMPLATEDEVICEINTERFACE.Name
        Get
            Dim s_name As String = "Short driver name - please customise"
            TL.LogMessage("Name Get", s_name)
            Return s_name
        End Get
    End Property

    Public Sub Dispose() Implements ITEMPLATEDEVICEINTERFACE.Dispose
        ' Clean up the tracelogger and util objects
        TL.Enabled = False
        TL.Dispose()
        TL = Nothing
        utilities.Dispose()
        utilities = Nothing
        astroUtilities.Dispose()
        astroUtilities = Nothing
    End Sub

#End Region

#Region "private properties and methods"
    ' here are some useful properties and methods that can be used as required
    ' to help with

    ''' <summary>
    ''' Returns true if there is a valid connection to the driver hardware
    ''' </summary>
    Private ReadOnly Property IsConnected As Boolean
        Get
            ' TODO check that the driver hardware connection exists and is connected to the hardware
            Return connectedState
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

#End Region

    '//INTERFACECODEINSERTIONPOINT
End Class