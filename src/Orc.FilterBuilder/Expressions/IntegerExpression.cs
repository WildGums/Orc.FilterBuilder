// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    [Serializable]
    public class IntegerExpression : NumericExpression<int>
    {
        #region Constructors
        protected IntegerExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IntegerExpression()
            : this(true)
        {
        }

        public IntegerExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Integer;
        }
        #endregion
    }
}
