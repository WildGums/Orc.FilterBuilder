namespace Orc.FilterBuilder;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Catel;

[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class BooleanExpression : DataTypeExpression
{
    public BooleanExpression()
    {
        BooleanValues = new List<bool> {true, false};
        Value = true;
        SelectedCondition = Condition.EqualTo;
        ValueControlType = ValueControlType.Boolean;
    }

    public bool Value { get; set; }

    [JsonIgnore]
    public List<bool> BooleanValues { get; set; }

    public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
    {
        var entityValue = propertyMetadata.GetValue<bool>(entity);

        return SelectedCondition switch
        {
            Condition.EqualTo => entityValue == Value,
            Condition.NotEqualTo => entityValue != Value,
            _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition))
        };
    }

    public override string ToString()
    {
        return $"{SelectedCondition.Humanize()} '{Value}'";
    }
}
