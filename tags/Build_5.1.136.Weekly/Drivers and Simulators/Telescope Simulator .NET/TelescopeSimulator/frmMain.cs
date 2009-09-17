using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.TelescopeSimulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);


        public frmMain()
        {
            InitializeComponent();
        }
        public void DoSetupDialog()
        {
            SetupDialogForm setupForm = new SetupDialogForm();

            setupForm.VersionOneOnly = TelescopeHardware.VersionOneOnly;
            setupForm.CanFindHome = TelescopeHardware.CanFindHome;
            setupForm.CanPark = TelescopeHardware.CanPark;
            setupForm.NumberMoveAxis = TelescopeHardware.NumberMoveAxis;
            setupForm.OnTop = TelescopeHardware.OnTop;
            setupForm.CanPulseGuide = TelescopeHardware.CanPulseGuide;
            setupForm.CanSetEquatorialRates = TelescopeHardware.CanSetEquatorialRates;
            setupForm.CanSetGuideRates = TelescopeHardware.CanSetGuideRates;
            setupForm.CanSetPark = TelescopeHardware.CanSetPark;
            setupForm.CanSetSideOfPier = TelescopeHardware.CanSetSideOfPier;

            DialogResult ans = setupForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {
                TelescopeHardware.VersionOneOnly = setupForm.VersionOneOnly;
                TelescopeHardware.CanFindHome = setupForm.CanFindHome;
                TelescopeHardware.CanPark = setupForm.CanPark;
                TelescopeHardware.NumberMoveAxis = setupForm.NumberMoveAxis;
                TelescopeHardware.OnTop = setupForm.OnTop;
                TelescopeHardware.CanPulseGuide = setupForm.CanPulseGuide;
                TelescopeHardware.CanSetEquatorialRates = setupForm.CanSetEquatorialRates;
                TelescopeHardware.CanSetGuideRates = setupForm.CanSetGuideRates;
                TelescopeHardware.CanSetPark = setupForm.CanSetPark;
                TelescopeHardware.CanSetSideOfPier = setupForm.CanSetSideOfPier;

                this.TopMost = setupForm.OnTop;
            }

            setupForm.Dispose();
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            DoSetupDialog();
        }

        private void buttonTraffic_Click(object sender, EventArgs e)
        {
            SharedResources.TrafficForm.Show();
        }
    }
}