// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditFilterView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Views
{
    using System.Windows.Controls;
    using System.Windows.Data;
    using Catel.IoC;
    using Catel.Windows;
    using Catel.Windows.Data;
    using Orc.FilterBuilder.Services;
    using Orc.FilterBuilder.ViewModels;

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
                    foreach (var instanceProperty in instanceProperties.Properties)
                    {
                        var column = new DataGridTextColumn();
                        column.Header = instanceProperty.Name;
                        column.Binding = new Binding(instanceProperty.Name);

                        dataGrid.Columns.Add(column);
                    }
                }
            }
        }
    }
}