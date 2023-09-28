namespace Orc.FilterBuilder.Automation;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public abstract class EditFilterConditionTreeItemBase : FrameworkElement
{
    protected readonly TreeItem _treeItem;

    protected EditFilterConditionTreeItemBase(AutomationElement element)
        : base(element)
    {
        _treeItem = element.As<TreeItem>();
    }

    public IReadOnlyList<EditFilterConditionTreeItemBase> Children
        => _treeItem.ChildItems.Select(EditFilterConditionTreeItemFactory.Create)
            .ToList();

    public abstract void Delete();
}