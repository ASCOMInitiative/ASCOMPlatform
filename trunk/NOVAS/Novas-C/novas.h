/*
   NOVAS-C Version 2.0 (1 Nov 98)
   Header file for novas.c

   Naval Observatory Vector Astrometry Subroutines
   C Version

   U. S. Naval Observatory
   Astronomical Applications Dept.
   3450 Massachusetts Ave., NW
   Washington, DC  20392-5420
*/

#ifndef _NOVAS_
   #define _NOVAS_

   #ifndef __STDIO__
      #include <stdio.h>
   #endif

   #ifndef __MATH__
      #include <math.h>
   #endif

   #ifndef __STRING__
      #include <string.h>
   #endif

   #ifndef __STDLIB__
      #include <stdlib.h>
   #endif

   #ifndef __CTYPE__
      #include <ctype.h>
   #endif

   #ifndef _CONSTS_
      #include "novascon.h"
   #endif

   #ifndef _SOLSYS_
      #include "solarsystem.h"
   #endif

/*
   Structures.
*/

/*
   struct body: designates a celestial object.

   type              = type of body
                     = 0 ... major planet, Sun, or Moon
                     = 1 ... minor planet
   number            = body number
                       For 'type' = 0: Mercury = 1, ..., Pluto = 9,
                                       Sun = 10, Moon = 11
                       For 'type' = 1: minor planet number
   name              = name of the body (limited to 99 characters)
*/

   typedef struct
   {
      short int type;
      short int number;
      char name[100];
   } body;

/*
   struct site_info: data for the observer's location.  The atmospheric 
                     parameters are used only by the refraction 
                     function called from function 'equ_to_hor'.
                     Additional parameters can be added to this 
                     structure if a more sophisticated refraction model 
                     is employed.
                     
   latitude           = geodetic latitude in degrees; north positive.
   longitude          = geodetic longitude in degrees; east positive.
   height             = height of the observer in meters.
   temperature        = temperature (degrees Celsius).
   pressure           = atmospheric pressure (millibars)
*/

   typedef struct
   {
      double latitude;
      double longitude;
      double height;
      double temperature;
      double pressure;
   } site_info;

/*
   struct cat_entry: the astrometric catalog data for a star; equator 
                     and equinox and units will depend on the catalog.
                     While this structure can be used as a generic
                     container for catalog data, all high-level 
                     NOVAS-C functions require J2000.0 catalog data 
                     with FK5-type units (shown in square brackets
                     below).
                     
   catalog[4]         = 3-character catalog designator.
   starname[51]       = name of star.
   starnumber         = integer identifier assigned to star.
   ra                 = mean right ascension [hours].
   dec                = mean declination [degrees].
   promora            = proper motion in RA [seconds of time per 
                        century].
   promodec           = proper motion in declination [arcseconds per 
                        century].
   parallax           = parallax [arcseconds].
   radialvelocity     = radial velocity [kilometers per second].
*/

   typedef struct
   {
      char catalog[4];
      char starname[51];
      long int starnumber;
      double ra;
      double dec;
      double promora;
      double promodec;
      double parallax;
      double radialvelocity;
   } cat_entry;

/*
   Define "origin" constants.
*/

   #define BARYC  0
   #define HELIOC 1

/*
   Function prototypes
*/

   EXPORT short int app_star (double tjd, body *earth, cat_entry *star,

                       double *ra, double *dec);

   EXPORT short int app_planet (double tjd, body *ss_object, body *earth, 

                         double *ra, double *dec, double *dis);

   EXPORT short int topo_star (double tjd, body *earth, double deltat,
                        cat_entry *star, site_info *location, 

                        double *ra, double *dec);

   EXPORT short int topo_planet (double tjd, body *ss_object, body *earth,
                          double deltat, site_info *location, 

                          double *ra, double *dec, double *dis);

   EXPORT short int virtual_star (double tjd, body *earth, cat_entry *star,

                           double *ra, double *dec);

   EXPORT short int virtual_planet (double tjd, body *ss_object, body *earth,

                             double *ra, double *dec, double *dis);

   EXPORT short int local_star (double tjd, body *earth, double deltat,
                         cat_entry *star, site_info *location,

                         double *ra, double *dec);

   EXPORT short int local_planet (double tjd, body *ss_object, body *earth,
                           double deltat, site_info *location,

                           double *ra, double *dec, double *dis);

   EXPORT short int astro_star (double tjd, body *earth, cat_entry *star,

                         double *ra, double *dec);

   EXPORT short int astro_planet (double tjd, body *ss_object, body *earth,

                           double *ra, double *dec, double *dis);

   EXPORT short int mean_star (double tjd, body *earth, double ra, double dec,

                        double *mra, double *mdec);

   EXPORT void sidereal_time (double julianhi, double julianlo, double ee,

                       double *gst);

   EXPORT void pnsw (double tjd, double gast, double x, double y, 
              double *vece,

              double *vecs);

   EXPORT void spin (double st, double *pos1,

              double *pos2);

   EXPORT void wobble (double x, double y, double *pos1,

               double *pos2);

   EXPORT void terra (site_info *locale, double st,

               double *pos, double *vel);

   EXPORT void earthtilt (double tjd,

                   double *mobl, double *tobl, double *eqeq,
                   double *psi, double *eps);

   EXPORT void cel_pole (double del_dpsi, double del_deps);

   EXPORT short int get_earth (double tjd, body *earth,

                        double *tdb, double *bary_earthp,
                        double *bary_earthv, double *helio_earthp,
                        double *helio_earthv);

   EXPORT void proper_motion (double tjd1, double *pos1, double *vel1,
                       double tjd2,

                       double *pos2);

   EXPORT void bary_to_geo (double *pos, double *earthvector,

                     double *pos2, double *lighttime);

   EXPORT short int sun_field (double *pos, double *earthvector,

                        double *pos2);

   EXPORT short int aberration (double *pos, double *vel, double lighttime,

                         double *pos2);

   EXPORT void precession (double tjd1, double *pos, double tjd2,

                    double *pos2);

   EXPORT short int nutate (double tjd, short int fn1, double *pos,

                     double *pos2);

   EXPORT short int nutation_angles (double tdbtime,

                              double *longnutation,
                              double *obliqnutation);

   EXPORT void fund_args (double t,

                   double a[5]);

   EXPORT short int vector2radec (double *pos,

                           double *ra, double *dec);

   EXPORT void radec2vector (double ra, double dec, double dist,

                      double *vector);

   EXPORT void starvectors (cat_entry *star,

                     double *pos, double *vel);

   EXPORT void tdb2tdt (double tdb,

                 double *tdtjd, double *secdiff);

   EXPORT short int set_body (short int type, short int number, char *name,

                       body *cel_obj);

   EXPORT short int ephemeris (double tjd, body *cel_obj, short int origin,

                        double *pos, double *vel);


   EXPORT short int solarsystem (double tjd, short int body, short int origin, 

                          double *pos, double *vel);

   EXPORT double *readeph (int mp, char *name, double jd,

                    int *err);

   EXPORT void make_cat_entry (char catalog[4], char star_name[51],
                        long int star_num, double ra, double dec,
                        double pm_ra, double pm_dec, double parallax,
                        double rad_vel,

                        cat_entry *star);

   EXPORT void transform_hip (cat_entry *hipparcos,

                       cat_entry *fk5);

   EXPORT void transform_cat (short int option, double date_incat, 
                        cat_entry *incat, double date_newcat,
                        char newcat_id[4],

                        cat_entry *newcat);

   EXPORT void equ2hor (double tjd, double deltat, double x, double y, 
                 site_info *location, double ra, double dec, 
                 short int ref_option,

                 double *zd, double *az, double *rar, double *decr);

   EXPORT double refract (site_info *location, short int ref_option, 
                   double zd_obs);

   EXPORT double julian_date (short int year, short int month, short int day,
                       double hour);

   EXPORT void cal_date (double tjd,

                  short int *year, short int *month, short int *day,
                  double *hour);

#endif
