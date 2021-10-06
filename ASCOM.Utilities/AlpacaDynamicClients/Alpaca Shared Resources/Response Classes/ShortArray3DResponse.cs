using System;

namespace ASCOM.DynamicRemoteClients
{
    public class ShortArray3DResponse : ImageArrayResponseBase
    {
        private short[,,] shortArray3D;

        private const int RANK = 3;
        private const SharedConstants.ImageArrayElementTypes TYPE = SharedConstants.ImageArrayElementTypes.Int16;

        public ShortArray3DResponse(uint clientTransactionID, uint transactionID, short[,,] value)
        {
            base.ServerTransactionID = transactionID;
            shortArray3D = value;
            base.Type = (int)TYPE;
            base.Rank = RANK;
            base.ClientTransactionID = clientTransactionID;
        }

        public short[,,] Value
        {
            get { return shortArray3D; }
            set
            {
                shortArray3D = value;
                base.Type = (int)TYPE;
                base.Rank = RANK;
            }
        }
    }
}
