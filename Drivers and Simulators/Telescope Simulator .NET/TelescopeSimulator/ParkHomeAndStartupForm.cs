using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.Simulator
{
    public partial class ParkHomeAndStartupForm : Form
    {

        public ParkHomeAndStartupForm()
        {
            InitializeComponent();

            // Draw the combobox in combobox style rather than comboboxlist style
            cmbStartupMode.DrawMode = DrawMode.OwnerDrawVariable;
            cmbStartupMode.DrawItem += DropDownListComboBox_DrawItem;

            // Set invalid input event handlers
            txtStartAzimuth.TextChanged += Textbox_TextChanged;
            txtStartAltitude.TextChanged += Textbox_TextChanged;
            txtParkAzimuth.TextChanged += Textbox_TextChanged;
            txtParkAltitude.TextChanged += Textbox_TextChanged;
            TxtHomeAzimuth.TextChanged += Textbox_TextChanged;
            TxtHomeAltitude.TextChanged += Textbox_TextChanged;

            txtStartAzimuth.Validating += TextBox_Validating;
            txtStartAltitude.Validating += TextBox_Validating;
            txtParkAzimuth.Validating += TextBox_Validating;
            txtParkAltitude.Validating += TextBox_Validating;
            TxtHomeAzimuth.Validating += TextBox_Validating;
            TxtHomeAltitude.Validating += TextBox_Validating;

            // Populate startup options list
            foreach (string option in TelescopeHardware.StartupOptions)
            {
                cmbStartupMode.Items.Add(option);
            }

            string startupOption;
            using (Profile profile = new Profile())
            {
                startupOption = profile.GetValue(SharedResources.PROGRAM_ID, "StartUpMode", "",TelescopeHardware.StartupOptions[0]);
            }

            cmbStartupMode.SelectedItem = startupOption;

            // Populate the form text boxes
            txtStartAzimuth.Text = TelescopeHardware.StartCoordinates.X.ToString();
            txtStartAltitude.Text = TelescopeHardware.StartCoordinates.Y.ToString();
            txtParkAzimuth.Text = TelescopeHardware.ParkAzimuth.ToString();
            txtParkAltitude.Text = TelescopeHardware.ParkAltitude.ToString();
            TxtHomeAzimuth.Text = TelescopeHardware.HomePosition.X.ToString();
            TxtHomeAltitude.Text = TelescopeHardware.HomePosition.Y.ToString();
        }

        /// <summary>
        /// Validate the textbox contents to ensure that it is a valid double number, Called every time a the text changes so that issues are flagged immediately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Textbox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            try
            {
                double x = double.Parse(textBox.Text);
                ErrorFlag.SetError(textBox, "");
                BtnOK.Enabled = true;
            }
            catch
            {
                ErrorFlag.SetError(textBox, "Not a valid number.");
                BtnOK.Enabled = false;
            }
        }

        /// <summary>
        /// Set the cancel flag if there is an error in this text box to ensure that the issue is addressed immedialtely
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if ((textBox.Name == txtStartAzimuth.Name) & (ErrorFlag.GetError(txtStartAzimuth) != "")) e.Cancel = true;
            else if ((textBox.Name == txtStartAltitude.Name) & (ErrorFlag.GetError(txtStartAltitude) != "")) e.Cancel = true;
            else if ((textBox.Name == txtParkAzimuth.Name) & (ErrorFlag.GetError(txtParkAzimuth) != "")) e.Cancel = true;
            else if ((textBox.Name == txtParkAltitude.Name) & (ErrorFlag.GetError(txtParkAltitude) != "")) e.Cancel = true;
            else if ((textBox.Name == TxtHomeAzimuth.Name) & (ErrorFlag.GetError(TxtHomeAzimuth) != "")) e.Cancel = true;
            else if ((textBox.Name == TxtHomeAltitude.Name) & (ErrorFlag.GetError(TxtHomeAltitude) != "")) e.Cancel = true;

        }

        private void DropDownListComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = sender as ComboBox;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) // Draw the selected item in menu highlight colour
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.MenuHighlight), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(SystemColors.HighlightText), new Point(e.Bounds.X, e.Bounds.Y));
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(combo.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            }

            e.DrawFocusRectangle();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            TelescopeHardware.StartCoordinates.X = double.Parse(txtStartAzimuth.Text);
            TelescopeHardware.StartCoordinates.Y = double.Parse(txtStartAltitude.Text);
            TelescopeHardware.ParkAzimuth = double.Parse(txtParkAzimuth.Text);
            TelescopeHardware.ParkAltitude = double.Parse(txtParkAltitude.Text);
            TelescopeHardware.HomePosition.X = double.Parse(TxtHomeAzimuth.Text);
            TelescopeHardware.HomePosition.Y = double.Parse(TxtHomeAltitude.Text);
            using (Profile profile = new Profile())
            {
                profile.WriteValue(SharedResources.PROGRAM_ID, "HomeAzimuth", TelescopeHardware.HomePosition.X.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(SharedResources.PROGRAM_ID, "HomeAltitude", TelescopeHardware.HomePosition.Y.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", TelescopeHardware.ParkAzimuth.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", TelescopeHardware.ParkAltitude.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuthConfigured", TelescopeHardware.StartCoordinates.X.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(SharedResources.PROGRAM_ID, "StartAltitudeConfigured", TelescopeHardware.StartCoordinates.Y.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(SharedResources.PROGRAM_ID, "StartUpMode", cmbStartupMode.Text);
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
