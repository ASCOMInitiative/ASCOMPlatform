using System;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class SetupDialogForm : Form
    {
        private readonly Focuser _focuser = new Focuser();
        private double _tempFlux;
        private static readonly Random RandomSeed = new Random();
        private const int positionClick = 250;
        static readonly Timer MyTimer = new Timer();
        
        /// <summary>
        /// This is the method to run when the class is constructed
        /// </summary>
        public SetupDialogForm()
        {
            InitializeComponent();
            UpdatePosition(0);
            UpdateTempature(0);

            if (_focuser.TempCompAvailable)
            {
                checkBox1.Enabled = true;
                checkBox1.Checked = _focuser.TempComp;
            }

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
            MyTimer.Interval = _focuser.TempPeriod * 1000;
        }

        /// <summary>
        /// This is the method to run when the timer is raised.
        /// </summary>
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            MyTimer.Enabled = true;

            _tempFlux  = GenerateFakeTempature();

            if (RandomBool())
            {
                UpdateTempature(_tempFlux);
                if (_focuser.Temperature > _focuser.TempMax)
                    _focuser.Temperature = _focuser.TempMax;
                UpdatePosition((int)(Math.Round((-_tempFlux * _focuser.StepSize), 0)));
            }
            else
            {
                UpdateTempature(-_tempFlux);
                if (_focuser.Temperature < _focuser.TempMin)
                    _focuser.Temperature = _focuser.TempMin;
                UpdatePosition((int)(Math.Round((-_tempFlux * _focuser.StepSize), 0)));
            }
        }

        /// <summary>
        /// Returns a random boolean value
        /// </summary>
        /// <returns>Random boolean value</returns>
        public static bool RandomBool()
        {
            return (RandomSeed.NextDouble() > 0.5);
        }

        /// <summary>
        /// This is the method to run when the tempature needs updated, also updates the screen
        /// </summary>
        private void UpdateTempature(double offSet)
        {
            _focuser.Temperature = Math.Round(_focuser.Temperature + offSet,2);
            label4.Text = _focuser.TempProbe ? _focuser.Temperature.ToString() : 0.ToString();
        }

        /// <summary>
        /// This is the method to run when you need to generate a tempature change
        /// </summary>
        private static double GenerateFakeTempature()
        {
            var random = new Random();
            return random.NextDouble();
        }

        /// <summary>
        /// This is the method to run when the position needs updated, also updates the screen
        /// </summary>
        private void UpdatePosition(int offSet )
        {
            if (offSet != 0)
            {
                _focuser.Move(offSet);
            }
            label2.Text = _focuser.Position.ToString();
        }

        /// <summary>
        /// This is the method to run when the In button is clicked
        /// </summary>
        private void Button1Click(object sender, EventArgs e)
        {
            UpdatePosition(-positionClick);
        }

        /// <summary>
        /// This is the method to run when the Out button is clicked
        /// </summary>
        private void Button2Click(object sender, EventArgs e)
        {
            UpdatePosition(positionClick);
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
            if (checkBox1.Checked)
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
            _focuser.SaveProfileSetting("TempComp", _focuser.TempComp.ToString());
        }
    }
}
