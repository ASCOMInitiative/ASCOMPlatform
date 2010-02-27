/* Functions for allocating and reallocating memory for various data types
   including error checking. */



#ifndef _ALLOCATE_
#define _ALLOCATE_

#ifndef _STDIO_
#define _STDIO_
#include<stdio.h>
#endif

#ifndef _STDLIB_
#define _STDLIB_
#include<stdlib.h>
#endif



double  *dmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to double and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	double* = pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



double  **dpmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to a pointer to double and do error
	checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	double** = pointer to a pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



double  ***dppmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to a pointer to a pointer to double and do
	error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	double*** = pointer to a pointer to a pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



char  *cmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to char and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	char* = pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



char  **cpmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to a pointer to char and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	char** = pointer to a pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



int  *imalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to int and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	int* = pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



int  **ipmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to a pointer to int and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	int** = pointer to a pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



long  *lmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to long and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	long* = pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



long  **lpmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory a pointer to a pointer to long and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	long** = pointer to a pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



FILE  *Fmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to FILE and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	FILE* = pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



FILE  **Fpmalloc ( size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Allocate memory for a pointer to a pointer to FILE and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENT:
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	FILE** = pointer to a pointer to allocated memory

FUNCTIONS
CALLED:
	fprintf   malloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



double  *drealloc ( double *ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to double and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to double that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	double* = pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



double  **dprealloc ( double **ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to a pointer to double and do error
	checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to pointer to double that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	double** = pointer to a pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



double  ***dpprealloc ( double ***ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to a pointer to a pointer to double and do
	error checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to pointer to pointer to double that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	double*** = pointer to a pointer to a pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



char  *crealloc ( char *ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to char and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to char that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	char* = pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



char  **cprealloc ( char **ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to a pointer to char and do error
	checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to pointer to char that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	char** = pointer to a pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



int  *irealloc ( int *ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to int and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to int that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	int* = pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



int  **iprealloc ( int **ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to a pointer int and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to pointer to int that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	int** = pointer to a pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



long  *lrealloc ( long *ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to long and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to long that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	long* = pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



long  **lprealloc ( long **ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to a pointer to long and do error
	checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to pointer to long that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	long** = pointer to a pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



FILE  *Frealloc ( FILE *ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to FILE and do error checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to FILE that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	FILE* = pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/



FILE  **Fprealloc ( FILE **ptr, size_t size, int err );
/*----------------------------------------------------------------------------
PURPOSE:
	Reallocate memory for a pointer to a pointer to FILE and do error
	checking.

REFERENCES:
	None.

INPUT
ARGUMENTS:
	ptr  = pointer to pointer to FILE that will be reallocated.
	size = number of elements in the array.

OUTPUT
ARGUMENT:
	err = error detection flag.
	    = 0 ( No error )
	    = 1 ( Memory allocation error )

RETURNED
VALUE:
	FILE** = pointer to a pointer to reallocated memory

FUNCTIONS
CALLED:
	fprintf   realloc

VER./DATE/
PROGRAMMER:
	V1.0/07-98/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/

#endif
