using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace RemoveASCOM
{

    public partial class Form1
    {
        private TraceLogger TL;
        private bool PlatformRemoved;
        private string CacheDirectory;
        private string UninstallDirectory;

        // Constants for use with SHGetSpecialFolderPath
        private const int CSIDL_COMMON_STARTMENU = 22; // 0x0016  
        private const int CSIDL_COMMON_DESKTOPDIRECTORY = 25; // 0x0019; 
        private const int CSIDL_PROGRAM_FILES = 38;    // 0x0026 
        private const int CSIDL_PROGRAM_FILESX86 = 42;    // 0x002a
        private const int CSIDL_WINDOWS = 36;    // 0x0024
        private const int CSIDL_SYSTEM = 37;    // 0x0025,
        private const int CSIDL_SYSTEMX86 = 41;    // 0x0029,
        private const int CSIDL_PROGRAM_FILES_COMMON = 43; // 0x002b 
        private const int CSIDL_PROGRAM_FILES_COMMONX86 = 44;    // 0x002c

        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetSpecialFolderPath([In()] IntPtr hwndOwner, StringBuilder lpszPath, [In()] int nFolder, [In()] int fCreate);

        private const string PLATFORM6_INSTALL_KEY = "{8961E141-B307-4882-ABAD-77A3E76A40C1}";
        private const string PLATFORM_INSTALLER_FILENAME_BASE = "ASCOMPlatform";

        private const string REMOVE_INSTALLER_COMBO_TEXT = "Platform and Installer only (Recommended)";
        private Color REMOVE_INSTALLER_BACK_COLOUR = Color.Yellow;
        private Color REMOVE_INSTALLER_FORE_COLOUR = Color.Black;
        private const string REMOVE_INSTALLER_TEXT = Constants.vbCrLf + "WARNING!" + Constants.vbCrLf + Constants.vbCrLf + "This option will remove the ASCOM Platform and its installer." + Constants.vbCrLf + Constants.vbCrLf + "If unsuccessful, use the \"" + REMOVE_ALL_COMBO_TEXT + "\" option as a last resort";
        private const string REMOVE_INSTALLER_CONFIRMATION_MESSAGE = "Are you sure you want to remove your ASCOM Platform?";

        private const string REMOVE_ALL_COMBO_TEXT = "Platform, Installer, Profile and 3rd Party Drivers";
        private Color REMOVE_ALL_BACK_COLOUR = Color.Red;
        private Color REMOVE_ALL_FORE_COLOUR = Color.White;
        private const string REMOVE_ALL_TEXT = Constants.vbCrLf + "WARNING!" + Constants.vbCrLf + Constants.vbCrLf + "This option will forcibly remove your entire ASCOM Platform including your drivers and Profile." + Constants.vbCrLf + Constants.vbCrLf + "Please use it only as a last resort.";
        private const string REMOVE_ALL_CONFIRMATION_MESSAGE = "Are you sure you want to FORCE remove your entire ASCOM Platform, Profile and 3rd Party drivers?";
        private const string REMOVAL_COMPLETE_MESSAGE = "The current Platform has been removed, press OK to end this program.";

        private const string ASCOM_TARGET_DIRECTORY_PLATFORM = @"\ASCOM\Platform 7";
        private const string ASCOM_TARGET_DIRECTORY_DEVELOPER = @"\ASCOM\Platform 7 Developer Components";

        #region Event handlers

        /// <summary>
    /// Update colours and text when the type of removal is changed
    /// </summary>
    /// <param name="sender">Object creating the event</param>
    /// <param name="e">Event arguments</param>
    /// <remarks></remarks>
        private void cmbRemoveMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbRemoveMode.SelectedItem)
            {
                case REMOVE_INSTALLER_COMBO_TEXT:
                    {
                        txtWarning.BackColor = REMOVE_INSTALLER_BACK_COLOUR;
                        txtWarning.ForeColor = REMOVE_INSTALLER_FORE_COLOUR;
                        txtWarning.Text = REMOVE_INSTALLER_TEXT;
                        break;
                    }
                case REMOVE_ALL_COMBO_TEXT:
                    {
                        txtWarning.BackColor = REMOVE_ALL_BACK_COLOUR;
                        txtWarning.ForeColor = REMOVE_ALL_FORE_COLOUR;
                        txtWarning.Text = REMOVE_ALL_TEXT;
                        break;
                    }

                default:
                    {
                        Interaction.MsgBox("Unrecognised cmbRemoveMode value: " + cmbRemoveMode.SelectedItem.ToString(), MsgBoxStyle.Critical);
                        break;
                    }
            }
        }

        /// <summary>
    /// Effect Platform removal
    /// </summary>
    /// <param name="sender">Object creating the event</param>
    /// <param name="e">Event arguments</param>
    /// <remarks></remarks>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            TopLevelRemovalScript(); // Run the overall uninstallation script

            if (PlatformRemoved) // We did remove the Platform so display a message and close this program so that the new installer can continue
            {
                if (string.IsNullOrEmpty(CacheDirectory))
                    MessageBox.Show(REMOVAL_COMPLETE_MESSAGE, "RemoveASCOM", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show this final message if running stand alone, otherwise leave it to the message in the IA installer
                Environment.Exit(0);
            }
        }

        /// <summary>
    /// Form load event handler
    /// </summary>
    /// <param name="sender">Object creating the event</param>
    /// <param name="e">Event arguments</param>
    /// <remarks></remarks>
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] arguments;

            try
            {
                TL = new TraceLogger("", "ForceRemove");
                TL.Enabled = true;
                TL.LogMessage("ForceRemove", string.Format("Program started on {0}", DateTime.Now.ToLongDateString()));
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("TraceLogger Load Exception: " + ex.ToString());
            }

            try
            {
                // Clear the update fields
                LblAction.Text = "";
                lblResult.Text = "";
                TL.LogMessage("ForceRemove", "Update fields cleared");

                // Initialise the removal options drop-down combo-box
                cmbRemoveMode.Items.Clear();
                cmbRemoveMode.Items.Add(REMOVE_INSTALLER_COMBO_TEXT);
                cmbRemoveMode.Items.Add(REMOVE_ALL_COMBO_TEXT);
                cmbRemoveMode.SelectedItem = REMOVE_INSTALLER_COMBO_TEXT; // This triggers a cmbRemoveMode_SelectedItemChanged event that paints the correct colours and text for the warning text box.
                TL.LogMessage("ForceRemove", "Removal options combo box populated OK");
                TL.LogMessage("ForceRemove", "Form loaded OK");

                arguments = Environment.GetCommandLineArgs(); // Get the command line arguments as an array, the 0th element is the name of this executable, the 1st element will be an InstallAware feature code
                if (arguments.Length > 1) // Assume we have been given an additional path to delete. Used to remove the installer cache stored in ProgramData
                {
                    CacheDirectory = arguments[1];
                    TL.LogMessage("ForceRemove", string.Format("Cache directory to be deleted: {0}", CacheDirectory));
                }
                else
                {
                    TL.LogMessage("ForceRemove", string.Format("No Cache directory to be deleted parameter was supplied"));
                }
            }


            catch (Exception ex)
            {
                TL.LogMessageCrLf("Form Load Exception", ex.ToString());
                throw;
            }

            // Initialise Platform removed variable and set default return code
            PlatformRemoved = false;
            Environment.ExitCode = 99;
        }

        /// <summary>
    /// Form close event handler
    /// </summary>
    /// <param name="sender">Object creating the event</param>
    /// <param name="e">Event arguments</param>
    /// <remarks></remarks>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                TL.LogMessage("ForceRemove", "Form closed, program ending");
                TL.Enabled = false;
                TL.Dispose();
                TL = null;

                // Set the return code depending on whether or not the user removed the Platform
                if (PlatformRemoved)
                {
                    Environment.ExitCode = 0;
                }
                else
                {
                    Environment.ExitCode = 99;
                }
            }
            catch
            {
            }
        }

        /// <summary>
    /// Exit button event handler
    /// </summary>
    /// <param name="sender">Object creating the event</param>
    /// <param name="e">Event arguments</param>
    /// <remarks></remarks>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Removal code

        private void TopLevelRemovalScript()
        {
            MsgBoxResult dlgResult;
            string dlgMessage = "WARNING: Uninitialised Message value!";
            try
            {
                TL.LogMessage("ForceRemove", "Start of removal script");

                Status("");
                Action("");

                // Obtain confirmation that Platform removal is required
                switch (cmbRemoveMode.SelectedItem)
                {
                    case REMOVE_INSTALLER_COMBO_TEXT:
                        {
                            dlgMessage = REMOVE_INSTALLER_CONFIRMATION_MESSAGE;
                            break;
                        }
                    case REMOVE_ALL_COMBO_TEXT:
                        {
                            dlgMessage = REMOVE_ALL_CONFIRMATION_MESSAGE;
                            break;
                        }

                    default:
                        {
                            Interaction.MsgBox("Unrecognised cmbRemoveMode value: " + cmbRemoveMode.SelectedItem.ToString(), MsgBoxStyle.Critical);
                            break;
                        }
                }

                TL.LogMessage("ForceRemove", "Removal option: " + cmbRemoveMode.SelectedItem.ToString());

                // Display the confirmation dialogue box
                dlgResult = Interaction.MsgBox(dlgMessage, MsgBoxStyle.Exclamation | MsgBoxStyle.YesNo, "Remove ASCOM");
                TL.LogMessage("ForceRemove", dlgMessage);
                if (dlgResult == MsgBoxResult.Yes) // User said YES so proceed
                {
                    TL.LogMessage("ForceRemove", "User said \"Yes\"");
                    TL.BlankLine();

                    // Flag that we did actually uninstall the Platform so that an appropriate return code can be returned.
                    PlatformRemoved = true;

                    switch (cmbRemoveMode.SelectedItem)
                    {
                        case REMOVE_INSTALLER_COMBO_TEXT:
                            {
                                RemoveInstallers();
                                RemovePlatformFiles();
                                RemoveGAC();
                                RemoveDekstopFilesAndLinks();
                                RemoveGUIDs();
                                break;
                            }
                        case REMOVE_ALL_COMBO_TEXT:
                            {
                                RemoveInstallers();
                                RemoveProfile();
                                RemovePlatformDirectories();
                                RemoveGAC();
                                RemoveDekstopFilesAndLinks();
                                RemoveGUIDs();
                                break;
                            }

                        default:
                            {
                                Interaction.MsgBox("Unrecognised cmbRemoveMode value: " + cmbRemoveMode.SelectedItem.ToString(), MsgBoxStyle.Critical);
                                break;
                            }
                    }

                    Status("Completed");
                }
                else // User said NO so 
                {
                    TL.LogMessage("ForceRemove", "User said \"No\"");
                }
                Action("");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("ForceRemove", "Exception: " + ex.ToString());
            }
            TL.LogMessage("ForceRemove", "End of removal script");

        }

        private void RemoveInstallers()
        {
            var InstallerKeys = new SortedList<string, string>();
            RegistryKey RKey;
            string[] SubKeys;
            string UninstallKey, UninstallProgram;
            string[] Vals = new string[] { "" };
            char[] SplitChars = new char[] { ' ' };

            // Variables for Platform clean up
            string ASCOMDirectory;
            DirectoryInfo DirInfo;
            FileInfo[] FileInfos;
            DirectoryInfo[] DirInfos;
            bool Found;

            const string PLATFORM41 = "Platform 4.1";
            const string PLATFORM50A = "Platform 5.0A";
            const string PLATFORM50A_PRODUCT = "Platform 5.0A Product";
            const string PLATFORM50B = "Platform 5.0B";
            const string PLATFORM50B_PRODUCT = "Platform 5.0B Product";
            const string PLATFORM55 = "Platform 5.5";
            const string PLATFORM60 = "Platform 6.0";
            const string UNINSTALL_STRING = "UninstallString";
            const string ARGS60 = "/s MODIFY=FALSE REMOVE=TRUE UNINSTALL=YES";
            const string ARGS55 = "/VERYSILENT /NORESTART /LOG";

            try
            {
                Status("Removing installer references");
                Action("");
                TL.LogMessage("RemoveInstallers", "Started");
                TL.BlankLine();

                if (Is64Bit())
                {
                    TL.LogMessage("RemoveInstallers", "64bit OS");
                    InstallerKeys.Add(PLATFORM41, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM Platform 4.1");
                    InstallerKeys.Add(PLATFORM50A, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{075F543B-97C5-4118-9D54-93910DE03FE9}");
                    InstallerKeys.Add(PLATFORM50A_PRODUCT, @"SOFTWARE\Classes\Installer\Products\B345F5705C798114D9453919D00EF39E");
                    InstallerKeys.Add(PLATFORM50B, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{14C10725-0018-4534-AE5E-547C08B737B7}");
                    InstallerKeys.Add(PLATFORM50B_PRODUCT, @"SOFTWARE\Classes\Installer\Products\52701C4181004354EAE545C7807B737B");
                    InstallerKeys.Add(PLATFORM55, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM.Platform.NET.Components_is1");
                    InstallerKeys.Add(PLATFORM60, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + PLATFORM6_INSTALL_KEY);
                    UninstallKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                }
                else
                {
                    TL.LogMessage("RemoveInstallers", "32bit OS");
                    InstallerKeys.Add(PLATFORM41, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM Platform 4.1");
                    InstallerKeys.Add(PLATFORM50A, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{075F543B-97C5-4118-9D54-93910DE03FE9}");
                    InstallerKeys.Add(PLATFORM50A_PRODUCT, @"SOFTWARE\Classes\Installer\Products\B345F5705C798114D9453919D00EF39E");
                    InstallerKeys.Add(PLATFORM50B, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{14C10725-0018-4534-AE5E-547C08B737B7}");
                    InstallerKeys.Add(PLATFORM50B_PRODUCT, @"SOFTWARE\Classes\Installer\Products\52701C4181004354EAE545C7807B737B");
                    InstallerKeys.Add(PLATFORM55, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM.Platform.NET.Components_is1");
                    InstallerKeys.Add(PLATFORM60, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + PLATFORM6_INSTALL_KEY);
                    UninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                }

                if (Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM60]) is not null) // Try and uninstall Platform 6
                {
                    TL.LogMessage("Uninstall", PLATFORM60);
                    UninstallProgram = Conversions.ToString(Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM60]).GetValue(UNINSTALL_STRING));
                    UninstallDirectory = Path.GetDirectoryName(UninstallProgram); // Save the current installation's install directory so that it can be deleted at the end if it is not empty
                    RunProcess(PLATFORM60, UninstallProgram, ARGS60);
                    WaitFor(2000);
                }


                else
                {
                    TL.LogMessage("Uninstall", "Installer key for Platform 6 not present");
                }

                // This clean-up code is moved here so that it is always executed, even if the Platform 6 installer is not detected
                // Remove any InstallAware Platform cached install files that remain
                TL.LogMessage("RemoveInstallers", "Removing InstallAware Installer Files");
                try
                {
                    ASCOMDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    DirInfo = new DirectoryInfo(ASCOMDirectory); // Get a directory info for the common application data directory
                    DirInfos = DirInfo.GetDirectories(); // Get a list of directories within the common application data directory

                    Action(Strings.Left(ASCOMDirectory, 70));
                    try // Get file details for each directory in this folder
                    {
                        foreach (var currentDirInfo in DirInfos)
                        {
                            DirInfo = currentDirInfo;
                            try
                            {
                                TL.LogMessageCrLf("RemoveInstallers", "  Processing directory -" + " " + DirInfo.Name + " - " + DirInfo.FullName + " ");
                                if (DirInfo.Name.StartsWith("mia", StringComparison.OrdinalIgnoreCase) & DirInfo.Name.EndsWith(".tmp", StringComparison.OrdinalIgnoreCase))
                                {
                                    TL.LogMessageCrLf("RemoveInstallers", string.Format("Ignoring directory {0} because it is an InstallAware temporary working directory", DirInfo.Name));
                                }
                                else
                                {
                                    FileInfos = DirInfo.GetFiles(); // Get the list of files in this directory
                                    Found = false;
                                    foreach (FileInfo MyFile in FileInfos) // Now delete them
                                    {
                                        TL.LogMessageCrLf("RemoveInstallers", "  Processing file -" + " " + MyFile.Name + " - " + MyFile.FullName + " ");

                                        if ((MyFile.Name.ToUpperInvariant() ?? "") == (PLATFORM6_INSTALL_KEY.ToUpperInvariant() ?? "") | MyFile.Name.StartsWith(PLATFORM_INSTALLER_FILENAME_BASE, StringComparison.OrdinalIgnoreCase))
                                        {
                                            Found = true;
                                            TL.LogMessageCrLf("RemoveInstallers", "  Found install directory - " + DirInfo.Name);
                                        }
                                    }
                                    if (Found)
                                    {
                                        TL.LogMessageCrLf("RemoveInstallers", "  Removing directory - " + DirInfo.FullName);
                                        RemoveFilesRecurse(DirInfo.FullName);
                                    }
                                }
                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                TL.LogMessage("RemoveInstallers 2", "UnauthorizedAccessException for directory; " + DirInfo.FullName);
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessageCrLf("RemoveInstallers 2", "Exception: " + ex.ToString());
                            }
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        TL.LogMessage("RemoveInstallers", "UnauthorizedAccessException for directory; " + DirInfo.FullName);
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveInstallers", "Exception: " + ex.ToString());
                    }

                    WaitFor(1000);
                    // Remove any left over cache directories of which we are aware
                    try
                    {
                        if (!string.IsNullOrEmpty(CacheDirectory))
                        {
                            TL.LogMessageCrLf("RemoveInstallers", string.Format("Removing this executable's installer cache {0}", CacheDirectory));
                            ASCOMDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                            RemoveFilesRecurse(string.Format(@"{0}\{1}", ASCOMDirectory, CacheDirectory));
                            TL.LogMessageCrLf("RemoveInstallers", string.Format("Removed this executable's installer cache {0}", CacheDirectory));

                            if (!string.IsNullOrEmpty(UninstallDirectory))
                            {
                                TL.LogMessageCrLf("RemoveInstallers", string.Format("Removing current executable's installer cache {0}", UninstallDirectory));
                                RemoveFilesRecurse(UninstallDirectory);
                                TL.LogMessageCrLf("RemoveInstallers", string.Format("Removed current installer cache {0}", UninstallDirectory));
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            TL.LogMessageCrLf("RemoveInstallers", "No installer feature code supplied - no action taken");
                        }
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveInstallers", "Exception: " + ex.ToString());
                    }
                }

                catch (Exception ex)
                {
                    TL.LogMessageCrLf("RemoveInstallers", "Exception: " + ex.ToString());
                }

                if (Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM55]) is not null) // Try and uninstall Platform 5.5
                {
                    TL.LogMessage("Uninstall", PLATFORM55);
                    UninstallProgram = Conversions.ToString(Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM55]).GetValue(UNINSTALL_STRING));
                    RunProcess(PLATFORM55, UninstallProgram, ARGS55);
                }
                else
                {
                    TL.LogMessage("Uninstall", "Installer key for Platform 5.5 not present");
                }

                if (Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM50B]) is not null) // Try and uninstall Platform 5.0B
                {
                    TL.LogMessage("Uninstall", PLATFORM50B);
                    UninstallProgram = Conversions.ToString(Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM50B]).GetValue(UNINSTALL_STRING));
                    RunProcess(PLATFORM50B, "MsiExec.exe", SplitKey(UninstallProgram));
                }
                else
                {
                    TL.LogMessage("Uninstall", "Installer key for Platform 5.0B not present");
                }

                if (Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM50A]) is not null) // Try and uninstall Platform 5.0A
                {
                    TL.LogMessage("Uninstall", PLATFORM50A);

                    try
                    {
                        // Now have to fix a missing registry key that fouls up the uninstaller - this was fixed in 5B but prevents 5A from uninstalling on 64bit systems
                        TL.LogMessage("Uninstall", "  Fixing missing AppId");
                        RKey = Registry.ClassesRoot.CreateSubKey(@"AppID\{DF2EB077-4D59-4231-9CB4-C61AD4ECB874}");
                        RKey.SetValue("", "Fixed registry key value");
                        RKey.Close();
                        RKey = null;
                        TL.LogMessage("Uninstall", @"  Successfully set AppID\{DF2EB077-4D59-4231-9CB4-C61AD4ECB874}");
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("Uninstall", "Exception: " + ex.ToString());
                    }

                    UninstallProgram = Conversions.ToString(Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM50A]).GetValue(UNINSTALL_STRING));
                    RunProcess(PLATFORM50A, "MsiExec.exe", SplitKey(UninstallProgram));
                }
                else
                {
                    TL.LogMessage("Uninstall", "Installer key for Platform 5.0A not present");
                }

                if (Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM41]) is not null) // Try and uninstall Platform 4.1
                {
                    try
                    {
                        TL.LogMessage("Uninstall", PLATFORM41);
                        UninstallProgram = Conversions.ToString(Registry.LocalMachine.OpenSubKey(InstallerKeys[PLATFORM41]).GetValue(UNINSTALL_STRING));
                        TL.LogMessage("Uninstall", "  Found uninstall string: \"" + UninstallProgram + "\"");
                        Vals = UninstallProgram.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);
                        TL.LogMessage("Uninstall", "  Found uninstall values: \"" + Vals[0] + "\", \"" + Vals[1] + "\"");
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("Uninstall", "Exception: " + ex.ToString());
                    }
                    RunProcess(PLATFORM41, Vals[0], "/S /Z " + Vals[1]);
                }
                else
                {
                    TL.LogMessage("Uninstall", "Installer key for Platform 4.1 not present");
                }
                TL.BlankLine();

                foreach (var Installer in InstallerKeys)
                {
                    try
                    {
                        Action("Removing registry key: " + Installer.Value);
                        Registry.LocalMachine.DeleteSubKeyTree(Installer.Value);
                        TL.LogMessage("RemoveInstallers", "Reference to " + Installer.Key + " - Removed OK");
                    }
                    catch (ArgumentException)
                    {
                        TL.LogMessage("RemoveInstallers", "Reference to " + Installer.Key + " - Not present");
                    }
                    catch (Exception ex2)
                    {
                        TL.LogMessageCrLf("RemoveInstallers", "Exception 2: " + ex2.ToString());
                    }
                }
                TL.BlankLine();

                TL.LogMessage("RemoveInstallers", "Removing installer references");
                RKey = Registry.LocalMachine.OpenSubKey(UninstallKey, true);
                SubKeys = RKey.GetSubKeyNames();
                foreach (string SubKey in SubKeys)
                {
                    try
                    {
                        if (SubKey.ToUpperInvariant().Contains("ASCOM PLATFORM"))
                        {
                            Action("Removing installer reference: " + SubKey);
                            TL.LogMessage("RemoveInstallers", "Removing Platform installer reference: " + SubKey);
                            RKey.DeleteSubKeyTree(SubKey);
                        }
                    }
                    catch (Exception ex3)
                    {
                        TL.LogMessageCrLf("RemoveInstallers", "Exception 3: " + ex3.ToString());
                    }
                }

                // Check for any Products remaining
                TL.BlankLine();
                try
                {
                    RKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Installer\Products", true);
                    SubKeys = RKey.GetSubKeyNames();
                    string ProductDescription;

                    foreach (var SubKey in SubKeys)
                    {
                        ProductDescription = Conversions.ToString(RKey.OpenSubKey(SubKey).GetValue("ProductName", "Product description not present"));
                        TL.LogMessage("RemoveInstallers", "Found Product: " + ProductDescription);
                        if (ProductDescription.ToUpperInvariant().Contains("ASCOM PLATFORM"))
                        {
                            Action("Removing installer: " + ProductDescription);
                            TL.LogMessage("RemoveInstallers", "  Deleting: " + ProductDescription);
                            RKey.DeleteSubKeyTree(SubKey);
                        }
                    }
                }

                catch (Exception ex4)
                {
                    TL.LogMessageCrLf("RemoveInstallers", "Exception 4: " + ex4.ToString());
                }

                TL.LogMessage("RemoveInstallers", "Completed");
            }
            catch (Exception ex1)
            {
                TL.LogMessageCrLf("RemoveInstallers", "Exception 1: " + ex1.ToString());
            }
            TL.BlankLine();
        }

        private void RemoveProfile()
        {
            RegistryKey RKey;
            string ASCOMDirectory;

            try
            {
                Status("Removing RemoveProfile Entries");
                TL.LogMessage("RemoveProfile", "Started");

                if (Is64Bit())
                {
                    try
                    {
                        TL.LogMessage("RemoveProfile", "Removing Profile from 32bit registry location on a 64bit OS");
                        RKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node", true);
                        RKey.DeleteSubKeyTree("ASCOM");
                        TL.LogMessage("RemoveProfile", "  Removed OK");
                    }
                    catch (ArgumentException)
                    {
                        TL.LogMessage("RemoveProfile", "  Not present");
                    }
                    catch (Exception ex1)
                    {
                        TL.LogMessageCrLf("RemoveProfile", "Exception 1: " + ex1.ToString());
                    }
                }
                else
                {
                    try
                    {
                        TL.LogMessage("RemoveProfile", "Removing Profile from registry on a 32bit OS");
                        RKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                        RKey.DeleteSubKeyTree("ASCOM");
                        TL.LogMessage("RemoveProfile", "  Removed OK");
                    }
                    catch (ArgumentException)
                    {
                        TL.LogMessage("RemoveProfile", "  Not present");
                    }
                    catch (Exception ex1)
                    {
                        TL.LogMessageCrLf("RemoveProfile", "Exception 1: " + ex1.ToString());
                    }
                }

                try
                {
                    TL.LogMessage("RemoveProfile", "Removing ASCOM User preferences");
                    RKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                    RKey.DeleteSubKeyTree("ASCOM");
                    TL.LogMessage("RemoveProfile", "  Removed OK");
                }
                catch (ArgumentException)
                {
                    TL.LogMessage("RemoveProfile", "  Not present");
                }
                catch (Exception ex3)
                {
                    TL.LogMessageCrLf("RemoveProfile", "Exception 3: " + ex3.ToString());
                }

                TL.LogMessage("RemoveProfile", "Removing ASCOM 5.5 Profile Files");
                try
                {
                    ASCOMDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\ASCOM";
                    RemoveFilesRecurse(ASCOMDirectory);
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("RemoveProfile", "Exception: " + ex.ToString());
                }

                TL.LogMessage("RemoveProfile", "Completed");
                TL.BlankLine();
            }

            catch (Exception ex)
            {
                TL.LogMessageCrLf("RemoveProfile", "Exception: " + ex.ToString());
            }
            TL.BlankLine();
        }

        /// <summary>
    /// Remove specific Platform files only leaving the directory structure and 3rd party files intact.
    /// </summary>
    /// <remarks></remarks>
        private void RemovePlatformFiles()
        {
            Regex regexInstallerVariables;
            Match mVar;

            string CommonFiles, CommonFiles64, TargetDirectoryPlatform, TargetDirectoryDeveloper;

            TL.LogMessage("RemovePlatformFiles", "Started");
            Status("Removing Platform files");

            // Set up a regular expression to pick out the compiler variable from the InstallPath part of an InstallAware Install Files line
            // $COMMONFILES$\ASCOM\Platform\v6
            // Group within the matched line: <--CompVar-->
            regexInstallerVariables = new Regex(@"\$(?<CompVar>[\w]*)\$.*", RegexOptions.IgnoreCase);

            // Set up variables once so they can be used many times
            if (Is64Bit()) // Set variables for when we are running on a 64bit OS
            {
                TargetDirectoryPlatform = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + ASCOM_TARGET_DIRECTORY_PLATFORM;
                TargetDirectoryDeveloper = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + ASCOM_TARGET_DIRECTORY_DEVELOPER;
                CommonFiles = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
                CommonFiles64 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);

                TL.LogMessage("RemovePlatformFiles", "This is a 64bit OS");
                TL.LogMessage("RemovePlatformFiles", "TargetDirectory: " + TargetDirectoryPlatform);
                TL.LogMessage("RemovePlatformFiles", "CommonFiles: " + CommonFiles);
                TL.LogMessage("RemovePlatformFiles", "CommonFiles64: " + CommonFiles64);
            }
            else // Set variables for when we are running on a 32bit OS
            {
                TargetDirectoryPlatform = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + ASCOM_TARGET_DIRECTORY_PLATFORM;
                TargetDirectoryDeveloper = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + ASCOM_TARGET_DIRECTORY_DEVELOPER;
                CommonFiles = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                CommonFiles64 = "Not set";

                TL.LogMessage("RemovePlatformFiles", "This is a 32bit OS");
                TL.LogMessage("RemovePlatformFiles", "TargetDirectory: " + TargetDirectoryPlatform);
                TL.LogMessage("RemovePlatformFiles", "CommonFiles: " + CommonFiles);
                TL.LogMessage("RemovePlatformFiles", "CommonFiles64: " + CommonFiles64);
            }

            // Iterate of the list of Platform files, convert compiler variables to real values on this system and remove the files
            foreach (string fileFullName in DynamicLists.PlatformFiles(TL))
            {
                try
                {
                    Action("Removing file: " + fileFullName);
                    // TL.LogMessage("RemovePlatformFiles", "Removing file: " & fileFullName)
                    mVar = regexInstallerVariables.Match(fileFullName);
                    if (mVar.Success) // We have found a compiler variable so process it
                    {
                        switch (mVar.Groups["CompVar"].ToString().ToUpperInvariant() ?? "")
                        {
                            case "TARGETDIR":
                                {
                                    DeleteFile(fileFullName.Replace("$TARGETDIR$", TargetDirectoryPlatform));
                                    break;
                                }
                            case "COMMONFILES":
                                {
                                    DeleteFile(fileFullName.Replace("$COMMONFILES$", CommonFiles));
                                    break;
                                }
                            case "COMMONFILES64":
                                {
                                    if (Is64Bit())
                                    {
                                        DeleteFile(fileFullName.Replace("$COMMONFILES64$", CommonFiles64));
                                    }
                                    else
                                    {
                                        TL.LogMessage("RemovePlatformFiles", "Ignoring 64bit variable: " + fileFullName);
                                    } // Unrecognised compiler variable so log an error

                                    break;
                                }

                            default:
                                {
                                    TL.LogMessage("RemovePlatformFiles", "***** UNKNOWN Compiler Variable: " + mVar.Groups["CompVar"].ToString() + " in file: " + fileFullName);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        TL.LogMessage("RemovePlatformFiles", "***** NO Compiler Variable in file: " + fileFullName);
                    }
                }
                // TL.LogMessage("RemovePlatformFiles", "Removing file: " & fileFullName)
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("", "Exception: " + ex.ToString());
                }
            }
            TL.BlankLine();

            // Iterate of the list of Developer files, convert compiler variables to real values on this system and remove the files
            foreach (string fileFullName in DynamicLists.DeveloperFiles(TL))
            {
                try
                {
                    Action("Removing file: " + fileFullName);
                    // TL.LogMessage("RemovePlatformFiles", "Removing file: " & fileFullName)
                    mVar = regexInstallerVariables.Match(fileFullName);
                    if (mVar.Success) // We have found a compiler variable so process it
                    {
                        switch (mVar.Groups["CompVar"].ToString().ToUpperInvariant() ?? "")
                        {
                            case "TARGETDIR":
                                {
                                    DeleteFile(fileFullName.Replace("$TARGETDIR$", TargetDirectoryDeveloper));
                                    break;
                                }
                            case "COMMONFILES":
                                {
                                    DeleteFile(fileFullName.Replace("$COMMONFILES$", CommonFiles));
                                    break;
                                }
                            case "COMMONFILES64":
                                {
                                    if (Is64Bit())
                                    {
                                        DeleteFile(fileFullName.Replace("$COMMONFILES64$", CommonFiles64));
                                    }
                                    else
                                    {
                                        TL.LogMessage("RemovePlatformFiles", "Ignoring 64bit variable: " + fileFullName);
                                    } // Unrecognised compiler variable so log an error

                                    break;
                                }

                            default:
                                {
                                    TL.LogMessage("RemovePlatformFiles", "***** UNKNOWN Compiler Variable: " + mVar.Groups["CompVar"].ToString() + " in file: " + fileFullName);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        TL.LogMessage("RemovePlatformFiles", "***** NO Compiler Variable in file: " + fileFullName);
                    }
                }
                // TL.LogMessage("RemovePlatformFiles", "Removing file: " & fileFullName)
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("", "Exception: " + ex.ToString());
                }
            }

            TL.LogMessage("RemovePlatformFiles", "Completed");
        }

        /// <summary>
    /// Recursively removes all Platform directories and their contents regardless of whether the files are Platform or 3rd party provided.
    /// </summary>
    /// <remarks></remarks>
        private void RemovePlatformDirectories()
        {
            var Path = new StringBuilder(260);
            int rc;
            string ASCOMDirectory;
            DirectoryInfo DirInfo;
            FileInfo[] FileInfos;
            DirectoryInfo[] DirInfos;
            bool Found;

            try
            {
                Status("Removing directories and files");
                TL.LogMessage("RemoveDirectories", "Started");

                if (Is64Bit())
                {
                    try
                    {
                        TL.LogMessage("RemoveDirectories", "Removing ASCOM 32bit Common Files from a 64bit OS");
                        rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES_COMMONX86, 0);
                        ASCOMDirectory = Path.ToString() + @"\ASCOM";
                        TL.LogMessage("RemoveDirectories", "  ASCOM directory: " + ASCOMDirectory);
                        RemoveFilesRecurse(ASCOMDirectory);
                        TL.LogMessage("RemoveDirectories", "  Removed OK");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        TL.LogMessage("RemoveDirectories", "  Not present");
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveDirectories", "Exception: " + ex.ToString());
                    }
                    TL.BlankLine();

                    try
                    {
                        TL.LogMessage("RemoveDirectories", "Removing ASCOM 64bit Common Files from a 64bit OS");
                        rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES_COMMON, 0);
                        ASCOMDirectory = Path.ToString() + @"\ASCOM";
                        TL.LogMessage("RemoveDirectories", "  ASCOM directory: " + ASCOMDirectory);
                        RemoveFilesRecurse(ASCOMDirectory);
                        TL.LogMessage("RemoveDirectories", "  Removed OK");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        TL.LogMessage("RemoveDirectories", "  Not present");
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveDirectories", "Exception: " + ex.ToString());
                    }
                    TL.BlankLine();

                    try
                    {
                        TL.LogMessage("RemoveDirectories", "Removing ASCOM 32bit Program Files from a 64bit OS");
                        rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILESX86, 0);
                        ASCOMDirectory = Path.ToString() + @"\ASCOM";
                        TL.LogMessage("RemoveDirectories", "  ASCOM directory: " + ASCOMDirectory);
                        RemoveFilesRecurse(ASCOMDirectory);
                        TL.LogMessage("RemoveDirectories", "  Removed OK");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        TL.LogMessage("RemoveDirectories", "  Not present");
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveDirectories", "Exception: " + ex.ToString());
                    }
                    TL.BlankLine();

                    try
                    {
                        TL.LogMessage("RemoveDirectories", "Removing ASCOM 64bit Program Files from a 64bit OS");
                        rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES, 0);
                        ASCOMDirectory = Path.ToString() + @"\ASCOM";
                        TL.LogMessage("RemoveDirectories", "  ASCOM directory: " + ASCOMDirectory);
                        RemoveFilesRecurse(ASCOMDirectory);
                        TL.LogMessage("RemoveDirectories", "  Removed OK");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        TL.LogMessage("RemoveDirectories", "  Not present");
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveDirectories", "Exception: " + ex.ToString());
                    }
                    TL.BlankLine();
                }
                else
                {
                    try
                    {
                        TL.LogMessage("RemoveDirectories", "Removing ASCOM Common Files from a 32 bit OS");
                        rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES_COMMON, 0);
                        ASCOMDirectory = Path.ToString() + @"\ASCOM";
                        TL.LogMessage("RemoveDirectories", "  ASCOM directory: " + ASCOMDirectory);
                        RemoveFilesRecurse(ASCOMDirectory);
                        TL.LogMessage("RemoveDirectories", "  Removed OK");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        TL.LogMessage("RemoveDirectories", "  Not present");
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveDirectories", "Exception: " + ex.ToString());
                    }
                    TL.BlankLine();

                    try
                    {
                        TL.LogMessage("RemoveDirectories", "Removing ASCOM Program Files from a 32bit OS");
                        rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES, 0);
                        ASCOMDirectory = Path.ToString() + @"\ASCOM";
                        TL.LogMessage("RemoveDirectories", "  ASCOM directory: " + ASCOMDirectory);
                        RemoveFilesRecurse(ASCOMDirectory);
                        TL.LogMessage("RemoveDirectories", "  Removed OK");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        TL.LogMessage("RemoveDirectories", "  Not present");
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveDirectories", "Exception: " + ex.ToString());
                    }
                    TL.BlankLine();
                }
            }

            catch (Exception ex1)
            {
                TL.LogMessageCrLf("RemoveDirectories", "Exception 1: " + ex1.ToString());
            }

            TL.LogMessage("RemoveDirectories", "Completed");
            TL.BlankLine();
        }

        private void RemoveGAC()
        {
            IAssemblyCache pCache;
            RemoveOutcome Outcome;
            IAssemblyEnum ae;
            IAssemblyName an = null;
            SortedList<string, string> ASCOMAssemblyNames;
            AssemblyName assname;
            RegistryKey RKey;
            string[] SubKeys, Values;

            TL.LogMessage("RemoveGAC", "Started");
            Status("Removing GAC references");

            try
            {
                RKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Installer\Assemblies", true);
                SubKeys = RKey.GetSubKeyNames();
                foreach (string SubKey in SubKeys)
                {
                    try
                    {
                        if (SubKey.ToUpperInvariant().Contains("ASCOM"))
                        {
                            TL.LogMessage("RemoveGAC", "Removing application reference: " + SubKey);
                            RKey.DeleteSubKeyTree(SubKey);
                        }
                    }
                    catch (Exception ex4)
                    {
                        TL.LogMessageCrLf("RemoveGAC", "Exception 4: " + ex4.ToString());
                    }
                }
                TL.BlankLine();

                TL.LogMessage("RemoveGAC", "Removing Assembly Global Values");
                RKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Installer\Assemblies\Global", true);
                Values = RKey.GetValueNames();
                foreach (string Value in Values)
                {
                    try
                    {
                        if (Value.ToUpperInvariant().Contains("ASCOM"))
                        {
                            TL.LogMessage("RemoveGAC", "Removing global value: " + Value);
                            RKey.DeleteValue(Value);
                        }
                    }
                    catch (Exception ex4)
                    {
                        TL.LogMessageCrLf("RemoveGAC", "Exception 4: " + ex4.ToString());
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                TL.LogMessageCrLf("RemoveGAC", @"Registry Key HKLM\SOFTWARE\Classes\Installer\Assemblies does not exist");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RemoveGAC", "Exception 3: " + ex.ToString());
            }
            TL.BlankLine();

            try
            {
                Status("Scanning Assemblies");
                ASCOMAssemblyNames = new SortedList<string, string>();

                TL.LogMessage("RemoveGAC", "Assemblies registered in the GAC");
                ae = AssemblyCache.CreateGACEnum(); // Get an enumerator for the GAC assemblies

                while (AssemblyCache.GetNextAssembly(ae, ref an) == 0) // Enumerate the assemblies
                {
                    try
                    {
                        assname = GetAssemblyName(an);
                        if (Strings.InStr(assname.FullName, "ASCOM") > 0) // Extra information for ASCOM files
                        {
                            TL.LogMessage("RemoveGAC", "Found: " + assname.FullName);
                            ASCOMAssemblyNames.Add(assname.FullName, assname.Name); // Convert the fusion representation to a standard AssemblyName and get its full name
                        }
                        else
                        {
                            TL.LogMessage("RemoveGAC", "Also : " + assname.FullName);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ignore an exceptions here due to duplicate names, these are all MS assemblies
                    }

                }

                // Get an IAssemblyCache interface
                pCache = AssemblyCache.CreateAssemblyCache();
                foreach (var AssemblyName in ASCOMAssemblyNames)
                {
                    try
                    {
                        TL.LogMessage("RemoveGAC", "Removing " + AssemblyName.Key);
                        TL.LogMessage("RemoveGAC", "  AssemblyCache.RemoveGAC - " + AssemblyName.Value);
                        Outcome = AssemblyCache.RemoveGAC(AssemblyName.Value);
                        if (Outcome.ReturnCode == 0)
                        {
                            TL.LogMessage("RemoveGAC", "  OK - Uninstalled with no error!");
                        }
                        else
                        {
                            TL.LogMessage("RemoveGAC #####", "  Bad RC: " + Outcome.ReturnCode);
                        }

                        switch (Outcome.Disposition)
                        {
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLEDField:
                                {
                                    TL.LogMessage("RemoveGAC #####", "  Outcome: Assembly already uninstalled");
                                    break;
                                }
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDINGField:
                                {
                                    TL.LogMessage("RemoveGAC #####", "   Outcome: Delete currently pending");
                                    break;
                                }
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCESField:
                                {
                                    TL.LogMessage("RemoveGAC #####", "  Outcome: Assembly has remaining install references");
                                    break;
                                }
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUNDField:
                                {
                                    TL.LogMessage("RemoveGAC #####", "  Outcome: Unable to find assembly - " + AssemblyName.Value + " - " + AssemblyName.Key);
                                    break;
                                }
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USEField:
                                {
                                    TL.LogMessage("RemoveGA #####C", "  Outcome: Assembly still in use");
                                    break;
                                }
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLEDField:
                                {
                                    TL.LogMessage("RemoveGAC", "  Outcome: Assembly uninstalled");
                                    break;
                                }

                            default:
                                {
                                    TL.LogMessage("RemoveGAC #####", "  Unknown uninstall outcome code: " + ((int)Outcome.Disposition).ToString());
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RemoveGAC", "Exception 2: " + ex.ToString());
                    }
                }
                TL.LogMessage("RemoveGAC", "Completed");
            }

            catch (Exception ex)
            {
                TL.LogMessageCrLf("RemoveGAC", "Exception 1: " + ex.ToString());
            }
            TL.BlankLine();
        }

        private void RemoveDekstopFilesAndLinks()
        {
            var Path = new StringBuilder(260);
            int rc;
            string DesktopDirectory, StartMenuDirectory;

            Status("Removing desktop files and links");
            TL.LogMessage("RemoveDekstopFilesAndLinks", "Started");

            try
            {
                rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_COMMON_DESKTOPDIRECTORY, 0);
                DesktopDirectory = Path.ToString();
                TL.LogMessage("RemoveDekstopFilesAndLinks", "Desktop directory: " + DesktopDirectory);
                File.Delete(DesktopDirectory + @"\ASCOM Diagnostics.lnk");
                File.Delete(DesktopDirectory + @"\ASCOM Profile Explorer.lnk");
                File.Delete(DesktopDirectory + @"\Profile Explorer.lnk");
                File.Delete(DesktopDirectory + @"\ProfileExplorer.lnk");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RemoveDekstopFilesAndLinks", "Exception: " + ex.ToString());
            }

            try
            {
                rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_COMMON_STARTMENU, 0);
                StartMenuDirectory = Path.ToString() + @"\Programs\ASCOM Platform";
                TL.LogMessage("RemoveDekstopFilesAndLinks", "Start Menu directory: " + StartMenuDirectory);
                RemoveFilesRecurse(StartMenuDirectory);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RemoveDekstopFilesAndLinks", "Exception: " + ex.ToString());
            }

            try
            {
                rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_COMMON_STARTMENU, 0);
                StartMenuDirectory = Path.ToString() + @"\Programs\ASCOM Platform 6";
                TL.LogMessage("RemoveDekstopFilesAndLinks", "Start Menu directory (P6): " + StartMenuDirectory);
                RemoveFilesRecurse(StartMenuDirectory);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RemoveDekstopFilesAndLinks", "Exception: " + ex.ToString());
            }

            TL.LogMessage("RemoveDekstopFilesAndLinks", "Completed");
            TL.BlankLine();

        }

        private void RemoveGUIDs()
        {

            TL.LogMessage("RemoveGUIDs", "Started");
            Status("Removing GUIDs");

            foreach (KeyValuePair<string, string> Guid in DynamicLists.GUIDs(TL))
                CleanGUID(Guid.Key, Guid.Value);
            TL.LogMessage("RemoveGUIDs", "Completed");
        }

        #endregion

        #region Support Routines

        /// <summary>
    /// Delete a single file, reporting success or an exception
    /// </summary>
    /// <param name="FileName">Full path to the file to delete</param>
    /// <remarks></remarks>
        private void DeleteFile(string FileName)
        {
            FileInfo TargetFile;

            try
            {
                Action(FileName);
                if (Path.GetFileNameWithoutExtension(FileName).IndexOfAny(Path.GetInvalidFileNameChars()) == -1) // The filename contains only valid characters
                {
                    TargetFile = new FileInfo(FileName);
                    TargetFile.Attributes = FileAttributes.Normal;
                    TargetFile.Delete();
                    TL.LogMessage("RemoveFile", "Removed OK:              " + FileName);
                }
                else
                {
                    TL.LogMessage("RemoveFile", "Invalid filename:        " + FileName);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                TL.LogMessage("RemoveFile", "Directory not found:     " + FileName);
            }
            catch (FileNotFoundException ex)
            {
                TL.LogMessage("RemoveFile", "File not found:          " + FileName);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RemoveFile", "ISSUE - " + FileName + ", Exception: " + ex.ToString());
            }
        }

        private void CleanGUID(string GUIDKey, string FileLocation)
        {
            Action("Removing GUID: " + GUIDKey.ToString());
            TL.LogMessage("CleanGUID", "Cleaning GUID: " + GUIDKey + " in " + FileLocation);

            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("CLSID", true), GUIDKey);
            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("Interface", true), GUIDKey);
            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("Record", true), GUIDKey);
            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("AppID", true), GUIDKey);
            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("TypeLib", true), GUIDKey);

            if (Is64Bit())
            {
                CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey(@"Wow6432Node\CLSID", true), GUIDKey);
                CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey(@"Wow6432Node\Interface", true), GUIDKey);
                CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey(@"Wow6432Node\AppID", true), GUIDKey);
                CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey(@"Wow6432Node\TypeLib", true), GUIDKey);
            }
        }

        private void CleanRegistryLocation(RegistryKey BaseKey, string GUIDSubKey)
        {
            try
            {
                GUIDSubKey = "{" + GUIDSubKey + "}";
                BaseKey.DeleteSubKeyTree(GUIDSubKey);
                TL.LogMessage("CleanRegistryLocation", "  SubKey removed: " + BaseKey.Name + @"\" + GUIDSubKey);
            }
            catch (ArgumentException)
            {
                TL.LogMessage("CleanRegistryLocation", "    SubKey does not exist: " + BaseKey.Name + @"\" + GUIDSubKey);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("CleanRegistryLocation", ex.ToString());
            }
        }

        private void RemoveFilesRecurse(string Folder)
        {
            DirectoryInfo DirInfo;
            FileInfo[] FileInfos;
            DirectoryInfo[] DirInfos;
            try
            {
                TL.LogMessageCrLf("RemoveFilesRecurse", "Processing folder - " + Folder);

                if (Directory.Exists(Folder))
                {
                    DirInfo = new DirectoryInfo(Folder);
                    Action(Strings.Left(Folder, 70));
                    try // Get file details for files in this folder
                    {
                        FileInfos = DirInfo.GetFiles();
                        foreach (FileInfo MyFile in FileInfos) // Now delete them
                        {
                            TL.LogMessageCrLf("RemoveFilesRecurse", "  Erasing file - " + MyFile.Name);
                            MyFile.Attributes = FileAttributes.Normal;
                            MyFile.Delete();
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        TL.LogMessage("RecurseProgramFiles 1", "UnauthorizedAccessException for directory; " + Folder);
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RecurseProgramFiles 1", "Exception: " + ex.ToString());
                    }

                    try // Iterate over the sub directories of this folder
                    {
                        DirInfos = DirInfo.GetDirectories();
                        foreach (DirectoryInfo Directory in DirInfos)
                            RemoveFilesRecurse(Directory.FullName); // Recursively process this sub directory
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        TL.LogMessage("RecurseProgramFiles 2", "UnauthorizedAccessException for directory; " + Folder);
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("RecurseProgramFiles 2", "Exception: " + ex.ToString());
                    }

                    TL.LogMessageCrLf("RemoveFilesRecurse", "Deleting folder - " + Folder);
                    Directory.Delete(Folder); // This directory should now be empty so remove it
                }
                else
                {
                    TL.LogMessageCrLf("RemoveFilesRecurse", "  Folder does not exist");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                TL.LogMessage("RecurseProgramFiles 3", "UnauthorizedAccessException for directory; " + Folder);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RecurseProgramFiles 3", "Exception: " + ex.ToString());
            }
        }

        private void Status(string Msg)
        {
            Application.DoEvents();
            lblResult.Text = Msg;
            Application.DoEvents();
        }

        private void Action(string Msg)
        {
            Application.DoEvents();
            LblAction.Text = Msg;
            Application.DoEvents();
        }

        private AssemblyName GetAssemblyName(IAssemblyName nameRef)
        {
            var AssName = new AssemblyName();
            try
            {
                AssName.Name = AssemblyCache.GetName(nameRef);
                AssName.Version = AssemblyCache.GetVersion(nameRef);
                AssName.CultureInfo = AssemblyCache.GetCulture(nameRef);
                AssName.SetPublicKeyToken(AssemblyCache.GetPublicKeyToken(nameRef));
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("GetAssemblyName", "Exception: " + ex.ToString());
            }
            return AssName;
        }

        private bool Is64Bit()
        {
            return IntPtr.Size == 8;
        }

        private void WaitFor(int period)
        {
            DateTime startTime;
            startTime = DateTime.Now;
            do
            {
                Thread.Sleep(20);
                Application.DoEvents();
            }
            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < period);

        }

        // run the uninstaller
        private void RunProcess(string InstallerName, string processToRun, string args)
        {
            ProcessStartInfo startInfo;
            Process myProcess;
            TimeSpan ProcessTimeout, elapsedTime;
            const int PROCESS_TIMEOUT_MINUTES = 3; // Define a timeout in minutes for this process after which it will be cancelled

            ProcessTimeout = TimeSpan.FromMinutes(PROCESS_TIMEOUT_MINUTES);

            try
            {
                TL.LogMessage("RunProcess", "  Process: " + processToRun);
                TL.LogMessage("RunProcess", "  Args: " + args);

                startInfo = new ProcessStartInfo(processToRun);
                startInfo.Arguments = args;
                startInfo.ErrorDialog = false;

                DateTime StartTime;
                StartTime = DateTime.Now;
                myProcess = Process.Start(startInfo);
                do
                {
                    elapsedTime = DateTime.Now.Subtract(StartTime);
                    Thread.Sleep(10);
                    Action(string.Format("Removing: {0} - {1} / {2} seconds", InstallerName, elapsedTime.TotalSeconds.ToString("0"), ProcessTimeout.TotalSeconds.ToString("0")));
                    Application.DoEvents();
                }
                while (!(myProcess.HasExited | elapsedTime > ProcessTimeout));

                if (myProcess.HasExited) // The uninstaller ran OK and terminated
                {
                    TL.LogMessage("RunProcess", "  Completed - exit code: " + myProcess.ExitCode.ToString());
                }
                else // The installer appears to be stuck so kill the process and continue
                {
                    TL.LogMessage("RunProcess", "  Installer did not complete in the allowed time - killing the process");
                    myProcess.Kill();
                    TL.LogMessage("RunProcess", "  Installer process killed");
                }

                try // Close the process and ignore any errors
                {
                    myProcess.Close();
                    myProcess.Dispose();
                    myProcess = null;
                }
                catch
                {
                }
            }
            catch (Exception e)
            {
                TL.LogMessageCrLf("RunProcess", "Exception: " + e.ToString());
            }
        }

        // split the installer string and select the first argument, converting /I to /q /x
        private static string SplitKey(string keyToSplit)
        {
            char[] SplitChars = new char[] { ' ' };
            string[] s = keyToSplit.Split(SplitChars);
            s[1] = s[1].Replace("/I", "/q /x ");
            return s[1];
        }

        #endregion

    }
}