Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IAxisRates Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Determine the rates at which the telescope may be moved about the specified axis by the MoveAxis() method.
''' This is only used if the telescope interface version is 2 or 3
''' </summary>
''' <remarks><para>See the description of MoveAxis() for more information.</para>
''' <para>This method must return an empty collection if MoveAxis is not supported.</para></remarks>
<ComVisible(True), Guid("E39480E6-9DBB-466e-9AA4-9D1B1EA8F849"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IAxisRates ' 2B8FD76E-AF7E-4faa-9FAC-4029E96129F4
    ''' <summary>
    ''' Return information about the rates at which the telescope may be moved about the specified axis by the MoveAxis() method.
    ''' </summary>
    ''' <param name="index">The axis about which rate information is desired</param>
    ''' <value>Collection of Rate objects describing the supported rates of motion that can be supplied to the MoveAxis() method for the specified axis.</value>
    ''' <returns>Collection of Rate objects </returns>
    ''' <remarks><para>The (symbolic) values for Index (TelescopeAxes) are:</para>
    ''' <bl>
    ''' <li>axisPrimary 0 Primary axis (e.g., Right Ascension or Azimuth)</li>
    ''' <li>axisSecondary 1 Secondary axis (e.g., Declination or Altitude)</li>
    ''' <li>axisTertiary 2 Tertiary axis (e.g. imager rotator/de-rotator)</li> 
    ''' </bl>
    ''' </remarks>
    Default ReadOnly Property Item(ByVal index As Integer) As IRate
    ''' <summary>
    ''' Number of items in the returned collection
    ''' </summary>
    ''' <value>Number of items</value>
    ''' <returns>Integer number of items</returns>
    ''' <remarks></remarks>
    ReadOnly Property Count() As Integer
    ''' <summary>
    ''' Disposes of the object and cleans up
    ''' </summary>
    ''' <remarks></remarks>
    Sub Dispose()
    ''' <summary>
    ''' Returns an enumerator for the collection
    ''' </summary>
    ''' <returns>An enumerator</returns>
    ''' <remarks></remarks>
    Function GetEnumerator() As System.Collections.IEnumerator
End Interface