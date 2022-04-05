//
// Now passes JSLint (Bob Denny 29-May-07)
// Reorganized (Bob Denny 23-Oct-07)
// Additional edits (Bob Denny 26-Dec-07)
// Many more edits (Bob Denny 05-Jan-08)
// More and more... (Bob Denny 07-Jan-08)
// Case sensitivity for Unix/A2 (Bob Denny 13-May-08)
// Developer Getting Started (Bob Denny 28-Jul-10)
// Unknown who/when SafetyMonitor Drivers was added (Jan 2015 maybe?)
// Observing Conditions Drivers (Bob Denny 06-Jan-2016)
// Reorganize topics (Bob Denny 29-Mar-2021?)
// Reorganize logic, add wiki entries to MainMenu (Bob Denny 14-Jul-2021 )
// Another reorg -- Will it ever end? (Bob Denny 21-Jul-2021)
// Another reorg, to separate Initiative info from ASCOM/Alpaca info
// Late July and August, many many changes..... (rbd)
// Mid-August New documents top section
// Mid-August loads of reorganization again
// September, more!!
// October, more!!

//
// ************************
// ** I AM AWARE OF THIS **
// ************************
// https://developers.google.com/web/updates/2016/08/removing-document-write

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
else if (sLocation.match(/Support\//i) !== null) {
    TopLevelIndex = 2;
}
else if (sLocation.match(/Downloads\//i) !== null) {
    TopLevelIndex = 3;
}
else if (sLocation.match(/Documentation\//i) !== null) {
    TopLevelIndex = 4;
}
else if (sLocation.match(/AlpacaDeveloper\//i) !== null) {
    TopLevelIndex = 5;
}
else if (sLocation.match(/COMDeveloper\//i) !== null) {
    TopLevelIndex = 6;
}
else if (sLocation.match(/FAQs\//i) !== null) {
    TopLevelIndex = 7;
}
else if (sLocation.match(/Community\//i) !== null) {
    TopLevelIndex = 8;
}
else if (sLocation.match(/Standards\//i) !== null) {
    TopLevelIndex = 9;
}
else if (sLocation.match(/Initiative\//i) !== null) {
    TopLevelIndex = 10;
}

if (TopLevelIndex > 0) RelativePath = "../";

// Setup Main Menu and its links
MainMenuItem[0] = "Home";
MainMenuURL[0] = RelativePath + "index.htm";

MainMenuItem[1] = "About";
MainMenuURL[1] = RelativePath + "About/Index.htm";

MainMenuItem[2] = "Support";
MainMenuURL[2] = RelativePath + "Support/Index.htm";

MainMenuItem[3] = "Downloads";
MainMenuURL[3] = RelativePath + "Downloads/Index.htm";

MainMenuItem[4] = "Docs";
MainMenuURL[4] = RelativePath + "Documentation/Index.htm";

MainMenuItem[5] = "Alpaca Devs";
MainMenuURL[5] = RelativePath + "AlpacaDeveloper/Index.htm";

MainMenuItem[6] = "COM Devs";
MainMenuURL[6] = RelativePath + "COMDeveloper/Index.htm";

MainMenuItem[7] = "FAQs";
MainMenuURL[7] = RelativePath + "FAQs/Index.htm";

MainMenuItem[8] = "Community";
MainMenuURL[8] = RelativePath + "Community/Index.htm";

MainMenuItem[9] = "Standards";
MainMenuURL[9] = RelativePath + "Standards/Index.htm";

MainMenuItem[10] = "Initiative";
MainMenuURL[10] = RelativePath + "Initiative/Index.htm";

var BreadCrumbString = "<a href=\"" + RelativePath + "index.htm" + "\">" +  "Home" + "</a>" + " > ";

if (TopLevelIndex > 0) {
    BreadCrumbString += "<a href=\"Index.htm\">" + MainMenuItem[TopLevelIndex] + "</a> > ";
}

//* End of Initialize globals

function writemainmenu() {
    var output = "";
    var classname = "";
	var target = "";
    
    document.write("<ul class=\"solidblockmenu\">");
    for (var i=0; i < MainMenuItem.length; i++)  {
        
        if (TopLevelIndex == i) {
            classname = "class='current'";
        } else {
			classname = "";
		}
		if (MainMenuItem[i] == "Users Wiki" ||
					MainMenuItem[i] == "Developers Wiki") {
			target = "target='_new'";
		} else {
			target = "";
		}
		
        //Concat a string for the <li> menu item
        output = "<li><a href=\"" + MainMenuURL[i] + "\" " + classname + " " + target + 
					 ">" + MainMenuItem[i] + "</a></li>";
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
        SubMenuOutput += "<li><a href=\"About/Index.htm\">About Alpaca and ASCOM</a></li>";
        SubMenuOutput += "<li><a href=\"Support/Index.htm\">Get Support</a></li>";
        SubMenuOutput += "<li><a href=\"Downloads/Index.htm\">Drivers &amp; Other Downloads</a></li>";
        SubMenuOutput += "<li><a href=\"Documentation/Index.htm\">Published Documents</a></li>";
        SubMenuOutput += "<li><a href=\"AlpacaDeveloper/Index.htm\">Alpaca Developers</a></li>";
        SubMenuOutput += "<li><a href=\"COMDeveloper/Index.htm\">Classic ASCOM Developers</a></li>";
        SubMenuOutput += "<li><a href=\"FAQs/Index.htm\">Frequently Asked Questions</a></li>";
        SubMenuOutput += "<li><a href=\"Community/Index.htm\">Our Community</a></li>";
        SubMenuOutput += "<li><a href=\"Standards/Index.htm\">ASCOM Standards &amp; Specs</a></li>";
        SubMenuOutput += "<li><a href=\"Initiative/Index.htm\">The ASCOM Initiative</a></li>";
// 	    SubMenuOutput += "<li><a href=\"https://ascomtalk.groups.io/g/Help/wiki\" target=\"_new\">Users Wiki</a></li>";
//	    SubMenuOutput += "<li><a href=\"https://ascomtalk.groups.io/g/Developer/wiki\" target=\"_new\">Developers Wiki</a></li>";
       SubMenuOutput += "</ul>";
        break;
    case 1: // About
        SubMenuOutput += "<h3>About ASCOM</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
        SubMenuOutput += "<li><a href=\"Index.htm\">0. Motivation for ASCOM</a></li>";
        SubMenuOutput += "<li><a href=\"Overview.htm\">1. Overview of ASCOM</a></li>";
        SubMenuOutput += "<li><a href=\"ModularArch.htm\">2. Modular Architectures</a></li>";
        SubMenuOutput += "<li><a href=\"UniversalInt.htm\">3. Universal Interfaces</a></li>";
        SubMenuOutput += "<li><a href=\"Basics.htm\">4. ASCOM Basics</a></li>";
        SubMenuOutput += "<li><a href=\"Characteristics.htm\">5. Key Characteristics</a></li>";
        SubMenuOutput += "<li><a href=\"Conn-COM.htm\">6. COM Connectivity</a></li>";
        SubMenuOutput += "<li><a href=\"Conn-Alpaca.htm\">7. Alpaca Connectivity</a></li>";
        SubMenuOutput += "<li><a href=\"ErrorHandling.htm\">8. Error Handling</a></li>";
        SubMenuOutput += "<li><a href=\"ProblemSolving.htm\">9. Problem Solving</a></li>";
        SubMenuOutput += "</ul>";
        break;
    case 2: // Support
        SubMenuOutput += "<h3>How to Get Support</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"DeviceSupport.htm\">Devices</a></li>";
		SubMenuOutput += "<li><a href=\"AppSupport.htm\">Applications</a></li>";
		SubMenuOutput += "<li><a href=\"PlatformSupport.htm\">Platform (Windows)</a></li>";
		SubMenuOutput += "<li><a href=\"AlpacaSupport.htm\">Alpaca Connectivity</a></li>";
		SubMenuOutput += "<li><a href=\"../AlpacaDeveloper/Index.htm\">Alpaca Development</a></li>";
		SubMenuOutput += "<li><a href=\"../COMDeveloper/Index.htm\">COM Development</a></li>";
		SubMenuOutput += "<li><a href=\"https://ascomtalk.groups.io/g/Help\" target=\"_new\">Community Support</a></li>";
	    SubMenuOutput += "</ul>";
        break;
    case 3: // Downloads
    	SubMenuOutput += "<h3>Driver Downloads</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"CameraDrivers.htm\">Camera &amp; Video</a></li>";
	    SubMenuOutput += "<li><a href=\"DomeDrivers.htm\">Dome &amp; Roof</a></li>";
	    SubMenuOutput += "<li><a href=\"FilterWheelDrivers.htm\">Filter Wheels</a></li>";
	    SubMenuOutput += "<li><a href=\"FocuserDrivers.htm\">Focuser</a></li>";
	    SubMenuOutput += "<li><a href=\"ObservingConditionsDrivers.htm\">ObservingConditions</a></li>";
	    SubMenuOutput += "<li><a href=\"RotatorDrivers.htm\">Rotator</a></li>";
	    SubMenuOutput += "<li><a href=\"SafetyMonitorDrivers.htm\">Safety Monitor</a></li>";
	    SubMenuOutput += "<li><a href=\"SwitchDrivers.htm\">Switch/Outlet</a></li>";
	    SubMenuOutput += "<li><a href=\"ScopeDrivers.htm\">Telescope/Mount</a></li>";
	    SubMenuOutput += "</ul>";
    	SubMenuOutput += "<h3>Plug-In Downloads</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"Plugins.htm\">Plug-Ins</a></li>";
	    SubMenuOutput += "</ul>";
    	SubMenuOutput += "<h3>Additional Downloads</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"ScriptableComponents.htm\">Scriptable Components</a></li>";
	    SubMenuOutput += "<li><a href=\"https://github.com/ASCOMInitiative/ASCOMRemote/releases\" target=\"_new\">ASCOM Remote (Alpaca)</a></li>";
	    SubMenuOutput += "<li><a href=\"https://github.com/synfinatic/alpacascope#readme\" target=\"_new\">AlpacaScope (SkySafari)</a></li>";
	    SubMenuOutput += "</ul>";
    	SubMenuOutput += "<h3>Developer Downloads</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"PlatDevComponents.htm\">Developer Components</a></li>";
	    SubMenuOutput += "<li><a href=\"DevTools.htm\">Additional Tools</a></li>";
	    SubMenuOutput += "<li><a href=\"DevSamples.htm\">Samples &amp; Sources</a></li>";
	    SubMenuOutput += "</ul>";
        break;
    case 4: // Documents
    	SubMenuOutput += "<h3>Documents</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"Index.htm#usr\">For End Users</a></li>";
	    SubMenuOutput += "<li><a href=\"Index.htm#dev\">For Developers</a></li>";
	    SubMenuOutput += "</ul>";
		break;
    case 5: // Alpaca Developers
        SubMenuOutput += "<h3>Design Principles</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"Principles.htm\">The General Principles</a></li>";
		SubMenuOutput += "<li><a href=\"Async.htm\">Asynchronous APIs</a></li>";
		SubMenuOutput += "<li><a href=\"Exceptions.htm\">Exceptions in ASCOM</a></li>";
	    SubMenuOutput += "</ul>";
        SubMenuOutput += "<h3>Alpaca References</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
 		SubMenuOutput += "<li><a href=\"../About/Index.htm\">About ASCOM &amp; Alpaca</a></li>";
		SubMenuOutput += "<li><a href=\"https://ascom-standards.org/api/\" target=\"_new\">ASCOM Alpaca API</a></li>";
		SubMenuOutput += "<li><a href=\"https://github.com/ASCOMInitiative/ASCOMRemote/raw/master/Documentation/ASCOM Alpaca API Reference.pdf\" target=\"_new\">API Reference (PDF)</a></li>";
	    SubMenuOutput += "</ul>";
        SubMenuOutput += "<h3>Alpaca App Development</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li>(to be written)</li>";
	    SubMenuOutput += "</ul>";
        SubMenuOutput += "<h3>Alpaca Driver Development</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li>(to be written)</li>";
	    SubMenuOutput += "</ul>";
		break;
    case 6: // COM Developers
        SubMenuOutput += "<h3>Design Principles</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"Principles.htm\">The General Principles</a></li>";
		SubMenuOutput += "<li><a href=\"Async.htm\">Asynchronous APIs</a></li>";
		SubMenuOutput += "<li><a href=\"Exceptions.htm\">Exceptions in ASCOM</a></li>";
	    SubMenuOutput += "</ul>";
        SubMenuOutput += "<h3>COM App Development</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"AppStart.htm\">Getting Started</a></li>";
		SubMenuOutput += "<li><a href=\"Chooser.htm\">Using the Chooser</a></li>";
		SubMenuOutput += "<li><a href=\"ClientToolkit.htm\">.NET Client Toolkit</a></li>";
 	    SubMenuOutput += "<li><a href=\"https://ascomtalk.groups.io/g/Developer/wiki\" target=\"_new\">Developers Wiki</a></li>";
	    SubMenuOutput += "</ul>";
        SubMenuOutput += "<h3>COM Driver Design Points</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"Errors.htm\">Errors and Retries</a></li>";
		SubMenuOutput += "<li><a href=\"Throttling.htm\">Traffic Throttling</a></li>";
		SubMenuOutput += "<li><a href=\"Binding.htm\">Early and Late Binding</a></li>";
		SubMenuOutput += "<li><a href=\"TheSky.htm\">EXE Drivers and TheSky&trade;</a></li>";
		SubMenuOutput += "<li><a href=\"Distributing.htm\">Distributing the ASCOM Platform</a></li>";
	    SubMenuOutput += "</ul>";
        SubMenuOutput += "<h3>COM Driver Practical Issues</h3>";
	    SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"DriverImpl.htm\">Driver Development</a></li>";
		SubMenuOutput += "<li><a href=\"DevFor32And64Bits.htm\">Developing for 32/64-bits</a></li>";
		SubMenuOutput += "<li><a href=\"Conformance.htm\">Conformance Testing</a></li>";
		SubMenuOutput += "<li><a href=\"DriverDist.htm\">Creating an Installer</a></li>";
		SubMenuOutput += "<li><a href=\"ReleaseTesting.htm\">Release Testing</a></li>";
		SubMenuOutput += "<li><a href=\"DriverSupt.htm\">Handling Support Issues</a></li>";
	    SubMenuOutput += "</ul>";
       break;
    case 7: // FAQs
        SubMenuOutput += "<h3>Detailed FAQ Answers</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"Plat6OnW7.htm\">Platform on Windows 7 &amp; 10</a></li>";
	    SubMenuOutput += "<li><a href=\"Plat6OnXP.htm\">Platform on Windows XP</a></li>";
	    SubMenuOutput += "<li><a href=\"Platform5.5only.htm\">Driver for Platform 5 Only?</a></li>";
	    SubMenuOutput += "<li><a href=\"SoftwareVictory.htm\">Correcting for Problems</a></li>";
	    SubMenuOutput += "<li><a href=\"DevHub.htm\">Using DeviceHub</a></li>";
	    SubMenuOutput += "<li><a href=\"https://ascomtalk.groups.io/g/Help/wiki\" target=\"_new\">Users Wiki</a></li>";
	    SubMenuOutput += "<li><a href=\"https://ascomtalk.groups.io/g/Developer/wiki\" target=\"_new\">Developers Wiki</a></li>";
	    SubMenuOutput += "</ul>";
	    break;
    case 8: // Community
        SubMenuOutput += "<h3>Community</h3>";
		SubMenuOutput += "<ul class=\"treeview\">";
	    SubMenuOutput += "<li><a href=\"Partners.htm\">ASCOM Partners</a></li>";
	    SubMenuOutput += "<li><a href=\"Advocacy.htm\">ASCOM Advocacy</a></li>";
	    SubMenuOutput += "<li><a href=\"https://ascomtalk.groups.io/g/Help/wiki\" target=\"_new\">Users Wiki</a></li>";
	    SubMenuOutput += "<li><a href=\"https://ascomtalk.groups.io/g/Developer/wiki\" target=\"_new\">Developers Wiki</a></li>";
	    SubMenuOutput += "</ul>";
	    break;
    case 9: // Standards
        SubMenuOutput += "<h3>Standards</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
		SubMenuOutput += "<li><a href=\"InterfacePrinciple.htm\">ASCOM Interface Principle</a></li>";
		SubMenuOutput += "<li><a href=\"StandardsProcess.htm\">The Standards Process</a></li>";
		SubMenuOutput += "<li><a href=\"Requirements.htm\">General Requirements</a></li>";
	    SubMenuOutput += "</ul>";
        break;
    case 10: // Initiative
        SubMenuOutput += "<h3>ASCOM Initiative</h3>";
        SubMenuOutput += "<ul class=\"treeview\">";
        SubMenuOutput += "<li><a href=\"Mission.htm\">ASCOM Mission Statement</a></li>";
        SubMenuOutput += "<li><a href=\"StructureAndPolicies.htm\">Structure and Policies</a></li>";
        SubMenuOutput += "<li><a href=\"Surveys.htm\">How Well are We Doing?</a></li>";
        SubMenuOutput += "<li><a href=\"Projects.htm\">Projects in Progress</a></li>";
        SubMenuOutput += "<li><a href=\"History.htm\">Detailed History</a></li>";
        SubMenuOutput += "<li><a href=\"Licensing.htm\">Ownership and Licensing</a></li>";
        SubMenuOutput += "<li><a href=\"HallOfFame.htm\">ASCOM Hall of Fame</a></li>";
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


