Attribute VB_Name = "Public"
' -----------------------------------------------------------------------------
'   ============
'   Public.BAS
'   ============
'
' Plain old telescope hub public definitions and variables
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 02-Sep-06 jab     Initial edit
' 04-Jun-07 jab     Eliminate g_bRunExecutable, use App.StartMode directly, to
'                   avoid startup problems with refactored initialization.
' -----------------------------------------------------------------------------

Option Explicit

'-----------------------------------
' startup / shutdown state managment
'-----------------------------------

Enum ComponentStates
    csLoaded = 0
    csStarting = 1
    csRunning = 2
End Enum

'-----------
' constants
'-----------

' parameter management constants
Public Const INVALID_PARAMETER As Double = -10000#
Public Const EMPTY_PARAMETER As Double = -20000#
Public Const ALG_UNKNOWN As Double = -1

' naming
Public Const PIPE_SCOPE As String = "Pipe.Telescope"
Public Const PIPE_DOME As String = "Pipe.Dome"
Public Const PIPE_FOCUSER As String = "Pipe.Focuser"
Public Const PIPE_DESC As String = "Pipe (ASCOM connection debugger)"
Public Const HUB_SCOPE As String = "Hub.Telescope"
Public Const HUB_DOME As String = "Hub.Dome"
Public Const HUB_FOCUSER As String = "Hub.Focuser"
Public Const HUB_DESC As String = "Hub (ASCOM driver hub)"

' registry version - change if registry needs to change
Public Const RegVer As String = "0.1"

Public g_Util As DriverHelper.Util

'------------------------------------
' who are we variables (pipe or hub)
'------------------------------------

Public g_sSCOPE As String
Public g_sDOME As String
Public g_sFOCUSER As String
Public g_sDESC As String

' ------------------------
' connectivity management
' ------------------------

Public g_Profile As DriverHelper.Profile
Public g_sScopeID As String
Public g_sScopeName As String
Public g_Scope As Object
Public g_IScope As ITelescope
Public g_bConnected As Boolean              ' Whether scope is connected
Public g_bManual                            ' Manual connection
Public g_bForceLate                         ' Force classic late binding

Public g_DomeProfile As DriverHelper.Profile
Public g_sDomeID As String
Public g_sDomeName As String
Public g_Dome As Object
Public g_IDome As IDome
Public g_bDomeConnected As Boolean         ' Whether dome is connected
Public g_bDomeManual                       ' Manual connection

Public g_FocuserProfile As DriverHelper.Profile
Public g_sFocuserID As String
Public g_sFocuserName As String
Public g_Focuser As Object
Public g_IFocuser As IFocuser
Public g_bFocuserConnected As Boolean       ' Whether focuser is connected
Public g_bFocuserManual                     ' Manual connection

Public g_ComponentState As ComponentStates
Public g_iConnections As Integer
Public g_iDomeConnections As Integer
Public g_iFocuserConnections As Integer

'----------------
' timer controls
'----------------

' Public g_ltimerID As Long                   ' timer ID

'------------------------------
' refraction state enumeration
'------------------------------

Public Enum Refraction
    refUnknown = 0
    refYes = 1
    refNo = 2
End Enum

' --------------------------
' GUI handles and variables
' --------------------------

Public g_setupDlg As frmSetup              ' Setup dialog
Public g_handBox As frmHandBox             ' Hand box

Public g_bSetupAdvanced As Boolean         ' True to display advanced options in setup window
Public g_bDomeMode As Boolean
Public g_bFocusMode As Boolean

