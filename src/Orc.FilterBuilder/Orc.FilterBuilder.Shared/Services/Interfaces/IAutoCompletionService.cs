// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAutoCompletionService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.FilterBuilder.Services
{
    using System;
    using System.Collections;
    using Models;

    public interface IAutoCompletionService
    {
        #region Methods
        /// <summary>
        /// Gets the auto complete values.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="source">The source.</param>
        /// <param name="metadataProvider">The metadata provider.</param>
        /// <returns>System.String[].</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source" /> is <c>null</c>.</exception>
        string[] GetAutoCompleteValues(string property, string filter, IEnumerable source, IMetadataProvider metadataProvider);
        #endregion
    }
}