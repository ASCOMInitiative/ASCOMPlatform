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
// Author: Bob Denny <rdenny@dc3.com>
//
// Edits:
// ---------    ---     ---------------------------------------------------------------------------
// 28-Apr-11    rbd     Initial edit
// 29-Apr-11    rbd     Add logic to try to get various versions of MS XMLHTTP component, newest 
//                      to oldest order. Find the ASCOM registry data on both 32- and 64-bit
//                      systems. Make it obvious that you can come back here to go to the d/l
//                      page for the first of any remaining available updates.
// ------------------------------------------------------------------------------------------------

var POPTITLE = "Check ASCOM Updates";
var REMWEB = "http://download.ascom-standards.org/ver/";
var REGROOT64 = "HKLM\\Software\\Wow6432Node\\ASCOM\\Platform\\";
var REGROOT32 = "HKLM\\Software\\ASCOM\\Platform\\";

var PLATPAGE = "http://ascom-standards.org/Downloads/PlatformUpdates.htm";
var DEVPAGE = "http://ascom-standards.org/Downloads/PlatToolUpdates.htm";
var CONFPAGE = "http://ascom-standards.org/Downloads/DevTools.htm";

var SH = new ActiveXObject("WScript.Shell");

//
// Compare "n.n.n.n" versions (strings), return true if second parameter
// string represents a newer version.
//
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

//
// Valiantly try to get an XMLHTTPRequest object.
//
function getXmlHttp()
{
    //WScript.Echo("**try XMLHTTP.6.0");
    try { return new ActiveXObject("Msxml2.XMLHTTP.6.0"); }
    catch (e) {}
    
    //WScript.Echo("**try XMLHTTP.5.0");
    try { return new ActiveXObject("Msxml2.XMLHTTP.5.0"); }
    catch (e) {}
    
    //WScript.Echo("**try XMLHTTP.4.0");
    try { return new ActiveXObject("Msxml2.XMLHTTP.4.0"); }
    catch (e) {}
    
    //WScript.Echo("**try XMLHTTP.3.0");
    try { return new ActiveXObject("Msxml2.XMLHTTP.3.0"); }
    catch (e) {}

    //WScript.Echo("**try XMLHTTP");
    try { return new ActiveXObject("Msxml2.XMLHTTP"); }
    catch (e)
    {
        SN.Popup("Missing or damaged web component. Cannot check for updates.",
                10, POPTITLE, 16);
        WScript.Quit();
    }
}

//
// Get local versions
//
var regRoot;
try { SH.RegRead(REGROOT64); regRoot = REGROOT64; }
catch(e) { regRoot = REGROOT32;}
//WScript.Echo("**using " + regRoot);
var pd = SH.RegRead(regRoot);                                   // Platform description
var cv = SH.RegRead(regRoot + "Conform Version");
var dv = SH.RegRead(regRoot + "Developer Tools Version");
var pv = SH.RegRead(regRoot + "Platform Version");

//
// Get remote versions. XMLHTTP will automatically use default proxy if any.
//
try
{
    var http = getXmlHttp();
    http.open("GET", REMWEB + "cv.txt", false);
    http.send();
    var rcv = http.responseText;
    http.open("GET", REMWEB + "dv.txt", false);
    http.send();
    var rdv = http.responseText;
    http.open("GET", REMWEB + "pv.txt", false);
    http.send();
    var rpv = http.responseText;
    http = null;
}
catch(ex)
{
    SH.Popup("Failed to contact the update server.\r\n" + ex.message + "Please try again later.", 
                10, POPTITLE, 16);
    WScript.Quit();
}

var np = isRemoteNewer(pv, rpv);
var nd = (dv !== "") && isRemoteNewer(dv, rdv);
var nc = (cv !== "") && isRemoteNewer(cv, rcv);

if (!np && !nd && !nc)
{
    SH.Popup("Everything is up to date.", 10, POPTITLE, 64);
    WScript.Quit();
}

var i = 0;
var msg = "Updates are available for the following ASCOM components:\r\n";

if (np) { msg += "* ASCOM Platform (" + rpv + ")\r\n"; i += 1; }
if (nd) { msg += "* Developer Kit (" + rdv + ")\r\n"; i += 1; }
if (nc) { msg += "* Driver Conformance Checker (" + rcv + ")\r\n"; i += 1; }

if (i > 1) {
    msg += "After downloading and installing the first one, re-run the checker " +
           "to be taken to the download page for the next new component. Would you " + 
           "like to go to the download page for the first new component?"
} else {
    msg += "Would you like to go to the download page for the new component?";
}

var ans = SH.Popup(msg, 0, POPTITLE, 36);
if (ans == 7) WScript.Quit();

if (np)
{
    SH.Run(PLATPAGE);
    WScript.Quit();
}

if (nd)
{
    SH.Run(DEVPAGE);
    WScript.Quit();
}

if (nc)
{
    SH.Run(CONFPAGE);
}
