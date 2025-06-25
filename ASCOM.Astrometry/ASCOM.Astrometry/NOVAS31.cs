using ASCOM.Utilities;
using ASCOM.Utilities.Exceptions;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Environment;

namespace ASCOM.Astrometry.NOVAS
{

    /// <summary>
    /// NOVAS31: Class presenting the contents of the USNO NOVAS 3.1 library. 
    /// NOVAS was developed by the Astronomical Applications department of the United States Naval 
    /// Observatory.
    /// </summary>
    /// <remarks>If you wish to explore or utilise NOVAS3.1 please see USNO's extensive help document "NOVAS 3.1 Users Guide" 
    /// (NOVAS C3.1 Guide.pdf) included in the ASCOM Platform Docs start menu folder. The latest revision is also available on the USNO web site at
    /// <href>http://www.usno.navy.mil/USNO/astronomical-applications/software-products/novas</href>
    /// in the "C Edition of NOVAS" link. 
    /// <para>If you use NOVAS, please send an e-mail through this page:
    /// <href>http://www.usno.navy.mil/help/astronomy-help</href> as this provides evidence to USNO that justifies further 
    /// improvements and developments of NOVAS capabilities.
    /// </para>
    /// </remarks>
    [Guid("B7203C35-B113-472D-9E5D-0602883AC835")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class NOVAS31 : INOVAS31, IDisposable
    {

        private const string NOVAS32DLL = "NOVAS31.dll"; // Names of NOVAS 32 and 64bit DLL files
        private const string NOVAS64DLL = "NOVAS31-64.dll";

        private const string JPL_EPHEM_FILE_NAME = "JPLEPH"; // Name of JPL ephemeredes file
        private double JPL_EPHEM_START_DATE = 2305424.5d; // First date of data in the ephemeredes file. Has to be a variable so to can be passed to ephem_open.
        private double JPL_EPHEM_END_DATE = 2525008.5d; // Last date of data in the ephemeredes file. Has to be a variable so to can be passed to ephem_open.

        private const string NOVAS_DLL_LOCATION = @"\ASCOM\Astrometry\"; // This is appended to the Common Files path
        private const string RACIO_FILE = "cio_ra.bin"; // Name of the RA of CIO binary data file

        private const string NOVAS31_MUTEX_NAME = "ASCOMNovas31Mutex";

        private TraceLogger TL;
        private Util Utl;
        private IntPtr Novas31DllHandle;

        private EarthRotationParameters Parameters;

        #region New and IDisposable
        /// <summary>
        /// Creates a new instance of the NOVAS31 component
        /// </summary>
        /// <exception cref="HelperException">Thrown if the NOVAS31 support library DLL cannot be loaded</exception>
        /// <remarks></remarks>
        public NOVAS31()
        {
            bool rc;
            short rc1;
            string Novas31DllFile, RACIOFile, JPLEphFile;
            var DENumber = default(short);
            var ReturnedPath = new System.Text.StringBuilder(260);
            int LastError;
            Mutex Novas31Mutex;
            var gotMutex = default(bool); // Flag indicating whether the NOVAS initialisation mutex was successfully claimed
            Log.Component(Assembly.GetExecutingAssembly().FullName, "NOVAS31");

            TL = new TraceLogger("", "NOVAS31");
            TL.Enabled = Utilities.Global.GetBool(Utilities.Global.NOVAS_TRACE, Utilities.Global.NOVAS_TRACE_DEFAULT); // Get enabled / disabled state from the user registry
            Novas31Mutex = new Mutex(false, NOVAS31_MUTEX_NAME); // Create a mutex that will ensure that only one NOVAS31 initialisation can occur at a time
            JPLEphFile = "";

            try
            {
                TL.LogMessage("New", "Creating EarthRotationParameters object");
                Parameters = new EarthRotationParameters(TL);
                TL.LogMessage("New", "Waiting for mutex");
                gotMutex = Novas31Mutex.WaitOne(10000); // Wait up to 10 seconds for the mutex to become available
                TL.LogMessage("New", $"Got mutex: {gotMutex}");

                Utl = new Util();

                // Find the root location of the common files directory containing the ASCOM support files.
                // On a 32bit system this is \Program Files\Common Files
                // On a 64bit system this is \Program Files (x86)\Common Files
                if (Is64Bit()) // 64bit application so find the 32bit folder location
                {
                    rc = SHGetSpecialFolderPath(IntPtr.Zero, ReturnedPath, CSIDL_PROGRAM_FILES_COMMONX86, false);
                    Novas31DllFile = ReturnedPath.ToString() + NOVAS_DLL_LOCATION + NOVAS64DLL;
                    RACIOFile = ReturnedPath.ToString() + NOVAS_DLL_LOCATION + RACIO_FILE;
                    JPLEphFile = ReturnedPath.ToString() + NOVAS_DLL_LOCATION + JPL_EPHEM_FILE_NAME;
                }
                else // 32bit application so just go with the .NET returned value
                {
                    Novas31DllFile = GetFolderPath(SpecialFolder.CommonProgramFiles) + NOVAS_DLL_LOCATION + NOVAS32DLL;
                    RACIOFile = GetFolderPath(SpecialFolder.CommonProgramFiles) + NOVAS_DLL_LOCATION + RACIO_FILE;
                    JPLEphFile = GetFolderPath(SpecialFolder.CommonProgramFiles) + NOVAS_DLL_LOCATION + JPL_EPHEM_FILE_NAME;
                }

                // Validate that the files exist
                TL.LogMessage("New", $"Path to NOVAS31 DLL: {Novas31DllFile}");
                TL.LogMessage("New", $"Path to RACIO file: {RACIOFile}");
                TL.LogMessage("New", $"Path to JPL ephemeris file: {Novas31DllFile}");

                if (!File.Exists(Novas31DllFile))
                {
                    TL.LogMessage("New", $"NOVAS31 Initialise - Unable to locate NOVAS support DLL: {Novas31DllFile}");
                    throw new HelperException($"NOVAS31 Initialise - Unable to locate NOVAS support DLL: {Novas31DllFile}");
                }
                else
                {
                    TL.LogMessage("New", $"Found NOVAS31 DLL: {Novas31DllFile}");
                }

                if (!File.Exists(RACIOFile))
                {
                    TL.LogMessage("New", $"NOVAS31 Initialise - Unable to locate RACIO file: {RACIOFile}");
                    throw new HelperException($"NOVAS31 Initialise - Unable to locate RACIO file: {RACIOFile}");
                }
                else
                {
                    TL.LogMessage("New", $"Found RACIO file: {RACIOFile}");
                }

                if (!File.Exists(JPLEphFile))
                {
                    TL.LogMessage("New", $"NOVAS31 Initialise - Unable to locate JPL ephemeris file: {JPLEphFile}");
                    throw new HelperException($"NOVAS31 Initialise - Unable to locate JPL ephemeris file: {JPLEphFile}");
                }
                else
                {
                    TL.LogMessage("New", $"Found  JPL ephemeris file: {JPLEphFile}");
                }

                TL.LogMessage("New", "Loading NOVAS31 library DLL: " + Novas31DllFile);

                Novas31DllHandle = LoadLibrary(Novas31DllFile);
                LastError = Marshal.GetLastWin32Error();

                if (Novas31DllHandle != IntPtr.Zero) // Loaded successfully
                {
                    TL.LogMessage("New", "Loaded NOVAS31 library OK");
                }
                else // Did not load 
                {
                    TL.LogMessage("New", $"Error loading NOVAS31 library: {LastError:X8} from {Novas31DllFile}");
                    throw new HelperException($"NOVAS31 Initialisation - Error code {LastError:X8} returned from LoadLibrary when loading NOVAS31 library {Novas31DllFile}");
                }

                // Establish the location of the file of CIO RAs
                SetRACIOFile(RACIOFile);

                // Open the ephemerides file and set its applicable date range
                rc1 = Ephem_Open(JPLEphFile, ref JPL_EPHEM_START_DATE, ref JPL_EPHEM_END_DATE, ref DENumber);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("New", "Exception: " + ex.ToString());
                throw new HelperException($"NOVAS31 Initialisation Exception - {ex.Message} (See inner exception for details)", ex);
            }
            finally
            {
                if (gotMutex)
                {
                    try
                    {
                        Novas31Mutex.ReleaseMutex();
                    }
                    catch
                    {
                    }
                }  // Release the initialisation mutex if we got it in the first place
            }

            if (rc1 > 0)
            {
                TL.LogMessage("New", "Unable to open ephemeris file: " + JPLEphFile + ", RC: " + rc1);
                throw new HelperException($"NOVAS31 Initialisation - Unable to open ephemeris file: {JPLEphFile} RC: {rc1}");
            }
            TL.LogMessage("New", $"Ephemeris file {JPLEphFile} opened OK - DE number: {DENumber}");

            TL.LogMessage("New", "NOVAS31 initialised OK");
        }

        private bool disposedValue = false;        // To detect redundant calls

        // IDisposable
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected virtual void Dispose(bool disposing)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Mutex Novas31Mutex;
            Novas31Mutex = null;

            try
            {
                Novas31Mutex = new Mutex(false, NOVAS31_MUTEX_NAME); // Create a mutex that will ensure that only one NOVAS31 dispose can occur at a time
                Novas31Mutex.WaitOne(10000); // Wait up to 10 seconds for the mutex to become available

                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // Free other state (managed objects).

                        if (Parameters is not null)
                        {
                            try
                            {
                                Parameters.Dispose();
                            }
                            catch
                            {
                            }
                            try
                            {
                                Parameters = null;
                            }
                            catch
                            {
                            }
                        }

                        if (Utl is not null)
                        {
                            try
                            {
                                Utl.Dispose();
                            }
                            catch
                            {
                            }
                            try
                            {
                                Utl = null;
                            }
                            catch
                            {
                            }
                        }
                        if (TL is not null)
                        {
                            try
                            {
                                TL.Enabled = false;
                            }
                            catch
                            {
                            }
                            try
                            {
                                TL.Dispose();
                            }
                            catch
                            {
                            }
                            try
                            {
                                TL = null;
                            }
                            catch
                            {
                            }
                        }
                    }

                    // Free your own state (unmanaged objects) and set large fields to null.
                    try
                    {
                        FreeLibrary(Novas31DllHandle);
                    }
                    catch
                    {
                    } // Free the NOVAS library but don't return any error value
                }
                disposedValue = true;
            }
            finally
            {
                if (Novas31Mutex is not null)
                    Novas31Mutex.ReleaseMutex();
            }
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// Cleans up the NOVAS3 object and releases its open file handle on the JPL planetary ephemeris file
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Public NOVAS Interface - Ephemeris Members

        /// <summary>
        /// Get position and velocity of target with respect to the centre object. 
        /// </summary>
        /// <param name="Tjd"> Two-element array containing the Julian date, which may be split any way (although the first 
        /// element is usually the "integer" part, and the second element is the "fractional" part).  Julian date is in the 
        /// TDB or "T_eph" time scale.</param>
        /// <param name="Target">Target object</param>
        /// <param name="Center">Centre object</param>
        /// <param name="Position">Position vector array of target relative to center, measured in AU.</param>
        /// <param name="Velocity">Velocity vector array of target relative to center, measured in AU/day.</param>
        /// <returns><pre>
        /// 0   ...everything OK.
        /// 1,2 ...error returned from State.</pre>
        /// </returns>
        /// <remarks>This function accesses the JPL planetary ephemeris to give the position and velocity of the target 
        /// object with respect to the center object.</remarks>
        public short PlanetEphemeris(ref double[] Tjd, Target Target, Target Center, ref double[] Position, ref double[] Velocity)
        {

            var JdHp = new JDHighPrecision();
            var VPos = new PosVector();
            var VVel = new VelVector();
            short rc;

            JdHp.JDPart1 = Tjd[0];
            JdHp.JDPart2 = Tjd[1];
            if (Is64Bit())
            {
                rc = PlanetEphemeris64(ref JdHp, Target, Center, ref VPos, ref VVel);
            }
            else
            {
                rc = PlanetEphemeris32(ref JdHp, Target, Center, ref VPos, ref VVel);
            }

            PosVecToArr(VPos, ref Position);
            VelVecToArr(VVel, ref Velocity);
            return rc;
        }

        /// <summary>
        /// Produces the Cartesian heliocentric equatorial coordinates of the asteroid for the J2000.0 epoch 
        /// coordinate system from a set of Chebyshev polynomials read from a file.
        /// </summary>
        /// <param name="Mp">The number of the asteroid for which the position in desired.</param>
        /// <param name="Name">The name of the asteroid.</param>
        /// <param name="Jd"> The Julian date on which to find the position and velocity.</param>
        /// <param name="Err"><pre>
        /// = 0 ( No error )
        /// = 1 ( Memory allocation error )
        /// = 2 ( Mismatch between asteroid name and number )
        /// = 3 ( Julian date out of bounds )
        /// = 4 ( Cannot find Chebyshev polynomial file )
        /// </pre>
        /// </param>
        /// <returns> 6-element array of double containing position and velocity vector values.</returns>
        /// <remarks>The file name of the asteroid is taken from the name given.  It is	assumed that the name 
        /// is all in lower case characters.
        /// <para>
        /// This routine will search in the application's current directory for a file of Chebyshev 
        /// polynomial coefficients whose name is based on the provided Name parameter: Name.chby 
        /// </para>
        /// <para>Further information on using NOVAS with minor planet data is given here: 
        /// http://www.usno.navy.mil/USNO/astronomical-applications/software-products/usnoae98</para>
        /// </remarks>
        public double[] ReadEph(int Mp, string Name, double Jd, ref int Err)
        {

            const int DOUBLE_LENGTH = 8;
            const int NUM_RETURN_VALUES = 6;

            var PosVec = new double[6];
            IntPtr EphPtr;
            var Bytes = new byte[49];

            if (Is64Bit())
            {
                EphPtr = ReadEph64(Mp, Name, Jd, ref Err);
            }
            else
            {
                EphPtr = ReadEph32(Mp, Name, Jd, ref Err);
            }

            if (Err == 0) // Get the returned values if the call was successful
            {
                if (EphPtr != IntPtr.Zero) // Only copy if the pointer is not NULL
                {
                    // Safely marshal unmanaged buffer to byte()
                    Marshal.Copy(EphPtr, Bytes, 0, NUM_RETURN_VALUES * DOUBLE_LENGTH);

                    // Convert to double()
                    for (int i = 0; i <= NUM_RETURN_VALUES - 1; i++)
                        PosVec[i] = BitConverter.ToDouble(Bytes, i * DOUBLE_LENGTH);
                }
                else
                {
                    for (int i = 0; i <= NUM_RETURN_VALUES - 1; i++)
                        PosVec[i] = double.NaN; // Return invalid values
                }
            }
            return PosVec;
        }

        /// <summary>
        /// Interface between the JPL direct-access solar system ephemerides and NOVAS-C.
        /// </summary>
        /// <param name="Tjd">Julian date of the desired time, on the TDB time scale.</param>
        /// <param name="Body">Body identification number for the solar system object of interest; 
        /// Mercury = 1, ..., Pluto= 9, Sun= 10, Moon = 11.</param>
        /// <param name="Origin">Origin code; solar system barycenter= 0, center of mass of the Sun = 1, center of Earth = 2.</param>
        /// <param name="Pos">Position vector of 'body' at tjd; equatorial rectangular coordinates in AU referred to the ICRS.</param>
        /// <param name="Vel">Velocity vector of 'body' at tjd; equatorial rectangular system referred to the ICRS.</param>
        /// <returns>Always returns 0</returns>
        /// <remarks></remarks>
        public short SolarSystem(double Tjd, Body Body, Origin Origin, ref double[] Pos, ref double[] Vel)
        {

            var VPos = new PosVector();
            var VVel = new VelVector();
            short rc;

            if (Is64Bit())
            {
                rc = SolarSystem64(Tjd, (short)Body, (short)Origin, ref VPos, ref VVel);
            }
            else
            {
                rc = SolarSystem32(Tjd, (short)Body, (short)Origin, ref VPos, ref VVel);
            }

            PosVecToArr(VPos, ref Pos);
            VelVecToArr(VVel, ref Vel);
            return rc;
        }

        /// <summary>
        /// Read and interpolate the JPL planetary ephemeris file.
        /// </summary>
        /// <param name="Jed">2-element Julian date (TDB) at which interpolation is wanted. Any combination of jed[0]+jed[1] which falls within the time span on the file is a permissible epoch.  See Note 1 below.</param>
        /// <param name="Target">The requested body to get data for from the ephemeris file.</param>
        /// <param name="TargetPos">The barycentric position vector array of the requested object, in AU. (If target object is the Moon, then the vector is geocentric.)</param>
        /// <param name="TargetVel">The barycentric velocity vector array of the requested object, in AU/Day.</param>
        /// <returns>
        /// <pre>
        /// 0 ...everything OK
        /// 1 ...error reading ephemeris file
        /// 2 ...epoch out of range.
        /// </pre></returns>
        /// <remarks>
        /// The target number designation of the astronomical bodies is:
        /// <pre>
        ///         = 0: Mercury,               1: Venus, 
        ///         = 2: Earth-Moon barycenter, 3: Mars, 
        ///         = 4: Jupiter,               5: Saturn, 
        ///         = 6: Uranus,                7: Neptune, 
        ///         = 8: Pluto,                 9: geocentric Moon, 
        ///         =10: Sun.
        /// </pre>
        /// <para>
        /// NOTE 1. For ease in programming, the user may put the entire epoch in jed[0] and set jed[1] = 0. 
        /// For maximum interpolation accuracy,  set jed[0] = the most recent midnight at or before interpolation epoch, 
        /// and set jed[1] = fractional part of a day elapsed between jed[0] and epoch. As an alternative, it may prove 
        /// convenient to set jed[0] = some fixed epoch, such as start of the integration and jed[1] = elapsed interval 
        /// between then and epoch.
        /// </para>
        /// </remarks>
        public short State(ref double[] Jed, Target Target, ref double[] TargetPos, ref double[] TargetVel)
        {

            var JdHp = new JDHighPrecision();
            var VPos = new PosVector();
            var VVel = new VelVector();
            short rc;

            JdHp.JDPart1 = Jed[0];
            JdHp.JDPart2 = Jed[1];
            if (Is64Bit())
            {
                rc = State64(ref JdHp, Target, ref VPos, ref VVel);
            }
            else
            {
                rc = State32(ref JdHp, Target, ref VPos, ref VVel);
            }

            PosVecToArr(VPos, ref TargetPos);
            VelVecToArr(VVel, ref TargetVel);
            return rc;
        }
        #endregion

        #region Public NOVAS Interface - NOVAS Members
        /// <summary>
        /// Corrects position vector for aberration of light.  Algorithm includes relativistic terms.
        /// </summary>
        /// <param name="Pos"> Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        /// <param name="Vel"> Velocity vector of center of mass of the Earth, referred to origin at solar system barycenter, components in AU/day.</param>
        /// <param name="LightTime"> Light time from object to Earth in days.</param>
        /// <param name="Pos2"> Position vector, referred to origin at center of mass of the Earth, corrected for aberration, components in AU</param>
        /// <remarks>If 'lighttime' = 0 on input, this function will compute it.</remarks>
        public void Aberration(double[] Pos, double[] Vel, double LightTime, ref double[] Pos2)
        {
            var VPos2 = default(PosVector);
            if (Is64Bit())
            {
                var argPos = ArrToPosVec(Pos);
                var argVel = ArrToVelVec(Vel);
                Aberration64(ref argPos, ref argVel, LightTime, ref VPos2);
            }
            else
            {
                var argPos1 = ArrToPosVec(Pos);
                var argVel1 = ArrToVelVec(Vel);
                Aberration32(ref argPos1, ref argVel1, LightTime, ref VPos2);
            }
            PosVecToArr(VPos2, ref Pos2);

        }

        /// <summary>
        /// Compute the apparent place of a planet or other solar system body.
        /// </summary>
        /// <param name="JdTt"> TT Julian date for apparent place.</param>
        /// <param name="SsBody"> Pointer to structure containing the body designation for the solar system body </param>
        /// <param name="Accuracy"> Code specifying the relative accuracy of the output position.</param>
        /// <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        /// <param name="Dec"> Apparent declination in degrees, referred to true equator and equinox of date.</param>
        /// <param name="Dis"> True distance from Earth to planet at 'JdTt' in AU.</param>
        /// <returns><pre>
        ///    0 ... Everything OK
        ///    1 ... Invalid value of 'Type' in structure 'SsBody'
        /// > 10 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short AppPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
        {
            if (Is64Bit())
            {
                var argSsBody = O3IFromObject3(SsBody);
                return AppPlanet64(JdTt, ref argSsBody, Accuracy, ref Ra, ref Dec, ref Dis);
            }
            else
            {
                var argSsBody1 = O3IFromObject3(SsBody);
                return AppPlanet32(JdTt, ref argSsBody1, Accuracy, ref Ra, ref Dec, ref Dis);
            }

        }

        /// <summary>
        /// Computes the apparent place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        /// </summary>
        /// <param name="JdTt">TT Julian date for apparent place.</param>
        /// <param name="Star">Catalog entry structure containing catalog data forthe object in the ICRS </param>
        /// <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        /// <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date 'JdTt'.</param>
        /// <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date 'JdTt'.</param>
        /// <returns>
        /// <pre>
        ///    0 ... Everything OK
        /// > 10 ... Error code from function 'MakeObject'
        /// > 20 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short AppStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
        {

            short rc;
            try
            {
                TL.LogMessage("AppStar", "JD Accuracy:        " + JdTt + " " + Accuracy.ToString());
                TL.LogMessage("AppStar", "  Star.RA:          " + Utl.HoursToHMS(Star.RA, ":", ":", "", 3));
                TL.LogMessage("AppStar", "  Dec:              " + Utl.DegreesToDMS(Star.Dec, ":", ":", "", 3));
                TL.LogMessage("AppStar", "  Catalog:          " + Star.Catalog);
                TL.LogMessage("AppStar", "  Parallax:         " + Star.Parallax);
                TL.LogMessage("AppStar", "  ProMoDec:         " + Star.ProMoDec);
                TL.LogMessage("AppStar", "  ProMoRA:          " + Star.ProMoRA);
                TL.LogMessage("AppStar", "  RadialVelocity:   " + Star.RadialVelocity);
                TL.LogMessage("AppStar", "  StarName:         " + Star.StarName);
                TL.LogMessage("AppStar", "  StarNumber:       " + Star.StarNumber);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("AppStar", "Exception: " + ex.ToString());
            }

            if (Is64Bit())
            {
                rc = AppStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
                TL.LogMessage("AppStar", "  64bit - Return Code: " + rc + ", RA Dec: " + Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + Utl.DegreesToDMS(Dec, ":", ":", "", 3));
                return rc;
            }
            else
            {
                rc = AppStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
                TL.LogMessage("AppStar", "  32bit - Return Code: " + rc + ", RA Dec: " + Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + Utl.DegreesToDMS(Dec, ":", ":", "", 3));
                return rc;
            }

        }

        /// <summary>
        /// Compute the astrometric place of a planet or other solar system body.
        /// </summary>
        /// <param name="JdTt">TT Julian date for astrometric place.</param>
        /// <param name="SsBody">structure containing the body designation for the solar system body </param>
        /// <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        /// <param name="Ra">Astrometric right ascension in hours (referred to the ICRS, without light deflection or aberration).</param>
        /// <param name="Dec">Astrometric declination in degrees (referred to the ICRS, without light deflection or aberration).</param>
        /// <param name="Dis">True distance from Earth to planet in AU.</param>
        /// <returns>
        /// <pre>
        ///    0 ... Everything OK
        ///    1 ... Invalid value of 'Type' in structure 'SsBody'
        /// > 10 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short AstroPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
        {
            if (Is64Bit())
            {
                var argSsBody = O3IFromObject3(SsBody);
                return AstroPlanet64(JdTt, ref argSsBody, Accuracy, ref Ra, ref Dec, ref Dis);
            }
            else
            {
                var argSsBody1 = O3IFromObject3(SsBody);
                return AstroPlanet32(JdTt, ref argSsBody1, Accuracy, ref Ra, ref Dec, ref Dis);
            }
        }

        /// <summary>
        /// Computes the astrometric place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        /// </summary>
        /// <param name="JdTt">TT Julian date for astrometric place.</param>
        /// <param name="Star">Catalog entry structure containing catalog data for the object in the ICRS</param>
        /// <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        /// <param name="Ra">Astrometric right ascension in hours (referred to the ICRS, without light deflection or aberration).</param>
        /// <param name="Dec">Astrometric declination in degrees (referred to the ICRS, without light deflection or aberration).</param>
        /// <returns><pre>
        ///    0 ... Everything OK
        /// > 10 ... Error code from function 'MakeObject'
        /// > 20 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short AstroStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
        {
            short rc;
            try
            {
                TL.LogMessage("AstroStar", "JD Accuracy:        " + JdTt + " " + Accuracy.ToString());
                TL.LogMessage("AstroStar", "  Star.RA:          " + Utl.HoursToHMS(Star.RA, ":", ":", "", 3));
                TL.LogMessage("AstroStar", "  Dec:              " + Utl.DegreesToDMS(Star.Dec, ":", ":", "", 3));
                TL.LogMessage("AstroStar", "  Catalog:          " + Star.Catalog);
                TL.LogMessage("AstroStar", "  Parallax:         " + Star.Parallax);
                TL.LogMessage("AstroStar", "  ProMoDec:         " + Star.ProMoDec);
                TL.LogMessage("AstroStar", "  ProMoRA:          " + Star.ProMoRA);
                TL.LogMessage("AstroStar", "  RadialVelocity:   " + Star.RadialVelocity);
                TL.LogMessage("AstroStar", "  StarName:         " + Star.StarName);
                TL.LogMessage("AstroStar", "  StarNumber:       " + Star.StarNumber);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("AstroStar", "Exception: " + ex.ToString());
            }

            if (Is64Bit())
            {
                rc = AstroStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
                TL.LogMessage("AstroStar", "  64bit - Return Code: " + rc + ", RA Dec: " + Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + Utl.DegreesToDMS(Dec, ":", ":", "", 3));
                return rc;
            }
            else
            {
                rc = AstroStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
                TL.LogMessage("AstroStar", "  32bit - Return Code: " + rc + ", RA Dec: " + Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + Utl.DegreesToDMS(Dec, ":", ":", "", 3));
                return rc;
            }
        }

        /// <summary>
        /// Move the origin of coordinates from the barycenter of the solar system to the observer (or the geocenter); i.e., this function accounts for parallax (annual+geocentric or justannual).
        /// </summary>
        /// <param name="Pos">Position vector, referred to origin at solar system barycenter, components in AU.</param>
        /// <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar system barycenter, components in AU.</param>
        /// <param name="Pos2"> Position vector, referred to origin at center of mass of the Earth, components in AU.</param>
        /// <param name="Lighttime">Light time from object to Earth in days.</param>
        /// <remarks></remarks>
        public void Bary2Obs(double[] Pos, double[] PosObs, ref double[] Pos2, ref double Lighttime)
        {
            var PosV = new PosVector();
            if (Is64Bit())
            {
                var argPos = ArrToPosVec(Pos);
                var argPosObs = ArrToPosVec(PosObs);
                Bary2Obs64(ref argPos, ref argPosObs, ref PosV, ref Lighttime);
                PosVecToArr(PosV, ref Pos2);
            }
            else
            {
                var argPos1 = ArrToPosVec(Pos);
                var argPosObs1 = ArrToPosVec(PosObs);
                Bary2Obs32(ref argPos1, ref argPosObs1, ref PosV, ref Lighttime);
                PosVecToArr(PosV, ref Pos2);
            }
        }

        /// <summary>
        /// This function will compute a date on the Gregorian calendar given the Julian date.
        /// </summary>
        /// <param name="Tjd">Julian date.</param>
        /// <param name="Year">Year</param>
        /// <param name="Month">Month number</param>
        /// <param name="Day">day number</param>
        /// <param name="Hour">Fractional hour of the day</param>
        /// <remarks></remarks>
        public void CalDate(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour)
        {
            if (Is64Bit())
            {
                CalDate64(Tjd, ref Year, ref Month, ref Day, ref Hour);
            }
            else
            {
                CalDate32(Tjd, ref Year, ref Month, ref Day, ref Hour);
            }
        }

        /// <summary>
        /// This function rotates a vector from the celestial to the terrestrial system.  Specifically, it transforms a vector in the
        /// GCRS (a local space-fixed system) to the ITRS (a rotating earth-fixed system) by applying rotations for the GCRS-to-dynamical
        /// frame tie, precession, nutation, Earth rotation, and polar motion.
        /// </summary>
        /// <param name="JdHigh">High-order part of UT1 Julian date.</param>
        /// <param name="JdLow">Low-order part of UT1 Julian date.</param>
        /// <param name="DeltaT">Value of Delta T (= TT - UT1) at the input UT1 Julian date.</param>
        /// <param name="Method"> Selection for method: 0 ... CIO-based method; 1 ... equinox-based method</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="OutputOption">0 ... The output vector is referred to GCRS axes; 1 ... The output 
        /// vector is produced with respect to the equator and equinox of date. (See note 2 below)</param>
        /// <param name="xp">Conventionally-defined X coordinate of celestial intermediate pole with respect to 
        /// ITRS pole, in arcseconds.</param>
        /// <param name="yp">Conventionally-defined Y coordinate of celestial intermediate pole with respect to 
        /// ITRS pole, in arcseconds.</param>
        /// <param name="VecT">Position vector, geocentric equatorial rectangular coordinates,
        /// referred to GCRS axes (celestial system) or with respect to
        /// the equator and equinox of date, depending on 'option'.</param>
        /// <param name="VecC">Position vector, geocentric equatorial rectangular coordinates,
        /// referred to ITRS axes (terrestrial system).</param>
        /// <returns><pre>
        ///    0 ... everything is ok
        ///    1 ... invalid value of 'Accuracy'
        ///    2 ... invalid value of 'Method'
        /// > 10 ... 10 + error from function 'CioLocation'
        /// > 20 ... 20 + error from function 'CioBasis'
        /// </pre></returns>
        /// <remarks>Note 1: 'x' = 'y' = 0 means no polar motion transformation.
        /// <para>
        /// Note2: 'option' = 1 only works for the equinox-based method.
        /// </para></remarks>
        public short Cel2Ter(double JdHigh, double JdLow, double DeltaT, Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double xp, double yp, double[] VecT, ref double[] VecC)
        {
            var VVecC = new PosVector();
            short rc;
            if (Is64Bit())
            {
                var argVecT = ArrToPosVec(VecT);
                rc = Cel2Ter64(JdHigh, JdLow, DeltaT, Method, Accuracy, OutputOption, xp, yp, ref argVecT, ref VVecC);
            }
            else
            {
                var argVecT1 = ArrToPosVec(VecT);
                rc = Cel2Ter32(JdHigh, JdLow, DeltaT, Method, Accuracy, OutputOption, xp, yp, ref argVecT1, ref VVecC);
            }

            PosVecToArr(VVecC, ref VecC);
            return rc;
        }


        /// <summary>
        /// This function allows for the specification of celestial pole offsets for high-precision applications.  Each set of offsets is a correction to the modeled position of the pole for a specific date, derived from observations and published by the IERS.
        /// </summary>
        /// <param name="Tjd">TDB or TT Julian date for pole offsets.</param>
        /// <param name="Type"> Type of pole offset. 1 for corrections to angular coordinates of modeled pole referred to mean ecliptic of date, that is, delta-delta-psi and delta-delta-epsilon.  2 for corrections to components of modeled pole unit vector referred to GCRS axes, that is, dx and dy.</param>
        /// <param name="Dpole1">Value of celestial pole offset in first coordinate, (delta-delta-psi or dx) in milliarcseconds.</param>
        /// <param name="Dpole2">Value of celestial pole offset in second coordinate, (delta-delta-epsilon or dy) in milliarcseconds.</param>
        /// <returns><pre>
        /// 0 ... Everything OK
        /// 1 ... Invalid value of 'Type'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short CelPole(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2)
        {
            if (Is64Bit())
            {
                return CelPole64(Tjd, Type, Dpole1, Dpole2);
            }
            else
            {
                return CelPole32(Tjd, Type, Dpole1, Dpole2);
            }
        }

        /// <summary>
        /// Calaculate an array of CIO RA values around a given date
        /// </summary>
        /// <param name="JdTdb">TDB Julian date.</param>
        /// <param name="NPts"> Number of Julian dates and right ascension values requested (not less than 2 or more than 20).</param>
        /// <param name="Cio"> An arraylist of RaOfCIO structures containing a time series of the right ascension of the 
        /// Celestial Intermediate Origin (CIO) with respect to the GCRS.</param>
        /// <returns><pre>
        /// 0 ... everything OK
        /// 1 ... error opening the 'cio_ra.bin' file
        /// 2 ... 'JdTdb' not in the range of the CIO file; 
        /// 3 ... 'NPts' out of range
        /// 4 ... unable to allocate memory for the internal 't' array; 
        /// 5 ... unable to allocate memory for the internal 'ra' array; 
        /// 6 ... 'JdTdb' is too close to either end of the CIO file; unable to put 'NPts' data points into the output object.
        /// </pre></returns>
        /// <remarks>
        /// <para>
        /// Given an input TDB Julian date and the number of data points desired, this function returns a set of 
        /// Julian dates and corresponding values of the GCRS right ascension of the celestial intermediate origin (CIO).  
        /// The range of dates is centered (at least approximately) on the requested date.  The function obtains 
        /// the data from an external data file.</para>
        /// <example>How to create and retrieve values from the arraylist
        /// <code>
        /// Dim CioList As New ArrayList, Nov3 As New ASCOM.Astrometry.NOVAS3
        /// 
        /// rc = Nov3.CioArray(2455251.5, 20, CioList) ' Get 20 values around date 00:00:00 February 24th 2010
        /// MsgBox("Nov3 RC= " <![CDATA[&]]>  rc)
        /// 
        /// For Each CioA As ASCOM.Astrometry.RAOfCio In CioList
        ///     MsgBox("CIO Array " <![CDATA[&]]> CioA.JdTdb <![CDATA[&]]> " " <![CDATA[&]]> CioA.RACio)
        /// Next
        /// </code>
        /// </example>
        /// </remarks>
        public short CioArray(double JdTdb, int NPts, ref ArrayList Cio)
        {
            var CioStruct = new RAOfCioArray();
            short rc;

            CioStruct.Initialise(); // Set internal default values so we can see which elements are changed by the NOVAS DLL.
            if (Is64Bit())
            {
                rc = CioArray64(JdTdb, NPts, ref CioStruct);
            }
            else
            {
                rc = CioArray32(JdTdb, NPts, ref CioStruct);
            }

            RACioArrayStructureToArr(CioStruct, ref Cio); // Copy data from the CioStruct structure to the returning arraylist
            return rc;
        }

        /// <summary>
        /// Compute the orthonormal basis vectors of the celestial intermediate system.
        /// </summary>
        /// <param name="JdTdbEquionx">TDB Julian date of epoch.</param>
        /// <param name="RaCioEquionx">Right ascension of the CIO at epoch (hours).</param>
        /// <param name="RefSys">Reference system in which right ascension is given. 1 ... GCRS; 2 ... True equator and equinox of date.</param>
        /// <param name="Accuracy">Accuracy</param>
        /// <param name="x">Unit vector toward the CIO, equatorial rectangular coordinates, referred to the GCRS.</param>
        /// <param name="y">Unit vector toward the y-direction, equatorial rectangular coordinates, referred to the GCRS.</param>
        /// <param name="z">Unit vector toward north celestial pole (CIP), equatorial rectangular coordinates, referred to the GCRS.</param>
        /// <returns><pre>
        /// 0 ... everything OK
        /// 1 ... invalid value of input variable 'RefSys'.
        /// </pre></returns>
        /// <remarks>
        /// To compute the orthonormal basis vectors, with respect to the GCRS (geocentric ICRS), of the celestial 
        /// intermediate system defined by the celestial intermediate pole (CIP) (in the z direction) and 
        /// the celestial intermediate origin (CIO) (in the x direction).  A TDB Julian date and the 
        /// right ascension of the CIO at that date is required as input.  The right ascension of the CIO 
        /// can be with respect to either the GCRS origin or the true equinox of date -- different algorithms 
        /// are used in the two cases.</remarks>
        public short CioBasis(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z)
        {
            if (Is64Bit())
            {
                return CioBasis64(JdTdbEquionx, RaCioEquionx, RefSys, Accuracy, ref x, ref y, ref z);
            }
            else
            {
                return CioBasis32(JdTdbEquionx, RaCioEquionx, RefSys, Accuracy, ref x, ref y, ref z);
            }
        }

        /// <summary>
        /// Returns the location of the celestial intermediate origin (CIO) for a given Julian date, as a right ascension 
        /// </summary>
        /// <param name="JdTdb">TDB Julian date.</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="RaCio">Right ascension of the CIO, in hours.</param>
        /// <param name="RefSys">Reference system in which right ascension is given</param>
        /// <returns><pre>
        ///    0 ... everything OK
        ///    1 ... unable to allocate memory for the 'cio' array
        /// > 10 ... 10 + the error code from function 'CioArray'.
        /// </pre></returns>
        /// <remarks>  This function returns the location of the celestial intermediate origin (CIO) for a given Julian date, as a right ascension with respect to either the GCRS (geocentric ICRS) origin or the true equinox of date.  The CIO is always located on the true equator (= intermediate equator) of date.</remarks>
        public short CioLocation(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys)
        {
            if (Is64Bit())
            {
                return CioLocation64(JdTdb, Accuracy, ref RaCio, ref RefSys);
            }
            else
            {
                return CioLocation32(JdTdb, Accuracy, ref RaCio, ref RefSys);
            }
        }

        /// <summary>
        /// Computes the true right ascension of the celestial intermediate origin (CIO) at a given TT Julian date.  This is -(equation of the origins).
        /// </summary>
        /// <param name="JdTt">TT Julian date</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="RaCio"> Right ascension of the CIO, with respect to the true equinox of date, in hours (+ or -).</param>
        /// <returns>
        /// <pre>
        ///   0  ... everything OK
        ///   1  ... invalid value of 'Accuracy'
        /// > 10 ... 10 + the error code from function 'CioLocation'
        /// > 20 ... 20 + the error code from function 'CioBasis'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short CioRa(double JdTt, Accuracy Accuracy, ref double RaCio)
        {
            if (Is64Bit())
            {
                return CioRa64(JdTt, Accuracy, ref RaCio);
            }
            else
            {
                return CioRa32(JdTt, Accuracy, ref RaCio);
            }
        }

        /// <summary>
        /// Returns the difference in light-time, for a star, between the barycenter of the solar system and the observer (or the geocenter).
        /// </summary>
        /// <param name="Pos1">Position vector of star, with respect to origin at solar system barycenter.</param>
        /// <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar system barycenter, components in AU.</param>
        /// <returns>Difference in light time, in the sense star to barycenter minus star to earth, in days.</returns>
        /// <remarks>
        /// Alternatively, this function returns the light-time from the observer (or the geocenter) to a point on a 
        /// light ray that is closest to a specific solar system body.  For this purpose, 'Pos1' is the position 
        /// vector toward observed object, with respect to origin at observer (or the geocenter); 'PosObs' is 
        /// the position vector of solar system body, with respect to origin at observer (or the geocenter), 
        /// components in AU; and the returned value is the light time to point on line defined by 'Pos1' 
        /// that is closest to solar system body (positive if light passes body before hitting observer, i.e., if 
        /// 'Pos1' is within 90 degrees of 'PosObs').
        /// </remarks>
        public double DLight(double[] Pos1, double[] PosObs)
        {
            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                var argPosObs = ArrToPosVec(PosObs);
                return DLight64(ref argPos1, ref argPosObs);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                var argPosObs1 = ArrToPosVec(PosObs);
                return DLight32(ref argPos11, ref argPosObs1);
            }
        }

        /// <summary>
        /// Converts an ecliptic position vector to an equatorial position vector.
        /// </summary>
        /// <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for coordinates.</param>
        /// <param name="CoordSys">Coordinate system selection. 0 ... mean equator and equinox of date; 1 ... true equator and equinox of date; 2 ... ICRS</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Pos1"> Position vector, referred to specified ecliptic and equinox of date.  If 'CoordSys' = 2, 'pos1' must be on mean ecliptic and equinox of J2000.0; see Note 1 below.</param>
        /// <param name="Pos2">Position vector, referred to specified equator and equinox of date.</param>
        /// <returns><pre>
        /// 0 ... everything OK
        /// 1 ... invalid value of 'CoordSys'
        /// </pre></returns>
        /// <remarks>
        /// To convert an ecliptic vector (mean ecliptic and equinox of J2000.0 only) to an ICRS vector, 
        /// set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. 
        /// Except for the output from this case, all vectors are assumed to be with respect to a dynamical system.
        /// </remarks>
        public short Ecl2EquVec(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double[] Pos1, ref double[] Pos2)
        {
            var VPos2 = new PosVector();
            short rc;
            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                rc = Ecl2EquVec64(JdTt, CoordSys, Accuracy, ref argPos1, ref VPos2);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                rc = Ecl2EquVec32(JdTt, CoordSys, Accuracy, ref argPos11, ref VPos2);
            }

            PosVecToArr(VPos2, ref Pos2);
            return rc;
        }

        /// <summary>
        /// Compute the "complementary terms" of the equation of the equinoxes consistent with IAU 2000 resolutions.
        /// </summary>
        /// <param name="JdHigh">High-order part of TT Julian date.</param>
        /// <param name="JdLow">Low-order part of TT Julian date.</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <returns>Complementary terms, in radians.</returns>
        /// <remarks>
        /// 1. The series used in this function was derived from Series from IERS Conventions (2003), Chapter 5, Table 5.2C.
        /// This same series was also adopted for use in the IAU's Standards of Fundamental Astronomy (SOFA) software (i.e., subroutine 
        /// eect00.for and function eect00.c).
        /// <para>2. The low-accuracy series used in this function is a simple implementation derived from the first reference, in which terms
        /// smaller than 2 microarcseconds have been omitted.</para>
        /// <para>3. This function is based on NOVAS Fortran routine 'eect2000', with the low-accuracy formula taken from NOVAS Fortran routine 'etilt'.</para>
        /// </remarks>
        public double EeCt(double JdHigh, double JdLow, Accuracy Accuracy)
        {
            if (Is64Bit())
            {
                return EeCt64(JdHigh, JdLow, Accuracy);
            }
            else
            {
                return EeCt32(JdHigh, JdLow, Accuracy);
            }
        }

        /// <summary>
        /// Retrieves the position and velocity of a solar system body from a fundamental ephemeris.
        /// </summary>
        /// <param name="Jd"> TDB Julian date split into two parts, where the sum jd[0] + jd[1] is the TDB Julian date.</param>
        /// <param name="CelObj">Structure containing the designation of the body of interest </param>
        /// <param name="Origin"> Origin code; solar system barycenter = 0, center of mass of the Sun = 1.</param>
        /// <param name="Accuracy">Slection for accuracy</param>
        /// <param name="Pos">Position vector of the body at 'Jd'; equatorial rectangular coordinates in AU referred to the ICRS.</param>
        /// <param name="Vel">Velocity vector of the body at 'Jd'; equatorial rectangular system referred to the mean equator and equinox of the ICRS, in AU/Day.</param>
        /// <returns><pre>
        ///    0 ... Everything OK
        ///    1 ... Invalid value of 'Origin'
        ///    2 ... Invalid value of 'Type' in 'CelObj'; 
        ///    3 ... Unable to allocate memory
        /// 10+n ... where n is the error code from 'SolarSystem'; 
        /// 20+n ... where n is the error code from 'ReadEph'.
        /// </pre></returns>
        /// <remarks>It is recommended that the input structure 'cel_obj' be created using function 'MakeObject' in file novas.c.</remarks>
        public short Ephemeris(double[] Jd, Object3 CelObj, Origin Origin, Accuracy Accuracy, ref double[] Pos, ref double[] Vel)
        {
            var VPos = new PosVector();
            var VVel = new VelVector();
            JDHighPrecision JdHp;
            short rc;
            JdHp.JDPart1 = Jd[0];
            JdHp.JDPart2 = Jd[1];
            if (Is64Bit())
            {
                var argCelObj = O3IFromObject3(CelObj);
                rc = Ephemeris64(ref JdHp, ref argCelObj, Origin, Accuracy, ref VPos, ref VVel);
            }
            else
            {
                var argCelObj1 = O3IFromObject3(CelObj);
                rc = Ephemeris32(ref JdHp, ref argCelObj1, Origin, Accuracy, ref VPos, ref VVel);
            }

            PosVecToArr(VPos, ref Pos);
            VelVecToArr(VVel, ref Vel);
            return rc;
        }

        /// <summary>
        /// To convert right ascension and declination to ecliptic longitude and latitude.
        /// </summary>
        /// <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for coordinates.</param>
        /// <param name="CoordSys"> Coordinate system: 0 ... mean equator and equinox of date 'JdTt'; 1 ... true equator and equinox of date 'JdTt'; 2 ... ICRS</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Ra">Right ascension in hours, referred to specified equator and equinox of date.</param>
        /// <param name="Dec">Declination in degrees, referred to specified equator and equinox of date.</param>
        /// <param name="ELon">Ecliptic longitude in degrees, referred to specified ecliptic and equinox of date.</param>
        /// <param name="ELat">Ecliptic latitude in degrees, referred to specified ecliptic and equinox of date.</param>
        /// <returns><pre>
        /// 0 ... everything OK
        /// 1 ... invalid value of 'CoordSys'
        /// </pre></returns>
        /// <remarks>
        /// To convert ICRS RA and dec to ecliptic coordinates (mean ecliptic and equinox of J2000.0), 
        /// set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. 
        /// Except for the input to this case, all input coordinates are dynamical.
        /// </remarks>
        public short Equ2Ecl(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat)
        {
            if (Is64Bit())
            {
                return Equ2Ecl64(JdTt, CoordSys, Accuracy, Ra, Dec, ref ELon, ref ELat);
            }
            else
            {
                return Equ2Ecl32(JdTt, CoordSys, Accuracy, Ra, Dec, ref ELon, ref ELat);
            }
        }

        /// <summary>
        /// Converts an equatorial position vector to an ecliptic position vector.
        /// </summary>
        /// <param name="JdTt">TT Julian date of equator, equinox, and ecliptic used for</param>
        /// <param name="CoordSys"> Coordinate system selection. 0 ... mean equator and equinox of date 'JdTt'; 1 ... true equator and equinox of date 'JdTt'; 2 ... ICRS</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Pos1">Position vector, referred to specified equator and equinox of date.</param>
        /// <param name="Pos2">Position vector, referred to specified ecliptic and equinox of date.</param>
        /// <returns><pre>
        /// 0 ... everything OK
        /// 1 ... invalid value of 'CoordSys'
        /// </pre></returns>
        /// <remarks>To convert an ICRS vector to an ecliptic vector (mean ecliptic and equinox of J2000.0 only), 
        /// set 'CoordSys' = 2; the value of 'JdTt' can be set to anything, since J2000.0 is assumed. Except for 
        /// the input to this case, all vectors are assumed to be with respect to a dynamical system.</remarks>
        public short Equ2EclVec(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double[] Pos1, ref double[] Pos2)
        {
            var VPos2 = new PosVector();
            short rc;
            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                rc = Equ2EclVec64(JdTt, CoordSys, Accuracy, ref argPos1, ref VPos2);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                rc = Equ2EclVec32(JdTt, CoordSys, Accuracy, ref argPos11, ref VPos2);
            }

            PosVecToArr(VPos2, ref Pos2);
            return rc;
        }

        /// <summary>
        /// Converts ICRS right ascension and declination to galactic longitude and latitude.
        /// </summary>
        /// <param name="RaI">ICRS right ascension in hours.</param>
        /// <param name="DecI">ICRS declination in degrees.</param>
        /// <param name="GLon">Galactic longitude in degrees.</param>
        /// <param name="GLat">Galactic latitude in degrees.</param>
        /// <remarks></remarks>
        public void Equ2Gal(double RaI, double DecI, ref double GLon, ref double GLat)
        {
            if (Is64Bit())
            {
                Equ2Gal64(RaI, DecI, ref GLon, ref GLat);
            }
            else
            {
                Equ2Gal32(RaI, DecI, ref GLon, ref GLat);
            }
        }

        /// <summary>
        /// Transforms topocentric right ascension and declination to zenith distance and azimuth.  
        /// </summary>
        /// <param name="Jd_Ut1">UT1 Julian date.</param>
        /// <param name="DeltT">Difference TT-UT1 at 'jd_ut1', in seconds.</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="xp">onventionally-defined x coordinate of celestial intermediate pole with respect to ITRS reference pole, in arcseconds.</param>
        /// <param name="yp">Conventionally-defined y coordinate of celestial intermediate pole with respect to ITRS reference pole, in arcseconds.</param>
        /// <param name="Location">Structure containing observer's location </param>
        /// <param name="Ra">Topocentric right ascension of object of interest, in hours, referred to true equator and equinox of date.</param>
        /// <param name="Dec">Topocentric declination of object of interest, in degrees, referred to true equator and equinox of date.</param>
        /// <param name="RefOption">Refraction option. 0 ... no refraction; 1 ... include refraction, using 'standard' atmospheric conditions;
        /// 2 ... include refraction, using atmospheric parametersinput in the 'Location' structure.</param>
        /// <param name="Zd">Topocentric zenith distance in degrees, affected by refraction if 'ref_option' is non-zero.</param>
        /// <param name="Az">Topocentric azimuth (measured east from north) in degrees.</param>
        /// <param name="RaR"> Topocentric right ascension of object of interest, in hours, referred to true equator and 
        /// equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        /// <param name="DecR">Topocentric declination of object of interest, in degrees, referred to true equator and 
        /// equinox of date, affected by refraction if 'ref_option' is non-zero.</param>
        /// <remarks>This function transforms topocentric right ascension and declination to zenith distance and azimuth.  
        /// It uses a method that properly accounts for polar motion, which is significant at the sub-arcsecond level.  
        /// This function can also adjust coordinates for atmospheric refraction.</remarks>
        public void Equ2Hor(double Jd_Ut1, double DeltT, Accuracy Accuracy, double xp, double yp, OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR)
        {
            try
            {

                TL.LogMessage("Equ2Hor", "JD Accuracy RA DEC:     " + Jd_Ut1 + " " + Accuracy.ToString() + " " + Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + Utl.DegreesToDMS(Dec, ":", ":", "", 3));
                TL.LogMessage("Equ2Hor", "  DeltaT:               " + DeltT);
                TL.LogMessage("Equ2Hor", "  xp:                   " + xp);
                TL.LogMessage("Equ2Hor", "  yp:                   " + yp);
                TL.LogMessage("Equ2Hor", "  Refraction:           " + RefOption.ToString());
                TL.LogMessage("Equ2Hor", "  Location.Height:      " + Location.Height);
                TL.LogMessage("Equ2Hor", "  Location.Latitude:    " + Location.Latitude);
                TL.LogMessage("Equ2Hor", "  Location.Longitude:   " + Location.Longitude);
                TL.LogMessage("Equ2Hor", "  Location.Pressure:    " + Location.Pressure);
                TL.LogMessage("Equ2Hor", "  Location.Temperature: " + Location.Temperature);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Equ2Hor", "Exception: " + ex.ToString());
            }

            if (Is64Bit())
            {
                Equ2Hor64(Jd_Ut1, DeltT, Accuracy, xp, yp, ref Location, Ra, Dec, RefOption, ref Zd, ref Az, ref RaR, ref DecR);
                TL.LogMessage("Equ2Hor", "  64bit - RA Dec: " + Utl.HoursToHMS(RaR, ":", ":", "", 3) + " " + Utl.DegreesToDMS(DecR, ":", ":", "", 3));
            }
            else
            {
                Equ2Hor32(Jd_Ut1, DeltT, Accuracy, xp, yp, ref Location, Ra, Dec, RefOption, ref Zd, ref Az, ref RaR, ref DecR);
                TL.LogMessage("Equ2Hor", "  32bit - RA Dec: " + Utl.HoursToHMS(RaR, ":", ":", "", 3) + " " + Utl.DegreesToDMS(DecR, ":", ":", "", 3));
            }

        }

        /// <summary>
        /// Returns the value of the Earth Rotation Angle (theta) for a given UT1 Julian date. 
        /// </summary>
        /// <param name="JdHigh">High-order part of UT1 Julian date.</param>
        /// <param name="JdLow">Low-order part of UT1 Julian date.</param>
        /// <returns>The Earth Rotation Angle (theta) in degrees.</returns>
        /// <remarks> The expression used is taken from the note to IAU Resolution B1.8 of 2000.  1. The algorithm used 
        /// here is equivalent to the canonical theta = 0.7790572732640 + 1.00273781191135448 * t, where t is the time 
        /// in days from J2000 (t = JdHigh + JdLow - T0), but it avoids many two-PI 'wraps' that 
        /// decrease precision (adopted from SOFA Fortran routine iau_era00; see also expression at top 
        /// of page 35 of IERS Conventions (1996)).</remarks>
        public double Era(double JdHigh, double JdLow)
        {
            if (Is64Bit())
            {
                return Era64(JdHigh, JdLow);
            }
            else
            {
                return Era32(JdHigh, JdLow);
            }
        }

        /// <summary>
        /// Computes quantities related to the orientation of the Earth's rotation axis at Julian date 'JdTdb'.
        /// </summary>
        /// <param name="JdTdb">TDB Julian Date.</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Mobl">Mean obliquity of the ecliptic in degrees at 'JdTdb'.</param>
        /// <param name="Tobl">True obliquity of the ecliptic in degrees at 'JdTdb'.</param>
        /// <param name="Ee">Equation of the equinoxes in seconds of time at 'JdTdb'.</param>
        /// <param name="Dpsi">Nutation in longitude in arcseconds at 'JdTdb'.</param>
        /// <param name="Deps">Nutation in obliquity in arcseconds at 'JdTdb'.</param>
        /// <remarks>Values of the celestial pole offsets 'PSI_COR' and 'EPS_COR' are set using function 'cel_pole', 
        /// if desired.  See the prolog of 'cel_pole' for details.</remarks>
        public void ETilt(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps)
        {
            if (Is64Bit())
            {
                ETilt64(JdTdb, Accuracy, ref Mobl, ref Tobl, ref Ee, ref Dpsi, ref Deps);
            }
            else
            {
                ETilt32(JdTdb, Accuracy, ref Mobl, ref Tobl, ref Ee, ref Dpsi, ref Deps);
            }
        }

        /// <summary>
        /// To transform a vector from the dynamical reference system to the International Celestial Reference System (ICRS), or vice versa.
        /// </summary>
        /// <param name="Pos1">Position vector, equatorial rectangular coordinates.</param>
        /// <param name="Direction">Set 'direction' <![CDATA[<]]> 0 for dynamical to ICRS transformation. Set 'direction' <![CDATA[>=]]> 0 for 
        /// ICRS to dynamical transformation.</param>
        /// <param name="Pos2">Position vector, equatorial rectangular coordinates.</param>
        /// <remarks></remarks>
        public void FrameTie(double[] Pos1, FrameConversionDirection Direction, ref double[] Pos2)
        {
            var VPos2 = new PosVector();

            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                FrameTie64(ref argPos1, Direction, ref VPos2);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                FrameTie32(ref argPos11, Direction, ref VPos2);
            }
            PosVecToArr(VPos2, ref Pos2);
        }

        /// <summary>
        /// To compute the fundamental arguments (mean elements) of the Sun and Moon.
        /// </summary>
        /// <param name="t">TDB time in Julian centuries since J2000.0</param>
        /// <param name="a">Double array of fundamental arguments</param>
        /// <remarks>
        /// Fundamental arguments, in radians:
        /// <pre>
        ///   a[0] = l (mean anomaly of the Moon)
        ///   a[1] = l' (mean anomaly of the Sun)
        ///   a[2] = F (mean argument of the latitude of the Moon)
        ///   a[3] = D (mean elongation of the Moon from the Sun)
        ///   a[4] = a[4] (mean longitude of the Moon's ascending node);
        ///                from Simon section 3.4(b.3),
        ///                precession = 5028.8200 arcsec/cy)
        /// </pre>
        /// </remarks>
        public void FundArgs(double t, ref double[] a)
        {
            var va = new FundamentalArgs();

            if (Is64Bit())
            {
                FundArgs64(t, ref va);
            }
            else
            {
                FundArgs32(t, ref va);
            }

            a[0] = va.l;
            a[1] = va.ldash;
            a[2] = va.F;
            a[3] = va.D;
            a[4] = va.Omega;

        }

        /// <summary>
        /// Converts GCRS right ascension and declination to coordinates with respect to the equator of date (mean or true).
        /// </summary>
        /// <param name="JdTt">TT Julian date of equator to be used for output coordinates.</param>
        /// <param name="CoordSys"> Coordinate system selection for output coordinates.; 0 ... mean equator and 
        /// equinox of date; 1 ... true equator and equinox of date; 2 ... true equator and CIO of date</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="RaG">GCRS right ascension in hours.</param>
        /// <param name="DecG">GCRS declination in degrees.</param>
        /// <param name="Ra"> Right ascension in hours, referred to specified equator and right ascension origin of date.</param>
        /// <param name="Dec">Declination in degrees, referred to specified equator of date.</param>
        /// <returns>
        /// <pre>
        ///    0 ... everything OK
        /// >  0 ... error from function 'Vector2RaDec'' 
        /// > 10 ... 10 + error from function 'CioLocation'
        /// > 20 ... 20 + error from function 'CioBasis'
        /// </pre>></returns>
        /// <remarks>For coordinates with respect to the true equator of date, the origin of right ascension can be either the true equinox or the celestial intermediate origin (CIO).
        /// <para> This function only supports the CIO-based method.</para></remarks>
        public short Gcrs2Equ(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec)
        {
            if (Is64Bit())
            {
                return Gcrs2Equ64(JdTt, CoordSys, Accuracy, RaG, DecG, ref Ra, ref Dec);
            }
            else
            {
                return Gcrs2Equ32(JdTt, CoordSys, Accuracy, RaG, DecG, ref Ra, ref Dec);
            }
        }

        /// <summary>
        /// This function computes the geocentric position and velocity of an observer on 
        /// the surface of the earth or on a near-earth spacecraft.</summary>
        /// <param name="JdTt">TT Julian date.</param>
        /// <param name="DeltaT">Value of Delta T (= TT - UT1) at 'JdTt'.</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Obs">Data specifying the location of the observer</param>
        /// <param name="Pos">Position vector of observer, with respect to origin at geocenter, 
        /// referred to GCRS axes, components in AU.</param>
        /// <param name="Vel">Velocity vector of observer, with respect to origin at geocenter, 
        /// referred to GCRS axes, components in AU/day.</param>
        /// <returns>
        /// <pre>
        /// 0 ... everything OK
        /// 1 ... invalid value of 'Accuracy'.
        /// </pre></returns>
        /// <remarks>The final vectors are expressed in the GCRS.</remarks>
        public short GeoPosVel(double JdTt, double DeltaT, Accuracy Accuracy, Observer Obs, ref double[] Pos, ref double[] Vel)
        {
            var VPos = new PosVector();
            var VVel = new VelVector();
            short rc;

            if (Is64Bit())
            {
                rc = GeoPosVel64(JdTt, DeltaT, Accuracy, ref Obs, ref VPos, ref VVel);
            }
            else
            {
                rc = GeoPosVel32(JdTt, DeltaT, Accuracy, ref Obs, ref VPos, ref VVel);
            }

            PosVecToArr(VPos, ref Pos);
            VelVecToArr(VVel, ref Vel);
            return rc;
        }

        /// <summary>
        /// Computes the total gravitational deflection of light for the observed object due to the major gravitating bodies in the solar system.
        /// </summary>
        /// <param name="JdTdb">TDB Julian date of observation.</param>
        /// <param name="LocCode">Code for location of observer, determining whether the gravitational deflection due to the earth itself is applied.</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Pos1"> Position vector of observed object, with respect to origin at observer (or the geocenter), 
        /// referred to ICRS axes, components in AU.</param>
        /// <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at solar 
        /// system barycenter, referred to ICRS axes, components in AU.</param>
        /// <param name="Pos2">Position vector of observed object, with respect to origin at observer (or the geocenter), 
        /// referred to ICRS axes, corrected for gravitational deflection, components in AU.</param>
        /// <returns><pre>
        ///    0 ... Everything OK
        /// <![CDATA[<]]> 30 ... Error from function 'Ephemeris'; 
        /// > 30 ... Error from function 'MakeObject'.
        /// </pre></returns>
        /// <remarks>This function valid for an observed body within the solar system as well as for a star.
        /// <para>
        /// If 'Accuracy' is set to zero (full accuracy), three bodies (Sun, Jupiter, and Saturn) are 
        /// used in the calculation.  If the reduced-accuracy option is set, only the Sun is used in the 
        /// calculation.  In both cases, if the observer is not at the geocenter, the deflection due to the Earth is included.
        /// </para>
        /// </remarks>
        public short GravDef(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, double[] Pos1, double[] PosObs, ref double[] Pos2)
        {
            var VPos2 = new PosVector();
            short rc;

            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                var argPosObs = ArrToPosVec(PosObs);
                rc = GravDef64(JdTdb, LocCode, Accuracy, ref argPos1, ref argPosObs, ref VPos2);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                var argPosObs1 = ArrToPosVec(PosObs);
                rc = GravDef32(JdTdb, LocCode, Accuracy, ref argPos11, ref argPosObs1, ref VPos2);
            }

            PosVecToArr(VPos2, ref Pos2);
            return rc;
        }

        /// <summary>
        /// Corrects position vector for the deflection of light in the gravitational field of an arbitrary body.
        /// </summary>
        /// <param name="Pos1">Position vector of observed object, with respect to origin at observer 
        /// (or the geocenter), components in AU.</param>
        /// <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin at 
        /// solar system barycenter, components in AU.</param>
        /// <param name="PosBody">Position vector of gravitating body, with respect to origin at solar system 
        /// barycenter, components in AU.</param>
        /// <param name="RMass">Reciprocal mass of gravitating body in solar mass units, that is, 
        /// Sun mass / body mass.</param>
        /// <param name="Pos2">Position vector of observed object, with respect to origin at observer 
        /// (or the geocenter), corrected for gravitational deflection, components in AU.</param>
        /// <remarks>This function valid for an observed body within the solar system as well as for a star.</remarks>
        public void GravVec(double[] Pos1, double[] PosObs, double[] PosBody, double RMass, ref double[] Pos2)
        {
            var VPos2 = new PosVector();

            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                var argPosObs = ArrToPosVec(PosObs);
                var argPosBody = ArrToPosVec(PosBody);
                GravVec64(ref argPos1, ref argPosObs, ref argPosBody, RMass, ref VPos2);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                var argPosObs1 = ArrToPosVec(PosObs);
                var argPosBody1 = ArrToPosVec(PosBody);
                GravVec32(ref argPos11, ref argPosObs1, ref argPosBody1, RMass, ref VPos2);
            }

            PosVecToArr(VPos2, ref Pos2);
        }

        /// <summary>
        /// Compute the intermediate right ascension of the equinox at the input Julian date
        /// </summary>
        /// <param name="JdTdb">TDB Julian date.</param>
        /// <param name="Equinox">Equinox selection flag: mean pr true</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <returns>Intermediate right ascension of the equinox, in hours (+ or -). If 'equinox' = 1 
        /// (i.e true equinox), then the returned value is the equation of the origins.</returns>
        /// <remarks></remarks>
        public double IraEquinox(double JdTdb, EquinoxType Equinox, Accuracy Accuracy)
        {
            if (Is64Bit())
            {
                return IraEquinox64(JdTdb, Equinox, Accuracy);
            }
            else
            {
                return IraEquinox32(JdTdb, Equinox, Accuracy);
            }
        }

        /// <summary>
        /// Compute the Julian date for a given calendar date (year, month, day, hour).
        /// </summary>
        /// <param name="Year">Year number</param>
        /// <param name="Month">Month number</param>
        /// <param name="Day">Day number</param>
        /// <param name="Hour">Fractional hour of the day</param>
        /// <returns>Computed Julian date.</returns>
        /// <remarks>This function makes no checks for a valid input calendar date. The input calendar date 
        /// must be Gregorian. The input time value can be based on any UT-like time scale (UTC, UT1, TT, etc.) 
        /// - output Julian date will have the same basis.</remarks>
        public double JulianDate(short Year, short Month, short Day, double Hour)
        {
            if (Is64Bit())
            {
                return JulianDate64(Year, Month, Day, Hour);
            }
            else
            {
                return JulianDate32(Year, Month, Day, Hour);
            }
        }

        /// <summary>
        /// Computes the geocentric position of a solar system body, as antedated for light-time.
        /// </summary>
        /// <param name="JdTdb">TDB Julian date of observation.</param>
        /// <param name="SsObject">Structure containing the designation for thesolar system body</param>
        /// <param name="PosObs">Position vector of observer (or the geocenter), with respect to origin 
        /// at solar system barycenter, referred to ICRS axes, components in AU.</param>
        /// <param name="TLight0">First approximation to light-time, in days (can be set to 0.0 if unknown)</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Pos">Position vector of body, with respect to origin at observer (or the geocenter), 
        /// referred to ICRS axes, components in AU.</param>
        /// <param name="TLight">Final light-time, in days.</param>
        /// <returns><pre>
        ///    0 ... everything OK
        ///    1 ... algorithm failed to converge after 10 iterations
        /// <![CDATA[>]]> 10 ... error is 10 + error from function 'SolarSystem'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short LightTime(double JdTdb, Object3 SsObject, double[] PosObs, double TLight0, Accuracy Accuracy, ref double[] Pos, ref double TLight)
        {
            var VPos = new PosVector();
            short rc;
            if (Is64Bit())
            {
                var argSsObject = O3IFromObject3(SsObject);
                var argPosObs = ArrToPosVec(PosObs);
                rc = LightTime64(JdTdb, ref argSsObject, ref argPosObs, TLight0, Accuracy, ref VPos, ref TLight);
            }
            else
            {
                var argSsObject1 = O3IFromObject3(SsObject);
                var argPosObs1 = ArrToPosVec(PosObs);
                rc = LightTime32(JdTdb, ref argSsObject1, ref argPosObs1, TLight0, Accuracy, ref VPos, ref TLight);
            }

            PosVecToArr(VPos, ref Pos);
            return rc;
        }

        /// <summary>
        /// Determines the angle of an object above or below the Earth's limb (horizon).
        /// </summary>
        /// <param name="PosObj">Position vector of observed object, with respect to origin at 
        /// geocenter, components in AU.</param>
        /// <param name="PosObs">Position vector of observer, with respect to origin at geocenter, 
        /// components in AU.</param>
        /// <param name="LimbAng">Angle of observed object above (+) or below (-) limb in degrees.</param>
        /// <param name="NadirAng">Nadir angle of observed object as a fraction of apparent radius of limb: <![CDATA[<]]> 1.0 ... 
        /// below the limb; = 1.0 ... on the limb;  <![CDATA[>]]> 1.0 ... above the limb</param>
        /// <remarks>The geometric limb is computed, assuming the Earth to be an airless sphere (no 
        /// refraction or oblateness is included).  The observer can be on or above the Earth.  
        /// For an observer on the surface of the Earth, this function returns the approximate unrefracted 
        /// altitude.</remarks>
        public void LimbAngle(double[] PosObj, double[] PosObs, ref double LimbAng, ref double NadirAng)
        {
            if (Is64Bit())
            {
                var argPosObj = ArrToPosVec(PosObj);
                var argPosObs = ArrToPosVec(PosObs);
                LimbAngle64(ref argPosObj, ref argPosObs, ref LimbAng, ref NadirAng);
            }
            else
            {
                var argPosObj1 = ArrToPosVec(PosObj);
                var argPosObs1 = ArrToPosVec(PosObs);
                LimbAngle32(ref argPosObj1, ref argPosObs1, ref LimbAng, ref NadirAng);
            }
        }

        /// <summary>
        /// Computes the local place of a solar system body.
        /// </summary>
        /// <param name="JdTt">TT Julian date for local place.</param>
        /// <param name="SsBody">structure containing the body designation for the solar system body</param>
        /// <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        /// <param name="Position">Specifies the position of the observer</param>
        /// <param name="Accuracy">Specifies accuracy level</param>
        /// <param name="Ra">Local right ascension in hours, referred to the 'local GCRS'.</param>
        /// <param name="Dec">Local declination in degrees, referred to the 'local GCRS'.</param>
        /// <param name="Dis">True distance from Earth to planet in AU.</param>
        /// <returns><pre>
        ///    0 ... Everything OK
        ///    1 ... Invalid value of 'Where' in structure 'Location'; 
        /// <![CDATA[>]]> 10 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short LocalPlanet(double JdTt, Object3 SsBody, double DeltaT, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
        {
            if (Is64Bit())
            {
                var argSsBody = O3IFromObject3(SsBody);
                return LocalPlanet64(JdTt, ref argSsBody, DeltaT, ref Position, Accuracy, ref Ra, ref Dec, ref Dis);
            }
            else
            {
                var argSsBody1 = O3IFromObject3(SsBody);
                return LocalPlanet32(JdTt, ref argSsBody1, DeltaT, ref Position, Accuracy, ref Ra, ref Dec, ref Dis);
            }
        }

        /// <summary>
        /// Computes the local place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        /// </summary>
        /// <param name="JdTt">TT Julian date for local place. delta_t (double)</param>
        /// <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        /// <param name="Star">catalog entry structure containing catalog data for the object in the ICRS</param>
        /// <param name="Position">Structure specifying the position of the observer </param>
        /// <param name="Accuracy">Specifies accuracy level.</param>
        /// <param name="Ra">Local right ascension in hours, referred to the 'local GCRS'.</param>
        /// <param name="Dec">Local declination in degrees, referred to the 'local GCRS'.</param>
        /// <returns><pre>
        ///    0 ... Everything OK
        ///    1 ... Invalid value of 'Where' in structure 'Location'
        /// > 10 ... Error code from function 'MakeObject'
        /// > 20 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short LocalStar(double JdTt, double DeltaT, CatEntry3 Star, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec)
        {
            if (Is64Bit())
            {
                return LocalStar64(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
            }
            else
            {
                return LocalStar32(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
            }
        }

        /// <summary>
        /// Create a structure of type 'cat_entry' containing catalog data for a star or "star-like" object.
        /// </summary>
        /// <param name="StarName">Object name (50 characters maximum).</param>
        /// <param name="Catalog">Three-character catalog identifier (e.g. HIP = Hipparcos, TY2 = Tycho-2)</param>
        /// <param name="StarNum">Object number in the catalog.</param>
        /// <param name="Ra">Right ascension of the object (hours).</param>
        /// <param name="Dec">Declination of the object (degrees).</param>
        /// <param name="PmRa">Proper motion in right ascension (milliarcseconds/year).</param>
        /// <param name="PmDec">Proper motion in declination (milliarcseconds/year).</param>
        /// <param name="Parallax">Parallax (milliarcseconds).</param>
        /// <param name="RadVel">Radial velocity (kilometers/second).</param>
        /// <param name="Star">CatEntry3 structure containing the input data</param>
        /// <remarks></remarks>
        public void MakeCatEntry(string StarName, string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star)
        {
            if (Is64Bit())
            {
                MakeCatEntry64(StarName, Catalog, StarNum, Ra, Dec, PmRa, PmDec, Parallax, RadVel, ref Star);
            }
            else
            {
                MakeCatEntry32(StarName, Catalog, StarNum, Ra, Dec, PmRa, PmDec, Parallax, RadVel, ref Star);
            }
        }

        /// <summary>
        /// Makes a structure of type 'InSpace' - specifying the position and velocity of an observer situated 
        /// on a near-Earth spacecraft.
        /// </summary>
        /// <param name="ScPos">Geocentric position vector (x, y, z) in km.</param>
        /// <param name="ScVel">Geocentric velocity vector (x_dot, y_dot, z_dot) in km/s.</param>
        /// <param name="ObsSpace">InSpace structure containing the position and velocity of an observer situated 
        /// on a near-Earth spacecraft</param>
        /// <remarks></remarks>
        public void MakeInSpace(double[] ScPos, double[] ScVel, ref InSpace ObsSpace)
        {
            if (Is64Bit())
            {
                var argScPos = ArrToPosVec(ScPos);
                var argScVel = ArrToVelVec(ScVel);
                MakeInSpace64(ref argScPos, ref argScVel, ref ObsSpace);
            }
            else
            {
                var argScPos1 = ArrToPosVec(ScPos);
                var argScVel1 = ArrToVelVec(ScVel);
                MakeInSpace32(ref argScPos1, ref argScVel1, ref ObsSpace);
            }
        }

        /// <summary>
        /// Makes a structure of type 'object' - specifying a celestial object - based on the input parameters.
        /// </summary>
        /// <param name="Type">Type of object: 0 ... major planet, Sun, or Moon;  1 ... minor planet; 
        /// 2 ... object located outside the solar system (e.g. star, galaxy, nebula, etc.)</param>
        /// <param name="Number">Body number: For 'Type' = 0: Mercury = 1,...,Pluto = 9, Sun = 10, Moon = 11; 
        /// For 'Type' = 1: minor planet numberFor 'Type' = 2: set to 0 (zero)</param>
        /// <param name="Name">Name of the object (50 characters maximum).</param>
        /// <param name="StarData">Structure containing basic astrometric data for any celestial object 
        /// located outside the solar system; the catalog data for a star</param>
        /// <param name="CelObj">Structure containing the object definition</param>
        /// <returns><pre>
        /// 0 ... everything OK
        /// 1 ... invalid value of 'Type'
        /// 2 ... 'Number' out of range
        /// </pre></returns>
        /// <remarks></remarks>
        public short MakeObject(ObjectType Type, short Number, string Name, CatEntry3 StarData, ref Object3 CelObj)
        {
            var O3I = new Object3Internal();
            short rc;

            if (Is64Bit())
            {
                rc = MakeObject64(Type, Number, Name, ref StarData, ref O3I);
            }
            else
            {
                rc = MakeObject32(Type, Number, Name, ref StarData, ref O3I);
            }
            O3FromO3Internal(O3I, ref CelObj);
            return rc;
        }

        /// <summary>
        /// Makes a structure of type 'observer' - specifying the location of the observer.
        /// </summary>
        /// <param name="Where">Integer code specifying location of observer: 0: observer at geocenter; 
        /// 1: observer on surface of earth; 2: observer on near-earth spacecraft</param>
        /// <param name="ObsSurface">Structure containing data for an observer's location on the surface 
        /// of the Earth; used when 'Where' = 1</param>
        /// <param name="ObsSpace"> Structure containing an observer's location on a near-Earth spacecraft; 
        /// used when 'Where' = 2 </param>
        /// <param name="Obs">Structure specifying the location of the observer </param>
        /// <returns><pre>
        /// 0 ... everything OK
        /// 1 ... input value of 'Where' is out-of-range.
        /// </pre></returns>
        /// <remarks></remarks>
        public short MakeObserver(ObserverLocation Where, OnSurface ObsSurface, InSpace ObsSpace, ref Observer Obs)
        {
            if (Is64Bit())
            {
                return MakeObserver64(Where, ref ObsSurface, ref ObsSpace, ref Obs);
            }
            else
            {
                return MakeObserver32(Where, ref ObsSurface, ref ObsSpace, ref Obs);
            }
        }

        /// <summary>
        /// Makes a structure of type 'observer' specifying an observer at the geocenter.
        /// </summary>
        /// <param name="ObsAtGeocenter">Structure specifying the location of the observer at the geocenter</param>
        /// <remarks></remarks>
        public void MakeObserverAtGeocenter(ref Observer ObsAtGeocenter)
        {
            if (Is64Bit())
            {
                MakeObserverAtGeocenter64(ref ObsAtGeocenter);
            }
            else
            {
                MakeObserverAtGeocenter32(ref ObsAtGeocenter);
            }
        }

        /// <summary>
        /// Makes a structure of type 'observer' specifying the position and velocity of an observer 
        /// situated on a near-Earth spacecraft.
        /// </summary>
        /// <param name="ScPos">Geocentric position vector (x, y, z) in km.</param>
        /// <param name="ScVel">Geocentric position vector (x, y, z) in km.</param>
        /// <param name="ObsInSpace">Structure containing the position and velocity of an observer 
        /// situated on a near-Earth spacecraft</param>
        /// <remarks>Both input vectors are with respect to true equator and equinox of date.</remarks>
        public void MakeObserverInSpace(double[] ScPos, double[] ScVel, ref Observer ObsInSpace)
        {
            if (Is64Bit())
            {
                var argScPos = ArrToPosVec(ScPos);
                var argScVel = ArrToVelVec(ScVel);
                MakeObserverInSpace64(ref argScPos, ref argScVel, ref ObsInSpace);
            }
            else
            {
                var argScPos1 = ArrToPosVec(ScPos);
                var argScVel1 = ArrToVelVec(ScVel);
                MakeObserverInSpace32(ref argScPos1, ref argScVel1, ref ObsInSpace);
            }
        }

        /// <summary>
        /// Makes a structure of type 'observer' specifying the location of and weather for an observer 
        /// on the surface of the Earth.
        /// </summary>
        /// <param name="Latitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        /// <param name="Longitude">Geodetic (ITRS) longitude in degrees; east positive.</param>
        /// <param name="Height">Height of the observer (meters).</param>
        /// <param name="Temperature">Temperature (degrees Celsius).</param>
        /// <param name="Pressure">Atmospheric pressure (millibars).</param>
        /// <param name="ObsOnSurface">Structure containing the location of and weather for an observer on 
        /// the surface of the Earth</param>
        /// <remarks></remarks>
        public void MakeObserverOnSurface(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface)
        {
            if (Is64Bit())
            {
                MakeObserverOnSurface64(Latitude, Longitude, Height, Temperature, Pressure, ref ObsOnSurface);
            }
            else
            {
                MakeObserverOnSurface32(Latitude, Longitude, Height, Temperature, Pressure, ref ObsOnSurface);
            }
        }

        /// <summary>
        /// Makes a structure of type 'on_surface' - specifying the location of and weather for an 
        /// observer on the surface of the Earth.
        /// </summary>
        /// <param name="Latitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        /// <param name="Longitude">Geodetic (ITRS) latitude in degrees; north positive.</param>
        /// <param name="Height">Height of the observer (meters).</param>
        /// <param name="Temperature">Temperature (degrees Celsius).</param>
        /// <param name="Pressure">Atmospheric pressure (millibars).</param>
        /// <param name="ObsSurface">Structure containing the location of and weather for an 
        /// observer on the surface of the Earth.</param>
        /// <remarks></remarks>
        public void MakeOnSurface(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface)
        {
            if (Is64Bit())
            {
                MakeOnSurface64(Latitude, Longitude, Height, Temperature, Pressure, ref ObsSurface);
            }
            else
            {
                MakeOnSurface32(Latitude, Longitude, Height, Temperature, Pressure, ref ObsSurface);
            }
        }

        /// <summary>
        /// Compute the mean obliquity of the ecliptic.
        /// </summary>
        /// <param name="JdTdb">TDB Julian Date.</param>
        /// <returns>Mean obliquity of the ecliptic in arcseconds.</returns>
        /// <remarks></remarks>
        public double MeanObliq(double JdTdb)
        {
            if (Is64Bit())
            {
                return MeanObliq64(JdTdb);
            }
            else
            {
                return MeanObliq32(JdTdb);
            }

        }

        /// <summary>
        /// Computes the ICRS position of a star, given its apparent place at date 'JdTt'.  
        /// Proper motion, parallax and radial velocity are assumed to be zero.
        /// </summary>
        /// <param name="JdTt">TT Julian date of apparent place.</param>
        /// <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        /// <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date.</param>
        /// <param name="Accuracy">Specifies accuracy level</param>
        /// <param name="IRa">ICRS right ascension in hours.</param>
        /// <param name="IDec">ICRS declination in degrees.</param>
        /// <returns><pre>
        ///    0 ... Everything OK
        ///    1 ... Iterative process did not converge after 30 iterations; 
        /// > 10 ... Error from function 'Vector2RaDec'
        /// > 20 ... Error from function 'AppStar'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short MeanStar(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec)
        {
            if (Is64Bit())
            {
                return MeanStar64(JdTt, Ra, Dec, Accuracy, ref IRa, ref IDec);
            }
            else
            {
                return MeanStar32(JdTt, Ra, Dec, Accuracy, ref IRa, ref IDec);
            }
        }

        /// <summary>
        /// Normalize angle into the range 0 <![CDATA[<=]]> angle <![CDATA[<]]> (2 * pi).
        /// </summary>
        /// <param name="Angle">Input angle (radians).</param>
        /// <returns>The input angle, normalized as described above (radians).</returns>
        /// <remarks></remarks>
        public double NormAng(double Angle)
        {
            if (Is64Bit())
            {
                return NormAng64(Angle);
            }
            else
            {
                return NormAng32(Angle);
            }
        }

        /// <summary>
        /// Nutates equatorial rectangular coordinates from mean equator and equinox of epoch to true equator and equinox of epoch.
        /// </summary>
        /// <param name="JdTdb">TDB Julian date of epoch.</param>
        /// <param name="Direction">Flag determining 'direction' of transformation; direction  = 0 
        /// transformation applied, mean to true; direction != 0 inverse transformation applied, true to mean.</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Pos">Position vector, geocentric equatorial rectangular coordinates, referred to 
        /// mean equator and equinox of epoch.</param>
        /// <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, referred to 
        /// true equator and equinox of epoch.</param>
        /// <remarks> Inverse transformation may be applied by setting flag 'direction'</remarks>
        public void Nutation(double JdTdb, NutationDirection Direction, Accuracy Accuracy, double[] Pos, ref double[] Pos2)
        {
            var VPOs2 = new PosVector();

            if (Is64Bit())
            {
                var argPos = ArrToPosVec(Pos);
                Nutation64(JdTdb, Direction, Accuracy, ref argPos, ref VPOs2);
            }
            else
            {
                var argPos1 = ArrToPosVec(Pos);
                Nutation32(JdTdb, Direction, Accuracy, ref argPos1, ref VPOs2);
            }
            PosVecToArr(VPOs2, ref Pos2);
        }

        /// <summary>
        /// Returns the values for nutation in longitude and nutation in obliquity for a given TDB Julian date.
        /// </summary>
        /// <param name="t">TDB time in Julian centuries since J2000.0</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="DPsi">Nutation in longitude in arcseconds.</param>
        /// <param name="DEps">Nutation in obliquity in arcseconds.</param>
        /// <remarks>The nutation model selected depends upon the input value of 'Accuracy'.  See notes below for important details.
        /// <para>
        /// This function selects the nutation model depending first upon the input value of 'Accuracy'.  
        /// If 'Accuracy' = 0 (full accuracy), the IAU 2000A nutation model is used.  If 'Accuracy' = 1 
        /// a specially truncated (and therefore faster) version of IAU 2000A, called 'NU2000K' is used.
        /// </para>
        /// </remarks>
        public void NutationAngles(double t, Accuracy Accuracy, ref double DPsi, ref double DEps)
        {
            if (Is64Bit())
            {
                NutationAngles64(t, Accuracy, ref DPsi, ref DEps);
            }
            else
            {
                NutationAngles32(t, Accuracy, ref DPsi, ref DEps);
            }
        }

        /// <summary>
        /// Computes the apparent direction of a star or solar system body at a specified time 
        /// and in a specified coordinate system.
        /// </summary>
        /// <param name="JdTt">TT Julian date for place.</param>
        /// <param name="CelObject"> Specifies the celestial object of interest</param>
        /// <param name="Location">Specifies the location of the observer</param>
        /// <param name="DeltaT"> Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        /// <param name="CoordSys">Code specifying coordinate system of the output position. 0 ... GCRS or 
        /// "local GCRS"; 1 ... true equator and equinox of date; 2 ... true equator and CIO of date; 
        /// 3 ... astrometric coordinates, i.e., without light deflection or aberration.</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Output">Structure specifying object's place on the sky at time 'JdTt', 
        /// with respect to the specified output coordinate system</param>
        /// <returns>
        /// <pre>
        /// = 0         ... No problems.
        /// = 1         ... invalid value of 'CoordSys'
        /// = 2         ... invalid value of 'Accuracy'
        /// = 3         ... Earth is the observed object, and the observer is either at the geocenter or on the Earth's surface (not permitted)
        /// > 10, <![CDATA[<]]> 40  ... 10 + error from function 'Ephemeris'
        /// > 40, <![CDATA[<]]> 50  ... 40 + error from function 'GeoPosVel'
        /// > 50, <![CDATA[<]]> 70  ... 50 + error from function 'LightTime'
        /// > 70, <![CDATA[<]]> 80  ... 70 + error from function 'GravDef'
        /// > 80, <![CDATA[<]]> 90  ... 80 + error from function 'CioLocation'
        /// > 90, <![CDATA[<]]> 100 ... 90 + error from function 'CioBasis'
        /// </pre>
        /// </returns>
        /// Values of 'location->where' and 'CoordSys' dictate the various standard kinds of place:
        /// <pre>
        ///     Location->Where = 0 and CoordSys = 1: apparent place
        ///     Location->Where = 1 and CoordSys = 1: topocentric place
        ///     Location->Where = 0 and CoordSys = 0: virtual place
        ///     Location->Where = 1 and CoordSys = 0: local place
        ///     Location->Where = 0 and CoordSys = 3: astrometric place
        ///     Location->Where = 1 and CoordSys = 3: topocentric astrometric place
        /// </pre>
        /// <para>Input value of 'DeltaT' is used only when 'Location->Where' equals 1 or 2 (observer is 
        /// on surface of Earth or in a near-Earth satellite). </para>
        /// <remarks>
        /// </remarks>
        public short Place(double JdTt, Object3 CelObject, Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output)
        {
            if (Is64Bit())
            {
                var argCelObject = O3IFromObject3(CelObject);
                return Place64(JdTt, ref argCelObject, ref Location, DeltaT, CoordSys, Accuracy, ref Output);
            }
            else
            {
                var argCelObject1 = O3IFromObject3(CelObject);
                return Place32(JdTt, ref argCelObject1, ref Location, DeltaT, CoordSys, Accuracy, ref Output);
            }
        }

        /// <summary>
        /// Precesses equatorial rectangular coordinates from one epoch to another.
        /// </summary>
        /// <param name="JdTdb1">TDB Julian date of first epoch.  See remarks below.</param>
        /// <param name="Pos1">Position vector, geocentric equatorial rectangular coordinates, referred to mean dynamical equator and equinox of first epoch.</param>
        /// <param name="JdTdb2">TDB Julian date of second epoch.  See remarks below.</param>
        /// <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, referred to mean dynamical equator and equinox of second epoch.</param>
        /// <returns><pre>
        /// 0 ... everything OK
        /// 1 ... Precession not to or from J2000.0; 'JdTdb1' or 'JdTdb2' not 2451545.0.
        /// </pre></returns>
        /// <remarks> One of the two epochs must be J2000.0.  The coordinates are referred to the mean dynamical equator and equinox of the two respective epochs.</remarks>
        public short Precession(double JdTdb1, double[] Pos1, double JdTdb2, ref double[] Pos2)
        {

            var VPos2 = new PosVector();
            short rc;

            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                rc = Precession64(JdTdb1, ref argPos1, JdTdb2, ref VPos2);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                rc = Precession32(JdTdb1, ref argPos11, JdTdb2, ref VPos2);
            }
            PosVecToArr(VPos2, ref Pos2);
            return rc;
        }

        /// <summary>
        /// Applies proper motion, including foreshortening effects, to a star's position.
        /// </summary>
        /// <param name="JdTdb1">TDB Julian date of first epoch.</param>
        /// <param name="Pos">Position vector at first epoch.</param>
        /// <param name="Vel">Velocity vector at first epoch.</param>
        /// <param name="JdTdb2">TDB Julian date of second epoch.</param>
        /// <param name="Pos2">Position vector at second epoch.</param>
        /// <remarks></remarks>
        public void ProperMotion(double JdTdb1, double[] Pos, double[] Vel, double JdTdb2, ref double[] Pos2)
        {
            var VPos2 = new PosVector();

            if (Is64Bit())
            {
                var argPos = ArrToPosVec(Pos);
                var argVel = ArrToVelVec(Vel);
                ProperMotion64(JdTdb1, ref argPos, ref argVel, JdTdb2, ref VPos2);
            }
            else
            {
                var argPos1 = ArrToPosVec(Pos);
                var argVel1 = ArrToVelVec(Vel);
                ProperMotion32(JdTdb1, ref argPos1, ref argVel1, JdTdb2, ref VPos2);
            }

            PosVecToArr(VPos2, ref Pos2);
        }

        /// <summary>
        /// Converts equatorial spherical coordinates to a vector (equatorial rectangular coordinates).
        /// </summary>
        /// <param name="Ra">Right ascension (hours).</param>
        /// <param name="Dec">Declination (degrees).</param>
        /// <param name="Dist">Distance in AU</param>
        /// <param name="Vector">Position vector, equatorial rectangular coordinates (AU).</param>
        /// <remarks></remarks>
        public void RaDec2Vector(double Ra, double Dec, double Dist, ref double[] Vector)
        {
            var VVector = new PosVector();

            if (Is64Bit())
            {
                RaDec2Vector64(Ra, Dec, Dist, ref VVector);
            }
            else
            {
                RaDec2Vector32(Ra, Dec, Dist, ref VVector);
            }
            PosVecToArr(VVector, ref Vector);
        }

        /// <summary>
        /// Predicts the radial velocity of the observed object as it would be measured by spectroscopic means.
        /// </summary>
        /// <param name="CelObject">Specifies the celestial object of interest</param>
        /// <param name="Pos"> Geometric position vector of object with respect to observer, corrected for light-time, in AU.</param>
        /// <param name="Vel">Velocity vector of object with respect to solar system barycenter, in AU/day.</param>
        /// <param name="VelObs">Velocity vector of observer with respect to solar system barycenter, in AU/day.</param>
        /// <param name="DObsGeo">Distance from observer to geocenter, in AU.</param>
        /// <param name="DObsSun">Distance from observer to Sun, in AU.</param>
        /// <param name="DObjSun">Distance from object to Sun, in AU.</param>
        /// <param name="Rv">The observed radial velocity measure times the speed of light, in kilometers/second.</param>
        /// <remarks> Radial velocity is here defined as the radial velocity measure (z) times the speed of light.  
        /// For a solar system body, it applies to a fictitious emitter at the center of the observed object, 
        /// assumed massless (no gravitational red shift), and does not in general apply to reflected light.  
        /// For stars, it includes all effects, such as gravitational red shift, contained in the catalog 
        /// barycentric radial velocity measure, a scalar derived from spectroscopy.  Nearby stars with a known 
        /// kinematic velocity vector (obtained independently of spectroscopy) can be treated like 
        /// solar system objects.</remarks>
        public void RadVel(Object3 CelObject, double[] Pos, double[] Vel, double[] VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv)
        {
            if (Is64Bit())
            {
                var argCelObject = O3IFromObject3(CelObject);
                var argPos = ArrToPosVec(Pos);
                var argVel = ArrToVelVec(Vel);
                var argVelObs = ArrToVelVec(VelObs);
                RadVel64(ref argCelObject, ref argPos, ref argVel, ref argVelObs, DObsGeo, DObsSun, DObjSun, ref Rv);
            }
            else
            {
                var argCelObject1 = O3IFromObject3(CelObject);
                var argPos1 = ArrToPosVec(Pos);
                var argVel1 = ArrToVelVec(Vel);
                var argVelObs1 = ArrToVelVec(VelObs);
                RadVel32(ref argCelObject1, ref argPos1, ref argVel1, ref argVelObs1, DObsGeo, DObsSun, DObjSun, ref Rv);
            }
        }

        /// <summary>
        /// Computes atmospheric refraction in zenith distance. 
        /// </summary>
        /// <param name="Location">Structure containing observer's location.</param>
        /// <param name="RefOption">1 ... Use 'standard' atmospheric conditions; 2 ... Use atmospheric 
        /// parameters input in the 'Location' structure.</param>
        /// <param name="ZdObs">Observed zenith distance, in degrees.</param>
        /// <returns>Atmospheric refraction, in degrees.</returns>
        /// <remarks>This version computes approximate refraction for optical wavelengths. This function 
        /// can be used for planning observations or telescope pointing, but should not be used for the 
        /// reduction of precise observations.</remarks>
        public double Refract(OnSurface Location, RefractionOption RefOption, double ZdObs)
        {
            if (Is64Bit())
            {
                return Refract64(ref Location, RefOption, ZdObs);
            }
            else
            {
                return Refract32(ref Location, RefOption, ZdObs);
            }
        }

        /// <summary>
        /// Computes the Greenwich sidereal time, either mean or apparent, at Julian date 'JdHigh' + 'JdLow'.
        /// </summary>
        /// <param name="JdHigh">High-order part of UT1 Julian date.</param>
        /// <param name="JdLow">Low-order part of UT1 Julian date.</param>
        /// <param name="DeltaT"> Difference TT-UT1 at 'JdHigh'+'JdLow', in seconds of time.</param>
        /// <param name="GstType">0 ... compute Greenwich mean sidereal time; 1 ... compute Greenwich apparent sidereal time</param>
        /// <param name="Method">Selection for method: 0 ... CIO-based method; 1 ... equinox-based method</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Gst">Greenwich apparent sidereal time, in hours.</param>
        /// <returns><pre>
        ///          0 ... everything OK
        ///          1 ... invalid value of 'Accuracy'
        ///          2 ... invalid value of 'Method'
        /// > 10, <![CDATA[<]]> 30 ... 10 + error from function 'CioRai'
        /// </pre></returns>
        /// <remarks> The Julian date may be split at any point, but for highest precision, set 'JdHigh' 
        /// to be the integral part of the Julian date, and set 'JdLow' to be the fractional part.</remarks>
        public short SiderealTime(double JdHigh, double JdLow, double DeltaT, GstType GstType, Method Method, Accuracy Accuracy, ref double Gst)
        {

            if (Is64Bit())
            {
                return SiderealTime64(JdHigh, JdLow, DeltaT, GstType, Method, Accuracy, ref Gst);
            }
            else
            {
                return SiderealTime32(JdHigh, JdLow, DeltaT, GstType, Method, Accuracy, ref Gst);
            }
        }

        /// <summary>
        /// Transforms a vector from one coordinate system to another with same origin and axes rotated about the z-axis.
        /// </summary>
        /// <param name="Angle"> Angle of coordinate system rotation, positive counterclockwise when viewed from +z, in degrees.</param>
        /// <param name="Pos1">Position vector.</param>
        /// <param name="Pos2">Position vector expressed in new coordinate system rotated about z by 'angle'.</param>
        /// <remarks></remarks>
        public void Spin(double Angle, double[] Pos1, ref double[] Pos2)
        {
            var VPOs2 = new PosVector();
            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                Spin64(Angle, ref argPos1, ref VPOs2);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                Spin32(Angle, ref argPos11, ref VPOs2);
            }

            PosVecToArr(VPOs2, ref Pos2);
        }

        /// <summary>
        /// Converts angular quantities for stars to vectors.
        /// </summary>
        /// <param name="Star">Catalog entry structure containing ICRS catalog data </param>
        /// <param name="Pos">Position vector, equatorial rectangular coordinates, components in AU.</param>
        /// <param name="Vel">Velocity vector, equatorial rectangular coordinates, components in AU/Day.</param>
        /// <remarks></remarks>
        public void StarVectors(CatEntry3 Star, ref double[] Pos, ref double[] Vel)
        {
            var VPos = new PosVector();
            var VVel = new VelVector();
            if (Is64Bit())
            {
                StarVectors64(ref Star, ref VPos, ref VVel);
            }
            else
            {
                StarVectors32(ref Star, ref VPos, ref VVel);
            }

            PosVecToArr(VPos, ref Pos);
            VelVecToArr(VVel, ref Vel);
        }

        /// <summary>
        /// Computes the Terrestrial Time (TT) or Terrestrial Dynamical Time (TDT) Julian date corresponding 
        /// to a Barycentric Dynamical Time (TDB) Julian date.
        /// </summary>
        /// <param name="TdbJd">TDB Julian date.</param>
        /// <param name="TtJd">TT Julian date.</param>
        /// <param name="SecDiff">Difference 'tdb_jd'-'tt_jd', in seconds.</param>
        /// <remarks>Expression used in this function is a truncated form of a longer and more precise 
        /// series given in: Explanatory Supplement to the Astronomical Almanac, pp. 42-44 and p. 316. 
        /// The result is good to about 10 microseconds.</remarks>
        public void Tdb2Tt(double TdbJd, ref double TtJd, ref double SecDiff)
        {
            if (Is64Bit())
            {
                Tdb2Tt64(TdbJd, ref TtJd, ref SecDiff);
            }
            else
            {
                Tdb2Tt32(TdbJd, ref TtJd, ref SecDiff);
            }
        }

        /// <summary>
        /// This function rotates a vector from the terrestrial to the celestial system. 
        /// </summary>
        /// <param name="JdHigh">High-order part of UT1 Julian date.</param>
        /// <param name="JdLow">Low-order part of UT1 Julian date.</param>
        /// <param name="DeltaT">Value of Delta T (= TT - UT1) at the input UT1 Julian date.</param>
        /// <param name="Method"> Selection for method: 0 ... CIO-based method; 1 ... equinox-based method</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="OutputOption">0 ... The output vector is referred to GCRS axes; 1 ... The output 
        /// vector is produced with respect to the equator and equinox of date.</param>
        /// <param name="xp">Conventionally-defined X coordinate of celestial intermediate pole with respect to 
        /// ITRF pole, in arcseconds.</param>
        /// <param name="yp">Conventionally-defined Y coordinate of celestial intermediate pole with respect to 
        /// ITRF pole, in arcseconds.</param>
        /// <param name="VecT">Position vector, geocentric equatorial rectangular coordinates, referred to ITRF 
        /// axes (terrestrial system) in the normal case where 'option' = 0.</param>
        /// <param name="VecC"> Position vector, geocentric equatorial rectangular coordinates, referred to GCRS 
        /// axes (celestial system) or with respect to the equator and equinox of date, depending on 'Option'.</param>
        /// <returns><pre>
        ///    0 ... everything is ok
        ///    1 ... invalid value of 'Accuracy'
        ///    2 ... invalid value of 'Method'
        /// > 10 ... 10 + error from function 'CioLocation'
        /// > 20 ... 20 + error from function 'CioBasis'
        /// </pre></returns>
        /// <remarks>'x' = 'y' = 0 means no polar motion transformation.
        /// <para>
        /// The 'option' flag only works for the equinox-based method.
        /// </para></remarks>
        public short Ter2Cel(double JdHigh, double JdLow, double DeltaT, Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double xp, double yp, double[] VecT, ref double[] VecC)
        {
            var VVecC = new PosVector();
            short rc;
            if (Is64Bit())
            {
                var argVecT = ArrToPosVec(VecT);
                rc = Ter2Cel64(JdHigh, JdLow, DeltaT, Method, Accuracy, OutputOption, xp, yp, ref argVecT, ref VVecC);
            }
            else
            {
                var argVecT1 = ArrToPosVec(VecT);
                rc = Ter2Cel32(JdHigh, JdLow, DeltaT, Method, Accuracy, OutputOption, xp, yp, ref argVecT1, ref VVecC);
            }

            PosVecToArr(VVecC, ref VecC);
            return rc;
        }

        /// <summary>
        /// Computes the position and velocity vectors of a terrestrial observer with respect to the center of the Earth.
        /// </summary>
        /// <param name="Location">Structure containing observer's location </param>
        /// <param name="St">Local apparent sidereal time at reference meridian in hours.</param>
        /// <param name="Pos">Position vector of observer with respect to center of Earth, equatorial 
        /// rectangular coordinates, referred to true equator and equinox of date, components in AU.</param>
        /// <param name="Vel">Velocity vector of observer with respect to center of Earth, equatorial rectangular 
        /// coordinates, referred to true equator and equinox of date, components in AU/day.</param>
        /// <remarks>
        /// If reference meridian is Greenwich and st=0, 'pos' is effectively referred to equator and Greenwich.
        /// <para> This function ignores polar motion, unless the observer's longitude and latitude have been 
        /// corrected for it, and variation in the length of day (angular velocity of earth).</para>
        /// <para>The true equator and equinox of date do not form an inertial system.  Therefore, with respect 
        /// to an inertial system, the very small velocity component (several meters/day) due to the precession 
        /// and nutation of the Earth's axis is not accounted for here.</para>
        /// </remarks>
        public void Terra(OnSurface Location, double St, ref double[] Pos, ref double[] Vel)
        {
            var VPos = new PosVector();
            var VVel = new VelVector();
            if (Is64Bit())
            {
                Terra64(ref Location, St, ref VPos, ref VVel);
            }
            else
            {
                Terra32(ref Location, St, ref VPos, ref VVel);
            }

            PosVecToArr(VPos, ref Pos);
            VelVecToArr(VVel, ref Vel);
        }

        /// <summary>
        /// Computes the topocentric place of a solar system body.
        /// </summary>
        /// <param name="JdTt">TT Julian date for topocentric place.</param>
        /// <param name="SsBody">structure containing the body designation for the solar system body</param>
        /// <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        /// <param name="Position">Specifies the position of the observer</param>
        /// <param name="Accuracy">Selection for accuracy</param>
        /// <param name="Ra">Apparent right ascension in hours, referred to true equator and equinox of date.</param>
        /// <param name="Dec">Apparent declination in degrees, referred to true equator and equinox of date.</param>
        /// <param name="Dis">True distance from Earth to planet at 'JdTt' in AU.</param>
        /// <returns><pre>
        /// =  0 ... Everything OK.
        /// =  1 ... Invalid value of 'Where' in structure 'Location'.
        /// > 10 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short TopoPlanet(double JdTt, Object3 SsBody, double DeltaT, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
        {
            if (Is64Bit())
            {
                var argSsBody = O3IFromObject3(SsBody);
                return TopoPlanet64(JdTt, ref argSsBody, DeltaT, ref Position, Accuracy, ref Ra, ref Dec, ref Dis);
            }
            else
            {
                var argSsBody1 = O3IFromObject3(SsBody);
                return TopoPlanet32(JdTt, ref argSsBody1, DeltaT, ref Position, Accuracy, ref Ra, ref Dec, ref Dis);
            }
        }

        /// <summary>
        /// Computes the topocentric place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        /// </summary>
        /// <param name="JdTt">TT Julian date for topocentric place.</param>
        /// <param name="DeltaT">Difference TT-UT1 at 'JdTt', in seconds of time.</param>
        /// <param name="Star">Catalog entry structure containing catalog data for the object in the ICRS</param>
        /// <param name="Position">Specifies the position of the observer</param>
        /// <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        /// <param name="Ra"> Topocentric right ascension in hours, referred to true equator and equinox of date 'JdTt'.</param>
        /// <param name="Dec">Topocentric declination in degrees, referred to true equator and equinox of date 'JdTt'.</param>
        /// <returns><pre>
        /// =  0 ... Everything OK.
        /// =  1 ... Invalid value of 'Where' in structure 'Location'.
        /// > 10 ... Error code from function 'MakeObject'.
        /// > 20 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short TopoStar(double JdTt, double DeltaT, CatEntry3 Star, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec)
        {

            short rc;
            try
            {
                TL.LogMessage("TopoStar", "JD Accuracy:            " + JdTt + " " + Accuracy.ToString());
                TL.LogMessage("TopoStar", "  Star.RA:              " + Utl.HoursToHMS(Star.RA, ":", ":", "", 3));
                TL.LogMessage("TopoStar", "  Dec:                  " + Utl.DegreesToDMS(Star.Dec, ":", ":", "", 3));
                TL.LogMessage("TopoStar", "  Catalog:              " + Star.Catalog);
                TL.LogMessage("TopoStar", "  Parallax:             " + Star.Parallax);
                TL.LogMessage("TopoStar", "  ProMoDec:             " + Star.ProMoDec);
                TL.LogMessage("TopoStar", "  ProMoRA:              " + Star.ProMoRA);
                TL.LogMessage("TopoStar", "  RadialVelocity:       " + Star.RadialVelocity);
                TL.LogMessage("TopoStar", "  StarName:             " + Star.StarName);
                TL.LogMessage("TopoStar", "  StarNumber:           " + Star.StarNumber);
                TL.LogMessage("TopoStar", "  Position.Height:      " + Position.Height);
                TL.LogMessage("TopoStar", "  Position.Latitude:    " + Position.Latitude);
                TL.LogMessage("TopoStar", "  Position.Longitude:   " + Position.Longitude);
                TL.LogMessage("TopoStar", "  Position.Pressure:    " + Position.Pressure);
                TL.LogMessage("TopoStar", "  Position.Temperature: " + Position.Temperature);
                if (Is64Bit())
                {
                    rc = TopoStar64(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
                    TL.LogMessage("TopoStar", "  64bit - Return Code: " + rc + ", RA Dec: " + Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + Utl.DegreesToDMS(Dec, ":", ":", "", 3));
                    return rc;
                }
                else
                {
                    rc = TopoStar32(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
                    TL.LogMessage("TopoStar", "  32bit - Return Code: " + rc + ", RA Dec: " + Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + Utl.DegreesToDMS(Dec, ":", ":", "", 3));
                    return rc;
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("TopoStar", "Exception: " + ex.ToString());
            }

            return default;
        }

        /// <summary>
        /// To transform a star's catalog quantities for a change of epoch and/or equator and equinox.
        /// </summary>
        /// <param name="TransformOption">Transformation option</param>
        /// <param name="DateInCat">TT Julian date, or year, of input catalog data.</param>
        /// <param name="InCat">An entry from the input catalog, with units as given in the struct definition </param>
        /// <param name="DateNewCat">TT Julian date, or year, of transformed catalog data.</param>
        /// <param name="NewCatId">Three-character abbreviated name of the transformed catalog. </param>
        /// <param name="NewCat"> The transformed catalog entry, with units as given in the struct definition </param>
        /// <returns>
        /// <pre>
        /// = 0 ... Everything OK.
        /// = 1 ... Invalid value of an input date for option 2 or 3 (see Note 1 below).
        /// = 2 ... Catalogue ID exceeds three characters
        /// </pre></returns>
        /// <remarks>Also used to rotate catalog quantities on the dynamical equator and equinox of J2000.0 to the ICRS or vice versa.
        /// <para>1. 'DateInCat' and 'DateNewCat' may be specified either as a Julian date (e.g., 2433282.5) or 
        /// a Julian year and fraction (e.g., 1950.0).  Values less than 10000 are assumed to be years. 
        /// For 'TransformOption' = 2 or 'TransformOption' = 3, either 'DateInCat' or 'DateNewCat' must be 2451545.0 or 
        /// 2000.0 (J2000.0).  For 'TransformOption' = 4 and 'TransformOption' = 5, 'DateInCat' and 'DateNewCat' are ignored.</para>
        /// <para>2. 'TransformOption' = 1 updates the star's data to account for the star's space motion between the first 
        /// and second dates, within a fixed reference frame. 'TransformOption' = 2 applies a rotation of the reference 
        /// frame corresponding to precession between the first and second dates, but leaves the star fixed in 
        /// space. 'TransformOption' = 3 provides both transformations. 'TransformOption' = 4 and 'TransformOption' = 5 provide a a 
        /// fixed rotation about very small angles (<![CDATA[<]]>0.1 arcsecond) to take data from the dynamical system 
        /// of J2000.0 to the ICRS ('TransformOption' = 4) or vice versa ('TransformOption' = 5).</para>
        /// <para>3. For 'TransformOption' = 1, input data can be in any fixed reference system. for 'TransformOption' = 2 or 
        /// 'TransformOption' = 3, this function assumes the input data is in the dynamical system and produces output 
        /// in the dynamical system.  for 'TransformOption' = 4, the input data must be on the dynamical equator and 
        /// equinox of J2000.0.  for 'TransformOption' = 5, the input data must be in the ICRS.</para>
        /// <para>4. This function cannot be properly used to bring data from old star catalogs into the 
        /// modern system, because old catalogs were compiled using a set of constants that are incompatible 
        /// with modern values.  In particular, it should not be used for catalogs whose positions and 
        /// proper motions were derived by assuming a precession constant significantly different from 
        /// the value implicit in function 'precession'.</para></remarks>
        public short TransformCat(TransformationOption3 TransformOption, double DateInCat, CatEntry3 InCat, double DateNewCat, string NewCatId, ref CatEntry3 NewCat)
        {
            if (Is64Bit())
            {
                return TransformCat64(TransformOption, DateInCat, ref InCat, DateNewCat, NewCatId, ref NewCat);
            }
            else
            {
                return TransformCat32(TransformOption, DateInCat, ref InCat, DateNewCat, NewCatId, ref NewCat);
            }
        }

        /// <summary>
        /// Convert Hipparcos catalog data at epoch J1991.25 to epoch J2000.0, for use within NOVAS.
        /// </summary>
        /// <param name="Hipparcos">An entry from the Hipparcos catalog, at epoch J1991.25, with all members 
        /// having Hipparcos catalog units.  See Note 1 below </param>
        /// <param name="Hip2000">The transformed input entry, at epoch J2000.0.  See Note 2 below</param>
        /// <remarks>To be used only for Hipparcos or Tycho stars with linear space motion.  Both input and 
        /// output data is in the ICRS.
        /// <para>
        /// 1. Input (Hipparcos catalog) epoch and units:
        /// <list type="bullet">
        /// <item>Epoch: J1991.25</item>
        /// <item>Right ascension (RA): degrees</item>
        /// <item>Declination (Dec): degrees</item>
        /// <item>Proper motion in RA: milliarcseconds per year</item>
        /// <item>Proper motion in Dec: milliarcseconds per year</item>
        /// <item>Parallax: milliarcseconds</item>
        /// <item>Radial velocity: kilometers per second (not in catalog)</item>
        /// </list>
        /// </para>
        /// <para>
        /// 2. Output (modified Hipparcos) epoch and units:
        /// <list type="bullet">
        /// <item>Epoch: J2000.0</item>
        /// <item>Right ascension: hours</item>
        /// <item>Declination: degrees</item>
        /// <item>Proper motion in RA: milliarcseconds per year</item>
        /// <item>Proper motion in Dec: milliarcseconds per year</item>
        /// <item>Parallax: milliarcseconds</item>
        /// <item>Radial velocity: kilometers per second</item>
        /// </list>>
        /// </para>
        /// </remarks>
        public void TransformHip(CatEntry3 Hipparcos, ref CatEntry3 Hip2000)
        {
            if (Is64Bit())
            {
                TransformHip64(ref Hipparcos, ref Hip2000);
            }
            else
            {
                TransformHip32(ref Hipparcos, ref Hip2000);
            }
        }

        /// <summary>
        /// Converts a vector in equatorial rectangular coordinates to equatorial spherical coordinates.
        /// </summary>
        /// <param name="Pos">Position vector, equatorial rectangular coordinates.</param>
        /// <param name="Ra">Right ascension in hours.</param>
        /// <param name="Dec">Declination in degrees.</param>
        /// <returns>
        /// <pre>
        /// = 0 ... Everything OK.
        /// = 1 ... All vector components are zero; 'Ra' and 'Dec' are indeterminate.
        /// = 2 ... Both Pos[0] and Pos[1] are zero, but Pos[2] is nonzero; 'Ra' is indeterminate.
        /// </pre></returns>
        /// <remarks></remarks>
        public short Vector2RaDec(double[] Pos, ref double Ra, ref double Dec)
        {
            if (Is64Bit())
            {
                var argPos = ArrToPosVec(Pos);
                return Vector2RaDec64(ref argPos, ref Ra, ref Dec);
            }
            else
            {
                var argPos1 = ArrToPosVec(Pos);
                return Vector2RaDec32(ref argPos1, ref Ra, ref Dec);
            }
        }

        /// <summary>
        /// Compute the virtual place of a planet or other solar system body.
        /// </summary>
        /// <param name="JdTt">TT Julian date for virtual place.</param>
        /// <param name="SsBody">structure containing the body designation for the solar system body(</param>
        /// <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        /// <param name="Ra">Virtual right ascension in hours, referred to the GCRS.</param>
        /// <param name="Dec">Virtual declination in degrees, referred to the GCRS.</param>
        /// <param name="Dis">True distance from Earth to planet in AU.</param>
        /// <returns>
        /// <pre>
        /// =  0 ... Everything OK.
        /// =  1 ... Invalid value of 'Type' in structure 'SsBody'.
        /// > 10 ... Error code from function 'Place'.
        /// </pre></returns>
        /// <remarks></remarks>
        public short VirtualPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
        {
            if (Is64Bit())
            {
                var argSsBody = O3IFromObject3(SsBody);
                return VirtualPlanet64(JdTt, ref argSsBody, Accuracy, ref Ra, ref Dec, ref Dis);
            }
            else
            {
                var argSsBody1 = O3IFromObject3(SsBody);
                return VirtualPlanet32(JdTt, ref argSsBody1, Accuracy, ref Ra, ref Dec, ref Dis);
            }
        }

        /// <summary>
        /// Computes the virtual place of a star at date 'JdTt', given its catalog mean place, proper motion, parallax, and radial velocity.
        /// </summary>
        /// <param name="JdTt">TT Julian date for virtual place.</param>
        /// <param name="Star">catalog entry structure containing catalog data for the object in the ICRS</param>
        /// <param name="Accuracy">Code specifying the relative accuracy of the output position.</param>
        /// <param name="Ra">Virtual right ascension in hours, referred to the GCRS.</param>
        /// <param name="Dec">Virtual declination in degrees, referred to the GCRS.</param>
        /// <returns>
        /// <pre>
        /// =  0 ... Everything OK.
        /// > 10 ... Error code from function 'MakeObject'.
        /// > 20 ... Error code from function 'Place'
        /// </pre></returns>
        /// <remarks></remarks>
        public short VirtualStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
        {
            if (Is64Bit())
            {
                return VirtualStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
            }
            else
            {
                return VirtualStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
            }
        }

        /// <summary>
        /// Corrects a vector in the ITRF (rotating Earth-fixed system) for polar motion, and also corrects 
        /// the longitude origin (by a tiny amount) to the Terrestrial Intermediate Origin (TIO).
        /// </summary>
        /// <param name="Tjd">TT or UT1 Julian date.</param>
        ///       direction (short int)
        /// <param name="Direction">Flag determining 'direction' of transformation;
        /// direction  = 0 transformation applied, ITRS to terrestrial intermediate system
        /// direction != 0 inverse transformation applied, terrestrial intermediate system to ITRS</param>
        /// <param name="xp">Conventionally-defined X coordinate of Celestial Intermediate Pole with 
        /// respect to ITRF pole, in arcseconds.</param>
        /// <param name="yp">Conventionally-defined Y coordinate of Celestial Intermediate Pole with 
        /// respect to ITRF pole, in arcseconds.</param>
        /// <param name="Pos1">Position vector, geocentric equatorial rectangular coordinates, 
        /// referred to ITRF axes.</param>
        /// <param name="Pos2">Position vector, geocentric equatorial rectangular coordinates, 
        /// referred to true equator and TIO.</param>
        /// <remarks></remarks>
        public void Wobble(double Tjd, TransformationDirection Direction, double xp, double yp, double[] Pos1, ref double[] Pos2)
        {
            var VPos2 = new PosVector();

            if (Is64Bit())
            {
                var argPos1 = ArrToPosVec(Pos1);
                Wobble64(Tjd, (short)Direction, xp, yp, ref argPos1, ref VPos2);
            }
            else
            {
                var argPos11 = ArrToPosVec(Pos1);
                Wobble32(Tjd, (short)Direction, xp, yp, ref argPos11, ref VPos2);
            }

            PosVecToArr(VPos2, ref Pos2);
        }
        #endregion

        #region Private Ephemeris And RACIOFile Routines
        private short Ephem_Open(string Ephem_Name, ref double JD_Begin, ref double JD_End, ref short DENumber)
        {

            short rc;
            if (Is64Bit())
            {
                rc = EphemOpen64(Ephem_Name, ref JD_Begin, ref JD_End, ref DENumber);
            }
            else
            {
                rc = EphemOpen32(Ephem_Name, ref JD_Begin, ref JD_End, ref DENumber);
            }
            return rc;
        }

        private short Ephem_Close()
        {
            if (Is64Bit())
            {
                return EphemClose64();
            }
            else
            {
                return EphemClose32();
            }
        }

        private void SetRACIOFile(string FName)
        {
            if (Is64Bit())
            {
                SetRACIOFile64(FName);
            }
            else
            {
                SetRACIOFile32(FName);
            }
        }
        #endregion

        #region DLL Entry Points for Ephemeris and RACIOFile (32bit)
        [DllImport(NOVAS32DLL, EntryPoint = "set_racio_file")]
        private static extern void SetRACIOFile32([MarshalAs(UnmanagedType.LPStr)] string FName);

        [DllImport(NOVAS32DLL, EntryPoint = "ephem_close")]
        private static extern short EphemClose32();

        [DllImport(NOVAS32DLL, EntryPoint = "ephem_open")]
        private static extern short EphemOpen32([MarshalAs(UnmanagedType.LPStr)] string Ephem_Name, ref double JD_Begin, ref double JD_End, ref short DENumber);

        [DllImport(NOVAS32DLL, EntryPoint = "planet_ephemeris")]
        private static extern short PlanetEphemeris32(ref JDHighPrecision Tjd, Target Target, Target Center, ref PosVector Position, ref VelVector Velocity);

        [DllImport(NOVAS32DLL, EntryPoint = "readeph")]
        private static extern IntPtr ReadEph32(int Mp, [MarshalAs(UnmanagedType.LPStr)] string Name, double Jd, ref int Err);

        [DllImport(NOVAS32DLL, EntryPoint = "cleaneph")]
        private static extern void CleanEph32();

        [DllImport(NOVAS32DLL, EntryPoint = "solarsystem")]
        private static extern short SolarSystem32(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);

        [DllImport(NOVAS32DLL, EntryPoint = "state")]
        private static extern short State32(ref JDHighPrecision Jed, Target Target, ref PosVector TargetPos, ref VelVector TargetVel);
        #endregion

        #region DLL Entry Points NOVAS (32bit)
        [DllImport(NOVAS32DLL, EntryPoint = "aberration")]
        private static extern void Aberration32(ref PosVector Pos, ref VelVector Vel, double LightTime, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "app_planet")]
        private static extern short AppPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS32DLL, EntryPoint = "app_star")]
        private static extern short AppStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS32DLL, EntryPoint = "astro_planet")]
        private static extern short AstroPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS32DLL, EntryPoint = "astro_star")]
        private static extern short AstroStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS32DLL, EntryPoint = "bary2obs")]
        private static extern void Bary2Obs32(ref PosVector Pos, ref PosVector PosObs, ref PosVector Pos2, ref double Lighttime);

        [DllImport(NOVAS32DLL, EntryPoint = "cal_date")]
        private static extern void CalDate32(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour);

        [DllImport(NOVAS32DLL, EntryPoint = "cel2ter")]
        private static extern short Cel2Ter32(double JdHigh, double JdLow, double DeltaT, Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

        [DllImport(NOVAS32DLL, EntryPoint = "cel_pole")]
        private static extern short CelPole32(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2);

        [DllImport(NOVAS32DLL, EntryPoint = "cio_array")]
        private static extern short CioArray32(double JdTdb, int NPts, ref RAOfCioArray Cio);

        [DllImport(NOVAS32DLL, EntryPoint = "cio_basis")]
        private static extern short CioBasis32(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z);

        [DllImport(NOVAS32DLL, EntryPoint = "cio_location")]
        private static extern short CioLocation32(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys);

        [DllImport(NOVAS32DLL, EntryPoint = "cio_ra")]
        private static extern short CioRa32(double JdTt, Accuracy Accuracy, ref double RaCio);

        [DllImport(NOVAS32DLL, EntryPoint = "d_light")]
        private static extern double DLight32(ref PosVector Pos1, ref PosVector PosObs);

        [DllImport(NOVAS32DLL, EntryPoint = "e_tilt")]
        private static extern void ETilt32(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps);

        [DllImport(NOVAS32DLL, EntryPoint = "ecl2equ_vec")]
        private static extern short Ecl2EquVec32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "ee_ct")]
        private static extern double EeCt32(double JdHigh, double JdLow, Accuracy Accuracy);

        [DllImport(NOVAS32DLL, EntryPoint = "ephemeris")]
        private static extern short Ephemeris32(ref JDHighPrecision Jd, ref Object3Internal CelObj, Origin Origin, Accuracy Accuracy, ref PosVector Pos, ref VelVector Vel);

        [DllImport(NOVAS32DLL, EntryPoint = "equ2ecl")]
        private static extern short Equ2Ecl32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat);

        [DllImport(NOVAS32DLL, EntryPoint = "equ2ecl_vec")]
        private static extern short Equ2EclVec32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "equ2gal")]
        private static extern void Equ2Gal32(double RaI, double DecI, ref double GLon, ref double GLat);

        [DllImport(NOVAS32DLL, EntryPoint = "equ2hor")]
        private static extern void Equ2Hor32(double Jd_Ut1, double DeltT, Accuracy Accuracy, double x, double y, ref OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR);

        [DllImport(NOVAS32DLL, EntryPoint = "era")]
        private static extern double Era32(double JdHigh, double JdLow);

        [DllImport(NOVAS32DLL, EntryPoint = "frame_tie")]
        private static extern void FrameTie32(ref PosVector Pos1, FrameConversionDirection Direction, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "fund_args")]
        private static extern void FundArgs32(double t, ref FundamentalArgs a);

        [DllImport(NOVAS32DLL, EntryPoint = "gcrs2equ")]
        private static extern short Gcrs2Equ32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec);

        [DllImport(NOVAS32DLL, EntryPoint = "geo_posvel")]
        private static extern short GeoPosVel32(double JdTt, double DeltaT, Accuracy Accuracy, ref Observer Obs, ref PosVector Pos, ref VelVector Vel);

        [DllImport(NOVAS32DLL, EntryPoint = "grav_def")]
        private static extern short GravDef32(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, ref PosVector Pos1, ref PosVector PosObs, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "grav_vec")]
        private static extern void GravVec32(ref PosVector Pos1, ref PosVector PosObs, ref PosVector PosBody, double RMass, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "ira_equinox")]
        private static extern double IraEquinox32(double JdTdb, EquinoxType Equinox, Accuracy Accuracy);

        [DllImport(NOVAS32DLL, EntryPoint = "julian_date")]
        private static extern double JulianDate32(short Year, short Month, short Day, double Hour);

        [DllImport(NOVAS32DLL, EntryPoint = "light_time")]
        private static extern short LightTime32(double JdTdb, ref Object3Internal SsObject, ref PosVector PosObs, double TLight0, Accuracy Accuracy, ref PosVector Pos, ref double TLight);

        [DllImport(NOVAS32DLL, EntryPoint = "limb_angle")]
        private static extern void LimbAngle32(ref PosVector PosObj, ref PosVector PosObs, ref double LimbAng, ref double NadirAng);

        [DllImport(NOVAS32DLL, EntryPoint = "local_planet")]
        private static extern short LocalPlanet32(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS32DLL, EntryPoint = "local_star")]
        private static extern short LocalStar32(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS32DLL, EntryPoint = "make_cat_entry")]
        private static extern void MakeCatEntry32([MarshalAs(UnmanagedType.LPStr)] string StarName, [MarshalAs(UnmanagedType.LPStr)] string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star);

        [DllImport(NOVAS32DLL, EntryPoint = "make_in_space")]
        private static extern void MakeInSpace32(ref PosVector ScPos, ref VelVector ScVel, ref InSpace ObsSpace);

        [DllImport(NOVAS32DLL, EntryPoint = "make_object")]
        private static extern short MakeObject32(ObjectType Type, short Number, [MarshalAs(UnmanagedType.LPStr)] string Name, ref CatEntry3 StarData, ref Object3Internal CelObj);

        [DllImport(NOVAS32DLL, EntryPoint = "make_observer")]
        private static extern short MakeObserver32(ObserverLocation Where, ref OnSurface ObsSurface, ref InSpace ObsSpace, ref Observer Obs);

        [DllImport(NOVAS32DLL, EntryPoint = "make_observer_at_geocenter")]
        private static extern void MakeObserverAtGeocenter32(ref Observer ObsAtGeocenter);

        [DllImport(NOVAS32DLL, EntryPoint = "make_observer_in_space")]
        private static extern void MakeObserverInSpace32(ref PosVector ScPos, ref VelVector ScVel, ref Observer ObsInSpace);

        [DllImport(NOVAS32DLL, EntryPoint = "make_observer_on_surface")]
        private static extern void MakeObserverOnSurface32(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface);

        [DllImport(NOVAS32DLL, EntryPoint = "make_on_surface")]
        private static extern void MakeOnSurface32(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface);

        [DllImport(NOVAS32DLL, EntryPoint = "mean_obliq")]
        private static extern double MeanObliq32(double JdTdb);

        [DllImport(NOVAS32DLL, EntryPoint = "mean_star")]
        private static extern short MeanStar32(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec);

        [DllImport(NOVAS32DLL, EntryPoint = "norm_ang")]
        private static extern double NormAng32(double Angle);

        [DllImport(NOVAS32DLL, EntryPoint = "nutation")]
        private static extern void Nutation32(double JdTdb, NutationDirection Direction, Accuracy Accuracy, ref PosVector Pos, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "nutation_angles")]
        private static extern void NutationAngles32(double t, Accuracy Accuracy, ref double DPsi, ref double DEps);

        [DllImport(NOVAS32DLL, EntryPoint = "place")]
        private static extern short Place32(double JdTt, ref Object3Internal CelObject, ref Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output);

        [DllImport(NOVAS32DLL, EntryPoint = "precession")]
        private static extern short Precession32(double JdTdb1, ref PosVector Pos1, double JdTdb2, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "proper_motion")]
        private static extern void ProperMotion32(double JdTdb1, ref PosVector Pos, ref VelVector Vel, double JdTdb2, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "rad_vel")]
        private static extern void RadVel32(ref Object3Internal CelObject, ref PosVector Pos, ref VelVector Vel, ref VelVector VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv);

        [DllImport(NOVAS32DLL, EntryPoint = "radec2vector")]
        private static extern void RaDec2Vector32(double Ra, double Dec, double Dist, ref PosVector Vector);

        [DllImport(NOVAS32DLL, EntryPoint = "refract")]
        private static extern double Refract32(ref OnSurface Location, RefractionOption RefOption, double ZdObs);

        [DllImport(NOVAS32DLL, EntryPoint = "sidereal_time")]
        private static extern short SiderealTime32(double JdHigh, double JdLow, double DeltaT, GstType GstType, Method Method, Accuracy Accuracy, ref double Gst);

        [DllImport(NOVAS32DLL, EntryPoint = "spin")]
        private static extern void Spin32(double Angle, ref PosVector Pos1, ref PosVector Pos2);

        [DllImport(NOVAS32DLL, EntryPoint = "starvectors")]
        private static extern void StarVectors32(ref CatEntry3 Star, ref PosVector Pos, ref VelVector Vel);

        [DllImport(NOVAS32DLL, EntryPoint = "tdb2tt")]
        private static extern void Tdb2Tt32(double TdbJd, ref double TtJd, ref double SecDiff);

        [DllImport(NOVAS32DLL, EntryPoint = "ter2cel")]
        private static extern short Ter2Cel32(double JdHigh, double JdLow, double DeltaT, Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

        [DllImport(NOVAS32DLL, EntryPoint = "terra")]
        private static extern void Terra32(ref OnSurface Location, double St, ref PosVector Pos, ref VelVector Vel);

        [DllImport(NOVAS32DLL, EntryPoint = "topo_planet")]
        private static extern short TopoPlanet32(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS32DLL, EntryPoint = "topo_star")]
        private static extern short TopoStar32(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS32DLL, EntryPoint = "transform_cat")]

        private static extern short TransformCat32(TransformationOption3 TransformOption, double DateInCat, ref CatEntry3 InCat, double DateNewCat, [MarshalAs(UnmanagedType.LPStr)] string NewCatId, ref CatEntry3 NewCat);

        [DllImport(NOVAS32DLL, EntryPoint = "transform_hip")]
        private static extern void TransformHip32(ref CatEntry3 Hipparcos, ref CatEntry3 Hip2000);

        [DllImport(NOVAS32DLL, EntryPoint = "vector2radec")]
        private static extern short Vector2RaDec32(ref PosVector Pos, ref double Ra, ref double Dec);

        [DllImport(NOVAS32DLL, EntryPoint = "virtual_planet")]
        private static extern short VirtualPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS32DLL, EntryPoint = "virtual_star")]
        private static extern short VirtualStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS32DLL, EntryPoint = "wobble")]
        private static extern void Wobble32(double Tjd, short Direction, double x, double y, ref PosVector Pos1, ref PosVector Pos2);
        #endregion

        #region DLL Entry Points for Ephemeris and RACIOFile (64bit)
        [DllImport(NOVAS64DLL, EntryPoint = "set_racio_file")]
        private static extern void SetRACIOFile64([MarshalAs(UnmanagedType.LPStr)] string Name);

        [DllImport(NOVAS64DLL, EntryPoint = "ephem_close")]
        private static extern short EphemClose64();

        [DllImport(NOVAS64DLL, EntryPoint = "ephem_open")]
        private static extern short EphemOpen64([MarshalAs(UnmanagedType.LPStr)] string Ephem_Name, ref double JD_Begin, ref double JD_End, ref short DENumber);

        [DllImport(NOVAS64DLL, EntryPoint = "planet_ephemeris")]
        private static extern short PlanetEphemeris64(ref JDHighPrecision Tjd, Target Target, Target Center, ref PosVector Position, ref VelVector Velocity);

        [DllImport(NOVAS64DLL, EntryPoint = "readeph")]
        private static extern IntPtr ReadEph64(int Mp, [MarshalAs(UnmanagedType.LPStr)] string Name, double Jd, ref int Err);

        [DllImport(NOVAS64DLL, EntryPoint = "cleaneph")]
        private static extern void CleanEph64();

        [DllImport(NOVAS64DLL, EntryPoint = "solarsystem")]
        private static extern short SolarSystem64(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);

        [DllImport(NOVAS64DLL, EntryPoint = "state")]
        private static extern short State64(ref JDHighPrecision Jed, Target Target, ref PosVector TargetPos, ref VelVector TargetVel);
        #endregion

        #region DLL Entry Points NOVAS (64bit)
        [DllImport(NOVAS64DLL, EntryPoint = "aberration")]
        private static extern void Aberration64(ref PosVector Pos, ref VelVector Vel, double LightTime, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "app_planet")]
        private static extern short AppPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS64DLL, EntryPoint = "app_star")]
        private static extern short AppStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS64DLL, EntryPoint = "astro_planet")]
        private static extern short AstroPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS64DLL, EntryPoint = "astro_star")]
        private static extern short AstroStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS64DLL, EntryPoint = "bary2obs")]
        private static extern void Bary2Obs64(ref PosVector Pos, ref PosVector PosObs, ref PosVector Pos2, ref double Lighttime);

        [DllImport(NOVAS64DLL, EntryPoint = "cal_date")]
        private static extern void CalDate64(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour);

        [DllImport(NOVAS64DLL, EntryPoint = "cel2ter")]
        private static extern short Cel2Ter64(double JdHigh, double JdLow, double DeltaT, Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

        [DllImport(NOVAS64DLL, EntryPoint = "cel_pole")]
        private static extern short CelPole64(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2);

        [DllImport(NOVAS64DLL, EntryPoint = "cio_array")]
        private static extern short CioArray64(double JdTdb, int NPts, ref RAOfCioArray Cio);

        [DllImport(NOVAS64DLL, EntryPoint = "cio_basis")]
        private static extern short CioBasis64(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z);

        [DllImport(NOVAS64DLL, EntryPoint = "cio_location")]
        private static extern short CioLocation64(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys);

        [DllImport(NOVAS64DLL, EntryPoint = "cio_ra")]
        private static extern short CioRa64(double JdTt, Accuracy Accuracy, ref double RaCio);

        [DllImport(NOVAS64DLL, EntryPoint = "d_light")]
        private static extern double DLight64(ref PosVector Pos1, ref PosVector PosObs);

        [DllImport(NOVAS64DLL, EntryPoint = "e_tilt")]
        private static extern void ETilt64(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps);

        [DllImport(NOVAS64DLL, EntryPoint = "ecl2equ_vec")]
        private static extern short Ecl2EquVec64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "ee_ct")]
        private static extern double EeCt64(double JdHigh, double JdLow, Accuracy Accuracy);

        [DllImport(NOVAS64DLL, EntryPoint = "ephemeris")]
        private static extern short Ephemeris64(ref JDHighPrecision Jd, ref Object3Internal CelObj, Origin Origin, Accuracy Accuracy, ref PosVector Pos, ref VelVector Vel);

        [DllImport(NOVAS64DLL, EntryPoint = "equ2ecl")]
        private static extern short Equ2Ecl64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat);

        [DllImport(NOVAS64DLL, EntryPoint = "equ2ecl_vec")]
        private static extern short Equ2EclVec64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "equ2gal")]
        private static extern void Equ2Gal64(double RaI, double DecI, ref double GLon, ref double GLat);

        [DllImport(NOVAS64DLL, EntryPoint = "equ2hor")]
        private static extern void Equ2Hor64(double Jd_Ut1, double DeltT, Accuracy Accuracy, double x, double y, ref OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR);

        [DllImport(NOVAS64DLL, EntryPoint = "era")]
        private static extern double Era64(double JdHigh, double JdLow);

        [DllImport(NOVAS64DLL, EntryPoint = "frame_tie")]
        private static extern void FrameTie64(ref PosVector Pos1, FrameConversionDirection Direction, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "fund_args")]
        private static extern void FundArgs64(double t, ref FundamentalArgs a);

        [DllImport(NOVAS64DLL, EntryPoint = "gcrs2equ")]
        private static extern short Gcrs2Equ64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec);

        [DllImport(NOVAS64DLL, EntryPoint = "geo_posvel")]
        private static extern short GeoPosVel64(double JdTt, double DeltaT, Accuracy Accuracy, ref Observer Obs, ref PosVector Pos, ref VelVector Vel);

        [DllImport(NOVAS64DLL, EntryPoint = "grav_def")]
        private static extern short GravDef64(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, ref PosVector Pos1, ref PosVector PosObs, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "grav_vec")]
        private static extern void GravVec64(ref PosVector Pos1, ref PosVector PosObs, ref PosVector PosBody, double RMass, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "ira_equinox")]
        private static extern double IraEquinox64(double JdTdb, EquinoxType Equinox, Accuracy Accuracy);

        [DllImport(NOVAS64DLL, EntryPoint = "julian_date")]
        private static extern double JulianDate64(short Year, short Month, short Day, double Hour);

        [DllImport(NOVAS64DLL, EntryPoint = "light_time")]
        private static extern short LightTime64(double JdTdb, ref Object3Internal SsObject, ref PosVector PosObs, double TLight0, Accuracy Accuracy, ref PosVector Pos, ref double TLight);

        [DllImport(NOVAS64DLL, EntryPoint = "limb_angle")]
        private static extern void LimbAngle64(ref PosVector PosObj, ref PosVector PosObs, ref double LimbAng, ref double NadirAng);

        [DllImport(NOVAS64DLL, EntryPoint = "local_planet")]
        private static extern short LocalPlanet64(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS64DLL, EntryPoint = "local_star")]
        private static extern short LocalStar64(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS64DLL, EntryPoint = "make_cat_entry")]
        private static extern void MakeCatEntry64([MarshalAs(UnmanagedType.LPStr)] string StarName, [MarshalAs(UnmanagedType.LPStr)] string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star);

        [DllImport(NOVAS64DLL, EntryPoint = "make_in_space")]
        private static extern void MakeInSpace64(ref PosVector ScPos, ref VelVector ScVel, ref InSpace ObsSpace);

        [DllImport(NOVAS64DLL, EntryPoint = "make_object")]
        private static extern short MakeObject64(ObjectType Type, short Number, [MarshalAs(UnmanagedType.LPStr)] string Name, ref CatEntry3 StarData, ref Object3Internal CelObj);

        [DllImport(NOVAS64DLL, EntryPoint = "make_observer")]
        private static extern short MakeObserver64(ObserverLocation Where, ref OnSurface ObsSurface, ref InSpace ObsSpace, ref Observer Obs);

        [DllImport(NOVAS64DLL, EntryPoint = "make_observer_at_geocenter")]
        private static extern void MakeObserverAtGeocenter64(ref Observer ObsAtGeocenter);

        [DllImport(NOVAS64DLL, EntryPoint = "make_observer_in_space")]
        private static extern void MakeObserverInSpace64(ref PosVector ScPos, ref VelVector ScVel, ref Observer ObsInSpace);

        [DllImport(NOVAS64DLL, EntryPoint = "make_observer_on_surface")]
        private static extern void MakeObserverOnSurface64(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface);

        [DllImport(NOVAS64DLL, EntryPoint = "make_on_surface")]
        private static extern void MakeOnSurface64(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface);

        [DllImport(NOVAS64DLL, EntryPoint = "mean_obliq")]
        private static extern double MeanObliq64(double JdTdb);

        [DllImport(NOVAS64DLL, EntryPoint = "mean_star")]
        private static extern short MeanStar64(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec);

        [DllImport(NOVAS64DLL, EntryPoint = "norm_ang")]
        private static extern double NormAng64(double Angle);

        [DllImport(NOVAS64DLL, EntryPoint = "nutation")]
        private static extern void Nutation64(double JdTdb, NutationDirection Direction, Accuracy Accuracy, ref PosVector Pos, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "nutation_angles")]
        private static extern void NutationAngles64(double t, Accuracy Accuracy, ref double DPsi, ref double DEps);

        [DllImport(NOVAS64DLL, EntryPoint = "place")]
        private static extern short Place64(double JdTt, ref Object3Internal CelObject, ref Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output);

        [DllImport(NOVAS64DLL, EntryPoint = "precession")]
        private static extern short Precession64(double JdTdb1, ref PosVector Pos1, double JdTdb2, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "proper_motion")]
        private static extern void ProperMotion64(double JdTdb1, ref PosVector Pos, ref VelVector Vel, double JdTdb2, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "rad_vel")]
        private static extern void RadVel64(ref Object3Internal CelObject, ref PosVector Pos, ref VelVector Vel, ref VelVector VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv);

        [DllImport(NOVAS64DLL, EntryPoint = "radec2vector")]
        private static extern void RaDec2Vector64(double Ra, double Dec, double Dist, ref PosVector Vector);

        [DllImport(NOVAS64DLL, EntryPoint = "refract")]
        private static extern double Refract64(ref OnSurface Location, RefractionOption RefOption, double ZdObs);

        [DllImport(NOVAS64DLL, EntryPoint = "sidereal_time")]
        private static extern short SiderealTime64(double JdHigh, double JdLow, double DeltaT, GstType GstType, Method Method, Accuracy Accuracy, ref double Gst);

        [DllImport(NOVAS64DLL, EntryPoint = "spin")]
        private static extern void Spin64(double Angle, ref PosVector Pos1, ref PosVector Pos2);

        [DllImport(NOVAS64DLL, EntryPoint = "starvectors")]
        private static extern void StarVectors64(ref CatEntry3 Star, ref PosVector Pos, ref VelVector Vel);

        [DllImport(NOVAS64DLL, EntryPoint = "tdb2tt")]
        private static extern void Tdb2Tt64(double TdbJd, ref double TtJd, ref double SecDiff);

        [DllImport(NOVAS64DLL, EntryPoint = "ter2cel")]
        private static extern short Ter2Cel64(double JdHigh, double JdLow, double DeltaT, Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

        [DllImport(NOVAS64DLL, EntryPoint = "terra")]
        private static extern void Terra64(ref OnSurface Location, double St, ref PosVector Pos, ref VelVector Vel);

        [DllImport(NOVAS64DLL, EntryPoint = "topo_planet")]
        private static extern short TopoPlanet64(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS64DLL, EntryPoint = "topo_star")]
        private static extern short TopoStar64(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS64DLL, EntryPoint = "transform_cat")]

        private static extern short TransformCat64(TransformationOption3 TransformOption, double DateInCat, ref CatEntry3 InCat, double DateNewCat, [MarshalAs(UnmanagedType.LPStr)] string NewCatId, ref CatEntry3 NewCat);

        [DllImport(NOVAS64DLL, EntryPoint = "transform_hip")]
        private static extern void TransformHip64(ref CatEntry3 Hipparcos, ref CatEntry3 Hip2000);

        [DllImport(NOVAS64DLL, EntryPoint = "vector2radec")]
        private static extern short Vector2RaDec64(ref PosVector Pos, ref double Ra, ref double Dec);

        [DllImport(NOVAS64DLL, EntryPoint = "virtual_planet")]
        private static extern short VirtualPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

        [DllImport(NOVAS64DLL, EntryPoint = "virtual_star")]
        private static extern short VirtualStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

        [DllImport(NOVAS64DLL, EntryPoint = "wobble")]
        private static extern void Wobble64(double Tjd, short Direction, double x, double y, ref PosVector Pos1, ref PosVector Pos2);
        #endregion

        #region Private Support Code
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

        /// <summary>
        /// Loads a library DLL
        /// </summary>
        /// <param name="lpFileName">Full path to the file to load</param>
        /// <returns>A pointer to the loaded DLL image</returns>
        /// <remarks>This is a wrapper for the Windows kernel32 function LoadLibraryA</remarks>
        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "LoadLibraryA")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// Unloads a library DLL
        /// </summary>
        /// <param name="hModule">Pointer to the loaded library returned by the LoadLibrary function.</param>
        /// <returns>True or false depending on whether the library was released.</returns>
        /// <remarks></remarks>
        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "FreeLibrary")]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static bool Is64Bit()
        {
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

        private static void RACioArrayStructureToArr(RAOfCioArray C, ref ArrayList Ar)
        {
            // Transfer all RACio values that have actually been set by the NOVAS DLL to the arraylist for return to the client
            if (C.Value1.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value1);
            if (C.Value2.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value2);
            if (C.Value3.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value3);
            if (C.Value4.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value4);
            if (C.Value5.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value5);
            if (C.Value6.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value6);
            if (C.Value7.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value7);
            if (C.Value8.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value8);
            if (C.Value9.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value9);
            if (C.Value10.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value10);
            if (C.Value11.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value11);
            if (C.Value12.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value12);
            if (C.Value13.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value13);
            if (C.Value14.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value14);
            if (C.Value15.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value15);
            if (C.Value16.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value16);
            if (C.Value17.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value17);
            if (C.Value18.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value18);
            if (C.Value19.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value19);
            if (C.Value20.RACio != GlobalItems.RACIO_DEFAULT_VALUE)
                Ar.Add(C.Value20);
        }

        private void O3FromO3Internal(Object3Internal O3I, ref Object3 O3)
        {
            O3.Name = O3I.Name;
            O3.Number = (Body)O3I.Number;
            O3.Star = O3I.Star;
            O3.Type = O3I.Type;
        }

        private Object3Internal O3IFromObject3(Object3 O3)
        {
            var O3I = new Object3Internal();
            O3I.Name = O3.Name;
            O3I.Number = (short)O3.Number;
            O3I.Star = O3.Star;
            O3I.Type = O3.Type;
            return O3I;
        }
        #endregion

        #region DeltaT Member
        /// <summary>
        /// Return the value of DeltaT for the given Julian date
        /// </summary>
        /// <param name="Tjd">Julian date for which the delta T value is required</param>
        /// <returns>Double value of DeltaT (seconds)</returns>
        /// <remarks>Valid between the years 1650 and 2050</remarks>
        public double DeltaT(double Tjd)
        {
            return Parameters.DeltaT(Tjd);
        }
        #endregion

    }

}