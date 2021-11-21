#ifndef _NOVAS_
#include "novas.h"
#endif
#include <windows.h>

#include <stdio.h>
#include <time.h>

BOOL WINAPI DllMain(
    HINSTANCE hinstDLL,  // handle to DLL module
    DWORD fdwReason,     // reason for calling function
    LPVOID lpReserved)  // reserved
{
    time_t seconds;

    seconds = time(NULL);
    printf("Seconds since January 1, 1970 = %ld\n", seconds);

    char number_str[20];

    sprintf(number_str, "%ld", seconds);
    printf("Converted to string : %s\n", number_str);

    char fileName[200];
    strcpy(fileName, "C:\\novas31\\log\\");
    strcat(fileName, number_str);
    strcat(fileName, ".txt");
    printf("File name: %s\n", fileName);

    //char sentence[1000];

    //// creating file pointer to work with files
    //FILE* fptr;

    //// opening file in writing mode
    //fptr = fopen("program.txt", "w");

    //// exiting program 
    //if (fptr == NULL) {
    //    printf("Error!");
    //    exit(1);
    //}
    //printf("Enter a sentence:\n");
    //fgets(sentence, sizeof(sentence), stdin);
    //fprintf(fptr, "%s", sentence);
    //fclose(fptr);


    // Perform actions based on the reason for calling.
    switch (fdwReason)
    {
    case DLL_PROCESS_ATTACH:
        // Initialize once for each new process.
        // Return FALSE to fail DLL load.
        break;

    case DLL_THREAD_ATTACH:
        // Do thread-specific initialization.
        break;

    case DLL_THREAD_DETACH:
        // Do thread-specific cleanup.
        break;

    case DLL_PROCESS_DETACH:
        // Perform any necessary cleanup.
        ephem_close();
        break;
    }
    return TRUE;  // Successful DLL_PROCESS_ATTACH.
}