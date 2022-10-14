namespace Orc.FilterBuilder
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

        object? GetValue(object instance);

        TValue GetValue<TValue>(object instance);

        void SetValue(object instance, object? value);
    }
}
