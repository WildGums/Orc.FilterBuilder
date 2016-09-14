// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    using Catel.Caching;
    using Catel.Caching.Policies;
    using Catel.Reflection;

    using Orc.FilterBuilder.Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class StringExpression : DataTypeExpression
    {
        #region Constants
        private static readonly CacheStorage<string, Regex> _regexCache = new CacheStorage<string, Regex>(() => ExpirationPolicy.Sliding(TimeSpan.FromMinutes(1)), false, EqualityComparer<string>.Default);
        private static readonly CacheStorage<string, bool> _regexIsValidCache = new CacheStorage<string, bool>(() => ExpirationPolicy.Sliding(TimeSpan.FromMinutes(1)), false, EqualityComparer<string>.Default);
        #endregion



        #region Constructors
        public StringExpression()
        {
            SelectedCondition = Condition.Contains;
            Value = string.Empty;
            ValueControlType = ValueControlType.Text;
        }
        #endregion



        #region Properties
        public string Value { get; set; }
        #endregion



        #region Methods
        public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
        {
            var entityValue = propertyMetadata.GetValue<string>(entity);
            if (entityValue == null && propertyMetadata.Type.IsEnumEx())
            {
                var entityValueAsObject = propertyMetadata.GetValue(entity);
                if (entityValueAsObject != null)
                {
                    entityValue = entityValueAsObject.ToString();
                }
            }

            switch (SelectedCondition)
            {
                case Condition.Contains:
                    return entityValue != null && entityValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) != -1;

                case Condition.DoesNotContain:
                    return entityValue != null && entityValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) == -1;

                case Condition.EndsWith:
                    return entityValue != null && entityValue.EndsWith(Value, StringComparison.CurrentCultureIgnoreCase);

                case Condition.DoesNotEndWith:
                    return entityValue != null && !entityValue.EndsWith(Value, StringComparison.CurrentCultureIgnoreCase);

                case Condition.EqualTo:
                    return entityValue == Value;

                case Condition.GreaterThan:
                    return string.Compare(entityValue, Value, StringComparison.InvariantCultureIgnoreCase) > 0;

                case Condition.GreaterThanOrEqualTo:
                    return string.Compare(entityValue, Value, StringComparison.InvariantCultureIgnoreCase) >= 0;

                case Condition.IsEmpty:
                    return entityValue == string.Empty;

                case Condition.IsNull:
                    return entityValue == null;

                case Condition.LessThan:
                    return string.Compare(entityValue, Value, StringComparison.InvariantCultureIgnoreCase) < 0;

                case Condition.LessThanOrEqualTo:
                    return string.Compare(entityValue, Value, StringComparison.InvariantCultureIgnoreCase) <= 0;

                case Condition.NotEqualTo:
                    return entityValue != Value;

                case Condition.NotIsEmpty:
                    return entityValue != string.Empty;

                case Condition.NotIsNull:
                    return entityValue != null;

                case Condition.StartsWith:
                    return entityValue != null && entityValue.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase);

                case Condition.DoesNotStartWith:
                    return entityValue != null && !entityValue.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase);

                case Condition.Matches:
                    return entityValue != null && _regexIsValidCache.GetFromCacheOrFetch(Value, () => RegexHelper.IsValid(Value)) && _regexCache.GetFromCacheOrFetch(Value, () => new Regex(Value, RegexOptions.Compiled)).IsMatch(entityValue);

                case Condition.DoesNotMatch:
                    return entityValue != null && _regexIsValidCache.GetFromCacheOrFetch(Value, () => RegexHelper.IsValid(Value)) && !_regexCache.GetFromCacheOrFetch(Value, () => new Regex(Value, RegexOptions.Compiled)).IsMatch(entityValue);

                default:
                    throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
            }
        }

        /// <summary>
        ///   Converts <see cref="ConditionTreeItem"/> to a LINQ <see cref="Expression"/>
        /// </summary>
        /// <param name="propertyExpr">LINQ <see cref="MemberExpression"/>.</param>
        /// <returns>LINQ Expression.</returns>
        public override Expression ToLinqExpression(Expression propertyExpr)
        {
            // Check non-value condition types
            switch (SelectedCondition)
            {
                case Condition.IsEmpty:
                    return Expression.Equal(propertyExpr, Expression.Constant(string.Empty));
                case Condition.NotIsEmpty:
                    return Expression.NotEqual(propertyExpr, Expression.Constant(string.Empty));
                case Condition.IsNull:
                    return Expression.Equal(propertyExpr, Expression.Constant(null));
                case Condition.NotIsNull:
                    return Expression.NotEqual(propertyExpr, Expression.Constant(null));
            }

            // Check value condition types
            var valueExpr = Expression.Constant(Value, typeof(string));
            Expression operatorExpr = null;

            Type[] comparisonMethodParams = { typeof(string), typeof(string), typeof(StringComparison) };

            switch (SelectedCondition)
            {
                case Condition.EqualTo:
                    return Expression.Equal(propertyExpr, valueExpr);

                case Condition.NotEqualTo:
                    return Expression.NotEqual(propertyExpr, valueExpr);

                case Condition.Contains:
                    operatorExpr = Expression.Call(propertyExpr, typeof(string).GetMethod("Contains", new[] { typeof(string) }), valueExpr);
                    break;

                case Condition.DoesNotContain:
                    operatorExpr = Expression.IsFalse(Expression.Call(propertyExpr, typeof(string).GetMethod("Contains", new[] { typeof(string) }), valueExpr));
                    break;

                case Condition.StartsWith:
                    operatorExpr = Expression.Call(propertyExpr, typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) }), valueExpr, Expression.Constant(StringComparison.CurrentCultureIgnoreCase));
                    break;

                case Condition.DoesNotStartWith:
                    operatorExpr = Expression.IsFalse(Expression.Call(propertyExpr, typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) }), valueExpr, Expression.Constant(StringComparison.CurrentCultureIgnoreCase)));
                    break;

                case Condition.EndsWith:
                    operatorExpr = Expression.Call(propertyExpr, typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) }), valueExpr, Expression.Constant(StringComparison.CurrentCultureIgnoreCase));
                    break;

                case Condition.DoesNotEndWith:
                    operatorExpr = Expression.IsFalse(Expression.Call(propertyExpr, typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) }), valueExpr, Expression.Constant(StringComparison.CurrentCultureIgnoreCase)));
                    break;

                // TODO: Handle REGEXP
                /*
              case Condition.Matches:
                return entityValue != null
                       &&
                       _regexIsValidCache.GetFromCacheOrFetch(Value,
                         () => RegexHelper.IsValid(Value))
                       &&
                       _regexCache.GetFromCacheOrFetch(Value, () => new Regex(Value))
                                  .IsMatch(entityValue);

              case Condition.DoesNotMatch:
                return entityValue != null
                       &&
                       _regexIsValidCache.GetFromCacheOrFetch(Value,
                         () => RegexHelper.IsValid(Value))
                       &&
                       !_regexCache.GetFromCacheOrFetch(Value, () => new Regex(Value))
                                   .IsMatch(entityValue);
                                   */
                case Condition.GreaterThan:
                    return Expression.GreaterThan(Expression.Call(Expression.Constant(null, typeof(string)), typeof(string).GetMethod("Compare", comparisonMethodParams), propertyExpr, valueExpr, Expression.Constant(StringComparison.CurrentCultureIgnoreCase)), Expression.Constant(0, typeof(int)));

                case Condition.GreaterThanOrEqualTo:
                    return Expression.GreaterThanOrEqual(Expression.Call(Expression.Constant(null, typeof(string)), typeof(string).GetMethod("Compare", comparisonMethodParams), propertyExpr, valueExpr, Expression.Constant(StringComparison.CurrentCultureIgnoreCase)), Expression.Constant(0, typeof(int)));

                case Condition.LessThan:
                    return Expression.LessThan(Expression.Call(Expression.Constant(null, typeof(string)), typeof(string).GetMethod("Compare", comparisonMethodParams), propertyExpr, valueExpr, Expression.Constant(StringComparison.CurrentCultureIgnoreCase)), Expression.Constant(0, typeof(int)));

                case Condition.LessThanOrEqualTo:
                    return Expression.LessThanOrEqual(Expression.Call(Expression.Constant(null, typeof(string)), typeof(string).GetMethod("Compare", comparisonMethodParams), propertyExpr, valueExpr, Expression.Constant(StringComparison.CurrentCultureIgnoreCase)), Expression.Constant(0, typeof(int)));

                default:
                    throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
            }

            return CreateNullCheckExpression(propertyExpr, operatorExpr);
        }

        private Expression CreateNullCheckExpression(Expression propExpr, Expression operatorExpr)
        {
            Expression nullCheckExpr = Expression.ReferenceNotEqual(propExpr, Expression.Constant(null));

            return Expression.AndAlso(nullCheckExpr, operatorExpr);
        }

        public override string ToString()
        {
            return string.Format("{0} '{1}'", SelectedCondition.Humanize(), Value);
        }
        #endregion
    }
}