namespace Orc.FilterBuilder;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Catel;
using Catel.Runtime.Serialization;

[ObsoleteEx(ReplacementTypeOrMember = "Use TimeSpanValueExpression instead")]
[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class TimeSpanExpression : NullableDataTypeExpression
{
    public TimeSpanExpression()
        : this(true)
    {

    }

    public TimeSpanExpression(bool isNullable)
    {
        IsNullable = isNullable;
        SelectedCondition = Condition.EqualTo;
        Value = TimeSpan.FromHours(1);
        ValueControlType = ValueControlType.TimeSpan;
        SpanTypes = Enum.GetValues(typeof(TimeSpanType)).Cast<TimeSpanType>().ToList();
        SelectedSpanType = TimeSpanType.Hours;
    }

    public TimeSpan Value { get; set; }

    [ExcludeFromSerialization]
    public List<TimeSpanType> SpanTypes { get; set; }

    public TimeSpanType SelectedSpanType { get; set; }

    public float Amount { get; set; }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        const int averageDaysInYear = 365;
        const int averageDaysInMonth = 30;

        if (!e.HasPropertyChanged(() => SelectedSpanType) && !e.HasPropertyChanged(() => Amount))
        {
            return;
        }

        Value = SelectedSpanType switch
        {
            TimeSpanType.Years => TimeSpan.FromDays(Amount * averageDaysInYear),
            TimeSpanType.Months => TimeSpan.FromDays(Amount * averageDaysInMonth),
            TimeSpanType.Days => TimeSpan.FromDays(Amount),
            TimeSpanType.Hours => TimeSpan.FromHours(Amount),
            TimeSpanType.Minutes => TimeSpan.FromMinutes(Amount),
            TimeSpanType.Seconds => TimeSpan.FromSeconds(Amount),
            _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_TypeIsNotSupported_Pattern"), SelectedSpanType))
        };
    }

    public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
    {
        var entityValue = propertyMetadata.GetValue<TimeSpan>(entity);
        return SelectedCondition switch
        {
            Condition.EqualTo => entityValue == Value,
            Condition.NotEqualTo => entityValue != Value,
            Condition.GreaterThan => entityValue > Value,
            Condition.LessThan => entityValue < Value,
            Condition.GreaterThanOrEqualTo => entityValue >= Value,
            Condition.LessThanOrEqualTo => entityValue <= Value,
            _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition))
        };
    }

    public override string ToString()
    {
        return $"{SelectedCondition.Humanize()} '{Value}'";
    }
}
