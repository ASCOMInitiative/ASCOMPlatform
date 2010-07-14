Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IRotator Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the IRotator Interface
''' </summary>
<Guid("692FA48C-4A30-4543-8681-DA0733758F11"), ComVisible(True)> _
Public Interface IRotator '"49003324-8DE2-4986-BC7D-4D85E1C4CF6B
    Inherits IAscomDriver
    Inherits IDeviceControl
    ''' <summary>
    ''' Returns True if the Rotator supports the Rotator.Reverse() method.
    ''' </summary>
    ReadOnly Property CanReverse() As Boolean

    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

    ''' <summary>
    ''' Immediately stop any Rotator motion due to a previous Move() or MoveAbsolute() method call.
    ''' </summary>
    Sub Halt()

    ''' <summary>
    ''' True if the Rotator is currently moving to a new position. False if the Rotator is stationary.
    ''' </summary>
    ReadOnly Property IsMoving() As Boolean

    ''' <summary>
    ''' Causes the rotator to move Position degrees relative to the current Position value.
    ''' </summary>
    ''' <param name="Position">Relative position to move in degrees from current Position.</param>
    Sub Move(ByVal Position As Single)

    ''' <summary>
    ''' Causes the rotator to move the absolute position of Position degrees.
    ''' </summary>
    ''' <param name="Position">absolute position in degrees.</param>
    Sub MoveAbsolute(ByVal Position As Single)

    ''' <summary>
    ''' Current instantaneous Rotator position, in degrees.
    ''' </summary>
    ReadOnly Property Position() As Single

    ''' <summary>
    ''' Sets or Returns the rotator’s Reverse state.
    ''' </summary>
    Property Reverse() As Boolean

    ''' <summary>
    ''' The minimum StepSize, in degrees.
    ''' </summary>
    ReadOnly Property StepSize() As Single

    ''' <summary>
    ''' Current Rotator target position, in degrees.
    ''' </summary>
    ReadOnly Property TargetPosition() As Single

End Interface