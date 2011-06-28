using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_S
{
    /*
    public partial class EndPointForm : Form
    {
        private string StartDate = "";
        private string StartTime = "";
        private SlopePoint stpt = new SlopePoint();
        private int CurrentPos = 0;
        private double CurrentTemp = 0;
 

        public EndPointForm()
        {
            InitializeComponent();
        }

        private void EndPointForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (OptecFocuser.ConnectionState != OptecFocuser.ConnectionStates.SerialMode)
                {
                    MessageBox.Show("Device Must Be Connected To Perform This Operation.");
                    this.Close();
                    return;
                }
                else if (DeviceSettings.TempProbePresent == false)
                {
                    MessageBox.Show("Temperature Probe Must Be Attached and Enabled to Perform This Operation.");
                    this.Close();
                    return;
                }

                stpt = DeviceSettings.StartPoint;
                if (stpt.Position == 11111)
                {
                    MessageBox.Show("You must set the start point first");
                    this.Close();
                }

                StartDate = stpt.DateAndTime.Date.ToString();
                StartTime = stpt.DateAndTime.TimeOfDay.ToString();

                StartPtTemp_TB.Text = stpt.Temperature.ToString() + " °C";
                StartPointPos_TB.Text = stpt.Position.ToString();
                StartPointDateTime_TB.Text = StartTime + Environment.NewLine + StartDate;


                // Update the current Info
                CurrentPos = OptecFocuser.GetPosition();
                CurrentTemp = OptecFocuser.GetTemperature();

                Pos_TB.Text = CurrentPos.ToString();
                Temp_TB.Text = CurrentTemp.ToString() + "°C";

            }
            catch (Exception ex)
            { 
                throw new ASCOM.DriverException("Error Loading Start Point Information\n" + ex.ToString(), ex);
            }
        }

        private void CapStPt_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                int slope = DeviceSettings.CalculateSlope(stpt.Position, stpt.Temperature,
                     CurrentPos, CurrentTemp);
                SetSlopeForm SSFrm = new SetSlopeForm(slope);
                SSFrm.ShowDialog();
                this.Close();
            }
            catch (InvalidOperationException Ex)
            {
                MessageBox.Show("Unacceptable slope for the following reason:\n" + Ex.Message);

            }
            catch (Exception Ex)
            {
                MessageBox.Show("An unexpected error orruced...\n" + Ex.ToString());
            }
        }

        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            DialogResult x = new DialogResult();
            x = MessageBox.Show("Close Without Capturing End Poing?\n" +
                "Slope Will Not Be Calculated or Stored.", "Close?", MessageBoxButtons.YesNo);
            if (x == DialogResult.Yes) this.Close();
        }
    }
     */
}
