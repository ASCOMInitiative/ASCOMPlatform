using System;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public interface ITelescopeManager
	{
		bool IsConnected { get; }
		string ConnectError { get; }
		Exception ConnectException { get; }

		bool Connect( string scopeID );
		void Disconnect();
		void StartJogScope( MoveDirections jogDirection, JogRate jogRate );
		void StopJogScope( MoveDirections jogDirection );
		void StartFixedSlew( MoveDirections direction, double distance );
		void AbortDirectSlew();
		void SetParkingState( ParkingStateEnum desiredState );
		void DoSlewToCoordinates( double ra, double dec, bool useSynchronousMethodCall );
		void BeginSlewToCoordinatesAsync( double ra, double dec );
		void DoSlewToTarget( bool useSynchronousMethodCall );
		void BeginSlewToTargetAsync();
		void DoSlewToAltAz( double azimuth, double altitude, bool useSynchronousMethodCall );
		void BeginSlewToAltAzAsync( double azimuth, double altitude );
		void SetTracking( bool state );
		void StartMeridianFlip();
		void SetRaOffsetTrackingRate( double rate );
		void SetDecOffsetTrackingRate( double rate );
		void SetTrackingRate( DriveRates rate );
		void SetParkPosition();
	}
}
