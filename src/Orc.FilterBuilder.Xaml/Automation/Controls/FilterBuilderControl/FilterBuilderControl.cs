namespace Orc.FilterBuilder.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Views.FilterBuilderControl))]
    public class FilterBuilderControl : FrameworkElement<FilterBuilderControlModel, FilterBuilderControlMap>
    {
        public FilterBuilderControl(AutomationElement element)
            : base(element)
        {
        }

        public IReadOnlyList<FilterBuilderControlListItem> Items
            => Map.FilterSchemesListBox.GetItemsOfType<FilterBuilderControlListItem>();
    }
}
