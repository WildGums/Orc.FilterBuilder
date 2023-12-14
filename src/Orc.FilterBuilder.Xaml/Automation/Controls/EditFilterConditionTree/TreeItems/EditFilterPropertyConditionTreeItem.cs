namespace Orc.FilterBuilder.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Condition = Condition;

public class EditFilterPropertyConditionTreeItem : EditFilterConditionTreeItemBase
{
    public EditFilterPropertyConditionTreeItem(AutomationElement element) 
        : base(element)
    {
    }

    public EditFilterPropertyConditionTreeItemMap Map => Map<EditFilterPropertyConditionTreeItemMap>();

    public string? Property
    {
        get => Map.PropertiesComboBox?.SelectedItem?.TryGetDisplayText();
        set => Map.PropertiesComboBox?.Select(x => Equals(x.TryGetDisplayText(), value));
    }

    public object? Value
    {
        get => Map.EditFilterPropertyValueEditorPart.Value;
        set => Map.EditFilterPropertyValueEditorPart.Value = value;
    }

    public Condition? Condition
    {
        get => Map.ConditionComboBox?.GetSelectedValue<Condition>();
        set => Map.ConditionComboBox?.SelectValue(value);
    }

    public override void Delete()
    {
        Map.DeleteButton?.Click();
    }
}
