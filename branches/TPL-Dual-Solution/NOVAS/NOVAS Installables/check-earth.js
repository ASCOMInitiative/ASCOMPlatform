//
// This checks out the NOVAS.vaEarth object. It also checks the
// ephemeris object (Kepler in this version) for sanity against
// the NOVAS internal earth/sun model used for barycenter calcs.
//
tjd = 2451911.54167;

r = new ActiveXObject("NOVAS.Earth");
e = new ActiveXObject("Kepler.Ephemeris");

r.SetForTime(tjd);
bt1 = r.BarycentricTime;
mo1 = r.MeanObliquity;
to1 = r.TrueObliquity;
nl1 = r.NutationInLongitude;
no1 = r.NutationInObliquity;
ee1 = r.EquationOfEquinoxes;

WScript.Echo("For TJD = " + tjd + ":");
WScript.Echo("  b-tim: " + bt1);
WScript.Echo("  m-obl: " + mo1);
WScript.Echo("  t-obl: " + to1);
WScript.Echo("  nut-l: " + nl1);
WScript.Echo("  nut-o: " + no1);
WScript.Echo("  e-equ: " + ee1);

bp1 = r.BarycentricPosition;
bv1 = r.BarycentricVelocity;
hp1 = r.HeliocentricPosition;
hv1 = r.HeliocentricVelocity;

WScript.Echo("\r\nInternal Earth model:");
WScript.Echo("  b-pos: " + bp1.x + " " + bp1.y + " " + bp1.z);
WScript.Echo("  b-vel: " + bv1.x + " " + bv1.y + " " + bv1.z);
WScript.Echo("  h-pos: " + hp1.x + " " + hp1.y + " " + hp1.z);
WScript.Echo("  h-vel: " + hv1.x + " " + hv1.y + " " + hv1.z);

r.EarthEphemeris = e;

r.SetForTime(tjd - 1);				// Force internal recalc
r.SetForTime(tjd);
bp2 = r.BarycentricPosition;
bv2 = r.BarycentricVelocity;
hp2 = r.HeliocentricPosition;
hv2 = r.HeliocentricVelocity;

WScript.Echo("\r\nExternal Earth model:");
WScript.Echo("  b-pos: " + bp2.x + " " + bp2.y + " " + bp2.z);
WScript.Echo("  b-vel: " + bv2.x + " " + bv2.y + " " + bv2.z);
WScript.Echo("  h-pos: " + hp2.x + " " + hp2.y + " " + hp2.z);
WScript.Echo("  h-vel: " + hv2.x + " " + hv2.y + " " + hv2.z);

WScript.Echo("\r\nDifferences:");
WScript.Echo("  b-pos: " + (bp2.x - bp1.x) + " " + (bp2.y - bp1.y) + " " + (bp2.z - bp1.z));
WScript.Echo("  b-vel: " + (bv2.x - bv1.x) + " " + (bv2.y - bv1.y) + " " + (bv2.z - bv1.z));
WScript.Echo("  h-pos: " + (hp2.x - hp1.x) + " " + (hp2.y - hp1.y) + " " + (hp2.z - hp1.z));
WScript.Echo("  h-vel: " + (hv2.x - hv1.x) + " " + (hv2.y - hv1.y) + " " + (hv2.z - hv1.z));
