namespace Orc.FilterBuilder;

using System;
using System.Linq;
using System.Linq.Expressions;
using Catel;
using Catel.Logging;
using Catel.Reflection;

public static class ConditionsLinqExtensions
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    public static Expression<Func<T, bool>> BuildLambda<T>(this ConditionTreeItem conditionTreeItem)
    {
        ArgumentNullException.ThrowIfNull(conditionTreeItem);

        var type = typeof(T);
        var parameterExpression = Expression.Parameter(type, "item");
        var expression = conditionTreeItem.BuildExpression(parameterExpression);
        if (expression is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>($"Cannot create expression from condition tree item type '{conditionTreeItem.GetType().Name}'");
        }

        return Expression.Lambda<Func<T, bool>>(expression, parameterExpression);
    }

    private static Expression? BuildExpression(this ConditionTreeItem conditionTreeItem,
        ParameterExpression parameterExpression)
    {
        switch (conditionTreeItem)
        {
            case ConditionGroup conditionGroup:
                return conditionGroup.BuildExpression(parameterExpression);

            case PropertyExpression propertyExpression:
                return propertyExpression.BuildExpression(parameterExpression);

            default:
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Cannot create expression from condition tree item type '{conditionTreeItem.GetType().Name}'");
        }
    }

    private static Expression? BuildExpression(this ConditionGroup conditionGroup, ParameterExpression parameterExpression)
    {
        if (!conditionGroup.Items.Any())
        {
            return null;
        }

        //// Revision: Why final is required.
        Expression? final = null;
        Expression? left = null;

        foreach (var item in conditionGroup.Items)
        {
            var curExp = item?.BuildExpression(parameterExpression);
            if (curExp is null)
            {
                continue;
            }

            if (left is null)
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

    private static Expression? BuildExpression(this PropertyExpression propertyExpression,
        ParameterExpression parameterExpression)
    {
        if (propertyExpression.DataTypeExpression is null ||
            propertyExpression.Property is null)
        {
            return null;
        }

        return propertyExpression.DataTypeExpression.BuildExpression(parameterExpression, propertyExpression.Property);
    }

    private static Expression? BuildExpression(this DataTypeExpression dataTypeExpression,
        ParameterExpression parameterExpression, IPropertyMetadata propertyMetadata)
    {
        switch (dataTypeExpression)
        {
            case BooleanExpression booleanExpression:
                return booleanExpression.BuildExpression(parameterExpression,
                    propertyMetadata.Name);

            case StringExpression stringExpression:
                return stringExpression.BuildExpression(parameterExpression, propertyMetadata.Name);

            case DateTimeExpression _:
                return dataTypeExpression.BuildNumericExpression(parameterExpression, propertyMetadata.Name);

            default:
                {
                    var type = dataTypeExpression.GetType();
                    var baseType = type.GetBaseTypeEx();

                    if (baseType is not null && baseType.IsGenericTypeEx() && baseType.GetGenericTypeDefinition() == typeof(NumericExpression<>))
                    {
                        return dataTypeExpression.BuildNumericExpression(parameterExpression, propertyMetadata.Name);
                    }

                    return null;
                }
        }
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
                throw Log.ErrorAndCreateException<NotSupportedException>(string.Format(
                    LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"),
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
                throw Log.ErrorAndCreateException<NotSupportedException>(string.Format(
                    LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"),
                    expression.SelectedCondition));
        }
    }

    private static Expression BuildExpression(this StringExpression expression,
        ParameterExpression parameterExpression, string propertyName)
    {
        return BuildStringExpression(expression.SelectedCondition, Expression.Property(parameterExpression, propertyName), Expression.Constant(expression.Value));
    }

    //// TODO: Try to combine expression builders as much as possible.
    private static Expression BuildStringExpression(Condition condition, Expression propertyExpression,
        Expression valueExpression)
    {
        switch (condition)
        {
            case Condition.Contains:
                return Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", TypeArray.From<string>())!, propertyExpression)),
                    Expression.Call(propertyExpression, typeof(string).GetMethod("Contains", TypeArray.From<string>())!,
                        valueExpression));

            case Condition.DoesNotContain:
                return Expression.Not(BuildStringExpression(Condition.Contains, propertyExpression, valueExpression));

            case Condition.StartsWith:
                return Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", TypeArray.From<string>())!, propertyExpression)),
                    Expression.Call(propertyExpression, typeof(string).GetMethod("StartsWith", TypeArray.From<string>())!,
                        valueExpression));

            case Condition.DoesNotStartWith:
                return Expression.Not(BuildStringExpression(Condition.StartsWith, propertyExpression, valueExpression));

            case Condition.EndsWith:
                return Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", TypeArray.From<string>())!, propertyExpression)),
                    Expression.Call(propertyExpression, typeof(string).GetMethod("EndsWith", TypeArray.From<string>())!,
                        valueExpression));

            case Condition.DoesNotEndWith:
                return Expression.Not(BuildStringExpression(Condition.EndsWith, propertyExpression, valueExpression));

            case Condition.EqualTo:
                return Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", TypeArray.From<string>())!, propertyExpression)),
                    Expression.Equal(propertyExpression, valueExpression));

            case Condition.NotEqualTo:
                return Expression.Not(BuildStringExpression(Condition.EqualTo, propertyExpression, valueExpression));

            case Condition.GreaterThan:
                return Expression.GreaterThan(Expression.Call(typeof(string).GetMethod("Compare", TypeArray.From<string, string, StringComparison>())!,
                        propertyExpression, valueExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase)),
                    Expression.Constant(0));

            case Condition.GreaterThanOrEqualTo:
                return Expression.GreaterThanOrEqual(
                    Expression.Call(
                        typeof(string).GetMethod("Compare", TypeArray.From<string, string, StringComparison>())!,
                        propertyExpression, valueExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase)), Expression.Constant(0));

            case Condition.LessThan:
                return Expression.LessThan(Expression.Call(
                        typeof(string).GetMethod("Compare", TypeArray.From<string, string, StringComparison>())!,
                        propertyExpression, valueExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase)),
                    Expression.Constant(0));

            case Condition.LessThanOrEqualTo:
                return Expression.LessThanOrEqual(Expression.Call(
                        typeof(string).GetMethod("Compare", TypeArray.From<string, string, StringComparison>())!,
                        propertyExpression, valueExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase)),
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
                throw Log.ErrorAndCreateException((x) => new NotSupportedException(x), 
                    string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), condition));
        }
    }

    private static Expression BuildIsNullExpression(ParameterExpression parameterExpression, string propertyName)
    {
        var type = Expression.Property(parameterExpression, propertyName).Type;
        if (type.IsGenericTypeEx() && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return Expression.Equal(Expression.Property(parameterExpression, propertyName), Expression.Constant(null));
        }

        return Expression.Constant(false);
    }

    private static Expression BuildPropertyExpression(ParameterExpression parameterExpression, string propertyName)
    {
        var type = Expression.Property(parameterExpression, propertyName).Type;
        if (type.IsGenericTypeEx() && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return Expression.Property(Expression.Property(parameterExpression, propertyName), "Value");
        }

        return Expression.Property(parameterExpression, propertyName);
    }
}