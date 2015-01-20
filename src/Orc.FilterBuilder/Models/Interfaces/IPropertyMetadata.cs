// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyMetadata.cs" company="Orcomp development team">
//     Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
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

        Type OwnerType { get; }

        Type Type { get; }

        /// <summary>
        /// <see cref="DisplayName"/> or <see cref="Name"/> if first is null
        /// </summary>
        string DisplayNameOrName { get; }

        object GetValue(object instance);

        TValue GetValue<TValue>(object instance);

        void SetValue(object instance, object value);
    }
}