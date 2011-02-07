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

            aGauge2.MinValue = Convert.ToSingle(_s2.State[0]);
            aGauge2.MaxValue = Convert.ToSingle(_s2.State[1]);
            trackBar2.Value = Convert.ToInt16(_s2.State[2]);
            aGauge2.Value = trackBar2.Value;
            label2.Text = _s2.Name;
            
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
            Switches.SetSwitch(_s1.Name, _s1.State);
            _s1 = (IRheostat)Switches.GetSwitch(_s1.Name);
            aGauge1.Value = Convert.ToSingle(_s1.State[2]);
        }

        private void Guage2MouseUp(object sender, MouseEventArgs e)
        {
            Switches.SetSwitch(_s2.Name, _s2.State);
            _s2 = (IRheostat)Switches.GetSwitch(_s2.Name);
            aGauge2.Value = Convert.ToSingle(_s2.State[2]);
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