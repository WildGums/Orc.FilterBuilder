// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SByteExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class SByteExpression : NumericExpression<sbyte>
    {
        public SByteExpression()
            : this(true)
        {
        }

        public SByteExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.SByte;
        }
    }
}
