using System;

namespace ASCOM.DynamicRemoteClients
{
    public class IntArray2DResponse : ImageArrayResponseBase
    {
        private int[,] intArray2D;

        private const int RANK = 2;
        private const SharedConstants.ImageArrayElementTypes TYPE = SharedConstants.ImageArrayElementTypes.Int32;

        public IntArray2DResponse(uint clientTransactionID, uint transactionID)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
        }

        public int[,] Value
        {
            get { return intArray2D; }
            set
            {
                intArray2D = value;
                base.Type = (int)TYPE;
                base.Rank = RANK;
            }
        }
    }
}
