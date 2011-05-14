var H = new ActiveXObject("DriverHelper.Chooser");
WScript.Echo("Choose your new driver in the Chooser that will appear. Then click Properties to activate its SetupDialog");
H.DeviceType = "Focuser";							// Make chooser for Focusers
var F = new ActiveXObject(H.Choose("")); 		// Create instance of selected Drive
var j = "";
F.Link = false;
F.Link = true;
F.TempComp = false;
j += "Connected = " + F.Link + "\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";
j += "TempComp = " + F.TempComp + "\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";
F.TempComp = true;
j += "Focuser Step Size = " + F.StepSize + "\n";
j += "TempComp = " + F.TempComp + "\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";
j += "TempComp = " + F.TempComp + "\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";
j += "TempComp = " + F.TempComp + "\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";
j += "TempComp = " + F.TempComp + "\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";

F.TempComp = false;
F.TempComp = false;
F.Move(2000);
j += "TempComp = " + F.TempComp + "\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";
j += "TempComp = " + F.TempComp + "\n";
j += "Temperature = " + F.Temperature + "\n";
j += "Position = " + F.Position + "\n";
F.Link = false;
j += "Connected = " + F.Link + "\n";


WScript.Echo(j);
j = null;
F = null;
H = null;
