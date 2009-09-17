using System;
using System.Runtime.InteropServices;

namespace ASCOM.FocuserSimulator
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            FocuserSimulator.CountObject();
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            FocuserSimulator.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            FocuserSimulator.ExitIf();
        }
    }
}
