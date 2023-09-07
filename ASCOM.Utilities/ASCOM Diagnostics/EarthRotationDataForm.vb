Imports ASCOM.Astrometry
Imports System.Globalization
Imports ASCOM.Utilities.Global

Public Class EarthRotationDataForm
    Private Const TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH As Integer = 35
    Private Const UPDATE_DATA_PROCESS_TIMEOUT As Double = 60.0 ' Timeout for the "Update now" function provided by this form
    Private Const REFRESH_TIMER_INTERVAL As Integer = 1000 ' Refresh interval (milliseconds) for the current deltaUT1 and leap second values displayed on the form
    Private Const DELTAUT1_ACCEPTABLE_RANGE As Double = 0.9 ' Acceptable range for manual deltaut1 values is +- this value
    Private Const MINIMUM_UPDATE_RUN_TIME As Double = 5.0 ' Minimum acceptable time (seconds)  for the time allowed for a manually triggered update task to run

    Private Const PLATFORM_HELP_FILE As String = "\ASCOM\Platform 7\Docs\PlatformHelp.chm"
    Private Const EARTH_ROTATION_HELP_TOPIC As String = "/html/98976954-6a00-4864-a223-7b3b25ffaaf1.htm"
    Private TL As TraceLogger
    Private Parameters As EarthRotationParameters
    Private WithEvents NowTimer As Windows.Forms.Timer
    Private aUtils As AstroUtils.AstroUtils

    Private EarthRotationDataUpdateType, AutomaticScheduleJobRepeatFrequency, EarthRotationDataSource, AutomaticScheduleJobLastUpdateTime, TraceFilePath, CurrentLeapSeconds, NextLeapSeconds, NextLeapSecondsDate As String
    Private ManualLeapSeconds, DownloadTimeout, ManualDeltaUT1Value As Double
    Private AutomaticScheduleJobRunTime As DateTime
    Private TraceEnabled As Boolean
    Private LeapSecondMinimumValue As Double

    ' Initialise drop-down list options
    Private dataDownloadSources As New List(Of String) From
        {
             EARTH_ROTATION_INTERNET_DATA_SOURCE_0
        }
    'EARTH_ROTATION_INTERNET_DATA_SOURCE_0,
    'EARTH_ROTATION_INTERNET_DATA_SOURCE_1,
    'EARTH_ROTATION_INTERNET_DATA_SOURCE_2,
    'EARTH_ROTATION_INTERNET_DATA_SOURCE_3,
    'EARTH_ROTATION_INTERNET_DATA_SOURCE_4

    Private ut1Sources As New List(Of String) From
        {
            UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1,
            UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1,
            UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1,
            UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1,
            UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
        }

    Private scheduleRepeatOptions As New List(Of String) From
        {
            SCHEDULE_REPEAT_NONE,
            SCHEDULE_REPEAT_DAILY,
            SCHEDULE_REPEAT_WEEKLY,
            SCHEDULE_REPEAT_MONTHLY
        }

#Region "New and form load"

    Private Sub EarthRotationDataForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim AutomaticScheduleTimeDefault As DateTime ' 

        Try
            TL = New TraceLogger("", "EarthRotation")
            TL.Enabled = GetBool(TRACE_EARTHROTATION_DATA_FORM, TRACE_EARTHROTATION_DATA_FORM_DEFAULT)
            TL.IdentifierWidth = TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH

            TL.LogMessage("Form Load", "Start of form load")
            TL.LogMessage("Form Load", String.Format("Log file name: {0}", TL.LogFileName))

            Parameters = New EarthRotationParameters(TL)
            TL.LogMessage("Form Load", "Calling ManageScheduledTask")
            Parameters.ManageScheduledTask()
            TL.LogMessage("Form Load", "Finished ManageScheduledTask")
            aUtils = New AstroUtils.AstroUtils(TL)

            ' Start a timer to periodically update the current DeltaUT1 and leap second values on the form
            NowTimer = New Windows.Forms.Timer
            NowTimer.Interval = REFRESH_TIMER_INTERVAL
            NowTimer.Start()

            ' Specify that these combo boxes will be painted by code in this form so that the backgrounds will be white rather than grey 
            CmbUpdateType.DrawMode = DrawMode.OwnerDrawFixed
            CmbScheduleRepeat.DrawMode = DrawMode.OwnerDrawFixed

            ' Create a default schedule time for use in case a time hasn't been set yet. Either noon today (local time) if we are before noon or noon tomorrow if we are after noon today.
            If DateTime.Now.Hour < 12 Then
                AutomaticScheduleTimeDefault = Date.Today.AddHours(12)
            Else
                AutomaticScheduleTimeDefault = Date.Today.AddHours(36)
            End If

            ' Get a value to use as the lowest valid value for leap seconds during validation
            LeapSecondMinimumValue = Parameters.CurrentBuiltInLeapSeconds

            ' Populate the combo box lists
            CmbDataSource.Items.Clear()
            CmbDataSource.Items.AddRange(dataDownloadSources.ToArray())

            CmbUpdateType.Items.Clear()
            CmbUpdateType.Items.AddRange(ut1Sources.ToArray())

            CmbScheduleRepeat.Items.Clear()
            CmbScheduleRepeat.Items.AddRange(scheduleRepeatOptions.ToArray())

            TL.LogMessage("Form Load", String.Format("Current UI culture: {0}, Current culture: {1}", CultureInfo.CurrentUICulture.Name, CultureInfo.CurrentCulture.Name))

            ' Get the current state from the Profile
            EarthRotationDataUpdateType = Parameters.UpdateType
            ManualLeapSeconds = Parameters.ManualLeapSeconds
            ManualDeltaUT1Value = Parameters.ManualDeltaUT1
            DownloadTimeout = Parameters.DownloadTaskTimeOut
            EarthRotationDataSource = Parameters.DownloadTaskDataSource
            AutomaticScheduleJobRunTime = Parameters.DownloadTaskScheduledTime
            AutomaticScheduleJobRepeatFrequency = Parameters.DownloadTaskRepeatFrequency
            AutomaticScheduleJobLastUpdateTime = Parameters.EarthRotationDataLastUpdatedString
            TraceFilePath = Parameters.DownloadTaskTracePath
            TraceEnabled = Parameters.DownloadTaskTraceEnabled
            CurrentLeapSeconds = Parameters.AutomaticLeapSecondsString
            NextLeapSeconds = Parameters.NextLeapSecondsString
            NextLeapSecondsDate = Parameters.NextLeapSecondsDateString

            TL.LogMessage("Form Load", "Current Earth rotation data update type: " & EarthRotationDataUpdateType)
            TL.LogMessage("Form Load", "Current manual leap seconds: " & ManualLeapSeconds)
            TL.LogMessage("Form Load", "Current manual delta UT1 value: " & ManualDeltaUT1Value)
            TL.LogMessage("Form Load", "Current download timeout: " & DownloadTimeout)
            TL.LogMessage("Form Load", "Current data download source: " & EarthRotationDataSource)
            TL.LogMessage("Form Load", "Current schedule job run time: " & AutomaticScheduleJobRunTime.ToString(DOWNLOAD_TASK_TIME_FORMAT))
            TL.LogMessage("Form Load", "Current schedule job repeat frequency: " & AutomaticScheduleJobRepeatFrequency)
            TL.LogMessage("Form Load", "Current schedule job last updated: " & AutomaticScheduleJobLastUpdateTime)
            TL.LogMessage("Form Load", "Current schedule job trace path: " & TraceFilePath)
            TL.LogMessage("Form Load", "Current schedule job trace enabled: " & TraceEnabled.ToString())
            TL.LogMessage("Form Load", "Current leap seconds: " & CurrentLeapSeconds)
            TL.LogMessage("Form Load", "Current next leap seconds: " & NextLeapSeconds)
            TL.LogMessage("Form Load", String.Format("Current next leap seconds date string: {0}", NextLeapSecondsDate))

            For Each dataSource As String In dataDownloadSources
                TL.LogMessage("Form Load", "Available data source: " & dataSource)
            Next

            ' Initialise display controls
            TL.LogMessage("Form Load", String.Format("About to set CmbUpdatetype to: {0}", EarthRotationDataUpdateType))
            CmbUpdateType.SelectedItem = EarthRotationDataUpdateType
            TL.LogMessage("Form Load", String.Format("Completed setting CmbUpdatetype to: {0}", EarthRotationDataUpdateType))

            TxtManualDeltaUT1.Text = ManualDeltaUT1Value.ToString("0.000", CultureInfo.CurrentUICulture)
            TxtManualLeapSeconds.Text = ManualLeapSeconds.ToString("0.0", CultureInfo.CurrentUICulture)
            If Not dataDownloadSources.Contains(EarthRotationDataSource) Then
                TL.LogMessage("Form Load", String.Format("Specified data source is not one of the built-in sources so add adding it to the list: {0}", EarthRotationDataSource))
                CmbDataSource.Items.Add(EarthRotationDataSource)
            Else
                TL.LogMessage("Form Load", String.Format("Specified data source is one of the built-in sources: {0}", EarthRotationDataSource))
            End If
            CmbDataSource.SelectedItem = EarthRotationDataSource
            TxtDownloadTimeout.Text = DownloadTimeout.ToString("0.0", CultureInfo.CurrentUICulture)
            DateScheduleRun.Value = AutomaticScheduleJobRunTime
            CmbScheduleRepeat.SelectedItem = AutomaticScheduleJobRepeatFrequency
            TxtTraceFilePath.Text = TraceFilePath
            ChkTraceEnabled.Checked = TraceEnabled

            UpdateStatus()
            EnableControlsAsRequired()
            UpdateCurrentLeapSecondsAndDeltaUT1(New Object, New EventArgs()) ' Update the current leap second and deltaUT1 displays so they have current values when the form appears

        Catch ex As Exception
            TL.LogMessageCrLf("Form Load", ex.ToString())
            MessageBox.Show("Something went wrong when loading the configuration form, please report this on the ASCOM Talk Groups.IO forum, including the ASCOM.EarthRotation.xx.yy.txt log file from your Documents\ASCOM\Logs yyyy-mm-dd folder." & vbCrLf & ex.ToString())
        End Try
    End Sub

#End Region

    Private Sub EnableControlsAsRequired()
        Select Case EarthRotationDataUpdateType
            Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1
                TxtManualLeapSeconds.Enabled = False
                TxtManualDeltaUT1.Enabled = False
                TxtDownloadTimeout.Enabled = False
                CmbDataSource.Enabled = False
                DateScheduleRun.Enabled = False
                CmbScheduleRepeat.Enabled = False
                TxtTraceFilePath.Enabled = False
                ChkTraceEnabled.Enabled = False
                BtnSetTraceDirectory.Enabled = False
                TxtRunStatus.Enabled = False
                BtnRunAutomaticUpdate.Enabled = False
                TxtLastRun.Enabled = False
                LblTraceEnabled.Enabled = False
                LblLastRun.Enabled = False
                LblRunStatus.Enabled = False
            Case UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1
                TxtManualLeapSeconds.Enabled = True
                TxtManualDeltaUT1.Enabled = True
                TxtDownloadTimeout.Enabled = False
                CmbDataSource.Enabled = False
                DateScheduleRun.Enabled = False
                CmbScheduleRepeat.Enabled = False
                TxtTraceFilePath.Enabled = False
                ChkTraceEnabled.Enabled = False
                BtnSetTraceDirectory.Enabled = False
                TxtRunStatus.Enabled = False
                BtnRunAutomaticUpdate.Enabled = False
                TxtLastRun.Enabled = False
                LblTraceEnabled.Enabled = False
                LblLastRun.Enabled = False
                LblRunStatus.Enabled = False
            Case UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1
                TxtManualLeapSeconds.Enabled = True
                TxtManualDeltaUT1.Enabled = False
                TxtDownloadTimeout.Enabled = False
                CmbDataSource.Enabled = False
                DateScheduleRun.Enabled = False
                CmbScheduleRepeat.Enabled = False
                TxtTraceFilePath.Enabled = False
                ChkTraceEnabled.Enabled = False
                BtnSetTraceDirectory.Enabled = False
                TxtRunStatus.Enabled = False
                BtnRunAutomaticUpdate.Enabled = False
                TxtLastRun.Enabled = False
                LblTraceEnabled.Enabled = False
                LblLastRun.Enabled = False
                LblRunStatus.Enabled = False
            Case UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1
                TxtManualLeapSeconds.Enabled = False
                TxtManualDeltaUT1.Enabled = False
                TxtDownloadTimeout.Enabled = True
                CmbDataSource.Enabled = True
                DateScheduleRun.Enabled = False
                CmbScheduleRepeat.Enabled = False
                TxtTraceFilePath.Enabled = True
                ChkTraceEnabled.Enabled = True
                BtnSetTraceDirectory.Enabled = True
                TxtRunStatus.Enabled = True
                BtnRunAutomaticUpdate.Enabled = True
                TxtLastRun.Enabled = True
                LblTraceEnabled.Enabled = True
                LblLastRun.Enabled = True
                LblRunStatus.Enabled = True

            Case UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
                TxtManualLeapSeconds.Enabled = False
                TxtManualDeltaUT1.Enabled = False
                TxtDownloadTimeout.Enabled = True
                CmbDataSource.Enabled = True
                DateScheduleRun.Enabled = True
                CmbScheduleRepeat.Enabled = True
                TxtTraceFilePath.Enabled = True
                ChkTraceEnabled.Enabled = True
                BtnSetTraceDirectory.Enabled = True
                TxtRunStatus.Enabled = True
                BtnRunAutomaticUpdate.Enabled = True
                TxtLastRun.Enabled = True
                LblTraceEnabled.Enabled = True
                LblLastRun.Enabled = True
                LblRunStatus.Enabled = True
            Case Else
                MsgBox("Unknown EarthRotationDataUpdateType: " & EarthRotationDataUpdateType)
        End Select
        GrpOnDemandAndAutomaticUpdateConfiguration.Refresh()
        GrpManualUpdate.Refresh()
        GrpUpdateType.Refresh()
        GrpStatus.Refresh()
        GrpScheduleTime.Refresh()
    End Sub

    Private Sub UpdateStatus()
        Dim DisplayDate As DateTime, jdUtc As Double

        aUtils.Refresh() ' Ensure that our astro utils object is using the latest data

        ' Calculate the display date, allowing for development test offsets if present. In production offsets will be 0 so DisplayDate will have a value of DateTime.Now as a UTC
        DisplayDate = DateTime.UtcNow.Subtract(New TimeSpan(TEST_UTC_DAYS_OFFSET, TEST_UTC_HOURS_OFFSET, TEST_UTC_MINUTES_OFFSET, 0))
        TxtNow.Text = String.Format("{0} {1}", DisplayDate.ToString(DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.CurrentUICulture), DisplayDate.Kind.ToString().ToUpperInvariant())
        jdUtc = DateTime.UtcNow.ToOADate + OLE_AUTOMATION_JULIAN_DATE_OFFSET
        TxtEffectiveDeltaUT1.Text = aUtils.DeltaUT(jdUtc).ToString("+0.000;-0.000;0.000", CultureInfo.CurrentUICulture)
        TxtEffectiveLeapSeconds.Text = aUtils.LeapSeconds.ToString("0.0", CultureInfo.CurrentUICulture)

        Parameters.RefreshState() ' Make sure we have the latest values, in case any have been updated
        TxtLastRun.Text = Parameters.EarthRotationDataLastUpdatedString
        TxtNextLeapSeconds.Text = Parameters.NextLeapSecondsString
        TxtNextLeapSecondsDate.Text = Parameters.NextLeapSecondsDateString & IIf((Parameters.NextLeapSecondsDateString = GlobalItems.DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE) Or (Parameters.NextLeapSecondsDateString = GlobalItems.NEXT_LEAP_SECONDS_DATE_NOT_AVAILABLE_DEFAULT), "", "UTC")
        TxtLastRun.Text = Parameters.EarthRotationDataLastUpdatedString

    End Sub

    Private Sub LogRunMessage(message As String)
        TL.LogMessageCrLf("RunAutomaticUpdate", message)
        TxtRunStatus.Text = message
    End Sub

    Private Sub ValidateURI()
        Dim UriValid As Boolean

        UriValid = False ' Set the valid flag false, then set to true if the download source starts with a supported URI prefix
        If CmbDataSource.Text.StartsWith(URI_PREFIX_HTTP, StringComparison.OrdinalIgnoreCase) Then UriValid = True
        If CmbDataSource.Text.StartsWith(URI_PREFIX_HTTPS, StringComparison.OrdinalIgnoreCase) Then UriValid = True
        If CmbDataSource.Text.StartsWith(URI_PREFIX_FTP, StringComparison.OrdinalIgnoreCase) Then UriValid = True
        If UriValid Then
            ErrorProvider1.SetError(CmbDataSource, "")
            EarthRotationDataSource = CmbDataSource.Text
            Parameters.DownloadTaskDataSource = EarthRotationDataSource
            TL.LogMessage("EarthRotationDataSource", String.Format("Data source updated to: {0}", EarthRotationDataSource))
            BtnClose.Enabled = True
        Else
            BtnClose.Enabled = False
            ErrorProvider1.SetError(CmbDataSource, "Must start with http:// or https:// or ftp://")
        End If
    End Sub

#Region "Timer event handler"

    Private Sub UpdateCurrentLeapSecondsAndDeltaUT1(myObject As Object, ByVal myEventArgs As EventArgs) Handles NowTimer.Tick
        UpdateStatus()
    End Sub

#End Region

#Region "Button and event handlers"

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnRunAutomaticUpdate_Click(sender As Object, e As EventArgs) Handles BtnRunAutomaticUpdate.Click
        Dim psi As ProcessStartInfo, proc As Process, CancelButtonState, OKButtonState, UpdateCompleted As Boolean, RunTimer As Stopwatch
        Try
            RunTimer = New Stopwatch()
            'CancelButtonState = BtnCancel.Enabled ' Save the current button enabled states so they can be restored later
            OKButtonState = BtnClose.Enabled
            LogRunMessage(String.Format("Cancel button state: {0}, OK button state: {1}", CancelButtonState, OKButtonState))
            'BtnCancel.Enabled = False
            BtnClose.Enabled = False
            BtnRunAutomaticUpdate.Enabled = False

            LogRunMessage(String.Format("Creating process info"))
            psi = New ProcessStartInfo()

            If Environment.Is64BitOperatingSystem Then
                psi.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & DOWNLOAD_TASK_EXECUTABLE_NAME
                LogRunMessage(String.Format("Running on a 64bit OS. Executable path: {0}", psi.FileName))
            Else
                psi.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & DOWNLOAD_TASK_EXECUTABLE_NAME
                LogRunMessage(String.Format("Running on a 32bit OS. Executable path: {0}", psi.FileName))
            End If

            psi.Arguments = "/datasource " & CmbDataSource.Text
            psi.WindowStyle = ProcessWindowStyle.Hidden
            LogRunMessage(String.Format("ProcessInfo Filename: {0}, Arguments: {1}", psi.FileName, psi.Arguments))

            RunTimer.Start()
            proc = Process.Start(psi)
            LogRunMessage(String.Format("Started process on {0} as {1}", proc.MachineName, proc.ProcessName))

            UpdateCompleted = False
            Do
                UpdateCompleted = proc.WaitForExit(200)
                Application.DoEvents()
                LogRunMessage(String.Format("Job running - {0} / {1} seconds", RunTimer.Elapsed.TotalSeconds.ToString("0"), UPDATE_DATA_PROCESS_TIMEOUT))
            Loop Until UpdateCompleted Or (RunTimer.Elapsed.TotalSeconds > UPDATE_DATA_PROCESS_TIMEOUT)
            RunTimer.Stop()

            proc.WaitForExit() ' Ensure that all processing is complete before proceeding
            If UpdateCompleted Then
                If proc.ExitCode = 0 Then
                    LogRunMessage(String.Format("Job completed OK in {0} seconds.", RunTimer.Elapsed.TotalSeconds.ToString("0.0")))
                Else
                    LogRunMessage(String.Format("Job failed with return code {0} after {1} seconds.", proc.ExitCode, RunTimer.Elapsed.TotalSeconds.ToString("0.0")))
                End If

            Else
                LogRunMessage(String.Format("Job timed out after {0} seconds, data not updated", RunTimer.Elapsed.TotalSeconds.ToString("0.0")))
                LogRunMessage(String.Format("Killing process"))
                Try
                    proc.Kill()
                Catch ex As Exception
                    LogRunMessage("Exception killing process: " & ex.ToString())
                End Try
            End If
            Parameters.RefreshState()

        Catch ex As Exception
            LogRunMessage(String.Format("Exception: {0}", ex.Message))
            TL.LogMessageCrLf("RunAutomaticUpdate", "Exception running process: " & ex.ToString())
        Finally
            'BtnCancel.Enabled = CancelButtonState ' Ensure that the original button states are restored
            BtnClose.Enabled = OKButtonState
            BtnRunAutomaticUpdate.Enabled = True
            UpdateStatus()
        End Try
    End Sub

    Private Sub BtnSetTraceDirectory_Click(sender As Object, e As EventArgs) Handles BtnSetTraceDirectory.Click
        Dim result As DialogResult
        FolderBrowser.RootFolder = Environment.SpecialFolder.Desktop
        FolderBrowser.SelectedPath = Parameters.DownloadTaskTracePath
        result = FolderBrowser.ShowDialog()
        If result = DialogResult.OK Then
            Parameters.DownloadTaskTracePath = FolderBrowser.SelectedPath
            TraceFilePath = FolderBrowser.SelectedPath
            TxtTraceFilePath.Text = TraceFilePath
            Parameters.DownloadTaskTracePath = TraceFilePath
            TL.LogMessage("TraceFilePath", String.Format("Trace file path updated to: {0}", TraceFilePath))
        End If
    End Sub

    Private Sub CmbUpdateType_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbUpdateType.SelectedIndexChanged
        Dim comboBox As ComboBox = CType(sender, ComboBox)
        Dim originalValue, newValue As String

        originalValue = EarthRotationDataUpdateType
        newValue = CType(comboBox.SelectedItem, String)
        If Not String.IsNullOrEmpty(newValue) Then
            EarthRotationDataUpdateType = newValue
            Parameters.UpdateType = EarthRotationDataUpdateType
            Parameters.ManageScheduledTask() ' Create, update or remove the scheduled task as appropriate
            TL.LogMessage("UpdateTypeEvent", String.Format("Changing current value: '{0}' to: '{1}'", originalValue, EarthRotationDataUpdateType))
        Else
            TL.LogMessage("UpdateTypeEvent", String.Format("New value is null or empty, ignoring change"))
        End If

        TL.BlankLine()
        TL.LogMessage("UpdateTypeEvent", String.Format("Earth rotation data update configuration changes completed."))

        EnableControlsAsRequired()

    End Sub

    Private Sub CmbSchedulRepeat_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbScheduleRepeat.SelectedIndexChanged
        Dim comboBox As ComboBox = CType(sender, ComboBox)
        Dim orig As String

        orig = AutomaticScheduleJobRepeatFrequency
        AutomaticScheduleJobRepeatFrequency = CType(comboBox.SelectedItem, String)
        Parameters.DownloadTaskRepeatFrequency = AutomaticScheduleJobRepeatFrequency
        Parameters.ManageScheduledTask()
        EnableControlsAsRequired()
        TL.LogMessage("DownloadTaskRepeatFrequency", String.Format("Schedule job repeat frequency updated from: {0} to: {1}", orig, AutomaticScheduleJobRepeatFrequency))
    End Sub

    Private Sub BtnHelp_Click(sender As Object, e As EventArgs) Handles BtnHelp.Click
        Dim Btn As Button, HelpFilePath As String
        Btn = CType(sender, Button)

        If Environment.Is64BitOperatingSystem Then
            HelpFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & PLATFORM_HELP_FILE
        Else
            HelpFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & PLATFORM_HELP_FILE
        End If

        Help.ShowHelp(Btn.Parent, HelpFilePath, HelpNavigator.Topic, EARTH_ROTATION_HELP_TOPIC)
    End Sub

    Private Sub ChkTraceEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles ChkTraceEnabled.CheckedChanged
        TraceEnabled = ChkTraceEnabled.Checked
        Parameters.DownloadTaskTraceEnabled = TraceEnabled
        TL.LogMessage("TraceEnabled", String.Format("Trace enabled updated to: {0}", TraceEnabled))
    End Sub

    Private Sub DateScheduleRun_ValueChanged(sender As Object, e As EventArgs) Handles DateScheduleRun.ValueChanged
        AutomaticScheduleJobRunTime = DateScheduleRun.Value
        Parameters.DownloadTaskScheduledTime = AutomaticScheduleJobRunTime
        Parameters.ManageScheduledTask()
        TL.LogMessage("AutomaticScheduleJobRunTime", String.Format("Schedule job run time updated to: {0}", AutomaticScheduleJobRunTime.ToString(DOWNLOAD_TASK_TIME_FORMAT)))
    End Sub

#End Region

#Region "Input validation routines"

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbDataSource.SelectedIndexChanged
        ValidateURI()
    End Sub

    Private Sub TxtDownloadTimeout_Validating(sender As Object, e As KeyEventArgs) Handles TxtDownloadTimeout.KeyUp
        Dim DoubleValue As Double = 0.0, IsDouble As Boolean

        IsDouble = Double.TryParse(TxtDownloadTimeout.Text, NumberStyles.Float, CultureInfo.CurrentUICulture, DoubleValue)
        If IsDouble And DoubleValue >= MINIMUM_UPDATE_RUN_TIME Then
            ErrorProvider1.SetError(TxtDownloadTimeout, "")
            DownloadTimeout = DoubleValue
            Parameters.DownloadTaskTimeOut = DownloadTimeout
            TL.LogMessage("DownloadTimeout", String.Format("Download timeout updated to: {0}", DownloadTimeout))
            BtnClose.Enabled = True
        Else
            BtnClose.Enabled = False
            ErrorProvider1.SetError(TxtDownloadTimeout, String.Format("Must be a number >= {0}!", MINIMUM_UPDATE_RUN_TIME.ToString("0.0", CultureInfo.CurrentUICulture)))
        End If
    End Sub

    Private Sub TxtManualLeapSeconds_Validating(sender As Object, e As KeyEventArgs) Handles TxtManualLeapSeconds.KeyUp
        Dim DoubleValue As Double = 0.0, IsDouble As Boolean

        IsDouble = Double.TryParse(TxtManualLeapSeconds.Text, NumberStyles.Float, CultureInfo.CurrentUICulture, DoubleValue)
        If IsDouble And DoubleValue >= LeapSecondMinimumValue Then
            ErrorProvider1.SetError(TxtManualLeapSeconds, "")
            ManualLeapSeconds = DoubleValue
            Parameters.ManualLeapSeconds = ManualLeapSeconds
            TL.LogMessage("ManualLeapSeconds", String.Format("Manual leap seconds updated to: {0}", ManualLeapSeconds))
            BtnClose.Enabled = True
        Else
            BtnClose.Enabled = False
            ErrorProvider1.SetError(TxtManualLeapSeconds, String.Format("Must be a number >= {0}!", LeapSecondMinimumValue.ToString("0.0", CultureInfo.CurrentUICulture)))
        End If
    End Sub

    Private Sub CmbDataSource_Validating(sender As Object, e As KeyEventArgs) Handles CmbDataSource.KeyUp
        ValidateURI()
    End Sub

    Private Sub TxtDeltaUT1Manuals_Validating(sender As Object, e As KeyEventArgs) Handles TxtManualDeltaUT1.KeyUp
        Dim DoubleValue As Double = 0.0, IsDouble As Boolean

        IsDouble = Double.TryParse(TxtManualDeltaUT1.Text, NumberStyles.Float, CultureInfo.CurrentUICulture, DoubleValue)
        If IsDouble And (DoubleValue >= -DELTAUT1_ACCEPTABLE_RANGE) And (DoubleValue <= +DELTAUT1_ACCEPTABLE_RANGE) Then
            ErrorProvider1.SetError(TxtManualDeltaUT1, "")
            ManualDeltaUT1Value = DoubleValue
            Parameters.ManualDeltaUT1 = ManualDeltaUT1Value
            TL.LogMessage("ManualDeltaUT1Value", String.Format(CultureInfo.CurrentUICulture, "Manual DeltaUT1 value updated to: {0}", ManualDeltaUT1Value))
            BtnClose.Enabled = True
        Else
            BtnClose.Enabled = False
            ErrorProvider1.SetError(TxtManualDeltaUT1, String.Format(CultureInfo.CurrentUICulture, "Must be in the range -{0} to +{0}!", DELTAUT1_ACCEPTABLE_RANGE))
        End If
    End Sub

#End Region

#Region "Display drawing event handlers"

    Private Sub GroupBox_Paint(sender As Object, e As PaintEventArgs) Handles GrpOnDemandAndAutomaticUpdateConfiguration.Paint, GrpManualUpdate.Paint, GrpUpdateType.Paint, GrpStatus.Paint, GrpScheduleTime.Paint
        Const HEIGHT_OFFSET As Integer = 8
        Const WIDTH_OFFSET As Integer = 1
        Const PEN_WIDTH As Integer = 1

        Dim thisControl As GroupBox, gfx As Graphics, p As Pen, tSize As Size, activeBorder, inactiveBorder, borderColour As Color

        activeBorder = SystemColors.Highlight
        inactiveBorder = Color.LightGray

        thisControl = CType(sender, GroupBox)
        gfx = e.Graphics
        tSize = TextRenderer.MeasureText(thisControl.Text, thisControl.Font)

        Select Case thisControl.Name
            Case GrpOnDemandAndAutomaticUpdateConfiguration.Name
                Select Case EarthRotationDataUpdateType
                    Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1, UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1, UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1
                        borderColour = inactiveBorder
                    Case UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1, UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
                        borderColour = activeBorder
                    Case Else
                        borderColour = Color.Red ' Warning colour that something has gone wrong in the code!
                End Select
                LblAutoDataSource.Enabled = borderColour = activeBorder
                LblAutoTimeout.Enabled = borderColour = activeBorder
                LblAutoSeconds.Enabled = borderColour = activeBorder

            Case GrpManualUpdate.Name
                Select Case EarthRotationDataUpdateType
                    Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1, UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1, UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
                        borderColour = inactiveBorder
                    Case UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1, UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1
                        borderColour = activeBorder
                    Case Else
                        borderColour = Color.Red ' Warning colour that something has gone wrong in the code!
                End Select
                LblManualDeltaUT1.Enabled = borderColour = activeBorder
                LblManualLeapSeconds.Enabled = borderColour = activeBorder

            Case GrpScheduleTime.Name
                If EarthRotationDataUpdateType = UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1 Then
                    borderColour = activeBorder
                Else
                    borderColour = inactiveBorder
                End If
                LblAutoDownloadTime.Enabled = borderColour = activeBorder
                LblAutoRepeatFrequency.Enabled = borderColour = activeBorder

            Case Else
                borderColour = activeBorder
        End Select

        p = New Pen(borderColour, PEN_WIDTH)
        thisControl.ForeColor = borderColour

        gfx.DrawLine(p, 0, HEIGHT_OFFSET, 0, e.ClipRectangle.Height - 2) ' left vertical
        gfx.DrawLine(p, 0, HEIGHT_OFFSET, 7, HEIGHT_OFFSET) ' Top left part
        gfx.DrawLine(p, tSize.Width + 7, HEIGHT_OFFSET, e.ClipRectangle.Width - WIDTH_OFFSET, HEIGHT_OFFSET) ' Top right part
        gfx.DrawLine(p, e.ClipRectangle.Width - WIDTH_OFFSET, HEIGHT_OFFSET, e.ClipRectangle.Width - WIDTH_OFFSET, e.ClipRectangle.Height - 2) ' Right vertical
        gfx.DrawLine(p, e.ClipRectangle.Width - WIDTH_OFFSET, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2) ' Bottom

    End Sub

    ''' <summary>
    ''' Event handler to paint the device list combo box in the "DropDown" rather than "DropDownList" style
    ''' </summary>
    ''' <param name="sender">Device to be painted</param>
    ''' <param name="e">Draw event arguments object</param>
    Private Sub ComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles CmbScheduleRepeat.DrawItem, CmbUpdateType.DrawItem
        Dim DisabledForeColour, DisabledBackColour As Color
        Dim combo As ComboBox = CType(sender, ComboBox)

        If (e.Index < 0) Then Return

        DisabledForeColour = SystemColors.GrayText
        DisabledBackColour = SystemColors.ButtonFace


        If ((e.State And DrawItemState.Selected) = DrawItemState.Selected) Then ' Draw Then the selected item In menu highlight colour

            If combo.Enabled Then
                e.Graphics.FillRectangle(New SolidBrush(SystemColors.MenuHighlight), e.Bounds)
                e.Graphics.DrawString(combo.Items(e.Index).ToString(), e.Font, New SolidBrush(SystemColors.HighlightText), New Point(e.Bounds.X, e.Bounds.Y))
            Else
                e.Graphics.FillRectangle(New SolidBrush(DisabledBackColour), e.Bounds)
                e.Graphics.DrawString(combo.Items(e.Index).ToString(), e.Font, New SolidBrush(DisabledForeColour), New Point(e.Bounds.X, e.Bounds.Y))
            End If

        Else
            If combo.Enabled Then
                e.Graphics.FillRectangle(New SolidBrush(SystemColors.Window), e.Bounds)
                e.Graphics.DrawString(combo.Items(e.Index).ToString(), e.Font, New SolidBrush(combo.ForeColor), New Point(e.Bounds.X, e.Bounds.Y))
            Else
                e.Graphics.FillRectangle(New SolidBrush(DisabledBackColour), e.Bounds)
                e.Graphics.DrawString(combo.Items(e.Index).ToString(), e.Font, New SolidBrush(DisabledForeColour), New Point(e.Bounds.X, e.Bounds.Y))

            End If
        End If

        e.DrawFocusRectangle()
    End Sub

#End Region

End Class