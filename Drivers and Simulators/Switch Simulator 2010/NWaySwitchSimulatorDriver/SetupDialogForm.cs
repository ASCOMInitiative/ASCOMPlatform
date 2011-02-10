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

            label5.Text = null;
            label10.Text = null;

            UpdateGuage1();
            UpdateGuage2();
            label1.Text = Switches.Name  + @" v" + Switches.DriverVersion;
        }

        #endregion

        #region Private Members

        private void UpdateGuage1()
        {
            label2.Text = _s1.Name;
            var guage1Min = Convert.ToSingle(_s1.State[0]);
            var guage1Max = Convert.ToSingle(_s1.State[1]);
            var guage1Value = Convert.ToInt16(_s1.State[2]);

            aGauge1.MinValue = guage1Min - 1;
            aGauge1.MaxValue = guage1Max;
            aGauge1.Value = guage1Value;

            trackBar1.Minimum = Convert.ToInt16(guage1Min);
            trackBar1.Maximum = Convert.ToInt16(guage1Max);
            trackBar1.Value = guage1Value;

            comboBox1.Text = guage1Min.ToString();
            comboBox2.Text = guage1Max.ToString();
        }

        private void UpdateGuage2()
        {
            label3.Text = _s2.Name;
            var guage2Min = Convert.ToSingle(_s2.State[0]);
            var guage2Max = Convert.ToSingle(_s2.State[1]);
            var guage2Value = Convert.ToInt16(_s2.State[2]);

            aGauge2.MinValue = guage2Min - 1;
            aGauge2.MaxValue = guage2Max;
            aGauge2.Value = guage2Value;

            trackBar2.Minimum = Convert.ToInt16(guage2Min);
            trackBar2.Maximum = Convert.ToInt16(guage2Max);
            trackBar2.Value = guage2Value;

            comboBox3.Text = guage2Min.ToString();
            comboBox4.Text = guage2Max.ToString();
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

        private void Button1Click(object sender, EventArgs e)
        {
            _s1.State[0] = comboBox1.Text;
            _s1.State[1] = comboBox2.Text;
            _s1.State[2] = comboBox1.Text;

            if (Convert.ToInt16(comboBox1.Text) < Convert.ToInt16(comboBox2.Text))
            {
                Switches.SetSwitch(_s1.Name, _s1.State);
                _s1 = (INWaySwitch)Switches.GetSwitch(_s1.Name);
                UpdateGuage1();
                label5.Text = @"Switch settings saved";
            }
            else
            {
                label5.Text = @"Min must be less than Max";
            }
        }

        private void Button8Click(object sender, EventArgs e)
        {

            _s2.State[0] = comboBox3.Text;
            _s2.State[1] = comboBox4.Text;
            _s2.State[2] = comboBox3.Text;

            if (Convert.ToInt16(comboBox3.Text) < Convert.ToInt16(comboBox4.Text))
            {
                Switches.SetSwitch(_s2.Name, _s2.State);
                _s2 = (INWaySwitch)Switches.GetSwitch(_s2.Name);
                UpdateGuage2();
                label10.Text = @"Switch settings saved";
            }
            else
            {
                label10.Text = @"Min must be less than Max";
            }

        }

        private void Timer1Tick(object sender, EventArgs e)
        {
            _s1.State[2] = Convert.ToString(trackBar1.Value);
            aGauge1.Value = trackBar1.Value;

            _s2.State[2] = Convert.ToString(trackBar2.Value);
            aGauge2.Value = trackBar2.Value;
        }

        private void TrackBar1MouseUp(object sender, MouseEventArgs e)
        {
            label5.Text = null;
            Switches.SetSwitch(_s1.Name, _s1.State);
            _s1 = (INWaySwitch)Switches.GetSwitch(_s1.Name);
            UpdateGuage1();
        }

        private void TrackBar2MouseUp(object sender, MouseEventArgs e)
        {
            label10.Text = null;
            Switches.SetSwitch(_s2.Name, _s2.State);
            _s2 = (INWaySwitch)Switches.GetSwitch(_s2.Name);
            UpdateGuage2();
        }


        #endregion
    }
}
