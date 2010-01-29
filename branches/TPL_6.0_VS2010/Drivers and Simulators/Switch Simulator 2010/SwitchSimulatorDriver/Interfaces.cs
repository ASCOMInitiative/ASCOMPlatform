using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ASCOM.Interface
{
    /// <summary>
    /// Defines the interface for an ASCOM Switch Driver, which exposes
    /// a collection of switches.
    /// </summary>
    public interface ISwitch
    {
        bool Connected { get; set; }
        string Description { get; }
        string DriverInfo { get; }
        string DriverVersion { get; }
        short InterfaceVersion { get; }
        string Name { get; }
        void SetupDialog();
        /// <summary>
        /// Yields a collection of ISwitchController objects.
        /// </summary>
        ArrayList SwitchCollection { get; }
    }

    /// <summary>
    /// Defines the interface for an individual switch object.
    /// </summary>
    public interface ISwitchDevice
    {
        /// <summary>
        /// The name of the switch (human readable).
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Sets the state of the switch. True = on, False = off.
        /// </summary>
        bool State { get; set; }
    }
}
