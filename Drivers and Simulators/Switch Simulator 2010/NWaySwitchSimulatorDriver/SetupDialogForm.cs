using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        #region Constants

        private readonly ArrayList _switches;
        private INWaySwitch _s1;
        private INWaySwitch _s2;

        #endregion

        #region Constructor

        public SetupDialogForm()
        {
            InitializeComponent();
            _switches = Switches.Switches;
            _s1 = (INWaySwitch)_switches[0];
            _s2 = (INWaySwitch)_switches[1];
            SetGuage1(Convert.ToInt16(_s1.State[2]));
            SetGuage2(Convert.ToInt16(_s2.State[2]));
            label1.Text = Switches.Name + @" v" + Switches.DriverVersion;
            label2.Text = _s1.Name;
            label3.Text = _s2.Name;
            aGauge1.MinValue = Convert.ToSingle(_s1.State[0]);
            aGauge2.MinValue = Convert.ToSingle(_s2.State[0]);
            aGauge1.MaxValue = Convert.ToSingle(_s1.State[1]);
            aGauge2.MaxValue = Convert.ToSingle(_s2.State[1]);
        }

        #endregion

        #region Private Members

        private void SetGuage1(int value)
        {
            _s1.State[2] = value.ToString();
            Switches.SetSwitch(_s1.Name, _s1.State);
            _s1 = (INWaySwitch)Switches.GetSwitch(_s1.Name);
            aGauge1.Value = Convert.ToSingle(_s1.State[2]);
        }

        private void SetGuage2(int value)
        {
            _s2.State[2] = value.ToString();
            Switches.SetSwitch(_s2.Name, _s2.State);
            _s2 = (INWaySwitch)Switches.GetSwitch(_s2.Name);
            aGauge2.Value = Convert.ToSingle(_s2.State[2]);
        }

        private void But1Click(object sender, EventArgs e)
        {
            SetGuage1(Convert.ToInt16(but1.Text));
        }

        private void But2Click(object sender, EventArgs e)
        {
            SetGuage1(Convert.ToInt16(but2.Text));
        }

        private void But3Click(object sender, EventArgs e)
        {
            SetGuage1(Convert.ToInt16(but3.Text));
        }

        private void But4Click(object sender, EventArgs e)
        {
            SetGuage1(Convert.ToInt16(but4.Text));
        }

        private void But5Click(object sender, EventArgs e)
        {
            SetGuage1(Convert.ToInt16(but5.Text));
        }

        private void But6Click(object sender, EventArgs e)
        {
            SetGuage1(Convert.ToInt16(but6.Text));
        }

        private void Button7Click(object sender, EventArgs e)
        {
            SetGuage2(Convert.ToInt16(button7.Text));
        }

        private void Button6Click(object sender, EventArgs e)
        {
            SetGuage2(Convert.ToInt16(button6.Text));
        }

        private void Button5Click(object sender, EventArgs e)
        {
            SetGuage2(Convert.ToInt16(button5.Text));
        }

        private void Button4Click(object sender, EventArgs e)
        {
            SetGuage2(Convert.ToInt16(button4.Text));
        }

        private void Button3Click(object sender, EventArgs e)
        {
            SetGuage2(Convert.ToInt16(button3.Text));
        }

        private void Button2Click(object sender, EventArgs e)
        {
            SetGuage2(Convert.ToInt16(button2.Text));
        }

        private static void BrowseToAscom(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start("http://ascom-standards.org/");
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        #endregion

    }
}
