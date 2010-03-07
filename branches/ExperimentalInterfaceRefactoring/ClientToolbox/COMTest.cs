using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.DriverAccess
{
    [ComVisible(true), Guid("1E0C7D4C-7860-41e4-9356-13BD70B50EF1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICOMTest
    {
        string Name { get; set; }
        double Value { get; set; }
    }

    [ComVisible(true), Guid("A4F1E671-DABF-43f9-A3F7-22AB004EFBE4"), ClassInterface(ClassInterfaceType.None)]
    public class COMTest : ICOMTest
    {
        public COMTest()
        {
        }


        #region ICOMTest Members

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
