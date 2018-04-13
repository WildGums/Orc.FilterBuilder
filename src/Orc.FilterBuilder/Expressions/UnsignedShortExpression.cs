// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsignedShortExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedShortExpression : NumericExpression<ushort>
    {
        #region Constructors
        public UnsignedShortExpression()
            : this(true)
        {
        }

        public UnsignedShortExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = false;
            ValueControlType = ValueControlType.UnsignedShort;
        }
        #endregion
    }
}
