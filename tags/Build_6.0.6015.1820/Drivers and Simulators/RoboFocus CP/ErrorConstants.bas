Attribute VB_Name = "ErrorConstants"
'====================
' ErrorConstants.bas
'====================
'
' Declarations of error codes and error strings used in the ASCOM
' RoboFocus implementation.
'
' Initial code by Jon Brewster in Apr 2003
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 24-Apr-05 jab     Initial edit
' 23-Sep-08 jab     Make ERR_SOURCE a variable for multi-instance
'---------------------------------------------------------------------

Option Explicit

Public ERR_SOURCE As String

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented by this focus driver."
    
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H401
    ' The error message for LOADFAIL is generated at run time
    
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = _
    "The focuser is not connected"
    
Public Const SCODE_MOVE_WHILE_COMP As Long = vbObjectError + &H403
Public Const MSG_MOVE_WHILE_COMP As String = _
    "The focuser cannot be moved while temperature compensation running"
    
Public Const SCODE_VAL_OUTOFRANGE As Long = vbObjectError + &H404
Public Const MSG_VAL_OUTOFRANGE As String = _
    "The value is out of range"

Public Const SCODE_MOVEMENT_FAIL As Long = vbObjectError + &H405
Public Const MSG_MOVEMENT_FAIL As String = _
    "Focuser movement has failed"
