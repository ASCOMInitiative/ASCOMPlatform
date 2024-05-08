using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformUpdateChecker
{
    public static class ExtensionMethods
    {
        public static bool IsNewerThan(this SemVersion newVersion, SemVersion comparisonSemver)
        {
            return SemVersion.ComparePrecedence(newVersion, comparisonSemver) == 1;
        }
        public static bool IsOlderThan(this SemVersion oldVersion, SemVersion comparisonSemver)
        {
            return SemVersion.ComparePrecedence(oldVersion, comparisonSemver) == -1;
        }
    }
}
