Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the ITrackingRates Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the ITrackingRates Interface
''' </summary>
<ComVisible(True), Guid("35C65270-9582-410d-93CB-A660C5C99D9D")> _
Public Interface ITrackingRates ' DC98F1DF-315A-43ef-81F6-23F3DD461F58
    Inherits IEnumerable
    ReadOnly Property Count() As Integer
    Sub Dispose()
    'Function GetEnumerator() As System.Collections.IEnumerator
    Default ReadOnly Property Item(ByVal index As Integer) As ASCOM.DeviceInterface.DriveRates
End Interface