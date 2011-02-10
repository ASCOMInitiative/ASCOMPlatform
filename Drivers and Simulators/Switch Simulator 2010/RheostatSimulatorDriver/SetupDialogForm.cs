using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using System.Collections;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        #region Constants

        private readonly ArrayList _switches;
        private IRheostat _s1;
        private IRheostat _s2;

        #endregion

        #region Constructor

        public SetupDialogForm()
        {
            InitializeComponent();
            _switches = Switches.Switches;
            _s1 = (IRheostat) _switches[0];
            _s2 = (IRheostat) _switches[1];
            
            aGauge1.MinValue = Convert.ToSingle(_s1.State[0]);
            aGauge1.MaxValue = Convert.ToSingle(_s1.State[1]);
            trackBar1.Value = Convert.ToInt16(_s1.State[2]);
            aGauge1.Value = trackBar1.Value;
            label1.Text = _s1.Name;

            SetupGuage1();
            SetupGuage2();

            label6.Text = null;
            label11.Text = null;
            label3.Text = Switches.Name + @" v" + Switches.DriverVersion;
        }

        #endregion

        #region Private Members

        private void Timer1Tick(object sender, EventArgs e)
        {
                _s1.State[2] = Convert.ToString(trackBar1.Value);
                aGauge1.Value = trackBar1.Value;
                label4.Text = trackBar1.Value.ToString();

                _s2.State[2] = Convert.ToString(trackBar2.Value);
                aGauge2.Value = trackBar2.Value;
                label5.Text = trackBar2.Value.ToString();
        }

        private void Guage1MouseUp(object sender, MouseEventArgs e)
        {
            label11.Text = null;
            Switches.SetSwitch(_s1.Name, _s1.State);
            _s1 = (IRheostat)Switches.GetSwitch(_s1.Name);
            aGauge1.Value = Convert.ToSingle(_s1.State[2]);
        }

        private void Guage2MouseUp(object sender, MouseEventArgs e)
        {
            label6.Text = null;
            Switches.SetSwitch(_s2.Name, _s2.State);
            _s2 = (IRheostat)Switches.GetSwitch(_s2.Name);
            SetupGuage2();
        }

        private void SetupGuage2()
        {

            label2.Text = _s2.Name;
            var guage2Min = Convert.ToSingle(_s2.State[0]);
            var guage2Max = Convert.ToSingle(_s2.State[1]);
            var guage2Value = Convert.ToInt16(_s2.State[2]);

            aGauge2.MinValue = guage2Min;
            aGauge2.MaxValue = guage2Max;
            aGauge2.RangeStartValue = Convert.ToSingle(guage2Max - (guage2Max * .10));
            aGauge2.Value = guage2Value;
            aGauge2.RangeEndValue = guage2Max;

            trackBar2.Minimum = Convert.ToInt16(guage2Min);
            trackBar2.Maximum = Convert.ToInt16(guage2Max);
            trackBar2.Value = guage2Value;

            comboBox1.Text = guage2Min.ToString();
            comboBox2.Text = guage2Max.ToString();

            aGauge2.RangeStartValue = Convert.ToSingle(guage2Max - (guage2Max * .10));
            aGauge2.RangeEndValue = guage2Max;

        }

        private void SetupGuage1()
        {

            label1.Text = _s1.Name;
            var guage1Min = Convert.ToSingle(_s1.State[0]);
            var guage1Max = Convert.ToSingle(_s1.State[1]);
            var guage1Value = Convert.ToInt16(_s1.State[2]);

            aGauge1.MinValue = guage1Min;
            aGauge1.MaxValue = guage1Max;
            aGauge1.RangeStartValue = Convert.ToSingle(guage1Max - (guage1Max * .10));
            aGauge1.Value = guage1Value;
            aGauge1.RangeEndValue = guage1Max;

            trackBar1.Minimum = Convert.ToInt16(guage1Min);
            trackBar1.Maximum = Convert.ToInt16(guage1Max);
            trackBar1.Value = guage1Value;

            comboBox3.Text = guage1Min.ToString();
            comboBox4.Text = guage1Max.ToString();

            aGauge1.RangeStartValue = Convert.ToSingle(guage1Max - (guage1Max * .10));
            aGauge1.RangeEndValue = guage1Max;

        }
  
        private void BrowseToAscom(object sender, MouseEventArgs e)
        {
                        try
            {
                Process.Start(@"http://ascom-standards.org/");
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

        private void Button2Click(object sender, EventArgs e)
        {
            _s2.State[0] = comboBox1.Text;
            _s2.State[1] = comboBox2.Text;
            _s2.State[2] = comboBox1.Text;

            if (Convert.ToInt16(comboBox1.Text) < Convert.ToInt16(comboBox2.Text))
            {
                Switches.SetSwitch(_s2.Name, _s2.State);
                _s2 = (IRheostat) Switches.GetSwitch(_s2.Name);
                SetupGuage2();
                label6.Text = @"Switch settings saved";
            }
            else
            {
                label6.Text = @"Min must be less than Max";
            }
        }

        private void Button3Click(object sender, EventArgs e)
        {
            _s1.State[0] = comboBox3.Text;
            _s1.State[1] = comboBox4.Text;
            _s1.State[2] = comboBox3.Text;

            if (Convert.ToInt16(comboBox3.Text) < Convert.ToInt16(comboBox4.Text))
            {
                Switches.SetSwitch(_s1.Name, _s1.State);
                _s1 = (IRheostat)Switches.GetSwitch(_s1.Name);
                SetupGuage1();
                label11.Text = @"Switch settings saved";
            }
            else
            {
                label11.Text = @"Min must be less than Max";
            }
        }

        #endregion
    }
}