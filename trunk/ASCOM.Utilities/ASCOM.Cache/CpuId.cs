using System;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Class to obtain CPUID information from Intel and AMD processors
    /// </summary>
    internal static class CpuID
    {
        /// <summary>
        /// Invoke the code that will read the requested CPUId leaf data and return it to managed memory
        /// </summary>
        /// <param name="leaf">Requested processor leaf</param>
        /// <returns>A CpuId structure containing the EAX, EBX, ECX and EDX register values.</returns>
        /// <remarks>This class is marked internal to keep it out of the ASCOM Help file. It can be accessed from other Platform assemblies by
        /// including an "InternalsVisibleToAttribute" for each external component that requires access. At January 2018 this is just Diagnostics.</remarks>
        internal static CpuIdResult Invoke(int leaf)
        {
            IntPtr codePointer = IntPtr.Zero;
            IntPtr buffer = IntPtr.Zero;
            CpuIdResult result;

            try
            {
                // Get 32 or 64bit code as necessary
                byte[] codeBytes;
                if (IntPtr.Size == 4)
                {
                    codeBytes = x86CodeBytes;
                }
                else
                {
                    codeBytes = x64CodeBytes;
                }

                // Get a pointer to an allocated memory space that will hold the code
                codePointer = VirtualAlloc(IntPtr.Zero, new UIntPtr((uint)codeBytes.Length), AllocationType.COMMIT | AllocationType.RESERVE, MemoryProtection.EXECUTE_READWRITE);

                // Copy the code into the buffer
                Marshal.Copy(codeBytes, 0, codePointer, codeBytes.Length);

                // Create a delegate to run the code and return the result
                CpuIDDelegate cpuIdDelg = (CpuIDDelegate)Marshal.GetDelegateForFunctionPointer(codePointer, typeof(CpuIDDelegate));

                // Allocate some memory to hold the returned data, call the delegate and save the resulting information
                try
                {
                    buffer = Marshal.AllocHGlobal(16);
                    cpuIdDelg(leaf, buffer);
                    result = (CpuIdResult)Marshal.PtrToStructure(buffer, typeof(CpuIdResult));
                }
                finally // Release the data buffer
                {
                    if (buffer != IntPtr.Zero) Marshal.FreeHGlobal(buffer);
                }
                return result;
            }
            finally // Release the memory space holding the code
            {
                if (codePointer != IntPtr.Zero)
                {
                    VirtualFree(codePointer, 0, 0x8000);
                    codePointer = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Structure to hold the returned CPUId data
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 16)]
        internal struct CpuIdResult
        {
            public uint Eax; // Bytes 0-3
            public uint Ebx; // Bytes 4-7
            public uint Ecx; // Bytes 8-11
            public uint Edx; // Bytes 12-15
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CpuIDDelegate(int level, IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32")]
        private static extern bool VirtualFree(IntPtr lpAddress, UInt32 dwSize, UInt32 dwFreeType);

        [Flags()]
        private enum AllocationType : uint
        {
            COMMIT = 0x1000,
            RESERVE = 0x2000,
            RESET = 0x80000,
            LARGE_PAGES = 0x20000000,
            PHYSICAL = 0x400000,
            TOP_DOWN = 0x100000,
            WRITE_WATCH = 0x200000
        }

        [Flags()]
        private enum MemoryProtection : uint
        {
            EXECUTE = 0x10,
            EXECUTE_READ = 0x20,
            EXECUTE_READWRITE = 0x40,
            EXECUTE_WRITECOPY = 0x80,
            NOACCESS = 0x01,
            READONLY = 0x02,
            READWRITE = 0x04,
            WRITECOPY = 0x08,
            GUARD_Modifierflag = 0x100,
            NOCACHE_Modifierflag = 0x200,
            WRITECOMBINE_Modifierflag = 0x400
        }

        /// <summary>
        /// Machine code for 32bit systems
        /// </summary>
        private readonly static byte[] x86CodeBytes = {
            0x55,                   // push        ebp  
            0x8B, 0xEC,             // mov         ebp,esp
            0x53,                   // push        ebx  
            0x57,                   // push        edi

            0x8B, 0x45, 0x08,       // mov         eax, dword ptr [ebp+8] (move level into eax)
            0x0F, 0xA2,              // cpuid

            0x8B, 0x7D, 0x0C,       // mov         edi, dword ptr [ebp+12] (move address of buffer into edi)
            0x89, 0x07,             // mov         dword ptr [edi+0], eax  (write eax, ... to buffer)
            0x89, 0x5F, 0x04,       // mov         dword ptr [edi+4], ebx 
            0x89, 0x4F, 0x08,       // mov         dword ptr [edi+8], ecx 
            0x89, 0x57, 0x0C,       // mov         dword ptr [edi+12],edx 

            0x5F,                   // pop         edi  
            0x5B,                   // pop         ebx  
            0x8B, 0xE5,             // mov         esp,ebp  
            0x5D,                   // pop         ebp 
            0xc3                    // ret
        };

        /// <summary>
        /// Machine code for 64bit systems
        /// </summary>
        private readonly static byte[] x64CodeBytes = {
            0x53,                       // push rbx    this gets clobbered by cpuid

            // rcx is level
            // rdx is buffer.
            // Need to save buffer elsewhere, cpuid overwrites rdx
            // Put buffer in r8, use r8 to reference buffer later.

            // Save rdx (buffer addy) to r8
            0x49, 0x89, 0xd0,           // mov r8,  rdx

            // Move ecx (level) to eax to call cpuid, call cpuid
            0x89, 0xc8,                 // mov eax, ecx
            0x0F, 0xA2,                 // cpuid

            // Write eax et al to buffer
            0x41, 0x89, 0x40, 0x00,     // mov    dword ptr [r8+0],  eax
            0x41, 0x89, 0x58, 0x04,     // mov    dword ptr [r8+4],  ebx
            0x41, 0x89, 0x48, 0x08,     // mov    dword ptr [r8+8],  ecx
            0x41, 0x89, 0x50, 0x0c,     // mov    dword ptr [r8+12], edx

            0x5b,                       // pop rbx
            0xc3                        // ret
        };
    }
}