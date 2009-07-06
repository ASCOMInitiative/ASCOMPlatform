Attribute VB_Name = "COM_Errors"
'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". SPACE.COM, INC. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'   =============
'   COMERRORS.BAS
'   =============
'
' COM error constants
'
' Written:  21-Aug-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 21-Aug-00 rbd     Initial edit
' 21-Jan-01 rbd     Add Chooser class errors, registry source, new
'                   Profile object errors & source.
' 22-Jan-00 rbd     More Profile error messages
' 08-Jun-01 rbd     One more for Profile and Chooser, Chooser is now
'                   a "device chooser" not a "telescope chooser".
' 28-Aug-03 rbd     Add hi-res timer message
' 26-Mar-07 rbd     5.0.1 - Add trace file error
' 03-Mar-07 rbd     5.0.2 - Changes for new XML based storage layer
'---------------------------------------------------------------------
Option Explicit

'
' Serial.cls
'
Public Const ERR_SOURCE_SERIAL As String = "ASCOM Helper Serial Port Object"
Public Const SCODE_UNSUP_SPEED As Long = vbObjectError + &H400
Public Const MSG_UNSUP_SPEED As String = "Unsupported port speed. Use the PortSpeed enumeration."
Public Const SCODE_INVALID_TIMEOUT As Long = vbObjectError + &H401
Public Const MSG_INVALID_TIMEOUT As String = "Timeout must be 1 - 120 seconds."
Public Const SCODE_RECEIVE_TIMEOUT As Long = vbObjectError + &H402
Public Const MSG_RECEIVE_TIMEOUT As String = "Timed out waiting for received data."
Public Const SCODE_EMPTY_TERM As Long = vbObjectError + &H403
Public Const MSG_EMPTY_TERM As String = "Terminator string must have at least one character."
Public Const SCODE_ILLEGAL_COUNT As Long = vbObjectError + &H404
Public Const MSG_ILLEGAL_COUNT As String = "Character count must be positive and greater than 0."
Public Const SCODE_TRACE_ERR As Long = vbObjectError + &H405
Public Const MSG_TRACE_ERR As String = "Serial Trace file: "    ' FSO error appended
'
' Chooser.cls (base = &H410)
'
Public Const ERR_SOURCE_CHOOSER As String = "ASCOM Helper Device Chooser Object"
'Public Const SCODE_ILLEGAL_DEVTYPE As Long = vbObjectError + &H421
'Public Const MSG_ILLEGAL_DEVTYPE As String = "Illegal DriverID value """" (empty string)"
'
' Profile.cls (base = &H420)
'
Public Const ERR_SOURCE_PROFILE As String = "ASCOM Helper Registry Profile Object"
Public Const SCODE_DRIVER_NOT_REG As Long = vbObjectError + &H420
    ' This uses runtime-generated message
Public Const SCODE_ILLEGAL_DRIVERID As Long = vbObjectError + &H421
Public Const MSG_ILLEGAL_DRIVERID As String = "Illegal DriverID value """" (empty string)"
Public Const SCODE_ILLEGAL_REGACC As Long = vbObjectError + &H422
Public Const MSG_ILLEGAL_REGACC As String = "Illegal access to profile data"
Public Const SCODE_ILLEGAL_DEVTYPE As Long = vbObjectError + &H423
Public Const MSG_ILLEGAL_DEVTYPE As String = "Illegal DeviceType value """" (empty string)"
'
' Util.cls (base = &H430)
'
Public Const ERR_SOURCE_UTIL As String = "ASCOM Helper Utilities Object"
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H430
    ' This uses runtime-generated message
Public Const SCODE_TIMER_FAIL As Long = vbObjectError + &H431
Public Const MSG_TIMER_FAIL As String = "Hi-res timer failed. Delay out of range?"
'
' Config.bas (base = &H440)
'
Public Const SCODE_CORRUPT_CONFIG As Long = vbObjectError + &H440
Public Const MSG_CORRUPT_CONFIG As String = "ASCOM Registry XML: "
Public Const SCODE_NOEXIST_UVAL As Long = vbObjectError + &H441
Public Const MSG_NOEXIST_UVAL As String = "Unnamed value does not exist"
Public Const SCODE_NOEXIST_VAL As Long = vbObjectError + &H442
Public Const MSG_NOEXIST_VAL As String = "Value does not exist: "
Public Const SCODE_NOEXIST_KEY As Long = vbObjectError + &H443
Public Const MSG_NOEXIST_KEY As String = "Key does not exist: "
Public Const SCODE_XML_LOADFAIL As Long = vbObjectError + &H444
Public Const MSG_XML_LOADFAIL As String = "Failed to load XML from "
Public Const SCODE_SPECFLD_FAIL As Long = vbObjectError + &H445
Public Const MSG_SPECFLD_FAIL As String = "Failed to get path to config folder"

