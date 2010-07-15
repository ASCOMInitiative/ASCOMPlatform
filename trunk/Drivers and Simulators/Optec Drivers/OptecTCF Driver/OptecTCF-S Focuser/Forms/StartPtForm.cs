using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_S
{
    public partial class StartPtForm : Form
    {
        private int CurrentPos = 0;
        private double CurrentTemp = 0;

        public StartPtForm()
        {
            InitializeComponent();


        }
        
        private void StartPtForm_Load(object sender, EventArgs e)
        {
            // First verify that Device is is serial mode
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


                // Retrieve the current position and temperature
                CurrentPos = OptecFocuser.GetPosition();
                CurrentTemp = OptecFocuser.GetTemperature();


                // Fill the position and temp boxes in the form.
                this.Pos_TB.Text = CurrentPos.ToString();
                this.Temp_TB.Text = CurrentTemp.ToString() + "°C";
            }
            catch (Exception ex)
            {

                throw new ASCOM.DriverException("An error occured while updating the form with current temp and position.", ex);
            }
        }

        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CapStPt_BTN_Click(object sender, EventArgs e)
        {
            SaveStartPoint();
        }

        private void SaveStartPoint()
        {
            try
            { 
                DateTime dt = DateTime.Now;

                SlopePoint stpt = new SlopePoint();
                stpt.DateAndTime = dt;
                stpt.Temperature = CurrentTemp;
                stpt.Position = CurrentPos;

                DeviceSettings.StartPoint = stpt;
                MessageBox.Show("Start point sucessfully stored!");
                this.Close();
            }
            catch(Exception Ex)
            {
                MessageBox.Show("An Error Occured While Storing Start Point.\n" + Ex.ToString());
            }
        }
    }
}
