using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ASCOM.GeminiTelescope.Properties;
using System.Globalization;

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

            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            
            InitializeComponent();
            tmrUpdate.Interval = 2000;
            tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
            tmrUpdate.Start();
            GeminiHardware.OnConnect += new ConnectDelegate(OnConnectEvent);
            GeminiHardware.OnError += new ErrorDelegate(OnError);
            GeminiHardware.OnInfo += new ErrorDelegate(OnInfo);

            m_BaloonMenu = new ContextMenu();

            MenuItem connectMenu = new MenuItem(Resources.Connect, new EventHandler(ConnectMenu));
            connectMenu.Name = "Connect";

            MenuItem notifyMenu = new MenuItem(Resources.ShowNotifications, new EventHandler(ShowNotificationsMenu));
            notifyMenu.Name = "Notifications";
            notifyMenu.Checked = m_ShowNotifications;

            MenuItem controlMenu = new MenuItem(Resources.ShowHandController, new EventHandler(ControlPanelMenu));
            controlMenu.Name = "Control";
            controlMenu.Checked = this.Visible;

            m_BaloonMenu.MenuItems.AddRange(new MenuItem[] { 
                connectMenu,

                new MenuItem("-"),
                controlMenu,
                new MenuItem(Resources.ConfigureTelescope + "...", new EventHandler(ConfigureTelescopeMenu)),

                new MenuItem(Resources.AdvancedSettings + "...", new EventHandler(AdvancedGeminiMenu)),

                new MenuItem(Resources.ConfigureFocuser+ "...", new EventHandler(ConfigureFocuserMenu)),
            new MenuItem("-"),
            notifyMenu,
            new MenuItem("-"),
            new MenuItem(Resources.AboutGeminiDriver + "...", new EventHandler(aboutGeminiDriverToolStripMenuItem_Click)),
            new MenuItem(Resources.Exit, new EventHandler(ExitMenu))
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
            toolTip1.SetToolTip(this.ButtonConnect, Resources.ConnectingToGemini);
            toolTip1.SetToolTip(this.ButtonFlip, Resources.MeridianFlip);
            toolTip1.SetToolTip(this.ButtonPark, Resources.ParkMount);
            toolTip1.SetToolTip(this.ButtonSetup, Resources.SetupDriver);

            toolTip1.SetToolTip(this.buttonSlew1, Resources.SlewDECP);
            toolTip1.SetToolTip(this.buttonSlew2, Resources.SlewDECM);
            toolTip1.SetToolTip(this.buttonSlew3, Resources.SlewRAM);
            toolTip1.SetToolTip(this.buttonSlew4, Resources.SlewRAP);

            toolTip1.SetToolTip(this.RadioButtonCenter , Resources.UseCenter);
            toolTip1.SetToolTip(this.RadioButtonGuide, Resources.UseGuide);
            toolTip1.SetToolTip(this.RadioButtonSlew , Resources.UseSlew);

            toolTip1.SetToolTip(this.labelLst, Resources.MountLST);
            toolTip1.SetToolTip(this.labelRa,  Resources.MountRA);
            toolTip1.SetToolTip(this.labelDec, Resources.MountDEC);

            toolTip1.SetToolTip(this.checkBoxTrack, Resources.MountStopStart);
            toolTip1.SetToolTip(this.CheckBoxFlipDec, Resources.SwapDEC);
            toolTip1.SetToolTip(this.CheckBoxFlipRa, Resources.SwapRA);

            toolTip1.SetToolTip(this.checkboxPEC, Resources.EnablePEC);

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
                    if (m_Joystick.Initialize(GeminiHardware.JoystickName, GeminiHardware.JoystickAxisRA, GeminiHardware.JoystickAxisDEC))
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
                        Speech.SayIt(Resources.JoystickStop, Speech.SpeechType.Command); 
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

                string s_dir, s_rate;

                if (val < 0.5) {rate = ":RG"; s_rate = "guide";}        // move at guiding speed
                else if (val < 0.75) { rate = ":RC"; s_rate = "center"; }   // centering speed
                else
                { rate = ":RS"; s_rate = "slew"; }                          // slewing speed

                if (Math.Abs(x) > Math.Abs(y))
                {
                    if (x < 0) { dir = ":Me"; s_dir = "East"; }
                    else {  dir = ":Mw"; s_dir = "West"; }
                }
                else
                {
                    if (y > 0) { dir = ":Ms"; s_dir = "South"; }
                    else { dir = ":Mn"; s_dir = "North"; }
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
                    Speech.SayIt(Resources.MoveJoystick + " "+ s_dir + " at " + s_rate + " speed", Speech.SpeechType.Command); 
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
            { this.Hide();
            Speech.SayIt(Resources.HideHandController, Speech.SpeechType.Command);
            }
            else
            { 
                this.Show();
                this.WindowState = FormWindowState.Normal;
                SharedResources.SetTopWindow(this);
                Speech.SayIt(Resources.ShowHandController, Speech.SpeechType.Command);
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
            if (m_ShowNotifications) 
                Speech.SayIt(Resources.ShowNotifications, Speech.SpeechType.Command);
            else
                Speech.SayIt(Resources.HideHandController, Speech.SpeechType.Command);

        }

        void ConnectMenu(object sender, EventArgs e)
        {
            ButtonConnect_Click(sender, e);
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

                if (m_Messages.Count > 0)
                {
                    msg m = m_Messages[0];
                    m_Messages.RemoveAt(0);

                    BaloonIcon.ShowBalloonTip(4000, m.text, m.title, m.icon);
                    m_LastMessageTime = DateTime.Now;
                    // time to turn off the baloon text, since Windows has a minimum of about 20-30 seconds before
                    // the message turns off on its own while the task bar is visible:
                    tmrBaloon.Interval = (m_Messages.Count > 0? 1500 : 4000);
                    tmrBaloon.Start();
                }
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

        class msg
        {
            public string title;
            public string text;
            public ToolTipIcon icon;
            public msg(string _title, string _text, ToolTipIcon _icon)
            {
                title = _title; text = _text; icon = _icon;
            }
        }

        List<msg> m_Messages = new List<msg>();
        DateTime m_LastMessageTime = DateTime.Now;

        void SetBaloonText(string title, string text, ToolTipIcon icon)
        {
            if (m_ShowNotifications)
            {
                // if messages pending or last message was visible for less than 1 second, queue up and wait
                if (m_Messages.Count > 0 || (DateTime.Now - m_LastMessageTime < TimeSpan.FromSeconds(1.5)))
                {
                    m_Messages.Add(new msg(title, text, icon));
                    return;
                }

                tmrBaloon.Stop();

                BaloonIcon.ShowBalloonTip(4000, text, title, icon);

                if (icon == ToolTipIcon.Error)
                    Speech.SayIt(text, Speech.SpeechType.Error);
                else
                    Speech.SayIt(text, Speech.SpeechType.Announcement);

                // time to turn off the baloon text, since Windows has a minimum of about 20-30 seconds before
                // the message turns off on its own while the task bar is visible:
                tmrBaloon.Interval = 4000;
                m_LastMessageTime = DateTime.Now;
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
                BaloonIcon.ContextMenu.MenuItems["Connect"].Text = "Disconnect";
                SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, "Mount is connected", ToolTipIcon.Info);
            }
            if (Connected)
            {
                if (GeminiHardware.UseJoystick && !string.IsNullOrEmpty(GeminiHardware.JoystickName)) StartJoystick();
            }
            if (!Connected && Clients <= 0) // last client to disconnect
            {
                ButtonConnect.Text = "Connect";
                BaloonIcon.ContextMenu.MenuItems["Connect"].Text = "Connect";

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

                string prev_label = labelSlew.Text;

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

                if (labelSlew.Text != prev_label)
                {
                    Speech.SayIt("Mount " + labelSlew.Text, Speech.SpeechType.Status);
                }

                bool prev_pec = checkboxPEC.Checked;

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

                if (checkboxPEC.Checked != prev_pec)
                {
                    Speech.SayIt("P E C " + (checkboxPEC.Checked? "enabled" : "disabled"), Speech.SpeechType.Status);
                }
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


            checkboxPEC.BackColor = (checkboxPEC.Enabled ? Color.Transparent : Color.FromArgb(64, 64, 64)) ;


            BaloonIcon.Text = ""; // tooltip;    
        }


        private void _DoSetupTelescopeDialog()
        {

            Speech.SayIt(Resources.SetupTelescope, Speech.SpeechType.Command);


            TelescopeSetupDialogForm setupForm = new TelescopeSetupDialogForm();

            setupForm.ComPort = GeminiHardware.ComPort;
            setupForm.BaudRate = GeminiHardware.BaudRate.ToString();

            setupForm.GpsBaudRate = GeminiHardware.GpsBaudRate.ToString();
            setupForm.GpsComPort = GeminiHardware.GpsComPort;
            setupForm.GpsUpdateClock = GeminiHardware.GpsUpdateClock;

            setupForm.Elevation = GeminiHardware.Elevation;
            setupForm.Latitude = GeminiHardware.Latitude;
            setupForm.Longitude = GeminiHardware.Longitude;

            setupForm.UseDriverSite = GeminiHardware.UseDriverSite;
            setupForm.UseDriverTime = GeminiHardware.UseDriverTime;

            setupForm.ShowHandbox = GeminiHardware.ShowHandbox;

            setupForm.BootMode = GeminiHardware.BootMode;
            setupForm.UseSpeech = GeminiHardware.UseSpeech;
            setupForm.SpeechFlags = GeminiHardware.SpeechFilter;
            setupForm.SpeechVoice = GeminiHardware.SpeechVoice;

            DialogResult ans;
            if (this.Visible==false)
                ans = setupForm.ShowDialog(this);
            else
                ans = setupForm.ShowDialog(null);

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

                GeminiHardware.UseDriverTime = setupForm.UseDriverTime;
                GeminiHardware.UseDriverSite = setupForm.UseDriverSite;

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

                GeminiHardware.UseSpeech = setupForm.UseSpeech;
                GeminiHardware.SpeechVoice = setupForm.SpeechVoice;
                GeminiHardware.SpeechFilter = setupForm.SpeechFlags;

                Speech.SpeechInitialize(this.Handle, GeminiHardware.UseSpeech? GeminiHardware.SpeechVoice : null);
                Speech.Filter = GeminiHardware.SpeechFilter;
                
                if (error != "")
                {
                    if (error.EndsWith(", ")) error = error.Substring(0, error.Length - 2);
                    MessageBox.Show(Resources.InvalidSettings + "\r\n" + error, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                tmrJoystick.Stop();
                StartJoystick();    //restart if settings have changed
            }

            setupForm.Dispose();
        }
        private void _DoFocuserSetupDialog()
        {
            Speech.SayIt(Resources.ShowSetupFocuser, Speech.SpeechType.Command);
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
                    MessageBox.Show(Resources.SomeInvalidSettings, SharedResources.TELESCOPE_DRIVER_NAME);
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

            try
            {
                Speech.SpeechInitialize(this.Handle, GeminiHardware.UseSpeech ? GeminiHardware.SpeechVoice : null);
                Speech.Filter = GeminiHardware.SpeechFilter;
            }
            catch
            {
            }

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
                Speech.SayIt(Resources.Connect, Speech.SpeechType.Command);

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
                    MessageBox.Show(Resources.CannotConnect + "\r\n" + m_LastError, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                    this.ButtonConnect.Text = "Disconnect";
            }
            else
            {
                Speech.SayIt(Resources.Disconnect, Speech.SpeechType.Command);
                if (GeminiHardware.Clients > 1)
                {
                    DialogResult res = MessageBox.Show(Resources.DisconnectWarning,
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
                    MessageBox.Show(Resources.CannotDisconnect, SharedResources.TELESCOPE_DRIVER_NAME);
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
                SetBaloonText(Resources.DriverRunning, Resources.DriverMinimized, ToolTipIcon.Info);
                return;
            }
            m_ExitFormMenuCall = false;
            if (GeminiHardware.Connected && GeminiHardware.Clients > 0)
            {
                DialogResult res = MessageBox.Show("Gemini connection" + (GeminiHardware.Clients > 1 ? "s are" : " is") + " still active. " + Resources.GeminiConnectionAlive1,
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
                Speech.SayIt(Resources.MedidianFlip, Speech.SpeechType.Command);
                string res = GeminiHardware.DoCommandResult(":Mf", GeminiHardware.MAX_TIMEOUT, false);
                switch (res)
                {
                    case "1Object below horizon.":
                        Speech.SayIt(Resources.ObjectBelowHorizon, Speech.SpeechType.Error);
                        MessageBox.Show(this, Resources.ObjectBelowHorizon, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case "4Position unreachable.":
                        Speech.SayIt(Resources.ObjectUnreachable, Speech.SpeechType.Error);
                        MessageBox.Show(this, Resources.ObjectUnreachable, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        private void buttonSlew1_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                Speech.SayIt(Resources.DownButton, Speech.SpeechType.Command);
                ((Control)sender).Capture = true;

                string[] cmds = { null, null };

                if (RadioButtonGuide.Checked) { cmds[0] = ":RG"; Speech.SayIt(Resources.AtGuideSpeed, Speech.SpeechType.Command); }

                if (RadioButtonCenter.Checked) { cmds[0] = ":RC"; Speech.SayIt(Resources.AtCenterSpeed, Speech.SpeechType.Command); } 
                if (RadioButtonSlew.Checked) { cmds[0] = ":RS"; Speech.SayIt(Resources.AtSlewSpeed, Speech.SpeechType.Command); }

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
            Speech.SayIt(Resources.UpButton, Speech.SpeechType.Command);
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
                Speech.SayIt(Resources.RightButton, Speech.SpeechType.Command);

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
                Speech.SayIt(Resources.LeftButton, Speech.SpeechType.Command);

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
            Speech.SayIt(Resources.ParkHere, Speech.SpeechType.Command);

            GeminiHardware.DoCommandResult(":hN", GeminiHardware.MAX_TIMEOUT, false);

        }

        private void toolStripMenuParkCWD_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            Speech.SayIt(Resources.ParkCWD, Speech.SpeechType.Command);
            GeminiHardware.DoCommandResult(":hC", GeminiHardware.MAX_TIMEOUT, false);
            GeminiHardware.WaitForHomeOrPark("Park");
            GeminiHardware.DoCommandResult(":hN", GeminiHardware.MAX_TIMEOUT, false);
            this.UseWaitCursor = false;
        }

        private void toolStripMenuParkHome_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            Speech.SayIt(Resources.ParkHome, Speech.SpeechType.Command);

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
            Speech.SayIt(Resources.StopSlew, Speech.SpeechType.Command);
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
            Speech.SayIt(Resources.AboutGeminiDriver, Speech.SpeechType.Command);

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

        private void buttonSlew4_Click(object sender, EventArgs e)
        {

        }

        private void exitDriverMenuItem_Click(object sender, EventArgs e)
        {
            ExitMenu(sender, e);
        }

    }
}