// ---------------------
// TARGET LIST AND EPOCH
// ---------------------
//
// Edit this to change and add to the target list, and to change
// the ephemeris epoch.
//
var targets = new Array("9468","1997 XF11");
var tt0 = new Date("July 1, 2000 00:00:00 UTC");		// TT epoch
var tjd0 = 2440587.5 + (tt0.getTime() / 86400000.0);	// TJD epoch

// -------
// GLOBALS
// -------
var inet = WScript.CreateObject("InetCtls.Inet.1", "INC_");
inet.RequestTimeout = 360;
var action = "http://cfaps8.harvard.edu/~cgi/MPEph2.COM";
var hdrs = "Content-type: application/x-www-form-urlencoded\r\n" + 
		   "Referer: http://cfa-www.harvard.edu/iau/MPEph/MPEph.html\r\n";
var inet_err;

//----------------------------------------------------------------------------
// Formats sexagesimal but leaving the fractional part of the seconds
// Used to display astrometric quality RA and Dec. It's a pain in 
// JScript... there are apparently no number formatting services.
// Seconds are rounded to the nearest millisecond, preventing 
// microscopic rounding errors. The display is thus limited to 
// deciconds (1 decimal digit). If "pm" is true leading plus is added.
//----------------------------------------------------------------------------
function fmtSexa(n, pm)
{
    var sgn = pm ? "+" : "";                // Assume positive
    if(n < 0) {                             // Check neg.
        n = -n;                             // Make pos.
        sgn = "-";                          // Remember sign
    }
    var u = Math.floor(n)                   // Units (deg or hr)
    var us = u.toString();                  // Add leading 0 if needed
    if(us.length < 2) { us = "0" + us;  }
    n = (n - u) * 60.0;
    var m = Math.floor(n);                  // Minutes
    var ms = m.toString();                  // Add leading 0 if needed
    if(ms.length < 2) { ms = "0" + ms;  }
    var s = (Math.round((n - m) * 600.0)) / 10.0; // Seconds (thru decisec.)
    var ss = s.toString();
    var sb = ss.split(".");                 // Split units and fractional
    if(sb[0].length < 2) { ss = "0" + ss; } // Add leading 0 if needed
	if(sb.length == 1) { ss = ss + ".0"; }	// Add trailing 0

    return(sgn + us + " " + ms + " " + ss);
}

//----------------------------------------------------------------------------
// Make a date string for the MPC form (YYYY MM DD) from the given Date.
//----------------------------------------------------------------------------
function makeMPCDate(dt)
{
	var y = dt.getUTCFullYear();
	var m = dt.getUTCMonth() + 1;
	var d = dt.getUTCDate();
	var ys = y.toString();
	var ms = m.toString();
    if(ms.length < 2) { ms = "0" + ms;  }
    var ds = d.toString();
    if(ds.length < 2) { ds = "0" + ds;  }
    return(ys + " " + ms + " " + ds);
}

// ----------------------------------------------------------------------------
// Event handler for Internet Transfer Control (get_planets() and ???)
// ----------------------------------------------------------------------------
function INC_StateChanged(s)
{
	switch(s) {
		case 1:
			WScript.Echo("Resolving MPC host name...");
			break;
		case 2:
			WScript.Echo("Resolved.");
			break;
		case 3:
			WScript.Echo("Connecting to MPC...");
			break;
		case 4:
			WScript.Echo("Connected...");
			break;
		case 5:
			WScript.Echo("Requesting orbital elements...");
			break;
		case 6:
			WScript.Echo("Request complete.");
			break;
		case 7:
			WScript.Echo("Receiving orbital elements...");
			break;
		case 8:
			WScript.Echo("Got all sets of orbital elements.");
			break;
		case 9:
			WScript.Echo("Disconnecting...");
			break;
		case 10:
			WScript.Echo("Disconnected.");
			break;
		case 11:						// Error
			WScript.Echo(inet.ResponseInfo);
			break;
		case 12:
			WScript.Echo("MPC query complete.");
			break;
	}
}

// ----------------------------------------------------------------------------
// Fetch orbital elements from the MPC, return NOVAS.Planet objects ready
// to use.
//
// With the form settings we use, the elements come back back in
// Starry Night format (we ignore the explanatory text at the front):
//
// 0        1         2         3         4         5         6         7         8         9         0     
// 123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012
// Num   Name                Mag.       a          e        i       Node        w         L      Epoch
//
//  9468 Brewer             14.5     2.319898  0.143709   9.9976  197.4892  158.4800   98.9906  2451600.5
//       1997 XF11          16.8     1.441663  0.483627   4.0954  214.1174  102.4800  192.1593  2451600.5
//
// ----------------------------------------------------------------------------
function get_planets(tgts, date)
{
	var buf, i, n = 0;
	var planets = new Array(tgts.length);		// Returned array of vaPlanet
	var objlist = "";
	for(i = 0; i < tgts.length; i++) {			// Make the object list
		objlist += (tgts[i] + "\r\n");
	}
	objlist = escape(objlist); // URL Encode (for CR/LF)
	var fdata = "ty=e&TextArea=" + objlist + "&d=" + makeMPCDate(date) +
				"&l=1&i=1&u=d&c=663&raty=h&s=t&m=m&e=7&tit=" +
				"&bu=&ch=c&ce=f&js=f";
	fdata = fdata.replace(/ /g, "+");			// Plus to space

	inet_error = false;
	inet.Execute(action, "POST", fdata, hdrs);	// Post the "form" data
	while(inet.StillExecuting && !inet_error) {	// Wait for completion 
		WScript.sleep(1000);
	}

	if(inet_error) {							// Check status
		WScript.Echo("Error: " + inet.ResponseInfo);
		return(null);
	} else {
		buf = inet.GetChunk(65536);				// Good! Read response
	}
	if(buf.search(/No elements/) != -1) {		// Failed to find body
		WScript.Echo("No elements found.");
		return(null);
	}
	i = 0;
	var lines = buf.split(/\n/);
	while(lines[i++].search(/^Num   /) == -1);	// Skip past intro
	for(; i < lines.length; i++)				// for each element line
	{
		if(lines[i].length <= 1)				// Skip blank (\n only) lines
			continue;
		//
		// Create the vaPlanet, then attach a couple of Keplers
		// one for doing Earth work and the other for the M.P.
		//
		var p = new ActiveXObject("NOVAS.Planet");
		p.Type = 1;								// 1 = minor planet (both)
		p.EarthEphemeris = new ActiveXObject("Kepler.Ephemeris");
		p.Ephemeris = new ActiveXObject("Kepler.Ephemeris");
		//
		// Set the properties of the vaPlanet and the MP ephemeris
		//
		buf = lines[i].slice(0, 5).replace(/ /g, "");
		if(buf != "") {							// vaPlanet copies to Kepler
			p.Number = parseFloat(buf);			// (name, type, number)
		} else {
			p.Number = 100000;					// Not numbered
		}
		p.Name = lines[i].slice(6, 25).replace(/ +$/, "");
		var bits = lines[i].substr(25).split(/\s/);	// Split up numbers
		with(p.Ephemeris) {						// Fill in orbital parameters
			a = parseFloat(bits[1]);			// "a"
			e = parseFloat(bits[2]);			// "e"
			Incl = parseFloat(bits[3]);			// "i"
			Node = parseFloat(bits[4]);			// "Node"
			Peri = parseFloat(bits[5]);			// "w"
			M = parseFloat(bits[6]);			// "L" (2000.0)
			Epoch = parseFloat(bits[7]);		// "Epoch"
		}
		//
		// Store this new vaPlanet into the return array
		//
		planets[n++] = p;
	}
	return(planets);							// Return planets array
}

// ====
// MAIN
// ====

//
// (1) Get an array of vaPlanet objects initialized with elements from
// the MPC, ready to use for calculating the positions of the given
// minor planets.
//
var planets = get_planets(targets, tt0);
if(planets != null) {					// null -> no elements for target list
	//
	// (2) Now generate a 5-day stepped ephemerides for 30 days prior to
	// and 60 days following the given date tt0. Dump in a format
	// similar to the MPC.
	//
	for(i = 0; i < planets.length; i++)				// dump ephemerides
	{
		var p = planets[i];
		if(p.Number == 100000) {					// 100000 = "not numbered"
			WScript.Echo("\n " + p.Name);
		} else {
			WScript.Echo("\n (" + p.Number + ") " + p.Name);
		}
		var ejd = p.Ephemeris.Epoch;
		var edate  = new Date((ejd - 2440587.5) * 86400000).toUTCString();
		WScript.Echo("MPC elements of epoch " + edate + " TT = JDT " + ejd);
		WScript.Echo("Date    TT   RA (J2000.0) Dec");
	
		for(tjd = (tjd0 - 30); tjd <= (tjd0 + 60); tjd += 5)
		{
			var pv = p.GetAstrometricPosition(tjd);
			WScript.Echo(makeMPCDate(new Date((tjd - 2440587.5) * 86400000)) + 
						"   " + fmtSexa(pv.RightAscension, false) + 
						"  " + fmtSexa(pv.Declination, true));
		}
	}
}
