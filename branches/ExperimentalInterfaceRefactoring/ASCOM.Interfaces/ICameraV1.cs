//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface.ICamera Camera Interface V1
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
// Defines:	ICamera interfaces
// Author:		(CDR) Chris Rowland <chris.rowland@dsl.pipex.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 21-Feb-2010	CDR	6.0.*	Initial edit. Mirrors platform 5.0 PIAs.
// 03-Mar-2010	TPL	6.0.*	Exact layout produced by .NET Reflector from the PIAs
//							No new interfaces yet (do that in V2)
// --------------------------------------------------------------------------------
//
using System;
using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
    //    uuid(D95FBC6E-0705-458b-84C0-57E3295DBCCE),

    /// <summary>
    /// ASCOM Camera Driver 1.0 Interface
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("62F242E6-98CD-4a9b-A4FC-5FCCB1BDBA02")]
    public interface ICameraV1 : IAscomDriver, IDeviceControl, ICamera
    {

        bool Connected {  get;   set; }

        string Description {   get; }

        void SetupDialog();

    }
}
