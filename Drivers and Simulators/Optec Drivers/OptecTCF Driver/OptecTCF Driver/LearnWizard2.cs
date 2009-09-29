using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    public partial class LearnWizard2 : Form
    {
        int CurrentPos = -100;
        public LearnWizard2()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            CurrentPos = DeviceComm.GetPosition();
            this.Position_LB.Text = CurrentPos.ToString();
            this.Temp_LB.Text = DeviceComm.GetTemperaterature().ToString();
           

        }

        private void In_Btn_Click(object sender, EventArgs e)
        {
            int pos2move2 = 0;
            if (CurrentPos != -100)
            {
                pos2move2 = CurrentPos - 1;
                DeviceComm.MoveFocus(pos2move2);

            }
        }

        private void Out_Btn_Click(object sender, EventArgs e)
        {
            int pos2move2 = 0;
            if (CurrentPos != -100)
            {
                pos2move2 = CurrentPos + 1;
                DeviceComm.MoveFocus(pos2move2);

            }
        }



    }
}
