var R = new ActiveXObject("ASCOM.Simulator.Rotator");
var U = new ActiveXObject("DriverHelper.Util");
try {
    R.Connected = true;
    WScript.Echo(R.Position);
    R.MoveAbsolute(30);
    while(R.IsMoving)
        U.WaitForMilliseconds(1000);
    WScript.Echo("move complete: " + R.Position);
    R.MoveAbsolute(330);
    while(R.IsMoving)
        U.WaitForMilliseconds(1000);
    WScript.Echo("move complete: " + R.Position);
    R.Connected = false;
    U.WaitForMilliseconds(5000);
} catch(ex) {
    WScript.Echo(ex.message);
}