using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwitchDriverAccess
{
    public class Switch : SwitchDriverAccess.ISwitch
    {
        List<SwitchDevice> switchList = new List<SwitchDevice>();

        #region Properties
        public bool Connected { get; set; }
        public string Description { get { return "description"; } }
        public string DriverInfo { get { return "Drivver Info"; } }
        public string DriverVersion { get { return "1.1"; } }
        public short InterfaceVersion { get { return 1; } }
        public string Name { get { return "Name"; } }
        public SwitchDevice SwitchDevice { get { return SwitchDevice; } }
        #endregion

        #region Methods
        public Switch()
        {
            SwitchDevice s = new SwitchDevice();
            s.Name = "test";
            s.Id = 1;
            s.State = false;
            switchList.Add(s);
        }
        public List<SwitchDevice> GetSwitches() {
            return switchList;
        }
        public SwitchDevice GetSwitch(short Id)
        {
            throw new Exception("SwitchDevice");
        }
         public SwitchDevice GetSwitch(string name)
        {
            throw new Exception("SwitchDevice");
        }
        public bool GetSwitchState(short Id, string name) 
        {
            throw new Exception("GetSwitchState");
        }
        public bool SetSwitchState(short Id, string name) 
        {
            throw new Exception("SetSwitchState");
        }
        public void Dispose() 
        {
            throw new Exception("Dispose");
        }
        #endregion
    }

    public class SwitchDevice
    {
       public short Id { get; set; }
       public string Name { get; set; }
       public bool State { get; set; }
    }

}
