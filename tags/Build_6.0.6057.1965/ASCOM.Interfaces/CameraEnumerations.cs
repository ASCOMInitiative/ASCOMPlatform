//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface Camera Enumerations
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
// Defines:	ICamera enumerations
// Author:		(CDR) Chris Rowland <chris.rowland@dsl.pipex.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 21-Feb-2010	CDR	6.0.*	Initial edit. Mirrors platform 5.0 PIAs.
// --------------------------------------------------------------------------------
using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
    /// <summary>
    /// ASCOM Camera status values.
    /// </summary>
    [Guid("D40EB54D-0F0F-406d-B68F-C2A7984235BC")]
    public enum CameraStates
    {
        cameraIdle = 0,
        cameraWaiting = 1,
        cameraExposing = 2,
        cameraReading = 3,
        cameraDownload = 4,
        cameraError = 5
    }

    /// <summary>
    /// Sensor type, identifies the type of colour sensor
    /// V2 cameras only
    /// </summary>
    [Guid("C9D7FD9C-1F22-47B3-88A3-2E3C5C5E9B94")]
    public enum SensorType
    {
        /// <summary>
        /// Camera produces monochrome array with no Bayer encoding
        /// </summary>
        Monochrome = 0,
        /// <summary>
        /// Camera produces color image directly, requiring not Bayer decoding
        /// </summary>
        Color = 1,
        /// <summary>
        /// Camera produces RGGB encoded Bayer array images
        /// </summary>
        RGGB = 2,
        /// <summary>
        /// Camera produces CMYG encoded Bayer array images
        /// </summary>
        CMYG = 3,
        /// <summary>
        /// Camera produces CMYG2 encoded Bayer array images
        /// </summary>
        CMYG2 = 4,
        /// <summary>
        /// Camera produces Kodak TRUESENSE Bayer LRGB array images
        /// </summary>
        LRGB = 5
    }
}
