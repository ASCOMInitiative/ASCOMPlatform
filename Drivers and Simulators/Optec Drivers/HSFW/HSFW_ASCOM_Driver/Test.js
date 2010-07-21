var H = new ActiveXObject("DriverHelper.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "FilterWheel";							// Make chooser for FilterWheels
var F = new ActiveXObject(H.Choose("")); 		// Create instance of selected Driver
try {
    F.Connected = true;
    if (F.Connected == true) WScript.Echo("FW is Connected")
    else WScript.Echo("FW is Disconnected");
    F.Position = 1;
    F.Position = 5;
    F.Position = 2;
    F.Position = 4;
    F.Position = 3;
    F.Connected = false;
    if (F.Connected == true) WScript.Echo("FW is Connected")
    else WScript.Echo("FW is Disconnected");
    F = null;
    H = null;
    var input;
}
catch (e) {
    WScript.Echo(e.description);
}

WScript.Echo("Press a key to continue...");
input += WScript.StdIn.Read(1);
