namespace Orc.FilterBuilder.Automation
{
    using Orc.Automation;

    public class EditFilterViewModel : FrameworkElementModel
    {
        public EditFilterViewModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public FilterSchemeEditInfo? FilterSchemeEditInfo
        {
            get => _accessor.Execute<FilterSchemeEditInfo>(nameof(EditFilterViewPeer.GetFilterSchemeEditInfo));
            set => _accessor.Execute(nameof(EditFilterViewPeer.SetFilterSchemeEditInfo), value);
        }
    }
}
