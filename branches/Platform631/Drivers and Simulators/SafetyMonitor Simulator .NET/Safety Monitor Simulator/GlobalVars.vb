Module GlobalVars
    Friend Const CAN_EMERGENCY_SHUTDOWN As String = "Can Emergency Shutdown"
    Friend Const CAN_IS_GOOD As String = "Can Is Good"
    Friend Const SAFETY_MONITOR As String = "SafetyMonitor"
    Friend s_csDriverID As String = "ASCOM.Simulator.SafetyMonitor"
    Friend s_csDriverDescription As String = "Simulator"

    Friend g_Connected As Boolean 'Connected state of the safety monitor
    Friend g_CanEmergencyShutdown, g_CanIsGood As Boolean 'Variables to hold the two can variables

End Module
