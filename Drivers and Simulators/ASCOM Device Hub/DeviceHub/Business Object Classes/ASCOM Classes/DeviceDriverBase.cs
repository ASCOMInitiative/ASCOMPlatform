using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

		#endregion Common Fields

		/// <summary>
		/// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
		/// </summary>
		protected TraceLogger _logger;

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

		protected void CheckCapabilityForMethod( string ident, string capabilityName, bool capabilityValue )
		{
			if ( !capabilityValue )
			{
				LogMessage( ident, "{0} = {1} prevents the operation.", capabilityName, capabilityValue );

				throw new MethodNotImplementedException( ident );
			}
		}

		protected void CheckCapabilityForProperty( string ident, string capabilityName, bool capabilityValue )
		{
			if ( !capabilityValue )
			{
				LogMessage( ident, "{0} = {1} prevents the operation.", capabilityName, capabilityValue );

				throw new PropertyNotImplementedException( ident );
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

				throw new InvalidValueException( ident, value.ToString( CultureInfo.CurrentCulture ), String.Format( CultureInfo.CurrentCulture, "{0} to {1}", min, max ) );
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
				LogMessage( ident, "{0} = {1} is out of range from {2} to {3}", valueProperty, value, min, max );

				throw new InvalidValueException( ident, valueProperty + " = " + value.ToString( CultureInfo.CurrentCulture ), string.Format( CultureInfo.CurrentCulture, "{0} to {1}", min, max ) );
			}
		}

		protected virtual void LogMessage( string identifier, string message, params object[] args )
		{
			if ( _logger != null && _logger.Enabled )
			{
				var msg = string.Format( CultureInfo.CurrentCulture, message, args );
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
	}
}
