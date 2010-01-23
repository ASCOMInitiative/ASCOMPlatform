/*
   NOVAS-C Version 3.0
   Header file for all source files containing versions of
   NOVAS-C function 'solarsystem'

   Naval Observatory Vector Astrometry Software
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

   short int solarsystem (double tjd, short int body, short int origin,

                          double *position, double *velocity);

   short int solarsystem_hp (double tjd[2], short body, short origin,

                          double *position, double *velocity);


#endif
