using System;

namespace ASCOM
{
    /// <summary>
    /// This attribute can be applied to any static local server class to mark it as a hardware class whose Dispose() method must be called prior to the local server shutting down.
    /// 
    /// The HardwareClassAttribute attribute can be applied to one or more classes, the only requirements are that the class is static and exposes a Dispose() method.
    /// The class's DIspose() method will be called by the local server just before it closes down.
    /// 
    /// NOTE: By default the local server code created by the template will only call the Dispose() method on static classes that have this attribute.
    /// 
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