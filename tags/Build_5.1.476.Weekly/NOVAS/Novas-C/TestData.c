#include "novas.h"

// Validation data from http://ssd.jpl.nasa.gov/tc.cgi#top
// Input Time Zone: UT
// -------------------------------------------------------
// A.D. 2009-May-19 01:21:12.01 = A.D. 2009-May-19.0563889
// A.D.  2009-05-19 01:21:12.01 = A.D.  2009-05-19.0563889
// A.D.   2009--139 01:21:12.01 = A.D.   2009--139.0563889
// Day-of-Week: Tuesday
// Julian Date: 2454970.5563889 UT

const short testYear = 2009;
const short testMonth = 5;
const short testDay = 19;
const short testHour = 0.0563889 * 24;
const double testJD = 2454970.5563889;

void main()
{
	char buffer[100];
	double expectedJD = testJD;
	double actualJD = julian_date(testYear, testMonth, testDay, testHour);
	printf("julian_day(%d,%d,%d,%f) Expected=%f Actual=%f",
		testYear, testMonth, testDay, testHour, expectedJD, actualJD);
	
	gets(buffer);
}