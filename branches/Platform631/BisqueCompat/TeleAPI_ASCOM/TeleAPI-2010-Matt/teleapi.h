#ifdef _MAC
	#define TELEAPIEXPORT EXPORT
#else
	#define TELEAPIEXPORT __declspec( dllexport )
#endif

TELEAPIEXPORT int CALLBACK tapiGetDLLVersion(void);
TELEAPIEXPORT int CALLBACK tapiEstablishLink(void);
TELEAPIEXPORT int CALLBACK tapiTerminateLink(void);
TELEAPIEXPORT int CALLBACK tapiGetRaDec(double* ra, double* dec);
TELEAPIEXPORT int CALLBACK tapiSetRaDec(const double ra, const double dec);
TELEAPIEXPORT int CALLBACK tapiGotoRaDec(const double ra, const double dec);
TELEAPIEXPORT int CALLBACK tapiIsGotoComplete(BOOL* pbComplete);
TELEAPIEXPORT int CALLBACK tapiAbortGoto(void);
TELEAPIEXPORT int CALLBACK tapiSettings(void);

TELEAPIEXPORT int CALLBACK tapiPulseFocuser(const BOOL bFastSpeed, const BOOL bIn);