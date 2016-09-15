// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShortExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class ShortExpression : NumericExpression<short>
    {
        #region Constructors
        public ShortExpression()
            : this(true)
        {
        }

        public ShortExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Short;
        }
        #endregion



        public override object Clone()
        {
            return new ShortExpression(IsNullable) { Value = this.Value, SelectedCondition = this.SelectedCondition };
        }
    }
}
