using System;
using System.Collections;
using System.Windows.Forms;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        private static readonly ISwitchV2 Switches = new NWaySwitchDriver();
        private  float _value1;
        private  float _value2;
        private readonly ArrayList _switches;
        private INWaySwitch _s1;
        private INWaySwitch _s2;

        public SetupDialogForm()
        {
            InitializeComponent();
            _switches = Switches.Switches;
            _s1 = (INWaySwitch)_switches[0];
            _s2 = (INWaySwitch)_switches[1];
            aGauge1.Value = Convert.ToSingle(_s1.State[2]);
            aGauge2.Value = Convert.ToSingle(_s2.State[2]);
            _value1 = aGauge1.Value;
            _value1 = aGauge2.Value;

        }

        private void Button2Click(object sender, EventArgs e)
        {
            if (_value1 > 0) _value1--;
            _s1.State[2] = _value1.ToString();
            Switches.SetSwitch(_s1.Name, _s1.State);
            _s1 = (INWaySwitch)Switches.GetSwitch(_s1.Name);
            aGauge1.Value = Convert.ToSingle(_s1.State[2]);
        }
        
        private void Button3Click(object sender, EventArgs e)
        {
            if (_value1 < 6) _value1++;
            _s1.State[2] = _value1.ToString();
            Switches.SetSwitch(_s1.Name, _s1.State);
            _s1 = (INWaySwitch)Switches.GetSwitch(_s1.Name);
            aGauge1.Value = Convert.ToSingle(_s1.State[2]);
        }

        private void Button4Click(object sender, EventArgs e)
        {
            if (_value1 < 6) _value1++;
            _s2.State[2] = _value1.ToString();
            Switches.SetSwitch(_s2.Name, _s2.State);
            _s2 = (INWaySwitch)Switches.GetSwitch(_s2.Name);
            aGauge2.Value = Convert.ToSingle(_s2.State[2]);
        }

        private void Button5Click(object sender, EventArgs e)
        {
            if (_value1 > 0 ) _value1--;
            _s2.State[2] = _value1.ToString();
            Switches.SetSwitch(_s2.Name, _s2.State);
            _s2 = (INWaySwitch)Switches.GetSwitch(_s2.Name);
            aGauge2.Value = Convert.ToSingle(_s2.State[2]);
        }
    }
}
