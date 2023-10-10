namespace Orc.FilterBuilder;

using System.Collections.Generic;
using System;

public class FilterCustomizationService : IFilterCustomizationService
{
    public virtual void CustomizeInstanceProperties(IPropertyCollection instanceProperties)
    {
        ArgumentNullException.ThrowIfNull(instanceProperties);

        var catelProperties = new HashSet<string> {
            "BusinessRuleErrorCount",
            "BusinessRuleWarningCount",
            "FieldErrorCount",
            "FieldWarningCount",
            "HasErrors",
            "HasWarnings",
            "IsDirty",
            "IsEditable",
            "IsInEditSession",
            "IsReadOnly"
        };

        // Remove Catel properties
        instanceProperties.Properties.RemoveAll(x => catelProperties.Contains(x.Name));

        // Remove unsupported type properties
        instanceProperties.Properties.RemoveAll(x => !x.IsSupportedType());
    }
}
