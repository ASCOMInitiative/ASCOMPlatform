Attribute VB_Name = "ErrorConstants"
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
'   ==================
'   ERRORCONSTANTS.BAS
'   ==================
'
' COM error constants for ASCOM Meade telescope driver
'
' Written:  22-Aug-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Aug-00 rbd     Initial edit
' 28-Aug-00 rbd     Many additions, integration complete.
' 13-Jan-01 rbd     Change not implemented message
' 08-Oct-01 rbd     Change MSG_UNKNOWN_MEADE to refer to ASCOM web
'                   site.
' 05-Dec-01 rbd     Bad format lat/long/time messages, renumber using
'                   hex range (forgot 40A-40F)
' 07-May-02 rbd     Add Focuser errors
' 28-Jul-02 LFW     Added Focuser MaxIncrement error
' 11-Oct-03 jab     Added Parked error code (V2 is more picky)
' 20-Oct 20 jab     Add Pulse duration error code
' 15-Sep-06 jab     Pealed out the LX200GPS into its own driver
' 21-Sep-06 jab     Added SCODE_FOCUSER_NOT_CONNECTED
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ASCOM Meade LX200GPS/R Driver"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented in this driver."
Public Const SCODE_LONGFMT_FAILED As Long = vbObjectError + &H401
Public Const MSG_LONGFMT_FAILED As String = _
    "The scope does not support the required long formats. The LX200 firmware is too old."
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = _
    "The scope is not connected."
Public Const SCODE_LAND_MODE As Long = vbObjectError + &H403
Public Const MSG_LAND_MODE As String = _
    "The scope is in LAND mode. Use the handbox to change to POLAR or ALTAZ."
Public Const SCODE_NOT_MEADE As Long = vbObjectError + &H404
Public Const MSG_NOT_MEADE As String = _
    "Something other than a Meade scope (a modem?) appears to be connected to the COM port."
Public Const SCODE_UNKNOWN_MEADE As Long = vbObjectError + &H405
Public Const MSG_UNKNOWN_MEADE As String = _
    "The scope appears to be an unrecognized Meade scope type. " & _
    "Check the ASCOM-Standards.org web site for driver updates."
Public Const SCODE_NEED_SETUP As Long = vbObjectError + &H406
Public Const MSG_NEED_SETUP As String = _
    "You must configure this driver before using it."
Public Const SCODE_NO_SCOPE As Long = vbObjectError + &H407
Public Const MSG_NO_SCOPE As String = _
    "There doesn't appear to be anything connected to the COM port."
Public Const SCODE_REGERR As Long = vbObjectError + &H408
    ' This uses runtime-generated messages
Public Const SCODE_SYNC_FAILED As Long = vbObjectError + &H409
Public Const MSG_SYNC_FAILED As String = _
    "The sync operation failed for some reason."
Public Const SCODE_PROP_RANGE_ERROR As Long = vbObjectError + &H410
Public Const MSG_PROP_RANGE_ERROR As String = _
    "The supplied value is out of range for this property."
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H40A
    ' This uses runtime-generated messages
Public Const SCODE_BELOW_HORIZ = vbObjectError + &H40B
Public Const MSG_BELOW_HORIZ = _
    "The selected coordinates are below the horizon."
Public Const SCODE_BELOW_MIN_EL = vbObjectError + &H40C
Public Const MSG_BELOW_MIN_EL = _
    "The selected coordinates are below the current minimum elevation setting."
Public Const SCODE_SLEW_FAIL = vbObjectError + &H40D
Public Const MSG_SLEW_FAIL = _
    "The slew failed for some unknown reason."
Public Const SCODE_SLEW_16FAIL = vbObjectError + &H40E
Public Const MSG_SLEW_16FAIL = _
    "The slew failed but the 16"" scope reported success. Check the handbox for failure reason."
Public Const SCODE_SETUP_CONNECTED = vbObjectError + &H40F
Public Const MSG_SETUP_CONNECTED = _
    "You cannot change the driver's configuration while it is connected."
Public Const SCODE_UNKNOWN_ALIGN = vbObjectError + &H410
Public Const MSG_UNKNOWN_ALIGN = _
    "The scope is in an unknown tracking mode, so it must remain there."
Public Const SCODE_BAD_LONGITUDE = vbObjectError + &H411
Public Const MSG_BAD_LONGITUDE = _
    "The longitude value was rejected by the scope."
Public Const SCODE_BAD_LATITUDE = vbObjectError + &H412
Public Const MSG_BAD_LATITUDE = _
    "The Latitude value was rejected by the scope."
Public Const SCODE_BAD_UTCOFFSET = vbObjectError + &H413
Public Const MSG_BAD_UTCOFFSET = _
    "The UTC offset value was rejected by the scope."
Public Const SCODE_BAD_DATETIME = vbObjectError + &H414
Public Const MSG_BAD_DATETIME = _
    "The date/time value was rejected by the scope."
Public Const SCODE_PARKED As Long = vbObjectError + &H415
Public Const MSG_PARKED As String = _
    "The scope is parked."
Public Const SCODE_WRONG_TRACKING As Long = vbObjectError + &H416
Public Const MSG_WRONG_TRACKING As String = _
    "Wrong tracking state"
Public Const SCODE_BAD_TARGET = vbObjectError + &H417
Public Const MSG_BAD_TARGET = _
    "The target value is not set."
Public Const SCODE_ASLEEP = vbObjectError + &H418
Public Const MSG_ASLEEP = _
    "The scope is asleep."
Public Const SCODE_PULSE_DUR = vbObjectError + &H419
Public Const MSG_PULSE_DUR = _
    "Bad duration."
Public Const SCODE_GPSREBOOT_FAIL = vbObjectError + &H419
Public Const MSG_GPSREBOOT_FAIL = _
    "Failed to warm-reboot. Power cycle the Autostar II and reconnect."
Public Const SCODE_NOT_LX200GPS As Long = vbObjectError + &H420
Public Const MSG_NOT_LX200GPS As String = _
    "Something other than a Meade LX200GPS/R appears to be connected to the COM port."
    
' Focuser-specific error messages & codes.
Public Const SCODE_FOCUSER_BASE = vbObjectError + &H500
Public Const SCODE_FOCUS_CONNECT_FAILED = SCODE_FOCUSER_BASE + 1
Public Const MSG_FOCUS_CONNECT_FAILED = _
        "The focuser is not connected to the specified COM port."
Public Const SCODE_MOVE_POSITION_GREATER_THAN_MAXINCREMENT = SCODE_FOCUSER_BASE + 2
Public Const MSG_MOVE_POSITION_GREATER_THAN_MAXINCREMENT = _
        "The focuser move position magnitude is greater than MaxIncrement."
Public Const SCODE_FOCUSER_NOT_CONNECTED As Long = SCODE_FOCUSER_BASE + 3
Public Const MSG_FOCUSER_NOT_CONNECTED As String = _
    "The focuser is not connected."

