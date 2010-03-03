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
namespace ASCOM.Interface
{
    //    uuid(D95FBC6E-0705-458b-84C0-57E3295DBCCE),
    /// <summary>
    /// ASCOM Camera Driver 1.0 Interface
    /// </summary>
	
	public interface ICamera
	{
		
		short BinX {  get;   set; }
		
		short BinY {  get;   set; }
		
		CameraStates CameraState {  get; }
		
		int CameraXSize {  get; }
		
		int CameraYSize {  get; }
		
		bool CanAbortExposure {  get; }

		bool CanAsymmetricBin { get; }
		
		bool CanGetCoolerPower {  get; }
		
		bool CanPulseGuide {  get; }
		
		bool CanSetCCDTemperature {  get; }
		
		bool CanStopExposure {  get; }
		
		double CCDTemperature {  get; }
		
		bool Connected {  get;   set; }
		
		bool CoolerOn {  get;   set; }
		
		double CoolerPower {  get; }
		
		string Description {   get; }
		
		double ElectronsPerADU {  get; }
		
		double FullWellCapacity {  get; }
		
		bool HasShutter {  get; }
		
		double HeatSinkTemperature {  get; }
		
		object ImageArray {   get; }
		
		object ImageArrayVariant {   get; }
		
		bool ImageReady {  get; }
		
		bool IsPulseGuiding {  get; }
		
		string LastError {   get; }
		
		double LastExposureDuration {  get; }
		
		string LastExposureStartTime {   get; }
		
		int MaxADU {  get; }
		
		short MaxBinX {  get; }
		
		short MaxBinY {  get; }
		
		int NumX {  get;   set; }
		
		int NumY {  get;   set; }
		
		double PixelSizeX {  get; }
		
		double PixelSizeY {  get; }
		
		double SetCCDTemperature {  get;   set; }
		
		int StartX {  get;   set; }
		
		int StartY {  get;   set; }
		
		void AbortExposure();
		
		void PulseGuide( GuideDirections Direction,  int Duration);
		
		void SetupDialog();
		
		void StartExposure( double Duration,  bool Light);
		
		void StopExposure();
	}

 

 

}
