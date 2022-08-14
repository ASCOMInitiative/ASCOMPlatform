using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
