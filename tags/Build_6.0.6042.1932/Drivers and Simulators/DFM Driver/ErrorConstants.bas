Attribute VB_Name = "ErrorConstants"
'---------------------------------------------------------------------
'   ==================
'   ERRORCONSTANTS.BAS
'   ==================
'
' Declarations of error codes and error strings used in the ASCOM
' Telescope Interface implementation.
'
' Written:  12-Oct-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 15-Oct-00 rbd     Initial edit.
' 13-Jan-01 rbd     Change not implemented message
' 13-Oct-08 rbd     5.0.1 - For DFM TCS driver, copied from ACL driver
'                   Additional errors for trackrate support.
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "DFM TCS Driver"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented in this driver."
Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H401
Public Const MSG_NOT_CONNECTED As String = _
    "The scope is not connected."
Public Const SCODE_NOT_ACL As Long = vbObjectError + &H402
Public Const MSG_NOT_ACL As String = _
    "Something other than a DFM TCS appears to be connected to the COM port."
Public Const SCODE_NEED_SETUP As Long = vbObjectError + &H402
Public Const MSG_NEED_SETUP As String = _
    "You must configure this driver before using it."
Public Const SCODE_NO_SCOPE As Long = vbObjectError + &H403
Public Const MSG_NO_SCOPE As String = _
    "There doesn't appear to be anything connected to the COM port."
Public Const SCODE_REGERR As Long = vbObjectError + &H404
    ' This uses runtime-generated messages
Public Const SCODE_ACLERR As Long = vbObjectError + &H405
    ' This uses runtime generated messages
Public Const SCODE_MNCP_RECVFAIL As Long = vbObjectError + &H406
Public Const MSG_MNCP_RECVFAIL As String = _
    "MNCP receive failure."
Public Const SCODE_MNCP_SBUFOVFLW As Long = vbObjectError + &H407
Public Const MSG_MNCP_SBUFOVFLW As String = _
    "MNCP send buffer overflow."
Public Const SCODE_PROP_NOT_SET As Long = vbObjectError + &H408
Public Const MSG_PROP_NOT_SET As String = _
    "This property has not yet been set."
Public Const SCODE_TARGET_NOT_SET As Long = vbObjectError + &H408
Public Const MSG_TARGET_NOT_SET As String = _
    "The target coordinates have not yet been set."
Public Const SCODE_PROP_RANGE_ERROR As Long = vbObjectError + &H410
Public Const MSG_PROP_RANGE_ERROR As String = _
    "The supplied value is out of range for this property."
Public Const SCODE_TRKRNG_FMT_ERROR As Long = vbObjectError + &H418
Public Const MSG_TRKRNG_FMT_ERROR As String = _
    "The TCS did not respond with exactly two tracking rates."
Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H411
    ' This uses runtime-generated messages
Public Const SCODE_BELOW_HORIZ = vbObjectError + &H412
Public Const MSG_BELOW_HORIZ = _
    "The selected coordinates are below the horizon."
Public Const SCODE_SETUP_CONNECTED = vbObjectError + &H416
Public Const MSG_SETUP_CONNECTED = _
    "You cannot change the driver's configuration while it is connected to a telescope."
Public Const SCODE_SYNC_FAILED As Long = vbObjectError + &H409
Public Const MSG_SYNC_FAILED As String = _
    "The sync operation failed for some reason."
Public Const SCODE_BELOW_MIN_EL = vbObjectError + &H413
Public Const MSG_BELOW_MIN_EL = _
    "The selected coordinates are below the current minimum elevation setting."
Public Const SCODE_SLEW_FAIL = vbObjectError + &H414
Public Const MSG_SLEW_FAIL = _
    "The slew failed for some unknown reason."
Public Const SCODE_SLEW_16FAIL = vbObjectError + &H415
Public Const MSG_SLEW_16FAIL = _
    "The slew failed but the 16"" scope reported success. Check the handbox for failure reason."
Public Const SCODE_MNCP_SENDFAIL As Long = vbObjectError + &H417
Public Const MSG_MNCP_SENDFAIL As String = _
    "MNCP send failure."

