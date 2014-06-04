using System.Collections;

namespace ASCOM.Interface
	{
	//[ComImport, Guid("2B8FD76E-AF7E-4FAA-9FAC-4029E96129F4"), TypeLibType((short)0x10c0)]
	//public interface IAxisRates : IEnumerable
	//    {
	//    [DispId(0x65)]
	//    int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; }
	//    [DispId(0)]
	//    IRate this[int Index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
	//    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
	//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4), TypeLibFunc((short)0x40)]
	//    IEnumerator GetEnumerator();
	//    }

	/// <summary>
	/// A collection of <see cref="Rate"/> objects describing the supported rates of motion
	/// for the <see cref="MoveAxis"/> method.
	/// </summary>
	public interface IAxisRates
		{
		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count of items in the collection.</value>
		int Count { get; }

		/// <summary>
		/// Gets the <see cref="ASCOM.Interface.IRate"/> at the specified index.
		/// </summary>
		/// <value>An <see cref="IRate"/> object.</value>
		IRate this[int Index] { get; }

		/// <summary>
		/// Gets an enumerator for the collection.
		/// </summary>
		/// <returns>An object that implements <see cref="IEnumerator"/>.</returns>
		IEnumerator GetEnumerator();
		}
	}
