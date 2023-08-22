using System;
using System.Runtime.InteropServices;
using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
    [ComVisible( false )]
	public class DeviceDriverBase : ReferenceCountedObjectBase
	{
		#region Common Fields

		/// <summary>
		/// ASCOM DeviceID (COM ProgID) for this driver.
		/// The DeviceID is used by ASCOM applications to load the driver at runtime.
		/// </summary>
		protected string _driverID;

		/// <summary>
		/// Driver description that displays in the ASCOM Chooser.
		/// </summary>
		protected string _driverDescription;

		protected const string _done = " (done)";
		protected const string _failed = " (failed)";

		/// <summary>
		/// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
		/// </summary>
		protected TraceLogger _logger;

		#endregion Common Fields

		#region Protected Methods

		protected void CheckConnected( string ident, bool isConnected )
		{
			if ( !isConnected )
			{
				LogMessage( ident, "The driver must be connected." );
				throw new NotConnectedException( ident );
			}
		}

		protected void CheckParked( string ident, bool parkState )
		{
			if ( parkState )
			{
				LogMessage( ident, "Invalid operation when parked." );

				throw new ParkedException( ident );
			}
		}

		protected void CheckParkingStatus( string ident, ParkingStateEnum actualState, ParkingStateEnum expectedState )
		{
			if ( actualState != expectedState )
			{
				string msg = $"Invalid operation when the parking status is {actualState}.";
				LogMessage( ident, msg );

				if ( actualState == ParkingStateEnum.ParkInProgress )
				{
					throw new InvalidOperationException( $"{ident} not allowed when the parking status is {actualState}." );
				}
				else if ( actualState == ParkingStateEnum.IsAtPark )
				{
					throw new ParkedException( $"{ident} not allowed when the telescope is AtPark." );
				}
			}
		}

		protected void CheckCapabilityForMethod( string ident, string capabilityName, bool capabilityValue )
		{
			if ( !capabilityValue )
			{
				string msg = $"{capabilityName} = {capabilityValue} prevents the operation.";
				LogMessage( ident, msg );

				throw new MethodNotImplementedException( ident, msg );
			}
		}

		protected void CheckCapabilityForProperty( string ident, string capabilityName, bool capabilityValue )
		{
			if ( !capabilityValue )
			{
				string msg = $"{capabilityName} = {capabilityValue} prevents the operation.";
				LogMessage( ident, msg );

				throw new PropertyNotImplementedException( ident, msg );
			}
		}

		protected void CheckRange( string ident, double value, double min, double max )
		{
			if ( double.IsNaN( value ) )
			{
				LogMessage( ident, "The value is not defined." );
				throw new ValueNotSetException( ident );
			}

			if ( value < min || value > max )
			{
				LogMessage( ident, "{0} is out of range from {1} to {2}", value, min, max );

				throw new InvalidValueException( ident, $"{value:f5}", $"{min:f5} to {max:f5}" );
			}
		}

		protected void CheckRange( string ident, string valueProperty, double value, double min, double max )
		{
			if ( double.IsNaN( value ) )
			{
				LogMessage( ident, "{0} value has not been set", valueProperty );

				throw new ValueNotSetException( ident );
			}

			if ( value < min || value > max )
			{
				LogMessage( ident, $"{valueProperty} = {value:f5} is out of range from {min:f5} to {max:f5}" );

				throw new InvalidValueException( ident, $"{valueProperty} = {value:f5}", $"{min:f5} to {max:f5}" );
			}
		}

		protected virtual void LogMessage( string identifier, string message, params object[] args )
		{
			if ( _logger != null && _logger.Enabled )
			{
				var msg = String.Format( message, args );

				_logger.LogMessage( identifier, msg );
			}
		}

		protected virtual void LogMessage( string identifier, string message )
		{
			if ( _logger != null && _logger.Enabled )
			{
				_logger.LogMessage( identifier, message );
			}
		}

		#endregion Protected Methods
	}
}
