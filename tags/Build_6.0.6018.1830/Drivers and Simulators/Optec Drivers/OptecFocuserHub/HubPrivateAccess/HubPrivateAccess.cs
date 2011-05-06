using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.OptecFocuserHub
{
    [Guid("A1EBA941-D519-4D2D-9D4E-1B7C26947C76")]
    [ClassInterface(ClassInterfaceType.None)]   // This forces only the above interface to be visible
    public class HubPrivateAccess : ReferenceCountedObjectBase, IHubPrivateAccess
    {
        public HubPrivateAccess() 
        { }

        #region IHubPrivateAccess Members

        public string F1Nickname
        {
            get
            {
                return SharedResources.SharedFocuserManager.Focuser1.Nickname;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string F2Nickname
        {
            get
            {
                return "Name 2";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}

