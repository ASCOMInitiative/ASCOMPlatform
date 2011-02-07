var H = new ActiveXObject("DriverHelper.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "Camera";							// Make chooser for Camera
var F = new ActiveXObject(H.Choose("")); 		// Create instance of selected Driver
F.Connected = true;
WScript.Echo("Description " + F.Description);
WScript.Echo("InterfaceVersion " + F.InterfaceVersion);
WScript.Echo("DriverInfo " + F.DriverInfo);
F = null;
H = null;
