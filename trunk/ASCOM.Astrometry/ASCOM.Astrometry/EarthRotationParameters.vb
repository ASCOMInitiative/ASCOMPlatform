Imports ASCOM.Utilities
Imports System.Globalization
Imports System.IO

Public Class EarthRotationParameters : Implements IDisposable

#Region "Variables and constants"

    Private UpdateTypeValue As String
    Private ManualDeltaUT1Value As Double
    Private ManualLeapSecondsValue As Double
    Private AutomaticLeapSecondsValue As Double
    Private AutomaticLeapSecondsStringValue As String
    Private NextLeapSecondsValue As Double
    Private NextLeapSecondsStringValue As String
    Private NextLeapSecondsDateValue As DateTime
    Private NextLeapSecondsDateStringValue As String
    Private DownloadTaskDataSourceValue As String
    Private DownloadTaskTimeOutValue As Double
    Private DownloadTaskRepeatFrequencyValue As String
    Private DownloadTaskTraceEnabledValue As Boolean
    Private DownloadTaskTracePathValue As String
    Private DownloadTaskScheduledTimeValue As DateTime
    Private EarthRotationDataLastUpdatedValue As String
    Private DownloadTaskCultureValue As CultureInfo

    Private TL As TraceLogger
    Private DebugTraceEnabled As Boolean = True
    Private profile As RegistryAccess
    Private disposedValue As Boolean ' To detect redundant calls

    ' Lock objects and caching control variables
    Private Shared LeapSecondLockObject As Object
    Private Shared LastLeapSecondJulianDate As Double = DOUBLE_VALUE_NOT_AVAILABLE
    Private Shared LastLeapSecondValue As Double

    Private Shared DeltaTLockObject As Object
    Private Shared LastDeltaTJulianDate As Double = DOUBLE_VALUE_NOT_AVAILABLE
    Private Shared LastDeltaTValue As Double

    Private Shared DeltaUT1LockObject As Object
    Private Shared LastDeltaUT1JulianDate As Double = DOUBLE_VALUE_NOT_AVAILABLE
    Private Shared LastDeltaUT1Value As Double

    ' Constants to reference columns in the DownloadedLeapSecondValues array 
    Private Const JULIAN_DATE As Integer = 0
    Private Const YEAR As Integer = 1
    Private Const MONTH As Integer = 2
    Private Const LEAP_SECONDS As Integer = 3

    ' Downloaded leap second data. Format:  JulianDate, Year, Month, Day LeapSeconds
    Private DownloadedLeapSecondValues As SortedList(Of Double, Double) = New SortedList(Of Double, Double) ' Initialise to an empty list


#End Region

#Region "New and IDisposable Support"
    Public Sub New()
        Me.New(Nothing) ' Call the main initialisation routine with no trace logger refernce
    End Sub

    Public Sub New(SuppliedTraceLogger As TraceLogger)
        TL = SuppliedTraceLogger ' Save the reference to the caller's trace logger so we can write to it
        profile = New RegistryAccess()

        ' Initialise lock objects
        LeapSecondLockObject = New Object()
        DeltaTLockObject = New Object()
        DeltaUT1LockObject = New Object()

        RefreshState()
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                Try : profile.Dispose() : Catch : End Try
            End If
        End If
        disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
    End Sub
#End Region

#Region "Public properties"

    Public Property DownloadTaskScheduledTime As DateTime
        Get
            Return DownloadTaskScheduledTimeValue
        End Get
        Set
            DownloadTaskScheduledTimeValue = Value
            LogDebugMessage("DownloadTaskRunTime Write", String.Format("DownloadTaskRunTime = {0}", DownloadTaskScheduledTimeValue.ToString(DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue)))
            profile.WriteProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_SCHEDULED_TIME_VALUE_NAME, DownloadTaskScheduledTimeValue.ToString(DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue))
        End Set
    End Property

    Public Property UpdateType As String
        Get
            Return UpdateTypeValue
        End Get
        Set
            UpdateTypeValue = Value
            LogDebugMessage("UpdateType Write", String.Format("UpdateTypeValue = {0}", UpdateTypeValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, UPDATE_TYPE_VALUE_NAME, UpdateTypeValue)
        End Set
    End Property

    Public Property ManualDeltaUT1 As Double
        Get
            Return ManualDeltaUT1Value
        End Get
        Set
            ManualDeltaUT1Value = Value
            LogDebugMessage("ManualDeltaUT1 Write", String.Format("ManualDeltaUT1Value = {0}", ManualDeltaUT1Value))
            profile.WriteProfile(ASTROMETRY_SUBKEY, MANUAL_DELTAUT1_VALUE_NAME, ManualDeltaUT1Value.ToString(DownloadTaskCultureValue))
        End Set
    End Property

    Public Property ManualLeapSeconds As Double
        Get
            Return ManualLeapSecondsValue
        End Get
        Set
            ManualLeapSecondsValue = Value
            LogDebugMessage("ManualLeapSeconds Write", String.Format("ManualTaiUtcOffsetValue = {0}", ManualLeapSecondsValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, MANUAL_LEAP_SECONDS_VALUENAME, ManualLeapSecondsValue.ToString(DownloadTaskCultureValue))
        End Set
    End Property

    Public ReadOnly Property AutomaticLeapSeconds As Double
        Get
            Return AutomaticLeapSecondsValue
        End Get
    End Property

    Public Property AutomaticLeapSecondsString As String
        Get
            Return AutomaticLeapSecondsStringValue
        End Get
        Set
            AutomaticLeapSecondsStringValue = Value
            LogDebugMessage("AutomaticLeapSeconds Write", String.Format("AutomaticLeapSeconds = {0}", AutomaticLeapSecondsStringValue))
            profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.AUTOMATIC_LEAP_SECONDS_VALUENAME, AutomaticLeapSecondsStringValue)
        End Set
    End Property

    Public ReadOnly Property NextLeapSeconds As Double
        Get
            Return NextLeapSecondsValue
        End Get
    End Property

    Public Property NextLeapSecondsString As String
        Get
            Return NextLeapSecondsStringValue
        End Get
        Set
            NextLeapSecondsStringValue = Value
            LogDebugMessage("NextLeapSeconds Write", String.Format("NextLeapSeconds = {0}", NextLeapSecondsStringValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, NEXT_LEAP_SECONDS_VALUENAME, NextLeapSecondsStringValue)
        End Set
    End Property

    Public ReadOnly Property NextLeapSecondsDate As DateTime
        Get
            Return NextLeapSecondsDateValue
        End Get
    End Property

    Public Property NextLeapSecondsDateString As String
        Get
            Return NextLeapSecondsDateStringValue
        End Get
        Set
            NextLeapSecondsDateStringValue = Value
            LogDebugMessage("NextLeapSecondsDate Write", String.Format("NextLeapSecondsDate = {0}", NextLeapSecondsDateStringValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, NEXT_LEAP_SECONDS_DATE_VALUENAME, NextLeapSecondsDateStringValue)
        End Set
    End Property

    Public Property DownloadTaskDataSource As String
        Get
            Return DownloadTaskDataSourceValue
        End Get
        Set
            DownloadTaskDataSourceValue = Value
            LogDebugMessage("DownloadTaskDataSource Write", String.Format("DownloadTaskDataSourceValue = {0}", DownloadTaskDataSourceValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_DATA_SOURCE_VALUE_NAME, DownloadTaskDataSourceValue)
        End Set
    End Property

    Public Property DownloadTaskTimeOut As Double
        Get
            Return DownloadTaskTimeOutValue
        End Get
        Set
            DownloadTaskTimeOutValue = Value
            LogDebugMessage("DownloadTaskTimeOut Write", String.Format("DownloadTaskTimeOutValue = {0}", DownloadTaskTimeOutValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_TIMEOUT_VALUE_NAME, DownloadTaskTimeOutValue.ToString(DownloadTaskCultureValue))
        End Set
    End Property

    Public Property DownloadTaskRepeatFrequency As String
        Get
            Return DownloadTaskRepeatFrequencyValue
        End Get
        Set
            DownloadTaskRepeatFrequencyValue = Value
            LogDebugMessage("DownloadTaskRepeatFrequency Write", String.Format("DownloadTaskRepeatFrequencyValue = {0}", DownloadTaskRepeatFrequencyValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_REPEAT_FREQUENCY_VALUE_NAME, DownloadTaskRepeatFrequencyValue)
        End Set
    End Property

    Public Property DownloadTaskTraceEnabled As Boolean
        Get
            Return DownloadTaskTraceEnabledValue
        End Get
        Set
            DownloadTaskTraceEnabledValue = Value
            LogDebugMessage("DownloadTaskTraceEnabled Write", String.Format("DownloadTaskTraceEnabledValue = {0}", DownloadTaskTraceEnabledValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_TRACE_ENABLED_VALUE_NAME, DownloadTaskTraceEnabledValue.ToString(DownloadTaskCultureValue))
        End Set
    End Property

    Public Property DownloadTaskTracePath As String
        Get
            Return DownloadTaskTracePathValue
        End Get
        Set
            DownloadTaskTracePathValue = Value
            LogDebugMessage("DownloadTaskTracePath Write", String.Format("DownloadTaskTracePathValue = {0}", DownloadTaskTracePathValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_TRACE_PATH_VALUE_NAME, DownloadTaskTracePathValue)
        End Set
    End Property

    Public Property EarthRotationDataLastUpdatedString As String
        Get
            Return EarthRotationDataLastUpdatedValue
        End Get
        Set
            EarthRotationDataLastUpdatedValue = Value
            LogDebugMessage("EarthRotationDataLastUpdated Write", String.Format("EarthRotationDataLastUpdatedValue = {0}", EarthRotationDataLastUpdatedValue))
            profile.WriteProfile(ASTROMETRY_SUBKEY, EARTH_ROTATION_DATA_LAST_UPDATED_VALUE_NAME, EarthRotationDataLastUpdatedValue)
        End Set
    End Property

    Public Property DownloadTaskCulture As CultureInfo
        Get
            Return DownloadTaskCultureValue
        End Get
        Set
            DownloadTaskCultureValue = Value
            LogDebugMessage("DownloadTaskCulture Write", String.Format("DownloadTaskCultureValue = {0}", DownloadTaskCultureValue.ToString()))
            profile.WriteProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_CULTURE_VALUE_NAME, DownloadTaskCultureValue.Name)
        End Set
    End Property

    ''' <summary>
    ''' Return today's number of leap seconds
    ''' </summary>
    ''' <returns>Current leap seconds as a double</returns>
    Public Overloads Function LeapSeconds() As Double
        Dim CurrentJulianDate As Double
        CurrentJulianDate = DateTime.UtcNow.ToOADate() + OLE_AUTOMATION_JULIAN_DATE_OFFSET ' Calculate today's Julian date

        SyncLock LeapSecondLockObject
            If Math.Truncate(CurrentJulianDate - MODIFIED_JULIAN_DAY_OFFSET) = Math.Truncate(LastLeapSecondJulianDate - MODIFIED_JULIAN_DAY_OFFSET) Then ' Return the cached value if its availahble otherwise calculate it and save the value for the next call
                LogDebugMessage("LeapSeconds", String.Format("Returning cached today's UTC leap second value: {0}", LastLeapSecondValue))
                Return LastLeapSecondValue
            End If
        End SyncLock

        Return LeapSeconds(CurrentJulianDate) ' Return today's number of leap seconds

    End Function

    ''' <summary>
    ''' Return the specified Julian day's number of leap seconds
    ''' </summary>
    ''' <param name="RequiredLeapSecondJulianDate"></param>
    ''' <returns>Leap seconds as a double</returns>
    Public Overloads Function LeapSeconds(RequiredLeapSecondJulianDate As Double) As Double
        Dim EffectiveDate As DateTime, ReturnValue, TodayJulianDate As Double
        Dim ActiveLeapSeconds As SortedList(Of Double, Double) ' Variable to hold either downloaded or built-in leap second values

        SyncLock LeapSecondLockObject
            If Math.Truncate(RequiredLeapSecondJulianDate - MODIFIED_JULIAN_DAY_OFFSET) = Math.Truncate(LastLeapSecondJulianDate - MODIFIED_JULIAN_DAY_OFFSET) Then ' Return the cached value if its availahble otherwise calculate it and save the value for the next call
                LogDebugMessage("LeapSeconds(JD)", String.Format("Returning cached leap second value: {0} for UTC Julian date: {1} ({2})",
                                                                 LastLeapSecondValue,
                                                                 RequiredLeapSecondJulianDate,
                                                                 DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                Return LastLeapSecondValue
            End If
        End SyncLock

        TodayJulianDate = DateTime.UtcNow.ToOADate() + OLE_AUTOMATION_JULIAN_DATE_OFFSET
        If Math.Truncate(RequiredLeapSecondJulianDate - MODIFIED_JULIAN_DAY_OFFSET) = Math.Truncate(TodayJulianDate - MODIFIED_JULIAN_DAY_OFFSET) Then ' Request is for the current day so process using all the options

            'Set the current number of leap seconds once for this instance using manual or automatic values as appropriate
            Select Case UpdateTypeValue
                Case UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
                    ' Approach to returning a leap second value:
                    ' Test whether the Next Leap Second Date is available
                    '     If yes then test whether we are past the next leap second date - measured in UTC time because leap seconds are applied at 00:00:00 UTC. 
                    '         If yes Then test whether the Next Leap Seconds value Is available
                    '             If yes then use it
                    '             If no then fall back To the manual value.
                    '         If no then test whether the Automatic Leap Seconds value is available
                    '             If yes then use it
                    '             If no then fall back To the manual value.
                    '     If no then test whether the Automatic Leap Seconds value is available
                    '         If yes then use it
                    '         If no then fall back To the manual value.
                    If NextLeapSecondsDateValue = DATE_VALUE_NOT_AVAILABLE Then ' A future leap second change date has not been published
                        If AutomaticLeapSecondsValue <> DOUBLE_VALUE_NOT_AVAILABLE Then ' We have a good automatic leap second value so use this
                            ReturnValue = AutomaticLeapSecondsValue
                            LogDebugMessage("LeapSeconds(JD)", String.Format("Automatic leap seconds are required and a valid value is available: {0} for JD {1} ({2})",
                                                                             ReturnValue,
                                                                             RequiredLeapSecondJulianDate,
                                                                             DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                        Else ' We do not have a downloaded leap second value so fall back to the Manual value
                            ReturnValue = ManualLeapSecondsValue
                            LogDebugMessage("LeapSeconds(JD)", String.Format("Automatic leap seconds are required but a valid value is not available - returning the manual leap seconds value instead: {0} for JD {1} ({2})",
                                                                             ReturnValue,
                                                                             RequiredLeapSecondJulianDate,
                                                                             DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                        End If
                    Else ' A future leap second date has been published
                        EffectiveDate = DateTime.UtcNow.Subtract(New TimeSpan(TEST_UTC_DAYS_OFFSET, TEST_UTC_HOURS_OFFSET, TEST_UTC_MINUTES_OFFSET, 0)) ' This is used to support development testing
                        LogDebugMessage("LeapSeconds(JD)", String.Format("Effective date: {0}, NextLeapSecondsDate: {1}",
                                                                         EffectiveDate.ToString(DOWNLOAD_TASK_TIME_FORMAT),
                                                                         NextLeapSecondsDateValue.ToUniversalTime.ToString(DOWNLOAD_TASK_TIME_FORMAT),
                                                                         RequiredLeapSecondJulianDate,
                                                                         DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))

                        If EffectiveDate > NextLeapSecondsDateValue.ToUniversalTime Then ' We are beyond the next leap second implementation date/time so use the next leap second value
                            If NextLeapSecondsValue <> DOUBLE_VALUE_NOT_AVAILABLE Then ' We have a good next leap seconds value so use it
                                ReturnValue = NextLeapSecondsValue
                                LogDebugMessage("LeapSeconds(JD)", String.Format("Automatic leap seconds are required, current time is after the next leap second implementation time and a valid next leap seconds value is available: {0} for JD {1} ({2})",
                                                                                 ReturnValue,
                                                                                 RequiredLeapSecondJulianDate,
                                                                                 DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                            Else ' We don't have a good next leap seconds value so fall back to the manual leap seconds value
                                ReturnValue = ManualLeapSecondsValue
                                LogDebugMessage("LeapSeconds(JD)", String.Format("Automatic leap seconds are required, current time is after the next leap second implementation time but a valid next leap seconds value is not available - returning the manual leap seconds value instead: {0} for JD {1} ({2})",
                                                                                 ReturnValue,
                                                                                 RequiredLeapSecondJulianDate,
                                                                                 DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                            End If
                        Else ' We are not beyond the next leap second implementation date so use the automatic leap second value
                            If AutomaticLeapSecondsValue <> DOUBLE_VALUE_NOT_AVAILABLE Then ' We have a good automatic leap seconds value so use it
                                ReturnValue = AutomaticLeapSecondsValue
                                LogDebugMessage("LeapSeconds(JD)", String.Format("Automatic leap seconds are required, current time is before the next leap second implementation time and a valid automatic leap seconds value is available: {0} for JD {1} ({2})",
                                                                                 ReturnValue,
                                                                                 RequiredLeapSecondJulianDate,
                                                                                 DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                            Else ' We don't have a good automatic leap seconds value so fall back to the manual leap seconds value
                                ReturnValue = ManualLeapSecondsValue
                                LogDebugMessage("LeapSeconds(JD)", String.Format("Automatic leap seconds are required, current time is before the next leap second implementation time but a valid automatic leap seconds value is not available - returning the manual leap seconds value instead: {0} for JD {1} ({2})",
                                                                                 ReturnValue,
                                                                                 RequiredLeapSecondJulianDate,
                                                                                 DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                            End If
                        End If
                    End If
                Case UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1
                    ReturnValue = ManualLeapSecondsValue
                    LogDebugMessage("LeapSeconds(JD)", String.Format("Manual leap seconds and delta UT1 are required, returning the manual leap seconds value: {0} for JD {1} ({2})",
                                                                     ReturnValue,
                                                                     RequiredLeapSecondJulianDate,
                                                                     DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                Case UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1
                    ReturnValue = ManualLeapSecondsValue
                    LogDebugMessage("LeapSeconds(JD)", String.Format("Manual leap seconds and predicted delta UT1 are required, returning the manual leap seconds value: {0} for JD {1} ({2})",
                                                                     ReturnValue,
                                                                     RequiredLeapSecondJulianDate,
                                                                     DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1
                    ' Find the leap second value from the built-in table of historic values
                    For i As Integer = BuiltInLeapSeconds.Count - 1 To 0 Step -1
                        LogDebugMessage("LeapSeconds(JD)", String.Format("Searching built-in JD {0} with leap second: {1}", BuiltInLeapSeconds.Keys(i), BuiltInLeapSeconds.Values(i)))

                        If Math.Truncate(RequiredLeapSecondJulianDate - MODIFIED_JULIAN_DAY_OFFSET) >= Math.Truncate(BuiltInLeapSeconds.Keys(i) - MODIFIED_JULIAN_DAY_OFFSET) Then ' Found a match
                            ReturnValue = BuiltInLeapSeconds.Values(i)
                            LogDebugMessage("LeapSeconds(JD)", String.Format("Found built-in leap second: {0} set on JD {1}", BuiltInLeapSeconds.Values(i), BuiltInLeapSeconds.Keys(i)))
                            Exit For
                        End If
                    Next
                    LogDebugMessage("LeapSeconds(JD)", String.Format("Built-in leap seconds and delta UT1 are required, returning the built-in leap seconds value: {0} for JD {1} ({2})",
                                                                     ReturnValue,
                                                                     RequiredLeapSecondJulianDate,
                                                                     DateTime.FromOADate(RequiredLeapSecondJulianDate - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                Case Else
                    LogDebugMessage("LeapSeconds(JD)", "Unknown UpdateTypeValue: " & UpdateTypeValue)
                    MsgBox("EarthRotationParameters.LeapSeconds(JD) - Unknown UpdateTypeValue: " & UpdateTypeValue)
            End Select

        Else ' Request is not for today so find value from downloaded values, if available, or fall back to built-in values

            If DownloadedLeapSecondValues.Count > 0 Then ' We have downloaded values so use them
                LogDebugMessage("LeapSeconds(JD)", String.Format("Historic leap second value required. Searching in {0} downloaded leap second values.", DownloadedLeapSecondValues.Count))
                ActiveLeapSeconds = DownloadedLeapSecondValues
            Else ' No dowloaded values so fall back to built-in values
                LogDebugMessage("LeapSeconds(JD)", String.Format("Historic leap second value required. Searching in {0} built-in leap second values.", BuiltInLeapSeconds.Count))
                ActiveLeapSeconds = BuiltInLeapSeconds
            End If

            For i As Integer = ActiveLeapSeconds.Count - 1 To 0 Step -1
                LogDebugMessage("LeapSeconds(JD)", String.Format("Searching downloaded JD {0} with leap second: {1}", ActiveLeapSeconds.Keys(i), ActiveLeapSeconds.Values(i)))

                If Math.Truncate(RequiredLeapSecondJulianDate - MODIFIED_JULIAN_DAY_OFFSET) >= Math.Truncate(ActiveLeapSeconds.Keys(i) - MODIFIED_JULIAN_DAY_OFFSET) Then ' Found a match
                    ReturnValue = ActiveLeapSeconds.Values(i)
                    LogDebugMessage("LeapSeconds(JD)", String.Format("Found downloaded leap second: {0} set on JD {1}", ActiveLeapSeconds.Values(i), ActiveLeapSeconds.Keys(i)))
                    Exit For
                End If
            Next
        End If

        SyncLock LeapSecondLockObject
            LastLeapSecondJulianDate = RequiredLeapSecondJulianDate
            LastLeapSecondValue = ReturnValue
            Return ReturnValue ' Return the assigned value
        End SyncLock

    End Function

    ''' <summary>
    ''' Return today's DeltaT value
    ''' </summary>
    ''' <returns>DeltaT value as a double</returns>
    Public Overloads Function DeltaT() As Double
        Dim CurrentJulianDateUTC As Double
        CurrentJulianDateUTC = DateTime.UtcNow.ToOADate() + GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET ' Calculate today's Julian date

        SyncLock DeltaTLockObject
            If Math.Truncate(CurrentJulianDateUTC - MODIFIED_JULIAN_DAY_OFFSET) = Math.Truncate(LastDeltaTJulianDate - MODIFIED_JULIAN_DAY_OFFSET) Then ' Return the cached value if its availahble otherwise calculate it and save the value for the next call
                LogDebugMessage("DeltaT", String.Format("Returning cached today's DeltaT value: {0}", LastDeltaTValue))
                Return LastDeltaTValue
            End If
        End SyncLock

        Return DeltaT(CurrentJulianDateUTC) ' Return today's number of leap seconds

    End Function

    ''' <summary>
    ''' Return the specified Julian day's DeltaT value
    ''' </summary>
    ''' <param name="RequiredDeltaTJulianDateUTC"></param>
    ''' <returns>DeltaT value as a double</returns>
    Public Overloads Function DeltaT(RequiredDeltaTJulianDateUTC As Double) As Double
        Dim DeltaUT1, ReturnValue As Double, UTCDate As DateTime, DeltaUT1ValueName, DeltaUT1String As String

        SyncLock DeltaTLockObject
            If Math.Truncate(RequiredDeltaTJulianDateUTC - MODIFIED_JULIAN_DAY_OFFSET) = Math.Truncate(LastDeltaTJulianDate - MODIFIED_JULIAN_DAY_OFFSET) Then ' Return the cached value if its availahble otherwise calculate it and save the value for the next call
                LogDebugMessage("DeltaT(JD)", String.Format("Returning cached DeltaT value: {0} for UTC Julian date: {1} ({2})",
                                                            LastDeltaTValue,
                                                            RequiredDeltaTJulianDateUTC,
                                                            DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                Return LastDeltaTValue
            End If
        End SyncLock

        ' We don't have a cached value so compute one and save it for the next call
        Select Case UpdateTypeValue
            Case UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1

                ' Approach: calculate DELTA_T as =  CURRENT_LEAP_SECONDS + TT_TAI_OFFSET - DUT1
                ' Determine whether a downloaded DeltaUT1 value exists for the given UTC Julian date then perform the calculation above
                '    if yes then 
                '        Determine whether the value is a valid double number
                '        If it is then 
                '            Test whether the value is in the acceptable range -0.0 to +0.9
                '                If it is then return this value
                '                If not then fall back to the predicted approach
                '        If not then fall back to the predicted approach
                '    if no then fall back to the predicted approach

                UTCDate = DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET) ' Convert the Julian day into a DateTime
                DeltaUT1ValueName = String.Format(DELTAUT1_VALUE_NAME_FORMAT, UTCDate.Year.ToString(DELTAUT1_VALUE_NAME_YEAR_FORMAT), UTCDate.Month.ToString(DELTAUT1_VALUE_NAME_MONTH_FORMAT), UTCDate.Day.ToString(DELTAUT1_VALUE_NAME_DAY_FORMAT))
                DeltaUT1String = profile.GetProfile(GlobalItems.AUTOMATIC_UPDATE_EARTH_ROTATION_DATA_SUBKEY_NAME, DeltaUT1ValueName)
                If DeltaUT1String <> "" Then ' We have got something back from the Profile so test whether it is a valid double number
                    If Double.TryParse(DeltaUT1String, NumberStyles.Float, DownloadTaskCultureValue, DeltaUT1) Then ' We have a valid double number so check that it is the acceptable range
                        If (DeltaUT1 >= GlobalItems.DELTAUT1_LOWER_BOUND) And (DeltaUT1 <= GlobalItems.DELTAUT1_UPPER_BOUND) Then

                            LogDebugMessage("DeltaT(JD)", String.Format("Automatic leap seconds and delta UT1 are required, found a good DeltaUT1 value so returning the calculated DeltaT value for Julian day: {0} ({1})",
                                                                        RequiredDeltaTJulianDateUTC,
                                                                        DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                            ReturnValue = Me.LeapSeconds() + TT_TAI_OFFSET - DeltaUT1 ' Calculate DeltaT using the valid DeltaUT1 value
                            LogDebugMessage("DeltaT(JD)", String.Format("Return value: {0} for Julian day: {1} ({2})",
                                                                        ReturnValue,
                                                                        RequiredDeltaTJulianDateUTC,
                                                                        DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))

                            SyncLock DeltaTLockObject ' Update cache values and return the calculated value
                                LastDeltaTJulianDate = RequiredDeltaTJulianDateUTC
                                LastDeltaTValue = ReturnValue
                                Return ReturnValue
                            End SyncLock

                        Else
                            LogDebugMessage("DeltaT(JD)", String.Format("Automatic leap seconds and delta UT1 are required, but the found DeltaUT1 value {0} from {1} was outside the correct range so falling through to the predicted approach for Julian day: {2} ({3})",
                                                                        DeltaUT1,
                                                                        DeltaUT1ValueName,
                                                                        RequiredDeltaTJulianDateUTC,
                                                                        DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                            Exit Select ' We have a double value that is outside the expected range so fall through to the default predicted approach
                        End If
                    Else
                        LogDebugMessage("DeltaT(JD)", String.Format("Automatic leap seconds and delta UT1 are required, but the Profile value {0} from {1} isn't a number so falling through to the predicted approach for Julian day: {2} ({3})",
                                                                    DeltaUT1String,
                                                                    DeltaUT1ValueName,
                                                                    RequiredDeltaTJulianDateUTC,
                                                                    DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                        Exit Select ' We have a profile value but it isn't a number so fall through to the default predicted approach
                    End If
                Else
                    LogDebugMessage("DeltaT(JD)", String.Format("Automatic leap seconds and delta UT1 are required, but no profile value was found for {0} so falling through to the predicted approach for Julian day: {1} ({2})",
                                                                DeltaUT1ValueName,
                                                                RequiredDeltaTJulianDateUTC,
                                                                DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                    Exit Select ' We have no profile value so fall through to the default predicted approach
                End If

            Case UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1
                ' Approach: calculate DELTA_T as =  CURRENT_LEAP_SECONDS + TT_TAI_OFFSET - DUT1
                ' Determine whether the manual DeltaUT1 value is valid 
                '    if yes then use this value in the equation above
                '    if no then fall back to the predicted approach

                If ManualDeltaUT1Value <> DOUBLE_VALUE_NOT_AVAILABLE Then ' We have a valid manual delta UT1 value so use it 
                    LogDebugMessage("DeltaT(JD)", String.Format("Manual leap seconds and delta UT1 are required, found a good DeltaUT1 value so returning the calculated DeltaT value for Julian day: {0} ({1})",
                                                                RequiredDeltaTJulianDateUTC,
                                                                DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                    ReturnValue = Me.LeapSeconds() + TT_TAI_OFFSET - ManualDeltaUT1Value ' Calculate DeltaT using the valid DeltaUT1 value
                    LogDebugMessage("DeltaT(JD)", String.Format("Return value: {0} for Julian day: {1} ({2})",
                                                                ReturnValue,
                                                                RequiredDeltaTJulianDateUTC,
                                                                DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))

                    SyncLock DeltaTLockObject ' Update cache values and return the calculated value
                        LastDeltaTJulianDate = RequiredDeltaTJulianDateUTC
                        LastDeltaTValue = ReturnValue
                        Return ReturnValue
                    End SyncLock
                Else
                    LogDebugMessage("DeltaT(JD)", String.Format("Manual leap seconds and manual delta UT1 are required, but the DeltaUT1 value is not available or invalid so falling through to the predicted approach for Julian day: {0} ({1})",
                                                                RequiredDeltaTJulianDateUTC,
                                                                DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                    Exit Select
                End If

            Case UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1
                LogDebugMessage("DeltaT(JD)", String.Format("Manual leap seconds and predicted delta UT1 are required, so falling through to the predicted approach for Julian day: {0} ({1})",
                                                            RequiredDeltaTJulianDateUTC,
                                                            DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                Exit Select

            Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1
                LogDebugMessage("DeltaT(JD)", String.Format("Built-in leap seconds and predicted delta UT1 are required, so falling through to the predicted approach for Julian day: {0} ({1})",
                                                            RequiredDeltaTJulianDateUTC,
                                                            DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                Exit Select

            Case Else
                LogDebugMessage("DeltaT(JD)", "Unknown UpdateTypeValue: " & UpdateTypeValue)
                MsgBox("AstroUtils.DeltaT(JD) - Unknown UpdateTypeValue: " & UpdateTypeValue)
        End Select

        ' Calculate the predicted value and return it
        ReturnValue = DeltaTCalc(RequiredDeltaTJulianDateUTC)
        LogDebugMessage("DeltaT(JD)", String.Format("Return value: {0} for Julian day: {1} ({2})",
                                                                ReturnValue,
                                                                RequiredDeltaTJulianDateUTC,
                                                                DateTime.FromOADate(RequiredDeltaTJulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))

        SyncLock DeltaTLockObject
            LastDeltaTJulianDate = RequiredDeltaTJulianDateUTC
            LastDeltaTValue = ReturnValue
            Return ReturnValue ' Return the assigned value
        End SyncLock

    End Function

    ''' <summary>
    ''' Return today's DeltaUT1 value
    ''' </summary>
    ''' <returns>DeltaUT1 value as a double</returns>
    Public Overloads Function DeltaUT1() As Double
        Dim CurrentJulianDateUTC As Double
        CurrentJulianDateUTC = DateTime.UtcNow.ToOADate() + GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET ' Calculate today's Julian date

        SyncLock DeltaUT1LockObject
            If Math.Truncate(CurrentJulianDateUTC - MODIFIED_JULIAN_DAY_OFFSET) = Math.Truncate(LastDeltaUT1JulianDate - MODIFIED_JULIAN_DAY_OFFSET) Then ' Return the cached value if its availahble otherwise calculate it and save the value for the next call
                LogDebugMessage("DeltaUT1", String.Format("Returning cached today's DeltaUT1 value: {0}", LastDeltaUT1Value))
                Return LastDeltaUT1Value
            End If
        End SyncLock

        Return DeltaUT1(CurrentJulianDateUTC) ' Return today's delta UT1 value

    End Function

    ''' <summary>
    ''' Return the specified Julian day'DeltaUT1 value
    ''' </summary>
    ''' <param name="RequiredDeltaUT1JulianDateUTC"></param>
    ''' <returns>DeltaUT1 value as a double</returns>
    Public Overloads Function DeltaUT1(RequiredDeltaUT1JulianDateUTC As Double) As Double
        Dim ReturnValue, ProfileValue As Double, UTCDate As DateTime, DeltaUT1ValueName, DeltaUT1String As String

        SyncLock DeltaTLockObject
            If Math.Truncate(RequiredDeltaUT1JulianDateUTC - MODIFIED_JULIAN_DAY_OFFSET) = Math.Truncate(LastDeltaUT1JulianDate - MODIFIED_JULIAN_DAY_OFFSET) Then ' Return the cached value if its availahble otherwise calculate it and save the value for the next call
                LogDebugMessage("DeltaUT1(JD)", String.Format("Returning cached DeltaUT1 value: {0} for UTC Julian date: {1} ({2})",
                                                              LastDeltaUT1Value,
                                                              RequiredDeltaUT1JulianDateUTC,
                                                              DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                Return LastDeltaUT1Value
            End If
        End SyncLock

        ' We don't have a cached value so compute one and save it for the next call

        Select Case UpdateTypeValue
            Case UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
                LogDebugMessage("DeltaUT1(JD)", String.Format("Automatic DeltaUT1 is required for Julian date: {0} ({1})", RequiredDeltaUT1JulianDateUTC,
                                                       DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                ' Approach
                ' Determine whether a downloaded DeltaUT1 value exists for the specified Julian Day (in UTC time)
                '    if yes then 
                '        Determine whether the value is a valid double number
                '        If it is then 
                '            Test whether the value is in the acceptable range -0.0 to +0.9
                '                If it is then return this value
                '                If not then fall back to the predicted value
                '        If not then fall back to the predicted value
                '    if no then fdall back to the predicted value
                UTCDate = DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET) ' Convert the Julian day into a DateTime
                DeltaUT1ValueName = String.Format(DELTAUT1_VALUE_NAME_FORMAT, UTCDate.Year.ToString(DELTAUT1_VALUE_NAME_YEAR_FORMAT), UTCDate.Month.ToString(DELTAUT1_VALUE_NAME_MONTH_FORMAT), UTCDate.Day.ToString(DELTAUT1_VALUE_NAME_DAY_FORMAT))

                DeltaUT1String = profile.GetProfile(GlobalItems.AUTOMATIC_UPDATE_EARTH_ROTATION_DATA_SUBKEY_NAME, DeltaUT1ValueName)
                If DeltaUT1String <> "" Then ' We have got something back from the Profile so test whether it is a valid double number
                    If Double.TryParse(DeltaUT1String, ProfileValue) Then ' We have a valid double number so check that it is the acceptable range
                        If (ProfileValue >= GlobalItems.DELTAUT1_LOWER_BOUND) And (ProfileValue <= GlobalItems.DELTAUT1_UPPER_BOUND) Then
                            ReturnValue = ProfileValue
                            LogDebugMessage("DeltaUT1(JD)", String.Format("Automatic DeltaUT1 is required and a valid value has been found: {0} at Julian date: {1} ({2})",
                                                                   ReturnValue,
                                                                   RequiredDeltaUT1JulianDateUTC,
                                                                   DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                        Else ' We don't have a valid number so fall back to the predicted value
                            LogDebugMessage("DeltaUT1(JD)", String.Format("Automatic DeltaUT1 is required but the Profile value {0} is outside the valid range: {1} - {2}, returning the predicted value at Julian date: {3} ({4})",
                                                                   ProfileValue,
                                                                   GlobalItems.DELTAUT1_LOWER_BOUND,
                                                                   GlobalItems.DELTAUT1_UPPER_BOUND,
                                                                   RequiredDeltaUT1JulianDateUTC,
                                                                   DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                            ReturnValue = Me.LeapSeconds(RequiredDeltaUT1JulianDateUTC) + TT_TAI_OFFSET - Me.DeltaT(RequiredDeltaUT1JulianDateUTC)
                            LogDebugMessage("DeltaUT1(JD)", String.Format("Return value: {0} for Julian day: {1} ({2})",
                                                                   ReturnValue,
                                                                   RequiredDeltaUT1JulianDateUTC,
                                                                   DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                        End If
                    Else ' We have an invalid double value so fall back to the predicted value
                        LogDebugMessage("DeltaUT1(JD)", String.Format("Automatic DeltaUT1 is required but the Profile value {0} is not a valid double value, returning the predicted value at Julian date: {1} ({2})",
                                                               ProfileValue,
                                                               RequiredDeltaUT1JulianDateUTC,
                                                               DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                        ReturnValue = Me.LeapSeconds(RequiredDeltaUT1JulianDateUTC) + TT_TAI_OFFSET - Me.DeltaT(RequiredDeltaUT1JulianDateUTC)
                        LogDebugMessage("DeltaUT1(JD)", String.Format("Return value: {0} for Julian day: {1} ({2})",
                                                                   ReturnValue,
                                                                   RequiredDeltaUT1JulianDateUTC,
                                                                   DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                    End If

                Else ' No value for this date so fall back to the predicted value
                    LogDebugMessage("DeltaUT1(JD)", String.Format("Automatic DeltaUT1 is required but there is no value for the requested date in the Profile, returning the predicted value at Julian date: {0} ({1})",
                                                           RequiredDeltaUT1JulianDateUTC,
                                                           DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                    ReturnValue = Me.LeapSeconds(RequiredDeltaUT1JulianDateUTC) + TT_TAI_OFFSET - Me.DeltaT(RequiredDeltaUT1JulianDateUTC)
                    LogDebugMessage("DeltaUT1(JD)", String.Format("Return value: {0} for Julian day: {1} ({2})",
                                                                   ReturnValue,
                                                                   RequiredDeltaUT1JulianDateUTC,
                                                                   DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                End If

            Case UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1 ' This was the method in Platform 6.3 and earlier
                LogDebugMessage("DeltaUT1(JD)", String.Format("Predicted DeltaUT1 is required so returning value determined from DeltaT calculation at Julian date: {0} ({1})",
                                                       RequiredDeltaUT1JulianDateUTC,
                                                       DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                ReturnValue = Me.LeapSeconds(RequiredDeltaUT1JulianDateUTC) + TT_TAI_OFFSET - Me.DeltaT(RequiredDeltaUT1JulianDateUTC)
                LogDebugMessage("DeltaUT1(JD)", String.Format("Return value: {0} for Julian day: {1} ({2})",
                                                                   ReturnValue,
                                                                   RequiredDeltaUT1JulianDateUTC,
                                                                   DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))

            Case UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1
                ReturnValue = ManualDeltaUT1Value
                LogDebugMessage("DeltaUT1(JD)", String.Format("Manual DeltaUT1 is required so returning manually configured value {0} at Julian date: {1} ({2})",
                                                       ReturnValue,
                                                       RequiredDeltaUT1JulianDateUTC,
                                                       DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))

            Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1
                LogDebugMessage("DeltaUT1(JD)", String.Format("Built-in DeltaUT1 is required so returning value determined from DeltaT calculation at Julian date: {0} ({1})",
                                                       RequiredDeltaUT1JulianDateUTC,
                                                       DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                ReturnValue = Me.LeapSeconds(RequiredDeltaUT1JulianDateUTC) + TT_TAI_OFFSET - Me.DeltaT(RequiredDeltaUT1JulianDateUTC)
                LogDebugMessage("DeltaUT1(JD)", String.Format("Return value: {0} for Julian day: {1} ({2})",
                                                                   ReturnValue,
                                                                   RequiredDeltaUT1JulianDateUTC,
                                                                   DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(DOWNLOAD_TASK_TIME_FORMAT)))

            Case Else
                LogMessage("DeltaUT1(JD)", "Unknown Parameters.UpdateType: " & UpdateTypeValue)
                MsgBox("AstroUtils.DeltaUT1 - Unknown Parameters.UpdateType: " & UpdateTypeValue)

        End Select

        SyncLock DeltaUT1LockObject
            LastDeltaUT1JulianDate = RequiredDeltaUT1JulianDateUTC
            LastDeltaUT1Value = ReturnValue
            Return ReturnValue ' Return the assigned value
        End SyncLock

    End Function

    Public Sub RefreshState()
        Dim DownloadTaskCultureName, OriginalProfileValue As String, AutomaticScheduleTimeDefault As DateTime, FoundCulture, UriValid As Boolean, InstalledCultures As CultureInfo()

        Dim UpdateTypes As New List(Of String) From {UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1,
                                                     UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1,
                                                     UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1,
                                                     UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1}

        Dim ScheduleRepeatOptions As New List(Of String) From {SCHEDULE_REPEAT_NONE,
                                                               SCHEDULE_REPEAT_DAILY,
                                                               SCHEDULE_REPEAT_WEEKLY,
                                                               SCHEDULE_REPEAT_MONTHLY}

        LogDebugMessage("RefreshState", "")
        LogDebugMessage("RefreshState", "Start of Refresh")

        ' Read all values from the Profile and validate them where possible. If they are corrupt then replace with default values

        ' Get the current culture to use as the default standard for writing and reading localaisable values to /from registry 
        DownloadTaskCultureName = profile.GetProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_CULTURE_VALUE_NAME, CultureInfo.CurrentCulture.Name) ' Get the requested culture name
        InstalledCultures = CultureInfo.GetCultures(CultureTypes.AllCultures) ' Get list of all installed cultures on the machine
        For Each culture As CultureInfo In InstalledCultures ' Check whether the requested culture is installed on this machine
            'LogDebugMessage("RefreshState", String.Format("Found installed culture: {0}, Culture name: {0}", culture.Name))
            If culture.Name = DownloadTaskCultureName Then
                FoundCulture = True
                LogDebugMessage("RefreshState", String.Format("Found requested culture on this machine: {0}", DownloadTaskCultureName))
            End If
        Next
        If FoundCulture Then ' Found the requested culture so get its CultureInfo object
            DownloadTaskCultureValue = CultureInfo.GetCultureInfo(DownloadTaskCultureName)
            LogDebugMessage("RefreshState", String.Format("Found culture {0} OK", DownloadTaskCultureValue.Name))
        Else ' Could not find the requested culture, most likely because the culture name in the Profile has become corrupted so replace it with the default value
            DownloadTaskCulture = CultureInfo.CurrentCulture
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter DownloadTaskCulture is corrupt: {0}, default value has been set: {1}", DownloadTaskCultureName, DownloadTaskCultureValue.Name))
            LogEvent(String.Format("EarthRoationParameter DownloadTaskCulture is corrupt: {0}, default value has been set: {1}", DownloadTaskCultureName, DownloadTaskCultureValue.Name))
        End If

        If DateTime.Now.Hour < 12 Then ' Create a default schedule time for use in case a time hasn't been set yet
            AutomaticScheduleTimeDefault = Date.Today.AddHours(12)
        Else
            AutomaticScheduleTimeDefault = Date.Today.AddHours(36)
        End If

        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_SCHEDULED_TIME_VALUE_NAME, AutomaticScheduleTimeDefault.ToString(DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue))
        If DateTime.TryParseExact(OriginalProfileValue, DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue, DateTimeStyles.None, DownloadTaskScheduledTimeValue) Then
            LogDebugMessage("RefreshState", String.Format("DownloadTaskRunTimeValue = {0}", DownloadTaskScheduledTimeValue.ToString(DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue)))
        Else
            DownloadTaskScheduledTime = AutomaticScheduleTimeDefault
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter DownloadTaskRunTimeValue is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskScheduledTimeValue.ToString(DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue)))
            LogEvent(String.Format("EarthRoationParameter DownloadTaskRunTimeValue is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskScheduledTimeValue.ToString(DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue)))
        End If

        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY, UPDATE_TYPE_VALUE_NAME, UPDATE_TYPE_DEFAULT)
        If UpdateTypes.Contains(OriginalProfileValue) Then ' The Profile value is one of the permitted values so we're done
            UpdateTypeValue = OriginalProfileValue
            LogDebugMessage("RefreshState", String.Format("UpdateTypeValue = {0}", UpdateTypeValue))
        Else ' The Profile value is not a permitted value so replace it with the default value
            UpdateType = UPDATE_TYPE_DEFAULT
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter UpdateType is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, UpdateTypeValue))
            LogEvent(String.Format("EarthRoationParameter UpdateType is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, UpdateTypeValue))
        End If

        Dim ManualDeltaUT1String As String = profile.GetProfile(ASTROMETRY_SUBKEY, MANUAL_DELTAUT1_VALUE_NAME, MANUAL_DELTAUT1_DEFAULT.ToString(DownloadTaskCultureValue))
        If (Double.TryParse(ManualDeltaUT1String, NumberStyles.Float, DownloadTaskCultureValue, ManualDeltaUT1Value)) Then ' String parsed OK so list value if debug is enabled
            LogDebugMessage("RefreshState", String.Format("ManualDeltaUT1String = {0}, ManualDeltaUT1Value: {1}", ManualDeltaUT1String, ManualDeltaUT1Value))
        Else 'Returned string doesn't represent a number so reapply the default
            ManualDeltaUT1 = MANUAL_DELTAUT1_DEFAULT
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter ManualDeltaUT1 is corrupt: {0}, default value has been set: {1}", ManualDeltaUT1String, ManualDeltaUT1Value))
            LogEvent(String.Format("EarthRoationParameter ManualDeltaUT1 is corrupt: {0}, default value has been set: {1}", ManualDeltaUT1String, ManualDeltaUT1Value))
        End If


        Dim NowJulian, CurrentLeapSeconds As Double
        NowJulian = DateTime.UtcNow.ToOADate + OLE_AUTOMATION_JULIAN_DATE_OFFSET

        ' Find the leap second value from the built-in table of historic values
        For i As Integer = BuiltInLeapSeconds.Count - 1 To 0 Step -1
            LogDebugMessage("RefreshState", String.Format("Leap second default - searching built-in JD {0} with leap second: {1}", BuiltInLeapSeconds.Keys(i), BuiltInLeapSeconds.Values(i)))

            If Math.Truncate(NowJulian - MODIFIED_JULIAN_DAY_OFFSET) >= Math.Truncate(BuiltInLeapSeconds.Keys(i) - MODIFIED_JULIAN_DAY_OFFSET) Then ' Found a match
                CurrentLeapSeconds = BuiltInLeapSeconds.Values(i)
                LogDebugMessage("RefreshState", String.Format("Leap second default - found built-in leap second: {0} set on JD {1}", BuiltInLeapSeconds.Values(i), BuiltInLeapSeconds.Keys(i)))
                Exit For
            End If
        Next

        Dim ManualTaiUtcOffsetString As String = profile.GetProfile(ASTROMETRY_SUBKEY, MANUAL_LEAP_SECONDS_VALUENAME, CurrentLeapSeconds.ToString(DownloadTaskCultureValue))
        If (Double.TryParse(ManualTaiUtcOffsetString, NumberStyles.Float, DownloadTaskCultureValue, ManualLeapSecondsValue)) Then ' String parsed OK so list value if debug is enabled
            LogDebugMessage("RefreshState", String.Format("ManualTaiUtcOffsetString = {0}, ManualTaiUtcOffsetValue: {1}", ManualTaiUtcOffsetString, ManualLeapSecondsValue))
        Else 'Returned string doesn't represent a number so reapply the default
            ManualLeapSeconds = CurrentLeapSeconds
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter ManualTaiUtcOffset is corrupt: {0}, default value has been set: {1}", ManualTaiUtcOffsetString, ManualLeapSecondsValue))
            LogEvent(String.Format("EarthRoationParameter ManualTaiUtcOffset is corrupt: {0}, default value has been set: {1}", ManualTaiUtcOffsetString, ManualLeapSecondsValue))
        End If

        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY, EARTH_ROTATION_DATA_LAST_UPDATED_VALUE_NAME, EARTH_ROTATION_DATA_NEVER_UPDATED)
        If OriginalProfileValue = EARTH_ROTATION_DATA_NEVER_UPDATED Then ' Has the default value so is OK
            EarthRotationDataLastUpdatedValue = OriginalProfileValue
            LogDebugMessage("RefreshState", String.Format("EarthRotationDataLastUpdatedValue = {0}", EarthRotationDataLastUpdatedValue))
        Else ' Not default so it shoudl be parseable
            Dim lastDate As DateTime
            If DateTime.TryParseExact(OriginalProfileValue, DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue, DateTimeStyles.None, lastDate) Then
                EarthRotationDataLastUpdatedValue = OriginalProfileValue
                LogDebugMessage("RefreshState", String.Format("EarthRotationDataLastUpdatedValue = {0}", EarthRotationDataLastUpdatedValue))
            Else
                EarthRotationDataLastUpdatedString = EARTH_ROTATION_DATA_NEVER_UPDATED
                LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter EarthRotationDataLastUpdated is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, EarthRotationDataLastUpdatedValue))
                LogEvent(String.Format("EarthRoationParameter EarthRotationDataLastUpdated is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, EarthRotationDataLastUpdatedValue))
            End If
        End If

        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_DATA_SOURCE_VALUE_NAME, DOWNLOAD_TASK_DATA_UPDATE_SOURCE_DEFAULT)
        UriValid = False ' Set the valid flag false, then set to true if the download source starts with a supported URI prefix
        If OriginalProfileValue.StartsWith(URI_PREFIX_HTTP, StringComparison.OrdinalIgnoreCase) Then UriValid = True
        If OriginalProfileValue.StartsWith(URI_PREFIX_HTTPS, StringComparison.OrdinalIgnoreCase) Then UriValid = True
        If OriginalProfileValue.StartsWith(URI_PREFIX_FTP, StringComparison.OrdinalIgnoreCase) Then UriValid = True

        If UriValid Then
            DownloadTaskDataSourceValue = OriginalProfileValue
            LogDebugMessage("RefreshState", String.Format("DownloadTaskDataSourceValue = {0}", DownloadTaskDataSourceValue))
        Else
            DownloadTaskDataSource = DOWNLOAD_TASK_DATA_UPDATE_SOURCE_DEFAULT
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter DownloadTaskDataSource is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskDataSourceValue))
            LogEvent(String.Format("EarthRoationParameter DownloadTaskDataSource is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskDataSourceValue))
        End If

        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_REPEAT_FREQUENCY_VALUE_NAME, DOWNLOAD_TASK_REPEAT_DEFAULT)
        If ScheduleRepeatOptions.Contains(OriginalProfileValue) Then ' The Profile value is one of the permitted values so we're done
            DownloadTaskRepeatFrequencyValue = OriginalProfileValue
            LogDebugMessage("RefreshState", String.Format("DownloadTaskRepeatFrequencyValue = {0}", DownloadTaskRepeatFrequencyValue))
        Else ' The Profile value is not a permitted value so replace it with the default value
            DownloadTaskRepeatFrequency = DOWNLOAD_TASK_REPEAT_DEFAULT
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter DownloadTaskRepeatFrequency is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskRepeatFrequencyValue))
            LogEvent(String.Format("EarthRoationParameter DownloadTaskRepeatFrequency is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskRepeatFrequencyValue))
        End If

        Dim DownloadTaskTimeOutString = profile.GetProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_TIMEOUT_VALUE_NAME, DOWNLOAD_TASK_TIMEOUT_DEFAULT.ToString(DownloadTaskCultureValue))
        If (Double.TryParse(DownloadTaskTimeOutString, NumberStyles.Float, DownloadTaskCultureValue, DownloadTaskTimeOutValue)) Then ' String parsed OK so list value if debug is enabled
            LogDebugMessage("RefreshState", String.Format("DownloadTaskTimeOutString = {0}, DownloadTaskTimeOutValue: {1}", DownloadTaskTimeOutString, DownloadTaskTimeOutValue))
        Else 'Returned string doesn't represent a number so reapply the default
            DownloadTaskTimeOut = DOWNLOAD_TASK_TIMEOUT_DEFAULT
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter DownloadTaskTimeOut is corrupt: {0}, default value has been set: {1}", DownloadTaskTimeOutString, DownloadTaskTimeOutValue))
            LogEvent(String.Format("EarthRoationParameter DownloadTaskTimeOut is corrupt: {0}, default value has been set: {1}", DownloadTaskTimeOutString, DownloadTaskTimeOutValue))
        End If

        Dim DownloadTaskTraceEnabledString = profile.GetProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_TRACE_ENABLED_VALUE_NAME, DOWNLOAD_TASK_TRACE_ENABLED_DEFAULT.ToString(DownloadTaskCultureValue))
        If (Boolean.TryParse(DownloadTaskTraceEnabledString, DownloadTaskTraceEnabledValue)) Then ' String parsed OK so list value if debug is enabled
            LogDebugMessage("RefreshState", String.Format("DownloadTaskTraceEnabledString = {0}, DownloadTaskTraceEnabledValue: {1}", DownloadTaskTraceEnabledString, DownloadTaskTraceEnabledValue))
        Else 'Returned string doesn't represent a number so reapply the default
            DownloadTaskTraceEnabled = DOWNLOAD_TASK_TRACE_ENABLED_DEFAULT
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter DownloadTaskTraceEnabled is corrupt: {0}, default value has been set: {1}", DownloadTaskTraceEnabledString, DownloadTaskTraceEnabledValue))
            LogEvent(String.Format("EarthRoationParameter DownloadTaskTraceEnabled is corrupt: {0}, default value has been set: {1}", DownloadTaskTraceEnabledString, DownloadTaskTraceEnabledValue))
        End If

        ' Get the configured trace file directory And make sure that it exists
        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY,
                                                  DOWNLOAD_TASK_TRACE_PATH_VALUE_NAME,
                                                  String.Format(DOWNLOAD_TASK_TRACE_DEFAULT_PATH_FORMAT, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                                                  ).TrimEnd(CChar("\"))
        Try
            Directory.CreateDirectory(OriginalProfileValue) ' Make sure we can create the directory or it already exists
            DownloadTaskTracePathValue = OriginalProfileValue
            LogDebugMessage("RefreshState", String.Format("DownloadTaskTracePathValue = {0}", DownloadTaskTracePathValue))
        Catch ex As Exception ' Something went wrong so restore the default value
            LogMessage("EarthRotParm CORRUPT!", String.Format("Exception thrown: {0}", ex.ToString()))
            LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter DownloadTaskTracePath is corrupt: {0}, default value will be set: {1}", OriginalProfileValue, DownloadTaskTracePathValue))
            LogEvent(String.Format("EarthRoationParameter DownloadTaskTracePath is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskTracePathValue))
            DownloadTaskTracePath = String.Format(DOWNLOAD_TASK_TRACE_DEFAULT_PATH_FORMAT, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).TrimEnd(CChar("\")) ' restore the default path
        End Try

        AutomaticLeapSecondsValue = DOUBLE_VALUE_NOT_AVAILABLE ' Initialise value as not available
        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY, AUTOMATIC_LEAP_SECONDS_VALUENAME, AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE)
        If OriginalProfileValue = AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE Then ' Has the default value so is OK
            AutomaticLeapSecondsStringValue = OriginalProfileValue
            LogDebugMessage("RefreshState", String.Format("AutomaticLeapSecondsStringValue: {0}", AutomaticLeapSecondsStringValue))
        Else ' Not default so it should be parseable
            If (Double.TryParse(OriginalProfileValue, NumberStyles.Float, DownloadTaskCultureValue, AutomaticLeapSecondsValue)) Then ' String parsed OK so save value and list it if debug is enabled
                AutomaticLeapSecondsStringValue = OriginalProfileValue
                LogDebugMessage("RefreshState", String.Format("AutomaticLeapSecondsStringValue: {0}, AutomaticLeapSecondsValue: {1}", AutomaticLeapSecondsStringValue, AutomaticLeapSecondsValue))
            Else 'Returned string doesn't represent a number so reapply the default
                AutomaticLeapSecondsString = AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE
                LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter AutomaticLeapSecondsString is corrupt: {0}, default value has been set: {1}, AutomaticLeapSecondsValue: {2}", OriginalProfileValue, AutomaticLeapSecondsStringValue, AutomaticLeapSecondsValue.ToString()))
                LogEvent(String.Format("EarthRoationParameter AutomaticLeapSecondsString is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, AutomaticLeapSecondsStringValue))
            End If
        End If

        NextLeapSecondsValue = DOUBLE_VALUE_NOT_AVAILABLE ' Initialise value as not available
        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY, NEXT_LEAP_SECONDS_VALUENAME, NEXT_LEAP_SECONDS_NOT_AVAILABLE)
        If (OriginalProfileValue = NEXT_LEAP_SECONDS_NOT_AVAILABLE) Or (OriginalProfileValue = DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE) Then ' Has the default or not published value so is OK
            NextLeapSecondsStringValue = OriginalProfileValue
            LogDebugMessage("RefreshState", String.Format("NextLeapSecondsStringValue: {0}", NextLeapSecondsStringValue))
        Else ' Not default so it should be parseable
            If (Double.TryParse(OriginalProfileValue, NumberStyles.Float, DownloadTaskCultureValue, NextLeapSecondsValue)) Then ' String parsed OK so save value and list it if debug is enabled
                NextLeapSecondsStringValue = OriginalProfileValue
                LogDebugMessage("RefreshState", String.Format("NextLeapSecondsStringValue: {0}, NextLeapSecondsValue: {1}", NextLeapSecondsStringValue, NextLeapSecondsValue))
            Else 'Returned string doesn't represent a number so reapply the default
                NextLeapSecondsString = NEXT_LEAP_SECONDS_NOT_AVAILABLE
                LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter NextLeapSecondsString is corrupt: {0}, default value has been set: {1}, NextLeapSecondsValue: {2}", OriginalProfileValue, NextLeapSecondsStringValue, NextLeapSecondsValue))
                LogEvent(String.Format("EarthRoationParameter NextLeapSecondsString is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, NextLeapSecondsStringValue))
            End If
        End If

        NextLeapSecondsDateValue = DATE_VALUE_NOT_AVAILABLE ' Initialise value as not available
        OriginalProfileValue = profile.GetProfile(ASTROMETRY_SUBKEY, NEXT_LEAP_SECONDS_DATE_VALUENAME, NEXT_LEAP_SECONDS_DATE_DEFAULT)
        If (OriginalProfileValue = NEXT_LEAP_SECONDS_DATE_DEFAULT) Or (OriginalProfileValue = DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE) Then ' Has the default or not published value so is OK
            NextLeapSecondsDateStringValue = OriginalProfileValue
            LogDebugMessage("RefreshState", String.Format("AutomaticNextTaiUtcOffsetDateValue = {0}", NextLeapSecondsDateStringValue))
        Else ' Not default so it should be parseable
            If DateTime.TryParseExact(OriginalProfileValue, DOWNLOAD_TASK_TIME_FORMAT, DownloadTaskCultureValue, DateTimeStyles.AssumeUniversal, NextLeapSecondsDateValue) Then
                NextLeapSecondsDateStringValue = OriginalProfileValue
                LogDebugMessage("RefreshState", String.Format("NextLeapSecondsDateStringValue = {0}, NextLeapSecondsDateValue: {1}", NextLeapSecondsDateStringValue, NextLeapSecondsDateValue.ToString(DOWNLOAD_TASK_TIME_FORMAT)))
            Else
                NextLeapSecondsDateString = DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE
                LogMessage("EarthRotParm CORRUPT!", String.Format("EarthRoationParameter NextLeapSecondsDateStringValue is corrupt: {0}, default value has been set: {1}, NextLeapSecondsDateValue: {2}", OriginalProfileValue, NextLeapSecondsDateStringValue, NextLeapSecondsDateValue.ToString(DOWNLOAD_TASK_TIME_FORMAT)))
                LogEvent(String.Format("EarthRoationParameter NextLeapSecondsDateStringValue is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, NextLeapSecondsDateStringValue))
            End If
        End If

        ' Read in the leap second history values
        Dim ProfileLeapSecondsValueStrings As SortedList(Of String, String) = New SortedList(Of String, String)
        Dim ProfileLeapSecondsValues = New SortedList(Of Double, Double)
        Dim ProfileLeapSecondDateOk, ProfileLeapSecondValueOk As Boolean
        Dim ProfileLeapSecondDate, ProfileLeapSecondsValue As Double

        LogDebugMessage("RefreshState", "")
        LogDebugMessage("RefreshState", String.Format("Reading historic leap seconds from Profile"))

        Try
            ProfileLeapSecondsValueStrings = profile.EnumProfile(AUTOMATIC_UPDATE_LEAP_SECOND_HISTORY_SUBKEY_NAME)
        Catch ex As NullReferenceException ' Key does not exist so supply an empty sorted list
            LogDebugMessage("RefreshState", String.Format("Profile key does not exist - there are no downloaded leap second values"))
            DownloadedLeapSecondValues = New SortedList(Of Double, Double)
        End Try
        LogDebugMessage("RefreshState", String.Format("Found {0} leap second values in the Profile", ProfileLeapSecondsValueStrings.Count))

        ' Parse the JulianDate-LeapSecond string pairs into double values and save them if they are valid
        For Each ProfileLeapSecondKeyValuePair As KeyValuePair(Of String, String) In ProfileLeapSecondsValueStrings
            ProfileLeapSecondDateOk = Double.TryParse(ProfileLeapSecondKeyValuePair.Key, NumberStyles.Float, DownloadTaskCultureValue, ProfileLeapSecondDate) ' Validate the Julian date as a double
            If ProfileLeapSecondDateOk Then ' Check that it is in the valid range to be use with dateTime.FromOADate
                If (ProfileLeapSecondDate < -657435.0) Or (ProfileLeapSecondDate >= 2958466.0) Then
                    ProfileLeapSecondDateOk = False
                    LogMessage("RefreshState", String.Format("Invalid leap second date: {0}, the valid range is -657435.0 to 2958465.999999", ProfileLeapSecondDate))
                End If
            End If
            ProfileLeapSecondValueOk = Double.TryParse(ProfileLeapSecondKeyValuePair.Value, NumberStyles.Float, DownloadTaskCultureValue, ProfileLeapSecondsValue) ' Validate tyhe leap seconds value as a double
            If ProfileLeapSecondDateOk And ProfileLeapSecondValueOk Then ' Both values are valid doubles so add them to the collection
                ProfileLeapSecondsValues.Add(ProfileLeapSecondDate, ProfileLeapSecondsValue)
            Else
                LogMessage("RefreshState", String.Format("Omitted Profile leap Second value JD: {0}, LeapSeconds: {1}, {2} {3}", ProfileLeapSecondKeyValuePair.Key, ProfileLeapSecondKeyValuePair.Value, ProfileLeapSecondDateOk, ProfileLeapSecondValueOk))
            End If
        Next

        ' List the current contents of the historic leap second list
        For Each LeapSecond As KeyValuePair(Of Double, Double) In DownloadedLeapSecondValues
            Dim LeapSecondDateTime As DateTime = DateTime.FromOADate(LeapSecond.Key - OLE_AUTOMATION_JULIAN_DATE_OFFSET)
            LogDebugMessage("RefreshState", String.Format("Found historic leap second value {0} implemented on JD {1} ({2})", LeapSecond.Value, LeapSecond.Key, LeapSecondDateTime.ToString(DOWNLOAD_TASK_TIME_FORMAT)))
        Next

        ' List the new values that will replace the current values
        For Each LeapSecond As KeyValuePair(Of Double, Double) In ProfileLeapSecondsValues
            Dim LeapSecondDateTime As DateTime = DateTime.FromOADate(LeapSecond.Key - OLE_AUTOMATION_JULIAN_DATE_OFFSET)
            LogDebugMessage("RefreshState", String.Format("Found profile leap second value {0} implemented on JD {1} ({2})", LeapSecond.Value, LeapSecond.Key, LeapSecondDateTime.ToString(DOWNLOAD_TASK_TIME_FORMAT)))
        Next

        If ProfileLeapSecondsValues.Count > 0 Then ' If there are any values in the Profile
            DownloadedLeapSecondValues = ProfileLeapSecondsValues ' Save the them for future use
            LogDebugMessage("RefreshState", String.Format("Profile values ({0}) saved to HistoricLeapSecondValues.", DownloadedLeapSecondValues.Count))
        End If

        ' Invalidate caches
        LastLeapSecondJulianDate = DOUBLE_VALUE_NOT_AVAILABLE ' Invalidate the cache so that any new values will be used
        LastDeltaTJulianDate = DOUBLE_VALUE_NOT_AVAILABLE
        LastDeltaUT1JulianDate = DOUBLE_VALUE_NOT_AVAILABLE

        LogDebugMessage("RefreshState", "End of Refresh")
        LogDebugMessage("RefreshState", "")

    End Sub

    Public ReadOnly Property DownloadedLeapSeconds As SortedList(Of Double, Double)
        Get
            Return DownloadedLeapSecondValues
        End Get
    End Property

#End Region

#Region "Support code"

    Private Sub LogMessage(Source As String, Message As String)
        If Not (TL Is Nothing) Then TL.LogMessage(Source, Message)
    End Sub

    Private Sub LogDebugMessage(Source As String, Message As String)
        If (Not (TL Is Nothing)) And DebugTraceEnabled Then TL.LogMessage(Source, Message)
    End Sub

    Private Sub LogEvent(message As String)
        EventLogCode.LogEvent("EarthRotationUpdate", message, EventLogEntryType.Warning, EventLogErrors.EarthRotationUpdate, "")
    End Sub

#End Region

End Class