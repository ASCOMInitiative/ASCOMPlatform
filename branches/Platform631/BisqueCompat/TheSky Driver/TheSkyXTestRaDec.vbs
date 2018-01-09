Dim T
Set T = CreateObject("TheSkyXAdaptor.RASCOMTele")
Set U = CreateObject("ASCOM.Utilities.Util")
T.Connect
WScript.Echo "Connected... " & T.IsConnected
T.GetRaDec
Wscript.Echo "Before" & U.HoursToHMS(T.dRa,":",":","",0) & " " & U.DegreesToDMS(T.dDec,":",":","",0)
Dim Ra, Dec
Ra = 3.0
Dec = 50.0
T.SlewToRaDec Ra, Dec,"Peter3.0, 50.0"
T.GetRaDec
Wscript.Echo "After" & U.HoursToHMS(T.dRa,":",":","",0) & " " & U.DegreesToDMS(T.dDec,":",":","",0)
Wscript.Echo T.dRa & " " & T.dDec



T.Disconnect
WScript.Echo "Disconnected."