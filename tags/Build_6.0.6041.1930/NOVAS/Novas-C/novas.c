/*
   NOVAS-C Version 2.0.1 (10 Dec 99)

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

#include <math.h>

/*
   Global variables.

   'PSI_COR' and 'EPS_COR' are celestial pole offsets for high-
   precision applications.  See function 'cel_pole' for more details.
*/

static double PSI_COR = 0.0;
static double EPS_COR = 0.0;



/********app_star */

short int app_star (double tjd, body *earth, cat_entry *star,

                    double *ra, double *dec)
/*
------------------------------------------------------------------------

   PURPOSE:
      Computes the apparent place of a star at date 'tjd', given its
      mean place, proper motion, parallax, and radial velocity for
      J2000.0.

   REFERENCES:
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for apparent place.
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).
      *star (struct cat_entry)
         Pointer to catalog entry structure containing J2000.0 catalog
         data with FK5-style units (defined in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Apparent right ascension in hours, referred to true equator
         and equinox of date 'tjd'.
      *dec (double)
         Apparent declination in degrees, referred to true equator
         and equinox of date 'tjd'.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...Error code from function 'solarsystem'.

   GLOBALS
   USED:
      T0, FN0

   FUNCTIONS
   CALLED:
      get_earth       novas.c
      starvectors     novas.c
      proper_motion   novas.c
      bary_to_geo     novas.c
      sun_field       novas.c
      aberration      novas.c
      precession      novas.c
      nutate          novas.c
      vector2radec    novas.c

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/07-93/WTH (USNO/AA) Update to C standards.
      V1.2/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.3/06-97/JAB (USNO/AA) Incorporate 'body' structure in input.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'apstar'.

------------------------------------------------------------------------
*/
{
   short int error = 0;

   double tdb, time2, peb[3],veb[3], pes[3], ves[3], pos1[3], pos2[3],
      pos3[3], pos4[3], pos5[3], pos6[3], pos7[3], vel1[3];

/*
   Get the position and velocity of the Earth w/r/t the solar system
   barycenter and the center of mass of the Sun, on the mean equator
   and equinox of J2000.0
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return error;
   }

/*
   Compute apparent place
*/

   starvectors (star, pos1,vel1);
   proper_motion (T0,pos1,vel1,tdb, pos2);

   bary_to_geo (pos2,peb, pos3,&time2);
   sun_field (pos3,pes, pos4);
   aberration (pos4,veb,time2, pos5);
   precession (T0,pos5,tdb, pos6);
   nutate (tdb,FN0,pos6, pos7);

   vector2radec (pos7, ra,dec);

   return 0;
}

/********app_planet */

short int app_planet (double tjd, body *ss_object, body *earth, 

                      double *ra, double *dec, double *dis)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Compute the apparent place of a planet or other solar system body.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for apparent place.
      *ss_object (struct body)
         Pointer to structure containing the body designation for the
         solar system body (defined in novas.h).
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Apparent right ascension in hours, referred to true equator
         and equinox of date 'tjd'.
      *dec (double)
         Apparent declination in degrees, referred to true equator
         and equinox of date 'tjd'.
      *dis (double)
         True distance from Earth to planet at 'tjd' in AU.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...See error description in function 'ephemeris'.

   GLOBALS
   USED:
      BARYC, C, T0, FN0

   FUNCTIONS
   CALLED:
      get_earth      novas.c
      ephemeris      novas.c
      bary_to_geo    novas.c
      sun_field      novas.c
      aberration     novas.c
      precession     novas.c
      nutate         novas.c
      vector2radec   novas.c
      fabs           math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.2/06-97/JAB (USNO/AA) Generalize ephemeris management.
      V1.3/12-99/JAB (USNO/AA) Fix error return from 'get_earth'.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'applan'.

------------------------------------------------------------------------
*/
{
   short int error = 0;

   double tdb, peb[3], veb[3], pes[3], ves[3], t2, t3, lighttime,
      pos1[3], vel1[3], pos2[3], pos3[3], pos4[3], pos5[3], pos6[3];

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return (error + 10);
   }

/*
   Get position of planet wrt barycenter of solar system.
*/

   if (error = ephemeris (tdb,ss_object,BARYC, pos1,vel1))
   {
      *ra = 0.0;
      *dec = 0.0;
      *dis = 0.0;
      return error;
   }

   bary_to_geo (pos1,peb, pos2,&lighttime);
   *dis = lighttime * C;
   t3 = tdb - lighttime;

   do
   {
      t2 = t3;
      if (error = ephemeris (t2,ss_object,BARYC, pos1,vel1))
      {
         *ra = 0.0;
         *dec = 0.0;
         *dis = 0.0;
         return error;
      }

      bary_to_geo (pos1,peb, pos2,&lighttime);
      t3 = tdb - lighttime;
   } while (fabs (t3-t2) > 1.0e-8);

/*
   Finish apparent place computation.
*/

   sun_field (pos2,pes, pos3);
   aberration (pos3,veb,lighttime, pos4);
   precession (T0,pos4,tdb, pos5);
   nutate (tdb,FN0,pos5, pos6);
   vector2radec (pos6, ra,dec);

   return 0;
}

/********topo_star */

short int topo_star (double tjd, body *earth, double deltat,
                     cat_entry *star, site_info *location, 

                     double *ra, double *dec)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the topocentric place of a star at date 'tjd', given its
      mean place, proper motion, parallax, and radial velocity for
      J2000.0 and the location of the observer.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for topocentric place.
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).
      deltat (double)
         Difference TT (or TDT)-UT1 at 'tjd', in seconds.
      *star (struct cat_entry)
         Pointer to catalog entry structure containing J2000.0 catalog
         data with FK5-style units (defined in novas.h).
      *location (struct site_info)
         Pointer to structure containing observer's location (defined
         in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Topocentric right ascension in hours, referred to true equator
         and equinox of date 'tjd'.
      *dec (double)
         Topocentric declination in degrees, referred to true equator
         and equinox of date 'tjd'.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...Error code from function 'solarsystem'.

   GLOBALS
   USED:
      T0, FN1, FN0

   FUNCTIONS
   CALLED:
      get_earth       novas.c
      earthtilt       novas.c
      sidereal_time   novas.c
      terra           novas.c
      nutate          novas.c
      precession      novas.c
      starvectors     novas.c
      proper_motion   novas.c
      bary_to_geo     novas.c
      sun_field       novas.c
      aberration      novas.c
      vector2radec    novas.c

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.2/06-97/JAB (USNO/AA) Incorporate 'body' structure in input.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'tpstar'.

------------------------------------------------------------------------
*/
{
   short int error = 0;
   short int j;

   double lighttime, ujd, pob[3], pog[3], vob[3], vog[3], pos[3], gast,
      pos1[3], pos2[3], pos3[3], pos4[3], pos5[3], pos6[3], pos7[3],
      vel1[3], vel2[3], tdb, peb[3], veb[3], pes[3], ves[3], oblm,
      oblt, eqeq, psi, eps;

/*
   Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
*/

   ujd = tjd - (deltat / 86400.0);

/*
   Compute position and velocity of the observer, on mean equator
   and equinox of J2000.0, wrt the solar system barycenter and
   wrt to the center of the Sun.
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return error;
   }

   earthtilt (tdb, &oblm,&oblt,&eqeq,&psi,&eps);

   sidereal_time (ujd,0.0,eqeq, &gast);
   terra (location,gast, pos1,vel1);
   nutate (tdb,FN1,pos1, pos2);
   precession (tdb,pos2,T0, pog);

   nutate (tdb,FN1,vel1, vel2);
   precession (tdb,vel2,T0, vog);

   for (j = 0; j < 3; j++)
   {
      pob[j] = peb[j] + pog[j];
      vob[j] = veb[j] + vog[j];
      pos[j] = pes[j] + pog[j];
   }

/*
   Finish topocentric place calculation.
*/

   starvectors (star, pos1,vel1);
   proper_motion (T0,pos1,vel1,tdb, pos2);
   bary_to_geo (pos2,pob, pos3,&lighttime);
   sun_field (pos3,pos, pos4);
   aberration (pos4,vob,lighttime, pos5);
   precession (T0,pos5,tdb, pos6);
   nutate (tdb,FN0,pos6, pos7);

   vector2radec (pos7, ra,dec);

   return 0;
 }

/********topo_planet */

short int topo_planet (double tjd, body *ss_object, body *earth,
                       double deltat, site_info *location, 

                       double *ra, double *dec, double *dis)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the topocentric place of a planet, given the location of
      the observer.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for topocentric place.
      *ss_object (struct body)
         Pointer to structure containing the body designation for the
         solar system body (defined in novas.h).
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).
      deltat (double)
         Difference TT(or TDT)-UT1 at 'tjd', in seconds.
      *location (struct site_info)
         Pointer to structure containing observer's location (defined
         in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Topocentric right ascension in hours, referred to true
         equator and equinox of date 'tjd'.
      *dec (double)
         Topocentric declination in degrees, referred to true equator
         and equinox of date 'tjd'.
      *dis (double)
         True distance from observer to planet at 'tjd' in AU.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...See error description in function 'ephemeris'.

   GLOBALS
   USED:
      T0, FN0, FN1, BARYC

   FUNCTIONS
   CALLED:
      get_earth        novas.c
      earthtilt        novas.c
      sidereal_time    novas.c
      terra            novas.c
      nutate           novas.c
      precession       novas.c
      bary_to_geo      novas.c
      sun_field        novas.c
      aberration       novas.c
      vector2radec     novas.c
      ephemeris        novas.c
      fabs             math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.2/06-97/JAB (USNO/AA) Generalize ephemeris management.
      V1.3/12-99/JAB (USNO/AA) Fix error return from 'get_earth'.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'tpplan'. 

------------------------------------------------------------------------
*/
{
   short int error = 0;
   short int j;

   double ujd, t2, t3, gast, pos1[3], pos2[3], pos4[3], pos5[3],
      pos6[3], pos7[3], vel1[3], vel2[3], pog[3], vog[3], pob[3],
      vob[3], pos[3], lighttime, tdb, peb[3], veb[3], pes[3], ves[3],
      oblm, oblt, eqeq, psi, eps;

/*
   Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
*/

   ujd = tjd - (deltat / 86400.0);

/*
   Compute position and velocity of the observer, on mean equator
   and equinox of J2000.0, wrt the solar system barycenter and
   wrt to the center of the Sun.
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      *dis = 0.0;
      return (error + 10);
   }

   earthtilt (tdb, &oblm,&oblt,&eqeq,&psi,&eps);

   sidereal_time (ujd,0.0,eqeq, &gast);
   terra (location,gast, pos1,vel1);
   nutate (tdb,FN1,pos1, pos2);
   precession (tdb,pos2,T0, pog);

   nutate (tdb,FN1,vel1, vel2);
   precession (tdb,vel2,T0, vog);

   for (j = 0; j < 3; j++)
   {
      pob[j] = peb[j] + pog[j];
      vob[j] = veb[j] + vog[j];
      pos[j] = pes[j] + pog[j];
   }

/*
   Compute the apparent place of the planet using the position and
   velocity of the observer.

   Get position of planet wrt barycenter of solar system.
*/

   if (error = ephemeris (tdb,ss_object,BARYC, pos1,vel1))
   {
      *ra = 0.0;
      *dec = 0.0;
      *dis = 0.0;
      return error;
   }

   bary_to_geo (pos1,pob, pos2,&lighttime);
   *dis = lighttime * C;
   t3 = tdb - lighttime;

   do
   {
      t2 = t3;

      if (error = ephemeris (t2,ss_object,BARYC, pos1,vel1))
      {
         *ra = 0.0;
         *dec = 0.0;
         *dis = 0.0;
         return error;
      }
      bary_to_geo (pos1,pob, pos2,&lighttime);
      t3 = tdb - lighttime;

   } while (fabs (t3-t2) > 1.0e-8);

/*
   Finish topocentric place.
*/

   sun_field (pos2,pos, pos4);
   aberration (pos4,vob,lighttime, pos5);
   precession (T0,pos5,tdb, pos6);
   nutate (tdb,FN0,pos6, pos7);
   vector2radec (pos7, ra,dec);

   return error ;
}


/********virtual_star */

short int virtual_star (double tjd, body *earth, cat_entry *star,

                        double *ra, double *dec)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the virtual place of a star at date 'tjd', given its
      mean place, proper motion, parallax, and radial velocity for
      J2000.0.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for virtual place.
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).
      *star (struct cat_entry)
         Pointer to catalog entry structure containing J2000.0 catalog
         data with FK5-style units (defined in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Virtual right ascension in hours, referred to mean equator
         and equinox of J2000.
      *dec (double)
         Virtual declination in degrees, referred to mean equator
         and equinox of J2000.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...Error code from function 'solarsystem'.

   GLOBALS
   USED:
      T0

   FUNCTIONS
   CALLED:
      get_earth       novas.c
      starvectors     novas.c
      proper_motion   novas.c
      bary_to_geo     novas.c
      sun_field       novas.c
      aberration      novas.c
      vector2radec    novas.c

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.2/06-97/JAB (USNO/AA) Incorporate 'body' structure in input.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'vpstar'.

------------------------------------------------------------------------
*/
{
   short int error = 0;

   double pos1[3], vel1[3], pos2[3], pos3[3], pos4[3], pos5[3],
      tdb, peb[3], veb[3], pes[3], ves[3], lighttime;

/*
   Get the position and velocity of the Earth w/r/t the solar system
   barycenter and the center of mass of the Sun, on the mean equator
   and equinox of J2000.0
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return error;
   }

/*
   Compute virtual place.
*/

   starvectors (star, pos1,vel1);
   proper_motion (T0,pos1,vel1,tdb, pos2);
   bary_to_geo (pos2,peb, pos3,&lighttime);
   sun_field (pos3,pes, pos4);
   aberration (pos4,veb,lighttime, pos5);

   vector2radec (pos5, ra,dec);

   return 0;
 }

/********virtual_planet */

short int virtual_planet (double tjd, body *ss_object, body *earth,

                          double *ra, double *dec, double *dis)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the virtual place of a planet or other solar system body.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for virtual place.
      *ss_object (struct body)
         Pointer to structure containing the body designation for the
         solar system body (defined in novas.h).
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Virtual right ascension in hours, referred to mean equator
         and equinox of J2000.
      *dec (double)
         Virtual declination in degrees, referred to mean equator
         and equinox of J2000.
      *dis (double)
         True distance from Earth to planet in AU.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...See error description in function 'ephemeris'.

   GLOBALS
   USED:
      BARYC, C 

   FUNCTIONS
   CALLED:
      ephemeris      novas.c
      bary_to_geo    novas.c
      sun_field      novas.c
      aberration     novas.c
      vector2radec   novas.c
      get_earth      novas.c
      earthtilt      novas.c
      fabs           math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.2/06-97/JAB (USNO/AA) Generalize ephemeris management.
      V1.3/12-99/JAB (USNO/AA) Fix error return from 'get_earth'.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'vpplan'.

------------------------------------------------------------------------
*/
{
   short int error = 0;

   double t2 = 0.0, t3 = 0.0;
   double lighttime, pos1[3], vel1[3], pos2[3], pos3[3], pos4[3],
      tdb, peb[3], veb[3], pes[3], ves[3], oblm, oblt, eqeq, psi, eps;

/*
   Get position of Earth wrt barycenter of solar system.
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return (error + 10);
   }

   earthtilt (tdb, &oblm,&oblt,&eqeq,&psi,&eps);

/*
   Get position of planet wrt barycenter of solar system.
*/

   if (error = ephemeris (tdb,ss_object,BARYC, pos1,vel1))
   {
      *ra = 0.0;
      *dec = 0.0;
      return error;
   }
   bary_to_geo (pos1,peb, pos2,&lighttime);
   *dis = lighttime * C;
   t3 = tdb - lighttime;

   do
   {
      t2 = t3;
      if (error = ephemeris (t2,ss_object,BARYC, pos1,vel1))
      {
         *ra = 0.0;
         *dec = 0.0;
         return error;
      }
      bary_to_geo (pos1,peb, pos2,&lighttime);
      t3 = tdb - lighttime;
   } while (fabs (t3 - t2) > 1.0e-8);

/*
   Finish virtual place computation.
*/

   sun_field (pos2,pes, pos3);
   aberration (pos3,veb,lighttime, pos4);
   vector2radec (pos4, ra,dec);

   return 0;
}

/********local_star */

short int local_star (double tjd, body *earth, double deltat,
                      cat_entry *star, site_info *location,

                      double *ra, double *dec)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the local place of a star, given its mean place, proper
      motion, parallax, and radial velocity for J2000.0, and the
      location of the observer.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for local place.
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).
      deltat (double)
         Difference TT(or TDT)-UT1 at 'tjd', in seconds.
      *star (struct cat_entry)
         Pointer to catalog entry structure containing J2000.0 catalog
         data with FK5-style units (defined in novas.h).
      *location (struct site_info)
         Pointer to structure containing observer's location (defined
         in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Local right ascension in hours, referred to mean equator and
         equinox of J2000.
      *dec (double)
         Local declination in degrees, referred to mean equator and
         equinox of J2000.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...Error code from function 'solarsystem'.

   GLOBALS
   USED:
      T0, FN1

   FUNCTIONS
   CALLED:
      get_earth        novas.c
      earthtilt        novas.c
      sidereal_time    novas.c
      terra            novas.c
      nutate           novas.c
      precession       novas.c
      starvectors      novas.c
      proper_motion    novas.c
      bary_to_geo      novas.c
      sun_field        novas.c
      aberration       novas.c
      vector2radec     novas.c

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.3/06-97/JAB (USNO/AA) Incorporate 'body' structure in input.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'lpstar'.

------------------------------------------------------------------------
*/
{
   short int error = 0;
   short int j;

   double gast, lighttime, ujd, pog[3], vog[3], pb[3], vb[3], ps[3],
      vs[3], pos1[3], vel1[3], pos2[3], vel2[3], pos3[3], pos4[3],
      pos5[3], tdb, peb[3], veb[3], pes[3], ves[3], oblm, oblt, eqeq,
      psi, eps;

/*
   Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
*/

   ujd = tjd - (deltat / 86400.0);

/*
   Get position and velocity of observer wrt barycenter of solar system
   and wrt center of the sun.
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return error;
   }

   earthtilt (tdb, &oblm,&oblt,&eqeq,&psi,&eps);

   sidereal_time (ujd,0.0,eqeq, &gast);
   terra (location,gast, pos1,vel1);
   nutate (tdb,FN1,pos1, pos2);
   precession (tdb,pos2,T0, pog);
   nutate (tdb,FN1,vel1, vel2);
   precession (tdb,vel2,T0, vog);

   for (j = 0; j < 3; j++)
   {
      pb[j] = peb[j] + pog[j];
      vb[j] = veb[j] + vog[j];
      ps[j] = pes[j] + pog[j];
      vs[j] = ves[j] + vog[j];
   }

/*
   Compute local place.
*/

   starvectors (star, pos1,vel1);
   proper_motion (T0,pos1,vel1,tdb, pos2);
   bary_to_geo (pos2,pb, pos3,&lighttime);
   sun_field (pos3,ps, pos4);
   aberration (pos4,vb,lighttime, pos5);

   vector2radec (pos5, ra,dec);

   return 0;
}

/********local_planet */

short int local_planet (double tjd, body *ss_object, body *earth,
                        double deltat, site_info *location,

                        double *ra, double *dec, double *dis)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the local place of a planet or other solar system body,
      given the location of the observer.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for local place.
      *ss_object (struct body)
         Pointer to structure containing the body designation for the
         solar system body (defined in novas.h).
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).
      deltat (double)
         Difference TT(or TDT)-UT1 at 'tjd', in seconds.
      *location (struct site_info)
         Pointer to structure containing observer's location (defined
         in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Local right ascension in hours, referred to mean equator and
         equinox of J2000.
      *dec (double)
         Local declination in degrees, referred to mean equator and
         equinox of J2000.
      *dis (double)
         True distance from Earth to planet in AU.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...See error description in function 'ephemeris'.

   GLOBALS
   USED:
      T0, BARYC, FN1

   FUNCTIONS
   CALLED:
      get_earth       novas.c
      earthtilt       novas.c
      sidereal_time   novas.c
      terra           novas.c
      nutate          novas.c
      precession      novas.c
      bary_to_geo     novas.c
      sun_field       novas.c
      aberration      novas.c
      vector2radec    novas.c
      ephemeris       novas.c
      fabs            math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.2/06-97/JAB (USNO/AA) Generalize ephemeris management.
      V1.3/12-99/JAB (USNO/AA) Fix error return from 'get_earth'.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'lpplan'.

------------------------------------------------------------------------
*/
{

   short int error = 0;
   short int j;

   double t2 = 0.0, t3 = 0.0;
   double gast, lighttime, ujd, pog[3], vog[3], pb[3], vb[3], ps[3],
      vs[3], pos1[3], vel1[3], pos2[3], vel2[3], pos3[3], pos4[3],
      tdb, peb[3], veb[3], pes[3], ves[3], oblm, oblt, eqeq, psi, eps;

/*
   Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
*/

   ujd = tjd - (deltat / 86400.0);

/*
   Get position of Earth wrt the center of the Sun and the barycenter
   of solar system.
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return (error + 10);
   }

   earthtilt (tdb, &oblm,&oblt,&eqeq,&psi,&eps);

/*
   Get position and velocity of observer wrt center of the Earth.
*/

   sidereal_time (ujd,0.0,eqeq, &gast);
   terra (location,gast, pos1,vel1);
   nutate (tdb,FN1,pos1, pos2);
   precession (tdb,pos2,T0, pog);
   nutate(tdb,FN1,vel1, vel2);
   precession (tdb,vel2,T0, vog);

/*
   Get position and velocity of observer wrt barycenter of solar system
   and wrt center of the sun.
*/

   for (j = 0; j < 3; j++)
   {
      pb[j] = peb[j] + pog[j];
      vb[j] = veb[j] + vog[j];
      ps[j] = pes[j] + pog[j];
      vs[j] = ves[j] + vog[j];
   }

/*
   Get position of planet wrt barycenter of solar system.
*/

   if (error = ephemeris (tdb,ss_object,BARYC, pos1,vel1))
   {
      *ra= 0.0;
      *dec = 0.0;
      *dis = 0.0;
      return error;
   }
   bary_to_geo (pos1,pb, pos2,&lighttime);

   *dis = lighttime * C;
   t3 = tdb - lighttime;

   do
   {
      t2 = t3;
      if (error = ephemeris (t2,ss_object,BARYC, pos1,vel1))
      {
         *ra = 0.0;
         *dec = 0.0;
         return error;
      }
      bary_to_geo (pos1,pb, pos2,&lighttime);
      t3 = tdb - lighttime;
   } while (fabs (t3 - t2) > 1.0e-8);

/*
   Finish local place computation.
*/

   sun_field (pos2,ps, pos3);
   aberration (pos3,vb,lighttime, pos4);
   vector2radec (pos4, ra,dec);

   return 0;
}

/********astro_star */

short int astro_star (double tjd, body *earth, cat_entry *star,

                      double *ra, double *dec)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the astrometric place of a star, given its mean place,
      proper motion, parallax, and radial velocity for J2000.0.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for astrometric place.
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).
      *star (struct cat_entry)
         Pointer to catalog entry structure containing J2000.0 catalog
         data with FK5-style units (defined in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Astrometric right ascension in hours, referred to mean equator
         and equinox of J2000.
      *dec (double)
         Astrometric declination in degrees, referred to mean equator
         and equinox of J2000.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...Error code from function 'solarsystem'.

   GLOBALS
   USED:
      T0

   FUNCTIONS
   CALLED:
      get_earth       novas.c
      starvectors     novas.c
      proper_motion   novas.c
      bary_to_geo     novas.c
      vector2radec    novas.c

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.2/06-97/JAB (USNO/AA) Incorporate 'body' structure in input.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'asstar'.

------------------------------------------------------------------------
*/
{
   short int error=0;

   double lighttime, pos1[3], vel1[3], pos2[3], pos3[3], tdb, peb[3],
      veb[3], pes[3], ves[3];

/*
   Get the position and velocity of the Earth w/r/t the solar system
   barycenter and the center of mass of the Sun, on the mean equator
   and equinox of J2000.0
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return error;
   }

/*
   Compute astrometric place.
*/

   starvectors (star, pos1,vel1);
   proper_motion (T0,pos1,vel1,tdb, pos2);
   bary_to_geo (pos2,peb, pos3,&lighttime);

   vector2radec (pos3, ra,dec);

   return 0;
}

/********astro_planet */

short int astro_planet (double tjd, body *ss_object, body *earth,

                        double *ra, double *dec, double *dis)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the astrometric place of a planet or other solar system
      body.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date for calculation.
      *ss_object (struct body)
         Pointer to structure containing the body designation for the
         solar system body (defined in novas.h).
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).

   OUTPUT
   ARGUMENTS:
      *ra (double)
         Astrometric right ascension in hours, referred to mean equator
         and equinox of J2000.
      *dec (double)
         Astrometric declination in degrees, referred to mean equator
         and equinox of J2000.
      *dis (double)
         True distance from Earth to planet in AU.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...See error description in function 'ephemeris'.

   GLOBALS
   USED:
      C, BARYC

   FUNCTIONS
   CALLED:
      get_earth      novas.c
      ephemeris      novas.c
      bary_to_geo    novas.c
      vector2radec   novas.c
      fabs           math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/10-95/WTH (USNO/AA) Added call to 'get_earth'.
      V1.2/06-97/JAB (USNO/AA) Generalize ephemeris management.
      V1.3/12-99/JAB (USNO/AA) Fix error return from 'get_earth'.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'asplan'.

------------------------------------------------------------------------
*/
{
   short int error = 0;

   double t2 = 0.0, t3 = 0.0;
   double lighttime, pos1[3], vel1[3], pos2[3], tdb, peb[3], veb[3],
      pes[3], ves[3];

/*
   Get position of the Earth wrt center of Sun and barycenter of the
   solar system.
*/

   if (error = get_earth (tjd,earth, &tdb,peb,veb,pes,ves))
   {
      *ra = 0.0;
      *dec = 0.0;
      return (error + 10);
   }

/*
   Get position of planet wrt barycenter of solar system.
*/

   if (error = ephemeris (tdb,ss_object,BARYC, pos1,vel1))
   {
      *ra = 0.0;
      *dec = 0.0;
      *dis = 0.0;
      return error;
   }
   bary_to_geo (pos1,peb, pos2,&lighttime);
   *dis = lighttime * C;
   t3 = tdb - lighttime;
   do
   {
      t2 = t3;
      if (error = ephemeris (t2,ss_object,BARYC, pos1,vel1))
      {
         *ra = 0.0;
         *dec = 0.0;
         *dis = 0.0;
         return error;
      }
      bary_to_geo (pos1,peb, pos2,&lighttime);
      t3 = tdb - lighttime;
   } while (fabs (t3 - t2) > 1.0e-8);

/*
   Finish astrometric place computation.
*/

   vector2radec (pos2, ra,dec);

   return 0;
}

/********mean_star */

short int mean_star (double tjd, body *earth, double ra, double dec,

                     double *mra, double *mdec)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the mean place of a star for J2000.0, given its apparent
      place at date 'tjd'.  Proper motion, parallax and radial velocity
      are assumed to be zero.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date of apparent place.
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).
      ra (double)
         Apparent right ascension in hours, referred to true equator
         and equinox of date 'tjd'.
      dec (double)
         Apparent declination in degrees, referred to true equator
         and equinox of date 'tjd'.

   OUTPUT
   ARGUMENTS:
      *mra (double)
         Mean right ascension J2000.0 in hours.
      *mdec (double)
         Mean declination J2000.0 in degrees.

   RETURNED
   VALUE:
      (short int)
           0...Everything OK.
           1...Iterative process did not converge after 20 iterations.
         >10...Error from function 'app_star'.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      app_star     novas.c
      fmod         math.h
      fabs         math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.2/06-97/JAB (USNO/AA) Incorporate 'body' structure in input.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'mpstar'.

------------------------------------------------------------------------
*/
{
   short int iter = 0, error = 0;

   double newmra, newdec, oldmra, olddec, ra2, dec2, deltara, deltadec;

   cat_entry tempstar = {"CAT","dummy",0,0.0,0.0,0.0,0.0,0.0,0.0};

   newmra = fmod (ra,24.0);
   if (newmra < 0.0) 
      newmra += 24.0;
   newdec = dec;

   do
   {
      oldmra = newmra;
      olddec = newdec;
      tempstar.ra = oldmra;
      tempstar.dec = olddec;
      if (error = app_star (tjd,earth,&tempstar, &ra2,&dec2))
      {
         *mra = 0.0;
         *mdec = 0.0;
         return (error + 10);
      }
      deltara = ra2 - oldmra;
      deltadec = dec2 - olddec;
      if (deltara < -12.0)
         deltara += 24.0;
      if (deltara > 12.0)
         deltara -= 24.0;
      newmra = ra - deltara;
      newdec = dec - deltadec;

      if (iter >= 20)
      {
         *mra = 0.0;
         *mdec = 0.0;
         return 1;
      }
       else
         iter++;

   } while ((fabs (newmra - oldmra) > 1.0e-10) && 
            (fabs (newdec - olddec) > 1.0e-9));

   *mra = newmra;
   *mdec = newdec;
   if (*mra < 0.0) 
      *mra += 24.0;
   if (*mra >= 24.0) 
      *mra -= 24.0;

   return 0;
}

/******** sidereal_time */

void sidereal_time (double jd_high, double jd_low, double ee,

                    double *gst)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the Greenwich apparent sidereal time, at Julian date
      'jd_high' + 'jd_low'.

   REFERENCES: 
      Aoki, et al. (1982) Astronomy and Astrophysics 105, 359-361.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      jd_high (double)
         Julian date, integral part.
      jd_low (double)
         Julian date, fractional part.
      ee (double)
         Equation of the equinoxes (seconds of time). [Note: this 
         quantity is computed by function 'earthtilt'.]

   OUTPUT
   ARGUMENTS:
      *gst (double)
         Greenwich apparent sidereal time, in hours.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      T0

   FUNCTIONS
   CALLED:
      fmod    math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/06-92/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C programing standards.
      V1.2/03-98/JAB (USNO/AA) Expand documentation.
      V1.3/08-98/JAB (USNO/AA) Match flow of the Fortran counterpart.

   NOTES:
      1. The Julian date may be split at any point, but for highest
      precision, set 'jd_high' to be the integral part of the Julian
      date, and set 'jd_low' to be the fractional part.
      2. For Greenwich mean sidereal time, set input variable 'ee'
      equal to zero.
      3. This function is based on Fortran NOVAS routine 'sidtim'.

------------------------------------------------------------------------
*/
{
   double t_hi, t_lo, t, t2, t3, st;

   t_hi = (jd_high -  T0) / 36525.0;
   t_lo = jd_low / 36525.0;
   t = t_hi + t_lo;
   t2 = t * t;
   t3 = t2 * t;

   st =  ee - 6.2e-6 * t3 + 0.093104 * t2 + 67310.54841
      + 8640184.812866 * t_lo 
      + 3155760000.0 * t_lo
      + 8640184.812866 * t_hi 
      + 3155760000.0 * t_hi;

   *gst = fmod ((st / 3600.0), 24.0);

   if (*gst < 0.0)
      *gst += 24.0;

   return;
}

/********pnsw */

void pnsw (double tjd, double gast, double x, double y, double *vece,

           double *vecs)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Transforms a vector from an Earth-fixed geographic system to a
      space-fixed system based on mean equator and equinox of J2000.0;
      applies rotations for wobble, spin, nutation, and precession.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date
      gast (double)
         Greenwich apparent sidereal time, in hours.
      x (double)
         Conventionally-defined X coordinate of rotational pole with
         respect to CIO, in arcseconds.
      y (double)
         Conventionally-defined Y coordinate of rotational pole with
         respect to CIO, in arcseconds.
      vece[3] (double)
         Vector in geocentric rectangular Earth-fixed system,
         referred to geographic equator and Greenwich meridian.

   OUTPUT
   ARGUMENTS:
      vecs[3] (double)
         Vector in geocentric rectangular space-fixed system,
         referred to mean equator and equinox of J2000.0.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      T0, FN0

   FUNCTIONS
   CALLED:
      tdb2tdt           novas.c
      wobble            novas.c
      spin              novas.c
      nutate            novas.c
      precession        novas.c

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'pnsw'.
      2. 'tjd' = 0 means no precession/nutation transformation.
      3. 'gast' = 0 means no spin transformation.
      4. 'x' = 'y' = 0 means no wobble transformation.

------------------------------------------------------------------------
*/
{
   double dummy, secdiff, v1[3], v2[3], v3[3], tdb;

   short int j;

/*
   Compute 'tdb', the TDB Julian date corresponding to 'tjd'.
*/

   if (tjd != 0.0)
   {
      tdb2tdt (tjd, &dummy,&secdiff);
      tdb = tjd + secdiff / 86400.0;
   }

   if ((x == 0.0) && (y == 0.0))
   {
      for (j = 0; j < 3; j++)
         v1[j] = vece[j];
   }
    else
      wobble (x,y,vece, v1);

   if (gast == 0.0)
   {
      for (j = 0; j < 3; j++)
         v2[j] = v1[j];
   }
    else
      spin (gast,v1, v2);

   if (tjd == 0.0)
   {
      for (j = 0; j < 3; j++)
         vecs[j] = v2[j];
   }
    else
   {
      nutate (tdb,FN1,v2, v3);
      precession (tdb,v3,T0, vecs);
   }

   return;
}

/********spin */

void spin (double st, double *pos1,

           double *pos2)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Transforms geocentric rectangular coordinates from rotating system
      based on rotational equator and orthogonal reference meridian to 
      non-rotating system based on true equator and equinox of date.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      st (double)
         Local apparent sidereal time at reference meridian, in hours.
      pos1[3] (double)
         Vector in geocentric rectangular rotating system, referred
         to rotational equator and orthogonal reference meridian.

   OUTPUT
   ARGUMENTS:
      pos2[3] (double)
         Vector in geocentric rectangular non-rotating system,
         referred to true equator and equinox of date.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      DEG2RAD

   FUNCTIONS
   CALLED:
      sin     math.h
      cos     math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'spin'.

------------------------------------------------------------------------
*/
{
   double str, cosst, sinst, xx, yx, xy, yy;

   str = st * 15.0 * DEG2RAD;
   cosst = cos (str);
   sinst = sin (str);

/*
   Sidereal time rotation matrix follows.
*/

   xx =  cosst;
   yx = -sinst;
   xy =  sinst;
   yy =  cosst;

/*
   Perform rotation.
*/

   pos2[0] = xx * pos1[0] + yx * pos1[1];
   pos2[1] = xy * pos1[0] + yy * pos1[1];
   pos2[2] = pos1[2];

   return;
}

/********wobble */

void wobble (double x, double y, double *pos1,

             double *pos2)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Corrects Earth-fixed geocentric rectangular coordinates for polar
      motion.  Transforms a vector from Earth-fixed geographic system to
      rotating system based on rotational equator and orthogonal
      Greenwich meridian through axis of rotation.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      x (double)
         Conventionally-defined X coordinate of rotational pole with
         respect to CIO, in arcseconds.
      y (double)
         Conventionally-defined Y coordinate of rotational pole with
         respect to CIO, in arcseconds.
      pos1[3] (double)
         Vector in geocentric rectangular Earth-fixed system,
         referred to geographic equator and Greenwich meridian.

   OUTPUT
   ARGUMENTS:
      pos2[3] (double)
         Vector in geocentric rectangular rotating system, referred
         to rotational equator and orthogonal Greenwich meridian

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      RAD2SEC

   FUNCTIONS
   CALLED:
      None.

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'wobble'.

------------------------------------------------------------------------
*/
{
   double xpole, ypole, zx, zy, xz, yz;

   xpole = x / RAD2SEC;
   ypole = y / RAD2SEC;
   
/*
   Wobble rotation matrix follows.
*/

   zx = -xpole;
   zy =  ypole;
   xz =  xpole;
   yz = -ypole;

/*
   Perform rotation.
*/

   pos2[0] = pos1[0] + zx * pos1[2];
   pos2[1] = pos1[1] + zy * pos1[2];
   pos2[2] = xz * pos1[0] + yz * pos1[1] + pos1[2];

   return;
}

/********terra */

void terra (site_info *locale, double st,

            double *pos, double *vel)
/*
------------------------------------------------------------------------

   PURPOSE:
      Computes the position and velocity vectors of a terrestrial
      observer with respect to the center of the Earth.

   REFERENCES:
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      glon (double)
         Longitude of observer with respect to reference meridian
         (East +) in degrees.
      glat (double)
         Geodetic latitude (North +) of observer in degrees.
      ht (double)
         Height of observer in meters.
      st (double)
         Local apparent sidereal time at reference meridian in hours.

   OUTPUT
   ARGUMENTS:
      pos[3] (double)
         Position vector of observer with respect to center of Earth,
         equatorial rectangular coordinates, referred to true equator
         and equinox of date, components in AU.
      vel[3] (double)
         Velocity vector of observer with respect to center of Earth,
         equatorial rectangular coordinates, referred to true equator
         and equinox of date, components in AU/Day.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      KMAU, EARTHRAD, F, OMEGA, DEG2RAD

   FUNCTIONS
   CALLED:
      pow    math.h
      sin    math.h
      cos    math.h
      sqrt   math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/04-93/WTH (USNO/AA):  Translate Fortran.
      V1.1/06-98/JAB (USNO/AA):  Move constants 'f' and 'omega' to
                                 file 'novascon.c'.

   NOTES:
      1. If reference meridian is Greenwich and st=0, 'pos' is
      effectively referred to equator and Greenwich.
      2. This function is the "C" version of Fortran NOVAS routine
      'terra'.

------------------------------------------------------------------------
*/
{
   short int j;

   double df2, sinphi, cosphi, c, s, ach, ash, stlocl, sinst, cosst;

/*
   Compute parameters relating to geodetic to geocentric conversion.
*/

   df2 = pow ((1.0 - F),2);

   sinphi = sin (locale->latitude * DEG2RAD);
   cosphi = cos (locale->latitude * DEG2RAD);
   c = 1.0 / sqrt (pow (cosphi,2.0) + df2 * pow (sinphi,2.0));
   s = df2 * c;
   ach = EARTHRAD * c + (locale->height / 1000.0);
   ash = EARTHRAD * s + (locale->height / 1000.0);

/*
   Compute local sidereal time factors at the observer's longitude.
*/

   stlocl = (st * 15.0 + locale->longitude) * DEG2RAD;
   sinst = sin (stlocl);
   cosst = cos (stlocl);

/*
   Compute position vector components in kilometers.
*/

   pos[0] = ach * cosphi * cosst;
   pos[1] = ach * cosphi * sinst;
   pos[2] = ash * sinphi;

/*
   Compute velocity vector components in kilometers/sec.
*/

   vel[0] = -OMEGA * ach * cosphi * sinst;
   vel[1] =  OMEGA * ach * cosphi * cosst;
   vel[2] =  0.0;

/*
   Convert position and velocity components to AU and AU/DAY.
*/

   for (j = 0; j < 3; j++)
   {
      pos[j] /= KMAU;
      vel[j] /= KMAU;
      vel[j] *= 86400.0;
   }

   return;
}

/********earthtilt */

void earthtilt (double tjd, 

                double *mobl, double *tobl, double *eq, double *dpsi,
                double *deps)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes quantities related to the orientation of the Earth's
      rotation axis at Julian date 'tjd'.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.
      Transactions of the IAU (1994). Resolution C7; Vol. XXIIB, p. 59.
      McCarthy, D. D. (ed.) (1996). IERS Technical Note 21. IERS
         Central Bureau, Observatoire de Paris), pp. 21-22.

   INPUT
   ARGUMENTS:
      tjd (double)
         TDB Julian date of the desired time

   OUTPUT
   ARGUMENTS:
      *mobl (double)
         Mean obliquity of the ecliptic in degrees at 'tjd'.
      *tobl (double)
         True obliquity of the ecliptic in degrees at 'tjd'.
      *eq (double)
         Equation of the equinoxes in seconds of time at 'tjd'.
      *dpsi (double)
         Nutation in longitude in arcseconds at 'tjd'.
      *deps (double)
         Nutation in obliquity in arcseconds at 'tjd'.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      PSI_COR, EPS_COR, DEG2RAD 

   FUNCTIONS
   CALLED:
      nutation_angles  novas.c
      fund_args        novas.c
      fabs             math.h
      pow              math.h
      cos              math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-93/WTH (USNO/AA) Translate Fortran.
      V1.1/06-97/JAB (USNO/AA) Incorporate IAU (1994) and IERS (1996) 
                               adjustment to the "equation of the 
                               equinoxes".
      V1.2/10-97/JAB (USNO/AA) Implement function that computes 
                               arguments of the nutation series.
      V1.3/07-98/JAB (USNO/AA) Use global variables 'PSI_COR' and 
                               'EPS_COR' to apply celestial pole offsets
                               for high-precision applications.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'etilt'.
      2. Values of the celestial pole offsets 'PSI_COR' and 'EPS_COR'
      are set using function 'cel_pole', if desired.  See the prolog
      of 'cel_pole' for details.

------------------------------------------------------------------------
*/
{
   static double tjd_last = 0.0;
   static double t, dp, de;
   double d_psi, d_eps, mean_obliq, true_obliq, eq_eq, args[5];

/*
   Compute time in Julian centuries from epoch J2000.0.
*/

  t = (tjd - T0) / 36525.0;

/*
   Compute the nutation angles (arcseconds) from the standard nutation 
   model if the input Julian date is significantly different from the 
   last Julian date.
*/

  if (fabs (tjd - tjd_last) > 1.0e-6)
      nutation_angles (t, &dp,&de);

/*
   Apply observed celestial pole offsets.
*/

   d_psi = dp + PSI_COR;
   d_eps = de + EPS_COR;

/*
   Compute mean obliquity of the ecliptic in arcseconds.
*/

   mean_obliq = 84381.4480 - 46.8150 * t - 0.00059 * pow (t, 2.0)
      + 0.001813 * pow (t, 3.0);

/*
   Compute true obliquity of the ecliptic in arcseconds.
*/

   true_obliq = mean_obliq + d_eps;

/*
   Convert obliquity values to degrees.
*/

   mean_obliq /= 3600.0;
   true_obliq /= 3600.0;

/*
   Compute equation of the equinoxes in seconds of time.

   'args[4]' is "omega", the longitude of the ascending node of the 
   Moon's mean orbit on the ecliptic in radians.  This is also an 
   argument of the nutation series.
*/

   fund_args (t, args);

   eq_eq = d_psi * cos (mean_obliq * DEG2RAD) +
      (0.00264  * sin (args[4]) + 0.000063 * sin (2.0 * args[4]));

   eq_eq /= 15.0;
                           
/*
   Reset the value of the last Julian date and set the output values.
*/

   tjd_last = tjd;

   *dpsi = d_psi;
   *deps = d_eps;
   *eq = eq_eq;
   *mobl = mean_obliq;
   *tobl = true_obliq;

   return;
}

/********cel_pole */

void cel_pole (double del_dpsi, double del_deps)
/*
------------------------------------------------------------------------

   PURPOSE:
      This function allows for the specification of celestial pole
      offsets for high-precision applications.  These are added
      to the nutation parameters delta psi and delta epsilon.

   REFERENCES:
      None.

   INPUT
   ARGUMENTS:
      del_dpsi (double)
         Value of offset in delta psi (dpsi) in arcseconds.
      del_deps (double)
         Value of offset in delta epsilon (deps) in arcseconds.


   OUTPUT
   ARGUMENTS:
      None.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      PSI_COR, EPS_COR

   FUNCTIONS
   CALLED:
      None.

   VER./DATE/
   PROGRAMMER:
      V1.0/06-98/JAB (USNO/AA)

   NOTES:
      1. This function sets the values of global variables 'PSI_COR'
      and 'EPS_COR' declared at the top of file 'novas.c'.  These
      global variables are used only in NOVAS function 'earthtilt'.
      2. This function, if used, should be called before any other 
      NOVAS functions for a given date.  Values of the pole offsets 
      specified via a call to this function will be used until 
      explicitly changed.
      3. Daily values of the offsets are published, for example, in 
      IERS Bulletins A and B.
      4. This function is the "C" version of Fortran NOVAS routine
      'celpol'.

------------------------------------------------------------------------
*/
{
   PSI_COR = del_dpsi;
   EPS_COR = del_deps;

   return;
}

/******** get_earth */

short int get_earth (double tjd, body *earth,

                     double *tdb,
                     double *bary_earthp, double *bary_earthv,
                     double *helio_earthp, double *helio_earthv)
/*
------------------------------------------------------------------------

   PURPOSE:
      Obtains the barycentric & heliocentric positions and velocities
      of the Earth from the solar system ephemeris.

   REFERENCES:
      None.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date.
      *earth (struct body)
         Pointer to structure containing the body designation for the
         Earth (defined in novas.h).

   OUTPUT
   ARGUMENTS:
      *tdb (double)
         TDB Julian date corresponding to 'tjd'.
      *bary_earthp (double)
         Barycentric position vector of Earth at 'tjd'; equatorial
         rectangular coordinates in AU referred to the mean equator
         and equinox of J2000.0.
      *bary_earthv (double)
         Barycentric velocity vector of Earth at 'tjd'; equatorial
         rectangular system referred to the mean equator and equinox 
         of J2000.0, in AU/Day.
      *helio_earthp (double)
         Heliocentric position vector of Earth at 'tjd'; equatorial
         rectangular coordinates in AU referred to the mean equator
         and equinox of J2000.0.
      *helio_earthv (double)
         Heliocentric velocity vector of Earth at 'tjd'; equatorial
         rectangular system referred to the mean equator and equinox
         of J2000.0, in AU/Day.

   RETURNED
   VALUE:
      (short int)
          0...Everything OK.
         >0...Error code from function 'solarsystem'.

   GLOBALS
   USED:
      BARYC, HELIOC

   FUNCTIONS
   CALLED:
      tdb2tdt             novas.c
      solarsystem         (user's choice)
      fabs                math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/10-95/WTH (USNO/AA)
      V1.1/06-97/JAB (USNO/AA): Incorporate 'body' structure in input.

   NOTES:
      None.

------------------------------------------------------------------------
*/
{
   short int error = 0;
   short int earth_num, i;

   static double tjd_last = 0.0;
   static double time1,  peb[3], veb[3], pes[3], ves[3];
   double dummy, secdiff;

/*
   Compute the TDB Julian date corresponding to 'tjd'.
*/

   if (fabs (tjd - tjd_last) > 1.0e-6)
   {
      tdb2tdt (tjd, &dummy,&secdiff);
      time1 = tjd + secdiff / 86400.0;


/*
   Get position and velocity of the Earth wrt barycenter of solar system
   and wrt center of the sun.
*/

      earth_num = earth->number;

      if (error = solarsystem (time1,earth_num,BARYC, peb,veb))
      {
         tjd_last = 0.0;
         return error;
      }

      if (error = solarsystem (time1,earth_num,HELIOC, pes,ves))
      {
         tjd_last = 0.0;
         return error;
      }
      tjd_last = tjd;
   }

   *tdb = time1;
   for (i = 0; i < 3; i++)
   {
      bary_earthp[i] = peb[i];
      bary_earthv[i] = veb[i];
      helio_earthp[i] = pes[i];
      helio_earthv[i] = ves[i];
   }

   return error;
}

/********proper_motion */

void proper_motion (double tjd1, double *pos, double *vel,
                    double tjd2,

                    double *pos2)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Applies proper motion, including foreshortening effects, to a
      star's position.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd1 (double)
         TDB Julian date of first epoch.
      pos[3] (double)
         Position vector at first epoch.
      vel[3] (double)
         Velocity vector at first epoch.
      tjd2 (double)
         TDB Julian date of second epoch.

   OUTPUT
   ARGUMENTS:
      pos2[3] (double)
         Position vector at second epoch.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      None.

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Updated to C programming standards.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'propmo'.

------------------------------------------------------------------------
*/
{
    short int j;

    for (j = 0; j < 3; j++)
       pos2[j] = pos[j] + (vel[j] * (tjd2 - tjd1));

    return;
}

/********bary_to_geo */

void bary_to_geo (double *pos, double *earthvector,

                  double *pos2, double *lighttime)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Moves the origin of coordinates from the barycenter of the
      solar system to the center of mass of the Earth; i.e. corrects
      for parallax.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      pos1[3] (double)
         Position vector, referred to origin at solar system barycenter,
         components in AU.
      earthvector[3] (double)
         Position vector of center of mass of the Earth, referred to
         origin at solar system barycenter, components in AU.

   OUTPUT
   ARGUMENTS:
      pos2[3] (double)
         Position vector, referred to origin at center of mass of the
         Earth, components in AU.
      *lighttime (double)
         Light time from body to Earth in days.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      C

   FUNCTIONS
   CALLED:
      pow     math.h
      sqrt    math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C Standards.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'geocen'.

------------------------------------------------------------------------
*/
{
   short int j;

   double sum_of_squares;

/*
   Translate vector to geocentric coordinates.
*/

   for (j = 0; j < 3; j++)
      pos2[j] = pos[j] - earthvector[j];

/*
   Calculate length of vector in terms of light time.
*/

   sum_of_squares = pow (pos2[0], 2.0) + pow (pos2[1], 2.0)
                  + pow (pos2[2], 2.0);

   *lighttime = sqrt (sum_of_squares) / C;

   return;
}

/********sun_field */

short int sun_field (double *pos, double *earthvector,

                     double *pos2)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Corrects position vector for the deflection of light in the
      gravitational field of the Sun.  This function is valid for
      bodies within the solar system as well as for stars.

   REFERENCES:
      Misner, C., Thorne, K., and Wheeler, J. (1973). Gravitation;
         pp. 184-185. 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      pos[3] (double)
         Position vector, referred to origin at center of mass of the
         Earth, components in AU.
      earthvector[3] (double)
         Position vector of center of mass of the Earth, referred to
         origin at center of mass of the Sun, components in AU.

   OUTPUT
   ARGUMENTS:
      pos2[3] (double)
         Position vector, referred to origin at center of mass of the
         Earth, corrected for gravitational deflection, components
         in AU.

   RETURNED
   VALUE:
      (short int)
         0...Everything OK.

   GLOBALS
   USED:
      C, MAU, GS

   FUNCTIONS
   CALLED:
      fabs    math.h
      sqrt    math.h
      pow     math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C Standards.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'sunfld', member 'vasun1'.

------------------------------------------------------------------------
*/
{
   short int j;

   double f = 0.0;
   double p1mag, pemag,  cosd, sind, b, bm, pqmag, zfinl, zinit,
      xifinl, xiinit, delphi, delphp, delp, p1hat[3], pehat[3];

/*
   c = speed of light in meters/second.
*/

   double c = (C * MAU) / 86400.0;

/*
   Compute vector magnitudes and unit vectors.
*/

   p1mag = sqrt (pow (pos[0], 2.0) + pow (pos[1], 2.0)
               + pow (pos[2], 2.0));
   pemag = sqrt (pow (earthvector[0], 2.0) + pow (earthvector[1], 2.0) 
               + pow (earthvector[2], 2.0));

   for (j = 0; j < 3; j++)
   {
      p1hat[j] = pos[j] / p1mag;
      pehat[j] = earthvector[j] / pemag;
   }

/*
   Compute geometrical quantities.

   'cosd' and 'sind' are cosine and sine of d, the angular separation
   of the body from the Sun as viewed from the Earth.
*/

   cosd = - pehat[0] * p1hat[0] - pehat[1] * p1hat[1] 
          - pehat[2] * p1hat[2];

   if (fabs (cosd) > 0.9999999999)
   {
      for (j = 0; j < 3; j++)
         pos2[j] = pos[j];
   }
    else
   {
      sind = sqrt (1.0 - pow (cosd, 2.0));

/*
   'b' is the impact parameter for the ray.
*/

      b = pemag * sind;
      bm = b * MAU;

/*
   'pqmag' is the distance of the body from the sun.
*/

      pqmag = sqrt (pow (p1mag, 2.0) + pow (pemag, 2.0) 
                 - 2.0 * p1mag * pemag * cosd);

/*
   Compute 'delphi', the angle of deflection of the ray.
*/

      zfinl = pemag * cosd;
      zinit = -p1mag + zfinl;
      xifinl = zfinl / b;
      xiinit = zinit / b;

      delphi = 2.0 * GS / (bm * c * c) * (xifinl / 
                sqrt (1.0 + pow (xifinl, 2.0)) - xiinit /
                sqrt (1.0 + pow (xiinit, 2.0)));

/*
   Compute 'delphp', the change in angle as seen at the Earth.
*/

      delphp = delphi / (1.0 + (pemag / pqmag));

/*
   Fix up position vector.
   'pos2' is 'pos' rotated through angle 'delphp' in plane defined by
   'pos' and 'earthvector'.
*/

      f = delphp * p1mag / sind;

      for (j = 0; j < 3; j++)
      {
         delp = f * (cosd * p1hat[j] + pehat[j]);
         pos2[j] = pos[j] + delp;
      }
   }

   return 0;
}

/********aberration */

short int aberration (double *pos, double *ve, double lighttime,

                      double *pos2)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Corrects position vector for aberration of light.  Algorithm
      includes relativistic terms.

   REFERENCES: 
      Murray, C. A. (1981) Mon. Notices Royal Ast. Society 195, 639-648.
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      pos[3] (double)
         Position vector, referred to origin at center of mass of the
         Earth, components in AU.
      ve[3] (double)
         Velocity vector of center of mass of the Earth, referred to
         origin at solar system barycenter, components in AU/day.
      lighttime (double)
         Light time from body to Earth in days.

   OUTPUT
   ARGUMENTS:
      pos2[3] (double)
         Position vector, referred to origin at center of mass of the
         Earth, corrected for aberration, components in AU

   RETURNED
   VALUE:
      (short int)
         0...Everything OK.

   GLOBALS
   USED:
      C

   FUNCTIONS
   CALLED:
      sqrt      math.h
      pow       math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C Standards.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'aberat'.
      2. If 'lighttime' = 0 on input, this function will compute it.

------------------------------------------------------------------------
*/
{
   short int j;

   double p1mag, vemag, beta, dot,cosd, gammai, p, q, r;

   if (lighttime == 0.0)
   {
      p1mag = sqrt (pow (pos[0], 2.0) + pow (pos[1], 2.0)
                  + pow (pos[2], 2.0));
      lighttime = p1mag / C;
   }
    else
      p1mag = lighttime * C;

   vemag = sqrt (pow (ve[0], 2.0) + pow (ve[1], 2.0) 
               + pow (ve[2], 2.0));
   beta = vemag / C;
   dot = pos[0] * ve[0] + pos[1] * ve[1] + pos[2] * ve[2];

   cosd = dot / (p1mag * vemag);
   gammai = sqrt (1.0 - pow (beta, 2.0));
   p = beta * cosd;
   q = (1.0 + p / (1.0 + gammai)) * lighttime;
   r = 1.0 + p;

   for (j = 0; j < 3; j++)
      pos2[j] = (gammai * pos[j] + q * ve[j]) / r;

   return 0;
}

/********precession */

void precession (double tjd1, double *pos, double tjd2,

                 double *pos2)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Precesses equatorial rectangular coordinates from one epoch to
      another.  The coordinates are referred to the mean equator and
      equinox of the two respective epochs.

   REFERENCES:
      Explanatory Supplement to AE and AENA (1961); pp. 30-34.
      Lieske, J., et al. (1977). Astron. & Astrophys. 58, 1-16. 
      Lieske, J. (1979). Astron. & Astrophys. 73, 282-284. 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tjd1 (double)
         TDB Julian date of first epoch.
      pos[3] (double)
         Position vector, geocentric equatorial rectangular coordinates,
         referred to mean equator and equinox of first epoch.
      tjd2 (double)
         TDB Julian date of second epoch.

   OUTPUT
   ARGUMENTS:
      pos2[3] (double)
         Position vector, geocentric equatorial rectangular coordinates,
         referred to mean equator and equinox of second epoch.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      T0, RAD2SEC

   FUNCTIONS
   CALLED:
      sin    math.h
      cos    math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C Standards.
      V1.2/03-98/JAB (USNO/AA) Change function type from 'short int' to
                               'void'.
      V1.3/12-99/JAB (USNO/AA) Precompute trig terms for greater 
                               efficiency.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'preces'.

------------------------------------------------------------------------
*/
{
   double xx, yx, zx, xy, yy, zy, xz, yz, zz, t, t1, t02, t2, t3,
      zeta0, zee, theta, cz0, sz0, ct, st, cz, sz;

/*
   't' and 't1' below correspond to Lieske's "big T" and "little t".
*/

   t = (tjd1 - T0) / 36525.0;
   t1 = (tjd2 - tjd1) / 36525.0;
   t02 = t * t;
   t2 = t1 * t1;
   t3 = t2 * t1;

/*
   'zeta0', 'zee', 'theta' below correspond to Lieske's "zeta-sub-a",
   "z-sub-a", and "theta-sub-a".
*/

   zeta0 = (2306.2181 + 1.39656 * t - 0.000139 * t02) * t1
         + (0.30188 - 0.000344 * t) * t2 + 0.017998 * t3;

   zee = (2306.2181 + 1.39656 * t - 0.000139 * t02) * t1
       + (1.09468 + 0.000066 * t) * t2 + 0.018203 * t3;

   theta = (2004.3109 - 0.85330 * t - 0.000217 * t02) * t1
         + (-0.42665 - 0.000217 * t) * t2 - 0.041833 * t3;

   zeta0 /= RAD2SEC;
   zee /= RAD2SEC;
   theta /= RAD2SEC;

/*
   Precalculate trig terms.
*/

   cz0 = cos (zeta0);
   sz0 = sin (zeta0);
   ct = cos (theta);
   st = sin (theta);
   cz = cos (zee);
   sz = sin (zee);

/*
   Precession rotation matrix follows.
*/

   xx =  cz0 * ct * cz - sz0 * sz;
   yx = -sz0 * ct * cz - cz0 * sz;
   zx = -st * cz;
   xy = cz0 * ct * sz + sz0 * cz;
   yy = -sz0 * ct * sz + cz0 * cz;
   zy = -st * sz;
   xz = cz0 * st;
   yz = -sz0 * st;
   zz = ct;

/*
   Perform rotation.
*/

   pos2[0] = xx * pos[0] + yx * pos[1] + zx * pos[2];
   pos2[1] = xy * pos[0] + yy * pos[1] + zy * pos[2];
   pos2[2] = xz * pos[0] + yz * pos[1] + zz * pos[2];

   return;
}

/********nutate */

short int nutate (double tjd, short int fn, double *pos, 

                  double *pos2)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Nutates equatorial rectangular coordinates from mean equator and
      equinox of epoch to true equator and equinox of epoch. Inverse
      transformation may be applied by setting flag 'fn'.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      tdb (double)
         TDB julian date of epoch.
      fn (short int)
         Flag determining 'direction' of transformation;
            fn  = 0 transformation applied, mean to true.
            fn != 0 inverse transformation applied, true to mean.
      pos[3] (double)
         Position vector, geocentric equatorial rectangular coordinates,
         referred to mean equator and equinox of epoch.

   OUTPUT
   ARGUMENTS:
      pos2[3] (double)
         Position vector, geocentric equatorial rectangular coordinates,
         referred to true equator and equinox of epoch.

   RETURNED
   VALUE:
      (short int)
         0...Everything OK.

   GLOBALS
   USED:
      DEG2RAD, RAD2SEC

   FUNCTIONS
   CALLED:
      earthtilt     novas.c
      cos           math.h
      sin           math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C Standards.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'nutate'.

------------------------------------------------------------------------
*/
{
   double cobm, sobm, cobt, sobt, cpsi, spsi, xx, yx, zx, xy, yy, zy,
      xz, yz, zz, oblm, oblt, eqeq, psi, eps;

   earthtilt (tjd, &oblm,&oblt,&eqeq,&psi,&eps);

   cobm = cos (oblm * DEG2RAD);
   sobm = sin (oblm * DEG2RAD);
   cobt = cos (oblt * DEG2RAD);
   sobt = sin (oblt * DEG2RAD);
   cpsi = cos (psi / RAD2SEC);
   spsi = sin (psi / RAD2SEC);

/*
   Nutation rotation matrix follows.
*/

   xx = cpsi;
   yx = -spsi * cobm;
   zx = -spsi * sobm;
   xy = spsi * cobt;
   yy = cpsi * cobm * cobt + sobm * sobt;
   zy = cpsi * sobm * cobt - cobm * sobt;
   xz = spsi * sobt;
   yz = cpsi * cobm * sobt - sobm * cobt;
   zz = cpsi * sobm * sobt + cobm * cobt;

   if (!fn)
   {

/*
   Perform rotation.
*/

      pos2[0] = xx * pos[0] + yx * pos[1] + zx * pos[2];
      pos2[1] = xy * pos[0] + yy * pos[1] + zy * pos[2];
      pos2[2] = xz * pos[0] + yz * pos[1] + zz * pos[2];
   }
    else
   {

/*
   Perform inverse rotation.
*/

      pos2[0] = xx * pos[0] + xy * pos[1] + xz * pos[2];
      pos2[1] = yx * pos[0] + yy * pos[1] + yz * pos[2];
      pos2[2] = zx * pos[0] + zy * pos[1] + zz * pos[2];
   }

   return 0;
}

/********nutation_angles */

short int nutation_angles (double t,

                           double *longnutation, double *obliqnutation)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Provides fast evaluation of the nutation components according to
      the 1980 IAU Theory of Nutation.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210, and references therein.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.
      Miller, B. R. (1989). Proceedings of the ACM-SIGSAM International
         Symposium on Symbolic and Algebraic Computation; pp. 199-206.

   INPUT
   ARGUMENTS:
      t (double)
         TDB time in Julian centuries since J2000.0

   OUTPUT
   ARGUMENTS:
      *longnutation (double)
         Nutation in longitude in arcseconds.
      *obliqnutation (double)
         Nutation in obliquity in arcseconds.

   RETURNED
   VALUE:
      (short int)
         0...Everything OK.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      fund_args         novas.c
      sin               math.h
      cos               math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/11-88/BRM (NIST)
      V1.1/08-93/WTH (USNO/AA): Translate Fortran.
      V1.2/10-97/JAB (USNO/AA): Add function to compute arguments.

   NOTES:
      1. This function is based on computer-generated Fortran code.
      Original Fortran code generated on 11/29/88 16:35:35 at the
      National Institutes of Standards and Technology (NIST), by 
      Bruce R. Miller.
      2. This function is the "C" version of Fortran NOVAS routine
      'nod', member 'vanut1f'.

------------------------------------------------------------------------
*/
{
   double clng[106] = {1.0,   1.0,  -1.0, -1.0,   1.0,  -1.0,  -1.0,
                      -1.0,  -1.0,  -1.0, -1.0,   1.0,  -1.0,   1.0,
                      -1.0,   1.0,   1.0, -1.0,  -1.0,   1.0,   1.0,
                      -1.0,   1.0,  -1.0,  1.0,  -1.0,  -1.0,  -1.0,
                       1.0,  -1.0,  -1.0,  1.0,  -1.0,   1.0,   2.0,
                       2.0,   2.0,   2.0,  2.0,  -2.0,   2.0,   2.0,
                       2.0,   3.0,  -3.0, -3.0,   3.0,  -3.0,   3.0,
                      -3.0,   3.0,   4.0,  4.0,  -4.0,  -4.0,   4.0,
                      -4.0,   5.0,   5.0,  5.0,  -5.0,   6.0,   6.0,
                       6.0,  -6.0,   6.0, -7.0,   7.0,   7.0,  -7.0,
                      -8.0,  10.0,  11.0, 12.0, -13.0, -15.0, -16.0,
                     -16.0,  17.0, -21.0,-22.0,  26.0,  29.0,  29.0,
                     -31.0, -38.0, -46.0, 48.0, -51.0,  58.0,  59.0,
                      63.0,  63.0,-123.0,129.0,-158.0,-217.0,-301.0,
                    -386.0,-517.0, 712.0,1426.0,2062.0,-2274.0,
                  -13187.0,-171996.0},
      clngx[14]={ 0.1,-0.1,0.1,0.1,0.1,0.1,0.2,-0.2,-0.4,0.5,1.2,
                 -1.6,-3.4,-174.2},
       cobl[64]={    1.0,    1.0,    1.0,   -1.0,   -1.0,   -1.0,
                     1.0,    1.0,    1.0,    1.0,    1.0,   -1.0,
                     1.0,   -1.0,    1.0,   -1.0,   -1.0,   -1.0,
                     1.0,   -1.0,    1.0,    1.0,   -1.0,   -2.0,
                    -2.0,   -2.0,    3.0,    3.0,   -3.0,    3.0,
                     3.0,   -3.0,    3.0,    3.0,   -3.0,    3.0,
                     3.0,    5.0,    6.0,    7.0,   -7.0,    7.0,
                    -8.0,    9.0,  -10.0,  -12.0,   13.0,   16.0,
                   -24.0,   26.0,   27.0,   32.0,  -33.0,  -53.0,
                    54.0,  -70.0,  -95.0,  129.0,  200.0,  224.0,
                  -895.0,  977.0, 5736.0,92025.0},
       coblx[8]={ -0.1, -0.1,  0.3,  0.5, -0.5, -0.6, -3.1,  8.9};

   short int i, ii, i1, i2, iop;
   short int nav1[10]={0,0,1,0,2,1,3,0,4,0},
       nav2[10]={ 0, 0, 0, 5, 1, 1, 3, 3, 4, 4},
       nav[183]={ 2, 0, 1, 1, 5, 2, 2, 0, 2, 1, 0, 3, 2, 5, 8, 1,17, 8,
                  1,18, 0, 2, 0, 8, 0, 1, 3, 2, 1, 8, 0,17, 1, 1,15, 1,
                  2,21, 1, 1, 2, 8, 2, 0,29, 1,21, 2, 2, 1,29, 2, 0, 9,
                  2, 5, 4, 2, 0, 4, 0, 1, 9, 2, 1, 4, 0, 2, 9, 2, 2, 4,
                  1,14,44, 2, 0,45, 2, 5,44, 2,50, 0, 1,36, 2, 2, 5,45,
                  1,37, 2, 2, 1,45, 2, 1,44, 2,53, 1, 2, 8, 4, 1,40, 3,
                  2,17, 4, 2, 0,64, 1,39, 8, 2,27, 4, 1,50,18, 1,21,47,
                  2,44, 3, 2,44, 8, 2,45, 8, 1,46, 8, 0,67, 2, 1, 5,74,
                  1, 0,74, 2,50, 8, 1, 5,78, 2,17,53, 2,53, 8, 2, 0,80,
                  2, 0,81, 0, 7,79, 1, 7,81, 2, 1,81, 2,24,44, 1, 1,79,
                  2,27,44},
      llng[106]={ 57, 25, 82, 34, 41, 66, 33, 36, 19, 88, 18,104, 93,
                  84, 47, 28, 83, 86, 69, 75, 89, 30, 58, 73, 46, 77,
                  23, 32, 59, 72, 31, 16, 74, 22, 98, 38, 62, 96, 37,
                  35,  6, 76, 85, 51, 26, 10, 13, 63,105, 52,102, 67,
                  99, 15, 24, 14,  3,100, 65, 11, 55, 68, 20, 87, 64,
                  95, 27, 60, 61, 80, 91, 94, 12, 43, 71, 42, 97, 70,
                   7, 49, 29,  2,  5, 92, 50, 78, 56, 17, 48, 40, 90,
                   8, 39, 54, 81, 21,103, 53, 45,101,  0,  1,  9, 44,
                  79,  4},
      llngx[14]={ 81, 7, 97, 0, 39, 40, 9, 44, 45,103,101, 79, 1, 4},
      lobl[64]={  51, 98, 17, 21,  5,  2, 63,105, 38, 52,102, 62, 96,
                  37, 35, 76, 36, 88, 85,104, 93, 84, 83, 67, 99,  8,
                  68,100, 60, 61, 91, 87, 64, 80, 95, 65, 55, 94, 43,
                  97,  0, 71, 70, 42, 49, 92, 50, 78, 56, 90, 48, 40,
                  39, 54,  1, 81,103, 53, 45,101,  9, 44, 79,  4},
      loblx[8] ={ 53,  1,103,  9, 44,101, 79,  4};

   double a[5], angle, cc, ss1, cs, sc, c[106], s[106], lng, lngx, obl,
      oblx;

/*
   Compute the arguments of the nutation series in radians.
*/

   fund_args (t, a);

/*
   Evaluate the series.
*/

   i = 0;
   for (ii = 0; ii < 10; ii += 2)
   {
      angle = a[nav1[ii]] * (double) (nav1[1+ii]+1);
      c[i] = cos (angle);
      s[i] = sin (angle);
      i += 1;
   }

   i = 5;
   for (ii = 0; ii < 10; ii += 2)
   {
      i1 = nav2[ii];
      i2 = nav2[1+ii];

      c[i] = c[i1] * c[i2] - s[i1] * s[i2];
      s[i] = s[i1] * c[i2] + c[i1] * s[i2];
      i += 1;
   }

   i = 10;
   for (ii = 0; ii < 183; ii += 3)
   {
      iop = nav[ii];
      i1 = nav[1+ii];
      i2 = nav[2+ii];
      switch (iop)
      {
         case 0:
            c[i] = c[i1] * c[i2] - s[i1] * s[i2];
            s[i] = s[i1] * c[i2] + c[i1] * s[i2];
            i += 1;
            break;
         case 1:
            c[i] = c[i1] * c[i2] + s[i1] * s[i2];
            s[i] = s[i1] * c[i2] - c[i1] * s[i2];
            i += 1;
            break;
         case 2:
            cc = c[i1] * c[i2];
            ss1 = s[i1] * s[i2];
            sc = s[i1] * c[i2];
            cs = c[i1] * s[i2];
            c[i] = cc - ss1;
            s[i] = sc + cs;
            i += 1;
            c[i] = cc + ss1;
            s[i] = sc - cs;
            i += 1;
            break;
      }
      if (iop == 3)
         break;
   }

   lng = 0.0;
   for (i = 0; i < 106; i++)
      lng += clng[i] * s[llng[i]];

   lngx = 0.0;
   for (i = 0; i < 14; i++)
      lngx += clngx[i] * s[llngx[i]];

   obl = 0.0;
   for (i = 0; i < 64; i++)
      obl += cobl[i] * c[lobl[i]];

   oblx = 0.0;
   for (i = 0; i < 8; i++)
      oblx += coblx[i] * c[loblx[i]];

   *longnutation = (lng + t * lngx) / 10000.0;
   *obliqnutation = (obl + t * oblx) / 10000.0;

   return 0;
}

/********fund_args */

void fund_args (double t,

                double a[5])
/*
------------------------------------------------------------------------

   PURPOSE:
      To compute the fundamental arguments.

   REFERENCES:
      Seidelmann, P.K. (1982) Celestial Mechanics 27, 79-106 (1980 IAU 
         Theory of Nutation).

   INPUT
   ARGUMENTS:
      t (double)
         TDB time in Julian centuries since J2000.0

   OUTPUT
   ARGUMENTS:
      a[5] (double)
         Fundamental arguments, in radians:
          a[0] = l (mean anomaly of the Moon)
          a[1] = l' (mean anomaly of the Sun)
          a[2] = F (L - omega; L = mean longitude of the Moon)
          a[3] = D (mean elongation of the Moon from the Sun)
          a[4] = omega (mean longitude of the Moon's ascending node)

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      TWOPI

   FUNCTIONS
   CALLED:
      fmod     math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/10-97/JAB (USNO/AA)
      V1.1/07-98/JAB (USNO/AA): Place arguments in the range 0-TWOPI
                                radians.

   NOTES:
      1. The fundamental arguments are used in computing the nutation
      angles and in the expression for sidereal time.

------------------------------------------------------------------------
*/
{
   short int i;

   a[0] = 2.3555483935439407 + t * (8328.691422883896
                             + t * (1.517951635553957e-4
                             + 3.1028075591010306e-7 * t));
   a[1] = 6.240035939326023 + t * (628.3019560241842
                            + t * (-2.7973749400020225e-6
                            - 5.817764173314431e-8 * t));
   a[2] = 1.6279019339719611 + t * (8433.466158318453 
                             + t * (-6.427174970469119e-5
                             + 5.332950492204896e-8 * t));
   a[3] = 5.198469513579922 + t * (7771.377146170642
                            + t * (-3.340851076525812e-5
                            + 9.211459941081184e-8 * t));
   a[4] = 2.1824386243609943 + t * (-33.75704593375351
                             + t * (3.614285992671591e-5
                             + 3.878509448876288e-8 * t));

   for (i = 0; i < 5; i++)
   {
      a[i] = fmod (a[i],TWOPI);
      if (a[i] < 0.0)
         a[i] += TWOPI; 
   }

   return;
}

/********vector2radec */

short int vector2radec (double *pos, 

                        double *ra, double *dec)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Converts an vector in equatorial rectangular coordinates to
      equatorial spherical coordinates.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      pos[3] (double)
         Position vector, equatorial rectangular coordinates.

   OUTPUT
   ARGUMENTS:
      *rightascension (double)
         Right ascension in hours.
      *declination (double)
         Declination in degrees.

   RETURNED
   VALUE:
      (short int)
         0...Everything OK.
         1...All vector components are zero; 'ra' and 'dec' are
             indeterminate.
         2...Both vec[0] and vec[1] are zero, but vec[2] is nonzero;
             'ra' is indeterminate.
   GLOBALS
   USED:
      RAD2SEC

   FUNCTIONS
   CALLED:
      sqrt     math.h
      pow      math.h
      atan2    math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C Standards.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'angles'.

------------------------------------------------------------------------
*/
{
   double xyproj;

   xyproj = sqrt (pow (pos[0], 2.0) + pow (pos[1], 2.0));
   if ((xyproj == 0.0) && (pos[2] == 0))
   {
      *ra = 0.0;
      *dec = 0.0;
      return 1;
   }
    else if (xyproj == 0.0)
   {
      *ra = 0.0;
      if (pos[2] < 0.0)
         *dec = -90.0;
       else
         *dec = 90.0;
      return 2;
   }
    else
   {
      *ra = atan2 (pos[1], pos[0]) * RAD2SEC / 54000.0;
      *dec = atan2 (pos[2], xyproj) * RAD2SEC / 3600.0;

      if (*ra < 0.0)
         *ra += 24.0;
   }
   return 0;
}

/********radec2vector */

void radec2vector (double ra, double dec, double dist,

                   double *vector)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Converts equatorial spherical coordinates to a vector (equatorial
      rectangular coordinates).

   REFERENCES: 
      None.

   INPUT
   ARGUMENTS:
      ra (double)
         Right ascension (hours).
      dec (double)
         Declination (degrees).

   OUTPUT
   ARGUMENTS:
      vector[3] (double)
         Position vector, equatorial rectangular coordinates (AU).

   RETURNED
   VALUE:
      (short int)
         0...Everything OK.

   GLOBALS
   USED:
      DEG2RAD

   FUNCTIONS
   CALLED:
      cos     math.h
      sin     math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/05-92/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C Standards.

   NOTES:
      None.

------------------------------------------------------------------------
*/
{

   vector[0] = dist * cos (DEG2RAD * dec) * cos (DEG2RAD * 15.0 * ra);
   vector[1] = dist * cos (DEG2RAD * dec) * sin (DEG2RAD * 15.0 * ra);
   vector[2] = dist * sin (DEG2RAD * dec);

   return;
}

/********starvectors */

void starvectors (cat_entry *star,

                  double *pos, double *vel)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Converts angular quanities for stars to vectors.

   REFERENCES: 
      Kaplan, G. H. et. al. (1989). Astron. Journ. Vol. 97, 
         pp. 1197-1210.
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.

   INPUT
   ARGUMENTS:
      *star (struct cat_entry)
         Pointer to catalog entry structure containing J2000.0 catalog
         data with FK5-style units (defined in novas.h).

   OUTPUT
   ARGUMENTS:
      pos[3] (double)
         Position vector, equatorial rectangular coordinates,
         components in AU.
      vel[3] (double)
         Velocity vector, equatorial rectangular coordinates,
         components in AU/Day.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      RAD2SEC, DEG2RAD, KMAU

   FUNCTIONS
   CALLED:
      sin     math.h
      cos     math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/01-93/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Updated to C programming standards.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'vectrs'.

------------------------------------------------------------------------
*/
{
   double paralx, dist, r, d, cra, sra, cdc, sdc, pmr, pmd, rvl;

/*
   If parallax is unknown, undetermined, or zero, set it to 1e-7 second
   of arc, corresponding to a distance of 10 megaparsecs.
*/

   paralx = star->parallax;

   if (star->parallax <= 0.0)
      paralx = 1.0e-7;

/*
   Convert right ascension, declination, and parallax to position vector
   in equatorial system with units of AU.
*/

   dist = RAD2SEC / paralx;
   r = (star->ra) * 15.0 * DEG2RAD;
   d = (star->dec) * DEG2RAD;
   cra = cos (r);
   sra = sin (r);
   cdc = cos (d);
   sdc = sin (d);

   pos[0] = dist * cdc * cra;
   pos[1] = dist * cdc * sra;
   pos[2] = dist * sdc;

/*
   Convert proper motion and radial velocity to orthogonal components of
   motion with units of AU/Day.
*/

   pmr = star->promora * 15.0 * cdc / (paralx * 36525.0);
   pmd = star->promodec / (paralx * 36525.0);
   rvl = star->radialvelocity * 86400.0 / KMAU;

/*
   Transform motion vector to equatorial system.
*/

   vel[0] = - pmr * sra - pmd * sdc * cra + rvl * cdc * cra;
   vel[1] =   pmr * cra - pmd * sdc * sra + rvl * cdc * sra;
   vel[2] =   pmd * cdc + rvl * sdc;

   return;
}

/********tdb2tdt */

void tdb2tdt (double tdb,

              double *tdtjd, double *secdiff)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Computes the terrestrial time (TT) or terrestrial dynamical time 
      (TDT) Julian date corresponding to a barycentric dynamical time 
      (TDB) Julian date.

   REFERENCES: 
      Explanatory Supplement to the Astronomical Almanac, pp. 42-44 and 
         p. 316.

   INPUT
   ARGUMENTS:
      tdb (double)
         TDB Julian date.

   OUTPUT
   ARGUMENTS:
      *tdtjd (double)
         TT (or TDT) Julian date.
      *secdiff (double)
         Difference tdbjd-tdtjd, in seconds.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      RAD2SEC, T0

   FUNCTIONS
   CALLED:
      sin   math.h
      fmod  math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/07-92/TKB (USNO/NRL Optical Interfer.) Translate Fortran.
      V1.1/08-93/WTH (USNO/AA) Update to C Standards.
      V1.2/06-98/JAB (USNO/AA) New algorithm (see reference).

   NOTES:
      1. Expressions used in this version are approximations resulting
      in accuracies of about 20 microseconds.
      2. This function is the "C" version of Fortran NOVAS routine
      'times'.
------------------------------------------------------------------------
*/
{

/*
   'ecc' = eccentricity of earth-moon barycenter orbit.
*/

   double ecc = 0.01671022;
   double rev = 1296000.0;
   double tdays, m, l, lj, e;

   tdays = tdb - T0;
   m = ( 357.51716 + 0.985599987 * tdays ) * 3600.0;
   l = ( 280.46435 + 0.985609100 * tdays ) * 3600.0;
   lj = ( 34.40438 + 0.083086762 * tdays ) * 3600.0;
   m = fmod (m,rev) / RAD2SEC;
   l = fmod (l,rev) / RAD2SEC;
   lj = fmod (lj,rev) / RAD2SEC;
   e = m + ecc * sin (m) + 0.5 * ecc * ecc * sin (2.0 * m);
   *secdiff = 1.658e-3 * sin (e) + 20.73e-6 * sin (l - lj);
   *tdtjd = tdb - *secdiff / 86400.0;

    return;
}

/********set_body */

short int set_body (short int type, short int number, char *name,

                    body *cel_obj)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Sets up a structure of type 'body' - defining a celestial object-
      based on the input parameters.

   REFERENCES: 
      None.

   INPUT
   ARGUMENTS:
      type (short int)
         Type of body
            = 0 ... major planet, Sun, or Moon
            = 1 ... minor planet
      number (short int)
         Body number
            For 'type' = 0: Mercury = 1,...,Pluto = 9, Sun = 10, 
                            Moon = 11
            For 'type' = 1: minor planet number
      *name (char)
         Name of the body.

   OUTPUT
   ARGUMENTS:
      struct body *cel_obj
         Structure containg the body definition (defined in novas.h)

   RETURNED
   VALUE:
      (short int)
         = 0 ... everything OK
         = 1 ... invalid value of 'type'
         = 2 ... 'number' out of range

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      strcpy        string.h
      tolower       ctype.h
      toupper       ctype.h

   VER./DATE/
   PROGRAMMER:
      V1.0/06-97/JAB (USNO/AA)
      V1.1/10-98/JAB (USNO/AA): Change body name to mixed case.

   NOTES:
      None.

------------------------------------------------------------------------
*/
{
   char temp;

   short int error = 0;
   short int i;

/*
   Initialize the structure.
*/

   cel_obj->type = 0;
   cel_obj->number = 0;
   strcpy (cel_obj->name, "  ");

/*
   Set the body type.
*/

   if ((type < 0) || (type > 1))
      return (error = 1);
    else
      cel_obj->type = type;

/*
   Set the body number.
*/

   if (type == 0)
      if ((number <= 0) || (number > 11))
         return (error = 2);
    else
      if (number <= 0)
         return (error = 2);

   cel_obj->number = number;

/*
   Set the body name in mixed case.
*/

    i = 0;
    while (name[i] != 0)
    {
       if (i == 0)
          temp = toupper (name[i]);
        else
          temp = tolower (name[i]);
       cel_obj->name[i++] = temp;
    }
    cel_obj->name[i] = '\0';

    return (error);
}

/********ephemeris */

short int ephemeris (double tjd, body *cel_obj, short int origin,

                     double *pos, double *vel)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Retrieves the position and velocity of a body from a fundamental
      ephemeris.

   REFERENCES:
      None.

   INPUT
   ARGUMENTS:
      tjd (double)
         TDB Julian date.
      *cel_obj (struct body)
         Pointer to structure containing the designation of the body
         of interest (defined in novas.h).
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
         0    ... Everything OK.
         1    ... Invalid value of 'origin'.
         2    ... Invalid value of 'type' in 'cel_obj'.
         3    ... Unable to allocate memory.
         10+n ... where n is the error code from 'solarsystem'.
         20+n ... where n is the error code from 'readeph'.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      solarsystem         novas.c
      readeph             readeph.c
      strlen              string.h
      strcpy              string.h
      malloc              stdlib.h
      free                stdlib.h

   VER./DATE/
   PROGRAMMER:
      V1.0/06-97/JAB (USNO/AA)
      V1.1/10-97/JAB (USNO/AA): Support error code from 'readeph'.
      V1.2/08-98/JAB (USNO/AA): Add computation of barycentric
                                coordinates of a minor planet; support
                                new 'readeph' argument list.
      V1.3/12-99/JAB (USNO/AA): Add error handling to call to 
                                'solarsystem' (case 1).

   NOTES:
      1. It is recommended that the input structure 'cel_obj' be
      created using function 'set_body' in file novas.c.

------------------------------------------------------------------------
*/
{
   char *mp_name;

   int err = 0;
   int mp_number;

   short int error = 0;
   short int ss_number, i;

   double *posvel, *sun_pos, *sun_vel;

   size_t name_len;

/*
   Check the value of 'origin'.
*/

   if ((origin < 0) || (origin > 1))
      return (error = 1);

/*
   Invoke the appropriate ephemeris access software depending upon the
   type of object.
*/

   switch (cel_obj->type)
   {

/*
   Get the position and velocity of a major planet, Sun, or Moon,
*/

      case 0:
         ss_number = cel_obj->number;
         if ((error = solarsystem (tjd,ss_number,origin, pos,vel))
               != 0)
            error += 10;
         break;

/*
   Get the position and velocity of a minor planet. 
   
*/

      case 1:
         mp_number = (int) cel_obj->number;

         name_len = (strlen (cel_obj->name) + 1L) * sizeof (char);
         mp_name = (char *) malloc (name_len);
         if (mp_name == NULL )
            return (error = 3);
         strcpy (mp_name, cel_obj->name);

         posvel = (double *) malloc (6L * sizeof (double));
         if (posvel == NULL )
         {
            free (mp_name);
           	return (error = 3);
         }

/*  The USNO minor planet software returns heliocentric positions and 
    velocities.
*/

         posvel = readeph (mp_number,mp_name,tjd, &err);
         if (err != 0)
            return ((short int) (20 + err));

/*  Barycentric coordinates of the minor planet, if desired, are 
    computed via the barycentric coordinates of the Sun, obtained 
    from the solar system ephemeris.
*/

         if (origin == 0)
         {
            sun_pos = (double *) malloc (3L * sizeof (double));
            if (sun_pos == NULL )
            {
               free (mp_name);
               free (posvel);
               return (error = 3);
            }

            sun_vel = (double *) malloc (3L * sizeof (double));
            if (sun_vel == NULL )
            {
               free (mp_name);
               free (posvel);
               free (sun_pos);
               return (error = 3);
            }

            if ((error = solarsystem (tjd,10,0, sun_pos,sun_vel)) != 0)
            {
               free (mp_name);
               free (posvel);
               free (sun_pos);
               free (sun_vel);
               return (error + 10);
            }

            for (i = 0; i < 3; i++)
            {
               posvel[i] += sun_pos[i];
               posvel[i+3] += sun_vel[i];
            }

            free (sun_pos);
            free (sun_vel);
         }

/*
   Break up 'posvel' into separate position and velocity vectors.
*/

         for (i = 0; i < 3; i++)
         {
            pos[i] = posvel[i];
            vel[i] = posvel[i+3];
         }

         free (mp_name);
         free (posvel);
         break;

/*
   Invalid type of object.
*/

      default:
         error = 2;
         break;

   }

   return (error);
}

/********make_cat_entry */

void make_cat_entry (char catalog[4], char star_name[51],
                     long int star_num, double ra, double dec,
                     double pm_ra, double pm_dec, double parallax,
                     double rad_vel,

                     cat_entry *star)
/*
------------------------------------------------------------------------

   PURPOSE:
      To create a structure of type 'cat_entry' containing catalog
      data for a star or "star-like" object.

   REFERENCES:
      None.

   INPUT
   ARGUMENTS:
      catalog[4] (char)
         Three-character catalog identifier (e.g. HIP = Hipparcos, FK5 =
         FK5).  This identifier also specifies the reference system
         and units of the data; i.e. they are the same as the
         specified catalog.
      star_name[51] (char)
         Object name (50 characters maximum).
      star_num (long int)
         Object number in the catalog.
      ra (double)
         Right ascension of the object.
      dec (double)
         Declination of the object.
      pm_ra (double)
         Proper motion in right ascension.
      pm_dec (double)
         Proper motion in declination.
      parallax (double)
         Parallax.
      rad_vel (double)
         Radial velocity.

   OUTPUT
   ARGUMENTS:
      *star (struct cat_entry)
         Structure containing the input data (structure defined in
         novas.h).

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      None.

   VER./DATE/
   PROGRAMMER:
      V1.0/03-98/JAB (USNO/AA)

   NOTES:
      1. This utility function simply creates a single data structure 
      encapsulating the input data.  The units, etc. of the data can be
      arbitrary, but are defined by the catalog to which the data
      belongs.

------------------------------------------------------------------------
*/
{
   short int i;

   for (i = 0; i < 4; i++)
   {
      star->catalog[i] = catalog[i];
      if (catalog[i] == '\0')
         break;
   }
   star->catalog[i] = '\0';

   for (i = 0; i < 51; i++)
   {
      star->starname[i] = star_name[i];
      if (star_name[i] == '\0')
         break;
   }
   star->starname[i] = '\0';

   star->starnumber = star_num;
   star->ra = ra;
   star->dec = dec;
   star->promora = pm_ra;
   star->promodec = pm_dec;
   star->parallax = parallax;
   star->radialvelocity = rad_vel;

   return;
}

/********transform_hip */

void transform_hip (cat_entry *hipparcos,

                    cat_entry *fk5)
/*
------------------------------------------------------------------------

   PURPOSE:
      To convert Hipparcos data at epoch J1991.25 to epoch J2000.0 and
      FK5-style units.  To be used only for Hipparcos or Tycho stars 
      with linear space motion.

   REFERENCES:
      None.

   INPUT
   ARGUMENTS:
      *hipparcos (struct cat_entry)
         An entry from the Hipparcos catalog, at epoch J1991.25, with 
         all members having Hipparcos catalog units.  See Note 1
         below (struct defined in novas.h).

   OUTPUT
   ARGUMENTS:
      *fk5 (struct cat_entry)
         The transformed input entry, at epoch J2000.0, with all 
         members having FK5 catalog units.  See Note 2 below (struct 
         defined in novas.h).


   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      DEG2RAD

   FUNCTIONS
   CALLED:
      transform_cat  novas.c
      cos            math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/03-98/JAB (USNO/AA)

   NOTES:
      1. Hipparcos epoch and units:
         Epoch: J1991.25
         Right ascension (RA): degrees
         Declination (Dec): degrees
         Proper motion in RA * cos (Dec): milliarcseconds per year
         Proper motion in Dec: milliarcseconds per year
         Parallax: milliarcseconds
         Radial velocity: kilometers per second (not in catalog)
      2. FK5 epoch and units:
         Epoch: J2000.0
         Right ascension: hours
         Declination: degrees
         Proper motion in RA: seconds of time per Julian century
         Proper motion in Dec: arcseconds per Julian century
         Parallax: arcseconds
         Radial velocity: kilometers per second
      3. This function based on subroutine 'gethip' from NOVAS Fortran.

------------------------------------------------------------------------
*/
{
   double epoch_hip = 2448349.0625;
   double epoch_fk5 = 2451545.0000;

   cat_entry scratch;

/*
   The "scratch" catalog entry contains data with FK5-like units at 
   epoch J1991.25.  Copy the catalog entry quantities that don't 
   change from the Hipparcos catalog entry to the "scratch" entry.
*/

   strcpy (scratch.starname, hipparcos->starname);
   scratch.starnumber = hipparcos->starnumber;
   scratch.dec = hipparcos->dec;
   scratch.radialvelocity = hipparcos->radialvelocity;

   strcpy (scratch.catalog, "SCR");

/*
   Convert Hipparcos units to FK5-like units; insert transformed
   quantities into the "scratch" catalog entry.
*/

   scratch.ra = hipparcos->ra / 15.0;
   scratch.promora = hipparcos->promora / (150.0 * 
      cos (hipparcos->dec * DEG2RAD));
   scratch.promodec = hipparcos->promodec / 10.0;
   scratch.parallax = hipparcos->parallax / 1000.0;

/*
   Change the epoch of the Hipparcos data from J1991.25 to J2000.0.
*/

   transform_cat (1,epoch_hip,&scratch,epoch_fk5,"FK5", fk5);

   return;
}

/********transform_cat */

void transform_cat (short int option, double date_incat, 
                    cat_entry *incat, double date_newcat,
                    char newcat_id[4],

                    cat_entry *newcat)
/*
------------------------------------------------------------------------

   PURPOSE:
      To transform a star's catalog quantities for a change of epoch 
      and/or equator and equinox.

   REFERENCES:
      None.

   INPUT
   ARGUMENTS:
      option (short int)
         Transformation option
            = 1 ... change epoch; same equator and equinox
            = 2 ... change equator and equinox; same epoch
            = 3 ... change equator and equinox and epoch
      date_incat (double)
         TT Julian date, or year, of input catalog data.
      *incat (struct cat_entry)
         An entry from the input catalog (struct defined in novas.h).
      date_newcat (double)
         TT Julian date, or year, of transformed catalog data.
      newcat_id[4] (char)
         Three-character abbreviated name of the transformed catalog.

   OUTPUT
   ARGUMENTS:
      newcat (struct cat_entry)
         The transformed catalog entry (struct defined in novas.h).


   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      T0, RAD2SEC, KMAU

   FUNCTIONS
   CALLED:
      precession   novas.c
      sin          math.h
      cos          math.h
      sqrt         math.h
      atan2        math.h
      strcpy       string.h

   VER./DATE/
   PROGRAMMER:
      V1.0/03-98/JAB (USNO/AA)

   NOTES:
      1. 'date_incat' and 'date_newcat' may be specified either as a 
      Julian date (e.g., 2433282.5) or a Julian year and fraction 
      (e.g., 1950.0).  Values less than 10000 are assumed to be years.
      2. 'option' = 1 updates the star's data to account for the 
      star's space motion between the first and second dates, within a 
      fixed reference frame.
         'option' = 2 applies a rotation of the reference frame 
      corresponding to precession between the first and second dates, 
      but leaves the star fixed in space.
         'option' = 3 provides both transformations.
      3. This subroutine cannot be properly used to bring data from 
      old (pre-FK5) star catalogs into the modern system, because old 
      catalogs were compiled using a set of constants that are 
      incompatible with the IAU (1976) system.
      4. This function uses TDB Julian dates internally, but no 
      distinction between TDB and TT is necessary.
      5. This function is based on subroutine 'gethip' from NOVAS 
      Fortran.

------------------------------------------------------------------------
*/
{
   short int j;

   double jd_incat, jd_newcat, paralx, dist, r, d, cra, sra, cdc, sdc, 
      pos1[3], term1, pmr, pmd, rvl, vel1[3], pos2[3], vel2[3], xyproj;

/*
   If necessary, compute Julian dates.
*/

   if (date_incat < 10000.0)
      jd_incat = T0 + (date_incat - 2000.0) * 365.25;
    else
      jd_incat = date_incat;

   if (date_newcat < 10000.0)
      jd_newcat = T0 + (date_newcat - 2000.0) * 365.25;
    else
      jd_newcat = date_newcat;

/*
   Convert input angular components to vectors

   If parallax is unknown, undetermined, or zero, set it to 1.0e-7 
   second of arc, corresponding to a distance of 10 megaparsecs.
*/

   paralx = incat->parallax;
   if (paralx <= 0.0)
      paralx = 1.0e-7;

/*
   Convert right ascension, declination, and parallax to position 
   vector in equatorial system with units of AU.
*/

   dist = RAD2SEC / paralx;
   r = incat->ra * 54000.0 / RAD2SEC;
   d = incat->dec * 3600.0 / RAD2SEC;
   cra = cos (r);
   sra = sin (r);
   cdc = cos (d);
   sdc = sin (d);
   pos1[0] = dist * cdc * cra;
   pos1[1] = dist * cdc * sra;
   pos1[2] = dist * sdc;

/*
   Convert proper motion and radial velocity to orthogonal components 
   of motion, in spherical polar system at star's original position, 
   with units of AU/day.
*/

   term1 = paralx * 36525.0;
   pmr = incat->promora * 15.0 * cdc / term1;
   pmd = incat->promodec             / term1;
   rvl = incat->radialvelocity * 86400.0 / KMAU;

/*
   Transform motion vector to equatorial system.
*/

   vel1[0] = - pmr * sra - pmd * sdc * cra + rvl * cdc * cra;
   vel1[1] =   pmr * cra - pmd * sdc * sra + rvl * cdc * sra;
   vel1[2] =               pmd * cdc       + rvl * sdc;

/*
   Update star's position vector for space motion (only if 'option' = 1 
   or 'option' = 3).
*/

   if ((option == 1) || (option == 3))
   {
      for (j = 0; j < 3; j++)
      {
         pos2[j] = pos1[j] + vel1[j] * (jd_newcat - jd_incat);
         vel2[j] = vel1[j];
      }
   }
    else
   {
      for (j = 0; j < 3; j++)
      {
           pos2[j] = pos1[j];
           vel2[j] = vel1[j];
      }
   }

/*
   Precess position and velocity vectors (only if 'option' = 2 or 
   'option' = 3).
*/

   if ((option == 2) || (option == 3))
   {
      for (j = 0; j < 3; j++)
      {
           pos1[j] = pos2[j];
           vel1[j] = vel2[j];
      }
      precession (jd_incat,pos1,jd_newcat, pos2);
      precession (jd_incat,vel1,jd_newcat, vel2);
   }

/*
   Convert vectors back to angular components for output.

   From updated position vector, obtain star's new position expressed 
   as angular quantities.
*/

   xyproj = sqrt (pos2[0] * pos2[0] + pos2[1] * pos2[1]);
   r = atan2 (pos2[1], pos2[0]);
   d = atan2 (pos2[2], xyproj);
   newcat->ra = r * RAD2SEC / 54000.0;
   newcat->dec = d * RAD2SEC / 3600.0;
   if (newcat->ra < 0.0 )
      newcat->ra += 24.0;

   dist = sqrt (pos2[0] * pos2[0] + pos2[1] * pos2[1] + 
      pos2[2] * pos2[2]);
   paralx = RAD2SEC / dist;
   newcat->parallax = paralx;

/*
   Transform motion vector back to spherical polar system at star's 
   new position.
*/

   cra = cos (r);
   sra = sin (r);
   cdc = cos (d);
   sdc = sin (d);
   pmr = - vel2[0] * sra       + vel2[1] * cra;
   pmd = - vel2[0] * cra * sdc - vel2[1] * sra * sdc + vel2[2] * cdc;
   rvl =   vel2[0] * cra * cdc + vel2[1] * sra * cdc + vel2[2] * sdc;

/*
   Convert components of motion to from AU/day to normal catalog units.
*/

   newcat->promora  = pmr * paralx * 36525.0 / (15.0 * cdc);
   newcat->promodec = pmd * paralx * 36525.0;
   newcat->radialvelocity = rvl * KMAU / 86400.0;

/*
  Take care of zero-parallax case.
*/

   if (newcat->parallax <= 1.01e-7)
   {
      newcat->parallax = 0.0;
      newcat->radialvelocity = incat->radialvelocity;
   }

/*
   Set the catalog identification code for the transformed catalog
   entry.
*/

   strcpy (newcat->catalog,newcat_id);

/*
   Copy unchanged quantities from the input catalog entry to the
   transformed catalog entry.
*/

   strcpy (newcat->starname, incat->starname);
   newcat->starnumber = incat->starnumber;

   return;
}

/******** equ2hor */

void equ2hor (double tjd, double deltat, double x, double y, 
              site_info *location, double ra, double dec, 
              short int ref_option,

              double *zd, double *az, double *rar, double *decr)
/*
------------------------------------------------------------------------

   PURPOSE:
      This function transforms apparent equatorial coordinates (right 
      ascension and declination) to horizon coordinates (zenith 
      distance and azimuth).  It uses a method that properly accounts 
      for polar motion, which is significant at the sub-arcsecond 
      level.  This function can also adjust coordinates for atmospheric 
      refraction.

   REFERENCES:
      None.

   INPUT
   ARGUMENTS:
      tjd (double)
         TT (or TDT) Julian date.
      deltat (double)
         Difference TT (or TDT)-UT1 at 'tjd', in seconds.
      x (double)
         Conventionally-defined x coordinate of celestial ephemeris 
         pole with respect to IERS reference pole, in arcseconds. 
      y (double)
         Conventionally-defined y coordinate of celestial ephemeris 
         pole with respect to IERS reference pole, in arcseconds.
      *location (struct site_info)
         Pointer to structure containing observer's location (defined
         in novas.h).
      ra (double)
         Topocentric right ascension of object of interest, in hours, 
         referred to true equator and equinox of date.
      dec (double)
         Topocentric declination of object of interest, in degrees, 
         referred to true equator and equinox of date.
      ref_option (short int)
         = 0 ... no refraction
         = 1 ... include refraction, using 'standard' atmospheric
                 conditions.
         = 2 ... include refraction, using atmospheric parameters 
                 input in the 'location' structure.
 
   OUTPUT
   ARGUMENTS:
      *zd (double)
         Topocentric zenith distance in degrees, affected by 
         refraction if 'ref_option' is non-zero.
      *az (double)
         Topocentric azimuth (measured east from north) in degrees.
      *rar (double)
         Topocentric right ascension of object of interest, in hours, 
         referred to true equator and equinox of date, affected by 
         refraction if 'ref_option' is non-zero.
      *decr (double)
         Topocentric declination of object of interest, in degrees, 
         referred to true equator and equinox of date, affected by 
         refraction if 'ref_option' is non-zero.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      DEG2RAD, RAD2DEG

   FUNCTIONS
   CALLED:
      tdb2tdt           novas.c
      sidereal_time     novas.c
      psnw              novas.c
      refract           novas.c
      sin               math.h
      cos               math.h
      sqrt              math.h
      atan2             math.h
      fabs              math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/06-98/JAB (USNO/AA)

   NOTES:
      1. 'x' and 'y' can be set to zero if sub-arcsecond accuracy is  
      not needed.  'ra' and 'dec' can be obtained from functions 
      'tpstar' or 'tpplan'.
      2. The directons 'zd'= 0 (zenith) and 'az'= 0 (North) are here 
      considered fixed in the terrestrial frame.  Specifically, the 
      zenith is along the geodetic normal, and North is toward
      the IERS reference pole.
      3. If 'ref_option'= 0, then 'rar'='ra' and 'decr'='dec'.
      4. This function is the "C" version of Fortran NOVAS routine
      'zdaz' written by G. H. Kaplan (USNO).

------------------------------------------------------------------------
*/
{
   short int j;

   double ujd, dummy, secdiff, tdb, mobl, tobl, ee, dpsi, deps, gast,
      sinlat, coslat, sinlon, coslon, sindc, cosdc, sinra, cosra, 
      uze[3], une[3], uwe[3], uz[3], un[3], uw[3], p[3], pz, pn, pw, 
      proj, zd0, zd1, refr, cosr, prlen, rlen, pr[3];

/*
   Compute 'ujd', the UT1 Julian date corresponding to 'tjd'.
*/

   ujd = tjd - (deltat / 86400.0);

/*
   Compute 'tdb', the TDB Julian date corresponding to 'tjd'.
*/

   tdb2tdt (tjd, &dummy,&secdiff);
   tdb = tjd + secdiff / 86400.0;

/*
   Compute the equation of the equinoxes.
*/

   earthtilt (tdb, &mobl,&tobl,&ee,&dpsi,&deps);

/*
   Compute the Greenwich apparent sidereal time at 'ujd'.
*/

   sidereal_time (ujd,0.0,ee, &gast);

/*
   Preliminaries.
*/

   *rar = ra;
   *decr = dec;

   sinlat = sin (location->latitude * DEG2RAD);
   coslat = cos (location->latitude * DEG2RAD);
   sinlon = sin (location->longitude * DEG2RAD);
   coslon = cos (location->longitude * DEG2RAD);
   sindc = sin (dec * DEG2RAD);
   cosdc = cos (dec * DEG2RAD);
   sinra = sin (ra * 15.0 * DEG2RAD);
   cosra = cos (ra * 15.0 * DEG2RAD);

/*
   Set up orthonormal basis vectors in local Earth-fixed system.

   Define vector toward local zenith in Earth-fixed system (z axis).
*/
   uze[0] = coslat * coslon;
   uze[1] = coslat * sinlon;
   uze[2] = sinlat;

/*
   Define vector toward local north in Earth-fixed system (x axis).
*/
   
   une[0] = -sinlat * coslon;
   une[1] = -sinlat * sinlon;
   une[2] = coslat;

/*
   Define vector toward local west in Earth-fixed system (y axis).
*/

   uwe[0] = sinlon;
   uwe[1] = -coslon;
   uwe[2] = 0.0;

/*
   Obtain vectors in celestial system.

   Rotate Earth-fixed orthonormal basis vectors to celestial system
   (wrt equator and equinox of date).
*/

   pnsw (0.0,gast,x,y,uze, uz);
   pnsw (0.0,gast,x,y,une, un);
   pnsw (0.0,gast,x,y,uwe, uw);

/*
   Define unit vector 'p' toward object in celestial system
   (wrt equator and equinox of date).
*/

   p[0] = cosdc * cosra;
   p[1] = cosdc * sinra;
   p[2] = sindc;

/*
   Compute coordinates of object wrt orthonormal basis.

   Compute components of 'p' - projections of 'p' onto rotated
   Earth-fixed basis vectors.
*/

   pz = p[0] * uz[0] + p[1] * uz[1] + p[2] * uz[2];
   pn = p[0] * un[0] + p[1] * un[1] + p[2] * un[2];
   pw = p[0] * uw[0] + p[1] * uw[1] + p[2] * uw[2];

/*
   Compute azimuth and zenith distance.
*/

   proj = sqrt (pn * pn + pw * pw);

   if (proj > 0.0)
      *az = -atan2 (pw, pn) * RAD2DEG;

   if (*az < 0.0)
      *az += 360.0;

   if (*az >= 360.0)
      *az -= 360.0;

   *zd = atan2 (proj, pz) * RAD2DEG;

/*
   Apply atmospheric refraction if requested.
*/

   if (ref_option != 0)
   {

/*
   Get refraction in zenith distance.

   Iterative process is required because refraction algorithms are
   always a function of observed (not computed) zenith distance.
   Require convergence to 0.2 arcsec (actual accuracy less).
*/

      zd0 = *zd;

      do
      {
         zd1 = *zd;
         refr = refract (location,ref_option,*zd);
         *zd = zd0 - refr;
      } while (fabs (*zd - zd1) > 5.0e-5);

/*
   Apply refraction to celestial coordinates of object.
*/
      if ((refr > 0.0) && (*zd > 0.01))
      {

/*
   Shift position vector of object in celestial system to account 
   for for refraction (see USNO/AA Technical Note 9).
*/

         cosr  = cos (refr * DEG2RAD);
         prlen = sin (zd0 * DEG2RAD) / sin (*zd * DEG2RAD);
         rlen  = sqrt (1.0 + prlen * prlen - 2.0 * prlen * cosr);

/*
   Add small refraction displacement vector to 'p'.
*/

         for (j = 0; j < 3; j++)
            pr[j] = (p[j] + rlen * uz[j]) / prlen;

/*
   Compute refracted right ascension and declination.
*/

         proj = sqrt (pr[0] * pr[0] + pr[1] * pr[1]);

         if (proj > 0.0)
           *rar = atan2 (pr[1],pr[0]) * RAD2DEG / 15.0;

         if (*rar < 0.0)
           *rar += 24.0;

         if (*rar >= 24.0)
           *rar -= 24.0;

         *decr = atan2 (pr[2],proj) * RAD2DEG;
      }
   }

   return;
}

/********refract */

double refract (site_info *location, short int ref_option, 
                double zd_obs)
/*
------------------------------------------------------------------------

   PURPOSE:
      This function computes atmospheric refraction in zenith
      distance.  This version computes approximate refraction for
      optical wavelengths.

   REFERENCES:
      Explanatory Supplement to the Astronomical Almanac, p. 144.
      Bennett (1982), Journal of Navigation (Royal Institute) 35, 
         pp. 255-259.

   INPUT
   ARGUMENTS:
      *location (struct site_info)
         Pointer to structure containing observer's location.  This
         structure also contains weather data (optional) for the 
         observer's location (defined in novas.h).
      ref_option (short int)
         = 1 ... Use 'standard' atmospheric conditions.
         = 2 ... Use atmospheric parameters input in the 'location' 
                 structure.
      zd_obs (double)
         Observed zenith distance, in degrees.

   OUTPUT
   ARGUMENTS:
      None.

   RETURNED
   VALUE:
      (double)
         Atmospheric refraction, in degrees.

   GLOBALS
   USED:
      DEG2RAD

   FUNCTIONS
   CALLED:
      exp      math.h
      tan      math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/06-98/JAB (USNO/AA)

   NOTES:
      1. This function can be used for planning observations or 
      telescope pointing, but should not be used for the reduction
      of precise observations.
      2. This function is the "C" version of Fortran NOVAS routine
      'refrac' written by G. H. Kaplan (USNO).

------------------------------------------------------------------------
*/
{

/*
   's' is the approximate scale height of atmosphere in meters.
*/

   double s = 9.1e3;
   double refr, p, t, h, r;

/*
   Compute refraction only for zenith distances between 0.1 and 
   91 degrees.
*/

   if ((zd_obs < 0.1) || (zd_obs > 91.0))
      refr = 0.0;

    else
   {

/*
   If observed weather data are available, use them.  Otherwise, use 
   crude estimates of average conditions.
*/

      if (ref_option == 2)
      {
         p = location->pressure;
         t = location->temperature;
      }
       else
      {
         p = 1010.0 * exp (-location->height / s);
         t = 10.0;
      }

      h = 90.0 - zd_obs;
      r = 0.016667 / tan ((h + 7.31 / (h + 4.4)) * DEG2RAD);
      refr = r * (0.28 * p / (t + 273.0));
   }

   return (refr);     
}

/********julian_date */

double julian_date (short int year, short int month, short int day,
                    double hour)
/*
------------------------------------------------------------------------

   PURPOSE:
      This function will compute the Julian date for a given calendar
      date (year, month, day, hour).

   REFERENCES: 
      Fliegel & Van Flandern, Comm. of the ACM, Vol. 11, No. 10, October
      1968, p. 657.

   INPUT
   ARGUMENTS:
      year (short int)
         Year.
      month (short int)
         Month number.
      day (short int)
         Day-of-month.
      hour (double)
         Hour-of-day.

   OUTPUT
   ARGUMENTS:
      None.

   RETURNED
   VALUE:
      (double)
         Julian date.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      None.

   VER./DATE/
   PROGRAMMER:
      V1.0/06-98/JAB (USNO/AA)

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'juldat'.
      2. This function makes no checks for a valid input calendar
      date.
------------------------------------------------------------------------
*/
{
   long int jd12h;

   double tjd;

   jd12h = (long) day - 32075L + 1461L * ((long) year + 4800L
      + ((long) month - 14L) / 12L) / 4L
      + 367L * ((long) month - 2L - ((long) month - 14L) / 12L * 12L)
      / 12L - 3L * (((long) year + 4900L + ((long) month - 14L) / 12L)
      / 100L) / 4L;
   tjd = (double) jd12h - 0.5 + hour / 24.0;

   return (tjd);
}

/********cal_date */

void cal_date (double tjd,

               short int *year, short int *month, short int *day,
               double *hour)
/*
------------------------------------------------------------------------

   PURPOSE:    
      This function will compute a date on the Gregorian calendar given
      the Julian date.

   REFERENCES: 
      Fliegel & Van Flandern, Comm. of the ACM, Vol. 11, No. 10,
         October 1968, p. 657.

   INPUT
   ARGUMENTS:
      tjd (double)
         Julian date.

   OUTPUT
   ARGUMENTS:
      *year (short int)
         Year.
      *month (short int)
         Month number.
      *day (short int)
         Day-of-month.
      *hour (double)
         Hour-of-day.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      None.

   FUNCTIONS
   CALLED:
      fmod     math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/06-98/JAB (USNO/AA)

   NOTES:
      1. This routine valid for any 'jd' greater than zero.
      2. Input julian date can be based on any UT-like time scale
      (UTC, UT1, TT, etc.) - output time value will have same basis.
      3. This function is the "C" version of Fortran NOVAS routine
      'caldat'.


------------------------------------------------------------------------
*/
{
   long int jd, k, m, n;

   double djd;

   djd = tjd + 0.5;
   jd = (long int) djd;

   *hour = fmod (djd,1.0) * 24.0;

   k     = jd + 68569L;
   n     = 4L * k / 146097L;

   k     = k - (146097L * n + 3L) / 4L;
   m     = 4000L * (k + 1L) / 1461001L;
   k     = k - 1461L * m / 4L + 31L;

   *month = (short int) (80L * k / 2447L);
   *day   = (short int) (k - 2447L * (long int) *month / 80L);
   k      = (long int) *month / 11L;

   *month = (short int) ((long int) *month + 2L - 12L * k);
   *year  = (short int) (100L * (n - 49L) + m + k);

   return;
}
