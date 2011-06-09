#pragma once

#define WIN32_LEAN_AND_MEAN      // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>

// Insert your headers here
#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
#define NOSERVICE
#define NOMCX
#define NOIME
#define NOSOUND
#define NOKANJI
#define NOIMAGE
#define NOTAPE
#define _WIN32_DCOM				// Enable DCOM extensions

#include <windows.h>
#include <stdio.h>
#include <objbase.h>
#include <ole2ver.h>
#include <initguid.h>
#include <stdlib.h>
#include <string.h>
#include "main.h"
#include "ASCOM.Focuser.h"

