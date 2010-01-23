using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.SwitchSimulator
{
    public interface IDriver : IAuthor, ISwitch, ISwitches
    {
        bool Connected { get; set; }
        string DriverName { get; set; }
        string Information { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        ushort InterfaceVersion { get; set; }

        void SetupDialog();

    }
}
