using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.DriverAccess;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace TestHarness
{
    public partial class Form1 : Form
    {
        private Focuser myFoc1;
        private Focuser myFoc2;

        private bool F1LastConnectedState;
        private bool F2LastConnectedState;
        private delegate void statusUpdater();

        public Form1()
        {
            InitializeComponent();
                     
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Set the view menu checked states
                positionAndTemperatureToolStripMenuItem.Checked = Properties.Settings.Default.DisplayPositionAndTemperature;
                relativeFocusAdjustToolStripMenuItem.Checked = Properties.Settings.Default.DisplayRelativeFocusAdjust;
                temperatureCompensationToolStripMenuItem.Checked = Properties.Settings.Default.DisplayTempComp;
                absoluteFocusAdjustToolStripMenuItem.Checked = Properties.Settings.Default.DisplayAbsoluteFocusAdjust;
                relativeFocusOffsetsToolStripMenuItem.Checked = Properties.Settings.Default.DisplayRelativeFocusOffsets;
                absoluteFocusPresetsToolStripMenuItem.Checked = Properties.Settings.Default.DisplayAbsoluteFocusPresets;
                
                // hook up the view item checked_changed events
                positionAndTemperatureToolStripMenuItem.CheckedChanged += new EventHandler(this.viewItemToolStripMenuItem_CheckStateChanged);
                relativeFocusAdjustToolStripMenuItem.CheckedChanged += new EventHandler(this.viewItemToolStripMenuItem_CheckStateChanged);
                temperatureCompensationToolStripMenuItem.CheckedChanged += new EventHandler(this.viewItemToolStripMenuItem_CheckStateChanged);
                absoluteFocusAdjustToolStripMenuItem.CheckedChanged += new EventHandler(this.viewItemToolStripMenuItem_CheckStateChanged);
                relativeFocusOffsetsToolStripMenuItem.CheckedChanged += new EventHandler(this.viewItemToolStripMenuItem_CheckStateChanged);
                absoluteFocusPresetsToolStripMenuItem.CheckedChanged += new EventHandler(this.viewItemToolStripMenuItem_CheckStateChanged);
                alwaysOnTopToolStripMenuItem.CheckedChanged += new EventHandler(alwaysOnTopToolStripMenuItem_CheckedChanged);
                alwaysOnTopToolStripMenuItem.Checked = Properties.Settings.Default.AlwaysOnTop; // We do this after so the change takes affect now...
                
                // adjust form height for user preferences
                setFormHeight();

                if (!Properties.Settings.Default.SwitchF1F2)
                {
                    myFoc1 = new ASCOM.DriverAccess.Focuser("ASCOM.OptecFocuserHub.Focuser");
                    myFoc2 = new ASCOM.DriverAccess.Focuser("ASCOM.OptecFocuserHub2.Focuser");
                }
                else
                {
                    myFoc1 = new ASCOM.DriverAccess.Focuser("ASCOM.OptecFocuserHub2.Focuser");
                    myFoc2 = new ASCOM.DriverAccess.Focuser("ASCOM.OptecFocuserHub.Focuser");
                }
                F1LastConnectedState = !myFoc1.Link;    // Set these to the opposite so that controls are immediatly refreshed.
                F2LastConnectedState = !myFoc2.Link;
               // ASCOM.OptecFocuserHub.IHubPrivateAccess hubAccessor;
               // Type oType = Type.GetTypeFromProgID("ASCOM.OptecFocuserHub.HubPrivateAccess");
               // hubAccessor = (ASCOM.OptecFocuserHub.IHubPrivateAccess)Activator.CreateInstance(oType); 

            }
            catch (Exception ex)
            {
                LogMessage("An error occurred while creating instances of Focuser Hub ASCOM Driver. " + ex.Message);
                MessageBox.Show("An error occurred while creating the initial instances of the OptecFocuserHub ASCOM Driver." +
                    " The program cannot continue. Please contact Optec tech support.");
                Application.Exit();
            }

            LoadFocusOffsets_F1();
            LoadFocusPresets_F1();
            LoadFocusOffsets_F2();
            LoadFocusPresets_F2();

            setFormWidth();
            //setFormHeight(); Automatically occurs.
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            statusUpdateTimer.Enabled = false;
        }

        private void LoadFocusOffsets_F1()
        {
            try
            {
                int count = 0;
                int spacing = 20;
                int topMargin = 0;
                int leftMargin = 10;

                // First clear the panel
                F1FocusOffsetPanel.Controls.Clear();
                Application.DoEvents();
                int j = Properties.Settings.Default.F1FocusOffsets.Count;
                for (int i = 0; i < Properties.Settings.Default.F1FocusOffsets.Count; i++)
                {
                    string s = Properties.Settings.Default.F1FocusOffsets[i];
                    if (s.Count(p => p.ToString() == ",") != 1)
                    {
                        Properties.Settings.Default.F1FocusOffsets.RemoveAt(i);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        RadioButton r = new RadioButton();
                        r.Text = s.Split(',')[0] + " : " + s.Split(',')[1];
                        r.Location = new Point(leftMargin, topMargin + (spacing * count));
                        r.Tag = s.Split(',')[1];
                        //if (count == 0) r.Checked = true;
                        r.CheckedChanged += new EventHandler(focusOffset_CheckChanged);
                        F1FocusOffsetPanel.Controls.Add(r);
                        count++;
                    }    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Focus Offsets");
            }

        }

        private void LoadFocusPresets_F1()
        {
            try
            {
                int count = 0;
                int spacing = 20;
                int topMargin = 0;
                int leftMargin = 10;

                // First clear the panel
                this.F1AbsolutePresetPanel.Controls.Clear();
                Application.DoEvents();

                for (int i = 0; i < Properties.Settings.Default.F1FocusPresets.Count; i++)
                {
                    string s = Properties.Settings.Default.F1FocusPresets[i];
                    if (s.Count(p => p.ToString() == ",") != 1)
                    {
                        Properties.Settings.Default.F1FocusPresets.RemoveAt(i);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        RadioButton r = new RadioButton();
                        r.Text = s.Split(',')[0] + " : " + s.Split(',')[1];
                        r.Location = new Point(leftMargin, topMargin + (spacing * count));
                        r.Tag = s.Split(',')[1];
                        //if (count == 0) r.Checked = true;
                        r.CheckedChanged += new EventHandler(focusPreset_CheckChanged);
                        F1AbsolutePresetPanel.Controls.Add(r);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Focus Presets");
            }

        }

        private void LoadFocusOffsets_F2()
        {
            try
            {
                int count = 0;
                int spacing = 20;
                int topMargin = 0;
                int leftMargin = 10;

                // First clear the panel
                F2FocusOffsetPanel.Controls.Clear();
                Application.DoEvents();

                for (int i = 0; i < Properties.Settings.Default.F2FocusOffsets.Count; i++)
                {
                    string s = Properties.Settings.Default.F2FocusOffsets[i];
                    if (s.Count(p => p.ToString() == ",") != 1)
                    {
                        Properties.Settings.Default.F2FocusOffsets.RemoveAt(i);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        RadioButton r = new RadioButton();
                        r.Text = s.Split(',')[0] + " : " + s.Split(',')[1];
                        r.Location = new Point(leftMargin, topMargin + (spacing * count));
                        r.Tag = s.Split(',')[1];
                        //if (count == 0) r.Checked = true;
                        r.CheckedChanged += new EventHandler(focusOffset_CheckChanged);
                        F2FocusOffsetPanel.Controls.Add(r);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Focus Offsets");
            }

        }

        private void LoadFocusPresets_F2()
        {
            try
            {
                int count = 0;
                int spacing = 20;
                int topMargin = 0;
                int leftMargin = 10;

                // First clear the panel
                this.F2AbsolutePresetPanel.Controls.Clear();
                Application.DoEvents();

                for (int i = 0; i < Properties.Settings.Default.F2FocusPresets.Count; i++)
                {
                    string s = Properties.Settings.Default.F2FocusPresets[i];
                    if (s.Count(p => p.ToString() == ",") != 1)
                    {
                        Properties.Settings.Default.F2FocusPresets.RemoveAt(i);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        RadioButton r = new RadioButton();
                        r.Text = s.Split(',')[0] + " : " + s.Split(',')[1];
                        r.Location = new Point(leftMargin, topMargin + (spacing * count));
                        r.Tag = s.Split(',')[1];
                        //if (count == 0) r.Checked = true;
                        r.CheckedChanged += new EventHandler(focusPreset_CheckChanged);
                        F2AbsolutePresetPanel.Controls.Add(r);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Focus Presets");
            }

        }
       
        void focusOffset_CheckChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void focusPreset_CheckChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void statusUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                this.Invoke(new statusUpdater(updateStatusFoc1));

                if (!Properties.Settings.Default.Focuser2Disabled)
                    this.Invoke(new statusUpdater(updateStatusFoc2));
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private void updateStatusFoc1()
        {
            try
            {
                if (myFoc1 != null)
                {
                    if (myFoc1.Link)
                    {
                        // First update the status incase it has changed
                        updateF1PositionAndTemp();
                        if (myFoc1.IsMoving) f1Halt.Enabled = true;
                        else f1Halt.Enabled = false;

                        try
                        {
                            if (myFoc1.TempCompAvailable)
                            {
                                if (myFoc1.TempComp) F1TempModeON_RB.Checked = true;
                                else F1TempModeOFF_RB.Checked = true;
                            }
                            else
                                F1TempModeOFF_RB.Checked = true;
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ToArray();
                        }

                        // Check if the enabed controls are already set -- THESE ARE PROPS THAT ONLY CHANGE BETWEEN CONNECTED AND DISCONNECTED ----------------
                        if (F1LastConnectedState == true)
                            return;
                        // f1NicknameLBL.Text = hubAccessor.F1Nickname;
                        F1LastConnectedState = true;            // So we don't keep entering this
                        PowerLight.Image = Properties.Resources.RedLight;
                        // Set the controls enabled properties
                        F1PosDRO.Enabled = true;
                        F1TempDRO.Enabled = true;
                        F1In_BTN.Enabled = true;
                        F1OUT_Btn.Enabled = true;
                        F1StepSize_NUD.Enabled = true;
                        F1TempModeON_RB.Enabled = true;
                        F1TempModeOFF_RB.Enabled = true;
                        F1AbsoluteMovePanel.Enabled = true;
                        F1FocusOffsetPanel.Enabled = true;
                        F1AbsolutePresetPanel.Enabled = true;
                        StatusLabel.Text = "Hub Connected!";
                    }
                    else
                    {
                        // Check to see if the controls states are already set for disconnected
                        if (F1LastConnectedState == false)
                            return;
                        f1Halt.Enabled = false;
                        f1NicknameLBL.Text = "Focuser 1 - Not Connected";
                        f1Halt.Enabled = false;
                        F1LastConnectedState = false;
                        PowerLight.Image = Properties.Resources.GreyLight;
                        // Set controls for Enabled properties
                        F1PosDRO.Enabled = false;
                        F1TempDRO.Enabled = false;
                        F1In_BTN.Enabled = false;
                        F1OUT_Btn.Enabled = false;
                        F1StepSize_NUD.Enabled = false;
                        F1TempModeON_RB.Enabled = false;
                        F1TempModeOFF_RB.Enabled = false;
                        F1AbsoluteMovePanel.Enabled = false;
                        F1FocusOffsetPanel.Enabled = false;
                        F1AbsolutePresetPanel.Enabled = false;
                        // Set the controls values
                        F1PosDRO.Text = "-----";
                        F1TempDRO.Text = "-----";
                        StatusLabel.Text = "Hub Not Connected";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
      
        private void updateStatusFoc2()
        {

            try
            {
                if (myFoc2 != null)
                {
                    if (myFoc2.Link)
                    {
                        // First update the status incase it has changed
                        updateF2PositionAndTemp();

                        if (myFoc2.IsMoving) f2Halt.Enabled = true;
                        else f2Halt.Enabled = false;

                        if (myFoc2.TempCompAvailable)
                        {
                            if (myFoc2.TempComp) F2TempModeON_RB.Checked = true;
                            else F2TempModeOFF_RB.Checked = true;
                        }
                        else F2TempModeOFF_RB.Checked = true;

                        // Check if the enabed controls are already set
                        if (F2LastConnectedState == true)
                            return;
                        F2LastConnectedState = true;            // So we don't keep entering this

                        F2PosDRO.Enabled = true;
                        F2TempDRO.Enabled = true;
                        F2In_BTN.Enabled = true;
                        F2OUT_Btn.Enabled = true;
                        F2StepSize_NUD.Enabled = true;
                        F2TempModeON_RB.Enabled = true;
                        F2TempModeOFF_RB.Enabled = true;
                        F2AbsoluteMovePanel.Enabled = true;
                        F2FocusOffsetPanel.Enabled = true;
                        F2AbsolutePresetPanel.Enabled = true;
                    }
                    else
                    {
                        // Check to see if the controls are already set
                        if (F2LastConnectedState == false)
                            return;
                        F2LastConnectedState = false;           // So we don't keep entering this
                        // Set controls for Enabled properties
                        F2PosDRO.Enabled = false;
                        F2TempDRO.Enabled = false;
                        F2In_BTN.Enabled = false;
                        F2OUT_Btn.Enabled = false;
                        F2StepSize_NUD.Enabled = false;
                        F2TempModeON_RB.Enabled = false;
                        F2TempModeOFF_RB.Enabled = false;
                        F2AbsoluteMovePanel.Enabled = false;
                        F2FocusOffsetPanel.Enabled = false;
                        F2AbsolutePresetPanel.Enabled = false;
                        // Set the controls values
                        F2PosDRO.Text = "-----";
                        F2TempDRO.Text = "-----";

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void updateF1PositionAndTemp()
        {
            this.F1PosDRO.Text = myFoc1.Position.ToString("00000");
            this.F1TempDRO.Text = myFoc1.Temperature.ToString("000.0°");
            Application.DoEvents();
        }

        private void updateF2PositionAndTemp()
        {
            this.F2PosDRO.Text = myFoc2.Position.ToString("00000");
            this.F2TempDRO.Text = myFoc2.Temperature.ToString("000.0°");
            Application.DoEvents();
        }

        private void setFormWidth()
        {
            if (Properties.Settings.Default.Focuser2Disabled)
            {
                this.MinimumSize = new Size(275, this.MinimumSize.Height);
                this.Size = new Size(275, this.Height);
                this.MaximumSize = new Size(275, int.MaxValue);
            }
            else
            {
                this.MaximumSize = new Size(530, int.MaxValue);
                this.Size = new Size(530, this.Height);
                this.MinimumSize = new Size(530, this.MinimumSize.Height);
            }
        }

        private void setFormHeight()
        {
            int startingHeight = 625;
            const int PTCellHeight = 130;
            const int RFACellHeight = 60;
            const int TCCellHeight = 50;
            const int AFACellHeight = 50;
            const int RFOCellHeight = 75;
            const int AFPCellHeight = 75;

            int newHeight = startingHeight;

            Properties.Settings.Default.DisplayPositionAndTemperature = positionAndTemperatureToolStripMenuItem.Checked;
            if (positionAndTemperatureToolStripMenuItem.Checked)
            {
                tableLayoutPanel1.RowStyles[1].Height = PTCellHeight;
                tableLayoutPanel2.RowStyles[1].Height = PTCellHeight;
            }
            else
            {
                tableLayoutPanel1.RowStyles[1].Height = 0;
                tableLayoutPanel2.RowStyles[1].Height = 0;
                newHeight -= PTCellHeight;
            }

            Properties.Settings.Default.DisplayRelativeFocusAdjust = this.relativeFocusAdjustToolStripMenuItem.Checked;
            if (this.relativeFocusAdjustToolStripMenuItem.Checked)
            {
                tableLayoutPanel1.RowStyles[2].Height = RFACellHeight;
                tableLayoutPanel2.RowStyles[2].Height = RFACellHeight;
            }
            else
            {
                tableLayoutPanel1.RowStyles[2].Height = 0;
                tableLayoutPanel2.RowStyles[2].Height = 0;
                newHeight -= RFACellHeight;
            }

            Properties.Settings.Default.DisplayTempComp = temperatureCompensationToolStripMenuItem.Checked;
            if (temperatureCompensationToolStripMenuItem.Checked)
            {
                tableLayoutPanel1.RowStyles[3].Height = TCCellHeight;
                tableLayoutPanel2.RowStyles[3].Height = TCCellHeight;
            }
            else
            {
                tableLayoutPanel1.RowStyles[3].Height = 0;
                tableLayoutPanel2.RowStyles[3].Height = 0;
                newHeight -= TCCellHeight;
            }

            Properties.Settings.Default.DisplayAbsoluteFocusAdjust = absoluteFocusAdjustToolStripMenuItem.Checked;
            if (absoluteFocusAdjustToolStripMenuItem.Checked)
            {
                tableLayoutPanel1.RowStyles[4].Height = 16;
                tableLayoutPanel2.RowStyles[4].Height = 16;
                tableLayoutPanel1.RowStyles[5].Height = AFACellHeight;
                tableLayoutPanel2.RowStyles[5].Height = AFACellHeight;
            }
            else
            {
                tableLayoutPanel1.RowStyles[4].Height = 0;
                tableLayoutPanel2.RowStyles[4].Height = 0;
                tableLayoutPanel1.RowStyles[5].Height = 0;
                tableLayoutPanel2.RowStyles[5].Height = 0;
                newHeight -= (AFACellHeight + 16);
            }

            Properties.Settings.Default.DisplayRelativeFocusOffsets = relativeFocusOffsetsToolStripMenuItem.Checked;
            if (relativeFocusOffsetsToolStripMenuItem.Checked)
            {
                tableLayoutPanel1.RowStyles[6].Height = 16;
                tableLayoutPanel2.RowStyles[6].Height = 16;
                tableLayoutPanel1.RowStyles[7].Height = RFOCellHeight;
                tableLayoutPanel2.RowStyles[7].Height = RFOCellHeight;
                F1FocusOffsetPanel.Visible = true;
                F2FocusOffsetPanel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[6].Height = 0;
                tableLayoutPanel2.RowStyles[6].Height = 0;
                tableLayoutPanel1.RowStyles[7].Height = 0;
                tableLayoutPanel2.RowStyles[7].Height = 0;
                F1FocusOffsetPanel.Visible = false;
                F2FocusOffsetPanel.Visible = false;
                newHeight -= (RFOCellHeight + 16);
            }

            Properties.Settings.Default.DisplayAbsoluteFocusPresets = absoluteFocusPresetsToolStripMenuItem.Checked;
            if (absoluteFocusPresetsToolStripMenuItem.Checked)
            {
                tableLayoutPanel1.RowStyles[8].Height = 16;
                tableLayoutPanel2.RowStyles[8].Height = 16;
                tableLayoutPanel1.RowStyles[9].Height = AFPCellHeight;
                tableLayoutPanel2.RowStyles[9].Height = AFPCellHeight;
                F1AbsolutePresetPanel.Visible = true;
                F2AbsolutePresetPanel.Visible = true;
            }
            else
            {
                tableLayoutPanel1.RowStyles[8].Height = 0;
                tableLayoutPanel2.RowStyles[8].Height = 0;
                tableLayoutPanel1.RowStyles[9].Height = 0;
                tableLayoutPanel2.RowStyles[9].Height = 0;               
                F1AbsolutePresetPanel.Visible = false;
                F2AbsolutePresetPanel.Visible = false;
                newHeight -= (AFPCellHeight + 16);
            }

            this.Size = new Size(this.Width, newHeight);
            Properties.Settings.Default.Save();
        }

        private void LogMessage(string s)
        {
        }

        private void PowerLight_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                hubConnected = !hubConnected;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                hubConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                hubConnected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool hubConnected
        {
            get { return myFoc1.Link; }
            set 
            {
                try { myFoc1.Link = value; }
                catch
                {
                    if (value) throw new ApplicationException("Unable to connect to Focuser Hub");
                }
            }
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                HubSetupForm hsf = new HubSetupForm();
                hsf.ShowDialog();
                setFormWidth();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void F1In_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                int requestedPos = myFoc1.Position - (int)F1StepSize_NUD.Value;
                if (requestedPos < 0) requestedPos = 0;
                myFoc1.Move(requestedPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void F1OUT_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                int requestedPos = myFoc1.Position + (int)F1StepSize_NUD.Value;
                if (requestedPos > myFoc1.MaxStep) requestedPos = myFoc1.MaxStep;
                myFoc1.Move(requestedPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void F2In_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                int requestedPos = myFoc2.Position - (int)F2StepSize_NUD.Value;
                if (requestedPos < 0) requestedPos = 0;
                myFoc2.Move(requestedPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void F2OUT_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                int requestedPos = myFoc2.Position + (int)F2StepSize_NUD.Value;
                if (requestedPos > myFoc2.MaxStep) requestedPos = myFoc2.MaxStep;
                myFoc2.Move(requestedPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void setupFocuser1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                myFoc1.SetupDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setupFocuser2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myFoc2.SetupDialog();
        }

        private void F1GoToBTN_Click(object sender, EventArgs e)
        {
            try
            {
                MaskedTextBox tb = F1AbsPosTB;
                int requestedPos;
                // Check that the value entered is a number
                if (!int.TryParse(tb.Text, out requestedPos))
                {
                    MessageBox.Show("The requested position is not a valid integer", "Error");
                    return;
                }
                // Check that the value is not less than zero
                if (requestedPos < 0)
                {
                    MessageBox.Show("The requested position must be positive", "Error");
                    return;
                }
                // Check that the value is not greater than MaxStep
                if (requestedPos > myFoc1.MaxStep)
                {
                    MessageBox.Show("The requested position must be less than or equal to " + myFoc1.MaxStep.ToString(), "Error");
                    return;
                }
                // Check that it's not the same position we are already at...
                if (requestedPos == myFoc1.Position) return;
                // Send the move request
                myFoc1.Move(requestedPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void F2GoToBtn_Click(object sender, EventArgs e)
        {
            try
            {
                MaskedTextBox tb = F2AbsPosTB;
                int requestedPos;
                // Check that the value entered is a number
                if (!int.TryParse(tb.Text, out requestedPos))
                {
                    MessageBox.Show("The requested position is not a valid integer", "Error");
                    return;
                }
                // Check that the value is not less than zero
                if (requestedPos < 0)
                {
                    MessageBox.Show("The requested position must be positive", "Error");
                    return;
                }
                // Check that the value is not greater than MaxStep
                if (requestedPos > myFoc2.MaxStep)
                {
                    MessageBox.Show("The requested position must be less than or equal to " + myFoc2.MaxStep.ToString(), "Error");
                    return;
                }
                // Check that it's not the same position we are already at...
                if (requestedPos == myFoc2.Position) return;
                // Send the move request
                myFoc2.Move(requestedPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void F1Center_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                int requestedPosition = myFoc1.MaxStep / 2;
                if (myFoc1.Position == requestedPosition) return;
                // Move to the center position
                myFoc1.Move(requestedPosition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void F2CenterBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int requestedPosition = myFoc2.MaxStep / 2;
                if (myFoc2.Position == requestedPosition) return;
                // Move to the center position
                myFoc2.Move(requestedPosition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void viewItemToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            setFormHeight(); 
        }

        private void F1StepSize_NUD_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void F2StepSize_NUD_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            positionAndTemperatureToolStripMenuItem.Checked = true;
            relativeFocusAdjustToolStripMenuItem.Checked = true;
            temperatureCompensationToolStripMenuItem.Checked = true;
            absoluteFocusAdjustToolStripMenuItem.Checked = true;
            relativeFocusOffsetsToolStripMenuItem.Checked = true;
            absoluteFocusPresetsToolStripMenuItem.Checked = true;
        }

        private void alwaysOnTopToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AlwaysOnTop = alwaysOnTopToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

       

        private void f1Halt_Click(object sender, EventArgs e)
        {
            myFoc1.Halt();
        }

        private void f2Halt_Click(object sender, EventArgs e)
        {
            myFoc2.Halt();
        }

        private void TempComp_Click(object sender, MouseEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null) return;
            try
            {
                switch (rb.Name)
                {
                    case "F1TempModeON_RB":
                        myFoc1.TempComp = true;
                        break;
                    case "F1TempModeOFF_RB":
                        myFoc1.TempComp = false;
                        break;
                    case "F2TempModeON_RB":
                        myFoc2.TempComp = true;
                        break;
                    case "F2TempModeOFF_RB":
                        myFoc2.TempComp = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

    } 
}