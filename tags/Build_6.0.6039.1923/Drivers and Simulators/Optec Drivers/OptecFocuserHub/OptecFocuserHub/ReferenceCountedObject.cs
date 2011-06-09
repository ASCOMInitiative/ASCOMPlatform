using System;
using System.Runtime.InteropServices;

namespace ASCOM.OptecFocuserHub
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            OptecFocuserHub.CountObject();
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            OptecFocuserHub.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            OptecFocuserHub.ExitIf();
        }
    }
}
