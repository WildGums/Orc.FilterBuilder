// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyCollection.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System.Collections.Generic;

    public interface IPropertyCollection
    {
        List<IPropertyMetadata> Properties { get; }

        IPropertyMetadata GetProperty(string propertyName);
    }
}
