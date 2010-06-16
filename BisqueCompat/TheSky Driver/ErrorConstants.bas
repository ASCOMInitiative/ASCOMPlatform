Attribute VB_Name = "ErrorConstants"
'---------------------------------------------------------------------
' Copyright © 2000-2001 SPACE.com Inc., New York, NY
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
' Telescope Interface implementation.
'
' Written:  27-Jun-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 27-Jun-00 rbd     Initial edit
' 13-Jan-01 rbd     Changed not implemented message
' 09-Aug-03 rbd     Removed references to RAS from messages.
' 16-Jun-10 rbd     Add ?not yet set" message (Target Coordinates)
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ASCOM TheSky Driver"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented by this telescope driver object."
Public Const SCODE_PROP_RANGE_ERROR As Long = vbObjectError + &H401
Public Const MSG_PROP_RANGE_ERROR As String = _
    "The supplied value is out of range for this property."
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = _
    "Not connected to TheSky."
Public Const SCODE_NOT_SET As Long = vbObjectError + &H403
Public Const MSG_NOT_SET As String = _
    " has not yet been set."""
Public Const SCODE_SLEW_FAILURE As Long = vbObjectError + &H404
Public Const MSG_SLEW_FAILURE As String = _
    "TheSky reports slew failure code "

