// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeSerializerModifier.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Runtime.Serialization
{
    using System;
    using System.Collections.Generic;
    using Catel.Reflection;
    using Catel.Runtime.Serialization;
    using Models;

    public class FilterSchemeSerializerModifier : SerializerModifierBase<FilterScheme>
    {
        public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "TargetDataDescriptor"))
            {
                var provider = memberValue.Value as IMetadataProvider;
                if (provider != null)
                {
                    memberValue.Value = string.Format("{0}|{1}", provider.GetType().FullName, provider.SerializeState());
                }
            }
        }

        public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "TargetDataDescriptor"))
            {
                var providerAsString = memberValue.Value as string;
                if (providerAsString != null)
                {
                    var kvp = providerAsString.Split('|');
                    var type = TypeCache.GetType(kvp[0]);
                    var provider = (IMetadataProvider)Activator.CreateInstance(type);
                    provider.DeserializeState(kvp[1]);
                    memberValue.Value = provider;
                }
            }
        }
    }
}