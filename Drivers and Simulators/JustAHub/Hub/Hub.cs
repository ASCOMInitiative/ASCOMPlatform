using ASCOM.JustAHub;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.JustAHub
{
    internal static class Hub
    {
        internal static T GetValue<T>(Func<T> interfacemember, Action<string, string> logMethod, [CallerMemberName] string callerMemberName = "UnknownCaller")
        {
            try
            {
                T result = interfacemember();
                logMethod(callerMemberName, result.ToString());
                return result;
            }
            catch (Exception ex)
            {
                logMethod(callerMemberName, $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
        internal static void CallMember(Action interfacemember, Action<string, string> logMethod, [CallerMemberName] string callerMemberName = "UnknownCaller")
        {
            try
            {
                logMethod(callerMemberName, $"Calling {callerMemberName}...");
                interfacemember();
                logMethod(callerMemberName, $"{callerMemberName} returned OK");
            }
            catch (Exception ex)
            {
                logMethod(callerMemberName, $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }
}
