using System;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        // Sets up an instance of focuser driver for use in this class
        private readonly Focuser _focuser = new Focuser();

        // sets up a new timer to use
        static readonly Timer MyTimer = new Timer();

        // This is the method to run when the class is constructed
        public SetupDialogForm()
        {
            InitializeComponent();
            UpdatePosition(0);
            UpdateTempature(0);

            checkBox1.Enabled = _focuser.TempCompAvailable;
            if (checkBox1.Checked)
            {MyTimer.Start();}
            else
            {
                MyTimer.Stop();
            }

            /* Adds the event and the event handler for the method that will 
          process the timer event to the timer. */
            MyTimer.Tick += TimerEventProcessor;

            // Sets the timer interval to 1 seconds.
            MyTimer.Interval = 1000;
        }

        // This is the method to run when the timer is raised.
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            MyTimer.Enabled = true;

            //todo  add in what happens when the temp comp is turned on
            UpdateTempature(-.1);
            
        }

        // This is the method to run when the tempature needs updated, also updates the screen
        private void UpdateTempature(double offSet)
        {
            _focuser.Temperature = _focuser.Temperature + offSet;
            label4.Text = _focuser.TempProbe ? _focuser.Temperature.ToString() : 0.ToString();
        }

        // This is the method to run when the position needs updated, also updates the screen
        private void UpdatePosition(int offSet )
        {
            if (offSet != 0)
            {
                _focuser.Move(offSet);
            }
            label2.Text = _focuser.Position.ToString();
        }

        // This is the method to run when the In button is clicked
        private void Button1Click(object sender, EventArgs e)
        {
                UpdatePosition(-250);
        }

        // This is the method to run when the Out button is clicked
        private void Button2Click(object sender, EventArgs e)
        {
                UpdatePosition(250);
        }

        // This is the method to run when the Settings button is clicked
        private void Button3Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            Hide();
            Close();
            settingsForm.ShowDialog();
        }

        // This is the method to run when the tempature compesator is turned on or off
        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                MyTimer.Start();
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                MyTimer.Stop();
            }
        }
    }
}
