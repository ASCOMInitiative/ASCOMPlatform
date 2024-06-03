using System;
using System.Collections.ObjectModel;

using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public interface ITelescopeManager
	{
		bool IsConnected { get; }
		string ConnectError { get; }
		Exception ConnectException { get; }
		ObservableCollection<JogDirection> JogDirections { get; }

		bool ConnectTelescope();
		bool ConnectTelescope( string scopeID, bool interactiveConnect = true );
		void DisconnectTelescope( bool interactiveDisconnect = false );
		void StartJogScope( int ndx, double rate );
		void StopJogScope( int ndx );
		void StopJogScope( TelescopeAxes axis );
		void StartFixedSlew( int ndx, double distance );
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
		void SetTargetDeclination( double targetDec );
		void SetTargetRightAscension( double targetRa );
	}
}
