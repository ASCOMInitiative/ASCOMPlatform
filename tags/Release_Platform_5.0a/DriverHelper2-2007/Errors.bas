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
' 06-Apr-07 rbd     Message for new default trace file location
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
' Config.bas (base = &H440)
'
Public Const SCODE_CORRUPT_CONFIG As Long = vbObjectError + &H440
Public Const MSG_CORRUPT_CONFIG As String = "ASCOM Registry XML: "
Public Const SCODE_NOEXIST_UVAL As Long = vbObjectError + &H441
Public Const MSG_NOEXIST_UVAL As String = "Unnamed value does not exist"
Public Const SCODE_NOEXIST_VAL As Long = vbObjectError + &H442
Public Const MSG_NOEXIST_VAL As String = "Value does not exist: "
Public Const SCODE_NOEXIST_KEY As Long = vbObjectError + &H443
Public Const MSG_NOEXIST_KEY As String = "Key does not exist: "
Public Const SCODE_XML_LOADFAIL As Long = vbObjectError + &H444
Public Const MSG_XML_LOADFAIL As String = "Failed to load XML from "
Public Const SCODE_XML_WRITELOCK As Long = vbObjectError + &H445
Public Const MSG_XML_WRITELOCK As String = "Failed to write XML to "
Public Const SCODE_SPECFLD_FAIL As Long = vbObjectError + &H446
Public Const MSG_SPECFLD_FAIL As String = "Failed to get path to default serial trace file"

