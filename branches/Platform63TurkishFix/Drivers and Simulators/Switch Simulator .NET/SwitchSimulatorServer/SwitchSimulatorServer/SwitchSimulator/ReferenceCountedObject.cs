using System;
using System.Runtime.InteropServices;

namespace ASCOM.SwitchSimulator
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            LocalServer.CountObject();
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            LocalServer.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            LocalServer.ExitIf();
        }
    }
}
