/*
   NOVAS-C Version 2.0 (1 Nov 98)
   Solar System function; version 3.

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
   Additional function prototype.
*/

void sun_eph (double jd,

              double *ra, double *dec, double *dis);



/********solarsystem */

short int solarsystem (double tjd, short int body, short int origin, 

                       double *pos, double *vel)
/*
------------------------------------------------------------------------

   PURPOSE:    
      Provides the position and velocity of the Earth at epoch 'tjd'
      by evaluating a closed-form theory without reference to an 
      external file.  This function can also provide the position
      and velocity of the Sun.

   REFERENCES: 
      Kaplan, G. H. "NOVAS: Naval Observatory Vector Astrometry
         Subroutines"; USNO internal document dated 20 Oct 1988;
         revised 15 Mar 1990.
      Explanatory Supplement to The Astronomical Almanac (1992).

   INPUT
   ARGUMENTS:
      tjd (double)
         TDB Julian date.
      body (short int)
         Body identification number.
         Set 'body' = 0 or 'body' = 1 or 'body' = 10 for the Sun.
         Set 'body' = 2 or 'body' = 3 for the Earth.
      origin (short int)
         Origin code; solar system barycenter   = 0,
                      center of mass of the Sun = 1.

   OUTPUT
   ARGUMENTS:
      pos[3] (double)
         Position vector of 'body' at 'tjd'; equatorial rectangular
         coordinates in AU referred to the mean equator and equinox
         of J2000.0.
      vel[3] (double)
         Velocity vector of 'body' at 'tjd'; equatorial rectangular
         system referred to the mean equator and equinox of J2000.0,
         in AU/Day.

   RETURNED
   VALUE:
      (short int)
         0...Everything OK.
         1...Input Julian date ('tjd') out of range.
         2...Invalid value of 'body'.

   GLOBALS
   USED:
      T0, TWOPI.

   FUNCTIONS
   CALLED:
      sun_eph          solsys3.c
      radec2vector     novas.c
      precession       novas.c
      sin              math.h
      cos              math.h
      fabs             math.h
      fmod             math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/05-96/JAB (USNO/AA) Convert to C; substitute new theory of
                               Sun.
      V1.1/06-98/JAB (USNO/AA) Updated planetary masses & mean elements.

   NOTES:
      1. This function is the "C" version of Fortran NOVAS routine
      'solsys' version 3.

------------------------------------------------------------------------
*/
{
   short int ierr = 0;
   short int i;

/*
   The arrays below contain data for the four largest planets.  Masses
   are DE405 values; elements are from Explanatory Supplement, p. 316). 
   These data are used for barycenter computations only.
*/

   const double pm[4] = {1047.349, 3497.898, 22903.0, 19412.2};
   const double pa[4] = {5.203363, 9.537070, 19.191264, 30.068963};
   const double pl[4] = {0.600470, 0.871693, 5.466933, 5.321160};
   const double pn[4] = {1.450138e-3, 5.841727e-4, 2.047497e-4, 
                         1.043891e-4};

/*
   'obl' is the obliquity of ecliptic at epoch J2000.0 in degrees.
*/

   const double obl = 23.43929111;

   static double tlast = 0.0;
   static double sine, cose, tmass, pbary[3], vbary[3];

   double oblr, qjd, ras, decs, diss, pos1[3], p[3][3], dlon, sinl,
      cosl, x, y, z, xdot, ydot, zdot, f;

/*
   Initialize constants.
*/

   if (tlast == 0.0)
   {
      oblr = obl * TWOPI / 360.0;
      sine = sin (oblr);
      cose = cos (oblr);
      tmass = 1.0;
      for (i = 0; i < 4; i++)
         tmass += 1.0 / pm[i];
      tlast = 1.0;
   }

/*
   Check if input Julian date is within range.
*/

   if ((tjd < 2340000.5) || (tjd > 2560000.5))
      return (ierr = 1);

/*
   Form helicentric coordinates of the Sun or Earth, depending on
   'body'.
*/

   if ((body == 0) || (body == 1) || (body == 10))
      for (i = 0; i < 3; i++)
         pos[i] = vel[i] = 0.0;

    else if ((body == 2) || (body == 3))
    {
      for (i = 0; i < 3; i++)
      {
         qjd = tjd + (double) (i - 1) * 0.1;
         sun_eph (qjd, &ras,&decs,&diss);
         radec2vector (ras,decs,diss, pos1);
         precession (qjd,pos1,T0, pos);
         p[i][0] = -pos[0];
         p[i][1] = -pos[1];
         p[i][2] = -pos[2];
      }
      for (i = 0; i < 3; i++)
      {
         pos[i] = p[1][i];
         vel[i] = (p[2][i] - p[0][i]) / 0.2;
      }
    }

    else
      return (ierr = 2);
           
/*
   If 'origin' = 0, move origin to solar system barycenter.

   Solar system barycenter coordinates are computed from rough
   approximations of the coordinates of the four largest planets.
*/

   if (origin == 0)
   {
      if (fabs (tjd - tlast) >= 1.0e-06)
      {
         for (i = 0; i < 3; i++)
            pbary[i] = vbary[i] = 0.0;

/*
   The following loop cycles once for each of the four planets.

   'sinl' and 'cosl' are the sine and cosine of the planet's mean
   longitude.
*/

         for (i = 0; i < 4; i++)
         {
            dlon = pl[i] + pn[i] * (tjd - T0);
            dlon = fmod (dlon, TWOPI);
            sinl = sin (dlon);
            cosl = cos (dlon);

            x =  pa[i] * cosl;
            y =  pa[i] * sinl * cose;
            z =  pa[i] * sinl * sine;
            xdot = -pa[i] * pn[i] * sinl;
            ydot =  pa[i] * pn[i] * cosl * cose;
            zdot =  pa[i] * pn[i] * cosl * sine;

            f = 1.0 / (pm[i] * tmass);

            pbary[0] += x * f;
            pbary[1] += y * f;
            pbary[2] += z * f;
            vbary[0] += xdot * f;
            vbary[1] += ydot * f;
            vbary[2] += zdot * f;
         }

         tlast = tjd;
      }

      for (i = 0; i < 3; i++)
      {
         pos[i] -= pbary[i];
         vel[i] -= vbary[i];
      }
   }

   return (ierr);
}

/********sun_eph */

void sun_eph (double jd,

              double *ra, double *dec, double *dis)
/*
------------------------------------------------------------------------

   PURPOSE:
      To compute equatorial spherical coordinates of Sun referred to
      the mean equator and equinox of date.

   REFERENCES:
      Bretagnon, P. and Simon, J.L. (1986).  Planetary Programs and
         Tables from -4000 to + 2800. (Richmond, VA: Willmann-Bell).

   INPUT
   ARGUMENTS:
      jd (double)
         Julian date on TDT or ET time scale.

   OUTPUT
   ARGUMENTS:
      ra (double)
         Right ascension referred to mean equator and equinox of date
         (hours).
      dec (double)
         Declination referred to mean equator and equinox of date 
         (degrees).
      dis (double)
         Geocentric distance (AU).

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      T0
      TWOPI
      RAD2DEG

   FUNCTIONS
   CALLED:
      sin           math.h
      cos           math.h
      asin          math.h
      atan2         math.h

   VER./DATE/
   PROGRAMMER:
      V1.0/08-94/JAB (USNO/AA)
      V1.1/05-96/JAB (USNO/AA): Compute mean coordinates instead of
                                apparent.

   NOTES:
      1. Quoted accuracy is 2.0 + 0.03 * T^2 arcsec, where T is
      measured in units of 1000 years from J2000.0.  See reference.

------------------------------------------------------------------------
*/
{
   short int i;

   double sum_lon = 0.0;
   double sum_r = 0.0;
   const double factor = 1.0e-07;
   double u, arg, lon, lat, t, t2, emean, sin_lon;

   struct sun_con
   {
   double l;
   double r;
   double alpha;
   double nu;
   };

   static const struct sun_con con[50] =
      {{403406.0,      0.0, 4.721964,     1.621043},
       {195207.0, -97597.0, 5.937458, 62830.348067}, 
       {119433.0, -59715.0, 1.115589, 62830.821524}, 
       {112392.0, -56188.0, 5.781616, 62829.634302}, 
       {  3891.0,  -1556.0, 5.5474  , 125660.5691 }, 
       {  2819.0,  -1126.0, 1.5120  , 125660.9845 }, 
       {  1721.0,   -861.0, 4.1897  ,  62832.4766 }, 
       {     0.0,    941.0, 1.163   ,      0.813  }, 
       {   660.0,   -264.0, 5.415   , 125659.310  }, 
       {   350.0,   -163.0, 4.315   ,  57533.850  }, 
       {   334.0,      0.0, 4.553   ,    -33.931  }, 
       {   314.0,    309.0, 5.198   , 777137.715  }, 
       {   268.0,   -158.0, 5.989   ,  78604.191  }, 
       {   242.0,      0.0, 2.911   ,      5.412  }, 
       {   234.0,    -54.0, 1.423   ,  39302.098  }, 
       {   158.0,      0.0, 0.061   ,    -34.861  }, 
       {   132.0,    -93.0, 2.317   , 115067.698  }, 
       {   129.0,    -20.0, 3.193   ,  15774.337  }, 
       {   114.0,      0.0, 2.828   ,   5296.670  }, 
       {    99.0,    -47.0, 0.52    ,  58849.27   }, 
       {    93.0,      0.0, 4.65    ,   5296.11   }, 
       {    86.0,      0.0, 4.35    ,  -3980.70   }, 
       {    78.0,    -33.0, 2.75    ,  52237.69   }, 
       {    72.0,    -32.0, 4.50    ,  55076.47   }, 
       {    68.0,      0.0, 3.23    ,    261.08   }, 
       {    64.0,    -10.0, 1.22    ,  15773.85   }, 
       {    46.0,    -16.0, 0.14    ,  188491.03  }, 
       {    38.0,      0.0, 3.44    ,   -7756.55  }, 
       {    37.0,      0.0, 4.37    ,     264.89  }, 
       {    32.0,    -24.0, 1.14    ,  117906.27  }, 
       {    29.0,    -13.0, 2.84    ,   55075.75  }, 
       {    28.0,      0.0, 5.96    ,   -7961.39  }, 
       {    27.0,     -9.0, 5.09    ,  188489.81  }, 
       {    27.0,      0.0, 1.72    ,    2132.19  }, 
       {    25.0,    -17.0, 2.56    ,  109771.03  }, 
       {    24.0,    -11.0, 1.92    ,   54868.56  }, 
       {    21.0,      0.0, 0.09    ,   25443.93  }, 
       {    21.0,     31.0, 5.98    ,  -55731.43  }, 
       {    20.0,    -10.0, 4.03    ,   60697.74  }, 
       {    18.0,      0.0, 4.27    ,    2132.79  }, 
       {    17.0,    -12.0, 0.79    ,  109771.63  }, 
       {    14.0,      0.0, 4.24    ,   -7752.82  }, 
       {    13.0,     -5.0, 2.01    ,  188491.91  }, 
       {    13.0,      0.0, 2.65    ,     207.81  }, 
       {    13.0,      0.0, 4.98    ,   29424.63  }, 
       {    12.0,      0.0, 0.93    ,      -7.99  }, 
       {    10.0,      0.0, 2.21    ,   46941.14  }, 
       {    10.0,      0.0, 3.59    ,     -68.29  }, 
       {    10.0,      0.0, 1.50    ,   21463.25  }, 
       {    10.0,     -9.0, 2.55    ,  157208.40  }};

/*
   Define the time unit 'u', measured in units of 10000 Julian years
   from J2000.0.
*/

   u = (jd - T0) / 3652500.0;
   
/*
   Compute longitude and distance terms from the series.
*/

   for (i = 0; i < 50; i++)
   {
      arg = con[i].alpha + con[i].nu * u;
      sum_lon += con[i].l * sin (arg);
      sum_r += con[i].r * cos (arg);
   }

/*
   Compute longitude, latitude, and distance referred to mean equinox
   and ecliptic of date.
*/

   lon = 4.9353929 + 62833.1961680 * u + factor * sum_lon;

   lon = fmod (lon, TWOPI);
   if (lon < 0.0)
      lon += TWOPI;

   lat = 0.0;

   *dis = 1.0001026 + factor * sum_r;

/*
   Compute mean obliquity of the ecliptic.
*/

   t = u * 100.0;
   t2 = t * t;
   emean = (0.001813 * t2 * t - 0.00059 * t2 - 46.8150 * t +
      84381.448) / RAD2SEC;

/*
   Compute equatorial spherical coordinates referred to the mean equator 
   and equinox of date.
*/

   sin_lon = sin (lon);
   *ra = atan2 ((cos (emean) * sin_lon), cos (lon)) * RAD2DEG;
   *ra = fmod (*ra, 360.0);
   if (*ra < 0.0)
      *ra += 360.0;
   *ra = *ra / 15.0;

   *dec = asin (sin (emean) * sin_lon) * RAD2DEG;
   
   return;
}
