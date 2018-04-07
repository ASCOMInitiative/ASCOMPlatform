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
' Declarations of error codes and error strings used in the ASCOM
' Telescope simulator implementation.
'
' Written:  28-Jun-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 28-Jun-00 rbd     Initial edit
' 28-Jan-00 rbd     Explicit not implemented messages, source constant
' 25-Sep-03 jab     Finished new V2 spec support (V2 definition changed)
' 10-Jan-06 dpp     Focuser implementation
' 04-Jun-07 jab     New error for new startup refactoring
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "Pipe/Hub"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented by this driver."
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H401
    ' Error message for above generated at run time
Public Const SCODE_NO_SCOPE As Long = vbObjectError + &H402
Public Const MSG_NO_SCOPE As String = _
    "The scope is not open"
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H403
Public Const MSG_NOT_CONNECTED As String = _
    "The scope is not connected"
Public Const SCODE_PROP_NOT_SET As Long = vbObjectError + &H404
Public Const MSG_PROP_NOT_SET As String = _
    "This property has not yet been set"
Public Const SCODE_NO_TARGET_COORDS As Long = vbObjectError + &H405
Public Const MSG_NO_TARGET_COORDS As String = _
    "Target coordinates have not yet been set"
Public Const SCODE_VAL_OUTOFRANGE As Long = vbObjectError + &H406
Public Const MSG_VAL_OUTOFRANGE As String = _
    "The property value is out of range"
Public Const SCODE_NO_DOME As Long = vbObjectError + &H407
Public Const MSG_NO_DOME As String = _
    "The dome is not open"
Public Const SCODE_DOME_NOT_CONNECTED As Long = vbObjectError + &H408
Public Const MSG_DOME_NOT_CONNECTED As String = _
    "The dome is not connected"
Public Const SCODE_SLEW_WHILE_SLAVED As Long = vbObjectError + &H409
Public Const MSG_SLEW_WHILE_SLAVED As String = _
    "Cannot slew while slaved"
Public Const SCODE_SLEW_WHILE_PARKED As Long = vbObjectError + &H409
Public Const MSG_SLEW_WHILE_PARKED As String = _
    "Illegal operation while parked"
Public Const SCODE_CAN_NOT_SET_PIERSIDE As Long = vbObjectError + &H410
Public Const MSG_CAN_NOT_SET_PIERSIDE As String = _
    "Can not set side of pier"
Public Const SCODE_WRONG_TRACKING As Long = vbObjectError + &H411
Public Const MSG_WRONG_TRACKING As String = _
    "Wrong tracking state"
Public Const SCODE_NO_FOCUSER As Long = vbObjectError + &H412
Public Const MSG_NO_FOCUSER As String = "The focuser is not available"
Public Const SCODE_FOCUSER_NOT_CONNECTED As Long = vbObjectError + &H413
Public Const MSG_FOCUSER_NOT_CONNECTED As String = _
    "The focuser is not connected"
Public Const SCODE_START_CONFLICT As Long = vbObjectError + &H415
Public Const MSG_START_CONFLICT As String = _
    "Failed: Attempt to create ActiveX object during manual startup"
