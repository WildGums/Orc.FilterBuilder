// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimalExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Orc.FilterBuilder.Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class DecimalExpression : NumericExpression<decimal>
    {
        #region Constructors
        public DecimalExpression()
            : this(true)
        {
        }

        public DecimalExpression(bool isNullable)
        {
            IsDecimal = true;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Decimal;
        }
        #endregion
    }
}