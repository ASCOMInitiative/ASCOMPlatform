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
    [Guid("97599F14-170A-4756-998A-9B271508E2D1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAlignerConfigurationV1
    {
        /// <summary>
        /// The intended use of this device
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        AlignerPurpose AlignerPurpose { get; }

        /// <summary>
        /// True if the device compensates for gear backlash, otherwise false.
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool BacklashCompensation { get; }

        /// <summary>
        /// True if the device can find home
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool CanFindHome { get; }

        /// <summary>
        /// True of the device can move the X axis
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool CanMoveX { get; }

        /// <summary>
        /// True if the device can move its Y axis
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool CanMoveY { get; }

        /// <summary>
        /// True if the device can move its Z axis
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool CanMoveZ { get; }

        /// <summary>
        /// True if the device can move to an absolute position
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool CanMoveAbsolute { get; }

        /// <summary>
        /// True if the device can enable / turn off backlash compensation.
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool CanSetBacklash { get; }

        /// <summary>
        /// True if the device can synchronize its state
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool CanSync { get; }

        /// <summary>
        /// The device's unit of measurement 1 = Degrees , 2 = Millimeters, 3 = Counts.
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        AlignerUnits Units { get; }

        /// <summary>
        /// Largest relative movement of which the X axis is capable in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum relative X movement.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
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
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
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
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        double XMinPosition { get; }

        /// <summary>
        ///  The smallest move the X axis can make in <see cref="AlignerUnits"/>.
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        double XResolution { get; }

        /// <summary>
        /// Largest relative movement of which the Y axis is capable in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum relative Y movement.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
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
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
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
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        double YMinPosition { get; }

        /// <summary>
        ///  The smallest move the Y axis can make in <see cref="AlignerUnits"/>.
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        double YResolution { get; }

        /// <summary>
        /// Largest relative movement of which the Z axis is capable in <see cref="AlignerUnits"/>. 
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not know the maximum relative Z movement.</exception>
        /// <remarks>
        /// <para>>PolarAligner: Measured in degrees.</para>
        /// <para>OpticalAxisAligner: Measured in mm.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
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
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
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
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        double ZMinPosition { get; }

        /// <summary>
        ///  The smallest move the Z axis can make in <see cref="AlignerUnits"/>.
        /// </summary>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// </remarks>
        double ZResolution { get; }
    }
}
