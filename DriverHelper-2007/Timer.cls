VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Timer"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM DriverHelper Timer object"
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
' 28-Jan-01 rbd     Initial edit
' 15-Mar-01 rbd     Unload the Timer form, prevents app exit!
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 01-Jan-07 rbd     5.0.1 - Version change for Platform 5.0
'---------------------------------------------------------------------
Option Explicit

Implements IObjectSafety

Private WithEvents m_frmTimer As TimerForm
Attribute m_frmTimer.VB_VarHelpID = -1
Private m_fSafeForScripting As Boolean

Public Event Tick()

' ------------------------
' Constructor / Destructor
' ------------------------
'
Private Sub Class_Initialize()

    Set m_frmTimer = New TimerForm              ' Create a timer form
    m_frmTimer.Timer1.Enabled = False           ' Default settings
    m_frmTimer.Timer1.Interval = 1000
    m_fSafeForScripting = True
    
End Sub

Private Sub Class_Terminate()

    m_frmTimer.Timer1.Enabled = False
    Unload m_frmTimer
    Set m_frmTimer = Nothing
    
End Sub

' ----------
' Properties
' ----------
'
Public Property Get Interval() As Long

    Interval = m_frmTimer.Timer1.Interval
    
End Property

Public Property Let Interval(ms As Long)

    m_frmTimer.Timer1.Interval = ms
    
End Property

Public Property Get Enabled() As Boolean

    Enabled = m_frmTimer.Timer1.Enabled
    
End Property

Public Property Let Enabled(yn As Boolean)

    m_frmTimer.Timer1.Enabled = yn
    
End Property

' ---------------
' Event Reflector
' ---------------
'
Private Sub m_frmTimer_Tick()

    RaiseEvent Tick
    
End Sub

' -------------------------------
' Implementation of IObjectSafety
' -------------------------------
'
Private Sub IObjectSafety_GetInterfaceSafetyOptions(ByVal riid As Long, _
                                                    ByRef pdwSupportedOptions As Long, _
                                                    ByRef pdwEnabledOptions As Long)

    Call GetInterfaceSafetyOptions(riid, _
                                pdwSupportedOptions, _
                                pdwEnabledOptions, _
                                m_fSafeForScripting)
                            
End Sub

Private Sub IObjectSafety_SetInterfaceSafetyOptions(ByVal riid As Long, _
                                                    ByVal dwOptionsSetMask As Long, _
                                                    ByVal dwEnabledOptions As Long)

    Call SetInterfaceSafetyOptions(riid, _
                                dwOptionsSetMask, _
                                dwEnabledOptions, _
                                m_fSafeForScripting)

End Sub


