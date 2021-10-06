using System;

namespace ASCOM.DynamicRemoteClients
{
    public class ShortArray2DResponse : ImageArrayResponseBase
    {
        private short[,] shortArray2D;

        private const int RANK = 2;
        private const SharedConstants.ImageArrayElementTypes TYPE = SharedConstants.ImageArrayElementTypes.Int16;

        public ShortArray2DResponse(uint clientTransactionID, uint transactionID, short[,] value)
        {
            base.ServerTransactionID = transactionID;
            shortArray2D = value;
            base.Type = (int)TYPE;
            base.Rank = RANK;
            base.ClientTransactionID = clientTransactionID;
        }

        public short[,] Value
        {
            get { return shortArray2D; }
            set
            {
                shortArray2D = value;
                base.Type = (int)TYPE;
                base.Rank = RANK;
            }
        }
    }
}
