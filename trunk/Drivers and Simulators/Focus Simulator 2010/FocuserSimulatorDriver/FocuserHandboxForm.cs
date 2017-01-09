using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;

namespace ASCOM.Simulator
{
    public partial class FocuserHandboxForm : Form
    {
        private readonly Focuser _focuser; // = new Focuser();
        private const int positionClick = 1;
        private Timer MyTimer = new Timer();

        /// <summary>
        /// This is the method to run when the class is constructed
        /// </summary>
        public FocuserHandboxForm(Focuser MyFocuser)
        {
            _focuser = MyFocuser; // Save supplied pointer to the owning focus driver
            InitializeComponent();
            UpdateDisplay();
            btnGoTo.Enabled = false;

            lblVersion.Text = _focuser.Name + @" v" + _focuser.DriverVersion;

            /* Adds the event and the event handler for the method that will process the timer event to the timer. */
            MyTimer.Tick += TimerEventProcessor;

            // Sets the timer interval to 0.1 seconds.
            MyTimer.Interval = 100;
            MyTimer.Start(); 

            btnMoveOut.MouseDown += new MouseEventHandler(btnMoveOut_MouseDown);
            btnMoveOut.MouseUp += new MouseEventHandler(btnMove_MouseUp);
            btnMoveIn.MouseDown += new MouseEventHandler(btnMoveIn_MouseDown);
            btnMoveIn.MouseUp += new MouseEventHandler(btnMove_MouseUp);
            txtGoTo.TextChanged += new EventHandler(txtGoTo_Changed);
        }

        private void btnMoveOut_MouseDown(object sender, MouseEventArgs e)
        {
            _focuser.KeepMoving = true;
            _focuser.Target = _focuser.Target + 1;
            _focuser.LastOffset = 1;
            _focuser.MouseDownTime = DateTime.Now;
        }
        private void btnMoveIn_MouseDown(object sender, MouseEventArgs e)
        {
            _focuser.KeepMoving = true;
            _focuser.Target = _focuser.Target - 1;
            _focuser.LastOffset = -1;
            _focuser.MouseDownTime = DateTime.Now;
        }
        private void btnMove_MouseUp(object sender, MouseEventArgs e)
        {
            _focuser.KeepMoving = false;
            _focuser.Target = _focuser.Position;
            _focuser.RateOfChange = 1;
            _focuser.MouseDownTime = DateTime.MaxValue;
        }

        /// <summary>
        /// This is the method to run when the timer is raised.
        /// </summary>
        private void TimerEventProcessor(object caller, EventArgs e)
        {
             UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            lblTempDisplay.Text = _focuser.TempProbe ? _focuser.Temperature.ToString(CultureInfo.CurrentCulture) : 0.ToString();
            lblPositionDisplay.Text = _focuser.Position.ToString(CultureInfo.CurrentCulture);

            if (_focuser.TempCompAvailable)
            {
                chkTempCompEnabled.Enabled = true;
                chkTempCompEnabled.Checked = _focuser.tempComp;
            }
            else
            {
                chkTempCompEnabled.Enabled = false;
                chkTempCompEnabled.Checked = false;
                chkTempCompEnabled.ForeColor = System.Drawing.Color.White;
            }
        }

        /// <summary>
        /// This is the method to run when the position needs updated, also updates the screen
        /// </summary>
        private void UpdatePosition(int offSet)
        {
          /*  if (offSet != 0)
            {
                if (_focuser.Absolute)
                    _focuser.Target = _focuser.Position + offSet;
                else
                {
                    _focuser.Position = offSet;
                    _focuser.Target = 0;
                }*/
                _focuser.LastOffset = offSet;
            //}
        }

        /// <summary>
        /// This is the method to run when the In button is clicked
        /// </summary>
  /*      private void BtnMoveIn_Click(object sender, EventArgs e)
        {
            UpdatePosition(-positionClick);
            UpdateDisplay();
        }

        /// <summary>
        /// This is the method to run when the Out button is clicked
        /// </summary>
        private void btnMoveOut_Click(object sender, EventArgs e)
        {
            UpdatePosition(positionClick);
            UpdateDisplay();
        }
        */
        /// <summary>
        /// This is the method to run when the tempature compensator is turned on or off
        /// </summary>
        private void chkTempCompEnabled_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkTempCompEnabled.Checked)
            {
                btnMoveIn.Enabled = false;
                btnMoveOut.Enabled = false;
                _focuser.tempComp = true;
            }
            else
            {
                btnMoveIn.Enabled = true;
                btnMoveOut.Enabled = true;
                _focuser.tempComp = false;
            }
            Focuser.SaveProfileSetting("TempComp", _focuser.tempComp.ToString(CultureInfo.InvariantCulture));
        }
        
        /// <summary>
        /// This disables the form close button i.e. the X in the top right hand corner of the form.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                const int CP_NOCLOSE_BUTTON = 0x200;
                CreateParams mdiCp = base.CreateParams;
                mdiCp.ClassStyle = mdiCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return mdiCp;
            }
        }

        /// <summary>
        /// This is the method to run when the Settings button is clicked
        /// </summary>
        private void btnShowSetupDialog_Click(object sender, EventArgs e)
        {
            _focuser.SetupDialog();
        }

        private void btnGoTo_Click(object sender, EventArgs e)
        {
            _focuser.Target = Convert.ToInt32(txtGoTo.Text);
            _focuser.Move(Convert.ToInt32(txtGoTo.Text));
        }

        private void txtGoTo_Changed(object sender, System.EventArgs e)
        {
            ValidateGoTo();
        }
        private bool ValidateGoTo()
        {
            bool Ok = true;
            GoToErrorProvider.Clear();
            if (!string.IsNullOrEmpty(txtGoTo.Text))
            {
                try
                {
                    if (Convert.ToInt32(txtGoTo.Text) > _focuser.MaxStep)
                    {
                        GoToErrorProvider.SetError(this.txtGoTo, "Exceeds maximum value");
                        Ok = false;
                    }
                    else if (Convert.ToInt32(txtGoTo.Text) < 0)
                    {
                        GoToErrorProvider.SetError(this.txtGoTo, "Less than minimum value");
                        Ok = false;
                    }
                }
                catch
                {
                    GoToErrorProvider.SetError(this.txtGoTo, "Invalid value");
                    Ok = false;
                }
                if (Ok) btnGoTo.Enabled = true;
                else btnGoTo.Enabled = false;
            }
            else btnGoTo.Enabled = false;


            return Ok;

        }

    }
}
