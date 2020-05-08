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
    public class NumericExpression : NumericExpression<double>
    {
        protected NumericExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public NumericExpression()
        {
            IsDecimal = true;
            IsSigned = true;
            ValueControlType = ValueControlType.Numeric;
        }

        public NumericExpression(Type type)
            : this()
        {
            IsNullable = type.IsNullable();
        }
    }
}
