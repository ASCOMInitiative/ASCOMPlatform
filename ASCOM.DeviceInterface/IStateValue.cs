using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Interface definition for DeviceState objects
    /// </summary>
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
