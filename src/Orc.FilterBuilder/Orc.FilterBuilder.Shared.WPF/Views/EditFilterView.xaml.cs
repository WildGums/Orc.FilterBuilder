// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditFilterView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder.Views
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Catel.IoC;
    using Converters;
    using Services;
    using ViewModels;

    public sealed partial class EditFilterView
    {
        #region Constructors
        public EditFilterView()
        {
            InitializeComponent();
        }
        #endregion

        protected override async void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            dataGrid.Columns.Clear();

            var vm = ViewModel as EditFilterViewModel;
            if (vm != null)
            {
                if (vm.AllowLivePreview)
                {
                    var dependencyResolver = this.GetDependencyResolver();
                    var reflectionService = dependencyResolver.Resolve<IReflectionService>(vm.FilterScheme.Scope);

                    var targetType = CollectionHelper.GetTargetType(vm.RawCollection);
                    var instanceProperties = await reflectionService.GetInstancePropertiesAsync(targetType);

                    foreach (var instanceProperty in instanceProperties.Properties)
                    {
                        var column = new DataGridTextColumn
                        {
                            Header = instanceProperty.DisplayName
                        };

                        var binding = new Binding
                        {
                            Converter = new ObjectToValueConverter(instanceProperty),
                            ConverterParameter = instanceProperty.Name
                        };

                        column.Binding = binding;

                        dataGrid.Columns.Add(column);
                    }
                }

                // Fix for SA-144
                var dispatcherOperation = Dispatcher.BeginInvoke(new Action(() => Focus()));
            }
        }
    }
}