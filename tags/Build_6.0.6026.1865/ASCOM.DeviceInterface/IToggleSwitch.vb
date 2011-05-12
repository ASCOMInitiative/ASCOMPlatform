Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IToggleSwitch Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' The toggle switch is used to create a single on/off switch.  Used with the switch object it will return in an arraylist
''' as a collection of switches
''' </summary>
<ComVisible(True), Guid("694A65AE-7DB0-4BE6-99CA-C319AC044FAF"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IToggleSwitch '694A65AE-7DB0-4BE6-99CA-C319AC044FAF
    ''' <summary>
    ''' String representing the type of switch
    ''' </summary>
    ReadOnly Property DeviceType() As String

    ''' <summary>
    ''' String representing the name of the switch
    ''' </summary>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' String representing the state of the switch, can be On or Off
    ''' First parameter is either On or OFF, string
    ''' </summary>
    ReadOnly Property State() As String()

End Interface