Imports System.Runtime.InteropServices

'-----------------------------------------------------------------------
' <summary>Defines the IFilterWheel Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the IFilterWheel Interface
''' </summary>
<Guid("DCF3858D-D68E-45ed-8141-1C899B4B432A"), ComVisible(True)> _
Public Interface IFilterWheel '756FD725-A6E2-436F-8C7A-67E358622027
    Inherits IAscomDriver
    Inherits IDeviceControl
    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

    ''' <summary>
    ''' For each valid slot number (from 0 to N-1), reports the focus offset for
    ''' the given filter position.  These values are focuser- and filter
    ''' -dependent, and  would usually be set up by the user via the SetupDialog.
    ''' The number of slots N can be determined from the length of the array.
    ''' If focuser offsets are not available, then it should report back 0 for all
    ''' array values.
    ''' </summary>
    ReadOnly Property FocusOffsets() As Integer()

    ''' <summary>
    ''' For each valid slot number (from 0 to N-1), reports the name given to the
    ''' filter position.  These names would usually be set up by the user via the
    ''' SetupDialog.  The number of slots N can be determined from the length of
    ''' the array.  If filter names are not available, then it should report back
    ''' "Filter 1", "Filter 2", etc.
    ''' </summary>
    ReadOnly Property Names() As String()

    ''' <summary>
    ''' Write number between 0 and N-1, where N is the number of filter slots (see
    ''' Filter.Names). Starts filter wheel rotation immediately when written*. Reading
    ''' the property gives current slot number (if wheel stationary) or -1 if wheel is
    ''' moving. This is mandatory; valid slot numbers shall not be reported back while
    ''' the filter wheel is rotating past filter positions.
    ''' 
    ''' Note that some filter wheels are built into the camera (one driver, two
    ''' interfaces).  Some cameras may not actually rotate the wheel until the
    ''' exposure is triggered.  In this case, the written value is available
    ''' immediately as the read value, and -1 is never produced.
    ''' </summary>
    Property Position() As Short

End Interface