// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericExpression.generic.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract class NumericExpression<TValue> : ValueDataTypeExpression<TValue>
        where TValue : struct, IComparable, IFormattable, IConvertible, IComparable<TValue>, IEquatable<TValue>
    {
        protected NumericExpression()
            : base()
        {

        }

        #region Properties
        public bool IsDecimal { get; protected set; }
        public bool IsSigned { get; protected set; }
        #endregion
    }
}
