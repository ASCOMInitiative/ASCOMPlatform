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
    Public g_Profile As HelperNET.Profile


    ' ----------------------------------------------------------
    ' Driver ID and descriptive string that shows in the Chooser
    ' ----------------------------------------------------------
    Public Const g_csDriverID As String = "ASCOM.FilterWheelSim.FilterWheel"
    Public Const g_csDriverDescription As String = "FilterWheelSimulator FilterWheel"




#End Region

End Module
