// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditFilterView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Views
{
    using System.Windows.Controls;
    using System.Windows.Data;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Reflection;
    using Catel.Windows;
    using Converters;
    using Services;
    using ViewModels;

    /// <summary>
    /// Interaction logic for EditFilterWindow.xaml
    /// </summary>
    public partial class EditFilterWindow
    {
        public EditFilterWindow()
            : base(DataWindowMode.OkCancel, infoBarMessageControlGenerationMode: InfoBarMessageControlGenerationMode.None)
        {
            InitializeComponent();
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            dataGrid.Columns.Clear();

            var vm = ViewModel as EditFilterViewModel;
            if (vm != null)
            {
                if (vm.AllowLivePreview)
                {
                    var dependencyResolver = this.GetDependencyResolver();
                    var reflectionService = dependencyResolver.Resolve<IReflectionService>();

                    var targetType = CollectionHelper.GetTargetType(vm.RawCollection);
                    var instanceProperties = reflectionService.GetInstanceProperties(targetType);

                    var isModelBase = typeof (ModelBase).IsAssignableFromEx(targetType);

                    foreach (var instanceProperty in instanceProperties.Properties)
                    {
                        var column = new DataGridTextColumn
                        {
                            Header = instanceProperty.DisplayName
                        };

                        Binding binding;

                        if (isModelBase)
                        {
                            binding = new Binding
                            {
                                Converter = new ObjectToValueConverter(),
                                ConverterParameter = instanceProperty.Name
                            };
                        }
                        else
                        {
                            binding = new Binding(instanceProperty.Name);
                        }

                        column.Binding = binding;

                        dataGrid.Columns.Add(column);
                    }
                }
            }
        }
    }
}