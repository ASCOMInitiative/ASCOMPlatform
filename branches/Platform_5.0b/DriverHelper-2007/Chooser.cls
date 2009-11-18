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
Attribute VB_Description = "ASCOM Driver Helper Chooser Object"
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
' 24-Aug-00 rbd     Initial edit
' 21-Jan-01 rbd     Moved into DriverHelper from separate component.
'                   EGAD! Forgot to mark parameter ByVal!
' 23-Jan-01 rbd     Make CurrentDriverID optional, implement
'                   IObjectSafety
' 08-Jun-01 rbd     Generalize Chooser for various devices
' 01-Jan-07 rbd     5.0.1 - Version change for Platform 5.0
' 05-Mar-07 rbd     5.0.2 - Call config layer to write XML on release
' 15-May-07 rbd     5.1.0 - New DeviceTypeV property that Lets by
'                   VALUE not REF! 5.0.3 is now old Helper with 'V
'---------------------------------------------------------------------
Option Explicit

Implements IObjectSafety

Private m_frmChooser As ChooserForm
Private m_sDeviceType As String
Private m_fSafeForScripting As Boolean

Private Sub Class_Initialize()
    
    Set m_frmChooser = New ChooserForm          ' Initially hidden
    m_sDeviceType = "Telescope"                 ' Default to Telescope chooser
    m_fSafeForScripting = True

End Sub

Private Sub Class_Terminate()
    SaveConfig ERR_SOURCE_CHOOSER
End Sub

Public Property Get DeviceType() As String
Attribute DeviceType.VB_Description = "The type of device (e.g. ""Telescope"") to be chosen via the Chooser"

    DeviceType = m_sDeviceType
    
End Property

Public Property Let DeviceType(newVal As String)

    If newVal = "" Then _
        Err.Raise SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_CHOOSER, MSG_ILLEGAL_DEVTYPE
    m_sDeviceType = newVal
    
End Property

Public Property Get DeviceTypeV() As String

    DeviceType = m_sDeviceType
    
End Property

Public Property Let DeviceTypeV(ByVal newVal As String)

    If newVal = "" Then _
        Err.Raise SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_CHOOSER, MSG_ILLEGAL_DEVTYPE
    m_sDeviceType = newVal
    
End Property

Public Function Choose(Optional ByVal CurrentDriverID As String = "") As String
Attribute Choose.VB_Description = "Select ASCOM Telescope Driver to use, return Driver ID of chosen driver"

    m_frmChooser.DeviceType = m_sDeviceType
    m_frmChooser.StartSel = CurrentDriverID
    
'   -------------------
    m_frmChooser.Show 1                         ' -- MODAL --
'   -------------------

    Choose = m_frmChooser.Result
    
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
