//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	This is the set up form for the Video Driver Simulator 
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.Simulator.Config;
using ASCOM.Simulator.Properties;

namespace ASCOM.Simulator
{
    public partial class frmSetupDialog : Form, ISettingsPagesManager
    {
        private int m_CurrentPropertyPageId = -1;
        private Dictionary<int, SettingsPannel> m_PropertyPages = new Dictionary<int, SettingsPannel>();

        private SettingsPannel m_CurrentPanel = null;

        private ucGeneral m_ucGeneral;
        private ucVideoSource m_ucVideoSource;
        private ucAnalogueCameraSettings m_ucAnalogueCameraSettings;
        private ucIntegratingCameraSettings m_ucIntegratingCameraSettings;
        private ucGamma m_ucGamma;
        private ucGain m_ucGain;

        public frmSetupDialog()
        {
            InitializeComponent();

            InitAllPropertyPages();
        }

        private void InitAllPropertyPages()
        {
            m_ucVideoSource = new ucVideoSource();
            m_ucAnalogueCameraSettings = new ucAnalogueCameraSettings();
            m_ucIntegratingCameraSettings = new ucIntegratingCameraSettings();
            m_ucGamma = new ucGamma();
            m_ucGain = new ucGain();
            m_ucGeneral = new ucGeneral(this);

            m_PropertyPages.Add(0, m_ucGeneral);
            m_PropertyPages.Add(1, m_ucVideoSource);
            m_PropertyPages.Add(2, m_ucAnalogueCameraSettings);
            m_PropertyPages.Add(3, m_ucIntegratingCameraSettings);
            m_PropertyPages.Add(4, m_ucGamma);
            m_PropertyPages.Add(5, m_ucGain);
        }

        void ISettingsPagesManager.CameraTypeChanged(SimulatedCameraType cameraType)
        {
            CameraTypeChangedInternal(cameraType);
        }

        private void CameraTypeChangedInternal(SimulatedCameraType cameraType)
        {
            if (cameraType == SimulatedCameraType.AnalogueIntegrating || cameraType == SimulatedCameraType.AnalogueNonIntegrating)
            {
                m_ucAnalogueCameraSettings.Enabled = true;
                m_ucAnalogueCameraSettings.BackColor = Color.Black;
            }
            else
            {
                m_ucAnalogueCameraSettings.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
                m_ucAnalogueCameraSettings.Enabled = false;
            }
            if (cameraType != SimulatedCameraType.AnalogueNonIntegrating)
            {
                m_ucIntegratingCameraSettings.Enabled = true;
                m_ucIntegratingCameraSettings.BackColor = Color.Black;
            }
            else
            {
                m_ucIntegratingCameraSettings.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
                m_ucIntegratingCameraSettings.Enabled = false;
            }
        }

        private void SetFormTitle(TreeNode currentNode)
        {
            if (currentNode != null)
            {
                string newTitle;

                if (currentNode.Parent == null)
                {
                    // Select the first sibling
                    if (currentNode.Nodes.Count > 0)
                        newTitle = string.Format("Video Simulator Settings - {0} - {1}", currentNode.Text, currentNode.Nodes[0].Text);
                    else
                        newTitle = string.Format("Video Simulator Settings - {0}", currentNode.Text);
                }
                else
                    newTitle = string.Format("Video Simulator Settings - {0} - {1}", currentNode.Parent.Text, currentNode.Text);

                this.Text = newTitle;
            }
        }


        void tvSettings_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            e.Cancel = m_CurrentPanel != null && !m_CurrentPanel.ValidateSettings();
        }

        private void tvSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                int propPageId = int.Parse((string)e.Node.Tag);

                if (m_CurrentPropertyPageId != propPageId)
                {
                    SettingsPannel propPage = null;
                    if (m_PropertyPages.TryGetValue(propPageId, out propPage))
                    {
                        LoadPropertyPage(propPage);
                        m_CurrentPanel = propPage;
                        m_CurrentPropertyPageId = propPageId;
                        SetFormTitle(e.Node);
                    }
                }

                if (e.Node.Nodes.Count > 0)
                    e.Node.Expand();
            }
        }

        private void LoadPropertyPage(Control propPage)
        {
            if (pnlPropertyPage.Controls.Count == 1)
                pnlPropertyPage.Controls.Remove(pnlPropertyPage.Controls[0]);

            if (propPage != null)
            {
                pnlPropertyPage.Controls.Add(propPage);
                propPage.Dock = DockStyle.Fill;
            }
        }

        private void frmSetupDialog_Load(object sender, EventArgs e)
        {
            LoadSettings();

            tvSettings.SelectedNode = tvSettings.Nodes[0];
            lblVersion.Text = string.Format("ver. {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_CurrentPanel != null)
            {
                if (m_CurrentPanel.ValidateSettings())
                {
                    foreach (SettingsPannel panel in m_PropertyPages.Values)
                    {
                        if (panel.ValidateSettings())
                            panel.SaveSettings();
                        else
                        {
                            m_CurrentPanel = panel;
                            LoadPropertyPage(m_CurrentPanel);
                            return;
                        }
                    }
                }
                else
                    return;
            }

            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void LoadSettings()
        {
            foreach (SettingsPannel panel in m_PropertyPages.Values)
                panel.LoadSettings();

            CameraTypeChangedInternal(Settings.Default.CameraType);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e)
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            var frmReset = new frmResetSettings();
            if (frmReset.ShowDialog(this) == DialogResult.OK)
            {
                LoadSettings();
            }
        }
    }
}