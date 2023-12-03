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
    [Guid("6720258E-E5B1-4160-BE34-95484A496E3B")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IComTest
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
        void Add(string name, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator GetEnumerator();
    }
}
