// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class NumericExpression : NumericExpression<double>
    {
        public NumericExpression()
        {
            IsDecimal = true;
            IsSigned = true;
            ValueControlType = ValueControlType.Numeric;
        }

        public NumericExpression(Type type)
            : this()
        {
            IsNullable = type.IsNullable();
        }

        public override object Clone()
        {
            return new NumericExpression() { Value = this.Value, SelectedCondition = this.SelectedCondition, IsNullable = this.IsNullable};
        }
    }
}