/* this file contains the actual definitions of */
/* the IIDs and CLSIDs */

/* link this file in with the server and any clients */


/* File created by MIDL compiler version 5.01.0164 */
/* at Sat Mar 28 19:43:31 2009
 */
/* Compiler settings for C:\Proj\ASCOM\RoboFocus\RoboFocus.idl:
    Oicf (OptLev=i2), W1, Zp8, env=Win32, ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
*/
//@@MIDL_FILE_HEADING(  )
#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

const IID IID_IFocuser = {0x7FECCD83,0x2147,0x48F0,{0x98,0x89,0x59,0x5E,0x1F,0x12,0xFA,0xAA}};


const IID LIBID_RoboFocusLib = {0xE8E01F61,0x2D1D,0x4585,{0x83,0x3A,0x38,0x48,0x0D,0xA2,0x2E,0xB1}};


const CLSID CLSID_Focuser = {0x026DAA75,0xC2C3,0x488d,{0xAC,0x0A,0xA8,0x34,0x2D,0x17,0x02,0xE6}};


#ifdef __cplusplus
}
#endif

