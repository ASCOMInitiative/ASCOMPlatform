'Common constants for the ASCOM.Utilities namesapce

Imports System.Reflection

#Region "Common Constants"

Module CommonConstants
    Friend Const SERIAL_FILE_NAME_VARNAME As String = "SerTraceFile" 'Constant naming the profile trace file variable name
    Friend Const SERIAL_AUTO_FILENAME As String = "C:\SerialTraceAuto.txt" 'Special value to indicate use of automatic trace filenames
    Friend Const SERIAL_DEFAULT_FILENAME As String = "C:\SerialTrace.txt" 'Default manual trace filename

    'Utilities configuration constants
    Friend Const TRACE_XMLACCESS As String = "Trace XMLAccess", TRACE_XMLACCESS_DEFAULT As Boolean = False
    Friend Const TRACE_PROFILE As String = "Trace Profile", TRACE_PROFILE_DEFAULT As Boolean = False
End Module

#End Region

#Region "Version Code"

Module VersionCode
    Friend Sub RunningVersions(ByVal TL As TraceLogger)
        Dim AssemblyNames() As AssemblyName
        TL.LogMessage("Versions", "Utilities version: " & Assembly.GetExecutingAssembly.GetName.Version.ToString)
        TL.LogMessage("Versions", "CLR version: " & System.Environment.Version.ToString)
        AssemblyNames = Assembly.GetExecutingAssembly.GetReferencedAssemblies

        'Get Operating system information
        Dim OS As System.OperatingSystem = System.Environment.OSVersion
        TL.LogMessage("Versions", "OS Version " & OS.Platform & " Service Pack: " & OS.ServicePack & " Full: " & OS.VersionString)
        'Get file system information
        Dim MachineName As String = System.Environment.MachineName
        Dim ProcCount As Integer = System.Environment.ProcessorCount
        Dim SysDir As String = System.Environment.SystemDirectory
        Dim WorkSet As Long = System.Environment.WorkingSet
        TL.LogMessage("Versions", "Machine name: " & MachineName & " Number of processors: " & ProcCount & " System directory: " & SysDir & " Working set size: " & WorkSet & " bytes")

        'Get fully qualified paths to particular directories in a non OS specific way
        'There are many more options in the SpecialFolders Enum than are shown here!
        TL.LogMessage("Versions", "Application Data: " & System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
        TL.LogMessage("Versions", "Common Files: " & System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles))
        TL.LogMessage("Versions", "My Documents: " & System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        TL.LogMessage("Versions", "Program Files: " & System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles))
        TL.LogMessage("Versions", "System: " & System.Environment.GetFolderPath(Environment.SpecialFolder.System))
        TL.LogMessage("Versions", "Current: " & System.Environment.CurrentDirectory)
    End Sub

End Module
#End Region