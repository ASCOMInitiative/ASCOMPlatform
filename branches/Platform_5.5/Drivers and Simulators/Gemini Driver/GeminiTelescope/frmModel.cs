//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Modeling Parameters editing/display form
//
// Description:	This implements editing of modeling parameters for the current profile
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 08-SEP-2009  pk  1.0.0   Initial implementation
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
    public partial class frmModel : Form
    {
        SerializableDictionary<string, object> mBkupProfile;

        public frmModel(GeminiProperties props)
        {
            InitializeComponent();

            // copy current profile, in case we need to restore it on Cancel:
            mBkupProfile = (SerializableDictionary<string, object>)props.Profile.Clone();

            geminiPropertiesBindingSource.Add(props);
            geminiPropertiesBindingSource.ResetBindings(false);

            GeminiHardware.Instance.OnConnect += new ConnectDelegate(OnConnectChange);

            OnConnectChange(false, 1);

        }

        void OnConnectChange(bool connect, int clients)
        {
            SetControlColor(tableLayoutPanel1);
            menuItemGetSettings.Enabled = GeminiHardware.Instance.Connected;
            menuItemSendSettings.Enabled = GeminiHardware.Instance.Connected;
        }

        private void pbOK_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
                DialogResult = DialogResult.OK;
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
            props.Profile = mBkupProfile;
            DialogResult = DialogResult.Cancel;
        }

        private void frmModel_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
            if (DialogResult != DialogResult.OK)
            {
                props.Profile = mBkupProfile;
                DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Clear all -- reset all modeling parameters to zero in the current profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is NumericUpDown)
                {
                    ((NumericUpDown)c).Value = 0;
                }
            }
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

        private void pbUpdate_Click(object sender, EventArgs e)
        {
            frmAdvancedSettings par = (frmAdvancedSettings)this.Owner;
            par.pbUpdate_Click(sender, e);
        }

        private void menuItemGetSettings_Click(object sender, EventArgs e)
        {
            frmAdvancedSettings par = (frmAdvancedSettings)this.Owner;
            par.menuItemGetSettings_Click(sender, e);
            geminiPropertiesBindingSource.ResetBindings(false);
        }

        private void menuItemSendSettings_Click(object sender, EventArgs e)
        {
            frmAdvancedSettings par = (frmAdvancedSettings)this.Owner;
            par.menuItemSendSettings_Click(sender, e);
            geminiPropertiesBindingSource.ResetBindings(false);
        }

        private void frmModel_Load(object sender, EventArgs e)
        {

        }

    }
}
