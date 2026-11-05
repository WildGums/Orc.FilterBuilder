namespace Orc.FilterBuilder;

using System;
using System.Collections;
using System.Linq;
using Catel.Collections;
using Catel.Reflection;
using MethodTimer;

public static class FilterSchemeExtensions
{
    private const string Separator = "||";

    public static void EnsureIntegrity(this FilterScheme filterScheme, IReflectionService reflectionService)
    {
        ArgumentNullException.ThrowIfNull(filterScheme);
        ArgumentNullException.ThrowIfNull(reflectionService);

        foreach (var item in filterScheme.ConditionItems)
        {
            item.EnsureIntegrity(reflectionService);
        }
    }

    private static void EnsureIntegrity(this ConditionTreeItem conditionTreeItem, IReflectionService reflectionService)
    {
        ArgumentNullException.ThrowIfNull(conditionTreeItem);
        ArgumentNullException.ThrowIfNull(reflectionService);

        var propertyExpression = conditionTreeItem as PropertyExpression;
        propertyExpression?.EnsureIntegrity(reflectionService);
        propertyExpression?.EnsureValuePropertyIntegrity(reflectionService);

        foreach (var item in conditionTreeItem.Items)
        {
            item.EnsureIntegrity(reflectionService);
        }
    }

    private static void EnsureIntegrity(this PropertyExpression propertyExpression, IReflectionService reflectionService)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        ArgumentNullException.ThrowIfNull(reflectionService);

        IPropertyCollection typeProperties;

        var property = propertyExpression.Property;
        if (property is not null)
        {
            // We already have it, but make sure to get the right instance

            typeProperties = reflectionService.GetInstanceProperties(property.OwnerType);
            propertyExpression.Property = typeProperties.GetProperty(property.Name);

            return;
        }

        var serializationValue = propertyExpression.PropertySerializationValue;
        if (string.IsNullOrWhiteSpace(serializationValue))
        {
            return;
        }

        var splittedString = serializationValue.Split(new[] {Separator}, StringSplitOptions.RemoveEmptyEntries);
        if (splittedString.Length != 2)
        {
            return;
        }

        var type = TypeCache.GetType(splittedString[0]);
        if (type is null)
        {
            return;
        }

        typeProperties = reflectionService.GetInstanceProperties(type);
        propertyExpression.Property = typeProperties.GetProperty(splittedString[1]);
    }

    private static void EnsureValuePropertyIntegrity(this PropertyExpression propertyExpression, IReflectionService reflectionService)
    {
        var dataTypeExpression = propertyExpression.DataTypeExpression;
        var valueProperty = dataTypeExpression?.ValueProperty;
        if (dataTypeExpression is null || valueProperty is null)
        {
            return;
        }

        // Use the live instance so values can be read. If the property no longer exists, keep the stored
        // one: the comparison then sees null instead of silently falling back to the constant value.
        var typeProperties = reflectionService.GetInstanceProperties(valueProperty.OwnerType);
        dataTypeExpression.ValueProperty = typeProperties.GetProperty(valueProperty.Name) ?? valueProperty;
    }

    [Time]
    public static void Apply(this FilterScheme filterScheme, IEnumerable rawCollection, IList filteredCollection)
    {
        ArgumentNullException.ThrowIfNull(filterScheme);
        ArgumentNullException.ThrowIfNull(rawCollection);
        ArgumentNullException.ThrowIfNull(filteredCollection);

        IDisposable? suspendToken = null;
        if (filteredCollection is ISuspendChangeNotificationsCollection collection)
        {
            suspendToken = collection.SuspendChangeNotifications();
        }

        filteredCollection.Clear();

        foreach (var item in rawCollection.Cast<object>().Where(filterScheme.CalculateResult))
        {
            filteredCollection.Add(item);
        }

        suspendToken?.Dispose();
    }
}
