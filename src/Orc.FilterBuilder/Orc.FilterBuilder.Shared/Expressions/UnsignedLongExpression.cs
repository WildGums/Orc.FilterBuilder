// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsignedLongExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedLongExpression : NumericExpression<ulong>
    {
        #region Constructors
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
        #endregion



        public override object Clone()
        {
            return new UnsignedLongExpression(IsNullable) { Value = this.Value, SelectedCondition = this.SelectedCondition };
        }
    }
}
