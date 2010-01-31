using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
    /// <summary>
    /// Defines the interface for an ASCOM Switch Driver, which exposes
    /// a collection of switches.
    /// </summary>
    [Guid("7E47D97E-1A41-4897-BAB2-DEA9AD33A9CC")]
    public interface ISwitch
    {
        /// <summary>
        /// Returns the connected state of the driver
        /// </summary>
        bool Connected { get; set; }
        /// <summary>
        /// Returns the description of the driver
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Returns the information on the driver
        /// </summary>
        string DriverInfo { get; }
        /// <summary>
        /// Returns the driver version
        /// </summary>
        string DriverVersion { get; }
        /// <summary>
        /// Returns the interface version
        /// </summary>
        short InterfaceVersion { get; }
        /// <summary>
        /// Returns the name of the driver
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Run the setup form for the driver
        /// </summary>
        void SetupDialog();
        /// <summary>
        /// Yields a collection of ISwitchDevices objects.
        /// </summary>
        ArrayList SwitchDevices { get; }
    }

    /// <summary>
    /// Defines the interface for an individual switch object.
    /// </summary>
    [Guid("BC8CCD51-A20A-49cf-9C35-9F75E3E2354F")]
    public interface ISwitchDevice
    {
        /// <summary>
        /// The name of the switch (human readable).
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Returns True = on, False = off.
        /// </summary>
        bool State { get; set; }
    }
}
