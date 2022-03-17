namespace Orc.FilterBuilder.Automation
{
    using Catel.IoC;
    using Orc.Automation;

    public class EditFilterViewPeer : ControlRunMethodAutomationPeerBase<Views.EditFilterView>
    {
        public EditFilterViewPeer(Views.EditFilterView owner)
            : base(owner)
        {
        }

        [AutomationMethod]
        public FilterScheme GetFilterScheme()
        {
            var viewModel = Control.ViewModel as ViewModels.EditFilterViewModel;
            return viewModel?.FilterScheme;
        }

        [AutomationMethod]
        public FilterSchemeEditInfo GetFilterSchemeEditInfo()
        {
            if (Control.DataContext is not ViewModels.EditFilterViewModel vm)
            {
                return null;
            }
            
            var filterSchemeEditInfo = new FilterSchemeEditInfo(vm.FilterScheme, vm.RawCollection, vm.AllowLivePreview, vm.EnableAutoCompletion);
            return filterSchemeEditInfo;
        }

        [AutomationMethod]
        public void SetFilterSchemeEditInfo(FilterSchemeEditInfo filterSchemeEditInfo)
        {
            var typeFactory = this.GetTypeFactory();
            var editFilterViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ViewModels.EditFilterViewModel>(filterSchemeEditInfo);

            Control.DataContext = editFilterViewModel;
        }
    }
}
