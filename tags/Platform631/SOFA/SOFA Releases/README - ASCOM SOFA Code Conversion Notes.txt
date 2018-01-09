To convert native SOFA source code to compile under Visual Studio you need to:

FILE t_sofa_c
In t_sofa_c.c change #include <sofa.h> to #include "sofa.h" 

FILE sofa.h
Add #define EXPORT __declspec(dllexport) after #define SOFAHDEF
#ifndef SOFAHDEF
#define SOFAHDEF
#define EXPORT __declspec(dllexport)

Insert EXPORT in front of every function definition. e.g.

/* Astronomy/Calendars */
int iauCal2jd(int iy, int im, int id, double *djm0, double *djm);

becomes

/* Astronomy/Calendars */
EXPORT int iauCal2jd(int iy, int im, int id, double *djm0, double *djm);

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