using ASCOM.Utilities;
using System;
using System.Reflection;

// NOVAS2 component implementation
// This class is a public front that simply calls the relevant 32 or 64bit funciton in compiled C code

// Version 5.5.6 is identical to the component shipped with Platform 5.5. except that the location of the support
// files has been updated to match the new ASCOM\Astrometry Platform 6 location and the code is now able to find the copy
// in the 32bit part of the file system when running on a 64bit system. There are no interface or function changes in this version.

using System.Runtime.InteropServices;
// See the Is64Bit function for code that enables this assembly to find the C NOVAS DLLs

namespace ASCOM.Astrometry.NOVAS
{

    /// <summary>
    /// NOVAS: Class presenting the contents of the NOVAS 2 library. 
    /// NOVAS was developed by the Astronomical Applications department of the United States Naval 
    /// Observatory. The C language version of NOVAS was developed by John Bangert at USNO/AA.
    /// </summary>
    /// <remarks>
    /// The NOVAS class is a STATIC class and is the component of preference for .NET programmers. 
    /// This means that you do not have to create an instance of the 
    /// class in order to access its members. Instead you reference them directly from the class. So, this works:
    /// <code>rc = ASCOM.Astrometry.NOVAS2.AppStar(tjd, earth, star, ra, dec)</code> 
    /// while this does not work: 
    /// <code>
    /// Dim Nov as New ASCOM.Astrometry.NOVAS2
    /// rc = Nov.AppStar(tjd, earth, star, ra, dec)
    /// </code>
    /// <para>Method names are identical to those used in NOVAS2, as are almost all paramaters. There are a few 
    /// changes that introduce some new structures but these should be self explanatory. One significant difference 
    /// is that position and velocity vectors are returned as structures rather than double arrays. This was done 
    /// to make type checking more effective.</para>
    /// <para>Testing of the high level supervisory functions has been carried out using real-time star data from
    /// the USNO web site. Values provided by this NOVAS2 implementation agree on average to about 50 milli 
    /// arc-seconds with current USNO web site values.</para>
    /// <para>This class is implemented using a thin layer of .NET code that calls functions in 
    /// either a 32 or 64 bit compiled version of the unmodified C code from ther USNO web site. The .NET code
    /// does not carry out calculations itself, it simply handles any interface presentation differences
    /// and calls the relevant 32 or 64bit code according to its environment.</para>
    /// <para><b>Note: </b> This class only supports Earth in the XXXXPlanet classes, which is a consequence of the implementation 
    /// used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
    /// can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.</para>
    /// </remarks>
    [ComVisible(false)]
    [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
    public class NOVAS2 // Static classes cannot be exposed through COM
    {

        private const string NOVAS32Dll = "NOVAS-C.dll";
        private const string NOVAS64Dll = "NOVAS-C64.dll";
        private const string NOVAS_DLL_LOCATION = @"\ASCOM\Astrometry"; // This is appended to the Common Files path

        /// <summary>
        /// Static initialiser called once per AppDomain to log the component name.
        /// </summary>
        static NOVAS2()
        {
            Log.Component(Assembly.GetExecutingAssembly().FullName, "NOVAS2");
        }

        #region Private Structures
        // Version of marshaling required by the DLLs
        // This structure is an analogue of the public BodyDescription structure except that integer items
        // Are replaced with short integer items. This allows the caller to work in integer values while
        // the NOVAS C routines receive the expected short integers.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct BodyDescriptionShort
        {
            public short Type;
            public short Number;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string Name; // char[100]
        }

        // Version of marshaling required by the DLLs
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct CatEntryNOVAS2
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Catalog; // char[4]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
            public string StarName; // char[51]
            public int StarNumber;
            public double RA;
            public double Dec;
            public double ProMoRA;
            public double ProMoDec;
            public double Parallax;
            public double RadialVelocity;
        }

        #endregion

        #region Public Interface
        /// <summary>
        /// Computes the apparent place of a star 
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date for apparent place.</param>
        /// <param name="earth">Structure containing the body designation for the earth</param>
        /// <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units (defined in novas.h).</param>
        /// <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        /// <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        /// <returns><pre>
        /// 0...Everything OK
        /// >0...Error code from function 'solarsystem'.</pre></returns>
        /// <remarks></remarks>
        public static short AppStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)
        {
            if (Is64Bit())
            {
                var argearth = BodyDescToShort(earth);
                var argstar = CatEntryToCatEntryNOVAS2(star);
                return app_star64(tjd, ref argearth, ref argstar, ref ra, ref dec);
            }
            else
            {
                var argearth1 = BodyDescToShort(earth);
                var argstar1 = CatEntryToCatEntryNOVAS2(star);
                return app_star32(tjd, ref argearth1, ref argstar1, ref ra, ref dec);
            }
        }
        /// <summary>
        /// Computes the topocentric place of a star
        /// </summary>
        /// <param name="tjd"> TT (or TDT) Julian date for topocentric place.</param>
        /// <param name="earth">Structure containing the body designation for the Earth.</param>
        /// <param name="deltat">Difference TT (or TDT)-UT1 at 'tjd', in seconds.</param>
        /// <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units.</param>
        /// <param name="location">Structure containing observer's location</param>
        /// <param name="ra">OUT: Topocentric right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        /// <param name="dec">OUT: Topocentric declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...Error code from function 'solarsystem'.</pre></returns>
        /// <remarks></remarks>
        public static short TopoStar(double tjd, ref BodyDescription earth, double deltat, ref CatEntry star, ref SiteInfo location, ref double ra, ref double dec)
        {
            if (Is64Bit())
            {
                var argearth = BodyDescToShort(earth);
                var argstar = CatEntryToCatEntryNOVAS2(star);
                return topo_star64(tjd, ref argearth, deltat, ref argstar, ref location, ref ra, ref dec);
            }
            else
            {
                var argearth1 = BodyDescToShort(earth);
                var argstar1 = CatEntryToCatEntryNOVAS2(star);
                return topo_star32(tjd, ref argearth1, deltat, ref argstar1, ref location, ref ra, ref dec);
            }

        }
        /// <summary>
        /// Compute the apparent place of a planet or other solar system body.
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date for apparent place.</param>
        /// <param name="ss_object">Structure containing the body designation for the solar system body</param>
        /// <param name="earth">Structure containing the body designation for the Earth</param>
        /// <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        /// <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        /// <param name="dis">OUT: True distance from Earth to planet at 'tjd' in AU.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...See error description in function 'ephemeris'.
        /// </pre></returns>
        /// <remarks>
        /// <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        /// used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        /// can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        /// </remarks>
        public static short AppPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
        {

            if (Is64Bit())
            {
                var argss_object = BodyDescToShort(ss_object);
                var argearth = BodyDescToShort(earth);
                return app_planet64(tjd, ref argss_object, ref argearth, ref ra, ref dec, ref dis);
            }
            else
            {
                var argss_object1 = BodyDescToShort(ss_object);
                var argearth1 = BodyDescToShort(earth);
                return app_planet32(tjd, ref argss_object1, ref argearth1, ref ra, ref dec, ref dis);
            }
        }
        /// <summary>
        /// Computes the topocentric place of a planet, given the location of the observer.
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date for topocentric place.</param>
        /// <param name="ss_object">structure containing the body designation for the solar system body</param>
        /// <param name="earth">structure containing the body designation for the Earth</param>
        /// <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        /// <param name="location">structure containing observer's location</param>
        /// <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        /// <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        /// <param name="dis">OUT: True distance from Earth to planet at 'tjd' in AU.</param>
        /// <returns> <pre>
        /// 0...Everything OK.
        /// >0...See error description in function 'ephemeris'.
        /// </pre></returns>
        /// <remarks>
        /// <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        /// used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        /// can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        /// </remarks>
        public static short TopoPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis)
        {
            if (Is64Bit())
            {
                var argss_object = BodyDescToShort(ss_object);
                var argearth = BodyDescToShort(earth);
                return topo_planet64(tjd, ref argss_object, ref argearth, deltat, ref location, ref ra, ref dec, ref dis);
            }
            else
            {
                var argss_object1 = BodyDescToShort(ss_object);
                var argearth1 = BodyDescToShort(earth);
                return topo_planet32(tjd, ref argss_object1, ref argearth1, deltat, ref location, ref ra, ref dec, ref dis);
            }
        }
        /// <summary>
        /// Computes the virtual place of a star
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date for virtual place.</param>
        /// <param name="earth">Pointer to structure containing the body designation for the Earth.</param>
        /// <param name="star">Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        /// <param name="ra">OUT: Virtual right ascension in hours, referred to mean equator and equinox of J2000.</param>
        /// <param name="dec">OUT: Virtual declination in degrees, referred to mean equator and equinox of J2000.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...Error code from function 'solarsystem'
        /// </pre></returns>
        /// <remarks>
        /// Computes the virtual place of a star at date 'tjd', given its 
        /// mean place, proper motion, parallax, and radial velocity for J2000.0.</remarks>
        public static short VirtualStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)        {
            if (Is64Bit())
            {
                var argearth = BodyDescToShort(earth);
                var argstar = CatEntryToCatEntryNOVAS2(star);
                return virtual_star64(tjd, ref argearth, ref argstar, ref ra, ref dec);
            }
            else
            {
                var argearth1 = BodyDescToShort(earth);
                var argstar1 = CatEntryToCatEntryNOVAS2(star);
                return virtual_star32(tjd, ref argearth1, ref argstar1, ref ra, ref dec);
            }
        }
        /// <summary>
        /// Computes the local place of a star
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date for local place.</param>
        /// <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        /// <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        /// <param name="star">Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        /// <param name="location">Pointer to structure containing observer's location</param>
        /// <param name="ra">OUT: Local right ascension in hours, referred to mean equator and equinox of J2000.</param>
        /// <param name="dec">OUT: Local declination in degrees, referred to mean equator and equinox of J2000.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...Error code from function 'solarsystem'.
        /// </pre></returns>
        /// <remarks></remarks>
        public static short LocalStar(double tjd, ref BodyDescription earth, double deltat, ref CatEntry star, ref SiteInfo location, ref double ra, ref double dec)
        {
            if (Is64Bit())
            {
                var argearth = BodyDescToShort(earth);
                var argstar = CatEntryToCatEntryNOVAS2(star);
                return local_star64(tjd, ref argearth, deltat, ref argstar, ref location, ref ra, ref dec);
            }
            else
            {
                var argearth1 = BodyDescToShort(earth);
                var argstar1 = CatEntryToCatEntryNOVAS2(star);
                return local_star32(tjd, ref argearth1, deltat, ref argstar1, ref location, ref ra, ref dec);
            }
        }
        /// <summary>
        /// Computes the virtual place of a planet or other solar system body.
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date for virtual place.</param>
        /// <param name="ss_object">Pointer to structure containing the body designation for the solar system body</param>
        /// <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        /// <param name="ra">OUT: Virtual right ascension in hours, referred to mean equator and equinox of J2000.</param>
        /// <param name="dec">OUT: Virtual declination in degrees, referred to mean equator and equinox of J2000.</param>
        /// <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...See error description in function 'ephemeris'.
        /// </pre></returns>
        /// <remarks>
        /// <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        /// used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        /// can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        /// </remarks>
        public static short VirtualPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
        {
            if (Is64Bit())
            {
                var argss_object = BodyDescToShort(ss_object);
                var argearth = BodyDescToShort(earth);
                return virtual_planet64(tjd, ref argss_object, ref argearth, ref ra, ref dec, ref dis);
            }
            else
            {
                var argss_object1 = BodyDescToShort(ss_object);
                var argearth1 = BodyDescToShort(earth);
                return virtual_planet32(tjd, ref argss_object1, ref argearth1, ref ra, ref dec, ref dis);
            }
        }
        /// <summary>
        /// Computes the local place of a planet or other solar system body, given the location of the observer.
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date for local place.</param>
        /// <param name="ss_object"> Pointer to structure containing the body designation for the solar system body</param>
        /// <param name="earth"> Pointer to structure containing the body designation for the Earth</param>
        /// <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        /// <param name="location"> Pointer to structure containing observer's location</param>
        /// <param name="ra">OUT: Local right ascension in hours, referred to mean equator and equinox of J2000.</param>
        /// <param name="dec">OUT: Local declination in degrees, referred to mean equator and equinox of J2000.</param>
        /// <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...See error description in function 'ephemeris'.
        /// </pre></returns>
        /// <remarks>
        /// <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        /// used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        /// can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        /// </remarks>
        public static short LocalPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis)
        {
            if (Is64Bit())
            {
                var argss_object = BodyDescToShort(ss_object);
                var argearth = BodyDescToShort(earth);
                return local_planet64(tjd, ref argss_object, ref argearth, deltat, ref location, ref ra, ref dec, ref dis);
            }
            else
            {
                var argss_object1 = BodyDescToShort(ss_object);
                var argearth1 = BodyDescToShort(earth);
                return local_planet32(tjd, ref argss_object1, ref argearth1, deltat, ref location, ref ra, ref dec, ref dis);
            }
        }
        /// <summary>
        /// Computes the astrometric place of a star
        /// </summary>
        /// <param name="tjd">  TT (or TDT) Julian date for astrometric place.</param>
        /// <param name="earth"> Pointer to structure containing the body designation for the Earth</param>
        /// <param name="star"> Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        /// <param name="ra">OUT:  Astrometric right ascension in hours, referred to mean equator and equinox of J2000.</param>
        /// <param name="dec">OUT:  Astrometric declination in degrees, referred to mean equator and equinox of J2000.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...Error code from function 'solarsystem'.
        /// </pre></returns>
        /// <remarks>     Computes the astrometric place of a star, given its mean place, proper motion, parallax, and radial velocity for J2000.0.</remarks>
        public static short AstroStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)        {
            if (Is64Bit())
            {
                var argearth = BodyDescToShort(earth);
                var argstar = CatEntryToCatEntryNOVAS2(star);
                return astro_star64(tjd, ref argearth, ref argstar, ref ra, ref dec);
            }
            else
            {
                var argearth1 = BodyDescToShort(earth);
                var argstar1 = CatEntryToCatEntryNOVAS2(star);
                return astro_star32(tjd, ref argearth1, ref argstar1, ref ra, ref dec);
            }
        }
        /// <summary>
        /// Computes the astrometric place of a planet or other solar system body.
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date for calculation.</param>
        /// <param name="ss_object">Pointer to structure containing the body designation for the solar system body</param>
        /// <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        /// <param name="ra">OUT: Astrometric right ascension in hours, referred to mean equator and equinox of J2000.</param>
        /// <param name="dec">OUT:  Astrometric declination in degrees, referred to mean equator and equinox of J2000.</param>
        /// <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...See error description in function 'ephemeris'.</pre></returns>
        /// <remarks>
        /// <b>Note: </b>This function only supports Earth, which is a consequence of the implementation 
        /// used. Please use the NOVAS3.1 or later classes in applications that require planetary or moon ephemeredes as these classes 
        /// can access the JPL 421 planetary ephemeris data provided as part of the ASCOM distribution.
        /// </remarks>
        public static short AstroPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
        {
            if (Is64Bit())
            {
                var argss_object = BodyDescToShort(ss_object);
                var argearth = BodyDescToShort(earth);
                return astro_planet64(tjd, ref argss_object, ref argearth, ref ra, ref dec, ref dis);
            }
            else
            {
                var argss_object1 = BodyDescToShort(ss_object);
                var argearth1 = BodyDescToShort(earth);
                return astro_planet32(tjd, ref argss_object1, ref argearth1, ref ra, ref dec, ref dis);
            }
        }
        /// <summary>
        /// Transform apparent equatorial coordinates to horizon coordinates
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date.</param>
        /// <param name="deltat">Difference TT (or TDT)-UT1 at 'tjd', in seconds.</param>
        /// <param name="x">Conventionally-defined x coordinate of celestial ephemeris  pole with respect to IERS reference pole, in arcseconds. </param>
        /// <param name="y">Conventionally-defined y coordinate of celestial ephemeris  pole with respect to IERS reference pole, in arcseconds.</param>
        /// <param name="location">structure containing observer's location</param>
        /// <param name="ra"> Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date.</param>
        /// <param name="dec">Topocentric declination of object of interest, in degrees,  referred to true equator and equinox of date.</param>
        /// <param name="ref_option">Refraction option</param>
        /// <param name="zd">OUT: Topocentric zenith distance in degrees, affected by  refraction if 'ref_option' is non-zero.</param>
        /// <param name="az">OUT: Topocentric azimuth (measured east from north) in degrees.</param>
        /// <param name="rar">OUT: Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        /// <param name="decr">OUT: Topocentric declination of object of interest, in degrees, referred to true equator and equinox of date, affected by  refraction if 'ref_option' is non-zero.</param>
        /// <remarks>This function transforms apparent equatorial coordinates (right 
        /// ascension and declination) to horizon coordinates (zenith 
        /// distance and azimuth).  It uses a method that properly accounts 
        /// for polar motion, which is significant at the sub-arcsecond 
        /// level.  This function can also adjust coordinates for atmospheric 
        /// refraction.</remarks>
        public static void Equ2Hor(double tjd, double deltat, double x, double y, ref SiteInfo location, double ra, double dec, RefractionOption ref_option, ref double zd, ref double az, ref double rar, ref double decr)
        {
            if (Is64Bit())
            {
                equ2hor64(tjd, deltat, x, y, ref location, ra, dec, (short)ref_option, ref zd, ref az, ref rar, ref decr);
            }
            else
            {
                equ2hor32(tjd, deltat, x, y, ref location, ra, dec, (short)ref_option, ref zd, ref az, ref rar, ref decr);
            }
        }
        /// <summary>
        /// To convert Hipparcos data at epoch J1991.25 to epoch J2000.0 and FK5-style units.
        /// </summary>
        /// <param name="hipparcos">An entry from the Hipparcos catalog, at epoch J1991.25, with all members having Hipparcos catalog units.  See Note 1 below</param>
        /// <param name="fk5">The transformed input entry, at epoch J2000.0, with all  members having FK5 catalog units.  See Note 2 below</param>
        /// <remarks>To be used only for Hipparcos or Tycho stars  with linear space motion.
        /// <para><pre>
        /// 1. Hipparcos epoch and units:
        ///    Epoch: J1991.25
        ///    Right ascension (RA): degrees
        ///    Declination (Dec): degrees
        ///    Proper motion in RA * cos (Dec): milliarcseconds per year
        ///    Proper motion in Dec: milliarcseconds per year
        ///    Parallax: milliarcseconds
        ///    Radial velocity: kilometers per second (not in catalog)
        /// 
        /// 2. FK5 epoch and units:
        ///    Epoch: J2000.0
        ///    Right ascension: hours
        ///    Declination: degrees
        ///    Proper motion in RA: seconds of time per Julian century
        ///    Proper motion in Dec: arcseconds per Julian century
        ///    Parallax: arcseconds
        ///    Radial velocity: kilometers per second
        /// </pre></para></remarks>
        public static void TransformHip(ref CatEntry hipparcos, ref CatEntry fk5)
        {
            var CEN2 = new CatEntryNOVAS2();

            if (Is64Bit())
            {
                var arghipparcos = CatEntryToCatEntryNOVAS2(hipparcos);
                transform_hip64(ref arghipparcos, ref CEN2);
            }
            else
            {
                var arghipparcos1 = CatEntryToCatEntryNOVAS2(hipparcos);
                transform_hip32(ref arghipparcos1, ref CEN2);
            }
            CatEntryNOVAS2ToCatEntry(CEN2, ref fk5); // Transform to external CatEntry presentation
        }
        /// <summary>
        /// To transform a star's catalog quantities for a change of epoch and/or equator and equinox.
        /// </summary>
        /// <param name="option">Transformation option<pre>
        ///    = 1 ... change epoch; same equator and equinox
        ///    = 2 ... change equator and equinox; same epoch
        ///    = 3 ... change equator and equinox and epoch
        /// </pre></param>
        /// <param name="date_incat">TT Julian date, or year, of input catalog data.</param>
        /// <param name="incat">An entry from the input catalog</param>
        /// <param name="date_newcat">TT Julian date, or year, of transformed catalog data.</param>
        /// <param name="newcat_id">Three-character abbreviated name of the transformed catalog.</param>
        /// <param name="newcat">OUT: The transformed catalog entry</param>
        /// <remarks><pre>
        /// 1. 'date_incat' and 'date_newcat' may be specified either as a 
        ///    Julian date (e.g., 2433282.5) or a Julian year and fraction 
        ///    (e.g., 1950.0).  Values less than 10000 are assumed to be years.
        /// 
        /// 2. option = 1 updates the star's data to account for the star's space motion between 
        ///               the first and second dates, within a fixed reference frame.
        ///    option = 2 applies a rotation of the reference frame corresponding to precession 
        ///               between the first and second dates, but leaves the star fixed in space.
        ///    option = 3 provides both transformations.
        /// 
        /// 3. This subroutine cannot be properly used to bring data from 
        ///    old (pre-FK5) star catalogs into the modern system, because old 
        ///    catalogs were compiled using a set of constants that are 
        ///    incompatible with the IAU (1976) system.
        /// 
        /// 4. This function uses TDB Julian dates internally, but no 
        ///    distinction between TDB and TT is necessary.
        /// </pre></remarks>
        public static void TransformCat(TransformationOption option, double date_incat, ref CatEntry incat, double date_newcat, ref byte[] newcat_id, ref CatEntry newcat)
        {
            var CEN2 = new CatEntryNOVAS2();

            if (Is64Bit())
            {
                var argincat = CatEntryToCatEntryNOVAS2(incat);
                transform_cat64((short)option, date_incat, ref argincat, date_newcat, ref newcat_id, ref CEN2);
            }
            else
            {
                var argincat1 = CatEntryToCatEntryNOVAS2(incat);
                transform_cat32((short)option, date_incat, ref argincat1, date_newcat, ref newcat_id, ref CEN2);
            }
            CatEntryNOVAS2ToCatEntry(CEN2, ref newcat); // Transform to external CatEntry presentation
        }
        /// <summary>
        /// Computes the Greenwich apparent sidereal time, at Julian date 'jd_high' + 'jd_low'.
        /// </summary>
        /// <param name="jd_high">Julian date, integral part.</param>
        /// <param name="jd_low">Julian date, fractional part.</param>
        /// <param name="ee"> Equation of the equinoxes (seconds of time). [Note: this  quantity is computed by function 'earthtilt'.]</param>
        /// <param name="gst">Greenwich apparent sidereal time, in hours.</param>
        /// <remarks></remarks>
        public static void SiderealTime(double jd_high, double jd_low, double ee, ref double gst)


        {
            if (Is64Bit())
            {
                sidereal_time64(jd_high, jd_low, ee, ref gst);
            }
            else
            {
                sidereal_time32(jd_high, jd_low, ee, ref gst);
            }
        }
        /// <summary>
        /// Precesses equatorial rectangular coordinates from one epoch to another.
        /// </summary>
        /// <param name="tjd1">TDB Julian date of first epoch.</param>
        /// <param name="pos">Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of first epoch.</param>
        /// <param name="tjd2">TDB Julian date of second epoch.</param>
        /// <param name="pos2">OUT: Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of second epoch.</param>
        /// <remarks>The coordinates are referred to the mean equator and equinox of the two respective epochs.</remarks>
        public static void Precession(double tjd1, double[] pos, double tjd2, ref double[] pos2)
        {
            var posv2 = new PosVector();
            if (Is64Bit())
            {
                var argpos = ArrToPosVec(pos);
                precession64(tjd1, ref argpos, tjd2, ref posv2);
            }
            else
            {
                var argpos1 = ArrToPosVec(pos);
                precession32(tjd1, ref argpos1, tjd2, ref posv2);
            }
            PosVecToArr(posv2, ref pos2);
        }
        /// <summary>
        /// Computes quantities related to the orientation of the Earth's rotation axis at Julian date 'tjd'.
        /// </summary>
        /// <param name="tjd">TDB Julian date of the desired time</param>
        /// <param name="mobl">OUT:  Mean obliquity of the ecliptic in degrees at 'tjd'.</param>
        /// <param name="tobl">OUT: True obliquity of the ecliptic in degrees at 'tjd'.</param>
        /// <param name="eq">OUT: Equation of the equinoxes in seconds of time at 'tjd'.</param>
        /// <param name="dpsi">OUT: Nutation in longitude in arcseconds at 'tjd'.</param>
        /// <param name="deps">OUT: Nutation in obliquity in arcseconds at 'tjd'.</param>
        /// <remarks></remarks>
        public static void EarthTilt(double tjd, ref double mobl, ref double tobl, ref double eq, ref double dpsi, ref double deps)
        {
            if (Is64Bit())
            {
                earthtilt64(tjd, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
            }
            else
            {
                earthtilt32(tjd, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
            }
        }
        /// <summary>
        /// This function allows for the specification of celestial pole offsets for high-precision applications.  
        /// </summary>
        /// <param name="del_dpsi">Value of offset in delta psi (dpsi) in arcseconds.</param>
        /// <param name="del_deps">Value of offset in delta epsilon (deps) in arcseconds.</param>
        /// <remarks>These are added to the nutation parameters delta psi and delta epsilon.
        /// <para>1. This function sets the values of global variables 'PSI_COR'and 'EPS_COR' declared at the top of file 'novas.c'.  These global variables are used only in NOVAS function 'earthtilt'.</para>
        /// <para>2. This function, if used, should be called before any other NOVAS functions for a given date.  Values of the pole offsets specified via a call to this function will be used until explicitly changed.</para>
        /// <para>3. Daily values of the offsets are published, for example, in IERS Bulletins A and B.</para>
        /// <para>4. This function is the "C" version of Fortran NOVAS routine "celpol".</para>
        /// </remarks>
        public static void CelPole(double del_dpsi, double del_deps)
        {
            if (Is64Bit())
            {
                cel_pole64(del_dpsi, del_deps);
            }
            else
            {
                cel_pole32(del_dpsi, del_deps);
            }
        }
        /// <summary>
        /// Retrieves the position and velocity of a body from a fundamental ephemeris.
        /// </summary>
        /// <param name="tjd">TDB Julian date.</param>
        /// <param name="cel_obj">Structure containing the designation of the body of interest</param>
        /// <param name="origin">Origin point (solar system barycentre or centre of mass of the Sun</param>
        /// <param name="pos">OUT: Position vector of 'body' at tjd; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        /// <param name="vel">OUT: Velocity vector of 'body' at tjd; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        /// <returns><pre>
        /// 0    ... Everything OK.
        /// 1    ... Invalid value of 'origin'.
        /// 2    ... Invalid value of 'type' in 'cel_obj'.
        /// 3    ... Unable to allocate memory.
        /// 10+n ... where n is the error code from 'solarsystem'.
        /// 20+n ... where n is the error code from 'readeph'.</pre></returns>
        /// <remarks></remarks>
        public static short Ephemeris(double tjd, ref BodyDescription cel_obj, Origin origin, ref double[] pos, ref double[] vel)        {
            var posv = new PosVector();
            var velv = new VelVector();
            short rc;
            if (Is64Bit())
            {
                var argcel_obj = BodyDescToShort(cel_obj);
                rc = ephemeris64(tjd, ref argcel_obj, (short)origin, ref posv, ref velv);
            }
            else
            {
                var argcel_obj1 = BodyDescToShort(cel_obj);
                rc = ephemeris32(tjd, ref argcel_obj1, (short)origin, ref posv, ref velv);
            }
            PosVecToArr(posv, ref pos);
            VelVecToArr(velv, ref vel);
            return rc;
        }
        /// <summary>
        /// Provides the position and velocity of the Earth
        /// </summary>
        /// <param name="tjd">TDB Julian date.</param>
        /// <param name="body">Body identification number.
        /// <pre>
        /// Set 'body' = 0 or 'body' = 1 or 'body' = 10 for the Sun.
        /// Set 'body' = 2 or 'body' = 3 for the Earth.
        /// </pre></param>
        /// <param name="origin">Required origin: solar system barycenter or center of mass of the Sun</param>
        /// <param name="pos">OUT: Position vector of 'body' at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        /// <param name="vel">OUT: Velocity vector of 'body' at 'tjd'; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// 1...Input Julian date ('tjd') out of range.
        /// 2...Invalid value of 'body'.
        /// </pre></returns>
        /// <remarks> Provides the position and velocity of the Earth at epoch 'tjd' by evaluating a closed-form theory without reference to an  external file.  This function can also provide the position and velocity of the Sun.</remarks>
        public static short SolarSystem(double tjd, Body body, Origin origin, ref double[] pos, ref double[] vel)        {
            var posv = new PosVector();
            var velv = new VelVector();
            short rc;
            if (Is64Bit())
            {
                rc = solarsystem64(tjd, (short)body, (short)origin, ref posv, ref velv);
            }
            else
            {
                rc = solarsystem32(tjd, (short)body, (short)origin, ref posv, ref velv);
            }
            PosVecToArr(posv, ref pos);
            VelVecToArr(velv, ref vel);
            return rc;
        }
        /// <summary>
        /// Converts an vector in equatorial rectangular coordinates to equatorial spherical coordinates.
        /// </summary>
        /// <param name="pos">Position vector, equatorial rectangular coordinates.</param>
        /// <param name="ra">OUT: Right ascension in hours.</param>
        /// <param name="dec">OUT: Declination in degrees.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// 1...All vector components are zero; 'ra' and 'dec' are indeterminate.
        /// 2...Both vec[0] and vec[1] are zero, but vec[2] is nonzero; 'ra' is indeterminate.</pre>
        /// </returns>
        /// <remarks></remarks>
        public static short Vector2RADec(double[] pos, ref double ra, ref double dec)

        {
            short rc;
            if (Is64Bit())
            {
                var argpos = ArrToPosVec(pos);
                rc = vector2radec64(ref argpos, ref ra, ref dec);
            }
            else
            {
                var argpos1 = ArrToPosVec(pos);
                rc = vector2radec32(ref argpos1, ref ra, ref dec);
            }
            return rc;
        }

        /// <summary>
        /// Converts angular quanities for stars to vectors.
        /// </summary>
        /// <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units </param>
        /// <param name="pos">Position vector, equatorial rectangular coordinates, components in AU.</param>
        /// <param name="vel">Velocity vector, equatorial rectangular coordinates, components in AU/Day.</param>
        /// <remarks></remarks>
        public static void StarVectors(CatEntry star, ref double[] pos, ref double[] vel)
        {
            var posv = new PosVector();
            var velv = new VelVector();
            if (Is64Bit())
            {
                var argstar = CatEntryToCatEntryNOVAS2(star);
                starvectors64(ref argstar, ref posv, ref velv);
            }
            else
            {
                var argstar1 = CatEntryToCatEntryNOVAS2(star);
                starvectors32(ref argstar1, ref posv, ref velv);
            }
            PosVecToArr(posv, ref pos);
            VelVecToArr(velv, ref vel);
        }

        /// <summary>
        /// Converts equatorial spherical coordinates to a vector (equatorial rectangular coordinates).
        /// </summary>
        /// <param name="ra">Right ascension (hours).</param>
        /// <param name="dec">Declination (degrees).</param>
        /// <param name="dist">Distance</param>
        /// <param name="pos">Position vector, equatorial rectangular coordinates (AU).</param>
        /// <remarks></remarks>
        public static void RADec2Vector(double ra, double dec, double dist, ref double[] pos)


        {
            var posv = new PosVector();
            if (Is64Bit())
            {
                radec2vector64(ra, dec, dist, ref posv);
            }
            else
            {
                radec2vector32(ra, dec, dist, ref posv);
            }
            PosVecToArr(posv, ref pos);
        }

        /// <summary>
        /// Obtains the barycentric and heliocentric positions and velocities of the Earth from the solar system ephemeris.
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date.</param>
        /// <param name="earth">Structure containing the body designation for the Earth.</param>
        /// <param name="tdb">OUT: TDB Julian date corresponding to 'tjd'.</param>
        /// <param name="bary_earthp">OUT: Barycentric position vector of Earth at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        /// <param name="bary_earthv">OUT: Barycentric velocity vector of Earth at 'tjd'; equatorial rectangular system referred to the mean equator and equinox  of J2000.0, in AU/Day.</param>
        /// <param name="helio_earthp">OUT: Heliocentric position vector of Earth at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        /// <param name="helio_earthv">OUT: Heliocentric velocity vector of Earth at 'tjd'; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        /// <returns><pre>
        /// 0...Everything OK.
        /// >0...Error code from function 'solarsystem'.</pre>
        /// </returns>
        /// <remarks></remarks>
        public static short GetEarth(double tjd, ref BodyDescription earth, ref double tdb, ref double[] bary_earthp, ref double[] bary_earthv, ref double[] helio_earthp, ref double[] helio_earthv)
        {
            short rc;
            PosVector vbary_earthp = new(), vhelio_earthp = new();
            VelVector vbary_earthv = new(), vhelio_earthv = new();
            if (Is64Bit())
            {
                var argearth = BodyDescToShort(earth);
                rc = get_earth64(tjd, ref argearth, ref tdb, ref vbary_earthp, ref vbary_earthv, ref vhelio_earthp, ref vhelio_earthv);
            }
            else
            {
                var argearth1 = BodyDescToShort(earth);
                rc = get_earth32(tjd, ref argearth1, ref tdb, ref vbary_earthp, ref vbary_earthv, ref vhelio_earthp, ref vhelio_earthv);
            }
            PosVecToArr(vbary_earthp, ref bary_earthp);
            VelVecToArr(vbary_earthv, ref bary_earthv);
            PosVecToArr(vhelio_earthp, ref helio_earthp);
            VelVecToArr(vhelio_earthv, ref helio_earthv);
            return rc;
        }
        // 
        // START OF NEW
        // 
        /// <summary>
        /// Computes the mean place of a star for J2000.0
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date of apparent place.</param>
        /// <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        /// <param name="ra">Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        /// <param name="dec"> Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        /// <param name="mra">OUT: Mean right ascension J2000.0 in hours.</param>
        /// <param name="mdec">OUT: Mean declination J2000.0 in degrees.</param>
        /// <returns><pre>
        ///   0...Everything OK.
        ///   1...Iterative process did not converge after 20 iterations.
        /// >10...Error from function 'app_star'.</pre></returns>
        /// <remarks>Computes the mean place of a star for J2000.0, given its apparent 
        /// place at date 'tjd'.  Proper motion, parallax and radial velocity 
        /// are assumed to be zero.
        /// </remarks>
        public static short MeanStar(double tjd, ref BodyDescription earth, double ra, double dec, ref double mra, ref double mdec)
        {
            short rc;
            if (Is64Bit())
            {
                var argearth = BodyDescToShort(earth);
                rc = mean_star64(tjd, ref argearth, ra, dec, ref mra, ref mdec);
            }
            else
            {
                var argearth1 = BodyDescToShort(earth);
                rc = mean_star32(tjd, ref argearth1, ra, dec, ref mra, ref mdec);
            }
            return rc;
        }
        /// <summary>
        /// Transforms a vector from an Earth-fixed geographic system to a space-fixed system
        /// </summary>
        /// <param name="tjd">TT (or TDT) Julian date</param>
        /// <param name="gast">Greenwich apparent sidereal time, in hours.</param>
        /// <param name="x"> Conventionally-defined X coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        /// <param name="y">Conventionally-defined Y coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        /// <param name="vece"> Vector in geocentric rectangular Earth-fixed system, referred to geographic equator and Greenwich meridian.</param>
        /// <param name="vecs">OUT: Vector in geocentric rectangular space-fixed system, referred to mean equator and equinox of J2000.0.</param>
        /// <remarks>Transforms a vector from an Earth-fixed geographic system to a space-fixed system based on mean equator and equinox of J2000.0; applies rotations for wobble, spin, nutation, and precession.</remarks>
        public static void Pnsw(double tjd, double gast, double x, double y, double[] vece, ref double[] vecs)
        {
            var vvecs = new PosVector();
            if (Is64Bit())
            {
                var argvece = ArrToPosVec(vece);
                pnsw64(tjd, gast, x, y, ref argvece, ref vvecs);
            }
            else
            {
                var argvece1 = ArrToPosVec(vece);
                pnsw32(tjd, gast, x, y, ref argvece1, ref vvecs);
            }
            PosVecToArr(vvecs, ref vecs);
        }
        /// <summary>
        /// Transforms geocentric rectangular coordinates from rotating system to non-rotating system
        /// </summary>
        /// <param name="st">Local apparent sidereal time at reference meridian, in hours.</param>
        /// <param name="pos1">Vector in geocentric rectangular rotating system, referred to rotational equator and orthogonal reference meridian.</param>
        /// <param name="pos2">OUT: Vector in geocentric rectangular non-rotating system, referred to true equator and equinox of date.</param>
        /// <remarks>Transforms geocentric rectangular coordinates from rotating system based on rotational equator and orthogonal reference meridian to  non-rotating system based on true equator and equinox of date.</remarks>
        public static void Spin(double st, double[] pos1, ref double[] pos2)

        {
            var vpos2 = new PosVector();
            if (Is64Bit())
            {
                var argpos1 = ArrToPosVec(pos1);
                spin64(st, ref argpos1, ref vpos2);
            }
            else
            {
                var argpos11 = ArrToPosVec(pos1);
                spin32(st, ref argpos11, ref vpos2);
            }
            PosVecToArr(vpos2, ref pos2);
        }

        /// <summary>
        /// Corrects Earth-fixed geocentric rectangular coordinates for polar motion.
        /// </summary>
        /// <param name="x"> Conventionally-defined X coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        /// <param name="y">Conventionally-defined Y coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        /// <param name="pos1">Vector in geocentric rectangular Earth-fixed system, referred to geographic equator and Greenwich meridian.</param>
        /// <param name="pos2">OUT: Vector in geocentric rectangular rotating system, referred to rotational equator and orthogonal Greenwich meridian</param>
        /// <remarks>Corrects Earth-fixed geocentric rectangular coordinates for polar motion.  Transforms a vector from Earth-fixed geographic system to rotating system based on rotational equator and orthogonal Greenwich meridian through axis of rotation.</remarks>
        public static void Wobble(double x, double y, double[] pos1, ref double[] pos2)


        {
            var vpos2 = new PosVector();
            if (Is64Bit())
            {
                var argpos1 = ArrToPosVec(pos1);
                wobble64(x, y, ref argpos1, ref vpos2);
            }
            else
            {
                var argpos11 = ArrToPosVec(pos1);
                wobble32(x, y, ref argpos11, ref vpos2);
            }
            PosVecToArr(vpos2, ref pos2);
        }
        /// <summary>
        /// Computes the position and velocity vectors of a terrestrial observer with respect to the center of the Earth.
        /// </summary>
        /// <param name="locale">Longitude, latitude and height of the observer (in a SiteInfoStruct)</param>
        /// <param name="st">Local apparent sidereal time at reference meridian in hours.</param>
        /// <param name="pos"> Position vector of observer with respect to center of Earth, equatorial rectangular coordinates, referred to true equator and equinox of date, components in AU.</param>
        /// <param name="vel"> Velocity vector of observer with respect to center of Earth, equatorial rectangular coordinates, referred to true equator and equinox of date, components in AU/Day.</param>
        /// <remarks></remarks>
        public static void Terra(ref SiteInfo locale, double st, ref double[] pos, ref double[] vel)


        {
            var vpos = new PosVector();
            var vvel = new VelVector();
            if (Is64Bit())
            {
                terra64(ref locale, st, ref vpos, ref vvel);
            }
            else
            {
                terra32(ref locale, st, ref vpos, ref vvel);
            }
            PosVecToArr(vpos, ref pos);
            VelVecToArr(vvel, ref vel);
        }

        /// <summary>
        /// Applies proper motion, including foreshortening effects, to a star's position.
        /// </summary>
        /// <param name="tjd1">TDB Julian date of first epoch.</param>
        /// <param name="pos">Position vector at first epoch.</param>
        /// <param name="vel">Velocity vector at first epoch.</param>
        /// <param name="tjd2">TDB Julian date of second epoch.</param>
        /// <param name="pos2">OUT: Position vector at second epoch.</param>
        /// <remarks></remarks>
        public static void ProperMotion(double tjd1, double[] pos, double[] vel, double tjd2, ref double[] pos2)        {
            var vpos2 = new PosVector();
            if (Is64Bit())
            {
                var argpos = ArrToPosVec(pos);
                var argvel = ArrToVelVec(vel);
                proper_motion64(tjd1, ref argpos, ref argvel, tjd2, ref vpos2);
            }
            else
            {
                var argpos1 = ArrToPosVec(pos);
                var argvel1 = ArrToVelVec(vel);
                proper_motion32(tjd1, ref argpos1, ref argvel1, tjd2, ref vpos2);
            }
            PosVecToArr(vpos2, ref pos2);
        }
        /// <summary>
        /// Moves the origin of coordinates from the barycenter of the solar system to the center of mass of the Earth
        /// </summary>
        /// <param name="pos">Position vector, referred to origin at solar system barycenter, components in AU.</param>
        /// <param name="earthvector">Position vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU.</param>
        /// <param name="pos2">OUT: Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        /// <param name="lighttime">OUT: Light time from body to Earth in days.</param>
        /// <remarks>This corrects for parallax.</remarks>
        public static void BaryToGeo(double[] pos, double[] earthvector, ref double[] pos2, ref double lighttime)


        {
            var vpos2 = default(PosVector);
            if (Is64Bit())
            {
                var argpos = ArrToPosVec(pos);
                var argearthvector = ArrToPosVec(earthvector);
                bary_to_geo64(ref argpos, ref argearthvector, ref vpos2, ref lighttime);
            }
            else
            {
                var argpos1 = ArrToPosVec(pos);
                var argearthvector1 = ArrToPosVec(earthvector);
                bary_to_geo32(ref argpos1, ref argearthvector1, ref vpos2, ref lighttime);
            }
            PosVecToArr(vpos2, ref pos2);
        }
        /// <summary>
        /// Corrects position vector for the deflection of light in the gravitational field of the Sun. 
        /// </summary>
        /// <param name="pos">Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        /// <param name="earthvector">Position vector of center of mass of the Earth, referred to origin at center of mass of the Sun, components in AU.</param>
        /// <param name="pos2">Position vector, referred to origin at center of mass of the Earth, corrected for gravitational deflection, components in AU.</param>
        /// <returns>0...Everything OK.</returns>
        /// <remarks>This function is valid for bodies within the solar system as well as for stars.</remarks>
        public static short SunField(double[] pos, double[] earthvector, ref double[] pos2)

        {
            var vpos2 = default(PosVector);
            short rc;
            if (Is64Bit())
            {
                var argpos = ArrToPosVec(pos);
                var argearthvector = ArrToPosVec(earthvector);
                rc = sun_field64(ref argpos, ref argearthvector, ref vpos2);
            }
            else
            {
                var argpos1 = ArrToPosVec(pos);
                var argearthvector1 = ArrToPosVec(earthvector);
                rc = sun_field32(ref argpos1, ref argearthvector1, ref vpos2);
            }
            PosVecToArr(vpos2, ref pos2);
            return rc;
        }

        /// <summary>
        /// Corrects position vector for aberration of light.
        /// </summary>
        /// <param name="pos">Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        /// <param name="vel">Velocity vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU/day.</param>
        /// <param name="lighttime">Light time from body to Earth in days.</param>
        /// <param name="pos2">OUT: Position vector, referred to origin at center of mass of the Earth, corrected for aberration, components in AU</param>
        /// <returns>0...Everything OK.</returns>
        /// <remarks>Algorithm includes relativistic terms.</remarks>
        public static short Aberration(double[] pos, double[] vel, double lighttime, ref double[] pos2)


        {
            var vpos2 = default(PosVector);
            short rc;
            if (Is64Bit())
            {
                var argpos = ArrToPosVec(pos);
                var argvel = ArrToVelVec(vel);
                rc = aberration64(ref argpos, ref argvel, lighttime, ref vpos2);
            }
            else
            {
                var argpos1 = ArrToPosVec(pos);
                var argvel1 = ArrToVelVec(vel);
                rc = aberration32(ref argpos1, ref argvel1, lighttime, ref vpos2);
            }
            PosVecToArr(vpos2, ref pos2);
            return rc;
        }

        /// <summary>
        /// Nutates equatorial rectangular coordinates from mean equator and equinox of epoch to true equator and equinox of epoch.
        /// </summary>
        /// <param name="tjd">TDB julian date of epoch.</param>
        /// <param name="fn">Flag determining 'direction' of transformation;<pre>
        ///    fn  = 0 transformation applied, mean to true.
        ///    fn != 0 inverse transformation applied, true to mean.</pre></param>
        /// <param name="pos">Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of epoch.</param>
        /// <param name="pos2">OUT: Position vector, geocentric equatorial rectangular coordinates, referred to true equator and equinox of epoch.</param>
        /// <returns>0...Everything OK.</returns>
        /// <remarks>Inverse transformation may be applied by setting flag 'fn'.</remarks>
        public static short Nutate(double tjd, NutationDirection fn, double[] pos, ref double[] pos2)


        {
            var vpos2 = default(PosVector);
            short rc;
            if (Is64Bit())
            {
                var argpos = ArrToPosVec(pos);
                rc = nutate64(tjd, (short)fn, ref argpos, ref vpos2);
            }
            else
            {
                var argpos1 = ArrToPosVec(pos);
                rc = nutate32(tjd, (short)fn, ref argpos1, ref vpos2);
            }
            PosVecToArr(vpos2, ref pos2);
            return rc;
        }

        /// <summary>
        /// Provides fast evaluation of the nutation components according to the 1980 IAU Theory of Nutation.
        /// </summary>
        /// <param name="tdbtime">TDB time in Julian centuries since J2000.0</param>
        /// <param name="longnutation">OUT: Nutation in longitude in arcseconds.</param>
        /// <param name="obliqnutation">OUT: Nutation in obliquity in arcseconds.</param>
        /// <returns>0...Everything OK.</returns>
        /// <remarks></remarks>
        public static short NutationAngles(double tdbtime, ref double longnutation, ref double obliqnutation)

        {
            if (Is64Bit())
            {
                return nutation_angles64(tdbtime, ref longnutation, ref obliqnutation);
            }
            else
            {
                return nutation_angles32(tdbtime, ref longnutation, ref obliqnutation);
            }
        }

        /// <summary>
        /// To compute the fundamental arguments.
        /// </summary>
        /// <param name="t">TDB time in Julian centuries since J2000.0</param>
        /// <param name="a">OUT: FundamentalArgsStruct containing: <pre>
        ///   a[0] = l (mean anomaly of the Moon)
        ///   a[1] = l' (mean anomaly of the Sun)
        ///   a[2] = F (L - omega; L = mean longitude of the Moon)
        ///   a[3] = D (mean elongation of the Moon from the Sun)
        ///   a[4] = omega (mean longitude of the Moon's ascending node)</pre></param>
        /// <remarks></remarks>
        public static void FundArgs(double t, ref double[] a)
        {
            var va = new FundamentalArgs();
            if (Is64Bit())
            {
                fund_args64(t, ref va);
            }
            else
            {
                fund_args32(t, ref va);
            }
            a[0] = va.l;
            a[1] = va.ldash;
            a[2] = va.F;
            a[3] = va.D;
            a[4] = va.Omega;
        }
        /// <summary>
        /// Converts TDB to TT or TDT
        /// </summary>
        /// <param name="tdb">TDB Julian date.</param>
        /// <param name="tdtjd">OUT: TT (or TDT) Julian date.</param>
        /// <param name="secdiff">OUT: Difference tdbjd-tdtjd, in seconds.</param>
        /// <remarks>Computes the terrestrial time (TT) or terrestrial dynamical time (TDT) Julian date corresponding to a barycentric dynamical time (TDB) Julian date.</remarks>
        public static void Tdb2Tdt(double tdb, ref double tdtjd, ref double secdiff)

        {
            if (Is64Bit())
            {
                tdb2tdt64(tdb, ref tdtjd, ref secdiff);
            }
            else
            {
                tdb2tdt32(tdb, ref tdtjd, ref secdiff);
            }

        }

        /// <summary>
        /// Sets up a structure of type 'body' - defining a celestial object- based on the input parameters.
        /// </summary>
        /// <param name="type">Type of body</param>
        /// <param name="number">Body number</param>
        /// <param name="name">Name of the body.</param>
        /// <param name="cel_obj">OUT: Structure containg the body definition </param>
        /// <returns><pre>
        /// = 0 ... everything OK
        /// = 1 ... invalid value of 'type'
        /// = 2 ... 'number' out of range
        /// </pre></returns>
        /// <remarks></remarks>
        public static short SetBody(BodyType type, Body number, string name, ref BodyDescription cel_obj)
        {
            var BDS = new BodyDescriptionShort();
            short rc;

            if (Is64Bit())
            {
                rc= set_body64((short)type, (short)number, name, ref BDS);
            }
            else
            {
                rc = set_body32((short)type, (short)number, name, ref BDS);
            }

            // Set values in the return object
            cel_obj.Name = BDS.Name;
            cel_obj.Number = (Body)BDS.Number;
            cel_obj.Type = (BodyType)BDS.Type;
            return rc;
        }

        // Public Shared Function readeph(ByVal mp As Integer, _
        // ByVal name As System.IntPtr, _
        // ByVal jd As Double, _
        // ByRef err As Integer) As posvector
        // End Function

        /// <summary>
        /// To create a structure of type 'cat_entry' containing catalog data for a star or "star-like" object.
        /// </summary>
        /// <param name="catalog">Three-character catalog identifier (e.g. HIP = Hipparcos, FK5 = FK5).  This identifier also specifies the reference system and units of the data; i.e. they are the same as the specified catalog.</param>
        /// <param name="star_name">Object name (50 characters maximum).</param>
        /// <param name="star_num">Object number in the catalog.</param>
        /// <param name="ra">Right ascension of the object.</param>
        /// <param name="dec">Declination of the object.</param>
        /// <param name="pm_ra">Proper motion in right ascension.</param>
        /// <param name="pm_dec">Proper motion in declination.</param>
        /// <param name="parallax">Parallax.</param>
        /// <param name="rad_vel">Radial velocity.</param>
        /// <param name="star">OUT: Structure containing the input data</param>
        /// <remarks></remarks>
        public static void MakeCatEntry(string catalog, string star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref CatEntry star)
        {
            var CEN2 = new CatEntryNOVAS2();

            if (Is64Bit())
            {
                make_cat_entry64(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, ref CEN2);
            }
            else
            {
                make_cat_entry32(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, ref CEN2);
            }
            CatEntryNOVAS2ToCatEntry(CEN2, ref star); // Convert to external presentation

        }

        /// <summary>
        /// Computes atmospheric refraction in zenith distance.
        /// </summary>
        /// <param name="location">structure containing observer's location</param>
        /// <param name="ref_option">refraction option</param>
        /// <param name="zd_obs">bserved zenith distance, in degrees.</param>
        /// <returns>Atmospheric refraction, in degrees.</returns>
        /// <remarks>This version computes approximate refraction for optical wavelengths.</remarks>
        public static double Refract(ref SiteInfo location, short ref_option, double zd_obs)

        {
            if (Is64Bit())
            {
                return refract64(ref location, ref_option, zd_obs);
            }
            else
            {
                return refract32(ref location, ref_option, zd_obs);
            }
        }

        /// <summary>
        /// This function will compute the Julian date for a given calendar date (year, month, day, hour).
        /// </summary>
        /// <param name="year">Year number</param>
        /// <param name="month">Month number.</param>
        /// <param name="day">Day number</param>
        /// <param name="hour">Time in hours</param>
        /// <returns>OUT: Julian date.</returns>
        /// <remarks></remarks>
        public static double JulianDate(short year, short month, short day, double hour)


        {
            if (Is64Bit())
            {
                return julian_date64(year, month, day, hour);
            }
            else
            {
                return julian_date32(year, month, day, hour);
            }
        }

        /// <summary>
        /// Compute a date on the Gregorian calendar given the Julian date.
        /// </summary>
        /// <param name="tjd">Julian date.</param>
        /// <param name="year">OUT: Year number</param>
        /// <param name="month">OUT: Month number.</param>
        /// <param name="day">OUT: Day number</param>
        /// <param name="hour">OUT: Time in hours</param>
        /// <remarks></remarks>
        public static void CalDate(double tjd, ref short year, ref short month, ref short day, ref double hour)        {
            if (Is64Bit())
            {
                cal_date64(tjd, ref year, ref month, ref day, ref hour);
            }
            else
            {
                cal_date32(tjd, ref year, ref month, ref day, ref hour);
            }
        }
        /// <summary>
        /// Compute equatorial spherical coordinates of Sun referred to the mean equator and equinox of date.
        /// </summary>
        /// <param name="jd">Julian date on TDT or ET time scale.</param>
        /// <param name="ra">OUT: Right ascension referred to mean equator and equinox of date (hours).</param>
        /// <param name="dec">OUT: Declination referred to mean equator and equinox of date  (degrees).</param>
        /// <param name="dis">OUT: Geocentric distance (AU).</param>
        /// <remarks></remarks>
        public static void SunEph(double jd, ref double ra, ref double dec, ref double dis)


        {
            if (Is64Bit())
            {
                sun_eph64(jd, ref ra, ref dec, ref dis);
            }
            else
            {
                sun_eph32(jd, ref ra, ref dec, ref dis);
            }
        }

        #endregion

        #region DeltaT Member
        /// <summary>
        /// Return the value of DeltaT for the given Julian date
        /// </summary>
        /// <param name="Tjd">Julian date for which the delta T value is required</param>
        /// <returns>Double value of DeltaT (seconds)</returns>
        /// <remarks>Valid between the years 1650 and 2050</remarks>
        public static double DeltaT(double Tjd)
        {
            return DeltatCode.DeltaTCalc(Tjd);
        }
        #endregion

        #region DLL Entry Points (32bit)
        [DllImport(NOVAS32Dll, EntryPoint = "app_star")]
        private static extern short app_star32(double tjd, ref BodyDescriptionShort earth, ref CatEntryNOVAS2 star, ref double ra, ref double dec);        [DllImport(NOVAS32Dll, EntryPoint = "topo_star")]
        private static extern short topo_star32(double tjd, ref BodyDescriptionShort earth, double deltat, ref CatEntryNOVAS2 star, ref SiteInfo location, ref double ra, ref double dec);

        [DllImport(NOVAS32Dll, EntryPoint = "app_planet")]
        private static extern short app_planet32(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);

        [DllImport(NOVAS32Dll, EntryPoint = "topo_planet")]
        private static extern short topo_planet32(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);


        [DllImport(NOVAS32Dll, EntryPoint = "virtual_star")]
        private static extern short virtual_star32(double tjd, ref BodyDescriptionShort earth, ref CatEntryNOVAS2 star, ref double ra, ref double dec);        [DllImport(NOVAS32Dll, EntryPoint = "local_star")]
        private static extern short local_star32(double tjd, ref BodyDescriptionShort earth, double deltat, ref CatEntryNOVAS2 star, ref SiteInfo location, ref double ra, ref double dec);

        [DllImport(NOVAS32Dll, EntryPoint = "virtual_planet")]
        private static extern short virtual_planet32(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);
        [DllImport(NOVAS32Dll, EntryPoint = "local_planet")]
        private static extern short local_planet32(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);


        [DllImport(NOVAS32Dll, EntryPoint = "astro_star")]
        private static extern short astro_star32(double tjd, ref BodyDescriptionShort earth, ref CatEntryNOVAS2 star, ref double ra, ref double dec);        [DllImport(NOVAS32Dll, EntryPoint = "astro_planet")]
        private static extern short astro_planet32(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);
        [DllImport(NOVAS32Dll, EntryPoint = "equ2hor")]
        private static extern void equ2hor32(double tjd, double deltat, double x, double y, ref SiteInfo location, double ra, double dec, short ref_option, ref double zd, ref double az, ref double rar, ref double decr);


        [DllImport(NOVAS32Dll, EntryPoint = "transform_hip")]
        private static extern void transform_hip32(ref CatEntryNOVAS2 hipparcos, ref CatEntryNOVAS2 fk5);
        [DllImport(NOVAS32Dll, EntryPoint = "transform_cat")]
        private static extern void transform_cat32(short option, double date_incat, ref CatEntryNOVAS2 incat, double date_newcat, ref byte[] newcat_id, ref CatEntryNOVAS2 newcat);
        [DllImport(NOVAS32Dll, EntryPoint = "sidereal_time")]
        private static extern void sidereal_time32(double jd_high, double jd_low, double ee, ref double gst);


        [DllImport(NOVAS32Dll, EntryPoint = "precession")]
        private static extern void precession32(double tjd1, ref PosVector pos, double tjd2, ref PosVector pos2);


        [DllImport(NOVAS32Dll, EntryPoint = "earthtilt")]
        private static extern void earthtilt32(double tjd, ref double mobl, ref double tobl, ref double eq, ref double dpsi, ref double deps);
        [DllImport(NOVAS32Dll, EntryPoint = "cel_pole")]
        private static extern void cel_pole32(double del_dpsi, double del_deps);
        [DllImport(NOVAS32Dll, EntryPoint = "ephemeris")]
        private static extern short ephemeris32(double tjd, ref BodyDescriptionShort cel_obj, short origin, ref PosVector pos, ref VelVector vel);        [DllImport(NOVAS32Dll, EntryPoint = "solarsystem")]
        private static extern short solarsystem32(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);        [DllImport(NOVAS32Dll, EntryPoint = "vector2radec")]
        private static extern short vector2radec32(ref PosVector pos, ref double ra, ref double dec);

        [DllImport(NOVAS32Dll, EntryPoint = "starvectors")]
        private static extern void starvectors32(ref CatEntryNOVAS2 star, ref PosVector pos, ref VelVector vel);


        [DllImport(NOVAS32Dll, EntryPoint = "radec2vector")]
        private static extern void radec2vector32(double ra, double dec, double dist, ref PosVector pos);


        [DllImport(NOVAS32Dll, EntryPoint = "get_earth")]
        private static extern short get_earth32(double tjd, ref BodyDescriptionShort earth, ref double tdb, ref PosVector bary_earthp, ref VelVector bary_earthv, ref PosVector helio_earthp, ref VelVector helio_earthv);

        // 
        // START OF NEW
        // 
        [DllImport(NOVAS32Dll, EntryPoint = "mean_star")]
        private static extern short mean_star32(double tjd, ref BodyDescriptionShort earth, double ra, double dec, ref double mra, ref double mdec);
        [DllImport(NOVAS32Dll, EntryPoint = "pnsw")]
        private static extern void pnsw32(double tjd, double gast, double x, double y, ref PosVector vece, ref PosVector vecs);
        [DllImport(NOVAS32Dll, EntryPoint = "spin")]
        private static extern void spin32(double st, ref PosVector pos1, ref PosVector pos2);


        [DllImport(NOVAS32Dll, EntryPoint = "wobble")]
        private static extern void wobble32(double x, double y, ref PosVector pos1, ref PosVector pos2);


        [DllImport(NOVAS32Dll, EntryPoint = "terra")]
        private static extern void terra32(ref SiteInfo locale, double st, ref PosVector pos, ref VelVector vel);        [DllImport(NOVAS32Dll, EntryPoint = "proper_motion")]
        private static extern void proper_motion32(double tjd1, ref PosVector pos, ref VelVector vel, double tjd2, ref PosVector pos2);        [DllImport(NOVAS32Dll, EntryPoint = "bary_to_geo")]
        private static extern void bary_to_geo32(ref PosVector pos, ref PosVector earthvector, ref PosVector pos2, ref double lighttime);


        [DllImport(NOVAS32Dll, EntryPoint = "sun_field")]
        private static extern short sun_field32(ref PosVector pos, ref PosVector earthvector, ref PosVector pos2);        [DllImport(NOVAS32Dll, EntryPoint = "aberration")]
        private static extern short aberration32(ref PosVector pos, ref VelVector vel, double lighttime, ref PosVector pos2);        [DllImport(NOVAS32Dll, EntryPoint = "nutate")]
        private static extern short nutate32(double tjd, short fn, ref PosVector pos, ref PosVector pos2);        [DllImport(NOVAS32Dll, EntryPoint = "nutation_angles")]
        private static extern short nutation_angles32(double tdbtime, ref double longnutation, ref double obliqnutation);        [DllImport(NOVAS32Dll, EntryPoint = "fund_args")]
        private static extern void fund_args32(double t, ref FundamentalArgs a);
        [DllImport(NOVAS32Dll, EntryPoint = "tdb2tdt")]
        private static extern void tdb2tdt32(double tdb, ref double tdtjd, ref double secdiff);


        [DllImport(NOVAS32Dll, EntryPoint = "set_body")]
        private static extern short set_body32(short type, short number, [MarshalAs(UnmanagedType.LPStr)] string name, ref BodyDescriptionShort cel_obj);
        [DllImport(NOVAS32Dll, EntryPoint = "readeph")]
        private static extern PosVector readeph32(int mp, IntPtr name, double jd, ref int err);        [DllImport(NOVAS32Dll, EntryPoint = "make_cat_entry")]
        private static extern void make_cat_entry32([MarshalAs(UnmanagedType.LPStr)] string catalog, [MarshalAs(UnmanagedType.LPStr)] string star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref CatEntryNOVAS2 star);        [DllImport(NOVAS32Dll, EntryPoint = "refract")]
        private static extern double refract32(ref SiteInfo location, short ref_option, double zd_obs);


        [DllImport(NOVAS32Dll, EntryPoint = "julian_date")]
        private static extern double julian_date32(short year, short month, short day, double hour);        [DllImport(NOVAS32Dll, EntryPoint = "cal_date")]
        private static extern void cal_date32(double tjd, ref short year, ref short month, ref short day, ref double hour);        [DllImport(NOVAS32Dll, EntryPoint = "sun_eph")]
        private static extern void sun_eph32(double jd, ref double ra, ref double dec, ref double dis);        // <DllImport(NOVAS32Dll, EntryPoint:="solarsystem")> _
        // Private Shared Function solarsystem32(ByVal tjd As Double, _
        // ByVal body As Body, _
        // ByRef origin As Integer, _
        // ByRef pos As posvector, _
        // ByRef vel As velvector) As Short
        // End Function
        #endregion

        #region DLL Entry Points (64bit)
        [DllImport(NOVAS64Dll, EntryPoint = "app_star")]
        private static extern short app_star64(double tjd, ref BodyDescriptionShort earth, ref CatEntryNOVAS2 star, ref double ra, ref double dec);        [DllImport(NOVAS64Dll, EntryPoint = "topo_star")]
        private static extern short topo_star64(double tjd, ref BodyDescriptionShort earth, double deltat, ref CatEntryNOVAS2 star, ref SiteInfo location, ref double ra, ref double dec);

        [DllImport(NOVAS64Dll, EntryPoint = "app_planet")]
        private static extern short app_planet64(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);
        [DllImport(NOVAS64Dll, EntryPoint = "topo_planet")]
        private static extern short topo_planet64(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);


        [DllImport(NOVAS64Dll, EntryPoint = "virtual_star")]
        private static extern short virtual_star64(double tjd, ref BodyDescriptionShort earth, ref CatEntryNOVAS2 star, ref double ra, ref double dec);        [DllImport(NOVAS64Dll, EntryPoint = "local_star")]
        private static extern short local_star64(double tjd, ref BodyDescriptionShort earth, double deltat, ref CatEntryNOVAS2 star, ref SiteInfo location, ref double ra, ref double dec);

        [DllImport(NOVAS64Dll, EntryPoint = "virtual_planet")]
        private static extern short virtual_planet64(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);
        [DllImport(NOVAS64Dll, EntryPoint = "local_planet")]
        private static extern short local_planet64(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);


        [DllImport(NOVAS64Dll, EntryPoint = "astro_star")]
        private static extern short astro_star64(double tjd, ref BodyDescriptionShort earth, ref CatEntryNOVAS2 star, ref double ra, ref double dec);        [DllImport(NOVAS64Dll, EntryPoint = "astro_planet")]
        private static extern short astro_planet64(double tjd, ref BodyDescriptionShort ss_object, ref BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);
        [DllImport(NOVAS64Dll, EntryPoint = "equ2hor")]
        private static extern void equ2hor64(double tjd, double deltat, double x, double y, ref SiteInfo location, double ra, double dec, short ref_option, ref double zd, ref double az, ref double rar, ref double decr);


        [DllImport(NOVAS64Dll, EntryPoint = "transform_hip")]
        private static extern void transform_hip64(ref CatEntryNOVAS2 hipparcos, ref CatEntryNOVAS2 fk5);
        [DllImport(NOVAS64Dll, EntryPoint = "transform_cat")]
        private static extern void transform_cat64(short option, double date_incat, ref CatEntryNOVAS2 incat, double date_newcat, ref byte[] newcat_id, ref CatEntryNOVAS2 newcat);
        [DllImport(NOVAS64Dll, EntryPoint = "sidereal_time")]
        private static extern void sidereal_time64(double jd_high, double jd_low, double ee, ref double gst);


        [DllImport(NOVAS64Dll, EntryPoint = "precession")]
        private static extern void precession64(double tjd1, ref PosVector pos, double tjd2, ref PosVector pos2);


        [DllImport(NOVAS64Dll, EntryPoint = "earthtilt")]
        private static extern void earthtilt64(double tjd, ref double mobl, ref double tobl, ref double eq, ref double dpsi, ref double deps);
        [DllImport(NOVAS64Dll, EntryPoint = "cel_pole")]
        private static extern void cel_pole64(double del_dpsi, double del_deps);
        [DllImport(NOVAS64Dll, EntryPoint = "ephemeris")]
        private static extern short ephemeris64(double tjd, ref BodyDescriptionShort cel_obj, short origin, ref PosVector pos, ref VelVector vel);        [DllImport(NOVAS64Dll, EntryPoint = "solarsystem")]
        private static extern short solarsystem64(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);        [DllImport(NOVAS64Dll, EntryPoint = "vector2radec")]
        private static extern short vector2radec64(ref PosVector pos, ref double ra, ref double dec);

        [DllImport(NOVAS64Dll, EntryPoint = "starvectors")]
        private static extern void starvectors64(ref CatEntryNOVAS2 star, ref PosVector pos, ref VelVector vel);

        [DllImport(NOVAS64Dll, EntryPoint = "radec2vector")]
        private static extern void radec2vector64(double ra, double dec, double dist, ref PosVector pos);


        [DllImport(NOVAS64Dll, EntryPoint = "get_earth")]
        private static extern short get_earth64(double tjd, ref BodyDescriptionShort earth, ref double tdb, ref PosVector bary_earthp, ref VelVector bary_earthv, ref PosVector helio_earthp, ref VelVector helio_earthv);

        // 
        // START OF NEW
        // 
        [DllImport(NOVAS64Dll, EntryPoint = "mean_star")]
        private static extern short mean_star64(double tjd, ref BodyDescriptionShort earth, double ra, double dec, ref double mra, ref double mdec);
        [DllImport(NOVAS64Dll, EntryPoint = "pnsw")]
        private static extern void pnsw64(double tjd, double gast, double x, double y, ref PosVector vece, ref PosVector vecs);
        [DllImport(NOVAS64Dll, EntryPoint = "spin")]
        private static extern void spin64(double st, ref PosVector pos1, ref PosVector pos2);


        [DllImport(NOVAS64Dll, EntryPoint = "wobble")]
        private static extern void wobble64(double x, double y, ref PosVector pos1, ref PosVector pos2);


        [DllImport(NOVAS64Dll, EntryPoint = "terra")]
        private static extern void terra64(ref SiteInfo locale, double st, ref PosVector pos, ref VelVector vel);        [DllImport(NOVAS64Dll, EntryPoint = "proper_motion")]
        private static extern void proper_motion64(double tjd1, ref PosVector pos, ref VelVector vel, double tjd2, ref PosVector pos2);        [DllImport(NOVAS64Dll, EntryPoint = "bary_to_geo")]
        private static extern void bary_to_geo64(ref PosVector pos, ref PosVector earthvector, ref PosVector pos2, ref double lighttime);


        [DllImport(NOVAS64Dll, EntryPoint = "sun_field")]
        private static extern short sun_field64(ref PosVector pos, ref PosVector earthvector, ref PosVector pos2);        [DllImport(NOVAS64Dll, EntryPoint = "aberration")]
        private static extern short aberration64(ref PosVector pos, ref VelVector vel, double lighttime, ref PosVector pos2);        [DllImport(NOVAS64Dll, EntryPoint = "nutate")]
        private static extern short nutate64(double tjd, short fn, ref PosVector pos, ref PosVector pos2);        [DllImport(NOVAS64Dll, EntryPoint = "nutation_angles")]
        private static extern short nutation_angles64(double tdbtime, ref double longnutation, ref double obliqnutation);


        [DllImport(NOVAS64Dll, EntryPoint = "fund_args")]
        private static extern void fund_args64(double t, ref FundamentalArgs a);
        [DllImport(NOVAS64Dll, EntryPoint = "tdb2tdt")]
        private static extern void tdb2tdt64(double tdb, ref double tdtjd, ref double secdiff);


        [DllImport(NOVAS64Dll, EntryPoint = "set_body")]
        private static extern short set_body64(short type, short number, [MarshalAs(UnmanagedType.LPStr)] string name, ref BodyDescriptionShort cel_obj);
        [DllImport(NOVAS64Dll, EntryPoint = "readeph")]
        private static extern PosVector readeph64(int mp, IntPtr name, double jd, ref int err);        [DllImport(NOVAS64Dll, EntryPoint = "make_cat_entry")]
        private static extern void make_cat_entry64([MarshalAs(UnmanagedType.LPStr)] string catalog, [MarshalAs(UnmanagedType.LPStr)] string star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref CatEntryNOVAS2 star);        [DllImport(NOVAS64Dll, EntryPoint = "refract")]
        private static extern double refract64(ref SiteInfo location, short ref_option, double zd_obs);


        [DllImport(NOVAS64Dll, EntryPoint = "julian_date")]
        private static extern double julian_date64(short year, short month, short day, double hour);        [DllImport(NOVAS64Dll, EntryPoint = "cal_date")]
        private static extern void cal_date64(double tjd, ref short year, ref short month, ref short day, ref double hour);        [DllImport(NOVAS64Dll, EntryPoint = "sun_eph")]
        private static extern void sun_eph64(double jd, ref double ra, ref double dec, ref double dis);        [DllImport(NOVAS64Dll, EntryPoint = "solarsystem")]
        private static extern short solarsystem64(double tjd, short body, ref int origin, ref PosVector pos, ref VelVector vel);        
        
        #endregion

        #region Support Code

        // Constants for SHGetSpecialFolderPath shell call
        private const int CSIDL_PROGRAM_FILES = 38; // 0x0026
        private const int CSIDL_PROGRAM_FILESX86 = 42; // 0x002a,
        private const int CSIDL_WINDOWS = 36; // 0x0024,
        private const int CSIDL_PROGRAM_FILES_COMMONX86 = 44; // 0x002c,

        // DLL to provide the path to Program Files(x86)\Common Files folder location that is not avialable through the .NET framework
        /// <summary>
        /// Get path to a system folder
        /// </summary>
        /// <param name="hwndOwner">SUpply null / nothing to use "current user"</param>
        /// <param name="lpszPath">returned string folder path</param>
        /// <param name="nFolder">Folder Number from CSIDL enumeration e.g. CSIDL_PROGRAM_FILES_COMMONX86 = 44 = 0x2c</param>
        /// <param name="fCreate">Indicates whether the folder should be created if it does not already exist. If this value is nonzero, 
        /// the folder is created. If this value is zero, the folder is not created</param>
        /// <returns>TRUE if successful; otherwise, FALSE.</returns>
        /// <remarks></remarks>
        [DllImport("shell32.dll")]
        private static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, System.Text.StringBuilder lpszPath, int nFolder, bool fCreate);

        // Declare the api call that sets the additional DLL search directory
        [DllImport("kernel32.dll", SetLastError = false)]
        private static extern bool SetDllDirectory(string lpPathName);

        private static bool Is64Bit()
        {
            bool rc;
            var StatPath = default(string);

            // StatPath is a static variable because GetFolderPath takes about 10 times longer to execute than
            // the supervisory NOVAS routines. If we called it every time the overhead would be enormous.
            // The static variable and If statement ensure that the call is only made once

            if (string.IsNullOrEmpty(StatPath))
            {

                string CommonProgramFilesPath;
                var ReturnedPath = new System.Text.StringBuilder(260);

                // Find the root location of the common files directory containing the ASCOM support files.
                // On a 32bit system this is \Program Files\Common Files
                // On a 64bit system this is \Program Files (x86)\Common Files
                if (IntPtr.Size == 8) // 64bit application so find the 32bit folder location
                {
                    rc = SHGetSpecialFolderPath(IntPtr.Zero, ReturnedPath, CSIDL_PROGRAM_FILES_COMMONX86, false);
                    CommonProgramFilesPath = ReturnedPath.ToString();
                }
                else // 32bit application so just go with the .NET returned value
                {
                    CommonProgramFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                }

                StatPath = CommonProgramFilesPath + NOVAS_DLL_LOCATION;
                // Add the ASCOM\.net directory to the DLL search path so that the NOVAS C 32 and 64bit DLLs can be found
                rc = SetDllDirectory(StatPath);
            }

            if (IntPtr.Size == 8) // Check whether we are running on a 32 or 64bit system.
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static PosVector ArrToPosVec(double[] Arr)
        {
            // Create a new vector having the values in the supplied double array
            var V = new PosVector();
            V.x = Arr[0];
            V.y = Arr[1];
            V.z = Arr[2];
            return V;
        }

        private static void PosVecToArr(PosVector V, ref double[] Ar)
        {
            // Copy a vector structure to a returned double array
            Ar[0] = V.x;
            Ar[1] = V.y;
            Ar[2] = V.z;
        }

        private static VelVector ArrToVelVec(double[] Arr)
        {
            // Create a new vector having the values in the supplied double array
            var V = new VelVector();
            V.x = Arr[0];
            V.y = Arr[1];
            V.z = Arr[2];
            return V;
        }

        private static void VelVecToArr(VelVector V, ref double[] Ar)
        {
            // Copy a vector structure to a returned double array
            Ar[0] = V.x;
            Ar[1] = V.y;
            Ar[2] = V.z;
        }

        private static BodyDescriptionShort BodyDescToShort(BodyDescription BD)
        {
            // Create a version of the body description that uses shorts instead of integers  for number and type
            // This ensures that the C functions run OK (using shorts) while the public face is integer.
            var BDS = new BodyDescriptionShort();
            BDS.Name = BD.Name;
            BDS.Number = (short)BD.Number;
            BDS.Type = (short)BD.Type;
            return BDS;
        }

        private static CatEntryNOVAS2 CatEntryToCatEntryNOVAS2(CatEntry CE)
        {
            // Create a version of the body description that uses shorts instead of integers  for number and type
            // This ensures that the C functions run OK (using shorts) while the public face is integer.
            var CEN2 = new CatEntryNOVAS2();
            CEN2.Catalog = CE.Catalog;
            CEN2.Dec = CE.Dec;
            CEN2.Parallax = CE.Parallax;
            CEN2.ProMoDec = CE.ProMoDec;
            CEN2.ProMoRA = CE.ProMoRA;
            CEN2.RA = CE.RA;
            CEN2.RadialVelocity = CE.RadialVelocity;
            CEN2.StarName = CE.StarName;
            CEN2.StarNumber = CE.StarNumber;
            return CEN2;
        }

        private static void CatEntryNOVAS2ToCatEntry(CatEntryNOVAS2 CEN2, ref CatEntry CE)
        {
            // Transfer to the external public structure that has differnt marshalling characteristics specifically for COM
            CE.Catalog = CEN2.Catalog;
            CE.Dec = CEN2.Dec;
            CE.Parallax = CEN2.Parallax;
            CE.ProMoDec = CEN2.ProMoDec;
            CE.ProMoRA = CEN2.ProMoRA;
            CE.RA = CEN2.RA;
            CE.RadialVelocity = CEN2.RadialVelocity;
            CE.StarName = CEN2.StarName;
            CE.StarNumber = CEN2.StarNumber;
        }
        #endregion

    }
}