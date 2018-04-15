using System;
using System.Globalization;
using System.Threading;

namespace VisualDocker.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsLoose(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool ContainsLoose(this string str1, string str2)
        {
            return Thread.CurrentThread.CurrentCulture.CompareInfo.IndexOf(str1, str2, CompareOptions.IgnoreCase) >= 0;
        }
    }
}