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
        int CurrentPosition = -100;
        public LearnWizard3()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CurrentPosition = DeviceComm.GetPosition();
            Temp_LB.Text = DeviceComm.GetTemperaterature().ToString();
            this.Position_LB.Text = CurrentPosition.ToString();

        }

        private void In_Btn_Click(object sender, EventArgs e)
        {
            int pos2move2 = 0;
            if (CurrentPosition != -100)
            {
                pos2move2 = CurrentPosition - 1;
                DeviceComm.MoveFocus(pos2move2);
            }
        }

        private void Out_Btn_Click(object sender, EventArgs e)
        {
            int pos2move2 = 0;
            if (CurrentPosition != -100)
            {
                pos2move2 = CurrentPosition + 1;
                DeviceComm.MoveFocus(pos2move2);
            }
        }
    }
}
