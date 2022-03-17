namespace Orc.FilterBuilder.Automation;

using Orc.Automation;
using Orc.Automation.Controls;

public static class EditFilterConditionTreeItemFactory
{
    public static EditFilterConditionTreeItemBase Create(TreeItem item)
    {
        //TODO:Vladimir: Take a look
        var isGroupItem = item.Find("PART_AddExpressionButton", numberOfWaits: 1) is not null;
        if (isGroupItem)
        {
            return new EditFilterConditionGroupTreeItem(item.Element);
        }

        return new EditFilterPropertyConditionTreeItem(item.Element);
    }
}