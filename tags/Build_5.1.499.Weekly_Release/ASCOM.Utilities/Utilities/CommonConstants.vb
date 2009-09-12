'Common constants for the ASCOM.Utilities namesapce

Imports System.Reflection
Imports System.Runtime.InteropServices

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
        AssemblyInfo(TL, "Executing Assembly", Assembly.GetExecutingAssembly)
        AssemblyInfo(TL, "Entry Assembly", Assembly.GetEntryAssembly)
        AssemblyInfo(TL, "Calling Assembly", Assembly.GetCallingAssembly)

        'Get loaded assemblies
        Dim Assemblies() As Assembly 'Define an array of assembly information
        Dim AppDom As System.AppDomain = AppDomain.CurrentDomain
        Assemblies = AppDom.GetAssemblies 'Get a list of loaded assemblies
        For Each FoundAssembly As Assembly In Assemblies
            TL.LogMessage("Versions", "Loaded Assemblies: " & FoundAssembly.GetName.Name & " " & FoundAssembly.GetName.Version.ToString)
        Next

        TL.LogMessage("Versions", "CLR version: " & System.Environment.Version.ToString)
        AssemblyNames = Assembly.GetExecutingAssembly.GetReferencedAssemblies

        'Get Operating system information
        Dim OS As System.OperatingSystem = System.Environment.OSVersion
        TL.LogMessage("Versions", "OS Version " & OS.Platform & " Service Pack: " & OS.ServicePack & " Full: " & OS.VersionString)
        Select Case System.IntPtr.Size
            Case 4
                TL.LogMessage("Versions", "Operating system is 32bit")
            Case 8
                TL.LogMessage("Versions", "Operating system is 64bit")
            Case Else
                TL.LogMessage("Versions", "Operating system is unknown bits, PTR length is: " & System.IntPtr.Size)
        End Select

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

    Sub AssemblyInfo(ByVal TL As TraceLogger, ByVal AssName As String, ByVal Ass As Assembly)
        If Not Ass Is Nothing Then
            TL.LogMessage("Versions", AssName & " Version: " & Ass.GetName.Version.ToString)
            TL.LogMessage("Versions", AssName & " Name: " & Ass.GetName.FullName.ToString)
            TL.LogMessage("Versions", AssName & " CodeBase: " & Ass.GetName.CodeBase.ToString)
            TL.LogMessage("Versions", AssName & " Location: " & Ass.Location.ToString)
            TL.LogMessage("Versions", AssName & " From GAC: " & Ass.GlobalAssemblyCache.ToString)

        Else
            TL.LogMessage("Versions", AssName & " No assembly found")

        End If

    End Sub



End Module
#End Region