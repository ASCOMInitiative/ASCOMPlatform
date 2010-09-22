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
    public partial class AddFilterOffsetForm : Form
    {

        public AddFilterOffsetForm()
        {
            InitializeComponent();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            TextBox t = sender as TextBox;
            string name = t.Text;
            if (!XMLSettings.CheckOffsetNameUnique(name))
            {
                errorProvider1.SetError(t, "An offset with the name " + name + " already exists. " +
                    "Please choose a different name.");
                AddBtn.Enabled = false;
            }
            else AddBtn.Enabled = true;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            XMLSettings.AddFocusOffset(new FocusOffset(nameTB.Text, (int)Offset_NUD.Value));
        }

        private void Offset_NUD_Enter(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            Offset_NUD.Select(0, n.Value.ToString().Length);
        }

        private void AddFilterOffsetForm_Load(object sender, EventArgs e)
        {

        }


    }
}
