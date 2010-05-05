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
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("4002DD8D-8555-4b36-A9A9-172163F83364")]
    public interface ICameraV2 : IAscomDriver, IDeviceControl
    {
        #region ICamera Members
        short BinX { get; set; }

        short BinY { get; set; }

        CameraStates CameraState { get; }

        int CameraXSize { get; }

        int CameraYSize { get; }

        bool CanAbortExposure { get; }

        bool CanAsymmetricBin { get; }

        bool CanGetCoolerPower { get; }

        bool CanPulseGuide { get; }

        bool CanSetCCDTemperature { get; }

        bool CanStopExposure { get; }

        double CCDTemperature { get; }

        //bool Connected {  get;   set; }

        bool CoolerOn { get; set; }

        double CoolerPower { get; }

        //string Description {   get; }

        double ElectronsPerADU { get; }

        double FullWellCapacity { get; }

        bool HasShutter { get; }

        double HeatSinkTemperature { get; }

        object ImageArray { get; }

        object ImageArrayVariant { get; }

        bool ImageReady { get; }

        bool IsPulseGuiding { get; }

        string LastError { get; }

        double LastExposureDuration { get; }

        string LastExposureStartTime { get; }

        int MaxADU { get; }

        short MaxBinX { get; }

        short MaxBinY { get; }

        int NumX { get; set; }

        int NumY { get; set; }

        double PixelSizeX { get; }

        double PixelSizeY { get; }

        double SetCCDTemperature { get; set; }

        int StartX { get; set; }

        int StartY { get; set; }

        void AbortExposure();

        void PulseGuide(GuideDirections Direction, int Duration);

        //void SetupDialog();

        void StartExposure(double Duration, bool Light);

        void StopExposure();


        #endregion

        #region ICameraV2 additional members

        // ToDo: add intellisense XML comments to each member.
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

        #endregion
    }
}
