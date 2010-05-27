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
' Your driver's ID is ASCOM.FilterWheelSim.FilterWheel
'
' The Guid attribute sets the CLSID for ASCOM.FilterWheelSim.FilterWheel
' The ClassInterface/None addribute prevents an empty interface called
' _FilterWheel from being created and used as the [default] interface
'
<Assembly: ServedClassName("Filter Wheel Simulator [.Net]")>    '[TPL] Mark this assembly as something that LocalServer should be interested in.

<Guid("F9043C88-F6F2-101A-A3C9-08002B2F49FC")> _
<ClassInterface(ClassInterfaceType.None)> Public Class FilterWheel
    '	==========
    Inherits ReferenceCountedObjectBase
    Implements IFilterWheel ' Early-bind interface implemented by this driver
    Implements IConform
    '	==========

    Private Const SCODE_NOT_CONNECTED As Integer = vbObjectError + &H402
    Private Const MSG_NOT_CONNECTED As String = "The filter wheel is not connected"

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()

        ' Plug in the hardware

    End Sub


    '
    ' PUBLIC COM INTERFACE IFilterWheel IMPLEMENTATION
    '

#Region "IFilterWheel Members"

    Public Property Connected() As Boolean Implements IFilterWheel.Connected
        Get
            Connected = SimulatedHardware.Connected
        End Get
        Set(ByVal value As Boolean)
            SimulatedHardware.Connected = value
        End Set
    End Property

    Public Property Position() As Short Implements IFilterWheel.Position
        Get
            Position = SimulatedHardware.Position
        End Get
        Set(ByVal value As Short)
            SimulatedHardware.Position = value
        End Set
    End Property

    Public ReadOnly Property FocusOffsets() As Integer() Implements IFilterWheel.FocusOffsets
        Get
            FocusOffsets = SimulatedHardware.FocusOffsets
        End Get
    End Property

    Public ReadOnly Property Names() As String() Implements IFilterWheel.Names
        Get
            Names = SimulatedHardware.FilterNames
        End Get
    End Property

    Public Sub SetupDialog() Implements IFilterWheel.SetupDialog
        SimulatedHardware.DoSetup()
    End Sub

#End Region


#Region "IConform Members"

    Public ReadOnly Property ConformCommands() As ConformCommandStrings Implements IConform.ConformCommands
        Get
            ConformCommands = New ConformCommandStrings()
        End Get
    End Property

    Public ReadOnly Property ConformCommandsRaw() As ConformCommandStrings Implements IConform.ConformCommandsRaw
        Get
            ConformCommandsRaw = New ConformCommandStrings()
        End Get
    End Property

    Public ReadOnly Property ConformErrors() As ConformErrorNumbers Implements IConform.ConformErrors
        Get
            ConformErrors = New ConformErrorNumbers(New Integer() {ErrorCodes.NotImplemented}, _
                                                    New Integer() {ErrorCodes.InvalidValue}, _
                                                    New Integer() {ErrorCodes.ValueNotSet})
        End Get
    End Property

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
