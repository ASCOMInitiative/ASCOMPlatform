using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USNO
	{

	public partial class NovasConstants
		{

		/// BARYC -> 0
		public const int BARYC = 0;

		/// HELIOC -> 1
		public const int HELIOC = 1;
		}

//typedef struct
//   {
//      short int type;
//      short int number;
//      char name[100];
//   } body;

	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public struct body
		{
		/// short
		public short type;
		/// <summary>
		///  short
		/// </summary>
		public short number;
		/// char[51]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 100)]
		public string starname;
		}


	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public struct site_info
		{

		/// double
		public double latitude;

		/// double
		public double longitude;

		/// double
		public double height;

		/// double
		public double temperature;

		/// double
		public double pressure;
		}

	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
	public struct cat_entry
		{

		/// char[4]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 4)]
		public string catalog;

		/// char[51]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 51)]
		public string starname;

		/// int
		public int starnumber;

		/// double
		public double ra;

		/// double
		public double dec;

		/// double
		public double promora;

		/// double
		public double promodec;

		/// double
		public double parallax;

		/// double
		public double radialvelocity;
		}

	public partial class Novas
		{

		/// Return Type: void
		///julianhi: double
		///julianlo: double
		///ee: double
		///gst: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "sidereal_time")]
		public static extern void sidereal_time(double julianhi, double julianlo, double ee, ref double gst);


		/// Return Type: void
		///tjd: double
		///gast: double
		///x: double
		///y: double
		///vece: double*
		///vecs: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "pnsw")]
		public static extern void pnsw(double tjd, double gast, double x, double y, ref double vece, ref double vecs);


		/// Return Type: void
		///st: double
		///pos1: double*
		///pos2: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "spin")]
		public static extern void spin(double st, ref double pos1, ref double pos2);


		/// Return Type: void
		///x: double
		///y: double
		///pos1: double*
		///pos2: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "wobble")]
		public static extern void wobble(double x, double y, ref double pos1, ref double pos2);


		/// Return Type: void
		///locale: site_info*
		///st: double
		///pos: double*
		///vel: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "terra")]
		public static extern void terra(ref site_info locale, double st, ref double pos, ref double vel);


		/// Return Type: void
		///tjd: double
		///mobl: double*
		///tobl: double*
		///eqeq: double*
		///psi: double*
		///eps: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "earthtilt")]
		public static extern void earthtilt(double tjd, ref double mobl, ref double tobl, ref double eqeq, ref double psi, ref double eps);


		/// Return Type: void
		///del_dpsi: double
		///del_deps: double
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cel_pole")]
		public static extern void cel_pole(double del_dpsi, double del_deps);


		/// Return Type: void
		///tjd1: double
		///pos1: double*
		///vel1: double*
		///tjd2: double
		///pos2: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "proper_motion")]
		public static extern void proper_motion(double tjd1, ref double pos1, ref double vel1, double tjd2, ref double pos2);


		/// Return Type: void
		///pos: double*
		///earthvector: double*
		///pos2: double*
		///lighttime: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "bary_to_geo")]
		public static extern void bary_to_geo(ref double pos, ref double earthvector, ref double pos2, ref double lighttime);


		/// Return Type: int
		///pos: double*
		///earthvector: double*
		///pos2: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "sun_field")]
		public static extern int sun_field(ref double pos, ref double earthvector, ref double pos2);


		/// Return Type: int
		///pos: double*
		///vel: double*
		///lighttime: double
		///pos2: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "aberration")]
		public static extern int aberration(ref double pos, ref double vel, double lighttime, ref double pos2);


		/// Return Type: void
		///tjd1: double
		///pos: double*
		///tjd2: double
		///pos2: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "precession")]
		public static extern void precession(double tjd1, ref double pos, double tjd2, ref double pos2);


		/// Return Type: int
		///tjd: double
		///param1: short
		///fn1: int
		///pos: double*
		///pos2: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "nutate")]
		public static extern int nutate(double tjd, short param1, int fn1, ref double pos, ref double pos2);


		/// Return Type: int
		///tdbtime: double
		///longnutation: double*
		///obliqnutation: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "nutation_angles")]
		public static extern int nutation_angles(double tdbtime, ref double longnutation, ref double obliqnutation);


		/// Return Type: void
		///t: double
		///a: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "fund_args")]
		public static extern void fund_args(double t, ref double a);


		/// Return Type: int
		///pos: double*
		///ra: double*
		///dec: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "vector2radec")]
		public static extern int vector2radec(ref double pos, ref double ra, ref double dec);


		/// Return Type: void
		///ra: double
		///dec: double
		///dist: double
		///vector: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "radec2vector")]
		public static extern void radec2vector(double ra, double dec, double dist, ref double vector);


		/// Return Type: void
		///star: cat_entry*
		///pos: double*
		///vel: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "starvectors")]
		public static extern void starvectors(ref cat_entry star, ref double pos, ref double vel);


		/// Return Type: void
		///tdb: double
		///tdtjd: double*
		///secdiff: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "tdb2tdt")]
		public static extern void tdb2tdt(double tdb, ref double tdtjd, ref double secdiff);


		/// Return Type: int
		///tjd: double
		///param1: short
		///body: int
		///param3: short
		///origin: int
		///pos: double*
		///vel: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "solarsystem")]
		public static extern int solarsystem(double tjd, short param1, int body, short param3, int origin, ref double pos, ref double vel);


		/// Return Type: double*
		///mp: int
		///name: char*
		///jd: double
		///err: int*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "readeph")]
		public static extern System.IntPtr readeph(int mp, System.IntPtr name, double jd, ref int err);


		/// Return Type: void
		///catalog: char*
		///star_name: char*
		///star_num: int
		///ra: double
		///dec: double
		///pm_ra: double
		///pm_dec: double
		///parallax: double
		///rad_vel: double
		///star: cat_entry*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "make_cat_entry")]
		public static extern void make_cat_entry(System.IntPtr catalog, System.IntPtr star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref cat_entry star);


		/// Return Type: void
		///hipparcos: cat_entry*
		///fk5: cat_entry*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "transform_hip")]
		public static extern void transform_hip(ref cat_entry hipparcos, ref cat_entry fk5);


		/// Return Type: void
		///param0: short
		///option: int
		///date_incat: double
		///incat: cat_entry*
		///date_newcat: double
		///newcat_id: char*
		///newcat: cat_entry*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "transform_cat")]
		public static extern void transform_cat(short param0, int option, double date_incat, ref cat_entry incat, double date_newcat, System.IntPtr newcat_id, ref cat_entry newcat);


		/// Return Type: void
		///tjd: double
		///deltat: double
		///x: double
		///y: double
		///location: site_info*
		///ra: double
		///dec: double
		///param7: short
		///ref_option: int
		///zd: double*
		///az: double*
		///rar: double*
		///decr: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "equ2hor")]
		public static extern void equ2hor(double tjd, double deltat, double x, double y, ref site_info location, double ra, double dec, short param7, int ref_option, ref double zd, ref double az, ref double rar, ref double decr);


		/// Return Type: double
		///location: site_info*
		///param1: short
		///ref_option: int
		///zd_obs: double
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "refract")]
		public static extern double refract(ref site_info location, short param1, int ref_option, double zd_obs);


		/// Return Type: double
		///param0: short
		///year: int
		///param2: short
		///month: int
		///param4: short
		///day: int
		///hour: double
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "julian_date")]
		public static extern double julian_date(short param0, int year, short param2, int month, short param4, int day, double hour);


		/// Return Type: void
		///tjd: double
		///param1: short
		///year: int*
		///param3: short
		///month: int*
		///param5: short
		///day: int*
		///hour: double*
		[System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cal_date")]
		public static extern void cal_date(double tjd, short param1, ref int year, short param3, ref int month, short param5, ref int day, ref double hour);

		}

	}
