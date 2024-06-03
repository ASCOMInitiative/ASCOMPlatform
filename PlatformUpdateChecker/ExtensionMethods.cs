using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformUpdateChecker
{
    internal static class ExtensionMethods
    {
        internal static bool IsNewerThan(this SemVersion newVersion, SemVersion comparisonSemver)
        {
            return SemVersion.ComparePrecedence(newVersion, comparisonSemver) == 1;
        }
        internal static bool IsOlderThan(this SemVersion oldVersion, SemVersion comparisonSemver)
        {
            return SemVersion.ComparePrecedence(oldVersion, comparisonSemver) == -1;
        }
    }
}
