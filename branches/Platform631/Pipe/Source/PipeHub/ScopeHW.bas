Attribute VB_Name = "ScopeHW"
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
' 01-Sep-06 jab     Initial edit
' -----------------------------------------------------------------------------

Option Explicit

Public Sub ScopeCreate(ID As String)
  
    If g_Scope Is Nothing Then
    
        g_sScopeName = "(None)"
        
        If ID = "" Then _
            Err.Raise SCODE_NO_SCOPE, App.Title, _
                "No Scope. " & MSG_NO_SCOPE
        
        If Not g_bForceLate Then
            On Error Resume Next
            Set g_IScope = CreateObject(ID)
            Set g_Scope = g_IScope
            On Error GoTo 0
        End If
        
        If g_Scope Is Nothing Then _
            Set g_Scope = CreateObject(ID)
        
        If g_Scope Is Nothing Then _
            Err.Raise SCODE_NO_SCOPE, App.Title, MSG_NO_SCOPE
               
        On Error Resume Next
            g_sScopeName = Trim(g_Scope.Name)
            g_bConnected = g_Scope.Connected
            g_bManual = False
        On Error GoTo 0
        
    End If
    
End Sub

Public Sub ScopeDelete()
      
    On Error Resume Next
    
    If Not g_Scope Is Nothing Then
        Set g_Scope = Nothing
        Set g_IScope = Nothing
    End If
    
    g_bConnected = False
    g_bManual = False
    
    On Error GoTo 0
    
End Sub

Public Sub ScopeSave()

    g_Profile.WriteValue g_sSCOPE, "ScopeID", g_sScopeID
    g_Profile.WriteValue g_sSCOPE, "ScopeName", g_sScopeName
   
End Sub

