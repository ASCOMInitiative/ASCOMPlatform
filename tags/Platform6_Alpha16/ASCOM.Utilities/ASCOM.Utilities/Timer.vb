'Implements the Timer component

Option Strict On
Option Explicit On
Imports ASCOM.Utilities.Interfaces
Imports System.Runtime.InteropServices

''' <summary>
''' Provides a repeating timer with associated tick event.
''' </summary>
''' <remarks>
''' <para>The interval resolution is about 20ms.If you need beter than this, you could use the WaitForMilliseconds 
''' method to create your own solution.</para>
''' <para>You can create multiple instances of this object. When enabled, the Timer delivers Tick events periodically 
''' (determined by setting the Interval property).</para>
''' </remarks>
<Guid("64FEE414-176D-44d0-99DF-47621D9C377F"), _
ComVisible(True), _
ComSourceInterfaces(GetType(ITimerEvent)), _
ClassInterface(ClassInterfaceType.None)> _
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
            Dim PE As New PEReader(Process.GetCurrentProcess().MainModule.FileName)
            TL.LogMessage("New", "SubSystem " & PE.SubSystem.ToString)
            Select Case PE.SubSystem
                Case PEReader.SubSystemType.WINDOWS_GUI ' Windows GUI app
                    IsForm = True
                Case PEReader.SubSystemType.WINDOWS_CUI 'Windows Console app
                    IsForm = False
                Case Else 'Unknown app type
                    IsForm = False

            End Select
            If Process.GetCurrentProcess.MainModule.FileName.ToUpper.Contains("WSCRIPT.EXE") Then 'WScript is an exception that is marked GUI but behaves like console!
                TL.LogMessage("New", "WScript.Exe found - Overriding IsForm to: False")
                IsForm = False
            End If
            TL.LogMessage("New", "IsForm: " & IsForm)
        Catch ex As Exception
            TL.LogMessageCrLf("New Exception", ex.ToString) 'Log error and record in the event log
            LogEvent("Timer:New", "Exception", EventLogEntryType.Error, EventLogErrors.TimerSetupException, ex.ToString)
        End Try
    End Sub


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
            TL.LogMessage("OnTimedEvent", "SignalTime: " & ec.SignalTime.ToString)

        End If
        If TraceEnabled Then TL.LogMessage("OnTimedEvent", "Raising Tick" & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString) 'Ensure minimum hit on timing under normal, non-trace conditions
        RaiseEvent Tick()
        If TraceEnabled Then TL.LogMessage("OnTimedEvent", "Raised Tick" & ", Thread: " & Threading.Thread.CurrentThread.ManagedThreadId.ToString) 'Ensure minimum hit on timing under normal, non-trace, conditions
    End Sub
#End Region



End Class