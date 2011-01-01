Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the ITrackingRates Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the ITrackingRates Interface
''' </summary>
<ComVisible(True), Guid("35C65270-9582-410d-93CB-A660C5C99D9D"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface ITrackingRates ' DC98F1DF-315A-43ef-81F6-23F3DD461F58 ', InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
    Default ReadOnly Property Item(ByVal index As Integer) As ASCOM.DeviceInterface.DriveRates
    ReadOnly Property Count() As Integer

    'IEnumerable
    Function GetEnumerator() As System.Collections.IEnumerator

    'IEnumerator
    'Function MoveNext() As Boolean
    'ReadOnly Property Current() As Object
    'Sub Reset()

    'IDisposable
    Sub Dispose()

End Interface