// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteExpression.cs" company="WildGums">
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
    public class ByteExpression : NumericExpression<byte>
    {
        #region Constructors
        protected ByteExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

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
    }
}
