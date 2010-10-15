using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Optec;
using System.Reflection;
using System.Diagnostics;

namespace Optec_TCF_S_Focuser
{
    public partial class Form1 : Form
    {
        private OptecFocuser myFocuser = OptecFocuser.Instance;
        private List<Control> ConnectedControlsList = new List<Control>();
        private List<Control> DisableForTempCompList = new List<Control>();
        private List<Control> DisabledForSleepList = new List<Control>();
        StatusBar myStatusBar;

        public Form1()
        {
            try
            {
                InitializeComponent();
                myStatusBar = new StatusBar(myFocuser);
                myFocuser.DeviceStatusChanged += new EventHandler(myFocuser_DeviceStatusChanged);
                myFocuser.ErrorOccurred += new EventHandler(myFocuser_ErrorOccurred);
                XMLSettings.LoadXML();
                UpdateFormSize();

                // Fill in the control lists...
                ConnectedControlsList.Add(PosDRO_TB);
                ConnectedControlsList.Add(TempDRO_TB);
                ConnectedControlsList.Add(In_BTN);
                ConnectedControlsList.Add(OUT_Btn);
                ConnectedControlsList.Add(AbsPosTB);
                ConnectedControlsList.Add(AbsoluteMovePanel);
                ConnectedControlsList.Add(StepSize_NUD);
                ConnectedControlsList.Add(StepSize_LBL);
                ConnectedControlsList.Add(this.FocusOffsetPanel);
                ConnectedControlsList.Add(AbsolutePresetPanel);
               // ConnectedControlsList.Add(PowerLight);
                ConnectedControlsList.Add(TempCompPanel);

                DisableForTempCompList.Add(In_BTN);
                DisableForTempCompList.Add(OUT_Btn);
                DisableForTempCompList.Add(StepSize_LBL);
                DisableForTempCompList.Add(StepSize_NUD);
                DisableForTempCompList.Add(AbsoluteMovePanel);
                DisableForTempCompList.Add(FocusOffsetPanel);
                DisableForTempCompList.Add(AbsolutePresetPanel);


                DisabledForSleepList.Add(In_BTN);
                DisabledForSleepList.Add(OUT_Btn);
                DisabledForSleepList.Add(AbsPosTB);
                DisabledForSleepList.Add(StepSize_LBL);
                DisabledForSleepList.Add(StepSize_NUD);
                DisabledForSleepList.Add(AbsoluteMovePanel);
                DisabledForSleepList.Add(FocusOffsetPanel);
                DisabledForSleepList.Add(AbsolutePresetPanel);
                DisabledForSleepList.Add(TempCompPanel);

                positionAndTemperatureToolStripMenuItem.Checked = Properties.Settings.Default.DisplayPosAndTemp;
                positionAndTemperatureToolStripMenuItem.Click += new EventHandler(viewItemToolStripMenuItem_Click);
                AbsoluteMoveToolStripMenuItem.Checked = Properties.Settings.Default.DisplayAbsoluteMove;
                AbsoluteMoveToolStripMenuItem.Click += new EventHandler(viewItemToolStripMenuItem_Click);
                relativeFocusAdjustToolStripMenuItem.Checked = Properties.Settings.Default.DisplayRelativeMoves;
                relativeFocusAdjustToolStripMenuItem.Click += new EventHandler(viewItemToolStripMenuItem_Click);
                temperatureCompensationToolStripMenuItem.Checked = Properties.Settings.Default.DisplayTempComp;
                temperatureCompensationToolStripMenuItem.Click += new EventHandler(viewItemToolStripMenuItem_Click);
                ReletiveFocusOffsetsToolStripMenuItem.Checked = Properties.Settings.Default.DisplayFocusOffsets;
                ReletiveFocusOffsetsToolStripMenuItem.Click += new EventHandler(viewItemToolStripMenuItem_Click);
                absoluteFocusPresetsToolStripMenuItem.Checked = Properties.Settings.Default.DisplayAbsolutePresets;
                absoluteFocusPresetsToolStripMenuItem.Click += new EventHandler(viewItemToolStripMenuItem_Click );
                alwaysOnTopToolStripMenuItem.Checked = Properties.Settings.Default.AlwaysOnTop;
                alwaysOnTopToolStripMenuItem.Click += new EventHandler(viewItemToolStripMenuItem_Click );

                UpdateFormSize();

                int xloc = Properties.Settings.Default.FormLocX;
                int yloc = Properties.Settings.Default.FormLocY;
                if (xloc != -1000)
                {
                    this.Location = new Point(xloc, yloc);
                    this.StartPosition = FormStartPosition.Manual;                
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                DisplayExceptionMessage(ex);
            }
        }

        void viewItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            switch (item.Tag.ToString())
            {
                case "PosAndTemp":
                    Properties.Settings.Default.DisplayPosAndTemp = item.Checked;
                    break;
                case "AbsoluteMove":
                    Properties.Settings.Default.DisplayAbsoluteMove = item.Checked;
                    break;
                case "RelativeFocusAdjust":
                    Properties.Settings.Default.DisplayRelativeMoves = item.Checked;
                    break;
                case "TempComp":
                    Properties.Settings.Default.DisplayTempComp = item.Checked;
                    break;
                case "RelativeFocusOffsets":
                    Properties.Settings.Default.DisplayFocusOffsets = item.Checked;
                    break;
                case "AbsoluteFocusPresets":
                    Properties.Settings.Default.DisplayAbsolutePresets = item.Checked;
                    break;
                case "AlwaysOnTop":
                    Properties.Settings.Default.AlwaysOnTop = item.Checked;
                    this.TopMost = item.Checked;
                    break;
                default: throw new ApplicationException();
                
            }
            Properties.Settings.Default.Save();
            UpdateFormSize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                LoadFocusOffsets();
                LoadAbsolutePresets();
                UpdateDisplay();        
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void In_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                myFocuser.TargetPosition = (int)myFocuser.CurrentPosition - (int)StepSize_NUD.Value;
            }
            catch (Exception ex)
            {
                 DisplayExceptionMessage(ex);
            }
        }

        private void OUT_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                myFocuser.TargetPosition = (int)myFocuser.CurrentPosition + (int)StepSize_NUD.Value;
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private delegate void DelNoParms();

        void myFocuser_DeviceStatusChanged(object sender, EventArgs e)
        {
            
            try
            {
                this.Invoke(new DelNoParms(UpdateDisplay));
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        void myFocuser_ErrorOccurred(object sender, EventArgs e)
        {
            
            try
            {
                Exception ex = sender as Exception;
                MessageBox.Show(ex.Message, "Attention");
               
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void UpdateDisplay()
        {           
            try
            {
                
                switch (myFocuser.ConnectionState)
                {
                    case OptecFocuser.ConnectionStates.SerialMode:
                        this.TempModeOFF_RB.Checked = true;
                        this.TempDRO_TB.ForeColor = Color.Red;
                        this.PosDRO_TB.ForeColor = Color.Red;
                        PowerLight.Image = Properties.Resources.RedLight;
                        if (myFocuser.TempProbeDisabled) this.TempDRO_TB.Text = "DISABLED";
                        else this.TempDRO_TB.Text = myFocuser.CurrentTempForDisplay;
                        if (myFocuser.IsMoving) PosDRO_TB.Text = "MOVING";
                        else this.PosDRO_TB.Text = myFocuser.CurrentPositionForDisplay;
                        Pos_LBL.Text = "Position (" + myFocuser.DisplayPositionUnits.ToString() + ")";
                        foreach (Control x in ConnectedControlsList)
                        {
                            x.Enabled = true;
                        }
                        break;
                    case OptecFocuser.ConnectionStates.TempCompMode:
                        foreach (Control x in DisableForTempCompList)
                        {
                            x.Enabled = false;
                        }
                        this.TempModeON_RB.Checked = true;
                        this.TempDRO_TB.ForeColor = Color.Red;
                        this.PosDRO_TB.ForeColor = Color.Red;
                        this.TempDRO_TB.Text = myFocuser.CurrentTempForDisplay;
                        if (myFocuser.IsMoving) PosDRO_TB.Text = "MOVING";
                        else this.PosDRO_TB.Text = myFocuser.CurrentPositionForDisplay;
                        Pos_LBL.Text = "Position (" + myFocuser.DisplayPositionUnits.ToString() + ")";
                        
                        PowerLight.Image = Properties.Resources.RedLight;
                        break;
                    case OptecFocuser.ConnectionStates.Disconnected:
                        foreach (Control x in ConnectedControlsList)
                        {
                            x.Enabled = false;
                        }
                        this.TempModeOFF_RB.Checked = true;
                        this.TempDRO_TB.ForeColor = Color.Red;
                        this.PosDRO_TB.ForeColor = Color.Red;
                        Pos_LBL.Text = "Position (" + myFocuser.DisplayPositionUnits.ToString() + ")";
                        PosDRO_TB.Text = "------";
                        TempDRO_TB.Text = "------";
                        PowerLight.Image = Properties.Resources.GreyLight;
                        break;
                    case OptecFocuser.ConnectionStates.Sleep:
                        foreach (Control x in DisabledForSleepList)
                        {
                            x.Enabled = false;
                        }
                        this.TempModeOFF_RB.Checked = true;
                        this.TempDRO_TB.ForeColor = Color.DarkRed;
                        this.PosDRO_TB.ForeColor = Color.DarkRed;
                        this.TempDRO_TB.Text = "SLEEP";
                        this.PosDRO_TB.Text = "SLEEP";
                        Pos_LBL.Text = "Position (" + myFocuser.DisplayPositionUnits.ToString() + ")";
                        PowerLight.Image = Properties.Resources.RedLight;                        
                        break;
                        
                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                string msg;
                switch (myFocuser.ConnectionState)
                {
                    case OptecFocuser.ConnectionStates.Disconnected:
                        myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
                        break;
                    case OptecFocuser.ConnectionStates.SerialMode:
                        msg = "Already Connected!";
                        MessageBox.Show(msg, "Attention");
                        break;
                    case OptecFocuser.ConnectionStates.TempCompMode:
                        msg = "The device is currently in temperature compensation mode." +
                            " Please exit temperature compensation mode before performing this operation.";
                        MessageBox.Show(msg, "Attention");
                        break;
                    case OptecFocuser.ConnectionStates.Sleep:
                        msg = "The device is currently in sleep mode." +
                            " Please exit sleep mode before performing this operation.";
                        MessageBox.Show(msg, "Attention");
                        break;
                }
            }
            catch (UnauthorizedAccessException)
            {
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
                MessageBox.Show("The COM port is currently in use. Is another program using it?", "Attention");
            }
            catch (ApplicationException )
            {
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
                MessageBox.Show("Unable to connect to the device. Verify that the serial cable is connected properly.", "Attention");
                //DisplayExceptionMessage(ex);
            }
            catch (InvalidOperationException ex)
            {
                DisplayExceptionMessage(ex);
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

        

        
        private void UpdateFormSize()
        {
            int relPanelHeight = Properties.Settings.Default.RelativePanelHeight;
            int absPanelHeight = Properties.Settings.Default.AbsolutePanelHeight;

            tableLayoutPanel1.RowStyles[1].Height = Properties.Settings.Default.DisplayPosAndTemp ? 130 : 0;
            tableLayoutPanel1.RowStyles[2].Height = Properties.Settings.Default.DisplayRelativeMoves ? 60 : 0;
            tableLayoutPanel1.RowStyles[3].Height = Properties.Settings.Default.DisplayTempComp ? 50 : 0;
            tableLayoutPanel1.RowStyles[4].Height = Properties.Settings.Default.DisplayAbsoluteMove ? 16 : 0;
            tableLayoutPanel1.RowStyles[5].Height = Properties.Settings.Default.DisplayAbsoluteMove ? 50 : 0;
            tableLayoutPanel1.RowStyles[6].Height = Properties.Settings.Default.DisplayFocusOffsets ? 16 : 0;
            tableLayoutPanel1.RowStyles[7].Height = Properties.Settings.Default.DisplayFocusOffsets ? relPanelHeight : 0;
            tableLayoutPanel1.RowStyles[8].Height = Properties.Settings.Default.DisplayAbsolutePresets ? 16 : 0;
            tableLayoutPanel1.RowStyles[9].Height = Properties.Settings.Default.DisplayAbsolutePresets ? absPanelHeight : 0;

            if (Properties.Settings.Default.DisplayFocusOffsets) FocusOffsetPanel.BorderStyle = BorderStyle.FixedSingle;
            else FocusOffsetPanel.BorderStyle = BorderStyle.None;

            float x =
                tableLayoutPanel1.RowStyles[0].Height
                + tableLayoutPanel1.RowStyles[1].Height
                + tableLayoutPanel1.RowStyles[2].Height
                + tableLayoutPanel1.RowStyles[3].Height
                + tableLayoutPanel1.RowStyles[4].Height
                + tableLayoutPanel1.RowStyles[5].Height
                + tableLayoutPanel1.RowStyles[6].Height
                + tableLayoutPanel1.RowStyles[7].Height
                + tableLayoutPanel1.RowStyles[8].Height
                + tableLayoutPanel1.RowStyles[9].Height
                + 88;
            this.MinimumSize = new Size(this.Width, (int)x);
            this.Size = new Size(this.Width, (int)x);
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;

        }

        private void LoadFocusOffsets()
        {

            
            try
            {
                int count = 0;
                int spacing = 20;
                int topMargin = 0;
                int leftMargin = 10;

                // First clear the panel
                FocusOffsetPanel.Controls.Clear();
                Application.DoEvents();

                foreach (FocusOffset f in myFocuser.FocusOffsets)
                {
                    RadioButton r = new RadioButton();
                    r.Text = f.OffsetName + " : " + f.OffsetSteps.ToString();
                    r.Location = new Point(leftMargin, topMargin + (spacing * count));
                    r.Tag = f.OffsetSteps;
                    //if (count == 0) r.Checked = true;
                    r.CheckedChanged += new EventHandler(FocusOffset_CheckedChanged);
                    FocusOffsetPanel.Controls.Add(r);
                    count++;

                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
            
        }

        private void LoadAbsolutePresets()
        {
            try
            {
                int count = 0;
                int spacing = 20;
                int topMargin = 0;
                int leftMargin = 10;

                // First clear the panel
                AbsolutePresetPanel.Controls.Clear();
                Application.DoEvents();

                foreach (FocusOffset f in myFocuser.AbsolutePresets)
                {
                    RadioButton r = new RadioButton();
                    r.Text = f.OffsetName + " : " + f.OffsetSteps.ToString();
                    r.Location = new Point(leftMargin, topMargin + (spacing * count));
                    r.Tag = f.OffsetSteps;
                    //if (count == 0) r.Checked = true;
                    r.CheckedChanged += new EventHandler(AbsolutePreset_CheckedChanged);
                    AbsolutePresetPanel.Controls.Add(r);
                    count++;

                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private int lastFocusOffset = 0;

        void FocusOffset_CheckedChanged(object sender, EventArgs e)
        {
            
            try
            {
                RadioButton r = sender as RadioButton;
                if (!r.Checked) lastFocusOffset = (int)r.Tag;
                else
                {
                    int newPos = (int)r.Tag;
                    int newOffset = -lastFocusOffset + newPos;
                    newPos = (int)myFocuser.CurrentPosition + newOffset;
                    if ((newPos < 0) || (newPos > myFocuser.MaxSteps))
                        MessageBox.Show("Target is outside the range of the focuser");
                    myFocuser.TargetPosition = newPos;
                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        void AbsolutePreset_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton r = sender as RadioButton;
                if (!r.Checked) return;
                else
                {
                    // Move to the preset position
                    myFocuser.TargetPosition = (int)r.Tag;
                    DateTime start = DateTime.Now;
                    while (DateTime.Now.Subtract(start).TotalSeconds < 1.5)
                    {
                        Application.DoEvents();
                    }
                    r.Checked = false;
                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int newpos = int.Parse(AbsPosTB.Text);
                myFocuser.TargetPosition = newpos;
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void addRelativeOffsetContextMenu_Click(object sender, EventArgs e)
        {
            
            try
            {
                AddOffsetForm frm = new AddOffsetForm(AddOffsetForm.FormFunctions.RelativeOffsets, myFocuser);
                DialogResult dr = frm.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    LoadFocusOffsets();
                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void removeOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
                 
            try
            {
                RemoveOffsetForm frm = new RemoveOffsetForm(RemoveOffsetForm.RemoveFormStates.Relative);
                DialogResult dr = frm.ShowDialog();
                LoadFocusOffsets();
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void DisplayExceptionMessage(Exception ex)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new dmbDelegate(dmb), new object[]{ex.Message});
            }
            else dmb(ex.Message);
        }

        private delegate void dmbDelegate(string s);
        private void dmb(string x)
        {
            MessageBox.Show(x, "Attention");
            UpdateDisplay();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
            }
            catch(Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void TempDRO_TB_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                switch(myFocuser.DisplayTempUnits)
                {
                    case OptecFocuser.TemperatureUnits.Celsius:
                        myFocuser.DisplayTempUnits = OptecFocuser.TemperatureUnits.Fahrenheit;
                        break;
                    case OptecFocuser.TemperatureUnits.Fahrenheit:
                        myFocuser.DisplayTempUnits = OptecFocuser.TemperatureUnits.Kelvin;
                        break;
                    case OptecFocuser.TemperatureUnits.Kelvin:
                        myFocuser.DisplayTempUnits = OptecFocuser.TemperatureUnits.Celsius;
                        break;
                }
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void enterExitSleepModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                    if(myFocuser.ConnectionState == OptecFocuser.ConnectionStates.Sleep)
                    {
                        myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
                    }
                    else myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Sleep;
              
            }
            catch(Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
                
        }

        private void enterExitTempCompModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (myFocuser.ConnectionState == OptecFocuser.ConnectionStates.TempCompMode)
                {
                    myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
                   // TempModeOFF_RB.Checked = true;
                }
                else
                {
                    myFocuser.ConnectionState = OptecFocuser.ConnectionStates.TempCompMode;
                   // TempModeON_RB.Checked = true;
                }

            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }

        }

        private void Center_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                myFocuser.TargetPosition = myFocuser.MaxSteps / 2;

            }
            catch(Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void deviceSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm frm = new SettingsForm(myFocuser);
            frm.ShowDialog();

        }

        private void addFocusOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddOffsetForm frm = new AddOffsetForm(AddOffsetForm.FormFunctions.RelativeOffsets, myFocuser);
                DialogResult dr = frm.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    LoadFocusOffsets();
                }
                if (!Properties.Settings.Default.DisplayFocusOffsets)
                {
                    Properties.Settings.Default.DisplayFocusOffsets = true;
                    Properties.Settings.Default.Save();
                    UpdateFormSize();
                }

            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void removeFocusOffsetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveOffsetForm frm = new RemoveOffsetForm(RemoveOffsetForm.RemoveFormStates.Relative);
                DialogResult dr = frm.ShowDialog();
                LoadFocusOffsets();
                if (!Properties.Settings.Default.DisplayFocusOffsets)
                {
                    Properties.Settings.Default.DisplayFocusOffsets = true;
                    Properties.Settings.Default.Save();
                    UpdateFormSize();
                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                NewVersionBGWorker.RunWorkerAsync();
                this.LocationChanged += new EventHandler(Form1_LocationChanged);
            }
            catch
            {
            }

            try
            {
                DisplayStatusMessage( "Attempting to Connect...", new TimeSpan(0,0,30));
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
                DisplayStatusMessage("Connected Successfully!", new TimeSpan(0, 0, 4));
            }
            catch
            {
                DisplayStatusMessage("No TCF-S Found", new TimeSpan(0, 0, 4));
            }
        }

        void Form1_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FormLocX = Location.X;
            Properties.Settings.Default.FormLocY = Location.Y;
            // Only save these when the form is closing...
        }

        #region Status Bar Methods

        private DateTime NextStatusLabelChangeTime = new DateTime();

        private void DisplayStatusMessage(string msg, TimeSpan time)
        {
            TimeSpan ts = time.Add(new TimeSpan(0,0,0,0,100));
            NextStatusLabelChangeTime = DateTime.Now.Add(time);
            StatusLabel.Text = msg;
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > NextStatusLabelChangeTime)
            {
                this.StatusLabel.Text = myStatusBar.GetNextMessage();
            }
        }

        private void StatusLabel_Click(object sender, EventArgs e)
        {
            this.StatusLabel.Text = myStatusBar.GetNextMessage();
        }

        private void StatusLabel_MouseEnter(object sender, EventArgs e)
        {
            StatusTimer.Enabled = false;
        }

        private void StatusLabel_MouseLeave(object sender, EventArgs e)
        {
            StatusTimer.Enabled = true;
        }

        private void StatusLabel_MouseHover(object sender, EventArgs e)
        {
            StatusBarToolTip.AutoPopDelay = 2500;
            StatusBarToolTip.ReshowDelay = 1000;
            StatusBarToolTip.SetToolTip(this.statusStrip1, "Click to see next message");

        }

        private void DeviceType_TB_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
            //SendKeys.Send(Keys.Tab.ToString());
        }

        #endregion Status Bar Methods

        private void ToggleTempCompRB_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                RadioButton rb = sender as RadioButton;
                if (!rb.Enabled) return;
                if (!rb.Checked) return;
                else
                {
                    if (rb.Tag.ToString() == "True")
                        myFocuser.ConnectionState = OptecFocuser.ConnectionStates.TempCompMode;

                    else myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
                }

            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem parent = sender as ToolStripMenuItem;
            changeCOMPortToolStripMenuItem.DropDownItems.Clear();
            foreach (string x in System.IO.Ports.SerialPort.GetPortNames())
            {
                changeCOMPortToolStripMenuItem.DropDownItems.Add(x, null, new EventHandler(ComPortName_Click));
            }
        }

        void ComPortName_Click(object sender, EventArgs e)
        {
            if (myFocuser.ConnectionState != OptecFocuser.ConnectionStates.Disconnected)
            {
                MessageBox.Show("A focuser is currently connected. You must disconnect before changing the COM port.");
                return;
            }
            ToolStripMenuItem x = sender as ToolStripMenuItem;
            myFocuser.SavedSerialPortName = x.Text;
        }

        private void PosDRO_TB_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (myFocuser.DisplayPositionUnits == OptecFocuser.PositionUnits.Microns)
                    myFocuser.DisplayPositionUnits = OptecFocuser.PositionUnits.Steps;
                else myFocuser.DisplayPositionUnits = OptecFocuser.PositionUnits.Microns;
                UpdateDisplay();
            }
            catch
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Save();
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
            }
            catch
            {
            }
        }

        private void NewVersionBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Check For A newer verison of the driver
                EventLogger.LogMessage("Checking for application updates", TraceLevel.Info);
                if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.TCF_S))
                {
                    //Found a VersionNumber, now check if it's newer
                    Assembly asm = Assembly.GetExecutingAssembly();
                    AssemblyName asmName = asm.GetName();
                    NewVersionChecker.CompareToLatestVersion(asmName.Version);
                    if (NewVersionChecker.NewerVersionAvailable)
                    {
                        EventLogger.LogMessage("This application is NOT the most recent version available", TraceLevel.Warning);
                        NewVersionFrm nvf = new NewVersionFrm(asmName.Version.ToString(),
                            NewVersionChecker.NewerVersionNumber, NewVersionChecker.NewerVersionURL);
                        nvf.ShowDialog();
                    }
                    else
                    {
                        EventLogger.LogMessage("This application is the most recent version available", TraceLevel.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
            } // Just ignore all errors. They mean the computer isn't connected to internet.
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.NewVersionBGWorker.IsBusy)
                {
                    string msg = "The Version Checker is currently busy. Please try again in a moment.";
                    MessageBox.Show(msg);
                }
                else
                {
                    if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.TCF_S))
                    {
                        //Found a VersionNumber, now check if it's newer
                        Assembly asm = Assembly.GetExecutingAssembly();
                        AssemblyName asmName = asm.GetName();
                        NewVersionChecker.CompareToLatestVersion(asmName.Version);
                        if (NewVersionChecker.NewerVersionAvailable)
                        {
                            NewVersionFrm nvf = new NewVersionFrm(asmName.Version.ToString(),
                                NewVersionChecker.NewerVersionNumber, NewVersionChecker.NewerVersionURL);
                            nvf.ShowDialog();
                        }
                        else MessageBox.Show("Congratulations! You have the most recent version of this program!\n" +
                            "This version number is " + asmName.Version.ToString(), "No Update Needed! - V" +
                            asmName.Version.ToString());
                    }
                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to connect to the server www.optecinc.com",
                    "Version Information Unavailable");
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abt = new AboutBox1(myFocuser);
            abt.ShowDialog();
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string asmpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int i = asmpath.IndexOf("TCF-S");
            asmpath = asmpath.Substring(0, i + 5);
            asmpath += "\\Documentation\\TCF-S_Help.chm";
            //MessageBox.Show(asmpath);
            Process p = new Process();
            p.StartInfo.FileName = asmpath;
            p.Start();
        }

        private void exitOptecTCFSFocuserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PowerLight_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (myFocuser.ConnectionState != OptecFocuser.ConnectionStates.Disconnected)
                {
                    myFocuser.ConnectionState = OptecFocuser.ConnectionStates.Disconnected;
                }
                else
                    myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
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

        private void addAbsolutePresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddOffsetForm frm = new AddOffsetForm(AddOffsetForm.FormFunctions.AbsolutePresets, myFocuser);
                DialogResult dr = frm.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    LoadAbsolutePresets();
                }
                if (!Properties.Settings.Default.DisplayAbsolutePresets)
                {
                    Properties.Settings.Default.DisplayAbsolutePresets = true;
                    Properties.Settings.Default.Save();
                    UpdateFormSize();
                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void removeAbsolutePresetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveOffsetForm frm = new RemoveOffsetForm(RemoveOffsetForm.RemoveFormStates.Absolute);
                DialogResult dr = frm.ShowDialog();
                LoadAbsolutePresets();
                if (!Properties.Settings.Default.DisplayFocusOffsets)
                {
                    Properties.Settings.Default.DisplayFocusOffsets = true;
                    Properties.Settings.Default.Save();
                    UpdateFormSize();
                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void PowerLight_MouseEnter(object sender, EventArgs e)
        {
            Control x = sender as Control;
            StatusBarToolTip.AutoPopDelay = 1500;
            StatusBarToolTip.ReshowDelay = 1000;
            StatusBarToolTip.SetToolTip(x, "Click to Connect/Disconnect");
        }

        private void EnableDisableProbeCTMS(object sender, EventArgs e)
        {
            string msg = "";
            if (myFocuser.ConnectionState == OptecFocuser.ConnectionStates.Disconnected)
                msg = "You must connect before enabling or disabling the probe.";
            else if (myFocuser.ConnectionState == OptecFocuser.ConnectionStates.TempCompMode)
                msg = "You must exit temperature compensation mode before enabling or disabling the probe.";
            else if (myFocuser.ConnectionState == OptecFocuser.ConnectionStates.Sleep)
                msg = "You must wake the device from sleep mode before enabling or disabling the probe.";
            if(msg.Length > 1)
            {
                MessageBox.Show(msg);
                return;
            }
            else
            {
                ToolStripMenuItem x = sender as ToolStripMenuItem;
                if (x.Tag.ToString() == "ENABLE") myFocuser.TempProbeDisabled = false;
                else if (x.Tag.ToString() == "DISABLE") myFocuser.TempProbeDisabled = true;
                UpdateDisplay();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("www.optecinc.com");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void addAbsolutePresetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                AddOffsetForm frm = new AddOffsetForm(AddOffsetForm.FormFunctions.AbsolutePresets, myFocuser);
                DialogResult dr = frm.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    LoadAbsolutePresets();
                }
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void removeAbsolutePresetsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveOffsetForm frm = new RemoveOffsetForm(RemoveOffsetForm.RemoveFormStates.Absolute);
                DialogResult dr = frm.ShowDialog();
                LoadAbsolutePresets();
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

       

        private void PanelHeightTextBox1_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox x = sender as ToolStripTextBox;
            int size = 0;
            try { size = int.Parse(x.Text); }
            catch
            {
                MessageBox.Show("Numbers only!");
                return;
            }
        }

        private void panelHeightToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            PanelHeightTextBox1.Text = Properties.Settings.Default.RelativePanelHeight.ToString();
        }

        private void PanelHeightTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                // Value entered
                e.Handled = true;
                // Check that it is a number
                int newval = 0;
                try
                {
                    newval = int.Parse(PanelHeightTextBox1.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid value. Must be a number.");
                    return;
                }

                // Check if it's in the valid range
                if (newval < 30)
                    MessageBox.Show("Height must be greater than 30");
                else if (newval > 500)
                    MessageBox.Show("Height must be less than 500");
                else
                {
                    // Value is good! Store it...
                    Properties.Settings.Default.RelativePanelHeight = newval;
                    Properties.Settings.Default.Save();
                    UpdateFormSize();
                }
                
            }
        }

        private void absolutePanelHeightTB_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox x = sender as ToolStripTextBox;
            int size = 0;
            try { size = int.Parse(x.Text); }
            catch
            {
                MessageBox.Show("Numbers only!");
                return;
            }
        }

        private void absolutePanelHeightTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                // Value entered
                e.Handled = true;
                // Check that it is a number
                int newval = 0;
                try
                {
                    newval = int.Parse( absolutePanelHeightTB.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid value. Must be a number.");
                    return;
                }

                // Check if it's in the valid range
                if (newval < 30)
                    MessageBox.Show("Height must be greater than 30");
                else if (newval > 500)
                    MessageBox.Show("Height must be less than 500");
                else
                {
                    // Value is good! Store it...
                    Properties.Settings.Default.AbsolutePanelHeight = newval;
                    Properties.Settings.Default.Save();
                    UpdateFormSize();
                }
            }
        }

        private void panelHeightToolStripMenuItem1_MouseEnter(object sender, EventArgs e)
        {
            absolutePanelHeightTB.Text = Properties.Settings.Default.AbsolutePanelHeight.ToString();
        }

   
    }

    class StatusBar
    {
        private OptecFocuser myFocuser;

        public StatusBar(OptecFocuser myFoc)
        {
            myFocuser = myFoc;
        }

        private int CurrentMessage = 0;

        public string GetNextMessage()
        {
            string MsgToPost = "";
            switch (CurrentMessage)
            {
                case 0:
                    MsgToPost = "Connection State:   " + GetLinkState();
                    break;
                case 1:
                    MsgToPost = "COM Port:   " + GetComPort();
                    break;
                case 2:
                    MsgToPost = "Probe Enabled:   " + GetProbeEnabled();
                    break;
                case 3:
                    MsgToPost = "Device Type:   " + GetDeviceType();
                    break;
            }
            CurrentMessage = (CurrentMessage == 3) ? 0 : ++CurrentMessage;
            return MsgToPost;
        }

        private string GetLinkState()
        {
            try
            {
                switch (myFocuser.ConnectionState)
                {
                    case OptecFocuser.ConnectionStates.Disconnected:
                        return "Disconnected";
                    case OptecFocuser.ConnectionStates.SerialMode:
                        return "Connected";
                    case OptecFocuser.ConnectionStates.Sleep:
                        return "Sleep Mode";
                    case OptecFocuser.ConnectionStates.TempCompMode:
                        return "Temp Comp Mode";
                    default:
                        return "Unknown Mode";
                }
            }
            catch
            {
                return "Not Available";
            }
        }

        private string GetComPort()
        {
            try
            {
                return myFocuser.SavedSerialPortName;
            }
            catch
            {
                return "Not Available";
            }
        }


      

        private string GetProbeEnabled()
        {
            try
            {
                return (!myFocuser.TempProbeDisabled).ToString();
            }
            catch
            {
                return "Not Available";
            }
        }

        private string GetDeviceType()
        {
            try
            {
                return myFocuser.DeviceType.ToString().Replace('_', '-');
            }
            catch
            {
                return "Not Available";
            }
        }

    }
}
