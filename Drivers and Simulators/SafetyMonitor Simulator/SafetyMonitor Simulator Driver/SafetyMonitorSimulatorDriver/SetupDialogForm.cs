using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        private static readonly ISafetyMonitor SafetyMonitor = new SafetyMonitor();
        Assembly _assembly;
        Stream _redLed;
        Stream _greenLed;

        public SetupDialogForm()
        {
            InitializeComponent();
            LoadImagesFromResources();
            DisplaySafetyMonitor();

        }

        private void Button1Click(object sender, EventArgs e)
        {
            Close();
        }

        private static void PictureBox1Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
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
        /// Populates the form from the driver with the Switch ArrayList
        /// </summary>
        private void DisplaySafetyMonitor()
        {
            if (SafetyMonitor != null)
            {
                ChangeImageState(pictureBox2, SafetyMonitor.IsSafe);
            }
        }
    }
}
