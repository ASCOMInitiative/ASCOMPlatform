var H = new ActiveXObject("ASCOM.Utilities.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "Focuser";							// Make chooser for Focusers
var F = new ActiveXObject(H.Choose("")); 		// Create instance of selected Driver
F = null;
H = null;
