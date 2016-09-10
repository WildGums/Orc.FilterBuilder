// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Catel.IoC;
    using Catel.MVVM;

    using Orc.FilterBuilder.Models;
    using Orc.FilterBuilder.Services;

    /// <summary>
    ///   View Model for generic FilterBuilder
    /// </summary>
    /// <seealso cref="Catel.MVVM.ViewModelBase" />
    public class FilterBuilderViewModel : ViewModelBase
    {
        #region Fields
        private readonly IReflectionService _reflectionService;
        private readonly bool _isEditing = false;
        #endregion



        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterBuilderViewModel" /> class.
        /// </summary>
        /// <param name="targetType">Filter target type.</param>
        /// <param name="reflectionService">The reflection service.</param>
        public FilterBuilderViewModel(Type targetType, IReflectionService reflectionService) : this(TypeFactory.Default.CreateInstanceWithParameters<FilterScheme>(targetType), reflectionService)
        {
            _isEditing = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterBuilderViewModel" /> class.
        /// </summary>
        /// <param name="scheme">Filter scheme.</param>
        /// <param name="reflectionService">The reflection service.</param>
        public FilterBuilderViewModel(FilterScheme scheme, IReflectionService reflectionService) : base(false)
        {
            _reflectionService = reflectionService;

            CurrentFilter = scheme;

            DeleteCommand = new Command<ConditionTreeItem>(OnDeleteCommandExecute, OnDeleteCommandCanExecute);
            AddAndConditionCommand = new Command<ConditionTreeItem>(OnAddAndConditionCommandExecute);
            AddOrConditionCommand = new Command<ConditionTreeItem>(OnAddOrConditionCommandExecute);
        }
        #endregion



        #region Properties
        /// <summary>
        ///   Current FilterScheme
        /// </summary>
        [Model]
        public FilterScheme CurrentFilter { get; set; }

        /// <summary>
        ///   Filter items to display.
        /// </summary>
        [ViewModelToModel]
        public ObservableCollection<ConditionTreeItem> ConditionItems { get; set; }

        [ViewModelToModel]
        public bool IsExpressionValid { get; set; }

        /// <summary>
        ///   Target type properties, used to display items in property combobox.
        /// </summary>
        public IEnumerable<IPropertyMetadata> TargetProperties { get; set; }

        /// <summary>
        ///   Command for adding a new And condition.
        /// </summary>
        public ICommand AddAndConditionCommand { get; set; }

        /// <summary>
        ///   Command for adding a new or condition.
        /// </summary>
        public ICommand AddOrConditionCommand { get; set; }

        /// <summary>
        ///   Command for removing a given item.
        /// </summary>
        public ICommand DeleteCommand { get; set; }
        #endregion



        #region Methods
        /// <inheritdoc />
        protected override async Task InitializeAsync()
        {
            TargetProperties = (await _reflectionService.GetInstancePropertiesAsync(CurrentFilter.TargetType).ConfigureAwait(false)).Properties;

            if (!_isEditing)
                CurrentFilter.CreateRootProperty(TargetProperties);
        }

        private void OnAddAndConditionCommandExecute(ConditionTreeItem item)
        {
            CurrentFilter.Add(item, ConditionGroupType.And, TargetProperties);
        }

        private void OnAddOrConditionCommandExecute(ConditionTreeItem item)
        {
            CurrentFilter.Add(item, ConditionGroupType.Or, TargetProperties);
        }

        private bool OnDeleteCommandCanExecute(ConditionTreeItem item)
        {
            return CurrentFilter.CanRemove(item);
        }

        private void OnDeleteCommandExecute(ConditionTreeItem item)
        {
            CurrentFilter.Remove(item);
        }
        #endregion
    }
}