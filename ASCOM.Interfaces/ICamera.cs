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
// --------------------------------------------------------------------------------
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
    //    uuid(D95FBC6E-0705-458b-84C0-57E3295DBCCE),
    /// <summary>
    /// ASCOM Camera Driver 1.0 Interface
    /// </summary>
    [ComImport, TypeLibType((short)0x10c0), Guid("D95FBC6E-0705-458B-84C0-57E3295DBCCE")]
    public interface ICamera
    {
        [DispId(0x65)]
        short BinX { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] set; }
        [DispId(0x66)]
        short BinY { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] set; }
        [DispId(0x67)]
        CameraStates CameraState { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] get; }
        [DispId(0x68)]
        int CameraXSize { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x68)] get; }
        [DispId(0x69)]
        int CameraYSize { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x69)] get; }
        [DispId(0x6a)]
        bool CanAbortExposure { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6a)] get; }
        [DispId(0x6b)]
        bool CanAsymmetricBin { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6b)] get; }
        [DispId(0x6c)]
        bool CanGetCoolerPower { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6c)] get; }
        [DispId(0x6d)]
        bool CanPulseGuide { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6d)] get; }
        [DispId(110)]
        bool CanSetCCDTemperature { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(110)] get; }
        [DispId(0x6f)]
        bool CanStopExposure { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6f)] get; }
        [DispId(0x70)]
        double CCDTemperature { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x70)] get; }
        [DispId(0x71)]
        bool Connected { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x71)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x71)] set; }
        [DispId(0x72)]
        bool CoolerOn { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x72)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x72)] set; }
        [DispId(0x73)]
        double CoolerPower { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x73)] get; }
        [DispId(0x74)]
        string Description { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x74)] get; }
        [DispId(0x75)]
        double ElectronsPerADU { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x75)] get; }
        [DispId(0x76)]
        double FullWellCapacity { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x76)] get; }
        [DispId(0x77)]
        bool HasShutter { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x77)] get; }
        [DispId(120)]
        double HeatSinkTemperature { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(120)] get; }
        [DispId(0x79)]
        object ImageArray { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x79)] get; }
        [DispId(0x7a)]
        object ImageArrayVariant { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7a)] get; }
        [DispId(0x7b)]
        bool ImageReady { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7b)] get; }
        [DispId(0x7c)]
        bool IsPulseGuiding { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7c)] get; }
        [DispId(0x7d)]
        string LastError { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7d)] get; }
        [DispId(0x7e)]
        double LastExposureDuration { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7e)] get; }
        [DispId(0x7f)]
        string LastExposureStartTime { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7f)] get; }
        [DispId(0x80)]
        int MaxADU { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x80)] get; }
        [DispId(0x81)]
        short MaxBinX { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x81)] get; }
        [DispId(130)]
        short MaxBinY { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(130)] get; }
        [DispId(0x83)]
        int NumX { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x83)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x83)] set; }
        [DispId(0x84)]
        int NumY { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x84)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x84)] set; }
        [DispId(0x85)]
        double PixelSizeX { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x85)] get; }
        [DispId(0x86)]
        double PixelSizeY { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x86)] get; }
        [DispId(0x87)]
        double SetCCDTemperature { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x87)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x87)] set; }
        [DispId(0x88)]
        int StartX { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x88)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x88)] set; }
        [DispId(0x89)]
        int StartY { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x89)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x89)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x191)]
        void AbortExposure();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x192)]
        void PulseGuide([In] GuideDirections Direction, [In] int Duration);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x193)]
        void SetupDialog();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x194)]
        void StartExposure([In] double Duration, [In] bool Light);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x195)]
        void StopExposure();
    }
}
