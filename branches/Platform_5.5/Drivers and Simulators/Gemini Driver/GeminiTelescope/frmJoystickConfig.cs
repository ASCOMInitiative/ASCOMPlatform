
//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Telescope Joystick configuration form
//
// Description:	This implements various joystick options for Gemini
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//              
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 22-SEP-2009	pk  1.0.0	Initial creation
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using ASCOM.GeminiTelescope.Properties;

namespace ASCOM.GeminiTelescope
{
    public partial class frmJoystickConfig : Form
    {

        public delegate void FireCommandDelegate();

        public class AssignmentClass
        {
            public string caption;
            public UserFunction func;
            public string tooltip;

 
            public AssignmentClass(string cap, UserFunction f)
            {
                caption = cap;
                func = f;
                tooltip = "";
            }

            public AssignmentClass(string cap, UserFunction f, string tt)
            {
                caption = cap;
                func = f;
                tooltip = tt;
            }

            public override string ToString()
            {
                return caption;
            }

            public AssignmentClass Self
            {
                get { return this; }
            }

            public string Display
            {
                get { return caption; }
            }
        }

        static AssignmentClass [] AllAssignments = 
        {
            new AssignmentClass("------------------------", UserFunction.Unassigned),
/*
            new AssignmentClass("Use Guiding Speed with Joystick", UserFunction.GuidingSpeed),
            new AssignmentClass("Use Centering Speed with Joystick", UserFunction.CenteringSpeed),
            new AssignmentClass("Use Slewing Speed with Joystick", UserFunction.SlewingSpeed),
            new AssignmentClass("Toggle Joystick Speed: Guiding/Centering/Slewing", UserFunction.ToggleSpeed),            
*/ 
            new AssignmentClass(Resources.AccJoystick, UserFunction.AccelerateSlew),
            new AssignmentClass(Resources.DecJoystick, UserFunction.DecelerateSlew),
            new AssignmentClass(Resources.HC_UP, UserFunction.HandUp),
            new AssignmentClass(Resources.HC_DN, UserFunction.HandDown),
            new AssignmentClass(Resources.HC_LF, UserFunction.HandLeft),
            new AssignmentClass(Resources.HC_RT, UserFunction.HandRight),
            new AssignmentClass(Resources.HC_MENU, UserFunction.HandMenu), // Menu button cmd doesn't work when Gemini is in local controller mode
            new AssignmentClass(Resources.PERF_FLIP, UserFunction.MeridianFlip),
            new AssignmentClass(Resources.ParkCWD, UserFunction.ParkCWD),
            new AssignmentClass(Resources.GoHome, UserFunction.GoHome),
            new AssignmentClass(Resources.StopSlew, UserFunction.StopSlew),
            new AssignmentClass(Resources.StopTracking, UserFunction.StopTrack),
            new AssignmentClass(Resources.StartTracking, UserFunction.StartTrack),
            new AssignmentClass(Resources.VisualMode, UserFunction.VisualMode),
            new AssignmentClass(Resources.AllSpeedsMode, UserFunction.AllSpeedMode),
            new AssignmentClass(Resources.PhotoMode, UserFunction.PhotoMode),
            new AssignmentClass(Resources.ToggleVAP, UserFunction.ToggleMode),
            new AssignmentClass(Resources.MoveFocusIn, UserFunction.FocuserIn),
            new AssignmentClass(Resources.MoveFocusOut, UserFunction.FocuserOut),
            new AssignmentClass(Resources.SetFastFocuser, UserFunction.FocuserFast),
            new AssignmentClass(Resources.SetMediumFocuser, UserFunction.FocuserMedium),
            new AssignmentClass(Resources.SetSlowFocuser, UserFunction.FocuserSlow),
            new AssignmentClass(Resources.ToggleFocuserSpeed, UserFunction.ToggleFocuserSpeed),
            new AssignmentClass(Resources.ObjSrch2, UserFunction.Search2),
            new AssignmentClass(Resources.ObjSrch1, UserFunction.Search1),
            new AssignmentClass(Resources.Sync, UserFunction.Sync),
            new AssignmentClass(Resources.Align, UserFunction.Align),
            new AssignmentClass(Resources.LimitOff, UserFunction.LimitSwitchOff)
        };


        Joystick m_JS = new Joystick();
        Timer tmrJoystick = new Timer();

        private string m_JoystickName;

        public string JoystickName
        {
            get { return m_JoystickName; }
            set { m_JoystickName = value; }
        }


        public frmJoystickConfig()
        {
            InitializeComponent();
        }

        private void frmJoystickConfig_Load(object sender, EventArgs e)
        {
            ButtonGrid.Rows.Add(36);    // 32 buttons and 4 POV directions
            ButtonGrid.RowHeadersVisible = false;

            string [] dir = new string[] { "Up", "Down", "Left", "Right" };

            ButtonGrid.DataError += new DataGridViewDataErrorEventHandler(ButtonGrid_DataError);
            for (int i = 0; i < 36; ++i)
            {
                DataGridViewTextBoxCell txt = ((DataGridViewTextBoxCell)ButtonGrid.Rows[i].Cells[0]);
                if (i >= 32)
                {
                    txt.Value = "POV " + dir[i - 32];

                } else 
                    txt.Value = "Button " + (i + 1).ToString();

                txt.Style.BackColor = Color.Black;
                txt.Style.ForeColor = Color.LightGray;
                //txt.Frozen = true;
                txt.ReadOnly = true;

                DataGridViewComboBoxCell cmb = ((DataGridViewComboBoxCell)ButtonGrid.Rows[i].Cells[1]);
                cmb.DisplayMember = "Display";
                cmb.ValueMember = "Self";

                cmb.FlatStyle = FlatStyle.Flat;
                cmb.Sorted = false;
                cmb.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                cmb.Style.BackColor = Color.Black;
                cmb.Style.ForeColor = Color.LightGray;
                cmb.DisplayStyleForCurrentCellOnly = true;
                
                foreach (AssignmentClass asc in AllAssignments)
                {
                    int idx = cmb.Items.Add(asc);
                }
            }


            if (!string.IsNullOrEmpty(JoystickName))
            {
                if (!m_JS.Initialize(JoystickName, GeminiHardware.Instance.JoystickAxisRA, GeminiHardware.Instance.JoystickAxisDEC))
                    m_JS = null;
                else
                {
                    tmrJoystick.Tick += new EventHandler(tmrJoystick_Tick);
                    tmrJoystick.Interval = 200;
                    tmrJoystick.Start();

                    int axis = m_JS.NumberOfAxis;
                    for (int i = 0; i < axis; i++)
                    {
                        cmbAxisRA.Items.Add("Axis " + (i + 1).ToString() );
                        cmbAxisDEC.Items.Add("Axis " + (i + 1).ToString());
                    }

//                    if (m_JS.HasPOV4)
//                        cmbAxis.Items.Add("POV 4-way Controller");

                    // default to axis 0 and 1 (X & Y):
                    if (cmbAxisRA.Items.Count > 0)
                        cmbAxisRA.SelectedIndex = 0;

                    if (cmbAxisDEC.Items.Count > 1)
                        cmbAxisDEC.SelectedIndex = 1;

                }
            }

            PersistProfile(false);  // load all settings from profile
        }

        ulong prev_buttons = 0;

        void tmrJoystick_Tick(object sender, EventArgs e)
        {
            double  x = m_JS.PosX;  //need this to poll the joystick!
            ulong buttons = m_JS.ButtonState;
            if (buttons != prev_buttons)
            {
                ulong mask = 1;

                for (int i = 0; i < 36; ++i, mask <<= 1)
                {
                    // button changed state to pressed?
                    if ((buttons & mask) != (prev_buttons & mask) && (buttons & mask) != 0)
                    {
                        ButtonGrid.CurrentCell = ButtonGrid.Rows[i].Cells[1];
                        break;
                    }
                }
            }
            prev_buttons = buttons;

        }

        void ButtonGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;    
        }

        private void ButtonGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        public bool PersistProfile(bool Save)
        {
            UserFunction[] button_mappings = new UserFunction[ButtonGrid.Rows.Count];

            if (Save)
            {
                for (int i = 0; i < ButtonGrid.Rows.Count; ++i)
                {
                    AssignmentClass ac = ButtonGrid.Rows[i].Cells[1].Value as AssignmentClass;
                    if (ac != null)
                        button_mappings[i] = ac.func;
                    else
                        button_mappings[i] = (int)UserFunction.Unassigned;
                }
                GeminiHardware.Instance.JoystickButtonMap = button_mappings;
                GeminiHardware.Instance.JoystickIsAnalog = rbAnalog.Checked;
                if (cmbAxisRA.SelectedIndex >= 0)
                {
                    GeminiHardware.Instance.JoystickAxisRA = cmbAxisRA.SelectedIndex;
                }
                if (cmbAxisDEC.SelectedIndex >= 0)
                {
                    GeminiHardware.Instance.JoystickAxisDEC = cmbAxisDEC.SelectedIndex;
                }

                GeminiHardware.Instance.JoystickFlipRA = ckFlipRA.Checked;
                GeminiHardware.Instance.JoystickFlipDEC = ckFlipDec.Checked;

                try
                {
                    GeminiHardware.Instance.JoystickSensitivity = (double)txtSensitivity.Value;
                }
                catch { }

                GeminiHardware.Instance.Profile = null;

                return true;
            }
            else
            {
                button_mappings = GeminiHardware.Instance.JoystickButtonMap;
                if (button_mappings.Length < 36) Array.Resize<UserFunction>(ref button_mappings, 36); // increase to include POV settings for profiles written with an older driver
                for (int i = 0; i < ButtonGrid.Rows.Count; ++i)
                {
                    AssignmentClass ac = FindAssignmentClass(button_mappings[i]);
                    ButtonGrid.Rows[i].Cells[1].Value = ac;
                }
                rbAnalog.Checked = GeminiHardware.Instance.JoystickIsAnalog;
                rbFixed.Checked = !GeminiHardware.Instance.JoystickIsAnalog;

                if (cmbAxisRA.Items.Count > GeminiHardware.Instance.JoystickAxisRA)
                    cmbAxisRA.SelectedIndex = GeminiHardware.Instance.JoystickAxisRA;

                if (cmbAxisDEC.Items.Count > GeminiHardware.Instance.JoystickAxisDEC)
                    cmbAxisDEC.SelectedIndex = GeminiHardware.Instance.JoystickAxisDEC;

                ckFlipRA.Checked = GeminiHardware.Instance.JoystickFlipRA;
                ckFlipDec.Checked = GeminiHardware.Instance.JoystickFlipDEC;

                try
                {
                    txtSensitivity.Value = (decimal)GeminiHardware.Instance.JoystickSensitivity;
                }
                catch
                {
                    txtSensitivity.Value = 100;
                }

                GeminiHardware.Instance.Profile = null;
                return true;
            }

        }

        public static AssignmentClass FindAssignmentClass(UserFunction val)
        {
            for (int i = 0; i < AllAssignments.Length; ++i)
                if (AllAssignments[i].func == val)
                    return AllAssignments[i];
            return AllAssignments[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ButtonGrid.EndEdit();
            if (cmbAxisDEC.SelectedIndex >= 0 &&
                cmbAxisDEC.SelectedIndex == cmbAxisRA.SelectedIndex)
            {
                MessageBox.Show(Resources.SameAxis, SharedResources.TELESCOPE_DRIVER_NAME);
                DialogResult = DialogResult.None;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ButtonGrid.Rows.Count; ++i)
                ButtonGrid.Rows[i].Cells[1].Value = AllAssignments[0];
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
