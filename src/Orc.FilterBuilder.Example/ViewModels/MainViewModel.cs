// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Example.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.MVVM;
    using FilterBuilder.Services;
    using global::FilterBuilder.Example.Models;
    using global::FilterBuilder.Example.Services;

    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

       private readonly ITestDataService _testDataService;
        private readonly IFilterService _filterService;

        #region Constructors
        public MainViewModel(ITestDataService testDataService, IFilterService filterService)
        {
            Argument.IsNotNull(() => testDataService);
            Argument.IsNotNull(() => filterService);

            _testDataService = testDataService;
            _filterService = filterService;
            _filterService.SelectedFilterChanged += OnFilterServiceSelectedFilterChanged;
            RawItems = _testDataService.GetTestItems();
            FilteredItems = new FastObservableCollection<TestEntity>();

            FilteredItems.CollectionChanged += (sender, e) => Log.Info("Collection updated");
        }

        private void OnFilterServiceSelectedFilterChanged(object sender, EventArgs e)
        {
            using (FilteredItems.SuspendChangeNotifications())
            {
                var filter = _filterService.SelectedFilter;
                var items = RawItems;
                var result = _filterService.FilterCollection(filter, items);
                FilteredItems.ReplaceRange(result);
            }
        }
        #endregion

        #region Properties
        public ObservableCollection<TestEntity> RawItems { get; private set; }
        public FastObservableCollection<TestEntity> FilteredItems { get; private set; }

        public override string Title
        {
            get { return "Filter Builder Test"; }
        }
        #endregion
    }
}