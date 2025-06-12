using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;

namespace ASCOM.DeviceHub.Business_Object_Classes.Dome_Classes
{
    internal class DomeSyncWallace
    {
        // int dome ( double phi, double rdome, double xm, double ym, double zm, double xt, double yt, double yo, double ta, double tb, double* az, double* el )
        public static Point DomePosition(double phi, double rdome, double xm, double ym, double zm, double xt, double yt, double yo, double ta, double tb)
        {
            const double D2PI = 6.2831853071795864769;

            double sp, cp, sa, ca, sb, cb, y, xmo, ymo, zmo, xdo, ydo, zdo, x, z, xs, ys, zs, sdt, tm2, w, f;

            /* Validate dome radius. */
            if (rdome <= 0.0)
                throw new InvalidValueException($"DomeSyncWallace - Dome radius must be non-zero and positive: {rdome}"); ;

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
                throw new Exception($"DomeSyncWallace - w is zero or negative: {w}");

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
