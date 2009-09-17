Attribute VB_Name = "ObjectSafety"
'---------------------------------------------------------------------
' Copyright © 2000-2001 SPACE.com Inc., New York, NY
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
'   ================
'   OBJECTSAFETY.BAS
'   ================
'
' Implementation of IObjectSafety interface
'
' Written:  22-Jan-01   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 23-Jan-01 rbd     Initial edit
'---------------------------------------------------------------------
Option Explicit

Private Const IID_IDispatch As String = "{00020400-0000-0000-C000-000000000046}"

Private Const INTERFACESAFE_FOR_UNTRUSTED_CALLER = &H1
Private Const E_NOINTERFACE = &H80004002
Private Const E_FAIL = &H80004005
Private Const MAX_GUIDLEN = 40

Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
                        (pDest As Any, _
                        pSource As Any, _
                        ByVal ByteLen As Long)
Private Declare Function StringFromGUID2 Lib "ole32.dll" _
                        (rguid As Any, _
                        ByVal lpstrClsId As Long, _
                        ByVal cbMax As Integer) As Long

Private Type udtGUID
    Data1 As Long
    Data2 As Integer
    Data3 As Integer
    Data4(7) As Byte
End Type

Public Sub GetInterfaceSafetyOptions(ByVal riid As Long, _
                                pdwSupportedOptions As Long, _
                                pdwEnabledOptions As Long, _
                                ByVal fSafeForScripting As Boolean)
    Dim Rc      As Long
    Dim rClsId  As udtGUID
    Dim IID     As String
    Dim bIID()  As Byte

    pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER
    
    If (riid <> 0) Then
        CopyMemory rClsId, ByVal riid, Len(rClsId)
    
        bIID = String$(MAX_GUIDLEN, 0)
        Rc = StringFromGUID2(rClsId, VarPtr(bIID(0)), MAX_GUIDLEN)
        Rc = InStr(1, bIID, vbNullChar) - 1
        IID = Left$(UCase(bIID), Rc)
    
        Select Case IID
            Case IID_IDispatch
                pdwEnabledOptions = IIf(fSafeForScripting, _
                                        INTERFACESAFE_FOR_UNTRUSTED_CALLER, _
                                        0)
                Exit Sub
            Case Else
                Err.Raise E_NOINTERFACE
                Exit Sub
        End Select
    End If

End Sub


Public Sub SetInterfaceSafetyOptions(ByVal riid As Long, _
                                ByVal dwOptionsSetMask As Long, _
                                ByVal dwEnabledOptions As Long, _
                                ByVal fSafeForScripting As Boolean)
    Dim Rc          As Long
    Dim rClsId      As udtGUID
    Dim IID         As String
    Dim bIID()      As Byte

    If (riid <> 0) Then
        CopyMemory rClsId, ByVal riid, Len(rClsId)

        bIID = String$(MAX_GUIDLEN, 0)
        Rc = StringFromGUID2(rClsId, VarPtr(bIID(0)), MAX_GUIDLEN)
        Rc = InStr(1, bIID, vbNullChar) - 1
        IID = Left$(UCase(bIID), Rc)

        Select Case IID
            Case IID_IDispatch
                If ((dwEnabledOptions And dwOptionsSetMask) <> _
                                    INTERFACESAFE_FOR_UNTRUSTED_CALLER) Then
                    Err.Raise E_FAIL
                    Exit Sub
                Else
                    If Not fSafeForScripting Then
                        Err.Raise E_FAIL
                    End If
                    Exit Sub
                End If

            Case Else
                Err.Raise E_NOINTERFACE
                Exit Sub
        End Select
    End If
End Sub

