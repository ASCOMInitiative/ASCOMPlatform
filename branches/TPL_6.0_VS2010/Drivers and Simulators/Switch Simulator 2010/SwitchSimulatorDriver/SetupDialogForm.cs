using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using ASCOM.Interface;

namespace ASCOM.SwitchSimulator
{
	[ComVisible(true)]					// Form not registered for COM!
	public partial class SetupDialogForm : Form
	{
        private static ISwitch switchA = new Switch();
        private static ArrayList switchDevices = switchA.SwitchCollection;
        Stream redLED;
        Stream greenLED;

        
		public SetupDialogForm()
		{
			InitializeComponent();

            this.lb_DriverVersion.Text = "v" + switchA.DriverVersion;
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();
            redLED = assembly.GetManifestResourceStream("ASCOM.SwitchSimulator.Resources.RedLED.bmp");
            greenLED = assembly.GetManifestResourceStream("ASCOM.SwitchSimulator.Resources.GreenLED.bmp");

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
            int i = 1;
            //loop the switch devices
            foreach (SwitchDevice s in switchDevices)
            {
                //loop the first group of text boxes
                
                foreach (System.Windows.Forms.Control gbChild in this.groupBox1.Controls)
                {
                    if (gbChild.Name == "textBox" + i)
                    {
                        gbChild.Text = s.Name;
                    }
                   
                }
                foreach (System.Windows.Forms.Control pbChild in this.groupBox2.Controls)
                {
                    if (pbChild.Name == "pictureBox" + i)
                    {
                        PictureBox pb = (PictureBox)pbChild;
                        if (s.State)
                        {
                            pb.Image = new Bitmap(greenLED);
                        }
                        else
                        {
                            pb.Image = new Bitmap(redLED);
                        }
                    }
                } 
              i++;
            }
        }

        private void UpdateSwitchState(int i, PictureBox p)
        {
            SwitchDevice s = (SwitchDevice)switchDevices[i];

            if (s.State)
            {
                s.State = false;
                p.Image = new Bitmap(redLED);
            }
            else
            {
                s.State = true;
                p.Image = new Bitmap(greenLED);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            UpdateSwitchState(0, pictureBox1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            UpdateSwitchState(1, pictureBox2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            UpdateSwitchState(2, pictureBox3);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            UpdateSwitchState(3, pictureBox4);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            UpdateSwitchState(4, pictureBox5);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            UpdateSwitchState(5, pictureBox6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            UpdateSwitchState(6, pictureBox7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            UpdateSwitchState(7, pictureBox8);
        }
	}
}