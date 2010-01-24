using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.SwitchSimulator
{
    public class SwitchDevice
    {

        public string Name
        {
            get { return SwitchHardware.DriverName; }
            set { SwitchHardware.DriverName = value; }
        }
        public string Description
        {
            get { return SwitchHardware.Description; }
            set { SwitchHardware.Description = value; }
        }
        public bool On
        {
            get { return SwitchHardware.Connected; }
            set { SwitchHardware.Connected = value; }
        }

    }
}
