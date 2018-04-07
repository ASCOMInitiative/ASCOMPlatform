#include <stdio.h>
#include <stdlib.h>
#include "novas.h"

#define N_TIMES 4

int main (void)
{
/*
   Main function to check out many parts of NOVAS-C by computing the 
   topocentric place of a minor planet.  The USNO/AE98 minor planet
   ephemerides are used along with version 3 of function
   'solarsystem'.

   For use with NOVAS-C 2.0
*/

   short int error = 0;
   short int i;

/*
   'deltat' is the difference in time scales, TT - UT1.

    The array 'tjd' contains four selected Julian dates at which the
    minor planet positions will be evaluated.
*/

   double deltat = 60.0;
   double tjd[N_TIMES] = {2450203.5, 2450203.5, 2450417.5, 2450300.5};
   double ra, dec, dis;

/*
   The observer's terrestrial coordinates (latitude, longitude, height).
*/

   site_info geo_loc = {45.0, -75.0, 0.0, 10.0, 1010.0};

/*
   Structures containing the body designations for Earth and Pallas.
*/

   body earth, pallas;

/*
   Set up the structure containing the body designation for Earth.
*/

   if (error = set_body (0,3,"Earth", &earth))
   {
      printf ("Error %d from set_body.\n", error);
      exit (1);
   }

/*
   Set up the structure containing the body designation for Pallas.
*/

   if (error = set_body (1,2,"Pallas", &pallas))
   {
      printf ("Error %d from set_body.\n", error);
      exit (1);
   }

/*
   Compute the topocentric place of Pallas at the four selected Julian
   dates.
*/

   for (i = 0; i < N_TIMES; i++)
   {
      if (error = topo_planet (tjd[i],&pallas,&earth,deltat,&geo_loc, 
            &ra,&dec,&dis))
      {
         printf ("Error %d from topo_planet.\n", error);
         exit (1);
      }
       else
      {
         printf ("JD = %f  Body: %s\n", tjd[i], pallas.name);
         printf ("RA = %12.9f  Dec = %12.8f  Dis = %14.10f\n",
            ra, dec, dis);
         printf ("\n");
      }
   }

   return (0);
}
