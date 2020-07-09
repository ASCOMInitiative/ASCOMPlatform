using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{

    /// <summary>
    /// Returns a collection of supported DriveRate values that describe the permissible values of the TrackingRate property for this telescope type.
    /// </summary>
    [ComVisible(true)]
    [Guid("35C65270-9582-410d-93CB-A660C5C99D9D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ITrackingRates // DC98F1DF-315A-43ef-81F6-23F3DD461F58 ', InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
    {
        /// <summary>
        /// Returns a specified item from the collection
        /// </summary>
        /// <param name="index">Number of the item to return</param>
        /// <value>A collection of supported DriveRate values that describe the permissible values of the TrackingRate property for this telescope type.</value>
        /// <returns>Returns a collection of supported DriveRate values</returns>
        /// <remarks>This is only used by telescope interface versions 2 and 3</remarks>
        DriveRates this[int index] { get; }
        /// <summary>
        /// Number of DriveRates supported by the Telescope
        /// </summary>
        /// <value>Number of DriveRates supported by the Telescope</value>
        /// <returns>Integer count</returns>
        /// <remarks></remarks>
        int Count { get; }

        /// <summary>
        /// Returns an enumerator for the collection
        /// </summary>
        /// <returns>An enumerator</returns>
        /// <remarks></remarks>
        IEnumerator GetEnumerator();

        /// <summary>
        /// Disposes of the TrackingRates object
        /// </summary>
        /// <remarks></remarks>
        void Dispose();
    }
}
