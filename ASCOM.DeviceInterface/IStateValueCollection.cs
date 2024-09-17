using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Interface defining collection of IStateValue objects
    /// </summary>
    /// <remarks>
    /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on how to implement DeviceState, which properties to include, and the implementation support provided by the Platform.</para>
    /// </remarks>
    [ComVisible(true)]
    [Guid("E1B91080-6D76-431E-BDBC-A1ECAFB92603")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IStateValueCollection
    {
        /// <summary>
        /// Returns the current StateValue object.
        /// </summary>
        /// <param name="index">The required object index in the collection.</param>
        IStateValue this[int index] { get; }

        /// <summary>
        /// Number of items in the returned collection
        /// </summary>
        /// <value>Number of items</value>
        /// <returns>Integer number of items</returns>
        /// <remarks></remarks>
        int Count { get; }
        
        /// <summary>
        /// Returns an enumerator for the collection
        /// </summary>
        /// <returns>An enumerator</returns>
        /// <remarks></remarks>
        IEnumerator GetEnumerator();
    }
}
