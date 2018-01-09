using System;
using System.Runtime.InteropServices;

namespace ASCOM.Simulator
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            TelescopeHardware.TL.LogMessage("ReferenceCountedObjectBase", "Incrementing object count");
            TelescopeSimulator.CountObject();
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            TelescopeHardware.TL.LogMessage("~ReferenceCountedObjectBase", "Decrementing object count");
            TelescopeSimulator.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            TelescopeHardware.TL.LogMessage("~ReferenceCountedObjectBase", "Calling ExitIf");
            TelescopeSimulator.ExitIf();
        }
    }
}
