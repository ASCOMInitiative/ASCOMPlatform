using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    public partial class LearnWizard3 : Form
    {
        public delegate void ControlUpdateHandler(string pos, string temp);

        int DesiredPosition = -100;
        int CurrentPosition = -100;
        int maxPos = 10000;
     
        public LearnWizard3()
        {
            InitializeComponent();
        }

        private void In_Btn_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                DesiredPosition = CurrentPosition - Convert.ToInt32(Increment_NUD.Value);
                if (DesiredPosition < 1)
                {
                    DesiredPosition = 1;
                    MessageBox.Show("Focuser is all the way in.");
                }
            }
        }  
     
        private void Out_Btn_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                DesiredPosition = CurrentPosition + Convert.ToInt32(Increment_NUD.Value);
                if (DesiredPosition > maxPos)
                {
                    DesiredPosition = maxPos;
                    MessageBox.Show("Focuser is all the way out.");
                }
            }
        }
      
        private void LearnWizard3_Load(object sender, EventArgs e)
        {
            maxPos = DeviceSettings.GetMaxStep();
            DesiredPosition = DeviceComm.Position;
            CurrentPosition = DesiredPosition;
            this.Position_LB.Text = DesiredPosition.ToString();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            double temp;
            while (this.Visible)
            {
                temp = DeviceComm.Temperature;
                if (CurrentPosition != DesiredPosition)
                {
                    lock (this)
                    {
                        DeviceComm.MoveFocus(DesiredPosition);
                        CurrentPosition = DesiredPosition;
                    }
                }
                //Update Controls
                if(this.Visible)
                {
                    this.BeginInvoke(new ControlUpdateHandler(UpdateControls),
                        new Object[] { CurrentPosition.ToString(), temp.ToString() });
                }
                
            }
        }

        private void UpdateControls(string pos, string temp)
        {
            Position_LB.Text = pos;
            Temp_LB.Text = temp;
        }



    }
}
