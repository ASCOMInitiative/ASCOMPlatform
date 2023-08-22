namespace ASCOM.DeviceHub
{
    internal class FocuserMoveAmountMessage
	{
		public FocuserMoveAmountMessage( int moveAmount )
		{
			MoveAmount = moveAmount;
		}

		public int MoveAmount { get; private set; }
	}
}
