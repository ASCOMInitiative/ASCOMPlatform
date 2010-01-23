using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.SwitchSimulator
{
    [Guid("A82EC48F-2528-4dcc-9ED4-AC672D0EE8E6")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Author : ReferenceCountedObjectBase, IAuthor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Author()
        {
            //TODO: Implement your additional construction here
        }

        public string Name
        {
            get { return SwitchHardware.AuthorName; }
            set { SwitchHardware.AuthorName = value; }
        }

        public string Email
        {
            get { return SwitchHardware.AuthorEmail; }
            set { SwitchHardware.AuthorEmail = value; }
        }


    }
}
