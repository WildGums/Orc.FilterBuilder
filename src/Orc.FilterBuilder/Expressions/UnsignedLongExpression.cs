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
    [Serializable]
    public class UnsignedLongExpression : NumericExpression<ulong>
    {
        #region Constructors
        protected UnsignedLongExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

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
    }
}
