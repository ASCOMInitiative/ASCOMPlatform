﻿//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	Trace switches to control the debug tracing
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//
using System.Diagnostics;

namespace ASCOM.Simulator.Utils
{
    public static class TraceSwitches
	{
		public static TraceSwitch DebugTracing;

		private static object syncLock = new object();

		static TraceSwitches()
		{
			lock (syncLock)
			{
				DebugTracing = new TraceSwitch("DebugTracing", "Determines the level of debug tracing", "Error");
			}
		}
	}
}
