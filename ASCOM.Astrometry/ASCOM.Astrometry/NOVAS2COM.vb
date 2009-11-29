'NOVAS2COM component implementation
'This class is an instanciable version of the NOVAS2 shared component because COM cannot see shared classes
'It is recommended that it only be used from COM clients and that .NET clients use the shared NOVAS2 component

Imports System.Runtime.InteropServices
'Imports ASCOM.Astrometry.Interfaces
Imports ASCOM.Astrometry.NOVASCOM

Namespace NOVAS
    ''' <summary>
    ''' NOVAS2COM: Instanciable class presenting the contents of the NOVAS 2 library. 
    ''' NOVAS was developed by the Astronomical Applications department of the United States Naval 
    ''' Observatory. The C language version of NOVAS was developed by John Bangert at USNO/AA.
    ''' </summary>
    ''' <remarks>
    ''' The NOVAS2COM class is an instanciable class usable by COM clients. This means that you have to create an instance of the 
    ''' class in order to access its members. So, this works:
    ''' <code>
    ''' Dim Nov as New ASCOM.Astrometry.NOVAS2COM
    ''' rc = Nov.AppStar(tjd, earth, star, ra, dec)
    ''' </code>
    ''' while this does not work: 
    ''' <code>rc = ASCOM.Astrometry.NOVAS2COM.AppStar(tjd, earth, star, ra, dec)</code> 
    ''' <para>Method names are identical to those used in NOVAS2, as are almost all paramaters. There are a few 
    ''' changes that introduce some new structures but these should be self explanatory. One significant difference 
    ''' is that position and velocity vectors are returned as structures rather than double arrays. This was done 
    ''' to make type checking more effective.</para>
    ''' <para>Testing of the high level supervisory functions has been carried out using real-time star data from
    ''' the USNO web site. Values provided by this NOVAS2 implementation agree on average to about 50 milli 
    ''' arc-seconds with current USNO web site values.</para>
    ''' <para>This class is implemented using a thin layer of .NET code that calls functions in 
    ''' either a 32 or 64 bit compiled version of the unmodified C code from ther USNO web site. The .NET code
    ''' does not carry out calculations itself, it simply handles any interface presentation differences
    ''' and calls the relevant 32 or 64bit code according to its environment.</para>
    ''' </remarks>
    <Guid("C3F04186-CD53-40fb-8B2A-B52BE955956D"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class NOVAS2COM
        Implements INOVAS2

        ''' <summary>
        ''' Corrects position vector for aberration of light.
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="vel">Velocity vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU/day.</param>
        ''' <param name="lighttime">Light time from body to Earth in days.</param>
        ''' <param name="pos2">OUT: Position vector, referred to origin at center of mass of the Earth, corrected for aberration, components in AU</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks>Algorithm includes relativistic terms.</remarks>
        Public Function Aberration(ByVal pos() As Double, ByVal vel() As Double, ByVal lighttime As Double, ByRef pos2() As Double) As Short Implements INOVAS2.Aberration
            Return NOVAS2.Aberration(pos, vel, lighttime, pos2)
        End Function

        ''' <summary>
        ''' Compute the apparent place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for apparent place.</param>
        ''' <param name="ss_object">Structure containing the body designation for the solar system body</param>
        ''' <param name="earth">Structure containing the body designation for the Earth</param>
        ''' <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet at 'tjd' in AU.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        Public Function AppPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.AppPlanet
            Return NOVAS2.AppPlanet(tjd, ss_object, earth, ra, dec, dis)
        End Function

        ''' <summary>
        ''' Computes the apparent place of a star 
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for apparent place.</param>
        ''' <param name="earth">Structure containing the body designation for the earth</param>
        ''' <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units (defined in novas.h).</param>
        ''' <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <returns><pre>
        '''  0...Everything OK
        ''' >0...Error code from function 'solarsystem'.</pre></returns>
        ''' <remarks></remarks>
        Public Function AppStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByRef star As CatEntry, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.AppStar
            Return NOVAS2.AppStar(tjd, earth, star, ra, dec)
        End Function

        ''' <summary>
        ''' Computes the astrometric place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for calculation.</param>
        ''' <param name="ss_object">Pointer to structure containing the body designation for the solar system body</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="ra">OUT: Astrometric right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT:  Astrometric declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.</pre></returns>
        ''' <remarks></remarks>
        Public Function AstroPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.AstroPlanet
            Return NOVAS2.AstroPlanet(tjd, ss_object, earth, ra, dec, dis)
        End Function

        ''' <summary>
        ''' Computes the astrometric place of a star
        ''' </summary>
        ''' <param name="tjd">  TT (or TDT) Julian date for astrometric place.</param>
        ''' <param name="earth"> Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="star"> Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        ''' <param name="ra">OUT:  Astrometric right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT:  Astrometric declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'.
        ''' </pre></returns>
        ''' <remarks>     Computes the astrometric place of a star, given its mean place, proper motion, parallax, and radial velocity for J2000.0.</remarks>
        Public Function AstroStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByRef star As CatEntry, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.AstroStar
            Return NOVAS2.AstroStar(tjd, earth, star, ra, dec)
        End Function

        ''' <summary>
        ''' Moves the origin of coordinates from the barycenter of the solar system to the center of mass of the Earth
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at solar system barycenter, components in AU.</param>
        ''' <param name="earthvector">Position vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU.</param>
        ''' <param name="pos2">OUT: Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="lighttime">OUT: Light time from body to Earth in days.</param>
        ''' <remarks>This corrects for parallax.</remarks>
        Public Sub BaryToGeo(ByVal pos() As Double, ByVal earthvector() As Double, ByRef pos2() As Double, ByRef lighttime As Double) Implements INOVAS2.BaryToGeo
            NOVAS2.BaryToGeo(pos, earthvector, pos2, lighttime)
        End Sub

        ''' <summary>
        ''' Compute a date on the Gregorian calendar given the Julian date.
        ''' </summary>
        ''' <param name="tjd">Julian date.</param>
        ''' <param name="year">OUT: Year number</param>
        ''' <param name="month">OUT: Month number.</param>
        ''' <param name="day">OUT: Day number</param>
        ''' <param name="hour">OUT: Time in hours</param>
        ''' <remarks></remarks>
        Public Sub CalDate(ByVal tjd As Double, ByRef year As Short, ByRef month As Short, ByRef day As Short, ByRef hour As Double) Implements INOVAS2.CalDate
            NOVAS2.CalDate(tjd, year, month, day, hour)
        End Sub

        ''' <summary>
        ''' This function allows for the specification of celestial pole offsets for high-precision applications.  
        ''' </summary>
        ''' <param name="del_dpsi">Value of offset in delta psi (dpsi) in arcseconds.</param>
        ''' <param name="del_deps">Value of offset in delta epsilon (deps) in arcseconds.</param>
        ''' <remarks>These are added to the nutation parameters delta psi and delta epsilon.
        ''' <para>1. This function sets the values of global variables 'PSI_COR'and 'EPS_COR' declared at the top of file 'novas.c'.  These global variables are used only in NOVAS function 'earthtilt'.</para>
        ''' <para>2. This function, if used, should be called before any other NOVAS functions for a given date.  Values of the pole offsets specified via a call to this function will be used until explicitly changed.</para>
        ''' <para>3. Daily values of the offsets are published, for example, in IERS Bulletins A and B.</para>
        ''' <para>4. This function is the "C" version of Fortran NOVAS routine "celpol".</para>
        ''' </remarks>
        Public Sub CelPole(ByVal del_dpsi As Double, ByVal del_deps As Double) Implements INOVAS2.CelPole
            NOVAS2.CelPole(del_dpsi, del_deps)
        End Sub

        ''' <summary>
        '''  Computes quantities related to the orientation of the Earth's rotation axis at Julian date 'tjd'.
        ''' </summary>
        ''' <param name="tjd">TDB Julian date of the desired time</param>
        ''' <param name="mobl">OUT:  Mean obliquity of the ecliptic in degrees at 'tjd'.</param>
        ''' <param name="tobl">OUT: True obliquity of the ecliptic in degrees at 'tjd'.</param>
        ''' <param name="eq">OUT: Equation of the equinoxes in seconds of time at 'tjd'.</param>
        ''' <param name="dpsi">OUT: Nutation in longitude in arcseconds at 'tjd'.</param>
        ''' <param name="deps">OUT: Nutation in obliquity in arcseconds at 'tjd'.</param>
        ''' <remarks></remarks>
        Public Sub EarthTilt(ByVal tjd As Double, ByRef mobl As Double, ByRef tobl As Double, ByRef eq As Double, ByRef dpsi As Double, ByRef deps As Double) Implements INOVAS2.EarthTilt
            NOVAS2.EarthTilt(tjd, mobl, tobl, eq, dpsi, deps)
        End Sub

        ''' <summary>
        ''' Retrieves the position and velocity of a body from a fundamental ephemeris.
        ''' </summary>
        ''' <param name="tjd">TDB Julian date.</param>
        ''' <param name="cel_obj">Structure containing the designation of the body of interest</param>
        ''' <param name="origin">Origin point (solar system barycentre or centre of mass of the Sun</param>
        ''' <param name="pos">OUT: Position vector of 'body' at tjd; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        ''' <param name="vel">OUT: Velocity vector of 'body' at tjd; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        ''' <returns><pre>
        ''' 0    ... Everything OK.
        ''' 1    ... Invalid value of 'origin'.
        ''' 2    ... Invalid value of 'type' in 'cel_obj'.
        ''' 3    ... Unable to allocate memory.
        ''' 10+n ... where n is the error code from 'solarsystem'.
        ''' 20+n ... where n is the error code from 'readeph'.</pre></returns>
        ''' <remarks></remarks>
        Public Function Ephemeris(ByVal tjd As Double, ByRef cel_obj As BodyDescription, ByVal origin As Origin, ByRef pos() As Double, ByRef vel() As Double) As Short Implements INOVAS2.Ephemeris
            Return NOVAS2.Ephemeris(tjd, cel_obj, origin, pos, vel)
        End Function

        ''' <summary>
        ''' Transform apparent equatorial coordinates to horizon coordinates
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date.</param>
        ''' <param name="deltat">Difference TT (or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="x">Conventionally-defined x coordinate of celestial ephemeris  pole with respect to IERS reference pole, in arcseconds. </param>
        ''' <param name="y">Conventionally-defined y coordinate of celestial ephemeris  pole with respect to IERS reference pole, in arcseconds.</param>
        ''' <param name="location">structure containing observer's location</param>
        ''' <param name="ra"> Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date.</param>
        ''' <param name="dec">Topocentric declination of object of interest, in degrees,  referred to true equator and equinox of date.</param>
        ''' <param name="ref_option">Refraction option</param>
        ''' <param name="zd">OUT: Topocentric zenith distance in degrees, affected by  refraction if 'ref_option' is non-zero.</param>
        ''' <param name="az">OUT: Topocentric azimuth (measured east from north) in degrees.</param>
        ''' <param name="rar">OUT: Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        ''' <param name="decr">OUT: Topocentric declination of object of interest, in degrees, referred to true equator and equinox of date, affected by  refraction if 'ref_option' is non-zero.</param>
        ''' <remarks>This function transforms apparent equatorial coordinates (right 
        ''' ascension and declination) to horizon coordinates (zenith 
        ''' distance and azimuth).  It uses a method that properly accounts 
        ''' for polar motion, which is significant at the sub-arcsecond 
        ''' level.  This function can also adjust coordinates for atmospheric 
        ''' refraction.</remarks>
        Public Sub Equ2Hor(ByVal tjd As Double, ByVal deltat As Double, ByVal x As Double, ByVal y As Double, ByRef location As SiteInfo, ByVal ra As Double, ByVal dec As Double, ByVal ref_option As RefractionOption, ByRef zd As Double, ByRef az As Double, ByRef rar As Double, ByRef decr As Double) Implements INOVAS2.Equ2Hor
            NOVAS2.Equ2Hor(tjd, deltat, x, y, location, ra, dec, ref_option, zd, az, rar, decr)
        End Sub

        ''' <summary>
        ''' To compute the fundamental arguments.
        ''' </summary>
        ''' <param name="t">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="a">OUT: FundamentalArgsStruct containing: <pre>
        '''   a[0] = l (mean anomaly of the Moon)
        '''   a[1] = l' (mean anomaly of the Sun)
        '''   a[2] = F (L - omega; L = mean longitude of the Moon)
        '''   a[3] = D (mean elongation of the Moon from the Sun)
        '''   a[4] = omega (mean longitude of the Moon's ascending node)</pre></param>
        ''' <remarks></remarks>
        Public Sub FundArgs(ByVal t As Double, ByRef a() As Double) Implements INOVAS2.FundArgs
            NOVAS2.FundArgs(t, a)
        End Sub

        ''' <summary>
        ''' Obtains the barycentric and heliocentric positions and velocities of the Earth from the solar system ephemeris.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date.</param>
        ''' <param name="earth">Structure containing the body designation for the Earth.</param>
        ''' <param name="tdb">OUT: TDB Julian date corresponding to 'tjd'.</param>
        ''' <param name="bary_earthp">OUT: Barycentric position vector of Earth at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        ''' <param name="bary_earthv">OUT: Barycentric velocity vector of Earth at 'tjd'; equatorial rectangular system referred to the mean equator and equinox  of J2000.0, in AU/Day.</param>
        ''' <param name="helio_earthp">OUT: Heliocentric position vector of Earth at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        ''' <param name="helio_earthv">OUT: Heliocentric velocity vector of Earth at 'tjd'; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'.</pre>
        ''' </returns>
        ''' <remarks></remarks>
        Public Function GetEarth(ByVal tjd As Double, ByRef earth As BodyDescription, ByRef tdb As Double, ByRef bary_earthp() As Double, ByRef bary_earthv() As Double, ByRef helio_earthp() As Double, ByRef helio_earthv() As Double) As Short Implements INOVAS2.GetEarth
            Return NOVAS2.GetEarth(tjd, earth, tdb, bary_earthp, bary_earthv, helio_earthp, helio_earthv)
        End Function

        ''' <summary>
        ''' This function will compute the Julian date for a given calendar date (year, month, day, hour).
        ''' </summary>
        ''' <param name="year">Year number</param>
        ''' <param name="month">Month number.</param>
        ''' <param name="day">Day number</param>
        ''' <param name="hour">Time in hours</param>
        ''' <returns>OUT: Julian date.</returns>
        ''' <remarks></remarks>
        Public Function JulianDate(ByVal year As Short, ByVal month As Short, ByVal day As Short, ByVal hour As Double) As Double Implements INOVAS2.JulianDate
            Return NOVAS2.JulianDate(year, month, day, hour)
        End Function

        ''' <summary>
        '''  Computes the local place of a planet or other solar system body, given the location of the observer.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for local place.</param>
        ''' <param name="ss_object"> Pointer to structure containing the body designation for the solar system body</param>
        ''' <param name="earth"> Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="location"> Pointer to structure containing observer's location</param>
        ''' <param name="ra">OUT: Local right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT: Local declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        Public Function LocalPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByVal deltat As Double, ByRef location As SiteInfo, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.LocalPlanet
            Return NOVAS2.LocalPlanet(tjd, ss_object, earth, deltat, location, ra, dec, dis)
        End Function

        ''' <summary>
        '''  Computes the local place of a star
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for local place.</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="star">Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        ''' <param name="location">Pointer to structure containing observer's location</param>
        ''' <param name="ra">OUT: Local right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT: Local declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'.
        '''</pre></returns>
        ''' <remarks></remarks>
        Public Function LocalStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByVal deltat As Double, ByRef star As CatEntry, ByRef location As SiteInfo, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.LocalStar
            Return NOVAS2.LocalStar(tjd, earth, deltat, star, location, ra, dec)
        End Function

        ''' <summary>
        ''' To create a structure of type 'cat_entry' containing catalog data for a star or "star-like" object.
        ''' </summary>
        ''' <param name="catalog">Three-character catalog identifier (e.g. HIP = Hipparcos, FK5 = FK5).  This identifier also specifies the reference system and units of the data; i.e. they are the same as the specified catalog.</param>
        ''' <param name="star_name">Object name (50 characters maximum).</param>
        ''' <param name="star_num">Object number in the catalog.</param>
        ''' <param name="ra">Right ascension of the object.</param>
        ''' <param name="dec">Declination of the object.</param>
        ''' <param name="pm_ra">Proper motion in right ascension.</param>
        ''' <param name="pm_dec">Proper motion in declination.</param>
        ''' <param name="parallax">Parallax.</param>
        ''' <param name="rad_vel">Radial velocity.</param>
        ''' <param name="star">OUT: Structure containing the input data</param>
        ''' <remarks></remarks>
        Public Sub MakeCatEntry(ByVal catalog As String, ByVal star_name As String, ByVal star_num As Integer, ByVal ra As Double, ByVal dec As Double, ByVal pm_ra As Double, ByVal pm_dec As Double, ByVal parallax As Double, ByVal rad_vel As Double, ByRef star As CatEntry) Implements INOVAS2.MakeCatEntry
            NOVAS2.MakeCatEntry(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, star)
        End Sub

        ''' <summary>
        ''' Computes the mean place of a star for J2000.0
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date of apparent place.</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="ra">Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec"> Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="mra">OUT: Mean right ascension J2000.0 in hours.</param>
        ''' <param name="mdec">OUT: Mean declination J2000.0 in degrees.</param>
        ''' <returns><pre>
        '''   0...Everything OK.
        '''   1...Iterative process did not converge after 20 iterations.
        ''' >10...Error from function 'app_star'.</pre></returns>
        ''' <remarks>Computes the mean place of a star for J2000.0, given its apparent 
        ''' place at date 'tjd'.  Proper motion, parallax and radial velocity 
        ''' are assumed to be zero.
        '''</remarks>
        Public Function MeanStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByVal ra As Double, ByVal dec As Double, ByRef mra As Double, ByRef mdec As Double) As Short Implements INOVAS2.MeanStar
            Return NOVAS2.MeanStar(tjd, earth, ra, dec, mra, mdec)
        End Function

        ''' <summary>
        ''' Nutates equatorial rectangular coordinates from mean equator and equinox of epoch to true equator and equinox of epoch.
        ''' </summary>
        ''' <param name="tjd">TDB julian date of epoch.</param>
        ''' <param name="fn">Flag determining 'direction' of transformation;<pre>
        '''    fn  = 0 transformation applied, mean to true.
        '''    fn != 0 inverse transformation applied, true to mean.</pre></param>
        ''' <param name="pos">Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of epoch.</param>
        ''' <param name="pos2">OUT: Position vector, geocentric equatorial rectangular coordinates, referred to true equator and equinox of epoch.</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks>Inverse transformation may be applied by setting flag 'fn'.</remarks>
        Public Function Nutate(ByVal tjd As Double, ByVal fn As NutationDirection, ByVal pos() As Double, ByRef pos2() As Double) As Short Implements INOVAS2.Nutate
            Return NOVAS2.Nutate(tjd, fn, pos, pos2)
        End Function

        ''' <summary>
        ''' Provides fast evaluation of the nutation components according to the 1980 IAU Theory of Nutation.
        ''' </summary>
        ''' <param name="tdbtime">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="longnutation">OUT: Nutation in longitude in arcseconds.</param>
        ''' <param name="obliqnutation">OUT: Nutation in obliquity in arcseconds.</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks></remarks>
        Public Function NutationAngles(ByVal tdbtime As Double, ByRef longnutation As Double, ByRef obliqnutation As Double) As Short Implements INOVAS2.NutationAngles
            Return NOVAS2.NutationAngles(tdbtime, longnutation, obliqnutation)
        End Function

        ''' <summary>
        ''' Transforms a vector from an Earth-fixed geographic system to a space-fixed system
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date</param>
        ''' <param name="gast">Greenwich apparent sidereal time, in hours.</param>
        ''' <param name="x"> Conventionally-defined X coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="vece"> Vector in geocentric rectangular Earth-fixed system, referred to geographic equator and Greenwich meridian.</param>
        ''' <param name="vecs">OUT: Vector in geocentric rectangular space-fixed system, referred to mean equator and equinox of J2000.0.</param>
        ''' <remarks>Transforms a vector from an Earth-fixed geographic system to a space-fixed system based on mean equator and equinox of J2000.0; applies rotations for wobble, spin, nutation, and precession.</remarks>
        Public Sub Pnsw(ByVal tjd As Double, ByVal gast As Double, ByVal x As Double, ByVal y As Double, ByVal vece() As Double, ByRef vecs() As Double) Implements INOVAS2.Pnsw
            NOVAS2.Pnsw(tjd, gast, x, y, vece, vecs)
        End Sub

        ''' <summary>
        ''' Precesses equatorial rectangular coordinates from one epoch to another.
        ''' </summary>
        ''' <param name="tjd1">TDB Julian date of first epoch.</param>
        ''' <param name="pos">Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of first epoch.</param>
        ''' <param name="tjd2">TDB Julian date of second epoch.</param>
        ''' <param name="pos2">OUT: Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of second epoch.</param>
        ''' <remarks>The coordinates are referred to the mean equator and equinox of the two respective epochs.</remarks>
        Public Sub Precession(ByVal tjd1 As Double, ByVal pos() As Double, ByVal tjd2 As Double, ByRef pos2() As Double) Implements INOVAS2.Precession
            NOVAS2.Precession(tjd1, pos, tjd2, pos2)
        End Sub

        ''' <summary>
        ''' Applies proper motion, including foreshortening effects, to a star's position.
        ''' </summary>
        ''' <param name="tjd1">TDB Julian date of first epoch.</param>
        ''' <param name="pos">Position vector at first epoch.</param>
        ''' <param name="vel">Velocity vector at first epoch.</param>
        ''' <param name="tjd2">TDB Julian date of second epoch.</param>
        ''' <param name="pos2">OUT: Position vector at second epoch.</param>
        ''' <remarks></remarks>
        Public Sub ProperMotion(ByVal tjd1 As Double, ByVal pos() As Double, ByVal vel() As Double, ByVal tjd2 As Double, ByRef pos2() As Double) Implements INOVAS2.ProperMotion
            NOVAS2.ProperMotion(tjd1, pos, vel, tjd2, pos2)
        End Sub

        ''' <summary>
        ''' Converts equatorial spherical coordinates to a vector (equatorial rectangular coordinates).
        ''' </summary>
        ''' <param name="ra">Right ascension (hours).</param>
        ''' <param name="dec">Declination (degrees).</param>
        ''' <param name="dist">Distance</param>
        ''' <param name="pos">Position vector, equatorial rectangular coordinates (AU).</param>
        ''' <remarks></remarks>
        Public Sub RADec2Vector(ByVal ra As Double, ByVal dec As Double, ByVal dist As Double, ByRef pos() As Double) Implements INOVAS2.RADec2Vector
            NOVAS2.RADec2Vector(ra, dec, dist, pos)
        End Sub

        ''' <summary>
        ''' Computes atmospheric refraction in zenith distance.
        ''' </summary>
        ''' <param name="location">structure containing observer's location</param>
        ''' <param name="ref_option">refraction option</param>
        ''' <param name="zd_obs">bserved zenith distance, in degrees.</param>
        ''' <returns>Atmospheric refraction, in degrees.</returns>
        ''' <remarks>This version computes approximate refraction for optical wavelengths.</remarks>
        Public Function Refract(ByRef location As SiteInfo, ByVal ref_option As Short, ByVal zd_obs As Double) As Double Implements INOVAS2.Refract
            Return NOVAS2.Refract(location, ref_option, zd_obs)
        End Function

        ''' <summary>
        ''' Sets up a structure of type 'body' - defining a celestial object- based on the input parameters.
        ''' </summary>
        ''' <param name="type">Type of body</param>
        ''' <param name="number">Body number</param>
        ''' <param name="name">Name of the body.</param>
        ''' <param name="cel_obj">OUT: Structure containg the body definition </param>
        ''' <returns><pre>
        ''' = 0 ... everything OK
        ''' = 1 ... invalid value of 'type'
        ''' = 2 ... 'number' out of range
        '''</pre></returns>
        ''' <remarks></remarks>
        Public Function SetBody(ByVal type As BodyType, ByVal number As Body, ByVal name As String, ByRef cel_obj As BodyDescription) As Short Implements INOVAS2.SetBody
            Return NOVAS2.SetBody(type, number, name, cel_obj)
        End Function

        ''' <summary>
        '''  Computes the Greenwich apparent sidereal time, at Julian date 'jd_high' + 'jd_low'.
        ''' </summary>
        ''' <param name="jd_high">Julian date, integral part.</param>
        ''' <param name="jd_low">Julian date, fractional part.</param>
        ''' <param name="ee"> Equation of the equinoxes (seconds of time). [Note: this  quantity is computed by function 'earthtilt'.]</param>
        ''' <param name="gst">Greenwich apparent sidereal time, in hours.</param>
        ''' <remarks></remarks>
        Public Sub SiderealTime(ByVal jd_high As Double, ByVal jd_low As Double, ByVal ee As Double, ByRef gst As Double) Implements INOVAS2.SiderealTime
            NOVAS2.SiderealTime(jd_high, jd_low, ee, gst)
        End Sub

        ''' <summary>
        ''' Provides the position and velocity of the Earth
        ''' </summary>
        ''' <param name="tjd">TDB Julian date.</param>
        ''' <param name="body">Body identification number.
        ''' <pre>
        ''' Set 'body' = 0 or 'body' = 1 or 'body' = 10 for the Sun.
        ''' Set 'body' = 2 or 'body' = 3 for the Earth.
        '''</pre></param>
        ''' <param name="origin">Required origin: solar system barycenter or center of mass of the Sun</param>
        ''' <param name="pos">OUT: Position vector of 'body' at 'tjd'; equatorial rectangular coordinates in AU referred to the mean equator and equinox of J2000.0.</param>
        ''' <param name="vel">OUT: Velocity vector of 'body' at 'tjd'; equatorial rectangular system referred to the mean equator and equinox of J2000.0, in AU/Day.</param>
        ''' <returns><pre>
        ''' 0...Everything OK.
        ''' 1...Input Julian date ('tjd') out of range.
        ''' 2...Invalid value of 'body'.
        '''</pre></returns>
        ''' <remarks> Provides the position and velocity of the Earth at epoch 'tjd' by evaluating a closed-form theory without reference to an  external file.  This function can also provide the position and velocity of the Sun.</remarks>
        Public Function SolarSystem(ByVal tjd As Double, ByVal body As Body, ByVal origin As Origin, ByRef pos() As Double, ByRef vel() As Double) As Short Implements INOVAS2.SolarSystem
            Return NOVAS2.SolarSystem(tjd, body, origin, pos, vel)
        End Function

        ''' <summary>
        ''' Transforms geocentric rectangular coordinates from rotating system to non-rotating system
        ''' </summary>
        ''' <param name="st">Local apparent sidereal time at reference meridian, in hours.</param>
        ''' <param name="pos1">Vector in geocentric rectangular rotating system, referred to rotational equator and orthogonal reference meridian.</param>
        ''' <param name="pos2">OUT: Vector in geocentric rectangular non-rotating system, referred to true equator and equinox of date.</param>
        ''' <remarks>Transforms geocentric rectangular coordinates from rotating system based on rotational equator and orthogonal reference meridian to  non-rotating system based on true equator and equinox of date.</remarks>
        Public Sub Spin(ByVal st As Double, ByVal pos1() As Double, ByRef pos2() As Double) Implements INOVAS2.Spin
            NOVAS2.Spin(st, pos1, pos2)
        End Sub

        ''' <summary>
        ''' Converts angular quanities for stars to vectors.
        ''' </summary>
        ''' <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units </param>
        ''' <param name="pos">Position vector, equatorial rectangular coordinates, components in AU.</param>
        ''' <param name="vel">Velocity vector, equatorial rectangular coordinates, components in AU/Day.</param>
        ''' <remarks></remarks>
        Public Sub StarVectors(ByVal star As CatEntry, ByRef pos() As Double, ByRef vel() As Double) Implements INOVAS2.StarVectors
            NOVAS2.StarVectors(star, pos, vel)
        End Sub

        ''' <summary>
        ''' Compute equatorial spherical coordinates of Sun referred to the mean equator and equinox of date.
        ''' </summary>
        ''' <param name="jd">Julian date on TDT or ET time scale.</param>
        ''' <param name="ra">OUT: Right ascension referred to mean equator and equinox of date (hours).</param>
        ''' <param name="dec">OUT: Declination referred to mean equator and equinox of date  (degrees).</param>
        ''' <param name="dis">OUT: Geocentric distance (AU).</param>
        ''' <remarks></remarks>
        Public Sub SunEph(ByVal jd As Double, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) Implements INOVAS2.SunEph
            NOVAS2.SunEph(jd, ra, dec, dis)
        End Sub

        ''' <summary>
        ''' Corrects position vector for the deflection of light in the gravitational field of the Sun. 
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="earthvector">Position vector of center of mass of the Earth, referred to origin at center of mass of the Sun, components in AU.</param>
        ''' <param name="pos2">Position vector, referred to origin at center of mass of the Earth, corrected for gravitational deflection, components in AU.</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks>This function is valid for bodies within the solar system as well as for stars.</remarks>
        Public Function SunField(ByVal pos() As Double, ByVal earthvector() As Double, ByRef pos2() As Double) As Short Implements INOVAS2.SunField
            Return NOVAS2.SunField(pos, earthvector, pos2)
        End Function

        ''' <summary>
        ''' Converts TDB to TT or TDT
        ''' </summary>
        ''' <param name="tdb">TDB Julian date.</param>
        ''' <param name="tdtjd">OUT: TT (or TDT) Julian date.</param>
        ''' <param name="secdiff">OUT: Difference tdbjd-tdtjd, in seconds.</param>
        ''' <remarks>Computes the terrestrial time (TT) or terrestrial dynamical time (TDT) Julian date corresponding to a barycentric dynamical time (TDB) Julian date.</remarks>
        Public Sub Tdb2Tdt(ByVal tdb As Double, ByRef tdtjd As Double, ByRef secdiff As Double) Implements INOVAS2.Tdb2Tdt
            NOVAS2.Tdb2Tdt(tdb, tdtjd, secdiff)
        End Sub

        ''' <summary>
        ''' Computes the position and velocity vectors of a terrestrial observer with respect to the center of the Earth.
        ''' </summary>
        ''' <param name="locale">Longitude, latitude and height of the observer (in a SiteInfoStruct)</param>
        ''' <param name="st">Local apparent sidereal time at reference meridian in hours.</param>
        ''' <param name="pos"> Position vector of observer with respect to center of Earth, equatorial rectangular coordinates, referred to true equator and equinox of date, components in AU.</param>
        ''' <param name="vel"> Velocity vector of observer with respect to center of Earth, equatorial rectangular coordinates, referred to true equator and equinox of date, components in AU/Day.</param>
        ''' <remarks></remarks>
        Public Sub Terra(ByRef locale As SiteInfo, ByVal st As Double, ByRef pos() As Double, ByRef vel() As Double) Implements INOVAS2.Terra
            NOVAS2.Terra(locale, st, pos, vel)
        End Sub

        ''' <summary>
        ''' Computes the topocentric place of a planet, given the location of the observer.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for topocentric place.</param>
        ''' <param name="ss_object">structure containing the body designation for the solar system body</param>
        ''' <param name="earth">structure containing the body designation for the Earth</param>
        ''' <param name="deltat">Difference TT(or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="location">structure containing observer's location</param>
        ''' <param name="ra">OUT: Apparent right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec">OUT: Apparent declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet at 'tjd' in AU.</param>
        ''' <returns> <pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        Public Function TopoPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByVal deltat As Double, ByRef location As SiteInfo, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.TopoPlanet
            Return NOVAS2.TopoPlanet(tjd, ss_object, earth, deltat, location, ra, dec, dis)
        End Function

        ''' <summary>
        ''' Computes the topocentric place of a star
        ''' </summary>
        ''' <param name="tjd"> TT (or TDT) Julian date for topocentric place.</param>
        ''' <param name="earth">Structure containing the body designation for the Earth.</param>
        ''' <param name="deltat">Difference TT (or TDT)-UT1 at 'tjd', in seconds.</param>
        ''' <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units.</param>
        ''' <param name="location">Structure containing observer's location</param>
        ''' <param name="ra">OUT: Topocentric right ascension in hours, referred to true equator and equinox of date 'tjd'.</param>
        ''' <param name="dec">OUT: Topocentric declination in degrees, referred to true equator and equinox of date 'tjd'.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'.</pre></returns>
        ''' <remarks></remarks>
        Public Function TopoStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByVal deltat As Double, ByRef star As CatEntry, ByRef location As SiteInfo, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.TopoStar
            Return NOVAS2.TopoStar(tjd, earth, deltat, star, location, ra, dec)
        End Function

        ''' <summary>
        ''' To transform a star's catalog quantities for a change of epoch and/or equator and equinox.
        ''' </summary>
        ''' <param name="option">Transformation option<pre>
        '''    = 1 ... change epoch; same equator and equinox
        '''    = 2 ... change equator and equinox; same epoch
        '''    = 3 ... change equator and equinox and epoch
        '''</pre></param>
        ''' <param name="date_incat">TT Julian date, or year, of input catalog data.</param>
        ''' <param name="incat">An entry from the input catalog</param>
        ''' <param name="date_newcat">TT Julian date, or year, of transformed catalog data.</param>
        ''' <param name="newcat_id">Three-character abbreviated name of the transformed catalog.</param>
        ''' <param name="newcat">OUT: The transformed catalog entry</param>
        ''' <remarks><pre>
        ''' 1. 'date_incat' and 'date_newcat' may be specified either as a 
        '''    Julian date (e.g., 2433282.5) or a Julian year and fraction 
        '''    (e.g., 1950.0).  Values less than 10000 are assumed to be years.
        ''' 
        ''' 2. option = 1 updates the star's data to account for the star's space motion between 
        '''               the first and second dates, within a fixed reference frame.
        '''    option = 2 applies a rotation of the reference frame corresponding to precession 
        '''               between the first and second dates, but leaves the star fixed in space.
        '''    option = 3 provides both transformations.
        ''' 
        ''' 3. This subroutine cannot be properly used to bring data from 
        '''    old (pre-FK5) star catalogs into the modern system, because old 
        '''    catalogs were compiled using a set of constants that are 
        '''    incompatible with the IAU (1976) system.
        ''' 
        ''' 4. This function uses TDB Julian dates internally, but no 
        '''    distinction between TDB and TT is necessary.
        '''</pre></remarks>
        Public Sub TransformCat(ByVal [option] As TransformationOption, ByVal date_incat As Double, ByRef incat As CatEntry, ByVal date_newcat As Double, ByRef newcat_id() As Byte, ByRef newcat As CatEntry) Implements INOVAS2.TransformCat
            NOVAS2.TransformCat([option], date_incat, incat, date_newcat, newcat_id, newcat)
        End Sub

        ''' <summary>
        '''  To convert Hipparcos data at epoch J1991.25 to epoch J2000.0 and FK5-style units.
        ''' </summary>
        ''' <param name="hipparcos">An entry from the Hipparcos catalog, at epoch J1991.25, with all members having Hipparcos catalog units.  See Note 1 below</param>
        ''' <param name="fk5">The transformed input entry, at epoch J2000.0, with all  members having FK5 catalog units.  See Note 2 below</param>
        ''' <remarks>To be used only for Hipparcos or Tycho stars  with linear space motion.
        ''' <para><pre>
        ''' 1. Hipparcos epoch and units:
        '''    Epoch: J1991.25
        '''    Right ascension (RA): degrees
        '''    Declination (Dec): degrees
        '''    Proper motion in RA * cos (Dec): milliarcseconds per year
        '''    Proper motion in Dec: milliarcseconds per year
        '''    Parallax: milliarcseconds
        '''    Radial velocity: kilometers per second (not in catalog)
        ''' 
        ''' 2. FK5 epoch and units:
        '''    Epoch: J2000.0
        '''    Right ascension: hours
        '''    Declination: degrees
        '''    Proper motion in RA: seconds of time per Julian century
        '''    Proper motion in Dec: arcseconds per Julian century
        '''    Parallax: arcseconds
        '''    Radial velocity: kilometers per second
        '''</pre></para></remarks>
        Public Sub TransformHip(ByRef hipparcos As CatEntry, ByRef fk5 As CatEntry) Implements INOVAS2.TransformHip
            NOVAS2.TransformHip(hipparcos, fk5)
        End Sub

        ''' <summary>
        ''' Converts an vector in equatorial rectangular coordinates to equatorial spherical coordinates.
        ''' </summary>
        ''' <param name="pos">Position vector, equatorial rectangular coordinates.</param>
        ''' <param name="ra">OUT: Right ascension in hours.</param>
        ''' <param name="dec">OUT: Declination in degrees.</param>
        ''' <returns><pre>
        ''' 0...Everything OK.
        ''' 1...All vector components are zero; 'ra' and 'dec' are indeterminate.
        ''' 2...Both vec[0] and vec[1] are zero, but vec[2] is nonzero; 'ra' is indeterminate.</pre>
        ''' </returns>
        ''' <remarks></remarks>
        Public Function Vector2RADec(ByVal pos() As Double, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.Vector2RADec
            Return NOVAS2.Vector2RADec(pos, ra, dec)
        End Function

        ''' <summary>
        ''' Computes the virtual place of a planet or other solar system body.
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for virtual place.</param>
        ''' <param name="ss_object">Pointer to structure containing the body designation for the solar system body</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth</param>
        ''' <param name="ra">OUT: Virtual right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT: Virtual declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dis">OUT: True distance from Earth to planet in AU.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...See error description in function 'ephemeris'.
        ''' </pre></returns>
        ''' <remarks></remarks>
        Public Function VirtualPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.VirtualPlanet
            Return NOVAS2.VirtualPlanet(tjd, ss_object, earth, ra, dec, dis)
        End Function

        ''' <summary>
        '''  Computes the virtual place of a star
        ''' </summary>
        ''' <param name="tjd">TT (or TDT) Julian date for virtual place.</param>
        ''' <param name="earth">Pointer to structure containing the body designation for the Earth.</param>
        ''' <param name="star">Pointer to catalog entry structure containing J2000.0 catalog data with FK5-style units</param>
        ''' <param name="ra">OUT: Virtual right ascension in hours, referred to mean equator and equinox of J2000.</param>
        ''' <param name="dec">OUT: Virtual declination in degrees, referred to mean equator and equinox of J2000.</param>
        ''' <returns><pre>
        '''  0...Everything OK.
        ''' >0...Error code from function 'solarsystem'
        ''' </pre></returns>
        ''' <remarks>
        ''' Computes the virtual place of a star at date 'tjd', given its 
        ''' mean place, proper motion, parallax, and radial velocity for J2000.0.</remarks>
        Public Function VirtualStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByRef star As CatEntry, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.VirtualStar
            Return NOVAS2.VirtualStar(tjd, earth, star, ra, dec)
        End Function

        ''' <summary>
        ''' Corrects Earth-fixed geocentric rectangular coordinates for polar motion.
        ''' </summary>
        ''' <param name="x"> Conventionally-defined X coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="pos1">Vector in geocentric rectangular Earth-fixed system, referred to geographic equator and Greenwich meridian.</param>
        ''' <param name="pos2">OUT: Vector in geocentric rectangular rotating system, referred to rotational equator and orthogonal Greenwich meridian</param>
        ''' <remarks>Corrects Earth-fixed geocentric rectangular coordinates for polar motion.  Transforms a vector from Earth-fixed geographic system to rotating system based on rotational equator and orthogonal Greenwich meridian through axis of rotation.</remarks>
        Public Sub Wobble(ByVal x As Double, ByVal y As Double, ByVal pos1() As Double, ByRef pos2() As Double) Implements INOVAS2.Wobble
            NOVAS2.Wobble(x, y, pos1, pos2)
        End Sub
    End Class
End Namespace