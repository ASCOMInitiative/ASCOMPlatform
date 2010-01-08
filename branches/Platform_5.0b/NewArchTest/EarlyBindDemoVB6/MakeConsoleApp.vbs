' LinkConsole.vbs
' http://www.angelfire.com/mi4/bvo/vb/vbconio.htm
'
' This is a WSH script used to make it easier to edit
' a compiled VB6 EXE using LINK.EXE to create a console
' mode program.
'
' Drag the EXE's icon onto the icon for this file (or onto
' a shortcut to this file), or execute it from a command
' prompt as in:
'
'    LinkConsole.vbs Project1.exe
'
Option Explicit

Dim strLINK, strEXE, WSHShell

' Be sure to set up strLINK to match your VB6 installation.
strLINK = """C:\Program Files (x86)\Microsoft Visual Studio\VB98\LINK.EXE"""

strEXE = """" & WScript.Arguments(0) & """"

Set WSHShell = CreateObject("WScript.Shell")

WSHShell.Run strLINK & " /EDIT /SUBSYSTEM:CONSOLE " & strEXE

Set WSHShell = Nothing
WScript.Echo "Console bit set"