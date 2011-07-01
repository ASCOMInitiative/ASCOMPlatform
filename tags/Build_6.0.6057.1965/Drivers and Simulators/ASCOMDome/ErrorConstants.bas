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
' 13-Jan-00 rbd     Add ERR_SOURCE, necessary for proper errors, and
'                   property range error, used in template.
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ASCOM Telescope Hub for Desktop Universe"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented by this telescope driver object."
Public Const SCODE_PROP_RANGE_ERROR As Long = vbObjectError + &H401
Public Const MSG_PROP_RANGE_ERROR As String = _
    " is not implemented by this telescope driver object."
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = "Telescope not connected."
Public Const SCODE_NOT_SELECTED As Long = vbObjectError + &H403
Public Const MSG_NOT_SELECTED As String = "Telescope driver not selected."
Public Const SCODE_CONNECTED As Long = vbObjectError + &H404
Public Const MSG_CONNECTED As String = "Telescope driver already connected."
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H405
    ' Error message for above generated at run time
Public Const SCODE_REGERR As Long = vbObjectError + &H406
    ' This uses runtime-generated messages and source
Public Const SCODE_SLAVE_NOT_POSSIBLE As Long = vbObjectError + &H407
Public Const MSG_SLAVE_NOT_OPSSIBLE As String = "Current dome or telescope state does not permit slaving."
    
    
Public Const ERR_SOURCE_DOME As String = "ASCOMDome"
