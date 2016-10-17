// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
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

#pragma warning disable AvoidAsyncVoid
        private async void OnFilterServiceSelectedFilterChanged(object sender, EventArgs e)
#pragma warning restore AvoidAsyncVoid
        {
            using (FilteredItems.SuspendChangeNotifications())
            {
                var filter = _filterService.SelectedFilter;
                var items = RawItems;
                var result = await _filterService.FilterCollectionAsync(filter, items);
                ((ICollection<TestEntity>)FilteredItems).ReplaceRange(result);
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