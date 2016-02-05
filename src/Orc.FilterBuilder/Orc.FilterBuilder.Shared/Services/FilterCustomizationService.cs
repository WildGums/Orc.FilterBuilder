// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterCustomizationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using Catel;
    using Models;

    public class FilterCustomizationService : IFilterCustomizationService
    {
        public virtual void CustomizeInstanceProperties(IPropertyCollection instanceProperties)
        {
            Argument.IsNotNull(() => instanceProperties);

            // Remove Catel properties
            RemoveProperty(instanceProperties, "BusinessRuleErrorCount");
            RemoveProperty(instanceProperties, "BusinessRuleWarningCount");
            RemoveProperty(instanceProperties, "FieldErrorCount");
            RemoveProperty(instanceProperties, "FieldWarningCount");
            RemoveProperty(instanceProperties, "HasErrors");
            RemoveProperty(instanceProperties, "HasWarnings");
            RemoveProperty(instanceProperties, "IsDirty");
            RemoveProperty(instanceProperties, "IsEditable");
            RemoveProperty(instanceProperties, "IsInEditSession");
            RemoveProperty(instanceProperties, "IsReadOnly");
        }

        protected void RemoveProperty(IPropertyCollection instanceProperties, string propertyName)
        {
            for (int i = 0; i < instanceProperties.Properties.Count; i++)
            {
                if (string.Equals(instanceProperties.Properties[i].Name, propertyName))
                {
                    instanceProperties.Properties.RemoveAt(i--);
                }
            }
        }
    }
}