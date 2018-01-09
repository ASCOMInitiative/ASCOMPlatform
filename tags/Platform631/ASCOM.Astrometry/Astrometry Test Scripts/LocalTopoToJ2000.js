//
// Sample/Test to convert Local Topo coordinates back to J2000
//

var I, SL, SJ, Sx, PJ, Px, PL, dx, dy, dz, RT, DT, TJD;

TJD = 2453391.36826;

I = new ActiveXObject("ASCOM.Astrometry.NOVASCOM.Site");
I.Latitude =  33.7833;
I.Longitude = -111.6167;
I.Height = 400;

S = new ActiveXObject("ASCOM.Astrometry.NOVASCOM.Star");
S.RightAscension = 20;
S.Declination = 10;

P = S.GetTopocentricPosition(TJD, I, false);    // NOVAS.PositionVector
RT = P.RightAscension;
DT = P.Declination;
WScript.Echo("Orig J2K " + S.RightAscension + " " + S.Declination +
"\r\nTopo " + RT + " " + DT);

P = null;                                       // Release this one
S = null;

// At this point we have a local topo to convert back

SL = new ActiveXObject("ASCOM.Astrometry.NOVASCOM.Star");
SL.RightAscension = RT;
SL.Declination = DT;
PL = SL.GetAstrometricPosition(TJD);             // Local topo vector

SJ = new ActiveXObject("ASCOM.Astrometry.NOVASCOM.Star");
SJ.RightAscension = RT;
SJ.Declination = DT;

for(j = 0; j < 4; j++)
{
    Px = SJ.GetTopocentricPosition(TJD, I, false);
    
    dx = PL.x - Px.x;
    dy = PL.y - Px.y;
    dz = PL.z - Px.z;
  
    PJ = SJ.GetAstrometricPosition(TJD);
    PJ.x += dx;
    PJ.y += dy;
    PJ.z += dz;
    SJ.RightAscension = PJ.RightAscension;
    SJ.Declination = PJ.Declination;
    
    WScript.Echo("OrgTopo " + PL.RightAscension + " " + PL.Declination +
    "\r\nNewTopo " + Px.RightAscension + " " + Px.Declination +
    "\r\n" + dx + " " + dy + " " + dz +
    "\r\nCalc J2K " + PJ.RightAscension + " " + PJ.Declination);
    
}

    WScript.Echo("Finished");