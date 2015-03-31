// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyMetadata.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System;

    public interface IPropertyMetadata
    {
        /// <summary>
        /// Display name of the property
        /// </summary>
        string DisplayName { get; set; }

        string Name { get; }
        Type Type { get; }
        object GetValue(object instance);
        TValue GetValue<TValue>(object instance);
        void SetValue(object instance, object value);
        string SerializeState();
        void DeserializeState(string contentAsString);
    }
}