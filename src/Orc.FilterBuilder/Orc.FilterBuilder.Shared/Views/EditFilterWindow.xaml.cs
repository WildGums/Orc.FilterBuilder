// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditFilterWindow.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Views
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Threading;
    using Catel.IoC;
    using Catel.Windows;
    using Converters;
    using Services;
    using ViewModels;

    /// <summary>
    /// Interaction logic for EditFilterWindow.xaml
    /// </summary>
    public partial class EditFilterWindow
    {
        #region Constructors
        public EditFilterWindow()
            : base(DataWindowMode.OkCancel, infoBarMessageControlGenerationMode: InfoBarMessageControlGenerationMode.None)
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
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
                    var reflectionService = dependencyResolver.Resolve<IReflectionService>(vm.FilterScheme.Tag);

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
        #endregion
    }
}