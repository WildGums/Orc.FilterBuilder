namespace Orc.FilterBuilder.Runtime.Serialization;

using System;
using Catel.Reflection;
using Catel.Runtime.Serialization;

public class FilterSchemeSerializerModifier : SerializerModifierBase<FilterScheme>
{
    public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
    {
        if (string.Equals(memberValue.Name, nameof(FilterScheme.TargetType)))
        {
            if (memberValue.Value is Type targetType)
            {
                memberValue.Value = targetType.FullName;
            }
        }
    }

    public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
    {
        if (string.Equals(memberValue.Name, nameof(FilterScheme.TargetType)))
        {
            if (memberValue.Value is string targetTypeAsString)
            {
                memberValue.Value = TypeCache.GetType(targetTypeAsString);
            }
        }
    }
}