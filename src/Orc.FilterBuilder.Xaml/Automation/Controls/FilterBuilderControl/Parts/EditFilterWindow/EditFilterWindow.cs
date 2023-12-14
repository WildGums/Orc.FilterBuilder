namespace Orc.FilterBuilder.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(ControlTypeName = nameof(ControlType.Window))]
public class EditFilterWindow : Window
{
    private EditFilterView? _editFilterView;

    public EditFilterWindow(AutomationElement element) 
        : base(element)
    {
    }

    private EditFilterWindowMap Map => Map<EditFilterWindowMap>();

    public EditFilterView? EditFilterView => _editFilterView ??= Map.EditFilterView;

    public void Accept() => Map.OkButton?.Click();
    public void Decline() => Map.CancelButton?.Click();
}