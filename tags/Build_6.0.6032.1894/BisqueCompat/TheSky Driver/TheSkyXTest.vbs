Dim T
Set T = CreateObject("TheSkyXAdaptor.RASCOMTele")
T.Connect
WScript.Echo "Connected..."
T.Disconnect
WScript.Echo "Disconnected."