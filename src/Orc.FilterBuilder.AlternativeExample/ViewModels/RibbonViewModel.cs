namespace Orc.FilterBuilder.AlternativeExample.ViewModels
{
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using FilterBuilder.Services;
    using FilterBuilder.ViewModels;
    using Models;
    using Services;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IFilterSchemeManager _filterSchemeManager;
        private readonly IUIVisualizerService _uiVisualizerService;
        private IDataProvider _dataProvider;

        public RibbonViewModel(IDataProvider dataProvider, IFilterSchemeManager filterSchemeManager, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => dataProvider);
            Argument.IsNotNull(() => filterSchemeManager);
            Argument.IsNotNull(() => uiVisualizerService);

            _dataProvider = dataProvider;
            _filterSchemeManager = filterSchemeManager;
            _uiVisualizerService = uiVisualizerService;

            NewSchemeCommand = new Command(OnNewSchemeExecute);
        }

        public Command NewSchemeCommand { get; private set; }

        private async void OnNewSchemeExecute()
        {
            //var filterScheme = new FilterScheme(_targetType);
            //var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, RawItems, true, true);

            //if (await _uiVisualizerService.ShowDialog<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
            //{
            //    AvailableSchemes.Add(filterScheme);
            //    _filterSchemes.Schemes.Add(filterScheme);
            //    SelectedFilterScheme = filterScheme;

            //    _filterSchemeManager.UpdateFilters();
            //}
        }
    }
}
