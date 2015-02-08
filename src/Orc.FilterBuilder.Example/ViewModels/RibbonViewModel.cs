// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Example.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel.MVVM;
    using global::FilterBuilder.Example.Models;
    using global::FilterBuilder.Example.Services;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly ITestDataService _testDataService;

        public RibbonViewModel(ITestDataService testDataService)
        {
            _testDataService = testDataService;
            RawItems = _testDataService.GetTestItems();
        }

        public ObservableCollection<TestEntity> RawItems { get; private set; }
    }
}