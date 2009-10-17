﻿//tabs=4
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
using ASCOM.GeminiTelescope.Properties;

namespace ASCOM.GeminiTelescope
{
    public partial class frmAdvancedSettings : Form
    {
        const string Cap = "Advanced Gemini Settings";

        public frmAdvancedSettings()
        {
            this.UseWaitCursor = true;

            InitializeComponent();

            comboBox1.Items.AddRange(GeminiProperties.Brightness_names);
            comboBox2.Items.AddRange(GeminiProperties.HandController_names);
            comboBox3.Items.AddRange(GeminiProperties.TrackingRate_names);
            comboBox5.Items.AddRange(GeminiProperties.Mount_names);

            GeminiProperties props = new GeminiProperties();

            // read default profile
            if (props.Serialize(false, null))
                this.Text = Cap + " (" + SharedResources.DEAULT_PROFILE + ")";


            if (props.SyncWithGemini(false))    // read all the properties from the mount
                this.Text = Cap + " [settings from Gemini]";

            this.geminiPropertiesBindingSource.Add(props);

            chkSendSettings.Checked = GeminiHardware.SendAdvancedSettings;

            OnConnectChange(true, 1);

            GeminiHardware.OnConnect += new ConnectDelegate(OnConnectChange);

            this.UseWaitCursor = false;
        }

        private void SetCollapsedGroups()
        {
            GeminiHardware.m_Profile.DeviceType = "Telescope";
            foreach (Control gc in this.flowLayoutPanel1.Controls)
            {
                if (gc is Indigo.CollapsibleGroupBox)
                {
                    Indigo.CollapsibleGroupBox gb = gc as Indigo.CollapsibleGroupBox;
                    bool bCollapse = false;

                    string res = GeminiHardware.m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "GroupCollapse_" + gb.Text, "");
                    bool.TryParse(res, out bCollapse);
                    gb.FullSize = gb.Size;
                    gb.IsCollapsed = bCollapse;
                    gb.ForeColor = (bCollapse ? Color.Gray : Color.White);
                }
            }
        }

        void pbButton_EnabledChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c.Enabled)
            {
                if (c.Tag == null) c.Tag = c.BackColor;
                c.BackColor = (Color)(c.Tag);
            }
            else
            {
                c.Tag = c.BackColor;
                c.BackColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void SetControlColor(Control panel)
        {
            foreach (Control c in panel.Controls)
            {
                if (c.BackColor == Color.Transparent || c.BackColor == Color.Black)
                    if (GeminiHardware.Connected)
                    {
                            c.ForeColor = Color.Lime;
                    }
                    else 
                        c.ForeColor = Color.LightGray;
            }

        }

        void OnConnectChange(bool connect, int clients)
        {
            SetControlColor(tableLayoutPanel1);
            SetControlColor(tableLayoutPanel2);
            SetControlColor(tableLayoutPanel3);
            SetControlColor(tableLayoutPanel4);
            SetControlColor(tableLayoutPanel5);
            SetControlColor(this.panel1);

            pbApply.Enabled = GeminiHardware.Connected;
            pbReboot.Enabled = GeminiHardware.Connected;
            pbOK.Enabled = GeminiHardware.Connected;
            pbButton_EnabledChanged(pbApply, null);
            pbButton_EnabledChanged(pbReboot, null);
            pbButton_EnabledChanged(pbOK, null);

            menuItemGetSettings.Enabled = GeminiHardware.Connected;
            menuItemSendSettings.Enabled = GeminiHardware.Connected;

        }


        private void pbApply_Click(object sender, EventArgs e)
        {
            GeminiHardware.Trace.Enter("AdvancedSettings:pbApply_Click");

            if (this.ValidateChildren())
            {

                DialogResult res = MessageBox.Show(Resources.OverwriteSettings, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes) return;

                this.UseWaitCursor = true;

                try
                {
                    GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
                    props.SyncWithGemini(true);    // write all properties from profile to Gemini

                    if (GeminiHardware.SendAdvancedSettings)
                        props.Serialize(true, null);   // save to default profile

                    props.ClearProfile();

                    props.SyncWithGemini(false);   // read all the properties from the mount
                    geminiPropertiesBindingSource.ResetBindings(false);
                    this.Text = Cap + " [settings from Gemini]";
                }
                catch (Exception ex)
                {
                    GeminiHardware.Trace.Except(ex);
                    MessageBox.Show( Resources.NoProfileSave + "\r\n" + ex.Message);
                }
                finally
                {
                    this.UseWaitCursor = false;
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
                {
                    geminiPropertiesBindingSource.ResetBindings(false);
                    this.Text = Cap + " (" + System.IO.Path.GetFileName(openFileDialog.FileName) + ")";

                }
            }
            catch (Exception ex)
            {
                GeminiHardware.Trace.Except(ex);
                MessageBox.Show(Resources.NoProfileLoad+ "\r\n" + ex.Message);
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
                if (props.Serialize(true, saveFileDialog.FileName))     // save profile         
                    this.Text = Cap + " (" + System.IO.Path.GetFileName(saveFileDialog.FileName) + ")";
            }
            catch (Exception ex)
            {
                GeminiHardware.Trace.Except(ex);
                MessageBox.Show(Resources.NoProfileSave + "\r\n" + ex.Message);
            }
            GeminiHardware.Trace.Exit("AdvancedSettings:pbSave_Click");
        }


        private void pbSetSafetyLimit_Click(object sender, EventArgs e)
        {
            pbSetSafetyLimit.ContextMenuStrip.Show(Cursor.Position);
        }


        void pbReboot_Click(object sender, System.EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                DialogResult res = MessageBox.Show(Resources.RebootController, "Reboot Gemini", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Hand);
                if (res == DialogResult.Yes)
                {
                    GeminiHardware.DoCommandResult(">65535:", GeminiHardware.MAX_TIMEOUT, false);
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
                        this.UseWaitCursor = true;
                        GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
                        props.SyncWithGemini(true);
                        if (GeminiHardware.SendAdvancedSettings) props.Serialize(true, null);     // save default profile
                    }
                    catch (Exception ex)
                    {
                        GeminiHardware.Trace.Except(ex);
                    }
                    finally
                    {
                        this.UseWaitCursor = false;
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

        private void SavePEC_CheckedChanged(object sender, EventArgs e)
        {
            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
            props.SavePEC = chkSavePEC.Checked;

        }

        private void menuSetSafetyHere_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                GeminiHardware.DoCommandResult(">220:", GeminiHardware.MAX_TIMEOUT, false);
                MessageBox.Show(Resources.SafetyLimitSet);                
            }
        }

        private void menuSetLimits_Click(object sender, EventArgs e)
        {
            frmSafetyLimits dlg = new frmSafetyLimits((GeminiProperties)this.geminiPropertiesBindingSource[0]);
            DialogResult res = dlg.ShowDialog(this);
        }

        private void g11DefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                GeminiHardware.DoCommandResult(">43690:", GeminiHardware.MAX_TIMEOUT, false);
                MessageBox.Show(Resources.SafetyLimitSet);
            }
        }

        private void mI250DefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                GeminiHardware.DoCommandResult(">43691:", GeminiHardware.MAX_TIMEOUT, false);
                MessageBox.Show(Resources.SafetyLimitSet);
            }

        }

        private void groupBox_CollapseBoxClickedEvent(object sender)
        {
            Indigo.CollapsibleGroupBox gb = (Indigo.CollapsibleGroupBox)sender;
            if (gb.IsCollapsed)
                gb.ForeColor = Color.Gray;
            else
                gb.ForeColor = Color.White;

            GeminiHardware.m_Profile.DeviceType = "Telescope";
            GeminiHardware.m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GroupCollapse_" + gb.Text, gb.IsCollapsed.ToString());
        }

        private void frmAdvancedSettings_Load(object sender, EventArgs e)
        {
            SetCollapsedGroups();
        }

        Control m_ParentPanel = null;

        public void pbUpdate_Click(object sender, EventArgs e)
        {
            Button pb = (Button)sender;
            m_ParentPanel = pb.Parent;
            pb.ContextMenuStrip.Show(pb, new Point(pb.Width, 0));
        }

        /// <summary>
        /// get values for all bound controls on current panel from Gemini:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuItemGetSettings_Click(object sender, EventArgs e)
        {
            List<string> p = GetAllBindings(m_ParentPanel);
            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];
            Cursor.Current = Cursors.WaitCursor;
            props.ClearProfile(p);
            props.SyncWithGemini(false, p);
            geminiPropertiesBindingSource.ResetBindings(false);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// write values for all bound controls on current panel to Gemini:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuItemSendSettings_Click(object sender, EventArgs e)
        {
            List<string> p = GetAllBindings(m_ParentPanel);
            GeminiProperties props = (GeminiProperties)geminiPropertiesBindingSource[0];

            if (this.ValidateChildren())
            {
                Cursor.Current = Cursors.WaitCursor;
                props.SyncWithGemini(true, p);
                props.ClearProfile(p);
                props.SyncWithGemini(false, p);  //fetch properties
                geminiPropertiesBindingSource.ResetBindings(false);
                Cursor.Current = Cursors.Default;
            }
        }

        private List<string> GetAllBindings(Control panel)
        {
            List<string> l = new List<string>();
            foreach (Control c in panel.Controls)
            {
                if (c.DataBindings != null && c.DataBindings.Count > 0)
                    l.Add(c.DataBindings[0].BindingMemberInfo.BindingMember);
                else
                {   // some controls may have multiple binding properties associated with them
                    // these will be listed in comma-separated format as a string, add all
                    // of them to the list:
                    if (c.Tag != null)
                    {
                        string[] props = ((string)c.Tag).Split(new char[] { ',' });
                        l.AddRange(props);
                    }
                }
            }
            return l;
        }
    }
}
