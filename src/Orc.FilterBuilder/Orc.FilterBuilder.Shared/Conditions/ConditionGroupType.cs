// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionGroupType.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Catel.ComponentModel;

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