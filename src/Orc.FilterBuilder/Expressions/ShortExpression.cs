// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShortExpression.cs" company="WildGums">
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
    public class ShortExpression : NumericExpression<short>
    {
        #region Constructors
        protected ShortExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ShortExpression()
            : this(true)
        {
        }

        public ShortExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Short;
        }
        #endregion
    }
}
