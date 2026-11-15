using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Interface defining an object that contains all configuration properties for an aligner device.
    /// </summary>
    [ComVisible(true)]
    [Guid("8A0C27DB-3E38-4620-A537-AA51787AAA15")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAlignerConfigurationV2
    {
        /// <summary>
        /// New test member added to the interface
        /// </summary>
        string NewMember { get; }

        /// <summary>
        /// The intended use of this device
        /// </summary>
        AlignerPurpose AlignerPurpose { get; }

        /// <summary>
        /// True if the device can find home
        /// </summary>
        bool CanFindHome { get; }

        /// <summary>
        /// True if the device can halt all movement
        /// </summary>
        bool CanHalt { get; }

        /// <summary>
        /// True of the device can move the X axis
        /// </summary>
        bool CanMoveX { get; }

        /// <summary>
        /// True if the device can move its Y axis
        /// </summary>
        bool CanMoveY { get; }

        /// <summary>
        /// True if the device can move its Z axis
        /// </summary>
        bool CanMoveZ { get; }

        /// <summary>
        /// True if the device can move to an absolute position
        /// </summary>
        bool CanMoveAbsolute { get; }

        /// <summary>
        /// True if the device can synchronize its state
        /// </summary>
        bool CanSync { get; }

        /// <summary>
        /// The device's unit of measurement 1 = Degrees , 2 = Millimeters.
        /// </summary>
        AlignerUnits Units { get; }

        /// <summary>
        /// Largest relative movement of which the X axis is capable in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum relative X movement.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// </remarks>
        double XMaxMove { get; }

        /// <summary>
        /// The maximum reachable position of the X axis in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum possible X position.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// <para>When XMaxPosition is implemented, XMinPosition must also be implemented, Likewise, when XMaxPosition is not implemented XMinPosition must also not be implemented.</para>
        /// </remarks>
        double XMaxPosition { get; }

        /// <summary>
        /// The minimum reachable position of the X axis in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the minimum possible X position.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// <para>When XMinPosition is implemented, XMaxPosition must also be implemented, Likewise, when XMinPosition is not implemented XMaxPosition must also not be implemented.</para>
        /// </remarks>
        double XMinPosition { get; }

        /// <summary>
        /// Largest relative movement of which the Y axis is capable in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum relative X movement.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// </remarks>
        double YMaxMove { get; }

        /// <summary>
        /// The maximum reachable position of the Y axis in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum possible Y position.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// <para>When YMaxPosition is implemented, YMinPosition must also be implemented, Likewise, when YMaxPosition is not implemented YMinPosition must also not be implemented.</para>
        /// </remarks>
        double YMaxPosition { get; }

        /// <summary>
        /// The minimum reachable position of the Y axis in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the minimum possible Y position.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// <para>When YMinPosition is implemented, YMaxPosition must also be implemented, Likewise, when YMinPosition is not implemented YMaxPosition must also not be implemented.</para>
        /// </remarks>
        double YMinPosition { get; }

        /// <summary>
        /// Largest relative movement of which the Z axis is capable in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum relative Z movement.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// </remarks>
        double ZMaxMove { get; }

        /// <summary>
        /// The maximum reachable position of the Z axis in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum possible Z position.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// <para>When ZMaxPosition is implemented, ZMinPosition must also be implemented, Likewise, when ZMaxPosition is not implemented ZMinPosition must also not be implemented.</para>
        /// </remarks>
        double ZMaxPosition { get; }

        /// <summary>
        /// The minimum reachable position of the Z axis in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the minimum possible Z position.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// <para>When ZMinPosition is implemented, ZMaxPosition must also be implemented, Likewise, when ZMinPosition is not implemented ZMaxPosition must also not be implemented.</para>
        /// </remarks>
        double ZMinPosition { get; }
    }
}
