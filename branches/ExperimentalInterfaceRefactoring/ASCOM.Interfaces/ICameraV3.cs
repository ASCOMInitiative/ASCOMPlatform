//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface Camera V2 interface
// </summary>
//
// <copyright company="The ASCOM Initiative" author="Timothy P. Long">
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
// Defines:	ICamera V2 enumerations
// Author:		(CDR) Chris Rowland <chris.rowland@dsl.pipex.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 21-Feb-2010	CDR	6.0.*	Initial edit.
// --------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
    /// <summary>
    /// Camera V2 interface is the aggregate of Camera V1
    /// and the new IAscomDriver and IDeviceControl interfaces.
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("12803E24-2C5C-43ba-80A2-B0652738E699")]
    public interface ICameraV3 : ICameraV2
    {
        string ANewCameraV3Property { get; }
        void ANewCameraV3Method();
    }
}
