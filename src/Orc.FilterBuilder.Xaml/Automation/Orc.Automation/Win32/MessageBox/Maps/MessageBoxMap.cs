namespace Orc.Automation
{
    using System.Windows.Automation;
    using Controls;

    public class MessageBoxMap : AutomationBase
    {
        public MessageBoxMap(AutomationElement element) 
            : base(element)
        {
        }

        public Text ContentText => By.One<Text>();
        public Button YesButton => By.Name("Yes").One<Button>();
        public Button NoButton => By.Name("No").One<Button>();
        public Button OkButton => By.Name("OK").One<Button>();
        public Button CancelButton => By.Name("Cancel").One<Button>();
    }
}
