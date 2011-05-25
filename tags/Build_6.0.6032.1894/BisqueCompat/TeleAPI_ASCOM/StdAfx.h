// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__E5B7297B_5CA9_48AD_B3C2_13087E0BD5AB__INCLUDED_)
#define AFX_STDAFX_H__E5B7297B_5CA9_48AD_B3C2_13087E0BD5AB__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


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
#include "AscomScope.h"
#include "teleapi.h"

// TODO: reference additional headers your program requires here

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__E5B7297B_5CA9_48AD_B3C2_13087E0BD5AB__INCLUDED_)
