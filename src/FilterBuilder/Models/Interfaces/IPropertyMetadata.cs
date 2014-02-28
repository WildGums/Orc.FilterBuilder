// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyMetadata.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System;

    public interface IPropertyMetadata
    {
        string Name { get; }
        Type Type { get; }
        Type OwnerType { get; }
        void SetValue(object instance, object value);
        object GetValue(object instance);
        TValue GetValue<TValue>(object instance);
    }
}