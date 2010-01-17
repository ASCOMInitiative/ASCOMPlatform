using System;
using System.Runtime.InteropServices;

namespace ASCOM.Simulator.Switch
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            SwitchSimulatorServer.CountObject();
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            SwitchSimulatorServer.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            SwitchSimulatorServer.ExitIf();
        }
    }
}
