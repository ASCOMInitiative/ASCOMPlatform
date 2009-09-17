var S = new ActiveXObject("ASCOM.SampleLocalServer.Rotator");
var U = new ActiveXObject("DriverHelper.Util");

S.Connected = true;
for(var i = 0; i < 100; i++) {
    WScript.Echo(S.Position);
    U.WaitForMilliseconds(1000);
}

