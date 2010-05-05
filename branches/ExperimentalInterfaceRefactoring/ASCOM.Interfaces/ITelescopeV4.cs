//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface.ITelescope Telescope Interface V2
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
// Defines:	ITelescope interfaces
// Author:		(TPL) Timothy P. Long <Tim@tigranetworks.co.uk>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 10-Feb-2010	TPL	6.0.*	Initial edit. Mirrors platform 5.0 PIAs.
// 21-Feb-2010  cdr 6.0.*   Remove properties and methods already in IAscomDriver
// 03-Mar-2010	TPL	6.0.*	Renamed to ITelescopeV3, added IDeviceControl
// --------------------------------------------------------------------------------
//
using System;
using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
    /// <summary>
    /// ITelescopeV4 inherits ITelescopeV3 and IDontKnowYet
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("06CFB59B-2D9A-4099-9EAB-AA3900CAAF8D")]
    public interface ITelescopeV4 : ITelescopeV3, IDontKnowYet
    {
        string ANewTelescopeV4Property { get; }
        void ANewTelescopeV4Method(string NewParameter1, double NewParameter2);
    }
}
