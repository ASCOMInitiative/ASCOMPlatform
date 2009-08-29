Attribute VB_Name = "FocuserHW"
' -----------------------------------------------------------------------------
' ==============
'  FocuserHW.BAS
' ==============
'
' Focuser hardware abstraction layer.
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     -----------------------------------------------------------
' 02-Sep-06 jab     Initial edit
' 12-Sep-06 jab     Simulate a friendly name since Name property not part of spec
' -----------------------------------------------------------------------------

Option Explicit

Public Sub FocuserCreate(ID As String)
    Dim dotPos As Long
  
    If g_Focuser Is Nothing Then
    
        g_sFocuserName = "(None)"
        
        If ID = "" Then _
            Err.Raise SCODE_NO_FOCUSER, App.Title, _
                "No Focuser. " & MSG_NO_FOCUSER
                
        If Not g_bForceLate Then
            On Error Resume Next
            Set g_IFocuser = CreateObject(ID)
            Set g_Focuser = g_IFocuser
            On Error GoTo 0
        End If
        
        If g_Focuser Is Nothing Then _
            Set g_Focuser = CreateObject(ID)
        
        If g_Focuser Is Nothing Then _
            Err.Raise SCODE_NO_FOCUSER, App.Title, MSG_NO_FOCUSER
                
        
        On Error Resume Next

        ' Name property does not exist for focusers
        '     g_sFocuserName = Trim(g_Focuser.Name)
        ' Just use leading part of ID (drop the ".Focuser")

        dotPos = InStr(1, ID, ".Focuser", vbTextCompare)
        If dotPos > 0 Then _
            g_sFocuserName = Left(ID, dotPos - 1)
            
        g_bFocuserConnected = g_Focuser.Link
        g_bFocuserManual = False
            
        On Error GoTo 0
    
    End If
    
End Sub

Public Sub FocuserDelete()

    On Error Resume Next
    
    If Not g_Focuser Is Nothing Then
        Set g_Focuser = Nothing
        Set g_IFocuser = Nothing
    End If
    
    g_bFocuserConnected = False
    g_bFocuserManual = False
    
    On Error GoTo 0
    
End Sub

Public Sub FocuserSave()

    g_FocuserProfile.WriteValue g_sFOCUSER, "FocuserID", g_sFocuserID
    g_FocuserProfile.WriteValue g_sFOCUSER, "FocuserName", g_sFocuserName
    
End Sub

