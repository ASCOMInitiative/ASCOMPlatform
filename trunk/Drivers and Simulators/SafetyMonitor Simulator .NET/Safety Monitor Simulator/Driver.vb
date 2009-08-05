Imports ASCOM.Utilities
Imports System.Reflection
Imports ASCOM.Interface
'tabs=4
' --------------------------------------------------------------------------------
'
' Description:	Safety Monitor Simulator first release 
'
' Implements:	ASCOM SafetyMonitor interface version: 1.0
' Author:		(PWGS) Peter Simpson<simpsons@nildram.co.uk>
'
' Edit Log:
'
' Date			Who	 Vers	Description
' -----------	---	 -----	-------------------------------------------------------
' 14-04-2009	PWGS 1.0.0	Initial edit
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.Simulator.SafetyMonitor
'
' The Guid attribute sets the CLSID for ASCOM.Test2.SafetyMonitor
' The ClassInterface/None addribute prevents an empty interface called
' _SafetyMonitor from being created and used as the [default] interface
'
#Region "ISafetyMonitorNET Interface Definition"
Interface ISafetyMonitorNET
    ReadOnly Property CanEmergencyShutdown() As Boolean
    ReadOnly Property CanIsGood() As Boolean
    Property Connected() As Boolean
    ReadOnly Property Description() As String
    ReadOnly Property DriverInfo() As String
    ReadOnly Property DriverVersion() As String
    ReadOnly Property EmergencyShutdown() As Boolean
    ReadOnly Property IsSafe() As Boolean
    ReadOnly Property IsGood() As Boolean
    Sub SetUpDialog()
End Interface
#End Region

'Specification from ASCOM-Talk
'
'Boolean IsSafe
' Required, read-only
' True when the Safety Monitor has determined that conditions are safe
' to operate. False when unsafe conditions exist.
' Returns False when not connected
' Returns False when EmergencyShutdown is True
' Can throw exceptions under fault conditions
'
'Boolean CanIsGood
' Required, read-only
' True when the driver implements the IsGood property
' Does not throw exceptions
'
'Boolean IsGood
' Optional, read-only
' True when the Safety Monitor has determined that conditions are
' "good" (driver/user defined). Good conditions are always Safe.
' Not Good conditions can be Safe.
' Returns False when not connected
' Returns False when EmergencyShutdown is True
' Throws a "Not Implemented" exception if CanIsGood is False
' Can throw exceptions under fault conditions
'
'Boolean CanEmergencyShutdown
' Required, read-only
' True when the driver implements the IsEmergencyShutdown property
' This must be set false if the safety monitor cannot perform an
' autonomous emergency shutdown either by lack of ability or lack
' of proper connections.
' Does not throw exceptions

'Boolean EmergencyShutdown
' Optional, read-only
' True when the Safety Monitor has performed an Emergency Shutdown
' autonomously from the PC due to an unsafe condition. This value
' must remain true until the emergency shutdown is no longer being
' forced upon the observatory hardware.
' Returns False when not connected
' Throws a "Not Implemented" exception if CanEmergencyShutdown is False
' Can throw exceptions under fault conditions
'
'Additional Properties and Methods
'
'Boolean Connected(Boolean)
' Required, read-write
' Returns True when all controlled devices are on-line, otherwise returns false
' Write:True connects the driver to the associated devices
' Write:False disconnects the driver from the associated devices
' Throws an exception if any device(s) fail to come on-line correctly
'
'String Description
' Required, read-only
' Description of the controlled device(s), such as manufacturer and model number
' Does not throw exceptions
'
'String DriverInfo
' Required, read-only
' Description, author and version information of the ASCOM Safety driver itself
' Does not throw exceptions
'
'String DriverVersion
' Required, read-only
' String containing only the major and minor version of the driver.
' This must be in the form "n.n".
' Does not throw exceptions
'
'SetupDialogue
' Required method
' Displays a setup dialog, allowing the user to configure the safety driver
' Does not throw exceptions

<Guid("C7A1DA6D-91A4-4165-A647-1A40E2D4D5F8")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class SafetyMonitor
    '	==========
    Implements ISafetyMonitor   ' Early-bind interface implemented by this driver

    ' Driver ID and descriptive string that shows in the Chooser - moved to GlobalVars module

#Region "New and Dispose"
    ' Constructor - Must be public for COM registration!
    Public Sub New()
        Dim Prof As Interfaces.IProfile = New Profile
        Try 'Read the CanEmergencyShutdown state from the profile
            Prof.DeviceType = SAFETY_MONITOR
            g_CanEmergencyShutdown = CBool(Prof.GetValue(s_csDriverID, CAN_EMERGENCY_SHUTDOWN))
        Catch ex As InvalidCastException 'Value has not been set yet so default it to false
            g_CanEmergencyShutdown = False
            Prof.WriteValue(s_csDriverID, CAN_EMERGENCY_SHUTDOWN, g_CanEmergencyShutdown.ToString)
        Catch ex As Exception 'Something bad happened
            MsgBox("SafetyMonitor:New exception -  " & ex.ToString)
        End Try

        Try
            g_CanIsGood = CBool(Prof.GetValue(s_csDriverID, CAN_IS_GOOD))
        Catch ex As InvalidCastException 'Value has not been set yet so default it to false
            g_CanIsGood = False
            Prof.WriteValue(s_csDriverID, CAN_IS_GOOD, g_CanIsGood.ToString)
        Catch ex As Exception 'Something bad happened
            MsgBox("SafetyMonitor:New exception -  " & ex.ToString)
        End Try

        g_Connected = False 'Ensure we start in the unconnected state

    End Sub
#End Region

#Region "ASCOM Registration"
    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)
        Dim P As ASCOM.Utilities.Interfaces.IProfile = New Profile() With {.DeviceType = "SafetyMonitor"}
        If bRegister Then
            P.Register(s_csDriverID, s_csDriverDescription)
        Else
            P.Unregister(s_csDriverID)
        End If

        P.Dispose()
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

    ' PUBLIC COM INTERFACE ISafetyMonitor IMPLEMENTATION

#Region "ISafetyMonitor members"
    Public ReadOnly Property CanEmergencyShutdown() As Boolean Implements ISafetyMonitor.CanEmergencyShutdown
        Get
            Return g_CanEmergencyShutdown 'Return the CanEmergencyShutdown value
        End Get
    End Property

    Public ReadOnly Property CanIsGood() As Boolean Implements ISafetyMonitor.CanIsGood
        Get
            Return g_CanIsGood 'Return the CanIsGood value
        End Get
    End Property

    Public Property Connected() As Boolean Implements ISafetyMonitor.Connected
        Get
            Return g_Connected 'Return the connected state
        End Get
        Set(ByVal Value As Boolean)
            g_Connected = Value 'Save the connected state
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ISafetyMonitor.Description
        Get
            Return "ASCOM Safety Monitor software simulation"
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements ISafetyMonitor.DriverInfo
        Get
            Return "ASCOM Safety Monitor simulator driver by Peter Simpson" & vbCrLf & "Version " & Assembly.GetExecutingAssembly.GetName.Version.ToString
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ISafetyMonitor.DriverVersion
        Get
            Dim Ver As Version = Assembly.GetExecutingAssembly.GetName.Version 'Get this assembly's version
            Return Ver.Major & "." & Ver.Minor 'Display the relevant portions of the version
        End Get
    End Property

    Public ReadOnly Property EmergencyShutdown() As Boolean Implements ISafetyMonitor.EmergencyShutdown
        Get 'Either return the value false or throw exception depending on whether the simulator is configured to support this option
            If g_CanEmergencyShutdown Then
                Return False
            Else
                Throw New ASCOM.PropertyNotImplementedException("EmergencyShutdown", False)
            End If
        End Get
    End Property

    Public ReadOnly Property IsSafe() As Boolean Implements ISafetyMonitor.IsSafe
        Get
            If g_Connected Then
                If g_CanEmergencyShutdown Then 'We are connected so check if we can emergency shutdown 
                    If EmergencyShutdown Then 'Return false if emergency shutdown is true
                        Return False
                    Else
                        Return True 'Return true if connected and not emergency shutdown
                    End If
                Else 'Can't emergency shutdown so return true
                    Return True
                End If
            Else 'Must return false if we are not connected
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property IsGood() As Boolean Implements ISafetyMonitor.IsGood
        Get
            If g_CanIsGood Then
                If g_Connected Then
                    If g_CanEmergencyShutdown Then
                        If EmergencyShutdown Then
                            Return False
                        Else
                            Return True
                        End If
                    Else
                        Return True
                    End If
                End If
            Else
                Throw New ASCOM.PropertyNotImplementedException("IsGood", False)
            End If
        End Get
    End Property

    Public Sub SetupDialog() Implements ISafetyMonitor.SetupDialog
        Dim F As SetupDialogForm = New SetupDialogForm()
        F.ShowDialog() 'Show the configuration dialogue
        F.Dispose()
        F = Nothing
    End Sub

#End Region

End Class
