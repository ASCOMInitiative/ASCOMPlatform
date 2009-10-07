'NOVAS2 component implementation
'This class is a public front that simply calls the relevant 32 or 64bit funciton in compiled C code

Imports System.Runtime.InteropServices
Imports ASCOM.Utilities
'See the Is64Bit function for code that enables this assembly to find the C NOVAS DLLs

Namespace NOVAS

    ''' <summary>
    ''' NOVAS: Class presenting the contents of the NOVAS 2 library. 
    ''' NOVAS was developed by the Astronomical Applications department of the United States Naval 
    ''' Observatory. The C language version of NOVAS was developed by John Bangert at USNO/AA.
    ''' </summary>
    ''' <remarks>
    ''' The NOVAS class is a STATIC class and is the component of preference for .NET programmers. 
    ''' This means that you do not have to create an instance of the 
    ''' class in order to access its members. Instead you reference them directly from the class. So, this works:
    ''' <code>rc = ASCOM.Astrometry.NOVAS2.AppStar(tjd, earth, star, ra, dec)</code> 
    ''' while this does not work: 
    ''' <code>
    ''' Dim Nov as New ASCOM.Astrometry.NOVAS2
    ''' rc = Nov.AppStar(tjd, earth, star, ra, dec)
    ''' </code>
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
    <ComVisible(False)> _
    Public Class NOVAS2 'Static classes cannot be exposed through COM

        Private Const NOVAS32Dll As String = "NOVAS-C.dll"
        Private Const NOVAS64Dll As String = "NOVAS-C64.dll"
        Private Const NOVAS_DLL_LOCATION As String = "\ASCOM\.net" 'This is appended to the Common Files path

#Region "Private Structures"
        'Version of marshaling required by the DLLs
        'This structure is an analogue of the public BodyDescription structure except that integer items
        'Are replaced with short integer items. This allows the caller to work in integer values while
        'the NOVAS C routines receive the expected short integers.
        <StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.[Ansi])> _
        Private Structure BodyDescriptionShort
            Public Type As Short
            Public Number As Short
            <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=100)> _
            Public Name As String 'char[100]
        End Structure

        'Version of marshaling required by the DLLs
        <StructLayoutAttribute(LayoutKind.Sequential, CharSet:=CharSet.[Ansi])> _
        Private Structure CatEntryNOVAS2
            <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=4)> _
            Public Catalog As String 'char[4]
            <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=51)> _
            Public StarName As String 'char[51]
            Public StarNumber As Integer
            Public RA As Double
            Public Dec As Double
            Public ProMoRA As Double
            Public ProMoDec As Double
            Public Parallax As Double
            Public RadialVelocity As Double
        End Structure

#End Region

#Region "Public Interface"
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
        Public Shared Function AppStar(ByVal tjd As Double, _
                                            ByRef earth As BodyDescription, _
                                            ByRef star As CatEntry, _
                                            ByRef ra As Double, _
                                            ByRef dec As Double) As Short
            If Is64Bit() Then
                Return app_star64(tjd, BodyDescToShort(earth), CatEntryToCatEntryNOVAS2(star), ra, dec)
            Else
                Return app_star32(tjd, BodyDescToShort(earth), CatEntryToCatEntryNOVAS2(star), ra, dec)
            End If
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
        Public Shared Function TopoStar(ByVal tjd As Double, _
                                             ByRef earth As BodyDescription, _
                                             ByVal deltat As Double, _
                                             ByRef star As CatEntry, _
                                             ByRef location As SiteInfo, _
                                             ByRef ra As Double, _
                                             ByRef dec As Double) As Short
            If Is64Bit() Then
                Return topo_star64(tjd, BodyDescToShort(earth), deltat, CatEntryToCatEntryNOVAS2(star), location, ra, dec)
            Else
                Return topo_star32(tjd, BodyDescToShort(earth), deltat, CatEntryToCatEntryNOVAS2(star), location, ra, dec)
            End If

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
        Public Shared Function AppPlanet(ByVal tjd As Double, _
                                              ByRef ss_object As BodyDescription, _
                                              ByRef earth As BodyDescription, _
                                              ByRef ra As Double, _
                                              ByRef dec As Double, _
                                              ByRef dis As Double) As Short

            If Is64Bit() Then
                Return app_planet64(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), ra, dec, dis)
            Else
                Return app_planet32(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), ra, dec, dis)
            End If
        End Function
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
        Public Shared Function TopoPlanet(ByVal tjd As Double, _
                                               ByRef ss_object As BodyDescription, _
                                               ByRef earth As BodyDescription, _
                                               ByVal deltat As Double, _
                                               ByRef location As SiteInfo, _
                                               ByRef ra As Double, _
                                               ByRef dec As Double, _
                                               ByRef dis As Double) As Short
            If Is64Bit() Then
                Return topo_planet64(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), deltat, location, ra, dec, dis)
            Else
                Return topo_planet32(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), deltat, location, ra, dec, dis)
            End If
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
        Public Shared Function VirtualStar(ByVal tjd As Double, _
                                                    ByRef earth As BodyDescription, _
                                                    ByRef star As CatEntry, _
                                                    ByRef ra As Double, _
                                                    ByRef dec As Double) As Short
            If Is64Bit() Then
                Return virtual_star64(tjd, BodyDescToShort(earth), CatEntryToCatEntryNOVAS2(star), ra, dec)
            Else
                Return virtual_star32(tjd, BodyDescToShort(earth), CatEntryToCatEntryNOVAS2(star), ra, dec)
            End If
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
        Public Shared Function LocalStar(ByVal tjd As Double, _
                                                  ByRef earth As BodyDescription, _
                                                  ByVal deltat As Double, _
                                                  ByRef star As CatEntry, _
                                                  ByRef location As SiteInfo, _
                                                  ByRef ra As Double, _
                                                  ByRef dec As Double) As Short
            If Is64Bit() Then
                Return local_star64(tjd, BodyDescToShort(earth), deltat, CatEntryToCatEntryNOVAS2(star), location, ra, dec)
            Else
                Return local_star32(tjd, BodyDescToShort(earth), deltat, CatEntryToCatEntryNOVAS2(star), location, ra, dec)
            End If
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
        Public Shared Function VirtualPlanet(ByVal tjd As Double, _
                                                      ByRef ss_object As BodyDescription, _
                                                      ByRef earth As BodyDescription, _
                                                      ByRef ra As Double, _
                                                      ByRef dec As Double, _
                                                      ByRef dis As Double) As Short
            If Is64Bit() Then
                Return virtual_planet64(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), ra, dec, dis)
            Else
                Return virtual_planet32(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), ra, dec, dis)
            End If
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
        Public Shared Function LocalPlanet(ByVal tjd As Double, _
                                                    ByRef ss_object As BodyDescription, _
                                                    ByRef earth As BodyDescription, _
                                                    ByVal deltat As Double, _
                                                    ByRef location As SiteInfo, _
                                                    ByRef ra As Double, _
                                                    ByRef dec As Double, _
                                                    ByRef dis As Double) As Short
            If Is64Bit() Then
                Return local_planet64(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), deltat, location, ra, dec, dis)
            Else
                Return local_planet32(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), deltat, location, ra, dec, dis)
            End If
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
        Public Shared Function AstroStar(ByVal tjd As Double, _
                                                  ByRef earth As BodyDescription, _
                                                  ByRef star As CatEntry, _
                                                  ByRef ra As Double, _
                                                  ByRef dec As Double) As Short
            If Is64Bit() Then
                Return astro_star64(tjd, BodyDescToShort(earth), CatEntryToCatEntryNOVAS2(star), ra, dec)
            Else
                Return astro_star32(tjd, BodyDescToShort(earth), CatEntryToCatEntryNOVAS2(star), ra, dec)
            End If
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
        Public Shared Function AstroPlanet(ByVal tjd As Double, _
                                                    ByRef ss_object As BodyDescription, _
                                                    ByRef earth As BodyDescription, _
                                                    ByRef ra As Double, _
                                                    ByRef dec As Double, _
                                                    ByRef dis As Double) As Short
            If Is64Bit() Then
                Return astro_planet64(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), ra, dec, dis)
            Else
                Return astro_planet32(tjd, BodyDescToShort(ss_object), BodyDescToShort(earth), ra, dec, dis)
            End If
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
        Public Shared Sub Equ2Hor(ByVal tjd As Double, _
                                          ByVal deltat As Double, _
                                          ByVal x As Double, _
                                          ByVal y As Double, _
                                          ByRef location As SiteInfo, _
                                          ByVal ra As Double, _
                                          ByVal dec As Double, _
                                          ByVal ref_option As RefractionOption, _
                                          ByRef zd As Double, _
                                          ByRef az As Double, _
                                          ByRef rar As Double, _
                                          ByRef decr As Double)
            If Is64Bit() Then
                equ2hor64(tjd, deltat, x, y, location, ra, dec, CShort(ref_option), zd, az, rar, decr)
            Else
                equ2hor32(tjd, deltat, x, y, location, ra, dec, CShort(ref_option), zd, az, rar, decr)
            End If
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
        Public Shared Sub TransformHip(ByRef hipparcos As CatEntry, _
                                                ByRef fk5 As CatEntry)
            Dim CEN2 As New CatEntryNOVAS2

            If Is64Bit() Then
                transform_hip64(CatEntryToCatEntryNOVAS2(hipparcos), CEN2)
            Else
                transform_hip32(CatEntryToCatEntryNOVAS2(hipparcos), CEN2)
            End If
            Call CatEntryNOVAS2ToCatEntry(CEN2, fk5) 'Transform to external CatEntry presentation
        End Sub
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
        Public Shared Sub TransformCat(ByVal [option] As TransformationOption, _
                                                ByVal date_incat As Double, _
                                                ByRef incat As CatEntry, _
                                                ByVal date_newcat As Double, _
                                                ByRef newcat_id As Byte(), _
                                                ByRef newcat As CatEntry)
            Dim CEN2 As New CatEntryNOVAS2

            If Is64Bit() Then
                transform_cat64(CShort([option]), date_incat, CatEntryToCatEntryNOVAS2(incat), date_newcat, newcat_id, CEN2)
            Else
                transform_cat32(CShort([option]), date_incat, CatEntryToCatEntryNOVAS2(incat), date_newcat, newcat_id, CEN2)
            End If
            Call CatEntryNOVAS2ToCatEntry(CEN2, newcat) 'Transform to external CatEntry presentation
        End Sub
        ''' <summary>
        '''  Computes the Greenwich apparent sidereal time, at Julian date 'jd_high' + 'jd_low'.
        ''' </summary>
        ''' <param name="jd_high">Julian date, integral part.</param>
        ''' <param name="jd_low">Julian date, fractional part.</param>
        ''' <param name="ee"> Equation of the equinoxes (seconds of time). [Note: this  quantity is computed by function 'earthtilt'.]</param>
        ''' <param name="gst">Greenwich apparent sidereal time, in hours.</param>
        ''' <remarks></remarks>
        Public Shared Sub SiderealTime(ByVal jd_high As Double, _
                                                ByVal jd_low As Double, _
                                                ByVal ee As Double, _
                                                ByRef gst As Double)
            If Is64Bit() Then
                sidereal_time64(jd_high, jd_low, ee, gst)
            Else
                sidereal_time32(jd_high, jd_low, ee, gst)
            End If
        End Sub
        ''' <summary>
        ''' Precesses equatorial rectangular coordinates from one epoch to another.
        ''' </summary>
        ''' <param name="tjd1">TDB Julian date of first epoch.</param>
        ''' <param name="pos">Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of first epoch.</param>
        ''' <param name="tjd2">TDB Julian date of second epoch.</param>
        ''' <param name="pos2">OUT: Position vector, geocentric equatorial rectangular coordinates, referred to mean equator and equinox of second epoch.</param>
        ''' <remarks>The coordinates are referred to the mean equator and equinox of the two respective epochs.</remarks>
        Public Shared Sub Precession(ByVal tjd1 As Double, ByVal pos As Double(), ByVal tjd2 As Double, ByRef pos2 As Double())
            Dim posv2 As New PosVector
            If Is64Bit() Then
                precession64(tjd1, ArrToPosVec(pos), tjd2, posv2)
            Else
                precession32(tjd1, ArrToPosVec(pos), tjd2, posv2)
            End If
            PosVecToArr(posv2, pos2)
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
        Public Shared Sub EarthTilt(ByVal tjd As Double, _
                                            ByRef mobl As Double, _
                                            ByRef tobl As Double, _
                                            ByRef eq As Double, _
                                            ByRef dpsi As Double, _
                                            ByRef deps As Double)
            If Is64Bit() Then
                earthtilt64(tjd, mobl, tobl, eq, dpsi, deps)
            Else
                earthtilt32(tjd, mobl, tobl, eq, dpsi, deps)
            End If
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
        Public Shared Sub CelPole(ByVal del_dpsi As Double, _
                                          ByVal del_deps As Double)
            If Is64Bit() Then
                cel_pole64(del_dpsi, del_deps)
            Else
                cel_pole32(del_dpsi, del_deps)
            End If
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
        Public Shared Function Ephemeris(ByVal tjd As Double, _
                                            ByRef cel_obj As BodyDescription, _
                                            ByVal origin As Origin, _
                                            ByRef pos As Double(), _
                                            ByRef vel As Double()) As Short
            Dim posv As New PosVector, velv As New VelVector, rc As Short
            If Is64Bit() Then
                rc = ephemeris64(tjd, BodyDescToShort(cel_obj), CShort(origin), posv, velv)
            Else
                rc = ephemeris32(tjd, BodyDescToShort(cel_obj), CShort(origin), posv, velv)
            End If
            PosVecToArr(posv, pos)
            VelVecToArr(velv, vel)
            Return rc
        End Function
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
        Public Shared Function SolarSystem(ByVal tjd As Double, _
                                                   ByVal body As Body, _
                                                   ByVal origin As Origin, _
                                                   ByRef pos As Double(), _
                                                   ByRef vel As Double()) As Short
            Dim posv As New PosVector, velv As New VelVector, rc As Short
            If Is64Bit() Then
                rc = solarsystem64(tjd, CShort(body), CShort(origin), posv, velv)
            Else
                rc = solarsystem32(tjd, CShort(body), CShort(origin), posv, velv)
            End If
            PosVecToArr(posv, pos)
            VelVecToArr(velv, vel)
            Return rc
        End Function
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
        Public Shared Function Vector2RADec(ByVal pos As Double(), _
                                                    ByRef ra As Double, _
                                                    ByRef dec As Double) As Short
            Dim rc As Short
            If Is64Bit() Then
                rc = vector2radec64(ArrToPosVec(pos), ra, dec)
            Else
                rc = vector2radec32(ArrToPosVec(pos), ra, dec)
            End If
            Return rc
        End Function

        ''' <summary>
        ''' Converts angular quanities for stars to vectors.
        ''' </summary>
        ''' <param name="star">Catalog entry structure containing J2000.0 catalog data with FK5-style units </param>
        ''' <param name="pos">Position vector, equatorial rectangular coordinates, components in AU.</param>
        ''' <param name="vel">Velocity vector, equatorial rectangular coordinates, components in AU/Day.</param>
        ''' <remarks></remarks>
        Public Shared Sub StarVectors(ByVal star As CatEntry, ByRef pos As Double(), ByRef vel As Double())
            Dim posv As New PosVector, velv As New VelVector
            If Is64Bit() Then
                starvectors64(CatEntryToCatEntryNOVAS2(star), posv, velv)
            Else
                starvectors32(CatEntryToCatEntryNOVAS2(star), posv, velv)
            End If
            PosVecToArr(posv, pos)
            VelVecToArr(velv, vel)
        End Sub

        ''' <summary>
        ''' Converts equatorial spherical coordinates to a vector (equatorial rectangular coordinates).
        ''' </summary>
        ''' <param name="ra">Right ascension (hours).</param>
        ''' <param name="dec">Declination (degrees).</param>
        ''' <param name="dist">Distance</param>
        ''' <param name="pos">Position vector, equatorial rectangular coordinates (AU).</param>
        ''' <remarks></remarks>
        Public Shared Sub RADec2Vector(ByVal ra As Double, _
                                                  ByVal dec As Double, _
                                                  ByVal dist As Double, _
                                                  ByRef pos As Double())
            Dim posv As New PosVector
            If Is64Bit() Then
                radec2vector64(ra, dec, dist, posv)
            Else
                radec2vector32(ra, dec, dist, posv)
            End If
            PosVecToArr(posv, pos)
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
        Public Shared Function GetEarth(ByVal tjd As Double, _
                                                ByRef earth As BodyDescription, _
                                                ByRef tdb As Double, _
                                                ByRef bary_earthp As Double(), _
                                                ByRef bary_earthv As Double(), _
                                                ByRef helio_earthp As Double(), _
                                                ByRef helio_earthv As Double()) As Short
            Dim rc As Short
            Dim vbary_earthp, vhelio_earthp As New PosVector, vbary_earthv, vhelio_earthv As New VelVector
            If Is64Bit() Then
                rc = get_earth64(tjd, BodyDescToShort(earth), tdb, vbary_earthp, vbary_earthv, vhelio_earthp, vhelio_earthv)
            Else
                rc = get_earth32(tjd, BodyDescToShort(earth), tdb, vbary_earthp, vbary_earthv, vhelio_earthp, vhelio_earthv)
            End If
            PosVecToArr(vbary_earthp, bary_earthp)
            VelVecToArr(vbary_earthv, bary_earthv)
            PosVecToArr(vhelio_earthp, helio_earthp)
            VelVecToArr(vhelio_earthv, helio_earthv)
            Return rc
        End Function
        '
        'START OF NEW
        '
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
        Public Shared Function MeanStar(ByVal tjd As Double, _
                                                    ByRef earth As BodyDescription, _
                                                    ByVal ra As Double, _
                                                    ByVal dec As Double, _
                                                    ByRef mra As Double, _
                                                    ByRef mdec As Double) As Short
            Dim rc As Short
            If Is64Bit() Then
                rc = mean_star64(tjd, BodyDescToShort(earth), ra, dec, mra, mdec)
            Else
                rc = mean_star32(tjd, BodyDescToShort(earth), ra, dec, mra, mdec)
            End If
            Return rc
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
        Public Shared Sub Pnsw(ByVal tjd As Double, _
                                       ByVal gast As Double, _
                                       ByVal x As Double, _
                                       ByVal y As Double, _
                                       ByVal vece As Double(), _
                                       ByRef vecs As Double())
            Dim vvecs As New PosVector
            If Is64Bit() Then
                pnsw64(tjd, gast, x, y, ArrToPosVec(vece), vvecs)
            Else
                pnsw32(tjd, gast, x, y, ArrToPosVec(vece), vvecs)
            End If
            PosVecToArr(vvecs, vecs)
        End Sub
        ''' <summary>
        ''' Transforms geocentric rectangular coordinates from rotating system to non-rotating system
        ''' </summary>
        ''' <param name="st">Local apparent sidereal time at reference meridian, in hours.</param>
        ''' <param name="pos1">Vector in geocentric rectangular rotating system, referred to rotational equator and orthogonal reference meridian.</param>
        ''' <param name="pos2">OUT: Vector in geocentric rectangular non-rotating system, referred to true equator and equinox of date.</param>
        ''' <remarks>Transforms geocentric rectangular coordinates from rotating system based on rotational equator and orthogonal reference meridian to  non-rotating system based on true equator and equinox of date.</remarks>
        Public Shared Sub Spin(ByVal st As Double, _
                                       ByVal pos1 As Double(), _
                                       ByRef pos2 As Double())
            Dim vpos2 As New PosVector
            If Is64Bit() Then
                spin64(st, ArrToPosVec(pos1), vpos2)
            Else
                spin32(st, ArrToPosVec(pos1), vpos2)
            End If
            PosVecToArr(vpos2, pos2)
        End Sub

        ''' <summary>
        ''' Corrects Earth-fixed geocentric rectangular coordinates for polar motion.
        ''' </summary>
        ''' <param name="x"> Conventionally-defined X coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="y">Conventionally-defined Y coordinate of rotational pole with respect to CIO, in arcseconds.</param>
        ''' <param name="pos1">Vector in geocentric rectangular Earth-fixed system, referred to geographic equator and Greenwich meridian.</param>
        ''' <param name="pos2">OUT: Vector in geocentric rectangular rotating system, referred to rotational equator and orthogonal Greenwich meridian</param>
        ''' <remarks>Corrects Earth-fixed geocentric rectangular coordinates for polar motion.  Transforms a vector from Earth-fixed geographic system to rotating system based on rotational equator and orthogonal Greenwich meridian through axis of rotation.</remarks>
        Public Shared Sub Wobble(ByVal x As Double, _
                                            ByVal y As Double, _
                                            ByVal pos1 As Double(), _
                                            ByRef pos2 As Double())
            Dim vpos2 As New PosVector
            If Is64Bit() Then
                wobble64(x, y, ArrToPosVec(pos1), vpos2)
            Else
                wobble32(x, y, ArrToPosVec(pos1), vpos2)
            End If
            PosVecToArr(vpos2, pos2)
        End Sub
        ''' <summary>
        ''' Computes the position and velocity vectors of a terrestrial observer with respect to the center of the Earth.
        ''' </summary>
        ''' <param name="locale">Longitude, latitude and height of the observer (in a SiteInfoStruct)</param>
        ''' <param name="st">Local apparent sidereal time at reference meridian in hours.</param>
        ''' <param name="pos"> Position vector of observer with respect to center of Earth, equatorial rectangular coordinates, referred to true equator and equinox of date, components in AU.</param>
        ''' <param name="vel"> Velocity vector of observer with respect to center of Earth, equatorial rectangular coordinates, referred to true equator and equinox of date, components in AU/Day.</param>
        ''' <remarks></remarks>
        Public Shared Sub Terra(ByRef locale As SiteInfo, _
                                           ByVal st As Double, _
                                           ByRef pos As Double(), _
                                           ByRef vel As Double())
            Dim vpos As New PosVector, vvel As New VelVector
            If Is64Bit() Then
                terra64(locale, st, vpos, vvel)
            Else
                terra32(locale, st, vpos, vvel)
            End If
            PosVecToArr(vpos, pos)
            VelVecToArr(vvel, vel)
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
        Public Shared Sub ProperMotion(ByVal tjd1 As Double, _
                                                ByVal pos As Double(), _
                                                ByVal vel As Double(), _
                                                ByVal tjd2 As Double, _
                                                ByRef pos2 As Double())
            Dim vpos2 As New PosVector
            If Is64Bit() Then
                proper_motion64(tjd1, ArrToPosVec(pos), ArrToVelVec(vel), tjd2, vpos2)
            Else
                proper_motion32(tjd1, ArrToPosVec(pos), ArrToVelVec(vel), tjd2, vpos2)
            End If
            PosVecToArr(vpos2, pos2)
        End Sub
        ''' <summary>
        ''' Moves the origin of coordinates from the barycenter of the solar system to the center of mass of the Earth
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at solar system barycenter, components in AU.</param>
        ''' <param name="earthvector">Position vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU.</param>
        ''' <param name="pos2">OUT: Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="lighttime">OUT: Light time from body to Earth in days.</param>
        ''' <remarks>This corrects for parallax.</remarks>
        Public Shared Sub BaryToGeo(ByVal pos As Double(), _
                                              ByVal earthvector As Double(), _
                                              ByRef pos2 As Double(), _
                                              ByRef lighttime As Double)
            Dim vpos2 As PosVector
            If Is64Bit() Then
                bary_to_geo64(ArrToPosVec(pos), ArrToPosVec(earthvector), vpos2, lighttime)
            Else
                bary_to_geo32(ArrToPosVec(pos), ArrToPosVec(earthvector), vpos2, lighttime)
            End If
            PosVecToArr(vpos2, pos2)
        End Sub
        ''' <summary>
        ''' Corrects position vector for the deflection of light in the gravitational field of the Sun. 
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="earthvector">Position vector of center of mass of the Earth, referred to origin at center of mass of the Sun, components in AU.</param>
        ''' <param name="pos2">Position vector, referred to origin at center of mass of the Earth, corrected for gravitational deflection, components in AU.</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks>This function is valid for bodies within the solar system as well as for stars.</remarks>
        Public Shared Function SunField(ByVal pos As Double(), _
                                                 ByVal earthvector As Double(), _
                                                 ByRef pos2 As Double()) As Short
            Dim vpos2 As PosVector, rc As Short
            If Is64Bit() Then
                rc = sun_field64(ArrToPosVec(pos), ArrToPosVec(earthvector), vpos2)
            Else
                rc = sun_field32(ArrToPosVec(pos), ArrToPosVec(earthvector), vpos2)
            End If
            PosVecToArr(vpos2, pos2)
            Return rc
        End Function

        ''' <summary>
        ''' Corrects position vector for aberration of light.
        ''' </summary>
        ''' <param name="pos">Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        ''' <param name="vel">Velocity vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU/day.</param>
        ''' <param name="lighttime">Light time from body to Earth in days.</param>
        ''' <param name="pos2">OUT: Position vector, referred to origin at center of mass of the Earth, corrected for aberration, components in AU</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks>Algorithm includes relativistic terms.</remarks>
        Public Shared Function Aberration(ByVal pos As Double(), _
                                                  ByVal vel As Double(), _
                                                  ByVal lighttime As Double, _
                                                  ByRef pos2 As Double()) As Short
            Dim vpos2 As PosVector, rc As Short
            If Is64Bit() Then
                rc = aberration64(ArrToPosVec(pos), ArrToVelVec(vel), lighttime, vpos2)
            Else
                rc = aberration32(ArrToPosVec(pos), ArrToVelVec(vel), lighttime, vpos2)
            End If
            PosVecToArr(vpos2, pos2)
            Return rc
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
        Public Shared Function Nutate(ByVal tjd As Double, _
                                              ByVal fn As NutationDirection, _
                                              ByVal pos As Double(), _
                                              ByRef pos2 As Double()) As Short
            Dim vpos2 As PosVector, rc As Short
            If Is64Bit() Then
                rc = nutate64(tjd, CShort(fn), ArrToPosVec(pos), vpos2)
            Else
                rc = nutate32(tjd, CShort(fn), ArrToPosVec(pos), vpos2)
            End If
            PosVecToArr(vpos2, pos2)
            Return rc
        End Function

        ''' <summary>
        ''' Provides fast evaluation of the nutation components according to the 1980 IAU Theory of Nutation.
        ''' </summary>
        ''' <param name="tdbtime">TDB time in Julian centuries since J2000.0</param>
        ''' <param name="longnutation">OUT: Nutation in longitude in arcseconds.</param>
        ''' <param name="obliqnutation">OUT: Nutation in obliquity in arcseconds.</param>
        ''' <returns>0...Everything OK.</returns>
        ''' <remarks></remarks>
        Public Shared Function NutationAngles(ByVal tdbtime As Double, _
                                                       ByRef longnutation As Double, _
                                                       ByRef obliqnutation As Double) As Short
            If Is64Bit() Then
                Return nutation_angles64(tdbtime, longnutation, obliqnutation)
            Else
                Return nutation_angles32(tdbtime, longnutation, obliqnutation)
            End If
        End Function

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
        Public Shared Sub FundArgs(ByVal t As Double, _
                                            ByRef a As Double())
            Dim va As New FundamentalArgs
            If Is64Bit() Then
                fund_args64(t, va)
            Else
                fund_args32(t, va)
            End If
            a(0) = va.l
            a(1) = va.ldash
            a(2) = va.F
            a(3) = va.D
            a(4) = va.Omega
        End Sub
        ''' <summary>
        ''' Converts TDB to TT or TDT
        ''' </summary>
        ''' <param name="tdb">TDB Julian date.</param>
        ''' <param name="tdtjd">OUT: TT (or TDT) Julian date.</param>
        ''' <param name="secdiff">OUT: Difference tdbjd-tdtjd, in seconds.</param>
        ''' <remarks>Computes the terrestrial time (TT) or terrestrial dynamical time (TDT) Julian date corresponding to a barycentric dynamical time (TDB) Julian date.</remarks>
        Public Shared Sub Tdb2Tdt(ByVal tdb As Double, _
                                          ByRef tdtjd As Double, _
                                          ByRef secdiff As Double)
            If Is64Bit() Then
                tdb2tdt64(tdb, tdtjd, secdiff)
            Else
                tdb2tdt32(tdb, tdtjd, secdiff)
            End If

        End Sub

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
        Public Shared Function SetBody(ByVal type As BodyType, _
                                                ByVal number As Body, _
                                                ByVal name As String, _
                                                ByRef cel_obj As BodyDescription) As Short
            Dim BDS As New BodyDescriptionShort
            If Is64Bit() Then
                Return set_body64(CShort(type), CShort(number), name, BDS)
            Else
                Return set_body32(CShort(type), CShort(number), name, BDS)
            End If

            'Set values in the return object
            cel_obj.Name = BDS.Name
            cel_obj.Number = CType(BDS.Number, Body)
            cel_obj.Type = CType(BDS.Type, BodyType)
        End Function

        'Public Shared Function readeph(ByVal mp As Integer, _
        '                               ByVal name As System.IntPtr, _
        '                               ByVal jd As Double, _
        '                               ByRef err As Integer) As posvector
        'End Function

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
        Public Shared Sub MakeCatEntry(ByVal catalog As String, _
                                                 ByVal star_name As String, _
                                                 ByVal star_num As Integer, _
                                                 ByVal ra As Double, _
                                                 ByVal dec As Double, _
                                                 ByVal pm_ra As Double, _
                                                 ByVal pm_dec As Double, _
                                                 ByVal parallax As Double, _
                                                 ByVal rad_vel As Double, _
                                                 ByRef star As CatEntry)
            Dim CEN2 As New CatEntryNOVAS2

            If Is64Bit() Then
                make_cat_entry64(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, CEN2)
            Else
                make_cat_entry32(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, CEN2)
            End If
            Call CatEntryNOVAS2ToCatEntry(CEN2, star) 'Convert to external presentation

        End Sub

        ''' <summary>
        ''' Computes atmospheric refraction in zenith distance.
        ''' </summary>
        ''' <param name="location">structure containing observer's location</param>
        ''' <param name="ref_option">refraction option</param>
        ''' <param name="zd_obs">bserved zenith distance, in degrees.</param>
        ''' <returns>Atmospheric refraction, in degrees.</returns>
        ''' <remarks>This version computes approximate refraction for optical wavelengths.</remarks>
        Public Shared Function Refract(ByRef location As SiteInfo, _
                                               ByVal ref_option As Short, _
                                               ByVal zd_obs As Double) As Double
            If Is64Bit() Then
                Return refract64(location, ref_option, zd_obs)
            Else
                Return refract32(location, ref_option, zd_obs)
            End If
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
        Public Shared Function JulianDate(ByVal year As Short, _
                                                   ByVal month As Short, _
                                                   ByVal day As Short, _
                                                   ByVal hour As Double) As Double
            If Is64Bit() Then
                Return julian_date64(year, month, day, hour)
            Else
                Return julian_date32(year, month, day, hour)
            End If
        End Function

        ''' <summary>
        ''' Compute a date on the Gregorian calendar given the Julian date.
        ''' </summary>
        ''' <param name="tjd">Julian date.</param>
        ''' <param name="year">OUT: Year number</param>
        ''' <param name="month">OUT: Month number.</param>
        ''' <param name="day">OUT: Day number</param>
        ''' <param name="hour">OUT: Time in hours</param>
        ''' <remarks></remarks>
        Public Shared Sub CalDate(ByVal tjd As Double, _
                                           ByRef year As Short, _
                                           ByRef month As Short, _
                                           ByRef day As Short, _
                                           ByRef hour As Double)
            If Is64Bit() Then
                cal_date64(tjd, year, month, day, hour)
            Else
                cal_date32(tjd, year, month, day, hour)
            End If
        End Sub
        ''' <summary>
        ''' Compute equatorial spherical coordinates of Sun referred to the mean equator and equinox of date.
        ''' </summary>
        ''' <param name="jd">Julian date on TDT or ET time scale.</param>
        ''' <param name="ra">OUT: Right ascension referred to mean equator and equinox of date (hours).</param>
        ''' <param name="dec">OUT: Declination referred to mean equator and equinox of date  (degrees).</param>
        ''' <param name="dis">OUT: Geocentric distance (AU).</param>
        ''' <remarks></remarks>
        Public Shared Sub SunEph(ByVal jd As Double, _
                                               ByRef ra As Double, _
                                               ByRef dec As Double, _
                                               ByRef dis As Double)
            If Is64Bit() Then
                sun_eph64(jd, ra, dec, dis)
            Else
                sun_eph32(jd, ra, dec, dis)
            End If
        End Sub
        'Public Shared Function solarsystem(ByVal tjd As Double, _
        '                                      ByVal body As Body, _
        '                                      ByRef origin As Origin, _
        '                                      ByRef pos As Double(), _
        '                                      ByRef vel As Double()) As Short
        'Dim vpos As New posvector, vvel As New velvector
        'Dim rc As Short
        '    If Is64Bit() Then
        '        rc = solarsystem64(tjd, body, origin, vpos, vvel)
        '    Else
        '        rc = solarsystem32(tjd, body, origin, vpos, vvel)
        '    End If
        '    PosVecToArr(vpos, pos)
        '    VelVecToArr(vvel, vel)
        '    Return rc
        'End Function
#End Region

#Region "DLL Entry Points (32bit)"
        <DllImport(NOVAS32Dll, EntryPoint:="app_star")> Private Shared Function app_star32(ByVal tjd As Double, _
                                                                                           ByRef earth As BodyDescriptionShort, _
                                                                                           ByRef star As CatEntryNOVAS2, _
                                                                                           ByRef ra As Double, _
                                                                                           ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="topo_star")> Private Shared Function topo_star32(ByVal tjd As Double, _
                                                                                             ByRef earth As BodyDescriptionShort, _
                                                                                             ByVal deltat As Double, _
                                                                                             ByRef star As CatEntryNOVAS2, _
                                                                                             ByRef location As SiteInfo, _
                                                                                             ByRef ra As Double, _
                                                                                             ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="app_planet")> Private Shared Function app_planet32(ByVal tjd As Double, _
                                                                                               ByRef ss_object As BodyDescriptionShort, _
                                                                                               ByRef earth As BodyDescriptionShort, _
                                                                                               ByRef ra As Double, _
                                                                                               ByRef dec As Double, _
                                                                                               ByRef dis As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="topo_planet")> Private Shared Function topo_planet32(ByVal tjd As Double, _
                                                                                                 ByRef ss_object As BodyDescriptionShort, _
                                                                                                 ByRef earth As BodyDescriptionShort, _
                                                                                                 ByVal deltat As Double, _
                                                                                                 ByRef location As SiteInfo, _
                                                                                                 ByRef ra As Double, _
                                                                                                 ByRef dec As Double, _
                                                                                                 ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="virtual_star")> Private Shared Function virtual_star32(ByVal tjd As Double, _
                                                                                                   ByRef earth As BodyDescriptionShort, _
                                                                                                   ByRef star As CatEntryNOVAS2, _
                                                                                                   ByRef ra As Double, _
                                                                                                   ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="local_star")> Private Shared Function local_star32(ByVal tjd As Double, _
                                                                                               ByRef earth As BodyDescriptionShort, _
                                                                                               ByVal deltat As Double, _
                                                                                               ByRef star As CatEntryNOVAS2, _
                                                                                               ByRef location As SiteInfo, _
                                                                                               ByRef ra As Double, _
                                                                                               ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="virtual_planet")> Private Shared Function virtual_planet32(ByVal tjd As Double, _
                                                                                                       ByRef ss_object As BodyDescriptionShort, _
                                                                                                       ByRef earth As BodyDescriptionShort, _
                                                                                                       ByRef ra As Double, _
                                                                                                       ByRef dec As Double, _
                                                                                                       ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="local_planet")> Private Shared Function local_planet32(ByVal tjd As Double, _
                                                                                                   ByRef ss_object As BodyDescriptionShort, _
                                                                                                   ByRef earth As BodyDescriptionShort, _
                                                                                                   ByVal deltat As Double, _
                                                                                                   ByRef location As SiteInfo, _
                                                                                                   ByRef ra As Double, _
                                                                                                   ByRef dec As Double, _
                                                                                                   ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="astro_star")> Private Shared Function astro_star32(ByVal tjd As Double, _
                                                                                               ByRef earth As BodyDescriptionShort, _
                                                                                               ByRef star As CatEntryNOVAS2, _
                                                                                               ByRef ra As Double, _
                                                                                               ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="astro_planet")> Private Shared Function astro_planet32(ByVal tjd As Double, _
                                                                                                   ByRef ss_object As BodyDescriptionShort, _
                                                                                                   ByRef earth As BodyDescriptionShort, _
                                                                                                   ByRef ra As Double, _
                                                                                                   ByRef dec As Double, _
                                                                                                   ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="equ2hor")> Private Shared Sub equ2hor32(ByVal tjd As Double, _
                                                                                    ByVal deltat As Double, _
                                                                                    ByVal x As Double, _
                                                                                    ByVal y As Double, _
                                                                                    ByRef location As SiteInfo, _
                                                                                    ByVal ra As Double, _
                                                                                    ByVal dec As Double, _
                                                                                    ByVal ref_option As Short, _
                                                                                    ByRef zd As Double, _
                                                                                    ByRef az As Double, _
                                                                                    ByRef rar As Double, _
                                                                                    ByRef decr As Double)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="transform_hip")> Private Shared Sub transform_hip32(ByRef hipparcos As CatEntryNOVAS2, _
                                                                                                ByRef fk5 As CatEntryNOVAS2)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="transform_cat")> Private Shared Sub transform_cat32(ByVal [option] As Short, _
                                                                                                ByVal date_incat As Double, _
                                                                                                ByRef incat As CatEntryNOVAS2, _
                                                                                                ByVal date_newcat As Double, _
                                                                                                ByRef newcat_id As Byte(), _
                                                                                                ByRef newcat As CatEntryNOVAS2)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="sidereal_time")> Private Shared Sub sidereal_time32(ByVal jd_high As Double, _
                                                                                                ByVal jd_low As Double, _
                                                                                                ByVal ee As Double, _
                                                                                                ByRef gst As Double)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="precession")> Private Shared Sub precession32(ByVal tjd1 As Double, _
                                                                                          ByRef pos As PosVector, _
                                                                                          ByVal tjd2 As Double, _
                                                                                          ByRef pos2 As PosVector)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="earthtilt")> Private Shared Sub earthtilt32(ByVal tjd As Double, _
                                                                                        ByRef mobl As Double, _
                                                                                        ByRef tobl As Double, _
                                                                                        ByRef eq As Double, _
                                                                                        ByRef dpsi As Double, _
                                                                                        ByRef deps As Double)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="cel_pole")> Private Shared Sub cel_pole32(ByVal del_dpsi As Double, _
                                                                                      ByVal del_deps As Double)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="ephemeris")> _
        Private Shared Function ephemeris32(ByVal tjd As Double, _
                                       ByRef cel_obj As BodyDescriptionShort, _
                                       ByVal origin As Short, _
                                       ByRef pos As PosVector, _
                                       ByRef vel As VelVector) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="solarsystem")> _
        Private Shared Function solarsystem32(ByVal tjd As Double, _
                                              ByVal body As Short, _
                                              ByVal origin As Short, _
                                              ByRef pos As PosVector, _
                                              ByRef vel As VelVector) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="vector2radec")> _
        Private Shared Function vector2radec32(ByRef pos As PosVector, _
                                               ByRef ra As Double, _
                                               ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="starvectors")> _
        Private Shared Sub starvectors32(ByRef star As CatEntryNOVAS2, _
                                         ByRef pos As PosVector, _
                                         ByRef vel As VelVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="radec2vector")> _
        Private Shared Sub radec2vector32(ByVal ra As Double, _
                                          ByVal dec As Double, _
                                          ByVal dist As Double, _
                                          ByRef pos As PosVector)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="get_earth")> _
        Private Shared Function get_earth32(ByVal tjd As Double, _
                                            ByRef earth As BodyDescriptionShort, _
                                            ByRef tdb As Double, _
                                            ByRef bary_earthp As PosVector, _
                                            ByRef bary_earthv As VelVector, _
                                            ByRef helio_earthp As PosVector, _
                                            ByRef helio_earthv As VelVector) As Short
        End Function
        '
        'START OF NEW
        '
        <DllImport(NOVAS32Dll, EntryPoint:="mean_star")> _
        Private Shared Function mean_star32(ByVal tjd As Double, _
                                            ByRef earth As BodyDescriptionShort, _
                                            ByVal ra As Double, _
                                            ByVal dec As Double, _
                                            ByRef mra As Double, _
                                            ByRef mdec As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="pnsw")> _
        Private Shared Sub pnsw32(ByVal tjd As Double, _
                                  ByVal gast As Double, _
                                  ByVal x As Double, _
                                  ByVal y As Double, _
                                  ByRef vece As PosVector, _
                                  ByRef vecs As PosVector)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="spin")> _
        Private Shared Sub spin32(ByVal st As Double, _
                                  ByRef pos1 As PosVector, _
                                  ByRef pos2 As PosVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="wobble")> _
        Private Shared Sub wobble32(ByVal x As Double, _
                                    ByVal y As Double, _
                                    ByRef pos1 As PosVector, _
                                    ByRef pos2 As PosVector)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="terra")> _
        Private Shared Sub terra32(ByRef locale As SiteInfo, _
                                   ByVal st As Double, _
                                   ByRef pos As PosVector, _
                                   ByRef vel As VelVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="proper_motion")> _
        Private Shared Sub proper_motion32(ByVal tjd1 As Double, _
                                           ByRef pos As PosVector, _
                                           ByRef vel As VelVector, _
                                           ByVal tjd2 As Double, _
                                           ByRef pos2 As PosVector)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="bary_to_geo")> _
        Private Shared Sub bary_to_geo32(ByRef pos As PosVector, _
                                         ByRef earthvector As PosVector, _
                                         ByRef pos2 As PosVector, _
                                         ByRef lighttime As Double)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="sun_field")> _
        Private Shared Function sun_field32(ByRef pos As PosVector, _
                                            ByRef earthvector As PosVector, _
                                            ByRef pos2 As PosVector) As Short
        End Function


        <DllImport(NOVAS32Dll, EntryPoint:="aberration")> _
        Private Shared Function aberration32(ByRef pos As PosVector, _
                                             ByRef vel As VelVector, _
                                             ByVal lighttime As Double, _
                                             ByRef pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="nutate")> _
        Private Shared Function nutate32(ByVal tjd As Double, _
                                         ByVal fn As Short, _
                                         ByRef pos As PosVector, _
                                         ByRef pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="nutation_angles")> _
        Private Shared Function nutation_angles32(ByVal tdbtime As Double, _
                                                  ByRef longnutation As Double, _
                                                  ByRef obliqnutation As Double) As Short
        End Function


        <DllImport(NOVAS32Dll, EntryPoint:="fund_args")> _
        Private Shared Sub fund_args32(ByVal t As Double, _
                                       ByRef a As FundamentalArgs)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="tdb2tdt")> _
        Private Shared Sub tdb2tdt32(ByVal tdb As Double, _
                                     ByRef tdtjd As Double, _
                                     ByRef secdiff As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="set_body")> _
        Private Shared Function set_body32(ByVal type As Short, _
                                          ByVal number As Short, _
                                          <MarshalAs(UnmanagedType.LPStr)> _
                                          ByVal name As String, _
                                          ByRef cel_obj As BodyDescriptionShort) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="readeph")> _
        Private Shared Function readeph32(ByVal mp As Integer, _
                                       ByVal name As System.IntPtr, _
                                       ByVal jd As Double, _
                                       ByRef err As Integer) As PosVector
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="make_cat_entry")> _
        Private Shared Sub make_cat_entry32(<MarshalAs(UnmanagedType.LPStr)> _
                                         ByVal catalog As String, _
                                         <MarshalAs(UnmanagedType.LPStr)> _
                                         ByVal star_name As String, _
                                         ByVal star_num As Integer, _
                                         ByVal ra As Double, _
                                         ByVal dec As Double, _
                                         ByVal pm_ra As Double, _
                                         ByVal pm_dec As Double, _
                                         ByVal parallax As Double, _
                                         ByVal rad_vel As Double, _
                                         ByRef star As CatEntryNOVAS2)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="refract")> _
        Private Shared Function refract32(ByRef location As SiteInfo, _
                                          ByVal ref_option As Short, _
                                          ByVal zd_obs As Double) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="julian_date")> _
        Private Shared Function julian_date32(ByVal year As Short, _
                                              ByVal month As Short, _
                                              ByVal day As Short, _
                                              ByVal hour As Double) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="cal_date")> _
        Private Shared Sub cal_date32(ByVal tjd As Double, _
                                      ByRef year As Short, _
                                      ByRef month As Short, _
                                      ByRef day As Short, _
                                      ByRef hour As Double)
        End Sub
        <DllImport(NOVAS32Dll, EntryPoint:="sun_eph")> _
        Private Shared Sub sun_eph32(ByVal jd As Double, _
                                          ByRef ra As Double, _
                                          ByRef dec As Double, _
                                          ByRef dis As Double)
        End Sub

        '  <DllImport(NOVAS32Dll, EntryPoint:="solarsystem")> _
        '   Private Shared Function solarsystem32(ByVal tjd As Double, _
        '                                        ByVal body As Body, _
        '                                        ByRef origin As Integer, _
        '                                        ByRef pos As posvector, _
        '                                        ByRef vel As velvector) As Short
        '  End Function
#End Region

#Region "DLL Entry Points (64bit)"
        <DllImport(NOVAS64Dll, EntryPoint:="app_star")> Private Shared Function app_star64(ByVal tjd As Double, _
                                                                                           ByRef earth As BodyDescriptionShort, _
                                                                                           ByRef star As CatEntryNOVAS2, _
                                                                                           ByRef ra As Double, _
                                                                                           ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="topo_star")> Private Shared Function topo_star64(ByVal tjd As Double, _
                                                                                             ByRef earth As BodyDescriptionShort, _
                                                                                             ByVal deltat As Double, _
                                                                                             ByRef star As CatEntryNOVAS2, _
                                                                                             ByRef location As SiteInfo, _
                                                                                             ByRef ra As Double, _
                                                                                             ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="app_planet")> Private Shared Function app_planet64(ByVal tjd As Double, _
                                                                                               ByRef ss_object As BodyDescriptionShort, _
                                                                                               ByRef earth As BodyDescriptionShort, _
                                                                                               ByRef ra As Double, _
                                                                                               ByRef dec As Double, _
                                                                                               ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="topo_planet")> Private Shared Function topo_planet64(ByVal tjd As Double, _
                                                                                                 ByRef ss_object As BodyDescriptionShort, _
                                                                                                 ByRef earth As BodyDescriptionShort, _
                                                                                                 ByVal deltat As Double, _
                                                                                                 ByRef location As SiteInfo, _
                                                                                                 ByRef ra As Double, _
                                                                                                 ByRef dec As Double, _
                                                                                                 ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="virtual_star")> Private Shared Function virtual_star64(ByVal tjd As Double, _
                                                                                                   ByRef earth As BodyDescriptionShort, _
                                                                                                   ByRef star As CatEntryNOVAS2, _
                                                                                                   ByRef ra As Double, _
                                                                                                   ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="local_star")> Private Shared Function local_star64(ByVal tjd As Double, _
                                                                                               ByRef earth As BodyDescriptionShort, _
                                                                                               ByVal deltat As Double, _
                                                                                               ByRef star As CatEntryNOVAS2, _
                                                                                               ByRef location As SiteInfo, _
                                                                                               ByRef ra As Double, _
                                                                                               ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="virtual_planet")> Private Shared Function virtual_planet64(ByVal tjd As Double, _
                                                                                                       ByRef ss_object As BodyDescriptionShort, _
                                                                                                       ByRef earth As BodyDescriptionShort, _
                                                                                                       ByRef ra As Double, _
                                                                                                       ByRef dec As Double, _
                                                                                                       ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="local_planet")> Private Shared Function local_planet64(ByVal tjd As Double, _
                                                                                                   ByRef ss_object As BodyDescriptionShort, _
                                                                                                   ByRef earth As BodyDescriptionShort, _
                                                                                                   ByVal deltat As Double, _
                                                                                                   ByRef location As SiteInfo, _
                                                                                                   ByRef ra As Double, _
                                                                                                   ByRef dec As Double, _
                                                                                                   ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="astro_star")> Private Shared Function astro_star64(ByVal tjd As Double, _
                                                                                               ByRef earth As BodyDescriptionShort, _
                                                                                               ByRef star As CatEntryNOVAS2, _
                                                                                               ByRef ra As Double, _
                                                                                               ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="astro_planet")> Private Shared Function astro_planet64(ByVal tjd As Double, _
                                                                                                   ByRef ss_object As BodyDescriptionShort, _
                                                                                                   ByRef earth As BodyDescriptionShort, _
                                                                                                   ByRef ra As Double, _
                                                                                                   ByRef dec As Double, _
                                                                                                   ByRef dis As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="equ2hor")> Private Shared Sub equ2hor64(ByVal tjd As Double, _
                                                                                    ByVal deltat As Double, _
                                                                                    ByVal x As Double, _
                                                                                    ByVal y As Double, _
                                                                                    ByRef location As SiteInfo, _
                                                                                    ByVal ra As Double, _
                                                                                    ByVal dec As Double, _
                                                                                    ByVal ref_option As Short, _
                                                                                    ByRef zd As Double, _
                                                                                    ByRef az As Double, _
                                                                                    ByRef rar As Double, _
                                                                                    ByRef decr As Double)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="transform_hip")> Private Shared Sub transform_hip64(ByRef hipparcos As CatEntryNOVAS2, _
                                                                                                ByRef fk5 As CatEntryNOVAS2)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="transform_cat")> Private Shared Sub transform_cat64(ByVal [option] As Short, _
                                                                                                ByVal date_incat As Double, _
                                                                                                ByRef incat As CatEntryNOVAS2, _
                                                                                                ByVal date_newcat As Double, _
                                                                                                ByRef newcat_id As Byte(), _
                                                                                                ByRef newcat As CatEntryNOVAS2)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="sidereal_time")> Private Shared Sub sidereal_time64(ByVal jd_high As Double, _
                                                                                                ByVal jd_low As Double, _
                                                                                                ByVal ee As Double, _
                                                                                                ByRef gst As Double)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="precession")> Private Shared Sub precession64(ByVal tjd1 As Double, _
                                                                                          ByRef pos As PosVector, _
                                                                                          ByVal tjd2 As Double, _
                                                                                          ByRef pos2 As PosVector)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="earthtilt")> Private Shared Sub earthtilt64(ByVal tjd As Double, _
                                                                                        ByRef mobl As Double, _
                                                                                        ByRef tobl As Double, _
                                                                                        ByRef eq As Double, _
                                                                                        ByRef dpsi As Double, _
                                                                                        ByRef deps As Double)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="cel_pole")> Private Shared Sub cel_pole64(ByVal del_dpsi As Double, _
                                                                                    ByVal del_deps As Double)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="ephemeris")> _
        Private Shared Function ephemeris64(ByVal tjd As Double, _
                                       ByRef cel_obj As BodyDescriptionShort, _
                                       ByVal origin As Short, _
                                       ByRef pos As PosVector, _
                                       ByRef vel As VelVector) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="solarsystem")> _
        Private Shared Function solarsystem64(ByVal tjd As Double, _
                                              ByVal body As Short, _
                                              ByVal origin As Short, _
                                              ByRef pos As PosVector, _
                                              ByRef vel As VelVector) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="vector2radec")> _
        Private Shared Function vector2radec64(ByRef pos As PosVector, _
                                               ByRef ra As Double, _
                                               ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="starvectors")> _
        Private Shared Sub starvectors64(ByRef star As CatEntryNOVAS2, _
                                         ByRef pos As PosVector, _
                                         ByRef vel As VelVector)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="radec2vector")> _
        Private Shared Sub radec2vector64(ByVal ra As Double, _
                                          ByVal dec As Double, _
                                          ByVal dist As Double, _
                                          ByRef pos As PosVector)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="get_earth")> _
        Private Shared Function get_earth64(ByVal tjd As Double, _
                                        ByRef earth As BodyDescriptionShort, _
                                        ByRef tdb As Double, _
                                        ByRef bary_earthp As PosVector, _
                                        ByRef bary_earthv As VelVector, _
                                        ByRef helio_earthp As PosVector, _
                                        ByRef helio_earthv As VelVector) As Short
        End Function
        '
        'START OF NEW
        '
        <DllImport(NOVAS64Dll, EntryPoint:="mean_star")> _
        Private Shared Function mean_star64(ByVal tjd As Double, _
                                            ByRef earth As BodyDescriptionShort, _
                                            ByVal ra As Double, _
                                            ByVal dec As Double, _
                                            ByRef mra As Double, _
                                            ByRef mdec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="pnsw")> _
        Private Shared Sub pnsw64(ByVal tjd As Double, _
                                  ByVal gast As Double, _
                                  ByVal x As Double, _
                                  ByVal y As Double, _
                                  ByRef vece As PosVector, _
                                  ByRef vecs As PosVector)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="spin")> _
        Private Shared Sub spin64(ByVal st As Double, _
                                  ByRef pos1 As PosVector, _
                                  ByRef pos2 As PosVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="wobble")> _
        Private Shared Sub wobble64(ByVal x As Double, _
                                    ByVal y As Double, _
                                    ByRef pos1 As PosVector, _
                                    ByRef pos2 As PosVector)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="terra")> _
        Private Shared Sub terra64(ByRef locale As SiteInfo, _
                                   ByVal st As Double, _
                                   ByRef pos As PosVector, _
                                   ByRef vel As VelVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="proper_motion")> _
        Private Shared Sub proper_motion64(ByVal tjd1 As Double, _
                                           ByRef pos As PosVector, _
                                           ByRef vel As VelVector, _
                                           ByVal tjd2 As Double, _
                                           ByRef pos2 As PosVector)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="bary_to_geo")> _
        Private Shared Sub bary_to_geo64(ByRef pos As PosVector, _
                                         ByRef earthvector As PosVector, _
                                         ByRef pos2 As PosVector, _
                                         ByRef lighttime As Double)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="sun_field")> _
        Private Shared Function sun_field64(ByRef pos As PosVector, _
                                            ByRef earthvector As PosVector, _
                                            ByRef pos2 As PosVector) As Short
        End Function


        <DllImport(NOVAS64Dll, EntryPoint:="aberration")> _
        Private Shared Function aberration64(ByRef pos As PosVector, _
                                             ByRef vel As VelVector, _
                                             ByVal lighttime As Double, _
                                             ByRef pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="nutate")> _
        Private Shared Function nutate64(ByVal tjd As Double, _
                                         ByVal fn As Short, _
                                         ByRef pos As PosVector, _
                                         ByRef pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="nutation_angles")> _
        Private Shared Function nutation_angles64(ByVal tdbtime As Double, _
                                                  ByRef longnutation As Double, _
                                                  ByRef obliqnutation As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="fund_args")> _
        Private Shared Sub fund_args64(ByVal t As Double, _
                                       ByRef a As FundamentalArgs)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="tdb2tdt")> _
        Private Shared Sub tdb2tdt64(ByVal tdb As Double, _
                                     ByRef tdtjd As Double, _
                                     ByRef secdiff As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="set_body")> _
        Private Shared Function set_body64(ByVal type As Short, _
                                          ByVal number As Short, _
                                          <MarshalAs(UnmanagedType.LPStr)> _
                                          ByVal name As String, _
                                          ByRef cel_obj As BodyDescriptionShort) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="readeph")> _
        Private Shared Function readeph64(ByVal mp As Integer, _
                                       ByVal name As System.IntPtr, _
                                       ByVal jd As Double, _
                                       ByRef err As Integer) As PosVector
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="make_cat_entry")> _
        Private Shared Sub make_cat_entry64(<MarshalAs(UnmanagedType.LPStr)> _
                                         ByVal catalog As String, _
                                         <MarshalAs(UnmanagedType.LPStr)> _
                                         ByVal star_name As String, _
                                         ByVal star_num As Integer, _
                                         ByVal ra As Double, _
                                         ByVal dec As Double, _
                                         ByVal pm_ra As Double, _
                                         ByVal pm_dec As Double, _
                                         ByVal parallax As Double, _
                                         ByVal rad_vel As Double, _
                                         ByRef star As CatEntryNOVAS2)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="refract")> _
        Private Shared Function refract64(ByRef location As SiteInfo, _
                                          ByVal ref_option As Short, _
                                          ByVal zd_obs As Double) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="julian_date")> _
        Private Shared Function julian_date64(ByVal year As Short, _
                                              ByVal month As Short, _
                                              ByVal day As Short, _
                                              ByVal hour As Double) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="cal_date")> _
        Private Shared Sub cal_date64(ByVal tjd As Double, _
                                    ByRef year As Short, _
                                    ByRef month As Short, _
                                    ByRef day As Short, _
                                    ByRef hour As Double)
        End Sub
        <DllImport(NOVAS64Dll, EntryPoint:="sun_eph")> _
        Private Shared Sub sun_eph64(ByVal jd As Double, _
                                          ByRef ra As Double, _
                                          ByRef dec As Double, _
                                          ByRef dis As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="solarsystem")> _
        Private Shared Function solarsystem64(ByVal tjd As Double, _
                                              ByVal body As Short, _
                                              ByRef origin As Integer, _
                                              ByRef pos As PosVector, _
                                              ByRef vel As VelVector) As Short
        End Function
#End Region

#Region "Support Code"

        'Declare the api call that sets the additional DLL search directory
        <DllImport("kernel32.dll", SetLastError:=False)> _
        Private Shared Function SetDllDirectory(ByVal lpPathName As String) As Boolean
        End Function

        Private Shared Function Is64Bit() As Boolean
            Dim rc As Boolean
            Static StatPath As String

            'StatPath is a static variable because GetFolderPath takes about 10 times longer to execute than
            'the supervisory NOVAS routines. If we called it every time the overhead would be enormous.
            'The static variable and If statement ensure that the call is only made once

            If StatPath = "" Then
                StatPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & NOVAS_DLL_LOCATION
                'Add the ASCOM\.net directory to the DLL search path so that the NOVAS C 32 and 64bit DLLs can be found
                rc = SetDllDirectory(StatPath)
            End If

            If IntPtr.Size = 8 Then 'Check whether we are running on a 32 or 64bit system.
                Return True
            Else
                Return False
            End If
        End Function

        Private Shared Function ArrToPosVec(ByVal Arr As Double()) As PosVector
            'Create a new vector having the values in the supplied double array
            Dim V As New PosVector
            V.x = Arr(0)
            V.y = Arr(1)
            V.z = Arr(2)
            Return V
        End Function

        Private Shared Sub PosVecToArr(ByVal V As PosVector, ByRef Ar As Double())
            'Copy a vector structure to a returned double array
            Ar(0) = V.x
            Ar(1) = V.y
            Ar(2) = V.z
        End Sub

        Private Shared Function ArrToVelVec(ByVal Arr As Double()) As VelVector
            'Create a new vector having the values in the supplied double array
            Dim V As New VelVector
            V.x = Arr(0)
            V.y = Arr(1)
            V.z = Arr(2)
            Return V
        End Function

        Private Shared Sub VelVecToArr(ByVal V As VelVector, ByRef Ar As Double())
            'Copy a vector structure to a returned double array
            Ar(0) = V.x
            Ar(1) = V.y
            Ar(2) = V.z
        End Sub

        Private Shared Function BodyDescToShort(ByVal BD As BodyDescription) As BodyDescriptionShort
            'Create a version of the body description that uses shorts instead of integers  for number and type
            'This ensures that the C functions run OK (using shorts) while the public face is integer.
            Dim BDS As New BodyDescriptionShort
            BDS.Name = BD.Name
            BDS.Number = CShort(BD.Number)
            BDS.Type = CShort(BD.Type)
            Return BDS
        End Function

        Private Shared Function CatEntryToCatEntryNOVAS2(ByVal CE As CatEntry) As CatEntryNOVAS2
            'Create a version of the body description that uses shorts instead of integers  for number and type
            'This ensures that the C functions run OK (using shorts) while the public face is integer.
            Dim CEN2 As New CatEntryNOVAS2
            CEN2.Catalog = CE.Catalog
            CEN2.Dec = CE.Dec
            CEN2.Parallax = CE.Parallax
            CEN2.ProMoDec = CE.ProMoDec
            CEN2.ProMoRA = CE.ProMoRA
            CEN2.RA = CE.RA
            CEN2.RadialVelocity = CE.RadialVelocity
            CEN2.StarName = CE.StarName
            CEN2.StarNumber = CE.StarNumber
            Return CEN2
        End Function

        Private Shared Sub CatEntryNOVAS2ToCatEntry(ByVal CEN2 As CatEntryNOVAS2, ByRef CE As CatEntry)
            'Transfer to the external public structure that has differnt marshalling characteristics specifically for COM
            CE.Catalog = CEN2.Catalog
            CE.Dec = CEN2.Dec
            CE.Parallax = CEN2.Parallax
            CE.ProMoDec = CEN2.ProMoDec
            CE.ProMoRA = CEN2.ProMoRA
            CE.RA = CEN2.RA
            CE.RadialVelocity = CEN2.RadialVelocity
            CE.StarName = CEN2.StarName
            CE.StarNumber = CEN2.StarNumber
        End Sub
#End Region

    End Class
End Namespace