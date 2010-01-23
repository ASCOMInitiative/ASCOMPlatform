using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.SwitchSimulator
{
    public class SwitchHardware
    {
        private static bool connected;   //tracking state
        private static string authorName;
        private static string authorEmail;
        private static string description;
        private static string driverInfo;
        private static string driverVersion;
        private static ushort interfaceVersion;
        private static string name;
        private static byte id;
        private static bool state;

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
        public static ushort InterfaceVersion
        {
            get { return interfaceVersion; }
            set { interfaceVersion = value; }
        }
        public static string Name
        {
            get { return name; }
            set { name = value; }
        }
        public static byte Id
        {
            get { return id; }
            set { id = value; }
        }
        public static bool State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
