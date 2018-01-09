var T = new ActiveXObject("DFM.Telescope");
T.SetupDialog();
T.Connected = true;

WScript.Echo("RARate=" + T.RightAscensionRate);
WScript.Echo("DERate=" + T.DeclinationRate);

T.RightAscensionRate = 0.05;
T.DeclinationRate = 0.05;

WScript.Echo("RARate=" + T.RightAscensionRate);
WScript.Echo("DERate=" + T.DeclinationRate);

T.Connected = false;
T = null;

