/*
   NOVAS-C Version 2.0 (1 Nov 98)
   Solar system function; version 2.

   Naval Observatory Vector Astrometry Subroutines
   C Version

   U. S. Naval Observatory
   Astronomical Applications Dept.
   3450 Massachusetts Ave., NW
   Washington, DC  20392-5420
*/


#ifndef _NOVAS_
   #include "novas.h"
#endif

/*
   Dummy function prototype for Fortran subroutine 'jplint'.
*/

void jplint (double *tjd, long int *targ, long int *cent,
 
            double *posvel, long int *err_flg);



/********solarsystem */

short int solarsystem (double tjd, short int body, short int origin,

                       double *pos, double *vel)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Provides an interface between the JPL direct-access solar system
      ephemerides and NOVAS-C.

   REFERENCES:
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TDB Julian date.
      body (int)
         Body identification number for the solar system object of
         interest;  Mercury = 1,...,Pluto = 9, Sun = 10, Moon = 11.
      origin (int)
         Origin code; solar system barycenter   = 0,
                      center of mass of the Sun = 1.

   OUTPUT
   ARGUMENTS:
      pos[3] (double)
         Position vector of 'body' at tjd; equatorial rectangular
         coordinates in AU referred to the mean equator and equinox
         of J2000.0.
      vel[3] (double)
         Velocity vector of 'body' at tjd; equatorial rectangular
         system referred to the mean equator and equinox of J2000.0,
         in AU/Day.

   RETURNED
   VALUE:
      (short int)
         0...Everything OK.
         1...Invalid value of body or origin.
         2...Error detected by JPL software.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      jplint (A Fortran subroutine that serves as the interface to 
              JPL's Fortran code that accesses the solar system 
              ephemerides)

   VER./DATE/
   PROGRAMMER:
      V1.0/10-97/JAB (USNO/AA)

   NOTES:
      1. This function is based on 'solsys2d.c' from version 1 of
         NOVAS-C.  It generalizes access to the JPL software by calling
         a Fortran interface subroutine, 'jplint', instead of making a
         direct call to the JPL subroutine 'pleph', whose arguments
         have changed several times throughout the years.  This way,
         any future change to the arguments can be accomodated in
         'jplint' rather than in this function.
------------------------------------------------------------------------
*/
{

   short int i, error = 0;

   long int targ, cent, err_flg = 0;

   double posvel[6] = {0.0,0.0,0.0,0.0,0.0,0.0};

/*
   Perform sanity checks on the input body and origin.
*/

   if ((body < 1) || (body > 11))
      return 1;
    else if ((origin < 0) || (origin > 1))
      return 1;

/*
   Select 'targ' according to the value of 'body'.
*/

   if (body == 10)
      targ = 11L;
     else if (body == 11)
      targ = 10L;
     else
      targ = (long int) body;

/*
   Select 'cent' according to the value of 'origin'.
*/

   if (origin == 0)
      cent = 12L;
    else if (origin == 1)
      cent = 11L;
    else
      return 1;

/*
   Call Fortran subroutine 'jplint' to obtain position and velocity
   array 'posvel'.  This is the only point in the NOVAS-C package
   where the Fortran/C interface occurs.
   Note that arguments must be sent to Fortran by reference, not by
   value.
*/

   jplint (&tjd, &targ, &cent, posvel, &err_flg);
   if (err_flg)
      return (error = 2);

/*
   Decompose 'posvel' into position 'pos' and velocity 'vel'.
*/

   for (i = 0; i < 3; i++)
   {
      pos[i] = posvel[i];
      vel[i] = posvel[i+3];
   }

   return 0;
}
