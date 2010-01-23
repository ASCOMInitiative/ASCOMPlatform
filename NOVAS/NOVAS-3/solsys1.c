/*
   NOVAS-C Version 3.0
   Solar system function; version 1.

   Naval Observatory Vector Astrometry Software
   C Version

   U. S. Naval Observatory
   Astronomical Applications Dept.
   3450 Massachusetts Ave., NW
   Washington, DC  20392-5420
*/


#ifndef _NOVAS_
   #include "novas.h"
#endif

#ifndef _EPHMAN_
   #include "eph_manager.h"
#endif


/********solarsystem */

short int solarsystem (double tjd, short int body, short int origin,

                       double *position, double *velocity)
/*
------------------------------------------------------------------------

   PURPOSE:
      Provides an interface between the JPL direct-access solar system
      ephemerides and NOVAS-C.

   REFERENCES:
      JPL. 2007, “JPL Planetary and Lunar Ephemerides: Export Information,”
	     (Pasadena, CA: JPL) http://ssd.jpl.nasa.gov/?planet_eph_export.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         Julian date of the desired time, on the TDB time scale.
      body (short)
         Body identification number for the solar system object of
         interest;Mercury = 1, ..., Pluto= 9, Sun= 10, Moon = 11.
      origin (short)
         Origin code; solar system barycenter= 0,
                      center of mass of the Sun = 1,
                      center of Earth = 2.

   OUTPUT
   ARGUMENTS:
      position (vectors)
         Position vector of 'body' at tjd; equatorial rectangular
         coordinates in AU referred to the ICRS.
      velocity (vectors)
         Velocity vector of 'body' at tjd; equatorial rectangular
         system referred to the ICRS.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      Planet_Ephemeris   (Eph_Manager.h)

   VER./DATE/
   PROGRAMMER:
      V1.0/03-93/WTH (USNO/AA): Convert FORTRAN to C.
      V1.1/07-93/WTH (USNO/AA): Update to C standards.
      V2.0/07-93/WTH (USNO/AA): Update to C standards.
      V2.1/06-99/JAB (USNO/AA): Minor style and documentation mods.
      V2.2/11-06/JAB (USNO/AA): Update to handle split-JD input
                                now supported in 'Planet_Ephemeris'.

   NOTES:
      1. This function and function 'Planet_Ephemeris' were designed
         to work with the 1997 version of the JPL ephemerides, as
         noted in the references.
      2. The user must create the binary ephemeris files using
         software from JPL, and open the file using function
         'Ephem_Open' in Eph_Manager.h, prior to calling this
         function.
      3. This function places the entire Julian date in the first
         element of the input time to 'Planet_Ephemeris'. This is
         adequate for all but the highest precision applications.  For
         highest precision, use function 'solarsystem_hp' in file
         'solsys1.c'.
      4. Function 'Planet_Ephemeris' is a C rewrite of the JPL Fortran
         subroutine 'pleph'.

------------------------------------------------------------------------
*/
{
   short int target, center;

   double jd[2];

/*
   Perform sanity checks on the input body and origin.
*/

   if ((body < 1) || (body > 11))
      return 1;
    else if ((origin < 0) || (origin > 2))
      return 2;

/*
   Select 'target' according to value of 'body'.
*/

   switch (body)
  {
      case 10:
         target = 10;
         break;
      case 11:
         target = 9;
         break;
      default:
         target = body - 1;
   }

/*
   Select 'center' according to the value of 'origin'.
*/

   if (!origin)
      center = 11;
    else if (origin == 1)
      center = 10;
    else if (origin == 2)
      center = 2;

/*
   Obtain position and velocity vectors.  The entire Julian date is
   contained in the first element of 'jd'.  This is adequate for all
   but the highest precision applications.
*/

   jd[0] = tjd;
   jd[1] = 0.0;

   Planet_Ephemeris (jd,target,center, position,velocity);


   return 0;
}

/********solarsystem_hp */

short int solarsystem_hp (double tjd[2], short int body,
                          short int origin,

                          double *position, double *velocity)
/*
------------------------------------------------------------------------

   PURPOSE:
      Provides an interface between the JPL direct-access solar system
      ephemerides and NOVAS-C for highest precision applications.

   REFERENCES:
      JPL. 2007, “JPL Planetary and Lunar Ephemerides: Export Information,”
	     (Pasadena, CA: JPL) http://ssd.jpl.nasa.gov/?planet_eph_export.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd[2] (double)
         Two-element array containing the Julian date, which may be
         split any way (although the first element is usually the
         "integer" part, and the second element is the "fractional"
         part).  Julian date is on the TDB or "T_eph" time scale.
      body (short)
         Body identification number for the solar system object of
         interest;Mercury = 1, ..., Pluto= 9, Sun= 10, Moon = 11.
      origin (short)
         Origin code; solar system barycenter= 0,
                      center of mass of the Sun = 1,
                      center of Earth = 2.

   OUTPUT
   ARGUMENTS:
      position (vectors)
         Position vector of 'body' at tjd; equatorial rectangular
         coordinates in AU referred to the ICRS.
      velocity (vectors)
         Velocity vector of 'body' at tjd; equatorial rectangular
         system referred to the ICRS.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      Planet_Ephemeris   (Eph_Manager.h)

   VER./DATE/
   PROGRAMMER:
      V1.0/11-06/JAB (USNO/AA): Update to handle split-JD input
                                now supported in 'Planet_Ephemeris'.

   NOTES:
      1. This function and function 'Planet_Ephemeris' were designed
         to work with the 1997 version of the JPL ephemerides, as
         noted in the references.
      2. The user must create the binary ephemeris files using
         software from JPL, and open the file using function
         'Ephem_Open' in Eph_Manager.h, prior to calling this
         function.
      3. This function supports the "split" Julian date feature of
         function 'Planet_Ephemeris' for highest precision.  For
         usual applications, use function 'solarsystem' in file
         'solsys1.c'.
      4. Function 'Planet_Ephemeris' is a C rewrite of the JPL Fortran
         subroutine 'pleph'.

------------------------------------------------------------------------
*/
{
   short int target, center;

/*
   Perform sanity checks on the input body and origin.
*/

   if ((body < 1) || (body > 11))
      return 1;
    else if ((origin < 0) || (origin > 2))
      return 2;

/*
   Select 'target' according to value of 'body'.
*/

   switch (body)
  {
      case 10:
         target = 10;
         break;
      case 11:
         target = 9;
         break;
      default:
         target = body - 1;
   }

/*
   Select 'center' according to the value of 'origin'.
*/

   if (!origin)
      center = 11;
    else if (origin == 1)
      center = 10;
    else if (origin == 2)
      center = 2;

/*
   Obtain position and velocity vectors.  The Julian date is split
   between two double-precision elements for highest precision.
*/

   Planet_Ephemeris (tjd,target,center, position,velocity);


   return 0;
}
