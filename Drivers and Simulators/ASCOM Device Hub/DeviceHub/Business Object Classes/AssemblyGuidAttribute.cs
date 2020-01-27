using System;

namespace ASCOM.DeviceHub
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyGuidAttribute : Attribute
    {
        public string Guid { get; private set; }

        public AssemblyGuidAttribute( string guid )
        {
            Guid = guid;
        }
    }
}
