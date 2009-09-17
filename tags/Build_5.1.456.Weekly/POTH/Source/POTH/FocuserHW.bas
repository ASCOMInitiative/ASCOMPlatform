Attribute VB_Name = "FocuserHW"
' -----------------------------------------------------------------------------
'  =============
'  FocuserHW.BAS
'  =============
'
' Focuser hardware abstraction layer.
'
' Written: Pierre de Ponthière
'          email pierredeponthiere@gmail.com
'          Website http://users.skynet.be/dppobservatory/
'
' Edits:
'
' When      Who     What
' --------- ---     -----------------------------------------------------------
' 10-Jan-06 dpp     Initial edit
' 31-Aug-06 jab     write out the registry during Clean
' 10-Sep-06 jab     some changes to handle new focuser gui/functionality
' 12-Sep-06 jab     Simulate a friendly name since Name property not part of spec
' -----------------------------------------------------------------------------
Option Explicit

Public Sub FocuserClean(clear As Boolean)
    
    g_bFocuserConnected = False
    g_bFocuserTempProbe = False
    g_bFocuserTempCompAvailable = False
    g_bFocuserAbsolute = False
    
    If clear Then
        g_lFocuserPosition = CLng(g_FocuserProfile.GetValue(IDFOCUSER, "FocuserRelativePosition"))
        g_lFocuserIncrement = CLng(g_FocuserProfile.GetValue(IDFOCUSER, "FocuserRelativeIncrement"))
        g_lFocuserMaxIncrement = CLng(g_FocuserProfile.GetValue(IDFOCUSER, "FocuserMaxIncrement"))
        g_lFocuserMaxStep = CLng(g_FocuserProfile.GetValue(IDFOCUSER, "FocuserMaxStep"))
        g_dFocuserStepSizeInMicrons = val(g_FocuserProfile.GetValue(IDFOCUSER, "FocuserStepSize"))
        g_bFocuserAbsMove = CBool(g_FocuserProfile.GetValue(IDFOCUSER, "FocuserAbsMove"))
        g_bFocuserMoveMicrons = CBool(g_FocuserProfile.GetValue(IDFOCUSER, "FocuserMoveMicrons"))
    End If

End Sub

Public Sub FocuserCreate(ID As String)
    Dim dotPos As Long
  
    If g_Focuser Is Nothing Then
    
        If ID = "" Then _
            Err.Raise SCODE_NO_FOCUSER, ERR_SOURCE, _
                "No Focuser. " & MSG_NO_FOCUSER
                
        If Not g_bForceLate Then
            On Error Resume Next
            Set g_IFocuser = CreateObject(ID)
            Set g_Focuser = g_IFocuser
            On Error GoTo 0
        End If
        
        If g_Focuser Is Nothing Then _
            Set g_Focuser = CreateObject(ID)
    
        If g_Focuser Is Nothing Then
            g_handBox.ErrorLEDFocuser True
        Else
            g_handBox.ErrorLEDFocuser False
        End If
        
        g_sFocuserName = "(None)"
        
        On Error Resume Next

        ' Name property does not exist for focusers
        '     g_sFocuserName = Trim(g_Focuser.Name)
        ' Just use leading part of ID (drop the ".Focuser")

        dotPos = InStr(1, ID, ".Focuser", vbTextCompare)
        If dotPos > 0 Then _
            g_sFocuserName = Left(ID, dotPos - 1)
            
        On Error GoTo 0
    
    End If
    
End Sub

Public Sub FocuserConnected()
    
    On Error GoTo ErrorHandler
    
    If g_bFocuserConnected = g_Focuser.Link Then _
        Exit Sub
        
ErrorHandler:

    If g_bFocuserConnected Then
        g_handBox.ErrorLEDFocuser True
        g_setupDlg.ConnectFocuser False
    End If
    
    On Error GoTo 0

End Sub

Public Sub FocuserDelete()

    On Error Resume Next
    
    If Not g_Focuser Is Nothing Then
        
        If g_bFocuserConnected Then _
            g_Focuser.Link = False
        g_bFocuserConnected = False
        Set g_Focuser = Nothing
        Set g_IFocuser = Nothing
         
    End If
    
    On Error GoTo 0
    
End Sub

Public Sub FocuserSave()

    g_handBox.SaveFocusMove

    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserID", g_sFocuserID
    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserName", g_sFocuserName
    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserRelativePosition", Str(g_lFocuserPosition)
    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserRelativeIncrement", Str(g_lFocuserIncrement)
    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserMaxIncrement", Str(g_lFocuserMaxIncrement)
    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserMaxStep", Str(g_lFocuserMaxStep)
    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserStepSize", Str(g_dFocuserStepSizeInMicrons)
    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserAbsMove", CStr(g_bFocuserAbsMove)
    g_FocuserProfile.WriteValue IDFOCUSER, "FocuserMoveMicrons", CStr(g_bFocuserMoveMicrons)

End Sub
