Imports System.Math
Imports ASCOM.HelperNET.NOVAS2.NOVAS
Imports ASCOM.HelperNET.NOVASCOM
Module EphemerisCode

    '//
    '// Function patterned after get_earth() in the original NOVAS-C V2 package.
    '// This function returns (via ref-params) the barycentric TDT and both
    '// heliocentric and barycentric position and velocity of the Earth, at
    '// the given TJD. You can pass an IDispatch pointer for an ephemeris
    '// component, and it will be used. If that is NULL the internal solsys3()
    '// function is used (see solarsystem().
    '//
    '// For more info, see the original NOVAS-C sources.
    '//
    Friend Sub get_earth_nov(ByVal pEphDisp As KEPLER.IEphemeris, ByVal tjd As Double, _
                      ByRef tdb As Double, ByRef peb() As Double, ByRef veb() As Double, ByRef pes() As Double, _
                      ByRef ves() As Double)
        Dim i, rc As Short
        Dim dummy, secdiff As Double
        Static tjd_last As Double = 0.0
        Dim ltdb, lpeb(3), lveb(3), lpes(3), lves(3) As Double

        '//
        '// Compute the TDB Julian date corresponding to 'tjd'.
        '//

        'If (Abs(tjd - tjd_last) > 0.000001) Then 'Optimize repeated calls
        Tdb2Tdt(tjd, dummy, secdiff)
        ltdb = tjd + secdiff / 86400.0

        '//
        '// Get position and velocity of the Earth wrt barycenter of 
        '// solar system and wrt center of the sun. These calls reflect
        '// exceptions thrown by the attached ephemeris generator, so
        '// we just return the hr ... the ErrorInfo is already set!
        '//
        Try
            rc = solarsystem_nov(pEphDisp, tjd, ltdb, PlanetNumber.nvEarth, OriginType.nvBarycentric, lpeb, lveb)
            If rc <> 0 Then Throw New Exceptions.NOVASFunctionException("EphemerisCode:get_earth_nov Earth eph exception", "solarsystem_nov", rc)
        Catch ex As Exception
            tjd_last = 0.0
            Throw
        End Try

        Try
            rc = solarsystem_nov(pEphDisp, tjd, ltdb, PlanetNumber.nvEarth, OriginType.nvHeliocentric, lpes, lves)
            If rc <> 0 Then Throw New Exceptions.NOVASFunctionException("EphemerisCode:get_earth_nov Earth eph exception", "solarsystem_nov", rc)
        Catch ex As Exception
            tjd_last = 0.0
            Throw
        End Try

        tjd_last = tjd
        'End If
        tdb = ltdb
        For i = 0 To 2
            peb(i) = lpeb(i)
            veb(i) = lveb(i)
            pes(i) = lpes(i)
            ves(i) = lves(i)
        Next


    End Sub

    '//
    '// Ephemeris() - Wrapper for external ephemeris generator
    '//
    '// The ephemeris generator must support a single method:
    '//
    '//     result(6) = GetPositionAndVelocity(tjd, Type, Number, Name)
    '//
    '//	tjd		Terrestrial Julian Date
    '//	Type	Type of body: 0 = major planet, Sun, or Moon
    '//						  1 = minor planet
    '//	Number: For Type = 0: Mercury = 1, ..., Pluto = 9
    '//			For Type = 1: minor planet number or 0 for unnumbered MP
    '//  Name:   For Type = 0: n/a
    '//			For Type = 1: n/a for numbered MPs. For unnumbered MPs, this
    '//						  is the MPC PACKED designation.
    '//  result	A SAFEARRAY of VARIANT, each element VT_R8 (double). Elements
    '//			0-2 are the position vector of the body, elements 3.5 are the
    '//			velocity vector of the body. 
    '//
    Friend Sub ephemeris_nov(ByVal ephDisp As KEPLER.IEphemeris, ByVal tjd As Double, ByVal btype As BodyType, _
                             ByVal num As Integer, ByVal name As String, ByVal origin As OriginType, _
                             ByRef pos() As Double, ByRef vel() As Double)
        Dim i As Integer
        Dim posvel(6), p(2), v(2) As Double
        'Dim bdy As bodystruct
        'Dim org As NOVAS2Net.Origin
        'Dim rc As Short
        Dim TL As New TraceLogger("", "EphNov")
        TL.Enabled = True

        '//
        '// Check inputs
        '//
        If (ephDisp Is Nothing) Then
            Throw New Exceptions.ValueNotSetException("Ephemeris_nov Ephemeris object not set")
            'bdy.name = name
            'bdy.number = Body.Earth 'CType(num, NOVAS2Net.Body)
            'bdy.type = NOVAS2Net.BodyType.MajorPlanet 'CType(btype, NOVAS2Net.BodyType)
            'Select Case origin
            '    Case OriginType.nvBarycentric
            'org = NOVAS2Net.Origin.SolarSystemBarycentre
            '    Case OriginType.nvHeliocentric
            'org = NOVAS2Net.Origin.CentreOfMassOfSun
            'End Select
            'rc = solsys3_nov(tjd, CType(num, PlanetNumber), origin, p, v)
            'If rc <> 0 Then Throw New Exceptions.NOVASFunctionException("Ephmeris_nov Exception in ephemeris", "ephemeris", rc)
            'TL.LogMessage("Ephemeris", "Body: " & bdy.name & " " & CInt(bdy.type).ToString & " " & bdy.number & " " & CInt(org).ToString & " " & p(0).ToString)
            'TL.LogMessage("EphBody", tjd.ToString & " " & bdy.ToString & " " & org.ToString)
            'TL.LogMessage("Solsys_nov P and V 0", p(0) & " " & v(0))
            'TL.LogMessage("Solsys_nov P and V 1", p(1) & " " & v(1))
            'TL.LogMessage("Solsys_nov P and V 2", p(1) & " " & v(2))

            'solarsystem(tjd, bdy.number, org, p, v)
            'TL.LogMessage("solarsystem P and V 0", p(0) & " " & v(0))
            'TL.LogMessage("solarsystem P and V 1", p(1) & " " & v(1))
            'TL.LogMessage("solarsystem P and V 2", p(1) & " " & v(2))
            'rc = ephemeris(tjd, bdy, org, p, v)
            'TL.LogMessage("ephemeris P and V 0", p(0) & " " & v(0))
            'TL.LogMessage("ephemeris P and V 1", p(1) & " " & v(1))
            'TL.LogMessage("ephemeris P and V 2", p(1) & " " & v(2))

            'For i = 0 To 2
            ' posvel(i) = p(i)
            ' posvel(i + 3) = v(i)
            ' Next
            'Dim K As New ASCOM.Kepler.Ephemeris
            'K.Name = name
            'K.Number = num
            'K.BodyType = ASCOM.Kepler.BodyType.kepMajorPlanet
            'Dim pv As System.Array
            '
            'pv = CType(K.GetPositionAndVelocity(tjd), System.Array)

            'For i = 0 To 5
            'posvel(i) = CDbl(PV(i))
            'Next
            'TL.LogMessage("Kepler P and V 0", posvel(0) & " " & posvel(3))
            'TL.LogMessage("Kepler P and V 1", posvel(1) & " " & posvel(4))
            'TL.LogMessage("Kepler P and V 2", posvel(2) & " " & posvel(5))
        Else
            If ((origin <> OriginType.nvBarycentric) And (origin <> OriginType.nvHeliocentric)) Then Throw New Exceptions.InvalidValueException("Ephemeris_nov Origin is neither barycentric or heliocentric")

            '//
            '// Call the ephemeris for the heliocentric J2000.0 equatorial coordinates
            Dim kbtype As ASCOM.HelperNET.KEPLER.BodyType
            Select Case btype
                Case BodyType.nvComet
                    kbtype = KEPLER.BodyType.kepComet
                Case BodyType.nvMajorPlanet
                    kbtype = KEPLER.BodyType.kepMajorPlanet
                Case BodyType.nvMinorPlanet
                    kbtype = KEPLER.BodyType.kepMinorPlanet
            End Select

            Dim knum As ASCOM.HelperNET.KEPLER.PlanetNumber
            Select Case num
                Case 1
                    knum = KEPLER.PlanetNumber.kepMercury
                Case 2
                    knum = KEPLER.PlanetNumber.kepVenus
                Case 3
                    knum = KEPLER.PlanetNumber.kepEarth
                Case 4
                    knum = KEPLER.PlanetNumber.kepMars
                Case 5
                    knum = KEPLER.PlanetNumber.kepjupiter
                Case 6
                    knum = KEPLER.PlanetNumber.kepSaturn
                Case 7
                    knum = KEPLER.PlanetNumber.kepUranus
                Case 8
                    knum = KEPLER.PlanetNumber.kepNeptune
                Case 9
                    knum = KEPLER.PlanetNumber.kepPluto
            End Select
            ephDisp.BodyType = kbtype
            ephDisp.Number = knum
            If (name <> "") Then ephDisp.Name = name
            posvel = ephDisp.GetPositionAndVelocity(tjd)
        End If

        If (origin = OriginType.nvBarycentric) Then

            Dim sun_pos(3), sun_vel(3) As Double

            '// CHICKEN AND EGG ALERT!!! WE CANNOT CALL OURSELVES FOR 
            '// BARYCENTER CALCULATION -- AS AN APPROXIMATION, WE USE
            '// OUR INTERNAL SOLSYS3() FUNCTION TO GET THE BARYCENTRIC
            '// SUN. THIS SHOULD BE "GOOD ENOUGH". IF WE EVER GET 
            '// AN EPHEMERIS GEN THAT HANDLES BARYCENTRIC, WE CAN 
            '// CAN THIS...
            solsys3_nov(tjd, PlanetNumber.nvSun, OriginType.nvBarycentric, sun_pos, sun_vel)

            For i = 0 To 2
                posvel(i) += sun_pos(i)
                posvel(i + 3) += sun_vel(i)
            Next
        End If

        For i = 0 To 2
            pos(i) = posvel(i)
            vel(i) = posvel(i + 3)
        Next
        TL.Dispose()
    End Sub
    '// ===============
    '// LOCAL FUNCTIONS
    '// ===============


    '//
    '// This is the function used to get the position and velocity vectors
    '// for the major solar system bodies and the moon. It is patterned after
    '// the solarsystem() function in the original NOVAS-C package. You can
    '// pass an IDispatch pointer for an ephemeris component, and it will be 
    '// used. If that is NULL the internal solsys3() function is used.
    '//
    '// This function must set error info... it is designed to work with 
    '// reflected exceptions from the attached ephemeris
    '// 
    Friend Function solarsystem_nov(ByVal ephDisp As KEPLER.IEphemeris, ByVal tjd As Double, _
          ByVal tdb As Double, ByVal planet As PlanetNumber, ByVal origin As OriginType, _
          ByRef pos() As Double, ByRef vel() As Double) As Short
        Dim pl As NOVAS2.Body, org As NOVAS2.Origin

        Dim rc As Short
        '//
        '// solsys3 takes tdb, ephemeris takes tjd
        '//
        Select Case origin
            Case OriginType.nvBarycentric
                org = NOVAS2.Origin.SolarSystemBarycentre
            Case OriginType.nvHeliocentric
                org = NOVAS2.Origin.CentreOfMassOfSun
        End Select
        Select Case planet
            Case PlanetNumber.nvEarth
                pl = NOVAS2.Body.Earth
            Case PlanetNumber.nvJupiter
                pl = NOVAS2.Body.Jupiter
            Case PlanetNumber.nvMars
                pl = NOVAS2.Body.Mars
            Case PlanetNumber.nvMercury
                pl = NOVAS2.Body.Mercury
            Case PlanetNumber.nvMoon
                pl = NOVAS2.Body.Moon
            Case PlanetNumber.nvNeptune
                pl = NOVAS2.Body.Neptune
            Case PlanetNumber.nvPluto
                pl = NOVAS2.Body.Pluto
            Case PlanetNumber.nvSaturn
                pl = NOVAS2.Body.Saturn
            Case PlanetNumber.nvSun
                pl = NOVAS2.Body.Sun
            Case PlanetNumber.nvUranus
                pl = NOVAS2.Body.Uranus
            Case PlanetNumber.nvVenus
                pl = NOVAS2.Body.Venus
        End Select
        If (ephDisp Is Nothing) Then 'No ephemeris attached
            'rc = solsys3_nov(tdb, planet, origin, pos, vel)
            Throw New Exceptions.ValueNotSetException("EphemerisCode:SolarSystem_Nov No emphemeris object supplied")
        Else
            'CHECK TDB BELOW IS CORRECT!
            ephemeris_nov(ephDisp, tdb, BodyType.nvMajorPlanet, planet, "", origin, pos, vel)
        End If
        Return rc
    End Function

    '//
    '// solsys3() - Internal function that gives reasonable ephemerides for 
    '// Sun or Earth, barycentric or heliocentric.
    '//
    Private Function solsys3_nov(ByVal tjd As Double, ByVal body As PlanetNumber, ByVal origin As OriginType, ByRef pos() As Double, ByRef vel() As Double) As Short

        Dim i As Integer

        '/*
        'The arrays below contain data for the four largest planets.  Masses
        'are DE405 values; elements are from Explanatory Supplement, p. 316). 
        'These data are used for barycenter computations only.
        '*/

        Dim pm() As Double = {1047.349, 3497.898, 22903.0, 19412.2}
        Dim pa() As Double = {5.203363, 9.53707, 19.191264, 30.068963}
        Dim pl() As Double = {0.60047, 0.871693, 5.466933, 5.32116}
        Dim pn() As Double = {0.001450138, 0.0005841727, 0.0002047497, 0.0001043891}

        '/*
        'obl' is the obliquity of ecliptic at epoch J2000.0 in degrees.
        '*/

        Const obl As Double = 23.43929111

        Static tlast As Double = 0.0
        Static sine, cose, tmass, pbary(3), vbary(3) As Double

        Dim oblr, qjd, ras, decs, diss, pos1(3), p(3, 3), dlon, sinl, _
            cosl, x, y, z, xdot, ydot, zdot, f As Double

        '//
        '// Check inputs
        '//
        If ((origin <> OriginType.nvBarycentric) And (origin <> OriginType.nvHeliocentric)) Then _
            Throw New Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid origin: " & origin)

        If ((tjd < 2340000.5) Or (tjd > 2560000.5)) Then _
                               Throw New Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid tjd: " & tjd)


        '/*
        'Initialize constants.
        '*/

        If (tlast = 0.0) Then
            oblr = obl * TWOPI / 360.0
            sine = Sin(oblr)
            cose = Cos(oblr)
            tmass = 1.0
            For i = 0 To 3
                tmass += (1.0 / pm(i))
            Next
            tlast = 1.0
        End If
        '/*
        'Form helicentric coordinates of the Sun or Earth, depending on
        'body'.
        '*/

        If ((body = 0) Or (body = 1) Or (body = 10)) Then
            For i = 0 To 2
                pos(i) = 0.0
                vel(i) = 0.0
            Next
        ElseIf ((body = 2) Or (body = 3)) Then
            For i = 0 To 2
                qjd = tjd + (CDbl(i) - 1.0) * 0.1
                sun_eph_nov(qjd, ras, decs, diss)
                RADec2Vector(ras, decs, diss, pos1)
                Precession(qjd, pos1, T0, pos)
                p(i, 0) = -pos(0)
                p(i, 1) = -pos(1)
                p(i, 2) = -pos(2)
            Next
            For i = 0 To 2
                pos(i) = p(1, i)
                vel(i) = (p(2, i) - p(0, i)) / 0.2
            Next
        Else
            Throw New Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid body: " & body)
        End If

        '/*
        'If 'origin' = 0, move origin to solar system barycenter.
        '
        'Solar system barycenter coordinates are computed from rough
        'approximations of the coordinates of the four largest planets.
        '*/

        If (origin = OriginType.nvBarycentric) Then
            If tjd <> tlast Then
                For i = 0 To 2
                    pbary(i) = 0.0
                    vbary(i) = 0.0
                Next

                '/*
                'The following loop cycles once for each of the four planets.
                '
                'sinl' and 'cosl' are the sine and cosine of the planet's mean
                'longitude.
                '*/

                For i = 0 To 3
                    dlon = pl(i) + pn(i) * (tjd - T0)
                    dlon = dlon Mod TWOPI
                    sinl = Sin(dlon)
                    cosl = Cos(dlon)

                    x = pa(i) * cosl
                    y = pa(i) * sinl * cose
                    z = pa(i) * sinl * sine
                    xdot = -pa(i) * pn(i) * sinl
                    ydot = pa(i) * pn(i) * cosl * cose
                    zdot = pa(i) * pn(i) * cosl * sine

                    f = 1.0 / (pm(i) * tmass)

                    pbary(0) += x * f
                    pbary(1) += y * f
                    pbary(2) += z * f
                    vbary(0) += xdot * f
                    vbary(1) += ydot * f
                    vbary(2) += zdot * f
                Next

                tlast = tjd
            End If

            For i = 0 To 2
                pos(i) -= pbary(i)
                vel(i) -= vbary(i)
            Next
        End If
        Return 0
    End Function

    Friend Structure sun_con
        Friend l As Double
        Friend r As Double
        Friend alpha As Double
        Friend nu As Double
        Friend Sub New(ByVal pl As Double, ByVal pr As Double, ByVal palpha As Double, ByVal pnu As Double)
            l = pl
            r = pr
            alpha = palpha
            nu = pnu
        End Sub
    End Structure


    Private Sub sun_eph_nov(ByVal jd As Double, ByVal ra As Double, ByVal dec As Double, ByVal dis As Double)
        Dim i As Integer

        Dim sum_lon As Double = 0.0
        Dim sum_r As Double = 0.0
        Const factor As Double = 0.0000001
        Dim u, arg, lon, lat, t, t2, emean, sin_lon As Double

        Dim con() As sun_con = { _
          New sun_con(403406.0, 0.0, 4.721964, 1.621043), _
          New sun_con(195207.0, -97597.0, 5.937458, 62830.348067), _
          New sun_con(119433.0, -59715.0, 1.115589, 62830.821524), _
          New sun_con(112392.0, -56188.0, 5.781616, 62829.634302), _
          New sun_con(3891.0, -1556.0, 5.5474, 125660.5691), _
          New sun_con(2819.0, -1126.0, 1.512, 125660.9845), _
          New sun_con(1721.0, -861.0, 4.1897, 62832.4766), _
          New sun_con(0.0, 941.0, 1.163, 0.813), _
          New sun_con(660.0, -264.0, 5.415, 125659.31), _
          New sun_con(350.0, -163.0, 4.315, 57533.85), _
          New sun_con(334.0, 0.0, 4.553, -33.931), _
          New sun_con(314.0, 309.0, 5.198, 777137.715), _
          New sun_con(268.0, -158.0, 5.989, 78604.191), _
          New sun_con(242.0, 0.0, 2.911, 5.412), _
          New sun_con(234.0, -54.0, 1.423, 39302.098), _
          New sun_con(158.0, 0.0, 0.061, -34.861), _
          New sun_con(132.0, -93.0, 2.317, 115067.698), _
          New sun_con(129.0, -20.0, 3.193, 15774.337), _
          New sun_con(114.0, 0.0, 2.828, 5296.67), _
          New sun_con(99.0, -47.0, 0.52, 58849.27), _
          New sun_con(93.0, 0.0, 4.65, 5296.11), _
          New sun_con(86.0, 0.0, 4.35, -3980.7), _
          New sun_con(78.0, -33.0, 2.75, 52237.69), _
          New sun_con(72.0, -32.0, 4.5, 55076.47), _
          New sun_con(68.0, 0.0, 3.23, 261.08), _
          New sun_con(64.0, -10.0, 1.22, 15773.85), _
          New sun_con(46.0, -16.0, 0.14, 188491.03), _
          New sun_con(38.0, 0.0, 3.44, -7756.55), _
          New sun_con(37.0, 0.0, 4.37, 264.89), _
          New sun_con(32.0, -24.0, 1.14, 117906.27), _
          New sun_con(29.0, -13.0, 2.84, 55075.75), _
          New sun_con(28.0, 0.0, 5.96, -7961.39), _
          New sun_con(27.0, -9.0, 5.09, 188489.81), _
          New sun_con(27.0, 0.0, 1.72, 2132.19), _
          New sun_con(25.0, -17.0, 2.56, 109771.03), _
          New sun_con(24.0, -11.0, 1.92, 54868.56), _
          New sun_con(21.0, 0.0, 0.09, 25443.93), _
          New sun_con(21.0, 31.0, 5.98, -55731.43), _
          New sun_con(20.0, -10.0, 4.03, 60697.74), _
          New sun_con(18.0, 0.0, 4.27, 2132.79), _
          New sun_con(17.0, -12.0, 0.79, 109771.63), _
          New sun_con(14.0, 0.0, 4.24, -7752.82), _
          New sun_con(13.0, -5.0, 2.01, 188491.91), _
          New sun_con(13.0, 0.0, 2.65, 207.81), _
          New sun_con(13.0, 0.0, 4.98, 29424.63), _
          New sun_con(12.0, 0.0, 0.93, -7.99), _
          New sun_con(10.0, 0.0, 2.21, 46941.14), _
          New sun_con(10.0, 0.0, 3.59, -68.29), _
          New sun_con(10.0, 0.0, 1.5, 21463.25), _
          New sun_con(10.0, -9.0, 2.55, 157208.4) _
        }

        '/*
        'Define the time unit 'u', measured in units of 10000 Julian years
        'from J2000.0.
        '*/

        u = (jd - T0) / 3652500.0

        '/*
        'Compute longitude and distance terms from the series.
        '*/

        For i = 0 To 49

            arg = con(i).alpha + con(i).nu * u
            sum_lon += con(i).l * Sin(arg)
            sum_r += con(i).r * Cos(arg)
        Next

        '/*
        'Compute longitude, latitude, and distance referred to mean equinox
        'and ecliptic of date.
        '*/

        lon = 4.9353929 + 62833.196168 * u + factor * sum_lon

        lon = lon Mod TWOPI
        If (lon < 0.0) Then lon += TWOPI

        lat = 0.0

        dis = 1.0001026 + factor * sum_r

        '/*
        'Compute mean obliquity of the ecliptic.
        '*/

        t = u * 100.0
        t2 = t * t
        emean = (0.001813 * t2 * t - 0.00059 * t2 - 46.815 * t + 84381.448) / RAD2SEC

        '/*
        'Compute equatorial spherical coordinates referred to the mean equator 
        'and equinox of date.
        '*/

        sin_lon = Sin(lon)
        ra = Atan2((Cos(emean) * sin_lon), Cos(lon)) * RAD2DEG
        ra = ra Mod 360.0
        If (ra < 0.0) Then ra += 360.0
        ra = ra / 15.0

        dec = Asin(Sin(emean) * sin_lon) * RAD2DEG

    End Sub


End Module