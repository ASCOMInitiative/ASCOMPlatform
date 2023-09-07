// Errors and strings reported by the VB6 COM components


namespace ASCOM.Utilities
{

    static class VB6COMErrors
    {
        // =============
        // COMERRORS.BAS
        // =============
        // 
        // COM error constants
        // 
        // Written:  21-Aug-00   Robert B. Denny <rdenny@dc3.com>
        // 
        // Edits:
        // 
        // When      Who     What
        // --------- ---     --------------------------------------------------
        // 21-Aug-00 rbd     Initial edit
        // 21-Jan-01 rbd     Add Chooser class errors, registry source, new
        // Profile object errors & source.
        // 22-Jan-00 rbd     More Profile error messages
        // 08-Jun-01 rbd     One more for Profile and Chooser, Chooser is now
        // a "device chooser" not a "telescope chooser".
        // 28-Aug-03 rbd     Add hi-res timer message
        // 26-Mar-07 rbd     5.0.1 - Add trace file error
        // ---------------------------------------------------------------------

        // 
        // Serial.cls
        // 
        internal const string ERR_SOURCE_SERIAL = "ASCOM Helper Serial Port Object";
        internal const int SCODE_UNSUP_SPEED = Microsoft.VisualBasic.Constants.vbObjectError + 0x400;
        internal const string MSG_UNSUP_SPEED = "Unsupported port speed. Use the PortSpeed enumeration.";
        internal const int SCODE_INVALID_TIMEOUT = Microsoft.VisualBasic.Constants.vbObjectError + 0x401;
        internal const string MSG_INVALID_TIMEOUT = "Timeout must be 1 - 120 seconds.";
        internal const int SCODE_RECEIVE_TIMEOUT = Microsoft.VisualBasic.Constants.vbObjectError + 0x402;
        internal const string MSG_RECEIVE_TIMEOUT = "Timed out waiting for received data.";
        internal const int SCODE_EMPTY_TERM = Microsoft.VisualBasic.Constants.vbObjectError + 0x403;
        internal const string MSG_EMPTY_TERM = "Terminator string must have at least one character.";
        internal const int SCODE_ILLEGAL_COUNT = Microsoft.VisualBasic.Constants.vbObjectError + 0x404;
        internal const string MSG_ILLEGAL_COUNT = "Character count must be positive and greater than 0.";
        internal const int SCODE_TRACE_ERR = Microsoft.VisualBasic.Constants.vbObjectError + 0x405;
        internal const string MSG_TRACE_ERR = "Serial Trace file: "; // FSO error appended
                                                                     // 
                                                                     // Chooser.cls (base = &H410)
                                                                     // 
        internal const string ERR_SOURCE_CHOOSER = "ASCOM Helper Device Chooser Object";
        // Friend Const SCODE_ILLEGAL_DEVTYPE As Long = vbObjectError + &H421
        // Friend Const MSG_ILLEGAL_DEVTYPE As String = "Illegal DriverID value """" (empty string)"
        // 
        // Profile.cls (base = &H420)
        // 
        internal const string ERR_SOURCE_PROFILE = "ASCOM Helper Registry Profile Object";
        internal const int SCODE_DRIVER_NOT_REG = Microsoft.VisualBasic.Constants.vbObjectError + 0x420;
        // This uses runtime-generated message
        internal const int SCODE_ILLEGAL_DRIVERID = Microsoft.VisualBasic.Constants.vbObjectError + 0x421;
        internal const string MSG_ILLEGAL_DRIVERID = "Illegal DriverID value \"\" (empty string)";
        internal const int SCODE_ILLEGAL_REGACC = Microsoft.VisualBasic.Constants.vbObjectError + 0x422;
        internal const string MSG_ILLEGAL_REGACC = "Illegal access to registry area";
        internal const int SCODE_ILLEGAL_DEVTYPE = Microsoft.VisualBasic.Constants.vbObjectError + 0x423;
        internal const string MSG_ILLEGAL_DEVTYPE = "Illegal DeviceType value \"\" (empty string)";
        // 
        // Util.cls (base = &H430)
        // 
        internal const string ERR_SOURCE_UTIL = "ASCOM Helper Utilities Object";
        internal const int SCODE_DLL_LOADFAIL = Microsoft.VisualBasic.Constants.vbObjectError + 0x430;
        // This uses runtime-generated message
        internal const int SCODE_TIMER_FAIL = Microsoft.VisualBasic.Constants.vbObjectError + 0x431;
        internal const string MSG_TIMER_FAIL = "Hi-res timer failed. Delay out of range?";
        // 
        // Registry.bas (base = &H440)
        // 
        internal const int SCODE_REGERR = Microsoft.VisualBasic.Constants.vbObjectError + 0x440;
        // This uses runtime-generated messages and source
    }
}