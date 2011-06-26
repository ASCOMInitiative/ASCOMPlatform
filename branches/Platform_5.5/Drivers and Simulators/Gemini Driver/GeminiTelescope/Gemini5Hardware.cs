//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Telescope Hardware
//
// Description:	This implements a simulated Telescope Hardware
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//              (pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------------
// 24-MAY-2011	pk	1.0.19	Initial edit, created for Gemini-2 L5 functionality overrides
// --------------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Timers;
using System.IO.Ports;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;    
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using ASCOM.GeminiTelescope.Properties;
using System.Threading;

namespace ASCOM.GeminiTelescope
{

    public delegate void SiteChangedDelegate();
    public delegate void TimeChangedDelegate();
    public delegate void MountChangedDelegate();
    public delegate void DisplayChangedDelegate();
    public delegate void ModelChangedDelegate();
    public delegate void SpeedChangedDelegate();

    /// <summary>
    /// Class encapsulating all communications with Gemini L5
    /// </summary>
    public partial class Gemini5Hardware :  GeminiHardwareBase
    {
        // for L5, Gemini returns a set of bytes to indicate internal
        // variable changes. Six revision characters
        // 1: Site,
        // 2: Date/Time,
        // 3: Mount Parameter,
        // 4: Display content,
        // 5: Modelling parameters,
        // 6: Speeds

        internal string m_PreviousUpdateState = "000000";
        internal string m_CurrentUpdateState = "000000";

        public event SiteChangedDelegate OnSiteChanged;
        public event TimeChangedDelegate OnTimeChanged;
        public event MountChangedDelegate OnMountChanged;
        public event DisplayChangedDelegate OnDisplayChanged;
        public event ModelChangedDelegate OnModelChanged;
        public event SpeedChangedDelegate OnSpeedChanged;

        public Gemini5Hardware() : base()
        {
        }

        public override int MaxCommands
        {
            get
            {
                if (GeminiLevel >= 5 && EthernetPort)
                    return 25;
                else
                    return base.MaxCommands;
            }
        }        

        internal override void _UpdatePolledVariables()
        {
            if (GeminiLevel >= 5)
            {
                if (!CanPoll) return; //don't tie up the serial port while pulse guiding -- timing is critical!

                Trace.Enter("_UpdatePolledVariables5");
                try
                {
                    CommandItem command;
                    int timeout = 3000; // polling should not hold up the queue for too long
                    DiscardInBuffer(); //clear all received data
                    Transmit("<97:");
                    command = new CommandItem("<97:", timeout, true);
                    string change = GetCommandResult(command);
                    if (change != null && change.Length == 6)
                    {
                        m_CurrentUpdateState = change;
                    }
                }
                catch { }

                if (m_CurrentUpdateState != m_PreviousUpdateState) ProcessUpdates();

                base._UpdatePolledVariables();
            }
        }

        /// <summary>
        /// Fire update delegates if Gemini5 returned a change in status flag
        /// </summary>
        private void ProcessUpdates()
        {
            if (m_CurrentUpdateState[0] != m_PreviousUpdateState[0] && OnSiteChanged != null)
                ThreadPool.QueueUserWorkItem(new WaitCallback(a => OnSiteChanged()));
            if (m_CurrentUpdateState[1] != m_PreviousUpdateState[1] && OnTimeChanged != null)
                ThreadPool.QueueUserWorkItem(new WaitCallback(a => OnTimeChanged()));
            if (m_CurrentUpdateState[2] != m_PreviousUpdateState[2] && OnMountChanged != null)
                ThreadPool.QueueUserWorkItem(new WaitCallback(a => OnMountChanged()));
            if (m_CurrentUpdateState[3] != m_PreviousUpdateState[3] && OnDisplayChanged != null)
                ThreadPool.QueueUserWorkItem(new WaitCallback(a => OnDisplayChanged()));
            if (m_CurrentUpdateState[4] != m_PreviousUpdateState[4] && OnModelChanged != null)
                ThreadPool.QueueUserWorkItem(new WaitCallback(a => OnDisplayChanged()));
            if (m_CurrentUpdateState[5] != m_PreviousUpdateState[5] && OnSpeedChanged != null)
                ThreadPool.QueueUserWorkItem(new WaitCallback(a => OnSpeedChanged()));
            m_PreviousUpdateState = m_CurrentUpdateState;
        }
    }
}