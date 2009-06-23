Module GlobalVariables

#Region "Error Constants"
    Public ERR_SOURCE As String = "ASCOM FilterWheel Simulator"

    Public SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
    Public MSG_NOT_IMPLEMENTED As String = _
        " is not implemented by this filter wheel driver."

    Public SCODE_DLL_LOADFAIL As Integer = vbObjectError + &H401
    ' Error message for above generated at run time

    Public SCODE_NOT_CONNECTED As Integer = vbObjectError + &H402
    Public MSG_NOT_CONNECTED As String = _
        "The filter wheel is not connected"

    Public SCODE_VAL_OUTOFRANGE As Integer = vbObjectError + &H404
    Public MSG_VAL_OUTOFRANGE As String = _
        "The value is out of range"
#End Region


#Region "Variables"

    ' ---------
    ' Variables
    ' ---------
    Public g_Profile As ASCOM.Helper.Profile
    Public g_trafficDialog As ShowTrafficForm           ' Traffic window


    ' ----------------------------------------------------------
    ' Driver ID and descriptive string that shows in the Chooser
    ' ----------------------------------------------------------
    Public g_csDriverID As String = "ASCOM.FilterWheelSimulator.FilterWheel"
    Public g_csDriverDescription As String = "FilterWheelSimulator FilterWheel"


    ' -----------
    ' Timer stuff
    ' -----------
    Public Const TIMER_INTERVAL As Double = 0.25      ' 4 tix/sec
    Public Const MOVE_INTERVAL As Integer = 2000       ' 2 secs per filter slot


#End Region
End Module
