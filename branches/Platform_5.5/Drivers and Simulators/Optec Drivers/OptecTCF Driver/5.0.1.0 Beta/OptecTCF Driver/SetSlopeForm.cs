using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    public partial class SetSlopeForm : Form
    {
        public SetSlopeForm()
        {
            InitializeComponent();
            this.label1.Text = "Enter Temperature Coefficient =";
            this.Slope_TB.ReadOnly = false;
        }
        public SetSlopeForm(string slope, char sign)
        {
            InitializeComponent();
            this.label1.Text = "Temperature Coefficient =";
            this.Slope_TB.Text = sign.ToString() + slope;
        }

        private void SetSlopeForm_Load(object sender, EventArgs e)
        {
            char mode = DeviceSettings.GetActiveMode();
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
                throw new InvalidPrerequisits("A or B is not selected");
            }
            
            this.ModeA_RB.Text += DeviceSettings.GetModeName('A') + ")";
            this.ModeB_RB.Text += DeviceSettings.GetModeName('B') + ")";
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
            char sign = '+';
            int slope = 0;
            try
            {
               /////SAVE DELAY //////////////////////////////////////////////
                //check if slope if for mode A or B
                if (ModeB_RB.Checked) AorB = 'B';           //otherwise already 'A'
                DeviceComm.SetDelay(AorB, Convert.ToDouble(this.Delay_NUD.Value));
                DeviceSettings.SetDelayToConfig(AorB, Convert.ToDouble(this.Delay_NUD.Value));
                ////SAVE TC ///////////////////////////////////////////////
                string text = this.Slope_TB.Text;
                int i = text.IndexOf('+');
                int j = text.IndexOf('-');
                if (i > 0 || j > 0)
                {
                    MessageBox.Show("Slope is not in correct format.\n"
                        + "Slope must be in format \"(+/-)nnn\" Ex: -125");
                    return;
                }
                else if (i == 0)
                {
                    //+ present
                    slope = Convert.ToInt32(Slope_TB.Text.Substring(1));
                }
                else if (j == 0)
                {
                    //- present
                    sign = '-';
                    slope = Convert.ToInt32(Slope_TB.Text.Substring(1));
                }
                else
                {
                    //assume positive slope
                    slope = Convert.ToInt32(Slope_TB.Text);
                }

                //check if slope is within bounds
                if (slope < 0 || slope > 999)
                {
                    MessageBox.Show("Slope must be between 0 and 999");
                    return;
                }
                
                //set slope
                DeviceComm.SetSlope(slope, AorB);
                DeviceComm.SetSlopeSign(sign, AorB);
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
            if (label1.Text.Contains("Enter"))
            {
                if (ModeA_RB.Checked)
                {
                    Slope_TB.Text = DeviceComm.GetSlopeSign('A').ToString() + DeviceComm.GetLearnedSlope('A').ToString();
                    Delay_NUD.Value = Convert.ToDecimal(DeviceSettings.GetDelayFromConfig('A'));
                }
                else
                {
                    Slope_TB.Text = DeviceComm.GetSlopeSign('B').ToString() + DeviceComm.GetLearnedSlope('B').ToString();
                    Delay_NUD.Value = Convert.ToDecimal(DeviceSettings.GetDelayFromConfig('B'));
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string msg;
            msg = "The update delay determines the length of time between focuser\n" +
                "temperature corrections when the device is opearting in temperature\n" +
                    "compensation mode. Default is 1 second. Range is 1 to 10.99 seconds.";
            MessageBox.Show(msg, "Update Delay",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

       
    }
}
