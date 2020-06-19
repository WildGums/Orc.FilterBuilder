// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Serialization;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;
    using Catel.Services;

    /// <summary>
    /// The EnumExpression class.
    /// </summary>
    /// <typeparam name="TEnum">
    /// The enum type parameter.
    /// </typeparam>
    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    [Serializable]
    public class EnumExpression<TEnum> : NullableDataTypeExpression
        where TEnum : struct
    {
        #region Constructors
        protected EnumExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        public EnumExpression()
            : this(false)
        {
        }

        /// <summary>
        /// Initialize an instance of <see cref="EnumExpression{TEnum}" />.
        /// </summary>
        /// <param name="isNullable">Indicates whether the type is nullable.</param>
        public EnumExpression(bool isNullable)
        {
            Argument.IsValid("TEnum", typeof(TEnum), typeof(TEnum).IsEnumEx());

            IsNullable = isNullable;

            SelectedCondition = Condition.EqualTo;
            EnumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

            Value = EnumValues.FirstOrDefault();
            ValueControlType = ValueControlType.Enum;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the enum values.
        /// </summary>
        public List<TEnum> EnumValues { get; }

        /// <summary>
        /// Gets and sets the value.
        /// </summary>
        public TEnum Value { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the result
        /// </summary>
        /// <param name="propertyMetadata">
        /// The property metadata
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// <c>True</c> if the entity property value match with the <see cref="Value" /> via <see cref="DataTypeExpression.SelectedCondition" /> operator, otherwise <c>False</c>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// If <see cref="DataTypeExpression.SelectedCondition" /> is not supported.
        /// </exception>
        public sealed override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
        {
            var entityValue = propertyMetadata.GetValue(entity);

            if (!IsNullable && (SelectedCondition == Condition.IsNull || SelectedCondition == Condition.NotIsNull))
            {
                throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
            }

            switch (SelectedCondition)
            {
                case Condition.EqualTo:
                    return Equals(entityValue, Value);

                case Condition.GreaterThan:
                    return Comparer.Default.Compare(entityValue, Value) > 0;

                case Condition.GreaterThanOrEqualTo:
                    return Comparer.Default.Compare(entityValue, Value) >= 0;

                case Condition.LessThan:
                    return Comparer.Default.Compare(entityValue, Value) < 0;

                case Condition.LessThanOrEqualTo:
                    return Comparer.Default.Compare(entityValue, Value) <= 0;

                case Condition.NotEqualTo:
                    return !Equals(entityValue, Value);

                case Condition.IsNull:
                    return entityValue is null;

                case Condition.NotIsNull:
                    return entityValue != null;

                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
            }
        }

        /// <summary>
        /// Converts the object to string.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            return $"{SelectedCondition.Humanize()} '{Value}'";
        }
        #endregion
    }
}
