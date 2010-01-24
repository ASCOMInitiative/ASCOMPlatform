/*
   Naval Observatory Vector Astrometry Software

   NOVAS-C Version 3.0
   Header file for nutation.c

   U. S. Naval Observatory
   Astronomical Applications Dept.
   3450 Massachusetts Ave., NW
   Washington, DC  20392-5420
*/

#ifndef _NUTATION_
   #define _NUTATION_

/*
   Function prototypes
*/

   EXPORT void iau2000a (double jd_high, double jd_low,

                  double *dpsi, double *deps);

   EXPORT void iau2000b (double jd_high, double jd_low,

                  double *dpsi, double *deps);

   EXPORT void nu2000k (double jd_high, double jd_low,

                 double *dpsi, double *deps);

#endif
