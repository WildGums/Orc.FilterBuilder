namespace Orc.FilterBuilder;

using Catel;
using Catel.Data;
using System.Collections.Generic;

public class PropertyExpressionValidator : ValidatorBase<PropertyExpression>
{
    protected override void ValidateFields(PropertyExpression instance, List<IFieldValidationResult> validationResults)
    {
        if (instance.Property is null)
        {
            validationResults.Add(FieldValidationResult.CreateError("Property", LanguageHelper.GetRequiredString("FilterBuilder_PropertyCanNotBeNull")));
        }
    }
}