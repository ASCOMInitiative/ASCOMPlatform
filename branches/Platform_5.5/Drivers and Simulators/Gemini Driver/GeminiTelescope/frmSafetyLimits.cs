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
using ASCOM.GeminiTelescope.Properties;

namespace ASCOM.GeminiTelescope
{
    public partial class frmSafetyLimits : Form
    {
        SerializableDictionary<string, object> mBkupProfile;
        const int LosmandyEastDefault = 114;
        const int G11EastConservative = 98;    //safer value for G11
        const int LosmandyWestDefault = 123;
        const int MI250EastDefault = 92;
        const int MI250WestDefault = 95;

        public frmSafetyLimits(GeminiProperties props)
        {

            InitializeComponent();
            // copy current profile, in case we need to restore it on Cancel:
            mBkupProfile = (SerializableDictionary<string, object>)props.Profile.Clone();

            geminiPropertiesBindingSource.Add(props);
            geminiPropertiesBindingSource.ResetBindings(false);
            SetControlColor(this.groupBox1);
            SetControlColor(this.groupBox2);
            SetControlColor(this.groupBox3);
            chkNudge.Checked = GeminiHardware.Instance.NudgeFromSafety;

            GeminiHardware.Instance.OnConnect += new ConnectDelegate(OnConnectChange);
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
                    if (GeminiHardware.Instance.Connected)
                        c.ForeColor = Color.Lime;
                    else
                        c.ForeColor = Color.LightGray;
            }

        }

        private void pbOK_Click(object sender, EventArgs e)
        {
            string losmandy_mounts = "GM-8,G-11,HGM-200,Titan,Titan50";
            if (ValidateChildren())
            {
                GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
                if ((losmandy_mounts.Contains(props.MountTypeSetting) && 
                        (numericUpDown1.Value > LosmandyEastDefault|| numericUpDown2.Value > LosmandyWestDefault)) ||
                    (props.MountTypeSetting == "MI-250" && 
                        (numericUpDown1.Value > MI250EastDefault || numericUpDown2.Value > MI250WestDefault)))
                {
                    DialogResult res = MessageBox.Show(Resources.SafetyLimitsAggressive,
                        Resources.SafetyLimitWarning, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (res != DialogResult.Yes)
                    {
                        DialogResult = DialogResult.None;
                        return;
                    }
                }

                GeminiHardware.Instance.NudgeFromSafety = chkNudge.Checked;
                DialogResult = DialogResult.OK;
            }
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

            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];

            if (props.MountTypeSetting == "G-11")
            {
                DialogResult res = MessageBox.Show(Resources.GeminiG11DefaultSafetyLimitWarning,
                    Resources.SafetyLimitWarning, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    numericUpDown1.Value = G11EastConservative;
                    numericUpDown2.Value = LosmandyWestDefault;
                    return;
                }
                else if (res == DialogResult.Cancel) return;    
            }

            numericUpDown1.Value = LosmandyEastDefault;
            numericUpDown2.Value = LosmandyWestDefault;
        }

        private void MI250_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = MI250EastDefault;
            numericUpDown2.Value = MI250WestDefault;
        }

        private void chkFlip_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkNudge_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNudge.Checked)
            {
                DialogResult res = MessageBox.Show("This feature bypasses the built-in Gemini slew-disabling mechanism when at safety limit. Do you really want to do this?", SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (res != DialogResult.Yes)
                    chkNudge.Checked = false;
            }
        }
    }
}
