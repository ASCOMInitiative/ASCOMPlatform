using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Net35CompopnentUseForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public Net35CompopnentUseForm()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnCreateReport_Click(object sender, EventArgs e)
        {
            ASCOM.Tools.TraceLogger TL= new ("Net35CompopnentUseReport", true);
            TL.LogMessage("Initialise", "Creating report for .NET 3.5 component use");

            using RegistryKey profileKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@$"{Global.REGISTRY_ROOT_KEY_NAME}\{Global.NET35_REGISTRY_BASE}");
            {
                TL.LogMessage("Initialise", "Created registry key");

                if (profileKey != null)
                {
                    TL.LogMessage("Initialise", "Registry key found");
                    string[] componentList = profileKey.GetSubKeyNames();
                    TL.LogMessage("Initialise", $"Found {componentList.Length} executables");
                    foreach (var component in componentList)
                    {
                        TL.LogMessage("Executable", component);
                    }
                }
                else
                {
                    TL.LogMessage("Initialise", "No registry key found for .NET 3.5 components");
                }
                MessageBox.Show("Report created in the ASCOM Trace Logger folder", "Report Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
