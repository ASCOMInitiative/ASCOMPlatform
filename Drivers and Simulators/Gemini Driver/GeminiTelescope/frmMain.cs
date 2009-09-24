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
        Timer tmrJoystick = new Timer();

        string m_LastError = "";

        ContextMenu m_BaloonMenu = null;

        Color m_ActiveBkColor = Color.FromArgb(255, 0, 0);
        Color m_InactiveBkColor = Color.FromArgb(64,0,0);
        Color m_ActiveForeColor = Color.FromArgb(0, 255, 255);
        Color m_InactiveForeColor = Color.FromArgb(128, 64, 64);

        int m_UpdateCount = 0;
        bool m_ShowNotifications = true;

        frmStatus m_StatusForm  = null;

        Joystick m_Joystick = null;

        string m_JoystickRate = null;           // these keep last joystick rate and
        string m_JoystickDirection =null;      // direction commands issued 

        bool m_ExitFormMenuCall = false;

        public frmMain()
        {
            InitializeComponent();
            tmrUpdate.Interval = 2000;
            tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
            tmrUpdate.Start();
            GeminiHardware.OnConnect += new ConnectDelegate(OnConnectEvent);
            GeminiHardware.OnError += new ErrorDelegate(OnError);
            GeminiHardware.OnInfo += new ErrorDelegate(OnInfo);

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

                new MenuItem("Advanced Gemini Settings...", new EventHandler(AdvancedGeminiMenu)),

                new MenuItem("Configure Focuser...", new EventHandler(ConfigureFocuserMenu)),
            new MenuItem("-"),
            notifyMenu,
            new MenuItem("-"),
            new MenuItem("About Gemini Driver...", new EventHandler(aboutGeminiDriverToolStripMenuItem_Click)),
            new MenuItem("Exit", new EventHandler(ExitMenu))
            });

            BaloonIcon.ContextMenu = m_BaloonMenu;
            BaloonIcon.MouseClick += new MouseEventHandler(BaloonIcon_MouseClick);

            BaloonIcon.Visible = true;
            tmrBaloon.Tick += new EventHandler(tmrBaloon_Tick);
            BaloonIcon.MouseDoubleClick += new MouseEventHandler(BaloonIcon_MouseDoubleClick);
            BaloonIcon.MouseMove += new MouseEventHandler(BaloonIcon_MouseMove);
            GeminiHardware.OnSafetyLimit += new SafetyDelegate(OnSafetyLimit);

            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            toolTip1.UseAnimation = true;
            toolTip1.UseFading = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.ButtonConnect, "Connect to Gemini");
            toolTip1.SetToolTip(this.ButtonFlip, "Perform Meridian Flip");
            toolTip1.SetToolTip(this.ButtonPark, "Park mount");
            toolTip1.SetToolTip(this.ButtonSetup, "Setup Driver and Gemini");

            toolTip1.SetToolTip(this.buttonSlew1, "Slew DEC+");
            toolTip1.SetToolTip(this.buttonSlew2, "Slew DEC-");
            toolTip1.SetToolTip(this.buttonSlew3, "Slew RA-");
            toolTip1.SetToolTip(this.buttonSlew4, "Slew RA+");

            toolTip1.SetToolTip(this.RadioButtonCenter , "Use Centering speed");
            toolTip1.SetToolTip(this.RadioButtonGuide, "Use Guiding speed");
            toolTip1.SetToolTip(this.RadioButtonSlew , "Use Slew speed");

            toolTip1.SetToolTip(this.labelLst, "Mount Local Sidereal Time");
            toolTip1.SetToolTip(this.labelRa,  "Mount Right Ascension");
            toolTip1.SetToolTip(this.labelDec, "Mount Declination");

            toolTip1.SetToolTip(this.checkBoxTrack, "Stop/Start Tracking");
            toolTip1.SetToolTip(this.CheckBoxFlipDec, "Swap DEC buttons");
            toolTip1.SetToolTip(this.CheckBoxFlipRa, "Swap RA buttons");

            toolTip1.SetToolTip(this.checkboxPEC, "Enable/Disable Periodic Error Correction (PEC)");

            tmrJoystick.Tick += new EventHandler(tmrJoystick_Tick);

        }

        private void StartJoystick()
        {
            m_JoystickRate = null;
            m_JoystickRate = null;

            int[] joys = Joystick.Joysticks;
            if (joys != null && joys.Length > 0)
            {
                if (!string.IsNullOrEmpty(GeminiHardware.JoystickName))
                {
                    m_Joystick = new Joystick();
                    if (m_Joystick.Initialize(GeminiHardware.JoystickName))
                    {
                        tmrJoystick.Interval = 200;
                        tmrJoystick.Start();
                    }
                    else
                        SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, "Joystick is not available\r\n" + GeminiHardware.JoystickName, ToolTipIcon.Warning);
                }
            }
        }

        private uint m_PreviousJoystickButtonState = 0;
        private int[] m_AutoFireCount = new int[32];
        
        // joystic position corrresponding to mount G/C/S speeds:
        private double[] joystick_speeds = { 0.251, 0.501, 0.751 };

        void tmrJoystick_Tick(object sender, EventArgs e)
        {
            tmrJoystick.Stop();
            if (GeminiHardware.Connected && this.Visible && GeminiHardware.UseJoystick && !string.IsNullOrEmpty(GeminiHardware.JoystickName))
            {
                double x = m_Joystick.PosX;
                double y = m_Joystick.PosY;

                System.Diagnostics.Trace.WriteLine("JOYSTICK : X = " + x.ToString() + "   Y = " + y.ToString());

                uint buttons = m_Joystick.ButtonState;
                
                ProcessButtonPress(ref buttons, m_PreviousJoystickButtonState);
                
                m_PreviousJoystickButtonState = buttons;


                // joystick positions are reported from -1 to 1
                // 0 .. 0.25 means no movement (near center)
                // 0.25 .. 0.5 => move at guiding speed
                // 0.5 .. 0.75 => move at centering speed
                // 0.75 .. 1.0 => move at slew speed


                List<string> cmds = new List<string>();

                if (this.CheckBoxFlipRa.Checked) y = -y;
                if (this.CheckBoxFlipDec.Checked) x = -x;

                string dir, rate;

                // if last joystick rate was zero, but we are still moving at speed, then
                // the mount must be slowing down from a slew.. continue waiting
                if ((m_JoystickRate == null) && (GeminiHardware.Velocity == "S"))
                {
                    tmrJoystick.Start();
                    return;
                }

                // stop all slew -- joystick is centered
                if (Math.Abs(x) < 0.25 && Math.Abs(y) < 0.25)
                {
                    if (m_JoystickDirection != null || m_JoystickRate != null)  //already stopped, don't process this again
                    {
                        m_JoystickRate = null;
                        m_JoystickDirection = null;
                        GeminiHardware.DoCommand(":Q", false);
                    }
                    tmrJoystick.Start();
                    return;
                }

                // fixed speed selected, use the value in JoysticFixedSpeed (0-2 == G/C/S):
                if (!GeminiHardware.JoystickIsAnalog)
                {
                    double speed = joystick_speeds[GeminiHardware.JoystickFixedSpeed];
                    if (x < -0.25) x = -speed;
                    if (x > 0.25) x = speed;
                    if (y < -0.25) y = -speed;
                    if (y > 0.25) y = speed;
                }

                double val = Math.Max(Math.Abs(x), Math.Abs(y));

                if (val < 0.5) rate = ":RG";        // move at guiding speed
                else if (val < 0.75) rate = ":RC";   // centering speed
                else
                    rate = ":RS";                          // slewing speed

                if (Math.Abs(x) > Math.Abs(y))
                {
                    if (x < 0) dir = ":Me";
                    else dir = ":Mw";
                }
                else
                {
                    if (y < 0) dir = ":Ms";
                    else dir = ":Mn";
                }

                if (rate != m_JoystickRate || dir != m_JoystickDirection)
                {
                    // if we are moving at speed, we can't change rates/directions until
                    // the mount slows down to tracking speed. Issue move quit command, and 
                    // return until next time...
                    if (((GeminiHardware.Velocity == "S") && (m_JoystickRate == ":RS" || rate == ":RS")) ||
                         ((GeminiHardware.Velocity == "C") && (rate == ":RS")))
                    {
                        GeminiHardware.DoCommand(":Q", false);
                        m_JoystickRate = null;
                        m_JoystickDirection = null;
                        tmrJoystick.Start();
                        return;
                    }

                    cmds.Add(rate);
                    cmds.Add(dir);
                    m_JoystickDirection = dir;
                    m_JoystickRate = rate;
                }
                if (cmds.Count > 0) GeminiHardware.DoCommand(cmds.ToArray(), false);
                tmrJoystick.Start();
            }
        }

        private void ProcessButtonPress(ref uint buttons, uint prev_buttons)
        {
            uint mask = 1;

            for (int i = 0; i < 32; ++i, mask<<=1)
            {
                // if previously fired this
                if (m_AutoFireCount[i] > 0 && m_AutoFireCount[i] < 5)
                {
                    if ((buttons & mask) == 0) //cancel auto-fire -- button is now up
                    {
                        m_AutoFireCount[i] = 0;
                        continue;
                    }
                    buttons &= ~mask;   // remove button press, so next time we detect this as a new press

                    m_AutoFireCount[i]++;
                    continue; //wait for 5 cycles before continuous firing
                }

                // button changed state?
                if ((buttons & mask) != (prev_buttons & mask))
                {
                    if ((buttons & mask) == 0 && m_AutoFireCount[i] > 0)
                        m_AutoFireCount[i] = 0;

                    UserCommand.Execute(GeminiHardware.JoystickButtonMap[i], (buttons & mask) != 0);

                    // if this is the first button down in a while, let up
                    // and wait for 5 cycles before continually firing:
                    if ((buttons & mask) != 0 && m_AutoFireCount[i] == 0)
                    {
                        UserCommand.Execute(GeminiHardware.JoystickButtonMap[i], false);    //key up
                        buttons &= ~mask;   //remove it from state, so it fires the next time
                        m_AutoFireCount[i]++;
                    }
                }
            }
        }


        void BaloonIcon_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_StatusForm == null || !m_StatusForm.Visible)
            {
                Screen scr = Screen.FromPoint(Cursor.Position);
                GeminiHardware.ShowStatus(new Point(scr.WorkingArea.Right, scr.WorkingArea.Bottom), true);
            }
        }

        void BaloonIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ControlPanelMenu(sender, e);
        }


        void BaloonIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //ShowStatus(Cursor.Position);
        }

        void ExitMenu(object sender, EventArgs e)
        {
            m_ExitFormMenuCall = true;
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

        void AdvancedGeminiMenu(object sender, EventArgs e)
        {
            _DoAdvancedDialog();
        }

        private void _DoAdvancedDialog()
        {
            frmAdvancedSettings frmSettings = new frmAdvancedSettings();
            frmSettings.ShowDialog();
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
            if (this.InvokeRequired)
                this.BeginInvoke(new InfoBaloonDelegate(SetBaloonText), new object[] { from, msg, ToolTipIcon.Error });
            else
                SetBaloonText(from, msg, ToolTipIcon.Info);
        }

        void OnInfo(string from, string msg)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new InfoBaloonDelegate(SetBaloonText), new object[] { from, msg, ToolTipIcon.Info});
            else 
                SetBaloonText(from, msg, ToolTipIcon.Info);
        }

        void SetBaloonText(string title, string text, ToolTipIcon icon)
        {
            if (m_ShowNotifications)
            {
                tmrBaloon.Stop();

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
            if (Connected)
            {
                if (GeminiHardware.UseJoystick && !string.IsNullOrEmpty(GeminiHardware.JoystickName)) StartJoystick();
            }
            if (!Connected && Clients <= 0) // last client to disconnect
            {
                ButtonConnect.Text = "Connect";
                labelLst.Text = "00:00:00";
                labelRa.Text = "00:00:00";
                labelDec.Text = "+00:00:00";
                SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, "Mount is disconnected", ToolTipIcon.Info);
                tmrJoystick.Stop();
            }        
        }

        /// <summary>
        /// Connect/Disconnect event from GeminiHardware, usually called on the background worker thread
        /// </summary>
        /// <param name="Connected"></param>
        /// <param name="Clients"></param>
        void OnConnectEvent(bool Connected, int Clients)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new ConnectDelegate(ConnectStateChanged), new object[] { Connected, Clients });
            else
                ConnectStateChanged(Connected, Clients);
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
            if (this.InvokeRequired)
                this.BeginInvoke(new SafetyDelegate(OnSafetyLimit));
            OnSafetyLimit();
        }

        void tmrUpdate_Tick(object sender, EventArgs e)
        {

            if (GeminiHardware.Connected)
            {
                m_UpdateCount++;

                RightAscension = GeminiHardware.RightAscension;
                Declination = GeminiHardware.Declination;
                SiderealTime = GeminiHardware.SiderealTime;

                labelSlew.BackColor = (GeminiHardware.Velocity == "S" ? m_ActiveBkColor : m_InactiveBkColor);

                // blink the text while slewing is active:
                Color active = ((m_UpdateCount & 1) == 0 ? m_ActiveForeColor : m_InactiveForeColor);

                labelSlew.ForeColor = (GeminiHardware.Velocity == "S" ? active : m_InactiveForeColor);
                labelPARK.BackColor = (GeminiHardware.AtPark ? m_ActiveBkColor : m_InactiveBkColor);
                labelPARK.ForeColor = (GeminiHardware.AtPark ? m_ActiveForeColor : m_InactiveForeColor);

                switch (GeminiHardware.Velocity)
                {
                    case "S": labelSlew.Text = "SLEW"; break;
                    case "C": labelSlew.Text = "CENTER"; break;
                    case "N": labelSlew.Text = "STOP"; break;
                    default: labelSlew.Text = "TRACK"; break;
                }
                //add an indicator for a "safety limit alert".
                if (GeminiHardware.AtSafetyLimit)
                {
                    labelSlew.Text = "LIMIT!";
                    labelSlew.ForeColor = m_ActiveForeColor;
                    labelSlew.BackColor = m_ActiveBkColor;
                }

                pbStop.Visible = (GeminiHardware.Velocity == "S");  //only show Stop! button when slewing


            }
            else
            {
                labelSlew.ForeColor = m_InactiveForeColor;
                labelPARK.BackColor = m_InactiveBkColor;
                labelPARK.ForeColor = m_InactiveForeColor;

                labelSlew.Text = "STOP"; 
                pbStop.Visible = false;  //only show Stop! button when slewing

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

            checkBoxTrack.Checked = (GeminiHardware.Velocity == "N" ? false : true);

            byte pec = GeminiHardware.PECStatus;
            if ((pec & 1) != 0)
            {
                checkboxPEC.Enabled = true;
                checkboxPEC.Checked = true;
            }
            else if ((pec & 32) != 0) //data available
            {
                checkboxPEC.Enabled = true;
                checkboxPEC.Checked = false;
            }
            else
            {
                checkboxPEC.Enabled = false;
                checkboxPEC.Checked = false;
            }

            checkboxPEC.BackColor = (checkboxPEC.Enabled ? Color.Transparent : Color.FromArgb(64, 64, 64)) ;


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

            setupForm.ShowHandbox = GeminiHardware.ShowHandbox;

            setupForm.BootMode = GeminiHardware.BootMode;


            DialogResult ans = setupForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {
                string error = "";

                try
                {
                    GeminiHardware.ComPort = setupForm.ComPort;
                }
                catch 
                {
                    error += "COM Port, ";
                }

                try{ GeminiHardware.BaudRate = int.Parse(setupForm.BaudRate); }
                catch { error += "Baud Rate, ";}

                try {GeminiHardware.Elevation = setupForm.Elevation;}
                catch {error += "Elevation, ";}

                try { GeminiHardware.Latitude = setupForm.Latitude; }
                catch {error+="Latitude, "; }
                
                try {GeminiHardware.Longitude = setupForm.Longitude;}
                catch { error += "Longitude, ";  }

                GeminiHardware.UseGeminiTime = setupForm.UseGeminiTime;
                GeminiHardware.UseGeminiSite = setupForm.UseGeminiSite;

                GeminiHardware.ShowHandbox = setupForm.ShowHandbox;

                GeminiHardware.BootMode = setupForm.BootMode;

                int gpsBaudRate;
                if (!int.TryParse(setupForm.GpsBaudRate, out gpsBaudRate))
                    error += "GPS Baud Rate, ";
                else
                    GeminiHardware.GpsBaudRate = gpsBaudRate;
                try { GeminiHardware.GpsComPort = setupForm.GpsComPort; }
                catch { error += "GPS COM Port"; }
                GeminiHardware.GpsUpdateClock = setupForm.GpsUpdateClock;

                if (setupForm.UseJoystick && !string.IsNullOrEmpty(setupForm.JoystickName))
                {
                    GeminiHardware.UseJoystick = true;
                    GeminiHardware.JoystickName = setupForm.JoystickName;
                }
                else
                {
                    GeminiHardware.UseJoystick = false;
                }

                
                if (error != "")
                {
                    if (error.EndsWith(", ")) error = error.Substring(0, error.Length - 2);
                    MessageBox.Show("The following setting(s) are invalid:\r\n" + error, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Some settings are invalid", SharedResources.TELESCOPE_DRIVER_NAME);
                }
            }

            setupForm.Dispose();
        }


        public void DoTelescopeSetupDialog()
        {
            if (this.InvokeRequired)
                this.Invoke(new SetupDialogDelegate(_DoSetupTelescopeDialog));
            else _DoSetupTelescopeDialog();

        }
        public void DoFocuserSetupDialog()
        {
            if (InvokeRequired)
                this.Invoke(new SetupDialogDelegate(_DoFocuserSetupDialog));
            else _DoFocuserSetupDialog();
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
               
                try{
                    if (InvokeRequired) this.BeginInvoke(setText, text);
                    else
                        SetLstText(text);
                }
                catch { }
                
           
            }
        }
        public double RightAscension
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetRaText);
                string text = GeminiHardware.m_Util.HoursToHMS(value, ":", ":", ""); 
                try {

                    if (InvokeRequired)
                        this.BeginInvoke(setText, text);
                    else SetRaText(text);
                }
                catch { }
            }
        }
        public double Declination
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetDecText);
                string text = GeminiHardware.m_Util.DegreesToDMS(value, ":", ":", ""); 
                try {
                    if (InvokeRequired)
                        this.BeginInvoke(setText, text);
                    else
                        SetDecText(text);
                }
                catch { }
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            SharedResources.SetTopWindow(this);

            SetSlewButtons();
            if (!GeminiHardware.ShowHandbox && GeminiTelescope.m_bComStart) this.Hide();
            
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
            if (true /*GeminiHardware.Connected*/)
            {
                frmAdvancedSettings parametersForm = new frmAdvancedSettings();


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
                ButtonConnect.Enabled = false;
                ButtonConnect.Text = "Connecting...";
                ButtonConnect.Update();
                try
                {
                    GeminiHardware.Connected = true;
                }
                catch { }

                ButtonConnect.Enabled = true;
                if (!GeminiHardware.Connected)
                {
                    ButtonConnect.Text = "Connect";
                    ButtonConnect.Update();
                    MessageBox.Show("Cannot connect to Gemini!\r\n" + m_LastError, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
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
                
                while (GeminiHardware.Clients > 0)
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
            //If we hit the close X on the form then hide it. If called from exit menu then exit for real.
            if (!m_ExitFormMenuCall)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }
            m_ExitFormMenuCall = false;
            if (GeminiHardware.Connected && GeminiHardware.Clients > 0)
            {
                DialogResult res = MessageBox.Show("Gemini connection" + (GeminiHardware.Clients > 1 ? "s are" : " is") + " still active. Are you sure you want to disconnect and exit?",
                    SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            while (GeminiHardware.Clients > 0)  //disconnect all clients
                GeminiHardware.Connected = false;

            GeminiHardware.CloseStatusForm();
            
        }

        private void focuserSetupDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
           DoFocuserSetupDialog();
        }

        private void ButtonPark_Click(object sender, EventArgs e)
        {
            ButtonPark.ContextMenuStrip.Show(Cursor.Position);
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
                ((Control)sender).Capture = true;

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
            ((Control)sender).Capture = false;
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
                ((Control)sender).Capture = true;
                string[] cmds = { null, null };

                if (RadioButtonGuide.Checked) cmds[0] = ":RG";
                if (RadioButtonCenter.Checked) cmds[0] = ":RC";
                if (RadioButtonSlew.Checked) cmds[0] = ":RS";
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
            ((Control)sender).Capture = false;

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
                ((Control)sender).Capture = true;

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
            ((Control)sender).Capture = false;

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
                ((Control)sender).Capture = true;

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
            ((Control)sender).Capture = false;
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

        private void toolStripMenuParkHere_Click(object sender, EventArgs e)
        {
            GeminiHardware.DoCommandResult(":hN", GeminiHardware.MAX_TIMEOUT, false);
        }

        private void toolStripMenuParkCWD_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            GeminiHardware.DoCommandResult(":hC", GeminiHardware.MAX_TIMEOUT, false);
            GeminiHardware.WaitForHomeOrPark("Park");
            GeminiHardware.DoCommandResult(":hN", GeminiHardware.MAX_TIMEOUT, false);
            this.UseWaitCursor = false;
        }

        private void toolStripMenuParkHome_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            GeminiHardware.DoCommandResult(":hP", GeminiHardware.MAX_TIMEOUT, false);
            GeminiHardware.WaitForHomeOrPark("Home");

            GeminiHardware.DoCommandResult(":hN", GeminiHardware.MAX_TIMEOUT, false);
            this.UseWaitCursor = false;
        }

        private void checkBoxTrack_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                checkBoxTrack.Checked = !checkBoxTrack.Checked;

                if (!checkBoxTrack.Checked)
                    GeminiHardware.DoCommandResult(":hN", GeminiHardware.MAX_TIMEOUT, false);
                else
                    GeminiHardware.DoCommandResult(":hW", GeminiHardware.MAX_TIMEOUT, false);
            }
        }

        private void pbStop_Click(object sender, EventArgs e)
        {
            //tmrJoystick.Stop();
            GeminiHardware.DoCommandResult(":Q", GeminiHardware.MAX_TIMEOUT, false);
        }

        private void frmMain_VisibleChanged(object sender, EventArgs e)
        {
            if (GeminiHardware.UseJoystick) 
                if (this.Visible)
                    if (GeminiHardware.UseJoystick)
                        if (GeminiHardware.Connected) StartJoystick();
                     else
                         tmrJoystick.Stop();
        }

        private void aboutGeminiDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeminiAbout.MainWindow win = new GeminiAbout.MainWindow();
            win.Show();
        }

        private void checkboxPEC_Clicked(object sender, EventArgs e)
        {
            checkboxPEC.Checked = !checkboxPEC.Checked;
            
            byte pec = GeminiHardware.PECStatus;
            pec =(byte)( (pec & 0xfe) | (checkboxPEC.Checked ? 1 : 0));
            GeminiHardware.PECStatus = pec;
        }

    }
}