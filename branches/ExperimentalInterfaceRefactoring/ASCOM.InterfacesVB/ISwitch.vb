'-----------------------------------------------------------------------
' <summary>Defines the ISwitch Interface</summary>
'-----------------------------------------------------------------------
Imports System.Runtime.InteropServices
Imports System.Collections
''' <summary>
''' Defines the ISwitch Interface
''' </summary> 
<Guid("E15267DD-E5EB-4a2c-A26E-56B68996A105"), ComVisible(True)> _
Public Interface ISwitch '44C03033-C60E-4101-856C-AAFB0F735F83
    Inherits IDeviceControl
    Inherits IAscomDriver
    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

    ''' <summary>
    ''' Yields a collection of ISwitchDevice objects.
    ''' </summary>
    ReadOnly Property Switches() As ArrayList

    ''' <summary>
    ''' Sets a switch to on or off
    ''' </summary>
    ''' <param name="Name">Name=name of switch to set</param> 
    ''' <param name="State">True=On, False=Off</param> 
    Sub SetSwitch(ByVal Name As String, ByVal State As Boolean)
End Interface