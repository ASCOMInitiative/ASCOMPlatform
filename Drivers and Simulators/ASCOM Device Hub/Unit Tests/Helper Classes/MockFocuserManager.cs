using System;
using System.Threading;
using System.Threading.Tasks;
using ASCOM.DeviceHub;
using ASCOM.DeviceHub.MvvmMessenger;

namespace Unit_Tests
{
	public class MockFocuserManager : IFocuserManager
	{
		public MockFocuserManager()
		{
			IsConnected = false;
			MockTemperature = 20.0;
		}

		public string MockFocuserID { get; set; }
		public string MockFocuserName { get; set; }

		public bool MockIsConnected { get; set; }
		public int MockPosition { get; set; }
		public double MockTemperature { get; set; }
		public bool MockTemperatureCompensation { get; set; }

		public FocuserParameters Parameters { get; set; }
		public DevHubFocuserStatus Status { get; set; }

		public string FocuserID => MockFocuserID;
		public string FocuserName => MockFocuserName;

		public string ConnectError { get; private set; }
		public Exception ConnectException { get; private set; }

		public bool IsConnected
		{
			get => MockIsConnected;
			set => MockIsConnected = value;
		}

		public bool Connect( string focuserID )
		{
			MockFocuserID = focuserID;
			IsConnected = true;

			return IsConnected;
		}

		public void Disconnect()
		{
			IsConnected = false;
		}

		public void HaltFocuser()
		{
			Status = DevHubFocuserStatus.GetEmptyStatus();
			Status.Connected = true;
			Status.Link = true;
			Status.IsMoving = false;

			if ( Parameters.Absolute )
			{
				Status.Position = MockPosition + 100;
			}

			Messenger.Default.Send( new FocuserStatusUpdatedMessage( Status ) );
		}

		public void MoveFocuserBy( int amount )
		{
			Status = DevHubFocuserStatus.GetEmptyStatus();
			Status.Connected = true;
			Status.Link = true;

			if ( Parameters.Absolute )
			{
				Status.Position = MockPosition;
			}

			Status.Temperature = MockTemperature;
			Status.IsMoving = true;

			Messenger.Default.Send( new FocuserStatusUpdatedMessage( Status ) );

			Thread.Sleep( 1500 );

			Status.IsMoving = false;

			if ( Parameters.Absolute )
			{
				amount = Math.Max( Math.Min( amount, Parameters.MaxIncrement ), -Parameters.MaxIncrement );

				int newPosition = MockPosition + amount;

				newPosition = Math.Max( Math.Min( newPosition, Parameters.MaxStep ), -Parameters.MaxStep );

				Status.Position = newPosition;
			}

			Messenger.Default.Send( new FocuserStatusUpdatedMessage( Status ) );
		}

		public void SetTemperatureCompensation( bool state )
		{
			MockTemperatureCompensation = state;
		}
	}
}
