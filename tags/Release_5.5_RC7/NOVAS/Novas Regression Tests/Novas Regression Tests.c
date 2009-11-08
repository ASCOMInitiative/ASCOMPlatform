// Novas Regression Tests.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include "novas.h"

#define N_STARS 3
#define N_TIMES 4

void print_caldate(short year, short month, short day, double hour)
{
	printf("Date %4d-%02d-%02d h=%f ", year,month, day, hour);
}
int _tmain(int argc, _TCHAR* argv[])
{

	char buffer[100];
	//double expectedJD = testJD;
	//double actualJD = julian_date(testYear, testMonth, testDay, testHour);
	//printf("julian_day(%d,%d,%d,%f) Expected=%f Actual=%f",
	//	testYear, testMonth, testDay, testHour, expectedJD, actualJD);
	/*
   Main function to check out many parts of NOVAS-C by calling
   function 'topo_star' with version 3 of function 'solarsystem'.

   For use with NOVAS-C 2.0
*/

   short int error = 0;
   short int i, j;

/*
   'deltat' is the difference in time scales, TT - UT1.

    The array 'tjd' contains four selected Julian dates at which the
    star positions will be evaluated.
*/

   double deltat = 60.0;
   double tjd[N_TIMES] = {2450203.5, 2450203.5, 2450417.5, 2450300.5};
   double ra, dec;

   			short yy; short mm; short dd; double hh;


/*
   FK5 catalog data for three selected stars.
*/

   cat_entry stars[N_STARS] = {
      {"FK5", "POLARIS",   0, 2.5301955556, 89.2640888889, 
               19.8770, -1.520,  0.0070, -17.0},
      {"FK5", "Delta ORI", 1, 5.5334438889, -0.2991333333,  
                0.0100, -0.220,  0.0140,  16.0},
      {"FK5", "Theta CAR", 2,10.7159355556,-64.3944666667, 
               -0.3480,  1.000,  0.0000,  24.0}};

/*
   The observer's terrestrial coordinates (latitude, longitude, height).
*/

   site_info geo_loc = {45.0, -75.0, 0.0, 10.0, 1010.0};

/*
   A structure containing the body designation for Earth.
*/

   body earth;

/*
   Set up the structure containing the body designation for Earth.
*/

   if (error = set_body (0,3,"Earth", &earth))
   {
      printf ("Error %d from set_body.\n", error);
      exit (1);
   }

/*
   Compute the topocentric places of the three stars at the four
   selected Julian dates.
*/

   for (i = 0; i < N_TIMES; i++)
   {
      for (j = 0; j < N_STARS; j++)
      {
         if (error = topo_star (tjd[i],&earth,deltat,&stars[j],&geo_loc,
            &ra,&dec))
            printf ("Error %d from topo_star. Star %d  Time %d\n",
               error, j, i);
          else
         {
            printf ("JD = %f  Star = %s\n", tjd[i], stars[j].starname);
			cal_date(tjd[i], &yy, &mm, &dd, &hh); 
			print_caldate(yy, mm, dd, hh);
            printf ("RA = %12.9f  Dec = %12.8f\n", ra, dec);
            printf ("\n");
         }
      }
      printf ("\n");
   }

	gets(buffer);
   return (0);
}



