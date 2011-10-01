The VS2010 solution here does work and compile OK but it produces DLLs having a dependency on the VC++ 10.0 support libraries that are not in widespread use. 

Platform 6 distributes NOVAS31 DLLs compiled with VS2008 because this targets the VC++ 9.0 libraries that are already in wide distribution and because there is not apparent difference in function or performance at the NOVAS level.