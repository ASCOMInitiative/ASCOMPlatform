using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OptecHID_FilterWheelAPI;
using System.Reflection;

namespace HSFWControlApp
{
    public partial class Form1 : Form
    {
        private delegate void UI_Update();
        private FilterWheels FilterWheelManager = new FilterWheels();

        public Form1()
        {
            InitializeComponent();
            ConstructTabs();
            FilterWheelManager.FilterWheelAttached += new EventHandler(FilterWheels_FilterWheelListChanged);
            FilterWheelManager.FilterWheelRemoved += new EventHandler(FilterWheels_FilterWheelListChanged);
        }

        private void FilterWheels_FilterWheelListChanged(object sender, EventArgs e)
        {
            this.Invoke(new UI_Update(ConstructTabs));
        }

        private void ConstructTabs()
        {
            
            tabControl1.TabPages.Clear();

            if (FilterWheelManager.AttachedDeviceCount == 0)
            {
                LoadNoWheelsTab(tabControl1);
                return;
            }

            foreach (FilterWheel fw in FilterWheelManager.FilterWheelList)
            {
                if (!fw.IsHomed && fw.IsAttached)
                {
                    try
                    {
                        fw.ClearErrorState();
                        fw.HomeDevice();
                    }
                    catch { }
                }
            }

            int deviceNumber = 1;
            foreach (FilterWheel fw in FilterWheelManager.FilterWheelList)
            {
                if (!fw.IsAttached) continue;
                // Add a tab page for the device
                TabPage tp = new TabPage("Device " + deviceNumber.ToString());
                tp.GotFocus += new EventHandler(tab_GotFocus);
                tabControl1.TabPages.Add(tp);

                // Check if the device is homed
                if (fw.IsHomed)
                {
                    // Add the buttons for each Filter on wheel
                    int buttonWidth = 80;
                    int PositionButtonsYCoord = 55;
                    if (fw.ErrorState != 0) fw.ClearErrorState();
                    if (!fw.IsHomed) fw.HomeDevice();
                    // Set the button spacing
                    int spacingBetweenButtons = 0;
                    spacingBetweenButtons = ((tabControl1.TabPages[deviceNumber - 1].Width - 14) -
                        (buttonWidth * fw.NumberOfFilters)) / (fw.NumberOfFilters - 1);
                    // Get the Filter Names
                    string[] FNames = fw.GetFilterNames(fw.WheelID);

                    // Add the buttons to the tab
                    for (int i = 0; i < FNames.Length; i++)
                    {
                        Button b = new Button();
                        b.Text = FNames[i];
                        b.Text = b.Text.Trim();
                        b.Width = buttonWidth;
                        b.Name = (deviceNumber - 1).ToString() + ":" + (i + 1).ToString();
                        b.Location = new Point(7 + (buttonWidth * i) + (spacingBetweenButtons * i), PositionButtonsYCoord);
                        tabControl1.TabPages[deviceNumber - 1].Controls.Add(b);
                        // Add a handler for the button click event
                        b.Click += new EventHandler(FilterPositionBtn_Click);
                        b.GotFocus += new EventHandler(FilterPositionBtn_GotFocus);
                        b.LostFocus += new EventHandler(FilterPositionBtn_LostFocus);
                        //Set the appearance of the current position button
                        if (fw.CurrentPosition == i + 1)
                        {
                            b.Focus();
                            b.BackColor = Color.DarkBlue;
                            b.ForeColor = Color.GhostWhite;
                            b.Font = new Font(b.Font, FontStyle.Bold);
                        }
                        else b.TabStop = false;
                    }


                    // Add the Home Button to the tab
                    Point HomeButtonPosition = new Point(225, 10);
                    Button h = new Button();
                    h.Text = "Home Wheel";
                    h.Width = 100;
                    h.Name = (deviceNumber - 1).ToString();
                    h.Location = HomeButtonPosition;
                    h.Click += new EventHandler(HomeBtn_Click);
                    tabControl1.TabPages[deviceNumber - 1].Controls.Add(h);
                    //h.Height = 50;

                    // Add the device settings button
                    Point DeviceSettingsPosition = new Point(350, 10);
                    Button DSettings = new Button();
                    DSettings.Text = "Device Settings...";
                    DSettings.Name = (deviceNumber - 1).ToString();
                    DSettings.Location = DeviceSettingsPosition;
                    DSettings.Click += new EventHandler(DSettings_Click);
                    DSettings.Width = 100;
                    //DSettings.Height = 50;
                    tabControl1.TabPages[deviceNumber - 1].Controls.Add(DSettings);

                    // Add the tablelayout panel
                    Point TLPPosition = new Point(10, 10);
                    TableLayoutPanel TLP = new TableLayoutPanel();
                    TLP.RowCount = 2;
                    TLP.ColumnCount = 2;
                    TLP.Location = TLPPosition;
                    TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
                    TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
                    //TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
                    //TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
                    TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
                    TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
                    TLP.Size = new System.Drawing.Size(300, 60);
                    tabControl1.TabPages[deviceNumber - 1].Controls.Add(TLP);

                    // Add the Wheel ID and Wheel Name Labels
                    Label WheelID_LB1 = new Label();
                    Label WheelID_LB2 = new Label();
                    Label WheelName_LB1 = new Label();
                    Label WheelName_LB2 = new Label();
                    WheelID_LB1.TextAlign = ContentAlignment.TopRight;
                    WheelID_LB1.Text = "Wheel ID:";
                    WheelID_LB1.Font = new Font(WheelID_LB1.Font, FontStyle.Bold);
                    WheelID_LB2.Text = fw.WheelID.ToString();
                    
                    string[] WNames = fw.GetWheelNames();
                    int wIndex = fw.WheelID - 'A';
                    WheelName_LB1.Text = "Wheel Name:";
                    WheelName_LB1.TextAlign = ContentAlignment.TopRight;
                    WheelName_LB1.Font = new Font(WheelName_LB1.Font, FontStyle.Bold);
                    WheelName_LB2.Text = WNames[wIndex];
                    WheelName_LB2.Width = WheelName_LB2.Text.Length * 10;
                    // Add the controls to the table
                    TLP.Controls.Add(WheelName_LB1, 0, 0);
                    TLP.Controls.Add(WheelName_LB2, 1, 0);
                    TLP.Controls.Add(WheelID_LB1, 0, 1);
                    TLP.Controls.Add(WheelID_LB2, 1, 1);

                    // Add the Optec Logo
                    PictureBox pb = new PictureBox();
                    pb.Image = Properties.Resources.Optec_Logo_medium_png;
                    pb.Location = new Point(500, -10);
                    pb.Width = 180;
                    pb.Height = 70;
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    tabControl1.TabPages[deviceNumber - 1].Controls.Add(pb);

                    #region Removed Functionality

                    //// Add the firmware version label
                    //Label FirmVer1 = new Label();
                    //Label FirmVer2 = new Label();
                    //FirmVer1.Text = "Firmware Version:";
                    //FirmVer1.Font = new Font(FirmVer1.Font, FontStyle.Bold);
                    //FirmVer1.Width = FirmVer1.Text.Length * 10;
                    //FirmVer2.Text =  fw.FirmwareVersion;

                    //FirmVer2.Width = FirmVer2.Text.Length * 10;
                    //TLP.Controls.Add(FirmVer1, 2, 0);
                    //TLP.Controls.Add(FirmVer2, 3, 0);

                    //// Add the serial number label
                    //Label SN_LB1 = new Label();
                    //Label SN_LB2 = new Label();
                    //SN_LB1.Text = "Serial Number:";
                    //SN_LB1.Font = new Font(SN_LB1.Font, FontStyle.Bold);
                    //SN_LB2.Text = fw.SerialNumber;
                    //SN_LB2.Width = SN_LB2.Text.Length * 10;
                    //TLP.Controls.Add(SN_LB1, 2, 1);
                    //TLP.Controls.Add(SN_LB2, 3, 1);
                    #endregion

                }
                else
                {
                    ConstructErrorTab(tabControl1.TabPages[deviceNumber - 1], fw);
                }

                //Increment for the next Device
                deviceNumber++;
            }
        }

        private void tab_GotFocus(object sender, EventArgs e)
        {
            TabPage tp = sender as TabPage;
            string ind = tp.Text.TrimStart(new char[] { 'D', 'e', 'v', 'i', 'c', 'e' });
            int index = int.Parse(ind);
            FilterWheel fw = (FilterWheel)FilterWheelManager.FilterWheelList[index];
            Properties.Settings.Default.LastSelectedSerialNumber = fw.SerialNumber;
            Properties.Settings.Default.Save();
        }

        private void LoadNoWheelsTab(TabControl tabControl1)
        {
            TabPage tp = new TabPage();
            tp.Text = "No Wheels Connected";
            Label txt = new Label();
            txt.Text = "There are no Filter Wheels currently connected to the system.";
            txt.Location = new Point(25, 25);
            txt.Width = 500;
            tp.Controls.Add(txt);
            tabControl1.TabPages.Add(tp);
        }

        private void ConstructErrorTab(TabPage tp, FilterWheel ErroredWheel)
        {
            TextBox ErrorTB = new TextBox();
            ErrorTB.BorderStyle = BorderStyle.None;
            ErrorTB.ReadOnly = true;
            ErrorTB.Text = "A problem occured while attempting to operate this wheel." + Environment.NewLine +
                "The device was unable to complete the Home procedure successfully." + Environment.NewLine +  
                "The device returned the following error condition:" + Environment.NewLine +
                ErroredWheel.GetErrorMessage(ErroredWheel.ErrorState);
            ErrorTB.Multiline = true;
            ErrorTB.Width = 500;
            ErrorTB.Height = 300;
            ErrorTB.Location = new Point(65, 15);
            ErrorTB.TabStop = false;
            ErrorTB.Font = new Font(ErrorTB.Font, FontStyle.Bold);
            ErrorTB.TextAlign = HorizontalAlignment.Center;
            tp.Controls.Add(ErrorTB); 
        }

        private void DSettings_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            FilterWheel fw = (FilterWheel)FilterWheelManager.FilterWheelList[int.Parse(b.Name)];
            SetupForm s = new SetupForm(fw);
            s.ShowDialog();
            ConstructTabs();
        }

        private void FilterPositionBtn_GotFocus(object sender, EventArgs e)
        {
            //Button b = sender as Button;
            //foreach (Control x in b.Parent.Controls)
            //{
            //    if (x.GetType() == b.GetType())
            //    {
            //        x.BackColor = System.Drawing.SystemColors.Control;
            //        x.Font = new Font(b.Font, FontStyle.Regular);
            //    }
            //}
            //if (b.BackColor != Color.GreenYellow)
            //{
            //    b.Font = new Font(b.Font, FontStyle.Regular);
            //    b.BackColor = Color.Red;
            //}
 
        }

        private void FilterPositionBtn_LostFocus(object sender, EventArgs e)
        {
            
            //Button b = (Button)sender;
            //b.Font = new Font(b.Font, FontStyle.Regular);
            //b.BackColor = System.Drawing.SystemColors.Control;
        }

        private void FilterPositionBtn_Click(object sender, EventArgs e)
        {
           
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    int colonIndex = b.Name.IndexOf(":");
                    int deviceIndex = int.Parse(b.Name.Substring(0, (int)(b.Name.Length - colonIndex - 1)));
                    short newPos = short.Parse(b.Name.Substring(colonIndex + 1, b.Name.Length - colonIndex - 1));
                    FilterWheel fw = (FilterWheel)FilterWheelManager.FilterWheelList[deviceIndex];

                    foreach (Control x in b.Parent.Controls)
                    {
                        if (x.GetType() == b.GetType())
                        {
                            x.BackColor = System.Drawing.SystemColors.Control;
                            x.ForeColor = System.Drawing.SystemColors.ControlText;
                            x.Font = new Font(b.Font, FontStyle.Regular);
                        }
                    }
                    b.Font = new Font(b.Font, FontStyle.Bold);
                    b.ForeColor = Color.Black;
                    b.BackColor = Color.LightSteelBlue;
                    string realName = b.Text;
                    b.Text = "Moving";
                    Application.DoEvents();
                    fw.CurrentPosition = newPos;
                    b.BackColor = Color.DarkBlue;
                    b.Text = realName;
                    b.ForeColor = Color.GhostWhite;
                    Application.DoEvents();
                }
                catch (FirmwareException ex)
                {
                    string fix = "Press 'Home Wheel' or 'File -> Refresh' Devices to attempt to clear this error.";
                    MessageBox.Show(ex.Message + "\n" + fix, "Error While Moving...");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unknown Error...");
                }
            }
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            FilterWheel fw = (FilterWheel)FilterWheelManager.FilterWheelList[int.Parse(b.Name)];

            try
            {
                
                fw.ClearErrorState();
                fw.HomeDevice();      
                foreach (Control x in tabControl1.TabPages[0].Controls)
                {
                    if (x.Name == b.Name + ":1")
                    {
                        ((Button)x).Focus();
                        ((Button)x).Font = new Font(((Button)x).Font, FontStyle.Bold);
                        ((Button)x).BackColor = Color.SteelBlue;
                        
                    }
                }
                ConstructTabs();
            }
            catch (OptecHID_FilterWheelAPI.FirmwareException ex)
            {
                MessageBox.Show("An Error Occurred During the Home Procedure. \n" + ex.Message, "Homing Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unknown error has occurred. \n" + "Error Details: " + ex.ToString());
            }
        }
     
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 x = new AboutBox1();
            x.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void productDocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHelpFile();
        }

        private void refreshDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConstructTabs();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ConstructTabs();
        }

        

        private void OpenHelpFile()
        {
            SendKeys.Send("{F1}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                helpProvider1.HelpNamespace = System.IO.Path.Combine(appPath, "Resources\\HSFW Control Help.chm");
                VersionCheckerBGWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void VersionCheckerBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Check For A newer verison of the driver
                if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.HSFWControl))
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
                }
            }
            catch{} // Just ignore all errors. They mean the computer isn't connected to internet.
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (VersionCheckerBGWorker.IsBusy)
                {
                    string msg = "The Version Checker is currently busy. Please try again in a moment.";
                    MessageBox.Show(msg);
                }
                else
                {
                    if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.HSFWControl))
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



       
    
    }
}
