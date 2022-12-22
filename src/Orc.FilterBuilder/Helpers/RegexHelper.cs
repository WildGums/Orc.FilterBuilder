namespace Orc.FilterBuilder
{
    using System;
    using System.Text.RegularExpressions;

    public static class RegexHelper
    {
        public static bool IsValid(string pattern)
        {
            ArgumentNullException.ThrowIfNull(pattern);

            try
            {
                new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1)).IsMatch(string.Empty);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
