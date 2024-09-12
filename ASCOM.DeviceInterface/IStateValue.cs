using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Interface definition for DeviceState objects
    /// </summary>
    /// <remarks>
    /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on how to implement DeviceState, which properties to include, and the implementation support provided by the Platform.</para>
    /// </remarks>
    [Guid("5300D26E-1C7D-4541-B2DA-8AF5F57E183D")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IStateValue
    {
        /// <summary>
        /// Property name with casing that must match the casing in the relevant interface definition
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Property value
        /// </summary>
        object Value { get; }
    }
}
