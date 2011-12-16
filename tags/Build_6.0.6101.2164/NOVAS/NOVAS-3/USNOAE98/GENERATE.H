/* Functions needed to generate Chebyshev form of asteroid ephemerides. */



#ifndef _GENEPH_
#define _GENEPH_

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

#ifndef _MATH_
#define _MATH_
#include<math.h>
#endif

#ifndef _CHBY_
#include"chby.h"
#endif

#ifndef _ALLOCATE_
#include"allocate.h"
#endif



void  generate ( int *mp, char *name, double *tol, char *head, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Generate a Chebyshev ephemeris file for an asteroid.

REFERENCE:
	Newhall, X X 1989, Celest. Mech., 45, 305

INPUT
ARGUMENTS:
	mp   = asteroid number
	name = asteroid name
	tol  = tolerance (maximum deviation in AU between the input ephemeris
	       and the generated Chebyshev ephemeris)
	head = header ( a char* giving relevant information on the ephemeris
	       appended to the beginning of the Chebyshev ephemeris file).

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )
	    = 2 ( Failure to open temporary files )
	    = 3 ( Failure to open input data file )
	    = 4 ( Failure of Chebyshev generator to converge. )
	    = 5 ( Failure to open output data file )

INPUT
FILE:
	Asteroid ASCII ephemeris file, '.eph' extension.

OUTPUT
FILE:
	Asteroid Chebyshev polynomial ephemeris file, '.chby' extension.

RETURNED
VALUE:
	void

FUNCTIONS
CALLED:
	atof       dmalloc   dpmalloc   fabs     fclose   feof     fflush   fopen
	fprintf    fread     free       fscanf   fseek    fwrite   gaussj   maket
	maketdot   maximum   printf     pwr      remove   sizeof   sqrt     strcat
	strcpy     strlen    times

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	This function is designed to compute an export ephemeris for an asteroid
	using the method described in Newhall, X X 1989, Celest. Mech., 45, 305.
	It requires a minimum kernel size of 32 days and computes the coefficients
	to find the Chebyshev polynomial that fits the data data in the maximum
	number of days with the minimum necessary order of Chebyshev polynomial.
----------------------------------------------------------------------------*/



void  gaussj (double **a, int n, int err);
/*----------------------------------------------------------------------------
PURPOSE:
	Perform Gauss-Jordan elimination on a n x n matrix to invert it.

REFERENCE:
	Press, W. H., Teukolsky, S. A., Vettering, W. T., & Flannery, B. P. 1992,
	Numerical Recipes in C (2nd edition), pp. 36 - 41.

INPUT
ARGUMENTS:
	a = square matrix to invert
	n = dimension of the matrix.

OUTPUT
ARGUMENTS:
	a   = inverted matrix.
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	void

FUNCTIONS
CALLED:
	imalloc   fabs   free

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



void times ( double **a, int i, int j, int k, double **b, double **c );
/*----------------------------------------------------------------------------
PURPOSE:
	Multiply an i x j matrix by a j x k matrix.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	a = i x j matrix
	i = number of rows in the i x j matrix
	j = number of columns in the i x j matrix and the number of columns in
	    the j x k matrix.
	k = number of columns in the j x k matrix

OUTPUT
ARGUMENT:
	c = i x k matrix product.

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



double maximum ( double *arr, int n );
/*----------------------------------------------------------------------------
PURPOSE:
	Find the largest member of an array of n doubles.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	arr = array
	n   = number of elements in the array.

OUTPUT
ARGUMENTS:
	None.

RETURNED
VALUE:
	double = largest member of the array

FUNCTIONS
CALLED:
	None.

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/

#endif
