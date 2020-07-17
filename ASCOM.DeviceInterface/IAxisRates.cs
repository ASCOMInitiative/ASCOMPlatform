using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    // -----------------------------------------------------------------------
    // <summary>Defines the IAxisRates Interface</summary>
    // -----------------------------------------------------------------------
    /// <summary>
    /// A collection of rates at which the telescope may be moved about the specified axis by the <see cref="ITelescopeV3.MoveAxis" /> method.
    /// This is only used if the telescope interface version is 2 or 3
    /// </summary>
    /// <remarks><para>See the description of the <see cref="ITelescopeV3.MoveAxis" /> method for more information.</para>
    /// <para>This method must return an empty collection if <see cref="ITelescopeV3.MoveAxis" /> is not supported.</para>
    /// <para>The values used in <see cref="IRate" /> members must be non-negative; forward and backward motion is achieved by the application
    /// applying an appropriate sign to the returned <see cref="IRate" /> values in the <see cref="ITelescopeV3.MoveAxis" /> command.</para>
    /// </remarks>
    [ComVisible(true)]
    [Guid("E39480E6-9DBB-466e-9AA4-9D1B1EA8F849")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAxisRates // 2B8FD76E-AF7E-4faa-9FAC-4029E96129F4
    {
        /// <summary>
        /// Return information about the rates at which the telescope may be moved about the specified axis by the <see cref="ITelescopeV3.MoveAxis" /> method.
        /// </summary>
        /// <param name="index">The axis about which rate information is desired</param>
        /// <value>Collection of Rate objects describing the supported rates of motion that can be supplied to the <see cref="ITelescopeV3.MoveAxis" /> method for the specified axis.</value>
        /// <returns>Collection of Rate objects </returns>
        /// <remarks><para>The (symbolic) values for Index (<see cref="TelescopeAxes" />) are:</para>
        /// <bl>
        /// <li><see cref="TelescopeAxes.axisPrimary"/> 0 Primary axis (e.g., Hour Angle or Azimuth)</li>
        /// <li><see cref="TelescopeAxes.axisSecondary"/> 1 Secondary axis (e.g., Declination or Altitude)</li>
        /// <li><see cref="TelescopeAxes.axisTertiary"/> 2 Tertiary axis (e.g. imager rotator/de-rotator)</li>
        /// </bl>
        /// </remarks>
        IRate this[int index] { get; }
        /// <summary>
        /// Number of items in the returned collection
        /// </summary>
        /// <value>Number of items</value>
        /// <returns>Integer number of items</returns>
        /// <remarks></remarks>
        int Count { get; }
        /// <summary>
        /// Disposes of the object and cleans up
        /// </summary>
        /// <remarks></remarks>
        void Dispose();
        /// <summary>
        /// Returns an enumerator for the collection
        /// </summary>
        /// <returns>An enumerator</returns>
        /// <remarks></remarks>
        IEnumerator GetEnumerator();
    }
}
