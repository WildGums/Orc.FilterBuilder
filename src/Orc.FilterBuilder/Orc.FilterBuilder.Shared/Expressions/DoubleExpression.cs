// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class DoubleExpression : NumericExpression<double>
    {
        #region Constructors
        public DoubleExpression()
            : this(true)
        {
        }

        public DoubleExpression(bool isNullable)
        {
            IsDecimal = true;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Double;
        }
        #endregion



        public override object Clone()
        {
            return new DoubleExpression(IsNullable) { Value = this.Value, SelectedCondition = this.SelectedCondition };
        }
    }
}