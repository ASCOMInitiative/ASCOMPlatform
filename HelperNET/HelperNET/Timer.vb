Option Strict On
Option Explicit On
Imports ASCOM.HelperNET.Interfaces

''' <summary>
''' Provides a repeating timer with associated tick event.
''' </summary>
''' <remarks>
''' <para>The interval resolution is about 20ms.If you need beter than this, you could use the WaitForMilliseconds 
''' method to create your own solution.</para>
''' <para>You can create multiple instances of this object. When enabled, the Timer delivers Tick events periodically 
''' (determined by setting the Interval property).</para>
''' </remarks>
Public Class [Timer]
    Implements ITimer, IDisposable
    '---------------------------------------------------------------------
    ' Copyright © 2002 SPACE.com Inc., New York, NY
    '
    ' Permission is hereby granted to use this Software for any purpose
    ' including combining with commercial products, creating derivative
    ' works, and redistribution of source or binary code, without
    ' limitation or consideration. Any redistributed copies of this
    ' Software must include the above Copyright Notice.
    '
    ' THIS SOFTWARE IS PROVIDED "AS IS". SPACE.COM, INC. MAKES NO
    ' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
    ' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
    '---------------------------------------------------------------------
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
    ' 03-Feb-09 pwgs    5.1.0 - refactored for Helper.NET
    '---------------------------------------------------------------------

    'Set up a timer to create the tick events. Use a FORMS timer so that it will fire on the current owner's thread
    'If you use a system timer it will  fire on its own thread and this will be invisble to the application!
    Private WithEvents m_Timer As System.Windows.Forms.Timer
    ''' <summary>
    ''' Fired once per Interval when timer is Enabled.
    ''' </summary>
    ''' <remarks>To sink this event in Visual Basic, declare the object variable using the WithEvents keyword.</remarks>
    Public Event Tick() Implements ITimer.Tick ' Declare the tick event

#Region "New and IDisposable Support"
    ' ------------------------
    ' Constructor / Destructor
    ' ------------------------
    Public Sub New()
        m_Timer = New System.Windows.Forms.Timer ' Create a form timer
        m_Timer.Enabled = False ' Default settings
        m_Timer.Interval = 1000 ' Inital period - 1 second
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
            End If
            If Not (m_Timer Is Nothing) Then
                m_Timer.Enabled = False
                m_Timer.Dispose()
                m_Timer = Nothing
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
            Return m_Timer.Interval
        End Get
        Set(ByVal Value As Integer)
            If Value > 0 Then
                m_Timer.Interval = Value
            Else
                m_Timer.Enabled = False
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
            Return m_Timer.Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_Timer.Enabled = Value
        End Set
    End Property
#End Region

#Region "Support code"
    'Raise the external event whenever a timer tick event occurs
    Private Sub OnTimedEvent(ByVal source As Object, ByVal e As Object) Handles m_Timer.Tick
        RaiseEvent Tick()
    End Sub
#End Region

End Class