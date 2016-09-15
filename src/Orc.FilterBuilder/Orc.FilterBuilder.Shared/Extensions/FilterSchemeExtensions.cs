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
        ///     Creates the root node if it doesn't exists.
        /// </summary>
        /// <param name="f">The f.</param>
        public static void CreateRootNode(this FilterScheme f)
        {
            if (f.Root != null)
                return;

            f.ConditionItems.Add(f.CreateConditionGroup(ConditionGroupType.And));
        }

        /// <summary>
        /// Creates the root node if it doesn't exists.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="targetProperties">The target properties.</param>
        public static void CreateRootProperty(this FilterScheme f, IEnumerable<IPropertyMetadata> targetProperties)
        {
            if (f.Root.Items.Count != 0)
                return;

            f.Root.Items.Add(f.CreatePropertyExpression(targetProperties, f.Root));
        }

        /// <summary>Removes redundant nodes, and reorder items if required.</summary>
        public static void OptimizeTree(this FilterScheme f)
        {
            if (f.Root == null)
            {
                return;
            }

            f.OptimizeTree(f.Root);
        }

        public static void OptimizeTree(this FilterScheme f, ConditionTreeItem node)
        {
            // If single node, optimize
            if (node.Items.Count == 1 && node.Parent != null)
            {
                var childNode = node.Items.FirstOrDefault();

                // If parent is not root condition, merge this condition with parent's parent
                if (node.Parent != null)
                {
                    childNode.Move(node.Parent);

                    node.Parent.Items.Remove(node);
                }
                else if (childNode is ConditionGroup)
                {
                    // Node is root, clear its items
                    node.Items.Clear();

                    // Make child the new Root
                    childNode.Parent = null;

                    f.ConditionItems.Clear();
                    f.ConditionItems.Add(childNode);
                }
            }

            // Recurse
            node.Items.ToList().ForEach(f.OptimizeTree);

            // Make sure PropertyExpression items are grouped together, before ConditionGroup items
            int lastPropIndex = 0;
            bool startSorting = false;

            for (int i = 0; i < node.Items.Count; i++)
            {
                if (!startSorting)
                {
                    if (node.Items[i] is PropertyExpression)
                    {
                        lastPropIndex = i;
                    }
                    else
                    {
                        startSorting = true;
                    }
                }
                else if (node.Items[i] is PropertyExpression)
                {
                    var propNode = node.Items[i];
                    node.Items.Remove(propNode);
                    node.Items.Insert(++lastPropIndex, propNode);
                }
            }
        }

        public static void Move(this ConditionTreeItem movingNode, ConditionTreeItem targetNode)
        {
            int propExpIndex = targetNode.Items.Count(cti => cti is PropertyExpression);

            movingNode.Parent.Items.Remove(movingNode);
            targetNode.Items.Insert(propExpIndex, movingNode);

            movingNode.Parent = targetNode;
        }

        /// <summary>
        ///     Determines whether target item can be removed.
        /// </summary>
        /// <param name="f">Filter instance</param>
        /// <param name="targetItem">
        ///     Target item underlying type should be
        ///     <see cref="PropertyExpression" />.
        /// </param>
        /// <returns>Whether target item can be removed.</returns>
        public static bool CanRemove(this FilterScheme f, ConditionTreeItem targetItem)
        {
            Argument.IsNotNull(() => targetItem);
            Argument.IsOfType("targetItem", targetItem, typeof(PropertyExpression));

            // Not root condition
            if (targetItem.Parent.Parent != null)
                return true;

            // If root condition, check there if there are other properties
            return targetItem.Parent.Items.Count > 1;
        }

        /// <summary>
        ///     Adds a new item to target's parent <see cref="ConditionGroup" /> if
        ///     type is the same as parent's <see cref="ConditionGroupType" />, or
        ///     create a new <see cref="ConditionGroup" /> containing targetItem and
        ///     a new, blank <see cref="PropertyExpression" /> item otherwise.
        /// </summary>
        /// <param name="f">Filter instance</param>
        /// <param name="targetItem">Target item</param>
        /// <param name="type">Condition type (and/or)</param>
        /// <param name="targetProperties">Target type properties metadata.</param>
        public static void Add(this FilterScheme f, ConditionTreeItem targetItem, ConditionGroupType type, IEnumerable<IPropertyMetadata> targetProperties)
        {
            Argument.IsNotNull(() => targetItem);
            Argument.IsOfType("targetItem", targetItem, typeof(PropertyExpression));

            ConditionGroup parent = targetItem.Parent as ConditionGroup;
            PropertyExpression newItem = f.CreatePropertyExpression(targetProperties);

            // Same condition type, add new item to parent
            if (type == parent.Type)
            {
                int targetItemIndex = parent.Items.IndexOf(targetItem);

                parent.Items.Insert(targetItemIndex + 1, newItem);
                newItem.Parent = parent;
            }

            // Different condition type
            else
            {
                // Special case, where parent is root, and contains a single item.
                // Since condition type is different, and a condition cannot contain
                // a single item of ConditionGroup type, switch root type to new type
                if (parent.Parent == null && parent.Items.Count == 1)
                {
                    parent.Type = type;

                    parent.Items.Add(newItem);
                    newItem.Parent = parent;
                }

                // New condition group
                // Create new condition group, relate with targetItem and newItem, and
                // deny initial parent targetItem's care in profit of that of new
                // condition group
                else
                {
                    ConditionGroup newParent = f.CreateConditionGroup(type, parent, targetItem, newItem);

                    parent.Items.Remove(targetItem);
                    parent.Items.Add(newParent);
                }
            }
        }

        /// <summary>
        ///     Removes specified target item.
        /// </summary>
        /// <param name="f">Filter instance</param>
        /// <param name="targetItem">
        ///     Target item underlying type should be
        ///     <see cref="PropertyExpression" />.
        /// </param>
        public static void Remove(this FilterScheme f, ConditionTreeItem targetItem)
        {
            Argument.IsNotNull(() => targetItem);
            Argument.IsOfType("targetItem", targetItem, typeof(PropertyExpression));

            if (!f.CanRemove(targetItem))
                return;

            ConditionGroup parent = targetItem.Parent as ConditionGroup;
            parent.Items.Remove(targetItem);

            f.OptimizeTree(parent);
        }

        /// <summary>
        ///     Helper method which setups a PropertyExpression with default property and optional parent.
        /// </summary>
        /// <param name="f">Filter instance</param>
        /// <param name="targetProperties">Target type properties metadata.</param>
        /// <param name="parent">The (optional) parent.</param>
        /// <returns></returns>
        public static PropertyExpression CreatePropertyExpression(this FilterScheme f, IEnumerable<IPropertyMetadata> targetProperties, ConditionTreeItem parent = null)
        {
            return new PropertyExpression { Parent = parent, Property = targetProperties.FirstOrDefault() };
        }

        /// <summary>
        ///     Helper method which setups a ConditionGroup with given parameters.
        /// </summary>
        /// <param name="f">Filter instance</param>
        /// <param name="type">Condition Type.</param>
        /// <param name="parent">The (optional) parent.</param>
        /// <param name="child1">The (optional) first child.</param>
        /// <param name="child2">The (optional) second child.</param>
        /// <returns></returns>
        public static ConditionGroup CreateConditionGroup(this FilterScheme f, ConditionGroupType type, ConditionTreeItem parent = null, ConditionTreeItem child1 = null, ConditionTreeItem child2 = null)
        {
            var conditionGroup = new ConditionGroup { Parent = parent, Type = type };

            if (child1 != null)
            {
                conditionGroup.Items.Add(child1);
                child1.Parent = conditionGroup;
            }

            if (child2 != null)
            {
                conditionGroup.Items.Add(child2);
                child2.Parent = conditionGroup;
            }

            return conditionGroup;
        }

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

            IDisposable suspendToken = null;
            if (filteredCollection is ISuspendChangeNotificationsCollection)
            {
                suspendToken = ((ISuspendChangeNotificationsCollection)filteredCollection).SuspendChangeNotifications();
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