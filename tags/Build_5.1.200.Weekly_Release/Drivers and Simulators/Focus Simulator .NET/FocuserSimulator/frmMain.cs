using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ASCOM.FocuserSimulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);

        public Random xRand;
        public int OldTemp = 999;
        public TextBoxTraceListener xLog;

        public frmMain()
        {
            InitializeComponent();
            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
            xRand = new Random();
            xLog = new TextBoxTraceListener(LogBox);
            Trace.Listeners.Add(xLog);
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
            TextTemp.Visible = Properties.Settings.Default.sTempComp;
            Application.DoEvents();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            FocuserHardware.DoSetup();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            FocuserHardware.MyLog(FocuserHardware.eLogKind.LogOther, "Init...");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FocuserHardware.Halt();
        }

        private void ButtonTraffic_Click(object sender, EventArgs e)
        {
            if (frmMain.ActiveForm.Height == 207) { frmMain.ActiveForm.Height = 434; }
            else { frmMain.ActiveForm.Height = 207; }
            Application.DoEvents();
        }

        private void TimerTempComp_Tick(object sender, EventArgs e)
        {
            int Delta;
            int xTemp = (int)((int)Properties.Settings.Default.sTempMin + 10 * xRand.NextDouble());
            TextTemp.Text = xTemp.ToString() + "�";
            if (OldTemp == 999 || OldTemp == xTemp) 
            { 
                OldTemp = xTemp;
                FocuserHardware.MyLog(FocuserHardware.eLogKind.LogTemp, "First time or same t�. No move.");
                return; 
            }   // First time or same t� : memorise temp & no move
            if ((Properties.Settings.Default.sTempMin > xTemp) || // Probed t� not in the interval (see SetupDialog()) so don't move
                (xTemp > Properties.Settings.Default.sTempMax)) 
            {
                FocuserHardware.MyLog(FocuserHardware.eLogKind.LogTemp, "Probed t� not in range. No move.");
                return; 
            } 
            Delta = xTemp - OldTemp;
            OldTemp = xTemp;
            FocuserHardware.IsTempCompMove = true;
            if (Properties.Settings.Default.sAbsolute) FocuserHardware.Move((int)Properties.Settings.Default.sPosition+Delta*(int)Properties.Settings.Default.sStepPerDeg);
            else FocuserHardware.Move(Delta*(int)Properties.Settings.Default.sStepPerDeg);
            FocuserHardware.IsTempCompMove = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LogBox.Clear();
        }
    }

    #region TextBox TraceListener
    // Code by Adam Crawford, MIT License

    public class TextBoxTraceListener : TraceListener
    {
        private System.Windows.Forms.TextBox _target;
        private StringSendDelegate _invokeWrite;

        public TextBoxTraceListener(System.Windows.Forms.TextBox target)
        {
            _target = target;
            _invokeWrite = new StringSendDelegate(SendString);
        }

        public override void Write(string message)
        {
            _target.Invoke(_invokeWrite, new object[] { message });
        }

        public override void WriteLine(string message)
        {
            _target.Invoke(_invokeWrite, new object[] { message + Environment.NewLine });
        }

        private delegate void StringSendDelegate(string message);
        private void SendString(string message)
        {
            _target.Text += message;
        }
    }
    #endregion
}