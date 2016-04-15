// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
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
    using Models;
    using Services;

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

        public static void EnsureIntegrity(this ConditionTreeItem conditionTreeItem, IReflectionService reflectionService)
        {
            Argument.IsNotNull(() => conditionTreeItem);
            Argument.IsNotNull(() => reflectionService);

            var propertyExpression = conditionTreeItem as PropertyExpression;
            if (propertyExpression != null)
            {
                propertyExpression.EnsureIntegrity(reflectionService);
            }

            foreach (var item in conditionTreeItem.Items)
            {
                item.EnsureIntegrity(reflectionService);
            }
        }

        public static void EnsureIntegrity(this PropertyExpression propertyExpression, IReflectionService reflectionService)
        {
            Argument.IsNotNull(() => propertyExpression);
            Argument.IsNotNull(() => reflectionService);

            if (propertyExpression.Property == null)
            {
                var serializationValue = propertyExpression.PropertySerializationValue;
                if (!string.IsNullOrWhiteSpace(serializationValue))
                {
                    var splittedString = serializationValue.Split(new[] {Separator}, StringSplitOptions.RemoveEmptyEntries);
                    if (splittedString.Length == 2)
                    {
                        var type = TypeCache.GetType(splittedString[0]);
                        if (type != null)
                        {
                            var typeProperties = reflectionService.GetInstanceProperties(type);
                            propertyExpression.Property = typeProperties.GetProperty(splittedString[1]);
                        }
                    }
                }
            }
            else
            {
                // We already have it, but make sure to get the right instance
                var property = propertyExpression.Property;

                var typeProperties = reflectionService.GetInstanceProperties(property.OwnerType);
                propertyExpression.Property = typeProperties.GetProperty(property.Name);
            }
        }

        [Time]
        public static void Apply(this FilterScheme filterScheme, IEnumerable rawCollection, IList filteredCollection)
        {
            Argument.IsNotNull(() => filterScheme);
            Argument.IsNotNull(() => rawCollection);
            Argument.IsNotNull(() => filteredCollection);

            IDisposable suspendToken = null;
            var filteredCollectionType = filteredCollection.GetType();
            if (filteredCollectionType.IsGenericTypeEx() && filteredCollectionType.GetGenericTypeDefinitionEx() == typeof (FastObservableCollection<>))
            {
                suspendToken = (IDisposable) filteredCollectionType.GetMethodEx("SuspendChangeNotifications", new Type [] { }).Invoke(filteredCollection, null);
            }

            filteredCollection.Clear();

            foreach (var item in rawCollection.Cast<object>().Where(filterScheme.CalculateResult))
            {
                filteredCollection.Add(item);
            }

            if (suspendToken != null)
            {
                suspendToken.Dispose();
            }
        }
        #endregion
    }
}