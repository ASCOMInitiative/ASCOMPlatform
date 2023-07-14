using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Operation completed interface
    /// </summary>
    [Guid("CCC5184E-ABE7-48E5-B0AB-25E6846DC823")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    public interface IOperationCompleted
    {
        /// <summary>
        /// 
        /// </summary>
        [DispId(1)]
        void OperationCompleted(OperationCompleteArgs args);
    }
}
