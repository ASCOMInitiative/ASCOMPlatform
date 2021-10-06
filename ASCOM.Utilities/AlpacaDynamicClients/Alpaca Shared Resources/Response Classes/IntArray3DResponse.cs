using System;

namespace ASCOM.DynamicRemoteClients
{
    public class IntArray3DResponse : ImageArrayResponseBase
    {
        private int[,,] intArray3D;

        private const int RANK = 3;
        private const SharedConstants.ImageArrayElementTypes TYPE = SharedConstants.ImageArrayElementTypes.Int32;

        public IntArray3DResponse(uint clientTransactionID, uint transactionID)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
        }

        public int[,,] Value
        {
            get { return intArray3D; }
            set
            {
                intArray3D = value;
                base.Type = (int)TYPE;
                base.Rank = RANK;
            }
        }
    }
}
