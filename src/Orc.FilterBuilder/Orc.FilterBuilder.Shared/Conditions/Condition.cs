// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Condition.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.ComponentModel.DataAnnotations;
using Orc.FilterBuilder.Properties;

namespace Orc.FilterBuilder
{
    public enum Condition
    {
        [Display(Name = "Contains", ResourceType = typeof(Strings))]
        Contains,
        [Display(Name = "StartsWith", ResourceType = typeof(Strings))]
        StartsWith,
        [Display(Name = "EndsWith", ResourceType = typeof(Strings))]
        EndsWith,
        [Display(Name = "EqualTo", ResourceType = typeof(Strings))]
        EqualTo,
        [Display(Name = "NotEqualTo", ResourceType = typeof(Strings))]
        NotEqualTo,
        [Display(Name = "GreaterThan", ResourceType = typeof(Strings))]
        GreaterThan,
        [Display(Name = "LessThan", ResourceType = typeof(Strings))]
        LessThan,
        [Display(Name = "GreaterThanOrEqualTo", ResourceType = typeof(Strings))]
        GreaterThanOrEqualTo,
        [Display(Name = "LessThanOrEqualTo", ResourceType = typeof(Strings))]
        LessThanOrEqualTo,
        [Display(Name = "IsEmpty", ResourceType = typeof(Strings))]
        IsEmpty,
        [Display(Name = "NotIsEmpty", ResourceType = typeof(Strings))]
        NotIsEmpty,
        [Display(Name = "IsNull", ResourceType = typeof(Strings))]
        IsNull,
        [Display(Name = "NotIsNull", ResourceType = typeof(Strings))]
        NotIsNull
    }
}