
//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Speech class -- main text-to-voice wrapper class 
//
// Description:	This implements speaking functions for Gemini driver. This is a wrapper class for _Speech
//              This exists to catch the initialization exception if speech is not installed on this computer
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//              
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 29-SEP-2009	pk  1.0.0	Initial creation
// --------------------------------------------------------------------------------
//


using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.GeminiTelescope
{
    class Speech
    {
        public enum SpeechType
        {
            Announcement = 1,       // all baloon announcements
            Error = 2,              // all error messages displayed as pop-ups
            Information = 4,        // all information 
            Command = 8,            // all commands received from user/API
            Status = 16,            // status changes
            Always = 256           // say it always, no matter what flags are set
        };

        public static SpeechType Filter
        {
            get { return _Speech.Filter; }
            set { _Speech.Filter = value; }
        }

        public static string[] Voices
        {
            get
            {
                return _Speech.Voices;
            }
        }

        public static bool SpeechInitialize(IntPtr win, string voice)
        {
            try
            {
                return _Speech.SpeechInitialize(win, voice);
            }
            catch
            {
                return false;
            }
        }

        public static bool SayIt(string s, SpeechType flags)
        {
                try
                {
                    return _Speech.SayIt(s, flags);
                }
                catch { }
            return false;
        }

    }
}
