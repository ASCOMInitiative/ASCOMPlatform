' Sample code for use of Kepler and NOVAS, lifted from ACP Observatory Control
' Software support library. For more info on ACP, see
'
'    http://acp3.dc3.com/
'
' NOTE: YOU WILL HAVE TO PROVIDE YOUR OWN SOURCE OF UTC JULIAN DATE IN PLACE
' OF THE CALL TO Util.SysJulianDate() (intrinsic to ACP's Util object) AND
' YOU CAN DELETE THE CALL TO Util.DeltaT(jd) AND USE NOVAS' INTERNAL DELTA-T
' CALCULATION. IT WILL BE GOOD ENOUGH FOR ALL BUT THE MOST EXACTING USES.
'
' NOTE: THE MPCORB DATABASE GENERATOR AND ACCESS COMPONENTS ARE AVAILABLE ON
' THE ASCOM DOWNLOAD SITE AT:
'
'  http://ASCOM-Standards.org/downloads.html
'
'----------------------------------------------------------------------------------------
'
' -------------
' MajorPlanet() - Return current J2000 coordinates and velocities of the given major planet
' -------------
'
' Return True if planet found, else False
'----------------------------------------------------------------------------------------
Function MajorPlanet(Name, RightAscension, Declination, RightAscensionRate, DeclinationRate)
    Dim pl, tvec, kt, ke, jd, pn
    
    Set pl = CreateObject("NOVAS.Planet")
    Set kt = CreateObject("Kepler.Ephemeris")
    Set ke = CreateObject("Kepler.Ephemeris")
    Select Case LCase(Name)
        Case "mercury":     pn = 1
        Case "venus":       pn = 2
        Case "mars":        pn = 4
        Case "jupiter":     pn = 5
        Case "saturn":      pn = 6
        Case "uranus":      pn = 7
        Case "neptune":     pn = 8
        Case "pluto":       pn = 9
        Case Else:
            MajorPlanet = False                                 ' Planet not found
            Exit Function
    End Select
    pl.Ephemeris = kt                                           ' Plug in target ephemeris gen
    pl.EarthEphemeris = ke                                      ' Plug in Earth ephemeris gen
    pl.Type = 0                                                 ' NOVAS: Major planet
    pl.Name = Name                                              ' Planet name
    pl.Number = pn                                              ' Decoded planet number
    jd = Util.SysJulianDate                                     ' Get current JD
    pl.DeltaT = Util.DeltaT(jd)                                 ' Delta T for NOVAS
    Call GetPositionAndVelocity(pl, jd - (pl.DeltaT / 86400), _
                                RightAscension, Declination, _
                                RightAscensionRate, DeclinationRate)
    Set pl = Nothing                                            ' Releases both Ephemeris objs
    MajorPlanet = True

End Function


'----------------------------------------------------------------------------------------
'
' -------------
' MinorPlanet() - Return name and current J2000 coordinates and velocities of the minor planet
' ------------
'
' May be called in two ways: 
'
' (1) Elements, must be in "MPC 1-line" format. Returns packed designation, 
'     position, and velocity
' (2) Elements = "" And Name given. Name can be MP number (as a string), Name, 
'     provisional designation (2000 SK100), or packed provisional designation 
'     (K00SA0K). In this case, ASCOM.MPCORB is used as the source for elements.
'
' Non-fatal if (2) fails to find in database, just returns false.
'----------------------------------------------------------------------------------------
Function MinorPlanet(Elements, Name, RightAscension, Declination, RightAscensionRate, DeclinationRate)
    Dim kt, ke, pl, jd, mp, key
    
    Set pl = CreateObject("NOVAS.Planet")
    Set kt = CreateObject("Kepler.Ephemeris")
    Set ke = CreateObject("Kepler.Ephemeris")
    pl.Ephemeris = kt                                           ' Plug in target ephemeris gen
    pl.EarthEphemeris = ke                                      ' Plug in Earth ephemeris gen
    pl.Type = 1                                                 ' NOVAS: Minor Planet (Passed to Kepler)
    If Elements = "" Then                                       ' No elements, try using MPCORB
        On Error Resume Next
        Set mp = CreateObject("ASCOM.MPCORB")
        mp.Open
        If Err.Number <> 0 Then
            Util.Console.PrintLine "**MPCORB.MDB database not present."
            MinorPlanet = False
            Exit Function
        End If
        Err.Clear
        mp.GetElements(Name)                                    ' Look up in database
        If Err.Number <> 0 Then                                 ' Not found
            Util.Console.PrintLine "**" & Name & " not found in MPCORB.MDB."
            MinorPlanet = False
            Exit Function
        End If
        On Error Goto 0
        kt.Name = mp.Designation
        If IsNumeric(kt.Name) Then                              ' Numbered object?
            pl.Number = CLng(kt.Name)                           ' NOVAS: Minor planet number
        Else
            pl.Number = 100000                                  ' NOVAS bug: Must set number. Ignored.
        End If
        kt.Epoch = mp.Epoch                                     ' Epoch of osculating elements
        kt.M = mp.M                                             ' Mean anomaly
        kt.n = mp.n                                             ' Mean daily motion (deg/day)
        kt.a = mp.a                                             ' Semimajor axis (AU)
        kt.e = mp.e                                             ' Orbital eccentricity
        kt.Peri = mp.Peri                                       ' Arg of perihelion (J2000, deg.)
        kt.Node = mp.Node                                       ' Long. of asc. node (J2000, deg.)
        kt.Incl = mp.Incl                                       ' Inclination (J2000, deg.)
        mp.Close                                                ' Release the database
        Set mp = Nothing                                        ' Release all that memory now!
    Else                                                        ' Elements given split and fill in
        Name = Trim(Left(Elements, 7))                          ' Object name (return)
        If IsNumeric(Name) Then                                 ' Numbered object
            pl.Number = CLng(Name)                              ' NOVAS: Minor planet number
        Else
            pl.Number = 100000                                  ' NOVAS bug: Must set number. Ignored.
        End If
        kt.Name = Name
        kt.Epoch = PackedToJulian(Trim(Mid(Elements, 21, 5)))   ' Epoch of osculating elements
        kt.M = CDbl(Trim(Mid(Elements, 27, 9)))                 ' Mean anomaly
        kt.n = CDbl(Trim(Mid(Elements, 81, 11)))                ' Mean daily motion (deg/day)
        kt.a = CDbl(Trim(Mid(Elements, 93, 11)))                ' Semimajor axis (AU)
        kt.e = CDbl(Trim(Mid(Elements, 71, 9)))                 ' Orbital eccentricity
        kt.Peri = CDbl(Trim(Mid(Elements, 38, 9)))              ' Arg of perihelion (J2000, deg.)
        kt.Node = CDbl(Trim(Mid(Elements, 49, 9)))              ' Long. of asc. node (J2000, deg.)
        kt.Incl = CDbl(Trim(Mid(Elements, 60, 9)))              ' Inclination (J2000, deg.)
    End If
    jd = Util.SysJulianDate                                     ' Get current JD
    pl.DeltaT = Util.DeltaT(jd)                                 ' Delta T for NOVAS and Kepler
    Call GetPositionAndVelocity(pl, jd - (pl.DeltaT / 86400), _
                                RightAscension, Declination, _
                                RightAscensionRate, DeclinationRate)
    Set pl = Nothing                                            ' Releases both Ephemeris objs
    MinorPlanet = True                                          ' Success
    
End Function

'----------------------------------------------------------------------------------------
'
' -------
' Comet() - Return name and current J2000 coordinates and velocities of a comet
' -------
'
' Elements must be in "MPC 1-line comet" format as obtainable from
'       http://cfa-www.harvard.edu/iau/Ephemerides/Comets/SoftwareComets.html
' specifically
'       http://cfa-www.harvard.edu/iau/Ephemerides/Comets/Soft00Cmt.txt
'----------------------------------------------------------------------------------------
Function Comet(Elements, Name, RightAscension, Declination, RightAscensionRate, DeclinationRate)
    Dim kt, ke, pl, jd, q
    
    Set pl = CreateObject("NOVAS.Planet")
    Set kt = CreateObject("Kepler.Ephemeris")
    Set ke = CreateObject("Kepler.Ephemeris")
    pl.Ephemeris = kt                                           ' Plug in target ephemeris gen
    pl.EarthEphemeris = ke                                      ' Plug in Earth ephemeris gen
    pl.Type = 2                                                 ' NOVAS: Comet (passed to Kepler)
    Name = Trim(Mid(Elements, 1, 12))                           ' Object name 'mpc packed designation'
    pl.Number = 100000                                          ' NOVAS bug: Must set number. Ignored.
    kt.Name = Name
    kt.Epoch = Util.Date_Julian(DateSerial(CInt(Mid(Elements,13,6)), _
                    CInt(Mid(Elements,19,3)),CDbl(Mid(Elements,22,8))))
    kt.e = CDbl(Trim(Mid(Elements, 40, 10)))                    ' Orbital eccentricity
    q = CDbl(Trim(Mid(Elements, 30, 10)))                       ' Perihelion distance
    If kt.e >= 1 Then                                           ' If parabolic or hyperbolic...
      kt.a = q                                                  ' ... then mean dist = perihelion dist
    Else                                                        ' But if elliptical... 
      kt.a = q / (1 - kt.e)                                     ' ... then mean dist =  q / (1 - e)
    End If
    kt.M = 0                                                    ' For comet mean anomaly = 0
    kt.n = 0                                                    ' For conet mean daily motion = 0 
    kt.Peri = CDbl(Trim(Mid(Elements, 50, 10)))                 ' Arg of perihelion (J2000, deg.)
    kt.Node = CDbl(Trim(Mid(Elements, 60, 10)))                 ' Long. of asc. node (J2000, deg.)
    kt.Incl = CDbl(Trim(Mid(Elements, 70, 10)))                 ' Inclination (J2000, deg.)
    jd = Util.SysJulianDate                                     ' Get current JD
    pl.DeltaT = Util.DeltaT(jd)                                 ' Delta T for NOVAS & Kepler
    Call GetPositionAndVelocity(pl, jd - (pl.DeltaT / 86400), _
                                RightAscension, Declination, _
                                RightAscensionRate, DeclinationRate)
    Set pl = Nothing                                            ' Releases both Ephemeris objs
    Comet = True
                                                                ' Always successful
End Function

