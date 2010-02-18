using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    public partial class DisplayTempCoEffs : Form
    {
        public DisplayTempCoEffs()
        {
            InitializeComponent();
        }

        private void DisplayTempCoEffs_Load(object sender, EventArgs e)
        {
            char SignA;
            char SignB;
            int SlopeA;
            int SlopeB;
            string NameA;
            string NameB;

            SignA = DeviceComm.GetSlopeSign('A');
            SignB = DeviceComm.GetSlopeSign('B');

            SlopeA = DeviceComm.GetLearnedSlope('A');
            SlopeB = DeviceComm.GetLearnedSlope('B');

            NameA = DeviceSettings.GetModeName('A');
            NameB = DeviceSettings.GetModeName('B');

            ModeA_TB.Text = "Mode A (" + NameA + ")" + " Temp. Coefficient = " + SignA.ToString() + SlopeA.ToString();
            ModeB_TB.Text = "Mode B (" + NameB + ")" + " Temp. Coefficient = " + SignB.ToString() + SlopeB.ToString();


        }
    }
}
