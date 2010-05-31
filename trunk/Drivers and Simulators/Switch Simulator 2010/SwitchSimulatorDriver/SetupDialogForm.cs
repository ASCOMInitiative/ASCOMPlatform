using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace ASCOM.Simulator
{
	[ComVisible(true)]
	public partial class SetupDialogForm : Form
	{
        private static ISwitch switchA = new Switch();
        private ArrayList switches = switchA.Switches;
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
            redLED = assembly.GetManifestResourceStream("ASCOM.Simulator.Resources.RedLED.bmp");
            greenLED = assembly.GetManifestResourceStream("ASCOM.Simulator.Resources.GreenLED.bmp");
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
            foreach (string[,] s in switches)
            {
                //loop group one of text boxes
                foreach (System.Windows.Forms.Control gbChild in this.groupBox1.Controls)
                {
                    if (gbChild.Name == String.Format("textBox{0}", i))
                    {
                        gbChild.Text = s[0,0]; 
                    }
                   
                }
                //loop group2 of picture boxes
                foreach (System.Windows.Forms.Control pbChild in this.groupBox2.Controls)
                {
                    if (pbChild.Name == String.Format("pictureBox{0}", i))
                    {
                        PictureBox pb = (PictureBox)pbChild;
                        ChangeImageState(pb, Convert.ToBoolean(s[0,1]));
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
            string[,] s = (string[,])switches[i];

            bool b = Convert.ToBoolean(s[0,1]);
            if (b)
            {
                s[0,1] = "False";
                switchA.SetSwitch(s[0, 0], Convert.ToBoolean(s[0, 1]));
                return false;
            }
            else
            {
                s[0,1] = "True";
                switchA.SetSwitch(s[0, 0], Convert.ToBoolean(s[0, 1]));
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