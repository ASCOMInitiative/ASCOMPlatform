'This script retrieves and displays the platform version number

'Version History
'Original version           - Distributed in Platform 5
'Peter Simpson - 06/02/2010 - Revised to use the new 32/64bit ASCOM.Utilities.Util component in place of the 
'                             32bit only DriverHelper2.Util component
'Peter Simpson - 25/06/2011 - Updated to add build version

Dim UTL, SH, PB, REGROOT32, REGROOT64
REGROOT64 = "HKLM\Software\Wow6432Node\ASCOM\Platform\" ' Registry key values for 64 and 32bit OS
REGROOT32 = "HKLM\Software\ASCOM\Platform\"

Set UTL = CreateObject("ASCOM.Utilities.Util") ' Create Utilities and Shell objects
Set SH = CreateObject("WScript.Shell")

On Error Resume Next ' Ensure we continue if we get an error because we aren't on a 64bit OS
SH.RegRead(REGROOT64) ' Attempt to read the 64bit registry key
If Err.Number <> 0 Then ' We are on a 32bit computer so use the 32bit registry key
	regRoot = REGROOT32
Else ' 64bit computer so use 64bit key
	regRoot = REGROOT64
End If

PB=SH.RegRead(regRoot & "Platform Build") ' Read the platform build version
WScript.Echo "This PC is running ASCOM Platform " & UTL.PlatformVersion & ", build " & PB & VBcRlF & vbcRlF & "Please use the ASCOM Diagnostics tool to obtain further information."

Set UTL = Nothing ' Clean up objects
Set PV = Nothing
