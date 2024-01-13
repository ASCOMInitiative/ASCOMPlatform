When a new SOFA release is available:
1. Create a new folder in the "SOFA\Release Zip files" folder whose name includes the release date and SOFA version number
2. Copy the "long name" source code zip file from http://www.iausofa.org/current_C.html#Downloads to the new folder
3. Unzip the zip file contents to the new folder
4. In Visual Studio, delete all of the files in the "SOFA Source Files" and "SOFA Header Files" folders in the SOFA Library project.
5. In Visual Studio, delete all of the files in the "Header Files"and "Source Files" folders in th SOFA Test Application project.
6. Copy the contents of the "SOFA\Release Zip Files\NewReleaseFolderName\sofa\yyyymmdd\c\src" folder to the \SOFA\Current Source Code folder
7. In Visual Studio, add the sofa.h and sofam.h header files, located in the "SOFA\Current Source Code" folder, to the "Sofa Library" and "Sofa Test Application" header files lists
8. In Visual Studio, add all the C Source code (*.c) files except t_sofa_c.c and dat.c to the "Sofa Source Files" list
9. In Visual Studio, add the t_sofa_c.c file to the "Sofa Test Application" Visual Studio Source Files list
10. In Visual Studio, change t_sofa_c.c file #include <sofa.h> line to #include "sofa.h"
11. In Visual Studio, adapt the SOFA source code so that it will compile under Visual Studio; you need to make the (non-functional) changes shown below.
12. In Visual Studio, adapt the ASCOMDat.c "Release year for this version of iauDat" enum value to the one in the latest dat.c file e.g. you are looking for a line that resembles this but contains a later year number: enum { IYV = 2019 };
13. In Visual Studio, edit the product version number stored in the resource files "Sofa Library.rc" (ASCOM Resouirce Files folder) and "Sofa Test Application.rc" (Resource Files folder) to reflect the new SOFA major and revision versions e.g. 18,0,0,0
14. In Visual Studio, update the release number, issue date, revision number and revision date constants at the top of the SOFA.cs file in the Astrometry project to match the new SOFA release.
15. Build the two projects to confirm that all is well.
16. At a command prompt, run both the 32 and 64bit versions of the the Sofa Test Application and confirm that no errors are generated.
17. Commit the changes, make a new build and test it with the Diagnostics application.

*************************************************************************
SOFA FILE CHANGES REQUIRED TO COMPILE IN VISUAL STUDIO
*************************************************************************

*************************************************************************
***** FILE t_sofa_c *****
Change #include <sofa.h> to #include "sofa.h" 
*************************************************************************

*************************************************************************
***** FILE sofa.h *****
ADD #define EXPORT __declspec(dllexport) AFTER #define SOFAHDEF as shown below
#ifndef SOFAHDEF
#define SOFAHDEF
#define EXPORT __declspec(dllexport)                 <== Additional line

Insert EXPORT in front of every function definition. e.g.

/* Astronomy/Calendars */
int iauCal2jd(int iy, int im, int id, double *djm0, double *djm);

becomes

/* Astronomy/Calendars */
EXPORT int iauCal2jd(int iy, int im, int id, double *djm0, double *djm);

You can use CTRL/H global change e.g. "void iau" ==> "EXPORT void iau" and same for int and double

ADD EXPORT to the iauASTROM and iauLDBODY STRUCT definitions e.g.

/* Body parameters for light deflection */
typedef struct {
   double bm;         /* mass of the body (solar masses) */
   double dl;         /* deflection limiter (radians^2/2) */
   double pv[2][3];   /* barycentric PV of the body (au, au/day) */
} iauLDBODY;

becomes
/* Body parameters for light deflection */
typedef struct {
   double bm;         /* mass of the body (solar masses) */
   double dl;         /* deflection limiter (radians^2/2) */
   double pv[2][3];   /* barycentric PV of the body (au, au/day) */
} EXPORT iauLDBODY;
*************************************************************************