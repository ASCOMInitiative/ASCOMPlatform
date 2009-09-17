                           NOVAS-C
                        Version 2.0.1

                Astronomical Applications Dept.
                    U.S. Naval Observatory
                  Washington, DC  20392-5420

                  http://aa.usno.navy.mil/AA/


Description

   The Naval Observatory Vector Astrometry Subroutines, NOVAS, is an
integrated package of source-code modules for computing a wide variety
of common astrometric quantities and transformations.  The package can
provide, in one function call, the instantaneous coordinates
(apparent, topocentric, or astrometric place) of any star or planet. 
At a lower level, NOVAS also provides general astrometric utility
transformations, such as those for precession, nutation, aberration,
parallax, and the gravitational deflection of light.  The computations
are highly precise.  The NOVAS algorithms are, in fact, virtually
identical to those now used in the production of The Astronomical
Almanac.  NOVAS is easy-to-use and can be incorporated into data
reduction programs, telescope control systems, and simulations.

   NOVAS-C uses, as input, astrometric reference data that is
expressed in the International Astronomical Union (IAU) J2000.0
system.  In particular, NOVAS-C 2.0 supports (but is not limited to)
data that conforms to the International Celestial Reference System
(ICRS), adopted by the IAU in 1996.  ICRS-compatible data includes the
Hipparcos and Tycho Catalogues, the ACT Reference Catalog, the
International Celestial Reference Frame (ICRF), the Jet Propulsion
Laboratory's DE405 planetary ephemeris, and Earth orientation
measurements from the International Earth Rotation Service (IERS). The
list of ICRS-compatible data of various types is continually
expanding.  NOVAS-C can also be used with data conforming to the FK5
system.

   Effective use of NOVAS-C requires some background in positional
astronomy and considerable programming experience.  Please read the
accompanying documentation (in file novasguide.pdf) before attempting
to use the software.
 

Files

  novas.c        contains all supervisory and utility functions and 
                 most basic functions
  novas.h        header file for novas.c (includes structure 
                 definitions and function prototypes)
  novascon.c     contains most mathematical and physical constants 
                 used by the NOVAS-C system
  novascon.h     header file for novascon.c
  solsys2.c      function that serves as an interface between 
                 NOVAS-C and the Jet Propulsion Laboratory's 
                 lunar and planetary ephemerides
  solsys3.c      function that provides the position and velocity of 
                 the Earth or Sun without reference to an external 
                 data file
  solarsystem.h  header file for the "solsys.c" files.

  jplint.f       Fortran subroutine that serves as the interface 
                 between NOVAS-C and JPL's (Fortran) ephemeris 
                 access code.  For use with the software in 
                 solsys2.c.
  readeph0.c     dummy version of function readeph, the highest 
                 level call to the USNO minor planet ephemerides 
                 software.  This file is replaced by readeph.c 
                 (not supplied with NOVAS-C) when positions of 
                 selected minor planet are desired.

  checkout-st.c  main function that calls functions in novas.c and 
                 solsys3.c for the purpose of validating a basic 
                 local installation
  checkout-st.no output from the "checkout" application computed at 
                 USNO; compare this file with results obtained from 
                 your local installation
  checkout-mp.c  main function that calls functions in novas.c, 
                 solsys3.c, and the USNO minor planet software for 
                 the purpose of validating a local installation of 
                 NOVAS-C for use with the minor planet ephemerides
  checkout-mp.no output from the minor planet "checkout" application 
                 computed at USNO; compare this file with results 
                 obtained from your local installation

  novasguide.pdf NOVAS-C documentation in Portable Document Format 
                 (PDF); readable by Adobe Acrobat and other 
                 PDF-capable products


Installation

To install NOVAS-C on your local system, do the following:

   1. Copy all NOVAS-C files to a directory on your local system.
   2. Compile and link files checkout-st.c, novas.c, novascon.c,
solsys3.c, and readeph0.c.  Name the resulting application "checkout".
   3. Run the checkout application.  Compare the results that you get
with the data in file checkout-st.no.  If the results agree, the 
installation has probably been successful, but see the important note 
below.
   4. If you plan to use the USNO minor planet ephemerides with
NOVAS-C, another checkout program, checkout-mp.c, has been supplied. 
To check the installation of NOVAS-C with the minor planet ephemerides
software, repeat step 2, replacing readeph0.c provided with NOVAS-C,
with readeph.c, allocate.c, and chby.c from the minor planet
ephemeris software.  Run the resulting application, and compare your
results with the contents of checkout-mp.no.  The ephemeris file for
minor planet 2 Pallas (not supplied with NOVAS-C) is required to run
this test.

   Important Note: The checkout application exercises one supervisory
function and most, but not all, of the low-level functions in novas.c. 
Also, the checkout application does not use solsys2.c; hence,
planetary positions (other than those of the Earth) are not tested. 
Thus, use of the checkout application is not a complete test of
NOVAS-C.  A more complete check of your NOVAS-C implementation can be
made by comparing the results from the NOVAS-C supervisory functions
with results from the analogous NOVAS Fortran supervisory functions.

   Note that the checkout programs also provide examples of how the
NOVAS-C functions are called from an application program.


Changes in NOVAS-C Version 2.0.1 From Version 2.0

   o error handling in the "_planet" supervisory functions and 
in function ephemeris has been improved.

   o trig terms in function precession are now calculated once
for greater efficiency.


Changes in NOVAS-C Version 2.0 From Version 1.0

   o changed the argument lists of the highest-order functions: body
designations are now structures instead of simple (short) integers. 
This accommodates a wider range of body types.

   o added direct support for USNO minor planet ephemerides
(USNO/AE98) and indirect support for other ephemerides with new
function ephemeris.

   o added support for latest (1997 CD-ROM) version of the JPL solar
system ephemeris software in solsys2.c and jplint.c

   o incorporated IAU 1994 (IERS 1996) definition of the sidereal time
(implemented as a change to the calculation of the equation of the
equinoxes in function earthtilt).

   o generalized data structure used to contain star catalog data
(cat_entry in novas.h)

   o added new function make_cat_entry that creates a cat_entry data
structure from "loose" star catalog data.

   o added two new functions (transform_hip and transform_cat) to
support use of non-FK5 data in NOVAS-C.  Specifically, transform_hip
supports use of Hipparcos data.

   o added two new functions (equ2hor and refract) to support
transformation of equatorial coordinates to horizon coordinates, with
refraction optional.

   o created new function, fund_args, which contains the fundamental
arguments of the nutation series.

   o created global variables PSI_COR and EPS_COR and function
cel_pole to provide observed celestial pole offsets.

   o changed names of several low-level functions to be more
descriptive:
      - geocentric to bary_to_geo
      - nutation to nutation_angles
      - convert_tdb2tdt to tdb2tdt

   o update function tdb2tdt using expressions for mean elements
referred to J2000 epoch and reference system.

   o changed type of function precession from short int to void

   o TT time scale is used interchangeably with TDT time scale.

   o constants:
      - moved f and omega from function terra to file novascon.c.
      - updated value of C in AU/day.
      - updated value of OMEGA.
      - updated value of T0.
      - removed PI from novascon.c to avoid conflict with definition
of PI in Linux math.h.

   o changed name of sun function in solsys3.c to sun_eph to fix
problem on Sun Unix systems.

   o updated prologs and documentation.

   o cosmetic changes.

-USNO
 6-99  Version 2.0
 3-00  Version 2.0.1
