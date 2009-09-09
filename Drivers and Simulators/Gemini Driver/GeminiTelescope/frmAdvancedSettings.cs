//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Advanced Settings form
//
// Description:	This implements advanced settings display and editing form
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 08-SEP-2009  pk  1.0.0   Added full complement of modeling, custom mount, and other Gemini property
//                          Implemented profile saving and editing
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
    public partial class frmAdvancedSettings : Form
    {
        public frmAdvancedSettings()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(GeminiProperties.Brightness_names);
            comboBox2.Items.AddRange(GeminiProperties.HandController_names);
            comboBox3.Items.AddRange(GeminiProperties.TrackingRate_names);
            comboBox5.Items.AddRange(GeminiProperties.Mount_names);

            GeminiProperties props = new GeminiProperties();

            // read default profile
            props.Serialize(false, null);

            
            props.SyncWithGemini(false);    // read all the properties from the mount

            this.geminiPropertiesBindingSource.Add(props);

            chkSendSettings.Checked = GeminiHardware.SendAdvancedSettings;
        }

        private void pbApply_Click(object sender, EventArgs e)
        {
            GeminiHardware.Trace.Enter("AdvancedSettings:pbApply_Click");

            if (this.ValidateChildren())
            {
                try
                {
                    GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
                    props.SyncWithGemini(true);    // read all the properties from the mount

                    if (GeminiHardware.SendAdvancedSettings) 
                        props.Serialize(true, null);   // save to default profile

                    props.SyncWithGemini(false);   // read all the properties from the mount
                    geminiPropertiesBindingSource.ResetBindings(false);
                }
                catch (Exception ex)
                {
                    GeminiHardware.Trace.Except(ex);
                    MessageBox.Show("Unable to save profile:\r\n" + ex.Message);
                }
            }
            GeminiHardware.Trace.Exit("AdvancedSettings:pbApply_Click");
        }

        private void pbLoad_Click(object sender, EventArgs e)
        {
            GeminiHardware.Trace.Enter("AdvancedSettings:pbLoad_Click");
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME;
                System.IO.Directory.CreateDirectory(path);

                openFileDialog.RestoreDirectory = true;
                openFileDialog.InitialDirectory = path;
                openFileDialog.FileName = SharedResources.DEAULT_PROFILE;

                DialogResult res = openFileDialog.ShowDialog(this);
                if (res != DialogResult.OK) return;
                GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
                if (props.Serialize(false, openFileDialog.FileName)) // read profile
                    geminiPropertiesBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                GeminiHardware.Trace.Except(ex);
                MessageBox.Show("Unable to load profile:\r\n" + ex.Message);
            }
            GeminiHardware.Trace.Enter("AdvancedSettings:pbSave_Click");
                 
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            GeminiHardware.Trace.Enter("AdvancedSettings:pbSave_Click");

            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME;
                System.IO.Directory.CreateDirectory(path);

                saveFileDialog.FileName = SharedResources.DEAULT_PROFILE;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.InitialDirectory = path;
                DialogResult res = saveFileDialog.ShowDialog(this);
                if (res != DialogResult.OK) return;
                GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
                props.Serialize(true, saveFileDialog.FileName);     // save profile         
            }
            catch (Exception ex)
            {
                GeminiHardware.Trace.Except(ex);
                MessageBox.Show("Unable to save profile:\r\n" + ex.Message);
            }
            GeminiHardware.Trace.Exit("AdvancedSettings:pbSave_Click");
        }


        private void pbSetSafetyLimit_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                GeminiHardware.DoCommandResult(">220:", GeminiHardware.MAX_TIMEOUT, false);
                MessageBox.Show("Safety Limit set to current position");
            }
        }


        void pbReboot_Click(object sender, System.EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                DialogResult res = MessageBox.Show("Are you sure you want to reboot Gemini controller?", "Reboot Gemini", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Hand);
                if (res == DialogResult.Yes)
                {
                    GeminiHardware.DoCommandResult(">65535:", GeminiHardware.MAX_TIMEOUT, false);
                    GeminiHardware.Resync();
                }
            }
        }

        private void pbOK_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                if (GeminiHardware.Connected)
                {
                    try
                    {
                        GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
                        props.SyncWithGemini(true);
                        if (GeminiHardware.SendAdvancedSettings) props.Serialize(true, null);     // save default profile
                    }
                    catch (Exception ex)
                    {
                        GeminiHardware.Trace.Except(ex);
                    }
                }
            }
        }

        private void pbModel_Click(object sender, EventArgs e)
        {
            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
            frmModel dlg = new frmModel(props);
            DialogResult res = dlg.ShowDialog(this);
        }

        private void chkSendSettings_CheckedChanged(object sender, EventArgs e)
        {
            GeminiHardware.SendAdvancedSettings = chkSendSettings.Checked;
        }
    }
}
