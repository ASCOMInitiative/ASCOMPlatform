using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// ComArrayList interface
    /// </summary>
    [Guid("F0A3D52D-7FF1-4889-9E3F-274ACBBE3393")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IComTest : IEnumerable
    {
        /// <summary>
        /// Number of items in the ArrayList
        /// </summary>
        /// <returns></returns>
        int NumberOfItems();

        /// <summary>
        /// Get stored values
        /// </summary>
        /// <returns></returns>
        string GetValues();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void Add(StateValue value);
    }
}
