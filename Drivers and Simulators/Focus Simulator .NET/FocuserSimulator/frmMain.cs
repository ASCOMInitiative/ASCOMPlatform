using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ASCOM.FocuserSimulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);

        public Random xRand;
        public int OldTemp = 999;

        public frmMain()
        {
            InitializeComponent();
            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
            xRand = new Random();
        }

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Properties.Settings.Default.sAbsolute)
            {
                LabelPosition.Text = Properties.Settings.Default.sPosition.ToString();
                LabelMinPosition.Visible = true;
                LabelMaxStep.Visible = true;
                LabelMaxStep.Text = Properties.Settings.Default.sMaxStep.ToString();
                progressBar1.Visible = true;
            }
            else
            {
                LabelPosition.Text = "No position feedback";
                LabelMinPosition.Visible = false;
                LabelMaxStep.Visible = false;
                progressBar1.Visible = false;
            }
            SetupButton.Enabled = !Properties.Settings.Default.IsMoving;
            TimerTempComp.Interval = (int)Properties.Settings.Default.sTempCompPeriod * 1000;
            TimerTempComp.Enabled = Properties.Settings.Default.sTempComp;
            Application.DoEvents();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            FocuserHardware.DoSetup();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //frmMain.ActiveForm.Width = 186;
            Properties.Settings.Default.Reload();
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
            int Delta;
            int xTemp = (int)((int)Properties.Settings.Default.sTempMin + 10 * xRand.NextDouble());
            TextTemp.Text = xTemp.ToString() + "°";
            if (OldTemp == 999 || OldTemp == xTemp) { OldTemp = xTemp; return; }   // First time : memorise temp & no move
            Delta = xTemp - OldTemp;
            FocuserHardware.IsTempCompMove = true;
            if (Properties.Settings.Default.sAbsolute) FocuserHardware.Move((int)Properties.Settings.Default.sPosition+Delta*(int)Properties.Settings.Default.sStepPerDeg);
            else FocuserHardware.Move(Delta*(int)Properties.Settings.Default.sStepPerDeg);
            FocuserHardware.IsTempCompMove = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LogTxt = "";
            Properties.Settings.Default.Save();
        }
    }
}