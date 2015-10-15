// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionGroupType.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.ComponentModel.DataAnnotations;
using Orc.FilterBuilder.Properties;

namespace Orc.FilterBuilder
{
    public enum ConditionGroupType
    {
        [Display(Name = "And", ResourceType = typeof(Strings))]
        And,
        [Display(Name = "Or", ResourceType = typeof(Strings))]
        Or
    }
}