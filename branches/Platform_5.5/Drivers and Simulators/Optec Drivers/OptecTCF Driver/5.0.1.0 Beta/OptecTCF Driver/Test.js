var H = new ActiveXObject("ASCOM.Utilities.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "Focuser";							// Make chooser for Focusers
var F = new ActiveXObject(H.Choose(""));			// Create instance of selected Driver
//var F = new ActiveXObject("ASCOM.OptecTCF_Driver.Focuser");
var j = "Output: " + "\n";
F.SetupDialog();
//Test Connect and Disconnect
j += "Testing Connect and Disconnect..........\n";
F.Link = true;
if (F.Link == true) j+= "Connect Passed!\n" 
F.Link = false;
if (F.Link == false) j+= "Disconnect Passed!\n" 
F.Link = true;
if (F.Link == true) j+= "Connect Passed!\n" 
//if(F.Link == true) WScript.Echo("Connected Successfully")
//connected!

j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";

j+= "Testing Temp Comp mode.........\n";
F.TempComp = true;
F.Move(3200);
if (F.TempComp == true) j+= "Entered Temperature Mode!\n";
else j+= "FAILED to enter Temp Comp Mode\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n"
F.TempComp = false;
if (F.TempComp == false) j+= "Exited Temperature Mode!\n";
else j+= "Failed to Exit Temp Comp Mode\n";
j+= "Max Increment = " + F.MaxIncrement + "\n";
j+= "Max Step = " + F.MaxStep + "\n";
j+= "Step Size = " + F.StepSize + "\n";
j+= "Position = " + F.Position + "\n";
j+= "Moving to 3300... \n";
F.Move(3300);
j+= "New Position = " + F.Position + "\n";
j+= "Moving to 3500... \n";
F.Move(3500);
j+= "New Position = " + F.Position + "\n";
j += "Temperature = " + F.Temperature + "\n";



//disconnect
F.Link = false;
//if(F.Link == false) WScript.Echo("Disconnected Successfully")

WScript.Echo(j)
F = null;
H = null;
j = null;
