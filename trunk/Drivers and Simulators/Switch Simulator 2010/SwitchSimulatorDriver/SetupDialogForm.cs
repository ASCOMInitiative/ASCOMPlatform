using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Interface;

namespace ASCOM.SwitchSimulator
{
	[ComVisible(true)]					// Form not registered for COM!
	public partial class SetupDialogForm : Form
	{
        private static ISwitch switchA = new Switch();
        private static ArrayList switchDevices = switchA.SwitchCollection;
        
		public SetupDialogForm()
		{
			InitializeComponent();

            this.lb_DriverVersion.Text = "v" + switchA.DriverVersion;
            DisplaySwitchSettings();
            
           
		    }

		private void cmdOK_Click(object sender, EventArgs e)
		{
			Dispose();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Dispose();
		}

		private void BrowseToAscom(object sender, EventArgs e)
		{
			try {
				System.Diagnostics.Process.Start("http://ascom-standards.org/");
			} catch (System.ComponentModel.Win32Exception noBrowser) {
				if (noBrowser.ErrorCode == -2147467259)
					MessageBox.Show(noBrowser.Message);
			} catch (System.Exception other) {
				MessageBox.Show(other.Message);
			}
		}

        private void DisplaySwitchSettings()
        {
            int i = 0;
            int j = 1;

            //loop the switch devices
            foreach (SwitchDevice s in switchDevices)
            {
                //loop the high level form controls
                foreach (Control c in this.Controls)
                {
                    //loop the child form controls
                    foreach (Control childc in c.Controls)
                    {
                        if (childc.Name == "textBox" + j)
                       {
                          ((TextBox)childc).Text= s.Name;
                       }
                        else if (childc.Name == "radioButton" + j)
                       {
                          ((RadioButton)childc).Checked = s.State;
                       }
                        else if (childc.Name == "checkBox" + j)
                       {
                          ((CheckBox)childc).Checked = s.State;
                       }
                        else if (childc.Name == "pictureBox" + j)
                       {
                          ((PictureBox)childc).Visible = s.State;
                       }
                    }
                }
                i++;
                j++;
            }
        }

        private void UpdateSwitchState(int i, PictureBox p)
        {
            SwitchDevice s = (SwitchDevice)switchDevices[i];

            if (s.State == true)
            {
                s.State = false;
                p.Visible = false;
            }
            else
            {
                s.State = true;
                p.Visible = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSwitchState(0, pictureBox1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSwitchState(1, pictureBox2);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSwitchState(2, pictureBox3);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSwitchState(3, pictureBox4);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSwitchState(4, pictureBox5);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSwitchState(5, pictureBox6);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSwitchState(6, pictureBox7);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSwitchState(7, pictureBox8);
        }
	}
}