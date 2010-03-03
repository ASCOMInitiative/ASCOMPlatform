using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

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

	[ComImport, Guid("756FD725-A6E2-436F-8C7A-67E358622027"), TypeLibType((short)0x10c0)]
	public interface IFilterWheel
	{
		[DispId(0x65)]
		bool Connected { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] set; }
		[DispId(0x66)]
		int[] FocusOffsets { [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] get; }
		[DispId(0x67)]
		short Position { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] set; }
		[DispId(0x68)]
		string[] Names { [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x68)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x191)]
		void SetupDialog();
	}

	[ComImport, Guid("C2E3FE9C-01CD-440C-B8E3-C56EE9E4EDBC"), TypeLibType((short)0x10c0)]
	public interface IFocuser
	{
		[DispId(0x65)]
		bool Absolute { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; }
		[DispId(0x66)]
		bool IsMoving { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] get; }
		[DispId(0x67)]
		bool Link { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] set; }
		[DispId(0x68)]
		int MaxIncrement { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x68)] get; }
		[DispId(0x69)]
		int MaxStep { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x69)] get; }
		[DispId(0x6a)]
		int Position { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6a)] get; }
		[DispId(0x6b)]
		double StepSize { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6b)] get; }
		[DispId(0x6c)]
		bool TempComp { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6c)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6c)] set; }
		[DispId(0x6d)]
		bool TempCompAvailable { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6d)] get; }
		[DispId(110)]
		double Temperature { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(110)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x191)]
		void Halt();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x192)]
		void SetupDialog();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x193)]
		void Move([In] int val);
	}

	[ComImport, TypeLibType((short)0x10c0), Guid("49003324-8DE2-4986-BC7D-4D85E1C4CF6B")]
	public interface IRotator
	{
		[DispId(0x65)]
		bool CanReverse { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; }
		[DispId(0x66)]
		bool Connected { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] set; }
		[DispId(0x67)]
		bool IsMoving { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] get; }
		[DispId(0x68)]
		float Position { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x68)] get; }
		[DispId(0x69)]
		bool Reverse { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x69)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x69)] set; }
		[DispId(0x6a)]
		float StepSize { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6a)] get; }
		[DispId(0x6b)]
		float TargetPosition { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6b)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x191)]
		void Halt();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x192)]
		void Move(float Position);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x193)]
		void MoveAbsolute(float Position);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x194)]
		void SetupDialog();
	}

	[ComImport, Guid("44C03033-C60E-4101-856C-AAFB0F735F83"), TypeLibType((short)0x10c0)]
	public interface ISwitch
	{
		[DispId(0x65)]
		bool Connected { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] set; }
		[DispId(0x6803000b)]
		string Description { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6803000b)] get; }
		[DispId(0x66)]
		string DriverInfo { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] get; }
		[DispId(0x67)]
		string DriverVersion { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] get; }
		[DispId(0x68)]
		short InterfaceVersion { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x68)] get; }
		[DispId(0x69)]
		short MaxSwitch { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x69)] get; }
		[DispId(0x6a)]
		string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6a)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x191)]
		bool GetSwitch([In] short ID);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x192)]
		void SetSwitch([In] short ID, [In] bool State);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x193)]
		void SetupDialog();
		[return: MarshalAs(UnmanagedType.BStr)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x194)]
		string GetSwitchName([In] short ID);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x195)]
		void SetSwitchName([In] short ID, [In, MarshalAs(UnmanagedType.BStr)] string State);
	}

	[Guid("8915DF3D-B055-4195-8D23-AAD7F58FDF3B")]
	public enum ShutterState
	{
		shutterOpen,
		shutterClosed,
		shutterOpening,
		shutterClosing,
		shutterError
	}

}
