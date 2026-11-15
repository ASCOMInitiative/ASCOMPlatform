using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <inheritdoc/>
    [Guid("19D8AEA3-212D-4BCE-B3F5-AA10808E566E")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class AlignerConfigurationV2 : IAlignerConfigurationV2, IAlignerConfigurationV1
    {
        /// <inheritdoc/>
        public string NewMember { get { return "This is the new member string!"; } }
        /// <inheritdoc/>
        public AlignerPurpose AlignerPurpose { get { return AlignerPurpose.OpticalAxisAligner; } }
        /// <inheritdoc/>
        public bool CanFindHome { get { return true; } }
        /// <inheritdoc/>
        public bool CanHalt { get { return true; } }
        /// <inheritdoc/>
        public bool CanMoveX { get { return true; } }
        /// <inheritdoc/>
        public bool CanMoveY { get { return true; } }
        /// <inheritdoc/>
        public bool CanMoveZ { get { return true; } }
        /// <inheritdoc/>
        public bool CanMoveAbsolute { get { return true; } }
        /// <inheritdoc/>
        public bool CanSync { get { return true; } }
        /// <inheritdoc/>
        public AlignerUnits Units { get { return AlignerUnits.Millimeters; } }
        /// <inheritdoc/>
        public double XMaxMove { get { return 100.0; } }
        /// <inheritdoc/>
        public double XMaxPosition { get { return 100.0; } }
        /// <inheritdoc/>
        public double XMinPosition { get { return -100.0; } }
        /// <inheritdoc/>
        public double YMaxMove { get { return 100.0; } }
        /// <inheritdoc/>
        public double YMaxPosition { get { return 100.0; } }
        /// <inheritdoc/>
        public double YMinPosition { get { return -100.0; } }
        /// <inheritdoc/>
        public double ZMaxMove { get { return 100.0; } }
        /// <inheritdoc/>
        public double ZMaxPosition { get { return 100.0; } }
        /// <inheritdoc/>
        public double ZMinPosition { get { return -100.0; } }
    }
}
