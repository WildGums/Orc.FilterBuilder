namespace Orc.FilterBuilder.Runtime.Serialization;

using System;
using Catel.Reflection;
using Catel.Runtime.Serialization;

public class PropertyExpressionSerializerModifier : SerializerModifierBase<PropertyExpression>
{
    private const string Separator = "||";

    private static readonly string[] Separators = { Separator };

    private readonly IReflectionService _reflectionService;

    public PropertyExpressionSerializerModifier(IReflectionService reflectionService)
    {
        ArgumentNullException.ThrowIfNull(reflectionService);

        _reflectionService = reflectionService;
    }

    public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
    {
        if (!string.Equals(memberValue.Name, nameof(PropertyExpression.Property)))
        {
            return;
        }

        if (memberValue.Value is IPropertyMetadata propertyInfo)
        {
            memberValue.Value = $"{propertyInfo.OwnerType.FullName}{Separator}{propertyInfo.Name}";
        }
    }

    public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
    {
        if (!string.Equals(memberValue.Name, nameof(PropertyExpression.Property)))
        {
            return;
        }

        if (memberValue.Value is not string propertyMetadata)
        {
            return;
        }

        // We need to delay this
        ((PropertyExpression)context.Model).PropertySerializationValue = propertyMetadata;

        var splitted = propertyMetadata.Split(Separators, StringSplitOptions.None);
        var type = TypeCache.GetTypeWithoutAssembly(splitted[0]);
        if (type is null)
        {
            return;
        }

        var instanceProperties = _reflectionService.GetInstanceProperties(type);
        memberValue.Value = instanceProperties.GetProperty(splitted[1]);
    }
}
