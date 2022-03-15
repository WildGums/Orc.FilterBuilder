namespace Orc.FilterBuilder.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Views.EditFilterView))]
    public class EditFilterView : FrameworkElement<EditFilterViewModel>
    {
        public EditFilterView(AutomationElement element) 
            : base(element)
        {
        }
    }
}
