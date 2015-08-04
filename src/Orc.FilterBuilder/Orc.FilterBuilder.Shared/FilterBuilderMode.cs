// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderMode.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Views;

    /// <summary>
    /// Enumeration of <see cref="FilterBuilderControl"/> work modes
    /// </summary>
    public enum FilterBuilderMode
    {
        /// <summary>
        /// <see cref="FilterBuilderControl"/> creates filtered collection
        /// </summary>
        Collection,

        /// <summary>
        /// <see cref="FilterBuilderControl"/> creates filtering function
        /// </summary>
        FilteringFunction
    }
}