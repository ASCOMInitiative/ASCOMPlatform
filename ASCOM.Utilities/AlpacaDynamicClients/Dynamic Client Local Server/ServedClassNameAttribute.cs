﻿using System;

namespace ASCOM.DynamicClients
{
    /// <summary>
    ///   An attribute that confers a 'friendly name' on a class and marks it as loadable by LocalServer.
    ///   The 'friendly name' is used by the ASCOM LocalServer to register the class with the ASCOM Chooser.
    ///   The 'friendly name' is what gets displayed to the user in the driver selection combo box.
    ///   This attribute is also used by the LocalServer to filter the assemblies that it will
    ///   attempt to load at runtime. LocalServer will only load classes bearing this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ServedClassNameAttribute : Attribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ServedClassNameAttribute" /> class.
        /// </summary>
        /// <param name = "servedClassName">The 'friendly name' of the served class.</param>
        public ServedClassNameAttribute(string servedClassName)
        {
            DisplayName = servedClassName;
        }

        /// <summary>
        ///   Gets or sets the 'friendly name' of the served class, as registered with the ASCOM Chooser.
        /// </summary>
        /// <value>The 'friendly name' of the served class.</value>
        public string DisplayName { get; private set; }
    }
}
