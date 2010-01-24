using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.SwitchSimulator
{
    /// <summary>
    /// Summary description for Switch.
    /// </summary>
    public class Switch : SwitchDevice
    {
        public Switch()
        {
        }
        public bool IsConnected
        {
            get
            {
                return SwitchHardware.Connected;
            }
            set
            {
                SwitchHardware.Connected = value;
            }
        }
        public string Description
        {
            get
            {
                return SwitchHardware.Description;
            }
            set
            {
                SwitchHardware.Description = value;
            }
        }
        public string AuthorName
        {
            get { return SwitchHardware.AuthorName; }
            set { SwitchHardware.AuthorName = value; }
        }
        public string AuthorEmail
        {
            get { return SwitchHardware.AuthorEmail; }
            set { SwitchHardware.AuthorEmail = value; }
        }
        public string DriverName
        {
            get { return SwitchHardware.DriverName; }
            set { SwitchHardware.DriverName = value; }
        }
        public string DriverInfo
        {
            get { return SwitchHardware.DriverInfo; }
            set { SwitchHardware.DriverInfo = value; }
        }
        public string DriverVersion
        {
            get { return SwitchHardware.DriverVersion; }
            set { SwitchHardware.DriverVersion = value; }
        }

        public void Add(SwitchDevice switchDevice)
        {
            SwitchHardware.Add(switchDevice);
        }
        public void Remove(SwitchDevice switchDevice)
        {
            SwitchHardware.Remove(switchDevice);
        }
        public SwitchDevice GetSwitchDevice(int i)
        {
            return SwitchHardware.GetSwitchDevice(i);
        }
        public ArrayList GetSwitchDevices()
        {
            return SwitchHardware.GetSwitchDevices();
        }
        public int Count()
        {
            return SwitchHardware.Count();
        }
    }
}
