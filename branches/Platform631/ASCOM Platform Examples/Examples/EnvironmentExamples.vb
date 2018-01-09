Public Class EnvironmentExamples
    Sub Example()
        'This information can be incorporated in driver or application logs and is useful in analysing issues

        'Get version information for the common language runtime
        Dim CLRRuntime As System.Version = System.Environment.Version
        MsgBox("CLR Runtime version Major: " & CLRRuntime.Major & " Minor: " & CLRRuntime.Minor & " Full: " & CLRRuntime.ToString)

        'Get Operating system information
        Dim OS As System.OperatingSystem = System.Environment.OSVersion
        MsgBox("OS Version " & OS.Platform & " Service Pack: " & OS.ServicePack & " Full: " & OS.VersionString)

        'Get file system information
        Dim MachineName As String = System.Environment.MachineName
        Dim ProcCount As Integer = System.Environment.ProcessorCount
        Dim SysDir As String = System.Environment.SystemDirectory
        Dim WorkSet As Long = System.Environment.WorkingSet
        MsgBox("Machine name: " & MachineName & " Number of processors: " & ProcCount & " System directory: " & SysDir & " Working set size: " & WorkSet & " bytes")

        'Get fully qualified paths to particular directories in a non OS specific way
        'There are many more options in the SpecialFolders Enum than are shown here!
        Dim Path As String
        Path = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        Path = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles)
        Path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Path = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
        Path = System.Environment.GetFolderPath(Environment.SpecialFolder.System)
        Path = System.Environment.CurrentDirectory

        'Get the version number of your assembly
        Dim MyVersion As System.Version = System.Reflection.Assembly.GetExecutingAssembly.GetName.Version
        MsgBox("My assembly version is: " & MyVersion.ToString)

        'Get loaded assemblies used by your driver or client
        Dim Assemblies() As System.Reflection.Assembly 'Define an array of assembly information
        Dim AppDom As System.AppDomain = AppDomain.CurrentDomain
        Assemblies = AppDom.GetAssemblies 'Get a list of loaded assemblies
        For Each FoundAssembly In Assemblies 'Parse each to find if DriverAccess dll is one of them
            MsgBox("Found loaded assembly: " & FoundAssembly.GetName.Name & " " & FoundAssembly.GetName.Version.ToString)
        Next

        'Get the version of an assembly given its file location
        'This example is based around the driver toolkit, modify next line to identify the file you want to check
        Dim DriverAccessDLL As String = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ASCOM\.net\ASCOM.DriverAccess.dll"
        Dim DriverAccessAssembly As System.Reflection.Assembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(DriverAccessDLL)
        MsgBox("Found Client Toolkit File DLL Version: " & DriverAccessAssembly.GetName.Name & " " & DriverAccessAssembly.GetName.Version.ToString)

    End Sub
End Class
