namespace Orc.FilterBuilder.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class FilterBuilderControlListItemMap : AutomationBase
{
    public FilterBuilderControlListItemMap(AutomationElement element)
        : base(element)
    {
    }

    public Text? Title => By.One<Text>();
    public Button? EditSchemeButton => By.Id().One<Button>();
    public Button? DeleteSchemeButton => By.Id().One<Button>();
}