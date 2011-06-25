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
    public partial class SetSlopeForm : Form
    {
        public SetSlopeForm()
        {
            InitializeComponent();
        }

        public SetSlopeForm(int slope)
        {
            InitializeComponent();
            this.label1.Text = "Temperature Coefficient =";
            this.Slope_TB.Text = slope.ToString();
        }

        private void SetSlopeForm_Load(object sender, EventArgs e)
        {
            char mode = DeviceSettings.ActiveTempCompMode;
            if (mode == 'A')
            {
                ModeA_RB.Checked = true;
            }
            else if (mode == 'B')
            {
                ModeB_RB.Checked = true;
            }
            else
            {
                throw new ASCOM.InvalidValueException("SetSlopeForm_Load->ActiveTempCompMode", mode.ToString(), "A or B");
            }

            this.ModeA_RB.Text += DeviceSettings.ModeA_Name + ") Current Coeff = " + OptecFocuser.GetLearnedSlope('A');
            this.ModeB_RB.Text += DeviceSettings.ModeB_Name + ") Current Coeff = " + OptecFocuser.GetLearnedSlope('B');
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult YesNo = new DialogResult();
            YesNo = MessageBox.Show("Are you sure you want to cancel?\n" +
                "Changes will not be saved.", "Cancel?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (YesNo == DialogResult.Yes) this.Close();
        }

        private void SetSlope_BTN_Click(object sender, EventArgs e)
        {
            char AorB = 'A';
            int slope = 0;
            try
            {
                if (ModeB_RB.Checked) AorB = 'B';           //otherwise already 'A'
                /////SAVE DELAY //////////////////////////////////////////////
                //check if slope if for mode A or B
                
               //// OptecFocuser.SetDelay(AorB, Convert.ToDouble(this.Delay_NUD.Value));

               // if (AorB == 'A')
               //     DeviceSettings.ModeA_Delay = Convert.ToInt32(this.Delay_NUD.Value);
               // else DeviceSettings.ModeB_Delay = Convert.ToInt32(this.Delay_NUD.Value);

                ////SAVE Coeff ///////////////////////////////////////////////
                string text = this.Slope_TB.Text;
                slope = Convert.ToInt32(Slope_TB.Text);

                //check if slope is within bounds
                if (slope < -998 || slope > 999)
                {
                    MessageBox.Show("Slope must be between -998 and +999");
                    return;
                }

                //set slope
                OptecFocuser.SetSlope(slope, AorB);
                MessageBox.Show("Slope successfully set!");
                this.Close();

            }
            catch (Exception Ex)
            {
                MessageBox.Show("An Error occured... \n" + Ex.ToString());
            }
        }

        private void ModeChecked_Changed(object sender, EventArgs e)
        {
            //if (label1.Text.Contains("Enter"))
            //{
            //    if (ModeA_RB.Checked)
            //    {
            //        Slope_TB.Text = OptecFocuser.GetLearnedSlope('A').ToString();
            //        //Delay_NUD.Value = Convert.ToDecimal(DeviceSettings.ModeA_Delay);
            //    }
            //    else
            //    {
            //        Slope_TB.Text = OptecFocuser.GetLearnedSlope('B').ToString();
            //        //Delay_NUD.Value = Convert.ToDecimal(DeviceSettings.ModeB_Delay);
            //    }
            //}
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string msg;
            msg = "The update delay determines the length of time between focuser\n" +
                "temperature corrections when the device is opearting in temperature\n" +
                    "compensation mode. Default is 1 second. Range is 1 to 10.99 seconds.";
            MessageBox.Show(msg, "Update Delay", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
     */
}
