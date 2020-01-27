using System;

namespace ASCOM.DeviceHub
{
	public interface IDomeManager
    {
		bool IsConnected { get; }
		string ConnectError { get; }
		Exception ConnectException { get; }
		bool IsScopeReadyToSlave { get; }
		bool IsDomeReadyToSlave { get; }

		bool Connect( string scopeID );
		void Disconnect();
		void OpenDomeShutter();
		void CloseDomeShutter();
		void SlewDomeShutter( double targetAltitude );
		void StopDomeMotion();
		void ParkTheDome();
		void FindHomePosition();
		void SlewDomeToAzimuth( double targetAzimuth );
		void SyncDomeToAzimuth( double azimuth );
		void SetSlavedState( bool state );
	}
}
