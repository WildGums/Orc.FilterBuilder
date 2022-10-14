namespace Orc.FilterBuilder.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class EditFilterWindowMap : AutomationBase
    {
        public EditFilterWindowMap(AutomationElement element) 
            : base(element)
        {
        }

        public EditFilterView? EditFilterView => By.One<EditFilterView>();
        public Button? OkButton => By.Name("OK").One<Button>();
        public Button? CancelButton => By.Name("Cancel").One<Button>();
    }
}
