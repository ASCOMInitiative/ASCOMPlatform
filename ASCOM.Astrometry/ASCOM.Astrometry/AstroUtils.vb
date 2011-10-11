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

        Private TL As TraceLogger, Utl As Util, Nov31 As NOVAS.NOVAS31
        Const MJDBase As Double = 2400000.5 'This is the offset of Modified Julian dates from true Julian dates

#Region "New and IDisposable Support"
        Sub New()
            TL = New TraceLogger("", "AstroUtils")
            TL.Enabled = True
            Utl = New Util
            Nov31 = New NOVAS.NOVAS31
            TL.LogMessage("New", "AstroUtils created Utilities component OK")
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
        ''' Use 0.0 if you do not know this value; it varies irregularly throughout the year.</param>
        ''' <returns>Double - Julian date on the UT1 timescale.</returns>
        ''' <remarks>Terrestrial time is calculated as TT = UTC + DeltaUT1 + DeltaT. This value is then converted to a Julian date and returned.
        ''' <para>Forecast values of DUT1 are published by IERS Bulletin A at http://maia.usno.navy.mil/ser7/ser7.dat
        ''' </para></remarks>
        Public Function JulianDateTT(DeltaUT1 As Double) As Double Implements IAstroUtils.JulianDateTT
            Dim UTCDate, UT1Date, TTDate As Date, JD As Double, DeltaTTimespan, DeltaUT1Timespan As TimeSpan

            If (DeltaUT1 < -0.9) Or (DeltaUT1 > 0.9) Then Throw New ASCOM.InvalidValueException("JulianDateUT1", DeltaUT1.ToString, "-0.9 to +0.9")

            UTCDate = Date.UtcNow
            DeltaTTimespan = TimeSpan.FromSeconds(Me.DeltaT) 'Get DeltaT as a timesapn
            DeltaUT1Timespan = TimeSpan.FromSeconds(DeltaUT1) 'Convert DeltaUT to a timesapn
            UT1Date = UTCDate.Add(DeltaUT1Timespan) 'Add delta-ut to UTC to yield UT1
            TTDate = UT1Date.Add(DeltaTTimespan) 'Add delta-t to UT1 to yield TT
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
        ''' <remarks>UT1 time is calculated as UT1 = UTC + DeltaUT1. This value is then converted to a Julian date and returned.
        ''' <para>Forecast values of DUT1 are published by IERS Bulletin A at http://maia.usno.navy.mil/ser7/ser7.dat
        ''' </para></remarks>
        Public Function JulianDateUT1(DeltaUT1 As Double) As Double Implements IAstroUtils.JulianDateUT1
            Dim UTCDate, UT1Date As Date, JD As Double, DeltaUT1Timespan As TimeSpan

            If (DeltaUT1 < -0.9) Or (DeltaUT1 > 0.9) Then Throw New ASCOM.InvalidValueException("JulianDateUT1", DeltaUT1.ToString, "-0.9 to +0.9")

            UTCDate = Date.UtcNow
            DeltaUT1Timespan = TimeSpan.FromSeconds(DeltaUT1) 'Convert DeltaUT to a timesapn
            UT1Date = UTCDate.Add(DeltaUT1Timespan) 'Add delta-ut to UTC to yield UT1

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
            MJD = JD - MJDBase
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

            JulianDate = Utl.DateJulianToLocal(MJD + MJDBase)
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

            JulianDate = Utl.DateJulianToLocal(MJD + MJDBase)

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

    End Class
End Namespace
