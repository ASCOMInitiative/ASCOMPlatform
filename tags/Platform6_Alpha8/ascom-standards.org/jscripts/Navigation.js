//
// Now passes JSLint (Bob Denny 29-May-07)
// Reorganized (Bob Denny 23-Oct-07)
// Additional edits (Bob Denny 26-Dec-07)
// Many more edits (Bob Denny 05-Jan-08)
// More and more... (Bob Denny 07-Jan-08)
// Case sensitivity for Unix/A2 (Bob Denny 13-May-08)
// Developer Getting Started (Bob Denny 28-Jul-10)
//
//* Initialize globals

var MainMenuItem = new Array();
var MainMenuURL = new Array();
var SubMenuOutput = new String;
var BreadCrumbOutput = new String;
var sLocation = new String;
var RelativePath = "./";
var TopLevelIndex = 0;

// Determine the top level index of where we are in the site
// Site is organized in a hierachical folder structure
// We use this to create the navigation menus and breadcrumbs

sLocation = location.href;
if (sLocation.match(/About\//i) !== null) {
    TopLevelIndex = 1;
}
else if (sLocation.match(/Downloads\//i) !== null) {
    TopLevelIndex = 2;
}
else if (sLocation.match(/Support\//i) !== null) {
    TopLevelIndex = 3;
}
else if (sLocation.match(/Standards\//i) !== null) {
    TopLevelIndex = 4;
}
else if (sLocation.match(/Developer\//i) !== null) {
    TopLevelIndex = 5;
}
else if (sLocation.match(/Community\//i) !== null) {
    TopLevelIndex = 6;
}
else if (sLocation.match(/FAQs\//i) !== null) {
    TopLevelIndex = 7;
}

if (TopLevelIndex > 0) RelativePath = "../";

// Setup Main Menu and its links
MainMenuItem[0] = "Home";
MainMenuURL[0] = RelativePath + "index.htm";

MainMenuItem[1] = "About";
MainMenuURL[1] = RelativePath + "About/Index.htm";

MainMenuItem[2] = "Downloads";
MainMenuURL[2] = RelativePath + "Downloads/Index.htm";

MainMenuItem[3] = "Support";
MainMenuURL[3] = RelativePath + "Support/Index.htm";

MainMenuItem[4] = "Standards";
MainMenuURL[4] = RelativePath + "Standards/Index.htm";

MainMenuItem[5] = "Developers";
MainMenuURL[5] = RelativePath + "Developer/Index.htm";

MainMenuItem[6] = "Community";
MainMenuURL[6] = RelativePath + "Community/Index.htm";

MainMenuItem[7] = "FAQs";
MainMenuURL[7] = RelativePath + "FAQs/Index.htm";

var BreadCrumbString = "<a href=\"" + RelativePath + "index.htm" + "\">" +  "Home" + "</a>" + " > ";

if (TopLevelIndex > 0) {
    BreadCrumbString += "<a href=\"Index.htm\">" + MainMenuItem[TopLevelIndex] + "</a> > ";
}

//* End of Initialize globals

function writemainmenu() {
    var output = "";
    var classname = "";
    
    document.write("<ul class=\"solidblockmenu\">");
    for (var i=0; i < MainMenuItem.length; i++)  {
        
        
        if (TopLevelIndex == i) {
            classname = "current";
        }
        else {
            classname = "";
        }  
        
        //Concat a string for the <li> menu item
        output = "<li><a href=\"" + MainMenuURL[i] + "\" class=\"" + classname + "\">" + MainMenuItem[i] + "</a></li>";
        document.write(output);
    }
    document.write("</ul>");
}

function writesubmenu() {
    
    switch (TopLevelIndex)
    {
    case 0: // Home Page
        SubMenuOutput += "<h3>Home</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
        SubMenuOutput += "<li><a href=\"About/Index.htm\">About ASCOM</a></li>";
        SubMenuOutput += "<li><a href=\"Downloads/Index.htm\">Downloads</a></li>";
        SubMenuOutput += "<li><a href=\"Support/Index.htm\">Support</a></li>";
        SubMenuOutput += "<li><a href=\"Standards/Index.htm\">Standards</a></li>";
        SubMenuOutput += "<li><a href=\"Developer/Index.htm\">Developers</a></li>";
        SubMenuOutput += "<li><a href=\"Community/Index.htm\">Community</a></li>";
        SubMenuOutput += "<li><a href=\"FAQs/Index.htm\">FAQs</a></li>";
        SubMenuOutput += "</ul>";
        break;
    case 1: // About
        SubMenuOutput += "<h3>About ASCOM</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
        SubMenuOutput += "<li><a href=\"Mission.htm\">ASCOM Mission Statement</a></li>";
        SubMenuOutput += "<li><a href=\"WhyImportant.htm\">Why are Drivers Important?</a></li>";
        SubMenuOutput += "<li><a href=\"HowWorks.htm\">How Does ASCOM Work?</a></li>";
        SubMenuOutput += "<li><a href=\"CompatLang.htm\">Compatible Languages</a></li>";
        SubMenuOutput += "<li><a href=\"Projects.htm\">Projects in Progress</a></li>";
        SubMenuOutput += "<li><a href=\"History.htm\">A Brief History</a></li>";
        SubMenuOutput += "<li><a href=\"Licensing.htm\">Ownership and Licensing</a></li>";
        SubMenuOutput += "<li><a href=\"HallOfFame.htm\">ASCOM Hall of Fame</a></li>";
        SubMenuOutput += "</ul>";
        break;
    case 2: // Downloads
    	SubMenuOutput += "<h3>Driver Downloads</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"CameraDrivers.htm\">Camera</a></li>";
	    SubMenuOutput += "<li><a href=\"DomeDrivers.htm\">Dome &amp; Roof</a></li>";
	    SubMenuOutput += "<li><a href=\"FilterWheelDrivers.htm\">Filter Wheels</a></li>";
	    SubMenuOutput += "<li><a href=\"FocuserDrivers.htm\">Focuser</a></li>";
	    SubMenuOutput += "<li><a href=\"RotatorDrivers.htm\">Rotator</a></li>";
	    SubMenuOutput += "<li><a href=\"ScopeDrivers.htm\">Telescope/Mount</a></li>";
	    SubMenuOutput += "</ul>";
    	SubMenuOutput += "<h3>Plug-In Downloads</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"Plugins.htm\">Plug-Ins</a></li>";
	    SubMenuOutput += "</ul>";
    	SubMenuOutput += "<h3>Platform Updates</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"PlatformUpdates.htm\">Platform Updates</a></li>";
	    SubMenuOutput += "</ul>";
    	SubMenuOutput += "<h3>Additional Downloads</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"ScriptableComponents.htm\">Scriptable Components</a></li>";
	    SubMenuOutput += "</ul>";
    	SubMenuOutput += "<h3>Developer Downloads</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"PlatToolUpdates.htm\">Platform Tool Updates</a></li>";
	    SubMenuOutput += "<li><a href=\"DevTools.htm\">Additional Tools</a></li>";
	    SubMenuOutput += "<li><a href=\"DevSamples.htm\">Samples &amp; Sources</a></li>";
	    SubMenuOutput += "</ul>";
        break;
    case 3: // Support
        SubMenuOutput += "<h3>Supported Devices</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"Cameras.htm\">Cameras</a></li>";
		SubMenuOutput += "<li><a href=\"Domes.htm\">Domes and Roofs</a></li>";
		SubMenuOutput += "<li><a href=\"FilterWheels.htm\">Filter Wheels</a></li>";
		SubMenuOutput += "<li><a href=\"Focusers.htm\">Focusers</a></li>";
		SubMenuOutput += "<li><a href=\"Rotators.htm\">Rotators</a></li>";
		SubMenuOutput += "<li><a href=\"Scopes.htm\">Telescopes and Mounts</a></li>";
	    SubMenuOutput += "</ul>";
        break;
    case 4: // Standards
        SubMenuOutput += "<h3>Standards</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"StandardsProcess.htm\">The Standards Process</a></li>";
		SubMenuOutput += "<li><a href=\"Requirements.htm\">General Requirements</a></li>";
	    SubMenuOutput += "</ul>";
        break;
    case 5: // Developers
        SubMenuOutput += "<h3>Application Development</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"AppStart.htm\">Getting Started</a></li>";
		SubMenuOutput += "<li><a href=\"ClientToolkit.htm\">.NET Client Toolkit</a></li>";
 	    SubMenuOutput += "</ul>";
        SubMenuOutput += "<h3>Driver Design Principles</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"Principles.htm\">The General Principles</a></li>";
		SubMenuOutput += "<li><a href=\"Errors.htm\">Errors and Retries</a></li>";
		SubMenuOutput += "<li><a href=\"Throttling.htm\">Traffic Throttling</a></li>";
		SubMenuOutput += "<li><a href=\"Binding.htm\">Early and Late Binding</a></li>";
		SubMenuOutput += "<li><a href=\"Chooser.htm\">Using the Chooser</a></li>";
		SubMenuOutput += "<li><a href=\"Distributing.htm\">Distributing the ASCOM Platform</a></li>";
		SubMenuOutput += "<li><a href=\"TheSky.htm\">EXE Drivers and TheSky&trade;</a></li>";
	    SubMenuOutput += "</ul>";
        SubMenuOutput += "<h3>Driver Practical Issues</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"DriverImpl.htm\">Driver Development</a></li>";
		SubMenuOutput += "<li><a href=\"Conformance.htm\">Conformance Testing</a></li>";
		SubMenuOutput += "<li><a href=\"DriverDist.htm\">Creating an Installer</a></li>";
		SubMenuOutput += "<li><a href=\"ReleaseTesting.htm\">Release Testing</a></li>";
		SubMenuOutput += "<li><a href=\"DriverSupt.htm\">Handling Support Issues</a></li>";
	    SubMenuOutput += "</ul>";
       break;
    case 6: // Community
        SubMenuOutput += "<h3>Community</h3>";
		SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"Partners.htm\">ASCOM Partners</a></li>";
	    SubMenuOutput += "<li><a href=\"Advocacy.htm\">ASCOM Advocacy</a></li>";
	    SubMenuOutput += "</ul>";
	    break;
    case 7: // FAQs
        SubMenuOutput += "<h3>Detailed FAQ Answers</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"SoftwareVictory.htm\">Correcting for Problems</a></li>";
	    SubMenuOutput += "<li><a href=\"TheSky.htm\">Working With TheSky</a></li>";
	    SubMenuOutput += "<li><a href=\"POTH.htm\">Using POTH</a></li>";
	    SubMenuOutput += "</ul>";
	    break;
	default:
	    SubMenuOutput += "<h3>OOPS! Missing SubMenu case " + TopLevelIndex + "</h3>";
	    break;
    }
    
    document.write(SubMenuOutput);
    
}

function breadcrumbs(){
  document.write(BreadCrumbString + document.title);
}


