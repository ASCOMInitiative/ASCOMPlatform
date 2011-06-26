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
            txtAlt.Text = GeminiHardware.Instance.ParkAlt.ToString();
            txtAz.Text = GeminiHardware.Instance.ParkAz.ToString();
            switch (GeminiHardware.Instance.ParkPosition)
            {
                case GeminiHardwareBase.GeminiParkMode.SlewAltAz: rbAltAz.Checked = true; break;
                case GeminiHardwareBase.GeminiParkMode.SlewCWD : this.rbCWD.Checked = true; break;
                case GeminiHardwareBase.GeminiParkMode.SlewHome: this.rbHome.Checked = true; break;
                default: rbNoSlew.Checked = true; break;
            }
        }

        private void pbGetPos_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
                try
                {
                    txtAlt.Text = GeminiHardware.Instance.Altitude.ToString("0.00");
                    txtAz.Text = GeminiHardware.Instance.Azimuth.ToString("0.00");
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
                GeminiHardware.Instance.ParkAlt = alt;
                GeminiHardware.Instance.ParkAz  = az;
            } catch (Exception ex)
            {
                MessageBox.Show(Resources.CannotSetParkCoordinates + " " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME);
                DialogResult = DialogResult.None;
                return;
            }

            if (rbNoSlew.Checked) GeminiHardware.Instance.ParkPosition = GeminiHardwareBase.GeminiParkMode.NoSlew;
            if (rbAltAz.Checked) GeminiHardware.Instance.ParkPosition = GeminiHardwareBase.GeminiParkMode.SlewAltAz;
            if (rbCWD.Checked) GeminiHardware.Instance.ParkPosition = GeminiHardwareBase.GeminiParkMode.SlewCWD;
            if (rbHome.Checked) GeminiHardware.Instance.ParkPosition = GeminiHardwareBase.GeminiParkMode.SlewHome;
            DialogResult = DialogResult.OK;
        }

        private void frmParkPosition_FormClosed(object sender, FormClosedEventArgs e)
        {
            GeminiHardware.Instance.Profile = null;
        }
    }
}
