/* Functions for generating a thirteenth order Chebyshev polynomial. */



#ifndef _CHBY_
#define _CHBY_

#ifndef _STDIO_
#define _STDIO_
#include<stdio.h>
#endif

#ifndef _STDLIB_
#define _STDLIB_
#include<stdlib.h>
#endif



double  pwr ( double x, int y);
/*----------------------------------------------------------------------------
PURPOSE:
	Raise a double x to an int power y.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	x = number to raise to a power.
	y = integer power to raise x.

OUTPUT
ARGUMENTS:
	None.

RETURNED
VALUE:
	double = result of raising x to the power y.

FUNCTIONS
CALLED:
	None

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



double  *maket ( double time );
/*----------------------------------------------------------------------------
PURPOSE:
	Compute the values for the Chebyshev polynomial to thirteenth order for a 
	given time.

REFERENCE:
	Newhall, X X 1989, Celestial Mechanics, 45, 305.

INPUT
ARGUMENT:
	time = time at which to calculate the Chebyshev polynomials.

OUTPUT
ARGUMENTS:
	None.

RETURNED
VALUE:
	double* = array of the zeroth through thirteenth order Chebyshev
	          polynomial values.

FUNCTIONS
CALLED:
	dmalloc   pwr

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



double  *maketdot ( double time );
/*----------------------------------------------------------------------------
PURPOSE:
	Compute the values for a the derivatives of the Chebyshev polynomial to
	thirteenth order for a given time.

REFERENCE:
	Newhall, X X 1989, Celestial Mechanics, 45, 305.

INPUT
ARGUMENT:
	time = time at which to calculate the Chebyshev polynomials.

OUTPUT
ARGUMENTS:
	None.

RETURNED
VALUE:
	double* = array of the zeroth through thirteenth order Chebyshev
	          polynomial derivative values.

FUNCTIONS
CALLED:
	dmalloc   pwr

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/

#endif
