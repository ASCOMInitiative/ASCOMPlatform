using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PyxisLE_API;
using System.Collections;

namespace PyxisLE_Control
{
    public partial class MainForm : Form
    {

        private Rotators RotatorMonitor;
        private Rotator myRotator;
        private ArrayList ControlList = new ArrayList();
        private const string NC_msg = "No connected Pyxis LE rotators are connected to the PC";
        private Timer UITimer = new Timer();
        private bool LastState = false;

        public MainForm()
        {
            InitializeComponent();
            ControlList.Add(SkyPA_TB);
            ControlList.Add(DA_TB);
            ControlList.Add(HomeBTN);
            UITimer.Interval = 500;
            UITimer.Enabled = true;
            UITimer.Tick += new EventHandler(UITimer_Tick);
        }

        void UITimer_Tick(object sender, EventArgs e)
        {
            if (LastState != Connected)
            {
                LastState = Connected;
                if (Connected)
                {
                    EnableControls();
                }
                else DisableControls();
            }
            else
            {
                try
                {
                    //DA_TB.
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RotatorMonitor = new Rotators();
            myRotator = FindMyDevice();
            if (myRotator != null)
            {
                StatusLabel.Text = "Connected to Pyxis LE with serial number: " + myRotator.SerialNumber;
                EnableControls();
            }
            else
            {
                StatusLabel.Text = "Searching for Pyxis LE...";
                DisableControls();
            }
        }

        #region Enable/Disable Controls

        private void EnableControls()
        {
            this.BeginInvoke(new DelNoParms(enableControls));
        }

        private void enableControls()
        {
            StatusLabel.Text = "Connected to Pyxis LE with serial number: " + myRotator.SerialNumber;
            foreach (Control x in ControlList)
            {
                x.Enabled = true;
            }
        }

        private void DisableControls()
        {
            this.BeginInvoke(new DelNoParms(disableControls));
        }

        private void disableControls()
        {
            StatusLabel.Text = "Searching for Pyxis LE...";
            foreach (Control x in ControlList)
            {
                x.Enabled = false;
            }
        }

        private Rotator FindMyDevice()
        {
            Rotator r = null;
            if (RotatorMonitor.RotatorList.Count > 0)
            {
                r = RotatorMonitor.RotatorList[0] as Rotator;
            }
            return r;
        }

        private delegate void DelNoParms();

        #endregion

        bool Connected
        {
            get
            {
                if(LastState == false)
                {
                    myRotator = FindMyDevice();
                }
                
                if (myRotator == null) return false;
                if (!myRotator.IsAttached)
                {
                    myRotator = null;
                    return false;
                }
                return true;
                
            }
        }

        private void HomeBTN_Click(object sender, EventArgs e)
        {
            myRotator.Home();
        }

        private void SetPA_BTN_Click(object sender, EventArgs e)
        {
            Double new_pa = double.Parse(SkyPA_TB.Text);
            //myRotator.CurrentPosition = 
        }

        private void RotatorDiagram_Click(object sender, EventArgs e)
        {
            MouseEventArgs ClickedPt = e as MouseEventArgs;
            double x1 = RotatorDiagram.Size.Width / 2;
            double y1 = RotatorDiagram.Size.Height / 2;
            double x2 = ClickedPt.X;
            double y2 = ClickedPt.Y;
            double dx = x2 - x1;
            double dy = y2 - y1;

            // Make sure it is within the blue border
            double DistFromCenter = Math.Sqrt((Math.Abs(dx) * Math.Abs(dx)) + (Math.Abs(dy) * Math.Abs(dy)));
            if (((DistFromCenter / y1) <= .76) && ((DistFromCenter / y1) >= .45))
                MessageBox.Show(DistFromCenter.ToString());
            
            double radians = Math.Atan2(dy, dx);
            double angle = -(radians * (180 / Math.PI) - 180);
            angle = angle + 90;
            if (angle > 360) angle = (angle - 360);
            //myRotator.ChangePosition(angle);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

    }
}
