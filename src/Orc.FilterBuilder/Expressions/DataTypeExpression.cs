namespace Orc.FilterBuilder;

using System.Text.Json.Serialization;
using Catel.Data;

public abstract class DataTypeExpression : ModelBase
{
    public Condition SelectedCondition { get; set; }

    [JsonIgnore]
    public bool IsValueRequired { get; set; } = true;

    public ValueControlType ValueControlType { get; set; }

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
}
