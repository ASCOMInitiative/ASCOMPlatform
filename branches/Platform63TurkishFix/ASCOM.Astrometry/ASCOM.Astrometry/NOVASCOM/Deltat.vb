Option Strict On
Option Explicit On
Imports System.Math
Module DeltatCode
    ''' <summary>
    ''' Calculates the value of DeltaT over a wide range of hstoric and future Julian dates
    ''' </summary>
    ''' <param name="tjd">Julian Date of interest</param>
    ''' <returns>DelatT value at the given Julian date</returns>
    ''' <remarks>
    ''' Post 2011, calculation is effected throgh a 2nd order polynomial best fit to real DeltaT data from: http://maia.usno.navy.mil/ser7/deltat.data 
    ''' together with projections of DeltaT from: http://maia.usno.navy.mil/ser7/deltat.preds
    ''' The analysis spreadsheets for DeltaT values at dates post 2011 are stored in the \NOVAS\DeltaT Predictions folder of the ASCOM source tree.
    ''' </remarks>
    Function DeltaTCalc(ByVal tjd As Double) As Double

        Const TABSTART1620 As Double = 1620.0
        Const TABSIZ As Integer = 392

        Dim YearFraction, B As Double

        Static ans As Double ' Variable To hold the last calculated DelatT value
        Static lasttjd As Double = Double.MinValue ' Initialise the last used Julian date to a value that will force a calculation on first time use

        ' Performance optimisation:
        If (tjd = lasttjd) Then Return ans ' Return last calculated value if tjd is same as last call
        lasttjd = tjd ' Save the Julian date for use in the caching approach above

        YearFraction = 2000.0 + (tjd - T0) / 365.25 ' This calculation is accurate enough for our purposes here (T0 = 2451545.0 is TDB Julian date of epoch J2000.0)

        ' DATE RANGE January 2017 Onwards - The analysis was performed on 29th December 2016 and creates values within 0.12 of a second of the projections to Q3 2019
        If (YearFraction >= 2017.0) And (YearFraction < Double.MaxValue) Then
            ans = (0.02465436 * YearFraction * YearFraction) + (-98.92626556 * YearFraction) + 99301.85784308
            Return (ans)
        End If

        ' DATE RANGE October 2015 Onwards - The analysis was performed on 24th October 2015 and creates values within 0.05 of a second of the projections to Q2 2018
        If (YearFraction >= 2015.75) And (YearFraction < Double.MaxValue) Then
            ans = (0.02002376 * YearFraction * YearFraction) + (-80.27921003 * YearFraction) + 80529.32
            Return (ans)
        End If

        ' DATE RANGE October 2011 to September 2015 - The analysis was performed on 6th February 2014 and creates values within 0.2 of a second of the projections to Q1 2016
        If (YearFraction >= 2011.75) And (YearFraction < 2015.75) Then
            ans = (0.00231189 * YearFraction * YearFraction) + (-8.85231952 * YearFraction) + 8518.54
            Return (ans)
        End If

        ' DATE RANGE January 2011 to September 2011
        If (YearFraction >= 2011.0) And (YearFraction < 2011.75) Then
            ' Following now superseded by above for 2012-16, this is left in for consistency with previous behaviour
            ' Use polynomial given at http://sunearth.gsfc.nasa.gov/eclipse/SEcat5/deltatpoly.html as retrtieved on 11-Jan-2009
            B = YearFraction - 2000.0
            ans = 62.92 + (B * (0.32217 + (B * 0.005589)))
            Return (ans)
        End If

        ' Setup for pre 2011 calculations using Bob Denny's original code

        '/* Note, Stephenson and Morrison's table starts at the year 1630.
        ' * The Chapronts' table does not agree with the Almanac prior to 1630.
        ' * The actual accuracy decreases rapidly prior to 1780.
        ' */
        'static short dt[] = {
        Dim dt() As Short = {
        12400, 11900, 11500, 11000, 10600, 10200, 9800, 9500, 9100, 8800,
        8500, 8200, 7900, 7700, 7400, 7200, 7000, 6700, 6500, 6300,
        6200, 6000, 5800, 5700, 5500, 5400, 5300, 5100, 5000, 4900,
        4800, 4700, 4600, 4500, 4400, 4300, 4200, 4100, 4000, 3800,
        3700, 3600, 3500, 3400, 3300, 3200, 3100, 3000, 2800, 2700,
        2600, 2500, 2400, 2300, 2200, 2100, 2000, 1900, 1800, 1700,
        1600, 1500, 1400, 1400, 1300, 1200, 1200, 1100, 1100, 1000,
        1000, 1000, 900, 900, 900, 900, 900, 900, 900, 900,
        900, 900, 900, 900, 900, 900, 900, 900, 1000, 1000,
        1000, 1000, 1000, 1000, 1000, 1000, 1000, 1100, 1100, 1100,
        1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100,
        1100, 1100, 1100, 1100, 1200, 1200, 1200, 1200, 1200, 1200,
        1200, 1200, 1200, 1200, 1300, 1300, 1300, 1300, 1300, 1300,
        1300, 1400, 1400, 1400, 1400, 1400, 1400, 1400, 1500, 1500,
        1500, 1500, 1500, 1500, 1500, 1600, 1600, 1600, 1600, 1600,
        1600, 1600, 1600, 1600, 1600, 1700, 1700, 1700, 1700, 1700,
        1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700,
        1700, 1700, 1600, 1600, 1600, 1600, 1500, 1500, 1400, 1400,
        1370, 1340, 1310, 1290, 1270, 1260, 1250, 1250, 1250, 1250,
        1250, 1250, 1250, 1250, 1250, 1250, 1250, 1240, 1230, 1220,
        1200, 1170, 1140, 1110, 1060, 1020, 960, 910, 860, 800,
        750, 700, 660, 630, 600, 580, 570, 560, 560, 560,
        570, 580, 590, 610, 620, 630, 650, 660, 680, 690,
        710, 720, 730, 740, 750, 760, 770, 770, 780, 780,
        788, 782, 754, 697, 640, 602, 541, 410, 292, 182,
        161, 10, -102, -128, -269, -324, -364, -454, -471, -511,
        -540, -542, -520, -546, -546, -579, -563, -564, -580, -566,
        -587, -601, -619, -664, -644, -647, -609, -576, -466, -374,
        -272, -154, -2, 124, 264, 386, 537, 614, 775, 913,
        1046, 1153, 1336, 1465, 1601, 1720, 1824, 1906, 2025, 2095,
        2116, 2225, 2241, 2303, 2349, 2362, 2386, 2449, 2434, 2408,
        2402, 2400, 2387, 2395, 2386, 2393, 2373, 2392, 2396, 2402,
        2433, 2483, 2530, 2570, 2624, 2677, 2728, 2778, 2825, 2871,
        2915, 2957, 2997, 3036, 3072, 3107, 3135, 3168, 3218, 3268,
        3315, 3359, 3400, 3447, 3503, 3573, 3654, 3743, 3829, 3920,
        4018, 4117, 4223, 4337, 4449, 4548, 4646, 4752, 4853, 4959,
        5054, 5138, 5217, 5296, 5379, 5434, 5487, 5532, 5582, 5630,
        5686, 5757, 5831, 5912, 5998, 6078, 6163, 6230, 6296, 6347,
        6383, 6409, 6430, 6447, 6457, 6469, 6485, 6515, 6546, 6570,
        6650, 6710}
        ' Change TABEND and TABSIZ if you add/delete anything

        ' Calculate  DeltaT = ET - UT in seconds.  Describes the irregularities of the Earth rotation rate in the ET time scale.
        Dim p As Double
        Dim d(6) As Integer
        Dim i, iy, k As Integer

        ' DATE RANGE <1620
        If (YearFraction < TABSTART1620) Then
            If (YearFraction >= 948.0) Then
                '/* Stephenson and Morrison, stated domain is 948 to 1600:
                ' * 25.5(centuries from 1800)^2 - 1.9159(centuries from 1955)^2
                ' */
                B = 0.01 * (YearFraction - 2000.0)
                ans = (23.58 * B + 100.3) * B + 101.6
            Else
                '/* Borkowski */
                B = 0.01 * (YearFraction - 2000.0) + 3.75
                ans = 35.0 * B * B + 40.0
            End If
            Return ans
        End If

        'DATE RANGE 1620 to 2011

        ' Besselian interpolation from tabulated values. See AA page K11.
        ' Index into the table.
        p = Floor(YearFraction)
        iy = CInt(p - TABSTART1620)            '// rbd - added cast
        '/* Zeroth order estimate is value at start of year */
        ans = dt(iy)
        k = iy + 1
        If (k >= TABSIZ) Then GoTo done ' /* No data, can't go on. */

        '/* The fraction of tabulation interval */
        p = YearFraction - p

        '/* First order interpolated value */
        ans += p * (dt(k) - dt(iy))
        If ((iy - 1 < 0) Or (iy + 2 >= TABSIZ)) Then GoTo done ' /* can't do second differences */

        '/* Make table of first differences */
        k = iy - 2
        For i = 0 To 4
            If ((k < 0) Or (k + 1 >= TABSIZ)) Then
                d(i) = 0
            Else
                d(i) = dt(k + 1) - dt(k)
            End If
            k += 1
        Next
        '/* Compute second differences */
        For i = 0 To 3
            d(i) = d(i + 1) - d(i)
        Next
        B = 0.25 * p * (p - 1.0)
        ans += B * (d(1) + d(2))
        If (iy + 2 >= TABSIZ) Then GoTo done

        '/* Compute third differences */
        For i = 0 To 2
            d(i) = d(i + 1) - d(i)
        Next
        B = 2.0 * B / 3.0
        ans += (p - 0.5) * B * d(1)
        If ((iy - 2 < 0) Or (iy + 3 > TABSIZ)) Then GoTo done

        '/* Compute fourth differences */
        For i = 0 To 1
            d(i) = d(i + 1) - d(i)
        Next
        B = 0.125 * B * (p + 1.0) * (p - 2.0)
        ans += B * (d(0) + d(1))

done:
        '/* Astronomical Almanac table is corrected by adding the expression
        ' *     -0.000091 (ndot + 26)(year-1955)^2  seconds
        ' * to entries prior to 1955 (AA page K8), where ndot is the secular
        ' * tidal term in the mean motion of the Moon.
        ' *
        ' * Entries after 1955 are referred to atomic time standards and
        ' * are not affected by errors in Lunar or planetary theory.
        ' */
        ans *= 0.01
        If (YearFraction < 1955.0) Then
            B = (YearFraction - 1955.0)
            ans += -0.000091 * (-25.8 + 26.0) * B * B
        End If
        Return ans

    End Function
End Module