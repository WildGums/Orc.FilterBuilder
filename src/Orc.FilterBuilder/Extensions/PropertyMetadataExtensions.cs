namespace Orc.FilterBuilder;

using System;

public static class PropertyMetadataExtensions
{
    /// <summary>
    /// Determines whether the property can be compared to another property of the same entity.
    /// </summary>
    /// <param name="propertyMetadata">The property on the left side of the comparison.</param>
    /// <param name="otherPropertyMetadata">The property on the right side of the comparison.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="otherPropertyMetadata"/> is a different property with the same type
    /// (ignoring nullability); otherwise <c>false</c>.
    /// </returns>
    public static bool CanCompareTo(this IPropertyMetadata propertyMetadata, IPropertyMetadata otherPropertyMetadata)
    {
        ArgumentNullException.ThrowIfNull(propertyMetadata);
        ArgumentNullException.ThrowIfNull(otherPropertyMetadata);

        if (ReferenceEquals(propertyMetadata, otherPropertyMetadata)
            || (propertyMetadata.OwnerType == otherPropertyMetadata.OwnerType && string.Equals(propertyMetadata.Name, otherPropertyMetadata.Name)))
        {
            return false;
        }

        var type = Nullable.GetUnderlyingType(propertyMetadata.Type) ?? propertyMetadata.Type;
        var otherType = Nullable.GetUnderlyingType(otherPropertyMetadata.Type) ?? otherPropertyMetadata.Type;

        return type == otherType;
    }
}
