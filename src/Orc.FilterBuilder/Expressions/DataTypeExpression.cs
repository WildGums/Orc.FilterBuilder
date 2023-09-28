namespace Orc.FilterBuilder;

using Catel.Data;

public abstract class DataTypeExpression : ModelBase
{
    protected DataTypeExpression()
    {
        IsValueRequired = true;
    }

    public Condition SelectedCondition { get; set; }

    public bool IsValueRequired { get; set; }

    public ValueControlType ValueControlType { get; set; }

    private void OnSelectedConditionChanged()
    {
        IsValueRequired = ConditionHelper.GetIsValueRequired(SelectedCondition);
    }

    public abstract bool CalculateResult(IPropertyMetadata propertyMetadata, object entity);
}