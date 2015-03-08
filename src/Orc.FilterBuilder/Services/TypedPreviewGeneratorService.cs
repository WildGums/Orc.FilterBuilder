// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedPreviewGeneratorService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Catel;
    using Catel.Data;
    using Catel.Reflection;
    using Converters;
    using Models;

    public class TypedPreviewGeneratorService : IPreviewGeneratorService
    {
        public FrameworkElement GeneratePreviewControl(IMetadataProvider metadataProvider, object bindingSource, string path)
        {
            Argument.IsOfType(() => metadataProvider, typeof (TypeMetadataProvider));

            var dataGrid = new DataGrid() {AutoGenerateColumns = false};
            BindingOperations.SetBinding(dataGrid, DataGrid.ItemsSourceProperty, new Binding(path) {Source = bindingSource});

            dataGrid.Columns.Clear();

            var instanceProperties = metadataProvider.Properties;

            var isModelBase = typeof (ModelBase).IsAssignableFromEx(((TypeMetadataProvider)metadataProvider).TargetType);

            foreach (var instanceProperty in instanceProperties)
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

            return dataGrid;
        }
    }
}