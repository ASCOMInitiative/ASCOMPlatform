using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        private static readonly ISwitchV2 SwitchA = new Switch();
        private Assembly _assembly;
        private Stream _greenLed;
        private Stream _redLed;
        private ArrayList _switches = SwitchA.Switches;

        /// <summary>
        /// Initialize the setup form 
        /// </summary>
        public SetupDialogForm()
        {
            InitializeComponent();

            //get the driver version number
            lb_DriverVersion.Text = @"v" + SwitchA.DriverVersion;

            LoadImagesFromResources();
            DisplaySwitchSettings();
        }

        /// <summary>
        /// Click the ok button, save, and quit
        /// </summary>
        private void CmdOkClick(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// Loads the red and green LED lights from resources 
        /// </summary>
        private void LoadImagesFromResources()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _assembly.GetManifestResourceNames();
            _redLed = _assembly.GetManifestResourceStream("ASCOM.Simulator.Resources.RedLED.bmp");
            _greenLed = _assembly.GetManifestResourceStream("ASCOM.Simulator.Resources.GreenLED.bmp");
        }

        /// <summary>
        /// browser link to the ASCOM website
        /// </summary>
        private static void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://ascom-standards.org/");
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
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
            foreach (IToggleSwitch t in _switches)
            {
                //loop group one of text boxes
                foreach (Control gbChild in groupBox1.Controls)
                {
                    if (gbChild.Name == String.Format("textBox{0}", i))
                    {
                        gbChild.Text = t.Name;
                    }
                }
                //loop group2 of picture boxes
                foreach (Control pbChild in groupBox2.Controls)
                {
                    if (pbChild.Name == String.Format("pictureBox{0}", i))
                    {
                        var pb = (PictureBox) pbChild;
                        ChangeImageState(pb, t.State[0] == "On");
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
            var t = (IToggleSwitch) _switches[i];

            switch (t.State[0])
            {
                case "On":
                    SwitchA.SetSwitch(t.Name, new[] {"Off"});
                    _switches = SwitchA.Switches;
                    return false;
                default:
                    SwitchA.SetSwitch(t.Name, new[] {"On"});
                    _switches = SwitchA.Switches;
                    return true;
            }
        }

        /// <summary>
        /// Changes the image for a given PictureBox based on the boolean 
        /// </summary>
        private void ChangeImageState(PictureBox p, bool b)
        {
            p.Image = b ? new Bitmap(_greenLed) : new Bitmap(_redLed);
        }

        /// <summary>
        /// Changes the Switch state and image based on the new state 
        /// </summary>
        private void ChangeDisplay(PictureBox p, int i)
        {
            bool b = ChangeSwitchState(i - 1);
            ChangeImageState(p, b);
            DisplaySwitchSettings();
        }

        private void PictureBox1Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox1, 1);
        }

        private void PictureBox2Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox2, 2);
        }

        private void PictureBox3Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox3, 3);
        }

        private void PictureBox4Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox4, 4);
        }

        private void PictureBox5Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox5, 5);
        }

        private void PictureBox6Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox6, 6);
        }

        private void PictureBox7Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox7, 7);
        }

        private void PictureBox8Click(object sender, EventArgs e)
        {
            ChangeDisplay(pictureBox8, 8);
        }
    }
}