using System;
using ASCOM.Interface;
using ASCOM.Helper;
using ASCOM.DriverAccess;

namespace ASCOM
{
	class ClientTest
	{
		static void Main(string[] args)
		{
			string progID;
            
			Helper.Util U = new Helper.Util();
			// Could use Helper here but the Focuser Choose() is nice!
			progID = Focuser.Choose("FocusSim.Focuser");
			if (progID != "")
            {
			    Focuser F = new Focuser(progID);
			    F.Link = true;
			    Console.WriteLine("Connected to " + progID);
			    Console.WriteLine("Current position is " + F.Position);
			    Console.Write("New position: ");
			    string sNewPos = Console.ReadLine();
			    F.Move(Convert.ToInt32(sNewPos));
			    while (F.IsMoving)
			    {
			        Console.Write(".");
			        U.WaitForMilliseconds(333);
			    }
			    Console.WriteLine("\r\nMove complete. New position is " + F.Position.ToString());
			    F.Link = false;
				F.Dispose();								// Release this now, not at exit (typ.)
            }

			progID = Telescope.Choose("ScopeSim.Telescope");
            if (progID != "")
            {
                Telescope T = new Telescope(progID);
                T.Connected = true;
                Console.WriteLine("Connected to " + progID);
                Console.WriteLine("Current LST = " + T.SiderealTime);
                IAxisRates AxR = T.AxisRates(TelescopeAxes.axisPrimary);
                Console.WriteLine(AxR.Count + " rates");
                if (AxR.Count == 0)
                    Console.WriteLine("Empty AxisRates!");
                else
                    foreach (IRate r in AxR)
                        Console.WriteLine("Max=" + r.Maximum + " Min=" + r.Minimum);
                ITrackingRates TrR = T.TrackingRates;
                if (TrR.Count == 0)
                    Console.WriteLine("Empty TrackingRates!");
                else
                    foreach (DriveRates dr in TrR)
                        Console.WriteLine("DriveRate=" + dr);
                T.Connected = false;
				T.Dispose();
            }
            progID = Camera.Choose("CCDSimulator.Camera");
            if (progID != "")
            {
                Camera C = new Camera(progID);
                C.Connected = true;
                Console.WriteLine("Connected to " + progID);
                Console.WriteLine("Description = " + C.Description);
                Console.WriteLine("Pixel size = " + C.PixelSizeX + " * " + C.PixelSizeY);
                Console.WriteLine("Camera size = " + C.CameraXSize + " * " + C.CameraYSize);
                Console.WriteLine("Max Bin = " + C.MaxBinX + " * " + C.MaxBinY);
                Console.WriteLine("Bin = " + C.BinX + " * " + C.BinY);
                Console.WriteLine("CameraState = " + C.CameraState);
                Console.WriteLine("CanAbortExposure = " + C.CanAbortExposure);
                Console.WriteLine("CanAsymmetricBin = " + C.CanAsymmetricBin);
                Console.WriteLine("CanGetCoolerPower = " + C.CanGetCoolerPower);
                Console.WriteLine("CanPulseGuide = " + C.CanPulseGuide);
                Console.WriteLine("CanSetCCDTemperature = " + C.CanSetCCDTemperature);
                Console.WriteLine("CanStopExposure = " + C.CanStopExposure);
                Console.WriteLine("CCDTemperature = " + C.CCDTemperature);
                Console.WriteLine("Connected = " + C.Connected);
                Console.WriteLine("CCDTemperature = " + C.CoolerOn);
                if (C.CanGetCoolerPower)
                    Console.WriteLine("Connected = " + C.CoolerPower);
                Console.WriteLine("ElectronsPerADU = " + C.ElectronsPerADU);
                Console.WriteLine("FullWellCapacity = " + C.FullWellCapacity);
                Console.WriteLine("HasShutter = " + C.HasShutter);
                Console.WriteLine("HeatSinkTemperature = " + C.HeatSinkTemperature);
                Console.WriteLine("ImageReady = " + C.ImageReady);
                if (C.CanPulseGuide)
                    Console.WriteLine("IsPulseGuiding = " + C.IsPulseGuiding);
                Console.WriteLine("LastError = " + C.LastError);
                Console.WriteLine("LastExposureDuration = " + C.LastExposureDuration);
                Console.WriteLine("LastExposureStartTime = " + C.LastExposureStartTime);
                Console.WriteLine("MaxADU = " + C.MaxADU);
                Console.WriteLine("StartX = " + C.StartX);
                Console.WriteLine("StartY = " + C.StartY);
				int[,] imgArray = (int[,])C.ImageArray;
				Console.WriteLine("Array is " + (imgArray.GetUpperBound(0) + 1) + " by " + (imgArray.GetUpperBound(1) + 1));
                C.Connected = false;
				C.Dispose();
            }

// No filter wheel drivers yet, prevent Chooser error (oops!)
			//progID = FilterWheel.Choose("");
			//if (progID != "")
			//{
			//    FilterWheel fw = new FilterWheel(progID);
			//    fw.Connected = true;
			//    Console.WriteLine("Position = " + fw.Position);
			//    fw.Connected = false;
			//    fw.Dispose();
			//}
            try
            {
                progID = Rotator.Choose("ASCOM.Simulator.Rotator");
                if (progID != "")
                {
                    Rotator R = new Rotator(progID);
                    R.Connected = true;
                    Console.WriteLine("Position = " + R.Position);
                    R.Connected = false;
					R.Dispose();
                }
            }
            catch { }
            progID = Switch.Choose("SwitchSim.Switch");
            if (progID != "")
            {
                Switch S = new Switch(progID);
                S.Connected = true;
                Console.WriteLine("Description = " + S.Description);
                Console.WriteLine("Name = " + S.Name);
                for (short i = 0; i <= S.MaxSwitch; i++)
                {
                    try
                    {
                        Console.WriteLine(i + " : " + S.GetSwitchName(i) + " = " + S.GetSwitch(i));
                    }
                    catch { }
                }
                S.Connected = false;
				S.Dispose();
            }

            Console.Write("Press enter to quit");
			Console.ReadLine();
		}
	}
}
