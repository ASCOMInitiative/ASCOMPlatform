//
// This checks out the NOVAS.vaEarth object. It also checks the
// ephemeris object (Kepler in this version) for sanity against
// the NOVAS internal earth/sun model used for barycenter calcs.
//
tjd = 2451911.54167;

r = new ActiveXObject("ASCOM.Astrometry.NOVASCOM.Earth");
e = new ActiveXObject("ASCOM.Astrometry.Kepler.Ephemeris");

r.SetForTime(tjd);
bt1 = r.BarycentricTime;
mo1 = r.MeanObliquity;
to1 = r.TrueObliquity;
nl1 = r.NutationInLongitude;
no1 = r.NutationInObliquity;
ee1 = r.EquationOfEquinoxes;

WScript.Echo("For TJD = " + tjd +
"\r\nb-tim: " + bt1 +
"\r\nm-obl: " + mo1 +
"\r\nt-obl: " + to1 +
"\r\nnut-l: " + nl1 +
"\r\nnut-o: " + no1 +
"\r\ne-equ: " + ee1);

bp1 = r.BarycentricPosition;
bv1 = r.BarycentricVelocity;
hp1 = r.HeliocentricPosition;
hv1 = r.HeliocentricVelocity;

WScript.Echo("\r\nInternal Earth model:" +
"\r\nb-pos: " + bp1.x + " " + bp1.y + " " + bp1.z +
"\r\nb-vel: " + bv1.x + " " + bv1.y + " " + bv1.z +
"\r\nh-pos: " + hp1.x + " " + hp1.y + " " + hp1.z +
"\r\nh-vel: " + hv1.x + " " + hv1.y + " " + hv1.z);

r.EarthEphemeris = e;

r.SetForTime(tjd - 1);				// Force internal recalc
r.SetForTime(tjd);
bp2 = r.BarycentricPosition;
bv2 = r.BarycentricVelocity;
hp2 = r.HeliocentricPosition;
hv2 = r.HeliocentricVelocity;

WScript.Echo("\r\nExternal Earth model:" +
"\r\nb-pos: " + bp2.x + " " + bp2.y + " " + bp2.z +
"\r\nb-vel: " + bv2.x + " " + bv2.y + " " + bv2.z +
"\r\nh-pos: " + hp2.x + " " + hp2.y + " " + hp2.z +
"\r\nh-vel: " + hv2.x + " " + hv2.y + " " + hv2.z);

WScript.Echo("\r\nDifferences:" +
"\r\nb-pos: " + (bp2.x - bp1.x) + " " + (bp2.y - bp1.y) + " " + (bp2.z - bp1.z) +
"\r\nb-vel: " + (bv2.x - bv1.x) + " " + (bv2.y - bv1.y) + " " + (bv2.z - bv1.z) +
"\r\nh-pos: " + (hp2.x - hp1.x) + " " + (hp2.y - hp1.y) + " " + (hp2.z - hp1.z) +
"\r\nh-vel: " + (hv2.x - hv1.x) + " " + (hv2.y - hv1.y) + " " + (hv2.z - hv1.z));
