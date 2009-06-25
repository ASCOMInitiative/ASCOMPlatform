using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.FocuserSimulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FocuserHardware.DoSetup();
            LabelMaxStep.Text = Properties.Settings.Default.sMaxStep.ToString();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            LabelMaxStep.Text = Properties.Settings.Default.sMaxStep.ToString();

            Application.DoEvents();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FocuserHardware.Halt();
        }

        private void ButtonTraffic_Click(object sender, EventArgs e)
        {
            if (frmMain.ActiveForm.Height == 186) { frmMain.ActiveForm.Height = 404; }
            else { frmMain.ActiveForm.Height = 186; }
            Application.DoEvents();
        }

        private void TimerTempComp_Tick(object sender, EventArgs e)
        {
            FocuserHardware.IsTempCompMove = true;
            FocuserHardware.Move((int)Properties.Settings.Default.sStepPerDeg);
            FocuserHardware.IsTempCompMove = false;
        }
    }
}