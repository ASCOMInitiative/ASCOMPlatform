#include"chby.h"

double pwr( double x, int y) {

/* Raise a double x to an int power y. */

	double accum = 1.0;
	int    i;
	for( i = 0; i < y; ++i)
		accum *= x;
	return accum;
}



double *maket( double time ) {

/* Compute the values for the Chebyshev polynomial to thirteenth order for a 
   given time. */

	double *t, t2pwr[14];
	int    i;      
	t = malloc( 14 * sizeof( double ) );
	for( i = 0; i < 14; ++i)
		t2pwr[i]  = pwr( time, i);
	*t        =         1.0;
	*(t + 1)  =         time;
	*(t + 2)  =     2 * t2pwr[2]  -     1;
	*(t + 3)  =     4 * t2pwr[3]  -     3 * time;
	*(t + 4)  =     8 * t2pwr[4]  -     8 * t2pwr[2]  +     1;
	*(t + 5)  =    16 * t2pwr[5]  -    20 * t2pwr[3]  +     5 * time;
	*(t + 6)  =    32 * t2pwr[6]  -    48 * t2pwr[4]  +    18 * t2pwr[2]
	          -     1;
	*(t + 7)  =    64 * t2pwr[7]  -   112 * t2pwr[5]  +    56 * t2pwr[3]
	          -     7 * time;
	*(t + 8)  =   128 * t2pwr[8]  -   256 * t2pwr[6]  +   160 * t2pwr[4]
	          -    32 * t2pwr[2]  +     1;
	*(t + 9)  =   256 * t2pwr[9]  -   576 * t2pwr[7]  +   432 * t2pwr[5]
	          -   120 * t2pwr[3]  +     9 * time;
	*(t + 10) =   512 * t2pwr[10] -  1280 * t2pwr[8]  +  1120 * t2pwr[6]
	          -   400 * t2pwr[4]  +    50 * t2pwr[2]  -     1;
	*(t + 11) =  1024 * t2pwr[11] -  2816 * t2pwr[9]  +  2816 * t2pwr[7]
	          -  1232 * t2pwr[5]  +   220 * t2pwr[3]  -    11 * time;
	*(t + 12) =  2048 * t2pwr[12] -  6144 * t2pwr[10] +  6912 * t2pwr[8]
	          -  3584 * t2pwr[6]  +   840 * t2pwr[4]  -    72 * t2pwr[2]
	          +     1;
	*(t + 13) =  4096 * t2pwr[13] - 13312 * t2pwr[11] + 16640 * t2pwr[9]
	          -  9984 * t2pwr[7]  +  2912 * t2pwr[5]  -   364 * t2pwr[3]
	          +    13 * time; 
	return t;
}



double *maketdot( double time ) {

/* Compute the values for the derivatives of the Chebyshev polynomial to
   thirteenth order for a given time. */

	double *t, t2pwr[14];
	int    i;      
	t = malloc( 14 * sizeof( double ) );
	for( i = 0; i < 14; ++i)
		t2pwr[i]  = pwr( time, i);
	*t        =         0.0;
	*(t + 1)  =         1.0;
	*(t + 2)  =     4 * time;
	*(t + 3)  =    12 * t2pwr[2]  -      3;
	*(t + 4)  =    32 * t2pwr[3]  -     16 * time;
	*(t + 5)  =    80 * t2pwr[4]  -     60 * t2pwr[2]  +      5;
	*(t + 6)  =   192 * t2pwr[5]  -    192 * t2pwr[3]  +     36 * time;
	*(t + 7)  =   448 * t2pwr[6]  -    560 * t2pwr[4]  +    168 * t2pwr[2]
	          -     7;
	*(t + 8)  =  1024 * t2pwr[7]  -   1536 * t2pwr[5]  +    640 * t2pwr[3]
	          -    64 * time;
	*(t + 9)  =  2304 * t2pwr[8]  -   4032 * t2pwr[6]  +   2160 * t2pwr[4]
	          -   360 * t2pwr[2]  +      9;
	*(t + 10) =  5120 * t2pwr[9]  -  10240 * t2pwr[7]  +   6720 * t2pwr[5]
	          -  1600 * t2pwr[3]  +    100 * time;
	*(t + 11) = 11264 * t2pwr[10] -  25344 * t2pwr[8]  +  19712 * t2pwr[6]
	          -  6160 * t2pwr[4]  +    660 * t2pwr[2]  -     11;
	*(t + 12) = 24576 * t2pwr[11] -  61440 * t2pwr[9]  +  55296 * t2pwr[7]
	          - 21504 * t2pwr[5]  +   3360 * t2pwr[3]  -    144 * time;
	*(t + 13) = 53248 * t2pwr[12] - 146432 * t2pwr[10] + 149760 * t2pwr[8]
	          - 69888 * t2pwr[6]  +  14560 * t2pwr[4]  -   1092 * t2pwr[2]
	          +    13;
	return t;
}
