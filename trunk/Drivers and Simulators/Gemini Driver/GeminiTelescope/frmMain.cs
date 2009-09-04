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
        delegate void InfoBaloonDelegate(string title, string text, ToolTipIcon icon);

        delegate void OnConnectDelegate(bool Connected, int Client);

        Timer tmrUpdate = new Timer();
        Timer tmrBaloon = new Timer();

        string m_LastError = "";

        ContextMenu m_BaloonMenu = null;

        Color m_ActiveBkColor = Color.FromArgb(255, 0, 0);
        Color m_InactiveBkColor = Color.FromArgb(64,0,0);
        Color m_ActiveForeColor = Color.FromArgb(0, 255, 255);
        Color m_InactiveForeColor = Color.FromArgb(128, 64, 64);

        int m_UpdateCount = 0;
        bool m_ShowNotifications = true;

        frmStatus m_StatusForm  = null;

        public frmMain()
        {
            InitializeComponent();
            tmrUpdate.Interval = 2000;
            tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
            tmrUpdate.Start();
            GeminiHardware.OnConnect += new ConnectDelegate(OnConnectEvent);
            GeminiHardware.OnError += new ErrorDelegate(OnError);
            m_BaloonMenu = new ContextMenu();

            MenuItem notifyMenu = new MenuItem("Show Notifications", new EventHandler(ShowNotificationsMenu));
            notifyMenu.Name = "Notifications";
            notifyMenu.Checked = m_ShowNotifications;

            MenuItem controlMenu = new MenuItem("Hand Controller...", new EventHandler(ControlPanelMenu));
            controlMenu.Name = "Control";
            controlMenu.Checked = this.Visible;

            m_BaloonMenu.MenuItems.AddRange(new MenuItem[] { 
                controlMenu,
                new MenuItem("Configure Telescope...", new EventHandler(ConfigureTelescopeMenu)),

                new MenuItem("Configure Focuser...", new EventHandler(ConfigureFocuserMenu)),
            new MenuItem("-"),
            notifyMenu,
            new MenuItem("-"),
            new MenuItem("Exit", new EventHandler(ExitMenu))
            });

            BaloonIcon.ContextMenu = m_BaloonMenu;
            BaloonIcon.MouseClick += new MouseEventHandler(BaloonIcon_MouseClick);

            BaloonIcon.Visible = true;
            tmrBaloon.Tick += new EventHandler(tmrBaloon_Tick);
            BaloonIcon.MouseDoubleClick += new MouseEventHandler(BaloonIcon_MouseDoubleClick);
            BaloonIcon.MouseMove += new MouseEventHandler(BaloonIcon_MouseMove);
            GeminiHardware.OnSafetyLimit += new SafetyDelegate(OnSafetyLimit);
        }

        void BaloonIcon_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_StatusForm == null || !m_StatusForm.Visible)
            {
                Screen scr = Screen.FromPoint(Cursor.Position);
                ShowStatus(new Point(scr.WorkingArea.Right, scr.WorkingArea.Bottom), true);
            }
        }

        void BaloonIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ControlPanelMenu(sender, e);
        }

        private void ShowStatus(Point pt, bool autoHide)
        {
            if (m_StatusForm != null) m_StatusForm.Close();
            m_StatusForm = new frmStatus();

            Screen scr = Screen.FromPoint(pt);

            Point top = (pt);
            top.Y -= m_StatusForm.Bounds.Height + 32;
            top.X -= 32;

            top.Y = Math.Min(top.Y, scr.WorkingArea.Height - m_StatusForm.Bounds.Height - 32);
            top.X = Math.Min(top.X, scr.WorkingArea.Width - m_StatusForm.Bounds.Width - 32);

            m_StatusForm.Location = top;

            m_StatusForm.Visible = true;
            m_StatusForm.AutoHide = autoHide;
            m_StatusForm.Show();

            /*
            if (BaloonIcon.ContextMenuStrip != null) return;           
            BaloonIcon.ContextMenu.Show(this, this.PointToClient(Cursor.Position));
          */
        }

        void BaloonIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //ShowStatus(Cursor.Position);
        }

        void ExitMenu(object sender, EventArgs e)
        {
            this.Close();
        }

        void ControlPanelMenu(object sender, EventArgs e)
        {
            if (this.Visible)
            { this.Hide(); }
            else
            { 
                this.Show();
                this.WindowState = FormWindowState.Normal;
                SharedResources.SetTopWindow(this);
            }

            m_BaloonMenu.MenuItems["Control"].Checked = this.Visible;
        }

        void ConfigureTelescopeMenu(object sender, EventArgs e)
        {
            _DoSetupTelescopeDialog();
        }

        void ConfigureFocuserMenu(object sender, EventArgs e)
        {
            _DoFocuserSetupDialog();
        }

        void ShowNotificationsMenu(object sender, EventArgs e)
        {
            m_ShowNotifications = !m_ShowNotifications;
            m_BaloonMenu.MenuItems["Notifications"].Checked = m_ShowNotifications;
        }

        /// <summary>
        /// hide the baloon text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmrBaloon_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrBaloon.Stop();
                BaloonIcon.Visible = false;
                BaloonIcon.Visible = true;
            }
            catch { }
        }

        /// <summary>
        /// GeminiHardware error event
        /// </summary>
        /// <param name="from"></param>
        /// <param name="msg"></param>
        void OnError(string from, string msg)
        {
            m_LastError = msg;
             this.Invoke(new InfoBaloonDelegate(SetBaloonText), new object[] {from, msg, ToolTipIcon.Error});
        }

        void SetBaloonText(string title, string text, ToolTipIcon icon)
        {
            if (m_ShowNotifications)
            {
                BaloonIcon.ShowBalloonTip(4000, text, title, icon);
                // time to turn off the baloon text, since Windows has a minimum of about 20-30 seconds before
                // the message turns off on its own while the task bar is visible:
                tmrBaloon.Interval = 4000;
                tmrBaloon.Start();
            }
        }

        /// <summary>
        /// Client connected or disconnected event
        /// </summary>
        /// <param name="Connected"></param>
        /// <param name="Clients"></param>
        void ConnectStateChanged(bool Connected, int Clients)
        {
            if (Connected && Clients == 1)  // first client to connect, change UI to show the connected status
            {
                ButtonConnect.Text = "Disconnect";

                SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, "Mount is connected", ToolTipIcon.Info);
            }
            if (!Connected && Clients == 0) // last client to disconnect
            {
                ButtonConnect.Text = "Connect";
                labelLst.Text = "00:00:00";
                labelRa.Text = "00:00:00";
                labelDec.Text = "+00:00:00";
                SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, "Mount is disconnected", ToolTipIcon.Info);
            }        
        }

        /// <summary>
        /// Connect/Disconnect event from GeminiHardware, usually called on the background worker thread
        /// </summary>
        /// <param name="Connected"></param>
        /// <param name="Clients"></param>
        void OnConnectEvent(bool Connected, int Clients)
        {
            this.Invoke(new ConnectDelegate(ConnectStateChanged), new object[] {Connected, Clients});
        }

        /// <summary>
        /// Safety limit reached event
        /// </summary>
        void OnSafetyLimit()
        {
            if (GeminiHardware.Connected)
                SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, "SAFETY LIMIT IS REACHED! Mount stopped.", ToolTipIcon.Warning);
            // may want to add some optional sound/alert here to notify the user!
        }

        void OnSafetyLimitEvent()
        {
            this.Invoke(new SafetyDelegate(OnSafetyLimit));
        }

        void tmrUpdate_Tick(object sender, EventArgs e)
        {

            if (GeminiHardware.Connected)
            {
                m_UpdateCount++;

                RightAscension = GeminiHardware.RightAscension;
                Declination = GeminiHardware.Declination;
                SiderealTime = GeminiHardware.SiderealTime;

                labelSlew.BackColor = (GeminiHardware.Velocity == "S"? m_ActiveBkColor : m_InactiveBkColor);

                // blink the text while slewing is active:
                Color active = ((m_UpdateCount & 1 )==0? m_ActiveForeColor : m_InactiveForeColor );

                labelSlew.ForeColor = (GeminiHardware.Velocity == "S" ? active : m_InactiveForeColor);
                labelPARK.BackColor = (GeminiHardware.AtPark ? m_ActiveBkColor : m_InactiveBkColor);
                labelPARK.ForeColor = (GeminiHardware.AtPark? m_ActiveForeColor : m_InactiveForeColor);

                switch (GeminiHardware.Velocity)
                {
                    case "S": labelSlew.Text = "SLEW"; break;
                    case "C": labelSlew.Text = "CENTER"; break;
                    case "N": labelSlew.Text = "STOP"; break;
                    default : labelSlew.Text = "TRACK"; break;
                }
                //add an indicator for a "safety limit alert".
                if (GeminiHardware.AtSafetyLimit)
                {
                    labelSlew.Text = "LIMIT!";
                    labelSlew.ForeColor = m_ActiveForeColor;
                    labelSlew.BackColor = m_ActiveBkColor;
                }

            }

            m_BaloonMenu.MenuItems["Control"].Checked = this.Visible;

            string tooltip = "Gemini is ";
            if (GeminiHardware.Connected)
            {
                tooltip += "connected\r\n" + (GeminiHardware.BaudRate.ToString()) + "b/s on " + GeminiHardware.ComPort;
                tooltip += "\r\nStatus: " + labelSlew.Text;

            }
            else
                tooltip+="not connected";


            BaloonIcon.Text = ""; // tooltip;    
        }


        private void _DoSetupTelescopeDialog()
        {
                    
            TelescopeSetupDialogForm setupForm = new TelescopeSetupDialogForm();

            setupForm.ComPort = GeminiHardware.ComPort;
            setupForm.BaudRate = GeminiHardware.BaudRate.ToString();

            setupForm.GpsBaudRate = GeminiHardware.GpsBaudRate.ToString();
            setupForm.GpsComPort = GeminiHardware.GpsComPort;
            setupForm.GpsUpdateClock = GeminiHardware.GpsUpdateClock;

            setupForm.Elevation = GeminiHardware.Elevation;
            setupForm.Latitude = GeminiHardware.Latitude;
            setupForm.Longitude = GeminiHardware.Longitude;

            setupForm.UseGeminiSite = GeminiHardware.UseGeminiSite;
            setupForm.UseGeminiTime = GeminiHardware.UseGeminiTime;

            setupForm.BootMode = GeminiHardware.BootMode;


            DialogResult ans = setupForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {
                try
                {
                    GeminiHardware.ComPort = setupForm.ComPort;
                    GeminiHardware.BaudRate = int.Parse(setupForm.BaudRate);

                    GeminiHardware.Elevation = setupForm.Elevation;
                    GeminiHardware.Latitude = setupForm.Latitude;
                    GeminiHardware.Longitude = setupForm.Longitude;

                    GeminiHardware.UseGeminiTime = setupForm.UseGeminiTime;
                    GeminiHardware.UseGeminiSite = setupForm.UseGeminiSite;

                    GeminiHardware.BootMode = setupForm.BootMode;

                    int gpsBaudRate;
                    int.TryParse(setupForm.GpsBaudRate, out gpsBaudRate);
                    GeminiHardware.GpsBaudRate = gpsBaudRate;
                    GeminiHardware.GpsComPort = setupForm.GpsComPort;
                    GeminiHardware.GpsUpdateClock = setupForm.GpsUpdateClock;

                }
                catch
                {
                    MessageBox.Show("Settings are invalid", SharedResources.TELESCOPE_DRIVER_NAME);
                }
            }

            setupForm.Dispose();
        }
        private void _DoFocuserSetupDialog()
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
                try
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
                catch
                {
                    MessageBox.Show("Settings are invalid", SharedResources.TELESCOPE_DRIVER_NAME);
                }
            }

            setupForm.Dispose();
        }


        public void DoTelescopeSetupDialog()
        {
            this.Invoke(new SetupDialogDelegate(_DoSetupTelescopeDialog));
        }
        public void DoFocuserSetupDialog()
        {
            this.Invoke(new SetupDialogDelegate(_DoFocuserSetupDialog));
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
            SharedResources.SetTopWindow(this);

            SetSlewButtons();
            this.Hide();
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
                try
                {
                    GeminiHardware.Connected = true;
                }
                catch { }
                if (!GeminiHardware.Connected)
                    MessageBox.Show("Cannot connect to Gemini!\r\n"+m_LastError, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK,   MessageBoxIcon.Hand);
                else
                    this.ButtonConnect.Text = "Disconnect";
            }
            else
            {
                if (GeminiHardware.Clients > 1)
                {
                    DialogResult res = MessageBox.Show("Other programs are still connected. Are you sure you want to disconnect from the mount?",
                        SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                    if (res != DialogResult.Yes)
                        return;
                }
                try
                {
                    GeminiHardware.Connected = false;
                }
                catch { }
                if (GeminiHardware.Connected != false)
                    MessageBox.Show("Cannot disconnect from telescope", SharedResources.TELESCOPE_DRIVER_NAME);
                else
                    this.ButtonConnect.Text = "Connect";
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GeminiHardware.Connected && GeminiHardware.Clients > 0)
            {
                DialogResult res = MessageBox.Show("Gemini connection"+(GeminiHardware.Clients>1? "s are": " is") +" still active. Are you sure you want to disconnect and exit?",
                    SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            GeminiHardware.Connected = false;
        }

        private void focuserSetupDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
           DoFocuserSetupDialog();
        }

        private void ButtonPark_Click(object sender, EventArgs e)
        {
            GeminiHardware.DoCommandResult(":hC", 5000, false);
        }

        private void buttonSlew0_MouseClick(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                GeminiHardware.DoCommand(":Q",false);
            }
        }

        private void ButtonFlip_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                GeminiHardware.DoCommand(":Mf", false);
            }
        }

        private void buttonSlew1_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                string[] cmds = { null, null };

                if (RadioButtonGuide.Checked) cmds[0] = ":RG";
                if (RadioButtonCenter.Checked) cmds[0] = ":RC";
                if (RadioButtonSlew.Checked) cmds[0] = ":RS";

                if (CheckBoxFlipDec.Checked)
                {
                    cmds[1] = ":Ms";
                }
                else
                {
                    cmds[1] = ":Mn";
                }

                GeminiHardware.DoCommand(cmds, false);

            }
        }

        private void buttonSlew1_MouseUp(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {

                if (CheckBoxFlipDec.Checked)
                {
                    GeminiHardware.DoCommand(":Qs", false);
                }
                else
                {
                    GeminiHardware.DoCommand(":Qn", false);
                }
            }
        }

        private void buttonSlew2_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                string[] cmds = { null, null };

                if (RadioButtonGuide.Checked) cmds[0] = ":RG";
                if (RadioButtonCenter.Checked) cmds[0] = ":RC";
                if (RadioButtonSlew.Checked) cmds[0] = ":RS";
                if (CheckBoxFlipDec.Checked)
                    if (CheckBoxFlipDec.Checked)
                    {
                        cmds[1] = ":Mn";
                    }
                    else
                    {
                        cmds[1] = ":Ms";
                    }

                GeminiHardware.DoCommand(cmds, false);
            }
        }

        private void buttonSlew2_MouseUp(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                if (CheckBoxFlipDec.Checked)
                {
                    GeminiHardware.DoCommand(":Qn", false);
                }
                else
                {
                    GeminiHardware.DoCommand(":Qs", false);
                }
            }
        }

        private void buttonSlew4_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                string[] cmds = { null, null };

                if (RadioButtonGuide.Checked) cmds[0] = ":RG";
                if (RadioButtonCenter.Checked) cmds[0] = ":RC";
                if (RadioButtonSlew.Checked) cmds[0] = ":RS";
                if (CheckBoxFlipRa.Checked)
                {
                    cmds[1] = ":Me";
                }
                else
                {
                    cmds[1] = ":Mw";
                }
                GeminiHardware.DoCommand(cmds, false);
            }
        }

        private void buttonSlew4_MouseUp(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                if (CheckBoxFlipRa.Checked)
                {
                    GeminiHardware.DoCommand(":Qe", false);
                }
                else
                {
                    GeminiHardware.DoCommand(":Qw", false);
                }
            }
        }

        private void buttonSlew3_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                string[] cmds = { null, null };

                if (RadioButtonGuide.Checked) cmds[0] = ":RG";
                if (RadioButtonCenter.Checked) cmds[0] = ":RC";
                if (RadioButtonSlew.Checked) cmds[0] = ":RS";
                if (CheckBoxFlipRa.Checked)
                {
                    cmds[1] = ":Mw";
                }
                else
                {
                    cmds[1] = ":Me";
                }
                GeminiHardware.DoCommand(cmds, false);

            }
        }

        private void buttonSlew3_MouseUp(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                if (CheckBoxFlipRa.Checked)
                {
                    GeminiHardware.DoCommand(":Qw", false);
                }
                else
                {
                    GeminiHardware.DoCommand(":Qe", false);
                }
            }
        }

    }
}