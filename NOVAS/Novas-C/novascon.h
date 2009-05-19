/*
   NOVAS-C Version 2.0 (1 Nov 98)
   Header file for novascon.c

   Naval Observatory Vector Astrometry Subroutines
   C Version

   U. S. Naval Observatory
   Astronomical Applications Dept.
   3450 Massachusetts Ave., NW
   Washington, DC  20392-5420
*/

#include "ascom.h"	// [TPL]

#ifndef _CONSTS_
   #define _CONSTS_

   extern const short int FN1;
   extern const short int FN0;

/*
   TDB Julian date of epoch J2000.0.
*/

   extern const double T0;

/*
   Astronomical Unit in kilometers.
*/

   extern const double KMAU;

/*
   Astronomical Unit in meters.
*/

   extern const double MAU;

/*
   Speed of light in AU/Day.
*/

   extern const double C;

/*
   Heliocentric gravitational constant.
*/

   extern const double GS;

/*
   Radius of Earth in kilometers.
*/

   extern const double EARTHRAD;

/*
   'f' = Earth ellipsoid flattening
*/

   extern const double F;

/*
   'omega' = rotational angular velocity of Earth in radians/sec
*/

   extern const double OMEGA;

/*
   Value of 2.0 * pi in radians.
*/

   extern const double TWOPI;

/*
   Angle conversion constants.
*/

   extern const double RAD2SEC;
   extern const double DEG2RAD;
   extern const double RAD2DEG;

#endif
