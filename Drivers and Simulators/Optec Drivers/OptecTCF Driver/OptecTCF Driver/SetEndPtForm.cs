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
        private delegate void ControlUpdateHandler(string pos, string temp);
        public SetEndPtForm()
        {
            InitializeComponent();
        }

        private void SetEndPtForm_Load(object sender, EventArgs e)
        {
            SlopePoint stpt = DeviceSettings.GetStartPoint();
            string date = stpt.DateAndTime.Date.ToString();
            string time = stpt.DateAndTime.TimeOfDay.ToString();
            string pos = stpt.Position.ToString();
            string temp = stpt.Temperature.ToString() + " °C";
            StartPtTemp_TB.Text = temp;
            StartPointPos_TB.Text = pos;
            StartPointDateTime_TB.Text = time + Environment.NewLine + date;

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int pos;
            double temp;

            while (this.Visible)
            {
                pos = DeviceComm.GetPosition();
                temp = DeviceComm.GetTemperaterature();
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
            Temp_TB.Text = temp;
        }

        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CapStPt_BTN_Click(object sender, EventArgs e)
        {
            
            SetSlopeForm SSFrm = new SetSlopeForm(100);
            SSFrm.ShowDialog();
        }
    }
}
