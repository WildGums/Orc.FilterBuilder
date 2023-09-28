namespace Orc.FilterBuilder.Automation;

using System.Linq;
using System.Windows.Automation;
using Orc.Automation;

public class EditFilterConditionGroupTreeItem : EditFilterConditionTreeItemBase
{
    public EditFilterConditionGroupTreeItem(AutomationElement element) 
        : base(element)
    {
    }

    private EditFilterConditionGroupTreeItemMap Map => Map<EditFilterConditionGroupTreeItemMap>();

    public bool IsExpanded
    {
        get => _treeItem.IsExpanded;
        set => _treeItem.IsExpanded = value;
    }

    public ConditionGroupType? GroupType
    {
        get => Map.GroupTypeComboBox?.GetSelectedValue<ConditionGroupType>();
        set => Map.GroupTypeComboBox?.SelectValue(value);
    }

    public EditFilterConditionGroupTreeItem? AddGroup(ConditionGroupType groupType = ConditionGroupType.And)
    {
        Map.AddGroupButton?.Click();

        //Give time to create new item
        Wait.UntilResponsive();

        var newGroup = Children.LastOrDefault() as EditFilterConditionGroupTreeItem;
        if (newGroup is not null)
        {
            newGroup.GroupType = groupType;
        }

        return newGroup;
    }

    public EditFilterPropertyConditionTreeItem? AddPropertyExpression()
    {
        Map.AddExpressionButton?.Click();

        //Give time to create new item
        Wait.UntilResponsive();

        return Children.LastOrDefault() as EditFilterPropertyConditionTreeItem;
    }

    public override void Delete()
    {
        Map.DeleteButton?.Click();
    }
}
