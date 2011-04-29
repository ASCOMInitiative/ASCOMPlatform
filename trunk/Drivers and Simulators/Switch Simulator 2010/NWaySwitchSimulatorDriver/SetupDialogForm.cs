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
        #region Local Variables

        private IController NWaySwitchController = new NWaySwitchController();
        private ArrayList ControllerDevices;
        private NWaySwitch NWaySWitch1;
        private NWaySwitch NWaySwitch2;

        #endregion

        #region Constructor

        public SetupDialogForm()
        {
            InitializeComponent();
            ControllerDevices = NWaySwitchController.ControllerDevices;
            NWaySWitch1 = (NWaySwitch)ControllerDevices[0];
            NWaySwitch2 = (NWaySwitch)ControllerDevices[1];

            label5.Text = null;
            label10.Text = null;

            UpdateGuage1();
            UpdateGuage2();
            label1.Text = NWaySwitchController.Name  + @" v" + NWaySwitchController.DriverVersion;
        }

        #endregion

        #region Private Members

        private void UpdateGuage1()
        {
            label2.Text = NWaySWitch1.Name;
            var guage1Min = Convert.ToSingle(NWaySWitch1.Minimum);
            var guage1Max = Convert.ToSingle(NWaySWitch1.Maximim);
            var guage1Value = Convert.ToInt16(NWaySWitch1.PresentValue);

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
            label3.Text = NWaySwitch2.Name;
            var guage2Min = Convert.ToSingle(NWaySwitch2.Minimum);
            var guage2Max = Convert.ToSingle(NWaySwitch2.Maximim);
            var guage2Value = Convert.ToInt16(NWaySwitch2.PresentValue);

            aGauge3.MinValue = guage2Min; // -1;
            aGauge3.MaxValue = guage2Max;
            aGauge3.Value = guage2Value;

            if (guage2Max > aGauge3.RangeEndValue)
            {
                aGauge3.RangeEndValue = guage2Max * 201 / 200;
                aGauge3.RangeStartValue = (guage2Max * 9) / 10;
            }
            else
            {
                aGauge3.RangeStartValue = (guage2Max * 9) / 10;
                aGauge3.RangeEndValue = guage2Max;
            }

            aGauge3.RangeStartValue = (guage2Max * 9) / 10;
            aGauge3.RangeEndValue = guage2Max * 201 / 200;

            trackBar2.Minimum = Convert.ToInt16(guage2Min);
            trackBar2.Maximum = Convert.ToInt16(guage2Max);
            trackBar2.Value = guage2Value;

            comboBox3.Text = guage2Min.ToString();
            comboBox4.Text = guage2Max.ToString();

            txtPowerWarning.Text = NWaySwitch2.StateName;
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
            double Min = double.Parse(comboBox1.Text);
            double Max = double.Parse(comboBox2.Text);
            double Val = aGauge1.Value;

            NWaySWitch1.SetMinMax(Min, Max);

            if (Val < Min) Val = Min; // Make sure the actual value is wihin the new range!
            if (Val > Max) Val = Max;
            NWaySWitch1.SetValue(Val);

            if (Convert.ToInt16(comboBox1.Text) < Convert.ToInt16(comboBox2.Text))
            {
                NWaySwitchController.SetControl(NWaySWitch1.Name, NWaySWitch1.PresentValue);
                NWaySWitch1 = (NWaySwitch)NWaySwitchController.GetControl(NWaySWitch1.Name);
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
            try
            {
                double Min = double.Parse(comboBox3.Text);
                double Max = double.Parse(comboBox4.Text);
                double Val = aGauge3.Value;
                NWaySwitch2.SetMinMax(Min, Max);

                if (Val < Min) Val = Min; // Make sure the actual value is wihin the new range!
                if (Val > Max) Val = Max;
                NWaySwitch2.SetValue(Val);

                if (Convert.ToInt16(comboBox3.Text) < Convert.ToInt16(comboBox4.Text))
                {
                    NWaySwitchController.SetControl(NWaySwitch2.Name, Val);
                    NWaySwitch2 = (NWaySwitch)NWaySwitchController.GetControl(NWaySwitch2.Name);
                    UpdateGuage2();
                    label10.Text = @"Switch settings saved";
                }
                else
                {
                    label10.Text = @"Min must be less than Max";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Timer1Tick(object sender, EventArgs e)
        {
            NWaySWitch1.SetValue(Convert.ToDouble(trackBar1.Value));
            aGauge1.Value = trackBar1.Value;

            NWaySwitch2.SetValue( Convert.ToDouble(trackBar2.Value));
            aGauge3.Value = trackBar2.Value;
            txtPowerWarning.Text = NWaySwitch2.StateName;
        }

        private void TrackBar1MouseUp(object sender, MouseEventArgs e)
        {
            label5.Text = null;
            NWaySwitchController.SetControl(NWaySWitch1.Name, NWaySWitch1.PresentValue);
            NWaySWitch1 = (NWaySwitch)NWaySwitchController.GetControl(NWaySWitch1.Name);
            UpdateGuage1();
        }

        private void TrackBar2MouseUp(object sender, MouseEventArgs e)
        {
            label10.Text = null;
            NWaySwitchController.SetControl(NWaySwitch2.Name, NWaySwitch2.PresentValue);
            NWaySwitch2 = (NWaySwitch)NWaySwitchController.GetControl(NWaySwitch2.Name);
            UpdateGuage2();
        }

        #endregion
    }
}
