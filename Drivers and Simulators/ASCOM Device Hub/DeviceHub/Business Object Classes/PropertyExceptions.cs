using System;
using System.Collections.Generic;

namespace ASCOM.DeviceHub
{
	public class PropertyExceptions : ICloneable
	{
		private Dictionary<string, Exception> Exceptions;

		public PropertyExceptions()
		{
			Exceptions = new Dictionary<string, Exception>();
		}

		public PropertyExceptions( PropertyExceptions other )
		{
			this.Exceptions = new Dictionary<string, Exception>();

		}

		public Exception GetException( string propertyName )
		{
			Exception xcp = null;

			if ( Exceptions.ContainsKey( propertyName ) )
			{
				xcp = Exceptions[propertyName];
			}

			return xcp;
		}

		public void SetException( string propertyName, Exception xcp )
		{
			Exceptions[propertyName] = xcp;
		}

		#region ICloneable Methods

		object ICloneable.Clone()
		{
			return new PropertyExceptions( this );
		}

		public PropertyExceptions Clone()
		{
			return new PropertyExceptions( this );
		}

		#endregion ICloneable Methods
	}
}
