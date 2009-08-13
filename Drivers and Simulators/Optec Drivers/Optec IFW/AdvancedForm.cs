using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows;

namespace ASCOM.Optec_IFW
{
    public partial class AdvancedForm : Form
    {

        #region Declarations
        public string COMPortString;
        public string CP;
        private bool CenteringValueChanged; 
        #endregion

        public AdvancedForm()
        {
            InitializeComponent();
            CenteringValueChanged = false;
        }

        private void Ad_Send_Btn_Click(object sender, EventArgs e)
        {

            try
            {
                string Response = DeviceComm.DebugSendCmd(this.Ad_CmdToSend.Text, int.Parse(CP), (int)this.ReadTimeout_Picker.Value);
                MessageBox.Show(Response,"Response From Device");
            }
            catch
            {
                MessageBox.Show("Error: Unknown Error");
            }

        }

        private void Ad_Connect_Btn_Click(object sender, EventArgs e)
        {
            DeviceComm.ConnectToDevice();
        }

        private void AdvancedForm_Load(object sender, EventArgs e)
        {
            this.Ad_ComPortLabel.Text = COMPortString;

        }

        private void Ad_CmdPicker_Cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Ad_CmdToSend.Text = this.Ad_CmdPicker_Cb.Text.Substring(0, 6);
        }

        private void Ad_Save_Click(object sender, EventArgs e)
        {
            StoreCentering();
            CenteringValueChanged = false;

        }

        private void v_Click(object sender, EventArgs e)
        {

            if (CenteringValueChanged)
            {
                DialogResult YesOrNo = new DialogResult();
                YesOrNo = MessageBox.Show("The Centering values have been changed but not stored. Would you like to store them before continuing?", "Centering Values Changed", MessageBoxButtons.YesNo);
                if (YesOrNo == DialogResult.Yes) StoreCentering();
            }
            this.Close();

        }

        private void Ad_Cancel_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StoreCentering()
        {
            try
            {
                DeviceComm.StoreCenteringData(Int32.Parse(this.CP), (int)this.NextValue_Picker.Value, (int)this.BackValue_Picker.Value);
            }
            catch
            {
                
                MessageBox.Show("Centering data not stored. Incorrect input value");
            }
        }

        private void BackValue_Picker_ValueChanged(object sender, EventArgs e)
        {
            CenteringValueChanged = true;
        }

        private void NextValue_Picker_ValueChanged(object sender, EventArgs e)
        {
            CenteringValueChanged = true;
        }
    }
}
