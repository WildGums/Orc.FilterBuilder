namespace Orc.FilterBuilder.Views;

using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using Catel.IoC;
using Converters;
using ViewModels;

public sealed partial class EditFilterView
{
    public EditFilterView()
    {
        InitializeComponent();
    }
        
    protected override void OnViewModelChanged()
    {
        base.OnViewModelChanged();

        PreviewDataGrid.Columns.Clear();

        if (ViewModel is not EditFilterViewModel vm)
        {
            return;
        }

        if (vm.AllowLivePreview)
        {
            var dependencyResolver = this.GetDependencyResolver();
            var reflectionService = dependencyResolver.ResolveRequired<IReflectionService>(vm.FilterScheme.Scope);

            var targetType = CollectionHelper.GetTargetType(vm.RawCollection);
            if (targetType is not null)
            {
                var instanceProperties = reflectionService.GetInstanceProperties(targetType);

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

                    PreviewDataGrid.Columns.Add(column);
                }
            }
        }

        // Fix for SA-144
        Dispatcher.BeginInvoke(new Action(() => Focus()));
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new Automation.EditFilterViewPeer(this);
    }
}