Attribute VB_Name = "WindowsAPI"
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
'   =============
'   WINDOWS32.BAS
'   =============
'
' Utilities that use the Win32 API for at least part of their
' implementation. The Win32 API declarations are private here.
' The registry API is wrapped in a separate module.
'
' Written:  28-Jun-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 28-Jun-00 rbd     Initial edit, from the code in ACP.
' 03-Feb-01 rbd     Make GetTickCount() public
' 08-Feb-01 rbd     FloatWindow takes boolean to float/unfloat
' 11-Mar-01 rbd     Add more Get/Set WindowPosition constants for future
' 27-Jun-03 jab     Initial release of dome simulator
' -----------------------------------------------------------------------------

Option Explicit

'
' Used for persisting the handbox window's position
'
Public Type RECT
      Left As Long
      Top  As Long
      Right  As Long
      Bottom As Long
End Type
Public Declare Function GetWindowRect Lib "user32" (ByVal hwnd As Long, _
               ByRef lpRect As RECT) As Long

'
' Used to move and float window
'
Private Const HWND_TOP = 0
Private Const HWND_BOTTOM = 1
Private Const HWND_TOPMOST = -1
Private Const HWND_NOTOPMOST = -2
Private Const SWP_NOSIZE = &H1
Private Const SWP_NOMOVE = &H2
Private Const SWP_NOZORDER = &H4
Private Const SWP_NOREDRAW = &H8
Private Const SWP_NOACTIVATE = &H10
Private Const SWP_FRAMECHANGED = &H20       ' The frame changed: send WM_NCCALCSIZE
Private Const SWP_SHOWWINDOW = &H40
Private Const SWP_HIDEWINDOW = &H80
Private Const SWP_NOCOPYBITS = &H100
Private Const SWP_NOOWNERZORDER = &H200     ' Don't do owner Z ordering
Private Const SWP_NOSENDCHANGING = &H400    ' Don't send WM_WINDOWPOSCHANGING
Private Const SWP_DRAWFRAME = SWP_FRAMECHANGED
Private Const SWP_NOREPOSITION = SWP_NOOWNERZORDER
Private Const SWP_DEFERERASE = &H2000
Private Const SWP_ASYNCWINDOWPOS = &H4000

Public Declare Function SetWindowPos Lib "user32" ( _
                ByVal hwnd As Long, _
                ByVal hWndInsertAfter As Long, _
                ByVal X As Long, _
                ByVal Y As Long, _
                ByVal cx As Long, _
                ByVal cy As Long, _
                ByVal wFlags As Long) As Long

Public Declare Function GetTickCount Lib "kernel32" () As Long

Private Declare Function LoadLibrary Lib "kernel32" _
                            Alias "LoadLibraryA" _
                            (ByVal Path As String) As Long

Private Const SW_SHOWNORMAL As Long = 1

Private Declare Function ShellExecute Lib "shell32" Alias "ShellExecuteA" _
   (ByVal hwnd As Long, _
    ByVal lpOperation As String, _
    ByVal lpFile As String, _
    ByVal lpParameters As String, _
    ByVal lpDirectory As String, _
    ByVal nShowCmd As Long) As Long

'---------------------------------------------------------------------
' FloatWindow()
'
' Convert window to floating palette
'---------------------------------------------------------------------
Public Sub FloatWindow(ByVal hwnd As Long, float As Boolean)
    Dim z As Long
    
    If float Then
        z = HWND_TOPMOST
    Else
        z = HWND_NOTOPMOST
    End If
    SetWindowPos hwnd, z, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)

End Sub

'---------------------------------------------------------------------
' Sleep()
'
' Pause while keeping host application's UI alive
'---------------------------------------------------------------------
Public Sub Sleep(milliseconds As Long)
    Dim t As Long
    
    t = GetTickCount() + milliseconds
    While GetTickCount() < t
        DoEvents
    Wend
End Sub


'---------------------------------------------------------------------
' LoadDLL(name)
'
' Load a DLL that is in the same directory as our client's EXE.
' Raise an error if loading fails.
'---------------------------------------------------------------------
Public Sub LoadDLL(Name As String)
    Dim Path As String
    
    Path = App.Path & "\" & Name
    If (LoadLibrary(Path) = 0) Then _
        Err.Raise SCODE_DLL_LOADFAIL, ERR_SOURCE, Path & " failed to load."
    
End Sub

'---------------------------------------------------------------------
' DisplayWebPage(path or URL)
'
' Displays a web page, best efforts. Using URL.DLL is bad, bacuse it may not
' even be on a user's system.  We try to use ShellExecute, which is better because
' it honors DDE type associations and will not needlessly start multiple
' copies of the browser.
'
' This can handle local file paths and URLs
'---------------------------------------------------------------------
Public Sub DisplayWebPage(Path As String)

    Dim z As Long
    Dim brwsr As String

    z = ShellExecute(0, "Open", Path, 0, 0, SW_SHOWNORMAL)
    If (z > 0) And (z <= 32) Then
        MsgBox _
            "It doesn't appear that you have a web browser installed " & _
            "on your system.", (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), "Dome Simulator"
        Exit Sub
    End If
    
End Sub
