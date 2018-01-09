VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Chooser"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Scope Driver Helper Chooser Object"
'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
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
'   ===========
'   CHOOSER.CLS
'   ===========
'
' Implementation of the ASCOM telescope driver Chooser class
'
' Written:  24-Aug-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 15-Mar-09 pwgs    5.1.0 - Refactored for HerlpNET
'---------------------------------------------------------------------
Option Explicit

Implements IObjectSafety

Private m_Chooser As Object
Private m_fSafeForScripting As Boolean

Private Sub Class_Initialize()
     On Error GoTo ERR100
     'MsgBox "DriverHelper Before Set m_Chooser Commented out"
  
     Set m_Chooser = CreateObject("DriverHelper.ChooserSupport") ' Get chooser object
     'MsgBox "DriverHelper After SEt m_Chooser"
     m_fSafeForScripting = True
     Exit Sub
   
ERR100:
     MsgBox "DriverHelper Class Init error " & Err.Description
     m_fSafeForScripting = True
 End Sub

Private Sub Class_Terminate()
    On Error Resume Next
    Call m_Chooser.Dispose
End Sub

Public Property Get DeviceType() As String

    DeviceType = m_Chooser.DeviceType
    
End Property

Public Property Let DeviceType(newVal As String)

    If newVal = "" Then _
        Err.Raise SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE
    m_Chooser.DeviceType = newVal
    
End Property

Public Property Get DeviceTypeV() As String

    DeviceTypeV = m_Chooser.DeviceType
    
End Property

Public Property Let DeviceTypeV(ByVal newVal As String)

    If newVal = "" Then _
        Err.Raise SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE
    m_Chooser.DeviceType = newVal
    
End Property

Public Function Choose(Optional ByVal CurrentDriverID As String = "") As String
Attribute Choose.VB_Description = "Select ASCOM Telescope Driver to use, return Driver ID of chosen driver"
On Error GoTo ERR1

    Choose = m_Chooser.Choose(CurrentDriverID)
Exit Function

ERR1:
MsgBox "DRIVERHELPER.CHOOSE ERROR " & Err.Description
    
    
End Function


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
