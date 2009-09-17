using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.GeminiTelescope
{

    /// <summary>
    /// A single command description for the table of exception commands
    /// </summary>
    
    internal class GeminiCommand
    {
        /// <summary>
        /// What type of a return this command expects
        /// </summary>
        internal enum ResultType 
        {
            NoResult,       // no result expected
            HashChar,       // hash-terminated ('#') string
            ZeroOrHash,     // character '0' (zero) or a hash ('#') terminated string
            OneOrHash,      // character '1' (one) or a hash ('#') terminated string
            NumberofChars   // specific number of characters
        }

        internal GeminiCommand(ResultType type, int chars) : this(type, chars, false)
        {
        }

        internal GeminiCommand(ResultType type, int chars, bool bUpdateStatus)
        {
            Type = type;
            Chars = chars;
            UpdateStatus = bUpdateStatus;
        }


        public ResultType Type; // expected return type
        public int Chars;   // expected number of characters if Type=NumberofChars
        public bool UpdateStatus; // this command changes mount status, and an update to polled variables should follow immediately
    }

    /// <summary>
    /// Class contains a dictionary of all the commands with "exceptional" return values
    ///   each Gemini command that expects a non-standard return (for example, one not terminated by '#' character)
    ///   is listed here, along with the type of return required.
    ///   
    /// </summary>
    static class GeminiCommands
    {
        /// <summary>
        /// Dictionary of all the supported Geimini commands and their expected result types
        ///   anything not in this table will either be expected not to return a value, or
        ///   return a '#' terminated string (by default)
        ///   
        /// </summary>
        public static Dictionary<string, GeminiCommand> Commands = new Dictionary<string, GeminiCommand>();
        static GeminiCommands()
        {
            Commands.Add("\x6", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":Cm", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":CM", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            
            Commands.Add(":F+", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0)); 
            Commands.Add(":F-", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0)); 
            Commands.Add(":FQ", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0)); 
            Commands.Add(":FF", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0)); 
            Commands.Add(":FM", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0)); 
            Commands.Add(":FS", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0)); 

            Commands.Add(":GA", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":GB", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":GC", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":Gc", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":GD", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":GE", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":GG", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":Gg", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":GH", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0)); 
            Commands.Add(":GL", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));

            Commands.Add(":Gm", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GM", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GN", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GO", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GP", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GR", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GS", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":Gt", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GV", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GVD", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));

            Commands.Add(":GVN", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GVP", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":GVT", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));

            Commands.Add(":Gv", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1)); //N/T/G//C/S are possible return values
            Commands.Add(":GZ", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":hP", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":hC", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":hN", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":hW", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":h?", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1)); //0/1/2

            Commands.Add(":MA", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0, true)); //0 or # terminated string

            Commands.Add(":MF", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0, true));
            Commands.Add(":ML", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":Ml", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":Mf", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":MM", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0, true));
            Commands.Add(":MS", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0, true));
            Commands.Add(":Me", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":Mw", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":Mn", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":Ms", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));

            Commands.Add(":mi", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":mm", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));

            Commands.Add(":Ma", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":Mi", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":Mg", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":OC", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":OI", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));

            Commands.Add(":ON", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":OR", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":OS", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":Oc", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));

            Commands.Add(":Od", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":On", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":Or", new GeminiCommand(GeminiCommand.ResultType.HashChar, 0));
            Commands.Add(":Os", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));

            Commands.Add(":p0", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":p1", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":p2", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":p3", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));

            Commands.Add(":P", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 14)); // "LOW  PRECISION" or "HIGH PRECISION"

            Commands.Add(":U", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));

            Commands.Add(":Q", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":Qe", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":Qw", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":Qn", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));
            Commands.Add(":Qs", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0, true));

            Commands.Add(":RC", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":RG", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":RM", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":RS", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));

            Commands.Add(":Sa", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
  
            Commands.Add(":SB", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));
            Commands.Add(":SC", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0, true));
            Commands.Add(":SE", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));
            Commands.Add(":SG", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1, true));
            Commands.Add(":SL", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1, true));
            Commands.Add(":Sd", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));
            Commands.Add(":SM", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));
            Commands.Add(":SN", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));
            Commands.Add(":SO", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));
            Commands.Add(":SP", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1, true));
            Commands.Add(":Sg", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));
            Commands.Add(":Sp", new GeminiCommand(GeminiCommand.ResultType.OneOrHash, 1));
            Commands.Add(":Sr", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));
            Commands.Add(":St", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1, true));
            Commands.Add(":Sw", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));
            Commands.Add(":Sz", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));

            Commands.Add(":W", new GeminiCommand(GeminiCommand.ResultType.NoResult, 0));                         
        }

    }
}
