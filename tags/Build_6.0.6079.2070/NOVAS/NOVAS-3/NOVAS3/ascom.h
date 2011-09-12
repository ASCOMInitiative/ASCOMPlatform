#ifndef __ASCOM__
#define __ASCOM__
#define EXPORT __declspec(dllexport)
#endif

// Prototype for the new method to set the ra cio file name
// The method itself will be found in ascom.c

EXPORT void set_racio_file(char star_name[255]);