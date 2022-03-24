namespace Orc.FilterBuilder.Automation;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(ControlTypeName = nameof(ControlType.Tree))]
public class EditFilterConditionTree : FrameworkElement, IEnumerable<EditFilterConditionTreeItemBase>
{
    protected readonly Tree _tree;

    public EditFilterConditionTree(AutomationElement element) 
        : base(element)
    {
        _tree = element.As<Tree>();
    }

    public IReadOnlyList<EditFilterConditionTreeItemBase> Children
        => _tree.ChildItems.Select(EditFilterConditionTreeItemFactory.Create)
            .ToList();

    public IEnumerator<EditFilterConditionTreeItemBase> GetEnumerator()
    {
        return Children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}