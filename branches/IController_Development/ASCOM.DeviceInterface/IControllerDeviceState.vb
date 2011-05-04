Imports System.Runtime.InteropServices

''' <summary>
''' A simple class relating a Value range to its descriptive name. 
''' 
''' Digital devices should return the same value in both ValueStart and ValueEnd to indicate that this name applies just to that discrete Value.
''' 
''' All devices should return descriptions for their MINIMUM and MAXIMUM Values. Other entries are optional.
''' 
''' It is not mandatory to have a description for every state, only MINIMUM and MAXIMUM are mandatory. This opens the way to a 
''' digital rheostat device that could have a thousand states most of which don't have special names. 
''' 
''' For analogue devices this could return a description such as "Quarter Power" for a range of values such as when the value is 
''' in the range 1-25% of max.
''' </summary>
<ComVisible(True), Guid("F92B5110-57EA-4B9E-8C3F-3A45F25B54DE"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IControllerDeviceState
    ''' <summary>
    ''' MANDATORY - Starting value of this named range
    ''' </summary>
    ReadOnly Property StartValue() As Double

    ''' <summary>
    ''' MANDATORY - Ending value of this named range. Can be the same as StartValue to create a single valued state
    ''' </summary>
    ReadOnly Property EndValue() As Double

    ''' <summary>
    ''' MANDATORY - Name of this state or range
    ''' </summary>
    ReadOnly Property Name() As String
End Interface
