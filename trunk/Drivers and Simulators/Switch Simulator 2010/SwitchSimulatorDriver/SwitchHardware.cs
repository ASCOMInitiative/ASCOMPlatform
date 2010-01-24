using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ASCOM.SwitchSimulator
{
    class SwitchHardware: SwitchDevice
    {
        private static bool isConnected;   //tracking state
        static ArrayList switchDevices = new ArrayList();

        public static bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value;
            }
        }

        public static void Add(SwitchDevice switchDevice)
        {
            switchDevices.Add(switchDevice);
        }
        public static void Remove(SwitchDevice switchDevice)
        {
            switchDevices.Remove(switchDevice);
        }
        public static SwitchDevice GetSwitchDevice(int i)
        {
            return (SwitchDevice)switchDevices[i];
        }
        public static ArrayList GetSwitchDevices()
        {
            return switchDevices;
        }
        public static int Count()
        {
            return switchDevices.Count;
        }
        public void SetSwitchOn(int i)
        {
        }
        public void SetSwitchOff(int i)
        {
        }
    }
}
