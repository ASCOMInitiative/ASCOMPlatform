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

namespace ASCOM.Interface
{
    interface ICameraV2 : ICamera
    {
        short BayerOffsetX { get; }

        short BayerOffsetY { get; }

        bool CanFastReadout { get; }

        double ExposureMax { get; }

        double ExposureMin { get; }

        double ExposureResolution { get; }

        bool FastReadout { set; get; }

        short Gain { set; get; }

        short GainMax { get; }

        short GainMin { get; }

        string[] Gains { get; }

        short PercentCompleted { get; }

        short ReadoutMode { get; }

        string[] ReadoutModes { get; }

        string SensorName { get; }

        SensorType SensorType { get; }
    }
}
