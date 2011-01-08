Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IRate Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Describes a range of rates supported by the MoveAxis() method (degrees/per second)
''' These are contained within the AxisRates collection. They serve to describe one or more supported ranges of rates of motion about a mechanical axis. 
''' It is possible that the Rate.Maximum and Rate.Minimum properties will be equal. In this case, the Rate object expresses a single discrete rate. 
''' Both the Rate.Maximum and Rate.Minimum properties are always expressed in units of degrees per second. 
''' </summary>
<ComVisible(True), Guid("2E7CEEE4-B5C6-4e9a-87F4-80445700D123"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IRate '221C0BC0-110B-4129-85A0-18BB28579290
    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

    ''' <summary>
    ''' The maximum rate (degrees per second)
    ''' This must always be a positive number. It indicates the maximum rate in either direction about the axis. 
    ''' </summary>
    Property Maximum() As Double

    ''' <summary>
    ''' The minimum rate (degrees per second)
    ''' This must always be a positive number. It indicates the maximum rate in either direction about the axis. 
    ''' </summary>
    Property Minimum() As Double
End Interface