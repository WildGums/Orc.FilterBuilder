// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Orc.FilterBuilder.Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class DateTimeExpression : ValueDataTypeExpression<DateTime>
    {
        #region Constructors
        public DateTimeExpression()
            : this(false)
        {
        }

        public DateTimeExpression(bool isNullable)
        {
            IsNullable = isNullable;
            Value = DateTime.Now;
            ValueControlType = ValueControlType.DateTime;
        }
        #endregion
    }
}