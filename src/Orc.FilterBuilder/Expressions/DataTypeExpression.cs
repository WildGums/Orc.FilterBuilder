namespace Orc.FilterBuilder;

using Catel.Data;

public abstract class DataTypeExpression : ModelBase
{
    public Condition SelectedCondition { get; set; }

    public bool IsValueRequired { get; set; } = true;

    public ValueControlType ValueControlType { get; set; }

    private void OnSelectedConditionChanged()
    {
        IsValueRequired = ConditionHelper.GetIsValueRequired(SelectedCondition);
    }

    public abstract bool CalculateResult(IPropertyMetadata propertyMetadata, object entity);
}
