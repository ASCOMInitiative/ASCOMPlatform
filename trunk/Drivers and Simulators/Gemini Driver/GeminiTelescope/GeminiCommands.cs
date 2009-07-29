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

        internal GeminiCommand(ResultType type, int chars)
        {
            Type = type;
            Chars = chars;
        }

        public ResultType Type; // expected return type
        public int Chars;   // expected number of characters if Type=NumberofChars
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
        /// Dictionary of all the exception commands and their expected result types
        /// </summary>
        public static Dictionary<string, GeminiCommand> Commands = new Dictionary<string, GeminiCommand>();
        static GeminiCommands()
        {
            Commands.Add(":Gv", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1)); //N/T/G//C/S are possible return values
            Commands.Add(":h?", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1)); //0/1/2
            Commands.Add(":MA", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0)); //0 or # terminated string
            Commands.Add(":Mf", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0));
            Commands.Add(":MM", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0));                       
            Commands.Add(":MS", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0));                        
            Commands.Add(":P", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 14)); // "LOW  PRECISION" or "HIGH PRECISION"
            Commands.Add(":SA", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":SC", new GeminiCommand(GeminiCommand.ResultType.ZeroOrHash, 0));                        
            Commands.Add(":SE", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":SG", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":SL", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":Sd", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":SM", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":SN", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":SO", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":SP", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":Sg", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":Sp", new GeminiCommand(GeminiCommand.ResultType.OneOrHash, 1));                        
            Commands.Add(":Sr", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":St", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":Sw", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
            Commands.Add(":Sz", new GeminiCommand(GeminiCommand.ResultType.NumberofChars, 1));                        
        }

    }
}
