Public Class TransformExamples
    Private T As ASCOM.Astrometry.Transform.ITransform
    Private U As ASCOM.Utilities.Util

    Sub Example()
        T = New ASCOM.Astrometry.Transform.Transform 'Create transform and util components
        U = New ASCOM.Utilities.Util

        'Set site parameters: 51.0 degrees North, 1 degree west, elevation 80 metres, temperature 20 degrees celsius
        T.SiteLatitude = 51.0
        T.SiteLongitude = -1.0 ' West longitudes are negative, east are positive
        T.SiteElevation = 80
        T.SiteTemperature = 20.0

        'Set Arcturus J2000 co-ordinates and read off the corresponding Topocentric co-ordinates 
        T.SetJ2000(U.HMSToHours("14:15:38.951"), U.DMSToDegrees("19:10:38.06"))
        MsgBox("RA Topo: " & U.DegreesToHMS(T.RATopocentric, ":", ":", "", 3) & " DEC Topo: " & U.DegreesToDMS(T.DECTopocentric, ":", ":", "", 3))

        'Set arbitary topocentric co-ordinates and read off the corresponding J2000 co-ordinates (site parameters remain the same)
        T.SetTopocentric(11.0, 25.0)
        MsgBox("RA J2000: " & U.DegreesToHMS(T.RAJ2000, ":", ":", "", 3) & " DEC J2000: " & U.DegreesToDMS(T.DECJ2000, ":", ":", "", 3))

        'Clean up components
        T.Dispose()
        T = Nothing
        U.Dispose()
        U = Nothing
    End Sub
End Class
