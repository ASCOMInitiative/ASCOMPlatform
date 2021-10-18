#include <stdio.h>
#include "ASCOMDat.h"
#include "..\Currrent Source Code\sofa.h"

/* This routine returns  the number of leap seconds for a given Gregorian calendar date. */

/* The IAU / SOFA supplied library makes use of hard coded leap second data to enable it to operate in a stand alone environment. */
/* However, this approach requires that any application that uses the library be recompiled with an updated SOFA library whenever a new leap second is announced.*/
/* To mitigate this, SOFA have provided a dispensation (SOFA License Note 3c below) for organisations to modify the iauDat routine to suite local conventions for dealing with leap second changes.*/

/* This revised routine has been developed for the ASCOM Platform and enables the SOFA library to make use of automatically downloaded */
/* Since C doesn't have provision for dynamic array sizes, a design decision has been made to over specify the likely maximum number of leap second values as 100. */

static LeapSecondData UpdatedLeapSecondData[100];  // Global LeapSecondData array to hold updated leap second data supplied to this DLL
static bool HasUpdatedData = false;				   // Global boolean flag to indicate whether updated data has been supplied. If TRUE, the updated data will be used, if FALSE, built-in data will be used
static int NUPDATED = 0;						   // Global integer to hold the number of updated leap second records supplied

// This is the master data table for the whole ASCOM Platform. It is located here in the SOFA DLLs so that they can be used independently of the Platform if required.
// The Platform EarthRotatiuonParameters class reads these values through a static SOFA assembly method and uses them as needed throughout the Platform
// When new leap seconds are announced:
//      1) The new value should be added to the end of the values here
//      2) The NBUILTIN enum in ASCOMDat.h should be incremented to reflect the new number of leap second values
static LeapSecondData BuiltInLeapSecondData[100] = // Global LeapSecondData array to hold built-in leap second values
{
	{ 1960,  1,  1.4178180 },  // BuiltInLeapSecondData[0]
	{ 1961,  1,  1.4228180 },
	{ 1961,  8,  1.3728180 },
	{ 1962,  1,  1.8458580 },
	{ 1963, 11,  1.9458580 },
	{ 1964,  1,  3.2401300 },
	{ 1964,  4,  3.3401300 },
	{ 1964,  9,  3.4401300 },
	{ 1965,  1,  3.5401300 },
	{ 1965,  3,  3.6401300 },
	{ 1965,  7,  3.7401300 },  // BuiltInLeapSecondData[10]
	{ 1965,  9,  3.8401300 },
	{ 1966,  1,  4.3131700 },
	{ 1968,  2,  4.2131700 },
	{ 1972,  1, 10.0 },
	{ 1972,  7, 11.0 },
	{ 1973,  1, 12.0 },
	{ 1974,  1, 13.0 },
	{ 1975,  1, 14.0 },
	{ 1976,  1, 15.0 },
	{ 1977,  1, 16.0 },  // BuiltInLeapSecondData[20]
	{ 1978,  1, 17.0 },
	{ 1979,  1, 18.0 },
	{ 1980,  1, 19.0 },
	{ 1981,  7, 20.0 },
	{ 1982,  7, 21.0 },
	{ 1983,  7, 22.0 },
	{ 1985,  7, 23.0 },
	{ 1988,  1, 24.0 },
	{ 1990,  1, 25.0 },
	{ 1991,  1, 26.0 },  // BuiltInLeapSecondData[30]
	{ 1992,  7, 27.0 },
	{ 1993,  7, 28.0 },
	{ 1994,  7, 29.0 },
	{ 1996,  1, 30.0 },
	{ 1997,  7, 31.0 },
	{ 1999,  1, 32.0 },
	{ 2006,  1, 33.0 },
	{ 2009,  1, 34.0 },
	{ 2012,  7, 35.0 },
	{ 2015,  7, 36.0 },  // BuiltInLeapSecondData[40]
	{ 2017,  1, 37.0 },  // BuiltInLeapSecondData[41] <=== Insert additional values after this one and UPDATE NBUILTIN in the ASCOMDat.h file or new values won't be used!
};

int iauDat(int iy, int im, int id, double fd, double *deltat)
/*
**  - - - - - - -
**   i a u D a t
**  - - - - - - -
**
**  For a given UTC date, calculate Delta(AT) = TAI-UTC.
**
**     :------------------------------------------:
**     :                                          :
**     :                 IMPORTANT                :
**     :                                          :
**     :  A new version of this function must be  : <=== No longer the case!
**     :  produced whenever a new leap second is  :
**     :  announced.  There are four items to     :
**     :  change on each such occasion:           :
**     :                                          :
**     :  1) A new line must be added to the set  :
**     :     of statements that initialize the    :
**     :     array "changes".                     :
**     :                                          :
**     :  2) The constant IYV must be set to the  :
**     :     current year.                        :
**     :                                          :
**     :  3) The "Latest leap second" comment     :
**     :     below must be set to the new leap    :
**     :     second date.                         :
**     :                                          :
**     :  4) The "This revision" comment, later,  :
**     :     must be set to the current date.     :
**     :                                          :
**     :  Change (2) must also be carried out     :
**     :  whenever the function is re-issued,     :
**     :  even if no leap seconds have been       :
**     :  added.                                  :
**     :                                          :
**     :  Latest leap second:  2016 December 31   :
**     :                                          :
**     :__________________________________________:
**
**  This function is part of the International Astronomical Union's
**  SOFA (Standards Of Fundamental Astronomy) software collection.
**
**  Status:  user-replaceable support function.
**
**  Given:
**     iy     int      UTC:  year (Notes 1 and 2)
**     im     int            month (Note 2)
**     id     int            day (Notes 2 and 3)
**     fd     double         fraction of day (Note 4)
**
**  Returned:
**     deltat double   TAI minus UTC, seconds
**
**  Returned (function value):
**            int      status (Note 5):
**                       1 = dubious year (Note 1)
**                       0 = OK
**                      -1 = bad year
**                      -2 = bad month
**                      -3 = bad day (Note 3)
**                      -4 = bad fraction (Note 4)
**                      -5 = internal error (Note 5)
**
**  Notes:
**
**  1) UTC began at 1960 January 1.0 (JD 2436934.5) and it is improper
**     to call the function with an earlier date.  If this is attempted,
**     zero is returned together with a warning status.
**
**     Because leap seconds cannot, in principle, be predicted in
**     advance, a reliable check for dates beyond the valid range is
**     impossible.  To guard against gross errors, a year five or more
**     after the release year of the present function (see the constant
**     IYV) is considered dubious.  In this case a warning status is
**     returned but the result is computed in the normal way.
**
**     For both too-early and too-late years, the warning status is +1.
**     This is distinct from the error status -1, which signifies a year
**     so early that JD could not be computed.
**
**  2) If the specified date is for a day which ends with a leap second,
**     the TAI-UTC value returned is for the period leading up to the
**     leap second.  If the date is for a day which begins as a leap
**     second ends, the TAI-UTC returned is for the period following the
**     leap second.
**
**  3) The day number must be in the normal calendar range, for example
**     1 through 30 for April.  The "almanac" convention of allowing
**     such dates as January 0 and December 32 is not supported in this
**     function, in order to avoid confusion near leap seconds.
**
**  4) The fraction of day is used only for dates before the
**     introduction of leap seconds, the first of which occurred at the
**     end of 1971.  It is tested for validity (0 to 1 is the valid
**     range) even if not used;  if invalid, zero is used and status -4
**     is returned.  For many applications, setting fd to zero is
**     acceptable;  the resulting error is always less than 3 ms (and
**     occurs only pre-1972).
**
**  5) The status value returned in the case where there are multiple
**     errors refers to the first error detected.  For example, if the
**     month and day are 13 and 32 respectively, status -2 (bad month)
**     will be returned.  The "internal error" status refers to a
**     case that is impossible but causes some compilers to issue a
**     warning.
**
**  6) In cases where a valid result is not available, zero is returned.
**
**  References:
**
**  1) For dates from 1961 January 1 onwards, the expressions from the
**     file ftp://maia.usno.navy.mil/ser7/tai-utc.dat are used.
**
**  2) The 5ms timestep at 1961 January 1 is taken from 2.58.1 (p87) of
**     the 1992 Explanatory Supplement.
**
**  Called:
**     iauCal2jd    Gregorian calendar to JD
**
**  This revision:  2017 October 7
**
**  SOFA release 2018-01-30
**
**  Copyright (C) 2018 IAU SOFA Board.  See notes at end.
*/
{
	LeapSecondData(*SelectedLeapSecondData)[100]; // Pointer based array definition. The pointer can reference either the BuiltInLeapSecondData array or the UpdatedLeapSecondData array.
	int NDAT; // Number of valid records in the pointer based table (not the dimension of the array itself)

	/* Release year for this version of iauDat */
	enum { IYV = 2021 };

	/* Reference dates (MJD) and drift rates (s/day), pre leap seconds */
	static const double drift[][2] =
	{
		{ 37300.0, 0.0012960 },
		{ 37300.0, 0.0012960 },
		{ 37300.0, 0.0012960 },
		{ 37665.0, 0.0011232 },
		{ 37665.0, 0.0011232 },
		{ 38761.0, 0.0012960 },
		{ 38761.0, 0.0012960 },
		{ 38761.0, 0.0012960 },
		{ 38761.0, 0.0012960 },
		{ 38761.0, 0.0012960 },
		{ 38761.0, 0.0012960 },
		{ 38761.0, 0.0012960 },
		{ 39126.0, 0.0025920 },
		{ 39126.0, 0.0025920 }
	};

	/* Number of Delta(AT) expressions before leap seconds were introduced */
	enum { NERA1 = (int)(sizeof drift / sizeof(double) / 2) };

	if (HasUpdatedData) // If outside data has been supplied then use the updated leap second data
	{
		NDAT = NUPDATED; // Save the number of built-in records
		SelectedLeapSecondData = &UpdatedLeapSecondData; // Assign the updated data array address to the SelectedLeapSecondArray variable
#ifdef _DEBUG 
		printf("Using %i UPDATED leap second data values to get leap seconds for day %i %i %i - ", NDAT, id, im, iy);
#endif
	}
	else // No external data has been supplied so use the built-in data instead
	{
		NDAT = NBUILTIN; // Save the number of updated data records
		SelectedLeapSecondData = &BuiltInLeapSecondData; // Assign the built-in data array address to the SelectedLeapSecondArray variable
#ifdef _DEBUG 
		printf("Using %i BUILT IN leap second data values to get leap seconds for day %i %i %i - ", NDAT, id, im, iy);
#endif
	}

	/* Above this line is the data preparation and selection section */

	/* Below this line is the unchanged algorithm from the February 2018 SOFA release. The only code changes are to replace references to the built-in data array
	   with references to the pointer array that can point at either the built-in or updated data arrays */

	   /* Miscellaneous local variables */
	int j, i, m;
	double da, djm0, djm;

	/* Initialize the result to zero. */
	*deltat = da = 0.0;

	/* If invalid fraction of a day, set error status and give up. */
	if (fd < 0.0 || fd > 1.0) return -4;

	/* Convert the date into an MJD. */
	j = iauCal2jd(iy, im, id, &djm0, &djm);

	/* If invalid year, month, or day, give up. */
	if (j < 0) return j;

	/* If pre-UTC year, set warning status and give up. */
	if (iy < (*SelectedLeapSecondData)[0].Year) return 1;

	/* If suspiciously late year, set warning status but proceed. */
	if (iy > IYV + 5) j = 1;

	/* Combine year and month to form a date-ordered integer... */
	m = 12 * iy + im;

	/* ...and use it to find the preceding table entry. */
	for (i = NDAT - 1; i >= 0; i--) {
		if (m >= (12 * (*SelectedLeapSecondData)[i].Year + (*SelectedLeapSecondData)[i].Month)) break;
	}

	/* Prevent underflow warnings. */
	if (i < 0) return -5;

	/* Get the Delta(AT). */
	da = (*SelectedLeapSecondData)[i].LeapSeconds;

	/* If pre-1972, adjust for drift. */
	if (i < NERA1) da += (djm + fd - drift[i][0]) * drift[i][1];

	/* Return the Delta(AT) value. */
	*deltat = da;

#ifdef _DEBUG 
	printf("Returning Leap Seconds: %g, Status: %i \n", da, j);
#endif

	/* Return the status. */
	return j;

	/*----------------------------------------------------------------------
	**
	**  Copyright (C) 2018
	**  Standards Of Fundamental Astronomy Board
	**  of the International Astronomical Union.
	**
	**  =====================
	**  SOFA Software License
	**  =====================
	**
	**  NOTICE TO USER:
	**
	**  BY USING THIS SOFTWARE YOU ACCEPT THE FOLLOWING SIX TERMS AND
	**  CONDITIONS WHICH APPLY TO ITS USE.
	**
	**  1. The Software is owned by the IAU SOFA Board ("SOFA").
	**
	**  2. Permission is granted to anyone to use the SOFA software for any
	**     purpose, including commercial applications, free of charge and
	**     without payment of royalties, subject to the conditions and
	**     restrictions listed below.
	**
	**  3. You (the user) may copy and distribute SOFA source code to others,
	**     and use and adapt its code and algorithms in your own software,
	**     on a world-wide, royalty-free basis.  That portion of your
	**     distribution that does not consist of intact and unchanged copies
	**     of SOFA source code files is a "derived work" that must comply
	**     with the following requirements:
	**
	**     a) Your work shall be marked or carry a statement that it
	**        (i) uses routines and computations derived by you from
	**        software provided by SOFA under license to you; and
	**        (ii) does not itself constitute software provided by and/or
	**        endorsed by SOFA.
	**
	**     b) The source code of your derived work must contain descriptions
	**        of how the derived work is based upon, contains and/or differs
	**        from the original SOFA software.
	**
	**     c) UNLIKE OTHER SOFA SOFTWARE, WHICH IS STRICTLY "READ ONLY",
	**        USERS ARE PERMITTED TO REPLACE THIS FUNCTION WITH ONE USING
	**        THE SAME NAME BUT DIFFERENT CODE.  This is to allow use of
	**        locally supported mechanisms for keeping track of leap
	**        seconds, perhaps file or network based.  It avoids the need
	**        for applications to be relinked periodically in order to pick
	**        up SOFA updates.
	**
	**     d) The origin of the SOFA components of your derived work must
	**        not be misrepresented;  you must not claim that you wrote the
	**        original software, nor file a patent application for SOFA
	**        software or algorithms embedded in the SOFA software.
	**
	**     e) These requirements must be reproduced intact in any source
	**        distribution and shall apply to anyone to whom you have
	**        granted a further right to modify the source code of your
	**        derived work.
	**
	**     Note that, as originally distributed, the SOFA software is
	**     intended to be a definitive implementation of the IAU standards,
	**     and consequently third-party modifications are discouraged.  All
	**     variations, no matter how minor, must be explicitly marked as
	**     such, as explained above.
	**
	**  4. You shall not cause the SOFA software to be brought into
	**     disrepute, either by misuse, or use for inappropriate tasks, or
	**     by inappropriate modification.
	**
	**  5. The SOFA software is provided "as is" and SOFA makes no warranty
	**     as to its use or performance.   SOFA does not and cannot warrant
	**     the performance or results which the user may obtain by using the
	**     SOFA software.  SOFA makes no warranties, express or implied, as
	**     to non-infringement of third party rights, merchantability, or
	**     fitness for any particular purpose.  In no event will SOFA be
	**     liable to the user for any consequential, incidental, or special
	**     damages, including any lost profits or lost savings, even if a
	**     SOFA representative has been advised of such damages, or for any
	**     claim by any third party.
	**
	**  6. The provision of any version of the SOFA software under the terms
	**     and conditions specified herein does not imply that future
	**     versions will also be made available under the same terms and
	**     conditions.
	*
	**  In any published work or commercial product which uses the SOFA
	**  software directly, acknowledgement (see www.iausofa.org) is
	**  appreciated.
	**
	**  Correspondence concerning SOFA software should be addressed as
	**  follows:
	**
	**      By email:  sofa@ukho.gov.uk
	**      By post:   IAU SOFA Center
	**                 HM Nautical Almanac Office
	**                 UK Hydrographic Office
	**                 Admiralty Way, Taunton
	**                 Somerset, TA1 2DN
	**                 United Kingdom
	**
	**--------------------------------------------------------------------*/

}

/* Method called from outside the SOFA DLL to accept updated leap second data and store it for use instead of the built-in data table */
int UpdateLeapSecondData(LeapSecondData SuppliedLeapSecondData[])

/************************************************************************************************************************************************************************/
/* INPUT:   Array of LeapSecondDataStruct values - maximum size 100 elements																							*/
/* STORES:  Supplied SuppliedLeapSecondData array data in global array UpdatedLeapSecondData                                                                            */
/* RETURNS: Integer status code:                                                                                                                                        */
/*                       0 - Success - data accepted																													*/
/*                       1 - Success - data ignored because it has already been supplied 			        															*/
/*                       2 - Failure - data rejected because at least 100 records were supplied and this is unreasonable since there are only 42 records at April 2018! */
/************************************************************************************************************************************************************************/

{
	int rc = 0;

	if (!HasUpdatedData) // Only read in updated data once per DLL instance
	{

#ifdef _DEBUG 
		printf("UpdateLeapSecondData uninitialised so reading supplied records...\n");
#endif

		/* Iterate over the supplied data and transfer it to an internal array */
		while ((SuppliedLeapSecondData[NUPDATED].Year != 0) & (NUPDATED <= 100))
		{
#ifdef _DEBUG 
			printf("UpdateLeapSecondData value: %g %i %i\n", SuppliedLeapSecondData[NUPDATED].LeapSeconds, SuppliedLeapSecondData[NUPDATED].Month, SuppliedLeapSecondData[NUPDATED].Year);
#endif

			UpdatedLeapSecondData[NUPDATED].Year = SuppliedLeapSecondData[NUPDATED].Year; // Save the data to the UpdatedLeapSecondData array
			UpdatedLeapSecondData[NUPDATED].Month = SuppliedLeapSecondData[NUPDATED].Month;
			UpdatedLeapSecondData[NUPDATED].LeapSeconds = SuppliedLeapSecondData[NUPDATED].LeapSeconds;

			NUPDATED += 1; // Increment the record count
		}

		if (NUPDATED <= 100) // We have less than 100 records so let's assume these are OK
		{
			HasUpdatedData = true; // Record that we will use the updated leap second data
			rc = 0; // Return a status code to show that the new data was accepted
		}
		else // We have at least 100 records so we will assume that there are more to come, which would exceed the data size of the array. Consequently we will not accept the new data
		{
			HasUpdatedData = false; // Record that we will use the built-in leap second data
			rc = 2; // Return a status code to show that the new data was rejected
		}

	}
	else // We already have the data so ignore the request to provide it again
	{
		rc = 1; // Return a status code to indicate that the data was ignored
	}

#ifdef _DEBUG 
	if (rc == 0)
	{
		printf("UpdateLeapSecondData - Updated leap second data accepted!\n");
	}
	else if (rc == 1)
	{
		printf("UpdateLeapSecondData - Updated leap second data has already been provided, ignoring this data.\n");
	}
	else if (rc == 2)
	{
		printf("UpdateLeapSecondData - Updated leap second data rejected, using built-in default. 100 or more records were supplied, which exceeds internal SOFA array capacity.\n");
	}
	else
	{
		printf("UpdateLeapSecondData - Unknown return code: %i\n", rc);
	}
#endif
	return rc;
}

int NumberOfBuiltInLeapSecondValues()
{
	return NBUILTIN;
}

/* Method called from outside the SOFA DLL to return the current leap second data table */
int GetLeapSecondData(LeapSecondData ReturnedLeapSecondData[], bool *UpdatedData)

/************************************************************************************************************************************************************************/
/* RETURNS: The currently in use array of leap second data                                                                                                              */
/* RETURNS: Boolean indicating whether the built-in or user supplied tables are being returned                                                                          */
/* RETURNS: Integer status code:  The number of records being returned                                                                                                  */
/************************************************************************************************************************************************************************/

{
	int rc = 0;
	int RecordNumber = 0;

	if (HasUpdatedData) // Return updated data array
	{

#ifdef _DEBUG 
		printf("GetLeapSecondData - Returning updated data...\n");
#endif

		/* Iterate over the supplied data and transfer it to the return array */
		while (RecordNumber < NUPDATED)
		{
			ReturnedLeapSecondData[RecordNumber].Year = UpdatedLeapSecondData[RecordNumber].Year; // Save the data to the UpdatedLeapSecondData array
			ReturnedLeapSecondData[RecordNumber].Month = UpdatedLeapSecondData[RecordNumber].Month;
			ReturnedLeapSecondData[RecordNumber].LeapSeconds = UpdatedLeapSecondData[RecordNumber].LeapSeconds;

#ifdef _DEBUG 
			printf("GetLeapSecondData value: %g %i %i\n", ReturnedLeapSecondData[RecordNumber].LeapSeconds, ReturnedLeapSecondData[RecordNumber].Month, ReturnedLeapSecondData[RecordNumber].Year);
#endif

			RecordNumber += 1; // Increment the record count
		}

		rc = NUPDATED; // Return a status code to indicate the number of returned items
	}
	else // Return the built-in data array
	{
#ifdef _DEBUG 
		printf("GetLeapSecondData - Returning built-in data...\n");
#endif

		//BuiltInLeapSecondData
		/* Iterate over the supplied data and transfer it to the return array */
		while (RecordNumber < NBUILTIN)
		{
			ReturnedLeapSecondData[RecordNumber].Year = BuiltInLeapSecondData[RecordNumber].Year; // Save the data to the UpdatedLeapSecondData array
			ReturnedLeapSecondData[RecordNumber].Month = BuiltInLeapSecondData[RecordNumber].Month;
			ReturnedLeapSecondData[RecordNumber].LeapSeconds = BuiltInLeapSecondData[RecordNumber].LeapSeconds;

#ifdef _DEBUG 
			printf("GetLeapSecondData value: %g %i %i\n", ReturnedLeapSecondData[RecordNumber].LeapSeconds, ReturnedLeapSecondData[RecordNumber].Month, ReturnedLeapSecondData[RecordNumber].Year);
#endif

			RecordNumber += 1; // Increment the record count
		}

		rc = NBUILTIN; // Return a status code to indicate the number of returned items
	}

#ifdef _DEBUG 
	printf("GetLeapSecondData - Return code: %i\n", rc);
#endif

	*UpdatedData = HasUpdatedData;

	return rc;
}

/* Method called from outside the SOFA DLL to return the current leap second data table */
int UsingUpdatedData()
{
	int intValue;
	intValue = HasUpdatedData;

#ifdef _DEBUG 
	printf("UsingUpdatedData - Integer value: %i, Boolean value: %i\n", intValue, HasUpdatedData);
#endif

	return HasUpdatedData;
}

/* Method called from outside the SOFA DLL to return the built-in leap second data table */
int GetBuiltInLeapSecondData(LeapSecondData ReturnedLeapSecondData[])

/************************************************************************************************************************************************************************/
/* RETURNS: The built-in array of leap second data                                                                                                                      */
/* RETURNS: Integer status code:  The number of records being returned                                                                                                  */
/************************************************************************************************************************************************************************/

{
	int rc = 0;
	int RecordNumber = 0;

#ifdef _DEBUG 
	printf("GetLeapSecondData - Returning built-in data...\n");
#endif

	//BuiltInLeapSecondData
	/* Iterate over the supplied data and transfer it to the return array */
	while (RecordNumber < NBUILTIN)
	{
		ReturnedLeapSecondData[RecordNumber].Year = BuiltInLeapSecondData[RecordNumber].Year; // Save the data to the UpdatedLeapSecondData array
		ReturnedLeapSecondData[RecordNumber].Month = BuiltInLeapSecondData[RecordNumber].Month;
		ReturnedLeapSecondData[RecordNumber].LeapSeconds = BuiltInLeapSecondData[RecordNumber].LeapSeconds;

#ifdef _DEBUG 
		printf("GetBuiltInLeapSecondData value: %g %i %i\n", ReturnedLeapSecondData[RecordNumber].LeapSeconds, ReturnedLeapSecondData[RecordNumber].Month, ReturnedLeapSecondData[RecordNumber].Year);
#endif

		RecordNumber += 1; // Increment the record count
	}

	rc = NBUILTIN; // Return a status code to indicate the number of returned items

#ifdef _DEBUG 
	printf("GetBuiltInLeapSecondData - Return code: %i\n", rc);
#endif

	return rc;
}
