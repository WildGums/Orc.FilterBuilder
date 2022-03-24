namespace Orc.Automation
{
    using System.Windows.Automation;

    //TODO:Vladimir: check class name on other machines
    [AutomatedControl(ClassName = "#32770")]
    public class MessageBox : AutomationControl
    {
        public MessageBox(AutomationElement element) 
            : base(element)
        {
        }

        private MessageBoxMap Map => Map<MessageBoxMap>();

        public string Title => Element.Current.Name;
        public string Message => Map.ContentText.Value;

        public void Yes()
        {
            Map.YesButton?.Click();
        }

        public void No()
        {
            Map.NoButton?.Click();
        }

        public void Ok()
        {
            Map.OkButton?.Click();
        }

        public void Cancel()
        {
            Map.CancelButton?.Click();
        }
    }
}
