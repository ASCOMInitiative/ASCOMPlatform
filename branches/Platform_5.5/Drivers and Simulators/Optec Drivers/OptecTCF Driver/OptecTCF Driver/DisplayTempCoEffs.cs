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

            ModeAName_LBL.Text = NameA;
            ModeBName_LBL.Text = NameB;

            ModeATempCoEff_LBL.Text = SignA.ToString() + SlopeA.ToString().PadLeft(3, '0');
            ModeBTempCoEff_LBL.Text = SignB.ToString() + SlopeB.ToString().PadLeft(3, '0');

        }

       
    }
}
