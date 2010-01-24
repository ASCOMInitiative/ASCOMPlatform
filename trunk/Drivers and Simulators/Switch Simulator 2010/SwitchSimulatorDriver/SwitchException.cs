using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.SwitchSimulator
{
    /// <summary>
    /// Summary description for UnsupportedOperationException.
    /// </summary>
    public class NotImplementedException : Exception
    {
        private string message;


        public NotImplementedException()
            : base()
        { }

        public NotImplementedException(string message)
            : base(message)
        {
            this.message = message;
        }

        public override string Message
        {
            get
            {
                return this.message;
            }
        }
    }
}
