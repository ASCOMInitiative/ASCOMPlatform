using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    public partial class SetEndPtForm : Form
    {
        int FirstPosition = 0;
        double FirstTemperature = 0;
        string Date = "";
        string Time = "";

        private delegate void ControlUpdateHandler(string pos, string temp);

        public SetEndPtForm()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void SetEndPtForm_Load(object sender, EventArgs e)
        {
            try
            {  
                SlopePoint stpt = DeviceSettings.GetStartPoint();
                if (stpt.Position == 11111)
                {
                    MessageBox.Show("You must set the start point first");
                    this.Close();
                }

                FirstPosition = stpt.Position;
                //if (FirstPosition == DeviceComm.GetPosition)
                //{
                //    MessageBox.Show("End point position can not equal 
                //}
                FirstTemperature = stpt.Temperature;
                Date = stpt.DateAndTime.Date.ToString();
                Time = stpt.DateAndTime.TimeOfDay.ToString();
                StartPtTemp_TB.Text = FirstTemperature.ToString() + " °C";
                StartPointPos_TB.Text = FirstPosition.ToString();
                StartPointDateTime_TB.Text = Time + Environment.NewLine + Date;
                backgroundWorker1.RunWorkerAsync();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int pos;
            double temp;

            while (this.Visible)
            {
                pos = DeviceComm.Position;
                temp = DeviceComm.Temperature;
                if (this.Visible)
                {
                    this.BeginInvoke(new ControlUpdateHandler(UpdateControls),
                            new Object[] { pos.ToString(), temp.ToString() });
                }
            }
        }
        private void UpdateControls(string pos, string temp)
        {
            Pos_TB.Text = pos;
            Temp_TB.Text = temp + " °C";
        }

        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CapStPt_BTN_Click(object sender, EventArgs e)
        {
            string TempString = Temp_TB.Text;
            int i = TempString.IndexOf("°");
            TempString = TempString.Substring(0, i);
            int SecondPosition = int.Parse(Pos_TB.Text);
            double SecondTemperature = double.Parse(TempString);

            try
            {
                string SignAndSlope = DeviceSettings.CalculateSlope(FirstPosition, FirstTemperature, 
                    SecondPosition, SecondTemperature);
                char sign = SignAndSlope[0];
                string slope = SignAndSlope.Substring(1);
                SetSlopeForm SSFrm = new SetSlopeForm(slope, sign);
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
    }
}
