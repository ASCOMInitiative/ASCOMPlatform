using System;
using System.Runtime.InteropServices;

namespace ASCOM.GeminiTelescope
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            GeminiTelescope.CountObject();
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            GeminiTelescope.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            GeminiTelescope.ExitIf();
        }
    }
}
