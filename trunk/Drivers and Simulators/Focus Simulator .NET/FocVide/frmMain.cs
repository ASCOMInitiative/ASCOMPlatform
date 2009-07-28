using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace ASCOM.FocVide
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);

        public TextBoxTraceListener xLog;

        public frmMain()
        {
            InitializeComponent();
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
                progressBar1.Value = (int)Properties.Settings.Default.sPosition;
            }
            else
            {
                LabelPosition.Text = "No position feedback";
                LabelMinPosition.Visible = false;
                LabelMaxStep.Visible = false;
                progressBar1.Visible = false;
            }
            HaltButton.Enabled = Properties.Settings.Default.IsMoving;
            HaltButton.Visible = Properties.Settings.Default.sEnableHalt;
            SetupButton.Enabled = !Properties.Settings.Default.IsMoving;
            Application.DoEvents();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            FocuserHardware.DoSetup();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            xLog = new TextBoxTraceListener(LogBox);
            Trace.Listeners.Add(xLog);
            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
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
            _target.AppendText(message);
        }
    }
    #endregion
}