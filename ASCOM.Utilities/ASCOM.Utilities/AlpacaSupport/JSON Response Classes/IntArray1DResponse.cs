
namespace ASCOM.Utilities
{
    internal class IntArray1DResponse : RestResponseBase
    {

        private int[] intArray1D;

        public IntArray1DResponse()
        {
        }

        public IntArray1DResponse(uint clientTransactionID, uint transactionID, int[] value)
        {
            ServerTransactionID = transactionID;
            ClientTransactionID = clientTransactionID;
            intArray1D = value;
        }

        public int[] Value
        {
            get
            {
                return intArray1D;
            }
            set
            {
                intArray1D = value;
            }
        }
    }
}