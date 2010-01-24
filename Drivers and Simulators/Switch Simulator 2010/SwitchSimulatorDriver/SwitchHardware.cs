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
        private static bool connected;   //tracking state
        private static string authorName = "Rob Morgan";
        private static string authorEmail = "Rob.Morgan.E@Gmail.Com";
        private static string driverName = "ASCOM Switch Simulator Driver";
        private static string description = "ASCOM simulator for the switch driver";
        private static string driverInfo = "Switch Simulator Driver in C#";
        private static string driverVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        static ArrayList switchDevices = new ArrayList();

        public static bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }
        public static string AuthorName
        {
            get { return authorName; }
            set { authorName = value; }
        }
        public static string AuthorEmail
        {
            get { return authorEmail; }
            set { authorEmail = value; }
        }
        public static string DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        public static string Description
        {
            get { return description; }
            set { description = value; }
        }
        public static string DriverInfo
        {
            get { return driverInfo; }
            set { driverInfo = value; }
        }
        public static string DriverVersion
        {
            get { return driverVersion; }
            set { driverVersion = value; }
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
    }
}
