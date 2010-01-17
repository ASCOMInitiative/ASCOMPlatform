/*
   C version of the JPL Ephemeris Manager
   Version 3.1: WKP 11/2007
*/

#ifndef __EPHMAN__
   #include "eph_manager.h"
#endif

char ephem_name[51];

short DE_Number;
short km;

long ipt[3][12], lpt[3], nrl, np, nv;
long record_length;

double ss[3], jplau, pc[18], vc[18], twot, em_ratio;
double *buffer;

FILE *EPHFILE = NULL;

/********Ephem_Open */

short Ephem_Open (char *Ephem_Name,

                  double *JD_Begin, double *JD_End)
/*
------------------------------------------------------------------------

   PURPOSE:    
      This function opens a JPL planetary ephemeris file and
      sets initial values.  This function must be called
      prior to calls to the other JPL ephemeris functions.

   REFERENCES: 
      Standish, E.M. and Newhall, X X (1988). "The JPL Export 
         Planetary Ephemeris"; JPL document dated 17 June 1988.

   INPUT
   ARGUMENTS:  
      Ephem_Name (char *)
         Name of the direct-access ephemeris file.
                          
   OUTPUT
   ARGUMENTS:  
      JD_Begin (double)
         Beginning Julian date of the ephemeris file.
      JD_End (double)
         Ending Julian date of the ephemeris file.

   RETURNED
   VALUE:
      (short)
          0   ...file exists and is opened correctly.
          1   ...file does not exist/not found.
          2-11...error reading from file header.

   GLOBALS
   USED:     
      ephem_name        eph_manager.h
      DE_Number         eph_manager.h
      ss                eph_manager.h
      jplau             eph_manager.h
      pc                eph_manager.h
      vc                eph_manager.h
      twot              eph_manager.h
      em_ratio          eph_manager.h
      buffer            eph_manager.h
      ipt               eph_manager.h
      lpt               eph_manager.h
      nrl               eph_manager.h
      km                eph_manager.h
      np                eph_manager.h
      nv                eph_manager.h
      record_length     eph_manager.h
      EPHFILE           eph_manager.h

   FUNCTIONS
   CALLED:     
      fclose            stdio.h
      free              stdlib.h
      strcpy            string.h
      fopen             stdio.h
      fread             stdio.h 
      calloc            stdlib.h

   VER./DATE/
   PROGRAMMER: 
      V1.0/06-90/JAB (USNO/NA)
      V1.1/06-92/JAB (USNO/AA): Restructure and add initializations.
      V1.2/07-98/WTH (USNO/AA): Modified to open files for different 
                                ephemeris types. (200,403,404,405,406)
      V1.3/11-07/WKP (USNO/AA): Updated prolog.

   NOTES:
      km...flag defining physical units of the output states. 
         = 1, km and km/sec
         = 0, AU and AU/day
      Default value is 0 (km determines time unit for nutations.
                          Angle unit is always radians.)

------------------------------------------------------------------------
*/
{
   char ttl[252], cnam[2400];

   short i,j;

   long ncon, denum;

   if (EPHFILE)
   {
      fclose (EPHFILE);
      free (buffer);
   }

   strcpy (ephem_name, Ephem_Name);

/*
   Open file ephem_name.
*/

   if ((EPHFILE = fopen (ephem_name, "rb")) == NULL)
   {
      return 1;
   }
    else
   {

/*
   File found. Set initializations and default values.
*/

      km = 0;

      nrl = 0;

      np = 2;
      nv = 3;
      twot = 0.0;

      for (i = 0; i < 18; i++)
      {
         pc[i] = 0.0;
         vc[i] = 0.0;
      }

      pc[0] = 1.0;
      vc[1] = 1.0;

/*
   Read in values from the first record, aka the header.
*/

      if (fread (ttl, sizeof ttl, 1, EPHFILE) != 1)
         return 2;
      if (fread (cnam, sizeof cnam, 1, EPHFILE) != 1)
         return 3;
      if (fread (ss, sizeof ss, 1, EPHFILE) != 1)
         return 4;
      if (fread (&ncon, sizeof ncon, 1, EPHFILE) != 1)
         return 5;
      if (fread (&jplau, sizeof jplau, 1, EPHFILE) != 1)
         return 7;
      if (fread (&em_ratio, sizeof em_ratio, 1, EPHFILE) != 1)
         return 8;
      for (i = 0; i < 12; i++)
         for (j = 0; j < 3; j++)
            if (fread (&ipt[j][i], sizeof(long), 1, EPHFILE) != 1)
               return 9;
      if (fread (&denum,sizeof denum, 1, EPHFILE) != 1)
         return 10;
      if (fread (lpt, sizeof lpt, 1, EPHFILE) != 1)
         return 11;

/*
   Set the value of the record length according to what JPL ephemeris is 
   being opened.
*/

      switch (denum) {
         case 200:
            record_length = 6608;
            break;
         case 403: case 405:
            record_length = 8144;
            break;
         case 404: case 406:
            record_length = 5824;
            break;
      }
      buffer = (double *) calloc (record_length / 8, sizeof(double));

      DE_Number = (short) denum;
      *JD_Begin = ss[0];
      *JD_End = ss[1];
   }

   return 0;
}

/********Ephem_Close */

short Ephem_Close (void)
/*
------------------------------------------------------------------------

   PURPOSE:    
      This function closes a JPL planetary ephemeris file.

   REFERENCES: 
      Standish, E.M. and Newhall, X X (1988). "The JPL Export 
         Planetary Ephemeris"; JPL document dated 17 June 1988.

   INPUT
   ARGUMENTS:  
      None.                          

   OUTPUT
   ARGUMENTS:  
      None.

   RETURNED
   VALUE:
      (short)
          0  ...file was already closed or closed correctly.
          EOF...error closing file, check global 'errno' variable for details.

   GLOBALS
   USED:     
      buffer            eph_manager.h
      EPHFILE           eph_manager.h

   FUNCTIONS
   CALLED:     
      fclose            stdio.h
      free              stdlib.h

   VER./DATE/
   PROGRAMMER: 
      V1.0/11-07/WKP (USNO/AA)

   NOTES:      
      None.

------------------------------------------------------------------------
*/
{
   short error = 0;
   if (EPHFILE)
   {
      error = fclose (EPHFILE);
      free (buffer);
   }
   return error;
}

/********Planet_Ephemeris */

short Planet_Ephemeris (double tjd[2], short target, short center,

                        double *position, double *velocity)
/*
------------------------------------------------------------------------

   PURPOSE:    
      This function accesses the JPL planetary ephemeris to give the
      position and velocity of the target object with respect to the 
      center object.

   REFERENCES: 
      Standish, E.M. and Newhall, X X (1988). "The JPL Export
         Planetary Ephemeris"; JPL document dated 17 June 1988.

   INPUT
   ARGUMENTS:
      tjd[2] (double)
         Two-element array containing the Julian date, which may be 
         split any way (although the first element is usually the
         "integer" part, and the second element is the "fractional" 
         part).  Julian date is in the TDB or "T_eph" time scale.
      target (short)
         Number of 'target' point.
      center (short)
         Number of 'center' (origin) point.
         The numbering convention for 'target' and'center' is:
            0  =  Mercury           7 = Neptune
            1  =  Venus             8 = Pluto
            2  =  Earth             9 = Moon
            3  =  Mars             10 = Sun
            4  =  Jupiter          11 = Solar system bary.
            5  =  Saturn           12 = Earth-Moon bary.
            6  =  Uranus           13 = Nutations (long. and obliq.)
            (If nutations are desired, set 'target' = 14;
             'center' will be ignored on that call.)

   OUTPUT
   ARGUMENTS:
      *position (double)
         Position vector array of target relative to center, measured 
         in AU. 
      *velocity (double)
         Velocity vector array of target relative to center, measured 
         in AU/day.

   RETURNED
   VALUE:
      (short)
         0  ...everything OK.
         1,2...error returned from State.


   GLOBALS
   USED:
      em_ratio          eph_manager.h

   FUNCTIONS
   CALLED:
      State             eph_manager.h

   VER./DATE/
   PROGRAMMER:
      V1.0/03-93/WTH (USNO/AA): Convert FORTRAN to C.
      V1.1/07-93/WTH (USNO/AA): Update to C standards.
      V2.0/07-98/WTH (USNO/AA): Modified for ease of use and linearity.
      V3.0/11-06/JAB (USNO/AA): Allowed for use of input 'split' Julian 
                                date for higher precision.
      V3.1/11-07/WKP (USNO/AA): Updated prolog and error codes.
      V3.1/12-07/WKP (USNO/AA): Removed unreferenced variables.
                                
   NOTES:
      None.

------------------------------------------------------------------------
*/
{
   short i, error = 0, earth = 2, moon = 9;
   short Do_Earth = 0, Do_Moon = 0;

   double jed[2];
   double Pos_Moon[3] = {0.0,0.0,0.0}, Vel_Moon[3] = {0.0,0.0,0.0},
          Pos_Earth[3] = {0.0,0.0,0.0}, Vel_Earth[3] = {0.0,0.0,0.0};
   double target_pos[3] = {0.0,0.0,0.0}, target_vel[3] = {0.0,0.0,0.0},
          center_pos[3] = {0.0,0.0,0.0}, center_vel[3] = {0.0,0.0,0.0};

/*
   Initialize 'jed' for 'state' and set up component count.
*/

   jed[0] = tjd[0];
   jed[1] = tjd[1];

/*
   Check for target point = center point.
*/

   if (target == center)
   {
      for (i = 0; i < 3; i++)
      {
         position[i] = 0.0;
         velocity[i] = 0.0;
      }
      return 0;
   }

/*
   Check for instances of target or center being Earth or Moon,
   and for target or center being the Earth-Moon barycenter.
*/

   if ((target == earth) || (center == earth))
      Do_Moon = 1;
   if ((target == moon) || (center == moon))
      Do_Earth = 1;
   if ((target == 12) || (center == 12))
      Do_Earth = 1;

   if (Do_Earth)
      error = State (jed,2, Pos_Earth,Vel_Earth);
   if (error)
      return error;

   if (Do_Moon)
      error = State (jed,9, Pos_Moon,Vel_Moon);
   if (error)
      return error;

/*
   Make call to State for target object.
*/

   if (target == 11)
   {
      for (i = 0; i < 3; i++)
      {
         target_pos[i] = 0.0;
         target_vel[i] = 0.0;
      }
   }
    else if (target == 12)
   {
      for (i = 0; i < 3; i++)
      {
         target_pos[i] = Pos_Earth[i];
         target_vel[i] = Vel_Earth[i];
      }
   }
    else
      error = State (jed,target, target_pos,target_vel);

   if (error)
      return error;

/*
   Make call to State for center object.
*/

/*
   If the requested center is the Solar System barycenter,
   then don't bother with a second call to State.
*/

   if (center == 11)
   {
      for (i = 0; i < 3; i++)
      {
         center_pos[i] = 0.0;
         center_vel[i] = 0.0;
      }
   }
/*
   Center is Earth-Moon barycenter, which was already computed above.
*/

    else if (center == 12)
   {
      for (i = 0; i < 3; i++)
      {
         center_pos[i] = Pos_Earth[i];
         center_vel[i] = Vel_Earth[i];
      }
   }
    else
      error = State (jed,center, center_pos,center_vel);

/*
   Check for cases of Earth as target and Moon as center or vice versa.
*/

   if ((target == earth) && (center == moon))
   {
      for (i = 0; i < 3; i++)
      {
         position[i] = -center_pos[i];
         velocity[i] = -center_vel[i];
      }
      return 0;
   }
    else if ((target == moon) && (center == earth))
   {
      for (i = 0; i < 3; i++)
      {
         position[i] = target_pos[i];
         velocity[i] = target_vel[i];
      }
      return 0;
   }

/*
   Check for Earth as target, or as center.
*/

    else if (target == earth)
   {
      for (i = 0; i < 3; i++)
      {
         target_pos[i] = target_pos[i] - (Pos_Moon[i] / (1.0 + em_ratio));
         target_vel[i] = target_vel[i] - (Vel_Moon[i] / (1.0 + em_ratio));
      }
   }
    else if (center == earth)
   {
      for (i=0; i<3; i++)
      {
         center_pos[i] = center_pos[i] - (Pos_Moon[i] / (1.0 + em_ratio));
         center_vel[i] = center_vel[i] - (Vel_Moon[i] / (1.0 + em_ratio));
      }
   }

/*
   Check for Moon as target, or as center.
*/

    else if (target == moon)
   {
      for (i = 0; i < 3; i++)
      {
         target_pos[i] = (Pos_Earth[i] - (target_pos[i] / (1.0 + em_ratio))) + target_pos[i];
         target_vel[i] = (Vel_Earth[i] - (target_vel[i] / (1.0 + em_ratio))) + target_vel[i];
      }
   }
    else if (center == moon)
   {
      for (i = 0; i < 3; i++)
      {
         center_pos[i] = (Pos_Earth[i] - (center_pos[i] / (1.0 + em_ratio))) + center_pos[i];
         center_vel[i] = (Vel_Earth[i] - (center_vel[i] / (1.0 + em_ratio))) + center_vel[i];
      }
   }

/*
   Compute position and velocity vectors.
*/

   for (i = 0; i < 3; i++)
   {
      position[i] = target_pos[i] - center_pos[i];
      velocity[i] = target_vel[i] - center_vel[i];
   }

   return 0;
}


/********State */

short State (double *jed, short target,

             double *target_pos, double *target_vel)
/*
------------------------------------------------------------------------

   PURPOSE:    
      This function reads and interpolates the JPL planetary 
      ephemeris file.

   REFERENCES:
      Standish, E.M. and Newhall, X X (1988). "The JPL Export
         Planetary Ephemeris"; JPL document dated 17 June 1988.

   INPUT
   ARGUMENTS:
      jed (double)
         2-element Julian date (TDB) at which interpolation is wanted.
         Any combination of jed[0]+jed[1] which falls within the time 
         span on the file is a permissible epoch.  See Note 1 below.
      target (short)
         The requested body to get data for from the ephemeris file.
         The designation of the astronomical bodies is:
                 = 0: Mercury,               1: Venus, 
                 = 2: Earth-Moon barycenter, 3: Mars, 
                 = 4: Jupiter,               5: Saturn, 
                 = 6: Uranus,                7: Neptune, 
                 = 8: Pluto,                 9: geocentric Moon, 
                 =10: Sun.

   OUTPUT
   ARGUMENTS:
      *target_pos (double)
         The barycentric position vector array of the requested object, in AU.
         (If target object is the Moon, then the vector is geocentric.)
      *target_vel (vectors)
         The barycentric velocity vector array of the requested object, in AU/Day.

         Both vectors are referenced to the Earth mean equator and
         equinox of epoch.

   RETURNED
   VALUE:
      (short)
         0...everything OK.
         1...error reading ephemeris file.
         2...epoch out of range.

   GLOBALS
   USED:
      km                eph_manager.h
      EPHFILE           eph_manager.h
      ipt               eph_manager.h
      buffer            eph_manager.h
      nrl               eph_manager.h
      record_length     eph_manager.h
      ss                eph_manager.h
      jplau             eph_manager.h


   FUNCTIONS
   CALLED:
      Split             eph_manager.h
      fseek             stdio.h
      fread             stdio.h
      Interpolate       eph_manager.h

   VER./DATE/
   PROGRAMMER:
      V1.0/03-93/WTH (USNO/AA): Convert FORTRAN to C.
      V1.1/07-93/WTH (USNO/AA): Update to C standards.
      V2.0/07-98/WTH (USNO/AA): Modify to make position and velocity
                                two distinct vector arrays.  Routine set
                                to compute one state per call.
      V2.1/11-07/WKP (USNO/AA): Updated prolog.

   NOTES:
      1. For ease in programming, the user may put the entire epoch in
         jed[0] and set jed[1] = 0. For maximum interpolation accuracy, 
         set jed[0] = the most recent midnight at or before
         interpolation epoch, and set jed[1] = fractional part of a day
         elapsed between jed[0] and epoch. As an alternative, it may
         prove convenient to set jed[0] = some fixed epoch, such as
         start of the integration and jed[1] = elapsed interval between
         then and epoch.

------------------------------------------------------------------------
*/
{
   short i;

   long nr, rec;

   double t[2], aufac = 1.0, jd[4], s;

/*
   Set units based on value of the 'km' flag.
*/

   if (km)
      t[1] = ss[2] * 86400.0;
    else
   {
      t[1] = ss[2];
      aufac = 1.0 / jplau;
   }

/*
   Check epoch.
*/

   s = jed[0] - 0.5;
   Split (s, &jd[0]);
   Split (jed[1], &jd[2]);
   jd[0] += jd[2] + 0.5;
   jd[1] += jd[3];
   Split (jd[1], &jd[2]);
   jd[0] += jd[2];

/*
   Return error code if date is out of range.
*/

   if ((jd[0] < ss[0]) || ((jd[0] + jd[3]) > ss[1]))
      return 2;

/*
   Calculate record number and relative time interval.
*/

   nr = (long) ((jd[0] - ss[0]) / ss[2]) + 3;
   if (jd[0] == ss[1])
      nr -= 2;
   t[0] = ((jd[0] - ((double) (nr-3) * ss[2] + ss[0])) + jd[3]) / ss[2];

/*
   Read correct record if it is not already in memory.
*/

   if (nr != nrl)
   {
      nrl = nr;
      rec = (nr - 1) * record_length;
      fseek (EPHFILE, rec, SEEK_SET);
      if (!fread (buffer, record_length, 1, EPHFILE))
         return 1;
   }

/*
   Check and interpolate for requested body.
*/

   Interpolate (&buffer[ipt[0][target]-1],t,ipt[1][target],ipt[2][target], target_pos,target_vel);

   for (i = 0; i < 3; i++)
   {
      target_pos[i] *= aufac;
      target_vel[i] *= aufac;
   }

   return 0;
}

/********Interpolate */

void Interpolate (double *buf, double *t, long ncf, long na,

                  double *position, double *velocity)
/*
------------------------------------------------------------------------

   PURPOSE:    
      This function differentiates and interpolates a set of
      Chebyshev coefficients to give position and velocity.

   REFERENCES: 
      Standish, E.M. and Newhall, X X (1988). "The JPL Export
         Planetary Ephemeris"; JPL document dated 17 June 1988.

   INPUT
   ARGUMENTS:
      *buf (double)
         Array of Chebyshev coefficients of position.
      t (double)
         t[0] is fractional time interval covered by coefficients at
         which interpolation is desired (0 <= t(1) <= 1).
         t[1] is length of whole interval in input time units.
      ncf (long)
         Number of coefficients per component.
      na (long)
         Number of sets of coefficients in full array
         (i.e., number of sub-intervals in full interval).

   OUTPUT
   ARGUMENTS:
      *position (double)
         Position array of requested object.
      *velocity (double)
         Velocity array of requested object.

   RETURNED
   VALUE:
      None.

   GLOBALS
   USED:
      np                eph_manager.h
      nv                eph_manager.h
      pc                eph_manager.h
      vc                eph_manager.h
      twot              eph_manager.h

   FUNCTIONS
   CALLED:
      fmod              math.h
      printf            stdio.h
      exit              stdlib.h      

   VER./DATE/
   PROGRAMMER:
      V1.0/03-93/WTH (USNO/AA): Convert FORTRAN to C.
      V1.1/07-93/WTH (USNO/AA): Update to C standards.
      V1.2/07-98/WTH (USNO/AA): Modify to make position and velocity
                                two distinct vector arrays.
      V1.3/11-07/WKP (USNO/AA): Updated prolog.
      V1.4/12-07/WKP (USNO/AA): Changed ncf and na arguments from short 
                                to long.

   NOTES:
      None.

------------------------------------------------------------------------
*/
{
   long i, j, k, l;

   double dna, dt1, temp, tc, vfac;

/*
   Get correct sub-interval number for this set of coefficients and
   then get normalized Chebyshev time within that subinterval.
*/

   dna = (double) na;
   dt1 = (double) ((long) t[0]);
   temp = dna * t[0];
   l = (long) (temp - dt1);

/*
   'tc' is the normalized Chebyshev time (-1 <= tc <= 1).
*/

   tc = 2.0 * (fmod (temp,1.0) + dt1) - 1.0;

/*
   Check to see whether Chebyshev time has changed, and compute new
   polynomial values if it has.  (The element pc[1] is the value of
   t1[tc] and hence contains the value of 'tc' on the previous call.)
*/

   if (tc != pc[1])
   {
      np = 2;
      nv = 3;
      pc[1] = tc;
      twot = tc + tc;
   }

/*
   Be sure that at least 'ncf' polynomials have been evaluated and
   are stored in the array 'pc'.
*/

   if (np < ncf)
   {
      for (i = np; i < ncf; i++)
         pc[i] = twot * pc[i-1] - pc[i-2];
      np = ncf;
   }

/*
   Interpolate to get position for each component.
*/

   for (i = 0; i < 3; i++)
   {
      position[i] = 0.0;
      for (j = ncf-1; j >= 0; j--)
      {
         k = j + (i * ncf) + (l * (3 * ncf));
         if ((k < 0) || (k > 1000))
         {
            printf("1 k = %d\n",k);
            exit(0);
         }
         position[i] += pc[j] * buf[k];
      }
   }

/*
   If velocity interpolation is desired, be sure enough derivative
   polynomials have been generated and stored.
*/

   vfac = (2.0 * dna) / t[1];
   vc[2] = 2.0 * twot;
   if (nv < ncf)
   {
      for (i = nv; i<ncf; i++)
         vc[i] = twot * vc[i-1] + pc[i-1] + pc[i-1] - vc[i-2];
      nv = ncf;
   }

/*
   Interpolate to get velocity for each component.
*/

   for (i = 0; i < 3; i++)
   {
      velocity[i] = 0.0;
      for (j = ncf-1; j > 0; j--)
      {
         k = j + (i * ncf) + (l * (3 * ncf));
         velocity[i] += vc[j] * buf[k];
         if ((k < 0) || (k > 1000))
         {
            printf("2 k = %d\n",k);
            exit(0);
         }
      }
      velocity[i] *= vfac;
   }

   return;
}

/********Split */

void Split (double tt, 

            double *fr)
/*
------------------------------------------------------------------------

   PURPOSE:    
      This function breaks up a double number into a double integer
      part and a fractional part.

   REFERENCES: 
      Standish, E.M. and Newhall, X X (1988). "The JPL Export
         Planetary Ephemeris"; JPL document dated 17 June 1988.

   INPUT
   ARGUMENTS:
      tt (double)
         Input number.

   OUTPUT
   ARGUMENTS:
      fr[2] (double)
         2-element output array; fr[0] contains integer part, 
         fr[1] contains fractional part. For negative input numbers, 
         fr[0] contains the next more negative integer; 
         fr[1] contains a positive fraction.

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
      V1.0/06-90/JAB (USNO/NA): CA coding standards
      V1.1/03-93/WTH (USNO/AA): Convert to C.
      V1.2/07-93/WTH (USNO/AA): Update to C standards.

   NOTES:
      None.

------------------------------------------------------------------------
*/
{

/*
   Get integer and fractional parts.
*/

   fr[0] = (double)((long) tt);
   fr[1] = tt - fr[0];

/*
   Make adjustments for negative input number.
*/

   if ((tt >= 0.0) || (fr[1] == 0.0))
      return;
    else
   {
      fr[0] = fr[0] - 1.0;
      fr[1] = fr[1] + 1.0;
   }

   return;
}
