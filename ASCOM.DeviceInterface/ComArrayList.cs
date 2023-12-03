using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// COM visible ArrayList class for use by .NET 5, 6, 7, 8 and later
    /// </summary>
    [Guid("E67780D1-7C92-442C-8DCA-FCD06718427D")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class ComArrayList : ArrayList, IComArrayList
    {
        /// <summary>
        /// Create a ComArrayList instance
        /// </summary>
        public ComArrayList()
        {
        }

        /// <summary>
        /// Return the number of items
        /// </summary>
        /// <returns>ArrayList.Count</returns>
        public int NumberOfItems() => base.Count;
    }
}
