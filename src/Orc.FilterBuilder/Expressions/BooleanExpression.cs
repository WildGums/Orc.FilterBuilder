// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using Catel;
    using Catel.Runtime.Serialization;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    [Serializable]
    public class BooleanExpression : DataTypeExpression
    {
        #region Constructors
        protected BooleanExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

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
            var entityValue = propertyMetadata.GetValue<bool>(entity);

            switch (SelectedCondition)
            {
                case Condition.EqualTo:
                    return entityValue == Value;

                case Condition.NotEqualTo:
                    return entityValue != Value;

                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
            }
        }

        public override string ToString()
        {
            return $"{SelectedCondition.Humanize()} '{Value}'";
        }
        #endregion
    }
}
