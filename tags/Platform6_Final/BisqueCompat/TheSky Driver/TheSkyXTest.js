var T = new ActiveXObject("TheSkyXAdaptor.RASCOMTele");
T.Connect();
WScript.Echo("Connected...");
T.Disconnect();
WScript.Echo("Disconnected.");