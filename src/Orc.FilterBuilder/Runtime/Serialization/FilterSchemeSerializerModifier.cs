// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeSerializerModifier.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Runtime.Serialization
{
    using System;
    using Catel.Reflection;
    using Catel.Runtime.Serialization;
    using Models;

    public class FilterSchemeSerializerModifier : SerializerModifierBase<FilterScheme>
    {
        public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "TargetDataDescriptor"))
            {
                var provider = memberValue.Value as TypeMetadataProvider;
                if (provider != null)
                {
                    memberValue.Value = provider.TargetType.FullName;
                }
            }
        }

        public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "TargetDataDescriptor"))
            {
                var targetTypeAsString = memberValue.Value as string;
                if (targetTypeAsString != null)
                {
                    var targetType = TypeCache.GetType(targetTypeAsString);
                    memberValue.Value = new TypeMetadataProvider(targetType);
                }
            }
        }
    }
}