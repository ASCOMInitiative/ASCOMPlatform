using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class SlewInProgressMessage
	{
		public bool IsSlewInProgress { get; private set; }
		public double RightAscension { get; private set; }
		public double Declination { get; private set; }
		public PierSide SideOfPier { get; private set; }
		public SlewInProgressMessage( bool isSlewInProgress, double rightAscension = double.NaN, double declination = double.NaN
										, PierSide sideOfPier = PierSide.pierUnknown )
		{
			IsSlewInProgress = isSlewInProgress;
			RightAscension = rightAscension;
			Declination = declination;
			SideOfPier = sideOfPier;
		}
	}
}
