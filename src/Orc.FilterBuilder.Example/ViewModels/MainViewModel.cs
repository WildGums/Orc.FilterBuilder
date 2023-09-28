namespace Orc.FilterBuilder.Example.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Catel.Collections;
using Catel.Logging;
using Catel.MVVM;
using global::FilterBuilder.Example.Models;
using global::FilterBuilder.Example.Services;

public class MainViewModel : ViewModelBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly ITestDataService _testDataService;
    private readonly IFilterService _filterService;

    public MainViewModel(ITestDataService testDataService, IFilterService filterService)
    {
        ArgumentNullException.ThrowIfNull(testDataService);
        ArgumentNullException.ThrowIfNull(filterService);

        _testDataService = testDataService;
        _filterService = filterService;
        _filterService.SelectedFilterChanged += OnFilterServiceSelectedFilterChanged;
        RawItems = _testDataService.GetTestItems();
        FilteredItems = new FastObservableCollection<TestEntity>();

        FilteredItems.CollectionChanged += (_, _) => Log.Info("Collection updated");
    }

    public override string Title => "Filter Builder Test";

#pragma warning disable AvoidAsyncVoid
    private async void OnFilterServiceSelectedFilterChanged(object sender, EventArgs e)
#pragma warning restore AvoidAsyncVoid
    {
        using (FilteredItems.SuspendChangeNotifications())
        {
            var filter = _filterService.SelectedFilter;
            var items = RawItems;
            var result = await _filterService.FilterCollectionAsync(filter, items);
            FilteredItems.ReplaceRange(result);
        }
    }

    public ObservableCollection<TestEntity> RawItems { get; }
    public FastObservableCollection<TestEntity> FilteredItems { get; }
}
