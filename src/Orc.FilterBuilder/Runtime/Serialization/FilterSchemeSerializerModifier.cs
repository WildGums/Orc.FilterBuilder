// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeSerializerModifier.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Runtime.Serialization
{
    using System;
    using Catel.Reflection;
    using Catel.Runtime.Serialization;

    public class FilterSchemeSerializerModifier : SerializerModifierBase<FilterScheme>
    {
        public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, nameof(FilterScheme.TargetType)))
            {
                var targetType = memberValue.Value as Type;
                if (targetType != null)
                {
                    memberValue.Value = targetType.FullName;
                }
            }
        }

        public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, nameof(FilterScheme.TargetType)))
            {
                var targetTypeAsString = memberValue.Value as string;
                if (targetTypeAsString != null)
                {
                    memberValue.Value = TypeCache.GetType(targetTypeAsString);
                }
            }
        }
    }
}
