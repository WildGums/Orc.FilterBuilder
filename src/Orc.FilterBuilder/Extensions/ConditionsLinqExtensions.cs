namespace Orc.FilterBuilder;

using System;
using System.Linq;
using System.Linq.Expressions;
using Catel;
using Catel.Logging;
using Catel.Reflection;
using Microsoft.Extensions.Logging;

public static class ConditionsLinqExtensions
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ConditionsLinqExtensions));

    public static Expression<Func<T, bool>> BuildLambda<T>(this ConditionTreeItem conditionTreeItem)
    {
        ArgumentNullException.ThrowIfNull(conditionTreeItem);

        var type = typeof(T);
        var parameterExpression = Expression.Parameter(type, "item");
        var expression = conditionTreeItem.BuildExpression(parameterExpression);
        return expression is null 
            ? throw Logger.LogErrorAndCreateException<InvalidOperationException>($"Cannot create expression from condition tree item type '{conditionTreeItem.GetType().Name}'") : Expression.Lambda<Func<T, bool>>(expression, parameterExpression);
    }

    private static Expression? BuildExpression(this ConditionTreeItem conditionTreeItem,
        ParameterExpression parameterExpression)
    {
        return conditionTreeItem switch
        {
            ConditionGroup conditionGroup => conditionGroup.BuildExpression(parameterExpression),
            PropertyExpression propertyExpression => propertyExpression.BuildExpression(parameterExpression),
            _ => throw Logger.LogErrorAndCreateException<InvalidOperationException>($"Cannot create expression from condition tree item type '{conditionTreeItem.GetType().Name}'")
        };
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
            var curExp = item.BuildExpression(parameterExpression);
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
                final = conditionGroup.Type == ConditionGroupType.And 
                    ? Expression.AndAlso(left, curExp)
                    : Expression.OrElse(left, curExp);

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
        return condition switch
        {
            Condition.EqualTo => Expression.AndAlso(Expression.Not(isNullExpression), Expression.Equal(propertyExpression, valueExpression)),
            Condition.NotEqualTo => Expression.OrElse(isNullExpression, Expression.NotEqual(propertyExpression, valueExpression)),
            Condition.GreaterThan => Expression.AndAlso(Expression.Not(isNullExpression), Expression.GreaterThan(propertyExpression, valueExpression)),
            Condition.GreaterThanOrEqualTo => Expression.AndAlso(Expression.Not(isNullExpression), Expression.GreaterThanOrEqual(propertyExpression, valueExpression)),
            Condition.LessThan => Expression.AndAlso(Expression.Not(isNullExpression), Expression.LessThan(propertyExpression, valueExpression)),
            Condition.LessThanOrEqualTo => Expression.AndAlso(Expression.Not(isNullExpression), Expression.LessThanOrEqual(propertyExpression, valueExpression)),
            Condition.IsNull => isNullExpression,
            Condition.NotIsNull => Expression.Not(isNullExpression),
            _ => throw Logger.LogErrorAndCreateException<NotSupportedException>(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), condition))
        };
    }

    private static Expression BuildExpression(this BooleanExpression expression,
        ParameterExpression parameterExpression, string propertyName)
    {
        return expression.SelectedCondition switch
        {
            Condition.EqualTo => Expression.AndAlso(Expression.Not(BuildIsNullExpression(parameterExpression, propertyName)), Expression.Equal(BuildPropertyExpression(parameterExpression, propertyName), Expression.Constant(expression.Value))),
            _ => throw Logger.LogErrorAndCreateException<NotSupportedException>(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), expression.SelectedCondition))
        };
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
        return condition switch
        {
            Condition.Contains => Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", TypeArray.From<string>())!, propertyExpression)), 
                Expression.Call(propertyExpression, typeof(string).GetMethod("Contains", TypeArray.From<string>())!, valueExpression)),
            Condition.DoesNotContain => Expression.Not(BuildStringExpression(Condition.Contains, propertyExpression, valueExpression)),
            Condition.StartsWith => Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", TypeArray.From<string>())!, propertyExpression)), 
                Expression.Call(propertyExpression, typeof(string).GetMethod("StartsWith", TypeArray.From<string>())!, valueExpression)),
            Condition.DoesNotStartWith => Expression.Not(BuildStringExpression(Condition.StartsWith, propertyExpression, valueExpression)),
            Condition.EndsWith => Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", TypeArray.From<string>())!, propertyExpression)), 
                Expression.Call(propertyExpression, typeof(string).GetMethod("EndsWith", TypeArray.From<string>())!, valueExpression)),
            Condition.DoesNotEndWith => Expression.Not(BuildStringExpression(Condition.EndsWith, propertyExpression, valueExpression)),
            Condition.EqualTo => Expression.AndAlso(Expression.Not(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty", TypeArray.From<string>())!, propertyExpression)), 
                Expression.Equal(propertyExpression, valueExpression)),
            Condition.NotEqualTo => Expression.Not(BuildStringExpression(Condition.EqualTo, propertyExpression, valueExpression)),
            Condition.GreaterThan => Expression.GreaterThan(Expression.Call(typeof(string).GetMethod("Compare", TypeArray.From<string, string, StringComparison>())!, propertyExpression, valueExpression, 
                Expression.Constant(StringComparison.OrdinalIgnoreCase)), Expression.Constant(0)),
            Condition.GreaterThanOrEqualTo => Expression.GreaterThanOrEqual(Expression.Call(typeof(string).GetMethod("Compare", TypeArray.From<string, string, StringComparison>())!, propertyExpression, valueExpression, 
                Expression.Constant(StringComparison.OrdinalIgnoreCase)), Expression.Constant(0)),
            Condition.LessThan => Expression.LessThan(Expression.Call(typeof(string).GetMethod("Compare", TypeArray.From<string, string, StringComparison>())!, propertyExpression, valueExpression, 
                Expression.Constant(StringComparison.OrdinalIgnoreCase)), Expression.Constant(0)),
            Condition.LessThanOrEqualTo => Expression.LessThanOrEqual(Expression.Call(typeof(string).GetMethod("Compare", TypeArray.From<string, string, StringComparison>())!, propertyExpression, valueExpression, 
                Expression.Constant(StringComparison.OrdinalIgnoreCase)), Expression.Constant(0)),
            Condition.IsNull => Expression.Equal(propertyExpression, Expression.Constant(null)),
            Condition.NotIsNull => Expression.Not(Expression.Equal(propertyExpression, Expression.Constant(null))),
            Condition.IsEmpty => Expression.Equal(propertyExpression, Expression.Constant(string.Empty)),
            Condition.NotIsEmpty => Expression.Not(Expression.Equal(propertyExpression, Expression.Constant(string.Empty))),
            _ => throw Logger.LogErrorAndCreateException((x) => new NotSupportedException(x), string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), condition))
        };
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
