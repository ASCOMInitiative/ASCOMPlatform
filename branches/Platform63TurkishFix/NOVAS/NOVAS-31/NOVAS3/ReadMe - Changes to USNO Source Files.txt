novas.h
	EXPORT prefix added to all function prototypes
	Added include of ascom.h

novas.c
	An extern statement for RACIO_FILE_NAME has been added to the top of novas.c
	cio_location - 1 line changed to suport RACIO_FILE_NAME see Peter Simpson comment
	cio_array - 1 line changed to suport RACIO_FILE_NAME see Peter Simpson comment

eph_manager.h
	EXPORT prefix added to ephem_open function prototype
	EXPORT prefix added to ephem_close function prototype
	EXPORT prefix added to planet_ephemeris function prototype
	EXPORT prefix added to state function prototype
	Added include of ascom.h

eph_manager.c
	Added line to ensure the file appears fully closed see Peter Simpson comment

readeph.c
	changed readeph function parameter (err to *err) to ensure an error value is returned : double *readeph( int mp, char *name, double jd, int *err )

nutation.h
	EXPORT prefix added to iau2000a function prototype
	EXPORT prefix added to iau2000b function prototype
	EXPORT prefix added to iau2000k function prototype

solarsystem.h
	EXPORT prefix added to solarsystem function prototype
	EXPORT prefix added to solarsystem_hp function prototype

ascom.h
	File added

ascom.c
	File added