// var P = new ActiveXObject("DriverHelper.Profile");
// P.DeviceType = "Dome";
// WScript.Echo(P.GetValue("DomeSim.Dome", "AzRate"));
// var y = P.GetValue("DomeSim.Dome", "CanFindHome", "Capabilities");
// var z = P.Values("DomeSim.Dome")
// var a = P.SubKeys("DomeSim.Dome")
// P.WriteValue("DomeSim.Dome", "test", "YoMama", "Capabilities");
// P.DeleteValue("DomeSim.Dome", "test", "Capabilities");
// P.DeleteSubKey("DomeSim.Dome", "Test");
// P.DeleteSubKey("DomeSim.Dome", "foo");
// var C = new ActiveXObject("DomeSim.Dome");
// C.DeviceType = "Telescope";
// WScript.Echo(C.Choose(""));
var T = new ActiveXObject("FocusSim.Focuser");
var T2 = new ActiveXObject("FocusSim.Focuser");
// var U = new ActiveXObject("DriverHelper.Util");
// U.WaitForMilliseconds(3000);
T2.Link = true;
T.Link = true;
WScript.Echo(T.Position + " " + T2.Position);
// T.SetupDialog();
