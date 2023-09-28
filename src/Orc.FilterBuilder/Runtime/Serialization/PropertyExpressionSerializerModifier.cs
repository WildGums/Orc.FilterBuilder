﻿namespace Orc.FilterBuilder.Runtime.Serialization;

using System;
using Catel.Reflection;
using Catel.Runtime.Serialization;

public class PropertyExpressionSerializerModifier : SerializerModifierBase<PropertyExpression>
{
    private const string Separator = "||";

    private static readonly string[] Separators = new string[] { Separator };

    private readonly IReflectionService _reflectionService;

    public PropertyExpressionSerializerModifier(IReflectionService reflectionService)
    {
        ArgumentNullException.ThrowIfNull(reflectionService);

        _reflectionService = reflectionService;
    }

    public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
    {
        if (string.Equals(memberValue.Name, nameof(PropertyExpression.Property)))
        {
            var propertyInfo = memberValue.Value as IPropertyMetadata;
            if (propertyInfo is not null)
            {
                memberValue.Value = string.Format("{0}{1}{2}", propertyInfo.OwnerType.FullName, Separator, propertyInfo.Name);
            }
        }
    }

    public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
    {
        if (string.Equals(memberValue.Name, nameof(PropertyExpression.Property)))
        {
            var propertyMetadata = memberValue.Value as string;
            if (propertyMetadata is not null)
            {
                // We need to delay this
                ((PropertyExpression)context.Model).PropertySerializationValue = propertyMetadata;

                var splitted = propertyMetadata.Split(Separators, System.StringSplitOptions.None);

                var type = TypeCache.GetTypeWithoutAssembly(splitted[0]);
                if (type is not null)
                {
                    var instanceProperties = _reflectionService.GetInstanceProperties(type);
                    memberValue.Value = instanceProperties.GetProperty(splitted[1]);
                }
            }
        }
    }
}