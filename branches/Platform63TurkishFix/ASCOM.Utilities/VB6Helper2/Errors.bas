Attribute VB_Name = "COM_Errors"
'---------------------------------------------------------------------
' Copyright © 2004 DC-3 Dreams, SP, Mesa, AZ
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". DC-3 DREAMS, SP MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'   =============
'   COMERRORS.BAS
'   =============
'
' COM error constants
'
' Written:  31-Aug-2004   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 31-Aug-04 rbd     Initial edit, adapted from original Driverhelper
'---------------------------------------------------------------------
Option Explicit

'
' Util.cls (base = &H400)
'
Public Const ERR_SOURCE_UTIL As String = "ASCOM Helper2 Utilities Object"
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H400
    ' This uses runtime-generated message
Public Const SCODE_TIMER_FAIL As Long = vbObjectError + &H431
Public Const MSG_TIMER_FAIL As String = "Hi-res timer failed. Delay out of range?"
'
' Registry.bas (base = &H410)
'
Public Const SCODE_REGERR As Long = vbObjectError + &H410
    ' This uses runtime-generated messages and source

