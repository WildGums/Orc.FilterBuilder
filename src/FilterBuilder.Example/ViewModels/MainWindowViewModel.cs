// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace FilterBuilder.Example.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using FilterBuilder.Example.Models;
    using FilterBuilder.Example.Services;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ITestDataService _testDataService;

        #region Constructors
        public MainWindowViewModel(ITestDataService testDataService)
        {
            Argument.IsNotNull(() => testDataService);

            _testDataService = testDataService;

            RawItems = _testDataService.GenerateTestItems();
            FilteredItems = new FastObservableCollection<TestEntity>();

            FilteredItems.CollectionChanged += (sender, e) => Console.WriteLine("Collection updated");
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

        #region Methods
        #endregion
    }
}