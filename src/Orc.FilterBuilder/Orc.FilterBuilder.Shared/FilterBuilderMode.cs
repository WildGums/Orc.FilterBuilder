// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderMode.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Views;

    /// <summary>
    /// Enumeration of <see cref="FilterManagerControl"/> work modes
    /// </summary>
    public enum FilterBuilderMode
    {
        /// <summary>
        /// <see cref="FilterManagerControl"/> creates filtered collection
        /// </summary>
        Collection,

        /// <summary>
        /// <see cref="FilterManagerControl"/> creates filtering function
        /// </summary>
        FilteringFunction
    }
}