using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
	[ComImport, TypeLibType((short)0x10c0), Guid("CCDA0D85-474A-4775-8105-1D513ADC3896")]
	public interface IDome
	{
		[DispId(0x65)]
		double Altitude { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; }
		[DispId(0x66)]
		bool AtHome { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] get; }
		[DispId(0x67)]
		bool AtPark { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] get; }
		[DispId(0x68)]
		double Azimuth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x68)] get; }
		[DispId(0x69)]
		bool CanFindHome { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x69)] get; }
		[DispId(0x6a)]
		bool CanPark { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6a)] get; }
		[DispId(0x6b)]
		bool CanSetAltitude { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6b)] get; }
		[DispId(0x6c)]
		bool CanSetAzimuth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6c)] get; }
		[DispId(0x6d)]
		bool CanSetPark { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6d)] get; }
		[DispId(110)]
		bool CanSetShutter { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(110)] get; }
		[DispId(0x6f)]
		bool CanSlave { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6f)] get; }
		[DispId(0x70)]
		bool CanSyncAzimuth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x70)] get; }
		[DispId(0x71)]
		bool Connected { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x71)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x71)] set; }
		[DispId(0x72)]
		string Description { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x72)] get; }
		[DispId(0x73)]
		string DriverInfo { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x73)] get; }
		[DispId(0x74)]
		short InterfaceVersion { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x74)] get; }
		[DispId(0x75)]
		string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x75)] get; }
		[DispId(0x76)]
		bool Slaved { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x76)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x76)] set; }
		[DispId(0x77)]
		ShutterState ShutterStatus { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x77)] get; }
		[DispId(120)]
		bool Slewing { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(120)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x191)]
		void AbortSlew();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x192)]
		void CloseShutter();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x193)]
		void FindHome();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x194)]
		void OpenShutter();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x195)]
		void Park();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x196)]
		void SetPark();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x197)]
		void SetupDialog();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x198)]
		void SlewToAltitude([In] double Altitude);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x199)]
		void SlewToAzimuth([In] double Azimuth);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(410)]
		void SyncToAzimuth([In] double Azimuth);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x19b)]
		void CommandBlind([In, MarshalAs(UnmanagedType.BStr)] string Command);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x19c)]
		bool CommandBool([In, MarshalAs(UnmanagedType.BStr)] string Command);
		[return: MarshalAs(UnmanagedType.BStr)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x19d)]
		string CommandString([In, MarshalAs(UnmanagedType.BStr)] string Command);
	}
}




