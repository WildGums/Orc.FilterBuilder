// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsignedLongExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedLongExpression : NumericExpression<ulong>
    {
        public UnsignedLongExpression()
            : this(true)
        {
        }

        public UnsignedLongExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = false;
            ValueControlType = ValueControlType.UnsignedLong;
        }
    }
}
