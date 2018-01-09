#include<math.h>
#include"readeph.h"

void main()
/*----------------------------------------------------------------------------
PURPOSE:
	This program reads the Chebyshev polynomial ephemeris and compares the
	output to a test file.

REFERENCES:
	Hilton, J. L. 1998, "U.S. Naval Observatory Ephemerides of the Largest Asteroids," (submitted to Astron. J.)

INPUT
ARGUMENTS:
	None.

INPUT
PROMPT:
	Asteroid name.

INPUT
FILES:
	Asteroid Chebyshev polynomial ephemeris file, '.chby' extension.
	Asteroid test comparison file, '.ephem.test' extension.

STANDARD
OUTPUT:
	Table showing the difference in position in AU and velocity in AU/day
	between the Chebyshev polynomial and the test data.

VER./DATE/
PROGRAMMER:
	V1.0/12-95/JLH (USNO/AA)
	V1.1/04-97/JLH (USNO/AA)

NOTES:
	None.
----------------------------------------------------------------------------*/
{
	char   *name, *buf, *testfile, ch;
	double *posvel, jd, *testpos, maxdist, maxspeed, dist, speed, dif;
	int    mp, i, err = 0;
	FILE   *fp;

	maxdist  = 0;
	maxspeed = 0;
	i        = 0;

/* Allocate memory for arrays. */

	buf     = cmalloc( 81 * sizeof( char ), err );
	posvel  = dmalloc( 6 * sizeof( double ), err );
	testpos = dmalloc( 6 * sizeof( double ), err );

/* Get asteroid number. */

	printf( "Asteroid Number? " );
	scanf ( "%i", &mp );

/* Throw away terminating line feed and get asteroid name. */

	getchar();
	printf( "Asteroid Name?   " );
	ch = (char) getchar();
	while( ch != '\n' ) {
		buf[i] = ch;
		++i;
		ch = (char) getchar();
	}
	buf[i] = '\0';
	name   = cmalloc( (1 + strlen( buf )) * sizeof( char ), err );
	strcpy( name, buf );

/* Open the file containing the test data. */

	testfile = cmalloc( (12 + strlen( name )) * sizeof( char ), err );
	strcpy( testfile, name );
	strcat( testfile, ".tst" );
	if( ( fp = fopen( testfile, "rt" ) ) == NULL ) {
		printf( " Can not open test data file.\n" );
		exit( 1 );
	}



/* Initialize the number of asteroids in the file information database. */

	initeph();

	while( !feof( fp ) ) {

/* Read Julian date, positions, and velocities, from test data */

		fscanf( fp, "%s", buf );
		if ( !feof( fp ) ) {
			jd = 2400000 + atof( buf );
			for( i = 0; i < 6; ++i ) {
				fscanf( fp, "%s", buf );
				testpos[i] = atof( buf );
			}

/* Get the position and date from the ephemeris file. */

			posvel = readeph( mp, name, jd, err );

/* Compare the test and ephemeris positions. */

			speed = 0;
			dist  = 0;
			for( i = 0; i < 6; ++i ) {
				dif = posvel[i] - testpos[i];
				if( i < 3 )
					dist  += dif * dif;
				else
					speed += dif * dif;
			}

			dist  = sqrt( dist );
			speed = sqrt( speed );
			if( speed > maxspeed )
				maxspeed = speed;
			if( dist > maxdist )
				maxdist  = dist;

			printf( "JD = %9.1f Delta Distance = %15.12f ", jd, dist );
			printf( "Delta Speed = %15.12f\n",              speed );
		}
	}

/* Print the maximum differences between the test and the ephemeris files. */

	printf( "\n\nMaximum Delta Distance = %15.12f ", maxdist );
	printf( "Maximum Delta Speed = %15.12f\n",       maxspeed );

/* Clean up and stop the program. */

	fclose( fp );
	cleaneph();
	free( buf );
	free( posvel );
	free( testpos );
	free( testfile );
	free( name );
}
