//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface.ITrackingRates interface
// </summary>
//
// <copyright company="TiGra Astronomy" author="Timothy P. Long">
//	Copyright © 2010 The ASCOM Initiative
// </copyright>
//
// <license>
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </license>
//
//
// Implements:	
// Author:		(TPL) Timothy P. Long <Tim@tigranetworks.co.uk>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 10-Feb-2010	TPL	6.0.*	Initial edit, mirrors platform 5.0 PIAs
// --------------------------------------------------------------------------------
//
using System.Collections;

namespace ASCOM.Interface
	{
	//[ComImport, TypeLibType((short)0x10c0), Guid("DC98F1DF-315A-43EF-81F6-23F3DD461F58")]
	//public interface ITrackingRates : IEnumerable
	//    {
	//    [DispId(0x65)]
	//    int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; }
	//    [DispId(0)]
	//    DriveRates this[int Index] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
	//    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
	//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short)0x40), DispId(-4)]
	//    IEnumerator GetEnumerator();
	//    }

	/// <summary>
	/// Supplies a collection of <see cref="DriveRate"/> objects that describe the permissible values of
	/// the <see cref="TrackingRate"/> property for a telescope.
	/// </summary>
	public interface ITrackingRates
		{
		/// <summary>
		/// Gets the count of items in the collection.
		/// </summary>
		/// <value>The count as an integer.</value>
		int Count { get; }

		/// <summary>
		/// Gets the <see cref="ASCOM.Interface.DriveRates"/> at the specified index.
		/// </summary>
		/// <value>A <see cref="ASCOM.Interface.DriveRates"/> object.</value>
		DriveRates this[int Index] { get; }

		/// <summary>
		/// Gets an enumerator for the collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> object.</returns>
		IEnumerator GetEnumerator();
		}
	}
