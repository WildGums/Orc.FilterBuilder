// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegexHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Catel;
    using System;
    using System.Collections;
    using System.Text.RegularExpressions;

    public static class RegexHelper
    {
        public static bool IsValid(string pattern)
        {
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
