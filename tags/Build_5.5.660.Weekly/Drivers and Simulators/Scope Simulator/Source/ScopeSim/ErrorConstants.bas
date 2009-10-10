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
' 05-Mar-04 jab     added illegal while parked
' 27-Jul-05 rbd     Changed symbols for illegal while parked
' 12-Apr-07 rbd     New error for new startup refactoring
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ASCOM Scope Simulator"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented by this telescope driver object."
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H401
    ' Error message for above generated at run time
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = _
    "The scope is not connected"
Public Const SCODE_PROP_NOT_SET As Long = vbObjectError + &H403
Public Const MSG_PROP_NOT_SET As String = _
    "This property has not yet been set"
Public Const SCODE_NO_TARGET_COORDS As Long = vbObjectError + &H404
Public Const MSG_NO_TARGET_COORDS As String = _
    "Target coordinates have not yet been set"
Public Const SCODE_VAL_OUTOFRANGE As Long = vbObjectError + &H405
Public Const MSG_VAL_OUTOFRANGE As String = _
    "The property value is out of range"
Public Const SCODE_ILLEGAL_WHILE_PARKED As Long = vbObjectError + &H406
Public Const MSG_ILLEGAL_WHILE_PARKED As String = _
    "Illegal operation while parked"
Public Const SCODE_WRONG_TRACKING As Long = vbObjectError + &H407
Public Const MSG_WRONG_TRACKING As String = _
    "Wrong tracking state"
Public Const SCODE_START_CONFLICT As Long = vbObjectError + &H408
Public Const MSG_START_CONFLICT As String = _
    "Failed: Attempt to create Telescope during manual startup"


