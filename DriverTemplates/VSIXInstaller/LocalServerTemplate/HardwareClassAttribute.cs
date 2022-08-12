using System;

namespace ASCOM
{
    /// <summary>
    /// This attribute can be applied to a class to mark it as a local server hardware class whose Dispose() method must be called prior to the local server shutting down.
    /// The attribute can be applied to one or more classes, the only requirement is that the class exposes a Dispose() method.
    /// The class's DIspose() method will be called just before the local server itself closes down.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HardwareClassAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "HardwareClassAttribute" /> class.
        /// </summary>
        public HardwareClassAttribute()
        {
        }

    }
}