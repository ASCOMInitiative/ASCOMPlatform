using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ASCOM.Optec
{
    public partial class OffsetsForm : Form
    {
        public OffsetsForm()
        {
            InitializeComponent();
        }
        public OffsetsForm(int currentPos)
        {
            InitializeComponent();
            PortNumber_CB.SelectedIndex = currentPos;
        }

        private void OK_Btn_Click(object sender, EventArgs e)
        {
               SavePortProperties();
               this.Close();
  
        }
        private void SavePortProperties()
        {
            try
            {
                ////////////////////// Check PORT NUMBER AND NAME VALUE ////////////////////
                short pn = short.Parse(PortNumber_CB.Text);
                if (pn < 1 || pn > 4)
                {
                    MessageBox.Show("Unacceptable Port Number. Port must be between 1 and 4");
                    return;
                }
                string name = PortName_TB.Text;
                if (name.Length < 1) 
                {
                    MessageBox.Show("You must enter a port name of at least one character");
                    return;
                }
                ////////////////////// Check RightAscension Value //////////////////////////
                double ra = (double)RightAscension_NUD.Value;
                if (ra < 0)
                {
                    MessageBox.Show("Unacceptable Right Ascension Value");
                    return;
                }
                ////////////////////// Check Declination Value /////////////////////////////
                double dec = (double)Declination_NUD.Value;
                if (dec < 0)
                {
                    MessageBox.Show("Unacceptable Declination Value");
                    return;
                }
                ////////////////////// Check Rotation Value ////////////////////////////////
                double rot = (double)Rotation_NUD.Value;
                if (rot < 0)
                {
                    MessageBox.Show("Unacceptable Rotation Value");
                    return;
                }
                ////////////////////// Check Focus Value ///////////////////////////////////
                short foc = (short)FocusOffset_NUD.Value;
                if (foc < 0)
                {
                    MessageBox.Show("Unacceptable Focus Value");
                    return;
                }
                ////////////////////// Assign Values to Port Object/////////////////////////
                MultiPortSelector TempMPS = new MultiPortSelector();
                Port prt = TempMPS.Ports[pn - 1] as Port;

                prt.Name = name;
                prt.RightAscensionOffset = ra;
                prt.DeclinationOffset = dec;
                prt.RotationOffset = rot;
                prt.FocusOffset = foc;

                TempMPS = null;
                prt = null;
                
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void OffsetsForm_Load(object sender, EventArgs e)
        {
        }

        private void PortNumber_CB_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadPortProperties(int.Parse(PortNumber_CB.Text));
        }
        private void LoadPortProperties(int portnum)
        {
            PortName_TB.Text = DeviceSettings.Name(portnum);
            FocusOffset_NUD.Value = DeviceSettings.FocusOffset(portnum);
            Declination_NUD.Value = (decimal)DeviceSettings.DeclinationOffset(portnum);
            RightAscension_NUD.Value = (decimal)DeviceSettings.RightAscensionOffset(portnum);
            Rotation_NUD.Value = (decimal)DeviceSettings.RotationOffset(portnum);
        }

        private void PortNumber_CB_Click(object sender, EventArgs e)
        {
            SavePortProperties();
        }

  
    }
}
