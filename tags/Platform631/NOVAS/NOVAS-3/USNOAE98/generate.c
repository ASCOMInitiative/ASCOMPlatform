/* This function is designed to compute an export ephemeris for an asteroid
   using the method described in Newhall, X X 1989, Celest. Mech., 45, 305.
   It requires a minimum kernel size of 32 days and computes the coefficients
   to find the Chebyshev polynomial that fits the data data in the maximum
   number of days with the minimum necessary order of Chebyshev polynomial. */

#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<math.h>
#include"generate.h"
#include"chby.h"
#include"allocate.h"
#define SWAP(a,b) {temp=(a);(a)=(b);(b)=temp;}



void   generate( int *mp, char* name, double *tol, char *head, int err ) {

/* This is the function which generates the Chebyshev polynomial rendering of
   an asteroid ephemeris.  The input parameters are:

   mp   = The number of the asteroid for which the export ephemeris is being
          calculated.
   name = The name of the asteroid for which the export ephemeris is being
          calculated.
   tol  = The tolerance to which the export ephemeris is to be calculated.
   head = A string of header information.

   The function assumes that the input ephemeris file is named, "name".eph.
   The input ephemeris is assumed to be in tabular format (JD - 240 0000), X,
   Y, Z, Vx, Vy, Vz with a tabulated interval of 1/2 day.  The distances are
   assumed to be in AU and the velocity components in AU/day.  The output file
   is named "name.chby" and is binary form. */

	FILE   *fp = NULL, *fout = NULL, *ftmp1 = NULL, *ftmp2 = NULL;
	double **t, **tdot, time, tw[14][18], twt[14][14], **c1, **c2, **c1c2,
			 jd[9], **pv, **pv1, f, fdot, **coef, pos[3], vel[3], jdt,
	       *t_test, *td_test, testd, *sigma, test, len,
	       jdinit, jdfinal;
	static double bigbuf[513][7];
	int    k, j, i, l, biglen, order, numrec, startord;
	char   buf[21], infile[29], tmpfile1[30], tmpfile2[30], outfile[30], hdrinfo[7];
	long   headlen, namelen;
	short  slen, sorder;

	biglen = 513;
	numrec = 0;
	err    = 0;

/* Setup file names. */

	strcpy( infile, name );
	strcat( infile, ".eph" );
	strcpy( tmpfile1, name );
	strcat( tmpfile1, ".tmp1" );
	strcpy( tmpfile2, name );
	strcat( tmpfile2, ".tmp2" );
	strcpy( outfile, name );
	strcat( outfile, ".chby" );

	t       = dpmalloc( 9 * sizeof( double* ), err );
	t[0]    = dmalloc( 126 * sizeof( double ), err );
	tdot    = dpmalloc( 9 * sizeof( double* ), err );
	tdot[0] = dmalloc( 126 * sizeof( double ), err );
	c1      = dpmalloc( 18 * sizeof( double* ), err );
	c1[0]   = dmalloc( 324 * sizeof( double ), err );
	c2      = dpmalloc( 18 * sizeof( double* ), err );
	c2[0]   = dmalloc( 324 * sizeof( double ), err );
	c1c2    = dpmalloc( 18 * sizeof( double* ), err );
	c1c2[0] = dmalloc( 324 * sizeof( double ), err );
	pv      = dpmalloc( 18 * sizeof( double* ), err );
	pv[0]   = dmalloc(  54 * sizeof( double ), err );
	pv1     = dpmalloc( 18 * sizeof( double* ), err );
	pv1[0]  = dmalloc(  18 * sizeof( double ), err );
	coef    = dpmalloc(  3 * sizeof( double* ), err );
	coef[0] = dmalloc(  42 * sizeof( double ), err );
	t_test  = dmalloc( 14 * sizeof( double ), err );
	td_test = dmalloc( 14 * sizeof( double ), err );
	sigma   = dmalloc(  6 * sizeof( double ), err );

	for( j = 1; j < 18; ++j ) {
		c1[j]   = c1[j - 1] + 18;
		c2[j]   = c2[j - 1] + 18;
		c1c2[j] = c1c2[j - 1] + 18;
		pv[j]   = pv[j - 1] + 3;
		pv1[j]  = pv1[j - 1] + 1;
	}
	for( j = 1; j < 3; ++j )
		coef[j] = coef[j - 1] + 14;
	for( j = 1; j < 9; ++j ) {
		t[j]    = t[j - 1] + 14;
		tdot[j] = tdot[j - 1] + 14;
	}

/* Compute the values of a thirteenth order Chebyshev polynomial and its
   derivative at nine equally spaced points along the [-1, 1].  This is the
   matrix T.           */

	i = 0;
	for( time = 1; time >= -1; time -= 0.25 ) {
		*(t    + i) = maket   ( time );
		*(tdot + i) = maketdot( time );
		++i;
	}

/* Compute the matrix T*W where T* is the transpose of T and W = (1.0, 0.16,
   1.0, 0.16, ...), the diagonal weight matrix. */

	for( j = 0; j < 9; ++j )
		for( i = 0; i < 14; ++i) {
			tw[i][j * 2]     = t[j][i];
			tw[i][j * 2 + 1] = tdot[j][i] * 0.16;
		}

/* Compute the matrix T*WT */

	for( i = 0; i < 14; ++i )
		for( j = 0; j < 14; ++j ) {
			twt[i][j] = 0;
			for(k = 0; k < 9; ++k)
				twt[i][j] += tw[i][k * 2    ] * *(*(t    + k) + j) +
				             tw[i][k * 2 + 1] * *(*(tdot + k) + j);
		}

/* Augment the matrix T*W to get the matrix C2 */

	for( i = 0; i < 18; ++i )
		for( j = 0; j < 18; ++j )
			if( i < 14 )
				c2[i][j] = tw[i][j];
			else
				c2[i][j] = 0;
	c2[14][0]  = 1;
	c2[15][1]  = 1;
	c2[16][16] = 1;
	c2[17][17] = 1;

/* Augment the matrix T*WT to get the matrix C1. */

	for( i = 0; i < 18; ++i )
		for( j = 0; j < 18; ++j )
			if( ( i < 14 ) && ( j < 14 ) )
				c1[i][j] = twt[i][j];
			else if( ( i >= 14 ) && ( j < 14 ) )
				switch (i) {
					case 14 : c1[i][j] = t[0][j]; break;
					case 15 : c1[i][j] = tdot[0][j]; break;
					case 16 : c1[i][j] = t[8][j]; break;
					case 17 : c1[i][j] = tdot[8][j]; break;
				}
			else if( ( i < 14 ) && ( j >= 14 ) )
				switch (j) {
					case 14 : c1[i][j] = t[0][i]; break;
					case 15 : c1[i][j] = tdot[0][i]; break;
					case 16 : c1[i][j] = t[8][i]; break;
					case 17 : c1[i][j] = tdot[8][i]; break;
				}
			else
				c1[i][j] = 0;

/* Invert c1 using gaussj from Numerical Recipes. */

	gaussj( c1, 18, err );

/* Multiply c1 inverse by c2. */

	times( c1, 18, 18, 18, c2, c1c2 );

/* Open temporary output files. */

	if( ( ftmp1 = fopen( tmpfile1, "wb+" ) ) == NULL ) {
		fprintf( stderr, " Can not open temporary output file 1.\n" );
		err = 2;
		return;
	}

	if( ( ftmp2 = fopen( tmpfile2, "wb+" ) ) == NULL ) {
		fprintf( stderr, " Can not open temporary output file 2.\n" );
		err = 2;
		return;
	}

/* Open file and read in Julian dates, positions and velocities to determine
   Chebyshev polynomials. */

	if( ( fp = fopen( infile, "r" ) ) == NULL ) {
		fprintf( stderr, " Can not open input file.\n" );
		err = 3;
		return;
	}

/* Read large buffer of positions and velocities at 2 day intervals. */

	for( i = 0; i < 513; ++i ) {
		if( feof( fp ) ) {
			biglen = i - 1;
			break;
		}
		for( j = 0; j < 7; ++j ) {
			fscanf( fp, "%s", buf );
			bigbuf[i][j] = atof( buf );
		}
		fprintf( stderr, " BigBuf %f %f %f %f %f %f %f \n", bigbuf[i][0], bigbuf[i][1], bigbuf[i][2], bigbuf[i][3], bigbuf[i][4], bigbuf[i][5], bigbuf[i][6] );
	}
	fprintf( stderr, " BigLen %d \n", biglen);

/* Set the initial Julian date. */

	jdinit = bigbuf[0][0] + 2400000;

/* Indicator to show that the program is not hung up. */

	printf( "Working " );
	fflush( stdout );

/* Start main loop to read tabular ephemeris data to create export
   ephemeris, and increment the number of records. */

	while ( biglen >= 9 ) {

		++ numrec;

/* Indicator to show that the program is not hung up. */

		if( ( numrec % 5 ) == 0 ) {
			printf( "." );
			fflush( stdout );
		}

/* Start main loop looking for best Chebyshev polynomial to represent a section
   of orbit.  Search is made by first increasing the order of the polynomial
   from 5 up to 13, and then decreasing the interval from 128 to 4 days in
   factors of 2.  Search continues until all of the standard deviations are
   less than tol. */

		for( len = 1024; len > 8; len /= 2 ) {

			l = (int) (len / 16);
			for( i = 8; i >= 0; --i ) {
				jd[i] = bigbuf[(8 - i) * l][0];
				for( j = 0; j < 3; ++j ) {
					pv[i * 2][j]     = bigbuf[(8 - i) * l][j + 1];
					pv[i * 2 + 1][j] = bigbuf[(8 - i) * l][j + 4] *
					                   len / 2;
				}
			}

/* Solve for Chebyshev coefficients. */

			for( i = 0; i < 3; ++i )
				for( j = 0; j < 14; ++j ) {
					coef[i][j] = 0;
					for( k = 0; k < 18; ++k )
						coef[i][j] += pv[k][i] * c1c2[j][k];
				}



/* Test Chebyshev Polynomial to determine how good it is. */

			startord = 0;

			for( i = 0; i < 3; ++i ) {
				if( fabs( coef[i][13] ) > * tol ) {
					startord = 0;
					break;
				}
				for( j = 5; j < 14; ++j )
					if( fabs( coef[i][j] ) < *tol ) {
						if( startord < j )
							startord = j;
						break;
					}
			}

			if( ( startord > 0 ) || ( len == 16 ) ) {
				for( order = startord; order < 14; ++ order ) {

/* Set initial values for standard deviations to 0. */

					for( i = 0; i < 6; ++i )
						sigma[i] = 0;

/* Calculate the standard deviations in position and velocity from the 512
   positions and velocities read in. */

					for( l = 0; l <= len / 2; ++l ) {
						jdt = bigbuf[l][0];
						for( i = 0; i < 3; ++i ) {
							pos[i] = bigbuf[l][i + 1];
							vel[i] = bigbuf[l][i + 4];
						}

/* Compute time of test data in system of Chebyshev polynomial. */

						time = ( jdt - jd[8] ) * 2 / len - 1;
						t_test  = maket   ( time );
						td_test = maketdot( time );

/* Compute position and velocity from Chebyshev polynomial. */


						for( i = 0; i < 3; ++i ) {
							f    = 0;
							fdot = 0;
							for( k = 0; k < order + 1; ++k ) {
								f    += coef[i][k] * t_test[k];
								fdot += coef[i][k] * td_test[k] * 2 / len;
							}

/* Add the square of the difference between the ephemeris and computed position
   and velocity component to sigma. */

							sigma[i]     += pwr( pos[i] - f, 2 );
							sigma[i + 3] += pwr( vel[i] - fdot, 2 );
						}
						free( t_test );
						free( td_test );
					}

/* Compute the standard deviations for each position and velocity
   coordinate. */

					for( i = 0; i < 6; ++i ) {
						sigma[i] = sqrt( sigma[i] / (2 * len - 1) );
					}

/* Find greatest member of the standard deviations and test for convergence
   of Chebyshev polynomials. */

					test = maximum( sigma, 6 );

/* If the maximum standard deviation is less than the convergence tolerance
	end the loop.  If the maximum number of iterations has been reached end
	the loop and print a warning message. */

					if( ( test < *tol ) || ( ( ( len - 16 ) < 1e-6 ) &&
					    ( order == 13 ) ) )
						break;
				}
				if( test < *tol )
					break;
				else if( ( ( len - 16 ) < 1e-6 ) && ( order == 13 ) ) {
					fprintf( stderr, "The segment beginning at JD %f did not converge.\n",
			    		    jd[8] + 2400000 );
					fprintf( stderr, "The convergence tolerance was %e and the ", *tol );
					fprintf( stderr, "maximum\nstandard deviation was %e.\n", test );
					err = 4;
					break;
				}
			}
		}

/* Add missing 240 0000 to initial Julian Date from PEP output, and set present
   final value for the covered segment. */

		jdt     = jd[8] + 2400000;
		jdfinal = jdt + len;

/* Write initial Julian date, kernel length, and order of Chebyshev polynomial
   to temporary output files. */

		slen   = (short) len;
		sorder = (short) order;
		fwrite( &jdt,    sizeof( double ), 1, ftmp1 );
		fwrite( &slen,   sizeof( short  ), 1, ftmp1 );
		fwrite( &sorder, sizeof( short  ), 1, ftmp1 );
		fwrite( &jdt,    sizeof( double ), 1, ftmp2 );
		fwrite( &slen,   sizeof( short  ), 1, ftmp2 );
		fwrite( &sorder, sizeof( short  ), 1, ftmp2 );
		fflush( ftmp1 );
		fflush( ftmp2 );


/* Write coefficients of Chebyshev polynomial to temporary output file 1. */

		for( i = 0; i < 3; ++i )
			for( j = 0; j < order + 1; ++j )
				fwrite( &coef[i][j], sizeof( double ), 1, ftmp1 );

/* Move members of bigbuf up len days and fill in the end of bigbuf with
   new positions and velocities. */

		k = (int) len / 2;
		for( i = k; i < biglen; ++i )
			for( j = 0; j < 7; ++j )
				bigbuf[i - k][j] = bigbuf[i][j];
		if( feof ( fp ) ) {
			biglen -= k;
		}
		if( !feof( fp ) )
			for( i = 513 - k; i < 513; ++i ) {
				if( feof ( fp ) ) {
					biglen = i - 1;
					break;
				}
				for( j = 0; j < 7; ++j ) {
					fscanf( fp, "%s", buf );
					bigbuf[i][j] = atof( buf );
				}
			}
		fflush( ftmp1 );
	}

/* Close input file and rewind temporary output files before constructing the
   final Chebyshev polynomial file. */

 	fclose( fp );
	fseek( ftmp1, 0, SEEK_SET );
	fseek( ftmp2, 0, SEEK_SET );

/* Open output file. */

	if( ( fout = fopen( outfile, "wb+" ) ) == NULL ) {
		fprintf( stderr, " Can not open output file 1.\n" );
		err = 5;
		return;
	}

/* Find length of header and name strings. */

	headlen = strlen( head ) + 1;
	namelen = strlen( name ) + 1;

/* Print length of header, header string, asteroid number, length of asteroid
   name, asteroid name, initial JD, final JD, and number of records to output
   file. */

	i = 6;
	strcpy( hdrinfo, "STRING" );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	strcpy( hdrinfo, "Header" );
	fwrite( &i,       sizeof( int ),    1,       fout );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	fwrite( &headlen, sizeof( long ),   1,       fout );
	fwrite( head,     sizeof( char ),   headlen, fout );
	strcpy( hdrinfo, "_INT__" );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	strcpy( hdrinfo, "MP_Num" );
	fwrite( &i,       sizeof( int ),    1,       fout );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	fwrite( mp,       sizeof( int ),    1,       fout );
	strcpy( hdrinfo, "STRING" );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	strcpy( hdrinfo, "MPName" );
	fwrite( &i,       sizeof( int ),    1,       fout );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	fwrite( &namelen, sizeof( long ),    1,       fout );
	fwrite( name,     sizeof( char ),   namelen, fout );
	strcpy( hdrinfo, "DOUBLE" );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	strcpy( hdrinfo, "InitJD" );
	fwrite( &i,       sizeof( int ),    1,       fout );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	fwrite( &jdinit,  sizeof( double ), 1,       fout );
	strcpy( hdrinfo, "DOUBLE" );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	strcpy( hdrinfo, "FinlJD" );
	fwrite( &i,       sizeof( int ),    1,       fout );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	fwrite( &jdfinal, sizeof( double ), 1,       fout );
	strcpy( hdrinfo, "_INT__" );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	strcpy( hdrinfo, "NumRec" );
	fwrite( &i,       sizeof( int ),    1,       fout );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );
	fwrite( &numrec,  sizeof( int ),    1,       fout );
	strcpy( hdrinfo, "_END__" );
	fwrite( hdrinfo,  sizeof( char ),   6,       fout );

/* Go through temporary output files and write data to main output file. */

	for( i = 0; i < numrec; ++i ) {

/* Indicator to show that the program is not hung up. */

		if( ( numrec % 500 ) == 0 ) {
			printf( "." );
			fflush( stdout );
		}

		fread ( &jdt,    sizeof( double ), 1, ftmp2 );
		fwrite( &jdt,    sizeof( double ), 1, fout );
		fread ( &slen,   sizeof( short ),  1, ftmp2 );
		fwrite( &slen,   sizeof( short ),  1, fout );
		fread ( &sorder, sizeof( short ),  1, ftmp2 );
		fwrite( &sorder, sizeof( short ),  1, fout );
		fflush( fout );
	}

	for( i = 0; i < numrec; ++i ) {

/* Indicator to show that the program is not hung up. */

		if( ( numrec % 500 ) == 0 ) {
			printf( "." );
			fflush( stdout );
		}

		fread ( &jdt,    sizeof( double ), 1, ftmp1 );
		fwrite( &jdt,    sizeof( double ), 1, fout );
		fread ( &slen,   sizeof( short ),  1, ftmp1 );
		fwrite( &slen,   sizeof( short ),  1, fout );
		fread ( &sorder, sizeof( short ),  1, ftmp1 );
		fwrite( &sorder, sizeof( short ),  1, fout );
		order = (int) sorder;
		for( k = 0; k < 3; ++k )
			for( j = 0; j < order + 1; ++j ) {
				fread ( &testd, sizeof( double ), 1, ftmp1 );
				fwrite( &testd, sizeof( double ), 1, fout );
			}
		fflush( fout );
	}

/* Close all files and remove temporary files. */

	fclose( ftmp1 );
	fclose( ftmp2 );
	fclose( fout );
	remove( tmpfile1 );
	remove( tmpfile2 );

/* Free memory. */
	free( t[0] );
	free( tdot[0] );
	free( c1[0] );
	free( c2[0] );
	free( c1c2[0] );
	free( pv[0] );
	free( pv1[0] );
	free( coef[0] );
	free( t );
	free( tdot );
	free( c1 );
	free( c2 );
	free( c1c2 );
	free( pv );
	free( pv1 );
	free( coef );
	free( sigma );

/* Indicator to show that the program is not hung up. */

		printf( "\nSuccessful completion of Chebyshev polynomial generation\n" );
}



void gaussj(double **a, int n, int err)
{
	int *indxc,*indxr,*ipiv;
	int i,icol,irow,j,k,l,ll;
	double big,dum,pivinv,temp;

	err = 0;

	indxc= imalloc( n * sizeof( int ), err );
	indxr= imalloc( n * sizeof( int ), err );
	ipiv= imalloc( n * sizeof( int ), err );
	for (j=0;j<n;j++) ipiv[j]=0;
	for (i=0;i<n;i++) {
		big=0.0;
		for (j=0;j<n;j++)
			if (ipiv[j] != 1)
				for (k=0;k<n;k++) {
					if (ipiv[k] == 0) {
						if (fabs(a[j][k]) >= big) {
							big=fabs(a[j][k]);
							irow=j;
							icol=k;
						}
					}
				}
		++(ipiv[icol]);
		if (irow != icol) {
			for (l=0;l<n;l++) SWAP(a[irow][l],a[icol][l])
		}
		indxr[i]=irow;
		indxc[i]=icol;
		pivinv=1.0/a[icol][icol];
		a[icol][icol]=1.0;
		for (l=0;l<n;l++) a[icol][l] *= pivinv;
		for (ll=0;ll<n;ll++)
			if (ll != icol) {
				dum=a[ll][icol];
				a[ll][icol]=0.0;
				for (l=0;l<n;l++) a[ll][l] -= a[icol][l]*dum;
			}
	}
	for (l=( n - 1 );l>=0;l--) {
		if (indxr[l] != indxc[l])
			for (k=0;k<n;k++)
				SWAP(a[k][indxr[l]],a[k][indxc[l]]);
	}
	free(ipiv);
	free(indxr);
	free(indxc);
}



void times( double **a, int i, int j, int k, double **b, double **c ) {

/* Multiply a i x j matrix by a j x k matrix. */

	int      l, m, n;

	for( l = 0; l < i; ++l ) {
		for( m = 0; m < k; ++m ) {
			c[l][m] = 0.0;
			for( n = 0; n < j; ++n ) {
				c[l][m] += a[l][n] * b[n][m];
			}
		}
	}
}


double maximum( double *arr, int n ) {

/* Find the largest member of an array of n doubles. */

	double tmp;
	int    i;

	tmp = arr[0];
	for( i = 1; i < n; ++i )
		if( arr[i] > tmp )
			tmp = arr[i];
	return tmp;
}

