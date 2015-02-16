using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.FilterBuilder.AlternativeExample.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using FilterBuilder.Services;
    using Services;

    public class MainViewModel: ViewModelBase
    {
        private IDataProvider _dataProvider;

        private IFilterService _filterService;

        public MainViewModel(IDataProvider dataProvider, IFilterService filterService)
        {
            Argument.IsNotNull(() => dataProvider);

            _dataProvider = dataProvider;
            _filterService = filterService;

            FilteredCollection = new List<object>();
        }

        public IEnumerable<object> OriginalCollection
        {
            get
            {
                return _dataProvider.GetData();
            }
        }

        public IEnumerable<object> FilteredCollection { get; private set; }

        protected override Task Initialize()
        {
            _filterService.SelectedFilterChanged += OnSelectedFilterChanged;
            return base.Initialize();
        }

        protected override Task Close()
        {
            _filterService.SelectedFilterChanged -= OnSelectedFilterChanged;
            return base.Close();
        }

        private void OnSelectedFilterChanged(object sender, EventArgs e)
        {
            FilteredCollection = _filterService.FilterCollection(_filterService.SelectedFilter, OriginalCollection);
            RaisePropertyChanged(() => FilteredCollection);
        }
    }
}
