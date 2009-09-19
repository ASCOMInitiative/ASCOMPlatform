var H = new ActiveXObject("DriverHelper.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "MultiPortSelector";					// Make chooser for MultiPortSelectors
var MPS = new ActiveXObject(H.Choose(""));			// Create instance of selected Driver

/////////////////////////////////////////////////////////////////////////////////////////////
MPS.Connected = true;
if (MPS.Connected == true) 
{
    WScript.Echo("\nCONNECTED!\n");
}
else{
    WScript.Echo("\nNot Connected\n");
}
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
//MPS.Ports(1).Name = "PortA";
//MPS.Ports(1).Name = "PortB";
//Pts(2).Name = "PortC";
//Pts(3).Name = "PortD";

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
//MPS.Ports(0).FocusOffset = 11;
//MPS.Ports(1).FocusOffset = 22;
//MPS.Ports(2).FocusOffset = 33;
//MPS.Ports(3).FocusOffset = 44;

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
//MPS.Ports(0).RightAscensionOffset = 11.11;
//MPS.Ports(1).RightAscensionOffset = 22.11;
//MPS.Ports(2).RightAscensionOffset = 33.11;
//MPS.Ports(3).RightAscensionOffset = 44.11;

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
//MPS.Ports(0).DeclinationOffset = 11.33;
//MPS.Ports(1).DeclinationOffset = 22.33;
//MPS.Ports(2).DeclinationOffset = 33.33;
//MPS.Ports(3).DeclinationOffset = 44.33;

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
//MPS.Ports(0).RotationOffset = 90.15;
//MPS.Ports(1).RotationOffset = 180.27;
//MPS.Ports(2).RotationOffset = 279.56;
//MPS.Ports(3).RotationOffset = 117.77;

for(i = 0; i<4; i++)
{
    WScript.Echo(Pts(i).RotationOffset)
} 

F = null;
MPS = null;
var WshShell = WScript.CreateObject("WScript.Shell");
var BtnCode = WshShell.Popup("All Finished!", 0, "Answer This Question:", 0 + 48);


