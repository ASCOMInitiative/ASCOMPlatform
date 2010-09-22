using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
                ConnectedControlsList.Add(AbsoluteGB);
                ConnectedControlsList.Add(StepSize_NUD);
                ConnectedControlsList.Add(StepSize_LBL);
                ConnectedControlsList.Add(FocusOffsetsGB);
                ConnectedControlsList.Add(PowerLight);
                ConnectedControlsList.Add(TempCompPanel);

                DisableForTempCompList.Add(In_BTN);
                DisableForTempCompList.Add(OUT_Btn);
                DisableForTempCompList.Add(StepSize_LBL);
                DisableForTempCompList.Add(StepSize_NUD);
                DisableForTempCompList.Add(AbsoluteGB);
                DisableForTempCompList.Add(FocusOffsetsGB);


                DisabledForSleepList.Add(In_BTN);
                DisabledForSleepList.Add(OUT_Btn);
                DisabledForSleepList.Add(AbsPosTB);
                DisabledForSleepList.Add(StepSize_LBL);
                DisabledForSleepList.Add(StepSize_NUD);
                DisabledForSleepList.Add(AbsoluteGB);
                DisabledForSleepList.Add(FocusOffsetsGB);
                DisabledForSleepList.Add(TempCompPanel);





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                DisplayExceptionMessage(ex);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                LoadFocusOffsets();
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
                        this.TempDRO_TB.ForeColor = Color.Red;
                        this.PosDRO_TB.ForeColor = Color.Red;
                        PowerLight.Image = Properties.Resources.RedLight;
                        if (myFocuser.TempProbeDisabled) this.TempDRO_TB.Text = "DISABLE";
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
                        this.TempDRO_TB.ForeColor = Color.Red;
                        this.PosDRO_TB.ForeColor = Color.Red;
                        this.TempDRO_TB.Text = myFocuser.CurrentTempForDisplay;
                        if (myFocuser.IsMoving) PosDRO_TB.Text = "MOVING";
                        else this.PosDRO_TB.Text = myFocuser.CurrentPositionForDisplay;
                        Pos_LBL.Text = "Position (" + myFocuser.DisplayPositionUnits.ToString() + ")";
                        foreach (Control x in DisableForTempCompList)
                        {
                            x.Enabled = false; 
                        }
                        PowerLight.Image = Properties.Resources.RedLight;
                        break;
                    case OptecFocuser.ConnectionStates.Disconnected:
                        this.TempDRO_TB.ForeColor = Color.Red;
                        this.PosDRO_TB.ForeColor = Color.Red;
                        Pos_LBL.Text = "Position (" + myFocuser.DisplayPositionUnits.ToString() + ")";
                        foreach (Control x in ConnectedControlsList)
                        {
                            x.Enabled = false;
                        }
                        PosDRO_TB.Text = "------";
                        TempDRO_TB.Text = "------";
                        PowerLight.Image = Properties.Resources.GreyLight;
                        break;
                    case OptecFocuser.ConnectionStates.Sleep:
                        this.TempDRO_TB.ForeColor = Color.DarkRed;
                        this.PosDRO_TB.ForeColor = Color.DarkRed;
                        this.TempDRO_TB.Text = "SLEEP";
                        this.PosDRO_TB.Text = "SLEEP";
                        Pos_LBL.Text = "Position (" + myFocuser.DisplayPositionUnits.ToString() + ")";
                        PowerLight.Image = Properties.Resources.RedLight;
                        foreach (Control x in DisabledForSleepList)
                        {
                            x.Enabled = false;
                        }
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

        

        private int MinHeight = 350;
        private int UpperPos = 303;
        private int LowerPos = 374;
        private void UpdateFormSize()
        {
            int height = MinHeight;
            if (Properties.Settings.Default.DisplayAbsoluteMove)
            {
                AbsoluteGB.Visible = true;

            }
            else
            {
                AbsoluteGB.Visible = false;
            }

            if (Properties.Settings.Default.DisplayFocusOffsets)
            {
                FocusOffsetsGB.Visible = true;
                FocusOffsetsGB.Location = new Point(FocusOffsetsGB.Location.X, UpperPos);
 
                if (Properties.Settings.Default.DisplayAbsoluteMove)
                {
                    FocusOffsetsGB.Location = new Point(FocusOffsetsGB.Location.X, LowerPos);
                }
                else
                {

                }
                
            }
            else
            {
                FocusOffsetsGB.Visible = false;
            }
        }

        private void LoadFocusOffsets()
        {

            
            try
            {
                int count = 0;
                int spacing = 20;
                int topMargin = 2;
                int leftMargin = 10;

                // First clear the panel
                FocusOffsetPanel.Controls.Clear();

                foreach (FocusOffset f in myFocuser.FocusOffsets)
                {
                    RadioButton r = new RadioButton();
                    r.Text = f.OffsetName + " : " + f.OffsetSteps.ToString();
                    r.Location = new Point(leftMargin, topMargin + (spacing * count));
                    r.Tag = f.OffsetSteps;
                    if (count == 0) r.Checked = true;
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
                    myFocuser.TargetPosition = (int)myFocuser.CurrentPosition + newOffset;
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            try
            {
                    AddFilterOffsetForm frm = new AddFilterOffsetForm();
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
                RemoveFocusOffsetFormcs frm = new RemoveFocusOffsetFormcs();
                DialogResult dr = frm.ShowDialog();
                LoadFocusOffsets();
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private void showHideAbsoluteMoveControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            try
            {
                Properties.Settings.Default.DisplayAbsoluteMove = !Properties.Settings.Default.DisplayAbsoluteMove;
                Properties.Settings.Default.Save();
                UpdateFormSize();
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
            
        }

        private void showHideFocusOffsetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            try
            {
                Properties.Settings.Default.DisplayFocusOffsets = !Properties.Settings.Default.DisplayFocusOffsets;
                Properties.Settings.Default.Save();
                UpdateFormSize();
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
                }
                else myFocuser.ConnectionState = OptecFocuser.ConnectionStates.TempCompMode;

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
                AddFilterOffsetForm frm = new AddFilterOffsetForm();
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

        private void removeFocusOffsetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveFocusOffsetFormcs frm = new RemoveFocusOffsetFormcs();
                DialogResult dr = frm.ShowDialog();
                LoadFocusOffsets();
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
                DisplayStatusMessage( "Attempting to Connect...", new TimeSpan(0,0,30));
                myFocuser.ConnectionState = OptecFocuser.ConnectionStates.SerialMode;
                DisplayStatusMessage("Connected Successfully!", new TimeSpan(0, 0, 4));
            }
            catch
            {
                DisplayStatusMessage("No TCF-S Found", new TimeSpan(0, 0, 4));
            }
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

        private void PosDRO_TB_MouseEnter(object sender, EventArgs e)
        {
            Label x = sender as Label;
            StatusBarToolTip.AutoPopDelay = 1500;
            StatusBarToolTip.ReshowDelay = 1000;
            StatusBarToolTip.SetToolTip(x, "Click to change units");
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
