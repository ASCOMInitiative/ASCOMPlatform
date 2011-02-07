//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini UserCommand class
//
// Description:	This implements various button options for Gemini joystick button support
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//              
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 22-SEP-2009	pk  1.0.0	Initial creation
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Text;
using ASCOM.GeminiTelescope.Properties;

namespace ASCOM.GeminiTelescope
{
    public static class UserCommand
    {

        private static string[] m_FocuserCmds = { ":FF", ":FM", ":FS" };
        private static int m_FocuserSpeed = 0;

        private static bool m_InMenu = false;


        private static bool Cmd(string cm)
        {

            if (GeminiHardware.Connected)
            {
                string result = GeminiHardware.DoCommandResult(cm, GeminiHardware.MAX_TIMEOUT, false);
                return true;
            }
            return false;
        }

        /// <summary>
        /// issue a key press command to Gemini. This changes key state
        /// just for that key, the rest of the key states are preserved.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyDown"></param>
        /// <returns></returns>
        private static bool SendKeyCmd(int key, bool keyDown)
        {
            if (!GeminiHardware.Connected) return false;

            // get current state
            string curr = GeminiHardware.DoCommandResult(":T", GeminiHardware.MAX_TIMEOUT, false);

            if (curr == null) return false;

            int value;
            int cvalue = (int)(curr[0]);

            // bad value returned?
            if ((cvalue & 0x40) == 0) return false;

            value = (cvalue & ~key) | (keyDown ? key : 0);  // set the current key state into the current state from hand controller
            GeminiHardware.DoCommandResult(":k" + (char)value, GeminiHardware.MAX_TIMEOUT, false);
            return true;
        }

        private static bool ToggleMode()
        {
            if (!GeminiHardware.Connected)  return false;

            string res = GeminiHardware.DoCommandResult("<160:", GeminiHardware.MAX_TIMEOUT, false);
            if (res == null) return false;

            int curr;
            if (!int.TryParse(res, out curr)) return false;// ???

            curr++;
            if (curr > 163) curr = 161;
            if (curr < 161) curr = 161;
            GeminiHardware.DoCommandResult(">" + curr.ToString() + ":", GeminiHardware.MAX_TIMEOUT, false);
            return true;
        }

        /// <summary>
        /// Execute gemini function on a button press (joystick)
        /// </summary>
        /// <param name="func">function to execute</param>
        /// <param name="keyDown">true if key is pressed, false if it's released</param>
        /// <returns></returns>
        
        public static bool Execute(UserFunction func, bool keyDown)
        {
            switch (func)
            {
                case UserFunction.Unassigned: return true; //nothing to do

/*
                case UserFunction.GuidingSpeed: if (!keyDown) return Cmd(""); break;
                case UserFunction.CenteringSpeed: if (!keyDown) return Cmd(""); break;
                case UserFunction.SlewingSpeed: if (!keyDown) return Cmd(""); break;
                case UserFunction.ToggleSpeed: if (!keyDown) return Cmd(""); break;
 */
                case UserFunction.AccelerateSlew:
                    if (!keyDown)
                    {
                        int acc = GeminiHardware.JoystickFixedSpeed + 1;
                        if (acc > 2) acc = 2;
                        GeminiHardware.JoystickFixedSpeed = acc;
                        Speech.SayIt("Accelerate", Speech.SpeechType.Command);
                    }
                    break;
                case UserFunction.DecelerateSlew:
                    if (!keyDown)
                    {
                        int dec = GeminiHardware.JoystickFixedSpeed - 1;
                        if (dec < 0) dec = 0;
                        GeminiHardware.JoystickFixedSpeed = dec;
                        Speech.SayIt("Decelerate", Speech.SpeechType.Command);
                    }
                    break;
                case UserFunction.MeridianFlip: if (!keyDown) return Cmd(":Mf"); break;
                case UserFunction.HandUp: { SendKeyCmd(4, keyDown); if (keyDown) Speech.SayIt(Resources.UpKey, Speech.SpeechType.Command); return true; }
                case UserFunction.HandDown: { SendKeyCmd(2, keyDown); if (keyDown) Speech.SayIt(Resources.DownKey, Speech.SpeechType.Command); return true; }
                case UserFunction.HandLeft: { SendKeyCmd(1, keyDown); if (keyDown) Speech.SayIt(Resources.LeftKey, Speech.SpeechType.Command); return true; }
                case UserFunction.HandRight: { SendKeyCmd(8, keyDown); if (keyDown) Speech.SayIt(Resources.RightKey, Speech.SpeechType.Command); return true; }
                case UserFunction.HandMenu: 
                    //SendKeyCmd(16, keyDown);    //menu button doesn't work if Gemini is in local control mode!
                    if (!keyDown) {
                        m_InMenu = !m_InMenu;
                        if (m_InMenu) { Cmd(":HM"); Speech.SayIt(Resources.EnterMenu, Speech.SpeechType.Command); return true; }

                        else { Cmd(":Hm"); Cmd(":Hm"); Speech.SayIt(Resources.ExitMenu, Speech.SpeechType.Command); return true; }
                    }
                    break;
                case UserFunction.ParkCWD:
                    if (!keyDown)
                    {
                        GeminiHardware.DoParkAsync(GeminiHardware.GeminiParkMode.SlewCWD);
                        Speech.SayIt(Resources.ParkCWD, Speech.SpeechType.Command);
                        return true;
                    }
                    break;
                case UserFunction.GoHome: if (!keyDown)
                    {
                        Cmd(":hP"); Speech.SayIt(Resources.GoHome, Speech.SpeechType.Command);
                        return true;
                    } 
                    break;
                case UserFunction.StopSlew: if (keyDown) { Cmd(":Q"); Speech.SayIt(Resources.StopSlew, Speech.SpeechType.Command); return true; }
                    break;
                case UserFunction.StopTrack: if (keyDown) { Cmd(":hN"); Speech.SayIt(Resources.StopTracking, Speech.SpeechType.Command); return true; }  break;
                case UserFunction.StartTrack: if (keyDown) { Cmd(":hW"); Speech.SayIt(Resources.StartTracking, Speech.SpeechType.Command); return true; }  break;
                case UserFunction.AllSpeedMode: if (!keyDown) { Cmd(">163:"); Speech.SayIt(Resources.AllSpeedsMode, Speech.SpeechType.Command); return true; }  break;
                case UserFunction.PhotoMode: if (!keyDown) { Cmd(">162:"); Speech.SayIt(Resources.PhotoMode, Speech.SpeechType.Command); return true; }  break;
                case UserFunction.VisualMode: if (!keyDown) { Cmd(">161:"); Speech.SayIt(Resources.VisualMode, Speech.SpeechType.Command); return true; }  break;
                case UserFunction.ToggleMode: if (!keyDown) { ToggleMode(); Speech.SayIt("Toggle mode", Speech.SpeechType.Command); return true; } break;
                case UserFunction.FocuserIn:
                    if (keyDown) { Cmd(":F+"); Speech.SayIt("Focuser In", Speech.SpeechType.Command); return true;  }
                    else { Cmd(":FQ"); return true; }
                  
                case UserFunction.FocuserOut:
                    if (keyDown) { Cmd(":F-"); Speech.SayIt("Focuser Out", Speech.SpeechType.Command); return true; }
                    else { Cmd(":FQ");  return true; }
                 
                case UserFunction.FocuserFast: if (!keyDown) { m_FocuserSpeed = 0; Cmd(":FF");  Speech.SayIt(Resources.FocuserFast, Speech.SpeechType.Command); return true; } break;
                case UserFunction.FocuserMedium: if (!keyDown) { m_FocuserSpeed = 1; Cmd(":FM");  Speech.SayIt(Resources.FocuserMedium, Speech.SpeechType.Command); return true;  } break;
                case UserFunction.FocuserSlow: if (!keyDown) { m_FocuserSpeed = 2; Cmd(":FS");  Speech.SayIt(Resources.FocuserSlow, Speech.SpeechType.Command); return true; } break;
                case UserFunction.ToggleFocuserSpeed: 
                    if (!keyDown)
                    {
                        if (++m_FocuserSpeed > 2) m_FocuserSpeed = 0;
                        Cmd(m_FocuserCmds[m_FocuserSpeed]);
                        Speech.SayIt("Focuser Speed Toggle", Speech.SpeechType.Command); 
                        return true;
                    }
                    break;
                case UserFunction.Search2: if (!keyDown) { Cmd(":MF6"); Speech.SayIt(Resources.ObjectSearch2, Speech.SpeechType.Command); return true; } break;
                case UserFunction.Search1: if (!keyDown) { Cmd(":MF8"); Speech.SayIt(Resources.ObjectSearch1, Speech.SpeechType.Command); return true; } break;
                case UserFunction.Sync:
                    if (!keyDown){Cmd(":CM"); Speech.SayIt(Resources.Sync, Speech.SpeechType.Command); return true; }; break;
                case UserFunction.Align:
                    if (!keyDown) { Cmd(":Cm"); Speech.SayIt(Resources.Align, Speech.SpeechType.Command); return true; }; break;
                case UserFunction.LimitSwitchOff:
                    if (keyDown) { Cmd(">15:"); Speech.SayIt(Resources.LimitOff, Speech.SpeechType.Command); return true; }
                    else { Cmd(">14:"); Speech.SayIt(Resources.LimitOn, Speech.SpeechType.Command); return true; }
            }
            return false;
        }
    }

    public enum UserFunction
    {
        Unassigned = 0,
        GuidingSpeed = 1,
        CenteringSpeed = 2,
        SlewingSpeed = 3,
        ToggleSpeed = 4,
        MeridianFlip = 5,
        HandUp = 6,
        HandDown = 7,
        HandLeft = 8,
        HandRight = 9,
        HandMenu = 10,
        ParkCWD = 11,
        GoHome = 12,
        StopSlew = 13,
        StopTrack = 14,
        VisualMode = 15,
        AllSpeedMode = 16,
        PhotoMode = 17,
        ToggleMode = 18,
        FocuserIn = 19,
        FocuserOut = 20,
        FocuserFast = 21,
        FocuserMedium = 22,
        FocuserSlow = 23,
        ToggleFocuserSpeed = 24,
        Search2 = 25,
        Search1 = 26,
        StartTrack=27,
        AccelerateSlew=28,
        DecelerateSlew=29,
        Sync=30,
        Align=31,
        LimitSwitchOff=32
    }

}
