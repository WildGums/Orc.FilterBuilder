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
                new Regex(pattern, RegexOptions.None).IsMatch(string.Empty);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
