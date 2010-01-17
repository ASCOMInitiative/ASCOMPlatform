/*
   readeph0.c: Dummy version of 'readeph.c' for use with NOVAS-C
               Version 3.0

   U. S. Naval Observatory
   Astronomical Applications Dept.
   3450 Massachusetts Ave., NW
   Washington, DC  20392-5420

   The real version of 'readeph' provides a heliocentric state vector
   (position and velocity) of a minor planet at a specified date.
   This dummy version is for use with NOVAS-C when positions of the
   minor planets are not of interest.
*/

#include <stdlib.h>

/*
   Function prototype.
*/

   double *readeph (int mp, char *name, double jd,

                    int *error );


/********readeph */

double *readeph (int mp, char *name, double jd,

                 int *error )
/*
------------------------------------------------------------------------

   PURPOSE:
      This is a dummy version of function 'readeph'.  It serves as a
      stub for the "real" 'readeph' (part of the USNO/AE98 minor
      planet ephemerides) when NOVAS-C is used without the
      minor planet ephemerides.

   REFERENCES:
      None.

   INPUT
   ARGUMENTS:
      mp (int)
         The number of the asteroid for which the position in desired.
      name (char*)
         The name of the asteroid.
      jd (double)
         The Julian date on which to find the position and velocity.

   OUTPUT
   ARGUEMENTS:
      *error (int)
         Error code; set equal to 9 (see note below).

   RETURNED
   VALUE:
      (double *)
         Pointer to the 6-element 'pv' array, with all elements set to
         zero.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      malloc      stdlib.c

   VER./DATE/
   PROGRAMMER:
      V1.0/06-97/JAB (USNO/AA)
      V1.1/08-98/JAB (USNO/AA): Support new 'readeph' argument list.
      V1.2/10-99/JAB (USNO/AA): Return a pointer to a double, rather
                                than an array of doubles.  Add error
                                9 on return.  Basic code courtesy JLH.

   NOTES:
      1.  This dummy function is not intended to be called.  It merely
      serves as a stub for the "real" 'readeph' when NOVAS-C is used
      without the minor planet ephemerides.  If this function is
      called, an error of 9 will be returned.

------------------------------------------------------------------------
*/
{
   int i;

   double *pv;

   pv = (double *) malloc (6L * sizeof (double));

   for (i = 0; i < 6; i++)
      pv[i] = 0.0;

   *error = 9;

   return pv;
}
