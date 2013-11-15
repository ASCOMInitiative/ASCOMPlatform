using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace FindGUIDs
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
            string OutputPath = @"\Remove ASCOM\Remove ASCOM\";
            string devlopmentPath = developmentSearchPath + OutputPath;
            string outputClassFileName = @"GUIDList.vb";
            string outputTextFileName = @"GUIDList.txt";

            // Construct the development environment values
            string sourceSearchPath = developmentSearchPath;
            string outputClassFullFileName = devlopmentPath + outputClassFileName;
            string outputTextFullFileName = devlopmentPath + outputTextFileName;

            // Storage for the GUIDs found and a suppression list of GUIDs that should be left undisturbed
            SortedList<string, string> guidList = new SortedList<string, string>();
            List<string> suppressionList = new List<string>();

            using (TraceLogger TL = new TraceLogger("", "FindGUIDs"))
            {
                try
                {
                    TL.Enabled = true;

                    // Override the source and output paths if a command line search path is provided
                    if (args.Length > 0)
                    {
                        sourceSearchPath = args[0];
                        outputClassFullFileName = args[0] + OutputPath + outputClassFileName;
                        outputTextFullFileName = args[0] + OutputPath + outputTextFileName;
                    }

                    TL.LogMessage("Main", "Search path: " + Path.GetFullPath(sourceSearchPath));
                    TL.LogMessage("Main", "Output class file: " + Path.GetFullPath(outputClassFullFileName));
                    TL.LogMessage("Main", "Output text file: " + Path.GetFullPath(outputTextFullFileName));

                    // Add required items to the suppression list so that they are not deleted by RemoveASCOM when it runs
                    suppressionList.Add("00000001-0000-0000-C000-000000000046");
                    suppressionList.Add("00000002-0000-0000-C000-000000000046");

                    // Set up a regular expression for a GUID format e.g.:   Guid("0EF59E5C-2715-4E91-8A5E-38FE388B4F00")
                    // Regular expression groups within the matched GUID:    <-G1-><--------------G2------------------> 
                    // Group 2 picks out the GUID value inside the double quote characters. This is used as m.Groups[2] below
                    Regex g = new Regex(@"(Guid\(\"")([0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12})""", RegexOptions.IgnoreCase);

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
                                Match m = g.Match(line); // Use the regular expression to search for a match
                                if (m.Success) // We have found a GUID
                                {
                                    try
                                    {
                                        // Check whether this a GUID to suppress
                                        if (!suppressionList.Contains(m.Groups[2].ToString())) // Not suppressed
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

                    // Create the streamwriter to make the updated GUIDList class file and the readable txt file
                    StreamWriter outputTextFile = new StreamWriter(outputTextFullFileName);
                    StreamWriter outputClassFile = new StreamWriter(outputClassFullFileName);

                    // Add the first lines of the GUID text list
                    outputTextFile.WriteLine(@"This is a list of the GUIDs that will be removed by RemoveASCOM");
                    outputTextFile.WriteLine(" ");
                    
                    // Add the first lines of the GUIDList Class
                    outputClassFile.WriteLine(@"' ***** WARNING ***** WARNING ***** WARNING *****");
                    outputClassFile.WriteLine(" ");
                    outputClassFile.WriteLine(@"' This class is dynamically generated by the FindGUIDs Program.");
                    outputClassFile.WriteLine(@"' Do not alter this class, alter the FindGUIDs program instead and your changes will appear when the next build is made.");
                    outputClassFile.WriteLine(" ");
                    outputClassFile.WriteLine("Imports System.Collections.Generic");
                    outputClassFile.WriteLine(" ");
                    outputClassFile.WriteLine("Class GUIDList");
                    outputClassFile.WriteLine("    Shared Function Members(TL as TraceLogger) As SortedList(Of String, String)");
                    outputClassFile.WriteLine("        Dim guids As SortedList(Of String, String)");
                    outputClassFile.WriteLine("        guids = New SortedList(Of String, String)");

                    // Add the GUID lines in the list
                    foreach (KeyValuePair<string, string> guid in guidList)
                    {
                        TL.LogMessage("Main", "Adding to class: " + guid.Key + " in: " + guid.Value);
                        outputClassFile.WriteLine("        Try : guids.Add(\"" + guid.Key + "\", \"" + guid.Value + "\") :TL.LogMessage(\"GUIDLIst\",\"Added GUID: \" + \"" + guid.Key + "\") : Catch ex As ArgumentException : TL.LogMessage(\"GUIDLIst\",\"Duplicate GUID: \" + \"" + guid.Key + "\"):End Try");
                        outputTextFile.WriteLine(guid.Key + " defined in: " + guid.Value);
                    }

                    // Add the closgin lines of the class
                    outputClassFile.WriteLine("        Return guids");
                    outputClassFile.WriteLine("    End Function");
                    outputClassFile.WriteLine("End Class");

                    // Close the file
                    outputTextFile.Flush();
                    outputTextFile.Close();
                    outputClassFile.Flush();
                    outputClassFile.Close();

                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("Main", "Exception: " + ex.ToString());
                }
            }

        }
    }
}
