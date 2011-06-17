/* Functions needed to read the binary Chebyshev file form of the asteroid
   ephemerides */


#ifndef __ASCOM__   
  #include "..\NOVAS3\ascom.h" //PWGS Added ascom.h include
#endif

#ifndef _READEPH_
#define _READEPH_

#ifndef _STDIO_
#define _STDIO_
#include<stdio.h>
#endif

#ifndef _STDLIB_
#define _STDLIB_
#include<stdlib.h>
#endif

#ifndef _STRING_
#define _STRING_
#include<string.h>
#endif

#ifndef _CHBY_
#include"chby.h"
#endif

#ifndef _ALLOCATE_
#include"allocate.h"
#endif


// err changed to *err in order to return a value by Peter Simpson 27th February 2010
EXPORT double  *readeph ( int mp, char *name, double jd, int *err ); //EXPORT Added by Peter Simpson to make it visible outside the DLL
/*----------------------------------------------------------------------------
PURPOSE:
	Given an asteroid number and name along with the Julian Date and origin
	produces the Cartesian heliocentric equatorial coordinates of the asteroid
	for the J2000.0 epoch 	coordinate system from a set of Chebyshev
	polynomials on file.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	mp   = number of the asteroid for which the position in desired.
	name = name of the asteroid.
	jd   = Julian date on which to find the position and velocity.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )
	    = 2 ( Mismatch between asteroid name and number )
	    = 3 ( Julian date out of bounds )
	    = 4 ( Cannot find Chebyshev polynomial file )

INPUT
FILE:
	Asteroid Chebyshev polynomial ephemeris file, '.chby' extension.

RETURNED
VALUE:
	double* = six element array giving first the position in AU and then the
	          velocity of the asteroid in AU/day.

FUNCTIONS
CALLED:
	cmalloc      cpmalloc    cprealloc   dmalloc    dpmalloc    dppmalloc
	dpprealloc   dprealloc   drealloc    fopen      Fpmalloc    Fprealloc
	fprintf      fread       free        fseek      imalloc     ipmalloc
	iprealloc    irealloc    maket       maketdot   printf      readdata
	sizeof      strcat       strcmp      strcpy     strlen

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	1. The file name of the asteroid is taken from the name given.  It is
	assumed that the name is all in lower case characters.
----------------------------------------------------------------------------*/



void  initeph ( void );
/*----------------------------------------------------------------------------
PURPOSE:
	Initialize ephemeris file structure to 0.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	None.

OUTPUT
ARGUMENTS:
	None.

RETURNED
VALUE:
	void

FUNCTIONS
CALLED:
	None.

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/


void  cleaneph ( void );
/*----------------------------------------------------------------------------
PURPOSE:
	Clean up ephemeris file structure

REFERENCES:
	None.

INPUT
ARGUMENTS:
	None.

OUTPUT
ARGUMENTS:
	None.

RETURNED
VALUE:
	void

FUNCTIONS
CALLED:
	fclose   free

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



void  readdata ( int num );
/*----------------------------------------------------------------------------
PURPOSE:
	Reads a data record from the pre-positioned file to get the Chebyshev
	polynomial series for a given group of dates.

REFERENCES:
	None.

INPUT
ARGUMENT:
	num = number of the file to read the data from.

OUTPUT
ARGUMENTS:
	None.

RETURNED
VALUE:
	void

FUNCTIONS
CALLED:
	fread   sizeof

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None
----------------------------------------------------------------------------*/

#endif
