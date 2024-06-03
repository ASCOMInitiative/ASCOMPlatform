using System;

namespace ASCOM.DeviceHub
{
	public interface IFocuserManager
	{
		bool IsConnected { get; }
		string ConnectError { get; }
		Exception ConnectException { get; }

		bool ConnectFocuser();
		bool ConnectFocuser( string focuserID, bool interactiveConnect = true );
		void DisconnectFocuser( bool interactiveDisconnect = false );

		void MoveFocuserBy( int amount );
		void HaltFocuser();
		void SetTemperatureCompensation( bool state );

	}
}
