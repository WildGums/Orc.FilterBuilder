// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Catel.Runtime.Serialization;
    using Orc.FilterBuilder.Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class BooleanExpression : DataTypeExpression
    {
        #region Constructors
        public BooleanExpression()
        {
            BooleanValues = new List<bool> {true, false};
            Value = true;
            SelectedCondition = Condition.EqualTo;
            ValueControlType = ValueControlType.Boolean;
        }
        #endregion

        #region Properties
        public bool Value { get; set; }

        [ExcludeFromSerialization]
        public List<bool> BooleanValues { get; set; }
        #endregion

        #region Methods
        public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
        {
            bool entityValue = propertyMetadata.GetValue<bool>(entity);
            switch (SelectedCondition)
            {
                case Condition.EqualTo:
                    return entityValue == Value;

                default:
                    throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} '{1}'", SelectedCondition.Humanize(), Value);
        }
        #endregion
    }
}