using System;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class frmSetup : Form
    {
        private float updateInterval;
        private float rotationRate;
        private float syncOffset;

        #region Initialisation and form load

        public frmSetup()
        {
            InitializeComponent();
            TxtSyncOfffset.Validated += TxtSyncOfffset_Validated;

        }

        private void TxtSyncOfffset_Validated(object sender, EventArgs e)
        {
            if (!float.TryParse(TxtSyncOfffset.Text, out syncOffset))
            {
                syncOffset = 0.0F;
                TxtSyncOfffset.Text = "";
            }

            UpdateUI();

        }

        private void frmSetup_Load(object sender, EventArgs e)
        {
            // Bring this form to the front of the screen
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        #endregion

        #region Public properties

        public bool CanReverse
        {
            get { return chkCanReverse.Checked; }
            set
            {
                chkCanReverse.Checked = value;
                UpdateUI();
            }
        }

        public bool Reverse
        {
            get { return chkReverse.Checked; }
            set { chkReverse.Checked = value; }
        }

        public float RotationRate
        {
            get { return rotationRate; }
            set
            {
                rotationRate = value;
                txtRotationRate.Text = value.ToString("0.0");
            }
        }

        public float SyncOffset
        {
            get { return syncOffset; }
            set
            {
                syncOffset = value;
                TxtSyncOfffset.Text = value.ToString("0.0");
            }
        }

        public float StepSize
        {
            get { return (rotationRate / 1000) * updateInterval; }
        }

        public float UpdateInterval
        {
            set
            {
                updateInterval = value;
                UpdateUI();
            }
        }

        #endregion

        #region Event handlers

        private void cmdOK_Click(object sender, EventArgs e)
        {

        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {

        }

        private void chkCanReverse_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void txtRotationRate_TextChanged(object sender, EventArgs e)
        {
            if (!float.TryParse(txtRotationRate.Text, out rotationRate))
            {
                rotationRate = 0.0F;
                txtRotationRate.Text = "";
            }
            UpdateUI();
        }

        private void ChkCanSync_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        #endregion

        #region Private code

        private void UpdateUI()
        {
            if (!chkCanReverse.Checked)
            {
                chkReverse.Checked = false;
                chkReverse.Enabled = false;
            }
            else
            {
                chkReverse.Enabled = true;
            }

            TxtSyncOfffset.Enabled = true;

            lblStepSize.Text = "Step size " + this.StepSize.ToString("0.0") + " deg. (4 steps/sec.)";
        }

        #endregion 
    }
}