using System;

namespace ASCOM.DeviceHub
{
	public interface IFocuserManager
	{
		bool IsConnected { get; }
		string ConnectError { get; }
		Exception ConnectException { get; }

		bool Connect( string focuserID );
		void Disconnect();

		void MoveFocuserBy( int amount );
		void HaltFocuser();
		void SetTemperatureCompensation( bool state );
	}
}
