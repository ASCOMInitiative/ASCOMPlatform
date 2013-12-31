using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MakeDynamicLists
{
    class Program
    {
        /// <summary>
        /// Trawls through the Platform source code base picking out all GUIDs explicitly defined. These are then written out as the
        /// GUIDList class in the RemoveASCOM program so that it can remove these from the user's PC. This dynamic approach is used so that this
        /// application is maintenance free as new GUIDs are added in future.
        /// 
        /// This program is run by the build process before RemoveASCOM is compiled so that GUIDList is up to date.
        /// </summary>
        /// <param name="args">Accepts one parameter, the full path to the base of the Exported Platform source code.</param>
        /// <remarks> If the command line parameter is omitted the program will default to values appropriate to the development environment as oposed to the build environment.</remarks>
        static void Main(string[] args)
        {
            // Settings for use in the development environment, these are overriden by a command line argument in the build environment
            string developmentSearchPath = @"..\..\..\..";
            string outputPath = @"\Remove ASCOM\Remove ASCOM\";
            string devlopmentPath = developmentSearchPath + outputPath;
            string outputClassFileName = @"DynamicLists.vb";
            string outputTextFileName = @"GUIDList.txt";
            string platformInstallerTextFileName = @"\Releases\ASCOM 6\Platform\Installer Project\Ascom Platform 6.mia.txt";
            string developerInstallerTextFileName = @"\Releases\ASCOM 6\Developer\Installer Project\ASCOM Platform 6 Developer.mia.txt";

            // Construct the development environment values
            string sourceSearchPath = developmentSearchPath;
            string outputClassFullFileName = devlopmentPath + outputClassFileName;
            string outputTextFullFileName = devlopmentPath + outputTextFileName;
            string platformInstallerTextFileFullName = developmentSearchPath + platformInstallerTextFileName;
            string developerInstallerTextFileFullName = developmentSearchPath + developerInstallerTextFileName;

            // Storage for the GUIDs found and a suppression list of GUIDs that should be left undisturbed
            SortedList<string, string> guidList = new SortedList<string, string>();
            List<string> guidSuppressionList = new List<string>();

            // Storage for the lists of files installed by the Platform
            SortedSet<string> platformFileList = new SortedSet<string>();
            SortedSet<string> developerFileList = new SortedSet<string>();

            using (TraceLogger TL = new TraceLogger("", "MakeDynamicLists"))
            {
                TL.Enabled = true;

                //
                // Create the list of GUIDs created by the Platform
                //
                try
                {

                    // Override the source and output paths if a command line search path is provided
                    if (args.Length > 0)
                    {
                        sourceSearchPath = args[0];
                        outputClassFullFileName = args[0] + outputPath + outputClassFileName;
                        outputTextFullFileName = args[0] + outputPath + outputTextFileName;
                        platformInstallerTextFileFullName = args[0] + platformInstallerTextFileName;
                        developerInstallerTextFileFullName = args[0] + developerInstallerTextFileName;
                    }

                    TL.LogMessage("Main", "Search path: " + Path.GetFullPath(sourceSearchPath));
                    TL.LogMessage("Main", "Output class file: " + Path.GetFullPath(outputClassFullFileName));
                    TL.LogMessage("Main", "Output text file: " + Path.GetFullPath(outputTextFullFileName));

                    // Add required items to the suppression list so that they are not deleted by RemoveASCOM when it runs
                    guidSuppressionList.Add("00000001-0000-0000-C000-000000000046");
                    guidSuppressionList.Add("00000002-0000-0000-C000-000000000046");

                    // Set up a regular expression for a GUID format e.g.:   Guid("0EF59E5C-2715-4E91-8A5E-38FE388B4F00")
                    // Regular expression groups within the matched GUID:    <-G1-><--------------G2------------------> 
                    // Group 2 picks out the GUID value inside the double quote characters. This is used as m.Groups[2] below
                    Regex regexGuid = new Regex(@"(Guid\(\"")([0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12})""", RegexOptions.IgnoreCase);

                    // Get a list of all the files with a .cs CSharp extension
                    List<string> files = new List<string>(Directory.GetFiles(sourceSearchPath, "*.cs*", SearchOption.AllDirectories));
                    TL.LogMessage("Main", "Number of C# files found: " + files.Count);

                    // Add the list of files with a .vb extension
                    files.AddRange(new List<string>(Directory.GetFiles(sourceSearchPath, "*.vb*", SearchOption.AllDirectories)));
                    TL.LogMessage("Main", "Total files found: " + files.Count);

                    // Process the list of files opening each file in turn and searching every line with the GUID regular expression
                    // Save every match except for those on the suppression list
                    foreach (string file in files)
                    {
                        if (!file.Contains(Path.GetFileName(outputClassFullFileName))) // Process all files except our output file!
                        {
                            List<string> lines = new List<string>(File.ReadAllLines(file)); // Get all lines into a list
                            foreach (string line in lines) // Iterate over the list of lines
                            {
                                Match m = regexGuid.Match(line); // Use the regular expression to search for a match
                                if (m.Success) // We have found a GUID
                                {
                                    try
                                    {
                                        // Check whether this a GUID to suppress
                                        if (!guidSuppressionList.Contains(m.Groups[2].ToString())) // Not suppressed
                                        {
                                            guidList.Add(m.Groups[2].ToString(), Path.GetFullPath(file)); // Add the GUID to the found GUIDs list
                                            TL.LogMessage("Main", "Match: " + m.Groups[2] + " in: " + file);
                                        }
                                        else // This GUID should be left alone so don't add it to the list
                                        {
                                            TL.LogMessage("Main", "Suppressing: " + m.Groups[2] + " in: " + file);
                                        }
                                    }
                                    catch (ArgumentException) // The GUID has already been added so ignore it as a duplicate
                                    {
                                        TL.LogMessage("Main", "Duplicate: " + m.Groups[2] + " in: " + file);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("Main", "Exception creating GUID list: " + ex.ToString());
                }
                TL.BlankLine();

                // Create lists of "Install Files" lines in the Platform and Developer installer scripts
                platformFileList = FindInstalledFiles(platformInstallerTextFileFullName, TL); // Lines from the Platform installer
                developerFileList = FindInstalledFiles(developerInstallerTextFileFullName, TL); // Lines from the Developer installer.
                TL.BlankLine();

                //
                // Create the class file that will be used by the RemoveASCOM program
                //
                try
                {
                    // Create the streamwriter to make the updated GUIDList class file and the readable txt file
                    StreamWriter outputTextFile = new StreamWriter(outputTextFullFileName);
                    StreamWriter outputClassFile = new StreamWriter(outputClassFullFileName);

                    // Add the first lines of the GUID text list
                    outputTextFile.WriteLine(@"This is a list of the GUIDs that will be removed by RemoveASCOM");
                    outputTextFile.WriteLine(" ");

                    // Add the first lines of the DynamicLists class
                    outputClassFile.WriteLine(@"' ***** WARNING ***** WARNING ***** WARNING *****");
                    outputClassFile.WriteLine(" ");
                    outputClassFile.WriteLine(@"' This class is dynamically generated by the MakeDynamicLists Program.");
                    outputClassFile.WriteLine(@"' Do not alter this class, alter the MakeDynamicLists program instead and your changes will appear when the next build is made.");
                    outputClassFile.WriteLine(" ");
                    outputClassFile.WriteLine("Imports System.Collections.Generic");
                    outputClassFile.WriteLine("Imports System.IO");
                    outputClassFile.WriteLine(" ");
                    outputClassFile.WriteLine("Class DynamicLists");
                    outputClassFile.WriteLine("");

                    // Add the first lines of the GUIDs member
                    outputClassFile.WriteLine("    Shared Function GUIDs(TL as TraceLogger) As SortedList(Of String, String)");
                    outputClassFile.WriteLine("        Dim guidList As SortedList(Of String, String)");
                    outputClassFile.WriteLine("        guidList = New SortedList(Of String, String)");

                    // Add the GUIDs in the list
                    foreach (KeyValuePair<string, string> guid in guidList)
                    {
                        TL.LogMessage("Main", "Adding to class: " + guid.Key + " in: " + guid.Value);
                        outputClassFile.WriteLine("        Try : guidList.Add(\"" + guid.Key + "\", \"" + guid.Value + "\") :TL.LogMessage(\"GUIDs\",\"Added GUID: \" + \"" + guid.Key + "\") : Catch ex As ArgumentException : TL.LogMessage(\"GUIDs\",\"Duplicate GUID: \" + \"" + guid.Key + "\"):End Try");
                        outputTextFile.WriteLine(guid.Key + " defined in: " + guid.Value);
                    }

                    // Add the closing lines of the GUIDs member
                    outputClassFile.WriteLine("        Return guidList");
                    outputClassFile.WriteLine("    End Function");
                    outputClassFile.WriteLine("");

                    // Add the first lines of the PlatformFiles member
                    outputClassFile.WriteLine("    Shared Function PlatformFiles(TL as TraceLogger) As SortedSet(Of String)");
                    outputClassFile.WriteLine("        Dim fileList As SortedSet(Of String)");
                    outputClassFile.WriteLine("        fileList = New SortedSet(Of String)");

                    // Add the files in the list
                    foreach (string file in platformFileList)
                    {
                        TL.LogMessage("Main", "Adding to class: " + file);
                        outputClassFile.WriteLine("        Try : fileList.Add(\"" + file + "\") :TL.LogMessage(\"PlatformFiles\",\"Added file: " + file + "\") : Catch ex As Exception : TL.LogMessage(\"PlatformFiles\",\"Exception: \" + ex.ToString()):End Try");
                    }

                    // Add the closing lines of the PlatformFiles member
                    outputClassFile.WriteLine("        Return fileList");
                    outputClassFile.WriteLine("    End Function");
                    outputClassFile.WriteLine("");


                    // Add the first lines of the DeveloperFiles member
                    outputClassFile.WriteLine("    Shared Function DeveloperFiles(TL as TraceLogger) As SortedSet(Of String)");
                    outputClassFile.WriteLine("        Dim fileList As SortedSet(Of String)");
                    outputClassFile.WriteLine("        fileList = New SortedSet(Of String)");

                    // Add the files in the list
                    foreach (string file in developerFileList)
                    {
                        TL.LogMessage("Main", "Adding to class: " + file);
                        outputClassFile.WriteLine("        Try : fileList.Add(\"" + file + "\") :TL.LogMessage(\"DeveloperFiles\",\"Added file: " + file + "\") : Catch ex As Exception : TL.LogMessage(\"PlatformFiles\",\"Exception: \" + ex.ToString()):End Try");
                    }

                    // Add the closing lines of the DeveloperFiles member
                    outputClassFile.WriteLine("        Return fileList");
                    outputClassFile.WriteLine("    End Function");
                    outputClassFile.WriteLine("");

                    // Add the closing lines of the DynamicLists class
                    outputClassFile.WriteLine("End Class");

                    // Close the files
                    outputTextFile.Flush();
                    outputTextFile.Close();
                    outputClassFile.Flush();
                    outputClassFile.Close();
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("Main", "Exception writing class file: " + ex.ToString());
                }
            }

        }

        static SortedSet<string> FindInstalledFiles(string installerTextFileFullName, TraceLogger TL)
        {
            SortedSet<String> fileList; // Variable to hold the returned list of Install File lines

            try
            {
                fileList = new SortedSet<string>(); // Create a SortedSet to hold the lines returned

                // Set up a regular expression to pick out the file name and install path from an Installaware Install Files line
                //                                 Comment: Install Files ..\..\..\..\ASCOM.Utilities\ASCOM.Utilities\bin\Release\ASCOM.Utilities.dll to $COMMONFILES$\ASCOM\Platform\v6
                // Groups within the matched line: <Comment>              <---------------------------------FileName-------------------------------->    <---------InstallPath---------> 
                Regex regexInstallFile = new Regex(@"\s*(?<Comment>[\w\W]*)\sInstall Files\s*(?<FileName>[\w\W]*) to (?<InstallPath>[\w\W]*)", RegexOptions.IgnoreCase);

                // Set up a regular expression to pick out the compiler variable from the InstallPath part of an Installaware Install Files line
                //                                $COMMONFILES$\ASCOM\Platform\v6
                // Group within the matched line: <--CompVar-->
                Regex regexInstallerVariables = new Regex(@"\$(?<CompVar>[\w]*)\$.*", RegexOptions.IgnoreCase);

                // Create the list of installer lines to be processed
                TL.LogMessage("FindInstalledFiles", "Reading installer file: " + installerTextFileFullName);
                List<string> lines = new List<string>(File.ReadAllLines(installerTextFileFullName)); // Get all Platform installer lines into a list

                // Iterate over the list of lines identifying "Install File" lines and recording them for use by the RemoveASCOM program
                foreach (string line in lines)
                {
                    Match m = regexInstallFile.Match(line); // Use the regular expression to search for a match
                    if (m.Success) // We have found an installed file
                    {
                        if (!m.Groups["Comment"].ToString().ToUpper().Contains("COMMENT:")) // Process non comment lines
                        {
                            TL.LogMessage("FindInstalledFiles", "Found installed file: " + m.Groups["FileName"].ToString() + " " + m.Groups["InstallPath"].ToString());

                            // Now check whether it has a variable
                            Match mVar = regexInstallerVariables.Match(m.Groups["InstallPath"].ToString());
                            if (mVar.Success) // Yes, we have a compiler variable
                            {
                                switch (mVar.Groups["CompVar"].ToString().ToUpper()) // Check that the variable is recognised 
                                {
                                    case "TARGETDIR": // These are the recognised variables for files that should be cleaned up by RemoveASCOM
                                    case "COMMONFILES":
                                    case "COMMONFILES64":
                                        TL.LogMessage("Main", "Found: " + mVar.Groups["CompVar"].ToString() + ", including this file");
                                        string targetFullFileName = m.Groups["InstallPath"].ToString() + @"\" + Path.GetFileName(m.Groups["FileName"].ToString());
                                        fileList.Add(targetFullFileName);
                                        break;
                                    case "WINSYSDIR": // These are the variables used where files should be left in place by RemoveASCOM
                                    case "WINDIR":
                                        TL.LogMessage("Main", "Found WINDIR or WINSYSDIR, ignoring this file");
                                        break;
                                    default: // Throw an error if a new variable is encountered so that it can be added to one of the preceeding groups.
                                        TL.LogMessage("FindInstalledFiles", "ERROR - Found UNKNOWN COMPILER VARIABLE: " + mVar.Groups["CompVar"].ToString() + " in line: " + line);
                                        MessageBox.Show("ERROR - Found UNKNOWN COMPILER VARIABLE: " + mVar.Groups["CompVar"].ToString() + " in line: " + line,
                                                        "MakeDynamicLists Build Environment Program", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        Environment.Exit(1);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            TL.LogMessage("FindInstalledFiles", "Ignoring comment line: " + line);
                        }
                    }
                    else // This is not an "Install Files" line so ignore it
                    {
                    }
                }

                return fileList;
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("FindInstalledFiles", "Exception reading installer text file: " + ex.ToString());
                return new SortedSet<string>();
            }
        }
    }
}
