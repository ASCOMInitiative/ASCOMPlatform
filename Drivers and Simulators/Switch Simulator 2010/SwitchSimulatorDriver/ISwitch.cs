using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.SwitchSimulator
{
    public interface ISwitch
    {
        byte Id { get; set; }
        string Name { get; set; }
        bool State { get; set; }
    }
}
