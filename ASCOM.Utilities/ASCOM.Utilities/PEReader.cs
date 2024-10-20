using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    internal class PEReader : IDisposable
    {

        #region Constants
        internal const int CLR_HEADER = 14; // Header number of the CLR information, if present
        private const int MAX_HEADERS_TO_CHECK = 1000; // Safety limit to ensure that we don't lock up the machine if we get a PE image that indicates it has a huge number of header directories

        // Possible error codes when an assembly is loaded for reflection
        private const int COR_E_BADIMAGEFORMAT = int.MinValue + 0x0007000B;
        private const int CLDB_E_FILE_OLDVER = int.MinValue + 0x00131107;
        private const int CLDB_E_INDEX_NOTFOUND = int.MinValue + 0x00131124;
        private const int CLDB_E_FILE_CORRUPT = int.MinValue + 0x0013110E;
        private const int COR_E_NEWER_RUNTIME = int.MinValue + 0x0013101B;
        private const int COR_E_ASSEMBLYEXPECTED = int.MinValue + 0x00131018;
        private const int ERROR_BAD_EXE_FORMAT = int.MinValue + 0x000700C1;
        private const int ERROR_EXE_MARKED_INVALID = int.MinValue + 0x000700C0;
        private const int CORSEC_E_INVALID_IMAGE_FORMAT = int.MinValue + 0x0013141D;
        private const int ERROR_NOACCESS = int.MinValue + 0x000703E6;
        private const int ERROR_INVALID_ORDINAL = int.MinValue + 0x000700B6;
        private const int ERROR_INVALID_DLL = int.MinValue + 0x00070482;
        private const int ERROR_FILE_CORRUPT = int.MinValue + 0x00070570;
        private const int COR_E_LOADING_REFERENCE_ASSEMBLY = int.MinValue + 0x00131058;
        private const int META_E_BAD_SIGNATURE = int.MinValue + 0x00131192;

        // Executable machine types
        private const ushort IMAGE_FILE_MACHINE_I386 = 0x14C; // x86
        private const ushort IMAGE_FILE_MACHINE_IA64 = 0x200; // Intel(Itanium)
        private const ushort IMAGE_FILE_MACHINE_AMD64 = 0x8664; // x64

        #endregion

        #region Enums
        internal enum CLR_FLAGSType
        {
            CLR_FLAGS_ILONLY = 0x1,
            CLR_FLAGS_32BITREQUIRED = 0x2,
            CLR_FLAGS_IL_LIBRARY = 0x4,
            CLR_FLAGS_STRONGNAMESIGNED = 0x8,
            CLR_FLAGS_NATIVE_ENTRYPOINT = 0x10,
            CLR_FLAGS_TRACKDEBUGDATA = 0x10000
        }

        internal enum SubSystemType
        {
            NATIVE = 1, // The binary doesn't need a subsystem. This is used for drivers.
            WINDOWS_GUI = 2, // The image is a Win32 graphical binary. (It can still open a console with AllocConsole() but won't get one automatically at startup.)
            WINDOWS_CUI = 3, // The binary is a Win32 console binary. (It will get a console per default at startup, or inherit the parent's console.)
            UNKNOWN_4 = 4, // Unknown allocation
            OS2_CUI = 5, // The binary is a OS/2 console binary. (OS/2 binaries will be in OS/2 format, so this value will seldom be used in a PE file.)
            UNKNOWN_6 = 6, // Unknown allocation
            POSIX_CUI = 7, // The binary uses the POSIX console subsystem.
            NATIVE_WINDOWS = 8,
            WINDOWS_CE_GUI = 9,
            EFI_APPLICATION = 10, // Extensible Firmware Interface (EFI) application.
            EFI_BOOT_SERVICE_DRIVER = 11, // EFI driver with boot services.
            EFI_RUNTIME_DRIVER = 12, // EFI driver with run-time services.
            EFI_ROM = 13, // EFI ROM image.
            XBOX = 14, // Xbox system.
            UNKNOWN_15 = 15, // Unknown allocation
            WINDOWS_BOOT_APPLICATION = 16 // Boot application.
        }
        #endregion

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGE_DOS_HEADER
        {
            internal ushort e_magic; // Magic number
            internal ushort e_cblp; // Bytes on last page of file
            internal ushort e_cp; // Pages in file
            internal ushort e_crlc; // Relocations
            internal ushort e_cparhdr; // Size of header in paragraphs
            internal ushort e_minalloc; // Minimum extra paragraphs needed
            internal ushort e_maxalloc; // Maximum extra paragraphs needed
            internal ushort e_ss; // Initial (relative) SS value
            internal ushort e_sp; // Initial SP value
            internal ushort e_csum; // Checksum
            internal ushort e_ip; // Initial IP value
            internal ushort e_cs; // Initial (relative) CS value
            internal ushort e_lfarlc; // File address of relocation table
            internal ushort e_ovno; // Overlay number
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            internal ushort[] e_res1; // Reserved words
            internal ushort e_oemid; // OEM identifier (for e_oeminfo)
            internal ushort e_oeminfo; // 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            internal ushort[] e_res2; // Reserved words
            internal uint e_lfanew; // File address of new EXE header
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGE_NT_HEADERS
        {
            internal uint Signature;
            internal IMAGE_FILE_HEADER FileHeader;
            internal IMAGE_OPTIONAL_HEADER32 OptionalHeader32;
            internal IMAGE_OPTIONAL_HEADER64 OptionalHeader64;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGE_FILE_HEADER
        {
            internal ushort Machine;
            internal ushort NumberOfSections;
            internal uint TimeDateStamp;
            internal uint PointerToSymbolTable;
            internal uint NumberOfSymbols;
            internal ushort SizeOfOptionalHeader;
            internal ushort Characteristics;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGE_OPTIONAL_HEADER32
        {
            internal ushort Magic;
            internal byte MajorLinkerVersion;
            internal byte MinorLinkerVersion;
            internal uint SizeOfCode;
            internal uint SizeOfInitializedData;
            internal uint SizeOfUninitializedData;
            internal uint AddressOfEntryPoint;
            internal uint BaseOfCode;
            internal uint BaseOfData;
            internal uint ImageBase;
            internal uint SectionAlignment;
            internal uint FileAlignment;
            internal ushort MajorOperatingSystemVersion;
            internal ushort MinorOperatingSystemVersion;
            internal ushort MajorImageVersion;
            internal ushort MinorImageVersion;
            internal ushort MajorSubsystemVersion;
            internal ushort MinorSubsystemVersion;
            internal uint Win32VersionValue;
            internal uint SizeOfImage;
            internal uint SizeOfHeaders;
            internal uint CheckSum;
            internal ushort Subsystem;
            internal ushort DllCharacteristics;
            internal uint SizeOfStackReserve;
            internal uint SizeOfStackCommit;
            internal uint SizeOfHeapReserve;
            internal uint SizeOfHeapCommit;
            internal uint LoaderFlags;
            internal uint NumberOfRvaAndSizes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal IMAGE_DATA_DIRECTORY[] DataDirectory;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGE_OPTIONAL_HEADER64
        {
            internal ushort Magic;
            internal byte MajorLinkerVersion;
            internal byte MinorLinkerVersion;
            internal uint SizeOfCode;
            internal uint SizeOfInitializedData;
            internal uint SizeOfUninitializedData;
            internal uint AddressOfEntryPoint;
            internal uint BaseOfCode;
            internal ulong ImageBase;
            internal uint SectionAlignment;
            internal uint FileAlignment;
            internal ushort MajorOperatingSystemVersion;
            internal ushort MinorOperatingSystemVersion;
            internal ushort MajorImageVersion;
            internal ushort MinorImageVersion;
            internal ushort MajorSubsystemVersion;
            internal ushort MinorSubsystemVersion;
            internal uint Win32VersionValue;
            internal uint SizeOfImage;
            internal uint SizeOfHeaders;
            internal uint CheckSum;
            internal ushort Subsystem;
            internal ushort DllCharacteristics;
            internal ulong SizeOfStackReserve;
            internal ulong SizeOfStackCommit;
            internal ulong SizeOfHeapReserve;
            internal ulong SizeOfHeapCommit;
            internal uint LoaderFlags;
            internal uint NumberOfRvaAndSizes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal IMAGE_DATA_DIRECTORY[] DataDirectory;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGE_DATA_DIRECTORY
        {
            internal uint VirtualAddress;
            internal uint Size;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGE_SECTION_HEADER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            internal string Name;
            internal Misc Misc;
            internal uint VirtualAddress;
            internal uint SizeOfRawData;
            internal uint PointerToRawData;
            internal uint PointerToRelocations;
            internal uint PointerToLinenumbers;
            internal ushort NumberOfRelocations;
            internal ushort NumberOfLinenumbers;
            internal uint Characteristics;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct Misc
        {
            [FieldOffset(0)]
            internal uint PhysicalAddress;
            [FieldOffset(0)]
            internal uint VirtualSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGE_COR20_HEADER
        {
            internal uint cb;
            internal ushort MajorRuntimeVersion;
            internal ushort MinorRuntimeVersion;
            internal IMAGE_DATA_DIRECTORY MetaData;       // // Symbol table and startup information
            internal uint Flags;
            internal uint EntryPointToken;
            internal IMAGE_DATA_DIRECTORY Resources;        // // Binding information
            internal IMAGE_DATA_DIRECTORY StrongNameSignature;
            internal IMAGE_DATA_DIRECTORY CodeManagerTable;        // // Regular fix-up and binding information
            internal IMAGE_DATA_DIRECTORY VTableFixups;
            internal IMAGE_DATA_DIRECTORY ExportAddressTableJumps;
            internal IMAGE_DATA_DIRECTORY ManagedNativeHeader;        // // Pre-compiled image info (internal use only - set to zero)
        }
        #endregion

        #region Fields
        private readonly IMAGE_DOS_HEADER dosHeader;
        private IMAGE_NT_HEADERS ntHeaders;
        private readonly IMAGE_COR20_HEADER CLR;
        private readonly IList<IMAGE_SECTION_HEADER> sectionHeaders = new List<IMAGE_SECTION_HEADER>();
        private uint TextBase;
        private BinaryReader reader;
        private Stream stream;
        private bool IsAssembly = false;
        private Assembly SuppliedAssembly;
        private bool OS32BitCompatible = false;
        private Bitness ExecutableBitness;

        private TraceLogger TL;
        #endregion

        internal PEReader(string FileName, TraceLogger TLogger)
        {
            TL = TLogger; // Save the TraceLogger instance we have been passed

            TL.LogMessage("PEReader", "Running within CLR version: " + RuntimeEnvironment.GetSystemVersion());

            if (Strings.Left(FileName, 5).ToUpperInvariant() == "FILE:")
            {
                // Convert Uri to local path if required, URI paths are not supported by FileStream - this method allows file names with # characters to be passed through
                var u = new Uri(FileName);
                FileName = u.LocalPath + Uri.UnescapeDataString(u.Fragment).Replace("/", @"\\");
            }
            TL.LogMessage("PEReader", "Filename to check: " + FileName);
            if (!File.Exists(FileName))
                throw new FileNotFoundException("PEReader - File not found: " + FileName);

            // Determine whether this is an assembly by testing whether we can load the file as an assembly, if so then it IS an assembly!
            TL.LogMessage("PEReader", "Determining whether this is an assembly");
            try
            {
                SuppliedAssembly = Assembly.ReflectionOnlyLoadFrom(FileName);
                IsAssembly = true; // We got here without an exception so it must be an assembly
                TL.LogMessage("PEReader.IsAssembly", "Found an assembly because it loaded OK to the reflection context: " + IsAssembly);
            }
            catch (FileNotFoundException)
            {
                TL.LogMessage("PEReader.IsAssembly", "FileNotFoundException: File not found so this is NOT an assembly: " + IsAssembly);
            }
            catch (BadImageFormatException ex1)
            {

                // There are multiple reasons why this can occur so now determine what actually happened by examining the hResult
                int hResult = Marshal.GetHRForException(ex1);

                switch (hResult)
                {
                    case COR_E_BADIMAGEFORMAT:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - COR_E_BADIMAGEFORMAT. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case CLDB_E_FILE_OLDVER:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - CLDB_E_FILE_OLDVER. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case CLDB_E_INDEX_NOTFOUND:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - CLDB_E_INDEX_NOTFOUND. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case CLDB_E_FILE_CORRUPT:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - CLDB_E_FILE_CORRUPT. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case COR_E_NEWER_RUNTIME: // This is an assembly but it requires a newer runtime than is currently running, so flag it as an assembly even though we can't load it
                        IsAssembly = true;
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - COR_E_NEWER_RUNTIME. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case COR_E_ASSEMBLYEXPECTED:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - COR_E_ASSEMBLYEXPECTED. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case ERROR_BAD_EXE_FORMAT:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - ERROR_BAD_EXE_FORMAT. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case ERROR_EXE_MARKED_INVALID:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - ERROR_EXE_MARKED_INVALID. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case CORSEC_E_INVALID_IMAGE_FORMAT:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - CORSEC_E_INVALID_IMAGE_FORMAT. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case ERROR_NOACCESS:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - ERROR_NOACCESS. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case ERROR_INVALID_ORDINAL:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - ERROR_INVALID_ORDINAL. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case ERROR_INVALID_DLL:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - ERROR_INVALID_DLL. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case ERROR_FILE_CORRUPT:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - ERROR_FILE_CORRUPT. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case COR_E_LOADING_REFERENCE_ASSEMBLY:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - COR_E_LOADING_REFERENCE_ASSEMBLY. Setting IsAssembly to: " + IsAssembly);
                        break;
                    case META_E_BAD_SIGNATURE:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - META_E_BAD_SIGNATURE. Setting IsAssembly to: " + IsAssembly);
                        break;

                    default:
                        TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hResult.ToString("X8") + " - Meaning of error code is unknown. Setting IsAssembly to: " + IsAssembly);
                        break;
                }
            }

            catch (FileLoadException) // This is an assembly but that has already been loaded so flag it as an assembly
            {
                IsAssembly = true;
                TL.LogMessage("PEReader.IsAssembly", "FileLoadException: Assembly already loaded so this is an assembly: " + IsAssembly);
            }

            TL.LogMessage("PEReader", "Determining PE Machine type");
            stream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            reader = new BinaryReader(stream);

            reader.BaseStream.Seek(0L, SeekOrigin.Begin); // Reset reader position, just in case
            dosHeader = MarshalBytesTo<IMAGE_DOS_HEADER>(reader); // Read MS-DOS header section
            if (dosHeader.e_magic != 0x5A4D) // MS-DOS magic number should read 'MZ'
            {
                throw new InvalidOperationException("File is not a portable executable.");
            }

            reader.BaseStream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin); // Skip MS-DOS stub and seek reader to NT Headers
            ntHeaders.Signature = MarshalBytesTo<uint>(reader); // Read NT Headers
            if (ntHeaders.Signature != 0x4550L) // Make sure we have 'PE' in the PE signature 
            {
                throw new InvalidOperationException("Invalid portable executable signature in NT header.");
            }
            ntHeaders.FileHeader = MarshalBytesTo<IMAGE_FILE_HEADER>(reader); // Read the IMAGE_FILE_HEADER which starts 4 bytes on from the start of the signature (already here by reading the signature itself)

            // Determine whether this executable is flagged as a 32bit or 64bit and set OS32BitCompatible accordingly
            switch (ntHeaders.FileHeader.Machine)
            {
                case IMAGE_FILE_MACHINE_I386:
                    {
                        OS32BitCompatible = true;
                        TL.LogMessage("PEReader.MachineType", "Machine - found \"Intel 32bit\" executable. Characteristics: " + ntHeaders.FileHeader.Characteristics.ToString("X8") + ", OS32BitCompatible: " + OS32BitCompatible);
                        break;
                    }
                case IMAGE_FILE_MACHINE_IA64:
                    {
                        OS32BitCompatible = false;
                        TL.LogMessage("PEReader.MachineType", "Machine - found \"Itanium 64bit\" executable. Characteristics: " + ntHeaders.FileHeader.Characteristics.ToString("X8") + ", OS32BitCompatible: " + OS32BitCompatible);
                        break;
                    }
                case IMAGE_FILE_MACHINE_AMD64:
                    {
                        OS32BitCompatible = false;
                        TL.LogMessage("PEReader.MachineType", "Machine - found \"Intel 64bit\" executable. Characteristics: " + ntHeaders.FileHeader.Characteristics.ToString("X8") + ", OS32BitCompatible: " + OS32BitCompatible);
                        break;
                    }

                default:
                    {
                        TL.LogMessage("PEReader.MachineType", "Found Unknown machine type: " + ntHeaders.FileHeader.Machine.ToString("X8") + ". Characteristics: " + ntHeaders.FileHeader.Characteristics.ToString("X8") + ", OS32BitCompatible: " + OS32BitCompatible);
                        break;
                    }
            }

            if (OS32BitCompatible) // Read optional 32bit header
            {
                TL.LogMessage("PEReader.MachineType", "Reading optional 32bit header");
                ntHeaders.OptionalHeader32 = MarshalBytesTo<IMAGE_OPTIONAL_HEADER32>(reader);
            }
            else // Read optional 64bit header
            {
                TL.LogMessage("PEReader.MachineType", "Reading optional 64bit header");
                ntHeaders.OptionalHeader64 = MarshalBytesTo<IMAGE_OPTIONAL_HEADER64>(reader);
            }

            if (IsAssembly)
            {
                TL.LogMessage("PEReader", "This is an assembly, determining Bitness through the CLR header");
                // Find the CLR header
                int NumberOfHeadersToCheck = MAX_HEADERS_TO_CHECK;
                if (OS32BitCompatible) // We have a 32bit assembly
                {
                    TL.LogMessage("PEReader.Bitness", "This is a 32 bit assembly, reading the CLR Header");
                    if (ntHeaders.OptionalHeader32.NumberOfRvaAndSizes < (long)MAX_HEADERS_TO_CHECK)
                        NumberOfHeadersToCheck = (int)ntHeaders.OptionalHeader32.NumberOfRvaAndSizes;
                    TL.LogMessage("PEReader.Bitness", "Checking " + NumberOfHeadersToCheck + " headers");

                    for (int i = 0, loopTo = NumberOfHeadersToCheck - 1; i <= loopTo; i++)
                    {
                        if (ntHeaders.OptionalHeader32.DataDirectory[i].Size > 0L)
                        {
                            sectionHeaders.Add(MarshalBytesTo<IMAGE_SECTION_HEADER>(reader));
                        }
                    }

                    foreach (IMAGE_SECTION_HEADER SectionHeader in sectionHeaders)
                    {
                        if (SectionHeader.Name == ".text")
                            TextBase = SectionHeader.PointerToRawData;
                    }

                    if (NumberOfHeadersToCheck >= CLR_HEADER + 1) // Only test if the number of headers meets or exceeds the location of the CLR header
                    {
                        if (ntHeaders.OptionalHeader32.DataDirectory[CLR_HEADER].VirtualAddress > 0L)
                        {
                            reader.BaseStream.Seek(ntHeaders.OptionalHeader32.DataDirectory[CLR_HEADER].VirtualAddress - ntHeaders.OptionalHeader32.BaseOfCode + TextBase, SeekOrigin.Begin);
                            CLR = MarshalBytesTo<IMAGE_COR20_HEADER>(reader);
                        }
                    }
                }
                else // We have a 64bit assembly
                {
                    TL.LogMessage("PEReader.Bitness", "This is a 64 bit assembly, reading the CLR Header");
                    if (ntHeaders.OptionalHeader64.NumberOfRvaAndSizes < (long)MAX_HEADERS_TO_CHECK)
                        NumberOfHeadersToCheck = (int)ntHeaders.OptionalHeader64.NumberOfRvaAndSizes;
                    TL.LogMessage("PEReader.Bitness", "Checking " + NumberOfHeadersToCheck + " headers");

                    for (int i = 0, loopTo1 = NumberOfHeadersToCheck - 1; i <= loopTo1; i++)
                    {
                        if (ntHeaders.OptionalHeader64.DataDirectory[i].Size > 0L)
                        {
                            sectionHeaders.Add(MarshalBytesTo<IMAGE_SECTION_HEADER>(reader));
                        }
                    }

                    foreach (IMAGE_SECTION_HEADER SectionHeader in sectionHeaders)
                    {
                        if (SectionHeader.Name == ".text")
                        {
                            TL.LogMessage("PEReader.Bitness", "Found TEXT section");
                            TextBase = SectionHeader.PointerToRawData;
                        }
                    }

                    if (NumberOfHeadersToCheck >= CLR_HEADER + 1) // Only test if the number of headers meets or exceeds the location of the CLR header
                    {
                        if (ntHeaders.OptionalHeader64.DataDirectory[CLR_HEADER].VirtualAddress > 0L)
                        {
                            reader.BaseStream.Seek(ntHeaders.OptionalHeader64.DataDirectory[CLR_HEADER].VirtualAddress - ntHeaders.OptionalHeader64.BaseOfCode + TextBase, SeekOrigin.Begin);
                            CLR = MarshalBytesTo<IMAGE_COR20_HEADER>(reader);
                            TL.LogMessage("PEReader.Bitness", "Read CLR header successfully");
                        }
                    }

                }

                // Determine the bitness from the CLR header
                if (OS32BitCompatible) // Could be an x86 or MSIL assembly so determine which
                {
                    if ((CLR.Flags & (long)CLR_FLAGSType.CLR_FLAGS_32BITREQUIRED) > 0L)
                    {
                        TL.LogMessage("PEReader.Bitness", "Found \"32bit Required\" assembly");
                        ExecutableBitness = Bitness.Bits32;
                    }
                    else
                    {
                        TL.LogMessage("PEReader.Bitness", "Found \"MSIL\" assembly");
                        ExecutableBitness = Bitness.BitsMSIL;
                    }
                }
                else // Must be an x64 assembly
                {
                    TL.LogMessage("PEReader.Bitness", "Found \"64bit Required\" assembly");
                    ExecutableBitness = Bitness.Bits64;
                }

                TL.LogMessage("PEReader", "Assembly required Runtime version: " + CLR.MajorRuntimeVersion + "." + CLR.MinorRuntimeVersion);
            }
            else // Not an assembly so just use the FileHeader.Machine value to determine bitness
            {
                TL.LogMessage("PEReader", "This is not an assembly, determining Bitness through the executable bitness flag");
                if (OS32BitCompatible)
                {
                    TL.LogMessage("PEReader.Bitness", "Found 32bit executable");
                    ExecutableBitness = Bitness.Bits32;
                }
                else
                {
                    TL.LogMessage("PEReader.Bitness", "Found 64bit executable");
                    ExecutableBitness = Bitness.Bits64;
                }

            }
        }

        internal Bitness BitNess
        {
            get
            {
                TL.LogMessage("PE.BitNess", "Returning: " + ((int)ExecutableBitness).ToString());
                return ExecutableBitness;
            }
        }

        internal bool IsDotNetAssembly()
        {
            TL.LogMessage("PE.IsDotNetAssembly", "Returning: " + IsAssembly);
            return IsAssembly;
        }

        internal SubSystemType SubSystem()
        {
            if (OS32BitCompatible)
            {
                TL.LogMessage("PE.SubSystem", "Returning 32bit value: " + ((SubSystemType)ntHeaders.OptionalHeader32.Subsystem).ToString());
                return (SubSystemType)ntHeaders.OptionalHeader32.Subsystem; // Return the 32bit header field
            }
            else
            {
                TL.LogMessage("PE.SubSystem", "Returning 64bit value: " + ((SubSystemType)ntHeaders.OptionalHeader64.Subsystem).ToString());
                return (SubSystemType)ntHeaders.OptionalHeader64.Subsystem;
            } // Return the 64bit field
        }

        private static T MarshalBytesTo<T>(BinaryReader reader)
        {
            // Unmanaged data
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            // Create a pointer to the unmanaged data pinned in memory to be accessed by unmanaged code
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            // Use our previously created pointer to unmanaged data and marshal to the specified type
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

            // Deallocate pointer
            handle.Free();

            return theStructure;
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        reader.Close();
                        stream.Close();
                        stream.Dispose();
                        stream = null;
                    }
                    catch (Exception) // Swallow any exceptions here
                    {
                    }
                }

            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
