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
' FilterWheel simulator implementation.
'
' From Focus Simulator, from Scope Simulator written 28-Jun-00
' Robert B. Denny <rdenny@dc3.com>
' Structure retained, otherwise rewriten into Filter Wheel Simulator
' by Mark Crossley in Nov 2008
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 18-Nov-08 mpc     Initial edit - Starting from Focuser Simulator
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ASCOM FilterWheel Simulator"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented by this filter wheel driver."
    
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H401
    ' Error message for above generated at run time
    
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = _
    "The filter wheel is not connected"
       
Public Const SCODE_VAL_OUTOFRANGE As Long = vbObjectError + &H404
Public Const MSG_VAL_OUTOFRANGE As String = _
    "The value is out of range"
