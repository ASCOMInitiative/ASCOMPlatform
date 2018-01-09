Imports System.Runtime.InteropServices
Imports ASCOM.Utilities

Namespace AstroUtils
    ''' <summary>
    ''' Class providing a suite of tested astronomy support functions to save develpment effort and provide consistant behaviour.
    ''' </summary>
    ''' <remarks>
    ''' A number of these routines are provided to support migration from the Astro32.dll. Unlike Astro32, these routines will work in 
    ''' both 32bit and 64bit applications.
    ''' </remarks>
    <Guid("5679F94A-D4D1-40D3-A0F8-7CE61100A691"), _
        ClassInterface(ClassInterfaceType.None), _
        ComVisible(True)> _
    Public Class AstroUtils
        Implements IAstroUtils, IDisposable

        Private TL As TraceLogger, Utl As Util, Nov31 As NOVAS.NOVAS31, RegAccess As RegistryAccess
        Private UtcTaiOffset As Integer

        Friend Structure BodyInfo
            Public Altitude As Double
            Public Distance As Double
            Public Radius As Double
        End Structure

#Region "New and IDisposable Support"
        Sub New()
            TL = New TraceLogger("", "AstroUtils")
            TL.Enabled = GetBool(ASTROUTILS_TRACE, ASTROUTILS_TRACE_DEFAULT) 'Get enabled / disabled state from the user registry
            Utl = New Util
            Nov31 = New NOVAS.NOVAS31
            RegAccess = New RegistryAccess
            TL.LogMessage("New", "AstroUtils created Utilities component OK")
            'Set the current number of leap seconds once for this instance
            UtcTaiOffset = CInt(RegAccess.GetProfile(ASTROMETRY_SUBKEY, UTC_TAI_OFFSET_VALUENAME, TAI_UTC_OFFSET.ToString))
            TL.LogMessage("New", "Leap seconds: " & UtcTaiOffset)
            TL.LogMessage("New", "Finished initialisation OK")
        End Sub
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If Not (TL Is Nothing) Then
                        TL.Enabled = False
                        TL.Dispose()
                        TL = Nothing
                    End If
                    If Not (Utl Is Nothing) Then
                        Utl.Dispose()
                        Utl = Nothing
                    End If
                End If
                If Not (Nov31 Is Nothing) Then
                    Nov31.Dispose()
                    Nov31 = Nothing
                End If
                If Not (RegAccess Is Nothing) Then
                    RegAccess.Dispose()
                    RegAccess = Nothing
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        ''' <summary>
        ''' Releases all resources owned by the AstroUtils component and readies it for disposal
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        ''' <summary>
        ''' Flexible routine to range a number into a given range between a lower and an higher bound.
        ''' </summary>
        ''' <param name="Value">Value to be ranged</param>
        ''' <param name="LowerBound">Lowest value of the range</param>
        ''' <param name="LowerEqual">Boolean flag indicating whether the ranged value can have the lower bound value</param>
        ''' <param name="UpperBound">Highest value of the range</param>
        ''' <param name="UpperEqual">Boolean flag indicating whether the ranged value can have the upper bound value</param>
        ''' <returns>The ranged nunmber as a double</returns>
        ''' <exception cref="ASCOM.InvalidValueException">Thrown if the lower bound is greater than the upper bound.</exception>
        ''' <exception cref="ASCOM.InvalidValueException">Thrown if LowerEqual and UpperEqual are both false and the ranged value equals
        ''' one of these values. This is impossible to handle as the algorithm will always violate one of the rules!</exception>
        ''' <remarks>
        ''' UpperEqual and LowerEqual switches control whether the ranged value can be equal to either the upper and lower bounds. So, 
        ''' to range an hour angle into the range 0 to 23.999999.. hours, use this call: 
        ''' <code>RangedValue = Range(InputValue, 0.0, True, 24.0, False)</code>
        ''' <para>The input value will be returned in the range where 0.0 is an allowable value and 24.0 is not i.e. in the range 0..23.999999..</para>
        ''' <para>It is not permissible for both LowerEqual and UpperEqual to be false because it will not be possible to return a value that is exactly equal 
        ''' to either lower or upper bounds. An exception is thrown if this scenario is requested.</para>
        ''' </remarks>
        Public Function Range(Value As Double, LowerBound As Double, LowerEqual As Boolean, UpperBound As Double, UpperEqual As Boolean) As Double Implements IAstroUtils.Range
            Dim ModuloValue As Double
            If LowerBound >= UpperBound Then Throw New ASCOM.InvalidValueException("Range", "LowerBound is >= UpperBound", "LowerBound must be less than UpperBound")

            ModuloValue = UpperBound - LowerBound

            If LowerEqual Then
                If UpperEqual Then 'Lowest >= Highest <=
                    Do
                        If Value < LowerBound Then Value += ModuloValue
                        If Value > UpperBound Then Value -= ModuloValue
                    Loop Until (Value >= LowerBound) And (Value <= UpperBound)
                Else 'Lowest >= Highest <
                    Do
                        If Value < LowerBound Then Value += ModuloValue
                        If Value >= UpperBound Then Value -= ModuloValue
                    Loop Until (Value >= LowerBound) And (Value < UpperBound)
                End If
            Else
                If UpperEqual Then 'Lowest > Highest<=
                    Do
                        If Value <= LowerBound Then Value += ModuloValue
                        If Value > UpperBound Then Value -= ModuloValue
                    Loop Until (Value > LowerBound) And (Value <= UpperBound)
                Else 'Lowest > Highest <
                    If (Value = LowerBound) Then Throw New InvalidValueException("Range", "The supplied value equals the LowerBound. This can not be ranged when LowerEqual and UpperEqual are both false ", "LowerBound > Value < UpperBound")
                    If (Value = UpperBound) Then Throw New InvalidValueException("Range", "The supplied value equals the UpperBound. This can not be ranged when LowerEqual and UpperEqual are both false ", "LowerBound > Value < UpperBound")
                    Do
                        If Value <= LowerBound Then Value += ModuloValue
                        If Value >= UpperBound Then Value -= ModuloValue
                    Loop Until (Value > LowerBound) And (Value < UpperBound)
                End If
            End If
            Return Value
        End Function

        ''' <summary>
        ''' Conditions an hour angle to be in the range -12.0 to +12.0 by adding or subtracting 24.0 hours
        ''' </summary>
        ''' <param name="HA">Hour angle to condition</param>
        ''' <returns>Hour angle in the range -12.0 to +12.0</returns>
        ''' <remarks></remarks>
        Public Function ConditionHA(HA As Double) As Double Implements IAstroUtils.ConditionHA
            Dim ReturnValue As Double

            ReturnValue = Range(HA, -12.0, True, +12.0, True)
            TL.LogMessage("ConditionHA", "Conditioned HA: " & Utl.HoursToHMS(HA, ":", ":", "", 3) & " to: " & Utl.HoursToHMS(ReturnValue, ":", ":", "", 3))

            Return ReturnValue
        End Function

        ''' <summary>
        ''' Conditions a Right Ascension value to be in the range 0 to 23.999999.. hours 
        ''' </summary>
        ''' <param name="RA">Right ascension to be conditioned</param>
        ''' <returns>Right ascension in the range 0 to 23.999999...</returns>
        ''' <remarks></remarks>
        Public Function ConditionRA(RA As Double) As Double Implements IAstroUtils.ConditionRA
            Dim ReturnValue As Double

            ReturnValue = Range(RA, 0.0, True, 24.0, False)
            TL.LogMessage("ConditionRA", "Conditioned RA: " & Utl.HoursToHMS(RA, ":", ":", "", 3) & " to: " & Utl.HoursToHMS(ReturnValue, ":", ":", "", 3))

            Return ReturnValue
        End Function

        ''' <summary>
        ''' Returns the current DeltaT value in seconds
        ''' </summary>
        ''' <returns>DeltaT in seconds</returns>
        ''' <remarks>DeltaT is the difference between terrestrial time and the UT1 variant of universal time. ie.e TT = UT1 + DeltaT</remarks>
        Public Function DeltaT() As Double Implements IAstroUtils.DeltaT
            Dim RetVal, JD As Double

            JD = Utl.JulianDate
            RetVal = DeltaTCalc(JD)

            TL.LogMessage("DeltaT", "Returned: " & RetVal & " at Julian date: " & JD)
            Return RetVal
        End Function

        ''' <summary>
        ''' Current Julian date based on the UTC time scale
        ''' </summary>
        ''' <value>Julian day</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property JulianDateUtc As Double Implements IAstroUtils.JulianDateUtc
            Get
                Dim CurrentDate As Date, JD As Double

                CurrentDate = Date.UtcNow
                JD = Nov31.JulianDate(Convert.ToInt16(CurrentDate.Year), Convert.ToInt16(CurrentDate.Month), Convert.ToInt16(CurrentDate.Day), CurrentDate.TimeOfDay.TotalHours)
                TL.LogMessage("JulianDateUtc", "Returning: " & JD & " at UTC: " & Format(CurrentDate, "dddd dd MMMM yyyy HH:mm:ss.fff"))
                Return JD
            End Get
        End Property

        ''' <summary>
        ''' Current Julian date based on the terrestrial time (TT) time scale
        ''' </summary>
        ''' <param name="DeltaUT1">Current value for Delta-UT1, the difference between UTC and UT1; always in the range -0.9 to +0.9 seconds.
        ''' Use 0.0 to calculate TT through TAI. Delta-UT1 varies irregularly throughout the year.</param>
        ''' <returns>Double - Julian date on the UT1 timescale.</returns>
        ''' <remarks>When Delta-UT1 is provided, Terrestrial time is calculated as TT = UTC + DeltaUT1 + DeltaT. Otherwise, when Delta-UT1 is 0.0, 
        ''' TT is calculated as TT = UTC + ΔAT + 32.184s, where ΔAT is the current number of leap seconds applied to UTC (34 at April 2012, with 
        ''' the 35th being added at the end of June 2012). The resulting TT value is then converted to a Julian date and returned.
        ''' <para>Forecast values of Delta-UT1 are published by IERS Bulletin A at http://maia.usno.navy.mil/ser7/ser7.dat
        ''' </para></remarks>
        Public Function JulianDateTT(DeltaUT1 As Double) As Double Implements IAstroUtils.JulianDateTT
            Dim UTCDate, UT1Date, TTDate As Date, JD As Double, DeltaTTimespan, DeltaUT1Timespan As TimeSpan

            If (DeltaUT1 < -0.9) Or (DeltaUT1 > 0.9) Then Throw New ASCOM.InvalidValueException("JulianDateUT1", DeltaUT1.ToString, "-0.9 to +0.9")

            UTCDate = Date.UtcNow

            If DeltaUT1 <> 0.0 Then ' A specific value has been provided so use it and get to TT via DeltaT
                ' Compute as TT = UTC + DeltaUT1 + DeltaT
                DeltaTTimespan = TimeSpan.FromSeconds(Me.DeltaT) 'Get DeltaT as a timesapn
                DeltaUT1Timespan = TimeSpan.FromSeconds(DeltaUT1) 'Convert DeltaUT to a timesapn
                UT1Date = UTCDate.Add(DeltaUT1Timespan) 'Add delta-ut to UTC to yield UT1
                TTDate = UT1Date.Add(DeltaTTimespan) 'Add delta-t to UT1 to yield TT
            Else ' No value provided so get to TT through TAI
                ' Computation method TT = UTC + ΔAT + 32.184s. ΔAT = 35.0 leap seconds in June 2012
                TTDate = UTCDate.Add(TimeSpan.FromSeconds(CDbl(UtcTaiOffset) + TT_TAI_OFFSET))
            End If

            JD = Nov31.JulianDate(Convert.ToInt16(TTDate.Year), Convert.ToInt16(TTDate.Month), Convert.ToInt16(TTDate.Day), TTDate.TimeOfDay.TotalHours)
            TL.LogMessage("JulianDateTT", "Returning: " & JD & "at TT: " & Format(TTDate, "dddd dd MMMM yyyy HH:mm:ss.fff") & ", at UTC: " & Format(UTCDate, "dddd dd MMMM yyyy HH:mm:ss.fff"))

            Return JD
        End Function

        ''' <summary>
        ''' Current Julian date based on the UT1 time scale
        ''' </summary>
        ''' <param name="DeltaUT1">Current value for Delta-UT1, the difference between UTC and UT1; always in the range -0.9 to +0.9 seconds.
        ''' Use 0.0 if you do not know this value; it varies irregularly throughout the year.</param>
        ''' <returns>Double - Julian date on the UT1 timescale.</returns>
        ''' <remarks>UT1 time is calculated as UT1 = UTC + DeltaUT1 when DeltaUT1 is non zero. otherwise it is calaulcated through TAI and DeltaT.
        ''' This value is then converted to a Julian date and returned.
        ''' <para>When Delta-UT1 is provided, UT1 is calculated as UT1 = UTC + DeltaUT1. Otherwise, when Delta-UT1 is 0.0, 
        ''' DeltaUT1 is calculated as DeltaUT1 = TT - DeltaT = UTC + ΔAT + 32.184s - DeltaT, where ΔAT is the current number of leap seconds applied 
        ''' to UTC (34 at April 2012, with the 35th being added at the end of June 2012).</para>
        ''' <para>Forecast values of DUT1 are published by IERS Bulletin A at http://maia.usno.navy.mil/ser7/ser7.dat
        ''' </para></remarks>
        Public Function JulianDateUT1(DeltaUT1 As Double) As Double Implements IAstroUtils.JulianDateUT1
            Dim UTCDate, UT1Date, TTDate As Date, JD, DeltaT As Double, DeltaUT1Timespan As TimeSpan

            If (DeltaUT1 < -0.9) Or (DeltaUT1 > 0.9) Then Throw New ASCOM.InvalidValueException("JulianDateUT1", DeltaUT1.ToString, "-0.9 to +0.9")

            UTCDate = Date.UtcNow

            If DeltaUT1 <> 0.0 Then ' Calculate as UT1 = UTC - DeltaUT1
                DeltaUT1Timespan = TimeSpan.FromSeconds(DeltaUT1) 'Convert DeltaUT1 to a timesapn
                UT1Date = UTCDate.Add(DeltaUT1Timespan) 'Add delta-ut1 to UTC to yield UT1
            Else
                ' Calculation UT1 = TT - DeltaT = UTC + ΔAT + 32.184s - DeltaT
                DeltaT = DeltaTCalc(Nov31.JulianDate(Convert.ToInt16(UTCDate.Year), Convert.ToInt16(UTCDate.Month), Convert.ToInt16(UTCDate.Day), UTCDate.TimeOfDay.TotalHours))
                TTDate = UTCDate.Add(TimeSpan.FromSeconds(CDbl(UtcTaiOffset) + TT_TAI_OFFSET))
                UT1Date = TTDate.Subtract(TimeSpan.FromSeconds(DeltaT))
            End If

            JD = Nov31.JulianDate(Convert.ToInt16(UT1Date.Year), Convert.ToInt16(UT1Date.Month), Convert.ToInt16(UT1Date.Day), UT1Date.TimeOfDay.TotalHours)
            TL.LogMessage("JulianDateUT1", "Returning: " & JD & "at UT1: " & Format(UT1Date, "dddd dd MMMM yyyy HH:mm:ss.fff") & ", at UTC: " & Format(UTCDate, "dddd dd MMMM yyyy HH:mm:ss.fff"))

            Return JD
        End Function

        ''' <summary>
        ''' Computes atmospheric refraction in zenith distance. 
        ''' </summary>
        ''' <param name="Location">Structure containing observer's location.</param>
        ''' <param name="RefOption">1 ... Use 'standard' atmospheric conditions; 2 ... Use atmospheric 
        ''' parameters input in the 'Location' structure.</param>
        ''' <param name="ZdObs">Observed zenith distance, in degrees.</param>
        ''' <returns>Unrefracted zenith distance in degrees.</returns>
        ''' <remarks>This version computes approximate refraction for optical wavelengths. This function 
        ''' can be used for planning observations or telescope pointing, but should not be used for the 
        ''' reduction of precise observations.
        ''' <para>Note: Unlike the NOVAS Refract method, Unrefract returns the unrefracted zenith distance itself rather than 
        ''' the difference between the refracted and unrefracted zenith distances.</para></remarks>
        Public Function UnRefract(ByVal Location As OnSurface, _
                                                ByVal RefOption As RefractionOption, _
                                                ByVal ZdObs As Double) As Double Implements IAstroUtils.UnRefract
            Dim LoopCount As Integer, RefractedPosition, UnrefractedPosition As Double

            If (ZdObs < 0.0) Or (ZdObs > 90.0) Then Throw New ASCOM.InvalidValueException("Unrefract", "Zenith distance", "0.0 to 90.0 degrees")

            LoopCount = 0
            UnrefractedPosition = ZdObs
            Do
                LoopCount += 1
                RefractedPosition = UnrefractedPosition - Nov31.Refract(Location, RefOption, UnrefractedPosition)
                UnrefractedPosition = UnrefractedPosition + (ZdObs - RefractedPosition)
                TL.LogMessage("Unrefract", LoopCount & ": " & RefractedPosition & " " & UnrefractedPosition)
            Loop Until (LoopCount = 20) Or (RefractedPosition = ZdObs)

            TL.LogMessage("Unrefract", "Final: " & LoopCount & ", Unrefracted zenith distance: " & UnrefractedPosition)
            Return UnrefractedPosition

        End Function

        ''' <summary>
        ''' Converts a calendar day, month, year to a modified Julian date
        ''' </summary>
        ''' <param name="Day">Integer day of ther month</param>
        ''' <param name="Month">Integer month of the year</param>
        ''' <param name="Year">Integer year</param>
        ''' <returns>Double modified julian date</returns>
        ''' <remarks></remarks>
        Public Function CalendarToMJD(Day As Integer, Month As Integer, Year As Integer) As Double Implements IAstroUtils.CalendarToMJD
            Dim JD, MJD As Double
            JD = Nov31.JulianDate(Convert.ToInt16(Year), Convert.ToInt16(Month), Convert.ToInt16(Day), 0.0)
            MJD = JD - MJDBASE
            Return MJD
        End Function

        ''' <summary>
        ''' Translates a modified Julian date to a VB ole automation date, presented as a double
        ''' </summary>
        ''' <param name="MJD">Modified Julian date</param>
        ''' <returns>Date as a VB ole automation date</returns>
        ''' <remarks></remarks>
        Public Function MJDToOADate(MJD As Double) As Double Implements IAstroUtils.MJDToOADate
            Dim JulianDate As Date, JulianOADate As Double

            JulianDate = Utl.DateJulianToLocal(MJD + MJDBASE)
            JulianOADate = JulianDate.ToOADate

            Return JulianOADate

        End Function

        ''' <summary>
        ''' Translates a modified Julian date to a date
        ''' </summary>
        ''' <param name="MJD">Modified Julian date</param>
        ''' <returns>Date representing the modified Julian date</returns>
        ''' <remarks></remarks>
        Public Function MJDToDate(MJD As Double) As Date Implements IAstroUtils.MJDToDate
            Dim JulianDate As Date

            JulianDate = Utl.DateJulianToLocal(MJD + MJDBASE)

            Return JulianDate

        End Function

        ''' <summary>
        ''' Returns a modified Julian date as a string formatted acording to the supplied presentation format
        ''' </summary>
        ''' <param name="MJD">Mofified julian date</param>
        ''' <param name="PresentationFormat">Format representation</param>
        ''' <returns>Date string</returns>
        ''' <exception cref="FormatException">Thrown if the provided PresentationFormat is not valid.</exception>
        ''' <remarks>This expects the standard Microsoft date and time formatting characters as described 
        ''' in http://msdn.microsoft.com/en-us/library/362btx8f(v=VS.90).aspx
        ''' </remarks>
        Public Function FormatMJD(MJD As Double, PresentationFormat As String) As String Implements IAstroUtils.FormatMJD
            Dim MJDDate As Date, MJDDateString As String
            MJDDate = MJDToDate(MJD)
            MJDDateString = Format(MJDDate, PresentationFormat)
            Return MJDDateString
        End Function

        ''' <summary>
        ''' Proivides an estimates of DeltaUT1, the difference between UTC and UT1. DeltaUT1 = UT1 - UTC
        ''' </summary>
        ''' <param name="JulianDate">Julian date when DeltaUT is required</param>
        ''' <returns>Double DeltaUT in seconds</returns>
        ''' <remarks>DeltaUT varies only slowly, so the Julian date can be based on UTC, UT1 or Terrestrial Time.</remarks>
        Public Function DeltaUT(JulianDate As Double) As Double Implements IAstroUtils.DeltaUT1
            Dim DUT1 As Double
            DUT1 = CDbl(UtcTaiOffset) + TT_TAI_OFFSET - DeltaTCalc(JulianDate)
            TL.LogMessage("DeltaUT", "Returning: " & DUT1 & " at Julian date: " & JulianDate)
            Return DUT1
        End Function

        ''' <summary>
        ''' Returns a Julian date as a string formatted according to the supplied presentation format
        ''' </summary>
        ''' <param name="JD">Julian date</param>
        ''' <param name="PresentationFormat">Format representation</param>
        ''' <returns>Date as a string</returns>
        ''' <remarks>This expects the standard Microsoft date and time formatting characters as described 
        ''' in http://msdn.microsoft.com/en-us/library/362btx8f(v=VS.90).aspx
        ''' </remarks>
        Public Function FormatJD(JD As Double, PresentationFormat As String) As String Implements IAstroUtils.FormatJD
            Dim DaysSinceJ2000 As Double, J2000Date, ActualDate As Date, Retval As String

            TL.LogMessage("FormatJD", "JD, PresentationFormat: " & JD & " " & PresentationFormat)

            DaysSinceJ2000 = JD - J2000BASE

            'J2000 corresponds to 2000 Jan 1st 12.00 midday
            J2000Date = New Date(2000, 1, 1, 12, 0, 0)
            ActualDate = J2000Date.AddDays(DaysSinceJ2000)
            TL.LogMessage("FormatJD", "  DaysSinceJ2000, J2000Date, ActualDate: " & DaysSinceJ2000 & " " & J2000Date.ToString & " " & ActualDate.ToString)

            Retval = Format(ActualDate, PresentationFormat)
            TL.LogMessage("FormatJD", "  Result: " & Retval)

            Return Retval
        End Function

        ''' <summary>
        ''' Sets or returns the number of leap seconds used in ASCOM Astrometry functions
        ''' </summary>
        ''' <value>Integer number of seconds</value>
        ''' <returns>Current number of leap seconds</returns>
        ''' <remarks>The property value is stored in the ASCOM Profile under the name \Astrometry\Leap Seconds. Any change made to this property 
        ''' will be persisted to the ASCOM Profile store and will be immediately availble to this and all future instances of AstroUtils.
        ''' <para>The current value and any announced but not yet actioned change are listed 
        ''' here: ftp://hpiers.obspm.fr/iers/bul/bulc/bulletinc.dat</para> </remarks>
        Public Property LeapSeconds As Integer Implements IAstroUtils.LeapSeconds
            Get
                Return UtcTaiOffset
            End Get
            Set(value As Integer)
                UtcTaiOffset = value
                RegAccess.WriteProfile(ASTROMETRY_SUBKEY, UTC_TAI_OFFSET_VALUENAME, value.ToString)
            End Set
        End Property

        ''' <summary>
        ''' Function that returns a list of rise and set events of a particular type that occur on a particular day at a given latitude, longitude and time zone
        ''' </summary>
        ''' <param name="TypeofEvent">Type of event e.g. Sunrise or Astronomical twighlight</param>
        ''' <param name="Day">Integer Day number</param>
        ''' <param name="Month">Integer Month number</param>
        ''' <param name="Year">Integer Year number</param>
        ''' <param name="SiteLatitude">Site latitude</param>
        ''' <param name="SiteLongitude">Site longitude (West of Greenwich is negative)</param>
        ''' <param name="SiteTimeZone">Site time zone offset (West of Greenwich is negative)</param>
        ''' <returns>An arraylist of event information (see Remarks for arraylist structure).
        '''</returns>
        ''' <exception cref="ASCOM.InvalidValueException">If the combination of day, month and year is invalid e.g. 31st September.</exception>
        ''' <remarks>
        ''' <para>The definitions of sunrise, sunset and the various twighlights that are used in this method are taken from the 
        ''' <a href="http://aa.usno.navy.mil/faq/docs/RST_defs.php">US Naval Observatory Definitions</a>.
        ''' </para>
        ''' <para>The dynamics of the sun, Earth and Moon can result at some latitudes in days where there may be no, 1 or 2 rise or set events during 
        ''' a 24 hour period; in consequence, results are returned in the flexible form of arraylist.</para>
        ''' <para>The returned zero based arraylist has the following values:
        ''' <list type="Bullet">
        ''' <item>Arraylist(0)                              - Boolean - True if the body is above the event limit at midnight (the beginning of the 24 hour day), false if it is below the event limit</item>
        ''' <item>Arraylist(1)                              - Integer - Number of rise events in this 24 hour period</item>
        ''' <item>Arraylist(2)                              - Integer - Number of set events in this 24 hour period</item>
        ''' <item>Arraylist(3) onwards                      - Double  - Values of rise events in hours </item>
        ''' <item>Arraylist(3 + NumberOfRiseEvents) onwards - Double  - Values of set events in hours </item>
        ''' </list></para>
        ''' <para>If the number of rise events is zero the first double value will be the first set event. If the numbers of both rise and set events
        ''' are zero, there will be no double values and the arraylist will just contain elements 0, 1 and 2, the above/below horizon flag and the integer count values.</para>
        ''' <para>The algorithm employed in this method is taken from Astronomy on the Personal Computer (Montenbruck and Pfleger) pp 46..56, 
        ''' Springer Fourth Edition 2000, Fourth Printing 2009. The day is divided into twelve two hour intervals and a quadratic equation is fitted
        ''' to the altitudes at the beginning, middle and end of each interval. The resulting equation coefficients are then processed to determine 
        ''' the number of roots within the interval (each of which corresponds to a rise or set event) and their sense (rise or set). 
        ''' These results are are then aggregated over the day and the resultant list of values returned as the function result.
        ''' </para>
        ''' <para>High precision ephemeredes for the Sun, Moon and Earth and other planets from the JPL DE421 series are employed as delivered by the 
        ''' ASCOM NOVAS 3.1 component rather than using the lower precision ephemeredes employed by Montenbruck and Pfleger.
        ''' </para>
        ''' <para><b>Accuracy</b> Whole year almanacs for Sunrise/Sunset, Moonrise/Moonset and the various twighlights every 5 degrees from the 
        ''' North pole to the South Pole at a variety of longitudes, timezones and dates have been compared to data from
        ''' the <a href="http://aa.usno.navy.mil/data/docs/RS_OneYear.php">US Naval Observatory Astronomical Data</a> web site. The RMS error has been found to be 
        ''' better than 0.5 minute over the latitude range 80 degrees North to 80 degrees South and better than 5 minutes from 80 degrees to the relevant pole.
        ''' Most returned values are within 1 minute of the USNO values although some very infrequent grazing event times at lattiudes from 67 to 90 degrees North and South can be up to 
        ''' 10 minutes different.
        ''' </para>
        ''' <para>An Almanac program that creates a year's worth of information for a given event, lattitude, longitude and timezone is included in the 
        ''' developer code examples elsewhere in this help file. This creates an output file with an almost identical format to that used by the USNO web site 
        ''' and allows comprehensive checking of acccuracy for a given set of parameters.</para>
        ''' </remarks>
        Public Function EventTimes(TypeofEvent As EventType, Day As Integer, Month As Integer, Year As Integer, SiteLatitude As Double, SiteLongitude As Double, SiteTimeZone As Double) As ArrayList Implements IAstroUtils.EventTimes
            Dim DoesRise, DoesSet, AboveHorizon As Boolean
            Dim CentreTime, AltitiudeMinus1, Altitiude0, AltitiudePlus1, a, b, c, XSymmetry, YExtreme, Discriminant, RefractionCorrection As Double
            Dim DeltaX, Zero1, Zero2, JD As Double
            Dim NZeros As Integer
            Dim Observer As New ASCOM.Astrometry.OnSurface
            Dim Retval As New ArrayList
            Dim BodyRises, BodySets As New Generic.List(Of Double)
            Dim BodyInfoMinus1, BodyInfo0, BodyInfoPlus1 As BodyInfo
            Dim TestDate As Date

            DoesRise = False
            DoesSet = False

            Try
                TestDate = Date.Parse(Month & "/" & Day & "/" & Year, System.Globalization.CultureInfo.InvariantCulture) ' Test whether this is a valid date e.g is not the 31st of February
            Catch ex As FormatException ' Catch case where day exceeds the maximum number of days in the month
                Throw New ASCOM.InvalidValueException("Day or Month", Day.ToString & " " & Month.ToString & " " & Year.ToString, "Day must not exceed the number of days in the month")
            Catch ex As Exception ' Throw all other exceptions as they are are received
                TL.LogMessageCrLf("EventTimes", ex.ToString)
                Throw
            End Try

            ' Calculate Julian date in the local timezone
            JD = Nov31.JulianDate(CShort(Year), CShort(Month), CShort(Day), 0.0) - SiteTimeZone / 24.0

            ' Initialise observer structure and calculate the refraction at the hozrizon
            Observer.Latitude = SiteLatitude
            Observer.Longitude = SiteLongitude
            RefractionCorrection = Nov31.Refract(Observer, ASCOM.Astrometry.RefractionOption.StandardRefraction, 90.0)

            ' Iterate over the day in two hour periods

            ' Start at 01:00 as the centre time i.e. then time range will be 00:00 to 02:00
            CentreTime = 1.0

            Do
                'Calculate body positional information
                BodyInfoMinus1 = BodyAltitude(TypeofEvent, JD, CentreTime - 1, SiteLatitude, SiteLongitude)
                BodyInfo0 = BodyAltitude(TypeofEvent, JD, CentreTime, SiteLatitude, SiteLongitude)
                BodyInfoPlus1 = BodyAltitude(TypeofEvent, JD, CentreTime + 1, SiteLatitude, SiteLongitude)

                'Correct alititude for body's apparent size, parallax, required distance below horizon and refraction
                Select Case TypeofEvent
                    Case EventType.MoonRiseMoonSet
                        ' Parallax and apparent size are dynamically calculated for the Moon because it is so close and does not transcribe a circular orbit
                        AltitiudeMinus1 = BodyInfoMinus1.Altitude - EARTH_RADIUS * RAD2DEG / BodyInfoMinus1.Distance + BodyInfoMinus1.Radius * RAD2DEG / BodyInfoMinus1.Distance + RefractionCorrection
                        Altitiude0 = BodyInfo0.Altitude - EARTH_RADIUS * RAD2DEG / BodyInfo0.Distance + BodyInfo0.Radius * RAD2DEG / BodyInfo0.Distance + RefractionCorrection
                        AltitiudePlus1 = BodyInfoPlus1.Altitude - EARTH_RADIUS * RAD2DEG / BodyInfoPlus1.Distance + BodyInfoPlus1.Radius * RAD2DEG / BodyInfoPlus1.Distance + RefractionCorrection
                    Case EventType.SunRiseSunset
                        AltitiudeMinus1 = BodyInfoMinus1.Altitude - SUN_RISE
                        Altitiude0 = BodyInfo0.Altitude - SUN_RISE
                        AltitiudePlus1 = BodyInfoPlus1.Altitude - SUN_RISE
                    Case EventType.CivilTwilight
                        AltitiudeMinus1 = BodyInfoMinus1.Altitude - CIVIL_TWIGHLIGHT
                        Altitiude0 = BodyInfo0.Altitude - CIVIL_TWIGHLIGHT
                        AltitiudePlus1 = BodyInfoPlus1.Altitude - CIVIL_TWIGHLIGHT
                    Case EventType.NauticalTwilight
                        AltitiudeMinus1 = BodyInfoMinus1.Altitude - NAUTICAL_TWIGHLIGHT
                        Altitiude0 = BodyInfo0.Altitude - NAUTICAL_TWIGHLIGHT
                        AltitiudePlus1 = BodyInfoPlus1.Altitude - NAUTICAL_TWIGHLIGHT
                    Case EventType.AmateurAstronomicalTwilight
                        AltitiudeMinus1 = BodyInfoMinus1.Altitude - AMATEUR_ASRONOMICAL_TWIGHLIGHT
                        Altitiude0 = BodyInfo0.Altitude - AMATEUR_ASRONOMICAL_TWIGHLIGHT
                        AltitiudePlus1 = BodyInfoPlus1.Altitude - AMATEUR_ASRONOMICAL_TWIGHLIGHT
                    Case EventType.AstronomicalTwilight
                        AltitiudeMinus1 = BodyInfoMinus1.Altitude - ASTRONOMICAL_TWIGHLIGHT
                        Altitiude0 = BodyInfo0.Altitude - ASTRONOMICAL_TWIGHLIGHT
                        AltitiudePlus1 = BodyInfoPlus1.Altitude - ASTRONOMICAL_TWIGHLIGHT
                    Case Else ' Planets so correct for radius of plant and refraction
                        AltitiudeMinus1 = BodyInfoMinus1.Altitude + RefractionCorrection + RAD2DEG * BodyInfo0.Radius / BodyInfo0.Distance
                        Altitiude0 = BodyInfo0.Altitude + RefractionCorrection + RAD2DEG * BodyInfo0.Radius / BodyInfo0.Distance
                        AltitiudePlus1 = BodyInfoPlus1.Altitude + RefractionCorrection + RAD2DEG * BodyInfo0.Radius / BodyInfo0.Distance
                End Select

                If CentreTime = 1.0 Then
                    If AltitiudeMinus1 < 0 Then
                        AboveHorizon = False
                    Else
                        AboveHorizon = True
                    End If
                End If

                ' Assess quadratic equation
                c = Altitiude0
                b = 0.5 * (AltitiudePlus1 - AltitiudeMinus1)
                a = 0.5 * (AltitiudePlus1 + AltitiudeMinus1) - Altitiude0

                XSymmetry = -b / (2.0 * a)
                YExtreme = (a * XSymmetry + b) * XSymmetry + c
                Discriminant = (b * b) - (4.0 * a * c)

                DeltaX = Double.NaN
                Zero1 = Double.NaN
                Zero2 = Double.NaN
                NZeros = 0

                If Discriminant > 0.0 Then                 'there are zeros
                    DeltaX = 0.5 * Math.Sqrt(Discriminant) / Math.Abs(a)
                    Zero1 = XSymmetry - DeltaX
                    Zero2 = XSymmetry + DeltaX
                    If (Math.Abs(Zero1) <= 1.0) Then NZeros = NZeros + 1 'This zero is in interval
                    If (Math.Abs(Zero2) <= 1.0) Then NZeros = NZeros + 1 'This zero is in interval

                    If (Zero1 < -1.0) Then Zero1 = Zero2
                End If

                Select Case NZeros
                    'cases depend on values of discriminant - inner part of STEP 4
                    Case 0 'nothing  - go to next time slot
                    Case 1                      ' simple rise / set event
                        If (AltitiudeMinus1 < 0.0) Then       ' The body is set at start of event so this must be a rising event
                            DoesRise = True
                            BodyRises.Add(CentreTime + Zero1)
                        Else                    ' must be setting
                            DoesSet = True
                            BodySets.Add(CentreTime + Zero1)
                        End If
                    Case 2                      ' rises and sets within interval
                        If (AltitiudeMinus1 < 0.0) Then ' The body is set at start of event so it must rise first then set
                            BodyRises.Add(CentreTime + Zero1)
                            BodySets.Add(CentreTime + Zero2)
                        Else                    ' The body is risen at the start of the event so it must set first then rise
                            BodyRises.Add(CentreTime + Zero2)
                            BodySets.Add(CentreTime + Zero1)
                        End If
                        DoesRise = True
                        DoesSet = True
                        'Zero2 = 1
                End Select
                CentreTime += 2.0 ' Increment by 2 hours to get the next 2 hour slot in the day

            Loop Until (DoesRise And DoesSet And (Math.Abs(SiteLatitude) < 60.0)) Or (CentreTime = 25.0)

            Retval.Add(AboveHorizon) 'Add above horizon at midnight flag
            Retval.Add(BodyRises.Count) ' Add the number of bodyrises
            Retval.Add(BodySets.Count) ' Add the number of bodysets

            For Each BodyRise As Double In BodyRises ' Add the list of moonrises
                Retval.Add(BodyRise)
            Next

            For Each BodySet As Double In BodySets ' Add the list of moonsets
                Retval.Add(BodySet)
            Next

            Return Retval
        End Function

        ''' <summary>
        ''' Returns the altitude of the body given the input parameters
        ''' </summary>
        ''' <param name="TypeOfEvent">Type of event to be calaculated</param>
        ''' <param name="JD">UTC Julian date</param>
        ''' <param name="Hour">Hour of Julian day</param>
        ''' <param name="Latitude">Site Latitude</param>
        ''' <param name="Longitude">Site Longitude</param>
        ''' <returns>The altitude of the body (degrees)</returns>
        ''' <remarks></remarks>
        Private Function BodyAltitude(TypeOfEvent As EventType, JD As Double, Hour As Double, Latitude As Double, Longitude As Double) As BodyInfo
            Dim Instant, Tau, Gmst, DeltaT As Double
            Dim rc As Short
            Dim Obj3 As New ASCOM.Astrometry.Object3
            Dim Location As New ASCOM.Astrometry.OnSurface
            Dim Cat As New ASCOM.Astrometry.CatEntry3
            Dim SkyPosition As New ASCOM.Astrometry.SkyPos
            Dim Obs As New ASCOM.Astrometry.Observer
            Dim Retval As New BodyInfo

            Instant = JD + Hour / 24.0 ' Add the hour to the whole Julian day number
            DeltaT = DeltaTCalc(JD)

            Select Case TypeOfEvent
                Case EventType.MercuryRiseSet
                    Obj3.Name = "Mercury" : Obj3.Number = ASCOM.Astrometry.Body.Mercury
                Case EventType.VenusRiseSet
                    Obj3.Name = "Venus" : Obj3.Number = ASCOM.Astrometry.Body.Venus
                Case EventType.MarsRiseSet
                    Obj3.Name = "Mars" : Obj3.Number = ASCOM.Astrometry.Body.Mars
                Case EventType.JupiterRiseSet
                    Obj3.Name = "Jupiter" : Obj3.Number = ASCOM.Astrometry.Body.Jupiter
                Case EventType.SaturnRiseSet
                    Obj3.Name = "Saturn" : Obj3.Number = ASCOM.Astrometry.Body.Saturn
                Case EventType.UranusRiseSet
                    Obj3.Name = "Uranus" : Obj3.Number = ASCOM.Astrometry.Body.Uranus
                Case EventType.NeptuneRiseSet
                    Obj3.Name = "Neptune" : Obj3.Number = ASCOM.Astrometry.Body.Neptune
                Case EventType.PlutoRiseSet
                    Obj3.Name = "Pluto" : Obj3.Number = ASCOM.Astrometry.Body.Pluto
                Case EventType.MoonRiseMoonSet
                    Obj3.Name = "Moon" : Obj3.Number = ASCOM.Astrometry.Body.Moon
                Case EventType.SunRiseSunset, EventType.AmateurAstronomicalTwilight, EventType.AstronomicalTwilight, EventType.CivilTwilight, EventType.NauticalTwilight
                    Obj3.Name = "Sun" : Obj3.Number = ASCOM.Astrometry.Body.Sun
                Case Else
                    Throw New ASCOM.InvalidValueException("TypeOfEvent", TypeOfEvent.ToString, "Unknown type of event")
            End Select

            Obj3.Star = Cat
            Obj3.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon

            Obs.OnSurf = Location
            Obs.Where = ASCOM.Astrometry.ObserverLocation.EarthGeoCenter

            Nov31.Place(Instant + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, ASCOM.Astrometry.CoordSys.EquinoxOfDate, ASCOM.Astrometry.Accuracy.Full, SkyPosition)
            Retval.Distance = SkyPosition.Dis * AU2KILOMETRE ' Distance is in AU so save it in km

            rc = Nov31.SiderealTime(Instant, 0.0, DeltaT, ASCOM.Astrometry.GstType.GreenwichApparentSiderealTime, ASCOM.Astrometry.Method.EquinoxBased, ASCOM.Astrometry.Accuracy.Full, Gmst)

            Tau = HOURS2DEG * (Range(Gmst + Longitude * DEG2HOURS, 0, True, 24.0, False) - SkyPosition.RA) ' East longitude is  positive
            Retval.Altitude = Math.Asin(Math.Sin(Latitude * DEG2RAD) * Math.Sin(SkyPosition.Dec * DEG2RAD) + _
                                        Math.Cos(Latitude * DEG2RAD) * Math.Cos(SkyPosition.Dec * DEG2RAD) * Math.Cos(Tau * DEG2RAD)) * RAD2DEG

            Select Case TypeOfEvent
                Case EventType.MercuryRiseSet
                    Retval.Radius = MERCURY_RADIUS ' km
                Case EventType.VenusRiseSet
                    Retval.Radius = VENUS_RADIUS ' km
                Case EventType.MarsRiseSet
                    Retval.Radius = MARS_RADIUS ' km
                Case EventType.JupiterRiseSet
                    Retval.Radius = JUPITER_RADIUS ' km
                Case EventType.SaturnRiseSet
                    Retval.Radius = SATURN_RADIUS ' km
                Case EventType.UranusRiseSet
                    Retval.Radius = URANUS_RADIUS ' km
                Case EventType.NeptuneRiseSet
                    Retval.Radius = NEPTUNE_RADIUS ' km
                Case EventType.PlutoRiseSet
                    Retval.Radius = PLUTO_RADIUS ' km
                Case EventType.MoonRiseMoonSet
                    Retval.Radius = MOON_RADIUS ' km
                Case Else
                    Retval.Radius = SUN_RADIUS ' km
            End Select

            Return Retval
        End Function

        ''' <summary>
        ''' Returns the fraction of the Moon's surface that is illuminated 
        ''' </summary>
        ''' <param name="JD">Julian day (UTC) for which the Moon illumination is required</param>
        ''' <returns>Percentage illumination of the Moon</returns>
        ''' <remarks> The algorithm used is that given in Astronomical Algorithms (Second Edition, Corrected to August 2009) 
        ''' Chapter 48 p345 by Jean Meeus (Willmann-Bell 1991). The Sun and Moon positions are calculated by high precision NOVAS 3.1 library using JPL DE 421 ephemeredes.
        ''' </remarks>
        Public Function MoonIllumination(JD As Double) As Double Implements IAstroUtils.MoonIllumination
            Dim Obj3 As New ASCOM.Astrometry.Object3
            Dim Location As New ASCOM.Astrometry.OnSurface
            Dim Cat As New ASCOM.Astrometry.CatEntry3
            Dim SunPosition, MoonPosition As New ASCOM.Astrometry.SkyPos
            Dim Obs As New ASCOM.Astrometry.Observer
            Dim Phi, Inc, k, DeltaT As Double

            DeltaT = DeltaTCalc(JD)

            ' Calculate Moon RA, Dec and distance
            Obj3.Name = "Moon"
            Obj3.Number = ASCOM.Astrometry.Body.Moon
            Obj3.Star = Cat
            Obj3.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon

            Obs.OnSurf = Location
            Obs.Where = ASCOM.Astrometry.ObserverLocation.EarthGeoCenter

            Nov31.Place(JD + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, ASCOM.Astrometry.CoordSys.EquinoxOfDate, ASCOM.Astrometry.Accuracy.Full, MoonPosition)

            'Calculate Sun RA, Dec and distance
            Obj3.Name = "Sun"
            Obj3.Number = ASCOM.Astrometry.Body.Sun
            Obj3.Star = Cat
            Obj3.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon

            Nov31.Place(JD + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, ASCOM.Astrometry.CoordSys.EquinoxOfDate, ASCOM.Astrometry.Accuracy.Full, SunPosition)

            ' Calculate geocentriic elongation of the Moon
            Phi = Math.Acos(Math.Sin(SunPosition.Dec * DEG2RAD) * Math.Sin(MoonPosition.Dec * DEG2RAD) + _
                            Math.Cos(SunPosition.Dec * DEG2RAD) * Math.Cos(MoonPosition.Dec * DEG2RAD) * Math.Cos((SunPosition.RA - MoonPosition.RA) * HOURS2DEG * DEG2RAD))

            'Calculate the phase angle of the Moon
            Inc = Math.Atan2(SunPosition.Dis * Math.Sin(Phi), MoonPosition.Dis - SunPosition.Dis * Math.Cos(Phi))

            ' Calculate the illuminated fraction of the Moon's disc
            k = (1.0 + Math.Cos(Inc)) / 2.0

            Return k
        End Function

        ''' <summary>
        ''' Returns the Moon phase as an angle
        ''' </summary>
        ''' <param name="JD">Julian day (UTC) for which the Moon phase is required</param>
        ''' <returns>Moon phase as an angle between -180.0 amd +180.0 (see Remarks for further description)</returns>
        ''' <remarks>To allow maximum freedom in displaying the Moon phase, this function returns the excess of the apparent geocentric longitude
        ''' of the Moon over the apparent geocentric longitude of the Sun, expressed as an angle in the range -180.0 to +180.0 degrees.
        ''' This definition is taken from Astronomical Algorithms (Second Edition, Corrected to August 2009) Chapter 49 p349
        ''' by Jean Meeus (Willmann-Bell 1991).
        ''' <para>The frequently used eight phase description for phases of the Moon can be easily constructed from the results of this function
        ''' using logic similar to the following:
        ''' <code>
        '''Select Case MoonPhase
        '''     Case -180.0 To -135.0
        '''         Phase = "Full Moon"
        '''     Case -135.0 To -90.0
        '''         Phase = "Waning Gibbous"
        '''     Case -90.0 To -45.0
        '''         Phase = "Last Quarter"
        '''     Case -45.0 To 0.0
        '''         Phase = "Waning Crescent"
        '''     Case 0.0 To 45.0
        '''         Phase = "New Moon"
        '''     Case 45.0 To 90.0
        '''         Phase = "Waxing Crescent"
        '''     Case 90.0 To 135.0
        '''         Phase = "First Quarter"
        '''     Case 135.0 To 180.0
        '''         Phase = "Waxing Gibbous"
        '''End Select
        ''' </code></para>
        ''' <para>Other representations can be easily constructed by changing the angle ranges and text descriptors as desired. The result range -180 to +180
        ''' was chosen so that negative values represent the Moon waning and positive values represent the Moon waxing.</para>
        '''</remarks>
        Public Function MoonPhase(JD As Double) As Double Implements IAstroUtils.MoonPhase
            Dim Obj3 As New ASCOM.Astrometry.Object3
            Dim Location As New ASCOM.Astrometry.OnSurface
            Dim Cat As New ASCOM.Astrometry.CatEntry3
            Dim SunPosition, MoonPosition As New ASCOM.Astrometry.SkyPos
            Dim Obs As New ASCOM.Astrometry.Observer
            Dim PositionAngle, DeltaT As Double

            DeltaT = DeltaTCalc(JD)

            ' Calculate Moon RA, Dec and distance
            Obj3.Name = "Moon"
            Obj3.Number = ASCOM.Astrometry.Body.Moon
            Obj3.Star = Cat
            Obj3.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon

            Obs.OnSurf = Location
            Obs.Where = ASCOM.Astrometry.ObserverLocation.EarthGeoCenter

            Nov31.Place(JD + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, ASCOM.Astrometry.CoordSys.EquinoxOfDate, ASCOM.Astrometry.Accuracy.Full, MoonPosition)

            'Calculate Sun RA, Dec and distance
            Obj3.Name = "Sun"
            Obj3.Number = ASCOM.Astrometry.Body.Sun
            Obj3.Star = Cat
            Obj3.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon

            Nov31.Place(JD + DeltaT * SECONDS2DAYS, Obj3, Obs, DeltaT, ASCOM.Astrometry.CoordSys.EquinoxOfDate, ASCOM.Astrometry.Accuracy.Full, SunPosition)

            'Return the difference between the sun and moon RA's expressed as degrees from -180 to +180
            PositionAngle = Range((MoonPosition.RA - SunPosition.RA) * HOURS2DEG, -180.0, False, 180.0, True)

            Return PositionAngle

        End Function

    End Class
End Namespace
