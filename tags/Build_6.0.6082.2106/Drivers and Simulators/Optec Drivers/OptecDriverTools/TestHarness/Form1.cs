using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Optec;
using System.Xml.Linq;
using System.Reflection;

namespace TestHarness
{
    public partial class Form1 : Form
    {
        NewVersionChecker myVersionChecker;

        public Form1()
        {
            InitializeComponent();
            myVersionChecker = new NewVersionChecker();
            myVersionChecker.NewVersionDetected += new EventHandler(myVersionChecker_NewVersionDetected);
            
        }

        void myVersionChecker_NewVersionDetected(object sender, EventArgs e)
        {
            Form s = sender as Form;
            s.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myVersionChecker.CheckForNewVersion(Assembly.GetExecutingAssembly(), "testDevice", true, Properties.Resources.TCF_S_2010);

            EventLogger.LoggingLevel = System.Diagnostics.TraceLevel.Info;
            XMLSettings mySettings = new XMLSettings();
            listBox1.Items.Add(mySettings.ComPortName);
            mySettings.ComPortName = "COM2";
            listBox1.Items.Add(mySettings.ComPortName);
            mySettings.ComPortName = "COM3";
            listBox1.Items.Add(mySettings.ComPortName);
            mySettings.ComPortName = "COM4";
            listBox1.Items.Add(mySettings.ComPortName);
            mySettings.ComPortName = "COM5";
            listBox1.Items.Add(mySettings.ComPortName);
            mySettings.ComPortName = "COM6";
            listBox1.Items.Add(mySettings.ComPortName);

            listBox1.Items.Add("Testing Instances...");
            listBox1.Items.Add("Current Instance Name = " + mySettings.CurrentInstance);
            listBox1.Items.Add("Default Instance Name = " + mySettings.DefaultInstance);

            listBox1.Items.Add("Creating new instance SomeDevice 3\"");
            mySettings.CreateNewInstance("SomeDevice 3\"");

            listBox1.Items.Add("Setting the new instance as Current 3\"");
            mySettings.CurrentInstance = "SomeDevice 3\"";
            listBox1.Items.Add("Current Instance Name = " + mySettings.CurrentInstance);
            listBox1.Items.Add("Default Instance Name = " + mySettings.DefaultInstance);

            listBox1.Items.Add("Setting the new instance as Default 3\"");
            mySettings.DefaultInstance = "SomeDevice 3\"";
            listBox1.Items.Add("Current Instance Name = " + mySettings.CurrentInstance);
            listBox1.Items.Add("Default Instance Name = " + mySettings.DefaultInstance);

            listBox1.Items.Add(mySettings.ComPortName);
            mySettings.ComPortName = "COM2";
            listBox1.Items.Add(mySettings.ComPortName);

            listBox1.Items.Add("Available Instances:");
            foreach (string x in mySettings.SettingsInstanceNames)
            {
                listBox1.Items.Add(x);
            }
            mySettings.AddFocusOffset("Blue", 13.5);
            mySettings.AddFocusOffset("Red / turqouise / purple", 15.8);
            mySettings.AddFocusOffset("Green", 1874521);

            listBox1.Items.Add("Getting Focus Offsets");
            string p = mySettings.PrintFocusOffsets();
            listBox1.Items.Add(p);

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myVersionChecker.CheckForNewVersion(Assembly.GetExecutingAssembly(),
                "testDevice",
                true,
                Properties.Resources.TCF_S_2010);
        }
    }
}
