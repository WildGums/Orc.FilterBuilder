namespace Orc.FilterBuilder;

using System.Text.Json.Serialization;
using Catel.Data;

public abstract class DataTypeExpression : ModelBase
{
    public Condition SelectedCondition { get; set; }

    [JsonIgnore]
    public bool IsValueRequired { get; set; } = true;

    public ValueControlType ValueControlType { get; set; }

    /// <summary>
    /// Gets or sets the property to compare against. When set, the value is read from this
    /// property on the same entity instead of using the constant value of the expression.
    /// </summary>
    public IPropertyMetadata? ValueProperty { get; set; }

    [JsonIgnore]
    public override bool IsReadOnly
    {
        get => base.IsReadOnly;
        protected set => base.IsReadOnly = value;
    }

    [JsonIgnore]
    public override bool IsDirty
    {
        get => base.IsDirty;
        protected set => base.IsDirty = value;
    }

    private void OnSelectedConditionChanged()
    {
        IsValueRequired = ConditionHelper.GetIsValueRequired(SelectedCondition);
    }

    public abstract bool CalculateResult(IPropertyMetadata propertyMetadata, object entity);

    private protected string ToDisplayString(object? value)
    {
        var valueProperty = ValueProperty;

        return valueProperty is not null
            ? $"{SelectedCondition.Humanize()} [{valueProperty.DisplayName}]"
            : $"{SelectedCondition.Humanize()} '{value}'";
    }
}
