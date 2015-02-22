// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPreviewGeneratorService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.FilterBuilder.Services
{
    using Models;
    using System.Windows;

    public interface IPreviewGeneratorService
    {
        FrameworkElement GeneratePreviewControl(IMetadataProvider metadataProvider, object bindingSource, string path);
    }
}