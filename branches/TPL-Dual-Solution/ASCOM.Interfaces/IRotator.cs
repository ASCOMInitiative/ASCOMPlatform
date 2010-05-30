using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
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
}