// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using Catel;
    using Catel.Collections;
    using Catel.Reflection;
    using Orc.FilterBuilder.Models;

    public static class FilterSchemeExtensions
    {
        public static void Apply(this FilterScheme filterScheme, IEnumerable rawCollection, IList filteredCollection)
        {
            Argument.IsNotNull("filterScheme", filterScheme);
            Argument.IsNotNull("rawCollection", rawCollection);
            Argument.IsNotNull("filteredCollection", filteredCollection);

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