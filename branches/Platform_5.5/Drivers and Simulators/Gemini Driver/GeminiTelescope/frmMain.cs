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

        delegate void UpdateDisplayDelegate(byte pec);

        //Timer tmrUpdate = new Timer();
        Timer tmrBaloon = new Timer();
        Timer tmrJoystick = new Timer();

        System.Timers.Timer tmrUpdate = new System.Timers.Timer();

        string m_LastError = "";


        ContextMenu m_BalloonMenu = null;

        Color m_ActiveBkColor = Color.FromArgb(255, 0, 0);
        Color m_InactiveBkColor = Color.FromArgb(64,0,0);
        Color m_ActiveForeColor = Color.FromArgb(0, 255, 255);
        Color m_InactiveForeColor = Color.FromArgb(128, 64, 64);

        int m_UpdateCount = 0;
        bool m_ShowNotifications = true;
        bool m_ShowStatusPanel = true;

        frmStatus m_StatusForm  = null;

        Joystick m_Joystick = null;

        string m_JoystickRate = null;           // these keep last joystick rate and
        string m_JoystickDirection =null;      // direction commands issued 

        bool m_ExitFormMenuCall = false;

        // last key pressed down (and not let up):
        Keys m_LastKey = Keys.None;
        Keys m_LastModifiers = Keys.None;

        public frmMain()
        {

            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            
            InitializeComponent();
            tmrUpdate.Interval = 2000;

            tmrUpdate.Elapsed+=new System.Timers.ElapsedEventHandler(tmrUpdate_Tick);
            tmrUpdate.Start();
            GeminiHardware.Instance.OnConnect += new ConnectDelegate(OnConnectEvent);
            GeminiHardware.Instance.OnError += new ErrorDelegate(OnError);
            GeminiHardware.Instance.OnInfo += new ErrorDelegate(OnInfo);

            GeminiHardware.Instance.OnDisplayChanged += new DisplayChangedDelegate(Instance_OnDisplayChanged);
            m_BalloonMenu = new ContextMenu();

            MenuItem connectMenu = new MenuItem(Resources.Connect, new EventHandler(ConnectMenu));
            connectMenu.Name = Resources.Connect;

            MenuItem notifyMenu = new MenuItem(Resources.ShowNotifications, new EventHandler(ShowNotificationsMenu));
            notifyMenu.Name = Resources.Notifications;
            notifyMenu.Checked = m_ShowNotifications;

            MenuItem statusMenu = new MenuItem(Resources.ShowStatusPanel, new EventHandler(ShowStatusPanel));
            statusMenu.Name = Resources.StatusPanel;
            statusMenu.Checked = m_ShowStatusPanel;

            MenuItem controlMenu = new MenuItem(Resources.ShowHandController, new EventHandler(ControlPanelMenu));
            controlMenu.Name = Resources.Control;
            controlMenu.Checked = this.Visible;

            string s = global::ASCOM.GeminiTelescope.Properties.Resources.ConfigureTelescope;
            string s2 = Resources.ConfigureTelescope;

            m_BalloonMenu.MenuItems.AddRange(new MenuItem[] { 
                connectMenu,

                new MenuItem("-"),
                controlMenu,
                new MenuItem(Resources.ConfigureTelescope + "...", new EventHandler(ConfigureTelescopeMenu)),

                new MenuItem(Resources.AdvancedSettings + "...", new EventHandler(AdvancedGeminiMenu)),

                new MenuItem(Resources.ConfigureFocuser+ "...", new EventHandler(ConfigureFocuserMenu)),

                new MenuItem("-"),
                new MenuItem(Resources.ObservationLog+"...", new EventHandler(observationLogToolStripMenuItem_Click)),
                new MenuItem(Resources.ConfigureCatalogs + "...", new EventHandler(configureCatalogsToolStripMenuItem_Click)),
            new MenuItem("-"),
            notifyMenu,
            statusMenu,
            new MenuItem("-"),
            new MenuItem(Resources.HelpMenu, new EventHandler(viewHelpToolStripMenuItem_Click)),
            new MenuItem(Resources.AboutGeminiDriver + "...", new EventHandler(aboutGeminiDriverToolStripMenuItem_Click)),
            new MenuItem(Resources.Exit, new EventHandler(ExitMenu))
            });

            BalloonIcon.ContextMenu = m_BalloonMenu;
            BalloonIcon.MouseClick += new MouseEventHandler(BalloonIcon_MouseClick);

            BalloonIcon.Visible = true;
            tmrBaloon.Tick += new EventHandler(tmrBaloon_Tick);
            BalloonIcon.MouseDoubleClick += new MouseEventHandler(BalloonIcon_MouseDoubleClick);
            BalloonIcon.MouseMove += new MouseEventHandler(BalloonIcon_MouseMove);
            GeminiHardware.Instance.OnSafetyLimit += new SafetyDelegate(OnSafetyLimit);

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
            toolTip1.SetToolTip(this.FuncMenu, Resources.PerformFunctions);
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
            toolTip1.SetToolTip(this.labelHA, Resources.MountHA);

            toolTip1.SetToolTip(this.checkBoxTrack, Resources.MountStopStart);
            toolTip1.SetToolTip(this.CheckBoxFlipDec, Resources.SwapDEC);
            toolTip1.SetToolTip(this.CheckBoxFlipRa, Resources.SwapRA);

            toolTip1.SetToolTip(this.checkboxPEC, Resources.EnablePEC);

            tmrJoystick.Tick += new EventHandler(tmrJoystick_Tick);

        }

        void Instance_OnDisplayChanged()
        {    
            string display = GeminiHardware.Instance.DoCommandResult(":OO", GeminiHardware.Instance.MAX_TIMEOUT, false);
            if (string.IsNullOrEmpty(display) || display.Trim()=="")
                display = GeminiHardware.Instance.DoCommandResult(":Oo", GeminiHardware.Instance.MAX_TIMEOUT, false); 
            
            if (!string.IsNullOrEmpty(display) && display.Trim()!="")
                OnInfo("Gemini Controller", display);
        }

        private void StartJoystick()
        {
            m_JoystickRate = null;
            m_JoystickRate = null;

            int[] joys = Joystick.Joysticks;
            if (joys != null && joys.Length > 0)
            {
                if (!string.IsNullOrEmpty(GeminiHardware.Instance.JoystickName))
                {
                    m_Joystick = new Joystick();
                    if (m_Joystick.Initialize(GeminiHardware.Instance.JoystickName, GeminiHardware.Instance.JoystickAxisRA, GeminiHardware.Instance.JoystickAxisDEC))
                    {
                        tmrJoystick.Interval = 200;
                        tmrJoystick.Start();
                    }
                    else
                        SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, Resources.JoystickIsNotAvailable + "\r\n" + GeminiHardware.Instance.JoystickName, ToolTipIcon.Warning);
                }
            }
        }

        private ulong m_PreviousJoystickButtonState = 0;
        private int[] m_AutoFireCount = new int[36];
        
        // joystic position corrresponding to mount G/C/S speeds:
        private double[] joystick_speeds = { 0.251, 0.501, 0.751 };

        void tmrJoystick_Tick(object sender, EventArgs e)
        {
            tmrJoystick.Stop();
            if (GeminiHardware.Instance.Connected && this.Visible && GeminiHardware.Instance.UseJoystick && !string.IsNullOrEmpty(GeminiHardware.Instance.JoystickName))
            {
                double x = m_Joystick.PosX;
                double y = m_Joystick.PosY;
#if DEBUG
                System.Diagnostics.Trace.WriteLine("JOYSTICK : X = " + x.ToString() + "   Y = " + y.ToString());
#endif
                GeminiHardware.Instance.Trace.Info(4, "JOYSTICK X,Y:", x.ToString() , y.ToString());
                ulong buttons = m_Joystick.ButtonState;
                
                ProcessButtonPress(ref buttons, m_PreviousJoystickButtonState);
                
                m_PreviousJoystickButtonState = buttons;

                if (!GeminiHardware.Instance.JoystickIsAnalog)
                {
                    // make the requested sensitivity adjustment to joystick coordinates:
                    if (GeminiHardware.Instance.JoystickSensitivity != 0) {
                        GeminiHardware.Instance.Trace.Info(4, "JOYSTICK Sensitivity:", GeminiHardware.Instance.JoystickSensitivity.ToString());
                        x *= GeminiHardware.Instance.JoystickSensitivity / 100.0;
                        y *= GeminiHardware.Instance.JoystickSensitivity / 100.0; 
                    }
                }


                // joystick positions are reported from -1 to 1
                // 0 .. 0.25 means no movement (near center)
                // 0.25 .. 0.5 => move at guiding speed
                // 0.5 .. 0.75 => move at centering speed
                // 0.75 .. 1.0 => move at slew speed


                List<string> cmds = new List<string>();

                if (this.CheckBoxFlipRa.Checked) x = -x;
                if (this.CheckBoxFlipDec.Checked) y = -y;
                if (GeminiHardware.Instance.JoystickFlipRA) x = -x;
                if (GeminiHardware.Instance.JoystickFlipDEC) y = -y;

                string dir, rate;

                // if last joystick rate was zero, but we are still moving at speed, then
                // the mount must be slowing down from a slew.. continue waiting
                if ((m_JoystickRate == null) && (GeminiHardware.Instance.Velocity == "S"))
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
                        GeminiHardware.Instance.DoCommand(":Q", false);
                        Speech.SayIt(Resources.JoystickStop, Speech.SpeechType.Command); 
                    }
                    tmrJoystick.Start();
                    return;
                }

                // fixed speed selected, use the value in JoysticFixedSpeed (0-2 == G/C/S):
                if (!GeminiHardware.Instance.JoystickIsAnalog)
                {
                    double speed = joystick_speeds[GeminiHardware.Instance.JoystickFixedSpeed];
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
                    if (x < 0) { dir = ":Me"; s_dir = "Left"; }
                    else {  dir = ":Mw"; s_dir = "Right"; }
                }
                else
                {
                    if (y > 0) { dir = ":Ms"; s_dir = "Down"; }
                    else { dir = ":Mn"; s_dir = "Up"; }
                }

                if (rate != m_JoystickRate || dir != m_JoystickDirection)
                {
                    // if we are moving at speed, we can't change rates/directions until
                    // the mount slows down to tracking speed. Issue move quit command, and 
                    // return until next time...
                    if (((GeminiHardware.Instance.Velocity == "S") && (m_JoystickRate == ":RS" || rate == ":RS")) ||
                         ((GeminiHardware.Instance.Velocity == "C") && (rate == ":RS")))
                    {
                        GeminiHardware.Instance.DoCommand(":Q", false);
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
                    GeminiHardware.Instance.Trace.Info(4, "Move Joystick", s_dir, s_rate);
                }
                if (cmds.Count > 0) GeminiHardware.Instance.DoCommand(cmds.ToArray(), false);
                tmrJoystick.Start();
            }
        }

        private void ProcessButtonPress(ref ulong buttons, ulong prev_buttons)
        {
            ulong mask = 1;

            for (int i = 0; i < 36; ++i, mask<<=1)
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

                    UserCommand.Execute(GeminiHardware.Instance.JoystickButtonMap[i], (buttons & mask) != 0);

                    // if this is the first button down in a while, let up
                    // and wait for 5 cycles before continually firing:
                    if ((buttons & mask) != 0 && m_AutoFireCount[i] == 0)
                    {
                        UserCommand.Execute(GeminiHardware.Instance.JoystickButtonMap[i], false);    //key up
                        buttons &= ~mask;   //remove it from state, so it fires the next time
                        m_AutoFireCount[i]++;
                    }
                }
            }
        }


        void BalloonIcon_MouseMove(object sender, MouseEventArgs e)
        {
            if ((m_StatusForm == null || !m_StatusForm.Visible) && m_ShowStatusPanel)
            {
                Screen scr = Screen.FromPoint(Cursor.Position);
                GeminiHardware.Instance.ShowStatus(new Point(scr.WorkingArea.Right, scr.WorkingArea.Bottom), true);
            }
        }

        void BalloonIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ControlPanelMenu(sender, e);
        }


        void BalloonIcon_MouseClick(object sender, MouseEventArgs e)
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
            {
                this.Hide();
                Speech.SayIt(Resources.HideHandController, Speech.SpeechType.Command);
            }
            else
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                SharedResources.SetTopWindow(this);
                SetTopMost();
                Speech.SayIt(Resources.ShowHandController, Speech.SpeechType.Command);
            }

            m_BalloonMenu.MenuItems[Resources.Control].Checked = this.Visible;
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
            m_BalloonMenu.MenuItems[Resources.Notifications].Checked = m_ShowNotifications;
            if (m_ShowNotifications) 
                Speech.SayIt(Resources.ShowNotifications, Speech.SpeechType.Command);
            else
                Speech.SayIt(Resources.HideNotifications, Speech.SpeechType.Command);

        }

        void ShowStatusPanel(object sender, EventArgs e)
        {
            m_ShowStatusPanel = !m_ShowStatusPanel;
            m_BalloonMenu.MenuItems[Resources.StatusPanel].Checked = m_ShowStatusPanel;
            if (m_ShowStatusPanel)
                Speech.SayIt(Resources.ShowStatusPanel, Speech.SpeechType.Command);
            else
                Speech.SayIt(Resources.HideStatusPanel, Speech.SpeechType.Command);
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
                BalloonIcon.Visible = false;
                BalloonIcon.Visible = true;

                if (m_Messages.Count > 0)
                {
                    msg m = m_Messages[0];
                    m_Messages.RemoveAt(0);

                    BalloonIcon.ShowBalloonTip(4000, m.text, m.title, m.icon);
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
                this.BeginInvoke(new InfoBaloonDelegate(SetBaloonText), from, msg, ToolTipIcon.Error);
            else
                SetBaloonText(from, msg, ToolTipIcon.Info);
        }

        void OnInfo(string from, string msg)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new InfoBaloonDelegate(SetBaloonText), from, msg, ToolTipIcon.Info);
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

                BalloonIcon.ShowBalloonTip(4000, text, title, icon);

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
                ButtonConnect.Text = Resources.Disconnect;
                BalloonIcon.ContextMenu.MenuItems[Resources.Connect].Text = Resources.Disconnect;

                m_Messages.Clear(); // remove all queued up messages, we don't care now that the mount is connected:
                
                SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME + "\r\n" + 
                    (GeminiHardware.Instance.EthernetPort? "" : (GeminiHardware.Instance.BaudRate.ToString()) + "b/s ") +  "On " + GeminiHardware.Instance.ComPort, Resources.MountIsConnected, ToolTipIcon.Info);
            }
            if (Connected)
            {
                if (GeminiHardware.Instance.UseJoystick && !string.IsNullOrEmpty(GeminiHardware.Instance.JoystickName)) StartJoystick();
            }
            if (!Connected && Clients <= 0) // last client to disconnect
            {
                ButtonConnect.Text = Resources.Connect;
                BalloonIcon.ContextMenu.MenuItems[Resources.Connect].Text = Resources.Connect;

                labelLst.Text = "00:00:00";
                labelRa.Text = "00:00:00";
                labelDec.Text = "+00:00:00";
                labelLimit.Text = "00:00:00";
                labelHA.Text = "00:00:00";
                labelSlew.Text = Resources.dispSTOP;
                labelPARK.Text = Resources.dispPARK;
                labelSlew.BackColor = m_InactiveBkColor;
                labelSlew.ForeColor = m_InactiveForeColor;
                labelPARK.BackColor = m_InactiveBkColor;
                labelPARK.ForeColor = m_InactiveForeColor;
                SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, Resources.MountIsDisconnected, ToolTipIcon.Info);
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
                this.BeginInvoke(new ConnectDelegate(ConnectStateChanged), Connected, Clients);
            else
                ConnectStateChanged(Connected, Clients);
        }

        /// <summary>
        /// Safety limit reached event
        /// </summary>
        void OnSafetyLimit()
        {
            if (GeminiHardware.Instance.Connected)
            {
                SetBaloonText(SharedResources.TELESCOPE_DRIVER_NAME, Resources.SafetyLimitReached + " " + Resources.MountStopped, ToolTipIcon.Warning);
                Speech.SayIt(Resources.SafetyLimitReached, Speech.SpeechType.Always);
                // may want to add some optional sound/alert here to notify the user!
            }
        }

        void OnSafetyLimitEvent()
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new SafetyDelegate(OnSafetyLimit));
            OnSafetyLimit();
        }


        void UpdateDisplay(byte pec)
        {
            if (GeminiHardware.Instance.Connected) 
            {
                labelSlew.BackColor = (GeminiHardware.Instance.Velocity == "S" ? m_ActiveBkColor : m_InactiveBkColor);

                // blink the text while slewing is active:
                Color active = ((m_UpdateCount & 1) == 0 ? m_ActiveForeColor : m_InactiveForeColor);

                labelSlew.ForeColor = (GeminiHardware.Instance.Velocity == "S" ? active : m_InactiveForeColor);
                labelPARK.BackColor = (GeminiHardware.Instance.AtPark ? m_ActiveBkColor : m_InactiveBkColor);
                labelPARK.ForeColor = (GeminiHardware.Instance.AtPark ? m_ActiveForeColor : m_InactiveForeColor);

                if (GeminiHardware.Instance.AtPark)
                    labelPARK.Text = Resources.dispPARK;
                else
                    labelPARK.Text = GeminiHardware.Instance.SideOfPier == "E" ? Resources.dispEAST : Resources.dispWEST;

                string prev_label = labelSlew.Text;

                switch (GeminiHardware.Instance.Velocity)
                {
                    case "S": labelSlew.Text = Resources.dispSLEW; break;
                    case "C": labelSlew.Text = Resources.dispCENTER; break;
                    case "N": labelSlew.Text = Resources.dispSTOP; break;
                    case "G": labelSlew.Text = Resources.dispGUIDE; break;
                    default: labelSlew.Text = Resources.dispTRACK; break;
                }
                //add an indicator for a "safety limit alert".
                if (GeminiHardware.Instance.AtSafetyLimit)
                {
                    labelSlew.Text = Resources.dispLIMIT;
                    labelSlew.ForeColor = m_ActiveForeColor;
                    labelSlew.BackColor = m_ActiveBkColor;
                    labelLimit.Text = "00:00:00";
                }

                pbStop.Visible = (GeminiHardware.Instance.Velocity == "S");  //only show Stop! button when slewing

                if (labelSlew.Text != prev_label)
                {
                    Speech.SayIt(Resources.Mount + " " + labelSlew.Text, Speech.SpeechType.Status);
                }

                if (pec != 0xff)
                {
                    bool prev_pec = checkboxPEC.Checked;

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
                        Speech.SayIt("P E C " + (checkboxPEC.Checked ? Resources.Enabled : Resources.Disabled), Speech.SpeechType.Status);
                    }
                }
            }
            else
            {
                labelSlew.ForeColor = m_InactiveForeColor;
                labelPARK.BackColor = m_InactiveBkColor;
                labelPARK.ForeColor = m_InactiveForeColor;

                labelSlew.Text = Resources.dispSTOP; 
                pbStop.Visible = false;  //only show Stop! button when slewing

            }

            m_BalloonMenu.MenuItems[Resources.Control].Checked = this.Visible;

            string tooltip = Resources.GeminiIs;
            if (GeminiHardware.Instance.Connected)
            {
                tooltip += Resources.Connected + "\r\n" + (GeminiHardware.Instance.BaudRate.ToString()) + "b/s on " + GeminiHardware.Instance.ComPort;
                tooltip += "\r\n" + Resources.Status + ": " + labelSlew.Text;

            }
            else
                tooltip+= Resources.NotConnected;

            checkBoxTrack.Checked = (GeminiHardware.Instance.Velocity == "N" ? false : true);


            checkboxPEC.BackColor = (checkboxPEC.Enabled ? Color.Transparent : Color.FromArgb(64, 64, 64)) ;


            BalloonIcon.Text = ""; // tooltip;    
        
        }


        static byte previous_PECStatus = 0xff;

        void tmrUpdate_Tick(object sender, EventArgs e)
        {


            if (GeminiHardware.Instance.Connected)
            {
                m_UpdateCount++;

                RightAscension = GeminiHardware.Instance.RightAscension;
                Declination = GeminiHardware.Instance.Declination;
                SiderealTime = GeminiHardware.Instance.SiderealTime;

                // don't bother with PEC status and
                // distance to safety while slewing: this is too
                // much for Gemini to handle
                if (GeminiHardware.Instance.Velocity != "S")
                {
                    // update limit distance every 4th time
                    if ((m_UpdateCount & 3) == 0)
                    {
                        UpdateTimeToLimit();
                        HourAngleDisplay = HourAngle; 
                    }

                    if (GeminiHardware.Instance.QueueDepth < 3)  //don't keep queuing if queue is large
                    {
                        byte pec = GeminiHardware.Instance.PECStatus;
                        if (pec != 0xff) previous_PECStatus = pec;
                    }

                }

                this.BeginInvoke(new UpdateDisplayDelegate(UpdateDisplay), previous_PECStatus);
            }
        }

        private void UpdateTimeToLimit()
        {
            if (GeminiHardware.Instance.QueueDepth > 2) return;  // Don't queue up if queue is large

            string safety = GeminiHardware.Instance.DoCommandResult("<230:", GeminiHardware.Instance.MAX_TIMEOUT, false);
            string position = GeminiHardware.Instance.DoCommandResult("<235:", GeminiHardware.Instance.MAX_TIMEOUT, false);
            string size = GeminiHardware.Instance.DoCommandResult("<237:", GeminiHardware.Instance.MAX_TIMEOUT, false);
            if (safety == null || position == null || size == null) return; //???

            string [] sp = safety.Split(new char[] {';'});
            if (sp==null || sp.Length !=2) return;

            int west_limit = 0;

            // west limit in clusters of 256 motor encoder ticks
            if (!int.TryParse(sp[1], out west_limit)) return;

            
            sp = position.Split(new char[] {';'});
            if (sp==null || sp.Length !=2) return;
            int ra_clusters = 0;

            // current RA position in clusters of 256 motor encoder ticks
            if (!int.TryParse(sp[0], out ra_clusters)) return;

            sp = size.Split(new char[] {';'});
            if (sp==null || sp.Length !=2) return;
            int size_clusters = 0;

            // size of 1/2 a cirlce (180 degrees) in RA in clusters of 256 motor encoder ticks
            if (!int.TryParse(sp[0], out size_clusters)) return;

            double rate = SharedResources.EARTH_ANG_ROT_DEG_MIN/60.0; //sidereal rate per second

            // sidereal tracking rate in clusters per second:
            rate = (double)size_clusters * (SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0) / 180.0;

            double distance = ra_clusters - west_limit;
            if (distance <= 0)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(new SetTextCallback(SetLimitText), "@ LIMIT");
                else
                    labelLimit.Text = "@ LIMIT";
            }
            else
            {
                double seconds = distance / rate;
                TimeSpan ts = TimeSpan.FromSeconds(seconds);
                string lmt = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                if (this.InvokeRequired)
                    this.BeginInvoke(new SetTextCallback(SetLimitText), lmt);
                else
                    labelLimit.Text = lmt;
            }
        }

        private void SetLimitText(object val)
        {
            labelLimit.Text = (string)val;
        }

        private void _DoSetupTelescopeDialog()
        {

            Speech.SayIt(Resources.SetupTelescope, Speech.SpeechType.Command);


            TelescopeSetupDialogForm setupForm = new TelescopeSetupDialogForm();

            setupForm.ComPort = GeminiHardware.Instance.ComPort;
            setupForm.BaudRate = GeminiHardware.Instance.BaudRate.ToString();

            setupForm.Elevation = GeminiHardware.Instance.Elevation;
            setupForm.Latitude = GeminiHardware.Instance.Latitude;
            setupForm.Longitude = GeminiHardware.Instance.Longitude;
            setupForm.TZ = -GeminiHardware.Instance.UTCOffset; 

            setupForm.UseDriverSite = GeminiHardware.Instance.UseDriverSite;
            setupForm.UseDriverTime = GeminiHardware.Instance.UseDriverTime;

            setupForm.ShowHandbox = GeminiHardware.Instance.ShowHandbox;

            setupForm.BootMode = GeminiHardware.Instance.BootMode;
            setupForm.UseSpeech = GeminiHardware.Instance.UseSpeech;
            setupForm.SpeechFlags = GeminiHardware.Instance.SpeechFilter;
            setupForm.SpeechVoice = GeminiHardware.Instance.SpeechVoice;
            setupForm.Sites = GeminiHardware.Instance.Sites;
            setupForm.AllowPortScan = GeminiHardware.Instance.ScanCOMPorts;

            setupForm.TraceLevel = GeminiHardware.Instance.TraceLevel;

            setupForm.AsyncPulseGuide = GeminiHardware.Instance.AsyncPulseGuide;
            setupForm.ReportPierSide = GeminiHardware.Instance.ReportPierSide;
            setupForm.PrecisionPulseGuide = GeminiHardware.Instance.PrecisionPulseGuide;

            string[] optics = GeminiHardware.Instance.OpticsNames.Split('~');
            string[] focallengths = GeminiHardware.Instance.FocalLength.Split('~');
            string[] apertures = GeminiHardware.Instance.ApertureDiameter.Split('~');
            string[] uoms = GeminiHardware.Instance.OpticsUnitOfMeasure.Split('~');
            string[] obstructions = GeminiHardware.Instance.OpticsObstruction.Split('~');

            setupForm.ClearOpticsInfos();

            for (int i = 0; i < optics.Length; i++)
            {
                OpticsInfo oi = new OpticsInfo();
                try
                {
                    oi.Name = optics[i];
                    oi.FocalLength = double.Parse(focallengths[i]);
                    oi.ApertureDiameter = double.Parse(apertures[i]);
                    oi.ObstructionDiameter = double.Parse(obstructions[i]);
                    oi.UnitOfMeasure = uoms[i];
                    setupForm.AddOpticsInfo(oi);
                }
                catch 
                {
                    setupForm.ClearOpticsInfos();
                    oi = new OpticsInfo();
                    oi.Name = "";
                    oi.FocalLength = 0;
                    oi.UnitOfMeasure = "millimeters";
                    oi.ApertureDiameter = 0;
                    oi.ObstructionDiameter = 0;
                    setupForm.AddOpticsInfo(oi);
                }
            }

            setupForm.SelectedOptic = GeminiHardware.Instance.OpticsValueIndex;

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
                    GeminiHardware.Instance.ComPort = setupForm.ComPort;                 
                }
                catch 
                {
                    error += Resources.COMport + ", ";
                }

                try{ GeminiHardware.Instance.BaudRate = int.Parse(setupForm.BaudRate); }
                catch { if (setupForm.ComPort != "Ethernet") error += Resources.BaudRate + ", ";}

                try {GeminiHardware.Instance.Elevation = setupForm.Elevation;}
                catch {error += Resources.Elevation + ", ";}

                try { GeminiHardware.Instance.Latitude = setupForm.Latitude; }
                catch {error+= Resources.Latitude + ", "; }
                
                try {GeminiHardware.Instance.Longitude = setupForm.Longitude;}
                catch { error += Resources.Longitude + ", ";  }

                try { GeminiHardware.Instance.UTCOffset = -setupForm.TZ; }
                catch { error += "Timezone" + ", "; }



                GeminiHardware.Instance.UseDriverTime = setupForm.UseDriverTime;
                GeminiHardware.Instance.UseDriverSite = setupForm.UseDriverSite;

                GeminiHardware.Instance.ShowHandbox = setupForm.ShowHandbox;

                GeminiHardware.Instance.BootMode = setupForm.BootMode;

                GeminiHardware.Instance.TraceLevel = setupForm.TraceLevel;

                GeminiHardware.Instance.AsyncPulseGuide = setupForm.AsyncPulseGuide;

                GeminiHardware.Instance.ReportPierSide = setupForm.ReportPierSide;

                GeminiHardware.Instance.PrecisionPulseGuide = setupForm.PrecisionPulseGuide;

                GeminiHardware.Instance.ScanCOMPorts = setupForm.AllowPortScan;

                if (setupForm.UseJoystick && !string.IsNullOrEmpty(setupForm.JoystickName))
                {
                    GeminiHardware.Instance.UseJoystick = true;
                    GeminiHardware.Instance.JoystickName = setupForm.JoystickName;
                }
                else
                {
                    GeminiHardware.Instance.UseJoystick = false;
                }

                GeminiHardware.Instance.UseSpeech = setupForm.UseSpeech;
                GeminiHardware.Instance.SpeechVoice = setupForm.SpeechVoice;
                GeminiHardware.Instance.SpeechFilter = setupForm.SpeechFlags;

                //Get the Optics Data
                GeminiHardware.Instance.OpticsValueIndex = setupForm.SelectedOptic;
                string focallength = "";
                string aperture = "";
                string uom = "";
                string name = "";
                string obstruction = "";
                for (int i = 0; i < setupForm.OpticsInfos.Count; i++)
                {
                    OpticsInfo oi = setupForm.GetOpticsInfo(i);

                    // always store in mm
                    if (uom == "inches")
                    {
                        oi.FocalLength *= 25.4;
                        oi.ApertureDiameter *= 25.4;
                        oi.ObstructionDiameter *= 25.4;
                    }

                    if (focallength == "") focallength += oi.FocalLength.ToString();
                    else focallength += "~" + oi.FocalLength.ToString();
                    if (aperture == "") aperture += oi.ApertureDiameter.ToString();
                    else aperture += "~" + oi.ApertureDiameter.ToString();
                    if (uom == "") uom += oi.UnitOfMeasure;
                    else uom += "~" + oi.UnitOfMeasure;
                    if (oi.Name != "") name += "~" + oi.Name;
                    if (obstruction == "") obstruction += oi.ObstructionDiameter.ToString();
                    else obstruction += "~" + oi.ObstructionDiameter.ToString();

                }

                GeminiHardware.Instance.FocalLength = focallength;
                GeminiHardware.Instance.ApertureDiameter = aperture;
                GeminiHardware.Instance.OpticsUnitOfMeasure = uom;
                GeminiHardware.Instance.OpticsNames = name;
                GeminiHardware.Instance.OpticsObstruction = obstruction;

                
                if (error != "")
                {
                    if (error.EndsWith(", ")) error = error.Substring(0, error.Length - 2);
                    MessageBox.Show(Resources.InvalidSettings + "\r\n" + error, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                tmrJoystick.Stop();
                StartJoystick();    //restart if settings have changed
                
            }

            Speech.SpeechInitialize(this.Handle, GeminiHardware.Instance.UseSpeech ? GeminiHardware.Instance.SpeechVoice : null);
            Speech.Filter = GeminiHardware.Instance.SpeechFilter;

            setupForm.Dispose();
            GeminiHardware.Instance.Profile = null;
        }

        private void _DoFocuserSetupDialog()
        {
            Speech.SayIt(Resources.ShowSetupFocuser, Speech.SpeechType.Command);
            FocuserSetupDialogForm setupForm = new FocuserSetupDialogForm();
            setupForm.ComPort = GeminiHardware.Instance.ComPort;
            setupForm.BaudRate = GeminiHardware.Instance.BaudRate.ToString();
            setupForm.ReverseDirection = GeminiHardware.Instance.ReverseDirection;
            setupForm.MaxIncrement = GeminiHardware.Instance.MaxIncrement;
            setupForm.StepSize = GeminiHardware.Instance.StepSize;
            setupForm.BrakeSize = GeminiHardware.Instance.BrakeSize;
            setupForm.BacklashSize = GeminiHardware.Instance.BacklashSize;
            setupForm.BacklashDirection = GeminiHardware.Instance.BacklashDirection;
            setupForm.Speed = GeminiHardware.Instance.Speed;
            setupForm.AbsoluteFocuser = GeminiHardware.Instance.AbsoluteFocuser;

            DialogResult ans = setupForm.ShowDialog();

            if (ans == DialogResult.OK)
            {
                try
                {
                    GeminiHardware.Instance.AbsoluteFocuser = setupForm.AbsoluteFocuser;
                    GeminiHardware.Instance.ComPort = setupForm.ComPort;
                    GeminiHardware.Instance.BaudRate = int.Parse(setupForm.BaudRate);
                    GeminiHardware.Instance.ReverseDirection = setupForm.ReverseDirection;
                    GeminiHardware.Instance.MaxIncrement = setupForm.MaxIncrement;
                    GeminiHardware.Instance.StepSize = setupForm.StepSize;
                    GeminiHardware.Instance.BrakeSize = setupForm.BrakeSize;
                    GeminiHardware.Instance.BacklashSize = setupForm.BacklashSize;
                    GeminiHardware.Instance.BacklashDirection = setupForm.BacklashDirection;
                    GeminiHardware.Instance.Speed = setupForm.Speed;
                }
                catch
                {
                    MessageBox.Show(Resources.SomeInvalidSettings, SharedResources.TELESCOPE_DRIVER_NAME);
                }
            }

            setupForm.Dispose();
            GeminiHardware.Instance.Profile = null;
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
            if (GeminiHardware.Instance.SouthernHemisphere)
            {
                //buttonSlew1.Text = "S";
                //buttonSlew2.Text = "N";
                //buttonSlew3.Text = "E";
                //buttonSlew4.Text = "W";
            }
            else
            {
                //buttonSlew1.Text = "N";
                //buttonSlew2.Text = "S";
                //buttonSlew3.Text = "E";
                //buttonSlew4.Text = "W";
            }
        }

        public double HourAngle
        {
            get
            {
                string res = GeminiHardware.Instance.DoCommandResult(":GH", 2000, false);               
                return GeminiHardware.Instance.m_Util.HMSToHours(res);
            }        
        }

        public double SiderealTime
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetLstText);
                string text = GeminiHardware.Instance.m_Util.HoursToHMS(value,":",":",""); // .ConvertDoubleToHMS(value);
               
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
                string text = GeminiHardware.Instance.m_Util.HoursToHMS(value, ":", ":", ""); 
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
                string text = GeminiHardware.Instance.m_Util.DegreesToDMS(value, ":", ":", ""); 
                try {
                    if (InvokeRequired)
                        this.BeginInvoke(setText, text);
                    else
                        SetDecText(text);
                }
                catch { }
            }
        }

        public double HourAngleDisplay
        {
            set
            {
                SetTextCallback setText = new SetTextCallback(SetHAText);
                string text = GeminiHardware.Instance.m_Util.HoursToHMS(value, ":", ":", "");
                try
                {
                    if (InvokeRequired)
                        this.BeginInvoke(setText, text);
                    else
                        SetHAText(text);
                }
                catch { }
            }
        }
       
        private void frmMain_Load(object sender, EventArgs e)
        {

            try
            {
                Speech.SpeechInitialize(this.Handle, GeminiHardware.Instance.UseSpeech ? GeminiHardware.Instance.SpeechVoice : null);
                Speech.Filter = GeminiHardware.Instance.SpeechFilter;
            }
            catch
            {
            }

            if (GeminiHardware.Instance.MainFormPosition != Point.Empty)
            {
                Rectangle rc = new Rectangle(GeminiHardware.Instance.MainFormPosition, this.Size);
                Screen scr = Screen.FromControl(this);

                // only set the recorded position if the window is at all visible on the current screen:
                if (scr.Bounds.IntersectsWith(rc))
                    this.Location = GeminiHardware.Instance.MainFormPosition;
            }

            SharedResources.SetTopWindow(this);

            SetSlewButtons();
            SetTopMost();
            if (!GeminiHardware.Instance.ShowHandbox && GeminiTelescope.m_bComStart) this.Hide();
            GeminiHardware.Instance.Profile = null;
            
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

        private void SetHAText(string text)
        {
            labelHA.Text = text;
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
            frmAdvancedSettings parametersForm = new frmAdvancedSettings();


            DialogResult ans = parametersForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {

            }
            parametersForm.Dispose();
            GeminiHardware.Instance.Profile = null;
        }

        private void UpdateConnectStatus()
        {
            if (GeminiHardware.Instance.Connected)
            {
                this.ButtonConnect.Text = Resources.Disconnect;
            }
            else
            {
                ButtonConnect.Text = Resources.Connect;
                ButtonConnect.Update();
            }
        }

        private void SetConnectText(string txt)
        {
            ButtonConnect.Text = txt;
        }

        public static void DoConnectAsync(object stateInfo)
        {
            frmMain frm = stateInfo as frmMain;

            try
            {
                GeminiHardware.Instance.Connected = true;
            }
            catch {
            }

            if (!GeminiHardware.Instance.Connected)
            {
                MessageBox.Show(Resources.CannotConnect + "\r\n" + frm.m_LastError, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                frm.BeginInvoke(new SetTextCallback(frm.SetConnectText), Resources.Connect);
            }
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.IsConnecting)
            {
                GeminiHardware.Instance.IsConnecting = false;
            }
            else
            if (!GeminiHardware.Instance.Connected)
            {
                Speech.SayIt(Resources.Connect, Speech.SpeechType.Command);
                ButtonConnect.Text = Resources.Stop;                
                System.Threading.ThreadPool.QueueUserWorkItem(DoConnectAsync, this);
                return;
            }
            else
            {
                Speech.SayIt(Resources.Disconnect, Speech.SpeechType.Command);
                if (GeminiHardware.Instance.Clients > 1)
                {
                    DialogResult res = MessageBox.Show(Resources.DisconnectWarning,
                        SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                    if (res != DialogResult.Yes)
                        return;
                }
                
                while (GeminiHardware.Instance.Clients > 0)
                    try
                    {
                        GeminiHardware.Instance.Connected = false;
                    }
                    catch { }

                if (GeminiHardware.Instance.Connected != false)
                    MessageBox.Show(Resources.CannotDisconnect, SharedResources.TELESCOPE_DRIVER_NAME);

                UpdateConnectStatus();
            }

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeminiHardware.Instance.MainFormPosition = this.Location;
 
            //If we hit the close X on the form then hide it. If called from exit menu then exit for real.
            if (!m_ExitFormMenuCall)
            {
                e.Cancel = true;
                this.Hide();
                SetBaloonText(Resources.DriverRunning, Resources.DriverMinimized, ToolTipIcon.Info);
                return;
            }
            m_ExitFormMenuCall = false;
            if (GeminiHardware.Instance.Connected && GeminiHardware.Instance.Clients > 0)
            {
                DialogResult res = MessageBox.Show(Resources.discWarning1 + (GeminiHardware.Instance.Clients > 1 ? Resources.discWarning2 : " " + Resources.discWarning3) + " " + Resources.discWarning4 + Resources.GeminiConnectionAlive1,
                    SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            while (GeminiHardware.Instance.Clients > 0)  //disconnect all clients
                GeminiHardware.Instance.Connected = false;

            GeminiHardware.Instance.CloseStatusForm();
            
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
            if (GeminiHardware.Instance.Connected)
            {

                GeminiHardware.Instance.DoCommand(":Q",false);
            }
        }


        private void buttonSlew1_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                Speech.SayIt(Resources.UpButton, Speech.SpeechType.Command);
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

                GeminiHardware.Instance.DoCommand(cmds, false);

            }
        }

        private void buttonSlew1_MouseUp(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;
            if (GeminiHardware.Instance.Connected)
            {

                if (CheckBoxFlipDec.Checked)
                {
                    GeminiHardware.Instance.DoCommand(":Qs", false);
                }
                else
                {
                    GeminiHardware.Instance.DoCommand(":Qn", false);
                }
            }
        }

        private void buttonSlew2_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                Speech.SayIt(Resources.DownButton, Speech.SpeechType.Command);

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

                GeminiHardware.Instance.DoCommand(cmds, false);
            }
        }

        private void buttonSlew2_MouseUp(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;

            if (GeminiHardware.Instance.Connected)
            {
                if (CheckBoxFlipDec.Checked)
                {
                    GeminiHardware.Instance.DoCommand(":Qn", false);
                }
                else
                {
                    GeminiHardware.Instance.DoCommand(":Qs", false);
                }
            }
        }

        private void buttonSlew4_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
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
                GeminiHardware.Instance.DoCommand(cmds, false);
            }
        }

        private void buttonSlew4_MouseUp(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;

            if (GeminiHardware.Instance.Connected)
            {
                if (CheckBoxFlipRa.Checked)
                {
                    GeminiHardware.Instance.DoCommand(":Qe", false);
                }
                else
                {
                    GeminiHardware.Instance.DoCommand(":Qw", false);
                }
            }
        }

        private void buttonSlew3_MouseDown(object sender, MouseEventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
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
                GeminiHardware.Instance.DoCommand(cmds, false);

            }
        }

        private void buttonSlew3_MouseUp(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;
            if (GeminiHardware.Instance.Connected)
            {
                if (CheckBoxFlipRa.Checked)
                {
                    GeminiHardware.Instance.DoCommand(":Qw", false);
                }
                else
                {
                    GeminiHardware.Instance.DoCommand(":Qe", false);
                }
            }
        }

        private void toolStripMenuParkHere_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                DialogResult res = MessageBox.Show("Do you really want to " + Resources.ParkHere + "?", "Telescope Park", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    Speech.SayIt(Resources.ParkHere, Speech.SpeechType.Command);
                    GeminiHardware.Instance.DoParkAsync(GeminiHardwareBase.GeminiParkMode.NoSlew);
                }
//                GeminiHardware.Instance.DoCommand(":hN", false);
            }
        }

        private void toolStripMenuParkCWD_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                DialogResult res = MessageBox.Show("Do you really want to " + Resources.ParkCWD+ "?", "Telescope Park", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    this.UseWaitCursor = true;
                    Speech.SayIt(Resources.ParkCWD, Speech.SpeechType.Command);
                    GeminiHardware.Instance.DoParkAsync(GeminiHardwareBase.GeminiParkMode.SlewCWD);
                    //                GeminiHardware.Instance.DoCommand(":hC", false);
                    this.UseWaitCursor = false;
                }
            }
        }

        private void toolStripMenuParkHome_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                DialogResult res = MessageBox.Show("Do you really want to " + Resources.ParkAtHome+ "?", "Telescope Park", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {

                    this.UseWaitCursor = true;
                    Speech.SayIt(Resources.ParkHome, Speech.SpeechType.Command);
                    GeminiHardware.Instance.DoParkAsync(GeminiHardwareBase.GeminiParkMode.SlewHome);
                    //                GeminiHardware.Instance.DoCommand(":hP", false);
                    this.UseWaitCursor = false;
                }
            }
        }

        private void checkBoxTrack_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                checkBoxTrack.Checked = !checkBoxTrack.Checked;

                if (!checkBoxTrack.Checked)
                    GeminiHardware.Instance.DoCommand(":hN", false);
                else
                    GeminiHardware.Instance.DoCommand(":hW", false);
            }
        }

        private void pbStop_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                GeminiHardware.Instance.AbortSlew();
                Speech.SayIt(Resources.StopSlew, Speech.SpeechType.Command);
            }
        }

        private void frmMain_VisibleChanged(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.UseJoystick) 
                if (this.Visible)
                    if (GeminiHardware.Instance.UseJoystick)
                        if (GeminiHardware.Instance.Connected) StartJoystick();
                     else
                         tmrJoystick.Stop();

            m_BalloonMenu.MenuItems[Resources.Control].Checked = this.Visible;
        }

        private void aboutGeminiDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Speech.SayIt(Resources.AboutGeminiDriver, Speech.SpeechType.Command);

//            GeminiAbout.MainWindow win = new GeminiAbout.MainWindow();
//            win.Show();
            AboutBox1 box = new AboutBox1();
            box.ShowDialog(this);
        }

        private void checkboxPEC_Clicked(object sender, EventArgs e)
        {
            checkboxPEC.Checked = !checkboxPEC.Checked;
            
            byte pec = GeminiHardware.Instance.PECStatus;
            if (pec != 0xff)
            {
                pec = (byte)((pec & 0xfe) | (checkboxPEC.Checked ? 1 : 0));
                GeminiHardware.Instance.PECStatus = pec;
            }
        }


        private void ButtonFlip_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                Speech.SayIt(Resources.MedidianFlip, Speech.SpeechType.Command);
                string res = GeminiHardware.Instance.DoMeridianFlip();
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

        private void exitDriverMenuItem_Click(object sender, EventArgs e)
        {
            ExitMenu(sender, e);
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {

                if (GeminiHardware.Instance.TargetDeclination != SharedResources.INVALID_DOUBLE || GeminiHardware.Instance.TargetRightAscension != SharedResources.INVALID_DOUBLE)
                {
                    try
                    {
                        GeminiHardware.Instance.SyncEquatorial();
                        GeminiHardware.Instance.ReportAlignResult("Sync");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gemini reported error: " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show(Resources.NoTarget, SharedResources.TELESCOPE_DRIVER_NAME);
                }
            }
        }

        private void FuncMenu_Click(object sender, EventArgs e)
        {
            FuncMenu.ContextMenuStrip.Show(Cursor.Position);
        }

        private void buttonAddlAlign_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                if (GeminiHardware.Instance.TargetDeclination != SharedResources.INVALID_DOUBLE || GeminiHardware.Instance.TargetRightAscension != SharedResources.INVALID_DOUBLE)
                {
                    try
                    {
                        GeminiHardware.Instance.AlignEquatorial();
                        GeminiHardware.Instance.ReportAlignResult("Additional Align");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gemini reported error: " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                { MessageBox.Show(Resources.NoTarget, SharedResources.TELESCOPE_DRIVER_NAME); }
            }
        }

        private void SetTopMost()
        {
            this.TopMost = GeminiHardware.Instance.KeepMainFormOnTop;
            //keepThisWindowOnTopToolStripMenuItem.Checked = GeminiHardware.Instance.KeepMainFormOnTop;
            if (GeminiHardware.Instance.KeepMainFormOnTop) keepThisWindowOnTopToolStripMenuItem.Image = Properties.Resources.Ticked;
            else keepThisWindowOnTopToolStripMenuItem.Image = Properties.Resources.UnTicked;
        }

        private void keepThisWindowOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeminiHardware.Instance.KeepMainFormOnTop = !GeminiHardware.Instance.KeepMainFormOnTop;
            SetTopMost();
        }

        static frmUserCatalog frmCatalog;

        public void DoCatalogManagerDialog()
        {
            configureCatalogsToolStripMenuItem_Click(null, null);
        }

        private void configureCatalogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmCatalog==null || frmCatalog.IsDisposed || !frmCatalog.Visible)
                frmCatalog = new frmUserCatalog();
            frmCatalog.Visible = false;
            frmCatalog.Show(this);
        }

        static frmObservationLog frmObservation = null;

        private void observationLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmObservation==null || frmObservation.IsDisposed || !frmObservation.Visible)
                frmObservation = new frmObservationLog();
            frmObservation.Visible = false;
            frmObservation.Show(this);
        }

        private void setCustomParkPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Speech.SayIt(Resources.ConfigurePark, Speech.SpeechType.Command); 

            frmParkPosition frm = new frmParkPosition();
            frm.ShowDialog(this);
        }

        private void parkAtCustomParkPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                string park = "";

                switch (GeminiHardware.Instance.ParkPosition)
                {
                    case GeminiHardwareBase.GeminiParkMode.NoSlew: park = Resources.ParkAtCurrent; break;
                    case GeminiHardwareBase.GeminiParkMode.SlewAltAz: park = Resources.ParkAtAltAz; break;
                    case GeminiHardwareBase.GeminiParkMode.SlewCWD: park = Resources.ParkAtCWD; break;
                    case GeminiHardwareBase.GeminiParkMode.SlewHome: park = Resources.ParkAtHome; break;
                    default: park = Resources.Park; break;
                }

                DialogResult res = MessageBox.Show("Do you really want to " + park + "?", "Telescope Park", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    Speech.SayIt(Resources.Park, Speech.SpeechType.Command);
                    GeminiHardware.Instance.DoParkAsync(GeminiHardware.Instance.ParkPosition);
                }
            }
        }

        private void unparkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                Speech.SayIt(Resources.Unpark, Speech.SpeechType.Command);
                GeminiHardware.Instance.DoCommand(":hW", false);
            }
        }

        private void buttonSlew3_Click(object sender, EventArgs e)
        {

        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            System.IO.FileInfo fi = new System.IO.FileInfo(path);

            path = System.IO.Path.Combine(fi.DirectoryName, Resources.HelpFileName);
            if (System.IO.File.Exists(path))
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(path);
                    if (p != null)
                        p.WaitForInputIdle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot launch help file: \r\n" + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            else
                MessageBox.Show("Cannot locate help file: \r\n " + path, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == m_LastKey && e.Modifiers == m_LastModifiers) return;   //auto-repeat, ignore
            m_LastModifiers = e.Modifiers;
            m_LastKey = e.KeyCode;

            if (e.Modifiers == Keys.None)
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        viewHelpToolStripMenuItem_Click(sender, null);  break;
                    case Keys.Escape:
                        pbStop_Click(sender, null); break;
                    case Keys.G:
                        RadioButtonGuide.PerformClick(); break;
                    case Keys.C:
                        RadioButtonCenter.PerformClick(); break;
                    case Keys.S:
                        RadioButtonSlew.PerformClick(); break;
                }
            else if (e.Modifiers == Keys.Control )
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        buttonSlew1_MouseDown(this, null); break ;
                    case Keys.Down:
                        buttonSlew2_MouseDown(this, null); break;
                    case Keys.Left:
                        buttonSlew3_MouseDown(this, null); break;
                    case Keys.Right:
                        buttonSlew4_MouseDown(this, null); break;
                }
            }
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            m_LastKey = Keys.None;
            m_LastModifiers = Keys.None;

            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        buttonSlew1_MouseUp(this, null); break ;
                    case Keys.Down:
                        buttonSlew2_MouseUp(this, null); break;
                    case Keys.Left:
                        buttonSlew3_MouseUp(this, null); break;
                    case Keys.Right:
                        buttonSlew4_MouseUp(this, null); break ;
                }
            }
        }

        private void frmMain_Move(object sender, EventArgs e)
        {
        }

        private void buttonSlew1_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        frmRA_DEC frmObject = null;

        private void objectAndCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (frmObject == null || frmObject.IsDisposed || !frmObject.Visible)
                frmObject = new frmRA_DEC();
            frmObject.Visible = false;


            frmObject.Left = this.Right;
            frmObject.Top = Cursor.Position.Y - frmObject.Height / 2;
            if (frmObject.Left + frmObject.Width / 2 > Screen.FromControl(this).WorkingArea.Width)
                frmObject.Left = Screen.FromControl(this).WorkingArea.Width - frmObject.Width;
            frmObject.Show(this);
        }

    }
}