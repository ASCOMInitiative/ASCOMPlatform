using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace OptecHIDTools
{
    [Guid("0C85811D-0A4C-4c06-973C-960083AD6C27")]
    public class Setup_API_Wrappers
    {
        // Used when registering for device notifications
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        [Guid("3E01A38C-63DB-4f5f-A73E-0375E1F5F6C0")]
        public struct DeviceBroadcastInterface
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public Guid ClassGuid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Name;
        }
        [Guid("2DFB9887-9E02-4a72-82AC-11BE3C374565")]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            internal Int32 cbSize;
            internal Guid InternalClassGuid;
            internal Int32 Flags;
            internal IntPtr Reserved;
        }

        // Unused
        //[Guid("15FF8B50-6F2C-49d4-AB16-F672A2F975EB")]
        //internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        //{
        //    internal Int32 cbSize;
        //    internal String DevicePath;
        //}


        // Used when registering for device notifications ******************
        internal const Int32 DBT_DEVTYP_DEVICEINTERFACE = 5; 

        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, Int32 Flags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern Boolean UnregisterDeviceNotification(IntPtr Handle);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs
            (ref System.Guid ClassGuid,
            IntPtr Enumerator,
            IntPtr hwndParent,
            Int32 Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces
            (IntPtr DeviceInfoSet,
            IntPtr DeviceInfoData,
            ref System.Guid InterfaceClassGuid,
            Int32 MemberIndex,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            IntPtr DeviceInterfaceDetailData,
            Int32 DeviceInterfaceDetailDataSize,
            ref Int32 RequiredSize,
            IntPtr DeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Int32 SetupDiDestroyDeviceInfoList(
            IntPtr DeviceInfoSet);

       

    }
    [Guid("90EF70FF-0689-42dd-8705-A377A5BF89D7")]
    public class HID_API_Wrapers
    {
        // Used with HidD_GetAttributes
        internal struct HIDD_ATTRIBUTES
        {
            internal Int32 Size;
            internal UInt16 VendorID;
            internal UInt16 ProductID;
            internal UInt16 VersionNumber;
        }

        //Used with HidP_GetCaps
        internal struct HIDP_CAPS
        {
            internal Int16 Usage;
            internal Int16 UsagePage;
            internal Int16 InputReportByteLength;
            internal Int16 OutputReportByteLength;
            internal Int16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            internal Int16[] Reserved;
            internal Int16 NumberLinkCollectionNodes;
            internal Int16 NumberInputButtonCaps;
            internal Int16 NumberInputValueCaps;
            internal Int16 NumberInputDataIndicies;
            internal Int16 NumberOutputButtonCaps;
            internal Int16 NumberOutputValueCaps;
            internal Int16 NumberOutputDataIndicies;
            internal Int16 NumberFeatureButtonCaps;
            internal Int16 NumberFeatureValueCaps;
            internal Int16 NumberFeartueDataIndicies;
        }

        //Used to get path when device attached
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class DEV_BROADCAST_DEVICEINTERFACE_1
        {
            internal Int32 dbcc_size;
            internal Int32 dbcc_devicetype;
            internal Int32 dbcc_reserved;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            internal Byte[] dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            internal Char[] dbcc_name;
        }
        //Used to get path when device attached
        [StructLayout(LayoutKind.Sequential)]
        internal class DEV_BROADCAST_HDR
        {
            internal Int32 dbch_size;
            internal Int32 dbch_devicetype;
            internal Int32 dbch_reserved;
        }

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern IntPtr HidD_GetHidGuid(ref System.Guid HidGuid);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetAttributes(
            SafeFileHandle HidDeviceObject,
            ref HIDD_ATTRIBUTES Attributes);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetPreparsedData(
            SafeFileHandle HidDeviceObject,
            ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Int32 HidP_GetCaps(
            IntPtr PreparsedData,
            ref HIDP_CAPS Capabilities);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetSerialNumberString(
            SafeFileHandle hFile,
            Byte[] SerialNumber,
            Int32 SerianNumberLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetManufacturerString(
            SafeFileHandle hFile,
            Byte[] SerialNumber,
            Int32 SerianNumberLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetProductString(
            SafeFileHandle hFile,
            Byte[] SerialNumber,
            Int32 SerianNumberLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetPhysicalDescriptor(
            SafeFileHandle hFile,
            Byte[] SerialNumber,
            Int32 SerianNumberLength);
    

        
        
        

    }
    [Guid("4F8FBD30-AC89-4ec3-9DE4-E5FDB73C5A01")]
    public class ReadWrite_API_Wrappers
    {
        // For Creating Handle***********************************************
        internal const Int32 FILE_ATTRIBUTE_NORMAL = 0X80;
        internal const Int32 FILE_FLAG_OVERLAPPED = 0X40000000;
        internal const Int32 FILE_SHARE_READ = 1;
        internal const Int32 FILE_SHARE_WRITE = 2;
        internal const UInt32 GENERIC_READ = 0X80000000;
        internal const UInt32 GENERIC_WRITE = 0X40000000;
        internal const Int32 OPEN_EXISTING = 3;
        //*******************************************************************

        // For Reading Input Reports*****************************************
        internal const Int32 WAIT_OBJECT_0 = 0;
        internal const Int32 WAIT_TIMEOUT = 0x102;
        //*******************************************************************

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern SafeFileHandle CreateFile(
            String FileName,
            UInt32 dwDesiredAccess,
            Int32 dwShareMode,
            IntPtr IpSecurityAttributes,
            Int32 dwCreationDisposition,
            Int32 dwFlagsAndAttributes,
            Int32 hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Boolean WriteFile
            (SafeFileHandle hFile,
            Byte[] lpBuffer,
            Int32 nNumberOfBytesToWrite,
            ref Int32 lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_SetOutputReport(
            SafeFileHandle HidDeviceObject, 
            Byte[] lpReportBuffer, 
            Int32 ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetInputReport(
            SafeFileHandle HidDeviceObject, 
            Byte[] lpReportBuffer,
            Int32 ReportBufferLength); 

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_SetFeature(
            SafeFileHandle hFile,
            Byte[] bReportBuffer,
            Int32 ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetFeature(
            SafeFileHandle hFile,
            Byte[] bReportBuffer,
            Int32 ReportBufferLength);

        // The following methods are all needed for Reading Input Reports...

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Int32 CancelIo(
            SafeFileHandle FileHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CreateEvent(
            IntPtr SecurityAttributes,
            Boolean bManualReset,
            Boolean bInitialState,
            String lpName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean GetOverlappedResult(
            SafeFileHandle hFileHandle,
            IntPtr IpOverlapped,
            ref Int32 IpNumberOfBytesTransfered,
            Boolean bWait);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Boolean ReadFile(
            SafeFileHandle hFileHandle,
            IntPtr IpBuffer,
            Int32 nNumberOfBytesToRead,
            ref Int32 nNumberOfBytesRead,
            IntPtr IpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Int32 WaitForSingleObject(
            IntPtr hHandle,
            Int32 dwMilliseconds);
    }
    [Guid("A3777DB0-02E2-48ff-8B5A-A63E748D240B")]
    public class Win32Errors
    {
        internal const Int16 FORMAT_MESSAGE_FROM_SYSTEM = 0X1000;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Int32 FormatMessage(
            Int32 dwFlags,
            ref Int64 lpSource,
            Int32 dwMessageId,
            Int32 dwLanguageZId,
            String lpBuffer, 
            Int32 nSize,
            Int32 Arguments);
    }

    

   
}
