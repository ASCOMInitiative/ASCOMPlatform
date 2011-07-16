//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface.IRate interface
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.Interface
	{
	//[ComImport, TypeLibType((short)0x10c0), Guid("221C0BC0-110B-4129-85A0-18BB28579290")]
	//public interface IRate
	//    {
	//    [DispId(0x65)]
	//    double Maximum { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] set; }
	//    [DispId(0x66)]
	//    double Minimum { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] set; }
	//    }

	/// <summary>
	/// Describes a range of rates supported by the MoveAxis() method (degrees/per second)
	/// </summary>
	/// <remarks>
	/// These are contained within the <see cref="AxisRates"/> collection.
	/// They serve to describe one or more supported ranges of rates of motion about a mechanical axis.
	/// <para>
	/// It is possible that the <see cref="Rate.Maximum"/>  and <see cref="Rate.Minimum"/> properties
	/// will be equal. In this case, the Rate object expresses a single discrete rate. 
	/// </para><para>
	/// Both the <see cref="Rate.Maximum"/> and <see cref="Rate.Minimum"/> properties are always
	/// expressed in units of degrees per second. 
	/// </para>
	/// </remarks>
	public interface IRate
		{
		/// <summary>
		/// Gets the maximum rate in degrees per second.
		/// </summary>
		/// <value>The maximum rate (degrees per second).</value>
		double Maximum { get; }
		/// <summary>
		/// Gets the minimum rate in degrees per second.
		/// </summary>
		/// <value>The minimum rate (degrees per second).</value>
		double Minimum { get; }
		}
	}
