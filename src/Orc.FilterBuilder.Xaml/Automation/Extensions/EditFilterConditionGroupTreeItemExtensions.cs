namespace Orc.FilterBuilder.Automation
{
    using System.Windows.Automation;
    using Catel;
    using Orc.Automation;
    using Condition = FilterBuilder.Condition;

    public static class EditFilterConditionGroupTreeItemExtensions
    {
        public static EditFilterConditionGroupTreeItem And(this EditFilterConditionGroupTreeItem group)
        {
            Argument.IsNotNull(() => group);

            var newGroup = group.AddGroup();

            return newGroup;
        }

        public static EditFilterConditionGroupTreeItem Or(this EditFilterConditionGroupTreeItem group)
        {
            Argument.IsNotNull(() => group);

            var newGroup = group.AddGroup(ConditionGroupType.Or);

            return newGroup;
        }

        public static EditFilterConditionGroupTreeItem FinishCondition(this EditFilterConditionGroupTreeItem group)
        {
            Argument.IsNotNull(() => group);

            var parentGroup = group.Find(controlType: ControlType.TreeItem, scope: TreeScope.Parent);

            return parentGroup?.As<EditFilterConditionGroupTreeItem>();
        }

        public static EditFilterConditionGroupTreeItem Property(this EditFilterConditionGroupTreeItem group, string propertyName, Condition condition,
            object value = default)
        {
            Argument.IsNotNull(() => group);

            var propertyExpression = group.AddPropertyExpression();

            propertyExpression.Property = propertyName;
            propertyExpression.Condition = condition;
            propertyExpression.Value = value;

            return group;
        }
    }
}
