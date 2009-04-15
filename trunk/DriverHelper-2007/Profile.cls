VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Profile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Driver Helper Profile Object"
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
'
'   ===========
'   PROFILE.CLS
'   ===========
'
' Written:  21-Jan-01   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 21-Jan-01 rbd     Initial edit
' 22-Jan-01 rbd     Protect ASCOM root area and unnamed value at driver's root.
'                   Implement IObjectSafety
' 08-Jun-01 rbd     Add DeviceType to generalize for drivers for arbitrary
'                   device type. Default is Telescope.
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 30-Aug-04 rbd     4.0.1 - Add ASCOM Global access (DeviceType and DriverID
'                   empty strings)
' 01-Jan-07 rbd     5.0.1 - Version change for Platform 5.0
' 04-Apr-07 rbd     5.0.2 - IsRegistered raises other errors instead of lumping
'                   them all into "not registered" (false). Remove " Drivers"
'                   from device type level for new XML storage layer.
' 05-Apr-07 rbd     5.0.2 - Call config layer to write XML on release
' 06-Apr-07 rbd     LowerCase keys and value names to provide caseless
'                   behavior like the registry.
' 12-Apr-07 rbd     5.0.3 - Provide back door for callers to write above the
'                   driver key level. Used by Helper2.
' 15-May-07 rbd     5.1.0 - New DeviceTypeV property that Lets by
'                   VALUE not REF! 5.0.3 is now old Helper with 'V
' 09-Oct-07 rbd     5.1.1 - Allow SubKeys() to enumerate subkeys of current
'                   device type.
' -----------------------------------------------------------------------------
Option Explicit

Implements IObjectSafety

Private m_sDeviceType As String
Private m_fSafeForScripting As Boolean

Private Sub Class_Initialize()

    m_sDeviceType = "Telescope"
    m_fSafeForScripting = True

End Sub

Private Sub Class_Terminate()
    SaveConfig ERR_SOURCE_PROFILE
End Sub

Public Property Get DeviceType() As String
Attribute DeviceType.VB_Description = "The device type (e.g. ""Telescope"") to which subsequent Profile operations will apply"

    DeviceType = m_sDeviceType
    
End Property

Public Property Let DeviceType(newVal As String)

    If newVal = "" Then _
        Err.Raise SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE
    m_sDeviceType = newVal
    
End Property

Public Property Get DeviceTypeV() As String

    DeviceType = m_sDeviceType
    
End Property

Public Property Let DeviceTypeV(ByVal newVal As String)

    If newVal = "" Then _
        Err.Raise SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE
    m_sDeviceType = newVal
    
End Property

Public Function IsRegistered(ByVal DriverID As String) As Boolean
Attribute IsRegistered.VB_Description = "True if the driver with the given DriverID is registered."
    Dim buf As String
    Dim errCode As Long
    Dim errDesc As String
    
    IsRegistered = False                        ' Assume failure
    If DriverID = "" Then Exit Function         ' Nothing is a failure
    On Error GoTo IR_ERR
    GetProfile MakeKey(DriverID, ""), "", ERR_SOURCE_PROFILE ' Try to get friendly name
    IsRegistered = True                         ' Found it
    Exit Function
    
IR_ERR:
    errCode = Err.Number                        ' Capture error info
    errDesc = Err.Description
    Resume IR_RES                               ' Drop out of error state
IR_RES:
    On Error GoTo 0                             ' Re-arm raising
    If errCode = SCODE_NOEXIST_KEY Then         ' If the key really doesn't exist
        IsRegistered = False
        Exit Function                           ' Return False
    End If
    Err.Raise errCode, , errDesc                ' Else re-raise error
    
End Function

Public Sub Register(ByVal DriverID As String, ByVal DescriptiveName As String)
Attribute Register.VB_Description = "Register the driver with the given DriverID and friendly name. Does nothing if already registered, so safe to call on driver load."

    If Not Me.IsRegistered(DriverID) Then
        WriteProfile MakeKey(DriverID, ""), "", DescriptiveName, ERR_SOURCE_PROFILE
    End If
    
End Sub

Public Sub Unregister(ByVal DriverID As String)
Attribute Unregister.VB_Description = "Remove all data for the given DriverID . This deletes the entire tree, including the DriverID root key."

    CheckRegistered DriverID
    DeleteKey MakeKey(DriverID, ""), ERR_SOURCE_PROFILE
    
End Sub

Public Function GetValue(ByVal DriverID As String, ByVal Name As String, _
                            Optional ByVal SubKey As String = "") As String
Attribute GetValue.VB_Description = "Retrieve the named string value. Name may be """" for the unnamed value. Optional path to subkey for given Driver ID may be supplied."
    
    CheckRegistered DriverID
    GetValue = GetProfile(MakeKey(DriverID, SubKey), Name, ERR_SOURCE_PROFILE)
    
End Function

Public Sub WriteValue(ByVal DriverID As String, ByVal Name As String, _
                            ByVal Value As String, _
                            Optional ByVal SubKey As String = "")
Attribute WriteValue.VB_Description = "Write the value to the profile. Name may be """" for the unnamed value. Optional path to subkey for given Driver ID may be supplied, and key(s) will be created as needed."

    CheckRegistered DriverID
    If Name = "" And SubKey = "" Then
        Err.Raise SCODE_ILLEGAL_REGACC, ERR_SOURCE_PROFILE, _
                    MSG_ILLEGAL_REGACC
    End If
    WriteProfile MakeKey(DriverID, SubKey), Name, Value, ERR_SOURCE_PROFILE
    
End Sub

Public Function Values(ByVal DriverID As String, _
                            Optional ByVal SubKey As String = "") As Dictionary
Attribute Values.VB_Description = "Return a list of the (unnamed and named) under the given DriverID. Optional path to starting subkey for given Driver ID may be supplied."

    CheckRegistered DriverID
    Set Values = EnumProfile(MakeKey(DriverID, SubKey), ERR_SOURCE_PROFILE)

End Function

Public Sub DeleteValue(ByVal DriverID As String, ByVal Name As String, _
                            Optional ByVal SubKey As String = "")
Attribute DeleteValue.VB_Description = "Delete the value. Name may be """" for the unnamed value. Optional path to subkey for given Driver ID may be supplied."
                            
    CheckRegistered DriverID
    DeleteProfile MakeKey(DriverID, SubKey), Name, ERR_SOURCE_PROFILE
    
End Sub

Public Sub CreateSubKey(ByVal DriverID As String, ByVal SubKey As String)
Attribute CreateSubKey.VB_Description = "Create a key for the given DriverID. SubKey may contain ""\\"" separated path to key to be created."

    CheckRegistered DriverID
    CreateKey MakeKey(DriverID, SubKey), ERR_SOURCE_PROFILE
    
End Sub
Public Function SubKeys(ByVal DriverID As String, _
                            Optional ByVal SubKey As String = "") As Dictionary
Attribute SubKeys.VB_Description = "Return a list of the subkeys under the  given DriverID. Optional path to starting subkey for given Driver ID may be given."
                            
    If DriverID <> "" Then CheckRegistered DriverID
    Set SubKeys = EnumKeys(MakeKey(DriverID, SubKey), ERR_SOURCE_PROFILE)

End Function

Public Sub DeleteSubKey(ByVal DriverID As String, ByVal SubKey As String)
Attribute DeleteSubKey.VB_Description = "Delete a key for the given DriverID. SubKey may contain ""\\"" separated path to key to be deleted."

    CheckRegistered DriverID
    DeleteKey MakeKey(DriverID, SubKey), ERR_SOURCE_PROFILE
    
End Sub

Private Function MakeKey(BaseKey As String, SubKey As String) As String
    Dim buf As String
    
    buf = BaseKey
    If BaseKey = "#root#" Then
        MakeKey = ""                            ' Convert secret driver ID into root
    Else
        MakeKey = m_sDeviceType                 ' New XML, remove " Drivers"
        If BaseKey <> "" Then MakeKey = MakeKey & "\" & BaseKey ' Allow blank BaseKey (See SubKeys())
    End If
    If SubKey <> "" Then MakeKey = MakeKey & "\" & SubKey

End Function

Private Sub CheckRegistered(DriverID As String)

    If DriverID = "" Then
        Err.Raise SCODE_ILLEGAL_DRIVERID, ERR_SOURCE_PROFILE, _
            MSG_ILLEGAL_DRIVERID
    ElseIf DriverID = "#root#" Then             ' Secret root access "ID"
        Exit Sub
    ElseIf Not Me.IsRegistered(DriverID) Then
        Err.Raise SCODE_DRIVER_NOT_REG, ERR_SOURCE_PROFILE, _
            "DriverID " & DriverID & " is not registered."
    End If
    
End Sub

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

