// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Reflection;
    using MethodTimer;

    public static class FilterSchemeExtensions
    {
        #region Constants
        private const string Separator = "||";
        #endregion

        #region Methods
        public static void EnsureIntegrity(this FilterScheme filterScheme, IReflectionService reflectionService)
        {
            Argument.IsNotNull(() => filterScheme);
            Argument.IsNotNull(() => reflectionService);

            foreach (var item in filterScheme.ConditionItems)
            {
                item.EnsureIntegrity(reflectionService);
            }
        }

        private static void EnsureIntegrity(this ConditionTreeItem conditionTreeItem, IReflectionService reflectionService)
        {
            Argument.IsNotNull(() => conditionTreeItem);
            Argument.IsNotNull(() => reflectionService);

            var propertyExpression = conditionTreeItem as PropertyExpression;
            propertyExpression?.EnsureIntegrity(reflectionService);

            foreach (var item in conditionTreeItem.Items)
            {
                item.EnsureIntegrity(reflectionService);
            }
        }

        private static void EnsureIntegrity(this PropertyExpression propertyExpression, IReflectionService reflectionService)
        {
            Argument.IsNotNull(() => propertyExpression);
            Argument.IsNotNull(() => reflectionService);

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
            if (typeProperties is null)
            {
                return;
            }

            propertyExpression.Property = typeProperties.GetProperty(splittedString[1]);
        }

        [Time]
        public static void Apply(this FilterScheme filterScheme, IEnumerable rawCollection, IList filteredCollection)
        {
            Argument.IsNotNull(() => filterScheme);
            Argument.IsNotNull(() => rawCollection);
            Argument.IsNotNull(() => filteredCollection);

            IDisposable suspendToken = null;
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
        #endregion
    }
}
