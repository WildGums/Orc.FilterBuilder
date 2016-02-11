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
        [DisplayName("Starts With")]
        StartsWith,
        [DisplayName("Ends With")]
        EndsWith,
        [DisplayName("Equal To")]
        EqualTo,
        [DisplayName("Not Equal To")]
        NotEqualTo,
        [DisplayName("Greater Than")]
        GreaterThan,
        [DisplayName("Less Than")]
        LessThan,
        [DisplayName("Greater Than Or Equal To")]
        GreaterThanOrEqualTo,
        [DisplayName("Less Than Or Equal To")]
        LessThanOrEqualTo,
        [DisplayName("Is Empty")]
        IsEmpty,
        [DisplayName("Not Is Empty")]
        NotIsEmpty,
        [DisplayName("Is Null")]
        IsNull,
        [DisplayName("Not Is Null")]
        NotIsNull
    }
}