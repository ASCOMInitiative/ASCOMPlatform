using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic;

namespace ASCOM.RedirectionPolicies
{
    static class RedirectPolicy
    {
        private const string BATCH_FILE_NAME = "BuildPolicy1.Cmd";
        private const string POLICY_FILE_NAME = "PublisherPolicy.xml";
        private const string UTILITIES_ASSEMBLY_NAME = "ASCOM.Utilities"; // Don't add .dll
        private const string UTILITIES_ASSEMBLY_DIRECTORY = @"..\..\..\..\ASCOM.Utilities\ASCOM.Utilities\Bin\Release\";

        private const string ASTROMETRY_ASSEMBLY_NAME = "ASCOM.Astrometry"; // Don't add .dll
        private const string ASTROMETRY_ASSEMBLY_DIRECTORY = @"..\..\..\..\ASCOM.Astrometry\ASCOM.Astrometry\Bin\Release\";

        private const string EXCEPTIONS_ASSEMBLY_NAME = "ASCOM.Exceptions"; // Don't add .dll
        private const string EXCEPTIONS_ASSEMBLY_DIRECTORY = @"..\..\..\..\Interfaces\ASCOMExceptions\bin\Release\";

        private const string AL_LINK = POLICY_FILE_NAME;
        // Private Const AL_OUT As String = "policy.5.0.ASCOM.Utilities.dll"
        private const string AL_KEYFILE = @"..\..\..\..\ASCOM.snk ";
        // Private Const AL_VERSION As String = "5.0.0.0 "
        // Private Const AL_FILEVERSION As String = "5.0.0.0"
        private const string AL_COMPANY = "\"ASCOM Initiative\"";
        private const string AL_PRODUCT = "\"ASCOM Platform\"";


        public static void Main()
        {
            try
            {
                Console.WriteLine("Writing batch file");
                // Create the batch file that will actually build the rediretion policy assembly
                try
                {
                    System.IO.File.Delete(BATCH_FILE_NAME);
                }
                catch
                {
                } // Remove any existing file
                File.WriteAllText(BATCH_FILE_NAME, "@Echo off" + "\r\n", System.Text.Encoding.ASCII); // Suppress line echoing
                                                                                                      // Get access to the AL command by setting the dev environment variables
                File.AppendAllText(BATCH_FILE_NAME, "Call \"%VS90COMNTOOLS%vsvars32\"" + "\r\n", System.Text.Encoding.ASCII);
                // Link the file to create the policy assembly

                Console.WriteLine("Current Dir: " + System.IO.Directory.GetCurrentDirectory() + "\r\n");
                Console.WriteLine("Assembly Dir: " + (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).Location);

                // Create the publisher policy file that will configure redirection
                Console.WriteLine("\r\n" + "Writing policy xml file");
                try
                {
                    System.IO.File.Delete(POLICY_FILE_NAME);
                }
                catch
                {
                }    // Remove any existing file
                File.WriteAllText(POLICY_FILE_NAME, "<configuration>" + "\r\n", System.Text.Encoding.ASCII);
                File.AppendAllText(POLICY_FILE_NAME, "   <runtime>" + "\r\n", System.Text.Encoding.ASCII);
                File.AppendAllText(POLICY_FILE_NAME, "      <assemblyBinding xmlns=\"urn:schemas-microsoft-com:asm.v1\">" + "\r\n", System.Text.Encoding.ASCII);

                // ProcessAssembly(UTILITIES_ASSEMBLY_DIRECTORY, UTILITIES_ASSEMBLY_NAME)
                // ProcessAssembly(ASTROMETRY_ASSEMBLY_DIRECTORY, ASTROMETRY_ASSEMBLY_NAME)
                ProcessAssembly(EXCEPTIONS_ASSEMBLY_DIRECTORY, EXCEPTIONS_ASSEMBLY_NAME);

                File.AppendAllText(POLICY_FILE_NAME, "      </assemblyBinding>" + "\r\n", System.Text.Encoding.ASCII);
                File.AppendAllText(POLICY_FILE_NAME, "   </runtime>" + "\r\n", System.Text.Encoding.ASCII);
                File.AppendAllText(POLICY_FILE_NAME, "</configuration>" + "\r\n", System.Text.Encoding.ASCII);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception writing batch file: " + ex.ToString());
            }

            try
            {
                Console.WriteLine("\r\n" + "Starting Link" + "\r\n");

                // Create a process to run the batch file
                Process process = new Process();

                // Set process parameters
                process.StartInfo.FileName = BATCH_FILE_NAME;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                // Start the process and wait up to 10 seconds for it to complete
                process.Start();
                process.WaitForExit(10000);
                Console.WriteLine("Completed Link");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception running the batch file: " + ex.ToString());
            }
        }

        public static void ProcessAssembly(string p_AssemblyDirectory, string p_AssemblyName)
        {
            Assembly ProfAss = null;
            Version Ver;
            string Al_Version, Al_Out;
            try
            {
                ProfAss = Assembly.ReflectionOnlyLoadFrom(p_AssemblyDirectory + p_AssemblyName + ".dll");
                try
                {
                    Ver = ProfAss.GetName().Version;
                    Console.WriteLine("Found Major version " + Ver.Major);
                    Console.WriteLine("Found Minor version " + Ver.Minor);
                    Console.WriteLine("Found Build version " + Ver.Build);
                    Console.WriteLine("Found Revision version " + Ver.Revision);
                    Console.WriteLine("Found ToString " + Ver.ToString());
                    Console.WriteLine("Found Major Revision version " + Ver.MajorRevision);
                    Console.WriteLine("Found Minor Revision version " + Ver.MinorRevision);

                    // Create the AL Out and Version strings
                    Al_Out = "policy." + Ver.Major.ToString() + "." + Ver.Minor.ToString() + "." + p_AssemblyName + ".dll";
                    Al_Version = Ver.Major.ToString() + "." + Ver.Minor.ToString() + ".0.0";
                    try
                    {
                        File.AppendAllText(POLICY_FILE_NAME, "         <dependentAssembly>" + "\r\n", System.Text.Encoding.ASCII);
                        File.AppendAllText(POLICY_FILE_NAME, "            <assemblyIdentity name=\"" + p_AssemblyName + "\" publicKeyToken=\"565de7938946fba7\" culture=\"neutral\" />" + "\r\n", System.Text.Encoding.ASCII);
                        File.AppendAllText(POLICY_FILE_NAME, "            <bindingRedirect oldVersion=\"1.0.0.0-" + Ver.Major.ToString() + "." + Ver.Minor.ToString() + "." + "65535.65535\" " + "\r\n", System.Text.Encoding.ASCII);
                        File.AppendAllText(POLICY_FILE_NAME, "                             newVersion=\"" + Ver.Major.ToString() + "." + Ver.Minor.ToString() + "." + Ver.Build.ToString() + "." + Ver.Revision.ToString() + "\" />" + "\r\n", System.Text.Encoding.ASCII);
                        File.AppendAllText(POLICY_FILE_NAME, "         </dependentAssembly>" + "\r\n", System.Text.Encoding.ASCII);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception writing publisher policy file: " + ex.ToString());
                    }
                    try
                    {
                        File.AppendAllText(BATCH_FILE_NAME, "al /link:" + AL_LINK + " /out:" + Al_Out + " /keyfile:" + AL_KEYFILE + " /version:" + Al_Version + " /fileversion:" + Al_Version + " /company:" + AL_COMPANY + " /product:" + AL_PRODUCT + "\r\n", System.Text.Encoding.ASCII);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception running the batch file: " + ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception getting version information: " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception accessing Utilities assembly: " + ex.ToString());
            }

        }

    }
}