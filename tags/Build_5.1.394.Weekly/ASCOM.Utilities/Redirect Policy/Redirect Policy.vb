Imports System.Reflection
Module RedirectPolicy
    Private Const BATCH_FILE_NAME As String = "BuildPolicy1.Cmd"
    Private Const POLICY_FILE_NAME As String = "PublisherPolicy.xml"
    Private Const UTILITIES_ASSEMBLY_NAME As String = "ASCOM.Utilities" ' Don't add .dll
    Private Const UTILITIES_ASSEMBLY_DIRECTORY As String = "..\..\..\Utilities\Bin\Debug\"

    Private Const AL_LINK As String = POLICY_FILE_NAME
    'Private Const AL_OUT As String = "policy.5.0.ASCOM.Utilities.dll"
    Private Const AL_KEYFILE As String = "..\..\ASCOM.snk "
    'Private Const AL_VERSION As String = "5.0.0.0 "
    'Private Const AL_FILEVERSION As String = "5.0.0.0"
    Private Const AL_COMPANY As String = """ASCOM Initiative"""
    Private Const AL_PRODUCT As String = "ASCOM.Utilities"

    Sub Main()
        Dim ProfAss As System.Reflection.Assembly = Nothing
        Dim Ver As Version
        Dim Al_Version, Al_Out As String

        Console.WriteLine("Current Dir: " & My.Computer.FileSystem.CurrentDirectory & vbCrLf)
        Try
            ProfAss = Assembly.ReflectionOnlyLoadFrom(UTILITIES_ASSEMBLY_DIRECTORY & UTILITIES_ASSEMBLY_NAME & ".dll")
            Try
                Ver = ProfAss.GetName.Version
                Console.WriteLine("Found Major version " & Ver.Major)
                Console.WriteLine("Found Minor version " & Ver.Minor)
                Console.WriteLine("Found Build version " & Ver.Build)
                Console.WriteLine("Found Revision version " & Ver.Revision)
                Console.WriteLine("Found ToString " & Ver.ToString)
                Console.WriteLine("Found Major Revision version " & Ver.MajorRevision)
                Console.WriteLine("Found Minor Revision version " & Ver.MinorRevision)

                'Create the AL Out and Version strings
                Al_Out = "policy." & Ver.Major.ToString & "." & Ver.Minor.ToString & "." & UTILITIES_ASSEMBLY_NAME & ".dll"
                Al_Version = Ver.Major.ToString & "." & Ver.Minor.ToString & ".0.0"
                Try
                    'Create the publisher policy file that will configure redirection
                    Console.WriteLine(vbCrLf & "Writing policy xml file")
                    Try : My.Computer.FileSystem.DeleteFile(POLICY_FILE_NAME) : Catch : End Try    'Remove any existing file
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "<configuration>" & vbCrLf, False, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "   <runtime>" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "      <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "         <dependentAssembly>" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "            <assemblyIdentity name=""" & UTILITIES_ASSEMBLY_NAME & """ publicKeyToken=""565de7938946fba7"" culture=""neutral"" />" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "            <bindingRedirect oldVersion=""5.0.0.0-" & Ver.Major.ToString & "." & Ver.Minor.ToString & "." & (Ver.Build - 1).ToString & ".65535"" " & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "                             newVersion=""" & Ver.Major.ToString & "." & Ver.Minor.ToString & "." & Ver.Build.ToString & "." & Ver.Revision.ToString & """ />" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "         </dependentAssembly>" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "      </assemblyBinding>" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "   </runtime>" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                    My.Computer.FileSystem.WriteAllText(POLICY_FILE_NAME, "</configuration>" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)

                    Try
                        Console.WriteLine("Writing batch file")
                        'Create the batch file that will actually build the rediretion policy assembly
                        Try : My.Computer.FileSystem.DeleteFile(BATCH_FILE_NAME) : Catch : End Try 'Remove any existing file
                        My.Computer.FileSystem.WriteAllText(BATCH_FILE_NAME, "@Echo off" & vbCrLf, False, System.Text.ASCIIEncoding.ASCII) 'Suppress line echoing
                        'Get access to the AL command by setting the dev environment variables
                        My.Computer.FileSystem.WriteAllText(BATCH_FILE_NAME, "Call ""%VS90COMNTOOLS%vsvars32""" & vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                        'Link the file to create the policy assembly
                        My.Computer.FileSystem.WriteAllText(BATCH_FILE_NAME, "al /link:" & AL_LINK & _
                                                                             " /out:" & Al_Out & _
                                                                             " /keyfile:" & AL_KEYFILE & _
                                                                             " /version:" & Al_Version & _
                                                                             " /fileversion:" & Al_Version & _
                                                                             " /company:" & AL_COMPANY & _
                                                                             " /product:" & AL_PRODUCT & _
                                                                             vbCrLf, True, System.Text.ASCIIEncoding.ASCII)
                        Try
                            Console.WriteLine(vbCrLf & "Starting Link" & vbCrLf)
                            Shell(BATCH_FILE_NAME, AppWinStyle.NormalFocus, True, 10000)
                            Console.WriteLine("Completed Link")
                        Catch ex As Exception
                            Console.WriteLine("Exception running the batch file: " & ex.ToString)
                        End Try
                    Catch ex As Exception
                        Console.WriteLine("Exception writing publisher policy file: " & ex.ToString)
                    End Try
                Catch ex As Exception
                    Console.WriteLine("Exception writing batch file: " & ex.ToString)
                End Try
            Catch ex As Exception
                Console.WriteLine("Exception getting version information: " & ex.ToString)
            End Try
        Catch ex As Exception
            Console.WriteLine("Exception accessing Utilities assembly: " & ex.ToString)
        End Try
    End Sub

End Module
