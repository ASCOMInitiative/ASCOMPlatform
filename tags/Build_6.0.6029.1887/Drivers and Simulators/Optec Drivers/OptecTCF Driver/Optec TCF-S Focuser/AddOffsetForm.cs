using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Optec_TCF_S_Focuser
{
    public partial class AddOffsetForm : Form
    {
        private FormFunctions thisFunction;

        public AddOffsetForm(FormFunctions f, OptecFocuser myFocuser )
        {
            InitializeComponent();
            thisFunction = f;
            if (f == FormFunctions.AbsolutePresets)
            {
                this.Text = "Add Absolute Preset";
                this.Prompt_TB.Text = "Enter a name and value for a absolute focus preset to be added";
                this.Offset_NUD.Maximum = myFocuser.MaxSteps;
                this.Offset_NUD.Minimum = 1;
            }
            else
            {
                this.Text = "Add Focus Offset";
                this.Prompt_TB.Text = "Enter a name and value for the relative focus offset to be added";
                this.Offset_NUD.Maximum = myFocuser.MaxSteps;
                this.Offset_NUD.Minimum = -myFocuser.MaxSteps;
            }

        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            TextBox t = sender as TextBox;
            string name = t.Text;
            if (name.Length < 1)
            {
                errorProvider1.SetError(t, "Enter at least one character");
                AddBtn.Enabled = false;
                return;
            }

            if (thisFunction == FormFunctions.RelativeOffsets)
            {
                if (!XMLSettings.CheckOffsetNameUnique(name))
                {
                    errorProvider1.SetError(t, "An offset with the name " + name + " already exists. " +
                        "Please choose a different name.");
                    AddBtn.Enabled = false;
                }
                else
                {
                    errorProvider1.Clear();
                    AddBtn.Enabled = true;
                }
            }
            else
            {
                if (!XMLSettings.CheckPresetNameUnique(name))
                {
                    errorProvider1.SetError(t, "A preset with the name " + name + " already exists. " +
                        "Please choose a different name.");
                    AddBtn.Enabled = false;
                }
                else
                {
                    errorProvider1.Clear();
                    AddBtn.Enabled = true;
                }
            }

        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            this.ValidateChildren();
            if (thisFunction == FormFunctions.RelativeOffsets)
                XMLSettings.AddFocusOffset(new FocusOffset(nameTB.Text, (int)Offset_NUD.Value));
            else XMLSettings.AddAbsolutePreset(new FocusOffset(nameTB.Text, (int)Offset_NUD.Value));
        }

        private void Offset_NUD_Enter(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            Offset_NUD.Select(0, n.Value.ToString().Length);
        }

        private void AddFilterOffsetForm_Load(object sender, EventArgs e)
        {

        }

        public enum FormFunctions { RelativeOffsets, AbsolutePresets }

    }
}
