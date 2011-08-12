
//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini _Speech class -- the actual text-to-voice class 
//
// Description:	This implements speaking functions for Gemini driver
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
using SpeechLib;

namespace ASCOM.GeminiTelescope
{
    class _Speech
    {

        static private SpVoice m_SPV = null;
        static public Speech.SpeechType m_Filter = Speech.SpeechType.Announcement | Speech.SpeechType.Error | Speech.SpeechType.Command | Speech.SpeechType.Information | Speech.SpeechType.Status;

        private static bool m_Speak = false;

        public static Speech.SpeechType Filter
        {
            get { return m_Filter; }
            set { m_Filter = value; }
        }

        public static string[] Voices
        {
            get
            {
                List<string> v = new List<string>();

                if (m_SPV != null)
                {
                    ISpeechObjectTokens voices = m_SPV.GetVoices("", "");
                    foreach (ISpeechObjectToken onev in voices)
                    {
                        v.Add(onev.GetDescription(0));
                    }
                }
                return v.ToArray();
            }
        }


        public  static bool SpeechInitialize(IntPtr win, string voice)
        {
            try
            {
                m_Speak = true;

                m_SPV = new SpVoice();
                if (!string.IsNullOrEmpty(voice))
                {
                    ISpeechObjectTokens voices = m_SPV.GetVoices("", "");
                    foreach (ISpeechObjectToken v in voices)
                    {
                        if (v.GetDescription(0) == voice)
                        {
                            m_SPV.Voice = (SpObjectToken)v;
                            break;
                        }
                    }
                }
                else if (voice == null)
                    m_Speak = false;

                return true;
            }
            catch {
                return false;
            }
        }

        public static bool SayIt(string s, Speech.SpeechType flags)
        {
            if (m_SPV!=null && (flags & (m_Filter|Speech.SpeechType.Always)) != 0 && m_Speak)
                try
                {
                    m_SPV.Speak(s, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    return true;
                }
                catch { }
            return false;
        }
    }
}
