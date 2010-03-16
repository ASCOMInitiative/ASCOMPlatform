// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
	/// <summary>
	/// Implements a telescope class to access any registered ASCOM telescope
	/// </summary>
	[Guid("50FA7A37-95F2-41AD-9159-EE6258A1D820")]
	[ClassInterface(ClassInterfaceType.None)]
	public class TelescopeV3 : Telescope, ITelescopeV3, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TelescopeV3"/> class.
		/// </summary>
		/// <param name="telescopeID">The ProgID for the telescope</param>
		public TelescopeV3(string telescopeID)
			: base(telescopeID)
		{
		}

		#region IDisposable Members
		/// <summary>
		/// Dispose the late-bound interface, if needed. Will release it via COM
		/// if it is a COM object, else if native .NET will just dereference it
		/// for GC.
		/// </summary>
		public void Dispose()
		{
			base.Dispose();
		}

		#endregion

		#region IAscomDriver Members
		// All of the members for this interface are inherited from the original Telescope class.
		// So no need to implement anything here.
		#endregion

		#region ITelescopeV3 Members
		// ITelescopeV3 has no new members over ITelescope (V2)
		#endregion

		#region IDeviceControl Members

		public string Action(string ActionName, string ActionParameters)
		{
			throw new NotImplementedException();
		}

		public string LastResult
		{
			get { throw new NotImplementedException(); }
		}

		public string[] SupportedActions
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}