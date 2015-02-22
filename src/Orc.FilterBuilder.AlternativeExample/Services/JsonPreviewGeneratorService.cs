// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonPreviewGeneratorService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.FilterBuilder.AlternativeExample.Services
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Catel;
    using Controls;
    using FilterBuilder.Services;
    using Models;

    public class JsonPreviewGeneratorService : IPreviewGeneratorService
    {
        public FrameworkElement GeneratePreviewControl(IMetadataProvider metadataProvider, object bindingSource, string path)
        {
            Argument.IsOfType(() => metadataProvider, typeof(DictionaryMetadataProvider));
            var jsonViewer = new JsonCollectionViewer();
            BindingOperations.SetBinding(jsonViewer, JsonCollectionViewer.SourceProperty, new Binding(path) { Source = bindingSource });

            return jsonViewer;
        }
    }
}