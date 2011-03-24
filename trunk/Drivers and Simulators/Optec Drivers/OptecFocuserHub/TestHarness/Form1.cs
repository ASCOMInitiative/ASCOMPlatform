using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.DriverAccess;

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
            setFormHeight();
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
            this.Invoke(new statusUpdater(updateStatusFoc1));

            if(!Properties.Settings.Default.Focuser2Disabled)
                this.Invoke(new statusUpdater(updateStatusFoc2));
        }

        private void updateStatusFoc1()
        {
            if (myFoc1 != null)
            {
                if (myFoc1.Link)
                {
                    // First update the status incase it has changed
                    updateF1PositionAndTemp();

                    // Check if the enabed controls are already set
                    if (F1LastConnectedState == true)
                        return;
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
                }
                else
                {
                    // Check to see if the controls states are already set for disconnected
                    if (F1LastConnectedState == false)
                        return;
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
                }
            }
        }

        
        private void updateStatusFoc2()
        {
            
            if (myFoc2 != null)
            {
                if (myFoc2.Link)
                {
                    // First update the status incase it has changed
                    updateF2PositionAndTemp();

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

        private void updateF1PositionAndTemp()
        {
            this.F1PosDRO.Text = myFoc1.Position.ToString("00000");
            this.F1TempDRO.Text = myFoc1.Temperature.ToString("000.0°");
        }

        private void updateF2PositionAndTemp()
        {
            this.F2PosDRO.Text = myFoc2.Position.ToString("00000");
            this.F2TempDRO.Text = myFoc2.Temperature.ToString("000.0°");
        }

        private void setFormWidth()
        {
            if (Properties.Settings.Default.Focuser2Disabled)
            {
                this.Size = new Size(275, this.Height);
            }
            else
            {
                this.Size = new Size(530, this.Height);
            }
        }

        private void setFormHeight()
        {
 
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
                MessageBox.Show(ex.Message);
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
            set { myFoc1.Link = value; }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            statusUpdateTimer.Enabled = false;
            
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

        

    }
}
