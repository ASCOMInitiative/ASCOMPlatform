/* Header for ASCOM version of SOFA iauDat routine */

#pragma once
#define EXPORT __declspec(dllexport)

/* Structure to hold a leap second value */
struct LeapSecondDataStruct {
	int Year;
	int Month;
	double LeapSeconds;
};

typedef struct LeapSecondDataStruct LeapSecondData;
typedef int bool; enum { false, true };

EXPORT int GetLeapSecondData(LeapSecondData ReturnedLeapSecondData[], bool *UpdatedData);
EXPORT int GetBuiltInLeapSecondData(LeapSecondData ReturnedLeapSecondData[]);
EXPORT int UpdateLeapSecondData(LeapSecondData UpdatedValues[]);
EXPORT int NumberOfBuiltInLeapSecondValues();
EXPORT int UsingUpdatedData();

/* Number of Delta(AT) leap seconds in the internal built-in array*/
/* Must equal the count of the number of entries in BuiltInLeapSecondData (not the array index of the last member, which is count - 1) */
enum { NBUILTIN = 42 };

// Set the maximum possible number of leap seconds. MUST be greater than NBUILTIN.
enum { MAX_LEAP_SECONDS = 100 };