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
            Assert.False(TL.UseMutexSynchronisation);
            output.WriteLine($"UseMutexSynchronisation: {TL.UseMutexSynchronisation}");
        }

        [Fact]
        public void UseMutexSynchronisationFalse()
        {
            TraceLogger TL = new TraceLogger("","UseMutexSynchronisationFalse");
            TL.Enabled= true; 
            TL.UseMutexSynchronisation = false;

            TL.LogMessage("", $"UseMutexSynchronisation is {TL.UseMutexSynchronisation}");
            Assert.False(TL.UseMutexSynchronisation);
            TL.Dispose();
        }

        [Fact]
        public void UseMutexSynchronisationTrue()
        {
            TraceLogger TL = new TraceLogger("", "UseMutexSynchronisationTrue");
            TL.Enabled = true;
            TL.UseMutexSynchronisation = true;

            TL.LogMessage("", $"UseMutexSynchronisation is {TL.UseMutexSynchronisation}");
            Assert.True(TL.UseMutexSynchronisation);
            TL.Dispose();
        }
    }
}
