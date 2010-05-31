using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Defines the interface for an ASCOM Switch Driver, which exposes
    /// a collection of switches.
    /// </summary>
    [Guid("7E47D97E-1A41-4897-BAB2-DEA9AD33A9CC")]
    public interface ISwitch
    {
        #region IAscomDriver
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
        /// Gets the last result.
        /// </summary>
        /// <value>
        /// The result of the last executed action, or <see cref="String.Empty"	/>
        /// if no action has yet been executed.
        /// </value>
        string LastResult { get; }
        /// <summary>
        /// Run the setup form for the driver
        /// </summary>
        void SetupDialog();
        /// <summary>
        /// Yields a collection of ISwitchDevices objects.
        /// </summary>
        #endregion

        /// <summary>
        /// Yields a collection of string[] objects.
        /// </summary>
        /// <value></value>
        ArrayList Switches { get; }

        /// <summary>
        /// Flips a switch on or off
        /// </summary>
        /// <value>name of the switch</value>
        void SetSwitch(string Name, bool State);
    }
}
