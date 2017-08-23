// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Conditions
{
    using Catel;
    using Orc.FilterBuilder.Models;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class ConditionsLinqExtention
    {
        public static Expression<Func<T, bool>> MakeFunction<T>(this ConditionTreeItem conditionTreeItem)
        {
            var type = typeof(T);
            var pe = Expression.Parameter(type, "item");
            Expression expression = conditionTreeItem.MakeExpression(pe);
            if (expression == null)
            {
                return null;
            }
            var lambda = Expression.Lambda<Func<T, bool>>(expression, pe);
            return lambda;
        }

        private static Expression MakeExpression(this ConditionTreeItem conditionTreeItem, ParameterExpression parametr)
        {
            if (conditionTreeItem.GetType() == typeof(ConditionGroup))
            {
                return ((ConditionGroup)conditionTreeItem).MakeExpression(parametr);
            }
            if (conditionTreeItem.GetType() == typeof(PropertyExpression))
            {
                return ((PropertyExpression)conditionTreeItem).MakeExpression(parametr);
            }
            return null;
        }

        private static Expression MakeExpression(this ConditionGroup conditionGroup, ParameterExpression parametr)
        {
            if (!conditionGroup.Items.Any())
            {
                return null;
            }

            Expression final = null;
            Expression left = null;
            foreach (var item in conditionGroup.Items)
            {
                var curExp = item?.MakeExpression(parametr);
                if (curExp == null) { continue; }
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

        private static Expression MakeExpression(this PropertyExpression propertyExpression, ParameterExpression pe)
        {
            return propertyExpression.DataTypeExpression.MakeExpression(pe, propertyExpression.Property);
        }

        private static Expression MakeExpression(this DataTypeExpression dataTypeExpression, ParameterExpression pe, IPropertyMetadata propertyMetadata)
        {

            if (dataTypeExpression.GetType() == typeof(BooleanExpression))
            {
                return ((BooleanExpression)dataTypeExpression).MakeExpression(pe, propertyMetadata.Name);
            }

            if (dataTypeExpression.GetType() == typeof(StringExpression))
            {
                return ((StringExpression)dataTypeExpression).MakeExpression(pe, propertyMetadata.Name);
            }

            if (
                    dataTypeExpression.GetType() == typeof(DateTimeExpression) ||
                    (
                        dataTypeExpression.GetType().BaseType.IsGenericType &&
                        dataTypeExpression.GetType().BaseType.GetGenericTypeDefinition() == typeof(NumericExpression<>)
                    ))
            {
                return dataTypeExpression.MakeNumericExpression(pe, propertyMetadata.Name);
            }

            return null;
        }

    
        private static Expression MakeNumericExpression(this DataTypeExpression expression, ParameterExpression pe, string propertyName)
        {
            Expression notNullExp= IsNotNullExpression(pe, propertyName);
            Expression nullExp = IsNullExpression(pe, propertyName);
            Expression valueExp = Expression.Constant(expression.GetType().GetProperty("Value")?.GetValue(expression));
            Expression propertyExp = PropertyExpression(pe, propertyName);
            Expression e;
            switch (expression.SelectedCondition)
            {
                case Condition.EqualTo:
                    e = Expression.Equal(propertyExp, valueExp);
                    return Expression.AndAlso(notNullExp, e);
                case Condition.NotEqualTo:
                    e = Expression.NotEqual(propertyExp, valueExp);
                    return Expression.OrElse(nullExp, e);
                case Condition.GreaterThan:      
                    e = Expression.GreaterThan(propertyExp, valueExp);
                    return Expression.AndAlso(notNullExp, e);
                case Condition.GreaterThanOrEqualTo:
                    e = Expression.GreaterThanOrEqual(propertyExp, valueExp);
                    return Expression.AndAlso(notNullExp, e);
                case Condition.LessThan:
                    e = Expression.LessThan(propertyExp, valueExp);
                    return Expression.AndAlso(notNullExp, e);
                case Condition.LessThanOrEqualTo:
                    e = Expression.LessThanOrEqual(propertyExp, valueExp);
                    return Expression.AndAlso(notNullExp, e);
                case Condition.IsNull:
                    return nullExp;
                case Condition.NotIsNull:
                    return notNullExp;
                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), expression.SelectedCondition));
            }
        }

        private static Expression MakeExpression(this BooleanExpression expression, ParameterExpression pe, string propertyName)
        {
            var value = expression.Value;
            var SelectedCondition = expression.SelectedCondition;

            switch (SelectedCondition)
            {
                case Condition.EqualTo:
                    Expression notNullExp = IsNotNullExpression(pe, propertyName);
                    Expression e = Expression.Equal(PropertyExpression(pe, propertyName), Expression.Constant(value));
                    return  Expression.AndAlso(notNullExp, e);
                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
            }
        }

        private static Expression MakeExpression(this StringExpression expression, ParameterExpression pe, string propertyName)
        {

            Expression valueExp = Expression.Constant(expression.Value);
            Expression propertyExp = Expression.Property(pe, propertyName);

            return StringExpression(expression.SelectedCondition, propertyExp, valueExp);
        }

      static   Expression StringExpression(Condition condition, Expression propertyExp, Expression valueExp )
        {
            Expression notNullEmtpyExp = IsNotNullOrEmptyExpression(propertyExp);
            Expression e;
            switch (condition)
            {
                case Condition.Contains:
                    e = Expression.Call(propertyExp, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), valueExp);
                    return Expression.AndAlso(notNullEmtpyExp, e);
                case Condition.DoesNotContain:
                    return Expression.Not(StringExpression(Condition.Contains, propertyExp, valueExp));
                case Condition.StartsWith:
                    e = Expression.Call(propertyExp, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), valueExp);
                    return Expression.AndAlso(notNullEmtpyExp, e);
                case Condition.DoesNotStartWith:
                    return Expression.Not(StringExpression(Condition.StartsWith, propertyExp, valueExp));
                case Condition.EndsWith:
                    e = Expression.Call(propertyExp, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), valueExp);
                    return Expression.AndAlso(notNullEmtpyExp, e);
                case Condition.DoesNotEndWith:
                    return Expression.Not(StringExpression(Condition.EndsWith, propertyExp, valueExp));
                case Condition.EqualTo:
                    e = Expression.Equal(propertyExp, valueExp);
                    return Expression.AndAlso(notNullEmtpyExp, e);
                case Condition.NotEqualTo:
                    return Expression.Not(StringExpression(Condition.EqualTo, propertyExp, valueExp));
                case Condition.GreaterThan:
                    e = CompareStringExpression(propertyExp, valueExp);
                    return Expression.GreaterThan(e, Expression.Constant(0));
                case Condition.GreaterThanOrEqualTo:
                    e = CompareStringExpression(propertyExp, valueExp);
                    return Expression.GreaterThanOrEqual(e, Expression.Constant(0));
                case Condition.LessThan:
                    e = CompareStringExpression(propertyExp, valueExp);
                    return Expression.LessThan(e, Expression.Constant(0));
                case Condition.LessThanOrEqualTo:
                    e = CompareStringExpression(propertyExp, valueExp);
                    return Expression.LessThanOrEqual(e, Expression.Constant(0));
                case Condition.IsNull:
                    return Expression.Equal(propertyExp, Expression.Constant(null));
                case Condition.NotIsNull:
                    e = Expression.Equal(propertyExp, Expression.Constant(null));
                    return Expression.Not(e);
                case Condition.IsEmpty:
                    return Expression.Equal(propertyExp, Expression.Constant(string.Empty));
                case Condition.NotIsEmpty:
                    e = Expression.Equal(propertyExp, Expression.Constant(string.Empty));
                    return Expression.Not(e);
                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), condition));
            }
        }
        private static Expression CompareStringExpression( Expression  value1, Expression value2)
        {
            var method = typeof(string).GetMethod("Compare", new Type[] { typeof(string), typeof(string), typeof(StringComparison) });
            return Expression.Call(method, value1, value2, Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
        }

        private static Expression IsNullOrEmptyExpression(ParameterExpression pe, string propertyName)
        {
            var method = typeof(string).GetMethod("IsNullOrEmpty", new Type[] { typeof(string) });
            Expression arg = Expression.Property(pe, propertyName);
            return Expression.Call(method, arg);
        }
        private static Expression IsNullOrEmptyExpression(Expression propertyExp)
        {
            var method = typeof(string).GetMethod("IsNullOrEmpty", new Type[] { typeof(string) });
            return Expression.Call(method, propertyExp);
        }

        private static Expression IsNotNullOrEmptyExpression(ParameterExpression pe, string propertyName)
        {
           return Expression.Not(IsNullOrEmptyExpression(pe, propertyName));
        }
        private static Expression IsNotNullOrEmptyExpression(Expression propertyExp)
        {
            return Expression.Not(IsNullOrEmptyExpression( propertyExp));
        }

        private static Expression IsNullExpression(ParameterExpression pe, string propertyName)
        {
            var type = Expression.Property(pe, propertyName).Type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Expression.Equal(Expression.Property(pe, propertyName), Expression.Constant(null));
            }
            else
            {
                return Expression.Constant(false);
            }
        }

        private static Expression IsNotNullExpression(ParameterExpression pe, string propertyName)
        {
            var type = Expression.Property(pe, propertyName).Type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Expression.NotEqual(Expression.Property(pe, propertyName), Expression.Constant(null));
            }
            else
            {
                return Expression.Constant(true);
            }
        }

        private static Expression PropertyExpression(ParameterExpression pe, string propertyName)
        {
            var type = Expression.Property(pe, propertyName).Type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var prop = Expression.Property(pe, propertyName);
                return Expression.Property(prop, "Value");
            }
            else
            {
                return Expression.Property(pe, propertyName);
            }
        }
    }
}
