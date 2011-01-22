Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IRheostat Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' The Rheostat switch is used to create a switch with an upper limit int and it's current int setting.  
''' Used with the switch object it will return in an arraylist as a collection of switches
''' </summary>
<ComVisible(True), Guid("9DD2CBDB-D114-4576-8072-4ED79CD3571A"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IRheostat
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
    ''' First parameter is the current min 
    ''' Second Parameter is the max
    ''' Third Parameter is the level set (must be between min and max)
    ''' </summary>
    ReadOnly Property State() As String()

End Interface
