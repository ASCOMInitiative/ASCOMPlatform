using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    public partial class SetStartPtForm : Form
    {
        private delegate void ControlUpdateHandler(string pos, string temp);
        public SetStartPtForm()
        {
            InitializeComponent();
        }

        private void CapStPt_BTN_Click(object sender, EventArgs e)
        {
            double temp = double.Parse(Temp_TB.Text);
            int pos = int.Parse(Pos_TB.Text);
            DateTime dt = DateTime.Now;

            SlopePoint stpt = new SlopePoint();
            stpt.DateAndTime = dt;
            stpt.Temperature = temp;
            stpt.Position = pos;

            DeviceSettings.SetStartPoint(stpt);
            
            this.Close();

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

        private void SetStartPtForm_Load(object sender, EventArgs e)
        {
            //Device must already be connected for form to open...
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
