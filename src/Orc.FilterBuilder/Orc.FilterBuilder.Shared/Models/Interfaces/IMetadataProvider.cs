// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataProvider.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System.Collections.Generic;

    public interface IMetadataProvider
    {
        List<IPropertyMetadata> Properties { get; }
        bool IsAssignableFromEx(IMetadataProvider otherProvider);
        string SerializeState();
        void DeserializeState(string contentAsString);
    }
}