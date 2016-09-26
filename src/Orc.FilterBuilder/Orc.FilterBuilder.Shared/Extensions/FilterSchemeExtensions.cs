// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Catel;
    using Catel.Collections;
    using Catel.Reflection;

    using MethodTimer;

    using Orc.FilterBuilder.Models;
    using Orc.FilterBuilder.Services;

    public static class FilterSchemeExtensions
    {
        #region Constants
        private const string Separator = "||";
        #endregion



        #region Methods
        /// <summary>
        ///     Apply current filter to entity and calculate result.
        /// </summary>
        /// <param name="f">Filter instance.</param>
        /// <param name="entity">Object on which to apply filter.</param>
        /// <returns></returns>
        public static bool CalculateResult(this FilterScheme f, object entity)
        {
            Argument.IsNotNull(() => entity);

            var root = f.Root;

            if (root != null)
                return root.CalculateResult(entity);

            return true;
        }

        /// <summary>
        ///     Convert internal expression tree to Linq expression tree.
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="f">Filter instance</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Invalid type</exception>
        [Time]
        public static Expression<Func<T, bool>> ToLinqExpression<T>(this FilterScheme f)
        {
            ParameterExpression parameterExpr = Expression.Parameter(typeof(T), "obj");

            return Expression.Lambda<Func<T, bool>>(f.Root.ToLinqExpression(parameterExpr), parameterExpr);
        }

        /// <summary>
        ///     Convert internal expression tree to Linq expression tree.
        /// </summary>
        /// <param name="f">Filter instance</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Invalid type</exception>
        [Time]
        public static LambdaExpression ToLinqExpression(this FilterScheme f)
        {
            ParameterExpression parameterExpr = Expression.Parameter(f.TargetType, "obj");

            return Expression.Lambda(f.Root.ToLinqExpression(parameterExpr), parameterExpr);
        }

        public static void EnsureIntegrity(this FilterScheme filterScheme, IReflectionService reflectionService)
        {
            Argument.IsNotNull(() => filterScheme);
            Argument.IsNotNull(() => reflectionService);

            foreach (var item in filterScheme.ConditionItems)
                item.EnsureIntegrity(reflectionService);
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
                    var splittedString = serializationValue.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
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

            var compiledDelegate = filterScheme.ToLinqExpression().Compile();

            IDisposable suspendToken = null;
            if (filteredCollection is ISuspendChangeNotificationsCollection)
                suspendToken = ((ISuspendChangeNotificationsCollection)filteredCollection).SuspendChangeNotifications();

            filteredCollection.Clear();

            foreach (var item in rawCollection.Cast<object>().Where(i => (bool)compiledDelegate.DynamicInvoke(i)))
                filteredCollection.Add(item);

            if (suspendToken != null)
                suspendToken.Dispose();
        }
        #endregion
    }
}