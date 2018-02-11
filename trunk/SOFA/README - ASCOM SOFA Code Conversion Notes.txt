When a new SOFA release is available:
1. Create a new folder in the "SOFA\Release Zip files" folder whose name includes the release date and SOFA version number
2. Copy the "long name" source code zip file from http://www.iausofa.org/current_C.html#Downloads to the new folder
3. Unzip the zip file contents to the new folder
4. In Visual Studio, delete all of the Source Files and Header Files from the SOFA Library and SOFA Test Application projects
5. Copy the contents of the "SOFA\Release Zip Files\NewReleaseFolderName\sofa\yyyymmdd\c\src" folder to the \SOFA\Current Source Code folder
6. In Visual Studio, add the sofa.h and sofam.h header files, located in the "SOFA\Current Source Code" folder, to the "Sofa Library" and "Sofa Test Application" header files lists
7. In Visual Studio, add all the C Source code (*.c) files except t_sofa_c.c to the "Sofa Library" Source Files list
8. In Visual Studio, add the t_sofa_c.c file to the "Sofa Test Application" Visual Studio Source Files list
9. In Visual Studio, adapt the SOFA source code so that it will compile under Visual Studio; you need to make the (non-functional) changes shown below.
10. In Visual Studio, edit the version information stored in the Sofa Library and Sofa Test Application resource files to reflect the new SOFA major and revision versions
11. In Visual Studio, update the release number, issue date, revision number and revision date constants at the top of the SOFA.vb file in the Astrometry project to match the new SOFA release.
12. Build the two projects to confirm that all is well.
13. At a command prompt, run both the 32 and 64bit versions of the the Sofa Test Application and confirm that no errors are generated.
12. Commit the changes, make a new build and test it with the Diagnostics application.

*************************************************************************
SOFA FILE CHANGES REQUIRED TO COMPILE IN VISUAL STUDIO
*************************************************************************

*************************************************************************
FILE t_sofa_c
Change #include <sofa.h> to #include "sofa.h" 
*************************************************************************

*************************************************************************
FILE sofa.h
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
*************************************************************************

*************************************************************************
FILE SOFAM.H
Add EXPORT to iauASTROM and iauLDBODY STRUCT definitions e.g.

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