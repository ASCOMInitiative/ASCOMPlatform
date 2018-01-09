Option Explicit On

Module CommonDeclarations
    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Public s_csDriverID As String = "ASCOM.SwitchSimulator.Switch"
    Public s_csDriverDescription As String = "SwitchSimulator Switch"
    ' ---------------------
    ' Simulation Parameters
    ' ---------------------

    Public Const NUM_SWITCHES As Short = 9 ' 0-8 as available switch "slots"
    Public Const RegVer As String = "1.2"
    ' ---------
    ' Variables
    ' ---------

    Public frmHandbox As HandBoxForm
    Public bUseHandbox As Boolean = True

    ' ---------------
    ' State Variables
    ' ---------------

    Public g_bConnected As Boolean ' Switch is connected
    Public g_iMaxSwitch As Short ' Maximum valid switch number
    Public g_bZero As Boolean ' Include switch zero

    Public g_bCanGetSwitch(NUM_SWITCHES - 1) As Boolean ' allowed to "get" array
    Public g_bCanSetSwitch(NUM_SWITCHES - 1) As Boolean ' allowed to "set" array

    Public g_bSwitchState(NUM_SWITCHES - 1) As Boolean ' actual switch state array
    Public g_sSwitchName(NUM_SWITCHES - 1) As String ' switch name array
End Module
