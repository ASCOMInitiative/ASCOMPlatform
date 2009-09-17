using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{

    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);
        delegate void SetupDialogDelegate();

        Timer tmrUpdate = new Timer();

        public frmMain()
        {
            InitializeComponent();
            tmrUpdate.Interval = 2000;
            tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
            tmrUpdate.Start();
        }

        void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                RightAscension = GeminiHardware.RightAscension;
                Declination = GeminiHardware.Declination;
                SiderealTime = GeminiHardware.SiderealTime;
            }
        }


        private void _DoSetupTelescopeDialog()
        {
                    TelescopeSetupDialogForm setupForm = new TelescopeSetupDialogForm();

            setupForm.ComPort = GeminiHardware.ComPort;
            setupForm.BaudRate = GeminiHardware.BaudRate.ToString();

            setupForm.Elevation = GeminiHardware.Elevation;
            setupForm.Latitude = GeminiHardware.Latitude;
            setupForm.Longitude = GeminiHardware.Longitude;

            DialogResult ans = setupForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {

                GeminiHardware.ComPort = setupForm.ComPort;
                GeminiHardware.BaudRate = int.Parse(setupForm.BaudRate);

                GeminiHardware.Elevation = setupForm.Elevation;
                GeminiHardware.Latitude = setupForm.Latitude;
                GeminiHardware.Longitude = setupForm.Longitude;
                
            }

            setupForm.Dispose();
        }

        public void DoTelescopeSetupDialog()
        {
            this.Invoke(new SetupDialogDelegate(_DoSetupTelescopeDialog));
        }

        public static void DoFocuserSetupDialog()
        {
            FocuserSetupDialogForm setupForm = new FocuserSetupDialogForm();
            setupForm.ComPort = GeminiHardware.ComPort;
            setupForm.BaudRate = GeminiHardware.BaudRate.ToString();
            setupForm.ReverseDirection = GeminiHardware.ReverseDirection;
            setupForm.MaxIncrement = GeminiHardware.MaxIncrement;
            setupForm.StepSize = GeminiHardware.StepSize;
            setupForm.BrakeSize = GeminiHardware.BrakeSize;
            setupForm.BacklashSize = GeminiHardware.BacklashSize;
            setupForm.BacklashDirection = GeminiHardware.BacklashDirection;
            setupForm.Speed = GeminiHardware.Speed;

            DialogResult ans = setupForm.ShowDialog();

            if (ans == DialogResult.OK)
            {

                GeminiHardware.ComPort = setupForm.ComPort;
                GeminiHardware.BaudRate = int.Parse(setupForm.BaudRate);
                GeminiHardware.ReverseDirection = setupForm.ReverseDirection;
                GeminiHardware.MaxIncrement = setupForm.MaxIncrement;
                GeminiHardware.StepSize = setupForm.StepSize;
                GeminiHardware.BrakeSize = setupForm.BrakeSize;
                GeminiHardware.BacklashSize = setupForm.BacklashSize;
                GeminiHardware.BacklashDirection = setupForm.BacklashDirection;
                GeminiHardware.Speed = setupForm.Speed;
            }

            setupForm.Dispose();
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            DoTelescopeSetupDialog();
            SetSlewButtons();
        }

        private void SetSlewButtons()
        {
            if (GeminiHardware.SouthernHemisphere)
            {
                buttonSlew1.Text = "S";
                buttonSlew2.Text = "N";
                buttonSlew3.Text = "E";
                buttonSlew4.Text = "W";
            }
            else
            {
                buttonSlew1.Text = "N";
                buttonSlew2.Text = "S";
                buttonSlew3.Text = "E";
                buttonSlew4.Text = "W";
            }
        }


        public double SiderealTime
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetLstText);
                string text = GeminiHardware.m_Util.HoursToHMS(value,":",":",""); // .ConvertDoubleToHMS(value);
               
                try{this.Invoke(setText, text);}
                catch { }
                
           
            }
        }
        public double RightAscension
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetRaText);
                string text = GeminiHardware.m_Util.HoursToHMS(value, ":", ":", ""); 
                try { this.Invoke(setText, text); }
                catch { }
            }
        }
        public double Declination
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetDecText);
                string text = GeminiHardware.m_Util.DegreesToDMS(value, ":", ":", ""); 
                try { this.Invoke(setText, text); }
                catch { }
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            SetSlewButtons();
            //GeminiHardware.Start();
        }

        #region Thread Safe Callback Functions
        private void SetLstText(string text)
        {
            labelLst.Text = text;
        }
        private void SetRaText(string text)
        {
            labelRa.Text = text;
        }
        private void SetDecText(string text)
        {
            labelDec.Text = text;
        }

        #endregion

        private void ButtonSetup_Click_1(object sender, EventArgs e)
        {
            ButtonSetup.ContextMenuStrip.Show(Cursor.Position);
        }

        private void setupDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoTelescopeSetupDialog();
        }

        private void mountParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                MountParameters parametersForm = new MountParameters();


                DialogResult ans = parametersForm.ShowDialog(this);

                if (ans == DialogResult.OK)
                {

                }
                parametersForm.Dispose();
            }
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (!GeminiHardware.Connected)
            {
                GeminiHardware.Connected = true;
                if (!GeminiHardware.Connected)
                    MessageBox.Show("Cannot connect to telescope");
                else
                    this.ButtonConnect.Text = "Disconnect";
            }
            else
            {
                GeminiHardware.Connected = false;
                if (GeminiHardware.Connected != false)
                    MessageBox.Show("Cannot disconnect from telescope");
                else
                    this.ButtonConnect.Text = "Connect";
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GeminiHardware.Connected)
                GeminiHardware.Connected = false;
        }

        private void focuserSetupDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoFocuserSetupDialog();
        }

        private void ButtonPark_Click(object sender, EventArgs e)
        {
            GeminiHardware.DoCommandResult(":hC", 1000, false);
        }
    }
}