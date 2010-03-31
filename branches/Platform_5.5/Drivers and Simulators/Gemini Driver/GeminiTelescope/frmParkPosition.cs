using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.GeminiTelescope.Properties;


namespace ASCOM.GeminiTelescope
{
    public partial class frmParkPosition : Form
    {
        public frmParkPosition()
        {
            InitializeComponent();
        }

        private void frmParkPosition_Load(object sender, EventArgs e)
        {
            txtAlt.Text = GeminiHardware.ParkAlt.ToString();
            txtAz.Text = GeminiHardware.ParkAz.ToString();
            switch (GeminiHardware.ParkPosition)
            {
                case GeminiHardware.GeminiParkMode.SlewAltAz: rbAltAz.Checked = true; break;
                case GeminiHardware.GeminiParkMode.SlewCWD : this.rbCWD.Checked = true; break;
                case GeminiHardware.GeminiParkMode.SlewHome: this.rbHome.Checked = true; break;
                default: rbNoSlew.Checked = true; break;
            }
        }

        private void pbGetPos_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
                try
                {
                    txtAlt.Text = GeminiHardware.Altitude.ToString("0.00");
                    txtAz.Text = GeminiHardware.Azimuth.ToString("0.00");
                }
                catch { }
        }

        private void pbOK_Click(object sender, EventArgs e)
        {
            double alt, az;

            if (!double.TryParse(txtAlt.Text, out alt))
            {
                MessageBox.Show(Resources.InvalidAltValue, SharedResources.TELESCOPE_DRIVER_NAME);
                DialogResult = DialogResult.None;
                return;
            }

            if (!double.TryParse(txtAz.Text, out az))
            {
                MessageBox.Show(Resources.InvalidAzValue, SharedResources.TELESCOPE_DRIVER_NAME);
                DialogResult = DialogResult.None;
                return;
            }

            try {
                GeminiHardware.ParkAlt = alt;
                GeminiHardware.ParkAz  = az;
            } catch (Exception ex)
            {
                MessageBox.Show(Resources.CannotSetParkCoordinates + " " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME);
                DialogResult = DialogResult.None;
                return;
            }

            if (rbNoSlew.Checked) GeminiHardware.ParkPosition = GeminiHardware.GeminiParkMode.NoSlew;
            if (rbAltAz.Checked) GeminiHardware.ParkPosition = GeminiHardware.GeminiParkMode.SlewAltAz;
            if (rbCWD.Checked) GeminiHardware.ParkPosition = GeminiHardware.GeminiParkMode.SlewCWD;
            if (rbHome.Checked) GeminiHardware.ParkPosition = GeminiHardware.GeminiParkMode.SlewHome;
            DialogResult = DialogResult.OK;
        }

        private void frmParkPosition_FormClosed(object sender, FormClosedEventArgs e)
        {
            GeminiHardware.Profile = null;
        }
    }
}
