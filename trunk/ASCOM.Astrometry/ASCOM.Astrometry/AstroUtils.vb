Imports System.Runtime.InteropServices
Imports ASCOM.Utilities

Namespace AstroUtils
    <Guid("5679F94A-D4D1-40D3-A0F8-7CE61100A691"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class AstroUtils
        Implements IAstroUtils, IDisposable

        Private TL As TraceLogger, Utl As Util, Nov3 As NOVAS.NOVAS3

#Region "New and IDisposable Support"
        Sub New()
            TL = New TraceLogger("", "AstroUtils")
            TL.Enabled = True
            Utl = New Util
            Nov3 = New NOVAS.NOVAS3
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
                If Not (Nov3 Is Nothing) Then
                    Nov3.Dispose()
                    Nov3 = Nothing
                End If

            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        ''' <summary>
        ''' Conditions an hour angle to be in the range -12.0 to +12.0 by adding or subtracting 24.0 hours
        ''' </summary>
        ''' <param name="HA">Hour angle to condition</param>
        ''' <returns>Hour angle in the range -12.0 to +12.0</returns>
        ''' <remarks></remarks>
        Public Function ConditionHA(HA As Double) As Double Implements IAstroUtils.ConditionHA
            Dim HAOrg As Double
            HAOrg = HA

            Do
                If HA < -12.0 Then HA += 24.0
                If HA > 12.0 Then HA -= 24.0
            Loop Until (HA >= -12.0) And (HA <= 12.0)
            TL.LogMessage("ConditionHA", "Conditioned HA: " & Utl.HoursToHMS(HAOrg, ":", ":", "", 3) & " to: " & Utl.HoursToHMS(HA, ":", ":", "", 3))
            Return HA
        End Function

        ''' <summary>
        ''' Conditions a Right Ascension value to be in the range 0 to 23.999... hours 
        ''' </summary>
        ''' <param name="RA">Right ascension to be conditioned</param>
        ''' <returns>Right ascension in the range 0 to 23.999...</returns>
        ''' <remarks></remarks>
        Public Function ConditionRA(RA As Double) As Double Implements IAstroUtils.ConditionRA
            Dim RAOrg As Double
            RAOrg = RA

            Do
                If RA < 0.0 Then RA += 24.0
                If RA >= 24.0 Then RA -= 24.0
            Loop Until (RA >= 0.0) And (RA < 24.0)

            TL.LogMessage("ConditionRA", "Conditioned RA: " & Utl.HoursToHMS(RAOrg, ":", ":", "", 3) & " to: " & Utl.HoursToHMS(RA, ":", ":", "", 3))
            Return RA
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
        Public ReadOnly Property JulianDateUtc As Double
            Get
                Dim CurrentDate As Date, JD As Double

                CurrentDate = Date.UtcNow
                JD = Nov3.JulianDate(Convert.ToInt16(CurrentDate.Year), Convert.ToInt16(CurrentDate.Month), Convert.ToInt16(CurrentDate.Day), CurrentDate.TimeOfDay.TotalHours)
                TL.LogMessage("JulianDateUtc", "Returning: " & JD & " at UTC: " & Format(CurrentDate, "dddd dd MMMM yyyy HH:mm:ss.fff"))
                Return JD
            End Get
        End Property

        ''' <summary>
        ''' Current Julian date based on the terrestrial time (TT) time scale
        ''' </summary>
        ''' <DeltaUT1>Current value for Delta-UT1, the difference between UTC and UT1; always in the range -0.9 to +0.9 seconds.
        ''' Use 0.0 if you do not know this value; it varies irregularly throughout the year.</DeltaUT1>
        ''' <returns>Double - Julian date on the UT1 timescale.</returns>
        ''' <remarks>Terrestrial time is calculated as TT = UTC + DeltaUT1 + DeltaT. This value is then converted to a Julian date and returned.
        ''' <para>Forecast values of DUT1 are published by IERS Bulletin A at http://maia.usno.navy.mil/ser7/ser7.dat
        ''' </para></remarks>
        Public Function JulianDateTT(DeltaUT1 As Double) As Double
            Dim UTCDate, UT1Date, TTDate As Date, JD As Double, DeltaTTimespan, DeltaUT1Timespan As TimeSpan

            If (DeltaUT1 < -0.9) Or (DeltaUT1 > 0.9) Then Throw New ASCOM.InvalidValueException("JulianDateUT1", DeltaUT1.ToString, "-0.9 to +0.9")

            UTCDate = Date.UtcNow
            DeltaTTimespan = TimeSpan.FromSeconds(Me.DeltaT) 'Get DeltaT as a timesapn
            DeltaUT1Timespan = TimeSpan.FromSeconds(DeltaUT1) 'Convert DeltaUT to a timesapn
            UT1Date = UTCDate.Add(DeltaUT1Timespan) 'Add delta-ut to UTC to yield UT1
            TTDate = UT1Date.Add(DeltaTTimespan) 'Add delta-t to UT1 to yield TT
            JD = Nov3.JulianDate(Convert.ToInt16(TTDate.Year), Convert.ToInt16(TTDate.Month), Convert.ToInt16(TTDate.Day), TTDate.TimeOfDay.TotalHours)
            TL.LogMessage("JulianDateTT", "Returning: " & JD & "at TT: " & Format(TTDate, "dddd dd MMMM yyyy HH:mm:ss.fff") & ", at UTC: " & Format(UTCDate, "dddd dd MMMM yyyy HH:mm:ss.fff"))
            Return JD
        End Function

        ''' <summary>
        ''' Current Julian date based on the UT1 time scale
        ''' </summary>
        ''' <DeltaUT>Current value for Delta-UT1, the difference between UTC and UT1; always in the range -0.9 to +0.9 seconds.
        ''' Use 0.0 if you do not know this value; it varies irregularly throughout the year.</DeltaUT>
        ''' <returns>Double - Julian date on the UT1 timescale.</returns>
        ''' <remarks>UT1 time is calculated as UT1 = UTC + DeltaUT1. This value is then converted to a Julian date and returned.
        ''' <para>Forecast values of DUT1 are published by IERS Bulletin A at http://maia.usno.navy.mil/ser7/ser7.dat
        ''' </para></remarks>
        Public Function JulianDateUT1(DeltaUT1 As Double) As Double
            Dim UTCDate, UT1Date As Date, JD As Double, DeltaUT1Timespan As TimeSpan

            If (DeltaUT1 < -0.9) Or (DeltaUT1 > 0.9) Then Throw New ASCOM.InvalidValueException("JulianDateUT1", DeltaUT1.ToString, "-0.9 to +0.9")

            UTCDate = Date.UtcNow
            DeltaUT1Timespan = TimeSpan.FromSeconds(DeltaUT1) 'Convert DeltaUT to a timesapn
            UT1Date = UTCDate.Add(DeltaUT1Timespan) 'Add delta-ut to UTC to yield UT1

            JD = Nov3.JulianDate(Convert.ToInt16(UT1Date.Year), Convert.ToInt16(UT1Date.Month), Convert.ToInt16(UT1Date.Day), UT1Date.TimeOfDay.TotalHours)
            TL.LogMessage("JulianDateUT1", "Returning: " & JD & "at UT1: " & Format(UT1Date, "dddd dd MMMM yyyy HH:mm:ss.fff") & ", at UTC: " & Format(UTCDate, "dddd dd MMMM yyyy HH:mm:ss.fff"))
            Return JD
        End Function

    End Class
End Namespace
