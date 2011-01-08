using System;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class SettingsForm : Form
    {
        private readonly Focuser _focuser = new Focuser();

        /// <summary>
        /// This is the method to run when the class is constructe
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This is the method to run when you want the form loaded with the class settings
        /// </summary>
        private void SettingsFormLoad(object sender, EventArgs e)
        {
            textBox1.Text = _focuser.MaxStep.ToString();
            textBox2.Text = _focuser.StepSize.ToString();
            textBox3.Text = _focuser.MaxIncrement.ToString();
            textBox4.Text = _focuser.Temperature.ToString();
            textBox5.Text = _focuser.TempMax.ToString();
            textBox6.Text = _focuser.TempMin.ToString();
            textBox7.Text = _focuser.TempPeriod.ToString();
            textBox8.Text = _focuser.TempSteps.ToString();

            checkBox1.Checked = _focuser.TempProbe;
            checkBox2.Checked = _focuser.TempCompAvailable;
            checkBox3.Checked = _focuser.CanStepSize;
            checkBox4.Checked = _focuser.CanHalt;
            checkBox5.Checked = _focuser.Synchronus;

            radioButton1.Checked = _focuser.Absolute;
            radioButton2.Checked = !radioButton1.Checked;
        }

        /// <summary>
        /// This is the method to run when you want the form reset with the defaults
        /// </summary>
        private void Button1Click(object sender, EventArgs e)
        {
            //_focuser.SetDefaultProfileSettings();
            //_focuser.SaveStaticProfileSettings();
            Dispose();
        }

        /// <summary>
        /// This is the method to run when you want save the contents of the form to the profile
        /// </summary>
        private void Button2Click(object sender, EventArgs e)
        {
            //save textboxes
            _focuser.MaxStep = Convert.ToInt32(textBox1.Text);
            _focuser.StepSize = Convert.ToDouble(textBox2.Text);
            _focuser.MaxIncrement = Convert.ToInt32(textBox3.Text);
            _focuser.Temperature = Convert.ToDouble(textBox4.Text);
            _focuser.TempMax = Convert.ToInt32(textBox5.Text);
            _focuser.TempMin = Convert.ToInt32(textBox6.Text);
            _focuser.TempPeriod = Convert.ToInt32(textBox7.Text);
            _focuser.TempSteps = Convert.ToInt32(textBox8.Text);

            //save checkboxes
            _focuser.TempProbe = checkBox1.Checked;
            _focuser.TempCompAvailable = checkBox2.Checked;
            _focuser.CanStepSize = checkBox3.Checked;
            _focuser.CanHalt = checkBox4.Checked;
            _focuser.Synchronus = checkBox5.Checked;

            //save radio button
            _focuser.Absolute = radioButton1.Checked;

            _focuser.SaveProfileSettings();
            Hide();
            Dispose();
        }

        /// <summary>
        /// This is the method to run when you want to turn the tempature probe off or on 
        /// </summary>
        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Enabled = false;
                checkBox2.Checked = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
            }
            else
            {
                checkBox1.Checked = true;
                checkBox2.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
            }
        }

        /// <summary>
        /// This is the method to run when you want to turn the tempature compensator off or on
        /// </summary>
        private void CheckBox2CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) checkBox1.Checked = true;
        }

        /// <summary>
        /// This is the method to run when you want to turn the step size off or on
        /// </summary>
        private void CheckBox3CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = checkBox3.Checked;
        }
    }
}
