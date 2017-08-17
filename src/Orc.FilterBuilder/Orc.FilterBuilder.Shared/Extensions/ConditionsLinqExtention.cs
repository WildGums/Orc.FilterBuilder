using Catel;
using Orc.FilterBuilder.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Orc.FilterBuilder.Conditions
{
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
            else if (conditionTreeItem.GetType() == typeof(PropertyExpression))
            {
                return ((PropertyExpression)conditionTreeItem).MakeExpression(parametr);
            }
            else
            {
                return null;
            }
        }


        private static Expression MakeExpression(this ConditionGroup conditionGroup, ParameterExpression parametr)
        {

            if (!conditionGroup.Items.Any())
            {
                return null;
            }


            Expression final = null;
            Expression left = null;
            Expression rigth = null;
            foreach (var item in conditionGroup.Items)
            {
                var curExp = item?.MakeExpression(parametr);
                if (curExp == null) continue;
                if (left == null)
                {
                    left = curExp;
                }
                else
                {
                    rigth = curExp;
                    if (conditionGroup.Type == ConditionGroupType.And)
                    {
                        final = Expression.AndAlso(left, rigth);
                    }
                    else
                    {
                        final = Expression.OrElse(left, rigth);
                    }
                    left = final;
                    rigth = null;
                }
            }

            //return final != null ? final : left;
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
            else if (dataTypeExpression.GetType() == typeof(StringExpression))
            {
                return ((StringExpression)dataTypeExpression).MakeExpression(pe, propertyMetadata.Name);
            }
            else if (dataTypeExpression.GetType().BaseType.IsGenericType && dataTypeExpression.GetType().BaseType.GetGenericTypeDefinition() == typeof(NumericExpression<>))
            {
                return dataTypeExpression.MakeNumericExpression(pe, propertyMetadata.Name);
            }
            else
            {
                return null;
            }
        }
        private static Expression MakeNumericExpression(this DataTypeExpression expression, ParameterExpression pe, string propertyName)

        {

            var valueInfo = expression.GetType().GetProperty("Value");
            var value = valueInfo.GetValue(expression);
            Expression nullExp;
            Expression left;
            Expression rigth;
            Expression e;
            Expression final;
            // left = PropertyExpression(pe, propertyName);
            switch (expression.SelectedCondition)
            {
                case Condition.EqualTo:
                    nullExp = IsNotNullExpression(pe, propertyName);
                    left = PropertyExpression(pe, propertyName);
                    rigth = Expression.Constant(value);
                    e = Expression.Equal(left, rigth);
                    final = Expression.AndAlso(nullExp, e);
                    return final;
                case Condition.NotEqualTo:
                    nullExp = IsNullExpression(pe, propertyName);
                    left = PropertyExpression(pe, propertyName);
                    rigth = Expression.Constant(value);
                    e = Expression.NotEqual(left, rigth);
                    final = Expression.OrElse(nullExp, e);
                    return final;

                case Condition.GreaterThan:
                    nullExp = IsNotNullExpression(pe, propertyName);
                    left = PropertyExpression(pe, propertyName);
                    rigth = Expression.Constant(value);
                    e = Expression.GreaterThan(left, rigth);
                    final = Expression.AndAlso(nullExp, e);
                    return final;
                case Condition.GreaterThanOrEqualTo:
                    nullExp = IsNotNullExpression(pe, propertyName);
                    left = PropertyExpression(pe, propertyName);
                    rigth = Expression.Constant(value);
                    e = Expression.GreaterThanOrEqual(left, rigth);
                    final = Expression.AndAlso(nullExp, e);
                    return final;
                case Condition.LessThan:
                    nullExp = IsNotNullExpression(pe, propertyName);
                    left = PropertyExpression(pe, propertyName);
                    rigth = Expression.Constant(value);
                    e = Expression.LessThan(left, rigth);
                    final = Expression.AndAlso(nullExp, e);
                    return final;


                case Condition.LessThanOrEqualTo:
                    nullExp = IsNotNullExpression(pe, propertyName);
                    left = PropertyExpression(pe, propertyName);
                    rigth = Expression.Constant(value);
                    e = Expression.LessThanOrEqual(left, rigth);
                    final = Expression.AndAlso(nullExp, e);
                    return final;
                case Condition.IsNull:
                    nullExp = IsNullExpression(pe, propertyName);
                    return nullExp;
                case Condition.NotIsNull:
                    nullExp = IsNotNullExpression(pe, propertyName);
                    return nullExp;

                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), expression.SelectedCondition));
            }




        }

        private static Expression MakeExpression(this BooleanExpression expression, ParameterExpression pe, string propertyName)
        {
            var Value = expression.Value;
            var SelectedCondition = expression.SelectedCondition;
            Expression notNullExp;
            Expression e;
            Expression final;
            switch (SelectedCondition)
            {
                case Condition.EqualTo:
                    notNullExp = Expression.Not(IsNullExpression(pe, propertyName));
                    Expression left = PropertyExpression(pe, propertyName);
                    Expression rigth = Expression.Constant(Value);
                    e = Expression.Equal(left, rigth);
                    final = Expression.AndAlso(notNullExp, e);
                    return final;
                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
            }
        }
        private static Expression MakeExpression(this StringExpression expression, ParameterExpression pe, string propertyName)
        {
            var Value = expression.Value;
            var SelectedCondition = expression.SelectedCondition;

            Expression notNulExp;
            Expression left;
            Expression rigth;
            Expression e;
            Expression final;
            switch (SelectedCondition)
            {

                case Condition.Contains:
                    notNulExp = NotIsNullOrEmptyExpression(pe, propertyName);
                    left = Expression.Property(pe, propertyName);
                    rigth = Expression.Constant(Value);
                    e = Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), rigth);
                    final = Expression.AndAlso(notNulExp, e);
                    return final;

                case Condition.DoesNotContain:
                    e = (new StringExpression()
                    {
                        SelectedCondition = Condition.Contains,
                        Value = expression.Value
                    }).MakeExpression(pe, propertyName);
                    return Expression.Not(e);

                case Condition.StartsWith:
                    notNulExp = NotIsNullOrEmptyExpression(pe, propertyName);
                    left = Expression.Property(pe, propertyName);
                    rigth = Expression.Constant(Value);
                    e = Expression.Call(left, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), rigth);
                    final = Expression.AndAlso(notNulExp, e);
                    return final;

                case Condition.DoesNotStartWith:
                    e = (new StringExpression()
                    {
                        SelectedCondition = Condition.StartsWith,
                        Value = expression.Value
                    }).MakeExpression(pe, propertyName);
                    return Expression.Not(e);

                case Condition.EndsWith:
                    notNulExp = NotIsNullOrEmptyExpression(pe, propertyName);
                    left = Expression.Property(pe, propertyName);
                    rigth = Expression.Constant(Value);
                    e = Expression.Call(left, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), rigth);
                    final = Expression.AndAlso(notNulExp, e);
                    return final;

                case Condition.DoesNotEndWith:
                    e = (new StringExpression()
                    {
                        SelectedCondition = Condition.EndsWith,
                        Value = expression.Value
                    }).MakeExpression(pe, propertyName);
                    return Expression.Not(e);

                case Condition.EqualTo:
                    notNulExp = NotIsNullOrEmptyExpression(pe, propertyName);
                    left = Expression.Property(pe, propertyName);
                    rigth = Expression.Constant(Value);
                    e = Expression.Equal(left, rigth);
                    final = Expression.AndAlso(notNulExp, e);
                    return final;

                case Condition.NotEqualTo:
                    e = (new StringExpression()
                    {
                        SelectedCondition = Condition.EqualTo,
                        Value = expression.Value
                    }).MakeExpression(pe, propertyName);
                    return Expression.Not(e);

                case Condition.GreaterThan:
                    e = CompareStringExpression(pe, propertyName, expression.Value);
                    final = Expression.GreaterThan(e, Expression.Constant(0));
                    return final;

                case Condition.GreaterThanOrEqualTo:
                    e = CompareStringExpression(pe, propertyName, expression.Value);

                    final = Expression.GreaterThanOrEqual(e, Expression.Constant(0));

                    return final;

                case Condition.LessThan:
                    e = CompareStringExpression(pe, propertyName, expression.Value);

                    final = Expression.LessThan(e, Expression.Constant(0));

                    return final;

                case Condition.LessThanOrEqualTo:
                    e = CompareStringExpression(pe, propertyName, expression.Value);
                    final = Expression.LessThanOrEqual(e, Expression.Constant(0));
                    return final;

                case Condition.IsNull:
                    left = Expression.Property(pe, propertyName);
                    rigth = Expression.Constant(null);
                    e = Expression.Equal(left, rigth);
                    return e;

                case Condition.NotIsNull:
                    left = Expression.Property(pe, propertyName);
                    rigth = Expression.Constant(null);
                    e = Expression.Equal(left, rigth);
                    final = Expression.Not(e);
                    return final;

                case Condition.IsEmpty:
                    left = Expression.Property(pe, propertyName);
                    rigth = Expression.Constant(string.Empty);
                    e = Expression.Equal(left, rigth);

                    return e;
                case Condition.NotIsEmpty:
                    left = Expression.Property(pe, propertyName);
                    rigth = Expression.Constant(string.Empty);
                    e = Expression.Equal(left, rigth);
                    final = Expression.Not(e);
                    return final;

                case Condition.Matches:
                    return null;

                case Condition.DoesNotMatch:
                    return null;

                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), expression.SelectedCondition));
            }
        }
        private static Expression CompareStringExpression(ParameterExpression pe, string propertyName, string value)
        {
            Expression left;
            Expression rigth;
            Expression e;
            left = Expression.Property(pe, propertyName);
            rigth = Expression.Constant(value);
            var method = typeof(string).GetMethod("Compare", new Type[] { typeof(string), typeof(string), typeof(StringComparison) });
            e = Expression.Call(method, left, rigth, Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            return e;
        }

        private static Expression IsNullOrEmptyExpression(ParameterExpression pe, string propertyName)
        {
            var method = typeof(string).GetMethod("IsNullOrEmpty", new Type[] { typeof(string) });
            Expression arg = Expression.Property(pe, propertyName);
            Expression e = Expression.Call(method, arg);
            return e;
        }
        private static Expression NotIsNullOrEmptyExpression(ParameterExpression pe, string propertyName)
        {
            Expression e = IsNullOrEmptyExpression(pe, propertyName);
            return Expression.Not(e);
        }
        private static Expression IsNullExpression(ParameterExpression pe, string propertyName)
        {
            var type = Expression.Property(pe, propertyName).Type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Expression left;
                Expression rigth;
                Expression e;
                left = Expression.Property(pe, propertyName);
                rigth = Expression.Constant(null);
                e = Expression.Equal(left, rigth);
                return e;
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
                Expression left;
                Expression rigth;
                Expression e;
                left = Expression.Property(pe, propertyName);
                rigth = Expression.Constant(null);
                e = Expression.NotEqual(left, rigth);
                return e;
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
                var propValue = Expression.Property(prop, "Value");
                return propValue;
            }
            else
            {
                return Expression.Property(pe, propertyName);
            }
        }
    }
}
