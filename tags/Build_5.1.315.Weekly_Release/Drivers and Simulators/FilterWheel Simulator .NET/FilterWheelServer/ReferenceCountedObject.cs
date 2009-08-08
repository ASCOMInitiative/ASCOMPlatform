using System;
using System.Runtime.InteropServices;

namespace ASCOM.FilterWheelSim
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            FilterWheelSim.CountObject();
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            FilterWheelSim.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            FilterWheelSim.ExitIf();
        }
    }
}
