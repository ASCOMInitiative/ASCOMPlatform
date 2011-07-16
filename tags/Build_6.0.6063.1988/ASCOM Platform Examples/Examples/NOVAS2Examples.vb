Public Class NOVAS2Examples
    Private Utl As ASCOM.Utilities.Util

    Private StarStruct As ASCOM.Astrometry.CatEntry 'Define structures
    Private Body As ASCOM.Astrometry.BodyDescription
    Private Earth As ASCOM.Astrometry.BodyDescription
    Private LocationStruct As ASCOM.Astrometry.SiteInfo

    Private POS(2), VEL(2), POSNow(2), RANow, DECNow, rc, Distance, JD As Double

    Private Const J2000 As Double = 2451545.0 'Julian day for J2000 epoch

    Sub Example()
        Utl = New ASCOM.Utilities.Util 'Create structures and get Julian date
        JD = Utl.JulianDate

        StarStruct = New ASCOM.Astrometry.CatEntry
        Body = New ASCOM.Astrometry.BodyDescription
        Earth = New ASCOM.Astrometry.BodyDescription
        LocationStruct = New ASCOM.Astrometry.SiteInfo

        StarStruct.RA = 12.0 'Initialise star structure with arbitary data
        StarStruct.Dec = 30.0
        StarStruct.Parallax = 2.0
        StarStruct.ProMoDec = 1.5
        StarStruct.ProMoRA = 2.5
        StarStruct.RadialVelocity = 3

        ASCOM.Astrometry.NOVAS.NOVAS2.StarVectors(StarStruct, POS, VEL) 'Convert the star data to position and velocity vectors
        ASCOM.Astrometry.NOVAS.NOVAS2.Precession(J2000, POS, JD, POSNow) 'Precess position vector to current date
        ASCOM.Astrometry.NOVAS.NOVAS2.Vector2RADec(POSNow, RANow, DECNow) 'Convert vector back into precessed RA and Dec
        MsgBox("POS StarVectors RA: " & Utl.HoursToHMS(RANow, ":", ":", "", 3) & "  DEC: " & Utl.DegreesToDMS(DECNow, ":", ":", "", 3))

        Body.Name = "Mars" 'Initialise planet object
        Body.Type = ASCOM.Astrometry.BodyType.MajorPlanet
        Body.Number = ASCOM.Astrometry.Body.Mars

        Earth.Name = "Earth" 'Initialise Earth object
        Earth.Type = ASCOM.Astrometry.BodyType.MajorPlanet
        Earth.Number = ASCOM.Astrometry.Body.Earth

        LocationStruct.Height = 80 'Initialise observing location structure
        LocationStruct.Latitude = 51
        LocationStruct.Longitude = 0.0
        LocationStruct.Pressure = 1000.0
        LocationStruct.Temperature = 10.0

        'Find Mars astrometric position at Julian date JD
        rc = ASCOM.Astrometry.NOVAS.NOVAS2.AstroPlanet(JD, Body, Earth, RANow, DECNow, Distance)
        MsgBox("Mars astrometric RC: " & rc & " RA: " & Utl.HoursToHMS(RANow) & "  DEC: " & Utl.DegreesToDMS(DECNow, ":", ":"))

        'Find Mars topocentric position at Julian date JD
        rc = ASCOM.Astrometry.NOVAS.NOVAS2.TopoPlanet(JD, Body, Earth, 0.0, LocationStruct, RANow, DECNow, Distance)
        MsgBox("Mars topocentric RC: " & rc & " RA: " & Utl.HoursToHMS(RANow) & "  DEC: " & Utl.DegreesToDMS(DECNow, ":", ":"))
    End Sub
End Class
