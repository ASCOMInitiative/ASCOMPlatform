var H = new ActiveXObject("DriverHelper.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "Dome";								// Make chooser for Domes
var F = new ActiveXObject(H.Choose(""));			// Create instance of selected Driver
F = null;
H = null;
