var H = new ActiveXObject("DriverHelper.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "MultiPortSelector";					// Make chooser for MultiPortSelectors


var MPS = new ActiveXObject(H.Choose(""));			// Create instance of selected Driver

/////////////////////////////////////////////////////////////////////////////////////////////
MPS.Connected = true;
if (MPS.Connected == true) 
{
    WScript.Echo("\nconnected!\n");
}
else{
    WScript.Echo("\nnot connected\n");
}
//MPS.SetupDialog();
/////////////////////////////////////////////////////////////////////////////////////////////
WScript.Echo("\nDriver Name = " + MPS.DriverName );
WScript.Echo("\nPosition = " + MPS.Position );
WScript.Echo("\nDescription = " + MPS.DriverName );
WScript.Echo("\nDriver Info = " + MPS.DriverInfo + "\n");
/////////////////////////////////////////////////////////////////////////////////////////////
WScript.Echo("Get Port Names...\n");
var Pts, i;
Pts = MPS.Ports;
for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).Name);
}

WScript.Echo("\nSet Port Names to PortA, Port B... Then Read Names again\n");
Pts(0).Name = "PortA";
Pts(1).Name = "PortB";
Pts(2).Name = "PortC";
Pts(3).Name = "PortD";


for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).Name)
}
/////////////////////////////////////////////////////////////////////////////////////////////
WScript.Echo("\nRead Port Focus Offsets...\n");
for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).FocusOffset)
}

WScript.Echo("\nSet Port Focus Offsets to 11, 22, 33, 44... Then Read Focus Offsets Again\n");
Pts(0).FocusOffset = 11;
Pts(1).FocusOffset = 22;
Pts(2).FocusOffset = 33;
Pts(3).FocusOffset = 44;

for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).FocusOffset)
}
/////////////////////////////////////////////////////////////////////////////////////////////
WScript.Echo("\nRead Port RA Offsets...\n");
for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).RightAscensionOffset)
}
WScript.Echo("\nSet Port RA Offsets to 11.11, 22.11, 33.11, 44.11... Then read RA offsets again\n");
Pts(0).RightAscensionOffset = 11.11;
Pts(1).RightAscensionOffset = 22.11;
Pts(2).RightAscensionOffset = 33.11;
Pts(3).RightAscensionOffset = 44.11;

for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).RightAscensionOffset)
}
/////////////////////////////////////////////////////////////////////////////////////////////
WScript.Echo("\nRead Port Dec Offsets...\n");
for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).DeclinationOffset)
}
WScript.Echo("\nSet Port Dec Offsets to 11.33, 22.33, 33.33, 44.33... Then read Dec offsets again\n");
Pts(0).DeclinationOffset = 11.33;
Pts(1).DeclinationOffset = 22.33;
Pts(2).DeclinationOffset = 33.33;
Pts(3).DeclinationOffset = 44.33;

for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).DeclinationOffset)
}
/////////////////////////////////////////////////////////////////////////////////////////////
WScript.Echo("\nRead Port Rotation Offsets...\n");
for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).RotationOffset)
}
WScript.Echo("\nSet Port Rotation Offsets to 11.33, 22.33, 33.33, 44.33... Then read Rotation offsets again\n");
Pts(0).RotationOffset = 90.15;
Pts(1).RotationOffset = 180.27;
Pts(2).RotationOffset = 279.56;
Pts(3).RotationOffset = 117.77;

for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).RotationOffset)
} 
for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).Name)
}

///////////////////////////////////////////////////////////////////////////////////////////////
WScript.Echo("\nTest Changing Positions...\n");
WScript.Echo("\nMoving to Position 1...\n");
MPS.Position = 1;
WScript.Echo(MPS.Position);
MPS.Position = 4;
WScript.Echo(MPS.Position);
MPS.Position = 3;
WScript.Echo(MPS.Position);
MPS.Position = 2;
WScript.Echo(MPS.Position);

F = null;
MPS = null;
var WshShell = WScript.CreateObject("WScript.Shell");
var BtnCode = WshShell.Popup("All Finished!", 0, "Answer This Question:", 0 + 48);


