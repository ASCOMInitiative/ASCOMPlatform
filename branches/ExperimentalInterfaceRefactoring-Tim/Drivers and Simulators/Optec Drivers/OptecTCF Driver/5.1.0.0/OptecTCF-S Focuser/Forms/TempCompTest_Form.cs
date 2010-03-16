using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_S
{
    public partial class TempCompTest_Form : Form
    {
        private int TestDuration = 0;
        private int TestProgress = 0;

        public TempCompTest_Form()
        {
            InitializeComponent();
        }

        private void TempCompTest_Form_Load(object sender, EventArgs e)
        {
            if (OptecFocuser.ConnectionState != OptecFocuser.ConnectionStates.SerialMode)
            {
                throw new ASCOM.DriverException("Device must be in serial mode to begin test");
            }

        }

        private void RunTest()
        {
            Output("Testing Temperature Compensation Mode " + DeviceSettings.ActiveTempCompMode.ToString());
            Output("Temperature Coefficient For Mode " + DeviceSettings.ActiveTempCompMode.ToString() +
                " = " + OptecFocuser.GetLearnedSlope( DeviceSettings.ActiveTempCompMode));
            Output("Test Duration  = " + duration_NUD.Value.ToString() + " Seconds");
            Output("Attempting To Enter Temp Comp Mode");

            try {OptecFocuser.EnterTempCompMode();}   catch{}

            if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.TempCompMode)
            {
                Output("Successfully Entered Temp Comp Mode");
            }
            else
            {
                Output("Failed To Enter Temp Comp Mode");
            }
            TestTimer.Interval = 1000;
            TestTimer.Enabled = true;
        }

        private void Output(string text)
        {
            Output_LB.Items.Add(text);
            if (Output_LB.Items.Count > 5)
            {
                Output_LB.SelectedIndex = Output_LB.Items.Count - 1;
            }
            Application.DoEvents();
        }

        private void clearData(object sender, EventArgs e)
        {
            Output_LB.Items.Clear();
            progressBar1.Value = 0;
        }

        private void Start_Btn_Click(object sender, EventArgs e)
        {
            Stop_Btn.Enabled = true;
            Start_Btn.Enabled = false;
            TestProgress = 0;
            TestDuration = Convert.ToInt32(duration_NUD.Value);
            progressBar1.Maximum = TestDuration;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            RunTest();
        }

        private void Stop_Btn_Click(object sender, EventArgs e)
        {
            TestTimer.Enabled = false;
            Stop_Btn.Enabled = false;
            Start_Btn.Enabled = true;
            Output("Test Stopped By User After " + TestProgress + " Seconds");
            StopTest();
        }

        private void TestTimer_Tick(object sender, EventArgs e)
        {
            TestProgress++;
            if (TestProgress >= TestDuration)
            {
                TestTimer.Enabled = false;
                Output("Test Time Has Expired");
                StopTest();
            }
            else
            {
                try
                {
                    Output("Temp = " + String.Format("{0:00.0}", OptecFocuser.GetTemperature()) + "°C  -  " +
                        "Position = " + OptecFocuser.CurrentPosition.ToString() +
                        "  -  Seconds = " + TestProgress.ToString());

                    UpdateProgressBar();
                }
                catch
                {
                    Output("Error Receiving Device Data");
                    Output("Text Aborted at " + TestProgress.ToString() + " Seconds");
                    StopTest();
                }
                
            }
        }

        private void StopTest()
        {
            try
            {
                Output("Exiting Temp Comp Mode...");
                OptecFocuser.ExitTempCompMode();
            }
            catch
            {
            }
            finally
            {
                Output("Exited Temp Comp Mode Successfully");
                Output("Test Complete");
                Stop_Btn.Enabled = false;
                Start_Btn.Enabled = true;
                TestTimer.Enabled = false;
                progressBar1.Visible = false;
            }
        }

        private void UpdateProgressBar()
        {
            if (TestProgress > progressBar1.Maximum)
            {
                progressBar1.Value = progressBar1.Maximum;
            }
            progressBar1.Value = TestProgress + 1;
            Application.DoEvents();
        }

        private void Close_Btn_Click(object sender, EventArgs e)
        {
            if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.TempCompMode)
            {
                try
                {
                    StopTest();
                }
                catch { }
                finally { }
            }
            this.Close();
        }
    }
}
