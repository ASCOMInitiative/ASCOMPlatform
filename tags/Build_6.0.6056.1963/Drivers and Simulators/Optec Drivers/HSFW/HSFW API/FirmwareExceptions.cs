using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OptecHID_FilterWheelAPI
{
    [Guid("1B1CF4B2-4A57-4845-838E-12E3199A6FCA")]
    public class FirmwareException : System.Exception
    {
        public FirmwareException(string message)
            : base(message)
        {
          
        }
    
        //public FirmwareException(string message)
        //    : base(message)
        //{
        //}

        //public FirmwareException(string message, System.Exception innerException)
        //    : base(message, innerException)
        //{
        //}

        //public FirmwareException(string message, ushort errorState)
        //    : base(message)
        //{
        //    _errorState = errorState;
        //}

        //public FirmwareException(string message, System.Exception innerException, ushort errorState)
        //    : base(message, innerException)
        //{
        //    _errorState = errorState;
        //}

        //public ushort ErrorState
        //{
        //    get { return _errorState; }
        //}




    }
}
