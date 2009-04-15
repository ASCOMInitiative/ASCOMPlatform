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
' COM error constants for ASCOM CCD camera simulator driver
'
' Written:  22-Aug-00   Robert B. Denny <rdenny@dc3.com>
' Modified by:          Matthias Busch <Matthias.Busch@easysky.de>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Aug-00 rbd     Initial edit
' 28-Aug-00 rbd     Many additions, integration complete.
' 13-Jan-01 rbd     Change not implemented message
' 08-Oct-01 rbd     Change MSG_UNKNOWN_MEADE to refer to ASCOM web
'                   site.
' 06-Feb-02 mab     Copied from Meade LX200 driver
' 25-Feb-07 mab     Removed unused stuff
'---------------------------------------------------------------------

Option Explicit

Public Const ERR_SOURCE As String = "ASCOM CCD Camera Simulator Driver"

Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
Public Const MSG_NOT_IMPLEMENTED As String = _
    " is not implemented in this driver."

Public Const SCODE_SUBFRAME_TOO_LARGE As Long = vbObjectError + &H401
Public Const MSG_SUBFRAME_TOO_LARGE As String = _
    "Subframe is larger than chip size."

Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
Public Const MSG_NOT_CONNECTED As String = _
    "The camera is not connected."

Public Const SCODE_BINNING_FACTOR_TOO_LARGE As Long = vbObjectError + &H403
Public Const MSG_BINNING_FACTOR_TOO_LARGE As String = _
    "Binning Factor too large."

Public Const SCODE_SETUP_CONNECTED = vbObjectError + &H404
Public Const MSG_SETUP_CONNECTED = _
    "You cannot change the driver's configuration while it is connected to a camera."

