namespace Orc.FilterBuilder.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(ControlTypeName = nameof(ControlType.ListItem))]
public class FilterBuilderControlListItem : ListItem
{
    public FilterBuilderControlListItem(AutomationElement element) 
        : base(element)
    {
    }

    private FilterBuilderControlListItemMap Map => Map<FilterBuilderControlListItemMap>();

    public string Title => Map.Title?.Value ?? string.Empty;
    public override string DisplayText => Title;

    public bool CanEdit()
    {
        return Map.EditSchemeButton?.IsVisible() ?? false;
    }

    public EditFilterWindow? Edit()
    {
        Map.EditSchemeButton?.Click();

        Wait.UntilResponsive(200);

        var hostWindow = Element.GetHostWindow();

        return hostWindow?.Find<EditFilterWindow>(name: "Filter scheme");
    }

    public bool CanDelete()
    {
        return Map.DeleteSchemeButton?.IsVisible() ?? false;
    }

    public void Delete()
    {
        Map.DeleteSchemeButton?.Click();

        Wait.UntilResponsive();

        var hostWindow = Element.GetHostWindow();
        var messageBox = hostWindow?.Find<MessageBox>();
        messageBox?.Yes();
    }
}
