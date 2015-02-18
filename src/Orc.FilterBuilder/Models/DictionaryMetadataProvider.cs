// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryMetadataProvider.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.FilterBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    public class DictionaryMetadataProvider : IMetadataProvider
    {
        private List<IPropertyMetadata> _properties;

        public DictionaryMetadataProvider(Dictionary<string, Type> dictionarySchema)
        {
            Argument.IsNotNull(() => dictionarySchema);
            _properties = dictionarySchema.Select(kvp => new DictionaryEntryMetadata(kvp.Key, kvp.Value)).Cast<IPropertyMetadata>().ToList();
        }

        public List<IPropertyMetadata> Properties
        {
            get { return _properties; }
        }

        public bool IsAssignableFromEx(IMetadataProvider otherProvider)
        {
            // suppose that anything can be converted into empty dictionary so the source can be really schemaless
            return true;
        }
    }
}