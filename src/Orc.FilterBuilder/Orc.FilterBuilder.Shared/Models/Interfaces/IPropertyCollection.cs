// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyCollection.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder.Models
{
    using System.Collections.Generic;

    public interface IPropertyCollection
    {
        List<IPropertyMetadata> Properties { get; }

        IPropertyMetadata GetProperty(string propertyName);
    }
}