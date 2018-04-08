'Implements the Timer component

Option Strict On
Option Explicit On
Imports ASCOM.Utilities.Interfaces
Imports System.Runtime.InteropServices
Imports System

''' <summary>
''' Provides a repeating timer with associated tick event.
''' </summary>
''' <remarks>
''' <para>The interval resolution is about 20ms.If you need beter than this, you could use the WaitForMilliseconds 
''' method to create your own solution.</para>
''' <para>You can create multiple instances of this object. When enabled, the Timer delivers Tick events periodically 
''' (determined by setting the Interval property).</para>
''' <para>This component is now considered <b>obsolete</b> for use in .NET clients and drivers. It is reliable under almost 
''' all circumstances but there are some environments, noteably console and some scripted applications, where it fails to fire.
''' The Platform 6 component improves performance over the Platform 5 component in this respect and can be further tuned 
''' for particular applications by placing an entry in the ForceSystemTimer Profile key.</para>
''' <para>For .NET applications, use of System.Timers.Timer is recommended but atention must be paid to getting threading correct
''' when using this control. The Windows.Forms.Timer control is not an improvement over the ASCOM timer which is based upon it.</para>
''' <para>Developers using non .NET languages are advised to use timers provided as part of their development environment, only falling 
''' back to the ASCOM Timer if no viable alternative can be found.</para>
''' </remarks>
<Guid("64FEE414-176D-44d0-99DF-47621D9C377F"), _
ComVisible(True), _
ComSourceInterfaces(GetType(ITimerEvent)), _
ClassInterface(ClassInterfaceType.None), _
Obsolete("Please replace it with Systems.Timers.Timer, which is reliable in all console and non-windowed applications.", False)> _
Public Class [Timer]
    Implements ITimer, IDisposable
    '   =========
    '   TIMER.CLS
    '   =========
    '
    ' Implementation of the ASCOM DriverHelper Timer class.
    '
    ' Written:  28-Jan-01   Robert B. Denny <rdenny@dc3.com>
    '
    ' Edits:
    '
    ' When      Who     What
    ' --------- ---     --------------------------------------------------
    ' 03-Feb-09 pwgs    5.1.0 - refactored for Utilities
    '---------------------------------------------------------------------

    'Set up a timer to create the tick events. Use a FORMS timer so that it will fire on the current owner's thread
    'If you use a system timer it will  fire on its own thread and this will be invisble to the application!
    'Private WithEvents m_Timer As System.Windows.Forms.Timer
    Private WithEvents FormTimer As Windows.Forms.Timer
    Private WithEvents TimersTimer As System.Timers.Timer

    Private IsForm, TraceEnabled As Boolean
    Private TL As TraceLogger

    ''' <summary>
    ''' Timer tick event handler
    ''' </summary>
    ''' <remarks></remarks>
    <ComVisible(False)> _
    Public Delegate Sub TickEventHandler()

    ''' <summary>
    ''' Fired once per Interval when timer is Enabled.
    ''' </summary>
    ''' <remarks>To sink this event in Visual Basic, declare the object variable using the WithEvents keyword.</remarks>
    Public Event Tick As TickEventHandler 'Implements ITimer.Tick ' Declare the tick event

#Region "New and IDisposable Support"
    ''' <summary>
    ''' Create a new timer component
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        TL = New TraceLogger("", "Timer")
        TraceEnabled = GetBool(TRACE_TIMER, TRACE_TIMER_DEFAULT)
        TL.Enabled = TraceEnabled

        TL.LogMessage("New", "Started on thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
        FormTimer = New Windows.Forms.Timer
        TL.LogMessage("New", "Created FormTimer")
        FormTimer.Enabled = False ' Default settings
        FormTimer.Interval = 1000 ' Inital period - 1 second
        TL.LogMessage("New", "Set FormTimer interval")

        TimersTimer = New System.Timers.Timer
        TL.LogMessage("New", "Created TimersTimer")

        TimersTimer.Enabled = False ' Default settings
        TimersTimer.Interval = 1000 ' Inital period - 1 second
        TL.LogMessage("New", "Set TimersTimer interval")

        Try
            TL.LogMessage("New", "Process FileName " & """" & Process.GetCurrentProcess().MainModule.FileName & """")
            Dim PE As New PEReader(Process.GetCurrentProcess().MainModule.FileName, TL)
            TL.LogMessage("New", "SubSystem " & PE.SubSystem.ToString)
            Select Case PE.SubSystem
                Case PEReader.SubSystemType.WINDOWS_GUI ' Windows GUI app
                    IsForm = True
                Case PEReader.SubSystemType.WINDOWS_CUI 'Windows Console app
                    IsForm = False
                Case Else 'Unknown app type
                    IsForm = False

            End Select
            ' If Process.GetCurrentProcess.MainModule.FileName.ToUpper.Contains("WSCRIPT.EXE") Then 'WScript is an exception that is marked GUI but behaves like console!
            ' TL.LogMessage("New", "WScript.Exe found - Overriding IsForm to: False")
            ' IsForm = False
            ' End If
            'If Process.GetCurrentProcess.MainModule.FileName.ToUpper.Contains("ASTROART.EXE") Then 'WScript is an exception that is marked GUI but behaves like console!
            ' TL.LogMessage("New", "AstroArt.Exe found - Overriding IsForm to: False")
            ' IsForm = False
            ' End If
            IsForm = Not ForceTimer(IsForm) 'Override the value of isform if required
            TL.LogMessage("New", "IsForm: " & IsForm)
        Catch ex As Exception
            TL.LogMessageCrLf("New Exception", ex.ToString) 'Log error and record in the event log
            LogEvent("Timer:New", "Exception", EventLogEntryType.Error, EventLogErrors.TimerSetupException, ex.ToString)
        End Try
    End Sub

    Private Function ForceTimer(ByVal CurrentIsForm As Boolean) As Boolean
        Dim Profile As New RegistryAccess, ForcedSystemTimers As Generic.SortedList(Of String, String)
        Dim ProcessFileName As String, ForceSystemTimer, MatchedName As Boolean

        ForceTimer = Not CurrentIsForm 'Set up default return value to supplied value. ForceTimer is opposite logic to IsForm, hence use of Not
        TL.LogMessage("ForceTimer", "Current IsForm: " & CurrentIsForm.ToString & ", this makes the default ForceTimer value: " & ForceTimer)

        ProcessFileName = Process.GetCurrentProcess.MainModule.FileName.ToUpperInvariant 'Get the current process processname
        TL.LogMessage("ForceTimer", "Main process file name: " & ProcessFileName)

        MatchedName = False
        ForcedSystemTimers = Profile.EnumProfile(FORCE_SYSTEM_TIMER) 'Get the list of applications requiring special timer handling
        For Each ForcedFileName As Generic.KeyValuePair(Of String, String) In ForcedSystemTimers ' Check each forced file in turn 
            If ProcessFileName.Contains(Trim(ForcedFileName.Key.ToUpperInvariant)) Then ' We have matched the filename
                TL.LogMessage("ForceTimer", "  Found: """ & ForcedFileName.Key & """ = """ & ForcedFileName.Value & """")
                MatchedName = True
                If Boolean.TryParse(ForcedFileName.Value, ForceSystemTimer) Then
                    ForceTimer = ForceSystemTimer
                    TL.LogMessage("ForceTimer", "    Parsed OK: " & ForceTimer.ToString & ", ForceTimer set to: " & ForceTimer)
                Else
                    TL.LogMessage("ForceTimer", "    ***** Error - Value is not boolean!")
                End If
            Else
                TL.LogMessage("ForceTimer", "  Tried: """ & ForcedFileName.Key & """ = """ & ForcedFileName.Value & """")
            End If
        Next
        If Not MatchedName Then TL.LogMessage("ForceTimer", "  Didn't match any force timer application names")

        TL.LogMessage("ForceTimer", "Returning: " & ForceTimer.ToString)
        Return ForceTimer
    End Function

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    ''' <summary>
    ''' Disposes of resources used by the profile object - called by IDisposable interface
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                TL.Enabled = False
                TL.Dispose()
            End If
            If Not (FormTimer Is Nothing) Then
                If Not FormTimer Is Nothing Then FormTimer.Enabled = False
                FormTimer.Dispose()
                FormTimer = Nothing
            End If

        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Disposes of resources used by the profile object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#Region "Timer Implementation"
    ''' <summary>
    ''' The interval between Tick events when the timer is Enabled in milliseconds, (default = 1000)
    ''' </summary>
    ''' <value>The interval between Tick events when the timer is Enabled (milliseconds, default = 1000)</value>
    ''' <returns>The interval between Tick events when the timer is Enabled in milliseconds</returns>
    ''' <remarks></remarks>
    Public Property Interval() As Integer Implements ITimer.Interval
        'Get and set the timer period
        Get
            If IsForm Then
                TL.LogMessage("Interval FormTimer Get", FormTimer.Interval.ToString & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
                Return FormTimer.Interval
            Else
                TL.LogMessage("Interval TimersTimer Get", TimersTimer.Interval.ToString & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
                Return FormTimer.Interval
            End If
        End Get
        Set(ByVal Value As Integer)
            If IsForm Then
                TL.LogMessage("Interval FormTimer Set", Value.ToString & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
                If Value > 0 Then
                    FormTimer.Interval = Value
                Else
                    FormTimer.Enabled = False
                End If
            Else
                TL.LogMessage("Interval TimersTimer Set", Value.ToString & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
                If Value > 0 Then
                    TimersTimer.Interval = Value
                Else
                    TimersTimer.Enabled = False
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Enable the timer tick events
    ''' </summary>
    ''' <value>True means the timer is active and will deliver Tick events every Interval milliseconds.</value>
    ''' <returns>Enabled state of timer tick events</returns>
    ''' <remarks></remarks>
    Public Property Enabled() As Boolean Implements ITimer.Enabled
        'Enable and disable the timer
        Get
            If IsForm Then
                TL.LogMessage("Enabled FormTimer Get", FormTimer.Enabled.ToString & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
                Return FormTimer.Enabled
            Else
                TL.LogMessage("Enabled TimersTimer Get", TimersTimer.Enabled.ToString & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
                Return TimersTimer.Enabled
            End If
        End Get
        Set(ByVal Value As Boolean)
            If IsForm Then
                TL.LogMessage("Enabled FormTimer Set", Value.ToString & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
                FormTimer.Enabled = Value
            Else
                TL.LogMessage("Enabled TimersTimer Set", Value.ToString & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString)
                TimersTimer.Enabled = Value
            End If
        End Set
    End Property
#End Region

#Region "Support code"
    'Raise the external event whenever a timer tick event occurs
    ''' <summary>
    ''' Timer event handler
    ''' </summary>
    ''' <remarks>Raises the Tick event</remarks>
    Private Sub OnTimedEvent(ByVal sender As Object, ByVal e As Object) Handles FormTimer.Tick, TimersTimer.Elapsed
        If IsForm Then
            Dim ec As System.EventArgs
            ec = CType(e, System.EventArgs)
        Else
            Dim ec As System.Timers.ElapsedEventArgs
            ec = CType(e, System.Timers.ElapsedEventArgs)
            If TraceEnabled Then TL.LogMessage("OnTimedEvent", "SignalTime: " & ec.SignalTime.ToString)

        End If
        If TraceEnabled Then TL.LogMessage("OnTimedEvent", "Raising Tick" & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString) 'Ensure minimum hit on timing under normal, non-trace conditions
        RaiseEvent Tick()
        If TraceEnabled Then TL.LogMessage("OnTimedEvent", "Raised Tick" & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString) 'Ensure minimum hit on timing under normal, non-trace, conditions
    End Sub
#End Region



    Private Sub Timer_Tick() Handles Me.Tick

    End Sub
End Class