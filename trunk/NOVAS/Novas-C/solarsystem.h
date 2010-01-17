/*
   NOVAS-C Version 2.0 (1 Nov 98)
   Header file for all source files containing versions of
   NOVAS-C function 'solarsystem'

   Naval Observatory Vector Astrometry Subroutines
   C Version

   U. S. Naval Observatory
   Astronomical Applications Dept.
   3450 Massachusetts Ave., NW
   Washington, DC  20392-5420
*/

#ifndef _SOLSYS_
   #define _SOLSYS_

/*
   Function prototypes
*/

   EXPORT short int solarsystem (double tjd, short int body, short int origin,
                          double *pos, double *vel);

   EXPORT void sun_eph (double jd,
              double *ra, double *dec, double *dis);

#endif
