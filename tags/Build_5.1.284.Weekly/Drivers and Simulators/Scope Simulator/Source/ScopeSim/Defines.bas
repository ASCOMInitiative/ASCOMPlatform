Attribute VB_Name = "Defines"
' -----------------------------------------------------------------------------
'   =============
'    Defines.BAS
'   =============
'
' ASCOM scope simulator defines module
'
' Written:  04-Mar-04   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 04-Mar-04 rbd     initial pull out from Startup module, and V2 features
' -----------------------------------------------------------------------------

Option Explicit

Public Const DEG_RAD As Double = 0.0174532925
Public Const RAD_DEG As Double = 57.2957795
Public Const HRS_RAD As Double = 0.2617993881
Public Const RAD_HRS As Double = 3.81971863
Public Const ALERT_TITLE As String = "ASCOM Scope Simulator"
Public Const INVALID_PARAMETER As Double = -10000#

'---------------------
' API timer functions
'---------------------

Declare Function SetTimer Lib "user32" _
       (ByVal hWnd As Long, _
       ByVal nIDEvent As Long, _
       ByVal uElapse As Long, _
       ByVal lpTimerFunc As Long) As Long

Declare Function KillTimer Lib "user32" _
       (ByVal hWnd As Long, _
       ByVal nIDEvent As Long) As Long
       
' ---------------------
' Simulation Parameters
' ---------------------
Public Const INSTRUMENT_APERTURE As Double = 0.2            ' 8 inch = 20 cm
Public Const INSTRUMENT_APERTURE_AREA As Double = 0.0269    ' 3 inch obstruction
Public Const INSTRUMENT_FOCAL_LENGTH As Double = 1.26       ' f/6.3 instrument
Public Const INSTRUMENT_NAME As String = "Simulator"        ' Our name
Public Const INSTRUMENT_DESCRIPTION As String = _
    "Software Telescope Simulator for ASCOM" & vbCrLf & _
    "Copyright © 2001-2002, SPACE.com"
Public Const INVALID_COORDINATE As Double = 100000#
'
' Timer interval (sec.)
'
Public Const TIMER_INTERVAL = 0.25                                  ' 4 tix/sec
'
' ASCOM Identifiers
'
Public Const ID As String = "ScopeSim.Telescope"        ' Our DriverID
Public Const DESC As String = "Telescope Simulator"
Public Const RegVer As String = "2.2"

'
' Slew simulation
'

Public Enum slewType
    slewNone = 0
    slewSettle = 1
    slewMoveAxis = 2
    slewRADec = 3
    slewAltAz = 4      ' all types after this use AltAz type slews
    slewPark = 5
    slewHome = 6
End Enum
