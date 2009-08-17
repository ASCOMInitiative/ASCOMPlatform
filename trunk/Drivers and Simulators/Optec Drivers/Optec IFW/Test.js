//var H = new ActiveXObject("DriverHelper.Chooser");
//WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
//H.DeviceType = "FilterWheel";							// Make chooser for FilterWheels
//WScript.Echo(H.Choose(""));
//var F = new ActiveXObject(H.Choose(""));			// Create instance of selected Driver
var F = new ActiveXObject("ASCOM.Optec_IFW.FilterWheel");
F.SetupDialog();
F.connected = true;
F.connected = true;
F.connected = true;
F.connected = true;
F.connected = true;
//F.position = 3;
//F.position = 8;
//WScript.Echo(F.position);

//WScript.Sleep(200)

//F.setupdialog();
F.connected = false;
F.connected = true;
F.connected = false;
F.connected = true;
F.connected = false;
F.connected = false;
F.connected = false;
WScript.Echo("Finished with no errors!");
//F.connected = false;
//F.offset;
//WScript.Echo(F.Names 1);
F = null;
H = null;

