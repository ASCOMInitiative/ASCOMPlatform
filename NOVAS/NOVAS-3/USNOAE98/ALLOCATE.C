#include<stdlib.h>
#include<stdio.h>
#include"allocate.h"



double *dmalloc( size_t size, int err ) {

/* Allocate memory for a pointer to double and do error checking. */

	double *tmp;
	err = 0;

	tmp = (double*) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO double!\n" );
		err = 1 ;
	}

	return tmp;
}



double **dpmalloc( size_t size, int err ){

/* Allocate memory for a pointer to a pointer to double and do error
   checking. */

	double **tmp;
	err = 0;

	tmp = (double**) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO double!\n" );
		err = 1 ;
	}

	return tmp;
}



double ***dppmalloc( size_t size, int err ){

/* Allocate memory for a pointer to a pointer to a pointer to double and do
   error checking. */

	double ***tmp;
	err = 0;

	tmp = (double***) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO POINTER TO double!\n" );
		err = 1 ;
	}

	return tmp;
}



char *cmalloc( size_t size, int err ){

/* Allocate memory for a pointer to char and do error checking. */

	char *tmp;
	err = 0;

	tmp = (char*) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO char!\n" );
		err = 1 ;
	}

	return tmp;
}



char **cpmalloc( size_t size, int err ){

/* Allocate memory for a pointer to a pointer to char and do error
   checking. */

	char **tmp;
	err = 0;

	tmp = (char**) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO char!\n" );
		err = 1 ;
	}

	return tmp;
}



int *imalloc( size_t size, int err ){

/* Allocate memory for a pointer to int and do error checking. */

	int *tmp;
	err = 0;

	tmp = (int*) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO int!\n" );
		err = 1 ;
	}

	return tmp;
}



int **ipmalloc( size_t size, int err ){

/* Allocate memory for a pointer to a pointer to int and do error checking. */

	int **tmp;
	err = 0;

	tmp = (int**) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO int!\n" );
		err = 1 ;
	}

	return tmp;
}



long *lmalloc( size_t size, int err ){

/* Allocate memory for a pointer to long and do error checking. */

	long *tmp;
	err = 0;

	tmp = (long*) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO long!\n" );
		err = 1 ;
	}

	return tmp;
}



long **lpmalloc( size_t size, int err ){

/* Allocate memory for a pointer to a pointer to long and do error
   checking. */

	long **tmp;
	err = 0;

	tmp = (long**) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO long!\n" );
		err = 1 ;
	}

	return tmp;
}



FILE *Fmalloc( size_t size, int err ){

/* Allocate memory for a pointer to FILE and do error checking. */

	FILE *tmp;
	err = 0;

	tmp = (FILE*) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO FILE!\n" );
		err = 1 ;
	}

	return tmp;
}



FILE **Fpmalloc( size_t size, int err ){

/* Allocate memory for a pointer to a pointer to FILE and do error
   checking. */

	FILE **tmp;
	err = 0;

	tmp = (FILE**) malloc( size );
	if( tmp == NULL ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO FILE!\n" );
		err = 1 ;
	}

	return tmp;
}



double *drealloc( double *ptr, size_t size, int err ){

/* Reallocate memory for a pointer to double and do error checking. */

	err = 0;
	ptr = (double*) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO double!\n" );
		err = 1 ;
	}
	return ptr;
}



double **dprealloc( double **ptr, size_t size, int err ){

/* Reallocate memory for a pointer to a pointer to double and do error
   checking. */

	err = 0;
	ptr = (double**) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO double!\n" );
		err = 1 ;
	}
	return ptr;
}



double ***dpprealloc( double ***ptr, size_t size, int err ){

/* Reallocate memory for a pointer to a pointer to a pointer to double and do
   error checking. */

	err = 0;
	ptr = (double***) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO double!\n" );
		err = 1 ;
	}
	return ptr;
}



char *crealloc( char *ptr, size_t size, int err ){

/* Reallocate memory for a pointer to char and do error checking. */

	err = 0;
	ptr = (char*) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO char!\n" );
		err = 1 ;
	}
	return ptr;
}



char **cprealloc( char **ptr, size_t size, int err ){

/* Reallocate memory for a pointer to a pointer to char and do error
   checking. */

	err = 0;
	ptr = (char**) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO char!\n" );
		err = 1 ;
	}
	return ptr;
}



int *irealloc( int *ptr, size_t size, int err ){

/* Reallocate memory for a pointer to int and do error checking. */

	err = 0;
	ptr = (int*) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO int!\n" );
		err = 1 ;
	}
	return ptr;
}



int **iprealloc( int **ptr, size_t size, int err ){

/* Reallocate memory for a pointer to a pointer to int and do error
   checking. */

	err = 0;
	ptr = (int**) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO int!\n" );
		err = 1 ;
	}
	return ptr;
}



long *lrealloc( long *ptr, size_t size, int err ){

/* Reallocate memory for a pointer to long and do error checking. */

	err = 0;
	ptr = (long*) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO long!\n" );
		err = 1 ;
	}
	return ptr;
}



long **lprealloc( long **ptr, size_t size, int err ){

/* Reallocate memory for a pointer to a pointer to long and do error
   checking. */

	err = 0;
	ptr = (long**) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO long!\n" );
		err = 1 ;
	}
	return ptr;
}



FILE *Frealloc( FILE *ptr, size_t size, int err ){

/* Reallocate memory for a pointer to FILE and do error checking. */

	err = 0;
	ptr = (FILE*) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO FILE!\n" );
		err = 1 ;
	}
	return ptr;
}



FILE **Fprealloc( FILE **ptr, size_t size, int err ){

/* Reallocate memory for a pointer to a pointer to FILE and do error
   checking. */

	err = 0;
	ptr = (FILE**) realloc( ptr, size );
	if( ( ptr == NULL ) && ( size != 0 ) ) {
		fprintf( stderr, "MEMORY ALLOCATION ERROR IN POINTER TO POINTER " );
		fprintf( stderr, "TO FILE!\n" );
		err = 1 ;
	}

	return ptr;
}

