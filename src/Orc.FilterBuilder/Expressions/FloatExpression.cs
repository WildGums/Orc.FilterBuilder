// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FloatExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    [Serializable]
    public class FloatExpression : NumericExpression<float>
    {
        #region Constructors
        protected FloatExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public FloatExpression()
            : this(true)
        {
        }

        public FloatExpression(bool isNullable)
        {
            IsDecimal = true;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Float;
        }
        #endregion
    }
}
