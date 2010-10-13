Dim T
Set T = CreateObject("TheSkyXAdaptor.RASCOMTele")
Set U = CreateObject("ASCOM.Utilities.Util")
T.Connect
WScript.Echo "Connected... " & T.IsConnected
T.GetRaDec
Wscript.Echo U.HoursToHMS(T.dRa,":",":","",0) & " " & U.DegreesToDMS(T.dDec,":",":","",0)
Dim Ra, Dec
Ra = 15.0
Dec = 19.0
T.SlewToRaDec Ra, Dec,"Peter15.0, 19.0"
T.GetRaDec
Wscript.Echo U.HoursToHMS(T.dRa,":",":","",0) & " " & U.DegreesToDMS(T.dDec,":",":","",0)
Wscript.Echo T.dRa & " " & T.dDec



T.Disconnect
WScript.Echo "Disconnected."