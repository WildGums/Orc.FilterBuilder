// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionGroupType.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Catel.ComponentModel;

    public enum ConditionGroupType
    {
        [DisplayName("FilterBuilder_And")]
        And,
        [DisplayName("FilterBuilder_Or")]
        Or
    }
}
