using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    // -----------------------------------------------------------------------
    // <summary>Defines the IRate Interface</summary>
    // -----------------------------------------------------------------------
    /// <summary>
    /// Describes a range of rates supported by the <see cref="ITelescopeV3.MoveAxis" /> method (degrees/per second)
    /// These are contained within an <see cref="IAxisRates" /> collection and serve to describe one or more supported ranges of rates of motion about a mechanical axis.
    /// It is possible that the <see cref="IRate.Maximum" /> and <see cref="IRate.Minimum" /> properties will be equal. In this case, the <see cref="IRate" /> object expresses a single discrete rate.
    /// Both the <see cref="IRate.Minimum" />  and <see cref="IRate.Maximum" />  properties are always expressed in units of degrees per second.
    /// This is only using for Telescope InterfaceVersions 2 and 3
    /// </summary>
    /// <remarks>Values used must be non-negative and are scalar values. You do not need to supply complementary negative rates for each positive
    /// rate that you specify. Movement in both directions is achieved by the application applying an appropriate positive or negative sign to the
    /// rate when it is used in the <see cref="ITelescopeV3.MoveAxis" /> command.</remarks>
    [ComVisible(true)]
    [Guid("2E7CEEE4-B5C6-4e9a-87F4-80445700D123")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IRate // 221C0BC0-110B-4129-85A0-18BB28579290
    {
        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        void Dispose();

        /// <summary>
        /// The maximum rate (degrees per second)
        /// This must always be a positive number. It indicates the maximum rate in either direction about the axis.
        /// </summary>
        double Maximum { get; set; }

        /// <summary>
        /// The minimum rate (degrees per second)
        /// This must always be a positive number. It indicates the maximum rate in either direction about the axis.
        /// </summary>
        double Minimum { get; set; }
    }
}
