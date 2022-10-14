namespace Orc.FilterBuilder.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class EditFilterConditionGroupTreeItemMap : AutomationBase
{
    public EditFilterConditionGroupTreeItemMap(AutomationElement element) 
        : base(element)
    {
    }

    public ComboBox? GroupTypeComboBox => By.Id("PART_GroupTypeComboBox").One<ComboBox>();
    public Button? AddExpressionButton => By.Id("PART_AddExpressionButton").One<Button>();
    public Button? AddGroupButton => By.Id("PART_AddGroupButton").One<Button>();
    public Button? DeleteButton => By.Id("PART_DeleteGroupButton").One<Button>();
}
