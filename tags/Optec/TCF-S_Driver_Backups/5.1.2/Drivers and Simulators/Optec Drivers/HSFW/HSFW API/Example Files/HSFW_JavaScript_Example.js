try{
//Create a FilterWheels Object to provide a means of accessing attached devices
var FWManager = new ActiveXObject("OptecHID_FilterWheelAPI.FilterWheels");
// Get the list of attached Devices (an ArrayList of FilterWheel objects)
var ListOfDevices = FWManager.FilterWheelList;
// Output the number of attached devices
WScript.Echo("Found " + ListOfDevices.Count + " High Speed Filter Wheel(s)");
// Create a reference to the first device in the list
var FW = ListOfDevices(0);
// Home the device
FW.HomeDevice;
// Move to various positions
FW.CurrentPosition = 4;
FW.CurrentPosition = 1;
FW.CurrentPosition = 2;
FW.CurrentPosition = 3;
FW.CurrentPosition = 5;
// Create a string do hold device info.
var msg = "";
msg += "Filter Wheel with Serial Number " + FW.SerialNumber + " has " + FW.NumberOfFilters + " filters in it.\n";
msg += "The inserted Wheels ID is " + String.fromCharCode(FW.WheelID) + "\n";
msg += "The devices firmware version is " + FW.FirmwareVersion + "\n";
msg += "Test Complete!";
WScript.Echo(msg);

}
catch (e) {
    WScript.Echo(e.description);
}

