Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IRheostat Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' The NwaySwitch switch is used to create a switch with an upper and lower limits and a current int setting.  
''' Used with the switch object it will return in an arraylist as a collection of switches
''' </summary>
<ComVisible(True), Guid("B826412C-B56B-439D-86F9-D116E6EB052C"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface INWaySwitch
    ''' <summary>
    ''' String representing the type of switch
    ''' </summary>
    ReadOnly Property DeviceType() As String

    ''' <summary>
    ''' String representing the name of the switch
    ''' </summary>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' String array representing the state of the switch
    ''' (int) First parameter is the current min 
    ''' (int) Second Parameter is the max
    ''' (Int) Third Parameter is the level set (must be between min and max)
    ''' </summary>
    ReadOnly Property State() As String()

End Interface
