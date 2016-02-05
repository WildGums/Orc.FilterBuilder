// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Condition.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.ComponentModel.DataAnnotations;
using Catel.ComponentModel;
using Orc.FilterBuilder.Properties;

namespace Orc.FilterBuilder
{
    public enum Condition
    {
        [DisplayName("Contains")]
        Contains,
        [DisplayName("StartsWith")]
        StartsWith,
        [DisplayName("EndsWith")]
        EndsWith,
        [DisplayName("EqualTo")]
        EqualTo,
        [DisplayName("NotEqualTo")]
        NotEqualTo,
        [DisplayName("GreaterThan")]
        GreaterThan,
        [DisplayName("LessThan")]
        LessThan,
        [DisplayName("GreaterThanOrEqualTo")]
        GreaterThanOrEqualTo,
        [DisplayName("LessThanOrEqualTo")]
        LessThanOrEqualTo,
        [DisplayName("IsEmpty")]
        IsEmpty,
        [DisplayName("NotIsEmpty")]
        NotIsEmpty,
        [DisplayName("IsNull")]
        IsNull,
        [DisplayName("NotIsNull")]
        NotIsNull
    }
}