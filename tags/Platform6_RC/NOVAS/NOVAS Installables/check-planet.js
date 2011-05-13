//
// This tries to approximate the tests that came with NOVAS 2.0 for the
// minor planet Pallas. I didn't have the elements that were originally
// used, so I grabbed the latest from the Minor Planet Ephemeris Service.
// I then used the MPES to generate ephemerides for the astrometric
// postion. MPES does not give local topo (I'm not sure WHAT it gives??)
// Needless to say, this is more of a test of Kepler than NOVAS, but it
// does test the functionality of NOVAS when hooked up to Kepler.
//
// If you comment out the EarthEphemeris hookup to Kepler, the results
// are a shade worse, but still not bad!
//

var u = new ActiveXObject("DriverHelper.Util")

var s = new ActiveXObject("NOVAS.Site");
s.Latitude = 45;                                        // Standard USNO test site
s.Longitude = -75;
s.Height = 0.0;
s.Temperature = 10.0;
s.Pressure = 1010.0;

var n = new ActiveXObject("NOVAS.Planet");
n.Ephemeris = new ActiveXObject("Kepler.Ephemeris");
n.EarthEphemeris = new ActiveXObject("Kepler.Ephemeris");   // NOVAS sets up Kepler
n.Type = 1;                                             // kepMinorPlanet (passed to Kepler)
n.Name = "Pallas";                                      // (passed to Kepler)
n.Number = 2;                                           // (passed to Kepler)
n.DeltaT = 60.0;
//
// Orbital elements as of May, 2003
//
n.Ephemeris.a = 2.7730346;                              // Semimajor axis
n.Ephemeris.e = 0.2299839;                              // Eccentricity
n.Ephemeris.Epoch = 2452800.5;                          // Epoch
n.Ephemeris.Incl = 34.84989;                            // Inclination
n.Ephemeris.M = 260.69458;                              // Mean anomaly
n.Ephemeris.n = 0.21343771;                             // Mean daily motion
n.Ephemeris.Node = 173.16479;                           // Longitude of ascending node
n.Ephemeris.Peri = 310.45917;                           // Argument of perihelion

// Jan 1, 2003   June 1, 2003   December 31, 2003 (plus delta-T for 2003)
var jd = new Array(2452640.5, 2452791.5, 2453004.5);   // Test dates
var ra = new Array(21.626611, 0.919389, 1.489500);
var de = new Array(-4.742222, +2.960278, -21.761111);
var i;
for(i = 0; i < jd.length; i++)
{
    WScript.Echo("\r\nJD = " + u.FormatVar(jd[i], "0.0") + "  Body: Pallas");
    
    p = n.GetAstrometricPosition(jd[i]);
    WScript.Echo("  Astrometric:\r\n" +
                    "   RA = " +  u.FormatVar(p.RightAscension, "00.000000") + 
    				"  DEC = " + u.FormatVar(p.Declination, "00.00000") + 
    				"\r\n   eRA = " + u.FormatVar((ra[i] - p.RightAscension) * 15 * 3600, "0.0") + 
    				" arcsec  eDec = " + u.FormatVar((de[i] - p.Declination) * 3600, "0.0") + " arcsec");
    WScript.Echo("   Dist = " + u.FormatVar(p.Distance, "0.000000") + 
                    " AU  Light = " + u.FormatVar((p.LightTime * 1440), "0.0000") + " min");
    
    p = n.GetLocalPosition(jd[i], s);
    WScript.Echo("  Topocentric (unrefracted):\r\n" +
                    "   RA = " +  u.FormatVar(p.RightAscension, "00.000000") + 
    				"  DEC = " + u.FormatVar(p.Declination, "00.00000"));
    WScript.Echo("   Dist = " + u.FormatVar(p.Distance, "0.000000") + 
                    " AU  Light = " + u.FormatVar((p.LightTime * 1440), "0.0000") + " min");
    
    p = n.GetTopocentricPosition(jd[i], s, true);
    WScript.Echo("  Topocentric (refracted):\r\n" + 
                    "   RA = " +  u.FormatVar(p.RightAscension, "00.000000") + 
    				"  DEC = " + u.FormatVar(p.Declination, "00.00000"));
}




