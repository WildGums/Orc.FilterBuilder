namespace Orc.FilterBuilder.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class FilterBuilderControlMap : AutomationBase
    {
        public FilterBuilderControlMap(AutomationElement element)
            : base(element)
        {
        }

        public Orc.Automation.Controls.List FilterSchemesListBox => By.Id().One<Orc.Automation.Controls.List>();
    }
}
