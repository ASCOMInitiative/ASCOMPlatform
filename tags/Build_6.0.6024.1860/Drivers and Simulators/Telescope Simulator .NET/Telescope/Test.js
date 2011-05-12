var H = new ActiveXObject("DriverHelper.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "Telescope";							// Make chooser for Telescopes
var drvrID = H.Choose("");
if(drvrID === "") WScript.Quit();                   // Cancelled chooser
var T = new ActiveXObject(drvrID);			        // Create instance of selected Driver
var axRates = T.AxisRates(0);                       // Primary axis rates collection
WScript.Echo(axRates.Count + " rates");
if(axRates.Count === 0) {
    WScript.Echo("Empty AxisRates!");
} else {
    var e = new Enumerator(axRates);                // Enumerate primary axis (same as foreach)
    for(;!e.atEnd(); e.moveNext())
	    WScript.Echo("Max=" + e.item().Maximum + " Min=" + e.item().Minimum);
}
var trRates  = T.TrackingRates;                     // Tracking rates collection
if (trRates.Count === 0) {
    WScript.Echo("Empty TrackingRates!");
} else {
    e = new Enumerator(trRates);
    for(; !e.atEnd(); e.moveNext())
	    WScript.Echo("DriveRate=" + e.item());
}
T = null;
H = null;
