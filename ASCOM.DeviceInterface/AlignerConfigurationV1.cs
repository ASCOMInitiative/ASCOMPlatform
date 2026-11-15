using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <inheritdoc/>
    [Guid("E7E22845-E742-49CA-9914-963F169AE98C")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class AlignerConfigurationV1 : IAlignerConfigurationV1
    {
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
