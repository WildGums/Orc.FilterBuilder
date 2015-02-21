// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeSerializerModifier.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Runtime.Serialization
{
    using System;
    using Catel.Data;
    using Catel.Reflection;
    using Catel.Runtime.Serialization;
    using Models;

    public class PropertyExpressionSerializerModifier : SerializerModifierBase<PropertyExpression>
    {
        public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "Property"))
            {
                var propertyInfo = memberValue.Value as IPropertyMetadata;
                if (propertyInfo != null)
                {
                    memberValue.Value = string.Format("{0}|{1}", propertyInfo.GetType().FullName, propertyInfo.SerializeState());
                }
            }
        }

        public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "Property"))
            {
                var propertyInfoAsString = memberValue.Value as string;
                if (propertyInfoAsString != null)
                {
                    // We need to delay this
                    ((PropertyExpression)context.Model).PropertySerializationValue = propertyInfoAsString;
                }
            }
        }

        public override void OnDeserialized(ISerializationContext context, IModel model)
        {
            base.OnDeserialized(context, model);

            var propertyExpression = (PropertyExpression)context.Model;
            propertyExpression.EnsureIntegrity();
        }
    }
}