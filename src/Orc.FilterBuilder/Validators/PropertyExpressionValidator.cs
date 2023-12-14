﻿namespace Orc.FilterBuilder;

using Catel.Data;
using System.Collections.Generic;

public class PropertyExpressionValidator : ValidatorBase<PropertyExpression>
{
    protected override void ValidateFields(PropertyExpression instance, List<IFieldValidationResult> validationResults)
    {
        if (instance.Property is null)
        {
            validationResults.Add(FieldValidationResult.CreateError("Property", "Property can not be null"));
        }
    }
}