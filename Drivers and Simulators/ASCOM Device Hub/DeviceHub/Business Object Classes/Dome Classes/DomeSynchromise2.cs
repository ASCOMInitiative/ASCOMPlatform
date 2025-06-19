using System;
using System.Windows;

namespace ASCOM.DeviceHub
{
    internal class DomeSynchromise2
    {
        /// <summary>
        /// Calculates the azimuth and elevation of the dome aperture required to align with the telescope's optical axis.
        /// </summary>
        /// <remarks>
        /// <para>Many thanks to Patrick Wallace for providing this algorithm: https://ascomtalk.groups.io/g/Help/message/48104</para>
        /// <para>This method computes the position of the dome aperture required to align with the
        /// telescope's optical axis based on the provided dome and telescope geometry. The azimuth and elevation are
        /// returned in degrees. Ensure that all input parameters are provided in consistent units and that the dome
        /// radius is positive.</para>
        /// </remarks>
        /// <param name="phi">The roll-axis elevation of the dome, in radians (must be 90 for Alt/Az mounts).</param>
        /// <param name="rdome">The radius of the dome, in the same units as the position coordinates. Must be positive.</param>
        /// <param name="xm">The x-coordinate of the mount axis intersection in the dome frame.</param>
        /// <param name="ym">The y-coordinate of the mount axis intersection in the dome frame.</param>
        /// <param name="zm">The z-coordinate of the mount axis intersection in the dome frame.</param>
        /// <param name="xt">The offset of the optical axis measured along the roll pitch axis (declination / altitude).</param>
        /// <param name="yt">Offset of the pitch and roll axes (almost always zero).</param>
        /// <param name="yo">The offset of the telescope's optical axis from the point of closest approach on the pitch axis (declination / altitude).</param>
        /// <param name="ta">The telescope azimuth angle, in radians.</param>
        /// <param name="tb">The telescope elevation angle, in radians.</param>
        /// <returns>A <see cref="Point"/> representing the azimuth and elevation of the dome aperture, in degrees. The X property of the point corresponds to the azimuth, and the Y property corresponds to the elevation.</returns>
        /// <exception cref="InvalidValueException">Thrown if <paramref name="rdome"/> is less than or equal to zero.</exception>
        /// <exception cref="Exception">Thrown if the calculated distance from the optical center to the dome aperture is invalid, indicating a potential issue with the dome geometry values.</exception>
        public static Point DomePosition(double phi, double rdome, double xm, double ym, double zm, double xt, double yt, double yo, double ta, double tb)
        {
            const double D2PI = 6.2831853071795864769;

            double sp, cp, sa, ca, sb, cb, y, xmo, ymo, zmo, xdo, ydo, zdo, x, z, xs, ys, zs, sdt, tm2, w, f;

            /* Validate dome radius. */
            if (rdome <= 0.0)
                throw new InvalidValueException($"DomeSynchromise2 - Dome radius must be non-zero and positive: {rdome}"); ;

            /* Functions of roll-axis elevation. */
            sp = Math.Sin(phi);
            cp = Math.Cos(phi);

            /* Functions of telescope attitude. */
            sa = -Math.Sin(ta);
            ca = Math.Cos(ta);
            sb = Math.Sin(tb);
            cb = Math.Cos(tb);

            /* Mount to optical center, mount frame. */
            y = yt + yo * sb;
            xmo = xt * ca + y * sa;
            ymo = -xt * sa + y * ca;
            zmo = yo * cb;

            /* Dome to optical center, az/el frame. */
            xdo = xm + xmo;
            ydo = ym + ymo * sp + zmo * cp;
            zdo = zm - ymo * cp + zmo * sp;

            /* Unit vector to star, az/el. */
            x = -sa * cb;
            y = -ca * cb;
            z = sb;
            xs = x;
            ys = y * sp + z * cp;
            zs = -y * cp + z * sp;

            /* Solve for distance from optical center to dome aperture. */
            sdt = xs * xdo + ys * ydo + zs * zdo;
            tm2 = xdo * xdo + ydo * ydo + zdo * zdo;
            w = sdt * sdt - tm2 + rdome * rdome;

            if (w < 0.0)
                throw new Exception($"Optical axis and dome slit cannot be aligned, are the dome geometry signs and values correct? (w: {w:0.0})");

            f = -sdt + Math.Sqrt(w);

            /* Vector from dome center to dome aperture. */
            x = xdo + f * xs;
            y = ydo + f * ys;
            z = zdo + f * zs;

            /* To spherical coordinates. */
            double az = (x != 0.0 || y != 0.0) ? Math.Atan2(x, y) : 0.0;
            if (az < 0.0)
                az += D2PI;

            double el = Math.Atan2(z, Math.Sqrt(x * x + y * y));

            return new Point(az * Globals.RAD_TO_DEG, el * Globals.RAD_TO_DEG);
        }
    }
}
