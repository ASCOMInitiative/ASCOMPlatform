Imports ASCOM.Astrometry
Imports System.Collections.Generic
Imports System.ComponentModel
Imports Microsoft.Win32.TaskScheduler
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Security.Principal

Public Class EarthRotationDataForm
    Private Const TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH As Integer = 35
    Private Const UPDATE_DATA_PROCESS_TIMEOUT As Double = 60.0

    Private TL As TraceLogger
    Private EarthRotationDataUpdateType As String
    Private AutomaticScheduleJobRunTime As DateTime
    Private AutomaticScheduleJobRepeatFrequency As String
    Private UtcTaiOffset As Double
    Private DownloadTimeout As Double
    Private EarthRotationDataSource As String
    Private DeltaUT1ManualValue As Double
    Private AutomaticScheduleJobLastUpdateTime As String
    Private TraceEnabled As Boolean
    Private TraceFilePath As String
    Private CurrentLeapSeconds As String
    Private NextLeapSeconds As String
    Private NextLeapSecondsDate As String
    Private Parameters As EarthRotationParameters
    Private WithEvents NowTimer As Windows.Forms.Timer
    Private aUtils As AstroUtils.AstroUtils

    Private dataDownloadSources As New List(Of String) From {EARTH_ROTATION_DATA_SOURCE_0,
                                                             EARTH_ROTATION_DATA_SOURCE_1,
                                                             EARTH_ROTATION_DATA_SOURCE_2,
                                                             EARTH_ROTATION_DATA_SOURCE_3,
                                                             EARTH_ROTATION_DATA_SOURCE_4}

    Private ut1Sources As New List(Of String) From {UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1,
                                                    UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1,
                                                    UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1,
                                                    UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1}

    Private scheduleRepeatOptions As New List(Of String) From {SCHEDULE_REPEAT_NONE,
                                                               SCHEDULE_REPEAT_DAILY,
                                                               SCHEDULE_REPEAT_WEEKLY,
                                                               SCHEDULE_REPEAT_MONTHLY}

    Private Sub EarthRotationDataForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TL = New TraceLogger("", "EarthRotation")
        TL.Enabled = True
        TL.IdentifierWidth = TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH

        TL.LogMessage("Form Load", "Start of form load")
        TL.LogMessage("Form Load", String.Format("Log file name: {0}", TL.LogFileName))

        Parameters = New EarthRotationParameters(TL)
        aUtils = New AstroUtils.AstroUtils()

        NowTimer = New Windows.Forms.Timer
        NowTimer.Interval = 1000
        NowTimer.Start()

        Try
            CmbUpdateType.DrawMode = DrawMode.OwnerDrawFixed
            CmbScheduleRepeat.DrawMode = DrawMode.OwnerDrawFixed

            CmbDataSource.Items.Clear()
            CmbDataSource.Items.AddRange(dataDownloadSources.ToArray())
            CmbDataSource.SelectedIndex = 0

            CmbUpdateType.Items.Clear()
            CmbUpdateType.Items.AddRange(ut1Sources.ToArray())
            CmbUpdateType.SelectedIndex = 0

            CmbScheduleRepeat.Items.Clear()
            CmbScheduleRepeat.Items.AddRange(scheduleRepeatOptions.ToArray())
            CmbScheduleRepeat.SelectedIndex = 0

            'earthRotationConfiguration = New RegistryAccess
            UtcTaiOffset = Parameters.ManualLeapSeconds '    Convert.ToDouble(earthRotationConfiguration.GetProfile(ASTROMETRY_SUBKEY, MANUAL_TAI_UTC_OFFSET_VALUENAME, MANUAL_TAI_UTC_OFFSET_DEFAULT.ToString))
            DownloadTimeout = Parameters.DownloadTaskTimeOut     'Convert.ToDouble(earthRotationConfiguration.GetProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_TIMEOUT_VALUE_NAME, DOWNLOAD_TASK_TIMEOUT_DEFAULT.ToString))
            EarthRotationDataSource = Parameters.DownloadTaskDataSource  ' earthRotationConfiguration.GetProfile(ASTROMETRY_SUBKEY, DOWNLOAD_TASK_DATA_SOURCE_VALUE_NAME, DOWNLOAD_TASK_DATA_UPDATE_SOURCE_DEFAULT.ToString)
            EarthRotationDataUpdateType = Parameters.UpdateType   ' earthRotationConfiguration.GetProfile(ASTROMETRY_SUBKEY, EARTH_ROTATION_DATA_UPDATE_TYPE_VALUE_NAME, EARTH_ROTATION_DATA_UPDATE_TYPE_DEFAULT)
            DeltaUT1ManualValue = Parameters.ManualDeltaUT1     ' Convert.ToDouble(earthRotationConfiguration.GetProfile(ASTROMETRY_SUBKEY, MANUAL_DELTAUT1_VALUE_NAME, MANUAL_DELTAUT1_DEFAULT.ToString()))

            ' Create a default schedule time for use in case a time hasn't been set yet
            Dim AutomaticScheduleTimeDefault As DateTime
            If DateTime.Now.Hour < 12 Then
                AutomaticScheduleTimeDefault = Date.Today.AddHours(12)
            Else
                AutomaticScheduleTimeDefault = Date.Today.AddHours(36)
            End If

            Dim cultname As String = CultureInfo.CurrentUICulture.Name
            Dim cultnamestring As String = CultureInfo.CurrentUICulture.ToString()
            Dim cult As CultureInfo = CultureInfo.GetCultureInfo(cultname)
            TL.LogMessage("Form Load", String.Format("Current UI culture name: {0}. ToString version: {1}, New culture name: {2}, new culture string: {2}", cultname, cultnamestring, cult.Name, cult.ToString()))

            AutomaticScheduleJobRunTime = Parameters.DownloadTaskScheduledTime
            AutomaticScheduleJobRepeatFrequency = Parameters.DownloadTaskRepeatFrequency
            AutomaticScheduleJobLastUpdateTime = Parameters.EarthRotationDataLastUpdatedString
            TraceFilePath = Parameters.DownloadTaskTracePath
            TraceEnabled = Parameters.DownloadTaskTraceEnabled
            CurrentLeapSeconds = Parameters.AutomaticLeapSecondsString
            NextLeapSeconds = Parameters.NextLeapSecondsString
            NextLeapSecondsDate = Parameters.NextLeapSecondsDateString

            TL.LogMessage("Form Load", "Current leap seconds: " & UtcTaiOffset)
            TL.LogMessage("Form Load", "Current download timeout: " & DownloadTimeout)
            TL.LogMessage("Form Load", "Current data download source: " & EarthRotationDataSource)
            TL.LogMessage("Form Load", "Current Earth rotation data update type: " & EarthRotationDataUpdateType)
            TL.LogMessage("Form Load", "Current manual delta UT1 value: " & DeltaUT1ManualValue)
            TL.LogMessage("Form Load", "Current schedule job run time: " & AutomaticScheduleJobRunTime.ToString(DOWNLOAD_TASK_TIME_FORMAT))
            TL.LogMessage("Form Load", "Current schedule job repeat frequency: " & AutomaticScheduleJobRepeatFrequency)
            TL.LogMessage("Form Load", "Current next leap seconds: " & NextLeapSeconds)
            TL.LogMessage("Form Load", String.Format("Current next leap seconds date string: {0}", NextLeapSecondsDate))

            For Each dataSource As String In dataDownloadSources
                TL.LogMessage("Form Load", "Available data source: " & dataSource)
            Next

            CmbUpdateType.SelectedItem = EarthRotationDataUpdateType

            TxtManualDeltaUT1.Text = DeltaUT1ManualValue.ToString()
            TxtManualLeapSeconds.Text = UtcTaiOffset.ToString()

            If Not dataDownloadSources.Contains(EarthRotationDataSource) Then CmbDataSource.Items.Add(EarthRotationDataSource)
            CmbDataSource.SelectedItem = EarthRotationDataSource
            TxtDownloadTimeout.Text = DownloadTimeout.ToString()
            DateScheduleRun.Value = AutomaticScheduleJobRunTime
            CmbScheduleRepeat.SelectedItem = AutomaticScheduleJobRepeatFrequency
            TxtTraceFilePath.Text = TraceFilePath
            ChkTraceEnabled.Checked = TraceEnabled
            UpdateStatus()
            EnableControlsAsRequired()
        Catch ex As Exception
            TL.LogMessageCrLf("Form Load", ex.ToString())
            MessageBox.Show("Something went wrong when loading the configuration form, please report this on the ASCOM Talk Yahoo forum, including the ASCOM.EarthRotation.xx.yy.txt log file from your Documents\ASCOM\Logs yyyy-mm-dd folder." & vbCrLf & ex.ToString())
        End Try
    End Sub

    Private Sub ApplyChanges()
        Dim taskDefinition As TaskDefinition, taskTrigger As Trigger = Nothing, executablePath As String

        Try
            ' Get values from UI components into the relvant variables
            UtcTaiOffset = Double.Parse(TxtManualLeapSeconds.Text)
            EarthRotationDataUpdateType = CmbUpdateType.SelectedItem.ToString()
            EarthRotationDataSource = CmbDataSource.Text
            DeltaUT1ManualValue = TxtManualDeltaUT1.Text
            DownloadTimeout = Double.Parse(TxtDownloadTimeout.Text)
            AutomaticScheduleJobRepeatFrequency = CmbScheduleRepeat.SelectedItem.ToString()
            AutomaticScheduleJobRunTime = DateScheduleRun.Value
            TraceFilePath = TxtTraceFilePath.Text
            TraceEnabled = ChkTraceEnabled.Checked

            ' Update the Profile with new values
            Parameters.UpdateType = EarthRotationDataUpdateType
            Parameters.ManualDeltaUT1 = DeltaUT1ManualValue
            Parameters.ManualLeapSeconds = UtcTaiOffset
            Parameters.DownloadTaskDataSource = EarthRotationDataSource
            Parameters.DownloadTaskTimeOut = DownloadTimeout
            Parameters.DownloadTaskScheduledTime = AutomaticScheduleJobRunTime
            Parameters.DownloadTaskRepeatFrequency = AutomaticScheduleJobRepeatFrequency
            Parameters.DownloadTaskTracePath = TraceFilePath
            Parameters.DownloadTaskTraceEnabled = TraceEnabled

            Parameters.RefreshState() ' Refresh parameter values in case they have changed while we are running

            ' Parameters have been updated so get a new AstroUntils object that will use the revised configuration
            Try : aUtils.Dispose() : Catch : End Try
            Try : aUtils = Nothing : Catch : End Try
            aUtils = New AstroUtils.AstroUtils()

            UpdateStatus()

            TL.LogMessage("OK", String.Format("Leap seconds updated to: {0}", UtcTaiOffset))
            TL.LogMessage("OK", String.Format("Download timeout updated to: {0}", DownloadTimeout))
            TL.LogMessage("OK", String.Format("Data source updated to: {0}", EarthRotationDataSource))
            TL.LogMessage("OK", String.Format("Type of earth rotation data updated changed to: {0}", EarthRotationDataUpdateType))
            TL.LogMessage("OK", String.Format("Manual DeltaUT1 value updated to: {0}", DeltaUT1ManualValue))
            TL.LogMessage("OK", String.Format("Schedule job repeat frequency updated to: {0}", AutomaticScheduleJobRepeatFrequency))
            TL.LogMessage("OK", String.Format("Schedule job run time updated to: {0}", AutomaticScheduleJobRunTime.ToString(DOWNLOAD_TASK_TIME_FORMAT)))
            TL.LogMessage("OK", String.Format("Trace file path updated to: {0}", TraceFilePath))
            TL.LogMessage("OK", String.Format("Trace enabled updated to: {0}", TraceEnabled))
            TL.BlankLine()

            TL.LogMessage("OK", "Obtaining Scheduler information")
            Using service = New TaskService()
                Dim tasks As List(Of Task), rootFolder As TaskFolder

                TL.LogMessage("OK", String.Format("Highest supported scheduler version: {0}, Library version: {1}, Connected: {2}", service.HighestSupportedVersion, TaskService.LibraryVersion, service.Connected))

                rootFolder = service.GetFolder("\")
                tasks = New List(Of Task)
                tasks.AddRange(rootFolder.GetTasks)
                For Each task As Task In tasks
                    TL.LogMessage("OK", String.Format("Found root task {0} last run: {1}               {2}", task.Path, task.LastRunTime, task.Name))
                Next
                TL.BlankLine()

                ' List current task state if any
                Dim ASCOMTask As Task = service.GetTask(DOWNLOAD_SCHEDULE_TASK_PATH)
                If (Not (ASCOMTask Is Nothing)) Then
                    TL.LogMessage("OK", String.Format("Found ASCOM task {0} last run: {1}, State: {2}, Enabled: {3}", ASCOMTask.Path, ASCOMTask.LastRunTime, ASCOMTask.State, ASCOMTask.Enabled))
                Else
                    TL.LogMessage("OK", "ASCOM task does not exist")
                End If
                TL.BlankLine()

                Select Case EarthRotationDataUpdateType
                    Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1, UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1, UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1 ' Just remove the update job if it exists so that it can't run
                        If (Not (ASCOMTask Is Nothing)) Then
                            TL.LogMessage("OK", String.Format("Update type is {0} and {1} task exists so it will be deleted.", EarthRotationDataUpdateType, DOWNLOAD_SCHEDULE_TASK_NAME))
                            service.RootFolder.DeleteTask(DOWNLOAD_SCHEDULE_TASK_NAME)
                            TL.LogMessage("OK", String.Format("Task {0} deleted OK.", DOWNLOAD_SCHEDULE_TASK_NAME))
                        Else
                            TL.LogMessage("OK", String.Format("Update type is {0} and {1} task does not exist so no action.", EarthRotationDataUpdateType, DOWNLOAD_SCHEDULE_TASK_NAME))
                        End If

                    Case UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1 ' Create a new or Update the existing scheduled job

                        ' Get the task definition to work on, either a new one or the existing task, if it exists
                        If (Not (ASCOMTask Is Nothing)) Then
                            TL.LogMessage("OK", String.Format("Update type is {0} and {1} task exists so it will be updated.", EarthRotationDataUpdateType, DOWNLOAD_SCHEDULE_TASK_NAME))
                            taskDefinition = ASCOMTask.Definition
                        Else
                            TL.LogMessage("OK", String.Format("Update type is {0} and {1} task does not exist so a new task definition will be created.", EarthRotationDataUpdateType, DOWNLOAD_SCHEDULE_TASK_NAME))
                            taskDefinition = service.NewTask
                        End If

                        taskDefinition.RegistrationInfo.Description = "ASCOM scheduled job to update earth rotation data: leap seconds and delta UT1. This job is managed through the ASCOM Diagnostics application and should not be manually edited."

                        executablePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & DOWNLOAD_EXECUTABLE_NAME
                        taskDefinition.Actions.Clear() ' Remove any existing actions and add the current one
                        taskDefinition.Actions.Add(New ExecAction(executablePath, Nothing, Nothing)) ' Add an action that will launch Notepad whenever the trigger fires
                        'TL.LogMessage("OK", String.Format("", ))
                        TL.LogMessage("OK", String.Format("Added scheduled job action to run {0}", executablePath))

                        'taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken

                        taskDefinition.Settings.AllowDemandStart = True ' Add settings appropriate to the task
                        taskDefinition.Settings.StartWhenAvailable = True
                        taskDefinition.Settings.ExecutionTimeLimit = New TimeSpan(0, 10, 0)
                        taskDefinition.Settings.StopIfGoingOnBatteries = False
                        taskDefinition.Settings.DisallowStartIfOnBatteries = False
                        taskDefinition.Settings.Enabled = True
                        'taskDefinition.Settings.RunOnlyIfLoggedOn = False
                        TL.LogMessage("OK", String.Format("Allow demand on start: {0}, Start when available: {1}, Execution time limit: {2} minutes, Stop if going on batteries: {3}, Disallow start if on batteries: {4}, Enabled: {5}, Run only iof logged on: {6}",
                                                          taskDefinition.Settings.AllowDemandStart, taskDefinition.Settings.StartWhenAvailable,
                                                          taskDefinition.Settings.ExecutionTimeLimit.TotalMinutes, taskDefinition.Settings.StopIfGoingOnBatteries, taskDefinition.Settings.DisallowStartIfOnBatteries,
                                                          taskDefinition.Settings.Enabled, taskDefinition.Settings.RunOnlyIfLoggedOn))

                        Select Case AutomaticScheduleJobRepeatFrequency
                            Case SCHEDULE_REPEAT_NONE ' Execute once at the specified day and time
                                taskTrigger = New TimeTrigger()
                                TL.LogMessage("OK", String.Format("Set trigger to run the job once at the specified time."))
                            Case SCHEDULE_REPEAT_DAILY ' Execute daily at the specified time
                                taskTrigger = New DailyTrigger()
                                TL.LogMessage("OK", String.Format("Set trigger to repeat the job daily at the specified time."))
                            Case SCHEDULE_REPEAT_WEEKLY ' Execute once per week on the specified day of week
                                taskTrigger = New WeeklyTrigger()
                                TL.LogMessage("OK", String.Format("Set trigger to repeat the job weekly on the specified day of the week at the specified time."))
                            Case SCHEDULE_REPEAT_MONTHLY ' Execute once per month on the specified day number of the month
                                taskTrigger = New MonthlyTrigger()
                                TL.LogMessage("OK", String.Format("Set trigger to repeat the job monthly on the specified day number of the month at the specified time."))
                            Case Else
                                MessageBox.Show(String.Format("EarthRotationDataForm.BtnOK - Unknown type of AutomaticScheduleJobRepeatFrequency: {0}", AutomaticScheduleJobRepeatFrequency))
                        End Select
                        taskTrigger.StartBoundary = AutomaticScheduleJobRunTime ' Add the user supplied date / time to the trigger

                        taskDefinition.Triggers.Clear() ' Remove any previous triggers and add the new trigger to the task as the only trigger
                        taskDefinition.Triggers.Add(taskTrigger)
                        TL.LogMessage("OK", String.Format("Added the new trigger to the task definition."))

                        ' Implement the new task in the root folder either by updating the existing task or creating a new task
                        If (Not (ASCOMTask Is Nothing)) Then ' The task already exists
                            TL.LogMessage("OK", String.Format("The {0} task exists so applying the updates.", DOWNLOAD_SCHEDULE_TASK_NAME))
                            ASCOMTask.RegisterChanges() ' Task exists so apply the changes made above
                            TL.LogMessage("OK", String.Format("Updates applied OK."))
                        Else ' The task does not already exist
                            TL.LogMessage("OK", String.Format("The {0} task does not exist so registering it now.", DOWNLOAD_SCHEDULE_TASK_NAME))
                            service.RootFolder.RegisterTaskDefinition(DOWNLOAD_SCHEDULE_TASK_NAME, taskDefinition, TaskCreation.CreateOrUpdate, "SYSTEM", Nothing, TaskLogonType.ServiceAccount)
                            'service.RootFolder.RegisterTaskDefinition(AUTOMATIC_SCHEDULE_JOB_NAME, taskDefinition)
                            TL.LogMessage("OK", String.Format("New task registered OK."))
                        End If
                    Case Else
                        MessageBox.Show(String.Format("EarthRotationDataForm.BtnOK - Unknown type of EarthRotationDataUpdateType: {0}", EarthRotationDataUpdateType))
                End Select
            End Using

        Catch ex As Exception
            TL.LogMessageCrLf("Exception", ex.ToString())
            MessageBox.Show("Something went wrong with the update, please report this on the ASCOM Talk Yahoo forum, including the ASCOM.EarthRotation.xx.yy.txt log file from your Documents\ASCOM\Logs yyyy-mm-dd folder." & vbCrLf & ex.ToString())
        End Try

        TL.BlankLine()
        TL.LogMessage("OK", String.Format("Earth rotation data update configuration changes completed."))
    End Sub

    Private Sub BtnApply_Click(sender As Object, e As EventArgs) Handles BtnApply.Click
        ApplyChanges()
    End Sub

    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOK.Click
        ApplyChanges()
        Me.Close()
    End Sub

    Private Sub CmbUpdateType_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbUpdateType.SelectedIndexChanged
        Dim comboBox As ComboBox = CType(sender, ComboBox)
        EarthRotationDataUpdateType = CType(comboBox.SelectedItem, String)
        EnableControlsAsRequired()
    End Sub

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
                TxtCurrentLeapSeconds.Enabled = False
                TxtNextLeapSeconds.Enabled = False
                TxtNextLeapSecondsDate.Enabled = False
                LblCurrentLeapSeconds.Enabled = False
                LblNextLeapSeconds.Enabled = False
                LblNextLeapSecondsDate.Enabled = False
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
                TxtCurrentLeapSeconds.Enabled = False
                TxtNextLeapSeconds.Enabled = False
                TxtNextLeapSecondsDate.Enabled = False
                LblCurrentLeapSeconds.Enabled = False
                LblNextLeapSeconds.Enabled = False
                LblNextLeapSecondsDate.Enabled = False
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
                TxtCurrentLeapSeconds.Enabled = False
                TxtNextLeapSeconds.Enabled = False
                TxtNextLeapSecondsDate.Enabled = False
                LblCurrentLeapSeconds.Enabled = False
                LblNextLeapSeconds.Enabled = False
                LblNextLeapSecondsDate.Enabled = False
                TxtLastRun.Enabled = False
                LblTraceEnabled.Enabled = False
                LblLastRun.Enabled = False
                LblRunStatus.Enabled = False
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
                TxtCurrentLeapSeconds.Enabled = True
                TxtNextLeapSeconds.Enabled = True
                TxtNextLeapSecondsDate.Enabled = True
                LblCurrentLeapSeconds.Enabled = True
                LblNextLeapSeconds.Enabled = True
                LblNextLeapSecondsDate.Enabled = True
                TxtLastRun.Enabled = True
                LblTraceEnabled.Enabled = True
                LblLastRun.Enabled = True
                LblRunStatus.Enabled = True
            Case Else
                MsgBox("Unknown EarthRotationDataUpdateType: " & EarthRotationDataUpdateType)
        End Select
        GrpAutomaticUpdate.Refresh()
        GrpManualUpdate.Refresh()
        GrpUpdateType.Refresh()
        GrpStatus.Refresh()
    End Sub

    Private Sub TxtDownloadTimeout_Validating(sender As Object, e As KeyEventArgs) Handles TxtDownloadTimeout.KeyUp
        Dim DoubleValue As Double = 0.0

        Dim IsDouble = Double.TryParse(TxtDownloadTimeout.Text, DoubleValue)
        If IsDouble And DoubleValue > 0.1 Then
            ErrorProvider1.SetError(TxtDownloadTimeout, "")
            BtnOK.Enabled = True
        Else
            BtnOK.Enabled = False
            ErrorProvider1.SetError(TxtDownloadTimeout, "Must be a number > 0.1!")
        End If
    End Sub

    Private Sub TxtLeapSeconds_Validating(sender As Object, e As KeyEventArgs) Handles TxtManualLeapSeconds.KeyUp
        Dim IntValue As Integer = 0.0

        Dim IsInt = Integer.TryParse(TxtManualLeapSeconds.Text, IntValue)
        If IsInt And IntValue >= 37 Then
            ErrorProvider1.SetError(TxtManualLeapSeconds, "")
            BtnCancel.Enabled = True
        Else
            BtnCancel.Enabled = False
            ErrorProvider1.SetError(TxtManualLeapSeconds, "Must be an integer >= 37!")
        End If
    End Sub

    Private Sub CmbDataSource_Validating(sender As Object, e As KeyEventArgs) Handles CmbDataSource.KeyUp
        ValidateURI(sender)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbDataSource.SelectedIndexChanged
        ValidateURI(sender)
    End Sub

    Private Sub ValidateURI(sender As Object)
        Dim combo As ComboBox = CType(sender, ComboBox), UriValid As Boolean

        UriValid = False ' Set the valid flag false, then set to true if the download source starts with a supported URI prefix
        If combo.Text.StartsWith(URI_PREFIX_HTTP, StringComparison.OrdinalIgnoreCase) Then UriValid = True
        If combo.Text.StartsWith(URI_PREFIX_HTTPS, StringComparison.OrdinalIgnoreCase) Then UriValid = True
        If combo.Text.StartsWith(URI_PREFIX_FTP, StringComparison.OrdinalIgnoreCase) Then UriValid = True
        If UriValid Then
            ErrorProvider1.SetError(CmbDataSource, "")
            BtnOK.Enabled = True
        Else
            BtnOK.Enabled = False
            ErrorProvider1.SetError(CmbDataSource, "Must start with http:// or https:// or ftp://")
        End If
    End Sub

    Private Sub TxtDeltaUT1Manuals_Validating(sender As Object, e As KeyEventArgs) Handles TxtManualDeltaUT1.KeyUp
        Dim DoubleValue As Double = 0.0
        Const DELTAUT1_ACCEPTABLE_RANGE As Double = 0.9

        Dim IsDouble = Double.TryParse(TxtManualDeltaUT1.Text, DoubleValue)
        If IsDouble And (DoubleValue >= -DELTAUT1_ACCEPTABLE_RANGE) And (DoubleValue <= +DELTAUT1_ACCEPTABLE_RANGE) Then
            ErrorProvider1.SetError(TxtManualDeltaUT1, "")
            BtnOK.Enabled = True
        Else
            BtnOK.Enabled = False
            ErrorProvider1.SetError(TxtManualDeltaUT1, String.Format("Must be in the range -{0} to +{0}!", DELTAUT1_ACCEPTABLE_RANGE))
        End If
    End Sub

    Private Sub GroupBox_Paint(sender As Object, e As PaintEventArgs) Handles GrpAutomaticUpdate.Paint, GrpManualUpdate.Paint, GrpUpdateType.Paint, GrpStatus.Paint
        Const HEIGHT_OFFSET As Integer = 8
        Const WIDTH_OFFSET As Integer = 1
        Const PEN_WIDTH As Integer = 1

        Dim thisControl As GroupBox, gfx As Graphics, p As Pen, tSize As Size, activeBorder, inactiveBorder, borderColour As Color

        activeBorder = SystemColors.Highlight
        inactiveBorder = Color.LightGray

        thisControl = CType(sender, GroupBox)
        gfx = e.Graphics
        tSize = TextRenderer.MeasureText(thisControl.Text, thisControl.Font)

        If sender Is GrpAutomaticUpdate Then
            Select Case EarthRotationDataUpdateType
                Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1
                    borderColour = inactiveBorder
                Case UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1
                    borderColour = inactiveBorder
                Case UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1
                    borderColour = inactiveBorder
                Case UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
                    borderColour = activeBorder
                Case Else
                    borderColour = Color.Red ' Warning colour that somethinhg has gone wrong in the code!
            End Select
            LblAutoDataSource.Enabled = borderColour = activeBorder
            LblAutoDownloadTime.Enabled = borderColour = activeBorder
            LblAutoRepeatFrequency.Enabled = borderColour = activeBorder
            LblAutoTimeout.Enabled = borderColour = activeBorder
            LblAutoSeconds.Enabled = borderColour = activeBorder
        ElseIf sender Is GrpManualUpdate Then
            Select Case EarthRotationDataUpdateType
                Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1
                    borderColour = inactiveBorder
                Case UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1
                    borderColour = activeBorder
                Case UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1
                    borderColour = activeBorder
                Case UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
                    borderColour = inactiveBorder
                Case Else
                    borderColour = Color.Red ' Warning colour that somethinhg has gone wrong in the code!
            End Select
            LblManualDeltaUT1.Enabled = borderColour = activeBorder
            LblManualLeapSeconds.Enabled = borderColour = activeBorder

        Else
            borderColour = activeBorder
        End If

        p = New Pen(borderColour, PEN_WIDTH)
        thisControl.ForeColor = borderColour

        gfx.DrawLine(p, 0, HEIGHT_OFFSET, 0, e.ClipRectangle.Height - 2) ' left vertical
        gfx.DrawLine(p, 0, HEIGHT_OFFSET, 7, HEIGHT_OFFSET) ' Top left part
        gfx.DrawLine(p, tSize.Width + 7, HEIGHT_OFFSET, e.ClipRectangle.Width - WIDTH_OFFSET, HEIGHT_OFFSET) ' Top right part
        gfx.DrawLine(p, e.ClipRectangle.Width - WIDTH_OFFSET, HEIGHT_OFFSET, e.ClipRectangle.Width - WIDTH_OFFSET, e.ClipRectangle.Height - 2) ' Right vertical
        gfx.DrawLine(p, e.ClipRectangle.Width - WIDTH_OFFSET, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2) ' Bottom

    End Sub

    Private Sub BtnSetTraceDirectory_Click(sender As Object, e As EventArgs) Handles BtnSetTraceDirectory.Click
        Dim result As DialogResult
        FolderBrowser.RootFolder = Environment.SpecialFolder.Desktop
        FolderBrowser.SelectedPath = Parameters.DownloadTaskTracePath
        result = FolderBrowser.ShowDialog()
        If result = DialogResult.OK Then
            Parameters.DownloadTaskTracePath = FolderBrowser.SelectedPath
            TxtTraceFilePath.Text = FolderBrowser.SelectedPath
        End If
    End Sub

    Private Sub BtnRunAutomaticUpdate_Click(sender As Object, e As EventArgs) Handles BtnRunAutomaticUpdate.Click
        Dim psi As ProcessStartInfo, proc As Process, CancelButtonState, OKButtonState, UpdateCompleted As Boolean, RunTimer As Stopwatch
        Try
            RunTimer = New Stopwatch()
            CancelButtonState = BtnCancel.Enabled ' Save the current button enabled states so they can be restored later
            OKButtonState = BtnOK.Enabled
            LogRunMessage(String.Format("Cancel button state: {0}, OK button state: {1}", CancelButtonState, OKButtonState))
            BtnCancel.Enabled = False
            BtnOK.Enabled = False
            BtnRunAutomaticUpdate.Enabled = False

            LogRunMessage(String.Format("Creating process info"))
            psi = New ProcessStartInfo()
            psi.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & DOWNLOAD_EXECUTABLE_NAME
            psi.Arguments = "/datasource " & CmbDataSource.SelectedItem.ToString()
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

            If UpdateCompleted Then
                LogRunMessage(String.Format("Job completed OK in {0} seconds.", RunTimer.Elapsed.TotalSeconds.ToString("0.0")))
            Else
                LogRunMessage(String.Format("Job timed out after {0} seconds, data not updated", RunTimer.Elapsed.TotalSeconds.ToString("0.0")))
                LogRunMessage(String.Format("Killing process"))
                Try
                    proc.Kill()
                Catch ex As Exception
                    LogRunMessage("Exception killing process: " & ex.ToString())
                End Try
            End If

        Catch ex As Exception
            TL.LogMessageCrLf("RunAutomaticUpdate", "Exception running process: " & ex.ToString())
        Finally
            BtnCancel.Enabled = CancelButtonState ' Ensure that the original button states are restored
            BtnOK.Enabled = OKButtonState
            BtnRunAutomaticUpdate.Enabled = True
            UpdateStatus()
        End Try
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

    Private Sub TimerEventProcessor(myObject As Object, ByVal myEventArgs As EventArgs) Handles NowTimer.Tick
        Dim DisplayDate As DateTime, jdUtc As Double

        ' Calaculate the display date, allowing for development test offsets if present. In production offsets wil be 0 so DisplayDate will have a value of DateTime.Now as a UTC
        DisplayDate = DateTime.UtcNow.Subtract(New TimeSpan(TEST_UTC_DAYS_OFFSET, TEST_UTC_HOURS_OFFSET, TEST_UTC_MINUTES_OFFSET, 0))
        TxtNow.Text = String.Format("{0} {1}", DisplayDate.ToString(DOWNLOAD_TASK_TIME_FORMAT), DisplayDate.Kind.ToString().ToUpperInvariant())

        jdUtc = aUtils.JulianDateUtc
        TxtEffectiveDeltaUT1.Text = aUtils.DeltaUT(jdUtc).ToString("0.000")

        TxtEffectiveLeapSeconds.Text = aUtils.LeapSeconds.ToString()

    End Sub

    Private Sub LogRunMessage(message As String)
        TL.LogMessageCrLf("RunAutomaticUpdate", message)
        TxtRunStatus.Text = message
    End Sub

    Private Sub UpdateStatus()

        Parameters.RefreshState() ' Make sure we have the latest values, in case any have been updated
        TxtLastRun.Text = Parameters.EarthRotationDataLastUpdatedString
        TxtCurrentLeapSeconds.Text = Parameters.AutomaticLeapSecondsString
        TxtNextLeapSeconds.Text = Parameters.NextLeapSecondsString
        TxtNextLeapSecondsDate.Text = Parameters.NextLeapSecondsDateString & IIf((Parameters.NextLeapSecondsDateString = GlobalItems.NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE) Or (Parameters.NextLeapSecondsDateString = GlobalItems.NEXT_LEAP_SECONDS_DATE_DEFAULT), "", "UTC")
        TxtLastRun.Text = Parameters.EarthRotationDataLastUpdatedString

    End Sub

End Class