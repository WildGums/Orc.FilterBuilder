// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Condition.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    public enum Condition
    {
        Contains,
        StartsWith,
        EndsWith,
        EqualTo,
        NotEqualTo,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        IsEmpty,
        NotIsEmpty,
        IsNull,
        NotIsNull
    }
}