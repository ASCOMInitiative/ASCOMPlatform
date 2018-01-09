'tabs=4
' --------------------------------------------------------------------------------
'
' ASCOM FilterWheel driver for FilterWheelSimulator
'
' Description:	A port of the VB6 ASCOM Filterwheel simulator to VB.Net.
'               Converted and built in Visual Studio 2008.
'               The port leaves some messy code - it could really do with
'               a ground up re-write!
'
' Implements:	ASCOM FilterWheel interface version: 5.1.0
' Author:		Mark Crossley <mark@markcrossley.co.uk>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' 06-Jun-2009	mpc	1.0.0	Initial edit, from FilterWheel template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.FilterWheelSim.FilterWheel  ???
'
' The Guid attribute sets the CLSID for ASCOM.FilterWheelSim.FilterWheel
' The ClassInterface/None addribute prevents an empty interface called
' _FilterWheel from being created and used as the [default] interface
'

<Guid("F9043C88-F6F2-101A-A3C9-08002B2F49FC")>
<ServedClassName("Filter Wheel Simulator [.Net]")>
<ProgId("ASCOM.Simulator.FilterWheel")>
<ComVisible(True), ClassInterface(ClassInterfaceType.None)>
Public Class FilterWheel
    '	==========
    Inherits ReferenceCountedObjectBase
    Implements IFilterWheelV2 ' Early-bind interface implemented by this driver
    '	==========

    Private Const SCODE_NOT_CONNECTED As Integer = vbObjectError + &H402
    Private Const MSG_NOT_CONNECTED As String = "The filter wheel is not connected"

    Private driverId As String = "ASCOM.Simulator.FilterWheel"

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        driverId = Marshal.GenerateProgIdForType(Me.GetType())

        ' Plug in the hardware

    End Sub

    Public Sub Dispose() Implements IFilterWheelV2.Dispose

    End Sub
    '
    ' PUBLIC COM INTERFACE IFilterWheel IMPLEMENTATION
    '
#Region "IAscomDriver"
    Public Property Connected() As Boolean Implements IFilterWheelV2.Connected
        Get
            Connected = SimulatedHardware.Connected
        End Get
        Set(ByVal value As Boolean)
            SimulatedHardware.Connected = value
        End Set
    End Property

    Public ReadOnly Property Description As String Implements IFilterWheelV2.Description
        Get
            Return "Simulator description"
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IFilterWheelV2.DriverInfo
        Get
            Return "ASCOM filter wheel driver simulator"
        End Get
    End Property

    Public ReadOnly Property DriverVersion As String Implements IFilterWheelV2.DriverVersion
        Get
            Return "6.0"
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion As Short Implements IFilterWheelV2.InterfaceVersion
        Get
            Return 2
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IFilterWheelV2.Name
        Get
            Return "Filter Wheel Simulator .NET"
        End Get
    End Property
#End Region

#Region "IDeviceControl"
    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IFilterWheelV2.Action
        Throw New ASCOM.MethodNotImplementedException("Action is not implemented in this driver")
    End Function

    Public ReadOnly Property SupportedActions As ArrayList Implements IFilterWheelV2.SupportedActions
        Get
            Dim actions As New ArrayList   ' declare an empty array list
            Return actions
            'Throw New ASCOM.PropertyNotImplementedException("Actions are not supported by this driver", False)
        End Get
    End Property

    Public Function CommandString(ByVal Cmd As String, Optional ByVal Raw As Boolean = False) As String Implements IFilterWheelV2.CommandString
        If Cmd = "CommandString" Then
            Return "FWCommandString"
        Else
            Return "Bad command: " & Cmd
        End If
    End Function

    Public Function CommandBool(ByVal Cmd As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IFilterWheelV2.CommandBool

        If Cmd = "CommandBool" Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Sub CommandBlind(ByVal Cmd As String, Optional ByVal Raw As Boolean = False) Implements IFilterWheelV2.CommandBlind

    End Sub
#End Region

#Region "IFilterWheel Members"

    Public Property Position() As Short Implements IFilterWheelV2.Position
        Get
            Position = SimulatedHardware.Position
        End Get
        Set(ByVal value As Short)
            SimulatedHardware.Position = value
        End Set
    End Property

    Public ReadOnly Property FocusOffsets() As Integer() Implements IFilterWheelV2.FocusOffsets
        Get
            FocusOffsets = SimulatedHardware.FocusOffsets
        End Get
    End Property

    Public ReadOnly Property Names() As String() Implements IFilterWheelV2.Names
        Get
            Names = SimulatedHardware.FilterNames
        End Get
    End Property

    Public Sub SetupDialog() Implements IFilterWheelV2.SetupDialog
        SimulatedHardware.DoSetup()
    End Sub

#End Region

    '---------------------------------------------------------------------
    '
    ' check_connected() - Raise an error if the focuser is not connected
    '
    '---------------------------------------------------------------------
    Private Sub check_connected()

        If Not SimulatedHardware.Connected Then _
        Throw New DriverException(MSG_NOT_CONNECTED, SCODE_NOT_CONNECTED)

    End Sub

End Class
