using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Form to manage and report on the use of .NET 3.5 components within ASCOM.
    /// Allows enabling/disabling logging, clearing log data, viewing registry entries, and creating usage reports.
    /// </summary>
    public partial class Net35CompopnentUseForm : Form
    {
        static bool logDetail = false;

        // Store the current enabled/disabled state of .NET 3.5 component use logging
        bool enabledState;

        /// <summary>
        /// Initializes a new instance of the Net35CompopnentUseForm class.
        /// Loads the current logging state from global settings.
        /// </summary>
        public Net35CompopnentUseForm()
        {
            InitializeComponent();
            enabledState = Global.GetBool(Global.DOTNET35_COMPONENT_USE_LOGGING, Global.DOTNET35_COMPONENT_USE_LOGGING_DEFAULT);
        }

        /// <summary>
        /// Handles the Close button click event. Closes the form.
        /// </summary>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the form load event. Sets the radio buttons to reflect the current logging state.
        /// </summary>
        private void Net35CompopnentUseForm_Load(object sender, EventArgs e)
        {
            RadOff.Checked = !enabledState;
            RadEnabled.Checked = enabledState;
        }

        /// <summary>
        /// Handles the "Off" radio button checked change event.
        /// Disables .NET 3.5 component use logging in global settings.
        /// </summary>
        private void RadOff_CheckedChanged(object sender, EventArgs e)
        {
            Global.SetName(Global.DOTNET35_COMPONENT_USE_LOGGING, "False");
        }

        /// <summary>
        /// Handles the "Enabled" radio button checked change event.
        /// Enables .NET 3.5 component use logging in global settings.
        /// </summary>
        private void RadEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Global.SetName(Global.DOTNET35_COMPONENT_USE_LOGGING, "True");
        }

        /// <summary>
        /// Handles the Clear Data button click event.
        /// Prompts the user for confirmation and deletes all .NET 3.5 component use log data from the registry.
        /// </summary>
        private void BtnClearData_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all .NET 3.5 component use data?", "Clear Data Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Delete any log entries
                try
                {
                    using (RegistryAccess ra = new())
                    {
                        ra.DeleteKey(Global.NET35_REGISTRY_BASE);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception: {ex.Message}", "Exception Clearing Log Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handles the View Regedit button click event.
        /// Opens the Windows Registry Editor at the .NET 3.5 component use registry location.
        /// </summary>
        private void BtnViewRegedit_Click(object sender, EventArgs e)
        {
            // Set the logging registry location based on the OS architecture
            string registryLocation = Environment.Is64BitOperatingSystem ? @$"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ASCOM\{Global.NET35_REGISTRY_BASE}" : @$"HKEY_LOCAL_MACHINE\SOFTWARE\ASCOM\{Global.NET35_REGISTRY_BASE}";
            string registryLastKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit";

            // Set the key that regedit navigates to when it starts
            try
            {
                Registry.SetValue(registryLastKey, "LastKey", registryLocation); // Set LastKey value that regedit will go directly to
                Process.Start("regedit.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Starting Regedit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void LogMessage(string key, string message, ASCOM.Tools.TraceLogger TL)
        {
            // Log a message to the TraceLogger
            if (logDetail)
                TL.LogMessage(key, message);
        }

        /// <summary>
        /// Handles the Create Report button click event.
        /// Generates a report of all .NET 3.5 component use entries found in the registry and logs them using TraceLogger.
        /// </summary>
        private void BtnCreateReport_Click(object sender, EventArgs e)
        {
            try
            {
                List<Net35Use> net35UseList = new();

                ASCOM.Tools.TraceLogger TL = new("Net35CompopnentUseReport", true);

                using RegistryKey profileKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@$"{Global.REGISTRY_ROOT_KEY_NAME}\{Global.NET35_REGISTRY_BASE}");
                {
                    LogMessage("Initialise", "Created registry key", TL);

                    if (profileKey != null)
                    {
                        LogMessage("Initialise", "Registry key found", TL);
                        string[] categoryList = profileKey.GetSubKeyNames();
                        LogMessage("Initialise", $"Found {categoryList.Length} categories", TL);
                        foreach (var category in categoryList)
                        {
                            LogMessage("Category", category, TL);
                            string[] executables = profileKey.OpenSubKey(category).GetSubKeyNames();
                            LogMessage("Executables", $"Found {executables.Length} executables", TL);

                            foreach (string executable in executables)
                            {
                                LogMessage("Name", $"Found name {executable}", TL);
                                using RegistryKey executableKey = profileKey.OpenSubKey(category).OpenSubKey(executable);
                                if (executableKey != null)
                                {
                                    // Get the .NET version from the Framework key
                                    string dotNetVersion = executableKey.GetValue("Framework")?.ToString() ?? "Unknown";

                                    // Get the consumed assemblies for this executable
                                    string[] assemblies = profileKey.OpenSubKey(category).OpenSubKey(executable).GetSubKeyNames();
                                    LogMessage("Assemblies", $"Found {assemblies.Length} assemblies", TL);

                                    foreach (string assembly in assemblies)
                                    {
                                        LogMessage("Assembly", $"Found assembly {assembly}", TL);
                                        using RegistryKey assemblyKey = profileKey.OpenSubKey(category).OpenSubKey(executable).OpenSubKey(assembly);
                                        if (assemblyKey != null)
                                        {

                                            string[] components = profileKey.OpenSubKey(category).OpenSubKey(executable).OpenSubKey(assembly).GetValueNames();
                                            LogMessage("Components", $"Found {components.Length} components", TL);
                                            foreach (string component in components)
                                            {
                                                string value = assemblyKey.GetValue(component)?.ToString() ?? "No Value";
                                                LogMessage("Value", $"Component {component} has value: {value}", TL);
                                                net35UseList.Add(new Net35Use
                                                {
                                                    Category = category,
                                                    Name = executable,
                                                    Assembly = assembly,
                                                    Component = component,
                                                    DotNetVersion = dotNetVersion
                                                });
                                            }
                                        }
                                        else
                                        {
                                            LogMessage("Assemblies", $"No data found for {assemblyKey}", TL);
                                        }
                                    }
                                }
                                else
                                {
                                    LogMessage("Names", $"No data found for {executable}", TL);
                                }
                            }
                        }
                    }
                    else
                    {
                        LogMessage("Initialise", "No registry key found for .NET 3.5 components", TL);
                    }
                }

                // Report by application and CLR version
                List<Net35Use> sortedListClrVersion = net35UseList.OrderBy(n => n.DotNetVersion).ToList();
                var grouplistClrVersion = sortedListClrVersion.GroupBy(n => n.DotNetVersion);
                foreach (var groupClrVersion in grouplistClrVersion)
                {
                    string message = $".NET 3.5 Component Use Ordered by Application/Driver - Running under .NET {groupClrVersion.Key}";
                    TL.LogMessage(message, "");
                    TL.LogMessage(new string('=', message.Length), "");
                    TL.LogMessage("", "");

                    var sortedClrList = groupClrVersion.OrderBy(n => n.Name).ToList();
                    var groupClrlist = sortedClrList.GroupBy(n => n.Name);
                    foreach (var group in groupClrlist)
                    {
                        TL.LogMessage("Executable", $"{group.Key} uses the following components:");
                        foreach (var item in group)
                        {
                            TL.LogMessage("Component", $"  {item.Component}");
                        }
                        TL.LogMessage("", "");
                    }
                }
                TL.LogMessage("", "");

                // Report by component
                TL.LogMessage(".NET 3.5 Component Use Ordered by Component", "");
                TL.LogMessage("===========================================", "");
                TL.LogMessage("", "");

                List<Net35Use> sortedList = net35UseList.OrderBy(n => n.Component).ToList();
                var grouplist = sortedList.GroupBy(n => n.Component);
                foreach (var group in grouplist)
                {
                    TL.LogMessage("Component", $"{group.Key} is used by the following executables:");
                    foreach (var item in group)
                    {
                        TL.LogMessage("Executable", $"  {item.Name}");
                    }
                    TL.LogMessage("", "");
                }

                // Save the report log file name so it can be displayed
                string logFileName = Path.Combine(TL.LogFilePath, TL.LogFileName);

                if (!string.IsNullOrEmpty(logFileName))
                {
                    try
                    {
                        new Process
                        {
                            StartInfo = new ProcessStartInfo(logFileName)
                            {
                                UseShellExecute = true
                            }
                        }.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No report file available. Please create a report first.", "No Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex}", "Diagnostics report exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
