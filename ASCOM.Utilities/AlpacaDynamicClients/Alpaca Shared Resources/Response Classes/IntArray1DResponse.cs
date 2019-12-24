﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Remote
{
    public class IntArray1DResponse : RestResponseBase
    {
        private int[] intArray1D;

        public IntArray1DResponse() { }

        public IntArray1DResponse(uint clientTransactionID, uint transactionID, int[] value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            intArray1D = value;
        }

        public int[] Value
        {
            get { return intArray1D; }
            set { intArray1D = value; }
        }
    }
}
