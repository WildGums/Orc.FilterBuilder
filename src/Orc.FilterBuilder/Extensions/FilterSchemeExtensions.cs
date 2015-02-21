// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.Reflection;
    using Models;
    using Services;

    public static class FilterSchemeExtensions
    {
        private const string Separator = "||";

        private static readonly IReflectionService _reflectionService = ServiceLocator.Default.ResolveType<IReflectionService>();

        public static void EnsureIntegrity(this FilterScheme filterScheme)
        {
            Argument.IsNotNull(() => filterScheme);

            foreach (var item in filterScheme.ConditionItems)
            {
                item.EnsureIntegrity();
            }
        }

        public static void EnsureIntegrity(this ConditionTreeItem conditionTreeItem)
        {
            Argument.IsNotNull(() => conditionTreeItem);

            var propertyExpression = conditionTreeItem as PropertyExpression;
            if (propertyExpression != null)
            {
                propertyExpression.EnsureIntegrity();
            }

            foreach (var item in conditionTreeItem.Items)
            {
                item.EnsureIntegrity();
            }
        }

        public static void EnsureIntegrity(this PropertyExpression propertyExpression)
        {
            if (propertyExpression.Property == null)
            {
                var serializationValue = propertyExpression.PropertySerializationValue;
                if (!string.IsNullOrWhiteSpace(serializationValue))
                {
                    var kvp = serializationValue.Split('|');
                    var type = TypeCache.GetType(kvp[0]);
                    var provider = (IPropertyMetadata)Activator.CreateInstance(type);
                    provider.DeserializeState(kvp[1]);
                    propertyExpression.Property = provider;
                }
            }
        }

        public static void Apply(this FilterScheme filterScheme, IEnumerable rawCollection, IList filteredCollection)
        {
            Argument.IsNotNull(() => filterScheme);
            Argument.IsNotNull(() => rawCollection);
            Argument.IsNotNull(() => filteredCollection);

            IDisposable suspendToken = null;
            var filteredCollectionType = filteredCollection.GetType();
            if (filteredCollectionType.IsGenericTypeEx() && filteredCollectionType.GetGenericTypeDefinitionEx() == typeof(FastObservableCollection<>))
            {
                suspendToken = (IDisposable)filteredCollectionType.GetMethodEx("SuspendChangeNotifications").Invoke(filteredCollection, null);
            }

            filteredCollection.Clear();

            foreach (object item in rawCollection)
            {
                if (filterScheme.CalculateResult(item))
                {
                    filteredCollection.Add(item);
                }
            }

            if (suspendToken != null)
            {
                suspendToken.Dispose();
            }
        }
    }
}