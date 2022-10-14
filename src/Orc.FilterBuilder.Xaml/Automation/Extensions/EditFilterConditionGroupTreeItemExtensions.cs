namespace Orc.FilterBuilder.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Orc.Automation;
    using Condition = FilterBuilder.Condition;

    public static class EditFilterConditionGroupTreeItemExtensions
    {
        public static EditFilterConditionGroupTreeItem And(this EditFilterConditionGroupTreeItem group)
        {
            ArgumentNullException.ThrowIfNull(group);

            var newGroup = group.AddGroup();
            if (newGroup is null)
            {
                throw new InvalidOperationException("Cannot add 'and' group");
            }

            return newGroup;
        }

        public static EditFilterConditionGroupTreeItem Or(this EditFilterConditionGroupTreeItem group)
        {
            ArgumentNullException.ThrowIfNull(group);

            var newGroup = group.AddGroup(ConditionGroupType.Or);
            if (newGroup is null)
            {
                throw new InvalidOperationException("Cannot add 'or' group");
            }

            return newGroup;
        }

        public static EditFilterConditionGroupTreeItem? FinishCondition(this EditFilterConditionGroupTreeItem group)
        {
            ArgumentNullException.ThrowIfNull(group);

            var parentGroup = group.Find(controlType: ControlType.TreeItem, scope: TreeScope.Parent);

            return parentGroup?.As<EditFilterConditionGroupTreeItem>();
        }

        public static EditFilterConditionGroupTreeItem Property(this EditFilterConditionGroupTreeItem group, string propertyName, Condition condition,
            object? value = default)
        {
            ArgumentNullException.ThrowIfNull(group);

            var propertyExpression = group.AddPropertyExpression();
            if (propertyExpression is null)
            {
                throw new InvalidOperationException($"Cannot add property expression for property '{propertyName}'");
            }

            propertyExpression.Property = propertyName;
            propertyExpression.Condition = condition;
            propertyExpression.Value = value;

            return group;
        }
    }
}
