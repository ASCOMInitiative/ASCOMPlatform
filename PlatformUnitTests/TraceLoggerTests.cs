using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PlatformUnitTests
{
    public class TraceLoggerTests
    {
        private readonly ITestOutputHelper output;

        public TraceLoggerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void UseMutexSynchronisationDefault()
        {
            TraceLogger TL = new TraceLogger();
            Assert.False(TraceLogger.UseMutexSynchronisation);
            output.WriteLine($"UseMutexSynchronisation: {TraceLogger.UseMutexSynchronisation}");
        }

        [Fact]
        public void UseMutexSynchronisationFalse()
        {
            TraceLogger TL = new TraceLogger("","UseMutexSynchronisationFalse");
            TL.Enabled= true; 
            TraceLogger.UseMutexSynchronisation = false;

            TL.LogMessage("", $"UseMutexSynchronisation is {TraceLogger.UseMutexSynchronisation}");
            Assert.False(TraceLogger.UseMutexSynchronisation);
            TL.Dispose();
        }

        [Fact]
        public void UseMutexSynchronisationTrue()
        {
            TraceLogger TL = new TraceLogger("", "UseMutexSynchronisationTrue");
            TL.Enabled = true;
            TraceLogger.UseMutexSynchronisation = true;

            TL.LogMessage("", $"UseMutexSynchronisation is {TraceLogger.UseMutexSynchronisation}");
            Assert.True(TraceLogger.UseMutexSynchronisation);
            TL.Dispose();
        }
    }
}
