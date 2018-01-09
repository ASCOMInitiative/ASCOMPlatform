using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.TrivialSimulator
	{
	public class SingleSwitch
		{
		private static int instanceCount = 0;
		public bool State { get; set; }
		public string Name { get; set; }
		public SingleSwitch()
			{
			lock (this)
				{
				++instanceCount;
				State = false;
				Name = String.Format("Switch {0}", instanceCount);
				}
			}
		}
	}
