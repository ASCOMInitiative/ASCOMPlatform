using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.Test
	{
	/// <summary>
	/// This interface simply aggregates the interfaces implemented by <see cref="ASCOM.Utilities.Profile"/>.
	/// We need this for dependency injection during unit testing.
	/// This interface is not to be used by application or driver developers, it is intended only for use
	/// within the ASCOM Platform for unit testing.
	/// </summary>
	public interface IProfileProvider : IProfile, IProfileExtra, IDisposable
		{
		}
	}
