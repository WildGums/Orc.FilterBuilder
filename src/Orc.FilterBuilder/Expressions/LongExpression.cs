// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LongExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class LongExpression : NumericExpression<long>
    {
        #region Constructors
        public LongExpression()
            : this(true)
        {
        }

        public LongExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Long;
        }
        #endregion
    }
}
