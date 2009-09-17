using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ASCOM.TrivialSimulator
	{
	public class SwitchCollection : List<SingleSwitch>
		{
		/// <summary>
		/// Initialise a switch collection, with the
		/// specified number of switches.
		/// </summary>
		/// <param name="numSwitches"></param>
		public SwitchCollection(int numSwitches)
			{
			for (int i = 0; i < numSwitches; i++)
				{
				this.Add(new SingleSwitch());
				}
			}
		/// <summary>
		/// Initialise a switch collection, using the default of 4 switches.
		/// </summary>
		public SwitchCollection() : this(4) { }
		}
	}
