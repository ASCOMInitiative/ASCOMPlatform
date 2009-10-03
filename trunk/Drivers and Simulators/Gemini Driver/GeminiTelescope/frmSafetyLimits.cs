//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Safety Limit editing/display form
//
// Description:	This implements editing of safety limit parameters for the current profile
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 02-OCT-2009  pk  1.0.0   Initial implementation
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class frmSafetyLimits : Form
    {
        SerializableDictionary<string, object> mBkupProfile;

        public frmSafetyLimits(GeminiProperties props)
        {

            InitializeComponent();
            // copy current profile, in case we need to restore it on Cancel:
            mBkupProfile = (SerializableDictionary<string, object>)props.Profile.Clone();

            geminiPropertiesBindingSource.Add(props);
            geminiPropertiesBindingSource.ResetBindings(false);
            SetControlColor(this.groupBox1);
            SetControlColor(this.groupBox2);
            GeminiHardware.OnConnect += new ConnectDelegate(OnConnectChange);
        }

        private void frmSafetyLimits_Load(object sender, EventArgs e)
        {

        }

        void OnConnectChange(bool connect, int clients)
        {
            SetControlColor(this.groupBox1);
            SetControlColor(this.groupBox2);
        }

        private void SetControlColor(Control panel)
        {
            foreach (Control c in panel.Controls)
            {
                if (c.BackColor == Color.Transparent || c.BackColor == Color.Black)
                    if (GeminiHardware.Connected)
                        c.ForeColor = Color.Lime;
                    else
                        c.ForeColor = Color.LightGray;
            }

        }

        private void pbOK_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
                DialogResult = DialogResult.OK;
        }

        private void frmSafetyLimits_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
            if (DialogResult != DialogResult.OK)
            {
                props.Profile = mBkupProfile;
                DialogResult = DialogResult.Cancel;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
            props.Profile = mBkupProfile;
            DialogResult = DialogResult.Cancel;
        }

        private void defaultGotoLimit_Click(object sender, EventArgs e)
        {
            numericUpDown4.Value = 0.00M;
        }

        private void LosmandyDefault_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 114M;
            numericUpDown2.Value = 123M;
        }

        private void MI250_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 92M;
            numericUpDown2.Value = 95M;
        }
    }
}
