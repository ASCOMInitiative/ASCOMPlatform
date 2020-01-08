using System;

namespace ASCOM.DynamicRemoteClients
{
    public class DoubleArray2DResponse : ImageArrayResponseBase
    {
        private double[,] doubleArray2D;

        private const int RANK = 2;
        private const SharedConstants.ImageArrayElementTypes TYPE = SharedConstants.ImageArrayElementTypes.Double;

        public DoubleArray2DResponse(uint clientTransactionID, uint transactionID, double[,] value)
        {
            base.ServerTransactionID = transactionID;
            doubleArray2D = value;
            base.Type = (int)TYPE;
            base.Rank = RANK;
            base.ClientTransactionID = clientTransactionID;
        }

        public double[,] Value
        {
            get { return doubleArray2D; }
            set
            {
                doubleArray2D = value;
                base.Type = (int)TYPE;
                base.Rank = RANK;
            }
        }
    }
}
