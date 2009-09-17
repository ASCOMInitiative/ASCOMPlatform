Option Strict On
Option Explicit On
Module COM_Errors
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
	'---------------------------------------------------------------------
	
	'
	' Serial.cls
	'
    Friend Const ERR_SOURCE_SERIAL As String = "ASCOM Helper Serial Port Object"
    Friend Const SCODE_UNSUP_SPEED As Integer = vbObjectError + &H400
    Friend Const MSG_UNSUP_SPEED As String = "Unsupported port speed. Use the PortSpeed enumeration."
    Friend Const SCODE_INVALID_TIMEOUT As Integer = vbObjectError + &H401
    Friend Const MSG_INVALID_TIMEOUT As String = "Timeout must be 1 - 120 seconds."
    Friend Const SCODE_RECEIVE_TIMEOUT As Integer = vbObjectError + &H402
    Friend Const MSG_RECEIVE_TIMEOUT As String = "Timed out waiting for received data."
    Friend Const SCODE_EMPTY_TERM As Integer = vbObjectError + &H403
    Friend Const MSG_EMPTY_TERM As String = "Terminator string must have at least one character."
    Friend Const SCODE_ILLEGAL_COUNT As Integer = vbObjectError + &H404
    Friend Const MSG_ILLEGAL_COUNT As String = "Character count must be positive and greater than 0."
    Friend Const SCODE_TRACE_ERR As Integer = vbObjectError + &H405
    Friend Const MSG_TRACE_ERR As String = "Serial Trace file: " ' FSO error appended
    '
    ' Chooser.cls (base = &H410)
    '
    Friend Const ERR_SOURCE_CHOOSER As String = "ASCOM Helper Device Chooser Object"
    'Friend Const SCODE_ILLEGAL_DEVTYPE As Long = vbObjectError + &H421
    'Friend Const MSG_ILLEGAL_DEVTYPE As String = "Illegal DriverID value """" (empty string)"
    '
    ' Profile.cls (base = &H420)
    '
    Friend Const ERR_SOURCE_PROFILE As String = "ASCOM Helper Registry Profile Object"
    Friend Const SCODE_DRIVER_NOT_REG As Integer = vbObjectError + &H420
    ' This uses runtime-generated message
    Friend Const SCODE_ILLEGAL_DRIVERID As Integer = vbObjectError + &H421
    Friend Const MSG_ILLEGAL_DRIVERID As String = "Illegal DriverID value """" (empty string)"
    Friend Const SCODE_ILLEGAL_REGACC As Integer = vbObjectError + &H422
    Friend Const MSG_ILLEGAL_REGACC As String = "Illegal access to registry area"
    Friend Const SCODE_ILLEGAL_DEVTYPE As Integer = vbObjectError + &H423
    Friend Const MSG_ILLEGAL_DEVTYPE As String = "Illegal DeviceType value """" (empty string)"
    '
    ' Util.cls (base = &H430)
    '
    Friend Const ERR_SOURCE_UTIL As String = "ASCOM Helper Utilities Object"
    Friend Const SCODE_DLL_LOADFAIL As Integer = vbObjectError + &H430
    ' This uses runtime-generated message
    Friend Const SCODE_TIMER_FAIL As Integer = vbObjectError + &H431
    Friend Const MSG_TIMER_FAIL As String = "Hi-res timer failed. Delay out of range?"
    '
    ' Registry.bas (base = &H440)
    '
    Friend Const SCODE_REGERR As Integer = vbObjectError + &H440
	' This uses runtime-generated messages and source
End Module