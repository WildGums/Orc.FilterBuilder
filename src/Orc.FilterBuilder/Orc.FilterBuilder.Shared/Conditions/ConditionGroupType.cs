// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionGroupType.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Catel.ComponentModel;
using Orc.FilterBuilder.Properties;

namespace Orc.FilterBuilder
{
    public enum ConditionGroupType
    {
        [DisplayName("And")]
        And,
        [DisplayName("Or")]
        Or
    }
}