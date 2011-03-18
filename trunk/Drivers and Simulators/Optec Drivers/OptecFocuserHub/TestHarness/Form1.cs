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
        public Form1()
        {
            InitializeComponent();
        }

        Focuser myFoc1;
        Focuser myFoc2;

        private void button1_Click(object sender, EventArgs e)
        {
            ASCOM.Utilities.Chooser ch = new ASCOM.Utilities.Chooser();
            ch.DeviceType = "Focuser";
            ProgID1.Text = ch.Choose();
            try
            {
                myFoc1 = new ASCOM.DriverAccess.Focuser(ProgID1.Text);
                Properties.Settings.Default.ProgID1 = ProgID1.Text;
                Properties.Settings.Default.Save();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ASCOM.Utilities.Chooser ch = new ASCOM.Utilities.Chooser();
            ch.DeviceType = "Focuser";
            ProgID2.Text = ch.Choose();
            try
            {
                myFoc2 = new ASCOM.DriverAccess.Focuser(ProgID2.Text);
                Properties.Settings.Default.ProgID2 = ProgID2.Text;
                Properties.Settings.Default.Save();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (myFoc1 == null) myFoc1 = new Focuser(ProgID1.Text);
                myFoc1.Link = true;
                RefreshStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Connect2BTN_Click(object sender, EventArgs e)
        {
            if (myFoc2 == null) myFoc2 = new Focuser(ProgID2.Text);
            myFoc2.Link = true;
            RefreshStatus();
        }

        private void RefreshStatus()
        {
            if(myFoc1 != null)
            {
                ConnLabel1.Text = myFoc1.Link.ToString();
                if (myFoc1.Link)
                {
                    TempLabel1.Text = myFoc1.Temperature.ToString();
                    CurrentPosLBL1.Text = myFoc1.Position.ToString();
                    
                }
            }
            if(myFoc2 != null)
            {
                ConnLabel2.Text = myFoc2.Link.ToString();
                if (myFoc2.Link)
                {
                    TempLabel2.Text = myFoc2.Temperature.ToString();
                    CurrentPosLabel2.Text = myFoc2.Position.ToString();
                }
            }
             
        }

        private void RefreshBTN_Click(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            myFoc2.Link = false;
            RefreshStatus();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            myFoc1.Link = false;
            RefreshStatus();
        }

        private void Move1_Click(object sender, EventArgs e)
        {
            myFoc1.Move(int.Parse(newPosTB.Text));
        }

        private void Move2_Click(object sender, EventArgs e)
        {
            myFoc2.Move(int.Parse(newPosTB.Text));
        }

        private void statusUpdateTimer_Tick(object sender, EventArgs e)
        {
            this.Invoke(new statusUpdater(updateStatus));
        }

        private delegate void statusUpdater();

        private void updateStatus()
        {
            if (myFoc1 != null && myFoc1.Link == true)
            {
                IsMoving1RB.Checked = myFoc1.IsMoving;
                TempLabel1.Text = myFoc1.Temperature.ToString();
                CurrentPosLBL1.Text = myFoc1.Position.ToString();
            }

            if (myFoc2 != null && myFoc2.Link == true)
            {
                IsMoving2RB.Checked = myFoc2.IsMoving;
                TempLabel2.Text = myFoc2.Temperature.ToString();
                CurrentPosLabel2.Text = myFoc2.Position.ToString();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProgID1.Text = Properties.Settings.Default.ProgID1;
            ProgID2.Text = Properties.Settings.Default.ProgID2;
        }

       


    }
}
