Attribute VB_Name = "Global"
'---------------------------------------------------------------------
' Copyright © 2003 Diffraction Limited
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". DIFFRACTION LIMITED. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'
'   ==========
'   Global.bas
'   ==========
'
' Written:  2003/06/24   Douglas B. George <dgeorge@cyanogen.com>
'
' Edits:
'
' When       Who     What
' ---------  ---     --------------------------------------------------
' 2003/06/24 dbg     Initial edit
' 2003/09/03 rbd     Remove hard-reference to Dome Simulator for enum
'                    ShutterStatus, add definition here.
' -----------------------------------------------------------------------------
Public Const d2r = PI / 180#
Public Const r2d = 1# / d2r
Public Const h2r = PI / 12#

Public Const DOMEID As String = "ASCOMDome.Dome"
Public Const SCOPEID As String = "ASCOMDome.Telescope"
Public Const DESC As String = "Dome Control Panel"

' Objects
Public TheScope As Object
Public LocalScope As Telescope ' Local copy of the scope object, so we can connect
Public TheDome As Object
Public Util As DriverHelper.Util
Public Profile As DriverHelper.Profile

' Dome status and controls
Public ScopeIsConnected As Boolean
Public DomeIsConnected As Boolean

Public DomeStatus As String
Public DomeAltitude As Double
Public DomeAzimuth As Double
Public DomeAtHome As Boolean
Public DomeAtPark As Boolean
Public DomeShutterStatus As ShutterState
Public DomeSlewing As Boolean
Public DomeCanFindHome As Boolean
Public DomeCanPark As Boolean
Public DomeCanSetAltitude As Boolean
Public DomeCanSyncAzimuth As Boolean
Public DomeCanSetShutter As Boolean
Public DomeCanSetPark As Boolean
Public UseHomeAzimuth As Boolean
Public HomeAzimuth As Double
Public AutoHome As Boolean

' Current scope and dome positions
Public ScopeRA As Double
Public ScopeDec As Double
Public ScopeAlt As Double
Public ScopeAz As Double
Public TargetAlt As Double
Public TargetAz As Double

' Dome parameters
Public DomeRadius As Double
Public Latitude As Double
Public Longitude As Double
Public OffsetEast As Double
Public OffsetNorth As Double
Public OffsetHeight As Double
Public OffsetOptical As Double
Public IsGerman As Boolean

' Global variables for Telescope class
Public GotRADec As Boolean
Public ScopeConnectedCount As Integer
Public IsSlewing As Boolean

' Global variables for Dome class
Public DomeConnectedCount As Integer
