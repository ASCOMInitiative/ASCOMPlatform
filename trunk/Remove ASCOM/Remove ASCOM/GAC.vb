' Source: Microsoft KB Article KB317540

'
'SUMMARY
'The native code application programming interfaces (APIs) that allow you to interact with the Global Assembly Cache (GAC) are not documented 
'in the .NET Framework Software Development Kit (SDK) documentation. 
'
'MORE INFORMATION
'CAUTION: Do not use these APIs in your application to perform assembly binds or to test for the presence of assemblies or other run time, 
'development, or design-time operations. Only administrative tools and setup programs must use these APIs. If you use the GAC, this directly 
'exposes your application to assembly binding fragility or may cause your application to work improperly on future versions of the .NET 
'Framework.
'
'The GAC stores assemblies that are shared across all applications on a computer. The actual storage location and structure of the GAC is 
'not documented and is subject to change in future versions of the .NET Framework and the Microsoft Windows operating system.
'
'The only supported method to access assemblies in the GAC is through the APIs that are documented in this article.
'
'Most applications do not have to use these APIs because the assembly binding is performed automatically by the common language runtime. 
'Only custom setup programs or management tools must use these APIs. Microsoft Windows Installer has native support for installing assemblies
' to the GAC.
'
'For more information about assemblies and the GAC, see the .NET Framework SDK.
'
'Use the GAC API in the following scenarios: 
'When you install an assembly to the GAC.
'When you remove an assembly from the GAC.
'When you export an assembly from the GAC.
'When you enumerate assemblies that are available in the GAC.
'NOTE: CoInitialize(Ex) must be called before you use any of the functions and interfaces that are described in this specification. 
'

Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Globalization
Imports System.Runtime.InteropServices.ComTypes

#Region "Flags"

''' <summary>
''' <see cref="IAssemblyName.GetDisplayName"/>
''' </summary>
<Flags()> _
Public Enum ASM_DISPLAY_FLAGS
    VERSION = &H1
    CULTURE = &H2
    PUBLIC_KEY_TOKEN = &H4
    PUBLIC_KEY = &H8
    [CUSTOM] = &H10
    PROCESSORARCHITECTURE = &H20
    LANGUAGEID = &H40
End Enum

<Flags()> _
Public Enum ASM_CMP_FLAGS
    NAME = &H1
    MAJOR_VERSION = &H2
    MINOR_VERSION = &H4
    BUILD_NUMBER = &H8
    REVISION_NUMBER = &H10
    PUBLIC_KEY_TOKEN = &H20
    CULTURE = &H40
    [CUSTOM] = &H80
    ALL = NAME Or MAJOR_VERSION Or MINOR_VERSION Or REVISION_NUMBER Or BUILD_NUMBER Or PUBLIC_KEY_TOKEN Or CULTURE Or [CUSTOM]
    [DEFAULT] = &H100
End Enum

''' <summary>
''' The ASM_NAME enumeration property ID describes the valid names of the name-value pairs in an assembly name. 
''' See the .NET Framework SDK for a description of these properties. 
''' </summary>
Public Enum ASM_NAME
    ASM_NAME_PUBLIC_KEY = 0
    ASM_NAME_PUBLIC_KEY_TOKEN
    ASM_NAME_HASH_VALUE
    ASM_NAME_NAME
    ASM_NAME_MAJOR_VERSION
    ASM_NAME_MINOR_VERSION
    ASM_NAME_BUILD_NUMBER
    ASM_NAME_REVISION_NUMBER
    ASM_NAME_CULTURE
    ASM_NAME_PROCESSOR_ID_ARRAY
    ASM_NAME_OSINFO_ARRAY
    ASM_NAME_HASH_ALGID
    ASM_NAME_ALIAS
    ASM_NAME_CODEBASE_URL
    ASM_NAME_CODEBASE_LASTMOD
    ASM_NAME_NULL_PUBLIC_KEY
    ASM_NAME_NULL_PUBLIC_KEY_TOKEN
    ASM_NAME_CUSTOM
    ASM_NAME_NULL_CUSTOM
    ASM_NAME_MVID
    ASM_NAME_MAX_PARAMS
End Enum

''' <summary>
''' <see cref="IAssemblyCache.UninstallAssembly"/>
''' </summary>
Public Enum IASSEMBLYCACHE_UNINSTALL_DISPOSITION
    IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNKNOWN = 0
    'Added by PWGS not officially part of the ENUM
    IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED = 1
    IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE = 2
    IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED = 3
    IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING = 4
    IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES = 5
    IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND = 6
End Enum

''' <summary>
''' <see cref="IAssemblyCache.QueryAssemblyInfo"/>
''' </summary>
Public Enum QUERYASMINFO_FLAG
    QUERYASMINFO_FLAG_VALIDATE = 1
    QUERYASMINFO_FLAG_GETSIZE = 2
End Enum

''' <summary>
''' <see cref="IAssemblyCache.InstallAssembly"/>
''' </summary>
Public Enum IASSEMBLYCACHE_INSTALL_FLAG
    IASSEMBLYCACHE_INSTALL_FLAG_REFRESH = 1
    IASSEMBLYCACHE_INSTALL_FLAG_FORCE_REFRESH = 2
End Enum

''' <summary>
''' The CREATE_ASM_NAME_OBJ_FLAGS enumeration contains the following values: 
'''	CANOF_PARSE_DISPLAY_NAME - If this flag is specified, the szAssemblyName parameter is a full assembly name and is parsed to 
'''		the individual properties. If the flag is not specified, szAssemblyName is the "Name" portion of the assembly name.
'''	CANOF_SET_DEFAULT_VALUES - If this flag is specified, certain properties, such as processor architecture, are set to 
'''		their default values.
'''	<see cref="AssemblyCache.CreateAssemblyNameObject"/>
''' </summary>
Public Enum CREATE_ASM_NAME_OBJ_FLAGS
    CANOF_PARSE_DISPLAY_NAME = &H1
    CANOF_SET_DEFAULT_VALUES = &H2
End Enum

''' <summary>
''' The ASM_CACHE_FLAGS enumeration contains the following values: 
''' ASM_CACHE_ZAP - Enumerates the cache of precompiled assemblies by using Ngen.exe.
''' ASM_CACHE_GAC - Enumerates the GAC.
''' ASM_CACHE_DOWNLOAD - Enumerates the assemblies that have been downloaded on-demand or that have been shadow-copied.
''' </summary>
<Flags()> _
Public Enum ASM_CACHE_FLAGS
    ASM_CACHE_ZAP = &H1
    ASM_CACHE_GAC = &H2
    ASM_CACHE_DOWNLOAD = &H4
End Enum


#End Region

#Region "Structs"

''' <summary>
''' The FUSION_INSTALL_REFERENCE structure represents a reference that is made when an application has installed an 
''' assembly in the GAC. 
''' The fields of the structure are defined as follows: 
'''		cbSize - The size of the structure in bytes.
'''		dwFlags - Reserved, must be zero.
'''		guidScheme - The entity that adds the reference.
'''		szIdentifier - A unique string that identifies the application that installed the assembly.
'''		szNonCannonicalData - A string that is only understood by the entity that adds the reference. 
'''				The GAC only stores this string.
''' Possible values for the guidScheme field can be one of the following: 
'''		FUSION_REFCOUNT_MSI_GUID - The assembly is referenced by an application that has been installed by using 
'''				Windows Installer. The szIdentifier field is set to MSI, and szNonCannonicalData is set to Windows Installer. 
'''				This scheme must only be used by Windows Installer itself.
'''		FUSION_REFCOUNT_UNINSTALL_SUBKEY_GUID - The assembly is referenced by an application that appears in Add/Remove 
'''				Programs. The szIdentifier field is the token that is used to register the application with Add/Remove programs.
'''		FUSION_REFCOUNT_FILEPATH_GUID - The assembly is referenced by an application that is represented by a file in 
'''				the file system. The szIdentifier field is the path to this file.
'''		FUSION_REFCOUNT_OPAQUE_STRING_GUID - The assembly is referenced by an application that is only represented 
'''				by an opaque string. The szIdentifier is this opaque string. The GAC does not perform existence checking 
'''				for opaque references when you remove this.
''' </summary>
<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
Public Structure FUSION_INSTALL_REFERENCE
    Public cbSize As UInteger
    Public dwFlags As UInteger
    Public guidScheme As Guid
    Public szIdentifier As String
    Public szNonCannonicalData As String
End Structure

''' <summary>
''' The ASSEMBLY_INFO structure represents information about an assembly in the assembly cache. 
''' The fields of the structure are defined as follows: 
'''		cbAssemblyInfo - Size of the structure in bytes. Permits additions to the structure in future version of the .NET Framework.
'''		dwAssemblyFlags - Indicates one or more of the ASSEMBLYINFO_FLAG_* bits.
'''		uliAssemblySizeInKB - The size of the files that make up the assembly in kilobytes (KB).
'''		pszCurrentAssemblyPathBuf - A pointer to a string buffer that holds the current path of the directory that contains the 
'''				files that make up the assembly. The path must end with a zero.
'''		cchBuf - Size of the buffer that the pszCurrentAssemblyPathBug field points to.
'''	dwAssemblyFlags can have one of the following values: 
'''		ASSEMBLYINFO_FLAG__INSTALLED - Indicates that the assembly is actually installed. Always set in current version of the 
'''				.NET Framework.
'''		ASSEMBLYINFO_FLAG__PAYLOADRESIDENT - Never set in the current version of the .NET Framework.
''' </summary>
<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
Public Structure ASSEMBLY_INFO
    Public cbAssemblyInfo As UInteger
    Public dwAssemblyFlags As UInteger
    Public uliAssemblySizeInKB As ULong
    Public pszCurrentAssemblyPathBuf As String
    Public cchBuf As UInteger
End Structure

Public Structure REMOVE_OUTCOME
    Public ReturnCode As Integer
    Public Disposition As IASSEMBLYCACHE_UNINSTALL_DISPOSITION
End Structure


#End Region

Public Class AssemblyCache
#Region "DLL Entries"

    ''' <summary>
    ''' The key entry point for reading the assembly cache.
    ''' </summary>
    ''' <param name="ppAsmCache">Pointer to return IAssemblyCache</param>
    ''' <param name="dwReserved">must be 0</param>
    <DllImport("fusion.dll", SetLastError:=True, PreserveSig:=False)> _
    Private Shared Sub CreateAssemblyCache(ByRef ppAsmCache As IAssemblyCache, dwReserved As UInteger)
    End Sub

    ''' <summary>
    ''' An instance of IAssemblyName is obtained by calling the CreateAssemblyNameObject API.
    ''' </summary>
    ''' <param name="ppAssemblyNameObj">Pointer to a memory location that receives the IAssemblyName pointer that is created.</param>
    ''' <param name="szAssemblyName">A string representation of the assembly name or of a full assembly reference that is 
    ''' determined by dwFlags. The string representation can be null.</param>
    ''' <param name="dwFlags">Zero or more of the bits that are defined in the CREATE_ASM_NAME_OBJ_FLAGS enumeration.</param>
    ''' <param name="pvReserved"> Must be null.</param>
    <DllImport("fusion.dll", SetLastError:=True, CharSet:=CharSet.Unicode, PreserveSig:=False)> _
    Private Shared Sub CreateAssemblyNameObject(ByRef ppAssemblyNameObj As IAssemblyName, szAssemblyName As String, dwFlags As UInteger, pvReserved As IntPtr)
    End Sub

    ''' <summary>
    ''' To obtain an instance of the CreateAssemblyEnum API, call the CreateAssemblyNameObject API.
    ''' </summary>
    ''' <param name="pEnum">Pointer to a memory location that contains the IAssemblyEnum pointer.</param>
    ''' <param name="pUnkReserved">Must be null.</param>
    ''' <param name="pName">An assembly name that is used to filter the enumeration. Can be null to enumerate all assemblies in the GAC.</param>
    ''' <param name="dwFlags">Exactly one bit from the ASM_CACHE_FLAGS enumeration.</param>
    ''' <param name="pvReserved">Must be NULL.</param>
    <DllImport("fusion.dll", SetLastError:=True, PreserveSig:=False)> _
    Private Shared Sub CreateAssemblyEnum(ByRef pEnum As IAssemblyEnum, pUnkReserved As IntPtr, pName As IAssemblyName, dwFlags As ASM_CACHE_FLAGS, pvReserved As IntPtr)
    End Sub

    ''' <summary>
    ''' To obtain an instance of the CreateInstallReferenceEnum API, call the CreateInstallReferenceEnum API.
    ''' </summary>
    ''' <param name="ppRefEnum">A pointer to a memory location that receives the IInstallReferenceEnum pointer.</param>
    ''' <param name="pName">The assembly name for which the references are enumerated.</param>
    ''' <param name="dwFlags"> Must be zero.</param>
    ''' <param name="pvReserved">Must be null.</param>
    <DllImport("fusion.dll", SetLastError:=True, PreserveSig:=False)> _
    Private Shared Sub CreateInstallReferenceEnum(ByRef ppRefEnum As IInstallReferenceEnum, pName As IAssemblyName, dwFlags As UInteger, pvReserved As IntPtr)
    End Sub

    ''' <summary>
    ''' The GetCachePath API returns the storage location of the GAC. 
    ''' </summary>
    ''' <param name="dwCacheFlags">Exactly one of the bits defined in the ASM_CACHE_FLAGS enumeration.</param>
    ''' <param name="pwzCachePath">Pointer to a buffer that is to receive the path of the GAC as a Unicode string.</param>
    ''' <param name="pcchPath">Length of the pwszCachePath buffer, in Unicode characters.</param>
    <DllImport("fusion.dll", SetLastError:=True, CharSet:=CharSet.Unicode, PreserveSig:=False)> _
    Private Shared Sub GetCachePath(dwCacheFlags As ASM_CACHE_FLAGS, <MarshalAs(UnmanagedType.LPWStr)> pwzCachePath As StringBuilder, ByRef pcchPath As UInteger)
    End Sub

#End Region

#Region "GUID Definition"

    ''' <summary>
    ''' GUID value for element guidScheme in the struct FUSION_INSTALL_REFERENCE
    ''' The assembly is referenced by an application that has been installed by using Windows Installer. 
    ''' The szIdentifier field is set to MSI, and szNonCannonicalData is set to Windows Installer. 
    ''' This scheme must only be used by Windows Installer itself.
    ''' </summary>
    Public Shared ReadOnly Property FUSION_REFCOUNT_UNINSTALL_SUBKEY_GUID() As Guid
        Get
            Return New Guid("8cedc215-ac4b-488b-93c0-a50a49cb2fb8")
        End Get
    End Property

    ''' <summary>
    ''' GUID value for element guidScheme in the struct FUSION_INSTALL_REFERENCE
    ''' 
    ''' </summary>
    Public Shared ReadOnly Property FUSION_REFCOUNT_FILEPATH_GUID() As Guid
        Get
            Return New Guid("b02f9d65-fb77-4f7a-afa5-b391309f11c9")
        End Get
    End Property

    ''' <summary>
    ''' GUID value for element guidScheme in the struct FUSION_INSTALL_REFERENCE
    ''' 
    ''' </summary>
    Public Shared ReadOnly Property FUSION_REFCOUNT_OPAQUE_STRING_GUID() As Guid
        Get
            Return New Guid("2ec93463-b0c3-45e1-8364-327e96aea856")
        End Get
    End Property

    ''' <summary>
    ''' GUID value for element guidScheme in the struct FUSION_INSTALL_REFERENCE
    ''' 
    ''' </summary>
    Public Shared ReadOnly Property FUSION_REFCOUNT_MSI_GUID() As Guid
        Get
            Return New Guid("25df0fc1-7f97-4070-add7-4b13bbfd7cb8")
        End Get
    End Property

#End Region

#Region "Public Functions for DLL - Assembly Cache"

    ''' <summary>
    ''' Use this method as a start for the GAC API
    ''' </summary>
    ''' <returns>IAssemblyCache COM interface</returns>
    Public Shared Function CreateAssemblyCache() As IAssemblyCache
        Dim ac As IAssemblyCache = Nothing

        AssemblyCache.CreateAssemblyCache(ac, 0)

        Return ac
    End Function

#End Region

#Region "Public Functions for DLL - AssemblyName"

    Public Shared Function CreateAssemblyName(name As String) As IAssemblyName
        Dim an As IAssemblyName = Nothing

        AssemblyCache.CreateAssemblyNameObject(an, name, 2, CType(0, IntPtr))

        Return an
    End Function

    Public Shared Function GetDisplayName(name As IAssemblyName, which As ASM_DISPLAY_FLAGS) As [String]
        Dim bufferSize As UInteger = 255
        Dim buffer As New StringBuilder(CInt(bufferSize))
        name.GetDisplayName(buffer, bufferSize, which)
        Return buffer.ToString()
    End Function

    Public Shared Function GetName(name As IAssemblyName) As [String]
        Dim bufferSize As UInteger = 255
        Dim buffer As New StringBuilder(CInt(bufferSize))
        name.GetName(bufferSize, buffer)
        Return buffer.ToString()
    End Function

    Public Shared Function GetVersion(name As IAssemblyName) As Version
        Dim major As UInteger
        Dim minor As UInteger
        name.GetVersion(major, minor)
        Return New Version(CInt(major >> 16), CInt(major And &HFFFF), CInt(minor >> 16), CInt(minor And &HFFFF))
    End Function

    Public Shared Function GetPublicKeyToken(name As IAssemblyName) As Byte()
        Dim result As Byte() = New Byte(7) {}
        Dim bufferSize As UInteger = 8
        Dim buffer As IntPtr = Marshal.AllocHGlobal(CInt(bufferSize))
        name.GetProperty(ASM_NAME.ASM_NAME_PUBLIC_KEY_TOKEN, buffer, bufferSize)
        For i As Integer = 0 To 7
            result(i) = Marshal.ReadByte(buffer, i)
        Next
        Marshal.FreeHGlobal(buffer)
        Return result
    End Function

    Public Shared Function GetPublicKey(name As IAssemblyName) As Byte()
        Dim bufferSize As UInteger = 512
        Dim buffer As IntPtr = Marshal.AllocHGlobal(CInt(bufferSize))
        name.GetProperty(ASM_NAME.ASM_NAME_PUBLIC_KEY, buffer, bufferSize)
        Dim result As Byte() = New Byte(bufferSize - 1) {}
        For i As Integer = 0 To bufferSize - 1
            result(i) = Marshal.ReadByte(buffer, i)
        Next
        Marshal.FreeHGlobal(buffer)
        Return result
    End Function

    Public Shared Function GetCulture(name As IAssemblyName) As CultureInfo
        Dim bufferSize As UInteger = 255
        Dim buffer As IntPtr = Marshal.AllocHGlobal(CInt(bufferSize))
        name.GetProperty(ASM_NAME.ASM_NAME_CULTURE, buffer, bufferSize)
        Dim result As String = Marshal.PtrToStringAuto(buffer)
        Marshal.FreeHGlobal(buffer)
        Return New CultureInfo(result)
    End Function

#End Region

#Region "Public Functions for DLL - AssemblyEnum"

    Public Shared Function CreateGACEnum() As IAssemblyEnum
        Dim ae As IAssemblyEnum = Nothing

        AssemblyCache.CreateAssemblyEnum(ae, CType(0, IntPtr), Nothing, ASM_CACHE_FLAGS.ASM_CACHE_GAC, CType(0, IntPtr))

        Return ae
    End Function

    ''' <summary>
    ''' Get the next assembly name in the current enumerator or fail
    ''' </summary>
    ''' <param name="enumerator"></param>
    ''' <param name="name"></param>
    ''' <returns>0 if the enumeration is not at its end</returns>
    Public Shared Function GetNextAssembly(enumerator As IAssemblyEnum, ByRef name As IAssemblyName) As Integer
        Return enumerator.GetNextAssembly(CType(0, IntPtr), name, 0)
    End Function

    Public Shared Function GetGACPath() As [String]
        Dim bufferSize As UInteger = 255
        Dim buffer As New StringBuilder(CInt(bufferSize))
        AssemblyCache.GetCachePath(ASM_CACHE_FLAGS.ASM_CACHE_GAC, buffer, bufferSize)
        Return buffer.ToString()
    End Function

    Public Shared Function GetZapPath() As [String]
        Dim bufferSize As UInteger = 255
        Dim buffer As New StringBuilder(CInt(bufferSize))
        AssemblyCache.GetCachePath(ASM_CACHE_FLAGS.ASM_CACHE_ZAP, buffer, bufferSize)
        Return buffer.ToString()
    End Function

    Public Shared Function GetDownloadPath() As [String]
        Dim bufferSize As UInteger = 255
        Dim buffer As New StringBuilder(CInt(bufferSize))
        AssemblyCache.GetCachePath(ASM_CACHE_FLAGS.ASM_CACHE_DOWNLOAD, buffer, bufferSize)
        Return buffer.ToString()
    End Function

#End Region

    Public Shared Function RemoveGAC(AssemblyName As String) As REMOVE_OUTCOME
        Dim retval As New REMOVE_OUTCOME()

        Dim pCache As IAssemblyCache = AssemblyCache.CreateAssemblyCache()
        ' Get an IAssemblyCache interface
        Dim puldisposition As IASSEMBLYCACHE_UNINSTALL_DISPOSITION = IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNKNOWN
        ' Initialise variable
        'Next line changed so that no install reference is provided which allows easy uninstall from Assembly explorer display
        'int result = pCache.UninstallAssembly(0, filename, installReference, out puldisposition);
        Dim result As Integer = pCache.UninstallAssembly(0, AssemblyName, Nothing, puldisposition)
        retval.ReturnCode = result
        retval.Disposition = puldisposition
        Return retval
    End Function
End Class

#Region "COM Interface Definitions"

''' <summary>
''' The IAssemblyCache interface is the top-level interface that provides access to the GAC.
''' </summary>
<ComImport(), Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IAssemblyCache
    ''' <summary>
    ''' The IAssemblyCache::UninstallAssembly method removes a reference to an assembly from the GAC. 
    ''' If other applications hold no other references to the assembly, the files that make up the assembly are removed from the GAC. 
    ''' </summary>
    ''' <param name="dwFlags">No flags defined. Must be zero.</param>
    ''' <param name="pszAssemblyName">The name of the assembly. A zero-ended Unicode string.</param>
    ''' <param name="pRefData">A pointer to a FUSION_INSTALL_REFERENCE structure. Although this is not recommended, 
    '''		this parameter can be null. The assembly is installed without an application reference, or all existing application 
    '''		references are gone.</param>
    ''' <param name="pulDisposition">Pointer to an integer that indicates the action that is performed by the function.</param>
    ''' <returns>The return values are defined as follows: 
    '''		S_OK - The assembly has been uninstalled.
    '''		S_FALSE - The operation succeeded, but the assembly was not removed from the GAC. 
    '''		The reason is described in pulDisposition.</returns>
    '''	<remarks>
    '''	NOTE: If pulDisposition is not null, pulDisposition contains one of the following values:
    '''		IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED - The assembly files have been removed from the GAC.
    '''		IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE - An application is using the assembly. 
    '''			This value is returned on Microsoft Windows 95 and Microsoft Windows 98.
    '''		IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED - The assembly does not exist in the GAC.
    '''		IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING - Not used.
    '''		IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES - The assembly has not been removed from the GAC because 
    '''			another application reference exists.
    '''		IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND - The reference that is specified in pRefData is not found 
    '''			in the GAC.
    '''	</remarks>
    <PreserveSig()> _
    Function UninstallAssembly(dwFlags As UInteger, <MarshalAs(UnmanagedType.LPWStr)> pszAssemblyName As String, <MarshalAs(UnmanagedType.LPArray)> pRefData As FUSION_INSTALL_REFERENCE(), ByRef pulDisposition As IASSEMBLYCACHE_UNINSTALL_DISPOSITION) As Integer

    ''' <summary>
    ''' The IAssemblyCache::QueryAssemblyInfo method retrieves information about an assembly from the GAC. 
    ''' </summary>
    ''' <param name="dwFlags">One of QUERYASMINFO_FLAG_VALIDATE or QUERYASMINFO_FLAG_GETSIZE: 
    '''		*_VALIDATE - Performs validation of the files in the GAC against the assembly manifest, including hash verification 
    '''			and strong name signature verification.
    '''		*_GETSIZE - Returns the size of all files in the assembly (disk footprint). If this is not specified, the 
    '''			ASSEMBLY_INFO::uliAssemblySizeInKB field is not modified.</param>
    ''' <param name="pszAssemblyName"></param>
    ''' <param name="pAsmInfo"></param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function QueryAssemblyInfo(dwFlags As UInteger, <MarshalAs(UnmanagedType.LPWStr)> pszAssemblyName As String, ByRef pAsmInfo As ASSEMBLY_INFO) As Integer

    ''' <summary>
    ''' Undocumented
    ''' </summary>
    ''' <param name="dwFlags"></param>
    ''' <param name="pvReserved"></param>
    ''' <param name="ppAsmItem"></param>
    ''' <param name="pszAssemblyName"></param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function CreateAssemblyCacheItem(dwFlags As UInteger, pvReserved As IntPtr, ByRef ppAsmItem As IAssemblyCacheItem, <MarshalAs(UnmanagedType.LPWStr)> pszAssemblyName As String) As Integer

    ''' <summary>
    ''' Undocumented
    ''' </summary>
    ''' <param name="ppAsmScavenger"></param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function CreateAssemblyScavenger(<MarshalAs(UnmanagedType.IUnknown)> ByRef ppAsmScavenger As Object) As Integer

    ''' <summary>
    ''' The IAssemblyCache::InstallAssembly method adds a new assembly to the GAC. The assembly must be persisted in the file 
    ''' system and is copied to the GAC.
    ''' </summary>
    ''' <param name="dwFlags">At most, one of the bits of the IASSEMBLYCACHE_INSTALL_FLAG_* values can be specified: 
    '''		*_REFRESH - If the assembly is already installed in the GAC and the file version numbers of the assembly being 
    '''		installed are the same or later, the files are replaced.
    '''		*_FORCE_REFRESH - The files of an existing assembly are overwritten regardless of their version number.</param>
    ''' <param name="pszManifestFilePath"> A string pointing to the dynamic-linked library (DLL) that contains the assembly manifest. 
    '''	Other assembly files must reside in the same directory as the DLL that contains the assembly manifest.</param>
    ''' <param name="pRefData">A pointer to a FUSION_INSTALL_REFERENCE that indicates the application on whose behalf the 
    ''' assembly is being installed. Although this is not recommended, this parameter can be null, but this leaves the assembly 
    ''' without any application reference.</param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function InstallAssembly(dwFlags As UInteger, <MarshalAs(UnmanagedType.LPWStr)> pszManifestFilePath As String, <MarshalAs(UnmanagedType.LPArray)> pRefData As FUSION_INSTALL_REFERENCE()) As Integer
End Interface


''' <summary>
''' The IAssemblyName interface represents an assembly name. An assembly name includes a predetermined set of name-value pairs. 
''' The assembly name is described in detail in the .NET Framework SDK.
''' </summary>
<ComImport(), Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IAssemblyName
    ''' <summary>
    ''' The IAssemblyName::SetProperty method adds a name-value pair to the assembly name, or, if a name-value pair 
    ''' with the same name already exists, modifies or deletes the value of a name-value pair.
    ''' </summary>
    ''' <param name="PropertyId">The ID that represents the name part of the name-value pair that is to be 
    ''' added or to be modified. Valid property IDs are defined in the ASM_NAME enumeration.</param>
    ''' <param name="pvProperty">A pointer to a buffer that contains the value of the property.</param>
    ''' <param name="cbProperty">The length of the pvProperty buffer in bytes. If cbProperty is zero, the name-value pair 
    ''' is removed from the assembly name.</param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function SetProperty(PropertyId As ASM_NAME, pvProperty As IntPtr, cbProperty As UInteger) As Integer

    ''' <summary>
    ''' The IAssemblyName::GetProperty method retrieves the value of a name-value pair in the assembly name that specifies the name.
    ''' </summary>
    ''' <param name="PropertyId">The ID that represents the name of the name-value pair whose value is to be retrieved.
    ''' Specified property IDs are defined in the ASM_NAME enumeration.</param>
    ''' <param name="pvProperty">A pointer to a buffer that is to contain the value of the property.</param>
    ''' <param name="pcbProperty">The length of the pvProperty buffer, in bytes.</param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function GetProperty(PropertyId As ASM_NAME, pvProperty As IntPtr, ByRef pcbProperty As UInteger) As Integer

    ''' <summary>
    ''' The IAssemblyName::Finalize method freezes an assembly name. Additional calls to IAssemblyName::SetProperty are 
    ''' unsuccessful after this method has been called.
    ''' </summary>
    ''' <returns></returns>
    <PreserveSig()> _
    Overloads Function Finalize() As Integer

    ''' <summary>
    ''' The IAssemblyName::GetDisplayName method returns a string representation of the assembly name.
    ''' </summary>
    ''' <param name="szDisplayName">A pointer to a buffer that is to contain the display name. The display name is returned in Unicode.</param>
    ''' <param name="pccDisplayName">The size of the buffer in characters (on input). The length of the returned display name (on return).</param>
    ''' <param name="dwDisplayFlags">One or more of the bits defined in the ASM_DISPLAY_FLAGS enumeration: 
    '''		*_VERSION - Includes the version number as part of the display name.
    '''		*_CULTURE - Includes the culture.
    '''		*_PUBLIC_KEY_TOKEN - Includes the public key token.
    '''		*_PUBLIC_KEY - Includes the public key.
    '''		*_CUSTOM - Includes the custom part of the assembly name.
    '''		*_PROCESSORARCHITECTURE - Includes the processor architecture.
    '''		*_LANGUAGEID - Includes the language ID.</param>
    ''' <returns></returns>
    ''' <remarks>http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpguide/html/cpcondefaultmarshalingforstrings.asp</remarks>
    <PreserveSig()> _
    Function GetDisplayName(<Out(), MarshalAs(UnmanagedType.LPWStr)> szDisplayName As StringBuilder, ByRef pccDisplayName As UInteger, dwDisplayFlags As ASM_DISPLAY_FLAGS) As Integer

    ''' <summary>
    ''' Undocumented
    ''' </summary>
    ''' <param name="refIID"></param>
    ''' <param name="pUnkSink"></param>
    ''' <param name="pUnkContext"></param>
    ''' <param name="szCodeBase"></param>
    ''' <param name="llFlags"></param>
    ''' <param name="pvReserved"></param>
    ''' <param name="cbReserved"></param>
    ''' <param name="ppv"></param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function BindToObject(ByRef refIID As Guid, <MarshalAs(UnmanagedType.IUnknown)> pUnkSink As Object, <MarshalAs(UnmanagedType.IUnknown)> pUnkContext As Object, <MarshalAs(UnmanagedType.LPWStr)> szCodeBase As String, llFlags As Long, pvReserved As IntPtr, _
   cbReserved As UInteger, ByRef ppv As IntPtr) As Integer

    ''' <summary>
    ''' The IAssemblyName::GetName method returns the name part of the assembly name.
    ''' </summary>
    ''' <param name="lpcwBuffer">Size of the pwszName buffer (on input). Length of the name (on return).</param>
    ''' <param name="pwzName">Pointer to the buffer that is to contain the name part of the assembly name.</param>
    ''' <returns></returns>
    ''' <remarks>http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpguide/html/cpcondefaultmarshalingforstrings.asp</remarks>
    <PreserveSig()> _
    Function GetName(ByRef lpcwBuffer As UInteger, <Out(), MarshalAs(UnmanagedType.LPWStr)> pwzName As StringBuilder) As Integer

    ''' <summary>
    ''' The IAssemblyName::GetVersion method returns the version part of the assembly name.
    ''' </summary>
    ''' <param name="pdwVersionHi">Pointer to a DWORD that contains the upper 32 bits of the version number.</param>
    ''' <param name="pdwVersionLow">Pointer to a DWORD that contain the lower 32 bits of the version number.</param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function GetVersion(ByRef pdwVersionHi As UInteger, ByRef pdwVersionLow As UInteger) As Integer

    ''' <summary>
    ''' The IAssemblyName::IsEqual method compares the assembly name to another assembly names.
    ''' </summary>
    ''' <param name="pName">The assembly name to compare to.</param>
    ''' <param name="dwCmpFlags">Indicates which part of the assembly name to use in the comparison. 
    ''' Values are one or more of the bits defined in the ASM_CMP_FLAGS enumeration.</param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function IsEqual(pName As IAssemblyName, dwCmpFlags As ASM_CMP_FLAGS) As Integer

    ''' <summary>
    ''' The IAssemblyName::Clone method creates a copy of an assembly name. 
    ''' </summary>
    ''' <param name="pName"></param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function Clone(ByRef pName As IAssemblyName) As Integer
End Interface


''' <summary>
''' The IAssemblyEnum interface enumerates the assemblies in the GAC.
''' </summary>
<ComImport(), Guid("21b8916c-f28e-11d2-a473-00c04f8ef448"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IAssemblyEnum
    ''' <summary>
    ''' The IAssemblyEnum::GetNextAssembly method enumerates the assemblies in the GAC. 
    ''' </summary>
    ''' <param name="pvReserved">Must be null.</param>
    ''' <param name="ppName">Pointer to a memory location that is to receive the interface pointer to the assembly 
    ''' name of the next assembly that is enumerated.</param>
    ''' <param name="dwFlags">Must be zero.</param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function GetNextAssembly(pvReserved As IntPtr, ByRef ppName As IAssemblyName, dwFlags As UInteger) As Integer

    ''' <summary>
    ''' Undocumented. Best guess: reset the enumeration to the first assembly.
    ''' </summary>
    ''' <returns></returns>
    <PreserveSig()> _
    Function Reset() As Integer

    ''' <summary>
    ''' Undocumented. Create a copy of the assembly enum that is independently enumerable.
    ''' </summary>
    ''' <param name="ppEnum"></param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function Clone(ByRef ppEnum As IAssemblyEnum) As Integer
End Interface


''' <summary>
''' The IInstallReferenceItem interface represents a reference that has been set on an assembly in the GAC. 
''' Instances of IInstallReferenceIteam are returned by the IInstallReferenceEnum interface.
''' </summary>
<ComImport(), Guid("582dac66-e678-449f-aba6-6faaec8a9394"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IInstallReferenceItem
    ''' <summary>
    ''' The IInstallReferenceItem::GetReference method returns a FUSION_INSTALL_REFERENCE structure. 
    ''' </summary>
    ''' <param name="ppRefData">A pointer to a FUSION_INSTALL_REFERENCE structure. The memory is allocated by the GetReference 
    ''' method and is freed when IInstallReferenceItem is released. Callers must not hold a reference to this buffer after the 
    ''' IInstallReferenceItem object is released.</param>
    ''' <param name="dwFlags">Must be zero.</param>
    ''' <param name="pvReserved">Must be null.</param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function GetReference(<MarshalAs(UnmanagedType.LPArray)> ByRef ppRefData As FUSION_INSTALL_REFERENCE(), dwFlags As UInteger, pvReserved As IntPtr) As Integer
End Interface


''' <summary>
''' The IInstallReferenceEnum interface enumerates all references that are set on an assembly in the GAC.
''' NOTE: References that belong to the assembly are locked for changes while those references are being enumerated. 
''' </summary>
<ComImport(), Guid("56b1a988-7c0c-4aa2-8639-c3eb5a90226f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IInstallReferenceEnum
    ''' <summary>
    ''' IInstallReferenceEnum::GetNextInstallReferenceItem returns the next reference information for an assembly. 
    ''' </summary>
    ''' <param name="ppRefItem">Pointer to a memory location that receives the IInstallReferenceItem pointer.</param>
    ''' <param name="dwFlags">Must be zero.</param>
    ''' <param name="pvReserved">Must be null.</param>
    ''' <returns></returns>
    <PreserveSig()> _
    Function GetNextInstallReferenceItem(ByRef ppRefItem As IInstallReferenceItem, dwFlags As UInteger, pvReserved As IntPtr) As Integer
End Interface



''' <summary>
''' Undocumented. Probably only for internal use.
''' <see cref="IAssemblyCache.CreateAssemblyCacheItem"/>
''' </summary>
<ComImport(), Guid("9E3AAEB4-D1CD-11D2-BAB9-00C04F8ECEAE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IAssemblyCacheItem
    ''' <summary>
    ''' Undocumented.
    ''' </summary>
    ''' <param name="dwFlags"></param>
    ''' <param name="pszStreamName"></param>
    ''' <param name="dwFormat"></param>
    ''' <param name="dwFormatFlags"></param>
    ''' <param name="ppIStream"></param>
    ''' <param name="puliMaxSize"></param>

    Sub CreateStream(dwFlags As UInteger, <MarshalAs(UnmanagedType.LPWStr)> pszStreamName As String, dwFormat As UInteger, dwFormatFlags As UInteger, ByRef ppIStream As IStream, ByRef puliMaxSize As Long)

    ''' <summary>
    ''' Undocumented.
    ''' </summary>
    ''' <param name="dwFlags"></param>
    ''' <param name="pulDisposition"></param>
    Sub Commit(dwFlags As UInteger, ByRef pulDisposition As Long)

    ''' <summary>
    ''' Undocumented.
    ''' </summary>
    Sub AbortItem()
End Interface

#End Region
