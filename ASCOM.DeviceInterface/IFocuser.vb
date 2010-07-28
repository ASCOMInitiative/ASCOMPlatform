Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IFocuser interface.</summary>
'-----------------------------------------------------------------------

''' <summary>
''' Provides universal access to Focuser drivers
''' </summary>
<Guid("E430C8A8-539E-4558-895D-A2F293D946E7"), ComVisible(True)> _
Public Interface IFocuser 'C2E3FE9C-01CD-440C-B8E3-C56EE9E4EDBC
    Inherits IAscomDriver
    Inherits IDeviceControl
    ''' <summary>
    ''' True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
    ''' </summary>
    ReadOnly Property Absolute() As Boolean

    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

    ''' <summary>
    ''' Immediately stop any focuser motion due to a previous Move() method call.
    ''' Some focusers may not support this function, in which case an exception will be raised. 
    ''' Recommendation: Host software should call this method upon initialization and,
    ''' if it fails, disable the Halt button in the user interface. 
    ''' </summary>
    Sub Halt()

    ''' <summary>
    ''' True if the focuser is currently moving to a new position. False if the focuser is stationary.
    ''' </summary>
    ReadOnly Property IsMoving() As Boolean

    ''' <summary>
    ''' State of the connection to the focuser.
    ''' et True to start the link to the focuser; set False to terminate the link. 
    ''' The current link status can also be read back as this property. 
    ''' An exception will be raised if the link fails to change state for any reason. 
    ''' </summary>
    Property Link() As Boolean

    ''' <summary>
    ''' Maximum increment size allowed by the focuser; 
    ''' i.e. the maximum number of steps allowed in one move operation.
    ''' For most focusers this is the same as the MaxStep property.
    ''' This is normally used to limit the Increment display in the host software. 
    ''' </summary>
    ReadOnly Property MaxIncrement() As Integer

    ''' <summary>
    ''' Maximum step position permitted.
    ''' The focuser can step between 0 and MaxStep. 
    ''' If an attempt is made to move the focuser beyond these limits,
    ''' it will automatically stop at the limit. 
    ''' </summary>
    ReadOnly Property MaxStep() As Integer

    ''' <summary>
    ''' Step size (microns) for the focuser.
    ''' Raises an exception if the focuser does not intrinsically know what the step size is. 
    ''' </summary>
    ''' <param name="val"></param>
    Sub Move(ByVal val As Integer)

    ''' <summary>
    ''' Current focuser position, in steps.
    ''' Valid only for absolute positioning focusers (see the Absolute property).
    ''' An exception will be raised for relative positioning focusers.   
    ''' </summary>
    ReadOnly Property Position() As Integer

    ''' <summary>
    ''' Step size (microns) for the focuser.
    ''' Raises an exception if the focuser does not intrinsically know what the step size is. 
    ''' 
    ''' </summary>
    ReadOnly Property StepSize() As Double

    ''' <summary>
    ''' The state of temperature compensation mode (if available), else always False.
    ''' If the TempCompAvailable property is True, then setting TempComp to True
    ''' puts the focuser into temperature tracking mode. While in temperature tracking mode,
    ''' Move commands will be rejected by the focuser. Set to False to turn off temperature tracking.
    ''' An exception will be raised if TempCompAvailable is False and an attempt is made to set TempComp to true. 
    ''' 
    ''' </summary>
    Property TempComp() As Boolean

    ''' <summary>
    ''' True if focuser has temperature compensation available.
    ''' Will be True only if the focuser's temperature compensation can be turned on and off via the TempComp property. 
    ''' </summary>
    ReadOnly Property TempCompAvailable() As Boolean

    ''' <summary>
    ''' Current ambient temperature as measured by the focuser.
    ''' Raises an exception if ambient temperature is not available.
    ''' Commonly available on focusers with a built-in temperature compensation mode. 
    ''' </summary>
    ReadOnly Property Temperature() As Double
End Interface