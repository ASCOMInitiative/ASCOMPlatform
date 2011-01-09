using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.FilterWheelSim
{
    public partial class frmSetupDialog : Form
    {

        // Declare Form control arrays.
        private static TextBoxArray m_arrNameTextBox; 
        private static TextBoxArray m_arrOffsetTextBox;
        private static PictureBoxArray m_arrColourPicBox;

        private static int m_iSlots;                 // Current number of filter slots

        public frmSetupDialog()
        {
            InitializeComponent();

            // Uncomment the line below to change the locale to German, for testing dot/comma decimal issues
            // System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");

            // Create a tooltip object, and assign a few values
            ToolTip aTooltip  = new ToolTip();
            aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website");
            aTooltip.SetToolTip(chkImplementsNames, "Driver returns default names if cleared");
            aTooltip.SetToolTip(chkImplementsOffsets, "Driver raises an exception if cleared");

            // Create control arrays for the filters (arrays make it easier to handle)
            m_arrNameTextBox = new TextBoxArray(this);
            m_arrOffsetTextBox = new TextBoxArray(this);
            m_arrColourPicBox = new PictureBoxArray(this);

            // Populate the time combo with localised values
            for (double j=0.5; j <= 4; j+=0.5)
                cmbTime.Items.Add(j.ToString("F1"));

            // Create the textbox and picture controls on the form
            for (int i=0; i <= 7; i++)
            {
                // Add a new control
                m_arrNameTextBox.AddNewTextBox(false);
                // Associate it with the TableLayout grid
                m_arrNameTextBox[i].Parent = TableLayoutPanel2;
                m_arrNameTextBox[i].Width = 120;
                // Set the cell position
                TableLayoutPanel2.SetRow(m_arrNameTextBox[i], i);
                TableLayoutPanel2.SetColumn(m_arrNameTextBox[i], 1);

                // Repeat for the other controls
                m_arrOffsetTextBox.AddNewTextBox(true);
                m_arrOffsetTextBox[i].Parent = TableLayoutPanel2;
                TableLayoutPanel2.SetRow(m_arrOffsetTextBox[i], i);
                TableLayoutPanel2.SetColumn(m_arrOffsetTextBox[i], 2);

                m_arrColourPicBox.AddNewPictureBox();
                m_arrColourPicBox[i].Parent = TableLayoutPanel2;
                TableLayoutPanel2.SetRow(m_arrColourPicBox[i], i);
                TableLayoutPanel2.SetColumn(m_arrColourPicBox[i], 3);
            }

            // Give 'em a bit of info...
            lblDriverInfo.Text = ProductName + " Version " + ProductVersion;

            EnableDisableControls();

        }

#region Properties and Methods

        public int Slots { set { cmbSlots.Text = value.ToString(); } }

        public int Time
        {
            set
            {
                // We store the time in millisecs, convert to seconds for display
                cmbTime.Text = String.Format("{0:F1}", value / 1000.0);
            }
        }

        public string[] Names
        {
            set
            {
                for (int i = 0; i <= 7; i++)
                    m_arrNameTextBox[i].Text = value[i];
            }
        }

        public int[] Offsets
        {
            set
            {
                for (int i = 0; i <= 7; i++)
                    m_arrOffsetTextBox[i].Text = String.Format("{0}", value[i]);    // No decimal digits
            }
        }

        public Color[] Colours
        {
            set
            {
                for (int i = 0; i <= 7; i++)
                    m_arrColourPicBox[i].BackColor = value[i];
            }
        }

        public bool ImplementsNames { set { chkImplementsNames.Checked = value; } }

        public bool ImplementsOffsets { set { chkImplementsOffsets.Checked = value; } }

        public bool PreemptsMoves { set {chkPreemptMoves.Checked = value; } }

#endregion


#region Event Handlers

        private void OK_Button_Click(object sender, EventArgs e)
        {
            int i;

            // Write all the settings to the registry
            m_iSlots = Convert.ToInt32(cmbSlots.Text);
            SimulatedHardware.g_Profile.WriteValue(SimulatedHardware.g_csDriverID, "Slots", m_iSlots.ToString());
            // Convert secs to millisecs
            try { i = Convert.ToInt32(float.Parse(cmbTime.Text, System.Globalization.NumberStyles.AllowDecimalPoint) * 1000); }
            catch { i = 1000; }
            SimulatedHardware.g_Profile.WriteValue(SimulatedHardware.g_csDriverID, "Time", i.ToString());
            for (i=0; i <= 7; i++)
            {
                SimulatedHardware.g_Profile.WriteValue(SimulatedHardware.g_csDriverID, i.ToString(), m_arrNameTextBox[i].Text, "FilterNames");
                SimulatedHardware.g_Profile.WriteValue(SimulatedHardware.g_csDriverID, i.ToString(), m_arrOffsetTextBox[i].Text, "FocusOffsets");
                SimulatedHardware.g_Profile.WriteValue(SimulatedHardware.g_csDriverID, i.ToString(), ColorTranslator.ToWin32(m_arrColourPicBox[i].BackColor).ToString(), "FilterColours");
            }
            SimulatedHardware.g_Profile.WriteValue(SimulatedHardware.g_csDriverID, "ImplementsNames", chkImplementsNames.Checked.ToString());
            SimulatedHardware.g_Profile.WriteValue(SimulatedHardware.g_csDriverID, "ImplementsOffsets", chkImplementsOffsets.Checked.ToString());
            SimulatedHardware.g_Profile.WriteValue(SimulatedHardware.g_csDriverID, "PreemptMoves", chkPreemptMoves.Checked.ToString());

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void picASCOM_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void cmbSlots_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow 0-9 and backspace
            if (e.KeyChar < '0' || e.KeyChar > '9')
                if (e.KeyChar != '\b')
                    e.Handled = true;
        }

        private void cmbSlots_SelectedValueChanged(object sender, EventArgs e)
        {
            int i = Convert.ToInt32("0" + cmbSlots.Text);    // Make blanks = 0
            if (i >= 1 && i <= 8)
            {
                m_iSlots = i;
                EnableDisableControls();
            }
        }

        private void cmbSlots_Validating(object sender, CancelEventArgs e)
        {
            int i = Convert.ToInt32("0" + cmbSlots.Text);    // Make blanks = 0
            if (i <= 0 || i >= 9)
            {
                e.Cancel = true;
                MessageBox.Show("Range of slot values is 1-8");
            }
        }

        private void cmbTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow one occurrence of the period
            // Oh-oh, decimal could be a comma!
            //if (e.KeyChar == '.' && cmbTime.Text.Contains("."))
            //    e.Handled = true;
            // Now carry out numerals and period check
            // Oh-oh, decimal could be a comma!
            //else if (e.KeyChar < '.' || e.KeyChar > '9' || e.KeyChar == '/')
            if (e.KeyChar < ',' || e.KeyChar > '9' || e.KeyChar == '-' || e.KeyChar == '/')
                if (e.KeyChar != '\b') e.Handled = true;
        }

        private void cmbTime_Validating(object sender, CancelEventArgs e)
        {
            double i = 99;      // force error if conversion fails

            try
            {
                i = Convert.ToDouble("0" + cmbTime.Text);        // Make blanks = 0
            }
            catch { }
            if (i < 0.1 || i >= 9)
            {
                MessageBox.Show("Range of time values is 0.1-8.0");
                e.Cancel = true;
            }
        }

        private void chkImplementsNames_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableControls();
        }

        private void chkImplementsOffsets_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableControls();
        }

        private void frmSetupDialog_Shown(object sender, EventArgs e)
        {
            EnableDisableControls();
            this.BringToFront();
        }

#endregion


#region Helpers

        //
        // Make sure GUI elements are in sync with the number of slots
        //
        private void EnableDisableControls()
        {
            for (int i = 0; i <= 7; i++)
            {
                if (m_iSlots > i)
                { // Enable controls
                    ControlEnable(m_arrNameTextBox[i], chkImplementsNames.Checked);
                    ControlEnable(m_arrOffsetTextBox[i], chkImplementsOffsets.Checked);
                    ControlEnable(m_arrColourPicBox[i], true);
                }
                else
                { // Disable controls
                    ControlEnable(m_arrNameTextBox[i], false);
                    ControlEnable(m_arrOffsetTextBox[i], false);
                    ControlEnable(m_arrColourPicBox[i], false);
                }
            }

            this.Refresh();
        }

        //
        // Enable/disable controls
        //
        private void ControlEnable(Control control, bool enabled)
        {
            if (enabled)
            {
                control.Enabled = true;
                control.ForeColor = Color.Black;
            }
            else
            {
                control.Enabled = false;
                control.ForeColor = Color.DarkGray;
            }
        }

#endregion


    }
}
