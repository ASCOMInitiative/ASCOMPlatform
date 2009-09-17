Public Class UtilExamples
    Sub Example()
        Dim Utl As ASCOM.Utilities.Interfaces.IUtil
        Dim PrmString, RspString As String, PrmDouble, RspDouble As Double, RspDate As Date

        Utl = New ASCOM.Utilities.Util

        'Get current julian datetime and ASCOM Platform version 
        RspDouble = Utl.JulianDate
        RspString = Utl.PlatformVersion

        With Utl 'Use "with" to get several properties
            RspString = .TimeZoneName 'Time zone name
            RspString = .TimeZoneOffset 'Time zone offset in hours
            RspDate = .UTCDate 'Current UTC datetime
        End With

        'Wait for 5 milli-seconds
        Utl.WaitForMilliseconds(5)

        'Miscellaneous formatting functions
        PrmDouble = 30.123456789 : RspString = Utl.DegreesToDM(PrmDouble, ":")
        PrmDouble = 60.987654321 : RspString = Utl.DegreesToDMS(PrmDouble, ":", ":", "", 4)
        PrmDouble = 50.123453456 : RspString = Utl.DegreesToHM(PrmDouble)
        PrmDouble = 70.763245689 : RspString = Utl.DegreesToHMS(PrmDouble)
        PrmDouble = 40.452387904 : RspString = Utl.DegreesToHMS(PrmDouble, " hours, ", " minutes, ", " seconds", 3)
        PrmDouble = 15.567234086 : RspString = Utl.HoursToHM(PrmDouble)
        PrmDouble = 9.4367290317 : RspString = Utl.HoursToHMS(PrmDouble)

        PrmString = "43:56:78.2567" : RspDouble = Utl.DMSToDegrees(PrmString)
        PrmString = "14:39:23" : RspDouble = Utl.HMSToDegrees(PrmString)
        PrmString = "14:37:23" : RspDouble = Utl.HMSToHours(PrmString)
        
        Utl.Dispose() 'Clean up and dispose of the util component
        Utl = Nothing

    End Sub
End Class
