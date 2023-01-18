using System;

using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.Kepler;
using ASCOM.Astrometry.NOVAS;
using ASCOM.Astrometry.NOVASCOM;
using ASCOM.Utilities;

namespace KeplerConsoleApp
{
    internal class Program
    {
        static TraceLogger TL;

        static void Main(string[] args)
        {
            // Create a TraceLogger for this run
            TL = new TraceLogger("NOVAS-Kepler")
            {
                Enabled = true
            };

            try
            {
                // Create components
                Util util = new Util();
                AstroUtils astroUtils = new AstroUtils();
                NOVAS31 novas31 = new NOVAS31();
                DateTime targetTime = new DateTime(2023, 1, 8, 14, 22, 12);

                // Set test parameters
                double targetJd = util.DateLocalToJulian(targetTime);
                LogMessage($"Target time is {targetTime}, JD = {targetJd:f5}");

                double deltaT = novas31.DeltaT(targetJd);
                LogMessage($"DeltaT is {deltaT}");
                //string text = "    CK19T040  2022 06  9.0116  4.242205  0.995633  351.1811  199.9392   53.6320  20230113   5.0  4.0  C / 2019 T4(ATLAS)                                        MPEC 2023 - A16"; double semiMajorAxis = 971.65; // Distance: 4.91
                //string text = "    CK17K020  2022 12 19.6877  1.796887  1.000807  236.2008   88.2357   87.5633  20230113   1.5  4.0  C/2017 K2 (PANSTARRS)                                    MPEC 2023-A16"; double semiMajorAxis = -2226.63; // Distance: 2.34

                //  C / 2022 U2(ATLAS) data
                string text = "    CK22U020  2023 01 14.2204  1.328037  0.986161  147.9085  304.4758   48.2504  20230113  16.0  4.0  C / 2022 U2(ATLAS)                                        MPEC 2023 - A16";
                double semiMajorAxis = 95.96;
                double earthDistance = 0.62010190095573; // AU
                double sunDistance = 1.3279845633525; // AU

                LogMessage($"Raw source data is {text}\r\n");

                // Extract orbital elements from the reference text line
                OrbitalElements elements = new OrbitalElements(text);

                LogMessage($"Orbital elements:\r\n{elements}\r\n");

                // ASCOM Planet
                Planet ascomComet = new Planet();
                ascomComet.Type = BodyType.Comet;
                ascomComet.Number = 9999;
                ascomComet.Name = elements.Name;
                ascomComet.DeltaT = deltaT;
                ascomComet.Ephemeris = CreateCometEphemerisASCOM(elements, util, semiMajorAxis);

                // NOVASCOM PLANET
                Type novascomCometType = Type.GetTypeFromProgID("NOVAS.Planet");
                dynamic novascomComet = Activator.CreateInstance(novascomCometType);
                novascomComet.Type = BodyType.Comet;
                novascomComet.Number = 9999; // Peter: NOVASCOM has a bug that I fixed in the Platform version where it throws an exception if Number == 0
                novascomComet.Name = elements.Name;
                novascomComet.DeltaT = deltaT;
                novascomComet.Ephemeris = CreateCometEphemerisNOVAS(elements, util);

                // This may be optional per remarks in the docs for the Planet object;

                // ASCOM Earth
                Earth ascomEarth = new Earth();
                ascomEarth.SetForTime(targetJd);

                // NOVASCOM Earth
                Type novascomEarthType = Type.GetTypeFromProgID("NOVAS.Earth");
                dynamic novascomEarth = Activator.CreateInstance(novascomEarthType);
                novascomEarth.SetForTime(targetJd);

                // ASCOM Site
                Site ascomSite = new Site();
                //ascomSite.Set( 31, 118, 100 ); // Mr Wu
                ascomSite.Set(31.5, -110, 1370); // Rick
                //ascomSite.Set(42, -76, 428);   // Dick

                // NOVASCOM Site
                Type novascomSiteType = Type.GetTypeFromProgID("NOVAS.Site");
                dynamic novascomSite = Activator.CreateInstance(novascomSiteType);
                //novascomSite.Set( 31, 118, 100 ); // Mr Wu
                novascomSite.Set(31.5, -110, 1370); // Rick
                //novascomSite.Set(42, -76, 428);   // Dick

                // Initialize the vector from Earth to the comet.

                // ASCOM Comet Vector
                PositionVector ascomCometVector = ascomComet.GetTopocentricPosition(targetJd, ascomSite, false);
                LogMessage($"Comet PositionVector returned by ASCOM GetTopocentricPosition (with semi-major axis) is       ({ascomCometVector.x}, {ascomCometVector.y}, {ascomCometVector.z})");

                // NOVASCOM Comet Vector
                dynamic novascomCometVector = novascomComet.GetTopocentricPosition(targetJd, novascomSite, false);
                LogMessage($"Comet PositionVector returned by NOVASCOM GetTopocentricPosition (without semi-major axis) is ({novascomCometVector.x}, {novascomCometVector.y}, {novascomCometVector.z})");

                // Initialize the vector from the Sun to the Earth.

                // ASCOM Earth Vector
                PositionVector ascomEarthVector = ascomEarth.HeliocentricPosition;
                LogMessage($"ASCOM Earth PositionVector is    ({ascomEarthVector.x:N10}, {ascomEarthVector.y:N10}, {ascomEarthVector.z:N10})");

                // ASCOM Earth Vector
                dynamic novascomEarthVector = novascomEarth.HeliocentricPosition;
                LogMessage($"NOVASCOM Earth PositionVector is ({novascomEarthVector.x:N10}, {novascomEarthVector.y:N10}, {novascomEarthVector.z:N10})");

                // Calculate the distance from the Earth to the comet.

                // ASCOM earth to comet distance
                double ascomDelta = ascomCometVector.Distance;
                LogMessage($"ASCOM Earth to comet distance (with semi-major axis) =                          {ascomDelta:f14} (For reference - Earth distance: {earthDistance:f14}, Sun distance: {sunDistance:f14})");

                // NOVASCOM earth to comet distance
                double novascomDelta = novascomCometVector.Distance;
                LogMessage($"NOVASCOM Earth to comet distance (without semi-major axis) =                    {novascomDelta:f14} (For reference - Earth distance: {earthDistance:f14}, Sun distance: {sunDistance:f14})");

                // Calculate the distance from the Sun to the comet.

                // ASCOM distance from sun to comet
                PositionVector ascomSunCometVector = VectorAddASCOM(ascomEarthVector, ascomCometVector);
                double ascomR = ascomSunCometVector.Distance;
                LogMessage($"ASCOM Sun to comet distance (with semi-major axis):                             {ascomR:f14} (For reference - Earth distance: {earthDistance:f14}, Sun distance: {sunDistance:f14})");

                // NOVASCOM distance from sun to comet
                dynamic novascomSunCometVector = VectorAddNOVAS(novascomEarthVector, novascomCometVector);
                double novascomR = novascomSunCometVector.Distance;
                LogMessage($"NOVASCOM Sun to comet distance (without semi-major axis):                       {novascomR:f14} (For reference - Earth distance: {earthDistance:f14}, Sun distance: {sunDistance:f14})");

                // ASCOM Alternative calculation for comet - sun distance without semi-major axis value
                Ephemeris ascomCometEphemeris = CreateCometEphemerisASCOM(elements, util, 0.0);
                double[] ascomCometPositionAndVelocity = ascomCometEphemeris.GetPositionAndVelocity(targetJd);
                ascomCometVector.x = ascomCometPositionAndVelocity[0];
                ascomCometVector.y = ascomCometPositionAndVelocity[1];
                ascomCometVector.z = ascomCometPositionAndVelocity[2];
                LogMessage($"ASCOM Alternative distance calculation (without setting the semi-major axis):   {ascomCometVector.Distance:f14} (For reference - Earth distance: {earthDistance:f14}, Sun distance: {sunDistance:f14})");

                ascomCometEphemeris = CreateCometEphemerisASCOM(elements, util, semiMajorAxis);
                ascomCometPositionAndVelocity = ascomCometEphemeris.GetPositionAndVelocity(targetJd);
                ascomCometVector.x = ascomCometPositionAndVelocity[0];
                ascomCometVector.y = ascomCometPositionAndVelocity[1];
                ascomCometVector.z = ascomCometPositionAndVelocity[2];
                LogMessage($"ASCOM Alternative distance calculation (including setting the semi-major axis): {ascomCometVector.Distance:f14} (For reference - Earth distance: {earthDistance:f14}, Sun distance: {sunDistance:f14})");

            }
            catch (Exception ex)
            {
                LogMessage($"Exception: {ex}");
            }

            TL.Enabled = false;
        }

        static PositionVector VectorAddASCOM(PositionVector v1, PositionVector v2)
        {
            PositionVector vecReturn = new PositionVector();
            vecReturn.x = v1.x + v2.x;
            vecReturn.y = v1.y + v2.y;
            vecReturn.z = v1.z + v2.z;

            return vecReturn;
        }

        static dynamic VectorAddNOVAS(dynamic v1, dynamic v2)
        {
            PositionVector vecReturn = new PositionVector();
            vecReturn.x = v1.x + v2.x;
            vecReturn.y = v1.y + v2.y;
            vecReturn.z = v1.z + v2.z;

            return vecReturn;
        }

        static Ephemeris CreateCometEphemerisASCOM(OrbitalElements elements, Util util, double semiMajorAxis)
        {
            Ephemeris kt = new Ephemeris();

            kt.BodyType = BodyType.Comet;

            kt.Name = elements.Name;


            kt.Epoch = util.DateLocalToJulian(elements.PerihelionPassage);

            kt.e = elements.OrbitalEccentricity;
            //kt.G = 0;
            //kt.H = 0;
            kt.M = 0;
            kt.n = 0;
            kt.Peri = elements.ArgOfPerihelion;
            kt.Node = elements.LongitudeOfAscNode;
            kt.Incl = elements.Inclination;
            kt.q = elements.PeriDistance;

            // Extra code to set the semi-major axis
            if (semiMajorAxis != 0.0) kt.a = semiMajorAxis;
            LogMessage($"kt.q: {kt.q} - kt.a: {kt.a}");
            return kt;
        }

        static object CreateCometEphemerisNOVAS(OrbitalElements elements, Util util)
        {
            Type ktType = Type.GetTypeFromProgID("Kepler.Ephemeris");
            dynamic kt = Activator.CreateInstance(ktType);

            kt.BodyType = BodyType.Comet;
            kt.Name = elements.Name;
            kt.Number = 9999;
            kt.Epoch = util.DateLocalToJulian(elements.PerihelionPassage);
            kt.e = elements.OrbitalEccentricity;
            kt.q = elements.PeriDistance;
            //kt.G = 0;
            //kt.H = 0;
            kt.M = 0;
            kt.n = 0;
            kt.Peri = elements.ArgOfPerihelion;
            kt.Node = elements.LongitudeOfAscNode;
            kt.Incl = elements.Inclination;

            return kt;
        }

        static void LogMessage(string message)
        {
            TL.LogMessageCrLf("NOVAS-Kepler", message);
            Console.WriteLine(message);
        }
    }
}
