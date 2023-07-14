using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Delegate describing an operation complete event handler
    /// </summary>
    /// <param name="operationCompleteArgs">Information about the event that completed.</param>
    public delegate void CompletionEventHandler(OperationCompleteArgs operationCompleteArgs);
}
