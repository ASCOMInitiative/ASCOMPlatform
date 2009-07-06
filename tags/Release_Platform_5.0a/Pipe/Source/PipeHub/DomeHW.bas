Attribute VB_Name = "DomeHW"
' -----------------------------------------------------------------------------
' ==============
'  Hardware.BAS
' ==============
'
' Dome hardware abstraction layer.
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     -----------------------------------------------------------
' 02-Sep-06 jab     Initial edit
' -----------------------------------------------------------------------------

Option Explicit

Public Sub DomeCreate(ID As String)
  
    If g_Dome Is Nothing Then
    
        g_sDomeName = "(None)"
        
        If ID = "" Then _
            Err.Raise SCODE_NO_DOME, App.Title, _
                "No Dome. " & MSG_NO_DOME
                
        If Not g_bForceLate Then
            On Error Resume Next
            Set g_IDome = CreateObject(ID)
            Set g_Dome = g_IDome
            On Error GoTo 0
        End If
        
        If g_Dome Is Nothing Then _
            Set g_Dome = CreateObject(ID)
        
        If g_Dome Is Nothing Then _
            Err.Raise SCODE_NO_DOME, App.Title, MSG_NO_DOME
        
        On Error Resume Next
            g_sDomeName = Trim(g_Dome.Name)
            g_bDomeConnected = g_Dome.Connected
            g_bDomeManual = False
        On Error GoTo 0
        
    End If
    
End Sub

Public Sub DomeDelete()
  
    On Error Resume Next
    
    If Not g_Dome Is Nothing Then
        Set g_Dome = Nothing
        Set g_IDome = Nothing
    End If
    
    g_bDomeConnected = False
    g_bDomeManual = False
    
    On Error GoTo 0
    
End Sub

Public Sub DomeSave()

    g_DomeProfile.WriteValue g_sDOME, "DomeID", g_sDomeID
    g_DomeProfile.WriteValue g_sDOME, "DomeName", g_sDomeName
   
End Sub

