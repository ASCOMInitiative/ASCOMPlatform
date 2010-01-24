using System;
namespace ASCOM.SwitchSimulator
{
    interface ISwitch
    {
        string AuthorEmail { get; }
        string AuthorName { get; }
        int Count();
        string Description { get; }
        string DriverInfo { get; set; }
        string DriverName { get; }
        string DriverVersion { get; }
        SwitchDevice GetSwitchDevice(int i);
        System.Collections.ArrayList GetSwitchDevices();
        bool IsConnected { get; set; }
        void SetSwitchOff(int i);
        void SetSwitchOn(int i);
        void SetupDialog();
    }
}
