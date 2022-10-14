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

        public Orc.Automation.Controls.ListBox? FilterSchemesListBox => By.One<Orc.Automation.Controls.ListBox>();
    }
}
