// ------------------------------------------------------------------------------------------------
// tabs=4
//
// ==================
// CheckForUpdates.js
// ==================
//
// This script reads the local versions of the Platform and optionally, the Platform
// Developer tools and Conform. Then it fetches the latest versions from the ASCOM
// website. For the Platform and each installed optional component, it then
// checks to see if a newer version is available. For the first one that is 
// newer, it opens a web page for downloading it. The order it checks is the same
// order that one would install the components. Thus you can't install the later ones
// until you get the earlier ones installed. Platform first, etc.
//
// Sorry I could not figure out how to get to Platform with the new Profile
//
// Author: Bob Denny <rdenny@dc3.com>
//
// Edits:
// ---------    ---     ---------------------------------------------------------------------------
// 28-Apr-11    rbd     Initial edit
// ------------------------------------------------------------------------------------------------

var POPTITLE = "Check ASCOM Updates";
var REMWEB = "http://download.ascom-standards.org/ver/";
var REGROOT = "HKLM\\Software\\ASCOM\\Platform\\";

var PLATPAGE = "http://ascom-standards.org/Downloads/PlatformUpdates.htm";
var DEVPAGE = "http://ascom-standards.org/Downloads/PlatToolUpdates.htm";
var CONFPAGE = "http://ascom-standards.org/Downloads/DevTools.htm";

function isRemoteNewer(local, remote)
{
    var lbits = local.split(".");
    var rbits = remote.split(".");
    for (var i = 0; i < 4; i++)
    {
        if (rbits[i] > lbits[i]) return true;  
    }
    return false;  
}

var SH = new ActiveXObject("WScript.Shell");
var pd = SH.RegRead(REGROOT);                                   // Platform description
var cv = SH.RegRead(REGROOT + "Conform Version");
var dv = SH.RegRead(REGROOT + "Developer Tools Version");
var pv = SH.RegRead(REGROOT + "Platform Version");

try
{
    var http = new ActiveXObject("Msxml2.XMLHTTP");
    http.open("GET", REMWEB + "cv.txt", false);
    http.send();
    var rcv = http.responseText;
    http.open("GET", REMWEB + "dv.txt", false);
    http.send();
    var rdv = http.responseText;
    var http = new ActiveXObject("Msxml2.XMLHTTP");
    http.open("GET", REMWEB + "pv.txt", false);
    http.send();
    var rpv = http.responseText;
}
catch(ex)
{
    SH.Popup("Failed to contact the update server.\r\n" + ex.message + "Please try again later.", 
                10, POPTITLE, 16);
    WScript.Quit();
}

if (isRemoteNewer(pv, rpv))
{
    var ans = SH.Popup("There is a newer version (" + rpv + ") of the ASCOM Platform. Would you like to download it?",
                        0, POPTITLE, 36);
    if (ans == 6) 
        SH.Run(PLATPAGE);
    WScript.Quit();
}

if (dv !== "" && isRemoteNewer(dv, rdv))
{
    var ans = SH.Popup("There is a newer version (" + rdv + ") of the ASCOM Software Developer's Kit. Would you like to download it?",
                        0, POPTITLE, 36);
    if (ans == 6) 
        SH.Run(DEVPAGE);
    WScript.Quit();
}

if (cv !== "" && isRemoteNewer(cv, rcv))
{
    var ans = SH.Popup("There is a newer version (" + rcv + ") of the ASCOM Conformance Checker. Would you like to download it?",
                        0, POPTITLE, 36);
    if (ans == 6) 
        SH.Run(CONFPAGE);
}