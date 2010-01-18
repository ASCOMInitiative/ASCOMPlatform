using System;
namespace SwitchDriverAccess
{
    interface ISwitch
    {
        bool Connected { get; set; }
        string Description { get; }
        void Dispose();
        string DriverInfo { get; }
        string DriverVersion { get; }
        SwitchDevice GetSwitch(string name);
        SwitchDevice GetSwitch(short Id);
        System.Collections.Generic.List<SwitchDevice> GetSwitches();
        bool GetSwitchState(short Id, string name);
        short InterfaceVersion { get; }
        string Name { get; }
        bool SetSwitchState(short Id, string name);
        SwitchDevice SwitchDevice { get; }
    }
}
