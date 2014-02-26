// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace FilterBuilder.Test.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel;
    using Catel.MVVM;
    using FilterBuilder.Test.Models;
    using FilterBuilder.Test.Services;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ITestDataService _testDataService;

        #region Constructors
        public MainWindowViewModel(ITestDataService testDataService)
        {
            Argument.IsNotNull(() => testDataService);

            _testDataService = testDataService;

            RawItems = _testDataService.GenerateTestItems();
            FilteredItems = new ObservableCollection<TestEntity>();
        }
        #endregion

        #region Properties
        public ObservableCollection<TestEntity> RawItems { get; private set; }
        public ObservableCollection<TestEntity> FilteredItems { get; private set; }

        public override string Title
        {
            get { return "Filter Builder Test"; }
        }
        #endregion

        #region Methods
        #endregion
    }
}