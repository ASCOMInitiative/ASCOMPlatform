Imports System.Math
Namespace KEPLER
    Module KeplerGlobalCode

#Region "Constants"
        Friend Const NARGS As Integer = 18

        '/* Conversion factors between degrees and radians */
        Private Const DTR As Double = 0.017453292519943295
        Private Const RTD As Double = 57.295779513082323
        Private Const RTS As Double = 206264.80624709636 '/* arc seconds per radian */
        Private Const STR As Double = 0.00000484813681109536 '/* radians per arc second */
        Private Const PI As Double = 3.1415926535897931
        Private Const TPI As Double = 2.0 * PI

        '/* Standard epochs.  Note Julian epochs (J) are measured in
        ' * years of 365.25 days.
        ' */
        Private Const J2000 As Double = 2451545.0 '/* 2000 January 1.5 */
        Private Const B1950 As Double = 2433282.423 '/* 1950 January 0.923 Besselian epoch */
        Private Const J1900 As Double = 2415020.0 '/* 1900 January 0, 12h UT */

        '/* Constants used elsewhere. These are DE403 values. */
        Private Const aearth As Double = 6378137.0 '/* Radius of the earth, in meters.  */
        Private Const au As Double = 149597870.691 '/* Astronomical unit, in kilometers.  */
        Private Const emrat As Double = 81.300585 '/* Earth/Moon mass ratio.  */
        Private Const Clight As Double = 299792.458 '/* Speed of light, km/sec  */
        Private Const Clightaud As Double = Nothing '/* C in au/day  */
#End Region

#Region "Utility Routines"
        '// ----------------
        '// Utility routines
        '// ----------------

        '// Obliquity of the ecliptic at Julian date J  
        '// according to the DE403 values. Refer to 
        '// S. Moshier's aa54e sources.

        Friend Sub epsiln(ByVal J As Double, ByRef eps As Double, ByRef coseps As Double, ByRef sineps As Double)
            Dim T As Double

            T = (J - 2451545.0) / 365250.0 '// T / 10
            eps = ((((((((((0.000000000245 * T + 0.00000000579) * T + 0.0000002787) * T _
            + 0.000000712) * T - 0.00003905) * T - 0.0024967) * T _
            - 0.005138) * T + 1.9989) * T - 0.0175) * T - 468.3396) * T _
            + 84381.406173) * STR
            coseps = Cos(eps)
            sineps = Sin(eps)
        End Sub

        '/* Precession of the equinox and ecliptic
        ' * from epoch Julian date J to or from J2000.0
        ' *
        ' * Program by Steve Moshier.  */
        '
        '/* James G. Williams, "Contributions to the Earth's obliquity rate,
        '   precession, and nutation,"  Astron. J. 108, 711-724 (1994)  */

        ' /* Corrections to Williams (1994) introduced in DE403.  */
        Friend pAcof() As Double = {-0.000000000866, -0.00000004759, 0.0000002424, 0.000013095, _
                                     0.00017451, -0.0018055, -0.235316, 0.076, 110.5414, 50287.91959}

        Friend nodecof() As Double = {0.00000000000000066402, -0.00000000000000269151, -0.000000000001547021, _
                                       0.000000000007521313, 0.00000000019, -0.00000000354, -0.00000018103, _
                                       0.000000126, 0.00007436169, -0.04207794833, 3.052115282424}

        Friend inclcof() As Double = {0.00000000000000012147, 7.3759E-17, -0.0000000000000826287, _
                                       0.000000000000250341, 0.000000000024650839, -0.000000000054000441, _
                                       0.00000000132115526, -0.0000006012, -0.0000162442, 0.00227850649, 0.0}

        '/* Subroutine arguments:
        ' *
        ' * R = rectangular equatorial coordinate vector to be precessed.
        ' *     The result is written back into the input vector.
        ' * J = Julian date
        ' * direction =
        ' *      Precess from J to J2000: direction = 1
        ' *      Precess from J2000 to J: direction = -1
        ' * Note that if you want to precess from J1 to J2, you would
        ' * first go from J1 to J2000, then call the program again
        ' * to go from J2000 to J2.
        ' */

        Friend Sub precess(ByRef R() As Double, ByVal J As Double, ByVal direction As Integer)
            Dim A, B, T, pA, W, z As Double
            Dim x(3) As Double
            Dim p() As Double
            Dim eps, coseps, sineps As Double
            Dim i As Integer

            If (J = J2000) Then Return
            '/* Each precession angle is specified by a polynomial in
            ' * T = Julian centuries from J2000.0.  See AA page B18.
            ' */
            T = (J - J2000) / 36525.0

            '/* Implementation by elementary rotations using Laskar's expansions.
            ' * First rotate about the x axis from the initial equator
            ' * to the ecliptic. (The input is equatorial.)
            ' */
            If (direction = 1) Then
                epsiln(J, eps, coseps, sineps) '/* To J2000 */
            Else
                epsiln(J2000, eps, coseps, sineps) '/* From J2000 */
            End If
            x(0) = R(0)
            z = coseps * R(1) + sineps * R(2)
            x(2) = -sineps * R(1) + coseps * R(2)
            x(1) = z

            '/* Precession in longitude	 */
            T /= 10.0 '/* thousands of years */
            p = pAcof
            pA = p(0)
            For i = 1 To 9
                pA = pA * T + p(i)
            Next
            pA *= STR * T

            '/* Node of the moving ecliptic on the J2000 ecliptic.*/
            p = nodecof
            W = p(0)
            For i = 1 To 10
                W = W * T + p(i)
            Next
            '/* Rotate about z axis to the node.*/
            If (direction = 1) Then
                z = W + pA
            Else
                z = W
            End If
            B = Cos(z)
            A = Sin(z)
            z = B * x(0) + A * x(1)
            x(1) = -A * x(0) + B * x(1)
            x(0) = z

            '/* Rotate about new x axis by the inclination of the moving
            ' * ecliptic on the J2000 ecliptic.
            ' */
            p = inclcof
            z = p(0)
            For i = 1 To 10
                z = z * T + p(i)
            Next
            If (direction = 1) Then z = -z
            B = Cos(z)
            A = Sin(z)
            z = B * x(1) + A * x(2)
            x(2) = -A * x(1) + B * x(2)
            x(1) = z

            '/* Rotate about new z axis back from the node.	 */
            If (direction = 1) Then
                z = -W
            Else
                z = -W - pA
            End If
            B = Cos(z)
            A = Sin(z)
            z = B * x(0) + A * x(1)
            x(1) = -A * x(0) + B * x(1)
            x(0) = z

            '/* Rotate about x axis to final equator.	 */
            If (direction = 1) Then
                epsiln(J2000, eps, coseps, sineps)
            Else
                epsiln(J, eps, coseps, sineps)
            End If
            z = coseps * x(1) - sineps * x(2)
            x(2) = sineps * x(1) + coseps * x(2)
            x(1) = z

            For i = 0 To 2
                R(i) = x(i)
            Next
        End Sub

        Friend Function atan4(ByVal x As Double, ByVal y As Double) As Double

            Dim z, w As Double
            Dim code As Integer

            code = 0

            If (x < 0.0) Then code = 2
            If (y < 0.0) Then code = code Or 1

            If (x = 0.0) Then
                If (code And 1) > 0 Then Return (1.5 * PI)
                If (y = 0.0) Then Return (0.0)
                Return (0.5 * PI)
            End If

            If (y = 0.0) Then
                If (code And 2) > 0 Then Return (PI)
                Return (0.0)
            End If

            Select Case code
                Case 0
                    w = 0.0
                Case 1
                    w = 2.0 * PI
                Case 2
                Case 3
                    w = PI
                Case Else
            End Select

            z = Atan(y / x)

            Return (w + z)
        End Function

        '//
        '// Reduce x modulo 2 pi
        '//
        Friend Function modtp(ByVal x As Double) As Double

            Dim y As Double

            y = Floor(x / TPI)
            y = x - y * TPI
            While (y < 0.0)
                y += TPI
            End While
            While (y >= TPI)
                y -= TPI
            End While
            Return (y)
        End Function

        '//
        '//  Reduce x modulo 360 degrees
        '//
        Friend Function mod360(ByVal x As Double) As Double

            Dim k As Integer
            Dim y As Double

            k = CInt(x / 360.0)
            y = x - k * 360.0
            While (y < 0.0)
                y += 360.0
            End While
            While (y > 360.0)
                y -= 360.0
            End While
            Return (y)
        End Function


        '/* Program to solve Keplerian orbit
        ' * given orbital parameters and the time.
        ' * Returns Heliocentric equatorial rectangular coordinates of
        ' * the object.
        ' *
        ' * This program detects several cases of given orbital elements.
        ' *
        ' * If a program for perturbations is pointed to, it is called
        ' * to calculate all the elements.
        ' *
        ' * If there is no program, then the mean longitude is calculated
        ' * from the mean anomaly and daily motion.
        ' *
        ' * If the daily motion is not given, it is calculated
        ' * by Kepler's law.
        ' *
        ' * If the eccentricity is given to be 1.0, it means that
        ' * meandistance is really the perihelion distance, as in a comet
        ' * specification, and the orbit is parabolic.
        ' *
        ' * Reference: Taff, L.G., "Celestial Mechanics, A Computational
        ' * Guide for the Practitioner."  Wiley, 1985.
        ' */


        Friend Sub KeplerCalc(ByVal J As Double, ByRef e As orbit, ByRef rect() As Double)

            Dim polar(3) As Double
            Dim alat, E1, M, W, temp As Double
            Dim epoch, inclination, ascnode, argperih As Double
            Dim meandistance, dailymotion, eccent, meananomaly As Double
            Dim r, coso, sino, cosa As Double
            Dim eps, coseps, sineps As Double

            '//
            '// Call program to compute position, if one is supplied.
            '//
            If (e.ptable.lon_tbl(0) <> 0.0) Then
                If (e.obname = "Earth") Then
                    g3plan(J, e.ptable, polar, 3)
                Else
                    gplan(J, e.ptable, polar)
                End If
                E1 = polar(0) '/* longitude */
                e.L = E1
                W = polar(1) '/* latitude */
                r = polar(2) '/* radius */
                e.r = r
                e.epoch = J
                e.equinox = J2000
                GoTo kepdon
            End If

            '// -----------------------------
            '// Compute from orbital elements 
            '// -----------------------------

            e.equinox = J2000 '// Always J2000 coordinates
            epoch = e.epoch
            inclination = e.i
            ascnode = e.W * DTR
            argperih = e.W
            meandistance = e.a '/* semimajor axis */
            dailymotion = e.dm
            eccent = e.ecc
            meananomaly = e.M

            '// ---------
            '// Parabolic
            '// ---------
            If (eccent = 1.0) Then
                '//
                '// meandistance = perihelion distance, q
                '// epoch = perihelion passage date
                '//
                temp = meandistance * Sqrt(meandistance)
                W = (J - epoch) * 0.0364911624 / temp
                '//
                '// The constant above is 3 k / sqrt(2),
                '// k = Gaussian gravitational constant = 0.01720209895
                '//
                E1 = 0.0
                M = 1.0
                While (Abs(M) > 0.00000000001)
                    temp = E1 * E1
                    temp = (2.0 * E1 * temp + W) / (3.0 * (1.0 + temp))
                    M = temp - E1
                    If (temp <> 0.0) Then M /= temp
                    E1 = temp
                End While
                r = meandistance * (1.0 + E1 * E1)
                M = Atan(E1)
                M = 2.0 * M
                alat = M + (DTR * argperih)
                '// ----------
                '// Hyperbolic
                '// ----------
            ElseIf (eccent > 1.0) Then

                '//
                '// The equation of the hyperbola in polar coordinates r, theta
                '// is r = a(e^2 - 1)/(1 + e cos(theta)) so the perihelion 
                '// distance q = a(e-1), the "mean distance"  a = q/(e-1).
                '//
                meandistance = meandistance / (eccent - 1.0)
                temp = meandistance * Sqrt(meandistance)
                W = (J - epoch) * 0.01720209895 / temp
                '/* solve M = -E + e sinh E */
                E1 = W / (eccent - 1.0)
                M = 1.0
                While (Abs(M) > 0.00000000001)

                    M = -E1 + eccent * Sinh(E1) - W
                    E1 += M / (1.0 - eccent * Cosh(E1))
                End While
                r = meandistance * (-1.0 + eccent * Cosh(E1))
                temp = (eccent + 1.0) / (eccent - 1.0)
                M = Sqrt(temp) * Tanh(0.5 * E1)
                M = 2.0 * Atan(M)
                alat = M + (DTR * argperih)

                '// -----------
                '// Ellipsoidal
                '// -----------
            Else '		// if(ecc < 1)
                '//
                '// Calculate the daily motion, if it is not given.
                '//
                If (dailymotion = 0.0) Then

                    '//
                    '// The constant is 180 k / pi, k = Gaussian gravitational 
                    '// constant. Assumes object in heliocentric orbit is 
                    '// massless.
                    '//
                    dailymotion = 0.9856076686 / (e.a * Sqrt(e.a))
                End If
                dailymotion *= J - epoch
                '//
                '// M is proportional to the area swept out by the radius
                '// vector of a circular orbit during the time between
                '// perihelion passage and Julian date J.
                '// It is the mean anomaly at time J.
                '//
                M = DTR * (meananomaly + dailymotion)
                M = modtp(M)
                '//
                '// If mean longitude was calculated, adjust it also
                '// for motion since epoch of elements.
                '//
                If (e.L) <> 0.0 Then
                    e.L += dailymotion
                    e.L = mod360(e.L)
                End If
                '//
                '// By Kepler's second law, M must be equal to
                '// the area swept out in the same time by an
                '// elliptical orbit of same total area.
                '// Integrate the ellipse expressed in polar coordinates
                '//     r = a(1-e^2)/(1 + e cosW)
                '// with respect to the angle W to get an expression for the
                '// area swept out by the radius vector.  The area is given
                '// by the mean anomaly; the angle is solved numerically.
                '// 
                '// The answer is obtained in two steps.  We first solve
                '// Kepler's equation
                '//    M = E - eccent*sin(E)
                '// for the eccentric anomaly E.  Then there is a
                '// closed form solution for W in terms of E.
                '//
                E1 = M '/* Initial guess is same as circular orbit. */
                temp = 1.0
                Do
                    '// The approximate area swept out in the ellipse
                    '// ...minus the area swept out in the circle
                    temp = E1 - eccent * Sin(E1) - M
                    '// ...should be zero.  Use the derivative of the error
                    '//to converge to solution by Newton's method.
                    E1 -= temp / (1.0 - (eccent * Cos(E1)))
                Loop While (Abs(temp) > 0.00000000001)

                '//
                '// The exact formula for the area in the ellipse is
                '//    2.0*atan(c2*tan(0.5*W)) - c1*eccent*sin(W)/(1+e*cos(W))
                '// where
                '//    c1 = sqrt( 1.0 - eccent*eccent )
                '//    c2 = sqrt( (1.0-eccent)/(1.0+eccent) ).
                '// Substituting the following value of W
                '// yields the exact solution.
                '//
                temp = Sqrt((1.0 + eccent) / (1.0 - eccent))
                W = 2.0 * Atan(temp * Tan(0.5 * E1))

                '//
                '// The true anomaly.
                '//
                W = modtp(W)

                meananomaly *= DTR
                '//
                '// Orbital longitude measured from node
                '// (argument of latitude)
                '//
                If (e.L <> 0.0) Then '// Mean longitude given
                    alat = ((e.L) * DTR) + W - meananomaly - ascnode
                Else
                    alat = W + (DTR * argperih) '// Mean longitude not given
                End If
                '//
                '// From the equation of the ellipse, get the
                '// radius from central focus to the object.
                '//
                r = meandistance * (1.0 - eccent * eccent) / (1.0 + eccent * Cos(W))
            End If

            inclination *= DTR '// Convert inclination to radians

            '// ----------
            '// ALL ORBITS
            '// ----------
            '//
            '// At this point:
            '//
            '//		alat		= argument of latitude (rad)
            '//		inclination	= inclination (rad)
            '//		r			= radius from central focus
            '//
            '// The heliocentric ecliptic longitude of the objectis given by:
            '//
            '//   tan(longitude - ascnode)  =  cos(inclination) * tan(alat)
            '//
            coso = Cos(alat)
            sino = Sin(alat)
            W = sino * Cos(inclination)
            E1 = atan4(coso, W) + ascnode

            '//
            '// The ecliptic latitude of the object
            '//
            W = Asin(sino * Sin(inclination))

            '// ------------------------------------
            '// Both from DE404 and from elements...
            '// ------------------------------------
            '//
            '// At this point we have the heliocentric ecliptic polar
            '// coordinates of the body.
            '//
kepdon:

            '//
            '// Convert to heliocentric ecliptic rectangular coordinates, 
            '// using the perturbed latitude.
            '//
            rect(2) = r * Sin(W)
            cosa = Cos(W)
            rect(1) = r * cosa * Sin(E1)
            rect(0) = r * cosa * Cos(E1)

            '//
            '// Convert from heliocentric ecliptic rectangular
            '// to heliocentric equatorial rectangular coordinates
            '// by rotating epsilon radians about the x axis.
            '//
            epsiln(e.equinox, eps, coseps, sineps)
            W = coseps * rect(1) - sineps * rect(2)
            M = sineps * rect(1) + coseps * rect(2)
            rect(1) = W
            rect(2) = M

            '//
            '// Precess the equatorial (rectangular) coordinates to the
            '// ecliptic & equinox of J2000.0, if not already there.
            '//
            precess(rect, e.equinox, 1)

            '//
            '// If earth, adjust from earth-moon barycenter to earth
            '// by AA page E2.
            '//
            If e.obname = "Earth" Then embofs(J, rect, r) '/* see embofs() below */

        End Sub

        '//
        '// Adjust position from Earth-Moon barycenter to Earth
        '//
        '// J = Julian day number
        '// emb = Equatorial rectangular coordinates of EMB.
        '// pr = Earth's distance to the Sun (au)
        '//
        Friend Sub embofs(ByVal J As Double, ByRef ea() As Double, ByRef pr As Double)

            Dim pm(3), polm(3) As Double
            Dim a, b As Double
            Dim i As Integer

            '//
            '// Compute the vector Moon - Earth.
            '//
            gmoon(J, pm, polm)

            '//
            '// Precess the lunar position
            '// to ecliptic and equinox of J2000.0
            '//
            precess(pm, J, 1)

            '//
            '// Adjust the coordinates of the Earth
            '//
            a = 1.0 / (emrat + 1.0)
            b = 0.0
            For i = 0 To 2
                ea(i) = ea(i) - a * pm(i)
                b = b + ea(i) * ea(i)
            Next

            '//
            '// Sun-Earth distance.
            '//
            pr = Sqrt(b)
        End Sub
#End Region

#Region "MajElems"
        '/* Orbits for each planet.  The indicated orbital elements are
        '* not actually used, since the positions are are now calculated
        '* from a formula.  Magnitude and semidiameter are still used.
        '*/


        '/* January 5.0, 1987 */
        Friend mercury As New orbit("Mercury", 2446800.5, 7.0048, 48.177, 29.074, 0.387098, 4.09236, _
                                    0.205628, 198.7199, 2446800.5, -0.42, 3.36, mer404, 0.0, 0.0, 0.0)

        '/* Note the calculated apparent visual magnitude for Venus is not very accurate. */
        Friend venus As New orbit("Venus", 2446800.5, 3.3946, 76.561, 54.889, 0.723329, 1.60214, _
                                    0.006757, 9.0369, 2446800.5, -4.4, 8.34, ven404, 0.0, 0.0, 0.0)

        '/* Fixed numerical values will be used for earth if read in from a file named earth.orb.  See kfiles.c, kep.h. */
        Friend earth As New orbit("Earth", 2446800.5, 0.0, 0.0, 102.884, 0.999999, 0.985611, _
                                   0.016713, 1.1791, 2446800.5, -3.86, 0.0, ear404, 0.0, 0.0, 0.0)

        Friend mars As New orbit("Mars", 2446800.5, 1.8498, 49.457, 286.343, 1.52371, 0.524023, _
                                  0.093472, 53.1893, 2446800.5, -1.52, 4.68, mar404, 0.0, 0.0, 0.0)

        Friend jupiter As New orbit("Jupiter", 2446800.5, 1.3051, 100.358, 275.129, 5.20265, 0.0830948, _
                                     0.0481, 344.5086, 2446800.5, -9.4, 98.44, jup404, 0.0, 0.0, 0.0)

        Friend saturn As New orbit("Saturn", 2446800.5, 2.4858, 113.555, 337.969, 9.5405, 0.033451, _
                                    0.052786, 159.6327, 2446800.5, -8.88, 82.73, sat404, 0.0, 0.0, 0.0)

        Friend uranus As New orbit("Uranus", 2446800.5, 0.7738, 73.994, 98.746, 19.2233, 0.0116943, _
                                    0.045682, 84.8516, 2446800.5, -7.19, 35.02, ura404, 0.0, 0.0, 0.0)

        Friend neptune As New orbit("Neptune", 2446800.5, 1.7697, 131.677, 250.623, 30.1631, 0.00594978, _
                                     0.009019, 254.2568, 2446800.5, -6.87, 33.5, nep404, 0.0, 0.0, 0.0)

        Friend pluto As New orbit("Pluto", 2446640.5, 17.1346, 110.204, 114.21, 39.4633, 0.0039757, _
                                   0.248662, 355.0554, 2446640.5, -1.0, 2.07, plu404, 0.0, 0.0, 0.0)
#End Region

#Region "GPlan"
        Private ss(NARGS, 31) As Double
        Private cc(NARGS, 31) As Double
        Private Args(NARGS) As Double
        Private LP_equinox, NF_arcsec, Ea_arcsec, pA_precession As Double

        '/*   Routines to chew through tables of perturbations.  */
        Friend Function mods3600(ByVal x As Double) As Double
            Return ((x) - 1296000.0 * Floor((x) / 1296000.0))
        End Function

        '/* From Simon et al (1994)  */
        '/* Arc sec per 10000 Julian years.  */
        Friend freqs() As Double = { _
              53810162868.8982, _
              21066413643.3548, _
              12959774228.3429, _
              6890507749.3988, _
              1092566037.7991, _
              439960985.5372, _
              154248119.3933, _
              78655032.0744, _
              52272245.1795}

        '/* Arc sec.  */
        Friend phases() As Double = { _
              252.25090552 * 3600.0, _
              181.97980085 * 3600.0, _
              100.46645683 * 3600.0, _
              355.43299958 * 3600.0, _
              34.35151874 * 3600.0, _
              50.0774443 * 3600.0, _
              314.05500511 * 3600.0, _
              304.34866548 * 3600.0, _
              860492.1546}

        Friend Function gplan(ByVal JD As Double, ByRef plan As plantbl, ByRef pobj() As Double) As Integer
            Dim su, cu, sv, cv, TI As Double
            Dim t, sl, sb, sr As Double
            Dim i, j, k, m, n, k1, ip, np, nt As Integer
            Dim p, pl, pb, pr As Integer

            TI = (JD - J2000) / plan.timescale
            n = plan.maxargs
            '/* Calculate sin( i*MM ), etc. for needed multiple angles.  */
            For i = 0 To n - 1
                j = plan.max_harmonic(i)
                If (j > 0) Then
                    sr = (mods3600(freqs(i) * TI) + phases(i)) * STR
                    sscc(i, sr, j)
                End If
            Next

            '/* Point to start of table of arguments. */

            p = 0 'p = plan.arg_tbl
            '/* Point to tabulated cosine and sine amplitudes.  */
            pl = 0   'pl = plan.lon_tbl
            pb = 0   'pb = plan.lat_tbl
            pr = 0  'pr = plan.rad_tbl

            sl = 0.0
            sb = 0.0
            sr = 0.0

            Do
                '/* argument of sine and cosine */
                '/* Number of periodic arguments. */
                np = plan.arg_tbl(p) : p += 1
                If (np < 0) Then Exit Do
                If (np = 0) Then  '/* It is a polynomial term.  */
                    nt = plan.arg_tbl(p) : p += 1
                    cu = plan.lon_tbl(pl) : pl += 1  '/* Longitude polynomial. */
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lon_tbl(pl) : pl += 1
                    Next
                    sl += mods3600(cu)

                    cu = plan.lat_tbl(pb) : pb += 1 '/* Latitude polynomial. */
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lat_tbl(pb) : pb += 1
                    Next
                    sb += cu

                    cu = plan.rad_tbl(pr) : pr += 1 '/* Radius polynomial. */
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.rad_tbl(pr) : pr += 1
                    Next
                    sr += cu
                Else
                    k1 = 0
                    cv = 0.0
                    sv = 0.0
                    For ip = 0 To np - 1
                        j = plan.arg_tbl(p) : p += 1 '/* What harmonic.  */
                        m = plan.arg_tbl(p) - 1 : p += 1  '/* Which planet.  */
                        If (j <> 0) Then
                            k = j
                            If (j < 0) Then k = -k
                            k -= 1
                            su = ss(m, k) '/* sin(k*angle) */
                            If (j < 0) Then su = -su
                            cu = cc(m, k)
                            If (k1 = 0) Then
                                '/* set first angle */
                                sv = su
                                cv = cu
                                k1 = 1
                            Else
                                '/* combine angles */
                                t = su * cv + cu * sv
                                cv = cu * cv - su * sv
                                sv = t
                            End If
                        End If
                    Next
                    '/* Highest power of T.  */
                    nt = plan.arg_tbl(p) : p += 1
                    cu = plan.lon_tbl(pl) : pl += 1 '/* Longitude. */
                    su = plan.lon_tbl(pl) : pl += 1
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lon_tbl(pl) : pl += 1
                        su = su * TI + plan.lon_tbl(pl) : pl += 1
                    Next
                    sl += cu * cv + su * sv

                    cu = plan.lat_tbl(pb) : pb += 1 '/* Latitiude. */
                    su = plan.lat_tbl(pb) : pb += 1
                    For ip = 1 To nt
                        cu = cu * TI + plan.lat_tbl(pb) : pb += 1
                        su = su * TI + plan.lat_tbl(pb) : pb += 1
                    Next
                    sb += cu * cv + su * sv

                    cu = plan.rad_tbl(pr) : pr += 1 '/* Radius. */
                    su = plan.rad_tbl(pr) : pr += 1
                    For ip = 1 To nt
                        cu = cu * TI + plan.rad_tbl(pr) : pr += 1
                        su = su * TI + plan.rad_tbl(pr) : pr += 1
                    Next
                    sr += cu * cv + su * sv
                End If
            Loop

            pobj(0) = STR * sl
            pobj(1) = STR * sb
            pobj(2) = STR * plan.distance * sr + plan.distance

            Return 0
        End Function


        '/* Prepare lookup table of sin and cos ( i*Lj )
        ' * for required multiple angles
        ' */
        Friend Function sscc(ByVal k As Integer, ByVal arg As Double, ByVal n As Integer) As Integer
            Dim cu, su, cv, sv, s As Double
            Dim i As Integer

            su = Sin(arg)
            cu = Cos(arg)
            ss(k, 0) = su '/* sin(L) */
            cc(k, 0) = cu '/* cos(L) */
            sv = 2.0 * su * cu
            cv = cu * cu - su * su
            ss(k, 1) = sv '/* sin(2L) */
            cc(k, 1) = cv
            For i = 2 To n - 1
                s = su * cv + cu * sv
                cv = cu * cv - su * sv
                sv = s
                ss(k, i) = sv '/* sin( i+1 L ) */
                cc(k, i) = cv
            Next
            Return 0
        End Function
        '/* Compute mean elements at Julian date J.  */

        Sub mean_elements(ByVal J As Double)
            Dim x, T, T2 As Double

            '/* Time variables.  T is in Julian centuries.  */
            T = (J - 2451545.0) / 36525.0
            T2 = T * T

            '/* Mean longitudes of planets (Simon et al, 1994) .047" subtracted from constant term for offset to DE403 origin. */

            '/* Mercury */
            x = mods3600(538101628.68898189 * T + 908103.213)
            x += (0.00000639 * T - 0.0192789) * T2
            Args(0) = STR * x

            '/* Venus */
            x = mods3600(210664136.43354821 * T + 655127.236)
            x += (-0.00000627 * T + 0.0059381) * T2
            Args(1) = STR * x

            '/* Earth  */
            x = mods3600(129597742.283429 * T + 361679.198)
            x += (-0.00000523 * T - 0.0204411) * T2
            Ea_arcsec = x
            Args(2) = STR * x

            '/* Mars */
            x = mods3600(68905077.493988 * T + 1279558.751)
            x += (-0.00001043 * T + 0.0094264) * T2
            Args(3) = STR * x

            '/* Jupiter */
            x = mods3600(10925660.377991 * T + 123665.42)
            x += ((((-0.00000000034 * T + 0.0000000591) * T + 0.000004667) * T + 0.00005706) * T - 0.3060378) * T2
            Args(4) = STR * x

            '/* Saturn */
            x = mods3600(4399609.855372 * T + 180278.752)
            x += ((((0.00000000083 * T - 0.0000001452) * T - 0.000011484) * T - 0.00016618) * T + 0.7561614) * T2
            Args(5) = STR * x

            '/* Uranus */
            x = mods3600(1542481.193933 * T + 1130597.971) + (0.00002156 * T - 0.0175083) * T2
            Args(6) = STR * x

            '/* Neptune */
            x = mods3600(786550.320744 * T + 1095655.149) + (-0.00000895 * T + 0.0021103) * T2
            Args(7) = STR * x

            '/* Copied from cmoon.c, DE404 version.  */
            '/* Mean elongation of moon = D */
            x = mods3600(1602961600.9939659 * T + 1072261.2202445078)
            x += (((((-0.0000000000003207663637426 * T + 0.00000000002555243317839) * T + 0.000000002560078201452) * T _
                    - 0.00003702060118571) * T + 0.0069492746836058421) * T - 6.7352202374457519) * T2 '/* D, t^2 */ 
            Args(9) = STR * x

            '/* Mean distance of moon from its ascending node = F */
            x = mods3600(1739527262.8437717 * T + 335779.5141288474)
            x += (((((0.0000000000004474984866301 * T + 0.00000000004189032191814) * T - 0.000000002790392351314) * T _
                    - 0.000002165750777942) * T - 0.00075311878482337989) * T - 13.117809789650071) * T2 '/* F, t^2 */
            NF_arcsec = x
            Args(10) = STR * x

            '/* Mean anomaly of sun = l' (J. Laskar) */
            x = mods3600(129596581.0230432 * T + 1287102.7407441526)
            x += ((((((((1.62E-20 * T - 1.039E-17) * T - 0.00000000000000383508) * T + 0.0000000000004237343) * T _
                   + 0.000000000088555011) * T - 0.0000000477258489) * T - 0.000011297037031) * T + 0.0000874737173673247) * T _
                   - 0.55281306421783094) * T2
            Args(11) = STR * x

            '/* Mean anomaly of moon = l */
            x = mods3600(1717915922.8846793 * T + 485868.17465825332)
            x += (((((-0.000000000001755312760154) * T + 0.00000000003452144225877 * T - 0.00000002506365935364) * T _
                  - 0.0002536291235258) * T + 0.052099641302735818) * T + 31.501359071894147) * T2 ' /* l, t^2 */
            Args(12) = STR * x

            '/* Mean longitude of moon, re mean ecliptic and equinox of date = L  */
            x = mods3600(1732564372.0442266 * T + 785939.8092105242)
            x += (((((0.00000000000007200592540556 * T + 0.0000000002235210987108) * T - 0.00000001024222633731) * T _
              - 0.00006073960534117) * T + 0.006901724852838049) * T - 5.65504600274714) * T2 ' /* L, t^2 */
            LP_equinox = x
            Args(13) = STR * x

            '/* Precession of the equinox  */
            x = (((((((((-8.66E-20 * T - 4.759E-17) * T + 0.000000000000002424) * T + 0.0000000000013095) * T _
                      + 0.00000000017451) * T - 0.000000018055) * T - 0.0000235316) * T + 0.000076) * T _
                      + 1.105414) * T + 5028.791959) * T
            '/* Moon's longitude re fixed J2000 equinox.  */
            '/*
            'Args(13) -= x;
            '*/
            pA_precession = STR * x

            '/* Free librations.  */
            '/* longitudinal libration 2.891725 years */
            x = mods3600(44817540.9 * T + 806045.7)
            Args(14) = STR * x
            '/* libration P, 24.2 years */
            x = mods3600(5364867.87 * T - 391702.8)
            Args(15) = STR * x

            'Args(16) = 0.0

            '/* libration W, 74.7 years. */
            x = mods3600(1735730.0 * T)
            Args(17) = STR * x
        End Sub


        '/* Generic program to accumulate sum of trigonometric series
        '   in three variables (e.g., longitude, latitude, radius)
        '   of the same list of arguments.  */

        Friend Function g3plan(ByVal JD As Double, ByRef plan As plantbl, ByRef pobj() As Double, ByVal objnum As Integer) As Integer
            Dim i, j, k, m, n, k1, ip, np, nt As Integer
            Dim p, pl, pb, pr As Integer
            Dim su, cu, sv, cv As Double
            Dim TI, t, sl, sb, sr As Double

            mean_elements(JD)
            '#If 0 Then
            '  /* For librations, moon's longitude is sidereal.  */
            '            If (flag) Then
            '    Args(13) -= pA_precession;
            '#End If

            TI = (JD - J2000) / plan.timescale
            n = plan.maxargs
            '/* Calculate sin( i*MM ), etc. for needed multiple angles.  */
            For i = 0 To n - 1
                j = plan.max_harmonic(i)
                If (j > 0) Then sscc(i, Args(i), j)
            Next

            '/* Point to start of table of arguments. */
            p = 0 'plan.arg_tbl
            '/* Point to tabulated cosine and sine amplitudes.  */
            pl = 0 'plan.lon_tbl
            pb = 0 'plan.lat_tbl
            pr = 0 'plan.rad_tbl
            sl = 0.0
            sb = 0.0
            sr = 0.0

            Do
                '/* argument of sine and cosine */
                '/* Number of periodic arguments. */
                np = plan.arg_tbl(p) : p += 1
                If (np < 0) Then Exit Do
                If (np = 0) Then  '/* It is a polynomial term.  */
                    nt = plan.arg_tbl(p) : p += 1
                    cu = plan.lon_tbl(pl) : pl += 1 '/* "Longitude" polynomial (phi). */
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lon_tbl(pl) : pl += 1
                    Next
                    '/*	  sl +=  mods3600 (cu); */
                    sl += cu

                    cu = plan.lat_tbl(pb) : pb += 1 '/* "Latitude" polynomial (theta). */
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lat_tbl(pb) : pb += 1
                    Next
                    sb += cu

                    cu = plan.rad_tbl(pr) : pr += 1 '/* Radius polynomial (psi). */
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.rad_tbl(pr) : pr += 1
                    Next
                    sr += cu
                Else
                    k1 = 0
                    cv = 0.0
                    sv = 0.0
                    For ip = 0 To np - 1
                        j = plan.arg_tbl(p) : p += 1  '/* What harmonic.  */
                        m = plan.arg_tbl(p) - 1 : p += 1 '/* Which planet.  */
                        If (j <> 0) Then
                            '/*	      k = abs (j); */
                            If (j < 0) Then
                                k = -j
                            Else
                                k = j
                            End If
                            k -= 1
                            su = ss(m, k) '/* sin(k*angle) */
                            If (j < 0) Then su = -su
                            cu = cc(m, k)
                            If (k1 = 0) Then '/* set first angle */
                                sv = su
                                cv = cu
                                k1 = 1
                            Else
                                '/* combine angles */	
                                t = su * cv + cu * sv
                                cv = cu * cv - su * sv
                                sv = t
                            End If
                        End If
                    Next
                    '/* Highest power of T.  */
                    nt = plan.arg_tbl(p) : p += 1

                    '/* Longitude. */
                    cu = plan.lon_tbl(pl) : pl += 1
                    su = plan.lon_tbl(pl) : pl += 1
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lon_tbl(pl) : pl += 1
                        su = su * TI + plan.lon_tbl(pl) : pl += 1
                    Next
                    sl += cu * cv + su * sv

                    '/* Latitiude. */
                    cu = plan.lat_tbl(pb) : pb += 1
                    su = plan.lat_tbl(pb) : pb += 1
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lat_tbl(pb) : pb += 1
                        su = su * TI + plan.lat_tbl(pb) : pb += 1
                    Next
                    sb += cu * cv + su * sv

                    '/* Radius. */
                    cu = plan.rad_tbl(pr) : pr += 1
                    su = plan.rad_tbl(pr) : pr += 1
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.rad_tbl(pr) : pr += 1
                        su = su * TI + plan.rad_tbl(pr) : pr += 1
                    Next
                    sr += cu * cv + su * sv
                End If
            Loop
            t = plan.trunclvl
            pobj(0) = Args(objnum - 1) + STR * t * sl
            pobj(1) = STR * t * sb
            pobj(2) = plan.distance * (1.0 + STR * t * sr)
            Return 0
        End Function

        '/* Generic program to accumulate sum of trigonometric series
        '   in two variables (e.g., longitude, radius)
        '   of the same list of arguments.  */
        Friend Function g2plan(ByVal JD As Double, ByRef plan As plantbl, ByRef pobj() As Double) As Integer
            Dim i, j, k, m, n, k1, ip, np, nt As Integer
            Dim p, pl, pr As Integer
            Dim su, cu, sv, cv As Double
            Dim TI, t, sl, sr As Double

            mean_elements(JD)
            '#If 0 Then
            '/* For librations, moon's longitude is sidereal.  */
            'If (flag) Then
            'Args(13) -= pA_precession;
            '#End If
            TI = (JD - J2000) / plan.timescale
            n = plan.maxargs
            '/* Calculate sin( i*MM ), etc. for needed multiple angles.  */
            For i = 0 To n - 1
                j = plan.max_harmonic(i)
                If (j > 0) Then sscc(i, Args(i), j)
            Next

            '/* Point to start of table of arguments. */
            p = 0 'plan.arg_tbl
            '/* Point to tabulated cosine and sine amplitudes.  */
            pl = 0 '(long *) plan.lon_tbl;
            pr = 0 '(long *) plan.rad_tbl;
            sl = 0.0
            sr = 0.0

            Do
                '/* argument of sine and cosine */
                '/* Number of periodic arguments. */
                np = plan.arg_tbl(p) : p += 1 '*p++;
                If (np < 0) Then Exit Do

                If (np = 0) Then  '/* It is a polynomial term.  */
                    nt = plan.arg_tbl(p) : p += 1
                    cu = plan.lon_tbl(pl) : pl += 1 '*pl++; '/* Longitude polynomial. */
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lon_tbl(pl) : pl += 1 '*pl++;
                    Next
                    '/*	  sl +=  mods3600 (cu); */
                    sl += cu
                    '/* Radius polynomial. */
                    cu = plan.rad_tbl(pr) : pr += 1 '*pr++;
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.rad_tbl(pr) : pr += 1
                    Next
                    sr += cu
                Else
                    k1 = 0
                    cv = 0.0
                    sv = 0.0
                    For ip = 0 To np - 1
                        j = plan.arg_tbl(p) : p += 1 '/* What harmonic.  */
                        m = plan.arg_tbl(p) - 1 : p += 1  '/* Which planet.  */
                        If (j <> 0) Then
                            '/*	      k = abs (j); */
                            If (j < 0) Then
                                k = -j
                            Else
                                k = j
                            End If
                            k -= 1
                            su = ss(m, k)  '/* sin(k*angle) */
                            If (j < 0) Then su = -su
                            cu = cc(m, k)
                            If (k1 = 0) Then
                                '/* set first angle */
                                sv = su
                                cv = cu
                                k1 = 1
                            Else
                                '/* combine angles */
                                t = su * cv + cu * sv
                                cv = cu * cv - su * sv
                                sv = t
                            End If
                        End If
                    Next
                    '/* Highest power of T.  */
                    nt = plan.arg_tbl(p) : p += 1 '*p++;
                    '/* Longitude. */
                    cu = plan.lon_tbl(pl) : pl += 1
                    su = plan.lon_tbl(pl) : pl += 1
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lon_tbl(pl) : pl += 1
                        su = su * TI + plan.lon_tbl(pl) : pl += 1
                    Next
                    sl += cu * cv + su * sv
                    '/* Radius. */
                    cu = plan.rad_tbl(pr) : pr += 1
                    su = plan.rad_tbl(pr) : pr += 1
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.rad_tbl(pr) : pr += 1
                        su = su * TI + plan.rad_tbl(pr) : pr += 1
                    Next
                    sr += cu * cv + su * sv
                End If
            Loop
            t = plan.trunclvl
            pobj(0) = t * sl
            pobj(2) = t * sr
            Return 0
        End Function


        '/* Generic program to accumulate sum of trigonometric series
        '   in one variable.  */

        Friend Function g1plan(ByVal JD As Double, ByRef plan As plantbl) As Double
            Dim i, j, k, m, k1, ip, np, nt As Integer
            Dim p, pl As Integer
            Dim su, cu, sv, cv As Double
            Dim TI, t, sl As Double

            TI = (JD - J2000) / plan.timescale
            mean_elements(JD)
            '/* Calculate sin( i*MM ), etc. for needed multiple angles.  */
            For i = 0 To NARGS - 1
                j = plan.max_harmonic(i)
                If (j > 0) Then sscc(i, Args(i), j)
            Next

            '/* Point to start of table of arguments. */
            p = 0 'plan.arg_tbl;
            '/* Point to tabulated cosine and sine amplitudes.  */
            pl = 0 '(long *) plan.lon_tbl;
            sl = 0.0

            Do    '/* argument of sine and cosine */
                '/* Number of periodic arguments. */
                np = plan.arg_tbl(p) : p += 1 '*p++;
                If (np < 0) Then Exit Do
                If (np = 0) Then
                    '/* It is a polynomial term.  */
                    nt = plan.arg_tbl(p) : p += 1 '*p++;
                    cu = plan.lon_tbl(pl) : pl += 1 '*pl++;
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lon_tbl(pl) : pl += 1 '*pl++;
                    Next
                    '/*	  sl +=  mods3600 (cu); */
                    sl += cu
                Else
                    k1 = 0
                    cv = 0.0
                    sv = 0.0
                    For ip = 0 To np - 1
                        j = plan.arg_tbl(p) : p += 1   '/* What harmonic.  */
                        m = plan.arg_tbl(p) - 1 : p += 1 '/* Which planet.  */
                        If (j <> 0) Then
                            '/*	      k = abs (j); */
                            If (j < 0) Then
                                k = -j
                            Else
                                k = j
                            End If
                            k -= 1
                            su = ss(m, k)  '/* sin(k*angle) */
                            If (j < 0) Then su = -su
                            cu = cc(m, k)
                            If (k1 = 0) Then
                                '/* set first angle */
                                sv = su
                                cv = cu
                                k1 = 1
                            Else
                                '/* combine angles */
                                t = su * cv + cu * sv
                                cv = cu * cv - su * sv
                                sv = t
                            End If
                        End If
                    Next
                    '/* Highest power of T.  */
                    nt = plan.arg_tbl(p) : p += 1
                    '/* Cosine and sine coefficients.  */
                    cu = plan.lon_tbl(pl) : pl += 1
                    su = plan.lon_tbl(pl) : pl += 1
                    For ip = 0 To nt - 1
                        cu = cu * TI + plan.lon_tbl(pl) : pl += 1
                        su = su * TI + plan.lon_tbl(pl) : pl += 1
                    Next
                    sl += cu * cv + su * sv
                End If
            Loop
            Return (plan.trunclvl * sl)
        End Function

        Friend Function gmoon(ByVal J As Double, ByRef rect() As Double, ByRef pol() As Double) As Integer
            Dim x, cosB, sinB, cosL, sinL, eps, coseps, sineps As Double

            g2plan(J, moonlr, pol)
            x = pol(0)
            x += LP_equinox
            If (x < -645000.0) Then x += 1296000.0
            If (x > 645000.0) Then x -= 1296000.0
            pol(0) = STR * x
            x = g1plan(J, moonlat)
            pol(1) = STR * x
            x = (1.0 + STR * pol(2)) * moonlr.distance
            pol(2) = x
            '/* Convert ecliptic polar to equatorial rectangular coordinates.  */
            epsiln(J, eps, coseps, sineps)
            cosB = Cos(pol(1))
            sinB = Sin(pol(1))
            cosL = Cos(pol(0))
            sinL = Sin(pol(0))
            rect(0) = cosB * cosL * x
            rect(1) = (coseps * cosB * sinL - sineps * sinB) * x
            rect(2) = (sineps * cosB * sinL + coseps * sinB) * x
            Return 0
        End Function

#End Region

    End Module
End Namespace