using System;
using static System.Math;
using ASCOM.Astrometry.Kepler;
using static ASCOM.Astrometry.NOVAS.NOVAS2;

namespace ASCOM.Astrometry
{

    static class EphemerisCode
    {

        // Function patterned after get_earth() in the original NOVAS-C V2 package.
        // This function returns (via ref-params) the barycentric TDT and both
        // heliocentric and barycentric position and velocity of the Earth, at
        // the given TJD. You can pass an IDispatch pointer for an ephemeris
        // component, and it will be used. If that is NULL the internal solsys3()
        // function is used (see solarsystem().
        // 
        // For more info, see the original NOVAS-C sources.
        internal static void get_earth_nov(ref IEphemeris pEphDisp, double tjd, ref double tdb, ref double[] peb, ref double[] veb, ref double[] pes, ref double[] ves)
        {
            short i, rc;
            double dummy = default, secdiff = default;
            double tjd_last = 0.0d;
            double ltdb;
            double[] lpeb = new double[4], lveb = new double[4], lpes = new double[4], lves = new double[4];
            // Dim TL As New TraceLogger("", "get_earth_nov")
            // TL.Enabled = True
            // TL.LogMessage("get_earth_nov", "Start")
            //
            // Compute the TDB Julian date corresponding to 'tjd'.
            //

            // If (Abs(tjd - tjd_last) > 0.000001) Then 'Optimize repeated calls
            Tdb2Tdt(tjd, ref dummy, ref secdiff);
            // TL.LogMessage("get_earth_nov", "after tbd2tdt")
            ltdb = tjd + secdiff / 86400.0d;

            //
            // Get position and velocity of the Earth wrt barycenter of 
            // solar system and wrt center of the sun. These calls reflect
            // exceptions thrown by the attached ephemeris generator, so
            // we just return the hr ... the ErrorInfo is already set!
            //
            try
            {
                // TL.LogMessage("get_earth_nov", "before solsysnov barycentric")
                rc = solarsystem_nov(ref pEphDisp, tjd, ltdb, Body.Earth, Origin.Barycentric, ref lpeb, ref lveb);
                // TL.LogMessage("get_earth_nov", "after solsysnov barycentric")
                if (rc != 0)
                    throw new Exceptions.NOVASFunctionException("EphemerisCode:get_earth_nov Earth eph exception", "solarsystem_nov", rc);
            }
            catch (Exception ex)
            {
                tjd_last = 0.0d;
                throw;
            }

            try
            {
                // TL.LogMessage("get_earth_nov", "before solsysnov heliocentric")
                rc = solarsystem_nov(ref pEphDisp, tjd, ltdb, Body.Earth, Origin.Heliocentric, ref lpes, ref lves);
                // TL.LogMessage("get_earth_nov", "after solsysnov heliocentric")
                if (rc != 0)
                    throw new Exceptions.NOVASFunctionException("EphemerisCode:get_earth_nov Earth eph exception", "solarsystem_nov", rc);
            }
            catch (Exception ex)
            {
                tjd_last = 0.0d;
                throw;
            }

            tjd_last = tjd;
            // End If
            tdb = ltdb;
            for (i = 0; i <= 2; i++)
            {
                peb[i] = lpeb[i];
                veb[i] = lveb[i];
                pes[i] = lpes[i];
                ves[i] = lves[i];
            }

            // TL.Enabled = False
            // TL.Dispose()
            // TL = Nothing

        }

        //
        // Ephemeris() - Wrapper for external ephemeris generator
        //
        // The ephemeris generator must support a single method:
        //
        //     result(6) = GetPositionAndVelocity(tjd, Type, Number, Name)
        //
        //	tjd		Terrestrial Julian Date
        //	Type	Type of body: 0 = major planet, Sun, or Moon
        //						  1 = minor planet
        //	Number: For Type = 0: Mercury = 1, ..., Pluto = 9
        //			For Type = 1: minor planet number or 0 for unnumbered MP
        //  Name:   For Type = 0: n/a
        //			For Type = 1: n/a for numbered MPs. For unnumbered MPs, this
        //						  is the MPC PACKED designation.
        //  result	A SAFEARRAY of VARIANT, each element VT_R8 (double). Elements
        //			0-2 are the position vector of the body, elements 3.5 are the
        //			velocity vector of the body. 
        //
        internal static void ephemeris_nov(ref IEphemeris ephDisp, double tjd, BodyType btype, int num, string name, Origin origin, ref double[] pos, ref double[] vel)
        {
            int i;
            double[] posvel = new double[7], p = new double[3], v = new double[3];
            // Dim bdy As bodystruct
            // Dim org As NOVAS2Net.Origin
            // Dim rc As Short
            // Dim TL As New TraceLogger("", "EphNov")
            // TL.Enabled = True
            // TL.LogMessage("EphNov", "Start")
            //
            // Check inputs
            //
            if (ephDisp is null)
            {
                throw new Exceptions.ValueNotSetException("Ephemeris_nov Ephemeris object not set");
            }
            else
            {
                if (origin != Origin.Barycentric & origin != Origin.Heliocentric)
                    throw new Utilities.Exceptions.InvalidValueException("Ephemeris_nov Origin is neither barycentric or heliocentric");

                //
                // Call the ephemeris for the heliocentric J2000.0 equatorial coordinates
                var kbtype = default(BodyType);
                // TL.LogMessage("EphNov", "Before Case Btype")
                switch (btype)
                {
                    case BodyType.Comet:
                        kbtype = BodyType.Comet;
                        break;
                    case BodyType.MajorPlanet:
                        kbtype = BodyType.MajorPlanet;
                        break;
                    case BodyType.MinorPlanet:
                        kbtype = BodyType.MinorPlanet;
                        break;
                }

                var knum = default(Body);
                switch (num)
                {
                    case 1:
                        knum = Body.Mercury;
                        break;
                    case 2:
                        knum = Body.Venus;
                        break;
                    case 3:
                        knum = Body.Earth;
                        break;
                    case 4:
                        knum = Body.Mars;
                        break;
                    case 5:
                        knum = Body.Jupiter;
                        break;
                    case 6:
                        knum = Body.Saturn;
                        break;
                    case 7:
                        knum = Body.Uranus;
                        break;
                    case 8:
                        knum = Body.Neptune;
                        break;
                    case 9:
                        knum = Body.Pluto;
                        break;
                }
                ephDisp.BodyType = kbtype;
                ephDisp.Number = knum;
                if (!string.IsNullOrEmpty(name))
                    ephDisp.Name = name;
                // TL.LogMessage("EphNov", "Before ephDisp GetPosAndVel")
                posvel = ephDisp.GetPositionAndVelocity(tjd);
                // TL.LogMessage("EphNov", "After ephDisp GetPosAndVel")
            }

            if (origin == Origin.Barycentric)
            {

                double[] sun_pos = new double[4], sun_vel = new double[4];

                // CHICKEN AND EGG ALERT!!! WE CANNOT CALL OURSELVES FOR 
                // BARYCENTER CALCULATION -- AS AN APPROXIMATION, WE USE
                // OUR INTERNAL SOLSYS3() FUNCTION TO GET THE BARYCENTRIC
                // SUN. THIS SHOULD BE "GOOD ENOUGH". IF WE EVER GET 
                // AN EPHEMERIS GEN THAT HANDLES BARYCENTRIC, WE CAN 
                // CAN THIS...
                // TL.LogMessage("EphNov", "Before solsys3")
                solsys3_nov(tjd, Body.Sun, Origin.Barycentric, ref sun_pos, ref sun_vel);
                // TL.LogMessage("EphNov", "After solsys3")
                for (i = 0; i <= 2; i++)
                {
                    posvel[i] += sun_pos[i];
                    posvel[i + 3] += sun_vel[i];
                }
            }

            for (i = 0; i <= 2; i++)
            {
                pos[i] = posvel[i];
                vel[i] = posvel[i + 3];
            }
            // TL.Enabled = False
            // TL.Dispose()
        }

        // ===============
        // LOCAL FUNCTIONS
        // ===============


        //
        // This is the function used to get the position and velocity vectors
        // for the major solar system bodies and the moon. It is patterned after
        // the solarsystem() function in the original NOVAS-C package. You can
        // pass an IDispatch pointer for an ephemeris component, and it will be 
        // used. If that is NULL the internal solsys3() function is used.
        //
        // This function must set error info... it is designed to work with 
        // reflected exceptions from the attached ephemeris
        // 
        internal static short solarsystem_nov(ref IEphemeris ephDisp, double tjd, double tdb, Body planet, Origin origin, ref double[] pos, ref double[] vel)
        {
            // Dim pl As NOVAS2.Body, org As NOVAS2.Origin
            // Dim TL As New TraceLogger("", "solarsystem_nov")
            var rc = default(short);
            // TL.Enabled = True
            // TL.LogMessage("solarsystem_nov", "Start")
            //
            // solsys3 takes tdb, ephemeris takes tjd
            //
            // Select Case origin
            // Case OriginType.nvBarycentric
            // org = NOVAS2.Origin.SolarSystemBarycentre
            // Case OriginType.nvHeliocentric
            // org = NOVAS2.Origin.CentreOfMassOfSun
            // End Select
            // Select Case planet
            // Case PlanetNumber.nvEarth
            // pl = NOVAS2.Body.Earth
            // Case PlanetNumber.nvJupiter
            // pl = NOVAS2.Body.Jupiter
            // Case PlanetNumber.nvMars
            // pl = NOVAS2.Body.Mars
            // Case PlanetNumber.nvMercury
            // pl = NOVAS2.Body.Mercury
            // Case PlanetNumber.nvMoon
            // pl = NOVAS2.Body.Moon
            // Case PlanetNumber.nvNeptune
            // pl = NOVAS2.Body.Neptune
            // Case PlanetNumber.nvPluto
            // pl = NOVAS2.Body.Pluto
            // Case PlanetNumber.nvSaturn
            // pl = NOVAS2.Body.Saturn
            // Case PlanetNumber.nvSun
            // pl = NOVAS2.Body.Sun
            // Case PlanetNumber.nvUranus
            // pl = NOVAS2.Body.Uranus
            // Case PlanetNumber.nvVenus
            // pl = NOVAS2.Body.Venus
            // End Select
            // TL.LogMessage("solarsystem_nov", "After planet")
            if (ephDisp is null) // No ephemeris attached
            {
                // rc = solsys3_nov(tdb, planet, origin, pos, vel)
                throw new Exceptions.ValueNotSetException("EphemerisCode:SolarSystem_Nov No emphemeris object supplied");
            }
            else
            {
                // CHECK TDB BELOW IS CORRECT!
                // TL.LogMessage("solarsystem_nov", "Before ephemeris_nov")
                ephemeris_nov(ref ephDisp, tdb, BodyType.MajorPlanet, (int)planet, "", origin, ref pos, ref vel);
                // TL.LogMessage("solarsystem_nov", "After ephemeris_nov")
            }
            // TL.Enabled = False
            // TL.Dispose()
            // TL = Nothing
            return rc;
        }

        //
        // solsys3() - Internal function that gives reasonable ephemerides for 
        // Sun or Earth, barycentric or heliocentric.
        //
        private static short solsys3_nov(double tjd, Body body, Origin origin, ref double[] pos, ref double[] vel)
        {

            int i;

            // /*
            // The arrays below contain data for the four largest planets.  Masses
            // are DE405 values; elements are from Explanatory Supplement, p. 316). 
            // These data are used for barycenter computations only.
            // */

            double[] pm = new double[] { 1047.349d, 3497.898d, 22903.0d, 19412.2d };
            double[] pa = new double[] { 5.203363d, 9.53707d, 19.191264d, 30.068963d };
            double[] pl = new double[] { 0.60047d, 0.871693d, 5.466933d, 5.32116d };
            double[] pn = new double[] { 0.001450138d, 0.0005841727d, 0.0002047497d, 0.0001043891d };

            // /*
            // obl' is the obliquity of ecliptic at epoch J2000.0 in degrees.
            // */

            const double obl = 23.43929111d;

            double tlast = 0.0d;
            double sine = default, cose = default, tmass = default;
            double[] pbary = new double[4], vbary = new double[4];

            double oblr, qjd, ras = default, decs = default, diss = default, dlon, sinl, cosl, x, y, z, xdot, ydot, zdot, f;
            double[] pos1 = new double[4];
            double[,] p = new double[4, 4];

            //
            // Check inputs
            //
            if (origin != Origin.Barycentric & origin != Origin.Heliocentric)
                throw new Utilities.Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid origin: " + ((int)origin).ToString());

            if (tjd < 2340000.5d | tjd > 2560000.5d)
                throw new Utilities.Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid tjd: " + tjd);


            // /*
            // Initialize constants.
            // */

            if (tlast == 0.0d)
            {
                oblr = obl * GlobalItems.TWOPI / 360.0d;
                sine = Sin(oblr);
                cose = Cos(oblr);
                tmass = 1.0d;
                for (i = 0; i <= 3; i++)
                    tmass += 1.0d / pm[i];
                tlast = 1.0d;
            }
            // /*
            // Form helicentric coordinates of the Sun or Earth, depending on
            // body'.
            // */

            if (body == 0 | (int)body == 1 | (int)body == 10)
            {
                for (i = 0; i <= 2; i++)
                {
                    pos[i] = 0.0d;
                    vel[i] = 0.0d;
                }
            }
            else if ((int)body == 2 | (int)body == 3)
            {
                for (i = 0; i <= 2; i++)
                {
                    qjd = tjd + (i - 1.0d) * 0.1d;
                    sun_eph_nov(qjd, ras, decs, diss);
                    RADec2Vector(ras, decs, diss, ref pos1);
                    Precession(qjd, pos1, GlobalItems.J2000BASE, ref pos);
                    p[i, 0] = -pos[0];
                    p[i, 1] = -pos[1];
                    p[i, 2] = -pos[2];
                }
                for (i = 0; i <= 2; i++)
                {
                    pos[i] = p[1, i];
                    vel[i] = (p[2, i] - p[0, i]) / 0.2d;
                }
            }
            else
            {
                throw new Utilities.Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid body: " + ((int)body).ToString());
            }

            // /*
            // If 'origin' = 0, move origin to solar system barycenter.
            // 
            // Solar system barycenter coordinates are computed from rough
            // approximations of the coordinates of the four largest planets.
            // */

            if (origin == Origin.Barycentric)
            {
                if (tjd != tlast)
                {
                    for (i = 0; i <= 2; i++)
                    {
                        pbary[i] = 0.0d;
                        vbary[i] = 0.0d;
                    }

                    // /*
                    // The following loop cycles once for each of the four planets.
                    // 
                    // sinl' and 'cosl' are the sine and cosine of the planet's mean
                    // longitude.
                    // */

                    for (i = 0; i <= 3; i++)
                    {
                        dlon = pl[i] + pn[i] * (tjd - GlobalItems.J2000BASE);
                        dlon = dlon % GlobalItems.TWOPI;
                        sinl = Sin(dlon);
                        cosl = Cos(dlon);

                        x = pa[i] * cosl;
                        y = pa[i] * sinl * cose;
                        z = pa[i] * sinl * sine;
                        xdot = -pa[i] * pn[i] * sinl;
                        ydot = pa[i] * pn[i] * cosl * cose;
                        zdot = pa[i] * pn[i] * cosl * sine;

                        f = 1.0d / (pm[i] * tmass);

                        pbary[0] += x * f;
                        pbary[1] += y * f;
                        pbary[2] += z * f;
                        vbary[0] += xdot * f;
                        vbary[1] += ydot * f;
                        vbary[2] += zdot * f;
                    }

                    tlast = tjd;
                }

                for (i = 0; i <= 2; i++)
                {
                    pos[i] -= pbary[i];
                    vel[i] -= vbary[i];
                }
            }
            return 0;
        }

        private struct sun_con
        {
            internal double l;
            internal double r;
            internal double alpha;
            internal double nu;
            internal sun_con(double pl, double pr, double palpha, double pnu)
            {
                l = pl;
                r = pr;
                alpha = palpha;
                nu = pnu;
            }
        }

        private static void sun_eph_nov(double jd, double ra, double dec, double dis)
        {
            int i;

            double sum_lon = 0.0d;
            double sum_r = 0.0d;
            const double factor = 0.0000001d;
            double u, arg, lon, lat, t, t2, emean, sin_lon;

            sun_con[] con = new sun_con[] { new sun_con(403406.0d, 0.0d, 4.721964d, 1.621043d), new sun_con(195207.0d, -97597.0d, 5.937458d, 62830.348067d), new sun_con(119433.0d, -59715.0d, 1.115589d, 62830.821524d), new sun_con(112392.0d, -56188.0d, 5.781616d, 62829.634302d), new sun_con(3891.0d, -1556.0d, 5.5474d, 125660.5691d), new sun_con(2819.0d, -1126.0d, 1.512d, 125660.9845d), new sun_con(1721.0d, -861.0d, 4.1897d, 62832.4766d), new sun_con(0.0d, 941.0d, 1.163d, 0.813d), new sun_con(660.0d, -264.0d, 5.415d, 125659.31d), new sun_con(350.0d, -163.0d, 4.315d, 57533.85d), new sun_con(334.0d, 0.0d, 4.553d, -33.931d), new sun_con(314.0d, 309.0d, 5.198d, 777137.715d), new sun_con(268.0d, -158.0d, 5.989d, 78604.191d), new sun_con(242.0d, 0.0d, 2.911d, 5.412d), new sun_con(234.0d, -54.0d, 1.423d, 39302.098d), new sun_con(158.0d, 0.0d, 0.061d, -34.861d), new sun_con(132.0d, -93.0d, 2.317d, 115067.698d), new sun_con(129.0d, -20.0d, 3.193d, 15774.337d), new sun_con(114.0d, 0.0d, 2.828d, 5296.67d), new sun_con(99.0d, -47.0d, 0.52d, 58849.27d), new sun_con(93.0d, 0.0d, 4.65d, 5296.11d), new sun_con(86.0d, 0.0d, 4.35d, -3980.7d), new sun_con(78.0d, -33.0d, 2.75d, 52237.69d), new sun_con(72.0d, -32.0d, 4.5d, 55076.47d), new sun_con(68.0d, 0.0d, 3.23d, 261.08d), new sun_con(64.0d, -10.0d, 1.22d, 15773.85d), new sun_con(46.0d, -16.0d, 0.14d, 188491.03d), new sun_con(38.0d, 0.0d, 3.44d, -7756.55d), new sun_con(37.0d, 0.0d, 4.37d, 264.89d), new sun_con(32.0d, -24.0d, 1.14d, 117906.27d), new sun_con(29.0d, -13.0d, 2.84d, 55075.75d), new sun_con(28.0d, 0.0d, 5.96d, -7961.39d), new sun_con(27.0d, -9.0d, 5.09d, 188489.81d), new sun_con(27.0d, 0.0d, 1.72d, 2132.19d), new sun_con(25.0d, -17.0d, 2.56d, 109771.03d), new sun_con(24.0d, -11.0d, 1.92d, 54868.56d), new sun_con(21.0d, 0.0d, 0.09d, 25443.93d), new sun_con(21.0d, 31.0d, 5.98d, -55731.43d), new sun_con(20.0d, -10.0d, 4.03d, 60697.74d), new sun_con(18.0d, 0.0d, 4.27d, 2132.79d), new sun_con(17.0d, -12.0d, 0.79d, 109771.63d), new sun_con(14.0d, 0.0d, 4.24d, -7752.82d), new sun_con(13.0d, -5.0d, 2.01d, 188491.91d), new sun_con(13.0d, 0.0d, 2.65d, 207.81d), new sun_con(13.0d, 0.0d, 4.98d, 29424.63d), new sun_con(12.0d, 0.0d, 0.93d, -7.99d), new sun_con(10.0d, 0.0d, 2.21d, 46941.14d), new sun_con(10.0d, 0.0d, 3.59d, -68.29d), new sun_con(10.0d, 0.0d, 1.5d, 21463.25d), new sun_con(10.0d, -9.0d, 2.55d, 157208.4d) };

            // /*
            // Define the time unit 'u', measured in units of 10000 Julian years
            // from J2000.0.
            // */

            u = (jd - GlobalItems.J2000BASE) / 3652500.0d;

            // /*
            // Compute longitude and distance terms from the series.
            // */

            for (i = 0; i <= 49; i++)
            {

                arg = con[i].alpha + con[i].nu * u;
                sum_lon += con[i].l * Sin(arg);
                sum_r += con[i].r * Cos(arg);
            }

            // /*
            // Compute longitude, latitude, and distance referred to mean equinox
            // and ecliptic of date.
            // */

            lon = 4.9353929d + 62833.196168d * u + factor * sum_lon;

            lon = lon % GlobalItems.TWOPI;
            if (lon < 0.0d)
                lon += GlobalItems.TWOPI;

            lat = 0.0d;

            dis = 1.0001026d + factor * sum_r;

            // /*
            // Compute mean obliquity of the ecliptic.
            // */

            t = u * 100.0d;
            t2 = t * t;
            emean = (0.001813d * t2 * t - 0.00059d * t2 - 46.815d * t + 84381.448d) / GlobalItems.RAD2SEC;

            // /*
            // Compute equatorial spherical coordinates referred to the mean equator 
            // and equinox of date.
            // */

            sin_lon = Sin(lon);
            ra = Atan2(Cos(emean) * sin_lon, Cos(lon)) * GlobalItems.RAD2DEG;
            ra = ra % 360.0d;
            if (ra < 0.0d)
                ra += 360.0d;
            ra = ra / 15.0d;

            dec = Asin(Sin(emean) * sin_lon) * GlobalItems.RAD2DEG;

        }

    }
}