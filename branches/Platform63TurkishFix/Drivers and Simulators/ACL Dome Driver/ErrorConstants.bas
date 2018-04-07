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
' Dome Interface implementation.
'
' Written:  12-Oct-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 15-Oct-00 rbd     Initial edit.
' 13-Jan-01 rbd     Change not implemented message
' 13-Jan-04 jab     ACL Dome 1.0.0 - initial version
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ACL Dome Driver"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented in this driver."
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H401
Public Const MSG_NOT_CONNECTED As String = _
    "The dome is not connected."
    
Public Const SCODE_NOT_ACL As Long = vbObjectError + &H402
Public Const MSG_NOT_ACL As String = _
    "Something other than an ACL dome (a modem?) appears to be connected to the COM port."
Public Const SCODE_NEED_SETUP As Long = vbObjectError + &H403
Public Const MSG_NEED_SETUP As String = _
    "You must configure this driver before using it."
Public Const SCODE_NO_SCOPE As Long = vbObjectError + &H404
Public Const MSG_NO_SCOPE As String = _
    "There doesn't appear to be anything connected to the COM port."
Public Const SCODE_REGERR As Long = vbObjectError + &H405
    ' This uses runtime-generated messages
Public Const SCODE_ACLERR As Long = vbObjectError + &H406
    ' This uses runtime generated messages
    
Public Const SCODE_PROP_NOT_SET As Long = vbObjectError + &H409
Public Const MSG_PROP_NOT_SET As String = _
    "This property has not yet been set."
Public Const SCODE_PROP_RANGE_ERROR As Long = vbObjectError + &H410
Public Const MSG_PROP_RANGE_ERROR As String = _
    "The supplied value is out of range for this property."
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H411
    ' This uses runtime-generated messages
Public Const SCODE_SETUP_CONNECTED = vbObjectError + &H413
Public Const MSG_SETUP_CONNECTED = _
    "You cannot change the driver's configuration while it is connected to a dome."
Public Const SCODE_SYNC_FAILED As Long = vbObjectError + &H414
Public Const MSG_SYNC_FAILED As String = _
    "The sync operation failed."
Public Const SCODE_SLEW_FAIL = vbObjectError + &H416
Public Const MSG_SLEW_FAIL = _
    "The slew operation failed."
    
Public Const SCODE_MNCP_RECVFAIL As Long = vbObjectError + &H407
Public Const MSG_MNCP_RECVFAIL As String = _
    "MNCP receive failure."
Public Const SCODE_MNCP_SBUFOVFLW As Long = vbObjectError + &H408
Public Const MSG_MNCP_SBUFOVFLW As String = _
    "MNCP send buffer overflow."
Public Const SCODE_MNCP_SENDFAIL As Long = vbObjectError + &H416
Public Const MSG_MNCP_SENDFAIL As String = _
    "MNCP send failure."

Public Const SCODE_SHUTTER_NOT_OPEN As Long = vbObjectError + &H417
Public Const MSG_SHUTTER_NOT_OPEN As String = _
    "The shutter is not open"
Public Const SCODE_SHUTTER_ERROR As Long = vbObjectError + &H418
Public Const MSG_SHUTTER_ERROR As String = _
    "The shutter is in an unknown state, close it."
Public Const SCODE_DOME_NOT_READY As Long = vbObjectError + &H419
Public Const MSG_DOME_NOT_READY As String = _
    "The dome needs to be homed"

