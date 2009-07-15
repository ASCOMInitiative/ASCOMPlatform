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


        public frmMain()
        {
            InitializeComponent();

        }
        public void DoSetupDialog()
        {
            SetupDialogForm setupForm = new SetupDialogForm();

            setupForm.ComPort = TelescopeHardware.ComPort;

            setupForm.Elevation = TelescopeHardware.Elevation;
            setupForm.Latitude = TelescopeHardware.Latitude;
            setupForm.Longitude = TelescopeHardware.Longitude;

            DialogResult ans = setupForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {

                TelescopeHardware.ComPort = setupForm.ComPort;

                TelescopeHardware.Elevation = setupForm.Elevation;
                TelescopeHardware.Latitude = setupForm.Latitude;
                TelescopeHardware.Longitude = setupForm.Longitude;
                
            }

            setupForm.Dispose();
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            DoSetupDialog();
            SetSlewButtons();
        }

        private void SetSlewButtons()
        {
            if (TelescopeHardware.SouthernHemisphere)
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
                string text = AstronomyFunctions.ConvertDoubleToHMS(value);
                try{this.Invoke(setText, text);}
                catch { }
                
           
            }
        }
        public double RightAscension
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetRaText);
                string text = AstronomyFunctions.ConvertDoubleToHMS(value);
                try { this.Invoke(setText, text); }
                catch { }
            }
        }
        public double Declination
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetDecText);
                string text = AstronomyFunctions.ConvertDoubleToDMS(value);
                try { this.Invoke(setText, text); }
                catch { }
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            SetSlewButtons();
            TelescopeHardware.Start();
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
            DoSetupDialog();
        }

        private void mountParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TelescopeHardware.Connected)
            {
                MountParameters parametersForm = new MountParameters();


                DialogResult ans = parametersForm.ShowDialog(this);

                if (ans == DialogResult.OK)
                {

                    

                }

                parametersForm.Dispose();
            }
        }
    }
}