﻿
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
            new AssignmentClass("(Not Assigned)", UserFunction.Unassigned),
/*
            new AssignmentClass("Use Guiding Speed with Joystick", UserFunction.GuidingSpeed),
            new AssignmentClass("Use Centering Speed with Joystick", UserFunction.CenteringSpeed),
            new AssignmentClass("Use Slewing Speed with Joystick", UserFunction.SlewingSpeed),
            new AssignmentClass("Toggle Joystick Speed: Guiding/Centering/Slewing", UserFunction.ToggleSpeed),            
*/ 
            new AssignmentClass("Accelerate Joystick Fixed Speed", UserFunction.AccelerateSlew),
            new AssignmentClass("Decelerate Joystick Fixed Speed", UserFunction.DecelerateSlew),
            new AssignmentClass("Hand Controller Up button", UserFunction.HandUp),
            new AssignmentClass("Hand Controller Down button", UserFunction.HandDown),
            new AssignmentClass("Hand Controller Left button", UserFunction.HandLeft),
            new AssignmentClass("Hand Controller Right button", UserFunction.HandRight),
            new AssignmentClass("Hand Controller Menu button", UserFunction.HandMenu), // Menu button cmd doesn't work when Gemini is in local controller mode
            new AssignmentClass("Perform Meridian Flip", UserFunction.MeridianFlip),
            new AssignmentClass("Park at CWD", UserFunction.ParkCWD),
            new AssignmentClass("Go to Home Position", UserFunction.GoHome),
            new AssignmentClass("Stop Slewing", UserFunction.StopSlew),
            new AssignmentClass("Stop Tracking", UserFunction.StopTrack),
            new AssignmentClass("Start Tracking", UserFunction.StartTrack),
            new AssignmentClass("Visual Mode", UserFunction.VisualMode),
            new AssignmentClass("All Speeds Mode", UserFunction.AllSpeedMode),
            new AssignmentClass("Photo Mode", UserFunction.PhotoMode),
            new AssignmentClass("Toggle between Visual, All Speed, and Photo modes", UserFunction.ToggleMode),
            new AssignmentClass("Move Focuser In", UserFunction.FocuserIn),
            new AssignmentClass("Move Focuser Out", UserFunction.FocuserOut),
            new AssignmentClass("Set Fast Focuser Speed", UserFunction.FocuserFast),
            new AssignmentClass("Set Medium Focuser Speed", UserFunction.FocuserMedium),
            new AssignmentClass("Set Slow Focuser Speed", UserFunction.FocuserSlow),
            new AssignmentClass("Toggle Focuser Speed: Slow/Medium/Fast", UserFunction.ToggleFocuserSpeed),
            new AssignmentClass("Object Search 2 Degrees", UserFunction.Search2),
            new AssignmentClass("Object Search 1 Degree", UserFunction.Search1),
        };


        Joystick m_JS = new Joystick();
        Timer tmrJoystick = new Timer();

        public frmJoystickConfig()
        {
            InitializeComponent();
        }

        private void frmJoystickConfig_Load(object sender, EventArgs e)
        {
            ButtonGrid.Rows.Add(32);
            ButtonGrid.RowHeadersVisible = false;

            ButtonGrid.DataError += new DataGridViewDataErrorEventHandler(ButtonGrid_DataError);
            for (int i = 0; i < 32; ++i)
            {
                DataGridViewTextBoxCell txt = ((DataGridViewTextBoxCell)ButtonGrid.Rows[i].Cells[0]);
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

            PersistProfile(false);  // load all settings from profile

            if (!string.IsNullOrEmpty(GeminiHardware.JoystickName))
            {
                if (!m_JS.Initialize(GeminiHardware.JoystickName))
                    m_JS = null;
                else
                {
                    tmrJoystick.Tick += new EventHandler(tmrJoystick_Tick);
                    tmrJoystick.Interval = 200;
                    tmrJoystick.Start();
                }
            }

        }

        uint prev_buttons = 0;

        void tmrJoystick_Tick(object sender, EventArgs e)
        {
            double  x = m_JS.PosX;  //need this to poll the joystick!
            uint buttons = m_JS.ButtonState;
            if (buttons != prev_buttons)
            {
                uint mask = 1;

                for (int i = 0; i < 32; ++i, mask <<= 1)
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
                GeminiHardware.JoystickButtonMap = button_mappings;
                GeminiHardware.JoystickIsAnalog = rbAnalog.Checked;

                return true;
            }
            else
            {
                button_mappings = GeminiHardware.JoystickButtonMap;

                for (int i = 0; i < ButtonGrid.Rows.Count; ++i)
                {
                    AssignmentClass ac = FindAssignmentClass(button_mappings[i]);
                    ButtonGrid.Rows[i].Cells[1].Value = ac;
                }
                rbAnalog.Checked = GeminiHardware.JoystickIsAnalog;
                rbFixed.Checked = !GeminiHardware.JoystickIsAnalog;
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

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ButtonGrid.Rows.Count; ++i)
                ButtonGrid.Rows[i].Cells[1].Value = AllAssignments[0];
        }
    }
}
