namespace Orc.FilterBuilder.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Automation;
    using Controls;
    using NUnit.Framework;

    public static class EditFilterViewAssert
    {
        public static void Match(EditFilterView target, FilterScheme filterScheme)
        {
            var filterSchemeItemsList = filterScheme.ConditionItems?.ToList();
            var conditionNodesList = target?.ToList();

            if (filterSchemeItemsList is null && conditionNodesList is null)
            {
                return;
            }

            Assert.That(filterSchemeItemsList, Is.Not.Null);
            Assert.That(conditionNodesList, Is.Not.Null);
            
            while (filterSchemeItemsList.Any())
            {
                Assert.That(filterSchemeItemsList, Is.EqualTo(conditionNodesList).Using<ConditionTreeItem, EditFilterConditionTreeItemBase>(IsEquivalent));

                filterSchemeItemsList = filterSchemeItemsList.SelectMany(x => x.Items).ToList();
                conditionNodesList = conditionNodesList.SelectMany(x => x.Children).ToList();
            }

            Assert.That(conditionNodesList, Is.Empty);
        }

        private static bool IsEquivalent(ConditionTreeItem filterSchemeItem, EditFilterConditionTreeItemBase treeItem)
        {
            var groupTreeItem = treeItem as EditFilterConditionGroupTreeItem;
            if (filterSchemeItem is ConditionGroup groupFilterSchemeItem)
            {
                return groupFilterSchemeItem.Type == groupTreeItem?.GroupType;
            }

            var expressionTreeItem = treeItem as EditFilterPropertyConditionTreeItem;
            if (filterSchemeItem is PropertyExpression expressionFilterSchemeItem)
            {
                if (expressionTreeItem is null)
                {
                    return false;
                }

                var dataTypeExpression = expressionFilterSchemeItem.DataTypeExpression;
                var dataTypeCondition = dataTypeExpression.SelectedCondition;

                if (!Equals(expressionTreeItem.Property, expressionFilterSchemeItem.Property.DisplayName)
                    || expressionTreeItem.Condition != dataTypeCondition)
                {
                    return false;
                }

                if (!dataTypeExpression.IsValueRequired)
                {
                    return true;
                }

                try
                {
                    dynamic valueExpression = dataTypeExpression;
                    object value = valueExpression.Value;

                    var expressionTreeItemValue = expressionTreeItem.Value;
                    if (expressionTreeItemValue is Number number)
                    {
                        expressionTreeItemValue = number.Value;
                    }

                    var result = Equals(value, expressionTreeItemValue);

                    return result;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
