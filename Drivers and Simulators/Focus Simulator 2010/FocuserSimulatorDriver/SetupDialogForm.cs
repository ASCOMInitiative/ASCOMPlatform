using System;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        //private readonly Focuser _focuser = new Focuser();
        private static readonly Random RandomSeed = new Random();
        private const int positionClick = 250;
        static readonly Timer MyTimer = new Timer();
        private const int timerInterval = 100; //Update the display every 100ms i.e. 10 times per second
        private Focuser _focuser;
        private int lastPosition;
        private double lastTemperature;
        
        /// <summary>
        /// This is the method to run when the class is constructed
        /// </summary>
        public SetupDialogForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructir used to initialise class and pass in the Focuser device that is currently running
        /// </summary>
        /// <param name="CallingFocuser">Focuser device calling this setup dialogue</param>
        public SetupDialogForm(Focuser CallingFocuser)
        {
            InitializeComponent();
            _focuser = CallingFocuser;
            MyTimer.Interval = timerInterval;
            MyTimer.Tick += new EventHandler(TimerEventProcessor);
            MyTimer.Start();
            UpdatePosition();
            UpdateTempature();
        }

        /// <summary>
        /// This is the method to run when the timer is raised.
        /// </summary>
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            
                UpdateTempature();
                UpdatePosition();    
        }

        /// <summary>
        /// This is the method to run when the tempature needs updated, also updates the screen
        /// </summary>
        private void UpdateTempature()
        {
            try
            {
                double currentTemperature = _focuser.Temperature; 
                if (currentTemperature != lastTemperature)
                {
                    label4.Text = currentTemperature.ToString(); 
                    lastTemperature = currentTemperature;
                }
            }
            catch (NotConnectedException ex)
            { string msg = ex.Message; }
        }

        /// <summary>
        /// This is the method to run when the position needs updated, also updates the screen
        /// </summary>
        private void UpdatePosition()
        {
            try
            {
                int currentPosition = _focuser.Position;
                if (currentPosition != lastPosition)
                {
                    label2.Text = currentPosition.ToString();
                    lastPosition = currentPosition;
                }
            }
            catch (NotConnectedException ex)
            { string msg = ex.Message; }
        }

        /// <summary>
        /// This is the method to run when the In button is clicked
        /// </summary>
        private void Button1Click(object sender, EventArgs e)
        {
            //UpdatePosition(-positionClick);
            _focuser.Position -= positionClick;
            UpdatePosition();
        }

        /// <summary>
        /// This is the method to run when the Out button is clicked
        /// </summary>
        private void Button2Click(object sender, EventArgs e)
        {
            //UpdatePosition(positionClick);
            _focuser.Position += positionClick;
            UpdatePosition();
        }

        /// <summary>
        /// This is the method to run when the Settings button is clicked
        /// </summary>
        private void Button3Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            Hide();
            settingsForm.ShowDialog();
        }

        /// <summary>
        /// This is the method to run when the tempature compesator is turned on or off
        /// </summary>
        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
   /*         if (checkBox1.Checked)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                _focuser.TempComp = true;
                MyTimer.Start();
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                _focuser.TempComp = false;
                MyTimer.Stop();
            }
            _focuser.SaveProfileSetting("TempComp", _focuser.TempComp.ToString()); */
        }
    }
}
