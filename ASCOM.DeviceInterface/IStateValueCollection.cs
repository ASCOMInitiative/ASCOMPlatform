using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Interface defining collection of IStateValue objects
    /// </summary>
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

        ///// <summary>
        ///// Disposes of the object and cleans up
        ///// </summary>
        ///// <remarks></remarks>
        //void Dispose();
    }
}
