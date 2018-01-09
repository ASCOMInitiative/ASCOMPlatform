//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Voice Announcer configuration form
//
// Description:	This implements editing of voice settings 
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 26-SEP-2009  pk  1.0.0   Initial implementation
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
    public partial class frmVoice: Form
    {
        private string m_Voice;

        public string Voice
        {
            get { return m_Voice; }
            set { 
                m_Voice = value;
                int idx = cmbVoices.FindStringExact(value);
                cmbVoices.SelectedIndex = idx;
            }
        }

        private Speech.SpeechType m_Flags;

        internal Speech.SpeechType Flags
        {
            get { return m_Flags; }
            set { 
                m_Flags = value;
                chkErrors.Checked = ((m_Flags & Speech.SpeechType.Error) != 0);
                this.chkNotify.Checked = ((m_Flags & Speech.SpeechType.Announcement) != 0);
                this.chkCommand.Checked = ((m_Flags & Speech.SpeechType.Command) != 0);
                this.chkStatus.Checked = ((m_Flags & Speech.SpeechType.Status) != 0);            
            }
        }


        public frmVoice()
        {
            InitializeComponent();
            string[] vs = Speech.Voices;
            foreach (string s in vs)
                cmbVoices.Items.Add(s);
        }

        private void pbOK_Click(object sender, EventArgs e)
        {
            Speech.SpeechType flags = 0;

            if (chkCommand.Checked) flags |= Speech.SpeechType.Command;
            if (chkErrors.Checked) flags |= Speech.SpeechType.Error;
            if (chkNotify.Checked) flags |= Speech.SpeechType.Announcement;
            if (chkStatus.Checked) flags |= Speech.SpeechType.Status;

            m_Flags = flags;
            m_Voice = (string)(cmbVoices.SelectedIndex>=0? cmbVoices.SelectedItem : "");
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
        }

        private void frmVoice_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cmbVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = (string)cmbVoices.SelectedItem;
            if (Speech.SpeechInitialize(this.Handle, s))
                if (!Speech.SayIt(s, Speech.SpeechType.Always))
                    MessageBox.Show(Resources.CannotUseVoice, SharedResources.TELESCOPE_DRIVER_NAME);
        }

        private void frmVoice_Load(object sender, EventArgs e)
        {

        }


    }
}
