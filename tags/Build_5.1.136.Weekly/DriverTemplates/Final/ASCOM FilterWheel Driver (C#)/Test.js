var H = new ActiveXObject("DriverHelper.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "FilterWheel";							// Make chooser for FilterWheels
var F = new ActiveXObject(H.Choose(""));			// Create instance of selected Driver
F = null;
H = null;
