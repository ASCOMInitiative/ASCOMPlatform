using System.Runtime.InteropServices;

namespace ASCOM.Simulator
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            OCSimulator.TL.LogMessage("ReferenceCountedObjectBase", "Incrementing object count");
            Server.CountObject();
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            OCSimulator.TL.LogMessage("~ReferenceCountedObjectBase", "Decrementing object count");
            Server.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            OCSimulator.TL.LogMessage("~ReferenceCountedObjectBase", "Calling ExitIf");
            Server.ExitIf();
        }
    }
}
