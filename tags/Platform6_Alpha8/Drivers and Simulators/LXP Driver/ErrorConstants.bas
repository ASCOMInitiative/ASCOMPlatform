Attribute VB_Name = "ErrorConstants"
'---------------------------------------------------------------------
' Copyright © 2001-2002 SPACE.com Inc., New York, NY
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
' Written:  27-Jan-01   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 27-Jan-01 rbd     Initial edit
' 01-Feb-01 rbd     Add property not set message
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ASCOM Generic LX200 Driver"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented in this driver."
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = _
    "The scope is not connected."
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
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H411
    ' This uses runtime-generated messages
Public Const SCODE_BELOW_HORIZ = vbObjectError + &H412
Public Const MSG_BELOW_HORIZ = _
    "The selected coordinates are below the horizon."
Public Const SCODE_BELOW_MIN_EL = vbObjectError + &H413
Public Const MSG_BELOW_MIN_EL = _
    "The selected coordinates are below the current minimum elevation setting."
Public Const SCODE_SLEW_FAIL = vbObjectError + &H414
Public Const MSG_SLEW_FAIL = _
    "The slew failed for some unknown reason."
Public Const SCODE_SETUP_CONNECTED = vbObjectError + &H416
Public Const MSG_SETUP_CONNECTED = _
    "You cannot change the driver's configuration while it is connected to a telescope."
Public Const SCODE_PROPNOTSET = vbObjectError + &H417
Public Const MSG_PROPNOTSET = " property has not yet been set."
Public Const SCODE_TGTSETERR = vbObjectError + &H418
Public Const MSG_TGTSETERR = "Controller rejected set "
