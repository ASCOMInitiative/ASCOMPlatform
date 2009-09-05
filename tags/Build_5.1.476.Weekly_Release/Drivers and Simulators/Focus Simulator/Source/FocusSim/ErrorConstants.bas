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
' Focuser simulator implementation.
'
' From Scope Simulator written 28-Jun-00   Robert B. Denny <rdenny@dc3.com>
' Structure retained, otherwise rewriten into Focus Simulator
' by Jon Brewster in Feb 2003
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 24-Feb-03 jab     Initial edit - Starting from Telescope Simulator
' 02-mar-03 jab     added move while compensating error
' 12-Apr-07 rbd     New error for new startup refactoring
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ASCOM Focus Simulator"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented by this focus driver."
    
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H401
    ' Error message for above generated at run time
    
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = _
    "The focuser is not connected"
    
Public Const SCODE_MOVE_WHILE_COMP As Long = vbObjectError + &H403
Public Const MSG_MOVE_WHILE_COMP As String = _
    "The focuser cannot be moved while temperature compensation running"
    
Public Const SCODE_VAL_OUTOFRANGE As Long = vbObjectError + &H404
Public Const MSG_VAL_OUTOFRANGE As String = _
    "The value is out of range"

Public Const SCODE_START_CONFLICT As Long = vbObjectError + &H405
Public Const MSG_START_CONFLICT As String = _
    "Failed: Attempt to create Focuser during manual startup"


