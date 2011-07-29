using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.DriverAccess;

namespace ASCOM
{
	class ClientTest
	{
		static void Main(string[] args)
		{
			string progID;
            
			Util U = new Util();

			#region Focuser
			Console.WriteLine("\r\nFocuser:");
			progID = Focuser.Choose("ASCOM.Simulator.Focuser");		// Pre-select simulator (typ.)
			if (progID != "")
            {
			    Focuser F = new Focuser(progID);
			    F.Link = true;
			    Console.WriteLine("  Connected to " + progID);
			    Console.WriteLine("  Current position is " + F.Position);
				int nfp = (int)(0.7 * F.Position);
				Console.Write("  Moving to " + nfp);
				F.Move(nfp);
			    while (F.IsMoving)
			    {
			        Console.Write(".");
			        U.WaitForMilliseconds(333);
			    }
			    Console.WriteLine("\r\n  Move complete. New position is " + F.Position.ToString());
			    F.Link = false;
				F.Dispose();									// Release this now, not at exit (typ.)
			}
			#endregion

			#region Telescope
			Console.WriteLine("\r\nTelescope:");
			progID = Telescope.Choose("ASCOM.Simulator.Telescope");
            if (progID != "")
            {
                Telescope T = new Telescope(progID);
                T.Connected = true;
                Console.WriteLine("  Connected to " + progID);
                Console.WriteLine("  Current LST = " + U.HoursToHMS(T.SiderealTime));
				Console.WriteLine("  Current RA  = " + U.HoursToHMS(T.RightAscension));
				Console.WriteLine("  Current DEC = " + U.DegreesToDMS(T.Declination));
				Console.WriteLine("  CanSetTracking = " + T.CanSetTracking);
				if (T.CanSetTracking)
				{
					Console.WriteLine("  Turning Tracking off...");
					T.Tracking = false;
					Console.WriteLine("  Tracking is now " + (T.Tracking ? "on" : "off") + ".");
					Console.WriteLine("  Wait 5 seconds...");
					U.WaitForMilliseconds(5000);
					Console.WriteLine("  Turning Tracking back on...");
					T.Tracking = true;
				}
				Console.WriteLine("  Latitude = " + U.DegreesToDMS(T.SiteLatitude));
				Console.WriteLine("  Longitude = " + U.DegreesToDMS(T.SiteLongitude));
				Console.Write("  Slewing to point 1");
				T.SlewToCoordinatesAsync(T.SiderealTime - 2, (T.SiteLatitude > 0 ? +55 : -55));
				while (T.Slewing)
				{
					Console.Write(".");
					U.WaitForMilliseconds(300);
				}
				Console.WriteLine("\r\n  Slew complete.");
				Console.Write("  Slewing to point 2");
				T.SlewToCoordinatesAsync(T.SiderealTime + 2, (T.SiteLatitude > 0 ? +35 : -35));
				while (T.Slewing)
				{
					Console.Write(".");
					U.WaitForMilliseconds(300);
				}
				Console.WriteLine("\r\n  Slew complete.");
                IAxisRates AxR = T.AxisRates(TelescopeAxes.axisPrimary);
                Console.WriteLine("  " + AxR.Count + " rates");
                if (AxR.Count == 0)
                    Console.WriteLine("  Empty AxisRates");
                else
                    foreach (IRate r in AxR)
                        Console.WriteLine("  Max=" + r.Maximum + " Min=" + r.Minimum);
                ITrackingRates TrR = T.TrackingRates;
                if (TrR.Count == 0)
                    Console.WriteLine("  Empty TrackingRates!");
                else
                    foreach (DriveRates dr in TrR)
                        Console.WriteLine("  DriveRate=" + dr);
                T.Connected = false;
				T.Dispose();
			}
			#endregion

			#region Camera
			Console.WriteLine("\r\nCamera:");
			progID = Camera.Choose("ASCOM.Simulator.Camera");
            if (progID != "")
            {
                Camera C = new Camera(progID);
                C.Connected = true;
                Console.WriteLine("  Connected to " + progID);
                Console.WriteLine("  Description = " + C.Description);
                Console.WriteLine("  Pixel size = " + C.PixelSizeX + " * " + C.PixelSizeY);
                Console.WriteLine("  Camera size = " + C.CameraXSize + " * " + C.CameraYSize);
                Console.WriteLine("  Max Bin = " + C.MaxBinX + " * " + C.MaxBinY);
                Console.WriteLine("  Bin = " + C.BinX + " * " + C.BinY);
                Console.WriteLine("  MaxADU = " + C.MaxADU);
                Console.WriteLine("  CameraState = " + C.CameraState.ToString());
                Console.WriteLine("  CanAbortExposure = " + C.CanAbortExposure);
                Console.WriteLine("  CanAsymmetricBin = " + C.CanAsymmetricBin);
                Console.WriteLine("  CanGetCoolerPower = " + C.CanGetCoolerPower);
                Console.WriteLine("  CanPulseGuide = " + C.CanPulseGuide);
                Console.WriteLine("  CanSetCCDTemperature = " + C.CanSetCCDTemperature);
                Console.WriteLine("  CanStopExposure = " + C.CanStopExposure);
                Console.WriteLine("  CCDTemperature = " + C.CCDTemperature);
                if (C.CanGetCoolerPower)
                    Console.WriteLine("  CoolerPower = " + C.CoolerPower);
                Console.WriteLine("  ElectronsPerADU = " + C.ElectronsPerADU);
                Console.WriteLine("  FullWellCapacity = " + C.FullWellCapacity);
                Console.WriteLine("  HasShutter = " + C.HasShutter);
                Console.WriteLine("  HeatSinkTemperature = " + C.HeatSinkTemperature);
                if (C.CanPulseGuide)
                    Console.WriteLine("  IsPulseGuiding = " + C.IsPulseGuiding);
				Console.Write("  Take 15 second image");
				C.StartExposure(15.0, true);
				while (!C.ImageReady)
				{
					Console.Write(".");
					U.WaitForMilliseconds(300);
				}
				Console.WriteLine("\r\n  Exposure complete, ready for download.");
                Console.WriteLine("  CameraState = " + C.CameraState.ToString());
                Console.WriteLine("  LastExposureDuration = " + C.LastExposureDuration);
                Console.WriteLine("  LastExposureStartTime = " + C.LastExposureStartTime);
				int[,] imgArray = (int[,])C.ImageArray;
				Console.WriteLine("  Array is " + (imgArray.GetUpperBound(0) + 1) + " by " + (imgArray.GetUpperBound(1) + 1));
                C.Connected = false;
				C.Dispose();
			}
			#endregion

			#region FilterWheel
			Console.WriteLine("\r\nFilterWheel:");
			progID = FilterWheel.Choose("ASCOM.Simulator.FilterWheel");
			if (progID != "")
			{
				FilterWheel fw = new FilterWheel(progID);
				fw.Connected = true;
				Console.WriteLine("  Position = " + fw.Position);
				string[] names = fw.Names;
				Console.WriteLine("  There are " + names.Length + " filters:\r\n  ");
				for (int i = 0; i < names.Length; i++)
				{
					Console.Write(names[i] + " " );
				}
				Console.WriteLine("");
				fw.Connected = false;
				fw.Dispose();
			}
			#endregion

			#region Rotator
			Console.WriteLine("\r\nRotator:");
			progID = Rotator.Choose("ASCOM.Simulator.Rotator");
			if (progID != "")
			{
				Rotator R = new Rotator(progID);
				R.Connected = true;
				Console.WriteLine("  Position = " + R.Position);
				float np = R.Position + 60;
				if (np >= 360) np -= 360;
				Console.Write("  Rotating to " + np.ToString("0"));
				R.MoveAbsolute(np);
				while (R.IsMoving)
				{
					Console.Write(".");
					U.WaitForMilliseconds(300);
				}
				Console.WriteLine("\r\n  Rotation complete.");
				R.Connected = false;
				R.Dispose();
			}
			#endregion

			#region Dome
			Console.WriteLine("\r\nDome:");
			progID = Dome.Choose("ASCOM.Simulator.Dome");
            if (progID != "")
            {
                Dome D = new Dome(progID);
                D.Connected = true;
                Console.WriteLine("  Description = " + D.Description);
                Console.WriteLine("  Name = " + D.Name);
				if (D.CanSetAzimuth)
				{
					Console.WriteLine("  This is a rotatable dome");
					Console.WriteLine("  Current slit azimuth = " + D.Azimuth.ToString("0.0"));
					double z = D.Azimuth + 60;
					if (z >= 360) z -= 360;
					D.SlewToAzimuth(z);
					Console.Write("  Rotating to azimuth " + z.ToString("0"));
					while (D.Slewing)
					{
						Console.Write(".");
						U.WaitForMilliseconds(300);
					}
					Console.WriteLine("\r\n  Rotation complete.");
				}
				if (D.CanSetShutter)
				{
					if (D.CanSetAzimuth)
						Console.WriteLine("  This dome has a controllable shutter");
					else
						Console.WriteLine("  This is a roll-off roof");
					Console.WriteLine("  It is currently " + D.ShutterStatus.ToString());
					switch (D.ShutterStatus)
					{
						case ShutterState.shutterClosed:
							Console.Write("  Opening");
							D.OpenShutter();
							while (D.ShutterStatus != ShutterState.shutterOpen && 
									D.ShutterStatus != ShutterState.shutterError)
							{
								Console.Write(".");
								U.WaitForMilliseconds(300);
							}
							Console.WriteLine("\r\n  It is now " + D.ShutterStatus.ToString());
							break;
						case ShutterState.shutterOpen:
							Console.Write("  Closing");
							D.CloseShutter();
							while (D.ShutterStatus != ShutterState.shutterClosed &&
									D.ShutterStatus != ShutterState.shutterError)
							{
								Console.Write(".");
								U.WaitForMilliseconds(300);
							}
							Console.WriteLine("\r\n  It is now " + D.ShutterStatus.ToString());
							break;
						case ShutterState.shutterError:
							Console.WriteLine("  ** cannot do anything right now **");
							break;
						default:
							Console.WriteLine("  ** it's moving so can't do anythjing else  now **");
							break;
					}
				}
				D.Connected = false;
				D.Dispose();
			}
			#endregion

			Console.Write("\r\nPress enter to quit...");
			Console.ReadLine();
		}
	}
}
