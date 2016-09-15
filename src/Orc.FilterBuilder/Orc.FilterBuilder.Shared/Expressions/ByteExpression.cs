// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class ByteExpression : NumericExpression<byte>
    {
        #region Constructors
        public ByteExpression()
            : this(true)
        {
        }

        public ByteExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = false;
            ValueControlType = ValueControlType.Byte;
        }
        #endregion



        public override object Clone()
        {
            return new ByteExpression(IsNullable) { Value = this.Value, SelectedCondition = this.SelectedCondition };
        }
    }
}
