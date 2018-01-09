#include"readeph.h"



/* Define file information structure */

struct fileinfo {
	int    num, *mp, *numrec, **order, *curorder, *currec;
	long   **pos;
	double *jdi, *jdf, **jd, **span, *curjd, *curspan, ***coef;
	char   **name;      /* asteroid name */
	FILE   **fp;        /* asteroid ephemeris file */
} astinf;


// err changed to *err in order to return a value by Peter Simpson 27th February 2010
double *readeph( int mp, char *name, double jd, int *err ) {

/* Given an asteroid number and name along with the Julian Date and origin
   produces the Cartesian heliocentric equatorial coordinates of the asteroid
   for the J2000.0 epoch coordinate system from a set of Chebyshev
   polynomials on file. */

	double *result, *poscheb, *velcheb, time, *spos, *svel;
	char   *infile, *head, *fname, hdrinfo[7];
	int    mpnum, fmp, i, j, counter, hdrint;
	long   headlen, namelen;
	short  stmp;

	*err = 0;
	result  = dmalloc(  6 * sizeof( double ), err );
	spos    = dmalloc(  3 * sizeof( double ), err );
	svel    = dmalloc(  3 * sizeof( double ), err );
	poscheb = dmalloc( 14 * sizeof( double ), err );
	velcheb = dmalloc( 14 * sizeof( double ), err );
	infile  = NULL;
	head    = NULL;
	fname   = NULL;
	if( *err != 0 )
		return NULL;

/* If no previous files opened, set up the first member of astinf. */

	if( astinf.num == 0 ) {
		mpnum = 0;
		astinf.num     = 1;
		astinf.mp      = imalloc ( sizeof( int ),
		                           err );
		astinf.name    = cpmalloc( sizeof( char* ),
		                           err );
		astinf.fp      = Fpmalloc( sizeof( FILE* ),
		                           err );
		infile         = cmalloc ( (strlen( name ) + 6) * sizeof( char ),
		                           err );
		astinf.name[0] = cmalloc ( (strlen( name ) + 1) * sizeof( char ),
		                           err );
		if( *err != 0 )
			return NULL;
		strcpy( astinf.name[0], name );
		strcpy( infile,         name );
		strcat( infile,         ".chby" );
		if( ( astinf.fp[0] = fopen( infile, "rb" ) ) == NULL ) {
			fprintf( stderr, "Can not open Chebyshev input file for " );
			fprintf( stderr, "%s.\n", name );
			*err = 4;
			return NULL;
		}

/* Read header information and put into header info table. */

		fread( hdrinfo,           sizeof( char ),   6,       astinf.fp[0] );
		fread( &hdrint,           sizeof( int ),    1,       astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   hdrint,  astinf.fp[0] );
		fread( &headlen,          sizeof( long ),   1,       astinf.fp[0] );
		head = cmalloc( headlen * sizeof( char ), err );
		if( *err != 0 )
			return NULL;
		fread( head,              sizeof( char ),   headlen, astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   6,       astinf.fp[0] );
		fread( &hdrint,           sizeof( int ),    1,       astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   hdrint,  astinf.fp[0] );
		fread( &fmp,              sizeof( int ),    1,       astinf.fp[0] );
		astinf.mp[0] = fmp;
		fread( hdrinfo,           sizeof( char ),   6,       astinf.fp[0] );
		fread( &hdrint,           sizeof( int ),    1,       astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   hdrint,  astinf.fp[0] );
		fread( &namelen,          sizeof( long ),   1,       astinf.fp[0] );
		fname = cmalloc( namelen * sizeof( char ), err );
		if( *err != 0 )
			return NULL;
		fread( fname,    sizeof( char ), namelen, astinf.fp[0] );
		astinf.jdi = dmalloc( sizeof( double ), err );
		astinf.jdf = dmalloc( sizeof( double ), err );
		if( *err != 0 )
			return NULL;
		fread( hdrinfo,           sizeof( char ),   6,       astinf.fp[0] );
		fread( &hdrint,           sizeof( int ),    1,       astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   hdrint,  astinf.fp[0] );
		fread( &astinf.jdi[0],    sizeof( double ), 1,       astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   6,       astinf.fp[0] );
		fread( &hdrint,           sizeof( int ),    1,       astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   hdrint,  astinf.fp[0] );
		fread( &astinf.jdf[0],    sizeof( double ), 1,       astinf.fp[0] );
		astinf.numrec = imalloc( sizeof( int ), err );
		if( *err != 0 )
			return NULL;
		fread( hdrinfo,           sizeof( char ),   6,       astinf.fp[0] );
		fread( &hdrint,           sizeof( int ),    1,       astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   hdrint,  astinf.fp[0] );
		fread( &astinf.numrec[0], sizeof( int ),    1,       astinf.fp[0] );
		fread( hdrinfo,           sizeof( char ),   6,       astinf.fp[0] );

/* Read table of Chebyshev polynomial information. */

		astinf.jd       = dpmalloc( sizeof( double* ),                 err );
		astinf.span     = dpmalloc( sizeof( double* ),                 err );
		astinf.order    = ipmalloc( sizeof( int* ),                    err );
		astinf.jd[0]    = dmalloc ( *astinf.numrec * sizeof( double ), err );
		astinf.span[0]  = dmalloc ( *astinf.numrec * sizeof( double ), err );
		astinf.order[0] = imalloc ( *astinf.numrec * sizeof( int ),    err );
		if( *err != 0 )
			return NULL;
		for( i = 0; i < *astinf.numrec; ++i ){
			fread( &astinf.jd[0][i], sizeof( double ), 1, *astinf.fp );
			fread( &stmp,            sizeof( short ),  1, *astinf.fp );
			astinf.span[0][i]  = (double) stmp;
			fread( &stmp,            sizeof( short ),  1, *astinf.fp );
			astinf.order[0][i] = (int) stmp;
		}

/* Set up memory for current Chebyshev polynomial and read in the first record
   as the default. */

		astinf.currec     = imalloc  ( sizeof( int ),         err );
		astinf.curjd      = dmalloc  ( sizeof( double ),      err );
		astinf.curspan    = dmalloc  ( sizeof( double ),      err );
		astinf.curorder   = imalloc  ( sizeof( int ),         err );
		astinf.coef       = dppmalloc( sizeof( double** ),    err );
		astinf.coef[0]    = dpmalloc ( 3 * sizeof( double* ), err );
		astinf.coef[0][0] = dmalloc  ( 42 * sizeof( double ), err );
		if( *err != 0 )
			return NULL;
		astinf.currec[0]  = 0;
		astinf.coef[0][1] = astinf.coef[0][0] + 14;
		astinf.coef[0][2] = astinf.coef[0][1] + 14;
		readdata( 0 );

	} else {

/* Check file information database to see if this asteroid has been accessed
   before. */

		for( mpnum = 0; mpnum < astinf.num; ++mpnum)
			if( astinf.mp[mpnum] == mp )
				break;

/* If the asteroid has not been accessed, store database information and open
   a new file. */

		if( mpnum == astinf.num ) {
			++astinf.num;
			astinf.mp   = irealloc ( astinf.mp,
			                         astinf.num * sizeof( int ),   err );
			astinf.name = cprealloc( astinf.name,
			                         astinf.num * sizeof( char* ), err );
			astinf.fp   = Fprealloc( astinf.fp,
			                         astinf.num * sizeof( FILE* ), err );
			infile             = cmalloc( (strlen( name ) + 6)
			                              * sizeof( char ), err );
			astinf.name[mpnum] = cmalloc( (strlen( name ) + 1)
			                              * sizeof( char ), err );
			if( *err != 0 )
				return NULL;
			strcpy( astinf.name[mpnum], name );
			strcpy( infile,             name );
			strcat( infile,             ".chby" );
			if( ( astinf.fp[mpnum] = fopen( infile, "rb" ) ) == NULL ) {
				fprintf(stderr, "Can not open Chebyshev input file for " );
				fprintf(stderr, "%s.\n", name );
				*err = 4;
				return NULL;
			}

/* Read header information and put into header info table. */

			fread( hdrinfo,  sizeof( char ), 6,      astinf.fp[mpnum] );
			fread( &hdrint,  sizeof( int ),  1,      astinf.fp[mpnum] );
			fread( hdrinfo,  sizeof( char ), hdrint, astinf.fp[mpnum] );
			fread( &headlen, sizeof( int ),  1,      astinf.fp[mpnum] );
			head = cmalloc( headlen * sizeof( char ), err );
			if( *err != 0 )
				return NULL;
			fread( head,     sizeof( char ), headlen, astinf.fp[mpnum] );
			fread( hdrinfo,  sizeof( char ), 6,       astinf.fp[mpnum] );
			fread( &hdrint,  sizeof( int ),  1,       astinf.fp[mpnum] );
			fread( hdrinfo,  sizeof( char ), hdrint,  astinf.fp[mpnum] );
			fread( &fmp,     sizeof( int ),  1,       astinf.fp[mpnum] );
			astinf.mp[mpnum] = fmp;
			fread( hdrinfo,  sizeof( char ), 6,       astinf.fp[mpnum] );
			fread( &hdrint,  sizeof( int ),  1,       astinf.fp[mpnum] );
			fread( hdrinfo,  sizeof( char ), hdrint,  astinf.fp[mpnum] );
			fread( &namelen, sizeof( int ),  1,       astinf.fp[mpnum] );
			fname = cmalloc( namelen * sizeof( char ), err );
			fread( fname,    sizeof( char ), namelen, astinf.fp[mpnum] );
			astinf.jdi = drealloc( astinf.jdi, astinf.num * sizeof( double ),
			                       err );
			astinf.jdf = drealloc( astinf.jdf, astinf.num * sizeof( double ),
			                       err );
			if( *err != 0 )
				return NULL;
			fread( hdrinfo,               sizeof( char ),   6,
			       astinf.fp[mpnum] );
			fread( &hdrint,               sizeof( int ),    1,
			       astinf.fp[mpnum] );
			fread( hdrinfo,               sizeof( char ),   hdrint,
			       astinf.fp[mpnum] );
			fread( &astinf.jdi[mpnum],    sizeof( double ), 1,
			       astinf.fp[mpnum] );
			fread( hdrinfo,               sizeof( char ),   6,
			       astinf.fp[mpnum] );
			fread( &hdrint,               sizeof( int ),    1,
			       astinf.fp[mpnum] );
			fread( hdrinfo,               sizeof( char ),   hdrint,
			       astinf.fp[mpnum] );
			fread( &astinf.jdf[mpnum],    sizeof( double ), 1,
			       astinf.fp[mpnum] );
			astinf.numrec = irealloc( astinf.numrec,
			                          astinf.num * sizeof( int ), err );
			if( *err != 0 )
				return NULL;
			fread( hdrinfo,               sizeof( char ),   6,
			       astinf.fp[mpnum] );
			fread( &hdrint,               sizeof( int ),    1,
			       astinf.fp[mpnum] );
			fread( hdrinfo,               sizeof( char ),   hdrint,
			       astinf.fp[mpnum] );
			fread( &astinf.numrec[mpnum], sizeof( int ),    1,
			       astinf.fp[mpnum] );
			fread( hdrinfo,  sizeof( char ), 6,      astinf.fp[mpnum] );

/* Read table of Chebyshev polynomial information. */

			astinf.jd           = dprealloc( astinf.jd,
			                                 astinf.num * sizeof( double* ),
			                                 err );
			astinf.span         = dprealloc( astinf.span,
			                                 astinf.num * sizeof( double* ),
			                                 err );
			astinf.order        = iprealloc( astinf.order,
			                                 astinf.num * sizeof( int* ),
			                                 err );
			astinf.jd[mpnum]    = dmalloc  ( astinf.numrec[mpnum]
			                                 * sizeof( double ), err );
			astinf.span[mpnum]  = dmalloc  ( astinf.numrec[mpnum] 
			                                 * sizeof( double ), err );
			astinf.order[mpnum] = imalloc  ( astinf.numrec[mpnum]
			                                 * sizeof( int ),    err );
			if( *err != 0 )
				return NULL;
			for( i = 0; i < astinf.numrec[mpnum]; ++i ){
				fread( &astinf.jd[mpnum][i], sizeof( double ), 1,
				       astinf.fp[mpnum] );
				fread( &stmp,                sizeof( short ),  1,
				       astinf.fp[mpnum] );
				astinf.span[mpnum][i] = (double) stmp;
				fread( &stmp,                sizeof( short ),  1,
				       astinf.fp[mpnum] );
				astinf.order[mpnum][i] = (int) stmp;
			}

/* Reallocate memory for current Chebyshev polynomial and read in the first
   record as the default. */

			astinf.currec   = irealloc  ( astinf.currec,
			                              astinf.num * sizeof( int ),
			                              err );
			astinf.curjd    = drealloc  ( astinf.curjd,
			                              astinf.num * sizeof( double ),
			                              err );
			astinf.curspan  = drealloc  ( astinf.curspan,
			                              astinf.num * sizeof( double ),
			                              err );
			astinf.curorder = irealloc  ( astinf.curorder,
			                              astinf.num * sizeof( int ),
			                              err );
			astinf.coef     = dpprealloc( astinf.coef,
			                              astinf.num * sizeof( double** ),
			                              err );
			astinf.coef[mpnum]    = dpmalloc(  3 * sizeof( double* ), err );
			astinf.coef[mpnum][0] = dmalloc ( 42 * sizeof( double ),  err );
			if( *err != 0 )
				return NULL;
			astinf.currec[mpnum]  = 0;
			astinf.coef[mpnum][1] = astinf.coef[mpnum][0] + 14;
			astinf.coef[mpnum][2] = astinf.coef[mpnum][1] + 14;
			readdata( mpnum );
		}
	}

/* Check for error conditions in file information. */

/* Error in asteroid number. */

	if( astinf.mp[mpnum] != mp ) {
		fprintf( stderr, "ERROR: The given asteroid number for " );
		fprintf( stderr, "%s does not match the number on file!\n", name );
		fprintf( stderr, "The file number is %i and the ", astinf.mp[mpnum] );
		fprintf( stderr, "given number is %i.\n ", mp );
		*err = 2;
		return NULL;
	}

/* Error in asteroid name. */

	if( strcmp( astinf.name[mpnum], name ) ) {
		fprintf( stderr, "ERROR: The given name for asteroid number " );
		fprintf( stderr, "%i does not match the name on file!\nThe", mp );
		fprintf( stderr, "file name is %s and the ", astinf.name[mpnum] );
		fprintf( stderr, "given name is %s.\n(C)ontinue with file ", name );
		fprintf( stderr, "name or (A)bort?\n" );
		*err = 2;
		return NULL;
	}

/* Julian date for computation is out of bounds. */

	if( jd < astinf.jdi[mpnum] ) {
		fprintf( stderr, "JD = %9.1f is before the first ephemeris ", jd );
		fprintf( stderr, "date of JD = %9.1f in the", astinf.jdi[mpnum] );
		fprintf( stderr, "\nfile for %i ", astinf.mp[mpnum] );
		fprintf( stderr, "%s!\n", astinf.name[mpnum] );
		*err = 3;
		return NULL;
	} else if( jd > astinf.jdf[mpnum] ) {
		fprintf( stderr, "JD = %9.1f is after the first ephemeris ", jd );
		fprintf( stderr, "date of JD = %9.1f in the\n", astinf.jdi[mpnum] );
		fprintf( stderr, "file for %i", astinf.mp[mpnum] );
		fprintf( stderr,  "%s!\n", astinf.name[mpnum] );
		*err = 3;
		return NULL;
	}

/* Search for and read the correct set of Chebyshev polynomials.  If the
   current record is not the right one, find the record to go to. */

	counter = 0;
	if( jd < astinf.curjd[mpnum] ) {
		i = 0;
		while( astinf.jd[mpnum][i] + astinf.span[mpnum][i] < jd )
			++i;
		astinf.currec[mpnum] = i;
		while( astinf.jd[mpnum][i] <= astinf.curjd[mpnum] ) {
			counter -= ( sizeof( double ) + 2 * sizeof( short ) + 3 * (astinf.order[mpnum][i] + 1) * sizeof( double ) );
			++i;
		}
	} else if( jd > astinf.curjd[mpnum] + astinf.curspan[mpnum] ) {
		i = astinf.currec[mpnum] + 1;
		while( astinf.jd[mpnum][i] + astinf.span[mpnum][i] < jd ) {
			counter += ( sizeof( double ) + 2 * sizeof( short ) + 3 * (astinf.order[mpnum][i] + 1) * sizeof( double ) );
			++i;
		}
		astinf.currec[mpnum] = i;
	}

/* Seek out the correct data and read the data. */

	if( ( jd <  astinf.curjd[mpnum] ) ||
	    ( jd > astinf.curjd[mpnum] + astinf.curspan[mpnum] ) ) {
		fseek( astinf.fp[mpnum], counter, SEEK_CUR );
		readdata( mpnum );
	}

/* Convert the date to -1 to +1 over the specified interval. */

	time = (jd - astinf.curjd[mpnum]) * 2 / astinf.curspan[mpnum] - 1;

/* Calculate the Chebyshev polynomials for the time. */

	poscheb = maket   ( time );
	velcheb = maketdot( time );

/* Compute position and velocity for asteroid and return the result. */

	for( i = 0; i < 3; ++i ) {
		result[i]     = 0;
		result[i + 3] = 0;
		for( j = 0; j <= astinf.curorder[mpnum]; ++j ) {
			result[i]     += poscheb[j] * astinf.coef[mpnum][i][j];
			result[i + 3] += velcheb[j] * astinf.coef[mpnum][i][j];
		}
	}

/* Reconvert time into days for the velocities. */

	for( i = 3; i < 6; ++i )
		result[i] *= (2 / astinf.curspan[mpnum]);

/* Free up pointers. */

	free( spos );
	free( svel );
	free( poscheb );
	free( velcheb );
	if( infile != NULL )
		free( infile );
	if( head != NULL )
		free( head );
	if( fname != NULL )
		free( fname );
	
	return result;
}



void initeph( void ) {

/* Initialize ephemeris file structure to 0. */

	astinf.num = 0;
}



void cleaneph( void ) {
	int i;

/* Clean up ephemeris file structure */

	for( i = 0; i < astinf.num; ++i )
		fclose( *(astinf.fp + i) );

/* Release memory */

	free( astinf.coef[0][0] );
	free( astinf.coef[0] );
	free( astinf.order[0] );
	free( astinf.jd[0] );
	free( astinf.span[0] );
	free( astinf.name[0] );
	free( astinf.mp );
	free( astinf.numrec );
	free( astinf.curorder );
	free( astinf.currec );
	free( astinf.jdi );
	free( astinf.jdf );
	free( astinf.curjd );
	free( astinf.curspan );
	free( astinf.order );
	free( astinf.jd );
	free( astinf.span );
	free( astinf.name );
	free( astinf.fp );
	free( astinf.coef );

}



void   readdata ( int num ) {

/* Reads a data record from the pre-positioned file to get the Chebyshev
	polynomial series for a given group of dates. */

	int   i, j;
	short stmp;

	fread( &astinf.curjd[num], sizeof( double ), 1, astinf.fp[num] );
	fread( &stmp,              sizeof( short ),  1, astinf.fp[num] );
	astinf.curspan[num] = (double) stmp;
	fread( &stmp, sizeof( short ),  1, astinf.fp[num] );
	astinf.curorder[num] = (int) stmp;
	for( j = 0; j < 3; ++j )
		for( i = 0; i <= astinf.curorder[num]; ++i )
			fread( &astinf.coef[num][j][i], sizeof( double ),  1,
			       astinf.fp[num] );
}

