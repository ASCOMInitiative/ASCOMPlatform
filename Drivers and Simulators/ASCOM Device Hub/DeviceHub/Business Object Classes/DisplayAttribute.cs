using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
	class DisplayAttribute : Attribute
	{
		public string Name { get; set; }
	}
}
