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
	[ComVisible(true)]
    [Guid("A168E7F7-06C5-4045-9919-CB2E1844B076")]
    [ClassInterface(ClassInterfaceType.None)]
	public partial class SetupDialogForm : Form
	{
        private static ISwitch switchA = new Switch();
        private static ArrayList switchDevices = switchA.SwitchDevices;
        Assembly assembly;
        Stream redLED;
        Stream greenLED;

        /// <summary>
        /// Initialize the setup form 
        /// </summary>
		public SetupDialogForm()
		{
			InitializeComponent();

            //get the driver version number
            this.lb_DriverVersion.Text = "v" + switchA.DriverVersion;

            LoadImagesFromResources();
            DisplaySwitchSettings();
		    }

        /// <summary>
        /// Click the ok button, save, and quit
        /// </summary>
		private void cmdOK_Click(object sender, EventArgs e)
		{
			Dispose();
		}

        /// <summary>
        /// Click the cancel button and quit
        /// </summary>
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Dispose();
		}

        /// <summary>
        /// Loads the red and green LED lights from resources 
        /// </summary>
        private void LoadImagesFromResources()
        {
            assembly = Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();
            redLED = assembly.GetManifestResourceStream("ASCOM.SwitchSimulator.Resources.RedLED.bmp");
            greenLED = assembly.GetManifestResourceStream("ASCOM.SwitchSimulator.Resources.GreenLED.bmp");
        }

        /// <summary>
        /// browser link to the ASCOM website
        /// </summary>
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

        /// <summary>
        /// Populates the form from the driver with the Switch ArrayList
        /// </summary>
        private void DisplaySwitchSettings()
        {
            int i = 1;
            //loop the switch devices
            foreach (SwitchDevice s in switchDevices)
            {
                //loop group one of text boxes
                foreach (System.Windows.Forms.Control gbChild in this.groupBox1.Controls)
                {
                    if (gbChild.Name == String.Format("textBox{0}", i))
                    {
                        gbChild.Text = s.Name; 
                    }
                   
                }
                //loop group2 of picture boxes
                foreach (System.Windows.Forms.Control pbChild in this.groupBox2.Controls)
                {
                    if (pbChild.Name == String.Format("pictureBox{0}", i))
                    {
                        PictureBox pb = (PictureBox)pbChild;
                        ChangeImageState(pb, s.State);
                    }
                } 
              i++;
            }
        }

        /// <summary>
        /// Updates the state of a Switch and changes the image to match
        /// </summary>
        private bool ChangeSwitchState(int i)
        {
            ISwitchDevice s = (SwitchDevice)switchDevices[i];

            bool b = s.State;
            if (b)
            {
                s.State = false;
                return false;
            }
            else
            {
                s.State = true;
                return true;
            }
        }

        /// <summary>
        /// Changes the image for a given PictureBox based on the boolean 
        /// </summary>
        private void ChangeImageState(PictureBox p, bool b)
        {
             if (b)
            {
                p.Image = new Bitmap(greenLED);
            }
            else
            {
                p.Image = new Bitmap(redLED);
            }
        }

        /// <summary>
        /// Changes the Switch state and image based on the new state 
        /// </summary>
        private void ChangeDisplay(PictureBox p, int i)
        {
            bool b = ChangeSwitchState(i - 1);
            ChangeImageState(p, b);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox1, 1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
              ChangeDisplay(pictureBox2, 2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
              ChangeDisplay(pictureBox3, 3);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
              ChangeDisplay(pictureBox4, 4);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
               ChangeDisplay(pictureBox5, 5);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
               ChangeDisplay(pictureBox6, 6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
              ChangeDisplay(pictureBox7, 7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
               ChangeDisplay(pictureBox8, 8);
        }

	}
}