namespace Orc.FilterBuilder.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class EditFilterPropertyConditionTreeItemMap : AutomationBase
{
    public EditFilterPropertyConditionTreeItemMap(AutomationElement element)
        : base(element)
    {
        EditFilterPropertyValueEditorPart = new EditFilterPropertyValueEditorPart(element);
    }

    public ComboBox PropertiesComboBox => By.Id("PART_PropertiesComboBox").One<ComboBox>();
    public ComboBox ConditionComboBox => By.Id("PART_ConditionComboBox").One<ComboBox>();
    public Button DeleteButton => By.Id("PART_DeleteExpressionButton").One<Button>();

    public EditFilterPropertyValueEditorPart EditFilterPropertyValueEditorPart { get; }
}