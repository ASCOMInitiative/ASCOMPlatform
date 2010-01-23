using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.SwitchSimulator
{
    [Guid("9FD36B85-5A8A-473d-9893-647E0B2871AB")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switches : ReferenceCountedObjectBase, ISwitches
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="Switch"/> class.
		/// Must be public for COM registration.
		/// </summary>
        public Switches()
		{
			//TODO: Implement your additional construction here
		}


        ArrayList ItemList = new ArrayList();
    }
}
