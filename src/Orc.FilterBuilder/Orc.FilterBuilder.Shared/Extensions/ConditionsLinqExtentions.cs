// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionsLinqExtentions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Conditions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Catel;
    using Orc.FilterBuilder.Models;

    public static class ConditionsLinqExtentions
    {
        public static Expression<Func<T, bool>> BuildLambda<T>(this ConditionTreeItem conditionTreeItem)
        {
            var type = typeof(T);
            var parameterExpression = Expression.Parameter(type, "item");
            var expression = conditionTreeItem.BuildExpression(parameterExpression);
            if (expression == null)
            {
                return null;
            }

            return Expression.Lambda<Func<T, bool>>(expression, parameterExpression);
        }

        private static Expression BuildExpression(this ConditionTreeItem conditionTreeItem,
            ParameterExpression parametr)
        {
            if (conditionTreeItem.GetType() == typeof(ConditionGroup))
            {
                return ((ConditionGroup)conditionTreeItem).BuildExpression(parametr);
            }

            if (conditionTreeItem.GetType() == typeof(PropertyExpression))
            {
                return ((PropertyExpression)conditionTreeItem).BuildExpression(parametr);
            }

            return null;
        }

        private static Expression BuildExpression(this ConditionGroup conditionGroup, ParameterExpression parametr)
        {
            if (!conditionGroup.Items.Any())
            {
                return null;
            }

            //// Revision: Why final is required.
            Expression final = null;
            Expression left = null;
            foreach (var item in conditionGroup.Items)
            {
                var curExp = item?.BuildExpression(parametr);
                if (curExp == null)
                {
                    continue;
                }
                if (left == null)
                {
                    left = curExp;
                }
                else
                {
                    var rigth = curExp;
                    if (conditionGroup.Type == ConditionGroupType.And)
                    {
                        final = Expression.AndAlso(left, rigth);
                    }
                    else
                    {
                        final = Expression.OrElse(left, rigth);
                    }

                    left = final;
                }
            }

            return final ?? left;
        }

        private static Expression BuildExpression(this PropertyExpression propertyExpression,
            ParameterExpression parameterExpression)
        {
            return propertyExpression.DataTypeExpression.BuildExpression(parameterExpression,
                propertyExpression.Property);
        }

        private static Expression BuildExpression(this DataTypeExpression dataTypeExpression,
            ParameterExpression parameterExpression, IPropertyMetadata propertyMetadata)
        {
            if (dataTypeExpression.GetType() == typeof(BooleanExpression))
            {
                return ((BooleanExpression)dataTypeExpression).BuildExpression(parameterExpression,
                    propertyMetadata.Name);
            }

            if (dataTypeExpression.GetType() == typeof(StringExpression))
            {
                return ((StringExpression)dataTypeExpression).BuildExpression(parameterExpression,
                    propertyMetadata.Name);
            }

            if (dataTypeExpression.GetType() == typeof(DateTimeExpression) ||
                dataTypeExpression.GetType().BaseType.IsGenericType &&
                dataTypeExpression.GetType().BaseType.GetGenericTypeDefinition() == typeof(NumericExpression<>))
            {
                return dataTypeExpression.BuildNumericExpression(parameterExpression, propertyMetadata.Name);
            }

            return null;
        }

        private static Expression BuildNumericExpression(this DataTypeExpression expression,
            ParameterExpression parameterExpression, string propertyName)
        {
            return NumericExpression(expression.SelectedCondition,
                BuildPropertyExpression(parameterExpression, propertyName),
                Expression.Constant(expression.GetType().GetProperty("Value")?.GetValue(expression)),
                BuildIsNullExpression(parameterExpression, propertyName));
        }

        private static Expression NumericExpression(Condition condition, Expression propertyExpression,
            Expression valueExpression, Expression isNullExpression)
        {
            switch (condition)
            {
                case Condition.EqualTo:
                    return Expression.AndAlso(Expression.Not(isNullExpression),
                        Expression.Equal(propertyExpression, valueExpression));
                case Condition.NotEqualTo:
                    return Expression.OrElse(isNullExpression,
                        Expression.NotEqual(propertyExpression, valueExpression));
                case Condition.GreaterThan:
                    return Expression.AndAlso(Expression.Not(isNullExpression),
                        Expression.GreaterThan(propertyExpression, valueExpression));
                case Condition.GreaterThanOrEqualTo:
                    return Expression.AndAlso(Expression.Not(isNullExpression),
                        Expression.GreaterThanOrEqual(propertyExpression, valueExpression));
                case Condition.LessThan:
                    return Expression.AndAlso(Expression.Not(isNullExpression),
                        Expression.LessThan(propertyExpression, valueExpression));
                case Condition.LessThanOrEqualTo:
                    return Expression.AndAlso(Expression.Not(isNullExpression),
                        Expression.LessThanOrEqual(propertyExpression, valueExpression));
                case Condition.IsNull:
                    return isNullExpression;
                case Condition.NotIsNull:
                    return Expression.Not(isNullExpression);
                default:
                    throw new NotSupportedException(string.Format(
                        LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"),
                        condition));
            }
        }

        private static Expression BuildExpression(this BooleanExpression expression,
            ParameterExpression parameterExpression, string propertyName)
        {
            switch (expression.SelectedCondition)
            {
                case Condition.EqualTo:
                    return Expression.AndAlso(Expression.Not(BuildIsNullExpression(parameterExpression, propertyName)),
                        Expression.Equal(BuildPropertyExpression(parameterExpression, propertyName),
                            Expression.Constant(expression.Value)));
                default:
                    throw new NotSupportedException(string.Format(
                        LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"),
                        expression.SelectedCondition));
            }
        }

        private static Expression BuildExpression(this StringExpression expression,
            ParameterExpression parameterExpression, string propertyName)
        {
            return BuildStringExpression(expression.SelectedCondition, Expression.Property(parameterExpression, propertyName), Expression.Constant(expression.Value));
        }

        /// Review: Try to combine expression builders as much as possible.
        private static Expression BuildStringExpression(Condition condition, Expression propertyExpression,
            Expression valueExpression)
        {
            switch (condition)
            {
                case Condition.Contains:
                    return Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) }), propertyExpression)),
                        Expression.Call(propertyExpression, typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                            valueExpression));
                case Condition.DoesNotContain:
                    return Expression.Not(BuildStringExpression(Condition.Contains, propertyExpression, valueExpression));
                case Condition.StartsWith:
                    return Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) }), propertyExpression)),
                        Expression.Call(propertyExpression, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                            valueExpression));
                case Condition.DoesNotStartWith:
                    return Expression.Not(BuildStringExpression(Condition.StartsWith, propertyExpression, valueExpression));
                case Condition.EndsWith:
                    return Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) }), propertyExpression)),
                        Expression.Call(propertyExpression, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                            valueExpression));
                case Condition.DoesNotEndWith:
                    return Expression.Not(BuildStringExpression(Condition.EndsWith, propertyExpression, valueExpression));
                case Condition.EqualTo:
                    return Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) }), propertyExpression)),
                        Expression.Equal(propertyExpression, valueExpression));
                case Condition.NotEqualTo:
                    return Expression.Not(BuildStringExpression(Condition.EqualTo, propertyExpression, valueExpression));
                case Condition.GreaterThan:
                    return Expression.GreaterThan(Expression.Call(
                            typeof(string).GetMethod("Compare", new[] { typeof(string), typeof(string), typeof(StringComparison) }),
                            propertyExpression, valueExpression, Expression.Constant(StringComparison.InvariantCultureIgnoreCase)),
                        Expression.Constant(0));
                case Condition.GreaterThanOrEqualTo:
                    return Expression.GreaterThanOrEqual(
                        Expression.Call(
                            typeof(string).GetMethod("Compare", new[] { typeof(string), typeof(string), typeof(StringComparison) }),
                            propertyExpression, valueExpression, Expression.Constant(StringComparison.InvariantCultureIgnoreCase)), Expression.Constant(0));
                case Condition.LessThan:
                    return Expression.LessThan(Expression.Call(
                            typeof(string).GetMethod("Compare", new[] { typeof(string), typeof(string), typeof(StringComparison) }),
                            propertyExpression, valueExpression, Expression.Constant(StringComparison.InvariantCultureIgnoreCase)),
                        Expression.Constant(0));
                case Condition.LessThanOrEqualTo:
                    return Expression.LessThanOrEqual(Expression.Call(
                            typeof(string).GetMethod("Compare", new[] { typeof(string), typeof(string), typeof(StringComparison) }),
                            propertyExpression, valueExpression, Expression.Constant(StringComparison.InvariantCultureIgnoreCase)),
                        Expression.Constant(0));
                case Condition.IsNull:
                    return Expression.Equal(propertyExpression, Expression.Constant(null));
                case Condition.NotIsNull:
                    return Expression.Not(Expression.Equal(propertyExpression, Expression.Constant(null)));
                case Condition.IsEmpty:
                    return Expression.Equal(propertyExpression, Expression.Constant(string.Empty));
                case Condition.NotIsEmpty:
                    return Expression.Not(Expression.Equal(propertyExpression, Expression.Constant(string.Empty)));
                default:
                    throw new NotSupportedException(string.Format(
                        LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"),
                        condition));
            }
        }

        private static Expression BuildIsNullExpression(ParameterExpression parameterExpression, string propertyName)
        {
            var type = Expression.Property(parameterExpression, propertyName).Type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Expression.Equal(Expression.Property(parameterExpression, propertyName), Expression.Constant(null));
            }

            return Expression.Constant(false);
        }

        private static Expression BuildPropertyExpression(ParameterExpression parameterExpression, string propertyName)
        {
            var type = Expression.Property(parameterExpression, propertyName).Type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Expression.Property(Expression.Property(parameterExpression, propertyName), "Value");
            }

            return Expression.Property(parameterExpression, propertyName);
        }
    }
}