Imports System.Runtime.InteropServices

''' <summary>
''' Returns a collection of supported DriveRate values that describe the permissible values of the TrackingRate property for this telescope type.
''' </summary>
<ComVisible(True), Guid("35C65270-9582-410d-93CB-A660C5C99D9D"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface ITrackingRates ' DC98F1DF-315A-43ef-81F6-23F3DD461F58 ', InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
    ''' <summary>
    ''' Returns a specified item from the collection
    ''' </summary>
    ''' <param name="index">Number of the item to return</param>
    ''' <value>A collection of supported DriveRate values that describe the permissible values of the TrackingRate property for this telescope type.</value>
    ''' <returns>Returns a collection of supported DriveRate values</returns>
    ''' <remarks></remarks>
    Default ReadOnly Property Item(ByVal index As Integer) As ASCOM.DeviceInterface.DriveRates
    ''' <summary>
    ''' Number of DriveRates supported by the Telescope
    ''' </summary>
    ''' <value>Number of DriveRates supported by the Telescope</value>
    ''' <returns>Integer count</returns>
    ''' <remarks></remarks>
    ReadOnly Property Count() As Integer

    ''' <summary>
    ''' Returns an enumerator for the collection
    ''' </summary>
    ''' <returns>An enumerator</returns>
    ''' <remarks></remarks>
    Function GetEnumerator() As System.Collections.IEnumerator

    ''' <summary>
    ''' Disposes of the TrackingRates object
    ''' </summary>
    ''' <remarks></remarks>
    Sub Dispose()

End Interface