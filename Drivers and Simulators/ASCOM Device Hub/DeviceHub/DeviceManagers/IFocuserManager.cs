using System;

namespace ASCOM.DeviceHub
{
	public interface IFocuserManager
	{
		bool IsConnected { get; }
		string ConnectError { get; }
		Exception ConnectException { get; }

		bool Connect();
		bool Connect( string focuserID, bool interactiveConnect = true );
		void Disconnect( bool interactiveDisconnect = false );

		void MoveFocuserBy( int amount );
		void HaltFocuser();
		void SetTemperatureCompensation( bool state );

	}
}
