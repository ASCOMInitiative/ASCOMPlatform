using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ASCOM.Utilities
{
    internal class Net35Use
    {
        internal Net35Use()
        {
            Category = "Not set";
            Name = "Not set";
            Assembly = "Not set";
            Component = "Not set";
            DotNetVersion = "Not set";
        }
        internal string Category { get; set; }
        internal string Name { get; set; }
        internal string Assembly { get; set; }
        internal string Component { get; set; }

        internal string DotNetVersion { get; set; }

        internal new string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Category: {Category}, ");
            sb.Append($"Name: {Name}, ");
            sb.Append($"Assembly: {Assembly}, ");
            sb.Append($"DotNetVersion: {DotNetVersion}, "); 
            sb.Append($"Component: {Component}.");
            return sb.ToString();
        }
    }
}
