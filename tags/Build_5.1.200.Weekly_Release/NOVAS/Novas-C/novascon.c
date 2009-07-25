/*
   NOVAS-C Version 2.0 (1 Nov 98)
   Constants file

   Naval Observatory Vector Astrometry Subroutines
   C Version

   U. S. Naval Observatory
   Astronomical Applications Dept.
   3450 Massachusetts Ave., NW
   Washington, DC  20392-5420
*/

#ifndef _CONSTS_
   #include "novascon.h"
#endif

const short int FN1 = 1;
const short int FN0 = 0;

/*
   TDB Julian date of epoch J2000.0.
*/

const double T0 = 2451545.00000000;

/*
   Astronomical Unit in kilometers.
*/

const double KMAU = 1.49597870e+8;

/*
   Astronomical Unit in meters.
*/

const double MAU = 1.49597870e+11;

/*
   Speed of light in AU/Day.
*/

const double C = 173.14463348;

/*
   Heliocentric gravitational constant.
*/

const double GS = 1.32712438e+20;

/*
   Radius of Earth in kilometers.
*/

const double EARTHRAD = 6378.140;

/*
   Earth ellipsoid flattening.
*/

const double F = 0.00335281;

/*
   Rotational angular velocity of Earth in radians/sec.
*/

const double OMEGA = 7.292115e-5;

/*
   Value of pi in radians.
*/

const double TWOPI = 6.28318530717958647692;

/*
   Angle conversion constants.
*/

const double RAD2SEC = 206264.806247096355;
const double DEG2RAD = 0.017453292519943296;
const double RAD2DEG = 57.295779513082321;

