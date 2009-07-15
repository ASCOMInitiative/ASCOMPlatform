using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
    [global::System.AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class ServedClassNameAttribute : Attribute
    {
        string servedClassName;

        public ServedClassNameAttribute(string servedClassName)
        {
            this.servedClassName = servedClassName;
        }
        public string ServedClassName
        {
            get { return servedClassName; }
            set { servedClassName = value; }
        }
    }
}
