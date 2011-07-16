using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
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
}